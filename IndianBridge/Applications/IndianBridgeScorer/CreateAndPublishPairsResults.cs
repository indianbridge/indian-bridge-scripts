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
        private string m_googleSiteName = "indiancitybridgeresults";
        String m_googleSiteRootPageName = "";

        public CreateAndPublishPairsResults(PairsEventInformation eventInformation, String googleSiteName, String googleSiteRootPageName)
        {
            m_eventInformation = eventInformation;
            m_googleSiteName = googleSiteName;
            m_googleSiteRootPageName = googleSiteRootPageName;
            InitializeComponent();
        }

        public CreateAndPublishPairsResults(PairsEventInformation eventInformation, string websiteRoot)
        {
            m_eventInformation = eventInformation;
            string[] tokens = websiteRoot.Split('/');

            m_googleSiteName = tokens[4];
            m_googleSiteRootPageName = "";
            for (int i = 5; i < tokens.Length; ++i)
            {
                m_googleSiteRootPageName += "/" + tokens[i];
            }
            InitializeComponent();
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
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                SitesAPI sa = new SitesAPI(sitename: m_googleSiteName, username: username, password: password, replaceLinks: true, logHTTPTraffic: false);
                sa.uploadDirectory(m_eventInformation.webpagesDirectory, m_googleSiteRootPageName);
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
                    if (uploadPages())
                    {
                        currentOperationTitle.Text = "Updating Google Calendar";
                        status.Text = "";
                        status.Refresh();

                        if (SelectCalendarEvent.calendarAPI.updateResults(SelectCalendarEvent.selectedEntryNumber, "https://sites.google.com/site/" + m_googleSiteName + m_googleSiteRootPageName))
                        {
                            MessageBox.Show("Results created and successfully uploaded to " + "https://sites.google.com/site/" + m_googleSiteName + m_googleSiteRootPageName);
                        }
                    }
                }
            }
            this.Close();
        }
    }
}
