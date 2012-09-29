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
            this.Text = "Pairs Scorer for " + m_eventName;
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

        private void publishResultsProgress(object sender, ProgressChangedEventArgs e)
        {
            operationStatus.ForeColor = Color.Orange;
            operationStatus.Text = e.UserState as string;
            operationProgressBar.Value = e.ProgressPercentage;
        }

        private void publishResultsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                operationStatus.ForeColor = Color.Red;
                operationStatus.Text = "Cancelled!";
                operationProgressBar.Value = 0;
            }

            else if (!(e.Error == null))
            {
                operationStatus.ForeColor = Color.Red;
                operationStatus.Text = "Error: ";
                Utilities.showErrorMessage(e.Error.Message);
                operationProgressBar.Value = 0;
            }
            else
            {
                operationStatus.ForeColor = Color.Green;
                operationStatus.Text = "Results published successfully to "+m_resultsWebsite;
                operationProgressBar.Value = 100;
                operationCancelButton.Text = "";
            }
        }


        private void publishResultsInternal()
        {
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
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork +=
                            new DoWorkEventHandler(sa.uploadDirectoryInBackground);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(publishResultsCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(publishResultsProgress);
            if (backgroundWorker1.IsBusy != true)
            {
                operationStatus.ForeColor = Color.Orange;
                operationStatus.Text = "Publish Results : Starting...";
                operationCancelButton.Text = "Cancel Publish Results";
                operationProgressBar.Value = 0;
                Tuple<string, string> values = new Tuple<string, string>(m_eventInformation.webpagesDirectory, pagePath);
                backgroundWorker1.RunWorkerAsync(values);
            }
            else
            {
                Utilities.showWarningessage("A Publish Results operation is already running! To start a new one cancel the previous one using the button in the status bar or wait for it to complete.");
            }
        }

        private void loadSummaryProgress(object sender, ProgressChangedEventArgs e)
        {
            operationStatus.ForeColor = Color.Orange;
            operationStatus.Text = e.UserState as string;
            operationProgressBar.Value = e.ProgressPercentage;
        }

        private void loadSummaryCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                operationStatus.ForeColor = Color.Red;
                operationStatus.Text = "Cancelled!";
                operationProgressBar.Value = 0;
            }

            else if (!(e.Error == null))
            {
                operationStatus.ForeColor = Color.Red;
                operationStatus.Text = "Error: ";
                Utilities.showErrorMessage(e.Error.Message);
                operationProgressBar.Value = 0;
            }
            else
            {
                operationStatus.ForeColor = Color.Green;
                operationStatus.Text = "Done!";
                operationProgressBar.Value = 100;
                m_databaseParameters = std.getDatabaseParameters();
                createWebpages();
            }
        }

        PairsSummaryToDatabase std;
        private void loadSummaryIntoDatabase()
        {
            std = new PairsSummaryToDatabase(m_eventInformation);
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork +=
                            new DoWorkEventHandler(std.loadSummaryIntoDatabaseInBackground);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(loadSummaryCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(loadSummaryProgress);
            if (backgroundWorker1.IsBusy != true)
            {
                operationStatus.ForeColor = Color.Orange;
                operationStatus.Text = "Load Summary : Starting...";
                operationCancelButton.Text = "Cancel Load Summary";
                operationProgressBar.Value = 0;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                Utilities.showWarningessage("A Load Summary operation is already running! To start a new one cancel the previous one using the button in the status bar or wait for it to complete.");
            }
        }


        private void createWebpagesProgress(object sender, ProgressChangedEventArgs e)
        {
            operationStatus.ForeColor = Color.Orange;
            operationStatus.Text = e.UserState as string;
            operationProgressBar.Value = e.ProgressPercentage;
        }

        private void createWebpagesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                operationStatus.ForeColor = Color.Red;
                operationStatus.Text = "Cancelled!";
                operationProgressBar.Value = 0;
            }

            else if (!(e.Error == null))
            {
                operationStatus.ForeColor = Color.Red;
                operationStatus.Text = "Error: ";
                Utilities.showErrorMessage(e.Error.Message);
                operationProgressBar.Value = 0;
            }
            else
            {
                operationStatus.ForeColor = Color.Green;
                operationStatus.Text = "Done!";
                operationProgressBar.Value = 100;
                publishResultsInternal();
            }
        }

        private void createWebpages()
        {
            PairsDatabaseToWebpages dtw = new PairsDatabaseToWebpages(m_eventInformation, m_databaseParameters);
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork +=
                            new DoWorkEventHandler(dtw.createWebpagesInBackground);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(createWebpagesCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(createWebpagesProgress);
            if (backgroundWorker1.IsBusy != true)
            {
                operationStatus.ForeColor = Color.Orange;
                operationStatus.Text = "Create Local Webpages : Starting...";
                operationCancelButton.Text = "Cancel Create Local Webpages";
                operationProgressBar.Value = 0;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                Utilities.showWarningessage("A Create Webpages operation is already running! To start a new one cancel the previous one using the button in the status bar or wait for it to complete.");
            }
        }

    }
}
