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

namespace IndianBridgeScorer
{
    public partial class EventManagement : Form
    {
        TourneyInformationDatabase m_tid;
        Dictionary<string, Form> m_scorers;
        public EventManagement(TourneyInformationDatabase tid)
        {
            m_tid = tid;
            InitializeComponent();
            m_scorers = new Dictionary<string, Form>();
            this.Text = "Tourney Name : " + tid.getTourneyName();
            tourneyNameTextBox.Text = tid.getTourneyName();
            resultsWebsiteTextBox.Text = tid.getTourneyResultsWebsite();
            updateEvents();
        }

        public void updateEvents() {
            eventsList.Controls.Clear();
            DataTable table = m_tid.m_ds.Tables[TourneyInformationDatabase.tourneyEventsTableName];
            Label label = new Label();
            label.Text = "Event Name";
            label.ForeColor = Color.Blue;
            eventsList.Controls.Add(label, 0, 0);
            label = new Label();
            label.Text = "Event Type";
            label.ForeColor = Color.Blue;
            eventsList.Controls.Add(label, 1, 0);

            for (int i = 0; i < table.Rows.Count; ++i)
            {
                int rowNumber = i + 1;
                DataRow dRow = table.Rows[i];
                label = new Label();
                string eventName = (string)dRow["Event_Name"];
                label.Text = eventName;
                eventsList.Controls.Add(label, 0, rowNumber);
                label = new Label();
                label.Text = (string)dRow["Event_type"];
                eventsList.Controls.Add(label, 1, rowNumber);
                Button button = new Button();
                button.Text = "Show Event";
                button.BackColor = Color.FromArgb(128, 128, 255);
                eventsList.Controls.Add(button, 2, rowNumber);
                button.Tag = eventName;
                button.Click += delegate(object sender, EventArgs e)
                {
                    showEvent(sender as Button);
                };
                button = new Button();
                button.Text = "Delete Event";
                button.BackColor = Color.Red;
                button.Tag = eventName;
                button.Click += delegate(object sender, EventArgs e)
                {
                    deleteEvent(sender as Button);
                };
                eventsList.Controls.Add(button, 3, rowNumber);
            }
        }

        public void showEvent(Button button)
        {
            string eventName = (string)button.Tag;
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
            DataTable table = m_tid.m_ds.Tables[TourneyInformationDatabase.tourneyEventsTableName];
            DataRow dRow = table.Rows.Find(eventName);
            string eventType = (string)dRow["Event_Type"];
            string databaseFileName = Path.Combine(Globals.m_rootDirectory,"Tourneys",(string)dRow["Event_File"]);
            DataTable infoTable = m_tid.m_ds.Tables[TourneyInformationDatabase.tourneyInfoTableName];
            DataRow foundRow = infoTable.Rows[0];
            string websiteRoot = resultsWebsiteTextBox.Text;
            string websiteAddress = websiteRoot + "/" + Utilities.makeIdentifier_(eventName);
            switch (eventType)
            {
                case "Team":
                    TeamScorer ts = new TeamScorer(m_tid, eventName, databaseFileName);
                    m_scorers[eventName] = ts;
                    break;
                case "Pairs":
                    PairsScorer ps = new PairsScorer(m_tid, eventName, databaseFileName);
                    m_scorers[eventName] = ps;
                    break;
                case "PD":
                    PDScorer pds = new PDScorer(m_tid, eventName,databaseFileName);
                    m_scorers[eventName] = pds;
                    break;
            }
        }

        public void deleteEvent(Button button)
        {
            string eventName = (string)button.Tag;
            DialogResult result = MessageBox.Show("Are you sure?\nAll information for this event will be deleted", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;
            m_tid.deleteEvent(eventName);
            if (m_scorers.ContainsKey(eventName) && m_scorers[eventName] != null && !m_scorers[eventName].IsDisposed) m_scorers[eventName].Close();
            m_scorers[eventName] = null;
            m_scorers.Remove(eventName);
            updateEvents();
        }

        private void addEvent(string eventType)
        {
            string eventName = Microsoft.VisualBasic.Interaction.InputBox("What is the name of the event?", "Event Name");
            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event Name cannot be empty!", "Empty Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_tid.eventExists(eventName))
            {
                MessageBox.Show("Another event with same name ("+eventName+") already exists!"+Environment.NewLine+"Either delete the other event first or provide a different event name!", "Duplicate Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_tid.addNewEvent(eventName,eventType);
            updateEvents();
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

    }
}
