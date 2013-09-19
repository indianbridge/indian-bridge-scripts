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
        
        private Dictionary<string, string> m_getTournamentLevelResult;
        private Dictionary<string, string> m_getTournamentResult;
        private Dictionary<string, string> m_getEventResult;
        public BFIMasterpointManagement()
        {
            InitializeComponent();
        }

        private void addUsersButton_Click(object sender, EventArgs e)
        {

        }

        private void addMasterpointsButton_Click(object sender, EventArgs e)
        {

        }

        private void loadEvents()
        {
            emDataGridView.Enabled = false;
            emLoadingPicture.Visible = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;
            bw.DoWork += new DoWorkEventHandler(getEvents);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getEventsCompleted);
            bw.RunWorkerAsync();
        }

        private void loadTournaments()
        {
            tmDataGridView.Enabled = false;
            tmLoadingPicture.Visible = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;
            bw.DoWork += new DoWorkEventHandler(getTournaments);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getTournamentsCompleted);
            bw.RunWorkerAsync();
        }

        private void loadTournamentLevels()
        {
            ttmDataGridView.Enabled = false;
            ttmLoadingPicture.Visible = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;
            bw.DoWork += new DoWorkEventHandler(getTournamentLevels);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(getTournamentLevelsCompleted);
            bw.RunWorkerAsync();
        }

        private void getEventsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool errorStatus = Convert.ToBoolean(m_getEventResult["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Unable to retrive event master data because : " + m_getEventResult["message"], "Get Table Data Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string m_emData = m_getEventResult["content"];
            string[] lines = m_emData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] columnHeaders = lines[0].Split(',');
            emDataGridView.Columns.Clear();
            emDataGridView.Rows.Clear();
            foreach (string columnHeader in columnHeaders)
            {
                emDataGridView.Columns.Add(columnHeader, columnHeader);
            }
            for (int i = 1; i < lines.Length; ++i)
            {
                emDataGridView.Rows.Add(lines[i].Split(','));
            }
            emLoadingPicture.Visible = false;
            emDataGridView.Enabled = true;
        }

        private void getEvents(object sender, DoWorkEventArgs e)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = "bfi_event_master";
            string json_result = mm.getTableData(tableInfo);
            var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
            m_getEventResult = serializer.Deserialize<Dictionary<string, string>>(json_result);
        }


        private void getTournamentsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool errorStatus = Convert.ToBoolean(m_getTournamentResult["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Unable to retrive tournament master data because : " + m_getTournamentResult["message"], "Get Table Data Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string m_tmData = m_getTournamentResult["content"];
            string[] lines = m_tmData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] columnHeaders = lines[0].Split(',');
            tmDataGridView.Columns.Clear();
            tmDataGridView.Rows.Clear();
            foreach (string columnHeader in columnHeaders)
            {
                tmDataGridView.Columns.Add(columnHeader, columnHeader);
            }
            for (int i = 1; i < lines.Length; ++i)
            {
                tmDataGridView.Rows.Add(lines[i].Split(','));
            }
            tmLoadingPicture.Visible = false;
            tmDataGridView.Enabled = true;
        }

        private void getTournaments(object sender, DoWorkEventArgs e)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = "bfi_tournament_master";
            string json_result = mm.getTableData(tableInfo);
            var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
            m_getTournamentResult = serializer.Deserialize<Dictionary<string, string>>(json_result);
        }

        private void getTournamentLevelsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool errorStatus = Convert.ToBoolean(m_getTournamentLevelResult["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Unable to retrive tournament level master data because : " + m_getTournamentLevelResult["message"], "Get Table Data Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string m_ttmData = m_getTournamentLevelResult["content"];
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
            m_getTournamentLevelResult = serializer.Deserialize<Dictionary<string, string>>(json_result);
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
            loadTournaments();
            loadEvents();
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
                loadTournamentLevels();
            }
        }

        private void ttmDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridView dgv = sender as DataGridView;
            DataGridViewRow row = dgv.Rows[rowIndex];
            AddNewTournament ant = new AddNewTournament(mm, ttmDataGridView, row.Cells["tournament_level_code"].Value.ToString());
            ant.StartPosition = FormStartPosition.CenterParent;
            if (ant.ShowDialog(this) != DialogResult.Cancel)
            {
                loadTournaments();
            }
        }

        private void addTournamentButton_Click(object sender, EventArgs e)
        {
            AddNewTournament ant = new AddNewTournament(mm, ttmDataGridView, null);
            ant.StartPosition = FormStartPosition.CenterParent;
            if (ant.ShowDialog(this) != DialogResult.Cancel)
            {
                loadTournaments();
            }

        }

        private void addEventButton_Click(object sender, EventArgs e)
        {
            AddNewEvent ane = new AddNewEvent(mm);
            ane.StartPosition = FormStartPosition.CenterParent;
            if (ane.ShowDialog(this) != DialogResult.Cancel)
            {
                loadEvents();
            }
        }

        private void uploadUsersButton_Click(object sender, EventArgs e)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.content = usersTextbox.Text;
            tableInfo.delimiter = ",";
            string json_result = mm.addUsers(tableInfo);
            var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
            Dictionary<string, string> result = serializer.Deserialize<Dictionary<string, string>>(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            if (errorStatus)
            {
                MessageBox.Show(result["message"]+Environment.NewLine+result["content"], "Error adding Users !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result["message"] + Environment.NewLine + result["content"], "Success adding Users !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
 
        }

        private void uploadMasterpointsButton_Click(object sender, EventArgs e)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.content = masterpointsTextbox.Text;
            tableInfo.delimiter = ",";
            string json_result = mm.addMasterpoints(tableInfo);
            var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
            Dictionary<string, string> result = serializer.Deserialize<Dictionary<string, string>>(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            if (errorStatus)
            {
                MessageBox.Show(result["message"] + Environment.NewLine + result["content"], "Error adding Masterpoints !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result["message"] + Environment.NewLine + result["content"], "Success adding Masterpoints !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
