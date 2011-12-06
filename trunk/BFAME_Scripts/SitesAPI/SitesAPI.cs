using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;

using Google.GData.Client;
using Google.GData.Extensions;

namespace Upload_To_Google_Sites
{
    public class SitesAPI
    {
        static public String APP_NAME = "BridgeTeamEventScoreManager-SitesAPI-v0.1";
        private SitesService service = null;
        private String sitename = null;
        private Boolean debugFlag = false;
        Hashtable lastRunTimes = new Hashtable();
        private String m_backUpDirectory = null;
        private bool m_deleteFilesAfterUpload = false;
        private String m_hashTableFileName = "";

        public SitesAPI(String sitename, String username, String password, bool debugFlag=false)
        {
            this.debugFlag = debugFlag;
            this.sitename = sitename;
            this.service = new SitesService(APP_NAME);
            this.service.setUserCredentials(username, password);
            if (this.debugFlag)
            {
                Google.GData.Client.GDataLoggingRequestFactory factory = new GDataLoggingRequestFactory("jotspot", "SpreadsheetsLoggingTest");
                factory.MethodOverride = true;
                factory.CombinedLogFileName = Path.Combine(Directory.GetCurrentDirectory(),"HTTP_Traffic.log");
                FileStream stream = new FileStream(factory.CombinedLogFileName, FileMode.Create);
                TextWriter writer = new StreamWriter(stream);
                writer.WriteLine("");
                stream.Close();
                service.RequestFactory = factory;
                if (this.debugFlag) printDebugMessage("All HTTP traffic will be reported in " + factory.CombinedLogFileName);
            }

            // read existing hash table data if any 
            m_hashTableFileName = Path.Combine(Directory.GetCurrentDirectory(), "LastModifiedTimes.log");
            if (File.Exists(m_hashTableFileName))
            {
                readHashTableFile(m_hashTableFileName);
            }
            File.Create(m_hashTableFileName);
        }

        private void readHashTableFile(String fileName) {
            String line = "";
            TextReader tr = new StreamReader(fileName);
            while (tr.Peek() >= 0)
            {
                line = tr.ReadLine();
                String[] values = line.Split(',');
                lastRunTimes[values[0]] = values[1];
            }
            tr.Close();
        }

        private void writeHashTableFile(String fileName)
        {
            TextWriter tw = new StreamWriter(fileName);
            foreach (DictionaryEntry pair in lastRunTimes) tw.WriteLine(pair.Key + "," + pair.Value);
            tw.Close();
        }

        private void printDebugMessage(String message) {
            if(this.debugFlag) Console.WriteLine(message);
        }

        private String makeIdentifier(String text) { return text.Replace(" ", "-"); }

        public void uploadDirectory(String directory, String siteRoot, String backUpDirectory=null)
        {
            if (!Directory.Exists(directory))
            {
                throw new System.ArgumentException("Only a directory structure can be uploaded");
            }
            m_backUpDirectory = backUpDirectory;
            printDebugMessage("Uploading " + directory + " to " + siteRoot);
            // First back up the directory 
            if (Directory.Exists(m_backUpDirectory))
            {
                m_deleteFilesAfterUpload = true;

                DirectoryInfo diSource = new DirectoryInfo(directory);
                DirectoryInfo diTarget = new DirectoryInfo(m_backUpDirectory);

                CopyAll(diSource, diTarget);                
            }
            else m_deleteFilesAfterUpload = false;
            try
            {
                uploadPath(directory, siteRoot);
                writeHashTableFile(m_hashTableFileName);
            } catch(Exception e) {
                writeHashTableFile(m_hashTableFileName);
                throw e;
            }
            if (m_deleteFilesAfterUpload)
            {
                if (Directory.Exists(directory)) Directory.Delete(directory, true);
                else if (File.Exists(directory)) File.Delete(directory);
            }
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void uploadPath(String path, String siteRoot)
        {        
            if (Directory.Exists(path) || File.Exists(path))
            {
                 if (File.Exists(path))
                {
                    String extension = Path.GetExtension(path).ToLower();
                    if ((extension == ".htm" || extension == ".html") && Path.GetFileNameWithoutExtension(path).ToLower()!="index")
                    {
                        updateWebpage(siteRoot, Path.GetFileNameWithoutExtension(path), File.ReadAllText(path), makeIdentifier(Path.GetFileNameWithoutExtension(path)),File.GetLastWriteTime(path));
                    }
                    return;
                } 
                else if (Directory.Exists(path)) {
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
                        updateWebpage(siteRoot,dirName, html, pageName, lastUpdateTime);
                        uploadPath(dir, siteRoot + "/" + pageName);
                    }
                    String[] files = Directory.GetFiles(path);
                    foreach (String file in files) uploadPath(file,siteRoot);
                }
            }
            
        }

        private String getSubPageListing()
        {
            return "<img src=\"https://www.google.com/chart?chc=sites&amp;cht=d&amp;chdp=sites&amp;chl=%5B%5BPage+listing'%3D16'f%5Cbf%5Chv'a%5C%3D123'0'%3D122'0'dim'%5Cbox1'b%5CDBD9BB'fC%5CDBD9BB'eC%5C15'sk'%5C%5B%22Subpage+Listing%22'%5Dh'a%5CV%5C%3D12'f%5Cbf%5C%5DV%5Cta%5C%3D124'%3D0'%3D123'%3D297'dim'%5C%3D124'%3D0'%3D123'%3D297'vdim'%5Cbox1'b%5Cva%5CFFFEF0'fC%5CDBD9BB'eC%5Csites_toc'i%5Chv-0-0'a%5C%5Do%5CLauto'f%5C&amp;sig=2KUvkDdGLdjMIvbcdbJtj3njQjc\" type=\"subpages\" props=\"align:left;borderTitle:Subpage Listing;displayAs:TOC;maxDepth:1;rootPage:THIS_PAGE;width:250;\" width=\"250\" height=\"300\" style=\"display:block;text-align:left;margin-right:auto;\"/><br/>";
        }

        private bool isDirectory(String path)
        {
            return Directory.Exists(path);
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
            String url = "https://sites.google.com/feeds/content/site/" + sitename + "?path=" + path+"/"+pageName;
            if (this.debugFlag) printDebugMessage("Updating Page : " + url);
            try
            {
                AtomEntry entry = service.Get(url);
                if (entry == null)
                {
                    return createWebPage(url,path, title, html, pageName,lastModified);
                }
                if (html != "")
                {
                    double diff = 0;
                    if (lastRunTimes.ContainsKey(url))
                    {
                        DateTime lastRunTime = DateTime.Parse((String)lastRunTimes[url]);
                        TimeSpan dt = lastModified-lastRunTime;
                        diff = Math.Abs(dt.TotalSeconds);
                    }
                    if (!lastRunTimes.ContainsKey(url) || diff > 1)
                    {
                        AtomContent newContent = new AtomContent();
                        newContent.Type = "html";
                        newContent.Content = html;
                        entry.Content = newContent;
                        entry.Title.Text = IndianBridge.Common.Utility.ConvertCaseString(title);
                        service.Update(entry);
                        if (!lastRunTimes.ContainsKey(url)) printDebugMessage(url + " - Updated. No entry was found for last run time.");
                        else
                        {
                            DateTime lastRunTime = DateTime.Parse((String)lastRunTimes[url]);
                            if (this.debugFlag) printDebugMessage(url + " - Updated. (Last Modified Time " + lastModified.ToString() + " is later than last update time " + lastRunTime.ToString() + ")");
                        }
                        lastRunTimes[url] = lastModified;
                        /*TextWriter tw = new StreamWriter(m_hashTableFileName, true);
                        tw.WriteLine(url + "," + lastModified);
                        tw.Close();*/
                    }
                    else
                    {
                        DateTime lastRunTime = DateTime.Parse((String)lastRunTimes[url]);
                        if (this.debugFlag) printDebugMessage(url + " - No Changes to Upload. (Last Modified Time " + lastModified.ToString() + " is earlier than (or equal to) last update time " + lastRunTime.ToString() + ")");
                    }
                }
                return entry;

            }
            catch (Exception)
            {
                //if (this.debugFlag) printDebugMessage("Exception : " + e.ToString() + ", Trying to create webpage.");
                return createWebPage(url,path, title, html, pageName,lastModified);
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

        public AtomEntry createWebPage(String originalUrl,String path, String title, String html, String pageName, DateTime lastModified)
        {
            String parentUrl = "https://sites.google.com/feeds/content/site/" + sitename + "?path=" + path;
            if (this.debugFlag) printDebugMessage("Creating page " + parentUrl + "/" + pageName);
            AtomEntry parent = service.Get(parentUrl);
            SiteEntry entry = new SiteEntry();
            AtomCategory category = new AtomCategory(SitesService.WEBPAGE_TERM, SitesService.KIND_SCHEME);
            category.Label = "webpage";
            entry.Categories.Add(category);
            AtomLink link = new AtomLink("application/atom+xml", "http://schemas.google.com/sites/2008#parent");
            link.HRef = parent.EditUri;
            entry.Links.Add(link);
            entry.Title.Text = IndianBridge.Common.Utility.ConvertCaseString(title);
            entry.Content.Type = "html";
            entry.Content.Content = html;
            entry.ExtensionElements.Add(makePageNameExtension(pageName));
            AtomEntry newEntry = null;
            String url = "https://sites.google.com/feeds/content/site/" + sitename;
            newEntry = service.Insert(new Uri(url), entry);
            lastRunTimes[originalUrl] = lastModified;
            /*TextWriter tw = new StreamWriter(m_hashTableFileName, true);
            tw.WriteLine(originalUrl + "," + lastModified);
            tw.Close();*/
            if (this.debugFlag) printDebugMessage(parentUrl + "/" + pageName + " - Created.");
            return newEntry;
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
