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

namespace IndianBridgeScorer
{

    public partial class PDScorer : Form
    {
        private string m_eventName;
        private string m_databaseFileName;

        private PDEventInfo m_pdEventInfo;

        private PDDatabaseToWebpages m_databaseToWebpages = null;
        private CustomBackgroundWorker m_createWebpagesCBW = null;
        private bool m_createWebpagesRunning = false;
        private double oldFontSize;
        private ResultsPublishParameters m_resultsPublishParameters;
        private SitesAPI m_sitesAPI = null;
        private CustomBackgroundWorker m_publishResultsCBW = null;
        private bool m_publishResultsRunning = false;

        public PDScorer(string eventName)
        {
            m_eventName = eventName;
            InitializeComponent();
            initialize();
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
            if (!m_pdEventInfo.isValid())
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
            setComboboxesValues();
        }


        private void loadDatabases()
        {
            m_pdEventInfo = new PDEventInfo(m_eventName, Constants.getEventInformationFileName(m_eventName), false);
            eventSetupPropertyGrid.SelectedObject = m_pdEventInfo;
            namesDataGridView.DataSource = loadTable(Constants.TableName.EventNames);
            loadTable(Constants.TableName.EventScores);
            m_resultsPublishParameters = new ResultsPublishParameters(m_eventName, Constants.getResultsPublishParametersFileName(m_eventName), true);
            resultsPublishPropertyGrid.SelectedObject = m_resultsPublishParameters;
            if (string.IsNullOrWhiteSpace(m_resultsPublishParameters.ResultsWebsite))
                m_resultsPublishParameters.ResultsWebsite = Constants.getEventResultsWebsite(m_eventName);

        }

        private void createDatabases()
        {
            AccessDatabaseUtilities.createDatabase(m_databaseFileName);
            createTeamsTable();
            createScoresTable();
        }

        private void createScoresTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Table_Number", "INTEGER"));
            fields.Add(new DatabaseField("Round_Number", "INTEGER"));
            fields.Add(new DatabaseField("Match_Number", "INTEGER"));
            fields.Add(new DatabaseField("Board_Number", "INTEGER"));
            fields.Add(new DatabaseField("NS_Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("EW_Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("NS_Score", "NUMBER"));
            fields.Add(new DatabaseField("EW_Score", "NUMBER"));
            fields.Add(new DatabaseField("NS_MPs", "NUMBER"));
            fields.Add(new DatabaseField("EW_MPs", "NUMBER"));
            ArrayList primaryKeys = new ArrayList();
            primaryKeys.Add("Table_Number");
            primaryKeys.Add("Round_Number");
            primaryKeys.Add("Match_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventScores, fields, primaryKeys);
        }

        private void createTeamsTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Member_Names", "TEXT", 255));
            fields.Add(new DatabaseField("Total_Score", "NUMBER"));
            fields.Add(new DatabaseField("Boards_Played", "NUMBER"));
            fields.Add(new DatabaseField("Percentage_Score", "NUMBER"));
            fields.Add(new DatabaseField("Rank", "INTEGER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Team_Number");
            AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.EventNames, fields, primaryKeys);
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
                m_pdEventInfo.load();
                eventSetupPropertyGrid.Refresh();
                Utilities.showBalloonNotification("Reload Completed", "Event setup reloaded from database successfully");
            }
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
                    dRow["Boards_Played"] = 0;
                    dRow["Percentage_Score"] = 0;
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
            Utilities.setReadOnlyAndVisibleColumns(namesDataGridView, null, new string[] { "Total_Score", "Boards_Played", "Percentage_Score", "Rank" });
        }

        private void saveNamesButton_Click(object sender, EventArgs e)
        {
            saveTable(Constants.TableName.EventNames);
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

        private void saveEventSetupButton_Click(object sender, EventArgs e)
        {
            updateEventSetup();
            Utilities.showTabs(mainControlTab);
            mainControlTab.SelectedTab = mainControlTab.TabPages["namesTab"];
        }


        private void updateEventSetup()
        {
            bool wasNonZero = m_pdEventInfo.wasNonZero();
            if (m_pdEventInfo.hasChanged())
            {
                if (wasNonZero)
                {
                    DialogResult result = MessageBox.Show("Changing event setup can result in losing names and scores already entered!" + Environment.NewLine + "Are you sure you want to change the Event Setup?", "Information Loss Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No) return;
                }
            }
            else return;
            int previousNumberOfTeams = m_pdEventInfo.previousNumberOfTeams;
            int previousNumberOfRounds = m_pdEventInfo.previousNumberOfRounds;
            int previousNumberOfBoardsPerRound = m_pdEventInfo.previousNumberOfBoardsPerRound;
            m_pdEventInfo.save();
            updateNamesTable(m_pdEventInfo.NumberOfTeams, previousNumberOfTeams);
            if (wasNonZero) updateScoresTable(previousNumberOfTeams, previousNumberOfRounds, previousNumberOfBoardsPerRound);
            else
            {
                DataTable table = getTable(Constants.TableName.EventScores);
                addToScoresTable(table, 1, 1, 1, m_pdEventInfo.NumberOfTeams, m_pdEventInfo.NumberOfRounds, m_pdEventInfo.NumberOfBoardsPerRound);
                saveTable(Constants.TableName.EventScores);
            }
            populateComboboxes();
            setComboboxesValues();
            Utilities.showBalloonNotification("Save Completed", "Saved Event Setup to Database successfully");
        }

        private void addToScoresTable(DataTable table, int teamsStart, int roundsStart, int boardsPerRoundStart, int teamsEnd, int roundsEnd, int boardEnd)
        {
            for (int i = teamsStart; i <= teamsEnd; ++i)
            {
                for (int j = roundsStart; j <= roundsEnd; ++j)
                {
                    for (int k = boardsPerRoundStart; k <= boardEnd; ++k)
                    {
                        DataRow dRow = table.NewRow();
                        dRow["Table_Number"] = i;
                        dRow["Round_Number"] = j;
                        dRow["Match_Number"] = k;
                        table.Rows.Add(dRow);
                    }
                }
            }
        }

        private void showScores()
        {
            DataView dView = new DataView(getTable(Constants.TableName.EventScores));
            List<string> filters = new List<string>();
            string roundText = showingScoresForRoundCombobox.Text;
            if (roundText != "All" && !string.IsNullOrWhiteSpace(roundText)) filters.Add("Round_Number = " + roundText);
            string boardText = showingScoresForBoardCombobox.Text;
            if (boardText != "All" && !string.IsNullOrWhiteSpace(boardText)) filters.Add("Board_Number = " + boardText);
            string tableText = showingScoresForTableCombobox.Text;
            if (tableText != "All" && !string.IsNullOrWhiteSpace(tableText)) filters.Add("Table_Number = " + tableText);
            int count = 1;
            dView.RowFilter = "";
            foreach (string filter in filters)
            {
                dView.RowFilter += (count > 1 ? " AND " : "") + filter;
                count++;
            }
            dView.Sort = "Table_Number ASC";
            scoresDataGridView.DataSource = dView;
        }

        private void updateScoresTable(int previousNumberOfTeams, int previousNumberOfRounds, int previousNumberOfBoardsPerRound)
        {
            DataTable table = getTable(Constants.TableName.EventScores);
            int newNumberOfTeams = m_pdEventInfo.NumberOfTeams;
            int newNumberOfRounds = m_pdEventInfo.NumberOfRounds;
            int newNumberOfBoardsPerRound = m_pdEventInfo.NumberOfBoardsPerRound;
            if (newNumberOfTeams > previousNumberOfTeams)
            {
                addToScoresTable(table, previousNumberOfTeams + 1, 1, 1, newNumberOfTeams, m_pdEventInfo.NumberOfRounds, m_pdEventInfo.NumberOfBoardsPerRound);
            }
            else if (newNumberOfTeams < previousNumberOfTeams)
            {
                for (int i = newNumberOfTeams + 1; i <= previousNumberOfTeams; ++i)
                {
                    DataRow[] dRows = table.Select("Table_Number = " + i);
                    foreach (DataRow dRow in dRows) dRow.Delete();
                }
            }

            if (newNumberOfRounds > previousNumberOfRounds)
            {
                addToScoresTable(table, 1, previousNumberOfRounds + 1, 1, previousNumberOfTeams, m_pdEventInfo.NumberOfRounds, m_pdEventInfo.NumberOfBoardsPerRound);
            }
            else if (newNumberOfRounds < previousNumberOfRounds)
            {
                for (int i = newNumberOfRounds + 1; i <= previousNumberOfRounds; ++i)
                {
                    DataRow[] dRows = table.Select("Round_Number = " + i);
                    foreach (DataRow dRow in dRows) dRow.Delete();
                }
            }

            if (newNumberOfBoardsPerRound > previousNumberOfBoardsPerRound)
            {
                addToScoresTable(table, 1, 1, previousNumberOfBoardsPerRound + 1, previousNumberOfTeams, previousNumberOfRounds, m_pdEventInfo.NumberOfBoardsPerRound);
            }
            else if (newNumberOfBoardsPerRound < previousNumberOfBoardsPerRound)
            {
                for (int i = newNumberOfBoardsPerRound + 1; i <= previousNumberOfBoardsPerRound; ++i)
                {
                    DataRow[] dRows = table.Select("Match_Number = " + i);
                    foreach (DataRow dRow in dRows) dRow.Delete();
                }
            }
            saveTable(Constants.TableName.EventScores);
        }

        private void setComboboxesValues()
        {
            if (m_pdEventInfo.NumberOfRounds > 0) setComboboxValue(showingScoresForRoundCombobox, "1");
            else setComboboxValue(showingScoresForRoundCombobox);
            setComboboxValue(showingScoresForBoardCombobox);
            setComboboxValue(showingScoresForTableCombobox);
            setComboboxValue(showBoardScoresCombobox, "1");
            setComboboxValue(showRoundScoresCombobox, "1");
            setComboboxValue(showTeamScoresCombobox, "1");
        }

        private void setComboboxValue(ComboBox cb, string value = null)
        {
            if (string.IsNullOrWhiteSpace(value)) cb.Text = "All";
            else cb.Text = value;
        }

        private void populateComboboxes()
        {
            populateCombobox(showingScoresForBoardCombobox, m_pdEventInfo.totalNumberOfBoards);
            populateCombobox(showingScoresForRoundCombobox, m_pdEventInfo.NumberOfRounds);
            populateCombobox(showingScoresForTableCombobox, m_pdEventInfo.NumberOfTeams);
            populateCombobox(showBoardScoresCombobox, m_pdEventInfo.totalNumberOfBoards);
            populateCombobox(showRoundScoresCombobox, m_pdEventInfo.NumberOfRounds);
            populateCombobox(showTeamScoresCombobox, m_pdEventInfo.NumberOfTeams);
        }

        private void populateCombobox(ComboBox cb, int maxValue)
        {
            cb.Items.Clear();
            cb.Items.Add("All");
            if (maxValue > 0)
            {
                for (int i = 1; i <= maxValue; ++i)
                {
                    cb.Items.Add(i);
                }
            }
        }

        private void showingScoresForRoundCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScores();
        }

        private void showingScoresForTableCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScores();
        }

        private void showingScoresForBoardCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showScores();
        }

        private void increaseBoardNumber(ref int boardNumber, int increment)
        {
            boardNumber += increment;
            while (boardNumber > m_pdEventInfo.totalNumberOfBoards) boardNumber = boardNumber - m_pdEventInfo.totalNumberOfBoards;
            while (boardNumber < 1) boardNumber = boardNumber + m_pdEventInfo.totalNumberOfBoards;
        }

        private void increaseTeamNumber(ref int teamNumber, int increment)
        {
            teamNumber += increment;
            while (teamNumber > m_pdEventInfo.NumberOfTeams) teamNumber = teamNumber - m_pdEventInfo.NumberOfTeams;
            while (teamNumber < 1) teamNumber = teamNumber + m_pdEventInfo.NumberOfTeams;
        }

        private void generateMovement()
        {
            int firstBoard = 1;
            bool shouldSkip = (m_pdEventInfo.NumberOfRounds < m_pdEventInfo.NumberOfTeams - 1);
            int skipAfterRound = m_pdEventInfo.NumberOfRounds / 2;
            int roundsSkipped = (m_pdEventInfo.NumberOfTeams - 1) - m_pdEventInfo.NumberOfRounds;
            DataTable table = getTable(Constants.TableName.EventScores);
            int firstBoardAtTable1 = firstBoard;
            int ewNumberAtTable1 = 1;
            for (int i = 1; i <= m_pdEventInfo.NumberOfRounds; ++i)
            {
                int teamToSkip = -2;
                int boardToSkip = -1;
                if (shouldSkip && skipAfterRound + 1 == i)
                {
                    teamToSkip = (shouldSkip ? -1 : 0) + roundsSkipped * -2;
                    boardToSkip = (shouldSkip ? -1 : 0) + roundsSkipped * -1;
                }
                increaseBoardNumber(ref firstBoardAtTable1, boardToSkip);
                increaseTeamNumber(ref ewNumberAtTable1, teamToSkip);
                int boardNumber = firstBoardAtTable1;
                int ewNumber = ewNumberAtTable1;
                for (int j = 1; j <= m_pdEventInfo.NumberOfTeams; ++j)
                {
                    for (int k = 1; k <= m_pdEventInfo.NumberOfBoardsPerRound; ++k)
                    {

                        DataRow[] dRows = table.Select("Round_Number = " + i + " AND Table_Number = " + j + " AND Match_Number = " + k);
                        Debug.Assert(dRows.Length == 1);
                        DataRow dRow = dRows[0];
                        Debug.Assert(dRow != null);
                        dRow["Board_Number"] = boardNumber;
                        dRow["NS_Team_Number"] = j > m_pdEventInfo.NumberOfTeams ? 0 : j;
                        dRow["EW_Team_Number"] = ewNumber > m_pdEventInfo.NumberOfTeams ? 0 : ewNumber;
                        increaseBoardNumber(ref boardNumber, 1);
                    }
                    increaseTeamNumber(ref ewNumber, 1);
                }
            }
        }

        private void generateForwardSkipMovementButton_Click(object sender, EventArgs e)
        {
            generateMovement();
        }

        private void scoresDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            if (columnName == "NS_Score")
            {
                dgv.Rows[e.RowIndex].Cells["EW_Score"].Value = -1*(double)dgv.Rows[e.RowIndex].Cells["NS_Score"].Value;
            }
            else if (columnName == "EW_Score")
            {
                dgv.Rows[e.RowIndex].Cells["NS_Score"].Value = -1*(double)dgv.Rows[e.RowIndex].Cells["EW_Score"].Value;
            }
            if (columnName == "NS_Score" || columnName == "EW_Score")
            {
                updateMPs(e.RowIndex);
            }
        }

        private void updateMPs(int rowNumber)
        {
            Object value = scoresDataGridView.Rows[rowNumber].Cells["NS_Team_Number"].Value;
            int nsTeamNumber = value == DBNull.Value ? 0 : (int)value;
            value = scoresDataGridView.Rows[rowNumber].Cells["EW_Team_Number"].Value;
            int ewTeamNumber = value == DBNull.Value ? 0 : (int)value;
            value = scoresDataGridView.Rows[rowNumber].Cells["NS_Score"].Value;
            double nsScore = value == DBNull.Value ? 0 : (double)value;
            DataTable table = getTable(Constants.TableName.EventScores);
            DataRow[] dRows = table.Select("NS_Team_Number = " + ewTeamNumber + " AND EW_Team_Number = " + nsTeamNumber);
            Debug.Assert(dRows.Length == 1);
            DataRow dRow = dRows[0];
            if (dRow["EW_Score"] == DBNull.Value) return;
            double ewScore = (double)dRow["NS_Score"];
            double difference = nsScore - ewScore;
            double absoluteDifference = Math.Abs(difference);
            double nsMPs = 0;
            double ewMPs = 0;
            if (absoluteDifference <= 10)
            {
                nsMPs = 3;
                ewMPs = 3;
            }
            else if (absoluteDifference <= 50)
            {
                nsMPs = (nsScore > ewScore ? 4 : 2);
                ewMPs = (nsScore > ewScore ? 2 : 4);
            }
            else if (absoluteDifference <= 200)
            {
                nsMPs = (nsScore > ewScore ? 5 : 1);
                ewMPs = (nsScore > ewScore ? 1 : 5);
            }
            else
            {
                nsMPs = (nsScore > ewScore ? 6 : 0);
                ewMPs = (nsScore > ewScore ? 0 : 6);
            }
            scoresDataGridView.Rows[rowNumber].Cells["NS_MPs"].Value = nsMPs;
            scoresDataGridView.Rows[rowNumber].Cells["EW_MPs"].Value = ewMPs;
            dRow["NS_MPs"] = ewMPs;
            dRow["EW_MPs"] = nsMPs;
        }

        private void doScoring()
        {
            DataTable scoresTable = getTable(Constants.TableName.EventScores);
            DataTable teamsTable = getTable(Constants.TableName.EventNames);
            for (int i = 1; i <= m_pdEventInfo.NumberOfTeams; ++i)
            {
                DataRow foundRow = teamsTable.Rows.Find(i);
                double totalMPs = 0;
                int boardsPlayed = 0;
                DataRow[] dRows = scoresTable.Select("NS_Team_Number = " + i);
                foreach (DataRow dRow in dRows)
                {
                    if (dRow["NS_MPs"] != DBNull.Value)
                    {
                        boardsPlayed++;
                        totalMPs += (double)dRow["NS_MPs"];
                    }
                }
                foundRow["Total_Score"] = totalMPs;
                foundRow["Boards_Played"] = boardsPlayed;
                if (boardsPlayed > 0) foundRow["Percentage_Score"] = (totalMPs*100) /  (6*boardsPlayed);
            }
        }

        private void doRanking()
        {
            DataTable teamsTable = getTable(Constants.TableName.EventNames);
            DataRow[] foundRows = teamsTable.Select("", "Percentage_Score DESC");
            int rank = 1;
            double previousValue = 0;
            string rankColumnName = "Rank";
            for (int i = 0;i < foundRows.Length;++i)
            {
                DataRow dRow = foundRows[i];
                double currentValue = getDoubleValue(dRow, "Percentage_Score");
                if (i > 0 && (currentValue != previousValue )) rank = i + 1;
                previousValue = currentValue;
                dRow[rankColumnName] = rank;
            }

        }

        private void saveScoresButton_Click(object sender, EventArgs e)
        {
            doScoring();
            doRanking();
            saveTable(Constants.TableName.EventScores);
            saveTable(Constants.TableName.EventNames);
            Utilities.showBalloonNotification("Save Completed", Constants.TableName.EventScores + " saved to Database successfully");
        }

        private void reloadScoresButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(Constants.TableName.EventScores))
            {
                loadTable(Constants.TableName.EventScores);
                Utilities.showBalloonNotification("Reload Completed", Constants.TableName.EventScores + " reloaded from database successfully");
            }
        }

        private void recalculateScoresButton_Click(object sender, EventArgs e)
        {
            doScoring();
            doRanking();
            saveTable(Constants.TableName.EventScores);
            saveTable(Constants.TableName.EventNames);
            Utilities.showBalloonNotification("Scoring Completed", "Scores and Ranks recomputed");
        }


        private void showUrl(string filename)
        {
            displayWebBrowser.Url = new Uri(filename);
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
            m_databaseToWebpages = new PDDatabaseToWebpages(m_eventName, m_databaseFileName, Constants.getEventWebpagesFolder(m_eventName));
            m_createWebpagesCBW = new CustomBackgroundWorker("Create Local Webpages", m_databaseToWebpages.createWebpagesInBackground, createWebpagesCompleted, createWebpagesStatus, createWebpagesProgressBar, cancelCreateWebpagesButton, createWebpagesStatusTextbox);
            oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            m_createWebpagesRunning = true;
            m_createWebpagesCBW.run();
        }

        private void regenerateWebpageButton_Click(object sender, EventArgs e)
        {
            createLocalWebpages();
            Utilities.showBalloonNotification("Regeneration Done", "Regenerated All Webpages");
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

        private void showBoardScoresButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showBoardScoresCombobox.Text))
            {
                MessageBox.Show("Select a board number first!");
                return;
            }
            int teamNumber = int.Parse(showBoardScoresCombobox.Text);
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "boards", "board" + teamNumber + "score.html"));
        }

        private void showRoundScoresButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(showRoundScoresCombobox.Text))
            {
                MessageBox.Show("Select a round number first!");
                return;
            }
            int teamNumber = int.Parse(showRoundScoresCombobox.Text);
            showUrl(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "rounds", "round" + teamNumber + "score.html"));
        }

        private void publishResultsButton_Click(object sender, EventArgs e)
        {
            publishResults();
        }
        private void publishResultsCompleted(bool success)
        {
            m_publishResultsRunning = false;
            Utilities.fontSize = oldFontSize;
            if (success) publishResultsStatusTextbox.Text = ("Results published succesfully at " + m_resultsPublishParameters.ResultsWebsite);
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
            Utilities.getGoogleSiteComponents(m_resultsPublishParameters.ResultsWebsite, out siteName, out pagePath);
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            m_sitesAPI = new SitesAPI(siteName, username, password, true, false);
            m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", m_sitesAPI.uploadDirectoryInBackground, publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, publishResultsStatusTextbox);
            oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            Tuple<string, string> values = new Tuple<string, string>(Constants.getEventWebpagesFolder(m_eventName), pagePath);
            m_publishResultsRunning = true;
            m_publishResultsCBW.run(values);
        }

    }
}
