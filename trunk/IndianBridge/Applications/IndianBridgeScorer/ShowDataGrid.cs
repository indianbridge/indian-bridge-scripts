using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace IndianBridgeScorer
{
    public partial class ShowDataGrid : Form
    {
        public bool cancelPressed = false;
        private bool updatePressed = false;
        private int m_roundNumber;
        private DataTable m_VPTable;
        public ShowDataGrid(DataView view, string title, string[] readOnlyColumns, string[] hideColumns, int roundNumber, DataTable VPTable)
        {
            InitializeComponent();
            dataGridView1.DataSource = view;
            initialize(title, readOnlyColumns, hideColumns);
            m_roundNumber = roundNumber;
            m_VPTable = VPTable;
        }

        private void initialize(string title, string[] readOnlyColumns, string[] hideColumns)
        {

            updatePressed = false;
            this.Text = title;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (string str in readOnlyColumns)
            {
                dataGridView1.Columns[str].ReadOnly = true;
            }
            foreach (string str in hideColumns)
            {
                dataGridView1.Columns[str].Visible = false;
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            updatePressed = false;
            this.Close();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Contains("Team_1_Number"))
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
            }
            cancelPressed = false;
            updatePressed = true;
            this.Close();
        }

        private void ShowDataGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            Button button = sender as Button;
            if (!updatePressed)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to cancel? Any changes you have made will be lost.", "Confirm Cancel!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    cancelPressed = true;
                }
                else
                {
                    e.Cancel = true;
                    cancelPressed = false;
                }
            }
        }

        private string checkDrawForErrors()
        {
            string message = "";
            if (dataGridView1.Columns.Contains("Team_1_Number"))
            {
                int totalTeams = TeamScorer.numberOfTeams + (TeamScorer.numberOfTeams % 2);
                DataView dView = ((DataView)dataGridView1.DataSource);
                int row = 1;
                foreach (DataRowView rowView in dView)
                {
                    DataRow dRow = rowView.Row;
                    int team1Number = (int)dRow["Team_1_Number"];
                    if (team1Number < 1 - TeamScorer.numberOfTeams % 2 || team1Number > totalTeams) message += "\nTeam 1 Number in row " + row + " is not between "+(1 - TeamScorer.numberOfTeams % 2)+ " and " + totalTeams;
                    int team2Number = (int)dRow["Team_2_Number"];
                    if (team2Number < 1 - TeamScorer.numberOfTeams % 2 || team2Number > totalTeams) message += "\nTeam 2 Number in row " + row + " is not between " + (1 - TeamScorer.numberOfTeams % 2) + " and " + totalTeams;
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
            DataTable table = ((DataView)dataGridView1.DataSource).Table;
            DataRow[] dRows = table.Select("Round_Number < " + m_roundNumber + " AND Team_1_Number = " + team1Number + " AND Team_2_Number = " + team2Number);
            if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            if (team1Number == totalTeams)
            {
                dRows = table.Select("Round_Number < " + m_roundNumber + " AND Team_1_Number = 0 AND Team_2_Number = " + team2Number);
                if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            }
            if (team2Number == totalTeams)
            {
                dRows = table.Select("Round_Number < " + m_roundNumber + " AND Team_1_Number = "+team1Number+" AND Team_2_Number = 0");
                if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            }            
            return -1;
        }

        private string checkDrawForWarnings()
        {
            string message = "";
            if (dataGridView1.Columns.Contains("Team_1_Number"))
            {
                int totalTeams =TeamScorer.numberOfTeams + (TeamScorer.numberOfTeams % 2);
                Occurences[] occurences = new Occurences[totalTeams];
                for (int i = 0; i < totalTeams; ++i)
                {
                    occurences[i].count = 0;
                    occurences[i].team1Occurence = new List<int>();
                    occurences[i].team2Occurence = new List<int>();
                }
                DataView dView = ((DataView)dataGridView1.DataSource);
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
                        foreach(int number in occurences[i].team1Occurence) {
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

        private void updateVPs(int rowNumber)
        {
            int roundNumber = (int)dataGridView1.Rows[rowNumber].Cells["Round_Number"].Value;
            int tableNumber = (int)dataGridView1.Rows[rowNumber].Cells["Table_Number"].Value;
            int team1Number = (int)dataGridView1.Rows[rowNumber].Cells["Team_1_Number"].Value;
            int team2Number = (int)dataGridView1.Rows[rowNumber].Cells["Team_2_Number"].Value;
            Object value = dataGridView1.Rows[rowNumber].Cells["Team_1_IMPs"].Value;
            double team1IMPs = value == DBNull.Value ? 0 : (double)value;
            value = dataGridView1.Rows[rowNumber].Cells["Team_2_IMPs"].Value;
            double team2IMPs = value == DBNull.Value ? 0 : (double)value;
            double difference = team1IMPs - team2IMPs;
            double absoluteDifference = Math.Abs(difference);
            DataRow[] dRows = m_VPTable.Select("Number_Of_IMPs_Lower<=" + absoluteDifference + " AND Number_Of_IMPs_Upper>=" + absoluteDifference);
            Debug.Assert(dRows.Length == 1, "There should be exactly one row in VP Scale for given number of imps");
            int team1VPs = (difference>=0)?(int)dRows[0]["Team_1_VPs"]:(int)dRows[0]["Team_2_VPs"];
            int team2VPs = (difference<0) ? (int)dRows[0]["Team_1_VPs"] : (int)dRows[0]["Team_2_VPs"];
            dataGridView1.Rows[rowNumber].Cells["Team_1_VPs"].Value = team1VPs;
            dataGridView1.Rows[rowNumber].Cells["Team_2_VPs"].Value = team2VPs;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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

        private bool calculateComplementaryVPs(int rowNumber, string columnName)
        {
            string otherColumnName = (columnName == "Team_1_VPs") ? "Team_2_VPs" : "Team_1_VPs";
            double vps = (double)dataGridView1.Rows[rowNumber].Cells[columnName].Value;
            if (vps < 0 || vps > 25)
            {
                MessageBox.Show(columnName + " is not between 0 and 25", "Not in Range!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Object otherCell = dataGridView1.Rows[rowNumber].Cells[otherColumnName].Value;
            double otherValue;
            if (vps == 25 && otherCell != DBNull.Value)
            {
                otherValue = (double)otherCell;
                if (otherValue <= 5) return true;
            }
            otherValue = 30 - vps;
            if (otherValue > 25) otherValue = 25;
            dataGridView1.Rows[rowNumber].Cells[otherColumnName].Value = otherValue;
            return true;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (cancelPressed)
            {
                e.Cancel = true;
                return;
            }
            DataGridView dgv = sender as DataGridView;
            string columnName = dgv.Columns[e.ColumnIndex].Name;
            MessageBox.Show("Invalid value specified for "+columnName+" at row index : "+e.RowIndex,"Invalid Value!",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }


    }
}
