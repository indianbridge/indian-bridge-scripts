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
using System.Diagnostics;
using IndianBridge.GoogleAPIs;

namespace IndianBridgeScorer
{
    public partial class EventManagement : Form
    {
        private string m_tourneyInfoFileName = "";
        private string m_tourneyEventsFileName = "";
        private string m_tourneyName;
        TourneyInfo m_tourneyInfo = null;
        private DataTable m_eventsTable;

        private Dictionary<string, Form> m_scorers = new Dictionary<string, Form>();
       
        public EventManagement()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            m_tourneyInfoFileName = Constants.getCurrentTourneyInformationFileName();
            m_tourneyEventsFileName = Constants.getCurrentTourneyEventsFileName();
            m_tourneyName = NiniUtilities.getStringValue(m_tourneyInfoFileName, Constants.TourneyNameFieldName);
            this.Text = "Tourney Name : " + m_tourneyName;
            m_tourneyInfo = new TourneyInfo(Constants.getCurrentTourneyInformationFileName(),false);
            this.tourneyInfoPropertyGrid.SelectedObject = m_tourneyInfo;
            m_eventsTable = AccessDatabaseUtilities.loadDatabaseToTable(m_tourneyEventsFileName, Constants.TableName.TourneyEvents);
            loadEvents();
        }

        private void loadEvents()
        {
            eventsDataGridView.DataSource = null;
            eventsDataGridView.Columns.Clear();
            eventsDataGridView.DataSource = m_eventsTable;
            var buttonCol = new DataGridViewButtonColumn();
            buttonCol.Name = "Show Event";
            buttonCol.HeaderText = "Show Event";
            buttonCol.Text = "Show Event";
            buttonCol.UseColumnTextForButtonValue = true;
            eventsDataGridView.Columns.Add(buttonCol);
            buttonCol.FlatStyle = FlatStyle.Popup;
            buttonCol.DefaultCellStyle.ForeColor = Color.Green;
            buttonCol = new DataGridViewButtonColumn();
            buttonCol.Name = "Copy Event";
            buttonCol.HeaderText = "Copy Event";
            buttonCol.Text = "Copy Event";
            buttonCol.UseColumnTextForButtonValue = true;
            buttonCol.FlatStyle = FlatStyle.Popup;
            buttonCol.DefaultCellStyle.ForeColor = Color.Blue;
            eventsDataGridView.Columns.Add(buttonCol);
            buttonCol = new DataGridViewButtonColumn();
            buttonCol.Name = "Delete Event";
            buttonCol.HeaderText = "Delete Event";
            buttonCol.Text = "Delete Event";
            buttonCol.UseColumnTextForButtonValue = true;
            buttonCol.FlatStyle = FlatStyle.Popup;
            buttonCol.DefaultCellStyle.ForeColor = Color.Red;
            eventsDataGridView.Columns.Add(buttonCol);
        }

        public void showEvent(string eventName)
        {
            if (!m_scorers.ContainsKey(eventName) || m_scorers[eventName] == null || m_scorers[eventName].IsDisposed)
            {
                createEvent(eventName);
            }
            m_scorers[eventName].Show();
            m_scorers[eventName].WindowState = FormWindowState.Normal;
            m_scorers[eventName].BringToFront();
            m_scorers[eventName].Focus();
        }

        public void createEvent(string eventName)
        {
            //DataTable table = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TableName.TourneyEvents);
            DataRow dRow = m_eventsTable.Rows.Find(eventName);
            string eventType = (string)dRow["Event_Type"];
            if (eventType == Constants.EventType.TeamsSwissLeague) {
                    TeamsScorer ts = new TeamsScorer(eventName);
                    m_scorers[eventName] = ts;
            }
            else if (eventType == Constants.EventType.TeamsKnockout) {
                    KnockoutScorer ks = new KnockoutScorer(eventName);
                    m_scorers[eventName] = ks;
            }
            else if (eventType == Constants.EventType.Pairs) {
                    PairsScorer ps = new PairsScorer(eventName);
                    m_scorers[eventName] = ps;
            }
            else if (eventType == Constants.EventType.PD) {
                    PDScorer pds = new PDScorer(eventName);
                    m_scorers[eventName] = pds;
            }
            else {
                Utilities.showErrorMessage("Unknown Event Type : " + eventType);
            }
        }

        public void copyEvent(string eventName)
        {
            DataRow dRow = m_eventsTable.Rows.Find(eventName);
            Debug.Assert(dRow != null, "Cannot find event with name : " + eventName + " in events table.");
            string eventType = (string)dRow["Event_Type"];
            string newEventName = addEvent(eventType, "What is the name of the copy of event : " + eventName);
            if (!string.IsNullOrEmpty(newEventName))
            {
                DirectoryCopy(Constants.getEventDatabasesFolder(eventName), Constants.getEventDatabasesFolder(newEventName), true);
                DirectoryCopy(Constants.getEventWebpagesFolder(eventName), Constants.getEventWebpagesFolder(newEventName), true);
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void deleteEvent(string eventName)
        {
            DialogResult result = MessageBox.Show("Are you sure?" + Environment.NewLine + "All information for this event will be deleted", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            //DataTable table = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TableName.TourneyEvents);
            DataRow dRow = m_eventsTable.Rows.Find(eventName);
            dRow.Delete();
            AccessDatabaseUtilities.saveTableToDatabase(m_tourneyEventsFileName, Constants.TableName.TourneyEvents);
            if (m_scorers.ContainsKey(eventName) && m_scorers[eventName] != null && !m_scorers[eventName].IsDisposed) m_scorers[eventName].Close();
            m_scorers[eventName] = null;
            m_scorers.Remove(eventName);
            Directory.Delete(Constants.getEventDatabasesFolder(eventName), true);
            Directory.Delete(Constants.getEventWebpagesFolder(eventName),true);
        }

        private string addEvent(string eventType, string message = "What is the name of the event?")
        {
            string eventName = Microsoft.VisualBasic.Interaction.InputBox("What is the name of the event?", "Event Name");
            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event Name cannot be empty!", "Empty Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            //DataTable table = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TableName.TourneyEvents);
            DataRow dRow = m_eventsTable.Rows.Find(eventName);
            if (dRow != null)
            {
                MessageBox.Show("Another event with same name (" + eventName + ") already exists!" + Environment.NewLine + "Either delete the other event first or provide a different event name!", "Duplicate Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            dRow = m_eventsTable.NewRow();
            dRow["Event_Name"] = eventName;
            dRow["Event_Type"] = eventType;
            m_eventsTable.Rows.Add(dRow);
            AccessDatabaseUtilities.saveTableToDatabase(m_tourneyEventsFileName, Constants.TableName.TourneyEvents);
            return eventName;
        }

        private void eventsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == eventsDataGridView.Columns["Show Event"].Index)
            {
                string eventName = (string)eventsDataGridView.Rows[e.RowIndex].Cells["Event_Name"].Value;
                showEvent(eventName);
            }
            if (e.RowIndex >= 0 && e.ColumnIndex == eventsDataGridView.Columns["Copy Event"].Index)
            {
                string eventName = (string)eventsDataGridView.Rows[e.RowIndex].Cells["Event_Name"].Value;
                copyEvent(eventName);
            }
            if (e.RowIndex >= 0 && e.ColumnIndex == eventsDataGridView.Columns["Delete Event"].Index)
            {
                string eventName = (string)eventsDataGridView.Rows[e.RowIndex].Cells["Event_Name"].Value;
                deleteEvent(eventName);
            }
        }

        private void eventsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            eventsDataGridView.Columns["Event_Name"].ReadOnly = true;
            eventsDataGridView.Columns["Event_Type"].ReadOnly = true;
            if (eventsDataGridView.Columns.Contains("Event_File")) eventsDataGridView.Columns["Event_File"].Visible = false;
        }

        private void addNewTeamEventButton_Click(object sender, EventArgs e)
        {
            addEvent(Constants.EventType.TeamsSwissLeague);
        }

        private void addNewPairEventButton_Click(object sender, EventArgs e)
        {
            addEvent(Constants.EventType.Pairs);
        }

        private void addNewPDEventButton_Click(object sender, EventArgs e)
        {
            addEvent(Constants.EventType.PD);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload("Tourney Info"))
            {
                m_tourneyInfo.load();
                tourneyInfoPropertyGrid.Refresh();
                Utilities.showBalloonNotification("Reload Success", "Reloaded Tourney Info from Database");
            }
        }

        private void saveTourneyInfoButton_Click(object sender, EventArgs e)
        {
            string newTourneyName = m_tourneyInfo.TourneyName;
            if (newTourneyName != m_tourneyName)
            {
                Utilities.showErrorMessage("Tourney Name can only be set when you create the tourney and cannot be editted now!" + Environment.NewLine + "Tourney Name will not be changed but any changes to the Results Website will be changed.");
                m_tourneyInfo.TourneyName = m_tourneyName;
                tourneyInfoPropertyGrid.Refresh();
            }
            m_tourneyInfo.save();
            Constants.CurrentTourneyName = NiniUtilities.getStringValue(Constants.getCurrentTourneyInformationFileName(), Constants.TourneyNameFieldName);
            Constants.CurrentTourneyResultsWebsite = NiniUtilities.getStringValue(Constants.getCurrentTourneyInformationFileName(), Constants.ResultsWebsiteFieldName);
            Utilities.showBalloonNotification("Save Success", "Saved Tourney Info to Database");
        }

        private void addNewKnockoutButton_Click(object sender, EventArgs e)
        {
            addEvent(Constants.EventType.TeamsKnockout);
        }

        private void generateFollowOnSwissButton_Click(object sender, EventArgs e)
        {
            CreateNewEvents cne = new CreateNewEvents(false);
            cne.ShowDialog();
        }

        private void generateFollowOnKnockout_Click(object sender, EventArgs e)
        {
            CreateNewEvents cne = new CreateNewEvents(true);
            cne.ShowDialog();
        }

        private void uploadToGoogleDocsButton_Click(object sender, EventArgs e)
        {
            UploadTourney ut = new UploadTourney();
            ut.StartPosition = FormStartPosition.CenterParent;
            ut.ShowDialog(this);
        }

        void ChangeEnabled(bool enabled)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = enabled;
            }
        } 


    }
}
