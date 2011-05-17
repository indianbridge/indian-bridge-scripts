using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Google.GData.Client;

namespace BridgeTeamEventScoreManager
{

    public partial class SitesAPI : Form
    {
        static public String APP_NAME = "BridgeTeamEventScoreManager-SitesAPI-v0.1";
        private SitesService service = null;
        private String loginToken = null;
        private String rootURL = null;
        private String username = null;
        private String password = null;
        private Boolean loginStatus = false;
        public Boolean getLoginStatus() { return loginStatus;}
        private Boolean authenticationStatus = false;
        public Boolean getAuthenticationStatus() { return authenticationStatus;  }
        private String errorMessage = null;
        public String getErrorMessage() { return errorMessage; }

        public SitesAPI()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            this.rootURL = siteTextBox.Text;
            this.username = usernameTextBox.Text;
            this.password = passwordTextBox.Text;
            try
            {
                this.service = new SitesService(APP_NAME);
                this.service.setUserCredentials(this.username, this.password);
                this.loginToken = service.QueryClientLoginToken();
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
                this.loginStatus = false;
                this.authenticationStatus = false;
                this.Close();
                return;
            }
            this.loginStatus = true;
            try
            {
                FeedQuery query = new FeedQuery(this.rootURL);
                AtomFeed feed = this.service.Query(query);
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.ToString();
                this.authenticationStatus = false;
                this.Close();
                return;
            }
            this.authenticationStatus = true;
            this.errorMessage = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
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
