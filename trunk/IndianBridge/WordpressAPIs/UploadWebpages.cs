using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using IndianBridge.Common;
using CookComputing.XmlRpc;

namespace IndianBridge.WordpressAPIs
{
    public struct PageInfo
    {
        public string pagePath;
        //public string pageName;
        public string pageTitle;
        public string pageContent;
        public string pageTemplate;
    }

    public struct CSVContent
    {
        public string content;
        public string delimiter;
    }

    public interface IgetCatList
    {
        [CookComputing.XmlRpc.XmlRpcMethod("indianbridge.postResults")]
        string postResults(int blogId, string username, string password, PageInfo content);
    }

    public class UploadWebpages
    {
        static public string LAST_UPDATED_TIMES_PREFIX = "LastModifiedTimes";
        private string m_siteName = null;
        private string m_xmlrpcURL = null;
        private string m_userName = null;
        private string m_password = null;
        Hashtable m_lastRunTimes = new Hashtable();
        private string m_hashTableFileName = "";
        private bool m_replaceLinks = false;

        public bool ReplaceLinks
        {
            get { return m_replaceLinks; }
            set { m_replaceLinks = value; }
        }
        private bool m_convertCase = true;
        private BackgroundWorker m_worker;
        private DoWorkEventArgs m_e;
        private bool m_runningInBackground = false;
        private int totalPagesToUpload = 0;
        private int numberOfPagesAlreadyUploaded = 0;
        private string m_prefixString = "";
        private bool m_useTourneyTemplate = true;

        public bool UseTourneyTemplate
        {
            get { return m_useTourneyTemplate; }
            set { m_useTourneyTemplate = value; }
        }
        private bool m_forceUpload = false;

        public bool ForceUpload
        {
            get { return m_forceUpload; }
            set { m_forceUpload = value; }
        }

        public XmlRpcClientProtocol m_clientProtocol;
        public IgetCatList m_categories;

        public void convertCase(bool convert) { m_convertCase = convert; }

        public UploadWebpages(string sitename, string username, string password, bool replaceLinks = false, bool forceUpload = false, bool useTourneyTemplate = true)
        {
            // Initialize member variables
            this.m_siteName = sitename;
            this.m_userName = username;
            this.m_password = password;
            this.m_replaceLinks = replaceLinks;
            this.m_useTourneyTemplate = useTourneyTemplate;
            this.m_forceUpload = forceUpload;
            this.m_xmlrpcURL = m_siteName.TrimEnd(new[] { '/', '\\' }) + "/xmlrpc.php";

            m_categories = (IgetCatList)XmlRpcProxyGen.Create(typeof(IgetCatList));
            m_clientProtocol = (XmlRpcClientProtocol)m_categories;
            m_clientProtocol.Url = this.m_xmlrpcURL;

            // read existing hash table data if any 
            m_hashTableFileName = Path.Combine(Globals.m_rootDirectory, LAST_UPDATED_TIMES_PREFIX + ".log");
            if (File.Exists(m_hashTableFileName)) readHashTableFile(m_hashTableFileName);
        }

        private void printMessage(String message)
        {
            Trace.WriteLine(message);
        }

        private void readHashTableFile(String fileName)
        {
            String line = "";
            TextReader tr = new StreamReader(fileName);
            while (tr.Peek() >= 0)
            {
                line = tr.ReadLine();
                String[] values = line.Split(',');
                m_lastRunTimes[values[0]] = values[1];
            }
            tr.Close();
        }

        private void writeHashTableFile(String fileName)
        {
            TextWriter tw = new StreamWriter(fileName);
            foreach (DictionaryEntry pair in m_lastRunTimes) tw.WriteLine(pair.Key + "," + pair.Value);
            tw.Close();
        }

        private String makeIdentifier(String text) { return text.Replace(" ", "-"); }

        public void uploadDirectoryInBackground(object sender, DoWorkEventArgs e)
        {
            Tuple<string, string> values = (Tuple<string, string>)e.Argument;
            string directory = values.Item1;
            string siteRoot = values.Item2.TrimEnd(new[] { '/', '\\' });
            m_runningInBackground = true;
            m_worker = sender as BackgroundWorker;
            m_e = e;
            uploadDirectoryInternal(directory, siteRoot);
            if (cancel()) m_e.Cancel = true;
        }

        public void uploadDirectory(String directory, String siteRoot)
        {
            m_runningInBackground = false;
            m_worker = null;
            m_e = null;
            uploadDirectoryInternal(directory, siteRoot.TrimEnd(new[] { '/', '\\' }));
        }

        public void uploadDirectoryInternal(String directory, String siteRoot)
        {
            if (File.Exists(directory)) {
                totalPagesToUpload = 1;
                numberOfPagesAlreadyUploaded = 0;
                uploadSingleFile(directory, siteRoot);
                return;
            }
            if (!Directory.Exists(directory))
                throw new System.ArgumentException("Only a file or directory structure can be uploaded");
            DirectoryInfo dir = new DirectoryInfo(directory);
            totalPagesToUpload = dir.GetDirectories("*", SearchOption.AllDirectories).Length + dir.GetFiles("*", SearchOption.AllDirectories).Length;
            printMessage("Uploading " + directory + " to " + siteRoot);
            numberOfPagesAlreadyUploaded = 0;
            try
            {
                handleRootIndexHtml(directory, siteRoot);
                if (cancel()) return;
                uploadPath(directory, siteRoot);
                if (cancel()) return;
                writeHashTableFile(m_hashTableFileName);
            }
            catch (Exception e)
            {
                writeHashTableFile(m_hashTableFileName);
                throw e;
            }
        }

        private void handleRootIndexHtml(string path, string siteRoot)
        {
            if (Directory.Exists(path))
            {
                String[] files = Directory.GetFiles(path);
                foreach (String file in files)
                {
                    String extension = Path.GetExtension(file).ToLower();
                    if ((extension.Equals(".htm", StringComparison.OrdinalIgnoreCase) || extension.Equals(".html", StringComparison.OrdinalIgnoreCase))
                        && Path.GetFileNameWithoutExtension(file).ToLower().Equals("index", StringComparison.OrdinalIgnoreCase))
                    {
                        string html = File.ReadAllText(file);
                        DateTime lastUpdateTime = File.GetLastWriteTime(file);
                        updateWebpage(siteRoot, Path.GetFileName(path), html, "", lastUpdateTime, true);
                        if (cancel()) return;
                    }
                }
            }
        }

        public void uploadSingleFile(String path, String siteRoot)
        {
            String extension = Path.GetExtension(path).ToLower();
            if ((extension == ".htm" || extension == ".html") && Path.GetFileNameWithoutExtension(path).ToLower() != "index")
            {
                updateSingleWebpage(siteRoot, Path.GetFileNameWithoutExtension(path), File.ReadAllText(path), File.GetLastWriteTime(path), false);
            }
            else
            {
                throw new Exception(path + " does not have .htm or .html extension or it is index page which cannot be uploaded.");
            }
            return;
        }

        public bool updateSingleWebpage(string url, string title, string html, DateTime lastModified, bool isIndexPage = false, string indexHtmlPath = "")
        {
            printMessage(url);
            try
            {
                if (html != "")
                {
                    double diff = 0;
                    if (m_lastRunTimes.ContainsKey(url) && !m_forceUpload)
                    {
                        DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                        TimeSpan dt = lastModified - lastRunTime;
                        diff = Math.Abs(dt.TotalSeconds);
                    }
                    if (m_forceUpload || !m_lastRunTimes.ContainsKey(url) || diff > 1)
                    {
                        PageInfo pageInfo = default(PageInfo);
                        pageInfo.pagePath = url;
                        pageInfo.pageTitle = m_convertCase ? IndianBridge.Common.Utilities.ConvertCaseString(title) : title;
                        pageInfo.pageContent = m_replaceLinks ? replaceLinks(html, url, isIndexPage, indexHtmlPath) : html;
                        pageInfo.pageTemplate = m_useTourneyTemplate ? "page-tourney.php" : "";
                        string result = m_categories.postResults(1, m_userName, m_password, pageInfo);
                        if (!m_lastRunTimes.ContainsKey(url)) printMessage("UPDATED. No entry was found for last run time.");
                        else
                        {
                            DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                            printMessage("UPDATED. (Last Modified Time " + lastModified.ToString() + " is later than last update time " + lastRunTime.ToString() + ")");
                        }
                        m_lastRunTimes[url] = lastModified;
                    }
                    else
                    {
                        DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                        printMessage("NOT UPDATED. (Last Modified Time " + lastModified.ToString() + " is earlier than (or equal to) last update time " + lastRunTime.ToString() + ")");
                    }
                }
                numberOfPagesAlreadyUploaded++;
                reportProgress(title);
                return true;
            }
            catch (Exception ex)
            {
                printMessage("Exception : " + ex.Message);
                throw ex;
            }
        }

        private void uploadPath(String path, String siteRoot)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                if (File.Exists(path))
                {
                    String extension = Path.GetExtension(path).ToLower();
                    if ((extension == ".htm" || extension == ".html") && Path.GetFileNameWithoutExtension(path).ToLower() != "index")
                    {
                        updateWebpage(siteRoot, Path.GetFileNameWithoutExtension(path), File.ReadAllText(path), makeIdentifier(Path.GetFileNameWithoutExtension(path)), File.GetLastWriteTime(path), false);
                        if (cancel()) return;
                    }
                    return;
                }
                else if (Directory.Exists(path))
                {
                    String[] directories = Directory.GetDirectories(path);
                    foreach (String dir in directories)
                    {
                        DateTime lastUpdateTime = Directory.GetLastWriteTime(dir);
                        var dirName = Path.GetFileName(dir);
                        var pageName = makeIdentifier(dirName);
                        string html = "";
                        string indexFileName = String.Format(@"{0}\index.htm", dir);
                        string indexHtmlPath = "";
                        if (File.Exists(indexFileName))
                        {
                            html = File.ReadAllText(indexFileName);
                            lastUpdateTime = File.GetLastWriteTime(indexFileName);
                            indexHtmlPath = dir;
                        }
                        else
                        {
                            indexFileName = String.Format(@"{0}\index.html", dir);
                            if (File.Exists(indexFileName))
                            {
                                html = File.ReadAllText(indexFileName);
                                lastUpdateTime = File.GetLastWriteTime(indexFileName);
                                indexHtmlPath = dir;
                            }
                            else html = getSubPageListing();
                        }
                        updateWebpage(siteRoot, dirName, html, pageName, lastUpdateTime, true, indexHtmlPath);
                        if (cancel()) return;
                        uploadPath(dir, siteRoot + "/" + pageName);
                        if (cancel()) return;
                    }
                    String[] files = Directory.GetFiles(path);
                    foreach (String file in files) uploadPath(file, siteRoot);
                }
            }

        }

        private String getSubPageListing()
        {
            return "[subpages]";
        }

        public bool updateWebpage(string path, string title, string html, string pageName, DateTime lastModified, bool isIndexPage, string indexHtmlPath = "")
        {
            if (cancel()) return false;
            int value;
            string url = int.TryParse(pageName, out value)?Utilities.combinePath(path, "y"+pageName):Utilities.combinePath(path, pageName);
            printMessage(url);
            try
            {
                if (html != "")
                {
                    double diff = 0;
                    if (m_lastRunTimes.ContainsKey(url) && !m_forceUpload)
                    {
                        DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                        TimeSpan dt = lastModified - lastRunTime;
                        diff = Math.Abs(dt.TotalSeconds);
                    }
                    if (m_forceUpload || !m_lastRunTimes.ContainsKey(url) || diff > 1)
                    {
                        PageInfo pageInfo = default(PageInfo);
                        pageInfo.pagePath = url;
                        pageInfo.pageTitle = m_convertCase ? IndianBridge.Common.Utilities.ConvertCaseString(title) : title;
                        pageInfo.pageContent = m_replaceLinks ? replaceLinks(html, url, isIndexPage, indexHtmlPath) : html;
                        pageInfo.pageTemplate = m_useTourneyTemplate?"page-tourney.php":"";
                        string result = m_categories.postResults(1, m_userName, m_password, pageInfo);
                        if (!m_lastRunTimes.ContainsKey(url)) printMessage("UPDATED. No entry was found for last run time.");
                        else
                        {
                            DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                            printMessage("UPDATED. (Last Modified Time " + lastModified.ToString() + " is later than last update time " + lastRunTime.ToString() + ")");
                        }
                        m_lastRunTimes[url] = lastModified;
                    }
                    else
                    {
                        DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                        printMessage("NOT UPDATED. (Last Modified Time " + lastModified.ToString() + " is earlier than (or equal to) last update time " + lastRunTime.ToString() + ")");
                    }
                }
                numberOfPagesAlreadyUploaded++;
                reportProgress(title);
                return true;

            }
            catch (Exception ex)
            {
                printMessage("Exception : "+ex.Message);
                throw ex;
            }
        }


        private void reportProgress(string title)
        {
            if (m_runningInBackground)
            {
                double percentage = ((double)numberOfPagesAlreadyUploaded / (double)totalPagesToUpload) * 100;
                m_worker.ReportProgress(Convert.ToInt32(percentage), m_prefixString + "Published " + title);
            }
        }

        private bool cancel()
        {
            return m_runningInBackground && m_worker.CancellationPending;
        }


        private string getParentPage(string path)
        {
            string parentPage = path.TrimEnd('/').TrimEnd('\\');
            int index1 = parentPage.LastIndexOf('/');
            int index2 = parentPage.LastIndexOf('\\');
            int index = index1 > index2 ? index1 : index2;
            return parentPage.Substring(0, index);
        }
        private string getPage(string path)
        {
            return path.TrimEnd('/').TrimEnd('\\');
        }


        private string replaceLinks(string html, string pathUrl, bool isIndexPage, string indexHtmlPath = "")
        {
            string url = Regex.Replace(pathUrl, @"\?Path=", "", RegexOptions.IgnoreCase);
            url = Regex.Replace(url, @"feeds/content/", "", RegexOptions.IgnoreCase);
            Regex re = new Regex(@"<a\s+href[^<]*</a>", RegexOptions.IgnoreCase);
            Regex re2 = new Regex("http", RegexOptions.IgnoreCase);
            return re.Replace(html, new MatchEvaluator(
            delegate(Match match)
            {
                string result = match.Value;
                if (re2.IsMatch(result)) return result;
                else
                {
                    string replaceDotUrl = "";
                    string replaceDotDotUrl = "";
                    string replaceDoubleDotDotUrl = "";
                    if (isIndexPage)
                    {
                        replaceDotUrl = getPage(url);
                        replaceDotDotUrl = getParentPage(url);
                        replaceDoubleDotDotUrl = getParentPage(getParentPage(url));
                    }
                    else
                    {
                        replaceDotUrl = getPage(url);
                        replaceDotDotUrl = getParentPage(getParentPage(url));
                        replaceDoubleDotDotUrl = getParentPage(getParentPage(getParentPage(url)));
                    }
                    result = Regex.Replace(result, @"(\.\./\.\./)|(\.\.\\\.\.\\)", m_siteName+"/"+replaceDoubleDotDotUrl + "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\.\./)|(\.\.\\)", m_siteName+"/"+replaceDotDotUrl + "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\./)|(\.\\)", m_siteName+"/"+replaceDotUrl + "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(index\.html)|(index\.htm)", "", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\.html)|(\.htm)", "", RegexOptions.IgnoreCase);
                    result = result.TrimEnd('/').TrimEnd('\\');
                    return result;
                }
            }));
        }

    }

}
