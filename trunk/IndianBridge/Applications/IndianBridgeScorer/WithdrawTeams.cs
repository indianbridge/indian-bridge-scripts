using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;

namespace IndianBridgeScorer
{
    public partial class WithdrawTeams : Form
    {
        private string m_databaseFileName;
        private string m_tableName;
        public WithdrawTeams(string databaseFileName)
        {
            m_databaseFileName = databaseFileName;
            m_tableName = Constants.TableName.EventNames;
            InitializeComponent();
            withdrawTeamsDataGridView.DataSource = AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, m_tableName);
        }

        private void withdrawTeamsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Utilities.setReadOnlyAndVisibleColumns(withdrawTeamsDataGridView,
                new string[] { "Team_Number", "Team_Name", "Total_Score", "Rank" },
                new string[] { "Member_Names", "Carryover", "Original_Team_Number", "Original_Event_Name", "Tiebreaker_Score" });
        }

        private void saveWithdrawTeamsToDatabase_Click(object sender, EventArgs e)
        {
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, m_tableName);
            Utilities.showBalloonNotification("Save Completed", m_tableName + " saved to Database successfully");
        }

        private void reloadWithdrawTeamsButton_Click(object sender, EventArgs e)
        {
            if (Utilities.confirmReload(Constants.TableName.EventNames))
            {
                AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, m_tableName);
                Utilities.showBalloonNotification("Reload Completed", m_tableName + " reloaded from database successfully");
            }
        }

    }
}
