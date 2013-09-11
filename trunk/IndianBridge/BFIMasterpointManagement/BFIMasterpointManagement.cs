using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.WordpressAPIs;
using System.Web.Script.Serialization;

namespace BFIMasterpointManagement
{
    public partial class BFIMasterpointManagement : Form
    {
        ManageMasterpoints mm;
        
        private Dictionary<string, string> m_getTableDataResult;
        public BFIMasterpointManagement()
        {
            InitializeComponent();
        }

        private void getTournamentTypesButton_Click(object sender, EventArgs e)
        {
            /*ManageMasterpoints mm = new ManageMasterpoints("http://127.0.0.1/bfitest", "nsriram", "cvans7671");
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = tableNameTextbox.Text;
            tableInfo.where = whereTextbox.Text;
            tableInfo.orderBy = orderByTextbox.Text;
            if (!String.IsNullOrWhiteSpace(limitLengthTextbox.Text) && !String.IsNullOrWhiteSpace(limitStartTextbox.Text))
                tableInfo.limit = limitStartTextbox.Text + "," + limitLengthTextbox.Text;
            tournamentTypesTextbox.Text = mm.getTableData(tableInfo);*/
        }

        private void addTournamentTypesButton_Click(object sender, EventArgs e)
        {
            /*ManageMasterpoints mm = new ManageMasterpoints("http://127.0.0.1/bfitest", "nsriram", "cvans7671");
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = tableNameTextbox.Text;
            tableInfo.content = tournamentTypesTextbox.Text;
            tableInfo.delimiter = ",";
            string result = mm.addTableData(tableInfo);
            MessageBox.Show(result, "Add Tournament Type Response");*/
        }

        private void addUsersButton_Click(object sender, EventArgs e)
        {
            ManageMasterpoints mm = new ManageMasterpoints("http://127.0.0.1/bfitest", "nsriram", "cvans7671");
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = "bfi_member";
            tableInfo.content = tournamentTypesTextbox.Text;
            tableInfo.delimiter = ",";
            string result = mm.addUsers(tableInfo);
            MessageBox.Show(result, "Add Users Response");
        }

        private void addMasterpointsButton_Click(object sender, EventArgs e)
        {
            ManageMasterpoints mm = new ManageMasterpoints("http://127.0.0.1/bfitest", "nsriram", "cvans7671");
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = "bfi_tournament_masterpoint";
            tableInfo.content = tournamentTypesTextbox.Text;
            tableInfo.delimiter = ",";
            string result = mm.addMasterpoints(tableInfo);
            MessageBox.Show(result, "Add Masterpoints Response");
        }

        private void loadTournamentLevels()
        {
            ttmDataGridView.Enabled = false;
            ttmLoadingPicture.Visible = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;
            bw.DoWork += new DoWorkEventHandler(getTournamentLevels);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool errorStatus = Convert.ToBoolean(m_getTableDataResult["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Unable to retrive table data because : " + m_getTableDataResult["message"], "Get Table Data Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string m_ttmData = m_getTableDataResult["content"];
            string[] lines = m_ttmData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] columnHeaders = lines[0].Split(',');
            ttmDataGridView.Columns.Clear();
            ttmDataGridView.Rows.Clear();
            foreach (string columnHeader in columnHeaders)
            {
                ttmDataGridView.Columns.Add(columnHeader, columnHeader);
            }
            for (int i = 1; i < lines.Length; ++i)
            {
                ttmDataGridView.Rows.Add(lines[i].Split(','));
            }
            ttmLoadingPicture.Visible = false;
            ttmDataGridView.Enabled = true;
        }

        private void getTournamentLevels(object sender, DoWorkEventArgs e)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = "bfi_tournament_level_master";
            string json_result = mm.getTableData(tableInfo);
            var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
            m_getTableDataResult = serializer.Deserialize<Dictionary<string, string>>(json_result);
        }


        private void validateCredentials(bool closeOnError)
        {
            AddTournamentLevel vmc = new AddTournamentLevel();
            vmc.StartPosition = FormStartPosition.CenterParent;
            if (vmc.ShowDialog(this) == DialogResult.Cancel)
            {
                if (closeOnError) this.Close();
                return;
            }
            mm = vmc.mm;
            toolStripUsername.Text = "Logged in as : " + mm.UserName;
            toolStripLoginButton.Enabled = true;
            loadTournamentLevels();
        }

        private void toolStripLoginButton_ButtonClick(object sender, EventArgs e)
        {
            validateCredentials(false);
        }

        private void BFIMasterpointManagement_Shown(object sender, EventArgs e)
        {
            validateCredentials(true);
        }

        private void addTournamentLevelButton_Click(object sender, EventArgs e)
        {
            AddNewTournamentLevel atl = new AddNewTournamentLevel(mm);
            atl.StartPosition = FormStartPosition.CenterParent;
            if (atl.ShowDialog(this) != DialogResult.Cancel)
            {
                //MessageBox.Show("Added Tournament Level Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ttmDataGridView.Rows.Add(atl.newRowCSV.Split(','));
            }
        }

        private void ttmDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridView dgv = sender as DataGridView;
            DataGridViewRow row = dgv.Rows[rowIndex];
            MessageBox.Show("Clicked "+row.Cells["description"].Value.ToString());
        }
    }
}
