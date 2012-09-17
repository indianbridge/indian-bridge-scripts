using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IndianBridge.Common;

namespace IndianBridgeScorer
{
    public partial class TourneyManagement : Form
    {
        SelectCalendarEvent selectCalendarEventForm;
        EventManagement eventManagement;
        public TourneyManagement()
        {
            Globals.m_rootDirectory = Directory.GetCurrentDirectory();
            InitializeComponent();
            selectCalendarEventForm = new SelectCalendarEvent(DateTime.Today.AddDays(1),DateTime.Today.AddDays(-7));
        }

        private void loadExistingTourneyButton_Click(object sender, EventArgs e)
        {
            selectTourneyRootFolder.SelectedPath = Path.Combine(Directory.GetCurrentDirectory(), "Tourneys");
            DialogResult result = selectTourneyRootFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                string destinationFileName = Path.Combine(selectTourneyRootFolder.SelectedPath, "TourneyInformation.mdb");           
                TourneyInformationDatabase tid = new TourneyInformationDatabase(destinationFileName);
                eventManagement = new EventManagement(tid);
                this.Hide();
                eventManagement.ShowDialog();
                this.Close();
            }
        }

        private void createTourneyButton_Click(object sender, EventArgs e)
        {
            selectCalendarEventForm.ShowDialog(this);
            if (selectCalendarEventForm.calendarEventInfo == null)
            {
                return;
            }
            string eventName = selectCalendarEventForm.calendarEventInfo.Item1;
            DateTime eventDate = selectCalendarEventForm.calendarEventInfo.Item2;
            string rootDirectory = Path.Combine(Globals.m_rootDirectory, "Tourneys", Utilities.makeIdentifier_(eventName) + "_" + eventDate.ToString("yyyy_MM_dd"));
            if (Directory.Exists(rootDirectory)) {
                DialogResult result = MessageBox.Show("A folder already exists at " + rootDirectory + "\nDo you want to erase all contents and create a new tourney?", "Tourney Exists!!!", MessageBoxButtons.YesNo);
                if (result == DialogResult.No) return;
                Directory.Delete(rootDirectory, true);
            }

            Directory.CreateDirectory(rootDirectory);
            string sourceFileName = Path.Combine(Directory.GetCurrentDirectory(), "TourneyInformationDatabaseTemplate.mdb");
            string destinationFileName = Path.Combine(rootDirectory, "Databases","TourneyInformation.mdb");
            System.IO.File.Copy(sourceFileName, destinationFileName);
            TourneyInformationDatabase tid = new TourneyInformationDatabase(destinationFileName);
            GetGoogleWebsiteRoot ggwr = new GetGoogleWebsiteRoot(eventName);
            ggwr.ShowDialog();
            tid.setTourneyInfo(eventName,eventDate,ggwr.websiteRoot);
            eventManagement = new EventManagement(tid);
            this.Hide();
            eventManagement.ShowDialog();
            this.Close();
        }
    }
}
