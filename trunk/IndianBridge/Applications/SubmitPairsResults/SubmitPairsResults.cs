using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.ResultsManager;
using System.IO;
using IndianBridge.Common;
using IndianBridge.GoogleAPIs;

namespace SubmitPairsResults
{
    public partial class SubmitPairsResults : Form
    {
        private Boolean resultsLoaded = false, eventSelected = false, publishEnabled = false;
        private PairsEventInformation m_eventInformation = PairsGeneral.createDefaultEventInformation();
        private PairsDatabaseParameters m_databaseParameters = PairsGeneral.createDefaultDatabaseParameters();
        private SelectCalendarEvent selectCalendarEventForm;
        private string m_googleSiteName = "indiancitybridgeresults";
        string m_googleSiteRootPageName = "";

        private PairsSummaryToDatabase m_pairsSummaryToDatabase = null;
        private CustomBackgroundWorker m_loadSummaryCBW = null;
        private bool m_loadSummaryRunning = false;
        private PairsDatabaseToWebpages m_databaseToWebpages = null;
        private CustomBackgroundWorker m_createWebpagesCBW = null;
        private bool m_createWebpagesRunning = false;
        private SitesAPI m_sitesAPI = null;
        private CustomBackgroundWorker m_publishResultsCBW = null;
        private bool m_publishResultsRunning = false;

        public SubmitPairsResults()
        {
            InitializeComponent();
            Globals.m_rootDirectory = Directory.GetCurrentDirectory();
            selectCalendarEventForm = new SelectCalendarEvent();
        }

        private void updateButtonStatus()
        {
            publishEnabled = resultsLoaded && eventSelected;
            if (!resultsLoaded)
            {
                this.selectedResultsFile_textBox.Text = "";
                this.eventInfo_textBox.Text = "";
            }
            if (!eventSelected) this.selectedCalendarEvent_textBox.Text = "";
            if (!publishEnabled) this.websiteAddress_textBox.Text = "";
            this.loadResults.BackColor = resultsLoaded ? Color.Green : Color.Red;
            this.selectCalendarEvent.BackColor = eventSelected ? Color.Green : Color.Red;
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

        private string getCityName(DataTable table, string location)
        {
            if (table == null) return "Other Cities";
            // For each row, print the values of each column. 
            foreach (DataRow row in table.Rows)
            {
                string cityName = row["City_Name"].ToString();
                if (location.IndexOf(cityName, StringComparison.OrdinalIgnoreCase) >= 0) return cityName;
            }
            return "Other Cities";
        }

        private string getEventName(DataTable table, string cityName, string title)
        {
            if (table == null) return "Other Events";
            DataRow row = table.Rows.Find(cityName);
            if (row == null) return "Other Events";
            string eventName = row["Event_Names"].ToString();
            string[] tokens = eventName.Split(',');
            foreach (string token in tokens)
            {
                if (title.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0) return token;
            }
            return "Other Events";
        }

        private void deriveWebsiteAddress()
        {
            DataTable cityEvents = selectCalendarEventForm.calendarInfo.Item2;
            String eventLocation = selectCalendarEventForm.calendarEventInfo.Item3;
            DateTime eventDate = selectCalendarEventForm.calendarEventInfo.Item2;
            String eventTitle = selectCalendarEventForm.calendarEventInfo.Item1;
            String cityName = getCityName(cityEvents, eventLocation);
            String eventName = getEventName(cityEvents, cityName, eventTitle);
            if (eventName == "Other Events")
            {
                string pageName = Utilities.makeIdentifier_(eventTitle + "  " + eventDate.ToString("yyyy_MM_dd"));
                m_eventInformation.databaseFileName = Path.Combine(Globals.m_rootDirectory, "Databases", cityName, pageName) + ".mdb";
                m_eventInformation.webpagesDirectory = Path.Combine(Globals.m_rootDirectory, "Webpages", cityName, pageName);
                m_googleSiteRootPageName = "/results/" + Utilities.makeIdentifier_(cityName) + "/" + Utilities.makeIdentifier_(eventTitle + "  " + eventDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                m_eventInformation.databaseFileName =
                    Path.Combine(Path.Combine(Globals.m_rootDirectory, "Databases", cityName, eventName), eventDate.ToString("yyyy_MM_dd")) + ".mdb";
                m_eventInformation.webpagesDirectory =
                    Path.Combine(Path.Combine(Globals.m_rootDirectory, "Webpages", cityName, eventName), eventDate.ToString("yyyy_MM_dd"));
                m_googleSiteRootPageName = "/results/" + Utilities.makeIdentifier_(cityName) + "/" + Utilities.makeIdentifier_(eventName) + "/" + eventDate.ToString("yyyy-MM-dd");
            }

        }

        private void selectCalendarEvent_Click(object sender, EventArgs e)
        {
            selectCalendarEventForm.ShowDialog(this);
            if (selectCalendarEventForm.calendarEventInfo != null)
            {
                eventSelected = true;
                this.selectedCalendarEvent_textBox.Text = "Event Title : " + selectCalendarEventForm.calendarEventInfo.Item1 + ", Event Date : " + selectCalendarEventForm.calendarEventInfo.Item2.ToString("MMMM dd, yyyy") + ", Event Location : " + selectCalendarEventForm.calendarEventInfo.Item3;
                deriveWebsiteAddress();
                this.websiteAddress_textBox.Text = "https://sites.google.com/site/" + m_googleSiteName + m_googleSiteRootPageName;
                updateButtonStatus();
            }
            else
            {
                eventSelected = false;
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
            if (success)
            {
                string calendarMessage = "";
                if (SelectCalendarEvent.calendarAPI.updateResults(SelectCalendarEvent.selectedEntryNumber, "https://sites.google.com/site/" + m_googleSiteName + m_googleSiteRootPageName))
                {
                    calendarMessage = "Indian Bridge Calendar updated with Results";
                }
                else calendarMessage = "Not able to update Indian Bridge Calendar";
                MessageBox.Show("Results created and successfully uploaded to " + "https://sites.google.com/site/" + m_googleSiteName + m_googleSiteRootPageName + Environment.NewLine + calendarMessage, "Results Uploaded Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

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
            m_sitesAPI = new SitesAPI(siteName, username, password, true, false);
            m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", m_sitesAPI.uploadDirectoryInBackground, publishResultsCompleted, publishStatus,
                publishProgressBar, publishCancelButton, null);
            Tuple<string, string> values = new Tuple<string, string>(m_eventInformation.webpagesDirectory, pagePath);
            m_publishResultsRunning = true;
            m_publishResultsCBW.run(values);
        }



        private void loadSummaryCompleted(bool success)
        {
            m_loadSummaryRunning = false;
            if (success)
            {
                m_databaseParameters = m_pairsSummaryToDatabase.getDatabaseParameters();
                createWebpages();
            }
        }


        private void loadSummaryIntoDatabase()
        {
            if (m_loadSummaryRunning)
            {
                Utilities.showErrorMessage("A Load Summary operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }

            m_pairsSummaryToDatabase = new PairsSummaryToDatabase(m_eventInformation);
            m_loadSummaryCBW = new CustomBackgroundWorker("Load Summary", m_pairsSummaryToDatabase.loadSummaryIntoDatabaseInBackground, loadSummaryCompleted, loadSummaryStatus,
            loadSummaryProgressBar, loadSummaryCancelButton, null);
            m_loadSummaryRunning = true;
            m_loadSummaryCBW.run();

        }

        private void createWebpagesCompleted(bool success)
        {
            m_createWebpagesRunning = false;
            if (success) publishResultsInternal();
        }

        private void createWebpages()
        {
            if (m_createWebpagesRunning)
            {
                Utilities.showErrorMessage("A Create Webpages operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            m_databaseToWebpages = new PairsDatabaseToWebpages(m_eventInformation, m_databaseParameters);
            m_createWebpagesCBW = new CustomBackgroundWorker("Create Local Webpages", m_databaseToWebpages.createWebpagesInBackground, createWebpagesCompleted, createWebpagesStatus, createWebpagesProgressBar, createWebpagesCancelButton, null);
            m_createWebpagesRunning = true;
            m_createWebpagesCBW.run();
        }
    }
}
