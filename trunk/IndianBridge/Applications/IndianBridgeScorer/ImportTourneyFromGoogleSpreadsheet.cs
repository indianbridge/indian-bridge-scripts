using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IndianBridge.GoogleAPIs;
using System.Windows.Forms;
using IndianBridge.Common;
using System.IO;
using System.Diagnostics;

namespace IndianBridgeScorer
{
    public static class ImportTourneyFromGoogleSpreadsheet
    {
        private static DataSet m_dataSet = new DataSet();
        private static string m_tourneyName = "";
        private static string m_databaseFileName = "";

        public static void importTourney()
        {
            DataTable infoTable = new DataTable("Info");
            infoTable.Columns.Add("Parameter_Name", Type.GetType("System.String"));
            infoTable.Columns.Add("Parameter_Value", Type.GetType("System.String"));
            m_dataSet.Tables.Add(infoTable);
            DataTable namesTable = new DataTable("Names");
            namesTable.Columns.Add("Number", Type.GetType("System.Int32"));
            namesTable.Columns.Add("Team Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Member 1 Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Member 2 Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Member 3 Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Member 4 Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Member 5 Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Member 6 Name", Type.GetType("System.String"));
            namesTable.Columns.Add("Penalty", Type.GetType("System.Double"));
            namesTable.Columns.Add("Tie Breaker", Type.GetType("System.Double"));
            namesTable.Columns.Add("Flags", Type.GetType("System.String"));
            m_dataSet.Tables.Add(namesTable);
            DataTable scoresTable = new DataTable("Scores");
            scoresTable.Columns.Add("Round Number", Type.GetType("System.Int32"));
            scoresTable.Columns.Add("Table Number", Type.GetType("System.Int32"));
            scoresTable.Columns.Add("Team 1 Number", Type.GetType("System.Int32"));
            scoresTable.Columns.Add("Team 2 Number", Type.GetType("System.Int32"));
            scoresTable.Columns.Add("Team 1 VPs", Type.GetType("System.Double"));
            scoresTable.Columns.Add("Team 2 VPs", Type.GetType("System.Double"));
            scoresTable.Columns.Add("Team 1 Adjustment", Type.GetType("System.Double"));
            scoresTable.Columns.Add("Team 2 Adjustment", Type.GetType("System.Double"));
            m_dataSet.Tables.Add(scoresTable);
            string username = "indianbridge.dummy@gmail.com";
            string password = "kibitzer";
            string spreadsheetName = Microsoft.VisualBasic.Interaction.InputBox("Provide a Spreadsheet Name", "Spreadsheet Name");
            string[] sheetNames = new string[] { "Info","Names","Scores"};
            if (!string.IsNullOrWhiteSpace(spreadsheetName))
            {
                SpreadSheetAPI ssa = new SpreadSheetAPI(spreadsheetName, username, password);
                foreach(string sheetName in sheetNames) ssa.getValues(sheetName, m_dataSet);
            }

            m_tourneyName = spreadsheetName;
            Constants.CurrentTourneyFolderName = Constants.generateTourneyFolder(m_tourneyName);
            createTourneyDatabases();
            createSwissLeague();
        }

        private static void createSwissLeague()
        {
            string eventName = "Swiss League";
            string rootFolder = Path.Combine(Constants.getCurrentTourneyDatabasesFolder(), Utilities.makeIdentifier_(eventName));
            if (Directory.Exists(rootFolder))
            {
                DialogResult result = MessageBox.Show("An event already exists at " + rootFolder + ". Do you want to overwrite all contents?", "Event Exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
                Directory.Delete(rootFolder, true);
                Directory.Delete(Path.Combine(Constants.getCurrentTourneyWebpagesFolder(), Utilities.makeIdentifier_(eventName)), true);
            }
            if (!addEvent(eventName, Constants.EventType.TeamsSwissLeague)) return;
            SwissTeamEventInfo swissTeamEventInfo = new SwissTeamEventInfo(eventName, Constants.getEventInformationFileName(eventName), true);
            swissTeamEventInfo.NumberOfBoardsPerRound = 8;
            DataTable table = m_dataSet.Tables["Info"];
            DataRow[] dRows = table.Select("Parameter_Name = 'Number of Teams'");
            swissTeamEventInfo.NumberOfTeams = dRows.Length == 1?getIntValue(dRows[0], "Parameter_Value"):0;
            dRows = table.Select("Parameter_Name = 'Number of Rounds'");
            int numberOfRounds = dRows.Length == 1?getIntValue(dRows[0], "Parameter_Value"):0;
            swissTeamEventInfo.NumberOfRounds = numberOfRounds;
            dRows = table.Select("Parameter_Name = 'Number of Qualifiers'");
            swissTeamEventInfo.NumberOfQualifiers = dRows.Length == 1?getIntValue(dRows[0], "Parameter_Value"):0;
            m_databaseFileName = Constants.getEventScoresFileName(eventName);
            createSwissLeagueDatabases(eventName, swissTeamEventInfo.NumberOfTeams, swissTeamEventInfo.NumberOfRounds);
            populateSwissLeagueDatabases();
            SwissTeamScoringProgressParameters stspp = new SwissTeamScoringProgressParameters(eventName, Constants.getEventScoringProgressParametersFileName(eventName), true);
            stspp.DrawsCompleted = numberOfRounds;
            stspp.RoundsCompleted = numberOfRounds;
            stspp.RoundsScored = 0;
        }

        private static void populateSwissLeagueDatabases()
        {
            populateNamesTable();
            populateScoresTable();  
        }

        private static void populateScoresTable()
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventScores);
            DataTable existingTable = m_dataSet.Tables["Scores"];
            foreach (DataRow existingRow in existingTable.Rows)
            {
                DataRow dRow = table.NewRow();
                dRow["Round_Number"] = AccessDatabaseUtilities.getIntValue(existingRow, "Round Number");
                dRow["Table_Number"] = AccessDatabaseUtilities.getIntValue(existingRow, "Table Number");
                dRow["Team_1_Number"] = AccessDatabaseUtilities.getIntValue(existingRow, "Team 1 Number");
                dRow["Team_2_Number"] = AccessDatabaseUtilities.getIntValue(existingRow, "Team 2 Number");
                dRow["Team_1_VPs"] = AccessDatabaseUtilities.getDoubleValue(existingRow, "Team 1 VPs");
                dRow["Team_2_VPs"] = AccessDatabaseUtilities.getDoubleValue(existingRow, "Team 2 VPs");
                dRow["Team_1_VP_Adjustment"] = -1*AccessDatabaseUtilities.getDoubleValue(existingRow, "Team 1 Adjustment");
                dRow["Team_2_VP_Adjustment"] = -1*AccessDatabaseUtilities.getDoubleValue(existingRow, "Team 2 Adjustment");
                table.Rows.Add(dRow);
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.EventScores);
        }

        private static int getIntValue(DataRow dRow, string columnName)
        {
            string value = AccessDatabaseUtilities.getStringValue(dRow, columnName);
            string newValue = value.Replace("\"", "");
            return (string.IsNullOrWhiteSpace(newValue) ? 0 : int.Parse(newValue));
        }

        private static double getDoubleValue(DataRow dRow, string columnName)
        {
            string value = AccessDatabaseUtilities.getStringValue(dRow, columnName);
            string newValue = value.Replace("\"", "");
            return (string.IsNullOrWhiteSpace(newValue) ? 0 : double.Parse(newValue));
        }

        private static string getStringValue(DataRow dRow, string columnName)
        {
            string value = AccessDatabaseUtilities.getStringValue(dRow, columnName);
            return value.Replace("\"", "");
        }

        private static void populateNamesTable()
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventNames);
            DataTable computedScoresTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventComputedScores);
            DataTable existingTable = m_dataSet.Tables["Names"];
            foreach (DataRow existingRow in existingTable.Rows)
            {
                DataRow dRow = table.NewRow();
                dRow["Team_Number"] = AccessDatabaseUtilities.getIntValue(existingRow, "Number");
                dRow["Team_Name"] = getStringValue(existingRow, "Team Name");
                dRow["Member_Names"] = getStringValue(existingRow, "Member 1 Name") + ", "+
                    getStringValue(existingRow, "Member 2 Name") + ", " +
                    getStringValue(existingRow, "Member 3 Name") + ", " +
                    getStringValue(existingRow, "Member 4 Name") + ", " +
                    getStringValue(existingRow, "Member 5 Name") + ", " +
                    getStringValue(existingRow, "Member 6 Name");
                table.Rows.Add(dRow);
                dRow = computedScoresTable.NewRow();
                dRow["Team_Number"] = AccessDatabaseUtilities.getIntValue(existingRow, "Number");
                computedScoresTable.Rows.Add(dRow);
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.EventNames);
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.EventComputedScores);
        }

        private static void createSwissLeagueDatabases(string eventName, int numberOfTeams, int numberOfRounds)
        {
            AccessDatabaseUtilities.createDatabase(m_databaseFileName);
            createSwissLeagueTeamsTable();
            createSwissLeagueScoresTable();
            createSwissLeagueComputedScoresTable(numberOfRounds);
        }


        private static void createSwissLeagueTeamsTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Member_Names", "TEXT", 255));
            fields.Add(new DatabaseField("Carryover", "NUMBER"));
            fields.Add(new DatabaseField("Original_Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Original_Event_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Total_Score", "NUMBER"));
            fields.Add(new DatabaseField("Tiebreaker_Score", "NUMBER"));
            fields.Add(new DatabaseField("Rank", "INTEGER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Team_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventNames, fields, primaryKeys);
        }

        private static void createSwissLeagueScoresTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Round_Number", "INTEGER"));
            fields.Add(new DatabaseField("Table_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_1_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_2_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_1_IMPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_IMPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_1_VPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_VPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_1_VP_Adjustment", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_VP_Adjustment", "NUMBER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Round_Number");
            primaryKeys.Add("Table_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventScores, fields, primaryKeys);
        }

        private static void createSwissLeagueComputedScoresTable(int numberOfRounds)
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            List<string> primaryKeys = new List<string>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            primaryKeys.Add("Team_Number");
            for (int i = 1; i <= numberOfRounds; ++i)
            {
                fields.Add(new DatabaseField("Score_After_Round_" + i, "NUMBER"));
                fields.Add(new DatabaseField("Rank_After_Round_" + i, "NUMBER"));
                fields.Add(new DatabaseField("Tiebreaker_After_Round_" + i, "NUMBER"));
            }
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventComputedScores, fields, primaryKeys);
        }


        private static bool addEvent(string eventName, string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event Name cannot be empty!", "Empty Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            DataTable table = AccessDatabaseUtilities.getDataTable(Constants.getCurrentTourneyEventsFileName(), Constants.TableName.TourneyEvents);
            DataRow dRow = table.Rows.Find(eventName);
            if (dRow != null)
            {
                MessageBox.Show("Another event with same name (" + eventName + ") already exists!" + Environment.NewLine + "Either delete the other event first or provide a different event name!", "Duplicate Event Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            dRow = table.NewRow();
            dRow["Event_Name"] = eventName;
            dRow["Event_Type"] = eventType;
            table.Rows.Add(dRow);
            AccessDatabaseUtilities.saveTableToDatabase(Constants.getCurrentTourneyEventsFileName(), Constants.TableName.TourneyEvents);
            return true;
        }


        private static void createTourneyDatabases()
        {
            TourneyInfo m_tourneyInfo = new TourneyInfo(Constants.getCurrentTourneyInformationFileName(), true);
            m_tourneyInfo.TourneyName = m_tourneyName;
            string databaseFileName = Constants.getCurrentTourneyEventsFileName();
            AccessDatabaseUtilities.createDatabase(databaseFileName);
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Event_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Event_Type", "TEXT", 255));
            List<string> primaryKeyFields = new List<string>();
            primaryKeyFields.Add("Event_Name");
            AccessDatabaseUtilities.createTable(databaseFileName, Constants.TableName.TourneyEvents, fields, primaryKeyFields);
        }
    }
}
