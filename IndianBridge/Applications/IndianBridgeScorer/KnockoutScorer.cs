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

    public partial class KnockoutScorer : Form
    {
        private string m_eventName;
        private string m_niniFileName = "";
        private string m_databaseFileName = "";
        private KnockoutSessions m_knockoutSessions = null;
        private ResultsPublishParameters m_resultsPublishParameters;

        public KnockoutScorer(string eventName)
        {
            m_eventName = eventName;
            m_niniFileName = Constants.getKnockoutEventInfoFileName(m_eventName);
            m_databaseFileName = Constants.getKnockoutEventScoresFileName(m_eventName);
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            this.Text = NiniUtilities.getStringValue(Constants.getCurrentTourneyInformationFileName(),Constants.TourneyNameFieldName) + " : " + m_eventName;
            Utilities.SetDataGridViewProperties(this);
            m_knockoutSessions = new KnockoutSessions(eventSetupPropertyGrid, m_eventName, m_niniFileName, m_databaseFileName, true);
            knockoutSessionsDataGridView.DataSource = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            knockoutNamesDataGridView.DataSource = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutTeams);
            populateKnockoutRounds();
            m_resultsPublishParameters = new ResultsPublishParameters(m_eventName, Constants.getResultsPublishParametersFileName(m_eventName), true);
            resultsPublishPropertyGrid.SelectedObject = m_resultsPublishParameters;
            if (string.IsNullOrWhiteSpace(m_resultsPublishParameters.ResultsWebsite))
                m_resultsPublishParameters.ResultsWebsite = Constants.getEventResultsWebsite(m_eventName);

        }

        private void populateKnockoutRounds()
        {
            int numberOfRounds = m_knockoutSessions.NumberOfRounds;
            knockoutRoundsCombobox.Items.Clear();
            for (int i = 1; i <= numberOfRounds; ++i)
            {
                string sessionName = (Constants.KnockoutSessionNames.Length >= i ? Constants.KnockoutSessionNames[i - 1] : "Round_of_" + Convert.ToInt32(Math.Pow(2, i)));
                    knockoutRoundsCombobox.Items.Add(sessionName);
            }
            if (knockoutRoundsCombobox.Items.Count > 0)
            {
                selectCurrentKnockoutRound();
            }
        }

        private void selectCurrentKnockoutRound()
        {
            for (int i = m_knockoutSessions.NumberOfRounds; i >= 1; --i)
            {
                List<string> skipColumnNames = new List<string>();
                skipColumnNames.Add("Carryover");
                if (Utilities.HasNull(AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + i), skipColumnNames))
                {
                    knockoutRoundsCombobox.SelectedIndex = i-1;
                    return;
                }
            }
            knockoutRoundsCombobox.SelectedIndex = 0;
        }

        private void knockoutSessionsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            m_knockoutSessions.updateSessions();
        }

        private void knockoutRoundsCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_"+(knockoutRoundsCombobox.SelectedIndex+1));
            DataView dView = new DataView(table);
            dView.RowFilter = "";
            dView.Sort = "Match_Number ASC";
            knockoutScoresDataGridView.DataSource = dView;
        }

        private void eventSetupPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            populateKnockoutRounds();
        }

        private void knockoutScoresDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            string[] readOnlyColumns = new string[] { "Match_Number","Team_Name","Total" };
            string[] hideColumns = new string[] { };
            Utilities.setReadOnlyAndVisibleColumns(knockoutScoresDataGridView, readOnlyColumns, hideColumns);
        }

        private void knockoutScoresDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            int roundNumber = knockoutRoundsCombobox.SelectedIndex + 1;
            DataTable sessionsTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            DataRow dRow = sessionsTable.Rows.Find(roundNumber);
            int numberOfSessions = (int)dRow["Number_Of_Sessions"];
            if (columnName.Contains("Session") || columnName == "Carryover")
            {
                double total = 0;
                Object value = dgv.Rows[e.RowIndex].Cells["Carryover"].Value;
                total += (value == DBNull.Value) ? 0 : (double)value;
                for (int i = 1; i <= numberOfSessions; ++i)
                {
                    value = dgv.Rows[e.RowIndex].Cells["Session_" + i + "_Score"].Value;
                    total += (value == DBNull.Value) ? 0 : (double)value;
                }
                dgv.Rows[e.RowIndex].Cells["Total"].Value = total;
            }
            else if (columnName == "Team_Number")
            {
                dgv.Rows[e.RowIndex].Cells["Team_Name"].Value = LocalUtilities.getTeamName(m_databaseFileName, Constants.TableName.KnockoutTeams, (int)dgv.Rows[e.RowIndex].Cells["Team_Number"].Value);
            }
        }

        private void checkRoundCompletion(int roundNumber, int numberOfSessions)
        {
            DataTable sessionsTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            List<string> skipColumnNames = new List<string>();
            skipColumnNames.Add("Carryover");
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            if (Utilities.HasNull(table, skipColumnNames)) return;
            roundNumber--;
            if (roundNumber < 1) return;
            table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            DataRow dRow = sessionsTable.Rows.Find(roundNumber);
            numberOfSessions = (int)dRow["Number_Of_Sessions"];
            for (int i = 1; i <= numberOfSessions; ++i)
            {
                if (!Utilities.AllNull(table, "Session_" + i + "_Score")) return;
            }
            setUpMatches(roundNumber);
        }

        private void setUpMatches(int roundNumber)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            DataTable previousTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + (roundNumber+1));
            int matchNumber = 1;
            foreach (DataRow dRow in table.Rows)
            {
                DataRow[] dRows = previousTable.Select("Match_Number = " + matchNumber);
                DataRow winnerRow = findWinner(dRows);
                dRow["Team_Number"] = AccessDatabaseUtilities.getIntValue(winnerRow, "Team_Number");
                dRow["Team_Name"] = AccessDatabaseUtilities.getStringValue(winnerRow, "Team_Name");
                matchNumber++;
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            knockoutRoundsCombobox.SelectedIndex = roundNumber - 1;
        }

        private DataRow findWinner(DataRow[] dRows)
        {
            return (AccessDatabaseUtilities.getDoubleValue(dRows[1], "Total") > AccessDatabaseUtilities.getDoubleValue(dRows[0], "Total") ? dRows[1] : dRows[0]);
        }

        private void saveScoresButton_Click(object sender, EventArgs e)
        {
            int roundNumber = knockoutRoundsCombobox.SelectedIndex + 1;
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutScores+"_"+roundNumber);
            Utilities.showBalloonNotification("Saved " + Constants.KnockoutScoresFileName + " to database successfully", "Save Done");
            KnockoutTeamsDatabaseToWebpages ktdw = new KnockoutTeamsDatabaseToWebpages(m_eventName, m_databaseFileName, Constants.getEventWebpagesFolder(m_eventName));
            ktdw.createWebpages_();
            showUrl();

        }

        private void reloadScoresButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(Constants.KnockoutScoresFileName))
            {
                int roundNumber = knockoutRoundsCombobox.SelectedIndex + 1;
                AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
                Utilities.showBalloonNotification("Reloaded " + Constants.KnockoutScoresFileName + " from database successfully", "Reload Done");
            }
        }

        private void regenerateWebpageButton_Click(object sender, EventArgs e)
        {
            double oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            KnockoutTeamsDatabaseToWebpages ktdw = new KnockoutTeamsDatabaseToWebpages(m_eventName, m_databaseFileName, Constants.getEventWebpagesFolder(m_eventName));
            ktdw.createWebpages_();
            Utilities.fontSize = oldFontSize;
            showUrl();
        }

        private void showUrl()
        {
            displayWebBrowser.Url = new Uri(Path.Combine(Constants.getEventWebpagesFolder(m_eventName), "index.html"));
        }

        private void refreshKnockoutPage_Click(object sender, EventArgs e)
        {
            showUrl();
        }

        private void knockoutNamesDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Utilities.setReadOnlyAndVisibleColumns(knockoutNamesDataGridView, new string[] {"Team_Number","Original_Team_Number","Original_Event_Name"}, null);
        }

        private void publishResultsButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_resultsPublishParameters.ResultsWebsite))
            {
                MessageBox.Show("Please provide a results website to publish to.");
                return;
            }
            string siteName, pagePath;
            Utilities.getGoogleSiteComponents(m_resultsPublishParameters.ResultsWebsite, out siteName, out pagePath);
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            SitesAPI sa = new SitesAPI(siteName, username, password, true, false);
            CustomBackgroundWorker cbw = new CustomBackgroundWorker("Publish Results", sa.uploadDirectoryInBackground, null, publishResultsStatus,
                publishResultsProgressBar, cancelPublishResultsButton, null);
            double oldFontSize = Utilities.fontSize;
            Utilities.fontSize = m_resultsPublishParameters.FontSize;
            Tuple<string, string> values = new Tuple<string, string>(Constants.getEventWebpagesFolder(m_eventName), pagePath);
            cbw.run(values);
            Utilities.fontSize = oldFontSize;

        }

        private void reloadKnockoutScoresButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload("Knockout Scores"))
            {
                int roundNumber = knockoutRoundsCombobox.SelectedIndex + 1;
                AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            }
        }

        private void saveKnockoutScoresButton_Click(object sender, EventArgs e)
        {
            int roundNumber = knockoutRoundsCombobox.SelectedIndex + 1;
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            DataTable sessionsTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            DataRow dRow = sessionsTable.Rows.Find(roundNumber);
            int numberOfSessions = (int)dRow["Number_Of_Sessions"];
            checkRoundCompletion(roundNumber, numberOfSessions);
        }

    }
}
