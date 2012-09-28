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

namespace IndianBridgeScorer
{
    public partial class EventManagement : Form
    {
        private string m_tourneyInfoFileName = "";
        private string m_tourneyEventsFileName = "";
        private string m_tourneyName;
        TourneyInfo m_tourneyInfo = null;

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
            AccessDatabaseUtilities.loadDatabaseToTable(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
            loadEvents();
        }

        private void loadEvents()
        {
            eventsDataGridView.DataSource = null;
            eventsDataGridView.Columns.Clear();
            eventsDataGridView.DataSource = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
            var buttonCol = new DataGridViewButtonColumn();
            buttonCol.Name = "Show Event";
            buttonCol.HeaderText = "Show Event";
            buttonCol.Text = "Show Event";
            buttonCol.UseColumnTextForButtonValue = true;
            eventsDataGridView.Columns.Add(buttonCol);
            buttonCol.FlatStyle = FlatStyle.Popup;
            buttonCol.DefaultCellStyle.ForeColor = Color.Green;
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
            DataTable table = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
            DataRow dRow = table.Rows.Find(eventName);
            string eventType = (string)dRow["Event_Type"];
            switch (eventType)
            {
                case "Team":
                    TeamsScorer ts = new TeamsScorer(eventName);
                    m_scorers[eventName] = ts;
                    break;
                case "Pairs":
                    PairsScorer ps = new PairsScorer(eventName);
                    m_scorers[eventName] = ps;
                    break;
                case "PD":
                    PDScorer pds = new PDScorer(eventName);
                    m_scorers[eventName] = pds;
                    break;
            }
        }

        public void deleteEvent(string eventName)
        {
            DialogResult result = MessageBox.Show("Are you sure?" + Environment.NewLine + "All information for this event will be deleted", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            DataTable table = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
            DataRow dRow = table.Rows.Find(eventName);
            dRow.Delete();
            AccessDatabaseUtilities.saveTableToDatabase(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
            if (m_scorers.ContainsKey(eventName) && m_scorers[eventName] != null && !m_scorers[eventName].IsDisposed) m_scorers[eventName].Close();
            m_scorers[eventName] = null;
            m_scorers.Remove(eventName);
        }

        private void addEvent(string eventType)
        {
            string eventName = Microsoft.VisualBasic.Interaction.InputBox("What is the name of the event?", "Event Name");
            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event Name cannot be empty!", "Empty Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable table = AccessDatabaseUtilities.getDataTable(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
            DataRow dRow = table.Rows.Find(eventName);
            if (dRow != null)
            {
                MessageBox.Show("Another event with same name (" + eventName + ") already exists!" + Environment.NewLine + "Either delete the other event first or provide a different event name!", "Duplicate Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dRow = table.NewRow();
            dRow["Event_Name"] = eventName;
            dRow["Event_Type"] = eventType;
            table.Rows.Add(dRow);
            AccessDatabaseUtilities.saveTableToDatabase(m_tourneyEventsFileName, Constants.TourneyEventsTableName);
        }

        private void eventsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == eventsDataGridView.Columns["Show Event"].Index)
            {
                string eventName = (string)eventsDataGridView.Rows[e.RowIndex].Cells["Event_Name"].Value;
                showEvent(eventName);
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
            addEvent("Team");
        }

        private void addNewPairEventButton_Click(object sender, EventArgs e)
        {
            addEvent("Pairs");
        }

        private void addNewPDEventButton_Click(object sender, EventArgs e)
        {
            addEvent("PD");
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
            Utilities.showBalloonNotification("Save Success", "Saved Tourney Info to Database");
        }

    }
}
