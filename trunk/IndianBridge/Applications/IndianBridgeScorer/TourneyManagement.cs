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
        public TourneyManagement()
        {
            InitializeComponent();
            loadExistingTourneys();
        }

        private void loadExistingTourneys()
        {
            // Populate the existing tourney list
            string[] tourneys = Directory.GetDirectories(Constants.getTourneysFolder());
            tourneyListCombobox.Items.Clear();
            foreach (string tourney in tourneys)
            {
                // Add to list if tourney info file exists
                if (tourneyExists(tourney)) tourneyListCombobox.Items.Add(Path.GetFileName(tourney));
            }
            if (tourneyListCombobox.Items.Count > 0) tourneyListCombobox.SelectedIndex = 0;
        }

        private bool tourneyExists(string tourneyFolderName)
        {
            string oldFolderName = Constants.CurrentTourneyFolderName;
            Constants.CurrentTourneyFolderName = tourneyFolderName;
            string tourneyInfoFileName = Constants.getCurrentTourneyInformationFileName();
            Constants.CurrentTourneyFolderName = oldFolderName;
            return File.Exists(tourneyInfoFileName);
        }

        private void loadTourney(string selectedFolder)
        {
            Constants.CurrentTourneyFolderName = selectedFolder;
            loadTourneyInfo();
            loadTourneyEvents();
            EventManagement eventManagement = new EventManagement();
            this.Hide();
            eventManagement.ShowDialog();
            this.Close();
        }

        private void loadExistingTourneyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tourneyListCombobox.Text))
            {
                Utilities.showErrorMessage("No Existing Tourneys were found! Please create a new tourney first.");
                return;
            }
            loadTourney(tourneyListCombobox.Text);
        }

        private void loadTourneyInfo()
        {
            string niniFileName = Constants.getCurrentTourneyInformationFileName();
            NiniUtilities.loadNiniConfig(niniFileName);
        }

        private void loadTourneyEvents()
        {
            string databaseFileName = Constants.getCurrentTourneyEventsFileName();
            if (!File.Exists(databaseFileName))
            {
                AccessDatabaseUtilities.createDatabase(databaseFileName);
                List<DatabaseField> fields = new List<DatabaseField>();
                fields.Add(new DatabaseField("Event_Name", "TEXT", 255));
                fields.Add(new DatabaseField("Event_Type", "TEXT", 255));
                List<string> primaryKeyFields = new List<string>();
                primaryKeyFields.Add("Event_Name");
                AccessDatabaseUtilities.createTable(databaseFileName, Constants.TourneyEventsTableName, fields, primaryKeyFields);
            }
            else AccessDatabaseUtilities.loadDatabaseToTable(databaseFileName, Constants.TourneyEventsTableName);
        }



        private void createTourneyButton_Click(object sender, EventArgs e)
        {
            SetTourneyInfo sti = new SetTourneyInfo();
            DialogResult result = sti.ShowDialog();
            if (result == DialogResult.Cancel) return;
            string tourneyName = NiniUtilities.getStringValue(Constants.getRootTourneyInformationFile(), Constants.TourneyNameFieldName);
            string oldFolderName = Constants.CurrentTourneyFolderName;
            Constants.CurrentTourneyFolderName = Constants.generateTourneyFolder(tourneyName);
            if (tourneyExists(Constants.CurrentTourneyFolderName))
            {
                if (MessageBox.Show("Tourney already exists! Do you want to erase all existing contents?" + Environment.NewLine + "If you want to load an already existing tourney click NO and select your tourney from drop down list and click Load Existing Tourney Button.", "Tourney Already Exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
                    Constants.CurrentTourneyFolderName = oldFolderName;
                    return;
                }
                Directory.Delete(Constants.getCurrentTourneyFolder(),true);
            }
            
            File.Copy(Constants.getRootTourneyInformationFile(), Constants.getCurrentTourneyInformationFileName(), true);
            loadTourney(Constants.CurrentTourneyFolderName);
        }

        private void deleteTourneyButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?" + Environment.NewLine + "All contents of the tourney will be delete!", "Confirm tourney delete!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string oldFolderName = Constants.CurrentTourneyFolderName;
                Constants.CurrentTourneyFolderName = tourneyListCombobox.Text;
                Directory.Delete(Constants.getCurrentTourneyFolder(), true);
                tourneyListCombobox.Items.Remove(Constants.CurrentTourneyFolderName);
                Constants.CurrentTourneyFolderName = oldFolderName;
            }
        }
    }
}
