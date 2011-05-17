using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.GData.Client;

namespace BridgeTeamEventScoreManager1
{
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
    class SitesAPI_old
    {
        static public String APP_NAME = "BridgeTeamEventScoreManager-SitesAPI-v0.1";
        private SitesService service = null;
        public String loginToken { get; set; }
        private String rootURL = null;
        public SitesAPI_old(String rootURL, String username, String password) 
        {
            this.rootURL = rootURL;
            this.service = new SitesService(APP_NAME);
            this.service.setUserCredentials(username, password);
            this.loginToken = service.QueryClientLoginToken();
        }
        public void checkPermissions() {
            FeedQuery query = new FeedQuery("https://sites.google.com/feeds/content/site/srirambridgetest?path=/test");
            AtomFeed feed = this.service.Query(query);
        }
    }
}
