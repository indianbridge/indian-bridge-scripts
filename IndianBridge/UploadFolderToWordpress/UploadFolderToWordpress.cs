using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.WordpressAPIs;
using System.IO;
using Excel;

namespace UploadFolderToWordpress
{
    public partial class UploadFolderToWordpress : Form
    {
        private bool m_publishResultsRunning = false;
        private CustomBackgroundWorker m_publishResultsCBW = null;
        private DataTable m_parsedDataTable = null;
        private string m_fileName;
        private string m_htmlFileName;

        public UploadFolderToWordpress()
        {
            InitializeComponent();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFolderTextbox.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void publishResultsCompleted(bool success)
        {
            m_publishResultsRunning = false;
            if (success)
            {
                MessageBox.Show("Upload Successful. See Status Textbox for details.","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void publishResultsToWordpress()
        {
            if (m_publishResultsRunning)
            {
                Utilities.showErrorMessage("A Publish Results operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            string siteName =  wordpressURLTextbox.Text;
            string pagePath = wordpressPathTextbox.Text;
            string username = usernameTextbox.Text;
            string password = passwordTextbox.Text;
            UploadWebpages uw = new UploadWebpages(siteName, username, password, true);
            uw.UseTourneyTemplate = useTourneyTemplateCheckbox.Checked;
            uw.ForceUpload = true;
            String fileName = selectedFolderTextbox.Text;
            if (Directory.Exists(fileName) || File.Exists(fileName))
            {
                m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground, publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, uploadStatusTextbox);
                Tuple<string, string> values = new Tuple<string, string>(selectedFolderTextbox.Text, pagePath);
                m_publishResultsRunning = true;
                m_publishResultsCBW.run(values);
            }
            else
            {
                MessageBox.Show(fileName + " is not a valid file or folder!");
            }


        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            publishResultsToWordpress();
        }

        private void selectFolderButton_Click_1(object sender, EventArgs e)
        {
            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFolderTextbox.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void selectFileToUploadButton_Click(object sender, EventArgs e)
        {
            if (selectFileOrFolderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFolderTextbox.Text = selectFileOrFolderDialog.FileName;
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == "uploadFiles")
            {
                statusStrip1.Visible = true;
            }
            else statusStrip1.Visible = false;
        }

        private void loadCSVButton_Click(object sender, EventArgs e)
        {
            DialogResult dResult = openFileDialog1.ShowDialog();
            if (dResult != DialogResult.OK) return;
            m_fileName = openFileDialog1.FileName;
            string errorMessage = "";
            bool error = false;
            if (m_fileName.EndsWith(".xls") || m_fileName.EndsWith(".xlsx"))
            {
                parseExcelFile(m_fileName, out m_parsedDataTable, ref errorMessage, ref error);
            }
            else if (m_fileName.EndsWith(".txt") || m_fileName.EndsWith(".csv"))
            {
                parseCSVFile(m_fileName, out m_parsedDataTable, ref errorMessage, ref error);
            }
            else
            {
                Utilities.showErrorMessage("Unknown file extension in file name : " + m_fileName);
                return;
            }
            if (error)
            {
                Utilities.showErrorMessage(errorMessage);
                return;
            }
            else if (m_parsedDataTable == null)
            {
                Utilities.showErrorMessage("Parsed DataTable is null for unknown reason.");
                return;
            }
            m_htmlFileName = Path.Combine(Path.GetDirectoryName(m_fileName), Path.GetFileNameWithoutExtension(m_fileName) + ".html");
            convertToHTML();
        }

        private void addRow(ref DataTable table, string[] columnNames, string[] values, ref string errorMessage, ref bool error, string prefix)
        {
            try
            {
                DataRow dRow = table.NewRow();
                for (int i = 0; i < columnNames.Length; ++i)
                {
                    if (columnNames[i] == "event_date")
                    {
                        DateTime date = Convert.ToDateTime(values[i]);
                        dRow[columnNames[i]] = date.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dRow[columnNames[i]] = values[i];
                    }
                }
                table.Rows.Add(dRow);
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, prefix + exc.Message);
            }
        }

        private string dataTableToCSV(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            string[] columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in table.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }
            return sb.ToString();
        }

        private void parseExcelSheet(DataTable excelTable, out DataTable table, ref string errorMessage, ref bool error, string prefix)
        {
            string[] columnNames = excelTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            table = new DataTable();
            foreach (string columnName in columnNames)
            {
                table.Columns.Add(columnName);
            }
            int rowNo = 2;
            foreach (DataRow row in excelTable.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                addRow(ref table, columnNames, fields, ref errorMessage, ref error, prefix + " at Row " + rowNo + " : ");
                rowNo++;
            }
        }

        private void parseExcelFile(string fileName, out DataTable table, ref string errorMessage, ref bool error)
        {
            try
            {
                FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = fileName.EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet(true);
                excelReader.Close();
                // Read only first sheet
                parseExcelSheet(result.Tables[0], out table, ref errorMessage, ref error, "In Excel Sheet " + result.Tables[0].TableName);
                //for (int i = 0; i < result.Tables.Count; ++i) parseExcelSheet(result.Tables[i], ref table, ref errorMessage, ref error, "In Excel Sheet " + result.Tables[i].TableName);
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, exc.Message);
                table = null;
                return;
            }
        }

        private void parseCSVFile(string fileName, out DataTable table, ref string errorMessage, ref bool error)
        {
            try
            {
                string fileContents = File.ReadAllText(fileName);
                string[] lines = fileContents.Split(new string[] { Utilities.getNewLineCharacter(fileContents) }, StringSplitOptions.RemoveEmptyEntries);
                int index = 0;
                while (index < lines.Length && string.IsNullOrWhiteSpace(lines[index])) index++;
                if (lines.Length - index < 2)
                {
                    error = true;
                    Utilities.appendToMessage(ref errorMessage, "CSV File contents needs minimum 2 lines - one for header and atleast one for actual content!");
                    table = null;
                    return;
                }
                // Header line
                string headerLine = lines[index++];
                string[] columnNames = headerLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                table = new DataTable();
                foreach (string columnName in columnNames)
                {
                    table.Columns.Add(columnName);
                }
                while (index < lines.Length)
                {
                    var currentLine = lines[index++];
                    if (string.IsNullOrWhiteSpace(currentLine)) break;
                    var rowValues = currentLine.Split(new[] { ',' });
                    addRow(ref table, columnNames, rowValues, ref errorMessage, ref error, "In Row " + index + " : ");
                }
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, exc.Message);
                table = null;
                return;
            }

        }

        private void convertToHTML()
        {
            try
            {
                string[] columnNames = m_parsedDataTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName).
                                                  ToArray();
                const string tableContainerClass = "datagrid";
                string title = Utilities.ConvertCaseString(Path.GetFileNameWithoutExtension(m_fileName), Utilities.Case.PascalCase);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<html>");
                sb.AppendLine("\t<head>");
                sb.AppendLine(String.Format("\t\t<title>{0}</title>", title));
                sb.AppendLine("\t</head>");
                sb.AppendLine("\t<body>");
                sb.AppendLine(String.Format("\t\t<h2>{0}</h2>", title));

                sb.AppendLine(String.Format("\t\t<div class=\"{0}\">", tableContainerClass));
                sb.AppendLine("\t\t\t<table class=\"stripeme\">");
                sb.AppendLine("\t\t\t\t<thead>");
                sb.AppendLine("\t\t\t\t\t" + Utilities.GetHTMLTableHeader(columnNames));
                sb.AppendLine("\t\t\t\t</thead>");
                sb.AppendLine("\t\t\t\t<tbody>");
                foreach (DataRow row in m_parsedDataTable.Rows)
                {
                    sb.AppendLine("\t\t\t\t\t" + Utilities.GetHTMLRowResult(row.ItemArray));
                }
                sb.AppendLine("\t\t\t\t</tbody>");
                sb.AppendLine("\t\t\t</table>");
                sb.AppendLine("\t\t</div>");
                sb.AppendLine("\t</body>");
                sb.AppendLine("</html>");
                Utilities.WriteFile(m_htmlFileName, sb.ToString());
                loadHTMLFile();
            }
            catch (Exception ex)
            {
                Utilities.showErrorMessage("Unable to write to HTML file : " + ex.Message);
            }
        }

        private void loadHTMLFile()
        {
            selectedFolderTextbox.Text = m_htmlFileName;
            htmlBrowser.Url = new System.Uri("file:///" + m_htmlFileName);
            savedAsLabel.Text = "Saved As : " + m_htmlFileName;
        }

        private void saveAsHTMLButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = m_htmlFileName;
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(m_htmlFileName);
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                File.Copy(m_htmlFileName, saveFileDialog1.FileName, true);
                m_htmlFileName = saveFileDialog1.FileName;
                loadHTMLFile();
            }
        }

    }
}
