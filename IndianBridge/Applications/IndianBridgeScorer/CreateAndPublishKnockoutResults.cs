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
    public partial class CreateAndPublishKnockoutResults : Form
    {
        private DataSet m_ds = null;
        private string m_localWebpagesRootDirectory = "";
        private string m_resultsWebsite = "";
        private string m_eventName = "";

        public CreateAndPublishKnockoutResults(DataSet ds, string webpagesRootDirectory, String resultsWebsite, string eventName)
        {
            m_ds = ds;
            m_localWebpagesRootDirectory = webpagesRootDirectory;
            m_resultsWebsite = resultsWebsite;
            m_eventName = eventName;
            InitializeComponent();
            this.Text = "Create and Publish Knockout Results for " + m_eventName;
        }

        private bool createWebpages()
        {
            currentOperationTitle.Text = "Creating Local Knockout webpages from results database";
            status.Text = "";
            status.Refresh();
            TeamsDatabaseToWebpages tdw = new TeamsDatabaseToWebpages(m_ds, m_localWebpagesRootDirectory);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                tdw.createKnockoutPage();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                MessageBox.Show("Unable to load create local webpages from results database"+Environment.NewLine + e.Message);
                return false;
            }
            return true;
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
                MessageBox.Show("Unable to upload results to webpage."+Environment.NewLine + e.Message);
                return false;
            }
            Trace.Listeners.Remove(_textBoxListener);
            return true;
        }

        private void PublishResults_Shown(object sender, EventArgs e)
        {
            if (createWebpages())
            {
                if (string.IsNullOrWhiteSpace(m_resultsWebsite)) MessageBox.Show("Local webpages created in " + m_localWebpagesRootDirectory, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else {
                    if (uploadPages()) MessageBox.Show("Results created and successfully uploaded to " + m_resultsWebsite,"Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            this.Close();
        }
    }
}
