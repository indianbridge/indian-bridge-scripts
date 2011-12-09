using System;
using System.Collections;
using System.IO;
using Google.GData.Extensions;
using System.Xml;
using Google.GData.Client;
using System.Diagnostics;
using IndianBridge.Common;
using System.Text.RegularExpressions;

namespace IndianBridge.GoogleAPIs
{
    public class SitesAPI
    {
        static public string APP_NAME = "SitesAPI-v0.1";
        static public string LOG_FILE_PREFIX = "HTTP_Traffic";
        static public string LAST_UPDATED_TIMES_PREFIX = "LastModifiedTimes";
        private SitesService m_service = null;
        private String m_siteName = null;
        Hashtable m_lastRunTimes = new Hashtable();
        private string m_hashTableFileName = "";
        private bool m_replaceLinks = false;

        public SitesAPI(String sitename, String username, String password, bool replaceLinks = false, bool logHTTPTraffic = false)
        {
            // Initialize member variables
            this.m_replaceLinks = replaceLinks;
            this.m_siteName = sitename;
            this.m_service = new SitesService(APP_NAME);
            this.m_service.setUserCredentials(username, password);

            // Start HTTP Traffic Logging if requested
            if (logHTTPTraffic) startHTTPTrafficLogging();

            // read existing hash table data if any 
            m_hashTableFileName = Path.Combine(Globals.m_rootDirectory, LAST_UPDATED_TIMES_PREFIX + ".log");
            if (File.Exists(m_hashTableFileName)) readHashTableFile(m_hashTableFileName);
        }

        private void printMessage(String message)
        {
            Trace.WriteLine(message);
            //Debug.WriteLine(message);
        }

        private void startHTTPTrafficLogging()
        {
            Google.GData.Client.GDataLoggingRequestFactory factory = new GDataLoggingRequestFactory("jotspot", "SpreadsheetsLoggingTest");
            factory.MethodOverride = true;
            string logFileName = LOG_FILE_PREFIX + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            factory.CombinedLogFileName = Path.Combine(Globals.m_rootDirectory, logFileName);
            m_service.RequestFactory = factory;
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

        public void uploadDirectory(String directory, String siteRoot)
        {
            if (!Directory.Exists(directory))
                throw new System.ArgumentException("Only a directory structure can be uploaded");

            printMessage("Uploading " + directory + " to " + siteRoot);
            try
            {
                handleRootIndexHtml(directory, siteRoot);
                uploadPath(directory, siteRoot);
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
                        updateWebpage(siteRoot, Path.GetFileName(path), html, "", lastUpdateTime);
                    }
                }
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
                        updateWebpage(siteRoot, Path.GetFileNameWithoutExtension(path), File.ReadAllText(path), makeIdentifier(Path.GetFileNameWithoutExtension(path)), File.GetLastWriteTime(path));
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
                        String html = "";
                        String indexFileName = String.Format(@"{0}\index.htm", dir);

                        if (File.Exists(indexFileName))
                        {
                            html = File.ReadAllText(indexFileName);
                            lastUpdateTime = File.GetLastWriteTime(indexFileName);
                        }
                        else
                        {
                            indexFileName = String.Format(@"{0}\index.html", dir);
                            if (File.Exists(indexFileName))
                            {
                                html = File.ReadAllText(indexFileName);
                                lastUpdateTime = File.GetLastWriteTime(indexFileName);
                            }
                            else html = getSubPageListing();
                        }
                        updateWebpage(siteRoot, dirName, html, pageName, lastUpdateTime);
                        uploadPath(dir, siteRoot + "/" + pageName);
                    }
                    String[] files = Directory.GetFiles(path);
                    foreach (String file in files) uploadPath(file, siteRoot);
                }
            }

        }

        private String getSubPageListing()
        {
            return "<img src=\"https://www.google.com/chart?chc=sites&amp;cht=d&amp;chdp=sites&amp;chl=%5B%5BPage+listing'%3D16'f%5Cbf%5Chv'a%5C%3D123'0'%3D122'0'dim'%5Cbox1'b%5CDBD9BB'fC%5CDBD9BB'eC%5C15'sk'%5C%5B%22Subpage+Listing%22'%5Dh'a%5CV%5C%3D12'f%5Cbf%5C%5DV%5Cta%5C%3D124'%3D0'%3D123'%3D297'dim'%5C%3D124'%3D0'%3D123'%3D297'vdim'%5Cbox1'b%5Cva%5CFFFEF0'fC%5CDBD9BB'eC%5Csites_toc'i%5Chv-0-0'a%5C%5Do%5CLauto'f%5C&amp;sig=2KUvkDdGLdjMIvbcdbJtj3njQjc\" type=\"subpages\" props=\"align:left;borderTitle:Subpage Listing;displayAs:TOC;maxDepth:1;rootPage:THIS_PAGE;width:250;\" width=\"250\" height=\"300\" style=\"display:block;text-align:left;margin-right:auto;\"/><br/>";
        }


        private XmlExtension makePageNameExtension(String pageName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode pageNameNode = xmlDocument.CreateNode(XmlNodeType.Element,
              "sites", "pageName", SitesService.SITES_NAMESPACE);
            pageNameNode.InnerText = pageName;

            return new XmlExtension(pageNameNode);
        }

        public AtomEntry updateWebpage(String path, String title, String html, String pageName, DateTime lastModified)
        {
            String url = "https://sites.google.com/feeds/content/site/" + m_siteName + "?path=" + path + (pageName==""?"":"/") + pageName;
            printMessage(url);
            try
            {
                AtomEntry entry = m_service.Get(url);
                if (entry == null)
                {
                    return createWebPage(url, path, title, html, pageName, lastModified);
                }
                if (html != "")
                {
                    double diff = 0;
                    if (m_lastRunTimes.ContainsKey(url))
                    {
                        DateTime lastRunTime = DateTime.Parse((String)m_lastRunTimes[url]);
                        TimeSpan dt = lastModified - lastRunTime;
                        diff = Math.Abs(dt.TotalSeconds);
                    }
                    if (!m_lastRunTimes.ContainsKey(url) || diff > 1)
                    {
                        AtomContent newContent = new AtomContent();
                        newContent.Type = "html";
                        newContent.Content = m_replaceLinks ? replaceLinks(html,url) : html;
                        entry.Content = newContent;
                        entry.Title.Text = IndianBridge.Common.Utilities.ConvertCaseString(title);
                        m_service.Update(entry);
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
                return entry;

            }
            catch (Exception)
            {
                return createWebPage(url, path, title, html, pageName, lastModified);
            }
        }

        public String getCategoryLabel(AtomCategoryCollection categories)
        {
            foreach (AtomCategory cat in categories)
            {
                if (cat.Scheme == SitesService.KIND_SCHEME)
                {
                    return cat.Label;
                }
            }
            return null;
        }

        public AtomEntry createWebPage(String originalUrl, String path, String title, String html, String pageName, DateTime lastModified)
        {
            String parentUrl = originalUrl.Substring(0, originalUrl.LastIndexOf("/"));
            pageName = originalUrl.Substring(originalUrl.LastIndexOf("/")+1);
            //printMessage("Creating page " + parentUrl + "/" + pageName);
            AtomEntry parent = m_service.Get(parentUrl);
            SiteEntry entry = new SiteEntry();
            AtomCategory category = new AtomCategory(SitesService.WEBPAGE_TERM, SitesService.KIND_SCHEME);
            category.Label = "webpage";
            entry.Categories.Add(category);
            AtomLink link = new AtomLink("application/atom+xml", "http://schemas.google.com/sites/2008#parent");
            link.HRef = parent.EditUri;
            entry.Links.Add(link);
            entry.Title.Text = IndianBridge.Common.Utilities.ConvertCaseString(title);
            entry.Content.Type = "html";
            entry.Content.Content = m_replaceLinks ? replaceLinks(html,originalUrl) : html;
            entry.ExtensionElements.Add(makePageNameExtension(pageName));
            AtomEntry newEntry = null;
            String url = "https://sites.google.com/feeds/content/site/" + m_siteName;
            newEntry = m_service.Insert(new Uri(url), entry);
            m_lastRunTimes[originalUrl] = lastModified;
            printMessage("CREATED.");
            return newEntry;
        }
        private string getParentPage(string path)
        {
            string parentPage = path.TrimEnd('/').TrimEnd('\\');
            int index1 = parentPage.LastIndexOf('/');
            int index2 = parentPage.LastIndexOf('\\');
            int index = index1 > index2 ? index1 : index2;
            return parentPage.Substring(0, index) ;
        }
        private string getPage(string path)
        {
            return path.TrimEnd('/').TrimEnd('\\');
        }


        private string replaceLinks(string html, string pathUrl)
        {
            string url = Regex.Replace(pathUrl, @"\?Path=","",RegexOptions.IgnoreCase);
            url = Regex.Replace(url, @"feeds/content/", "", RegexOptions.IgnoreCase);
            Regex re = new Regex(@"<a\s+href[^<]*</a>", RegexOptions.IgnoreCase);
            Regex re2 = new Regex("http", RegexOptions.IgnoreCase);
            return re.Replace(html, new MatchEvaluator(
            delegate(Match match)
            {
                Console.WriteLine(match.Value);
                string result = match.Value;
                if (re2.IsMatch(result)) return result;
                else
                {
                    result = Regex.Replace(result, @"(\.\./)|(\.\.\\)", getParentPage(getParentPage(url)) + "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\./)|(\.\\)", getPage(url) + "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(index\.html)|(index\.htm)", "", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\.html)|(\.htm)", "", RegexOptions.IgnoreCase);
                    result = result.TrimEnd('/').TrimEnd('\\');
                    return result;
                }
            }));
        }

    }

    class SiteEntry : AbstractEntry
    {
        public SiteEntry() : base() { }
    }

    class SitesService : MediaService
    {
        public const string GSitesService = "jotspot";
        public const string SITES_NAMESPACE = "http://schemas.google.com/sites/2008";
        public const string KIND_SCHEME = "http://schemas.google.com/g/2005#kind";
        public const string ATTACHMENT_TERM = SITES_NAMESPACE + "#attachment";
        public const string WEBPAGE_TERM = SITES_NAMESPACE + "#webpage";
        public const string FILECABINET_TERM = SITES_NAMESPACE + "#filecabinet";
        public const string PARENT_REL = SITES_NAMESPACE + "#parent";

        public SitesService(string applicationName) : base(GSitesService, applicationName) { }
    }
}
