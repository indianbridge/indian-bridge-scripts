using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.ResultsManager;
using IndianBridge.Common;
using System.Diagnostics;
using IndianBridge.GoogleAPIs;

namespace IndianBridgeScorer
{
    public partial class CreateAndPublishPairsResults : Form
    {
        private PairsEventInformation m_eventInformation;
        private PairsDatabaseParameters m_databaseParameters = PairsGeneral.createDefaultDatabaseParameters();
        private string m_resultsWebsite = "";

        public CreateAndPublishPairsResults(PairsEventInformation eventInformation, string resultsWebsite)
        {
            m_eventInformation = eventInformation;
            m_resultsWebsite = resultsWebsite;
            InitializeComponent();
            this.Text = "Create and Publish Results for " + m_eventInformation.eventName;
        }



        private bool loadSummaryIntoDatabase()
        {
            currentOperationTitle.Text = "Loading Results into Database";
            status.Text = "";
            status.Refresh();
            PairsSummaryToDatabase std = new PairsSummaryToDatabase(m_eventInformation);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                std.loadSummaryIntoDatabase();
                m_databaseParameters = std.getDatabaseParameters();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                MessageBox.Show("Unable to load results to database/n" + e.Message);
                return false;
            }
            return true;
        }

        private bool createWebpages()
        {
            currentOperationTitle.Text = "Creating Local webpages from results database";
            status.Text = "";
            status.Refresh();
            PairsDatabaseToWebpages dtw = new PairsDatabaseToWebpages(m_eventInformation, m_databaseParameters);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                dtw.createWebpages_();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                MessageBox.Show("Unable to load create local webpages from results database/n" + e.Message);
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
                sa.uploadDirectory(m_eventInformation.webpagesDirectory, pagePath);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                MessageBox.Show("Unable to upload results to webpage./n" + e.Message);
                return false;
            }
            Trace.Listeners.Remove(_textBoxListener);
            return true;
        }

        private void PublishResults_Shown(object sender, EventArgs e)
        {
            if (loadSummaryIntoDatabase())
            {
                if (createWebpages())
                {
                    if (string.IsNullOrWhiteSpace(m_resultsWebsite)) MessageBox.Show("Local webpages created in " +m_eventInformation.webpagesDirectory, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else {
                        if (uploadPages())
                        {
                            MessageBox.Show("Results created and successfully uploaded to " + m_resultsWebsite,"Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                    }
                }
            }
            this.Close();
        }
    }
}
