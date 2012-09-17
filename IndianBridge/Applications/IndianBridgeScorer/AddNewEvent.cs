using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IndianBridgeScorer
{
    public partial class AddNewEvent : Form
    {
        TourneyInformationDatabase m_tid;
        public AddNewEvent(TourneyInformationDatabase tid)
        {
            m_tid = tid;
            string tourneyName = (string)tid.m_ds.Tables[TourneyInformationDatabase.tourneyInfoTableName].Rows[0]["Tourney_Name"];
            InitializeComponent();
            this.Text = "Add New Event To Tourney : " + tourneyName;
        }

        private void addNewEventButton_Click(object sender, EventArgs e)
        {
            bool error = false;
            string message = "";
            if (string.IsNullOrWhiteSpace(eventNameTextbox.Text))
            {
                error = true;
                message += "Event Name cannot be empty";
            }
            else if (m_tid.eventExists(eventNameTextbox.Text))
            {
                error = true;
                message += "Event with name : "+eventNameTextbox.Text+" already exists in database. If you are creating a different event then provide a different name.";
            }
            if (string.IsNullOrWhiteSpace(eventTypeComboBox.Text))
            {
                error = true;
                message += "\nPlease select an Event Type from the drop down box";
            }
            if (error)
            {
                MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_tid.addNewEvent(eventNameTextbox.Text,eventTypeComboBox.Text);
            this.Close();
        }

        private void cancelAddNewEventButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
