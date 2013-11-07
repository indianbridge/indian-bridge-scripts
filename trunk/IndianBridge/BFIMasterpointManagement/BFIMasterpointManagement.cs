using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.WordpressAPIs;
using System.Web.Script.Serialization;
using Excel;

namespace BFIMasterpointManagement
{


	public partial class BFIMasterpointManagement : Form
	{
		ManageMasterpoints mm;
        UploadProgress up = null;
        bool loggedIn = false;
		
		private Dictionary<string, string> m_getTournamentLevelResult;
		private Dictionary<string, string> m_getTournamentResult;
		private Dictionary<string, string> m_getEventResult;
        private string fieldSeparator = "#";

        public BFIMasterpointManagement()
		{
			InitializeComponent();
		}


		private void loadEvents()
		{
			emDataGridView.Enabled = false;
			emLoadingPicture.Visible = true;
            addEventButton.Enabled = false;
            addEventButton.Visible = false;
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
            addTournamentButton.Enabled = false;
            addTournamentButton.Visible = false;
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
			ttmLoadingPicture.BringToFront();
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
            string[] lines = m_emData.Split(new string[] { Utilities.getNewLineCharacter(m_emData) }, StringSplitOptions.RemoveEmptyEntries);
			string[] columnHeaders = lines[0].Split(new string[] { fieldSeparator},StringSplitOptions.None);
			emDataGridView.Columns.Clear();
			emDataGridView.Rows.Clear();
			foreach (string columnHeader in columnHeaders)
			{
				emDataGridView.Columns.Add(columnHeader, columnHeader);
			}
			for (int i = 1; i < lines.Length; ++i)
			{
				emDataGridView.Rows.Add(lines[i].Split(new string[] { fieldSeparator }, StringSplitOptions.None));
			}
            emDataGridView.Sort(emDataGridView.Columns["event_code"], ListSortDirection.Descending);
			emLoadingPicture.Visible = false;
			emDataGridView.Enabled = true;
            addEventButton.Enabled = true;
            addEventButton.Visible = true;
		}

		private void getEvents(object sender, DoWorkEventArgs e)
		{
            string json_result = mm.getTableData("bfi_event_master");
            m_getEventResult = Utilities.convertJsonOutput(json_result);
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
            string[] lines = m_tmData.Split(new string[] { Utilities.getNewLineCharacter(m_tmData) }, StringSplitOptions.RemoveEmptyEntries);
            string[] columnHeaders = lines[0].Split(new string[] { fieldSeparator }, StringSplitOptions.None);
            tmDataGridView.Rows.Clear();
			tmDataGridView.Columns.Clear();
			foreach (string columnHeader in columnHeaders)
			{
				tmDataGridView.Columns.Add(columnHeader, columnHeader);
			}
			for (int i = 1; i < lines.Length; ++i)
			{
				tmDataGridView.Rows.Add(lines[i].Split(new string[] { fieldSeparator }, StringSplitOptions.None));
			}
            tmDataGridView.Sort(tmDataGridView.Columns["tournament_code"], ListSortDirection.Descending);
			tmLoadingPicture.Visible = false;
			tmDataGridView.Enabled = true;
            addTournamentButton.Enabled = true;
            addTournamentButton.Visible = true;
            tmDataGridView.Refresh();
		}

		private void getTournaments(object sender, DoWorkEventArgs e)
		{
            string json_result = mm.getTableData("bfi_tournament_master");
            m_getTournamentResult = Utilities.convertJsonOutput(json_result);
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
			string[] lines = m_ttmData.Split(new string[] { Utilities.getNewLineCharacter(m_ttmData)}, StringSplitOptions.RemoveEmptyEntries);
            string[] columnHeaders = lines[0].Split(new string[] { fieldSeparator }, StringSplitOptions.None);
			ttmDataGridView.Columns.Clear();
			ttmDataGridView.Rows.Clear();
			foreach (string columnHeader in columnHeaders)
			{
				ttmDataGridView.Columns.Add(columnHeader, columnHeader);
			}
			for (int i = 1; i < lines.Length; ++i)
			{
				ttmDataGridView.Rows.Add(lines[i].Split(new string[] { fieldSeparator }, StringSplitOptions.None));
			}
			ttmLoadingPicture.Visible = false;
			ttmLoadingPicture.SendToBack();
			ttmDataGridView.Enabled = true;
		}

		private void getTournamentLevels(object sender, DoWorkEventArgs e)
		{
            string json_result = mm.getTableData("bfi_tournament_level_master");
            m_getTournamentLevelResult = Utilities.convertJsonOutput(json_result);
		}


		private void validateCredentials()
		{
            loggingOutLabel.SendToBack();
            loggingOutLabel.Visible = false;
			AddTournamentLevel vmc = new AddTournamentLevel();
			vmc.StartPosition = FormStartPosition.CenterParent;
            if (vmc.ShowDialog(this) == DialogResult.Cancel)
            {
                toolStripUsername.Text = "Not Logged in";
                toolStripLoginButton.Enabled = true;
                loggedIn = false;
            }
            else
            {
                mm = vmc.mm;
                up = new UploadProgress(mm);
                toolStripUsername.Text = "Logged in as : " + mm.UserName;
                toolStripLoginButton.Enabled = true;
                loggedIn = true;
                loadTournamentLevels();
                loadTournaments();
                loadEvents();
            }
		}

		private void toolStripLoginButton_ButtonClick(object sender, EventArgs e)
		{
            DialogResult result = MessageBox.Show("Are you sure you want to logout current user and login as different user?", "Are you Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                logOut();
                validateCredentials();
            }
		}

		private void BFIMasterpointManagement_Shown(object sender, EventArgs e)
		{
			validateCredentials();
		}

        private bool checkIfLoggedIn()
        {
            if (!loggedIn)
            {
                MessageBox.Show("You are not logged in. Please Login using the button at the bottom before proceeding", "Not Logged In!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return loggedIn;
        }

		private void addTournamentLevelButton_Click(object sender, EventArgs e)
		{
            if (checkIfLoggedIn())
            {
                AddNewTournamentLevel atl = new AddNewTournamentLevel(mm);
                atl.StartPosition = FormStartPosition.CenterParent;
                if (atl.ShowDialog(this) != DialogResult.Cancel)
                {
                    loadTournamentLevels();
                }
            }
		}

		private void ttmDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int rowIndex = e.RowIndex;
			DataGridView dgv = sender as DataGridView;
			DataGridViewRow row = dgv.Rows[rowIndex];
			AddNewTournament ant = new AddNewTournament(mm, ttmDataGridView, tmDataGridView ,row.Cells["description"].Value.ToString());
			ant.StartPosition = FormStartPosition.CenterParent;
			if (ant.ShowDialog(this) != DialogResult.Cancel)
			{
				loadTournaments();
			}
		}

		private void addTournamentButton_Click(object sender, EventArgs e)
		{
            if (checkIfLoggedIn())
            {
                AddNewTournament ant = new AddNewTournament(mm, ttmDataGridView, tmDataGridView, null);
                ant.StartPosition = FormStartPosition.CenterParent;
                if (ant.ShowDialog(this) != DialogResult.Cancel)
                {
                    loadTournaments();
                }
            }

		}

		private void addEventButton_Click(object sender, EventArgs e)
		{
            if (checkIfLoggedIn())
            {
                AddNewEvent ane = new AddNewEvent(mm, emDataGridView);
                ane.StartPosition = FormStartPosition.CenterParent;
                if (ane.ShowDialog(this) != DialogResult.Cancel)
                {
                    loadEvents();
                }
            }
		}


		private void loadUsersButton_Click(object sender, EventArgs e)
		{
            var dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
            var file = openFileDialog1.FileName;
            string errorMessage = "";
            bool error = false;
            string csvContents = MasterpointManagementUtilties.loadUsersFile(file, ref errorMessage, ref error);
            if (error)
            {
                uploadUsersButton.Enabled = false;
                uploadUsersButton.Visible = false;
                usersTextbox.ForeColor = Color.Red;
                usersTextbox.Text = errorMessage;
                Utilities.showErrorMessage("Errors Found! See textbox for details.");
            }
            else
            {
                uploadUsersButton.Enabled = true;
                uploadUsersButton.Visible = true;
                usersTextbox.ForeColor = Color.Green;
                usersTextbox.Text = csvContents;
            }
            return;
		}

        private void uploadUsersButton_Click(object sender, EventArgs e)
        {
            if (checkIfLoggedIn())
            {
                statusMessageTextbox.Text = "";
                controlTabs.SelectTab("responseMessage");
                controlTabs.Enabled = false;
                up.uploadUsers(usersTextbox.Text);
                if (up.errorFound) statusMessageTextbox.ForeColor = Color.Red;
                else statusMessageTextbox.ForeColor = Color.Green;
                statusMessageTextbox.AppendText(up.statusText + Environment.NewLine + "Details" + Environment.NewLine + up.m_returnContent);
                controlTabs.Enabled = true;
            }
        }

		private void loadMasterpointsButton_Click(object sender, EventArgs e)
		{
            var dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
            var file = openFileDialog1.FileName;
            string errorMessage = "";
            bool error = false;
            string csvContents = MasterpointManagementUtilties.loadMasterpointsFile(file, ref errorMessage, ref error);
            if (error)
            {
                uploadMasterpointsButton.Enabled = false;
                uploadMasterpointsButton.Visible = false;
                masterpointsTextbox.ForeColor = Color.Red;
                masterpointsTextbox.Text = errorMessage;
                Utilities.showErrorMessage("Errors Found! See textbox for details.");
            }
            else
            {
                masterpointsTextbox.ForeColor = Color.Green;
                masterpointsTextbox.Text = csvContents;
                uploadMasterpointsButton.Enabled = true;
                uploadMasterpointsButton.Visible = true;
            }
            return;
		}

        private void uploadMasterpointsButton_Click(object sender, EventArgs e)
        {
            if (checkIfLoggedIn())
            {
                statusMessageTextbox.Text = "";
                controlTabs.SelectTab("responseMessage");
                controlTabs.Enabled = false;
                up.uploadMasterpoints(masterpointsTextbox.Text);
                if (up.errorFound) statusMessageTextbox.ForeColor = Color.Red;
                else statusMessageTextbox.ForeColor = Color.Green;
                statusMessageTextbox.AppendText(up.statusText);
                controlTabs.Enabled = true;
            }
        }

        private void loadUsersToDeleteButton_Click(object sender, EventArgs e)
        {
            var dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
            var file = openFileDialog1.FileName;
            string errorMessage = "";
            bool error = false;
            string csvContents = MasterpointManagementUtilties.loadDeleteUsersFile(file, ref errorMessage, ref error);
            if (error)
            {
                deleteUsersButton.Enabled = false;
                deleteUsersButton.Visible = false;
                deleteUsersTextbox.ForeColor = Color.Red;
                deleteUsersTextbox.Text = errorMessage;
                Utilities.showErrorMessage("Errors Found! See textbox for details.");
            }
            else
            {
                deleteUsersButton.Enabled = true;
                deleteUsersButton.Visible = true;
                deleteUsersTextbox.ForeColor = Color.Green;
                deleteUsersTextbox.Text = csvContents;
            }
            return;
        }

        private void deleteUsersButton_Click(object sender, EventArgs e)
        {
            if (checkIfLoggedIn())
            {
                statusMessageTextbox.Text = "";
                controlTabs.SelectTab("responseMessage");
                controlTabs.Enabled = false;
                up.deleteUsers(deleteUsersTextbox.Text);
                if (up.errorFound) statusMessageTextbox.ForeColor = Color.Red;
                else statusMessageTextbox.ForeColor = Color.Green;
                statusMessageTextbox.AppendText(up.statusText);
                controlTabs.Enabled = true;
            }
        }

        private void loadUsersToTransferButton_Click(object sender, EventArgs e)
        {
            var dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
            var file = openFileDialog1.FileName;
            string errorMessage = "";
            bool error = false;
            string csvContents = MasterpointManagementUtilties.loadTransferUsersFile(file, ref errorMessage, ref error);
            if (error)
            {
                transferUsersButton.Enabled = false;
                transferUsersButton.Visible = false;
                transferUsersTextbox.ForeColor = Color.Red;
                transferUsersTextbox.Text = errorMessage;
                Utilities.showErrorMessage("Errors Found! See textbox for details.");
            }
            else
            {
                transferUsersButton.Enabled = true;
                transferUsersButton.Visible = true;
                transferUsersTextbox.ForeColor = Color.Green;
                transferUsersTextbox.Text = csvContents;
            }
            return;
        }

        private void transferUsersButton_Click(object sender, EventArgs e)
        {
            if (checkIfLoggedIn())
            {
                statusMessageTextbox.Text = "";
                controlTabs.SelectTab("responseMessage");
                controlTabs.Enabled = false;
                up.transferUsers(transferUsersTextbox.Text);
                if (up.errorFound) statusMessageTextbox.ForeColor = Color.Red;
                else statusMessageTextbox.ForeColor = Color.Green;
                statusMessageTextbox.AppendText(up.statusText);
                controlTabs.Enabled = true;
            }
        }

        private void logOut()
        {
            controlTabs.Visible = false;
            loggingOutLabel.Visible = true;
            controlTabs.SendToBack();
            loggingOutLabel.BringToFront();
            this.Refresh();
            mm.invalidateCredentials();
            controlTabs.Visible = true;
            loggingOutLabel.Visible = false;
            controlTabs.BringToFront();
            loggingOutLabel.SendToBack();
        }

        private void BFIMasterpointManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (loggedIn) logOut();
        }
	}
}
