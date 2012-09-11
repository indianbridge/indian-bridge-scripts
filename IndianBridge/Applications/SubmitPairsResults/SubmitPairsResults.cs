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

namespace SubmitPairsResults
{
    public partial class SubmitPairsResults : Form
    {
        private Boolean resultsLoaded = false, eventSelected = false, publishEnabled = false;
        private PairsEventInformation m_eventInformation = PairsGeneral.createDefaultEventInformation();
        SelectCalendarEvent selectCalendarEventForm;
        private string googleSiteName = "indiancitybridgeresults";
        String googleSiteRootPageName = "";
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
                googleSiteRootPageName = "/results/" + Utilities.makeIdentifier_(cityName) + "/" + Utilities.makeIdentifier_(eventTitle + "  " + eventDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                m_eventInformation.databaseFileName =
                    Path.Combine(Path.Combine(Globals.m_rootDirectory, "Databases", cityName, eventName), eventDate.ToString("yyyy_MM_dd")) + ".mdb";
                m_eventInformation.webpagesDirectory =
                    Path.Combine(Path.Combine(Globals.m_rootDirectory, "Webpages", cityName, eventName), eventDate.ToString("yyyy_MM_dd"));
                googleSiteRootPageName = "/results/" + Utilities.makeIdentifier_(cityName) + "/" + Utilities.makeIdentifier_(eventName) + "/" + eventDate.ToString("yyyy-MM-dd");
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
                this.websiteAddress_textBox.Text = "https://sites.google.com/site/"+googleSiteName+googleSiteRootPageName;
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
            PublishResults publishResults = new PublishResults(m_eventInformation,googleSiteName,googleSiteRootPageName);
            publishResults.ShowDialog(this);
            publishResults.Dispose();
        }
    }
}
