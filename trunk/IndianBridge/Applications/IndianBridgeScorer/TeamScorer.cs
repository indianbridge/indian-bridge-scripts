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
        string m_databaseFileName;
        public static string infoTableName = "Info";
        public static string namesTableName = "Teams";
        public static string scoresTableName = "Scores";
        public static string computedScoresTableName = "ComputedScores";
        public static string VPScaleTableName = "VPScales";
        public static int numberOfRounds = 0;
        public static int numberOfTeams = 0;
        public static int numberOfBoards = 0;
        private string m_googleSiteRoot = "https://sites.google.com/site/srirambridgetest/results";
        private string m_localWebpagesRootDirectory = "";

        public TeamScorer()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.FileName = Path.Combine(Directory.GetCurrentDirectory(), "MyTeams.mdb");
            dialog.Filter = "mdb files (*.mdb)|*.txt|All files (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                InitializeComponent();
                initialize(dialog.FileName, "https://sites.google.com/site/srirambridgetest/results");
            }
            else this.Close();
        }
        public TeamScorer(string databaseFileName,string googleSiteRoot)
        {
            InitializeComponent();
            initialize(databaseFileName, googleSiteRoot);
        }

        private void initialize(string databaseFileName, string googleSiteRoot)
        {
           m_databaseFileName = databaseFileName;
            m_googleSiteRoot = googleSiteRoot;
            m_localWebpagesRootDirectory = Path.Combine(Path.GetDirectoryName(m_databaseFileName), "Webpages");
            initializeDataSet();
            loadDatabase(m_databaseFileName);
            populateComboboxes();
            updateComboboxes();
        }

        private void getEventParameters()
        {
            if (m_ds.Tables[infoTableName].Rows.Count < 1)
            {
                numberOfRounds = 0;
                numberOfTeams = 0;
                numberOfBoards = 0;
                return;
            }
            DataRow dRow = m_ds.Tables[infoTableName].Rows[0];
            numberOfRounds = (int)dRow["Number_Of_Rounds"];
            numberOfTeams = (int)dRow["Number_Of_Teams"];
            Object value = dRow["Number_Of_Boards"];
            numberOfBoards = (value == DBNull.Value) ? 0 : (int)value;
        }

        private void populateCombobox(ComboBox cb, int numberOfRounds)
        {
            cb.Items.Clear();
            if (numberOfRounds > 0)
            {
                for (int i = 1; i <= numberOfRounds; ++i)
                {
                    cb.Items.Add(i);
                }
            }
        }
        private void updateCombobox(ComboBox cb, string columnName)
        {
            updateCombobox(cb, columnName, 0);
        }
 
        private void updateCombobox(ComboBox cb, string columnName,int adjustment)
        {
            DataTable table = m_ds.Tables[infoTableName];
            if (table.Rows.Count > 0)
            {
                DataRow dRow = table.Rows[0];
                if (table.Columns.Contains(columnName))
                {
                    int value = (int)dRow[columnName] + 1 + adjustment;
                    if (value > numberOfRounds) value = numberOfRounds;
                    if (value < 1) value = 1;
                    cb.Text = "" + value;
                }
                else cb.Text = "1";
            }
        }

        private void updateComboboxes()
        {
            updateCombobox(drawUsingRoundCombobox, "Rounds_Completed",-1);
            updateCombobox(generateDrawForCombobox.ComboBox, "Draws_Completed");
            updateCombobox(scoredForRoundCombobox.ComboBox, "Rounds_Completed");
        }

        private void populateComboboxes()
        {     
            DataTable table = m_ds.Tables[infoTableName];
            if (table.Rows.Count > 0)
            {
                populateCombobox(drawUsingRoundCombobox, numberOfRounds);
                populateCombobox(generateDrawForCombobox.ComboBox, numberOfRounds);
                populateCombobox(scoredForRoundCombobox.ComboBox, numberOfRounds);
            }
        }

        private bool isEventSetup()
        {
            DataTable table = m_ds.Tables[infoTableName];
            return table.Rows.Count > 0;
        }

        private void setupEvent()
        {
            InitializeTeamEvent ite = new InitializeTeamEvent(m_ds);
            ite.ShowDialog();
            if (!ite.cancelPressed)
            {
                m_daInfo.Update(m_ds, infoTableName);
                m_daNames.Update(m_ds, namesTableName);
                m_daScores.Update(m_ds, scoresTableName);
                getEventParameters();
                createComputedScoresTable(numberOfRounds, true);
            }
        }

        private void loadDatabase(string databaseFileName)
        {
            if (!File.Exists(m_databaseFileName))
            {
                Utilities.createDatabase(m_databaseFileName);
                createTables();
            }
            loadTablesFromDatabase();
            getEventParameters();
            loadVPScale(m_ds);
            if (!isEventSetup())
            {
                setupEvent();
            }
        }

        private void createTables()
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string query = "CREATE TABLE "+infoTableName+"([Event_Name] TEXT(255) PRIMARY KEY, [Number_Of_Rounds] INTEGER, [Number_Of_Teams] INTEGER,"
            +"[Number_Of_Boards] INTEGER,[Rounds_Completed] INTEGER,[Draws_Completed] INTEGER, [Rounds_Scored] INTEGER)";
            OleDbCommand myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            query = "CREATE TABLE " + namesTableName + "([Team_Number] INTEGER PRIMARY KEY, [Team_Name] TEXT(255), [Member_Names] TEXT(255))";
            myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            query = "CREATE TABLE " + scoresTableName + "([Round_Number] INTEGER, [Table_Number] INTEGER, [Team_1_Number] INTEGER ,"
            +" [Team_2_Number] INTEGER, [Team_1_IMPs] NUMBER, [Team_2_IMPs] NUMBER, [Team_1_VPs] NUMBER, [Team_2_VPs] NUMBER, "
            +"[Team_1_VP_Adjustment] NUMBER, [Team_2_VP_Adjustment] NUMBER,"
            + "CONSTRAINT primarykey PRIMARY KEY(Round_Number,Table_Number))";
            myCommand = new OleDbCommand(query, myAccessConn);
            myCommand.ExecuteNonQuery();
            myAccessConn.Close();
            myAccessConn = null;
            createComputedScoresTable(0,false);
        }

        public static void loadVPScale(DataSet ds)
        {
            string VPScaleDatabaseFileName = Path.Combine(Directory.GetCurrentDirectory(), "Databases", VPScaleTableName+".mdb");
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + VPScaleDatabaseFileName + ";";
            OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string sql = "SELECT * From " + VPScaleTableName+" WHERE VP_Scale=30 AND Number_Of_Boards_Lower<="+numberOfBoards+" AND Number_Of_Boards_Upper>="+numberOfBoards;
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
            m_daNames.Fill(m_ds, namesTableName);
            DataTable table = m_ds.Tables[namesTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Team_Number"] };
        }

        private void fillScoresTable()
        {
            m_daScores.Fill(m_ds, scoresTableName);
            DataTable table = m_ds.Tables[scoresTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Round_Number"], table.Columns["Table_Number"] };
        }

        private void fillComputedScoresTable()
        {
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
            myAccessConn.Close();
            myAccessConn = null;
        }

        private void initializeDataSet() {
            m_ds = new DataSet();
            m_daInfo = null;
            m_cbInfo = null;
            m_daNames = null;
            m_cbNames = null;
            m_daScores = null;
            m_cbScores = null;
        }

        private void changeEventSetup_Click(object sender, EventArgs e)
        {
            setupEvent();
        }

        private void enterNames_Click(object sender, EventArgs e)
        {
            ShowDataGrid sdg = new ShowDataGrid(m_ds.Tables[namesTableName].DefaultView, "Edit Team and Member Names",
                new string[]{"Team_Number"},new string[]{},0,m_ds.Tables[VPScaleTableName]);
            sdg.ShowDialog();
            if (!sdg.cancelPressed) m_daNames.Update(m_ds, namesTableName);
            else
            {
                fillNamesTable();
            }
        }

        private void showScores(bool includeDraw, bool useIMPs)
        {
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
                int selectedRound = int.Parse(scoredForRoundCombobox.ComboBox.Text);
            if (!includeDraw)
            {
                int drawsCompleted = (int)dRow["Draws_Completed"];
                if (selectedRound > drawsCompleted)
                {
                    MessageBox.Show("Scores for round " + selectedRound + " cannot be entered before a draw has been entered.", "Invalid Operation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
                int numberOfRounds = (int)dRow["Number_Of_Rounds"];
                if (selectedRound < 1 || selectedRound > numberOfRounds)
                {
                    MessageBox.Show("Selected Round : " + scoredForRoundCombobox.ComboBox.Text + " is not valid or not within number of rounds (" + numberOfRounds + ")", "Invalid Round Number!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                DataView dView = new DataView(m_ds.Tables[scoresTableName]);
                dView.RowFilter = "Round_Number = " + selectedRound;
                dView.Sort = "Table_Number ASC";
                List<string> readOnlyColumns = new List<string>();
                List<string> hideColumns = new List<string>();
                readOnlyColumns.Add("Round_Number");
                readOnlyColumns.Add("Table_Number");
                if (!includeDraw)
                {
                    readOnlyColumns.Add("Team_1_Number");
                    readOnlyColumns.Add("Team_2_Number");
                }
                if (useIMPs)
                {
                    readOnlyColumns.Add("Team_1_VPs");
                    readOnlyColumns.Add("Team_2_VPs");
                }
                else
                {
                    hideColumns.Add("Team_1_IMPs");
                    hideColumns.Add("Team_2_IMPs");
                }
                hideColumns.Add("Team_1_VP_Adjustment");
                hideColumns.Add("Team_2_VP_Adjustment");
                ShowDataGrid sdg = new ShowDataGrid(dView, "Enter Scores for Round : " + selectedRound,
                    readOnlyColumns.ToArray(), hideColumns.ToArray(),selectedRound,m_ds.Tables[VPScaleTableName]);
                sdg.ShowDialog();
                if (!sdg.cancelPressed)
                {
                    m_daScores.Update(m_ds, scoresTableName);
                    int roundsCompleted = (int)m_ds.Tables[infoTableName].Rows[0]["Rounds_Completed"];
                    DataRow foundRow = m_ds.Tables[infoTableName].Rows[0];
                    if (selectedRound > roundsCompleted)
                    {
                        foundRow["Rounds_Completed"] = selectedRound;
                    }
                    foundRow["Rounds_Scored"] = selectedRound - 1;
                    m_daInfo.Update(m_ds, infoTableName);
                    doScoring();
                    m_daComputedScores.Update(m_ds, computedScoresTableName);
                    updateComboboxes();
                }
                else
                {
                    fillScoresTable();
                }
        }

        private void doScoring()
        {
            int roundsScored = (int)m_ds.Tables[infoTableName].Rows[0]["Rounds_Scored"];
            int roundsCompleted = (int)m_ds.Tables[infoTableName].Rows[0]["Rounds_Completed"];
            for (int i = roundsScored + 1; i <= roundsCompleted; ++i)
            {
                doScoring(i);
                doRanking(i);
            }
            DataRow foundRow = m_ds.Tables[infoTableName].Rows[0];
            foundRow["Rounds_Scored"] = roundsCompleted;
            m_daInfo.Update(m_ds, infoTableName);
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
                double team2Adjustment = getValue(dRow, "Team_1_VP_Adjustment");
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
            DataRow[] foundRows = table.Select("", "Score_After_Round_" + roundNumber+" DESC, Tiebreaker_After_Round_"+roundNumber+" ASC");
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
            int drawRoundNumber = int.Parse(generateDrawForCombobox.Text);
            int numberOfTeams = (int)dRow["Number_Of_Teams"];
            int totalTeams = numberOfTeams + (numberOfTeams % 2);
            bool[] assigned = new bool[totalTeams];
            for (int i = 0; i < totalTeams; ++i) assigned[i] = false;
            int[] teamNumber = new int[totalTeams];
            for (int i = 0; i < totalTeams; ++i) teamNumber[i] = i+1;
            int numberOfMatches = (numberOfTeams / 2) + (numberOfTeams % 2);
            if (createMatch(drawRoundNumber, 1, numberOfMatches, assigned,teamNumber))
            {
                showDraw(drawRoundNumber);
            }
            else
            {
                MessageBox.Show("Unable to generate random draw for round : " + drawRoundNumber+"\nPlease generate by hand and enter directly.", "Random Draw Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fillScoresTable();
            }
        }

        private void showDraw(int drawRoundNumber)
        {
            DataView dView = new DataView(m_ds.Tables[scoresTableName]);
            dView.RowFilter = "Round_Number = " + drawRoundNumber;
            dView.Sort = "Table_Number ASC";
            ShowDataGrid sdg = new ShowDataGrid(dView, "Check/Edit Draw for Round : " + drawRoundNumber,
                new string[] { "Round_Number", "Table_Number" }, 
                new string[] { "Team_1_IMPS","Team_2_IMPs","Team_1_VPs", "Team_2_VPs" , "Team_1_VP_Adjustment", "Team_2_VP_Adjustment"},
                drawRoundNumber,m_ds.Tables[VPScaleTableName]);
            sdg.ShowDialog();
            if (!sdg.cancelPressed)
            {
                m_daScores.Update(m_ds, scoresTableName);
                int drawsCompleted = (int)m_ds.Tables[infoTableName].Rows[0]["Draws_Completed"];
                DataRow foundRow = m_ds.Tables[infoTableName].Rows[0];
                if (drawRoundNumber > drawsCompleted)
                {
                    foundRow["Draws_Completed"] = drawRoundNumber;
                }
                foundRow["Rounds_Scored"] = drawRoundNumber - 1;
                m_daInfo.Update(m_ds, infoTableName);
                doScoring();
                m_daComputedScores.Update(m_ds, computedScoresTableName);
                updateComboboxes();
            }
            else
            {
                fillScoresTable();
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

        private int findOpponent(int drawRoundNumber, int index1, bool[] assigned, int[] teamNumber)
        {
            bool flag = true;
            int index2 = index1;
            int team1 = teamNumber[index1];
            while (flag)
            {
                index2 = findFirstUnassigned(index2+1, assigned);
                Console.WriteLine("index2 = " + index2);
                if (index2 == -1) return -1;
                int team2 = teamNumber[index2];
                DataRow[] dRows = m_ds.Tables[scoresTableName].Select("Round_Number < "+drawRoundNumber+" AND Team_1_Number = "+(team1)+" AND Team_2_Number = "+(team2));
                if (dRows.Length == 0) return index2;
            }
            return -1;
        }

        private bool createMatch(int drawRoundNumber, int matchNumber, int numberOfMatches, bool[] assigned, int[] teamNumber)
        {
            Console.WriteLine("Match : " + matchNumber);
            if (matchNumber > numberOfMatches) return true;
            int index1 = findFirstUnassigned(0,assigned);
            Debug.Assert(index1 != -1, "For Match Number : "+matchNumber+" unable to find unassigned team");
            int team1 = teamNumber[index1];
            assigned[index1] = true;
            int index2 = index1;
            bool[] localAssigned = new bool[assigned.Length];
            Array.Copy(assigned, localAssigned, assigned.Length);
            while (true)
            {
                index2 = findOpponent(drawRoundNumber, index2, assigned,teamNumber);
                if (index2 == -1) return false;
                int team2 = teamNumber[index2];
                assigned[index2] = true;
                DataRow[] dRows = m_ds.Tables[scoresTableName].Select("Round_Number = " + drawRoundNumber + " AND Table_Number = " + matchNumber);
                Debug.Assert(dRows.Length == 1, "Cannot find exactly one row with Round Number : " + drawRoundNumber + " and Table Number : " + matchNumber);
                DataRow dRow = dRows[0];
                dRow["Team_1_Number"] = team1;
                dRow["Team_2_Number"] = team2;
                bool flag  = createMatch(drawRoundNumber, matchNumber + 1, numberOfMatches, assigned,teamNumber);
                if (flag) return true;
                Array.Copy(localAssigned, assigned, assigned.Length);
            }
        }

        private void enterScoresIMPsButton_Click(object sender, EventArgs e)
        {
            showScores(false, true);
        }

        private void enterScoresVPsButton_Click(object sender, EventArgs e)
        {
            showScores(false, false);
        }

        private void enterDrawScoresIMPsButton_Click(object sender, EventArgs e)
        {
            showScores(true, true);
        }

        private void enterDrawScoresVPsButton_Click(object sender, EventArgs e)
        {
            showScores(true, false);
        }

        private void enterDrawButton_Click(object sender, EventArgs e)
        {
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
            int drawRoundNumber = int.Parse(generateDrawForCombobox.Text);
            showDraw(drawRoundNumber);
        }

        private void createDrawBasedOnRoundScores(int roundNumber)
        {
            int drawRoundNumber = int.Parse(generateDrawForCombobox.Text);
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
                for (int count = 0; count < totalTeams; count++) teamNumber[count] = count+1;
            }
            if (createMatch(drawRoundNumber, 1, numberOfMatches, assigned,teamNumber))
            {
                showDraw(drawRoundNumber);
            }
            else
            {
                MessageBox.Show("Unable to generate draw based on scores after round : " + drawRoundNumber + "\nPlease generate by hand and enter directly.", "Round Score Based Draw Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fillScoresTable();
            }

        }

        private void currentDrawButton_Click(object sender, EventArgs e)
        {
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
            int roundsScored = (int)dRow["Rounds_Scored"];
            createDrawBasedOnRoundScores(roundsScored);
        }

        private void roundDrawButton_Click(object sender, EventArgs e)
        {
            DataRow dRow = getInfoDataRow();
            if (dRow == null) return;
            int roundsScored = (int)dRow["Rounds_Scored"];
            int selectedRound = int.Parse(drawUsingRoundCombobox.Text);
            if (selectedRound > roundsScored)
            {
                MessageBox.Show("Draw can only be created based on rounds earlier than currently completed round (" + roundsScored + ")"
                    + Environment.NewLine + "You have selected round " + selectedRound, "Invalid Round Selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            createDrawBasedOnRoundScores(selectedRound);
        }

        private void createLocalWebpagesButton_Click(object sender, EventArgs e)
        {
            CreateAndPublishTeamResults cptr = new CreateAndPublishTeamResults(m_ds, m_localWebpagesRootDirectory, m_googleSiteRoot);
            cptr.ShowDialog();
            cptr.Dispose();
        }
    }
}
