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
			emLoadingPicture.Visible = false;
			emDataGridView.Enabled = true;
		}

		private void getEvents(object sender, DoWorkEventArgs e)
		{
			TableInfo tableInfo = new TableInfo();
			tableInfo.tableName = "bfi_event_master";
            tableInfo.delimiter = fieldSeparator;
			string json_result = mm.getTableData(tableInfo);
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
			tmLoadingPicture.Visible = false;
			tmDataGridView.Enabled = true;
            tmDataGridView.Refresh();
		}

		private void getTournaments(object sender, DoWorkEventArgs e)
		{
			TableInfo tableInfo = new TableInfo();
			tableInfo.tableName = "bfi_tournament_master";
            tableInfo.delimiter = fieldSeparator;
			string json_result = mm.getTableData(tableInfo);
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
			TableInfo tableInfo = new TableInfo();
			tableInfo.tableName = "bfi_tournament_level_master";
            tableInfo.delimiter = fieldSeparator;
			string json_result = mm.getTableData(tableInfo);
            m_getTournamentLevelResult = Utilities.convertJsonOutput(json_result);
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
                Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
				bool errorStatus = Convert.ToBoolean(result["error"]);
                result["content"] = result["content"].Replace(Utilities.getNewLineCharacter(result["content"]),System.Environment.NewLine);
				if (errorStatus)
				{
					statusMessageTextbox.ForeColor = Color.Red;
					statusMessageTextbox.Text = result["message"] + Environment.NewLine + result["content"];
				}
				else
				{
					statusMessageTextbox.ForeColor = Color.Green;
					statusMessageTextbox.Text = result["message"] + Environment.NewLine + result["content"];
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
            statusMessageTextbox.ForeColor = Color.Blue;
            statusMessageTextbox.Text = "Processing...";
            controlTabs.SelectTab("responseMessage");
            TableInfo tableInfo = new TableInfo();
			tableInfo.content = masterpointsTextbox.Text;
			tableInfo.delimiter = ",";
			string json_result = mm.addMasterpoints(tableInfo);
            Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
			bool errorStatus = Convert.ToBoolean(result["error"]);
            result["content"] = result["content"].Replace(Utilities.getNewLineCharacter(result["content"]), System.Environment.NewLine);
            if (errorStatus)
            {
                statusMessageTextbox.ForeColor = Color.Red;
                statusMessageTextbox.Text = result["message"] + Environment.NewLine + result["content"];
            }
            else
            {
                statusMessageTextbox.ForeColor = Color.Green;
                statusMessageTextbox.Text = result["message"] + Environment.NewLine + result["content"];
            }
		}

		private void loadUsersButton_Click(object sender, EventArgs e)
		{
            string fileContents = "";
			// Show the dialog and get result.
			var dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
			var file = openFileDialog1.FileName;
            //IExcelDataReader excelReader = null;
            if (file.EndsWith(".xls") || file.EndsWith(".xlsx"))
            {
                MessageBox.Show("xls and xlsx files cannot be processed directly. Please convert to csv file first and then load it!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
                /*FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
                excelReader = file.EndsWith(".xls")?ExcelReaderFactory.CreateBinaryReader(stream):ExcelReaderFactory.CreateOpenXmlReader(stream);
                DataSet result = excelReader.AsDataSet();
                fileContents = "";
                excelReader.Close();
                int row_no = 0;
                int ind = 0;
                while (row_no < result.Tables[ind].Rows.Count) // ind is the index of table
                // (sheet name) which you want to convert to csv
                {
                    for (int i = 0; i < result.Tables[ind].Columns.Count; i++)
                    {
                        fileContents += result.Tables[ind].Rows[row_no][i].ToString() + ",";
                    }
                    row_no++;
                    fileContents += System.Environment.NewLine;
                }*/
            }
            else if (file.EndsWith(".txt") || file.EndsWith(".csv"))
            {
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
            else
            {
                MessageBox.Show("Unknown Extension in file : " + file+ " (only .txt, .csv, .xls, .xlsx are allowed)");
                return;
            }
            // Get the column names and the datatable of results
            var table = CSVUtilities.ParseCSV(fileContents).Item1;


            bool error = false;
            string message = CheckUserDatabaseColumns(table, out error);
            // Insert default values for missing columns that are required in the db
            CheckForColumn(ref table, "address_1", typeof(string), String.Empty);
            CheckForColumn(ref table, "city", typeof(string), String.Empty);
            CheckForColumn(ref table, "country", typeof(string), "India");
            CheckForColumn(ref table, "rank_code", typeof(string), "R00");
            CheckForColumn(ref table, "total_current_lp", typeof(double), 0);
            CheckForColumn(ref table, "total_current_fp", typeof(double), 0);
            CheckForColumn(ref table, "total_lp_yearend", typeof(double), 0);
            CheckForColumn(ref table, "total_fp_yearend", typeof(double), 0);
            CheckForColumn(ref table, "activation", typeof(int), 1);
            CheckForColumn(ref table, "block", typeof(int), 0);
            CheckForColumn(ref table, "state", typeof(string), String.Empty);
            CheckForColumn(ref table, "zone_code", typeof(string), "AM0");

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
                var contents = GetColumnNames(table) + System.Environment.NewLine;
                contents += GetRowData(table);
                usersTextbox.ForeColor = Color.Black;
                usersTextbox.Text = contents;
            }

		}

		private string CheckValueLength(DataRow row, string columnName, int length, ref bool error)
		{
			string tempMessage = "";
            if (!row.Table.Columns.Contains(columnName))
            {
                tempMessage += "Required Column : " + columnName + " is not present!";
                error = true;
                return tempMessage;
            }
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
            string value = "";
            bool localError = false;
            if (row[columnName].GetType() == typeof(double))
            {
                num = (double)row[columnName];
                localError = false;
            }
            else
            {
                value = (string)row[columnName];
                localError = !(Double.TryParse(value, out num));
            }
            error |= localError;
			return localError ? "Value for " + columnName + " : " + value + " is not numeric!" : "";
		}

		private string CheckInt(DataRow row, string columnName,ref bool error)
		{
			int num;
            string value = "";
            bool localError = false;
            if (row[columnName].GetType() == typeof(int))
            {
                num = (int)row[columnName];
                localError = false;
            }
            else
            {
                value = (string)row[columnName];
                localError = !(int.TryParse(value, out num));
            }
            error |= localError;
			return localError ? "Value for "+columnName+" : "+value+" is not an integer!" : "";
		}

		private string CheckUserColumns(DataTable table, out bool error)
		{
			error = false;
			string message = "";
			int rowIndex = 1;
			foreach (DataRow row in table.Rows)
			{
				bool localError = false;
				string tempMessage = "";
				tempMessage += CheckValueLength(row, "member_id", 8, ref localError);
				if (localError)
				{
					message += "In Row : " + rowIndex + " - " + tempMessage + Environment.NewLine;
				}
                error |= localError;
				rowIndex++;
			}
			return message;
		}

		private string CheckMasterpointColumns(DataTable table,out bool error)
		{
			error = false;
			string message = "";
			int rowIndex = 1;
			foreach (DataRow row in table.Rows)
			{
                bool localError = false;
				string tempMessage = "";
                tempMessage += CheckValueLength(row, "tournament_code", 5, ref localError);
                tempMessage += CheckValueLength(row, "event_code", 3, ref localError);
                tempMessage += CheckValueLength(row, "member_id", 8, ref localError);
                tempMessage += CheckDouble(row, "localpoints_earned", ref localError);
                tempMessage += CheckDouble(row, "fedpoints_earned", ref localError);
                if (localError)
                {
					message += "In Row : "+rowIndex+" - "+tempMessage + Environment.NewLine;
				}
                error |= localError;
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
                foreach (DataRow row in table.Rows)
                {
                    row[columnName] = defaultValue;
                }
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
            string fileContents = "";
            // Show the dialog and get result.
            var dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
            var file = openFileDialog1.FileName;
            //IExcelDataReader excelReader = null;
            if (file.EndsWith(".xls") || file.EndsWith(".xlsx"))
            {
                MessageBox.Show("xls and xlsx files cannot be processed directly. Please convert to csv file first and then load it!","Warning!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
                /*FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
                excelReader = file.EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
                DataSet result = excelReader.AsDataSet(true);
                fileContents = "";
                excelReader.Close();
                int row_no = 0;
                int ind = 0;
                while (row_no < result.Tables[ind].Rows.Count) // ind is the index of table
                // (sheet name) which you want to convert to csv
                {
                    for (int i = 0; i < result.Tables[ind].Columns.Count; i++)
                    {
                        fileContents += result.Tables[ind].Rows[row_no][i].ToString() + ",";
                    }
                    row_no++;
                    fileContents += System.Environment.NewLine;
                }*/
            }
            else if (file.EndsWith(".txt") || file.EndsWith(".csv"))
            {
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
            else
            {
                MessageBox.Show("Unknown Extension in file : " + file + " (only .txt, .csv, .xls, .xlsx are allowed)");
                return;
            }

			// Get the column names and the datatable of results
			var table = CSVUtilities.ParseCSV(fileContents).Item1;

			// Check for non-database columns
			bool error = false;
            string message = CheckMasterpointDatabaseColumns(table, out error);
			// Insert default values for missing columns that are required in the db
			CheckForColumn(ref table, "tournament_code", typeof(string), String.Empty);
			CheckForColumn(ref table, "event_code", typeof(string), String.Empty);
			CheckForColumn(ref table, "member_id", typeof(string), String.Empty);
			DateTime time = DateTime.Now; 
			CheckForColumn(ref table, "event_date", typeof(string), time.ToString("yyyy-MM-dd"));
			CheckForColumn(ref table, "participant_no", typeof(int), 0);
			CheckForColumn(ref table, "rank", typeof(int), 0);
			CheckForColumn(ref table, "localpoints_earned", typeof(double), 0);
			CheckForColumn(ref table, "fedpoints_earned", typeof(double), 0);
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
				var contents = GetColumnNames(table) + System.Environment.NewLine;
				contents += GetRowData(table);
				masterpointsTextbox.ForeColor = Color.Black;
				masterpointsTextbox.Text = contents;
			}
		}
	}
}
