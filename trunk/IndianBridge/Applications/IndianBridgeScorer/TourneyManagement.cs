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
        //SelectCalendarEvent selectCalendarEventForm;
        EventManagement eventManagement;
        public TourneyManagement()
        {
            Globals.m_rootDirectory = Directory.GetCurrentDirectory();
            InitializeComponent();
            loadExistingTourneys();
            //selectCalendarEventForm = new SelectCalendarEvent(DateTime.Today.AddDays(1),DateTime.Today.AddDays(-7));
        }

        private void loadExistingTourneys()
        {
            string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "Tourneys");
            string[] tourneys = Directory.GetDirectories(rootFolder);
            tourneyListCombobox.Items.Clear();
            foreach (string tourney in tourneys) tourneyListCombobox.Items.Add(Path.GetFileName(tourney));
            if (tourneys.Length > 0) tourneyListCombobox.SelectedIndex = 0;
        }

        private void loadExistingTourneyButton_Click(object sender, EventArgs e)
        {
            string selectedTourney = tourneyListCombobox.Text;
            string selectedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Tourneys", selectedTourney);
            if (!Directory.Exists(selectedFolder))
            {
                MessageBox.Show(selectedFolder + " tourney folder does not exist!", "Missing Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string destinationFileName = Path.Combine(selectedFolder,"Databases", "TourneyInformation.mdb");
            if (!File.Exists(destinationFileName))
            {
                MessageBox.Show(selectedFolder + " folder does not contains tourney databases!", "Missing database files!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TourneyInformationDatabase tid = new TourneyInformationDatabase(selectedTourney);
            eventManagement = new EventManagement(tid);
            this.Hide();
            eventManagement.ShowDialog();
            this.Close();
        }

        private string createTourneyFolderName(string tourneyName, DateTime eventDate)
        {
            return Utilities.makeIdentifier_(tourneyName) + "_" + eventDate.ToString("yyyy_MM_dd");
        }

        private void createTourneyButton_Click(object sender, EventArgs e)
        {
            SetTourneyInfo sti = new SetTourneyInfo();
            sti.ShowDialog();
            if (sti.cancelPressed) return;
            DateTime eventDate = DateTime.Now;
            string tourneyFolderName = createTourneyFolderName(sti.tourneyName,eventDate);
            string rootDirectory = Path.Combine(Globals.m_rootDirectory, "Tourneys", tourneyFolderName);
            if (Directory.Exists(rootDirectory))
            {
                DialogResult result = MessageBox.Show("A folder already exists at " + rootDirectory + Environment.NewLine + "Do you want to erase all contents and create a new tourney?", "Tourney Exists!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
                Directory.Delete(rootDirectory, true);
            }
            Directory.CreateDirectory(rootDirectory);
            Directory.CreateDirectory(Path.Combine(rootDirectory, "Databases"));
            Directory.CreateDirectory(Path.Combine(rootDirectory, "Webpages"));
            string sourceFileName = Path.Combine(Directory.GetCurrentDirectory(), "Databases", "TourneyInformationDatabaseTemplate.mdb");
            string destinationFileName = Path.Combine(rootDirectory, "Databases", "TourneyInformation.mdb");
            System.IO.File.Copy(sourceFileName, destinationFileName);
            TourneyInformationDatabase tid = new TourneyInformationDatabase(tourneyFolderName);
            tid.setTourneyInfo(sti.tourneyName, eventDate, sti.resultsWebsiteRoot);
            eventManagement = new EventManagement(tid);
            this.Hide();
            eventManagement.ShowDialog();
            this.Close();
        }

        private void deleteTourneyButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?" + Environment.NewLine + "All contents of the tourney will be delete!", "Confirm tourney delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string selectedTourney = tourneyListCombobox.Text;
                string selectedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Tourneys", selectedTourney);
                Directory.Delete(selectedFolder, true);
                loadExistingTourneys();
            }
        }
    }
}
