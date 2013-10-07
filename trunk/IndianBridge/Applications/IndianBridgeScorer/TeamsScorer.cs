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
using System.Diagnostics;
using System.Collections;
using IndianBridge.GoogleAPIs;
using IndianBridge.WordpressAPIs;

namespace IndianBridgeScorer
{

    public partial class TeamsScorer : Form
    {
        private string m_eventName;
        private string m_databaseFileName;

        private double oldFontSize;

        private SwissTeamEventInfo m_swissTeamEventInfo;
        private SwissTeamScoringProgressParameters m_swissTeamScoringProgressParameters;
        private SwissTeamPrintDrawParameters m_swissTeamPrintDrawParameters;
        private SwissTeamScoringParameters m_swissTeamScoringParameters;
        private ResultsPublishParameters m_resultsPublishParameters;

        private SwissTeamsDatabaseToWebpages m_databaseToWebpages = null;
        private CustomBackgroundWorker m_createWebpagesCBW = null;
        private bool m_createWebpagesRunning = false;
        private SitesAPI m_sitesAPI = null;
        private CustomBackgroundWorker m_publishResultsCBW = null;
        private bool m_publishResultsRunning = false;

        private SwissDrawCreation m_drawCreation = null;
        private SwissScoringAndRanking m_swissRankingAndScoring = null;

        public TeamsScorer(string eventName)
        {
            m_eventName = eventName;
            InitializeComponent();
            initialize();
            mainControlTab.TabPages.RemoveByKey("wordpressUpload");
        }

        private void initialize()
        {
            this.Text = NiniUtilities.getStringValue(Constants.getCurrentTourneyInformationFileName(), Constants.TourneyNameFieldName) + " : " + m_eventName;
            m_databaseFileName = Constants.getEventScoresFileName(m_eventName);
            // Save all tabs
            Utilities.saveTabs(mainControlTab);
            Utilities.SetDataGridViewProperties(this);
            if (!File.Exists(m_databaseFileName)) createDatabases();
            loadDatabases();
            if (!m_swissTeamEventInfo.isValid())
            {
                // Show only event Setup Tab
                Utilities.hideTabs(mainControlTab);
                List<string> tabNames = new List<string>();
                tabNames.Add("eventSetupTab");
                Utilities.showTabs(mainControlTab, tabNames);
                mainControlTab.SelectedTab = mainControlTab.TabPages["eventSetupTab"];
            }
            else
            {
                Utilities.showTabs(mainControlTab);
                mainControlTab.SelectedTab = mainControlTab.TabPages["namesTab"];
            }
            populateComboboxes();
            updateComboboxes();
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "leaderboard/index.html"));
            m_drawCreation = new SwissDrawCreation(m_databaseFileName, m_swissTeamEventInfo,m_swissTeamScoringParameters);
            m_swissRankingAndScoring = new SwissScoringAndRanking(m_databaseFileName, m_swissTeamEventInfo, m_swissTeamScoringParameters, m_swissTeamScoringProgressParameters);
        }


        private void updateTeamEventParameters()
        {
            showTeamScoresCombobox.Items.Clear();
            DataTable table = getTable(Constants.TableName.EventNames);
            foreach (DataRow dRow in table.Select("","Team_Number ASC"))
            {
                int teamNumber = getIntValue(dRow, "Team_Number");
                showTeamScoresCombobox.Items.Add(teamNumber);
            }

            //for (int i = 1; i <= m_swissTeamEventInfo.NumberOfTeams; ++i) 
            if (m_swissTeamEventInfo.NumberOfTeams > 0) showTeamScoresCombobox.SelectedIndex = 0;
            showRoundScoresCombobox.Items.Clear();
            for (int i = 1; i <= m_swissTeamEventInfo.NumberOfRounds; ++i) showRoundScoresCombobox.Items.Add(i);
            if (m_swissTeamEventInfo.NumberOfRounds > 0) showRoundScoresCombobox.SelectedIndex = 0;

        }

        private void loadVPScaleTable()
        {
            string vpScaleMaximum = (m_swissTeamEventInfo.VPScale == VPScaleOptions.BFI_30VP_Scale)?"30":"20";

                int numberOfBoards = m_swissTeamEventInfo.NumberOfBoardsPerRound;
                string query = " WHERE VP_Scale="+vpScaleMaximum+" AND Number_Of_Boards_Lower<=" + numberOfBoards + " AND Number_Of_Boards_Upper>=" + numberOfBoards;
                DataTable table = AccessDatabaseUtilities.loadDatabaseToTable(Path.Combine(Directory.GetCurrentDirectory(), "Databases", Constants.TableName.VPScale + ".mdb"), Constants.TableName.VPScale, query);
                DataView dView = new DataView(table);
                dView.RowFilter = "";
                dView.Sort = "Number_Of_IMPs_Lower ASC";
                editVPScaleDataGridView.DataSource = dView;
        }

        private void loadDatabases()
        {
            m_swissTeamEventInfo = new SwissTeamEventInfo(m_eventName, Constants.getEventInformationFileName(m_eventName), false);
            eventSetupPropertyGrid.SelectedObject = m_swissTeamEventInfo;
            m_swissTeamScoringProgressParameters = new SwissTeamScoringProgressParameters(m_eventName, Constants.getEventScoringProgressParametersFileName(m_eventName), true);
            m_swissTeamPrintDrawParameters = new SwissTeamPrintDrawParameters(m_eventName, Constants.getSwissTeamPrintDrawParametersFileName(m_eventName), true);
            printDrawPropertyGrid.SelectedObject = m_swissTeamPrintDrawParameters;
            m_swissTeamScoringParameters = new SwissTeamScoringParameters(m_eventName, Constants.getSwissTeamScoringParametersFileName(m_eventName), true);
            scoringParametersPropertyGrid.SelectedObject = m_swissTeamScoringParameters;
            m_resultsPublishParameters = new ResultsPublishParameters(m_eventName, Constants.getResultsPublishParametersFileName(m_eventName), true);
            resultsPublishPropertyGrid.SelectedObject = m_resultsPublishParameters;
            if (string.IsNullOrWhiteSpace(m_resultsPublishParameters.ResultsWebsite))
                m_resultsPublishParameters.ResultsWebsite = Constants.getEventResultsWebsite(m_eventName);
            if (!AccessDatabaseUtilities.hasColumn(m_databaseFileName, Constants.TableName.EventNames, "Withdraw_Round"))
            {
                addWithDrawField();
            }
            namesDataGridView.DataSource = loadTable(Constants.TableName.EventNames);
            loadTable(Constants.TableName.EventScores);
            loadTable(Constants.TableName.EventComputedScores);
            loadVPScaleTable();
            updateTeamEventParameters();
            m_swissTeamPrintDrawParameters.DrawForRound = m_swissTeamScoringProgressParameters.DrawsCompleted;
            printDrawPropertyGrid.Refresh();
            generateDrawHtml();
        }

        private void addWithDrawField()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Withdraw_Round", "INTEGER"));
            AccessDatabaseUtilities.addColumn(m_databaseFileName, Constants.TableName.EventNames, fields);
        }

        private void createDatabases()
        {
            AccessDatabaseUtilities.createDatabase(m_databaseFileName);
            createTeamsTable();
            createScoresTable();
            createComputedScoresTable();
        }

        private void createTeamsTable()
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
            fields.Add(new DatabaseField("Withdraw_Round", "INTEGER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Team_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventNames, fields, primaryKeys);
        }

        private void createScoresTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Round_Number", "INTEGER"));
            fields.Add(new DatabaseField("Table_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_1_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_2_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_1_IMPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_IMPs", "NUMBER"));
            fields.Add(new DatabaseField("Team_1_VPs", "DECIMAL"));
            fields.Add(new DatabaseField("Team_2_VPs", "DECIMAL"));
            fields.Add(new DatabaseField("Team_1_VP_Adjustment", "NUMBER"));
            fields.Add(new DatabaseField("Team_2_VP_Adjustment", "NUMBER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Table_Number");
            primaryKeys.Add("Round_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventScores, fields, primaryKeys);
        }

        private void createComputedScoresTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            List<string> primaryKeys = new List<string>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            primaryKeys.Add("Team_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventComputedScores, fields, primaryKeys);
        }

        private void resetScoring(int newNumberOfRounds)
        {
            m_swissTeamScoringProgressParameters.reset(newNumberOfRounds);
            m_swissTeamScoringProgressParameters.save();
            populateComboboxes();
            updateComboboxes();
        }

        private DataTable loadTable(string tableName, string query = "")
        {
            return AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, tableName, query);
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

        private void reloadEventSetupButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload("Event Setup"))
            {
                m_swissTeamEventInfo.load();
                eventSetupPropertyGrid.Refresh();
                Utilities.showBalloonNotification("Reload Completed", "Event setup reloaded from database successfully");
            }
        }

        private void saveEventSetupButton_Click(object sender, EventArgs e)
        {
            updateEventSetup();
            Utilities.showTabs(mainControlTab);
            mainControlTab.SelectedTab = mainControlTab.TabPages["namesTab"];
        }

        private void updateEventSetup()
        {
            int currentTeams = m_swissTeamEventInfo.NumberOfTeams;
            int currentRounds = m_swissTeamEventInfo.NumberOfRounds;
            int previousTeams = m_swissTeamEventInfo.previousNumberOfTeams;
            int previousRounds = m_swissTeamEventInfo.previousNumberOfRounds;
            if ((previousTeams != 0 && currentTeams != previousTeams))
            {
                DialogResult result = MessageBox.Show("Changing " + Constants.NumberOfTeamsFieldName + " can result in losing names and scores already entered!" + Environment.NewLine + "Are you sure you want to change the Event Setup?", "Information Loss Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
                resetScoring(0);
            }
            else if ((previousRounds != 0 && currentRounds < previousRounds))
            {
                DialogResult result = MessageBox.Show("Reducing " + Constants.NumberOfRoundsFieldName + " can result in losing scores already entered for removed rounds!" + Environment.NewLine + "Are you sure you want to change the Event Setup?", "Information Loss Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
                resetScoring(currentRounds);
            } 
            
            m_swissTeamEventInfo.save();
            //m_drawCreation.setSwissTeamEventInfo(m_swissTeamEventInfo);
            updateNamesTable(currentTeams, previousTeams);
            updateScoresTable(previousRounds, currentRounds, previousTeams, currentTeams);
            updateComputedScoresTable(previousRounds, currentRounds, currentTeams);
            updateTeamEventParameters();
            populateComboboxes();
            updateComboboxes();
            loadVPScaleTable();
            Utilities.showBalloonNotification("Save Completed", "Saved Event Setup to Database successfully");
        }

        private void updateComputedScoresTable(int previousNumberOfRounds, int newNumberOfRounds, int newNumberOfTeams)
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            if (newNumberOfRounds > previousNumberOfRounds)
            {
                for (int i = previousNumberOfRounds + 1; i <= newNumberOfRounds; ++i)
                {
                    fields.Add(new DatabaseField("Score_After_Round_" + i, "NUMBER"));
                    fields.Add(new DatabaseField("Rank_After_Round_" + i, "INTEGER"));
                    fields.Add(new DatabaseField("Tiebreaker_After_Round_" + i, "NUMBER"));
                }

                AccessDatabaseUtilities.addColumn(m_databaseFileName, Constants.TableName.EventComputedScores, fields);
            }
            else if (newNumberOfRounds < previousNumberOfRounds)
            {
                for (int i = newNumberOfRounds + 1; i <= previousNumberOfRounds; ++i)
                {
                    fields.Add(new DatabaseField("Score_After_Round_" + i, "NUMBER"));
                    fields.Add(new DatabaseField("Rank_After_Round_" + i, "INTEGER"));
                    fields.Add(new DatabaseField("Tiebreaker_After_Round_" + i, "NUMBER"));
                }

                AccessDatabaseUtilities.dropColumn(m_databaseFileName, Constants.TableName.EventComputedScores, fields);
            }
            resetComputedScoresTable();
        }

        private void resetComputedScoresTable()
        {
            DataTable table = getTable(Constants.TableName.EventComputedScores);
            foreach (DataRow dRow in table.Rows) dRow.Delete();
            saveTable(Constants.TableName.EventComputedScores);
            DataTable teamTable = getTable(Constants.TableName.EventNames);
            foreach (DataRow dTeamRow in teamTable.Rows)
            {
                int teamNumber = getIntValue(dTeamRow, "Team_Number");
                DataRow dRow = table.NewRow();
                dRow["Team_Number"] = teamNumber;
                table.Rows.Add(dRow);
            }
            saveTable(Constants.TableName.EventComputedScores);
            m_swissRankingAndScoring.recalculateScoresAndRanks(1);
            saveTable(Constants.TableName.EventComputedScores);
        }

        private void updateScoresTable(int previousNumberOfRounds, int newNumberOfRounds, int previousNumberOfTeams, int newNumberOfTeams)
        {
            DataTable table = getTable(Constants.TableName.EventScores);
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
            saveTable(Constants.TableName.EventScores);
        }

        private void updateNamesTable(int newNumberOfTeams, int previousNumberOfTeams)
        {
            DataTable table = getTable(Constants.TableName.EventNames);
            if (newNumberOfTeams > previousNumberOfTeams)
            {
                for (int i = previousNumberOfTeams + 1; i <= newNumberOfTeams; ++i)
                {
                    DataRow dRow = table.NewRow();
                    dRow["Team_Number"] = i;
                    dRow["Team_Name"] = "Team " + i;
                    dRow["Total_Score"] = 0;
                    dRow["Tiebreaker_Score"] = 0;
                    dRow["Rank"] = 1;
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
            saveTable(Constants.TableName.EventNames);
        }

        private void namesDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Utilities.setReadOnlyAndVisibleColumns(namesDataGridView, null, new string[] { "Total_Score", "Tiebreaker_Score", "Rank" });
        }

        private void saveNamesButton_Click(object sender, EventArgs e)
        {
            saveTable(Constants.TableName.EventNames);
            resetComputedScoresTable();
            Utilities.showBalloonNotification("Save Completed", Constants.TableName.EventNames + " saved to Database successfully");
        }

        private void reloadNamesButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(Constants.TableName.EventNames))
            {
                loadTable(Constants.TableName.EventNames);
                Utilities.showBalloonNotification("Reload Completed", Constants.TableName.EventNames + " reloaded from database successfully");
            }
        }


        private void showDraw()
        {
            int selectedRound = int.Parse(showingDrawCombobox.Text);
            int numberOfTeams = LocalUtilities.teamsLeft(m_databaseFileName,selectedRound);
            int numberOfMatches = (numberOfTeams / 2) + (numberOfTeams % 2);
            DataView dView = new DataView(getTable(Constants.TableName.EventScores));
            dView.RowFilter = "Round_Number = " + selectedRound+" AND Table_Number <= "+numberOfMatches;
            dView.Sort = "Table_Number ASC";
            drawDataGridView.DataSource = dView;
        }

        private void updateCombobox(ComboBox cb, int oldValue)
        {
            updateCombobox(cb, oldValue, 0);
        }

        private void updateCombobox(ComboBox cb, int oldValue, int adjustment)
        {
            updateCombobox(cb, oldValue, adjustment, m_swissTeamEventInfo.NumberOfRounds);
        }

        private void updateCombobox(ComboBox cb, int oldValue, int adjustment, int maxValue)
        {
            int value = oldValue + 1 + adjustment;
            if (value > maxValue) value = maxValue;
            if (value >= 1) cb.Text = "" + value;
        }

        private void updateComboboxes()
        {
            updateCombobox(showingDrawCombobox, m_swissTeamScoringProgressParameters.DrawsCompleted);
            updateCombobox(drawBasedOnCombobox, m_swissTeamScoringProgressParameters.RoundsScored, -1);
            int drawsCompleted = m_swissTeamScoringProgressParameters.DrawsCompleted;
            if (drawsCompleted > 0) updateCombobox(showingScoresForRoundCombobox, m_swissTeamScoringProgressParameters.RoundsCompleted, 0, drawsCompleted);
        }

        private void populateComboboxes()
        {
            int drawsCompleted = m_swissTeamScoringProgressParameters.DrawsCompleted;
            populateCombobox(showingDrawCombobox, drawsCompleted + 1);
            int roundsScored = m_swissTeamScoringProgressParameters.RoundsScored;
            if (roundsScored > 0) populateCombobox(drawBasedOnCombobox, roundsScored);
            if (drawsCompleted > 0) populateCombobox(showingScoresForRoundCombobox, drawsCompleted);
        }
        private void populateCombobox(ComboBox cb, int rounds)
        {
            cb.Items.Clear();
            if (rounds > 0)
            {
                if (rounds > m_swissTeamEventInfo.NumberOfRounds) rounds = m_swissTeamEventInfo.NumberOfRounds;
                for (int i = 1; i <= rounds; ++i)
                {
                    cb.Items.Add(i);
                }
            }
        }

        private void showingDrawCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showDraw();
        }

        private void drawDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string[] readOnlyColumns = new string[] { "Table_Number" };
            string[] hideColumns = new string[] { "Round_Number", "Team_1_IMPS", "Team_2_IMPs", "Team_1_VPs", "Team_2_VPs", "Team_1_VP_Adjustment", "Team_2_VP_Adjustment" };
            Utilities.setReadOnlyAndVisibleColumns(drawDataGridView, readOnlyColumns, hideColumns);
        }

        private int getTeamNumber(int rowNumber)
        {
            DataTable table = getTable(Constants.TableName.EventNames);
            return (int)table.Rows[rowNumber - 1]["Team_Number"];
        }

        private void randomDrawButton_Click(object sender, EventArgs e)
        {
            int drawRoundNumber = int.Parse(showingDrawCombobox.Text);
            m_drawCreation.createRandomDraw(drawRoundNumber);
        }

        private void roundDrawButton_Click(object sender, EventArgs e)
        {
            int roundsScored = m_swissTeamScoringProgressParameters.RoundsScored;
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

        private void createDrawBasedOnRoundScores(int roundNumber)
        {
            int drawRoundNumber = int.Parse(showingDrawCombobox.Text);
            m_drawCreation.createDrawBasedOnScore(drawRoundNumber, roundNumber);
        }

        private void saveDrawButton_Click(object sender, EventArgs e)
        {
            int drawRoundNumber = int.Parse(showingDrawCombobox.Text);
            string message = m_drawCreation.checkDrawForErrors(drawRoundNumber);
            if (message != "")
            {
                MessageBox.Show("Following Errors were found in draw. Fix them before saving to database." + Environment.NewLine + message, "Possible Errors in Draw!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            message = m_drawCreation.checkDrawForWarnings(drawRoundNumber);
            if (message != "")
            {
                DialogResult result = MessageBox.Show("Following Warning were found. Do you still want to accept draw?" + Environment.NewLine + message, "Possible Errors in Draw!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;
            }
            saveTable(Constants.TableName.EventScores);
            Utilities.showBalloonNotification("Save Completed", "Saved Round " + showingDrawCombobox.Text + " Draw to Database");
            int value = int.Parse(showingDrawCombobox.Text);
            m_swissTeamScoringProgressParameters.DrawsCompleted = value;
            populateComboboxes();
            updateComboboxes();
            mainControlTab.SelectedTab = mainControlTab.TabPages["printDrawTab"]; 
            m_swissTeamPrintDrawParameters.DrawForRound = value;
            printDrawPropertyGrid.Refresh();
            generateDrawHtml();
        }

        private void reloadDrawButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload("Draw"))
            {
                loadTable(Constants.TableName.EventScores);
                Utilities.showBalloonNotification("Reload Completed", "Reloaded Round " + showingDrawCombobox.Text + " Draw from Database");
            }
        }

        private void generateDrawHtml()
        {
            bool vpsInSeparateColumn = m_swissTeamPrintDrawParameters.VPsInSeparateColumn;
            int drawForRound = m_swissTeamPrintDrawParameters.DrawForRound;
            string fileName = Constants.getSwissTeamPrintDrawFileName(m_eventName);
            StreamWriter sw = new StreamWriter(fileName);
            if (drawForRound < 1 || drawForRound > m_swissTeamScoringProgressParameters.DrawsCompleted)
            {
                sw.WriteLine("<html><head></head><body><h1>No Draw Available</h1></body></html>");
                sw.Close();
                printDrawBrowser.Url = new Uri(fileName);
                return;
            }
            double oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_swissTeamPrintDrawParameters.FontSize;
            double oldPaddingSize = Utilities.paddingSize;
            Utilities.paddingSize = m_swissTeamPrintDrawParameters.PaddingSize;
            bool oldUseBorder = Utilities.useBorder;
            Utilities.useBorder = m_swissTeamPrintDrawParameters.UseBorder;
            sw.WriteLine("<html><head><title>Draw for Round " + drawForRound + "</title>");
            sw.WriteLine(Utilities.makeHtmlPrintStyle_());
            sw.WriteLine("</head><body>");
            sw.WriteLine("<h1>Draw for Round " + drawForRound + "</h1>");
            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Add("Table");
            tableHeader.Add("Team 1");
            if (vpsInSeparateColumn) tableHeader.Add("VPs");
            tableHeader.Add("Vs.");
            if (vpsInSeparateColumn) tableHeader.Add("VPs");
            tableHeader.Add("Team2");
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader, true) + "</tr></thead><tbody>");
            int numberOfTeams = LocalUtilities.teamsLeft(m_databaseFileName, drawForRound);
            int numberOfMatches = (numberOfTeams / 2) + (numberOfTeams % 2);
            DataRow[] foundRows = getTable(Constants.TableName.EventScores).Select("Round_Number = " + drawForRound + " AND Table_Number <= "+numberOfMatches, "Table_Number ASC");
            int i = 0;
            foreach (DataRow dRow in foundRows)
            {
                ArrayList tableRow = new ArrayList();
                tableRow.Add("" + dRow["Table_Number"]);
                if (vpsInSeparateColumn)
                {
                    tableRow.Add(getDrawTeam(dRow, "Team_1_Number"));
                    tableRow.Add(getScore((int)dRow["Team_1_Number"]));
                }
                else tableRow.Add(getDrawTeamAndScore(dRow, "Team_1_Number"));
                tableRow.Add("Vs.");
                if (vpsInSeparateColumn)
                {
                    tableRow.Add(getScore((int)dRow["Team_2_Number"]));
                    tableRow.Add(getDrawTeam(dRow, "Team_2_Number"));
                }
                else tableRow.Add(getDrawTeamAndScore(dRow, "Team_2_Number"));
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i++, true) + "</tr>");
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
            Utilities.fontSize = oldFontSize;
            Utilities.paddingSize = oldPaddingSize;
            Utilities.useBorder = oldUseBorder;
            printDrawBrowser.Url = new Uri(fileName);

        }

        private string getScore(int teamNumber)
        {
            int numberOfTeams = m_swissTeamEventInfo.NumberOfTeams;
            if (teamNumber < 1) return "BYE";
            int scoreRound = (string.IsNullOrWhiteSpace(drawBasedOnCombobox.Text)) ? 0 : int.Parse(drawBasedOnCombobox.Text);
            DataRow[] dRows = getTable(Constants.TableName.EventComputedScores).Select("Team_Number = " + teamNumber);
            Debug.Assert(dRows.Length == 1);
            double score = (scoreRound < 1) ? 0 : getDoubleValue(dRows[0], "Score_After_Round_" + scoreRound);
            return "" + score;
        }

        private string getDrawTeam(DataRow dRow, string columnName)
        {
            int numberOfTeams = m_swissTeamEventInfo.NumberOfTeams;
            int teamNumber = (int)dRow[columnName];
            if (teamNumber < 1) return "BYE";
            DataRow[] dRows = getTable(Constants.TableName.EventNames).Select("Team_Number = " + teamNumber);
            string teamName = (string)dRows[0]["Team_Name"];
            return "" + teamNumber + " " + teamName;
        }

        private string getDrawTeamAndScore(DataRow dRow, string columnName)
        {
            int numberOfTeams = m_swissTeamEventInfo.NumberOfTeams;
            int number = (int)dRow[columnName];
            if (number < 1) return "BYE";
            DataRow[] dRows = getTable(Constants.TableName.EventComputedScores).Select("Team_Number = " + number);
            Debug.Assert(dRows.Length == 1);
            int scoreRound = (string.IsNullOrWhiteSpace(drawBasedOnCombobox.Text)) ? 0 : int.Parse(drawBasedOnCombobox.Text);
            double score = (scoreRound < 1) ? 0 : getDoubleValue(dRows[0], "Score_After_Round_" + scoreRound);
            dRows = getTable(Constants.TableName.EventNames).Select("Team_Number = " + number);
            string teamName = (string)dRows[0]["Team_Name"];
            return "" + number + " " + teamName + " (" + score + ")";
        }

        private void printDrawButton_Click(object sender, EventArgs e)
        {
            printDrawBrowser.ShowPrintDialog();
            mainControlTab.SelectedTab = mainControlTab.TabPages["scoresTab"];
        }

        private void showScores()
        {
            if (string.IsNullOrWhiteSpace(showingScoresForRoundCombobox.Text)) return;
            int selectedRound = int.Parse(showingScoresForRoundCombobox.Text);
            int numberOfTeams = LocalUtilities.teamsLeft(m_databaseFileName, selectedRound);
            int numberOfMatches = (numberOfTeams / 2) + (numberOfTeams % 2);
            DataView dView = new DataView(getTable(Constants.TableName.EventScores));
            dView.RowFilter = "Round_Number = " + selectedRound + " AND Table_Number <= " + numberOfMatches;
            dView.Sort = "Table_Number ASC";
            scoresDataGridView.DataSource = dView;
            //scoresDataGridView.Columns["Team_1_VPs"].DefaultCellStyle.Format = "N2";
            //scoresDataGridView.Columns["Team_2_VPs"].DefaultCellStyle.Format = "N2";
        }

        private void scoresDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            bool useIMPs = (m_swissTeamScoringParameters.ScoringType == ScoringTypeValues.IMP);
            string[] readOnlyColumns = (useIMPs ? new string[] { "Table_Number", "Team_1_VPs", "Team_2_VPs" } : new string[] { "Table_Number" });
            string[] hideColumns = (useIMPs ? new string[] { "Round_Number" } : new string[] { "Round_Number", "Team_1_IMPS", "Team_2_IMPs" });
            Utilities.setReadOnlyAndVisibleColumns(scoresDataGridView, readOnlyColumns, hideColumns);
        }

        private void showingScoresForRoundCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScores();
        }

        private void scoresDataGridView_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
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
            DataTable table = AccessDatabaseUtilities.getDataTable(Path.Combine(Directory.GetCurrentDirectory(), "Databases", Constants.TableName.VPScale + ".mdb"), Constants.TableName.VPScale);
            DataRow[] dRows = table.Select("Number_Of_IMPs_Lower<=" + absoluteDifference + " AND Number_Of_IMPs_Upper>=" + absoluteDifference);
            Debug.Assert(dRows.Length == 1, "There should be exactly one row in VP Scale for given number of imps");
            double team1VPs = (difference >= 0) ? (double)dRows[0]["Team_1_VPs"] : (double)dRows[0]["Team_2_VPs"];
            double team2VPs = (difference < 0) ? (double)dRows[0]["Team_1_VPs"] : (double)dRows[0]["Team_2_VPs"];
            scoresDataGridView.Rows[rowNumber].Cells["Team_1_VPs"].Value = team1VPs;
            scoresDataGridView.Rows[rowNumber].Cells["Team_2_VPs"].Value = team2VPs;
        }

        private bool calculateComplementaryVPs(int rowNumber, string columnName)
        {
            string otherColumnName = (columnName == "Team_1_VPs") ? "Team_2_VPs" : "Team_1_VPs";
            decimal vps = (decimal)scoresDataGridView.Rows[rowNumber].Cells[columnName].Value;
            decimal maxVPs = (m_swissTeamEventInfo.VPScale == VPScaleOptions.BFI_30VP_Scale) ? 25 : 20;
            if (vps < 0 || vps > maxVPs)
            {
                MessageBox.Show(columnName + " is not between 0 and "+maxVPs, "Not in Range!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Object otherCell = scoresDataGridView.Rows[rowNumber].Cells[otherColumnName].Value;
            decimal otherValue;
            if (m_swissTeamEventInfo.VPScale == VPScaleOptions.BFI_30VP_Scale)
            {
                if (vps == 25 && otherCell != DBNull.Value)
                {
                    otherValue = (decimal)otherCell;
                    if (otherValue <= 5) return true;
                }
                otherValue = 30 - vps;
                if (otherValue > 25) otherValue = 25;
            }
            else
            {
                otherValue = 20 - vps;
            }
            scoresDataGridView.Rows[rowNumber].Cells[otherColumnName].Value = otherValue;
            return true;
        }

        private void editVPScaleDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string[] readOnlyColumns = new string[] { };
            string[] hideColumns = new string[] { "VP_Scale", "Number_Of_Boards_Lower", "Number_Of_Boards_Upper" };
            Utilities.setReadOnlyAndVisibleColumns(editVPScaleDataGridView, readOnlyColumns, hideColumns);
        }

        private void reloadVPScaleButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(Constants.TableName.VPScale))
            {
                loadVPScaleTable();
                Utilities.showBalloonNotification("Reload Done", "Reloaded " + Constants.TableName.VPScale + " from database successfully");
            }
        }

        private void saveVPScaleButton_Click(object sender, EventArgs e)
        {
            AccessDatabaseUtilities.saveTableToDatabase(Path.Combine(Directory.GetCurrentDirectory(), "Databases", Constants.TableName.VPScale + ".mdb"), Constants.TableName.VPScale);
            Utilities.showBalloonNotification("Save Done", "Saved " + Constants.TableName.VPScale + " to database successfully");
        }

        private void saveScoresButton_Click(object sender, EventArgs e)
        {
            saveTable(Constants.TableName.EventScores);
            string roundCompletedString = showingScoresForRoundCombobox.Text;
            if (string.IsNullOrWhiteSpace(roundCompletedString)) return;
            int roundCompleted = int.Parse(showingScoresForRoundCombobox.Text);
            m_swissTeamScoringProgressParameters.RoundsCompleted = roundCompleted;
            doScoring(roundCompletedString);
            Utilities.showBalloonNotification("Saved Scores", "Saved Round " + roundCompleted + " scores and re-calculated leaderboard");
            createLocalWebpages();
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "leaderboard/index.html"));
            mainControlTab.SelectedTab = mainControlTab.TabPages["viewResultsTab"];
        }

        private void publishResultsCompleted(bool success)
        {
            m_publishResultsRunning = false;
            Utilities.fontSize = oldFontSize;
            if (success) publishResultsStatusTextbox.Text = ("Results published succesfully at " + m_resultsPublishParameters.ResultsWebsite);
        }

        private void publishResultsToWordpress()
        {
            if (m_publishResultsRunning)
            {
                Utilities.showErrorMessage("A Publish Results operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            string siteName = wpSiteNameTextbox.Text;
            string pagePath = wpPagePathTextbox.Text;
            string username = "indianbridge";
            string password = "kibitzer";
            UploadWebpages uw = new UploadWebpages(siteName, username, password, true);
            m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground, publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, publishResultsStatusTextbox);
            oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            Tuple<string, string> values = new Tuple<string, string>(Constants.getEventWebpagesFolder(m_eventName), pagePath);
            m_publishResultsRunning = true;
            m_publishResultsCBW.run(values);
        }

        private void publishResults()
        {
            if (m_publishResultsRunning)
            {
                Utilities.showErrorMessage("A Publish Results operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            if (string.IsNullOrWhiteSpace(m_resultsPublishParameters.ResultsWebsite))
            {
                MessageBox.Show("Please provide a results website to publish to.");
                return;
            }
            string siteName, pagePath;
            if (Utilities.getGoogleSiteComponents(m_resultsPublishParameters.ResultsWebsite, out siteName, out pagePath))
            {
                String username = "indianbridge.dummy@gmail.com";
                String password = "kibitzer";
                m_sitesAPI = new SitesAPI(siteName, username, password, true, false);
                m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", m_sitesAPI.uploadDirectoryInBackground, publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, publishResultsStatusTextbox);
            }
            else
            {
                Utilities.getWordpressSiteComponents(m_resultsPublishParameters.ResultsWebsite, out siteName, out pagePath);
                string username = "indianbridge";
                string password = "kibitzer";
                UploadWebpages uw = new UploadWebpages(siteName, username, password, true);
                m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground, publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, publishResultsStatusTextbox);
            }
            oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            Tuple<string, string> values = new Tuple<string, string>(Constants.getEventWebpagesFolder(m_eventName), pagePath);
            m_publishResultsRunning = true;
            m_publishResultsCBW.run(values);
        }


        private void createWebpagesCompleted(bool success)
        {
            m_createWebpagesRunning = false;
            Utilities.fontSize = oldFontSize;
            if (success) showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "leaderboard/index.html"));
        }

        private void createLocalWebpages()
        {
            if (m_createWebpagesRunning)
            {
                Utilities.showErrorMessage("A Create Webpages operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            m_databaseToWebpages = new SwissTeamsDatabaseToWebpages(m_eventName, m_databaseFileName, Constants.getEventWebpagesFolder(m_eventName));
            m_createWebpagesCBW = new CustomBackgroundWorker("Create Local Webpages", m_databaseToWebpages.createWebpagesInBackground, createWebpagesCompleted, createWebpagesStatus, createWebpagesProgressBar, cancelCreateWebpagesButton, createWebpagesStatusTextbox);
            oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            m_createWebpagesRunning = true;
            m_createWebpagesCBW.run();
        }

        private void showUrl(string filename)
        {
            displayWebBrowser.Url = new Uri(filename);
        }

        private void copyTotalAndRank(int roundNumber)
        {
            DataTable table = getTable(Constants.TableName.EventNames);
            DataTable computedScoresTable = getTable(Constants.TableName.EventComputedScores);
            foreach (DataRow dRow in table.Rows)
            {
                int teamNumber = getIntValue(dRow, "Team_Number");
                DataRow foundRow = computedScoresTable.Rows.Find(teamNumber);
                Debug.Assert(foundRow != null);
                dRow["Total_Score"] = getDoubleValue(foundRow, "Score_After_Round_" + roundNumber);
                dRow["Tiebreaker_Score"] = getDoubleValue(foundRow, "Tiebreaker_After_Round_" + roundNumber);
                dRow["Rank"] = getIntValue(foundRow, "Rank_After_Round_" + roundNumber);
            }
            saveTable(Constants.TableName.EventNames);
        }

        private void doScoring(string roundChanged)
        {
            int roundsScored = int.Parse(roundChanged);
            m_swissRankingAndScoring.recalculateScoresAndRanks(roundsScored);
            populateComboboxes();
            updateComboboxes();
        }

        private void showLeaderboardButton_Click(object sender, EventArgs e)
        {
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "leaderboard/index.html"));
        }

        private void showTeamScoresButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showTeamScoresCombobox.Text))
            {
                MessageBox.Show("Select a team number first!");
                return;
            }
            int teamNumber = int.Parse(showTeamScoresCombobox.Text);
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "teams", "team" + teamNumber + "score.html"));
        }

        private void showRoundScoresButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showRoundScoresCombobox.Text))
            {
                MessageBox.Show("Select a round number first!");
                return;
            }
            int roundNumber = int.Parse(showRoundScoresCombobox.Text);
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "rounds", "round" + roundNumber + "score.html"));
        }

        private void showingScoresForRoundCombobox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            showScores();
        }

        private void reloadScoresButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(Constants.TableName.EventScores))
            {
                loadTable(Constants.TableName.EventScores);
                Utilities.showBalloonNotification("Reload Done", "Reloaded " + Constants.TableName.EventScores + " from database successfully");
            }
        }


        private void printDrawPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            generateDrawHtml();
        }

        private void scoringParametersPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.PropertyDescriptor.Name == "ScoringType") showScores();
        }

        private void regenerateWebpageButton_Click(object sender, EventArgs e)
        {
            createLocalWebpages();
            Utilities.showBalloonNotification("Regeneration Done", "Regenerated All Webpages");
        }

        private void recalculateScoresButton_Click(object sender, EventArgs e)
        {
            doScoring("1");
            Utilities.showBalloonNotification("Recalculation Done", "Re-calculated All Round Scores and Leaderboard");
            createLocalWebpages();
            Utilities.showBalloonNotification("Regeneration Done", "Regenerated All Webpages");
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "leaderboard/index.html"));
        }

        private void publishResultsButton_Click(object sender, EventArgs e)
        {
            publishResults();
        }

        private void withDrawTeamsButton_Click(object sender, EventArgs e)
        {
            WithdrawTeams wt = new WithdrawTeams(m_databaseFileName);
            wt.ShowDialog();
            showDraw();
            showScores();
        }

        private void publishToWordpressButton_Click(object sender, EventArgs e)
        {
            publishResultsToWordpress();
        }


    }
}
