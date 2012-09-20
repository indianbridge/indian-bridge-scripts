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
    public partial class TeamScorer : Form
    {
        public DataSet m_ds;
        public OleDbDataAdapter m_daInfo;
        public OleDbCommandBuilder m_cbInfo;
        public OleDbDataAdapter m_daNames;
        public OleDbCommandBuilder m_cbNames;
        public OleDbDataAdapter m_daScores;
        public OleDbCommandBuilder m_cbScores;
        public OleDbDataAdapter m_daComputedScores;
        public OleDbCommandBuilder m_cbComputedScores;
        public OleDbDataAdapter m_daVPScale;
        public OleDbCommandBuilder m_cbVPScale;
        public OleDbDataAdapter m_daKnockoutSessions;
        public OleDbCommandBuilder m_cbKnockoutSessions;
        Dictionary<string, OleDbDataAdapter> m_daKnockout = new Dictionary<string, OleDbDataAdapter>();
        Dictionary<string, OleDbCommandBuilder> m_cbKnockout = new Dictionary<string, OleDbCommandBuilder>();
        string m_databaseFileName;
        public static string infoTableName = "Info";
        public static string namesTableName = "Teams";
        public static string scoresTableName = "Scores";
        public static string computedScoresTableName = "ComputedScores";
        public static string VPScaleTableName = "VPScales";
        //public static string knockoutTableName = "Knockout";
        public static string knockoutSessionsTableName = "KnockoutSessions";
        public static int numberOfRounds = 0;
        public static int numberOfTeams = 0;
        public static int numberOfBoards = 0;
        private int numberOfQualifiers = 0;
        private string m_localWebpagesRootDirectory = "";
        private TourneyInformationDatabase m_tid;
        private string m_eventName = "";
        private string m_resultsWebsite = "";
        private List<TabPage> m_tabPages = null;
        private bool m_tabsHidden = false;
        private string[] sessionsNames = new string[] { "", "Finals", "Semi_Finals", "Quarter_Finals", "Pre_Quarter_Finals" };
        Dictionary<string, int> m_oldKnockoutSessions = new Dictionary<string, int>();
        Dictionary<string, int> m_knockoutSessions = new Dictionary<string, int>();
        Dictionary<string, int> m_numberOfMatches = new Dictionary<string, int>();
        private double m_oldFontSize;

        public TeamScorer(TourneyInformationDatabase tid, string eventName, string databaseFileName)
        {
            InitializeComponent();
            m_eventName = eventName;
            m_tid = tid;
            m_databaseFileName = databaseFileName;
            initialize();
        }

        private void hideTabs()
        {
            foreach (TabPage tp in m_tabPages) mainControlTab.TabPages.Remove(tp);
            m_tabsHidden = true;

        }

        private void showTabs()
        {
            foreach (TabPage tp in m_tabPages) mainControlTab.TabPages.Add(tp);
            m_tabsHidden = false;
            populateComboboxes();
            updateComboboxes();
            scoresEntryFormatCombobox.SelectedIndex = scoresEntryFormatCombobox.FindStringExact("IMPs");
        }

        private void showNames()
        {
            DataView dView = new DataView(m_ds.Tables[namesTableName]);
            dView.RowFilter = "";
            dView.Sort = "Team_Number ASC";
            string[] readOnlyColumns = new string[] { "Team_Number" };
            enterNamesDataGridView.AutoResizeColumns();
            enterNamesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            enterNamesDataGridView.DataSource = dView;
            foreach (string str in readOnlyColumns)
            {
                enterNamesDataGridView.Columns[str].ReadOnly = true;
            }

        }

        private void initialize()
        {
            this.Text = "Team Scorer for " + m_eventName;
            m_resultsWebsite = m_tid.getTourneyResultsWebsite();
            websiteResultsTextbox.Text = m_resultsWebsite;
            m_localWebpagesRootDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(m_databaseFileName)), "Webpages", Utilities.makeIdentifier_(m_eventName));
            initializeDataSet();
            loadDatabase(m_databaseFileName);
            m_tabPages = new List<TabPage>();
            eventSetupDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            showNames();
            foreach (TabPage tp in mainControlTab.TabPages)
            {
                if (tp.Name != "eventSetupTab")
                {
                    m_tabPages.Add(tp);
                }
            }

            if (!isEventSetup())
            {
                if (!m_tabsHidden) hideTabs();
                getEventParameters();
            }
            else
            {
                if (m_tabsHidden) showTabs();
                getEventParameters();
                loadVPScale(m_ds);
                populateComboboxes();
                updateComboboxes();
                scoresEntryFormatCombobox.SelectedIndex = scoresEntryFormatCombobox.FindStringExact("IMPs");
            }
        }

        private bool parseParameterAsInt(DataTable table, string parameterName, out int value)
        {
            DataRow[] dRows = table.Select("Parameter_Name = '" + parameterName + "'");
            if (dRows.Length < 1)
            {
                value = 0;
                return false;
            }
            return int.TryParse((string)dRows[0]["Parameter_Value"], out value);
        }

        private void updateEventParameters()
        {
            DataTable table = m_ds.Tables[TeamScorer.infoTableName];
            DataRow dRow = (table.Rows.Count < 1) ? table.NewRow() : table.Rows[0];
            dRow["Event_Name"] = m_eventName;
            dRow["Number_Of_Rounds"] = numberOfRounds;
            dRow["Number_Of_Teams"] = numberOfTeams;
            dRow["Number_Of_Boards"] = numberOfBoards;
            dRow["Number_Of_Qualifiers"] = numberOfQualifiers;
            dRow["Rounds_Completed"] = 0;
            dRow["Rounds_Scored"] = 0;
            dRow["Draws_Completed"] = 0;
            if (table.Rows.Count < 1) table.Rows.Add(dRow);
            m_daInfo.Update(m_ds, infoTableName);
        }

        private void updateNamesTable(int previousNumberOfTeams, int newNumberOfTeams)
        {
            DataTable table = m_ds.Tables[TeamScorer.namesTableName];
            if (newNumberOfTeams > previousNumberOfTeams)
            {
                for (int i = previousNumberOfTeams + 1; i <= newNumberOfTeams; ++i)
                {
                    DataRow dRow = table.NewRow();
                    dRow["Team_Number"] = i;
                    dRow["Team_Name"] = "Team " + i;
                    table.Rows.Add(dRow);
                }
            }
            else if (newNumberOfTeams < previousNumberOfTeams)
            {
                for (int i = newNumberOfTeams + 1; i <= previousNumberOfTeams; ++i)
                {
                    DataRow dRow = table.Rows.Find(i);
                    dRow.Delete();
                }
            }
            m_daNames.Update(m_ds, namesTableName);
        }

        private void updateScoresTable(int previousNumberOfRounds, int newNumberOfRounds, int previousNumberOfTeams, int newNumberOfTeams)
        {
            DataTable table = m_ds.Tables[TeamScorer.scoresTableName];
            int numberOfMatches = (newNumberOfTeams / 2) + newNumberOfTeams % 2;
            int previousNumberOfMatches = (previousNumberOfTeams / 2) + previousNumberOfTeams % 2;
            if (newNumberOfRounds > previousNumberOfRounds)
            {
                for (int i = previousNumberOfRounds + 1; i <= newNumberOfRounds; ++i)
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
            else if (newNumberOfRounds < previousNumberOfRounds)
            {
                for (int i = newNumberOfRounds + 1; i <= previousNumberOfRounds; ++i)
                {
                    for (int j = 1; j <= previousNumberOfMatches; ++j)
                    {
                        Object[] keys = { i, j };
                        DataRow dRow = table.Rows.Find(keys);
                        dRow.Delete();
                    }
                }
            }
            if (numberOfMatches > previousNumberOfMatches)
            {
                for (int i = 1; i <= Math.Min(newNumberOfRounds, previousNumberOfRounds); ++i)
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
                for (int i = 1; i <= Math.Min(newNumberOfRounds, previousNumberOfRounds); ++i)
                {
                    for (int j = numberOfMatches + 1; j <= previousNumberOfMatches; ++j)
                    {
                        Object[] keys = { i, j };
                        DataRow dRow = table.Rows.Find(keys);
                        dRow.Delete();
                    }
                }
            }
            m_daScores.Update(m_ds, scoresTableName);
        }

        private void updateKnockoutTab()
        {
        }

        private bool validateEventParameters()
        {
            int teams, rounds, boards, qualifiers;
            string message = "";
            DataTable table = (DataTable)eventSetupDataGridView.DataSource;
            bool result = parseParameterAsInt(table, "Number_Of_Teams", out teams);
            if (!result) message += "Number of Teams is not a valid Integer!";
            result = parseParameterAsInt(table, "Number_Of_Rounds", out rounds);
            if (!result) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + "Number of Rounds is not a valid Integer!";
            result = parseParameterAsInt(table, "Number_Of_Boards", out boards);
            if (!result) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + "Number of Boards is not a valid Integer!";
            result = parseParameterAsInt(table, "Number_Of_Qualifiers", out qualifiers);
            if (!result) qualifiers = 0;
            if (teams < 2) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + "Number of Teams has to be atleast 2!";
            if (rounds < 1) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + "Number of Rounds has to be atleast 1!";
            if (boards < 1) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + "Number of Boards has to be atleast 1!";
            if (qualifiers != 0 && !isExponentOfTwo(qualifiers)) message += (string.IsNullOrWhiteSpace(message) ? "" : Environment.NewLine) + "Number of Qualifiers has to be an exponent of 2!";
            if (string.IsNullOrWhiteSpace(message))
            {
                if (teams != numberOfTeams || rounds != numberOfRounds)
                {
                    if (numberOfTeams != 0 || numberOfRounds != 0)
                    {
                        DialogResult dResult = MessageBox.Show("Team Names and Team scores may become inconsistent! Are you sure you want to change event setup?", "Loss of Information Risk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dResult == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
                if (boards != numberOfBoards)
                {
                    numberOfBoards = boards;
                    loadVPScale(m_ds);
                }
                if (numberOfQualifiers != qualifiers)
                {
                    numberOfQualifiers = qualifiers;
                    updateKnockoutTab();
                    createKnockoutSessionsTable(true);
                }
                if (teams != numberOfTeams || rounds != numberOfRounds)
                {
                    updateNamesTable(numberOfTeams, teams);
                    updateScoresTable(numberOfRounds, rounds, numberOfTeams, teams);
                    numberOfRounds = rounds;
                    numberOfTeams = teams;
                    updateEventParameters();
                    createComputedScoresTable(numberOfRounds, true);
                    if (m_tabsHidden) showTabs();
                }
                else
                {
                    updateEventParameters();
                    if (m_tabsHidden) showTabs();
                }
            }
            else
            {
                MessageBox.Show("Following Errors noted in the Event Setup. Fix them before setting up event." + Environment.NewLine + message, "Event Setup Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void getEventParameters()
        {
            if (m_ds.Tables[infoTableName].Rows.Count < 1)
            {
                numberOfRounds = 0;
                numberOfTeams = 0;
                numberOfBoards = 0;
                numberOfQualifiers = 0;
            }
            else
            {
                DataRow dRow = m_ds.Tables[infoTableName].Rows[0];
                numberOfRounds = (int)dRow["Number_Of_Rounds"];
                numberOfTeams = (int)dRow["Number_Of_Teams"];
                Object value = dRow["Number_Of_Boards"];
                numberOfBoards = (value == DBNull.Value) ? 0 : (int)value;
                value = dRow["Number_Of_Qualifiers"];
                numberOfQualifiers = (value == DBNull.Value) ? 0 : (int)value;
            }
            showTeamScoresCombobox.Items.Clear();
            for (int i = 1; i <= numberOfTeams; ++i) showTeamScoresCombobox.Items.Add(i);
            if (numberOfTeams > 0) showTeamScoresCombobox.SelectedIndex = 0;
            showRoundScoresCombobox.Items.Clear();
            for (int i = 1; i <= numberOfRounds; ++i) showRoundScoresCombobox.Items.Add(i);
            if (numberOfRounds > 0) showRoundScoresCombobox.SelectedIndex = 0;
            DataTable table = new DataTable();
            table.Columns.Add("Parameter_Name");
            table.Columns.Add("Parameter_Value");
            table.Columns["Parameter_Name"].ReadOnly = true;
            table.PrimaryKey = new DataColumn[] { table.Columns["Parameter_Name"] };
            table.Rows.Add(new string[] { "Number_Of_Teams", "" + numberOfTeams });
            table.Rows.Add(new string[] { "Number_Of_Rounds", "" + numberOfRounds });
            table.Rows.Add(new string[] { "Number_Of_Boards", "" + numberOfBoards });
            table.Rows.Add(new string[] { "Number_Of_Qualifiers", "" + numberOfQualifiers });
            eventSetupDataGridView.DataSource = table;
            populateComboboxes();
            updateComboboxes();
        }

        private void populateCombobox(ComboBox cb, int rounds)
        {
            cb.Items.Clear();
            if (rounds > 0)
            {
                if (rounds > numberOfRounds) rounds = numberOfRounds;
                for (int i = 1; i <= rounds; ++i)
                {
                    cb.Items.Add(i);
                }
            }
        }
        private void updateCombobox(ComboBox cb, string columnName)
        {
            updateCombobox(cb, columnName, 0);
        }
        private void updateCombobox(ComboBox cb, string columnName, int adjustment)
        {
            updateCombobox(cb, columnName, adjustment, numberOfRounds);
        }

        private void updateCombobox(ComboBox cb, string columnName, int adjustment, int maxValue)
        {
            DataTable table = m_ds.Tables[infoTableName];
            if (table.Rows.Count > 0)
            {
                DataRow dRow = table.Rows[0];
                if (table.Columns.Contains(columnName))
                {
                    int value = (int)dRow[columnName] + 1 + adjustment;
                    if (value > maxValue) value = maxValue;
                    if (value >= 1) cb.Text = "" + value;
                }
            }
        }

        private void updateComboboxes()
        {
            updateCombobox(showingDrawCombobox, "Draws_Completed");
            updateCombobox(drawBasedOnCombobox, "Rounds_Scored", -1);
            if (m_ds.Tables[infoTableName].Rows.Count > 0)
                updateCombobox(showingScoresForRoundCombobox, "Rounds_Completed", 0, getParameterValue("Draws_Completed"));
            else updateCombobox(showingScoresForRoundCombobox, "Rounds_Completed", 0);
        }

        private void populateComboboxes()
        {
            DataTable table = m_ds.Tables[infoTableName];
            if (table.Rows.Count > 0)
            {
                populateCombobox(showingDrawCombobox, getParameterValue("Draws_Completed") + 1);
                populateCombobox(drawBasedOnCombobox, getParameterValue("Rounds_Scored"));
                populateCombobox(showingScoresForRoundCombobox, getParameterValue("Draws_Completed"));
            }
        }

        private void updateVPs(int rowNumber)
        {
            int roundNumber = (int)scoresDataGridView.Rows[rowNumber].Cells["Round_Number"].Value;
            int tableNumber = (int)scoresDataGridView.Rows[rowNumber].Cells["Table_Number"].Value;
            int team1Number = (int)scoresDataGridView.Rows[rowNumber].Cells["Team_1_Number"].Value;
            int team2Number = (int)scoresDataGridView.Rows[rowNumber].Cells["Team_2_Number"].Value;
            Object value = scoresDataGridView.Rows[rowNumber].Cells["Team_1_IMPs"].Value;
            double team1IMPs = value == DBNull.Value ? 0 : (double)value;
            value = scoresDataGridView.Rows[rowNumber].Cells["Team_2_IMPs"].Value;
            double team2IMPs = value == DBNull.Value ? 0 : (double)value;
            double difference = team1IMPs - team2IMPs;
            double absoluteDifference = Math.Abs(difference);
            DataRow[] dRows = m_ds.Tables[VPScaleTableName].Select("Number_Of_IMPs_Lower<=" + absoluteDifference + " AND Number_Of_IMPs_Upper>=" + absoluteDifference);
            Debug.Assert(dRows.Length == 1, "There should be exactly one row in VP Scale for given number of imps");
            int team1VPs = (difference >= 0) ? (int)dRows[0]["Team_1_VPs"] : (int)dRows[0]["Team_2_VPs"];
            int team2VPs = (difference < 0) ? (int)dRows[0]["Team_1_VPs"] : (int)dRows[0]["Team_2_VPs"];
            scoresDataGridView.Rows[rowNumber].Cells["Team_1_VPs"].Value = team1VPs;
            scoresDataGridView.Rows[rowNumber].Cells["Team_2_VPs"].Value = team2VPs;
        }

        private bool calculateComplementaryVPs(int rowNumber, string columnName)
        {
            string otherColumnName = (columnName == "Team_1_VPs") ? "Team_2_VPs" : "Team_1_VPs";
            double vps = (double)scoresDataGridView.Rows[rowNumber].Cells[columnName].Value;
            if (vps < 0 || vps > 25)
            {
                MessageBox.Show(columnName + " is not between 0 and 25", "Not in Range!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Object otherCell = scoresDataGridView.Rows[rowNumber].Cells[otherColumnName].Value;
            double otherValue;
            if (vps == 25 && otherCell != DBNull.Value)
            {
                otherValue = (double)otherCell;
                if (otherValue <= 5) return true;
            }
            otherValue = 30 - vps;
            if (otherValue > 25) otherValue = 25;
            scoresDataGridView.Rows[rowNumber].Cells[otherColumnName].Value = otherValue;
            return true;
        }

        private bool isEventSetup()
        {
            DataTable table = m_ds.Tables[infoTableName];
            return table.Rows.Count > 0;
        }

        private void loadDatabase(string databaseFileName)
        {
            if (!File.Exists(m_databaseFileName))
            {
                Utilities.createDatabase(m_databaseFileName);
                createTables();
            }
            loadTablesFromDatabase();
        }

        private void createTables()
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string query = "CREATE TABLE " + infoTableName + "([Event_Name] TEXT(255) PRIMARY KEY, [Number_Of_Rounds] INTEGER, [Number_Of_Teams] INTEGER,"
            + "[Number_Of_Boards] INTEGER,[Number_Of_Qualifiers] INTEGER,[Rounds_Completed] INTEGER,[Draws_Completed] INTEGER, [Rounds_Scored] INTEGER)";
            OleDbCommand myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            query = "CREATE TABLE " + namesTableName + "([Team_Number] INTEGER PRIMARY KEY, [Team_Name] TEXT(255), [Member_Names] TEXT(255))";
            myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            query = "CREATE TABLE " + scoresTableName + "([Round_Number] INTEGER, [Table_Number] INTEGER, [Team_1_Number] INTEGER ,"
            + " [Team_2_Number] INTEGER, [Team_1_IMPs] NUMBER, [Team_2_IMPs] NUMBER, [Team_1_VPs] NUMBER, [Team_2_VPs] NUMBER, "
            + "[Team_1_VP_Adjustment] NUMBER, [Team_2_VP_Adjustment] NUMBER,"
            + "CONSTRAINT primarykey PRIMARY KEY(Round_Number,Table_Number))";
            myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            myAccessConn.Close();
            myAccessConn = null;
            createComputedScoresTable(0, false);
            createKnockoutSessionsTable(false);
        }

        public static void loadVPScale(DataSet ds)
        {
            string VPScaleDatabaseFileName = Path.Combine(Directory.GetCurrentDirectory(), "Databases", VPScaleTableName + ".mdb");
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + VPScaleDatabaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string sql = "SELECT * From " + VPScaleTableName + " WHERE VP_Scale=30 AND Number_Of_Boards_Lower<=" + numberOfBoards + " AND Number_Of_Boards_Upper>=" + numberOfBoards;
            OleDbDataAdapter da = new OleDbDataAdapter(sql, myAccessConn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            if (ds.Tables[VPScaleTableName] != null)
            {
                ds.Tables[VPScaleTableName].Clear();
            }
            da.Fill(ds, VPScaleTableName);
            DataTable table = ds.Tables[VPScaleTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["VP_Scale"], table.Columns["Number_Of_Boards_Lower"], table.Columns["Number_Of_Boards_Upper"], table.Columns["Number_Of_IMPs_Lower"], table.Columns["Number_Of_IMPs_Upper"] };
            myAccessConn.Close();
            myAccessConn = null;
        }

        private void populateKnockoutCombobox()
        {
            knockoutCombobox.Items.Clear();
            foreach (KeyValuePair<string, int> entry in m_knockoutSessions)
            {
                knockoutCombobox.Items.Add(entry.Key);
            }
        }

        private void getKnockoutSessions()
        {
            m_oldKnockoutSessions = new Dictionary<string, int>(m_knockoutSessions);
            m_knockoutSessions.Clear();
            m_numberOfMatches.Clear();
            foreach (DataRow dRow in m_ds.Tables[knockoutSessionsTableName].Rows)
            {
                int i = (int)dRow["Round_Number"];
                string sessionName = (sessionsNames.Length > i ? sessionsNames[i] : "Round_of_" + Math.Pow(2, i));
                m_knockoutSessions[sessionName] = (int)dRow["Number_Of_Sessions"];
                m_numberOfMatches[sessionName] = (int)Math.Pow(2, i) / 2;
            }
            populateKnockoutCombobox();
            setKnockoutRound();
        }

        private void createKnockoutSessionsTable(bool deleteFirst)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string query;
            OleDbCommand myCommand;
            if (deleteFirst)
            {
                query = "DROP TABLE " + knockoutSessionsTableName;
                myCommand = new OleDbCommand(query, myAccessConn);
                myCommand.ExecuteNonQuery();
            }
            query = "CREATE TABLE " + knockoutSessionsTableName + "([Round_Number] INTEGER PRIMARY KEY, [Round] TEXT(255),[Number_Of_Sessions] INTEGER)";
            myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            myAccessConn.Close();
            myAccessConn = null;
            m_oldKnockoutSessions = new Dictionary<string, int>(m_knockoutSessions);
            m_knockoutSessions.Clear();
            m_numberOfMatches.Clear();
            if (deleteFirst)
            {
                fillKnockoutSessionsTable();
                DataTable table = m_ds.Tables[knockoutSessionsTableName];
                int knockoutRounds = (int)Math.Log(numberOfQualifiers, 2);
                for (int i = 1; i <= knockoutRounds; i++)
                {
                    DataRow dRow = table.NewRow();
                    string sessionName = (sessionsNames.Length > i ? sessionsNames[i] : "Round_of_" + Math.Pow(2, i));
                    dRow["Round_Number"] = i;
                    dRow["Round"] = sessionName;
                    dRow["Number_Of_Sessions"] = 3;
                    m_knockoutSessions[sessionName] = 3;
                    m_numberOfMatches[sessionName] = (int)Math.Pow(2, i) / 2;
                    table.Rows.Add(dRow);
                }
                populateKnockoutCombobox();
                m_daKnockoutSessions.Update(m_ds, knockoutSessionsTableName);
                showSessions();
            }
            createKnockoutTable();
            setKnockoutRound();
        }

        private bool roundCompleted(string tableName)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string sql = "SELECT * From " + tableName;
            OleDbDataAdapter da = new OleDbDataAdapter(sql, myAccessConn);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, tableName);
            DataTable table = ds.Tables[tableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Match_Number"], table.Columns["Team_Number"] };
            myAccessConn.Close();
            myAccessConn = null;
            return !Utilities.HasNull(table);
        }



        private void setKnockoutRound()
        {
            if (m_ds.Tables[knockoutSessionsTableName] != null)
            {
                DataRow[] dRows = m_ds.Tables[knockoutSessionsTableName].Select("", "Round_Number DESC");
                foreach (DataRow dRow in dRows)
                {
                    string roundName = (string)dRow["Round"];
                    if (!roundCompleted(roundName))
                    {
                        knockoutCombobox.Text = roundName;
                        return;
                    }
                }
            }
            if (knockoutCombobox.Items.Count > 0) knockoutCombobox.SelectedIndex = 0;
        }

        private bool isExponentOfTwo(int qualifiers)
        {
            double value = Math.Log(qualifiers, 2);
            return ((value % 1) == 0);
        }


        private void createKnockoutTable()
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            foreach (KeyValuePair<string, int> entry in m_knockoutSessions)
            {
                if (!m_oldKnockoutSessions.ContainsKey(entry.Key))
                {
                    createKnockoutSessionTable(myAccessConn, entry.Key, entry.Value);
                }
                else
                {
                    modifyKnockoutSessionTable(myAccessConn, entry.Key, entry.Value, m_oldKnockoutSessions[entry.Key]);
                }
            }
            foreach (KeyValuePair<string, int> entry in m_oldKnockoutSessions)
            {
                if (!m_knockoutSessions.ContainsKey(entry.Key))
                {
                    dropKnockoutSessionTable(myAccessConn, entry.Key, entry.Value);
                }
            }
            myAccessConn.Close();
            myAccessConn = null;
        }

        private void modifyKnockoutSessionTable(OleDbConnection myAccessConn, string tableName, int numberOfSessions, int oldNumberOfSessions)
        {
            string query = "";
            OleDbCommand myCommand;
            if (oldNumberOfSessions > numberOfSessions)
            {
                for (int i = numberOfSessions + 1; i <= oldNumberOfSessions; ++i)
                {
                    query = "ALTER TABLE " + tableName + " DROP COLUMN Session_" + i + "_Score";
                    myCommand = new OleDbCommand(query, myAccessConn);
                    myCommand.ExecuteNonQuery();
                }
            }
            else if (numberOfSessions > oldNumberOfSessions)
            {
                for (int i = oldNumberOfSessions + 1; i <= numberOfSessions; ++i)
                {
                    query = "ALTER TABLE " + tableName + " ADD COLUMN Session_" + i + "_Score NUMBER";
                    myCommand = new OleDbCommand(query, myAccessConn);
                    myCommand.ExecuteNonQuery();
                }
            }
            fillTable(tableName);
        }

        private void fillTable(string tableName)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string sql = "SELECT * From " + tableName;
            m_daKnockout[tableName] = new System.Data.OleDb.OleDbDataAdapter(sql, myAccessConn);
            m_cbKnockout[tableName] = new OleDbCommandBuilder(m_daKnockout[tableName]);
            m_daKnockout[tableName].Fill(m_ds, tableName);
            DataTable table = m_ds.Tables[tableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Match_Number"], table.Columns["Team_Number"] };
        }

        private void dropKnockoutSessionTable(OleDbConnection myAccessConn, string tableName, int numberOfSessions)
        {
            string query = "DROP TABLE " + tableName;
            OleDbCommand myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            m_daKnockout.Remove(tableName);
            m_cbKnockout.Remove(tableName);
        }

        private void createKnockoutSessionTable(OleDbConnection myAccessConn, string tableName, int numberOfSessions)
        {
            string query = "CREATE TABLE " + tableName + "([Match_Number] INTEGER, [Team_Number] INTEGER, [Team_Name] TEXT(255), [Carryover] NUMBER, [Total_IMPs] NUMBER, CONSTRAINT primarykey PRIMARY KEY(Match_Number,Team_Number))";
            OleDbCommand myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            for (int i = 1; i <= numberOfSessions; ++i)
            {
                query = "ALTER TABLE " + tableName + " ADD COLUMN Session_" + i + "_Score NUMBER";
                myCommand = new OleDbCommand(query, myAccessConn);
                myCommand.ExecuteNonQuery();
            }
            fillTable(tableName);
            DataTable table = m_ds.Tables[tableName];
            int numberOfMatches = m_numberOfMatches[tableName];
            for (int i = 1; i <= numberOfMatches; ++i)
            {
                DataRow dRow = table.NewRow();
                dRow["Match_Number"] = i;
                int teamNumber = i;
                dRow["Team_Number"] = i;
                dRow["Team_Name"] = getTeamName(teamNumber);
                table.Rows.Add(dRow);
                dRow = table.NewRow();
                dRow["Match_Number"] = i;
                teamNumber = 2 * numberOfMatches - (i - 1);
                dRow["Team_Number"] = teamNumber;
                dRow["Team_Name"] = getTeamName(teamNumber);
                table.Rows.Add(dRow);
            }
            m_daKnockout[tableName].Update(m_ds, tableName);
        }

        private void createComputedScoresTable(int numberOfRounds, bool deleteFirst)
        {

            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string query;
            OleDbCommand myCommand;
            if (deleteFirst)
            {
                query = "DROP TABLE " + computedScoresTableName;
                myCommand = new OleDbCommand(query, myAccessConn);
                myCommand.ExecuteNonQuery();
            }
            query = "CREATE TABLE " + computedScoresTableName + "([Team_Number] INTEGER PRIMARY KEY)";
            myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            for (int i = 1; i <= numberOfRounds; ++i)
            {
                query = "ALTER TABLE " + computedScoresTableName + " ADD COLUMN Score_After_Round_" + i + " NUMBER";
                myCommand = new OleDbCommand(query, myAccessConn);
                myCommand.ExecuteNonQuery();
                query = "ALTER TABLE " + computedScoresTableName + " ADD COLUMN Rank_After_Round_" + i + " NUMBER";
                myCommand = new OleDbCommand(query, myAccessConn);
                myCommand.ExecuteNonQuery();
                query = "ALTER TABLE " + computedScoresTableName + " ADD COLUMN Tiebreaker_After_Round_" + i + " NUMBER";
                myCommand = new OleDbCommand(query, myAccessConn);
                myCommand.ExecuteNonQuery();
            }
            myAccessConn.Close();
            myAccessConn = null;
            if (deleteFirst)
            {
                fillComputedScoresTable();
                DataTable table = m_ds.Tables[computedScoresTableName];
                for (int i = 1; i <= numberOfTeams; ++i)
                {
                    DataRow dRow = table.NewRow();
                    dRow["Team_Number"] = i;
                    table.Rows.Add(dRow);
                }
                m_daComputedScores.Update(m_ds, computedScoresTableName);
            }
        }

        private void fillNamesTable()
        {
            if (m_ds.Tables.Contains(namesTableName)) m_ds.Tables.Remove(namesTableName);
            m_daNames.Fill(m_ds, namesTableName);
            DataTable table = m_ds.Tables[namesTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Team_Number"] };
        }

        private void fillScoresTable()
        {
            if (m_ds.Tables.Contains(scoresTableName)) m_ds.Tables.Remove(scoresTableName);
            m_daScores.Fill(m_ds, scoresTableName);
            DataTable table = m_ds.Tables[scoresTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Round_Number"], table.Columns["Table_Number"] };
        }

        private void fillKnockoutSessionsTable()
        {
            if (m_ds.Tables.Contains(knockoutSessionsTableName)) m_ds.Tables.Remove(knockoutSessionsTableName);
            m_daKnockoutSessions.Fill(m_ds, knockoutSessionsTableName);
            DataTable table = m_ds.Tables[knockoutSessionsTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Round_Number"] };
        }

        private void fillKnockoutTable(string knockoutTableName)
        {
            if (m_ds.Tables.Contains(knockoutTableName)) m_ds.Tables.Remove(knockoutTableName);
            m_daKnockout[knockoutTableName].Fill(m_ds, knockoutTableName);
            DataTable table = m_ds.Tables[knockoutTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Round"], table.Columns["Match_Number"] };
        }

        private void fillComputedScoresTable()
        {
            if (m_ds.Tables.Contains(computedScoresTableName)) m_ds.Tables.Remove(computedScoresTableName);
            m_daComputedScores.Fill(m_ds, computedScoresTableName);
            DataTable table = m_ds.Tables[computedScoresTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Team_Number"] };
        }

        public void loadTablesFromDatabase()
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string sql = "SELECT * From " + infoTableName;
            m_daInfo = new System.Data.OleDb.OleDbDataAdapter(sql, myAccessConn);
            m_cbInfo = new OleDbCommandBuilder(m_daInfo);
            m_daInfo.Fill(m_ds, infoTableName);
            DataTable table = m_ds.Tables[infoTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Event_Name"] };
            sql = "SELECT * From " + namesTableName;
            m_daNames = new System.Data.OleDb.OleDbDataAdapter(sql, myAccessConn);
            m_cbNames = new OleDbCommandBuilder(m_daNames);
            fillNamesTable();
            sql = "SELECT * From " + scoresTableName;
            m_daScores = new System.Data.OleDb.OleDbDataAdapter(sql, myAccessConn);
            m_cbScores = new OleDbCommandBuilder(m_daScores);
            fillScoresTable();
            sql = "SELECT * From " + computedScoresTableName;
            m_daComputedScores = new System.Data.OleDb.OleDbDataAdapter(sql, myAccessConn);
            m_cbComputedScores = new OleDbCommandBuilder(m_daComputedScores);
            fillComputedScoresTable();
            sql = "SELECT * From " + knockoutSessionsTableName;
            m_daKnockoutSessions = new System.Data.OleDb.OleDbDataAdapter(sql, myAccessConn);
            m_cbKnockoutSessions = new OleDbCommandBuilder(m_daKnockoutSessions);
            fillKnockoutSessionsTable();
            showSessions();
            loadKnockoutTables();
            getKnockoutSessions();
            m_oldKnockoutSessions = new Dictionary<string, int>(m_knockoutSessions);
            myAccessConn.Close();
            myAccessConn = null;
        }

        private void loadKnockoutTables()
        {
            foreach (DataRow dRow in m_ds.Tables[knockoutSessionsTableName].Select())
            {
                fillTable((string)dRow["Round"]);
            }
        }

        private void initializeDataSet()
        {
            m_ds = new DataSet();
            m_daInfo = null;
            m_cbInfo = null;
            m_daNames = null;
            m_cbNames = null;
            m_daScores = null;
            m_cbScores = null;
        }
        private void showSessions()
        {
            numberOfSessionsDataGridView.Columns.Clear();
            DataView dView = new DataView(m_ds.Tables[knockoutSessionsTableName]);
            dView.RowFilter = "";
            dView.Sort = "Round_Number DESC";
            string[] readOnlyColumns = new string[] { "Round" };
            string[] hideColumns = new string[] { "Round_Number" };
            numberOfSessionsDataGridView.AutoResizeColumns();
            numberOfSessionsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            numberOfSessionsDataGridView.DataSource = dView;
            /*foreach (string str in readOnlyColumns)
            {
                numberOfSessionsDataGridView.Columns[str].ReadOnly = true;
            }
            foreach (string str in hideColumns)
            {
                numberOfSessionsDataGridView.Columns[str].Visible = false;
            }*/

        }

        private void showScores()
        {
            scoresDataGridView.Columns.Clear();
            bool useIMPs = (scoresEntryFormatCombobox.Text == "IMPs");
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
            if (string.IsNullOrWhiteSpace(showingScoresForRoundCombobox.Text)) return;
            int selectedRound = int.Parse(showingScoresForRoundCombobox.Text);
            DataView dView = new DataView(m_ds.Tables[scoresTableName]);
            dView.RowFilter = "Round_Number = " + selectedRound;
            dView.Sort = "Table_Number ASC";
            string[] readOnlyColumns;
            string[] hideColumns;
            if (useIMPs)
            {
                readOnlyColumns = new string[] { "Table_Number", "Team_1_VPs", "Team_2_VPs" };
                hideColumns = new string[] { "Round_Number" };
            }
            else
            {
                readOnlyColumns = new string[] { "Table_Number" };
                hideColumns = new string[] { "Round_Number", "Team_1_IMPS", "Team_2_IMPs" };
            }
            scoresDataGridView.AutoResizeColumns();
            scoresDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            scoresDataGridView.DataSource = dView;
            foreach (string str in readOnlyColumns)
            {
                scoresDataGridView.Columns[str].ReadOnly = true;
            }
            foreach (string str in hideColumns)
            {
                scoresDataGridView.Columns[str].Visible = false;
            }
        }

        private void doScoring(string roundChanged)
        {
            if (string.IsNullOrWhiteSpace(roundChanged)) roundChanged = "1";
            int roundsScored = int.Parse(roundChanged); ;
            int roundsCompleted = getParameterValue("Rounds_Completed"); ;
            for (int i = roundsScored; i <= roundsCompleted; ++i)
            {
                doScoring(i);
                doRanking(i);
            }
            m_daComputedScores.Update(m_ds, computedScoresTableName);
            setParameterValue("Rounds_Scored", roundsCompleted);
        }

        private void doScoring(int roundNumber)
        {
            DataTable table = m_ds.Tables[scoresTableName];
            DataTable computedScoresTable = m_ds.Tables[computedScoresTableName];
            DataRow[] dRows = table.Select("Round_Number = " + roundNumber);
            foreach (DataRow dRow in dRows)
            {
                int team1Number = (int)dRow["Team_1_Number"];
                int team2Number = (int)dRow["Team_2_Number"];
                double team1VPs = getValue(dRow, "Team_1_VPs");
                double team2VPs = getValue(dRow, "Team_2_VPs");
                double team1Adjustment = getValue(dRow, "Team_1_VP_Adjustment");
                double team2Adjustment = getValue(dRow, "Team_2_VP_Adjustment");
                if (team1Number <= numberOfTeams)
                {
                    DataRow dComputedRow = computedScoresTable.Rows.Find(team1Number);
                    Debug.Assert(dComputedRow != null, "Row for team number " + team1Number + " was not found in computed scores table");
                    double previousScore = (roundNumber == 1) ? 0 : getValue(dComputedRow, "Score_After_Round_" + (roundNumber - 1));
                    dComputedRow["Score_After_Round_" + roundNumber] = previousScore + team1VPs + team1Adjustment;
                }
                if (team2Number <= numberOfTeams)
                {
                    DataRow dComputedRow = computedScoresTable.Rows.Find(team2Number);
                    Debug.Assert(dComputedRow != null, "Row for team number " + team2Number + " was not found in computed scores table");
                    double previousScore = (roundNumber == 1) ? 0 : getValue(dComputedRow, "Score_After_Round_" + (roundNumber - 1));
                    dComputedRow["Score_After_Round_" + roundNumber] = previousScore + team2VPs + team2Adjustment;
                }
            }
        }

        private void doRanking(int roundNumber)
        {
            DataTable table = m_ds.Tables[computedScoresTableName];
            DataRow[] foundRows = table.Select("", "Score_After_Round_" + roundNumber + " DESC, Tiebreaker_After_Round_" + roundNumber + " ASC");
            int rank = 1;
            double previousValue = 0;
            double previousTiebreaker = 0;
            string rankColumnName = "Rank_After_Round_" + roundNumber;
            for (int i = 0; i < foundRows.Length; ++i)
            {
                DataRow dRow = foundRows[i];
                double currentValue = getValue(dRow, "Score_After_Round_" + roundNumber);
                double currentTiebreaker = getValue(dRow, "Tiebreaker_After_Round_" + roundNumber);
                if (i > 0 && (currentValue != previousValue || currentTiebreaker != previousTiebreaker)) rank = i + 1;
                previousValue = currentValue;
                previousTiebreaker = currentTiebreaker;
                dRow[rankColumnName] = rank;
            }

        }

        private double getValue(DataRow dRow, string columnName)
        {
            Object value = dRow[columnName];
            return value == DBNull.Value ? 0 : (double)value;
        }

        private DataRow getInfoDataRow()
        {
            DataTable table = m_ds.Tables[infoTableName];
            if (table.Rows.Count > 0)
            {
                return table.Rows[0];
            }
            else
            {
                MessageBox.Show("Cannot perform actions without setting up the event first. Please use Setup Event button to create event first", "Event not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private void randomDrawButton_Click(object sender, EventArgs e)
        {
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
            int drawRoundNumber = int.Parse(showingDrawCombobox.Text);
            int numberOfTeams = (int)dRow["Number_Of_Teams"];
            int totalTeams = numberOfTeams + (numberOfTeams % 2);
            bool[] assigned = new bool[totalTeams];
            for (int i = 0; i < totalTeams; ++i) assigned[i] = false;
            int[] teamNumber = new int[totalTeams];
            for (int i = 0; i < totalTeams; ++i) teamNumber[i] = i + 1;
            int numberOfMatches = (numberOfTeams / 2) + (numberOfTeams % 2);
            if (!createMatch(drawRoundNumber, 1, numberOfMatches, assigned, teamNumber))
            {
                MessageBox.Show("Unable to generate random draw for round : " + drawRoundNumber + Environment.NewLine + "Please generate by hand and enter directly.", "Random Draw Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fillScoresTable();
            }
        }

        private void showDraw()
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(showingDrawCombobox.Text));
            int selectedRound = int.Parse(showingDrawCombobox.Text);
            DataView dView = new DataView(m_ds.Tables[scoresTableName]);
            dView.RowFilter = "Round_Number = " + selectedRound;
            dView.Sort = "Table_Number ASC";
            string[] readOnlyColumns = new string[] { "Table_Number" };
            string[] hideColumns = new string[] { "Round_Number", "Team_1_IMPS", "Team_2_IMPs", "Team_1_VPs", "Team_2_VPs", "Team_1_VP_Adjustment", "Team_2_VP_Adjustment" };
            drawDataGridView.AutoResizeColumns();
            drawDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            drawDataGridView.DataSource = dView;
            foreach (string str in readOnlyColumns)
            {
                drawDataGridView.Columns[str].ReadOnly = true;
            }
            foreach (string str in hideColumns)
            {
                drawDataGridView.Columns[str].Visible = false;
            }
        }

        private bool allAssigned(bool[] assigned)
        {
            for (int i = 0; i < assigned.Length; ++i)
                if (!assigned[i]) return false;
            return true;
        }

        private int findFirstUnassigned(int startIndex, bool[] assigned)
        {
            for (int i = startIndex; i < assigned.Length; ++i)
            {
                if (!assigned[i]) return i;
            }
            return -1;
        }

        private int findOpponent(int drawRoundNumber, int index1, int startIndex, bool[] assigned, int[] teamNumber)
        {
            bool flag = true;
            int index2 = startIndex;
            int team1 = teamNumber[index1];
            while (flag)
            {
                index2 = findFirstUnassigned(index2 + 1, assigned);
                Console.WriteLine("index2 = " + index2);
                if (index2 == -1) return -1;
                int team2 = teamNumber[index2];
                DataRow[] dRows = m_ds.Tables[scoresTableName].Select("Round_Number < " + drawRoundNumber + " AND Team_1_Number = " + (team1) + " AND Team_2_Number = " + (team2));
                if (dRows.Length == 0) return index2;
            }
            return -1;
        }

        private bool createMatch(int drawRoundNumber, int matchNumber, int numberOfMatches, bool[] assigned, int[] teamNumber)
        {
            Console.WriteLine("Match : " + matchNumber);
            if (matchNumber > numberOfMatches) return true;
            int index1 = findFirstUnassigned(0, assigned);
            Debug.Assert(index1 != -1, "For Match Number : " + matchNumber + " unable to find unassigned team");
            int team1 = teamNumber[index1];
            assigned[index1] = true;
            int index2 = index1;
            bool[] localAssigned = new bool[assigned.Length];
            Array.Copy(assigned, localAssigned, assigned.Length);
            while (true)
            {
                index2 = findOpponent(drawRoundNumber, index1, index2, assigned, teamNumber);
                if (index2 == -1) return false;
                int team2 = teamNumber[index2];
                assigned[index2] = true;
                DataRow[] dRows = m_ds.Tables[scoresTableName].Select("Round_Number = " + drawRoundNumber + " AND Table_Number = " + matchNumber);
                Debug.Assert(dRows.Length == 1, "Cannot find exactly one row with Round Number : " + drawRoundNumber + " and Table Number : " + matchNumber);
                DataRow dRow = dRows[0];
                dRow["Team_1_Number"] = team1;
                dRow["Team_2_Number"] = team2;
                bool flag = createMatch(drawRoundNumber, matchNumber + 1, numberOfMatches, assigned, teamNumber);
                if (flag) return true;
                Array.Copy(localAssigned, assigned, assigned.Length);
            }
        }

        private string checkDrawForErrors()
        {
            string message = "";
            if (drawDataGridView.Columns.Contains("Team_1_Number"))
            {
                int totalTeams = TeamScorer.numberOfTeams + (TeamScorer.numberOfTeams % 2);
                DataView dView = ((DataView)drawDataGridView.DataSource);
                int row = 1;
                Object value;
                foreach (DataRowView rowView in dView)
                {
                    DataRow dRow = rowView.Row;
                    value = dRow["Team_1_Number"];
                    if (value == DBNull.Value)
                    {
                        message += Environment.NewLine + "Team 1 Number in row " + row + " is empty";
                    }
                    else
                    {
                        int team1Number = (int)dRow["Team_1_Number"];
                        if (team1Number < 1 - TeamScorer.numberOfTeams % 2 || team1Number > totalTeams) message += Environment.NewLine + "Team 1 Number in row " + row + " is not between " + (1 - TeamScorer.numberOfTeams % 2) + " and " + totalTeams;
                    }
                    value = dRow["Team_2_Number"];
                    if (value == DBNull.Value)
                    {
                        message += Environment.NewLine + "Team 2 Number in row " + row + " is empty";
                    }
                    else
                    {
                        int team2Number = (int)dRow["Team_2_Number"];
                        if (team2Number < 1 - TeamScorer.numberOfTeams % 2 || team2Number > totalTeams) message += Environment.NewLine + "Team 2 Number in row " + row + " is not between " + (1 - TeamScorer.numberOfTeams % 2) + " and " + totalTeams;
                    }
                    row++;
                }
            }
            return message;
        }
        struct Occurences
        {
            public int count;
            public List<int> team1Occurence;
            public List<int> team2Occurence;
        };

        private int doesMatchExist(int team1Number, int team2Number, int totalTeams)
        {
            DataTable table = ((DataView)drawDataGridView.DataSource).Table;
            DataRow[] dRows = table.Select("Round_Number < " + showingDrawCombobox.Text + " AND Team_1_Number = " + team1Number + " AND Team_2_Number = " + team2Number);
            if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            if (team1Number == totalTeams)
            {
                dRows = table.Select("Round_Number < " + showingDrawCombobox.Text + " AND Team_1_Number = 0 AND Team_2_Number = " + team2Number);
                if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            }
            if (team2Number == totalTeams)
            {
                dRows = table.Select("Round_Number < " + showingDrawCombobox.Text + " AND Team_1_Number = " + team1Number + " AND Team_2_Number = 0");
                if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            }
            return -1;
        }

        private string checkDrawForWarnings()
        {
            string message = "";
            if (drawDataGridView.Columns.Contains("Team_1_Number"))
            {
                int totalTeams = TeamScorer.numberOfTeams + (TeamScorer.numberOfTeams % 2);
                Occurences[] occurences = new Occurences[totalTeams];
                for (int i = 0; i < totalTeams; ++i)
                {
                    occurences[i].count = 0;
                    occurences[i].team1Occurence = new List<int>();
                    occurences[i].team2Occurence = new List<int>();
                }
                DataView dView = ((DataView)drawDataGridView.DataSource);
                int row = 1;
                foreach (DataRowView rowView in dView)
                {
                    DataRow dRow = rowView.Row;
                    int team1Number = (int)dRow["Team_1_Number"];
                    if (team1Number == 0) team1Number = totalTeams;
                    int team2Number = (int)dRow["Team_2_Number"];
                    if (team2Number == 0) team2Number = totalTeams;
                    int previousRound = doesMatchExist(team1Number, team2Number, totalTeams);
                    if (previousRound != -1)
                    {
                        message += Environment.NewLine + team1Number + " and " + team2Number + " have already played in round " + previousRound + " and are matched in this round in row " + row;
                    }
                    occurences[team1Number - 1].count++;
                    occurences[team1Number - 1].team1Occurence.Add(row);
                    occurences[team2Number - 1].count++;
                    occurences[team2Number - 1].team2Occurence.Add(row);
                    row++;
                }
                for (int i = 0; i < TeamScorer.numberOfTeams; ++i)
                {
                    if (occurences[i].count == 0)
                    {
                        message += Environment.NewLine + "Team Number " + (i + 1) + " is not included in the draw.";
                    }
                    else if (occurences[i].count > 1)
                    {
                        message += Environment.NewLine + "Team Number " + (i + 1) + " appears more than once in draw (as Team 1 in Rows : ";
                        foreach (int number in occurences[i].team1Occurence)
                        {
                            message += "" + (number) + " ";
                        }
                        message += " as Team 2 in Rows : ";
                        foreach (int number in occurences[i].team2Occurence)
                        {
                            message += "" + (number) + " ";
                        }
                        message += ")";
                    }
                }
                if (totalTeams > TeamScorer.numberOfTeams)
                {
                    int i = totalTeams - 1;
                    if (occurences[i].count == 0)
                    {
                        message += Environment.NewLine + "No team has a bye even though there are an odd number of teams.";
                    }
                    else if (occurences[i].count > 1)
                    {
                        message += Environment.NewLine + "Bye has been specified more than once in draw (as Team 1 in Rows : ";
                        foreach (int number in occurences[i].team1Occurence)
                        {
                            message += "" + (number) + " ";
                        }
                        message += " as Team 2 in Rows : ";
                        foreach (int number in occurences[i].team2Occurence)
                        {
                            message += "" + (number) + " ";
                        }
                        message += ")";
                    }
                }
            }
            return message;
        }


        private void createDrawBasedOnRoundScores(int roundNumber)
        {
            int drawRoundNumber = int.Parse(showingDrawCombobox.Text);
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
            int numberOfTeams = (int)dRow["Number_Of_Teams"];
            int totalTeams = numberOfTeams + (numberOfTeams % 2);
            bool[] assigned = new bool[totalTeams];
            for (int i = 0; i < totalTeams; ++i) assigned[i] = false;
            int numberOfMatches = (numberOfTeams / 2) + (numberOfTeams % 2);
            int[] teamNumber = new int[totalTeams];
            if (roundNumber > 0)
            {
                DataTable table = m_ds.Tables[computedScoresTableName];
                DataRow[] dRows = table.Select("", "Rank_After_Round_" + roundNumber + " ASC");
                int count = 0;
                foreach (DataRow foundRow in dRows)
                {
                    teamNumber[count++] = (int)foundRow["Team_Number"];
                }
            }
            else
            {
                for (int count = 0; count < totalTeams; count++) teamNumber[count] = count + 1;
            }
            if (!createMatch(drawRoundNumber, 1, numberOfMatches, assigned, teamNumber))
            {
                MessageBox.Show("Unable to generate draw based on scores after round : " + drawRoundNumber + Environment.NewLine + "Please generate by hand and enter directly.", "Round Score Based Draw Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fillScoresTable();
            }
        }

        private void roundDrawButton_Click(object sender, EventArgs e)
        {
            int roundsScored = getParameterValue("Rounds_Scored");
            if (string.IsNullOrWhiteSpace(drawBasedOnCombobox.Text))
            {
                MessageBox.Show("No scores have been entered for any rounds. So Draws cannot be created based on scores!", "No Scores Yet!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedRound = int.Parse(drawBasedOnCombobox.Text);
            if (selectedRound > roundsScored)
            {
                MessageBox.Show("Draw can only be created based on rounds earlier than currently completed round (" + roundsScored + ")"
                    + Environment.NewLine + "You have selected round " + selectedRound, "Invalid Round Selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            createDrawBasedOnRoundScores(selectedRound);
        }

        private void setFontSize()
        {
            double fontSize;
            bool result = double.TryParse(fontSizeTextBox.Text, out fontSize);
            if (!result) fontSize = 1;
            m_oldFontSize = Utilities.fontSize;
            Utilities.fontSize = fontSize;
        }

        private void resetFontSize()
        {
            Utilities.fontSize = m_oldFontSize;
        }

        private void createLocalWebpagesButton_Click(object sender, EventArgs e)
        {
            setFontSize();
            string m_resultsWebsite = websiteResultsTextbox.Text;
            if (string.IsNullOrWhiteSpace(m_resultsWebsite))
            {
                MessageBox.Show("No website for publishing the results was provided. So only local webpages will be created.", "No Website Provided!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string m_googleSiteRoot = m_resultsWebsite + "/" + Utilities.makeIdentifier_(m_eventName);
            CreateAndPublishTeamResults cptr = new CreateAndPublishTeamResults(m_ds, m_localWebpagesRootDirectory, string.IsNullOrWhiteSpace(m_resultsWebsite) ? "" : m_googleSiteRoot, m_eventName);
            cptr.ShowDialog();
            cptr.Dispose();
            showUrl(Path.Combine(m_localWebpagesRootDirectory, "index.html"));
            resetFontSize();
        }

        private void updateEventSetupButton_Click(object sender, EventArgs e)
        {
            if (validateEventParameters()) showBalloonNotification("Success", "Updated Event Parameters and all tabs should now be visible");
        }

        private bool confirmReload(string databaseName)
        {
            DialogResult result = MessageBox.Show("Are you sure? Any changes you have made to the " + databaseName + " table above will be lost!", "Confirm reload!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }

        private void reloadEventSetupButton_Click(object sender, EventArgs e)
        {
            if (confirmReload("Event Setup")) getEventParameters();
        }

        private void updateNamesButton_Click(object sender, EventArgs e)
        {
            m_daNames.Update(m_ds, namesTableName);
            showBalloonNotification("Success", "Saved Teams Information");
        }

        private void reloadNamesButton_Click(object sender, EventArgs e)
        {
            if (confirmReload("Names"))
            {
                fillNamesTable();
                showNames();
            }
        }

        private void showingDrawCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showDraw();
        }

        private int getParameterValue(string parameterName)
        {
            DataRow dRow = getInfoDataRow();
            Debug.Assert(dRow != null);
            return (int)dRow[parameterName];
        }

        private void setParameterValue(string parameterName, int value)
        {
            DataRow dRow = getInfoDataRow();
            Debug.Assert(dRow != null);
            if (value >= (int)dRow[parameterName])
            {
                dRow[parameterName] = value;
                m_daInfo.Update(m_ds, infoTableName);
                populateComboboxes();
                updateComboboxes();
            }
        }

        private void writeDrawButton_Click(object sender, EventArgs e)
        {
            string message = checkDrawForErrors();
            if (message != "")
            {
                MessageBox.Show("Following Errors were found in draw. Fix them before saving to database." + Environment.NewLine + message, "Possible Errors in Draw!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            message = checkDrawForWarnings();
            if (message != "")
            {
                DialogResult result = MessageBox.Show("Following Warning were found. Do you still want to accept draw?" + Environment.NewLine + message, "Possible Errors in Draw!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
            }
            m_daScores.Update(m_ds, scoresTableName);
            setParameterValue("Draws_Completed", int.Parse(showingDrawCombobox.Text));
            showBalloonNotification("Success", "Saved Round " + showingDrawCombobox.Text + " Draw");
        }

        private void reloadDrawButton_Click(object sender, EventArgs e)
        {
            if (confirmReload("Draw"))
            {
                fillScoresTable();
                showDraw();
            }
        }

        private void scoresDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            if (columnName == "Team_1_IMPs" || columnName == "Team_2_IMPs")
            {
                updateVPs(e.RowIndex);
            }
            if (columnName == "Team_1_VPs" || columnName == "Team_2_VPs")
            {
                calculateComplementaryVPs(e.RowIndex, columnName);
            }
        }

        private void writeScoresButton_Click(object sender, EventArgs e)
        {
            m_daScores.Update(m_ds, scoresTableName);
            string roundChanged = showingScoresForRoundCombobox.Text;
            setParameterValue("Rounds_Completed", int.Parse(showingScoresForRoundCombobox.Text));
            doScoring(roundChanged);
            showBalloonNotification("Success", "Saved Round " + roundChanged + " scores and re-calculated leaderboard");
        }

        private void scoresEntryFormatCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScores();
        }

        private void showingScoresForRoundCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScores();
        }

        private void reloadScoresButton_Click(object sender, EventArgs e)
        {
            if (confirmReload("Scores"))
            {
                fillScoresTable();
                showScores();
            }
        }

        private string getTeamName(int teamNumber)
        {
            DataRow[] dRows = m_ds.Tables[namesTableName].Select("Team_Number = " + teamNumber);
            return (dRows.Length > 0) ? (string)dRows[0]["Team_Name"] : "Unknown";
        }

        private string getDrawTeam(DataRow dRow, string columnName, int roundNumber)
        {
            int number = (int)dRow[columnName];
            DataRow[] dRows = m_ds.Tables[computedScoresTableName].Select("Team_Number = " + number);
            Debug.Assert(dRows.Length == 1);
            double score = getValue(dRows[0], "Score_After_Round_" + roundNumber);
            dRows = m_ds.Tables[namesTableName].Select("Team_Number = " + number);
            string teamName = (string)dRows[0]["Team_Name"];
            return "" + number + " " + teamName + " (" + score + ")";
        }

        private void printDrawButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showingDrawCombobox.Text))
            {
                MessageBox.Show("No Draw Available to Print!", "No Draw", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int drawRoundNumber = int.Parse(showingDrawCombobox.Text);
            Utilities.fontSize = 1.5;
            string fileName = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(m_localWebpagesRootDirectory)), "draw.html");
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("<html><head><title>Draw for Round " + drawRoundNumber + "</title></head><body>");
            sw.WriteLine("<h1>Draw for Round " + drawRoundNumber + "</h1>");
            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Add("Table");
            tableHeader.Add("Team 1");
            tableHeader.Add("Vs.");
            tableHeader.Add("Team2");
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader, true) + "</tr></thead><tbody>");
            DataRow[] foundRows = m_ds.Tables[scoresTableName].Select("Round_Number = " + drawRoundNumber, "Table_Number ASC");
            int i = 0;
            foreach (DataRow dRow in foundRows)
            {
                ArrayList tableRow = new ArrayList();
                tableRow.Add("" + dRow["Table_Number"]);
                tableRow.Add(getDrawTeam(dRow, "Team_1_Number", drawRoundNumber));
                tableRow.Add("Vs.");
                tableRow.Add(getDrawTeam(dRow, "Team_2_Number", drawRoundNumber));
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i++, true) + "</tr>");
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
            Utilities.fontSize = 0.8;
            printDrawBrowser.Url = new Uri(fileName);
        }

        private void printDrawBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            printDrawBrowser.ShowPrintDialog();
        }

        private void showUrl(string filename)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show(filename + " could not be found!", "File not found!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            displayWebBrowser.Url = new Uri(filename);
        }

        private void showLeaderboardButton_Click(object sender, EventArgs e)
        {
            showUrl(Path.Combine(m_localWebpagesRootDirectory, "leaderboard/index.html"));
        }

        private void showTeamScoresButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showTeamScoresCombobox.Text))
            {
                MessageBox.Show("Select a team number first!");
                return;
            }
            int teamNumber = int.Parse(showTeamScoresCombobox.Text);
            showUrl(Path.Combine(m_localWebpagesRootDirectory, "teams", "team" + teamNumber + "score.html"));
        }

        private void showRoundScoresButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showRoundScoresCombobox.Text))
            {
                MessageBox.Show("Select a round number first!");
                return;
            }
            int roundNumber = int.Parse(showRoundScoresCombobox.Text);
            showUrl(Path.Combine(m_localWebpagesRootDirectory, "rounds", "round" + roundNumber + "score.html"));

        }

        private void reloadSessions_Click(object sender, EventArgs e)
        {
            if (confirmReload("Number Of Sessions"))
            {
                fillKnockoutSessionsTable();
                showSessions();
            }
        }

        private void writeSessionsButton_Click(object sender, EventArgs e)
        {
            m_daKnockoutSessions.Update(m_ds, knockoutSessionsTableName);
            getKnockoutSessions();
            createKnockoutTable();
            showBalloonNotification("Success", "Saved Knockout Sessions Information");
        }

        private void knockoutCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dView = new DataView(m_ds.Tables[knockoutCombobox.Text]);
            dView.RowFilter = "";
            dView.Sort = "Match_Number ASC";
            knockoutDataGridView.AutoResizeColumns();
            knockoutDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            knockoutDataGridView.DataSource = dView;
        }


        private void knockoutDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            string sessionName = knockoutCombobox.Text;
            int numberOfSessions = m_knockoutSessions[sessionName];
            if (columnName.Contains("Session"))
            {
                double total = 0;
                for (int i = 1; i <= numberOfSessions; ++i)
                {
                    Object value = dgv.Rows[e.RowIndex].Cells["Session_" + i + "_Score"].Value;
                    total += (value == DBNull.Value) ? 0 : (double)value;
                }
                dgv.Rows[e.RowIndex].Cells["Total_IMPs"].Value = total;
            }
            else if (columnName == "Team_Number")
            {
                dgv.Rows[e.RowIndex].Cells["Team_Name"].Value = getTeamName((int)dgv.Rows[e.RowIndex].Cells["Team_Number"].Value);
            }
        }



        private void reloadKnockoutButton_Click(object sender, EventArgs e)
        {
            if (confirmReload("Draw"))
            {
                fillTable(knockoutCombobox.Text);
            }
        }

        private void showBalloonNotification(string title, string text)
        {
            notifyMessage.BalloonTipText = text;
            notifyMessage.BalloonTipTitle = title;
            notifyMessage.Icon = SystemIcons.Information;
            notifyMessage.Visible = true;
            notifyMessage.ShowBalloonTip(3);
        }

        private void writeKnockoutButton_Click(object sender, EventArgs e)
        {
            m_daKnockout[knockoutCombobox.Text].Update(m_ds, knockoutCombobox.Text);
            showBalloonNotification("Success", "Saved Knockout Scores");
        }


        private void showKnockoutButton_Click(object sender, EventArgs e)
        {
            showUrl(Path.Combine(m_localWebpagesRootDirectory, "knockout/index.html"));
        }

        private void createAndPublishKnockoutButton_Click_1(object sender, EventArgs e)
        {
            setFontSize();
            string m_resultsWebsite = websiteResultsTextbox.Text;
            if (string.IsNullOrWhiteSpace(m_resultsWebsite))
            {
                MessageBox.Show("No website for publishing the results was provided. So only local webpages will be created.", "No Website Provided!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string m_googleSiteRoot = m_resultsWebsite + "/" + Utilities.makeIdentifier_(m_eventName);
            CreateAndPublishKnockoutResults cptr = new CreateAndPublishKnockoutResults(m_ds, m_localWebpagesRootDirectory, string.IsNullOrWhiteSpace(m_resultsWebsite) ? "" : m_googleSiteRoot, m_eventName);
            cptr.ShowDialog();
            cptr.Dispose();
            showUrl(Path.Combine(m_localWebpagesRootDirectory, "index.html"));
            resetFontSize();
        }

        private void numberOfSessionsDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (numberOfSessionsDataGridView.Columns.Count > 0)
            {
                string[] readOnlyColumns = new string[] { "Round" };
                string[] hideColumns = new string[] { "Round_Number" };
                foreach (string str in readOnlyColumns)
                {
                    numberOfSessionsDataGridView.Columns[str].ReadOnly = true;
                }
                foreach (string str in hideColumns)
                {
                    numberOfSessionsDataGridView.Columns[str].Visible = false;
                }
            }

        }

        private void knockoutDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (knockoutDataGridView.Columns.Count > 0)
            {
                string[] readOnlyColumns = new string[] { "Match_Number" };
                foreach (string str in readOnlyColumns)
                {
                    knockoutDataGridView.Columns[str].ReadOnly = true;
                }
            }
        }

        private void handleCopyPaste(object sender, KeyEventArgs e) {
            DataGridView dgv = sender as DataGridView;
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject d = dgv.GetClipboardContent();
                Clipboard.SetDataObject(d);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int row = dgv.CurrentCell.RowIndex;
                int col = dgv.CurrentCell.ColumnIndex;
                foreach (string line in lines)
                {
                    if (row < dgv.RowCount && line.Length >0)
                    {
                        string[] cells = line.Split('\t');
                        for (int i = 0; i < cells.GetLength(0); ++i)
                        {
                            if (col + i <dgv.ColumnCount) dgv[col + i, row].Value =Convert.ChangeType(cells[i], dgv[col + i, row].ValueType);
                            else break;
                        }
                        row++;
                    }
                    else break;
                }
            }
        }


        private void enterNamesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            handleCopyPaste(sender, e);
        }

        private void drawDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            handleCopyPaste(sender, e);
        }

        private void scoresDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            handleCopyPaste(sender, e);
        }

        private void knockoutDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            handleCopyPaste(sender, e);
        }

        private void eventSetupDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            handleCopyPaste(sender, e);
        }

    }
}
