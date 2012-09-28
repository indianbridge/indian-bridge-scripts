using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using System.Diagnostics;
using IndianBridge.GoogleAPIs;

namespace IndianBridgeScorer
{
    public partial class PublishSwissTeamResults : Form
    {
        private string m_localWebpagesRootDirectory = "";
        private string m_resultsWebsite = "";
        private string m_eventName = "";
        public string m_message = "";

        public PublishSwissTeamResults(string eventName, string webpagesRootDirectory, String resultsWebsite)
        {
            m_localWebpagesRootDirectory = webpagesRootDirectory;
            m_resultsWebsite = resultsWebsite;
            m_eventName = eventName;
            InitializeComponent();
            this.Text = "Publish Results for " + m_eventName;
        }
        private bool uploadPages()
        {
            currentOperationTitle.Text = "Uploading Results to Google Sites";
            status.Text = "";
            status.Refresh();
            string siteName, pagePath;
            Utilities.getGoogleSiteComponents(m_resultsWebsite, out siteName, out pagePath);
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                SitesAPI sa = new SitesAPI(sitename: siteName, username: username, password: password, replaceLinks: true, logHTTPTraffic: false);
                sa.uploadDirectory(m_localWebpagesRootDirectory, pagePath);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                m_message = "Unable to upload results to webpage." + Environment.NewLine + e.Message;
                return false;
            }
            Trace.Listeners.Remove(_textBoxListener);
            return true;
        }

        private void PublishResults_Shown(object sender, EventArgs e)
        {
            if (uploadPages()) m_message = "Results created and successfully uploaded to " + m_resultsWebsite;
            this.Close();
        }
    }
}
