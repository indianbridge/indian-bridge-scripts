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
                printDebugMessage("All HTTP traffic will be reported in " + factory.CombinedLogFileName);
            }

        }

        private void printDebugMessage(String message) {
            if(this.debugFlag) Console.WriteLine(message);
        }

        private String makeIdentifier(String text) { return text.Replace(" ", "-"); }

        public void uploadDirectory(String directory, String siteRoot)
        {
            if (!Directory.Exists(directory))
            {
                throw new System.ArgumentException("Only a directory structure can be uploaded");
            }
            printDebugMessage("Uploading " + directory + " to " + siteRoot);
            uploadPath(directory, siteRoot);

        }

        private void uploadPath(String path, String siteRoot)
        {
            DateTime lastUpdateTime = DateTime.Now;
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
            title = IndianBridge.Common.Utility.ToCamelCase(title);
            String url = "https://sites.google.com/feeds/content/site/" + sitename + "?path=" + path+"/"+pageName;
            printDebugMessage("Updating Page : " + url);
            try
            {
                AtomEntry entry = service.Get(url);
                if (entry == null)
                {
                    return createWebPage(path, title, html, pageName);
                }
                if (html != "")
                {
                    if (!lastRunTimes.ContainsKey(url) || lastModified >= (DateTime)lastRunTimes[url])
                    {
                        AtomContent newContent = new AtomContent();
                        newContent.Type = "html";
                        newContent.Content = html;
                        entry.Content = newContent;
                        entry.Title.Text = title;
                        service.Update(entry);
                        lastRunTimes[url] = DateTime.Now;
                        if (this.debugFlag) Console.WriteLine("Updated " + url + " successfully!!!");
                    }
                    else
                    {
                        if (this.debugFlag) Console.WriteLine("Last Modified Time " + lastModified.ToString() + " is before last update time " + ((DateTime)lastRunTimes[url]).ToString() + ", so not updating!!!");
                    }
                }
                return entry;

            }
            catch (Exception)
            {
                return createWebPage(path, title, html, pageName);
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

        public AtomEntry createWebPage(String path,String title, String html, String pageName)
        {
            String parentUrl = "http://sites.google.com/feeds/content/site/" + sitename + "?path=" + path;
            printDebugMessage("Creating page "+ parentUrl + "/" + pageName);
            AtomEntry parent = service.Get(parentUrl);
            SiteEntry entry = new SiteEntry();
            AtomCategory category = new AtomCategory(SitesService.WEBPAGE_TERM, SitesService.KIND_SCHEME);
            category.Label = "webpage";
            entry.Categories.Add(category);
            AtomLink link = new AtomLink("application/atom+xml", "http://schemas.google.com/sites/2008#parent");
            link.HRef = parent.EditUri;
            entry.Links.Add(link);
            entry.Title.Text = IndianBridge.Common.Utility.ToCamelCase(title);
            entry.Content.Type = "html";
            //entry.Content.Content = getValidXml(html);
            entry.Content.Content = html;
            entry.ExtensionElements.Add(makePageNameExtension(pageName));

            AtomEntry newEntry = null;
            String url = "http://sites.google.com/feeds/content/site/" + sitename;
            newEntry = service.Insert(new Uri(url), entry);
            if (this.debugFlag) Console.WriteLine("Created " + parentUrl + "/" + pageName + " successfully!");
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
