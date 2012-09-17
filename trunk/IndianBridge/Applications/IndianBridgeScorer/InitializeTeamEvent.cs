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
    public partial class InitializeTeamEvent : Form
    {

        public bool cancelPressed = true;
        private DataSet m_ds;
        private string eventName = "";
        private int numberOfRounds = 0, numberOfTeams = 0, numberOfBoards = 0;
        private string previousEventName = "";
        private int previousNumberOfRounds = 0, previousNumberOfTeams = 0, previousNumberOfBoards = 0;
        private bool dataChanged = false;
        public InitializeTeamEvent(DataSet ds)
        {
            m_ds = ds;
            InitializeComponent();
            initializeFields();
        }

        private void updateTables()
        {
            if (numberOfBoards != previousNumberOfBoards)
            {

                TeamScorer.loadVPScale(m_ds);
            }
            DataTable table = m_ds.Tables[TeamScorer.namesTableName];
            if (numberOfTeams > previousNumberOfTeams)
            {
                for (int i = previousNumberOfTeams + 1; i <= numberOfTeams; ++i)
                {
                    DataRow dRow = table.NewRow();
                    dRow["Team_Number"] = i;
                    dRow["Team_Name"] = "Team " + i;
                    table.Rows.Add(dRow);
                }
            }
            else if (numberOfTeams < previousNumberOfTeams)
            {
                for (int i = numberOfTeams + 1; i <= previousNumberOfTeams; ++i)
                {
                    DataRow dRow = table.Rows.Find(i);
                    dRow.Delete();
                }
            }

            table = m_ds.Tables[TeamScorer.scoresTableName];
            int numberOfMatches = (numberOfTeams / 2) + numberOfTeams % 2;
            int previousNumberOfMatches = (previousNumberOfTeams / 2) + previousNumberOfTeams % 2;
            if (numberOfRounds > previousNumberOfRounds)
            {
                for (int i = previousNumberOfRounds + 1; i <= numberOfRounds; ++i)
                {
                    for (int j = 1; j <= numberOfMatches; ++j)
                    {
                        DataRow dRow = table.NewRow();
                        dRow["Table_Number"] = j;
                        dRow["Round_Number"] = i;
                        table.Rows.Add(dRow);
                    }
                }
            }
            else if (numberOfRounds < previousNumberOfRounds)
            {
                for (int i = numberOfRounds + 1; i <= previousNumberOfRounds; ++i)
                {
                    for (int j = 1; j <= previousNumberOfMatches; ++j)
                    {
                        Object[] keys = {i,j};
                        DataRow dRow = table.Rows.Find(keys);
                        dRow.Delete();
                    }
                }
            }
            if (numberOfMatches > previousNumberOfMatches)
            {
                for (int i = 1; i <= Math.Min(numberOfRounds, previousNumberOfRounds); ++i)
                {
                    for (int j = previousNumberOfMatches + 1; j <= numberOfMatches; ++j)
                    {
                        DataRow dRow = table.NewRow();
                        dRow["Table_Number"] = j;
                        dRow["Round_Number"] = i;
                        table.Rows.Add(dRow);
                    }
                }
            }
            else if (numberOfMatches < previousNumberOfMatches)
            {
                for (int i = 1; i <= Math.Min(numberOfRounds,previousNumberOfRounds); ++i)
                {
                    for (int j = numberOfMatches + 1; j <= previousNumberOfMatches; ++j)
                    {
                        Object[] keys = { i, j };
                        DataRow dRow = table.Rows.Find(keys);
                        dRow.Delete();
                    }
                }
            }
        }

        private void initializeFields()
        {
            DataTable table = m_ds.Tables[TeamScorer.infoTableName];
            if (table.Rows.Count > 0)
            {
                DataRow dRow = table.Rows[0];
                previousEventName = (string)dRow["Event_Name"];
                eventNameTextbox.Text = previousEventName;
                previousNumberOfRounds = (int)dRow["Number_Of_Rounds"];
                previousNumberOfTeams = (int)dRow["Number_Of_Teams"];
                previousNumberOfBoards = (int)dRow["Number_Of_Boards"];
                numberOfRoundsTextbox.Text = previousNumberOfRounds.ToString();
                numberOfTeamsTextBox.Text = previousNumberOfTeams.ToString();
                numberOfBoardsTextbox.Text = previousNumberOfBoards.ToString();
            }
        }

        public InitializeTeamEvent(string eventName)
        {
            InitializeComponent();
            eventNameTextbox.Text = eventName;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (verifyParameters())
            {
                if (dataChanged) updateTables();
                cancelPressed = !dataChanged;
                this.Close();
            }
        }


        private bool verifyParameters()
        {

            string message = "";
            bool error = false;
            dataChanged = false;
            eventName = eventNameTextbox.Text;
            if (string.IsNullOrWhiteSpace(eventName))
            {
                message += "Event Name cannot be empty!";
                error = true;
            }
            else if (eventName != previousEventName) dataChanged = true;
            bool result = int.TryParse(numberOfRoundsTextbox.Text, out numberOfRounds);
            if (!result || numberOfRounds < 1)
            {
                error = true;
                message += "\nNumber of Rounds " + numberOfRoundsTextbox.Text + " is not valid!";
            }
            else if (numberOfRounds != previousNumberOfRounds) dataChanged = true;
            result = int.TryParse(numberOfTeamsTextBox.Text, out numberOfTeams);
            if (!result || numberOfTeams < 2)
            {
                error = true;
                message += "\nNumber of Teams " + numberOfTeamsTextBox.Text + " is not valid!";
            }
            else if (numberOfTeams != previousNumberOfTeams) dataChanged = true;
            result = int.TryParse(numberOfBoardsTextbox.Text, out numberOfBoards);
            if (!result || numberOfBoards < 1)
            {
                error = true;
                message += "\nNumber of Boards " + numberOfBoardsTextbox.Text + " is not valid!";
            }
            
            if (!error && dataChanged)
            {
                if ((previousNumberOfRounds > 0 && numberOfRounds != previousNumberOfRounds) || (previousNumberOfTeams != 0 && numberOfTeams != previousNumberOfTeams))
                {
                    DialogResult dResult = MessageBox.Show("Team Names and Team scores may become inconsistent! Are you sure you want to change event setup?", "Loss of Information Risk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dResult == DialogResult.No)
                    {
                        return false;
                    }
                }
                DataTable table = m_ds.Tables[TeamScorer.infoTableName];
                DataRow dRow = (table.Rows.Count < 1)?table.NewRow():table.Rows[0];
                dRow["Event_Name"] = eventNameTextbox.Text;
                dRow["Number_Of_Rounds"] = numberOfRounds;
                dRow["Number_Of_Teams"] = numberOfTeams;
                dRow["Number_Of_Boards"] = numberOfBoards;
                dRow["Rounds_Completed"] = 0;
                dRow["Rounds_Scored"] = 0;
                dRow["Draws_Completed"] = 0;
                if (table.Rows.Count < 1) table.Rows.Add(dRow);
            }
            else if (error)
            {
                MessageBox.Show(message, "Error in information!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return !error;
        }

        private void canceButton_Click(object sender, EventArgs e)
        {
            cancelPressed = true;
            this.Close();
        }
    }
}
