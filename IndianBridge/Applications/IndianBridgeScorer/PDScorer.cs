using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using IndianBridge.Common;
using System.Diagnostics;
using System.Collections;
using System.Drawing.Printing;

namespace IndianBridgeScorer
{

    public partial class PDScorer : Form
    {
        //private EventParameters m_eventParameters = new EventParameters();
        private string m_databaseFileName = "";
        //private string m_localWebpagesRoot = "";
        private string m_resultsWebsite = "";
        private string m_eventName;
        public static string infoTableName = "Info";
        public static string teamsTableName = "Teams";
        public static string scoresTableName = "Scores";
        public static string computedScoresTableName = "ComputedScores";
        public static string numberOfTeamsString = "Number_Of_Teams";
        public static string numberOfBoardsString = "Number_Of_Boards";
        public static string numberOfRoundsString = "Number_Of_Rounds";


        public PDScorer(string eventName)
        {
            InitializeComponent();
            m_eventName = eventName;
            //m_eventParameters = new EventParameters();
            initialize();
        }

        private DataTable loadTable(string tableName)
        {
            return AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, tableName);
        }

        private DataTable getTable(string tableName)
        {
            return AccessDatabaseUtilities.getDataTable(m_databaseFileName, tableName);
        }

        private void saveTable(string tableName)
        {
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, tableName);
        }

        private string getStringValue(DataRow dRow, string columnName)
        {
            return AccessDatabaseUtilities.getStringValue(dRow, columnName);
        }

        private int getIntValue(DataRow dRow, string columnName)
        {
            return AccessDatabaseUtilities.getIntValue(dRow, columnName);
        }

        private double getDoubleValue(DataRow dRow, string columnName)
        {
            return AccessDatabaseUtilities.getDoubleValue(dRow, columnName);
        }

        private void createPDDatabase()
        {
            AccessDatabaseUtilities.createDatabase(m_databaseFileName);
            createInfoTable();
            createTeamsTable();
            createScoresTable();
        }

        private void createScoresTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Round", "INTEGER"));
            fields.Add(new DatabaseField("Board", "INTEGER"));
            fields.Add(new DatabaseField("NS_Number", "INTEGER"));
            fields.Add(new DatabaseField("EW_Number", "INTEGER"));
            fields.Add(new DatabaseField("NS_Score", "NUMBER"));
            fields.Add(new DatabaseField("EW_Score", "NUMBER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Board");
            primaryKeys.Add("NS_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, scoresTableName, fields, primaryKeys);
            loadTable(scoresTableName);
        }

        private void createTeamsTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Name", "TEXT",255));
            fields.Add(new DatabaseField("Members", "TEXT",255));
            fields.Add(new DatabaseField("Score", "NUMBER"));
            fields.Add(new DatabaseField("Boards_Scored", "INTEGER"));
            fields.Add(new DatabaseField("Percentage", "NUMBER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Team_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, teamsTableName, fields, primaryKeys);
            loadTable(teamsTableName);
        }

        private void createInfoTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Parameter_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Parameter_Value", "INTEGER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Parameter_Name");
            AccessDatabaseUtilities.createTable(m_databaseFileName, infoTableName, fields, primaryKeys);
            DataTable table = loadTable(infoTableName);
            string[] parameters = new string[] { "Number_Of_Team", "Number_Of_Boards", "Number_Of_Rounds" };
            foreach (string parameter in parameters)
            {
                DataRow dRow = table.NewRow();
                dRow["Parameter_Name"] = parameter;
                dRow["Parameter_Value"] = 0;
                table.Rows.Add(dRow);
            }
            saveTable(infoTableName);
        }

        private string getEventParameter(string parameterName, int parameterValue)
        {
            string message = "";
            if (parameterValue < 2) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + parameterName + " has to at least 2!";
            return message;
        }

        private string checkEventParameters()
        {
            string message = "";
            DataTable table = getTable(infoTableName);
            Debug.Assert(table.Rows.Count == 3);
            foreach (DataRow dRow in table.Rows)
            {
                message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine)+getEventParameter(getStringValue(dRow, "Parameter_Name"), getIntValue(dRow, "Parameter_Value"));
            }
            return message;
        }

        private bool isEventSetup()
        {
            DataTable table = getTable(infoTableName);
            Debug.Assert(table.Rows.Count == 3);
            foreach (DataRow dRow in table.Rows)
            {
                if (getIntValue(dRow, "Parameter_Value") < 2) return false;
            }
            return true;
        }

        private void initialize()
        {
            /*this.Text = LocalUtilities.getTourneyName() + " : " + m_eventName;
            m_databaseFileName = LocalUtilities.getEventDatabase(m_eventName);
            m_localWebpagesRoot = LocalUtilities.getEventWebpagesFolder(m_eventName);
            m_resultsWebsite = LocalUtilities.getEventResultsWebsite(m_eventName);*/
            websiteResultsTextbox.Text = m_resultsWebsite;
            // Save all tabs
            Utilities.saveTabs(mainControlTab);
            // Enable copy paste in all datagridview's
            foreach (DataGridView control in this.Controls.OfType<DataGridView>())
            {
                control.KeyDown += new KeyEventHandler(Utilities.handleCopyPaste);
            }

            // Check if database already exists
            if (!File.Exists(m_databaseFileName))
            {
                //create database first
                createPDDatabase();
            }
            eventSetupDataGridView.DataSource = loadTable(infoTableName);
            teamsDataGridView.DataSource = loadTable(teamsTableName);
            scoresDataGridView.DataSource = loadTable(scoresTableName);
            if (isEventSetup())
            {
                Utilities.showTabs(mainControlTab);
                mainControlTab.SelectedTab = mainControlTab.TabPages["scoresTab"];
            }
            else
            {
                Utilities.hideTabs(mainControlTab);
                List<string> tabNames = new List<string>();
                tabNames.Add("eventSetupTab");
                Utilities.showTabs(mainControlTab, tabNames);
                mainControlTab.SelectedTab = mainControlTab.TabPages["eventSetupTab"];
            }
        }

        private void updateEventSetupButton_Click(object sender, EventArgs e)
        {
            string message = checkEventParameters();
            if (!string.IsNullOrWhiteSpace(message))
            {
                Utilities.showErrorMessage("Fix the following errors in the Event Setup before continuing!" + Environment.NewLine + message);
                return;
            }
            saveTable(infoTableName);
            Utilities.showTabs(mainControlTab);
            mainControlTab.SelectedTab = mainControlTab.TabPages["scoresTab"];
            Utilities.showBalloonNotification("Save Completed Successfully", "Saved Event Information to database.");
        }

        private void reloadEventSetupButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(infoTableName))
            {
                loadTable(infoTableName);
                Utilities.showBalloonNotification("Reload Completed Successfully", "Event Information has been reloaded from database.");
            }
        }

        private void eventSetupDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string[] readOnlyColumns = new string[] {"Parameter_Name"};
            Utilities.setReadOnlyAndVisibleColumns(eventSetupDataGridView, readOnlyColumns, null);
        }

        private void teamsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string[] readOnlyColumns = new string[] { "Team_Number" };
            string[] hideColumns = new string[] { "Score","Boards_Scored","Percentage" };
            Utilities.setReadOnlyAndVisibleColumns(teamsDataGridView, readOnlyColumns, hideColumns);
        }

        private void scoresDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            if (columnName == "NS_Score")
            {
                dgv.Rows[e.RowIndex].Cells["EW_Score"].Value = -1 * (double)dgv.Rows[e.RowIndex].Cells["NS_Score"].Value;
            }
        }

        private void reloadScoresButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(scoresTableName))
            {
                loadTable(scoresTableName);
                Utilities.showBalloonNotification("Reload Completed Successfully", "Scores have been reloaded from database.");
            }
        }

        private void reloadNamesButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(teamsTableName))
            {
                loadTable(teamsTableName);
                Utilities.showBalloonNotification("Reload Completed Successfully", "Team Information has been reloaded from database.");
            }
        }

        private void updateNamesButton_Click(object sender, EventArgs e)
        {
            saveTable(teamsTableName);
        }

        private void writeScoresButton_Click(object sender, EventArgs e)
        {
            saveTable(scoresTableName);
        }

        private int getValue(DataGridViewCellCollection cells, string columnName, ref string message)
        {
            Object value = cells[columnName].Value;
            if (value == DBNull.Value)
            {
                Utilities.appendToMessage(ref message, columnName+" cannot be empty!");
                return 1;
            }
            else
            {
               return (int)value;
            }

        }

        private void scoresDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
           /* DataGridView dgv = sender as DataGridView;
            string message = "";
            DataGridViewCellCollection cells = dgv.Rows[e.RowIndex].Cells;
            int round = getValue(cells, "Round", ref message);
            int board = getValue(cells, "Board", ref message);
            int ns_number = getValue(cells, "NS_Number", ref message);
            int ew_number = getValue(cells, "EW_Number", ref message);
            if (round < 1 || round > m_eventParameters.getNumberOfRounds())
            {
                Utilities.appendToMessage(message, "Invalid Round Number " + round);
            }
            if (board < 1 || board > m_eventParameters.getNumberOfBoards())
            {
                Utilities.appendToMessage(message, "Invalid Board Number " + board);
            }
            // Check if team numbers are valid
            if (ns_number < 1 || ns_number > m_eventParameters.getNumberOfTeams())
            {
                Utilities.appendToMessage(message, "Invalid NS Number " + ns_number);
            }
            if (ew_number < 1 || ew_number > m_eventParameters.getNumberOfTeams())
            {
                Utilities.appendToMessage(message, "Invalid EW Number " + ew_number);
            }
            if (ns_number == ew_number)
            {
                Utilities.appendToMessage(message, "NS Number and EW Number cannot be the same");
            }
            // Check if duplicate
            DataRow[] dRows = ((DataTable)dgv.DataSource).Select("Round = " + round + " AND Board = " + board + " AND NS_Number = " + ns_number + " AND EW_Number = " + ew_number);
            if (dRows.Length > 0) Utilities.appendToMessage(message, "Duplicate entry with same round, board, NS_Number and EW_Number exists!");
            if (!string.IsNullOrWhiteSpace(message))
            {
                Utilities.showErrorMessage("Error in score entry on line " + e.RowIndex + " as noted below. Please fix before continuing." + Environment.NewLine + message);
                e.Cancel = true;
            }*/
        }

    }
}
