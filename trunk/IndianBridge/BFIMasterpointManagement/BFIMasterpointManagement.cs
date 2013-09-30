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
            ttmLoadingPicture.SendToBack();
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
            statusMessageTextbox.ForeColor = Color.Blue;
            statusMessageTextbox.Text = "Processing...";
            controlTabs.SelectTab("responseMessage");
			TableInfo tableInfo = new TableInfo();
			tableInfo.content = usersTextbox.Text;
			tableInfo.delimiter = ",";
            try
            {
                string json_result = mm.addUsers(tableInfo);
                var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
                Dictionary<string, string> result = serializer.Deserialize<Dictionary<string, string>>(json_result);
                bool errorStatus = Convert.ToBoolean(result["error"]);
                if (errorStatus)
                {
                    statusMessageTextbox.ForeColor = Color.Red;
                    statusMessageTextbox.Text = result["message"] + Environment.NewLine + result["content"];
                    //MessageBox.Show(result["message"] + Environment.NewLine + result["content"], "Error adding Users !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    statusMessageTextbox.ForeColor = Color.Green;
                    statusMessageTextbox.Text = result["message"] + Environment.NewLine + result["content"];
                    //MessageBox.Show(result["message"] + Environment.NewLine + result["content"], "Success adding Users !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                statusMessageTextbox.ForeColor = Color.Red;
                statusMessageTextbox.Text = ex.Message;
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

		private void loadUsersButton_Click(object sender, EventArgs e)
		{
			var fileContents = String.Empty;
			// Show the dialog and get result.
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK) // Test result.
			{
				var file = openFileDialog1.FileName;
				try
				{
					fileContents = File.ReadAllText(file);
				}
				catch (Exception exc)
				{
                    usersTextbox.ForeColor = Color.Red;
					usersTextbox.Text = exc.Message;
					usersTextbox.Visible = true;
				}
			}

			// Get the column names and the datatable of results
			var table = CSVUtilities.ParseCSV(fileContents).Item1;

            bool error = false;
            string message = CheckUserDatabaseColumns(table, out error);
			// Insert default values for missing columns that are required in the db
			CheckForColumn(ref table, "address_1", typeof(String), String.Empty);
			CheckForColumn(ref table, "city", typeof(String), String.Empty);
			CheckForColumn(ref table, "country", typeof(String), "India");
			CheckForColumn(ref table, "rank_code", typeof(String), "R00");
			CheckForColumn(ref table, "total_current_lp", typeof(Decimal), 0);
			CheckForColumn(ref table, "total_current_fp", typeof(Decimal), 0);
			CheckForColumn(ref table, "total_lp_yearend", typeof(Decimal), 0);
			CheckForColumn(ref table, "total_fp_yearend", typeof(Decimal), 0);
			CheckForColumn(ref table, "activation", typeof(Int16), 1);
			CheckForColumn(ref table, "block", typeof(Int16), 0);
			CheckForColumn(ref table, "state", typeof(String), String.Empty);
			CheckForColumn(ref table, "zone_code", typeof(String), "AM0");

            bool tempError = false;
            message += CheckUserColumns(table, out tempError);
            error = error | tempError;
            if (error)
            {
                MessageBox.Show("Errors in file contents! Please see textbox for details!", "Errors Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                usersTextbox.ForeColor = Color.Red;
                usersTextbox.Text = message;
            }
            else
            {
                var contents = GetColumnNames(table) + "\r\n";
                contents += GetRowData(table);
                usersTextbox.ForeColor = Color.Black;
                usersTextbox.Text = contents;
            }

		}

        private string CheckValueLength(DataRow row, string columnName, int length, ref bool error)
        {
                string tempMessage = "";
                string value = (string)row[columnName];
                if (string.IsNullOrWhiteSpace(value)) {
                    tempMessage += "Value for " + columnName + " - " + value + " cannot be empty!";
                    error = true;
                }
                else if (value.Length != length) {
                    tempMessage += "Value for " + columnName + " - " + value + " has to be exactly "+length+" characters long!";
                    error = true;
                }
                return tempMessage;
        }

        private string CheckDouble(DataRow row, string columnName, ref bool error)
        {
            double num;
            string value = (string)row[columnName];
            error = Double.TryParse(value, out num);
            return error ? "Value for " + columnName + " : " + value + " is not numeric!" : "";
        }

        private string CheckInt(DataRow row, string columnName,ref bool error)
        {
            int num;
            string value = (string)row[columnName];
            error = Int32.TryParse(value, out num);
            return error ? "Value for "+columnName+" : "+value+" is not an integer!" : "";
        }

        private string CheckUserColumns(DataTable table, out bool outError)
        {
            outError = false;
            string message = "";
            int rowIndex = 1;
            foreach (DataRow row in table.Rows)
            {
                bool error = false;
                string tempMessage = "";
                tempMessage += CheckValueLength(row, "member_id", 8, ref error);
                if (error)
                {
                    message += "In Row : " + rowIndex + " - " + tempMessage + Environment.NewLine;
                }
                outError = outError | error;
                rowIndex++;
            }
            return message;
        }

        private string CheckMasterpointColumns(DataTable table,out bool outError)
        {
            outError = false;
            string message = "";
            int rowIndex = 1;
            foreach (DataRow row in table.Rows)
            {
                bool error = false;
                string tempMessage = "";
                tempMessage += CheckValueLength(row, "tournament_code", 5, ref error);
                tempMessage += CheckValueLength(row, "event_code", 3, ref error);
                tempMessage += CheckValueLength(row, "member_id", 8, ref error);
                tempMessage += CheckDouble(row, "localpoints_earned", ref error);
                tempMessage += CheckDouble(row, "fedpoints_earned", ref error);
                if (error) {
                    message += "In Row : "+rowIndex+" - "+tempMessage + Environment.NewLine;
                }
                outError = outError | error;
                rowIndex++;
            }
            return message;
        }

		// Check for the existence of a column in a datatable and if not present, add it
		private static void CheckForColumn(ref DataTable table, string columnName, Type columnType, object defaultValue)
		{
			if (!table.Columns.Contains(columnName))
			{
				table.Columns.Add(columnName, columnType);
			}
			foreach (DataRow row in table.Rows)
			{
				row[columnName] = defaultValue;
			}
		}

		// Get the column names as a comma separated string
		private static string GetColumnNames(DataTable table)
		{
			return table.Columns.Cast<DataColumn>().Aggregate(String.Empty, (current, column) => 
				current + String.Format("{0},", column.ColumnName.Replace("\r", String.Empty))).TrimEnd(new [] {','});
		}

		// Get all the data rows as a comma-separated string
		private static string GetRowData(DataTable table)
		{
			var result = String.Empty;
			foreach (DataRow row in table.Rows)
			{
				foreach (DataColumn column in table.Columns)
				{
					result += String.Format("{0},", row[column.ColumnName].ToString().Replace("\r", String.Empty));
				}
				result = result.TrimEnd(new[] { ',' });
				result += "\r\n";
			}
			return result;
		}
        private string CheckMasterpointDatabaseColumns(DataTable table, out bool error)
        {
            error = false;
            string message = "";
            string[] columnNames = {"tournament_code","event_code","member_id", "event_date","participant_no","rank","localpoints_earned","fedpoints_earned"};
            foreach (DataColumn column in table.Columns)
            {
                if (!columnNames.Contains(column.ColumnName))
                {
                    error = true;
                    message += "Column Name : " + column.ColumnName + " is not masterpoints database column name!" + Environment.NewLine;
                }
            }
            return message;
        }

        private string CheckUserDatabaseColumns(DataTable table, out bool error)
        {
            error = false;
            string message = "";
            string[] columnNames = { "member_id", "first_name", "last_name", "address_1", "address_2", "address_3", "city", "country","email",
                                   "residence_phone","mobile_no","sex","dob","rank","total_current_lp","total_current_fp"
                                   ,"total_lp_yearend","total_fp_yearend"
                                   ,"activation","block","area_code","country_code","state","mob_country_code","zone_code"};
            foreach (DataColumn column in table.Columns)
            {
                if (!columnNames.Contains(column.ColumnName))
                {
                    error = true;
                    message += "Column Name : " + column.ColumnName + " is not user database column name!" + Environment.NewLine;
                }
            }
            return message;
        }

        private void loadMasterpointsButton_Click(object sender, EventArgs e)
        {
            var fileContents = String.Empty;
            // Show the dialog and get result.
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var file = openFileDialog1.FileName;
                try
                {
                    fileContents = File.ReadAllText(file);
                }
                catch (Exception exc)
                {
                    masterpointsTextbox.ForeColor = Color.Red;
                    masterpointsTextbox.Text = exc.Message;
                    masterpointsTextbox.Visible = true;
                }
            }

            // Get the column names and the datatable of results
            var table = CSVUtilities.ParseCSV(fileContents).Item1;

            // Check for non-database columns
            bool error = false;
            string message = CheckMasterpointDatabaseColumns(table, out error);
            // Insert default values for missing columns that are required in the db
            CheckForColumn(ref table, "tournament_code", typeof(String), String.Empty);
            CheckForColumn(ref table, "event_code", typeof(String), String.Empty);
            CheckForColumn(ref table, "member_id", typeof(String), String.Empty);
            DateTime time = DateTime.Now; 
            CheckForColumn(ref table, "event_date", typeof(String), time.ToString("yyyy-MM-dd"));
            CheckForColumn(ref table, "participant_no", typeof(Int16), 0);
            CheckForColumn(ref table, "rank", typeof(Int16), 0);
            CheckForColumn(ref table, "local_points_earned", typeof(Decimal), 0);
            CheckForColumn(ref table, "fed_points_earned", typeof(Decimal), 0);
            bool tempError = false;
            message += CheckMasterpointColumns(table, out tempError);
            error = error | tempError;
            if (error)
            {
                MessageBox.Show("Errors in file contents! Please see textbox for details!", "Errors Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                masterpointsTextbox.ForeColor = Color.Red;
                masterpointsTextbox.Text = message;
            }
            else
            {
                var contents = GetColumnNames(table) + "\r\n";
                contents += GetRowData(table);
                masterpointsTextbox.ForeColor = Color.Black;
                masterpointsTextbox.Text = contents;
            }
        }
	}
}
