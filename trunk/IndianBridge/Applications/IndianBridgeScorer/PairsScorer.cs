using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using System.IO;
using IndianBridge.ResultsManager;
using IndianBridge.GoogleAPIs;

namespace IndianBridgeScorer
{
    public partial class PairsScorer : Form
    {
        private Boolean resultsLoaded = false, publishEnabled = false;
        private PairsEventInformation m_eventInformation = PairsGeneral.createDefaultEventInformation();
        private PairsDatabaseParameters m_databaseParameters = PairsGeneral.createDefaultDatabaseParameters();

        private string m_databaseFileName;
        private string m_eventName;
        private string m_localWebpagesRoot;
        private string m_resultsWebsite;
        public PairsScorer(string eventName)
        {
            m_eventName = eventName;
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            this.Text = NiniUtilities.getStringValue(Constants.getCurrentTourneyInformationFileName(), Constants.TourneyNameFieldName) + " : " + m_eventName;
            m_databaseFileName = Constants.getEventScoresFileName(m_eventName);
            m_localWebpagesRoot = Constants.getEventWebpagesFolder(m_eventName);
            m_resultsWebsite = Constants.getEventResultsWebsite(m_eventName);
            websiteAddress_textBox.Text = m_resultsWebsite;
        }

        private void setResultsWebsite(string resultsWebsite)
        {
            m_resultsWebsite = string.IsNullOrWhiteSpace(resultsWebsite) ? "" : resultsWebsite + "/" + Utilities.makeIdentifier_(m_eventName);
            websiteAddress_textBox.Text = m_resultsWebsite;
        }

        private void updateButtonStatus()
        {
            publishEnabled = resultsLoaded;
            if (!resultsLoaded)
            {
                this.selectedResultsFile_textBox.Text = "";
                this.eventInfo_textBox.Text = "";
            }
            this.loadResults.BackColor = resultsLoaded ? Color.Green : Color.Red;
            this.publishResults.BackColor = publishEnabled ? Color.Green : Color.Red;
            this.publishResults.Enabled = publishEnabled;
            this.Refresh();
        }

        private void loadResults_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectSummaryFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    String text = File.ReadAllText(SelectSummaryFileDialog.FileName);
                    String summaryText = Utilities.compressText_(text);
                    m_eventInformation = PairsGeneral.getEventInformation_(summaryText);
                    m_eventInformation.databaseFileName = m_databaseFileName;
                    m_eventInformation.webpagesDirectory = m_localWebpagesRoot;
                }
                catch (Exception exception)
                {
                    resultsLoaded = false;
                    updateButtonStatus();
                    MessageBox.Show("Exception when trying to read Summary file : " + exception.Message);
                }
                resultsLoaded = true;
                this.selectedResultsFile_textBox.Text = SelectSummaryFileDialog.FileName;
                this.eventInfo_textBox.Text = "Event Title : " + m_eventInformation.eventName + ", Event Date : " + m_eventInformation.eventDate.ToString("MMMM dd, yyyy");
                updateButtonStatus();
            }
            else
            {
                resultsLoaded = false;
                updateButtonStatus();
            }
        }

        private void publishResults_Click(object sender, EventArgs e)
        {
            loadSummaryIntoDatabase();
        }

        private void publishResultsCompleted(bool success)
        {
            m_publishResultsRunning = false;
            if (success) MessageBox.Show("Results published successfully to " + m_resultsWebsite, "Results Uploaded Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool m_publishResultsRunning = false;
        private void publishResultsInternal()
        {
            if (m_publishResultsRunning)
            {
                Utilities.showErrorMessage("A Publish Results operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            string resultsWebsite = websiteAddress_textBox.Text;
            if (string.IsNullOrWhiteSpace(resultsWebsite))
            {
                MessageBox.Show("Please provide a results website to publish to.");
                return;
            }
            string siteName, pagePath;
            Utilities.getGoogleSiteComponents(resultsWebsite, out siteName, out pagePath);
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            SitesAPI sa = new SitesAPI(siteName, username, password, true, false);
            CustomBackgroundWorker cbw = new CustomBackgroundWorker("Create Local Webpages", sa.uploadDirectoryInBackground, publishResultsCompleted, operationStatus,
operationProgressBar, operationCancelButton, null);
            Tuple<string, string> values = new Tuple<string, string>(m_eventInformation.webpagesDirectory, pagePath);
            m_publishResultsRunning = true;
            cbw.run(values);
        }


 
        private void loadSummaryCompleted(bool success)
        {
            m_loadSummaryRunning = false;
            if (success)
            {
                m_databaseParameters = std.getDatabaseParameters();
                createWebpages();
            }
        }

        private PairsSummaryToDatabase std;
        private bool m_loadSummaryRunning = false;
        private void loadSummaryIntoDatabase()
        {
            if (m_loadSummaryRunning)
            {
                Utilities.showErrorMessage("A Load Summary operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            std = new PairsSummaryToDatabase(m_eventInformation);
            CustomBackgroundWorker cbw = new CustomBackgroundWorker("Load Summary", std.loadSummaryIntoDatabaseInBackground, loadSummaryCompleted, operationStatus,
                operationProgressBar, operationCancelButton, null);
            m_loadSummaryRunning = true;
            cbw.run();

         }

        private void createWebpagesCompleted(bool success)
        {
            m_createWebpagesRunning = false;
            if(success) publishResultsInternal();
        }

        private bool m_createWebpagesRunning = false;
        private void createWebpages()
        {
            if (m_createWebpagesRunning)
            {
                Utilities.showErrorMessage("A Create Webpages operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            PairsDatabaseToWebpages dtw = new PairsDatabaseToWebpages(m_eventInformation, m_databaseParameters);
            CustomBackgroundWorker cbw = new CustomBackgroundWorker("Create Local Webpages", dtw.createWebpagesInBackground, createWebpagesCompleted, operationStatus,
    operationProgressBar, operationCancelButton, null);
            m_createWebpagesRunning = true;
            cbw.run();
        }
    }
}
