using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.WordpressAPIs;

namespace CSVParser
{
	public partial class Form1 : Form
	{
		private Tuple<DataTable, string[]> m_parseResults;
		private CustomBackgroundWorker m_publishResultsCBW = null;
		private bool m_publishResultsRunning = false;
		private double oldFontSize;

		public Form1()
		{
			InitializeComponent();
			this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
			txtFileContents.Visible = lblContents.Visible = btnSaveHtml.Visible = txtTitle.Visible = lblTitle.Visible = false;
		}

		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			var size = -1;
			var fileContents = String.Empty;
			// Show the dialog and get result.
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK) // Test result.
			{
				var file = openFileDialog1.FileName;
				try
				{
					fileContents = File.ReadAllText(file);
					size = fileContents.Length;
				}
				catch (Exception exc)
				{
					txtResults.AppendText(exc.Message);
					txtResults.Visible = true;
				}
			}

			txtResults.AppendText("File size is " + size + "\n");
			txtResults.AppendText(result.ToString());

			m_parseResults = ParseCSV(fileContents);

			txtFileContents.Visible = lblContents.Visible = btnSaveHtml.Visible = txtTitle.Visible = lblTitle.Visible = true;
		}

		private Tuple<DataTable, string[]> ParseCSV(string fileContents)
		{
			Tuple<DataTable, string[]> parseResults = null;
			try
			{
				// Tracks the position within the file
				var previousPosition = 0;

				// Skip empty lines at the beginning of the file, if any
				var results = SkipEmptyLines(fileContents);

				var currentPosition = results.IndexOf("\n");
				var headerLine = results.Substring(previousPosition, currentPosition - previousPosition);

				var columnNames = GetColumns(headerLine);
				var values = ConstructDataTable(columnNames);

				txtResults.AppendText("\n");
				txtResults.AppendText("Columns: ");
				var columnsOutput = columnNames.Aggregate(String.Empty,
				                                          (current, column) => current + String.Format("{0} ,", column));

				columnsOutput = columnsOutput.TrimEnd(new[] {','});
				txtResults.AppendText(columnsOutput);

				// Skip empty lines after the header, if any
				results = SkipEmptyLines(results.Substring(currentPosition + 1));

				//txtFileContents.AppendText(results);

				previousPosition = 0;
				currentPosition = results.IndexOf("\n");

				while (true)
				{
					var currentLine = results.Substring(previousPosition, currentPosition - previousPosition);
					if (currentLine == String.Empty || currentLine == "\r") break;
					var rowValues = currentLine.Split(new[] {','});
					AddRow(rowValues, columnNames, ref values);
					previousPosition = currentPosition + 1;
					currentPosition = results.IndexOf("\n", previousPosition);
					if (currentPosition < 0) break;
				}

				// Show the preview
				PublishResults(values);

				parseResults = new Tuple<DataTable, string[]>(values, columnNames);
			}
			catch (Exception exc)
			{
				txtResults.AppendText("\n");
				txtResults.AppendText(exc.Message);
				txtResults.AppendText("\n");
				txtResults.AppendText(exc.StackTrace);
			}

			return parseResults;
		}

		private void PublishResults(DataTable results)
		{
			foreach (DataRow row in results.Rows)
			{
				WriteRowResult(row.ItemArray);
			}
		}

		private void PublishResultsToHTML(DataTable results, IEnumerable<string> columnNames, string fileName = @"C:\results.html")
		{
			string tableContainerClass = "datagrid";
			var htmlHeader = String.Format("<html><head><title></title>{0}</head><h2>{1}</h2><div class=\"{2}\"><table class=\"stripeme\"><thead>\n", txtTitle.Text,txtTitle.Text,tableContainerClass);
			var htmlString = htmlHeader  + GetHTMLTableHeader(columnNames)+"</thead><tbody>";
			htmlString = results.Rows.Cast<DataRow>().Aggregate(htmlString, (current, row) => 
				current + GetHTMLRowResult(row.ItemArray));
			htmlString += "\n</tbody></table><div></html>";
			WriteFile(fileName, htmlString);
		}

		private static void WriteFile(string fileName, string content)
		{
			var fileStream = new StreamWriter(fileName);
			fileStream.Write(content);
			fileStream.Close();
		}

		private static string GetHTMLTableHeader(IEnumerable<object> columnNames)
		{
			var tableHeader = columnNames.Aggregate("<tr>", (current, value) =>
				current + String.Format("<th>{0}</th>", value.ToString().Trim()));
			tableHeader += "</tr>\n";
			return tableHeader;
		}

		private string GetHTMLRowResult(IEnumerable<object> rowValues)
		{
			var rowString = rowValues.Aggregate("<tr>", (current, value) => 
				current + String.Format("<td>{0}</td>", value.ToString().Trim()));
			rowString += "</tr>\n";
			return rowString;
		}

		private void WriteRowResult(IEnumerable<object> rowValues)
		{
			var rowString = rowValues.Aggregate(String.Empty, (current, rowValue) => current + String.Format("{0},", rowValue.ToString().Trim()));
			rowString = rowString.TrimEnd(new[] { ',' });
			rowString = rowString.Replace("\r", String.Empty);
			txtFileContents.AppendText(rowString + "\n");
		}

		private static DataTable ConstructDataTable(IEnumerable<string> columnNames)
		{
			var result = new DataTable();
			foreach (var columnName in columnNames)
			{
				result.Columns.Add(columnName, Type.GetType("System.String"));
			}
			return result;
		}

		private void AddRow(string[] rowValues, string[] columnNames, ref DataTable result)
		{
			var row = result.NewRow();
			for (var i = 0; i < columnNames.Length; i++ )
			{
				row[columnNames[i]] = rowValues[i];
			}
			result.Rows.Add(row);
		}

		private string[] GetColumns(string headerLine)
		{
			return headerLine.Split(new char[] { ',' });
		}

		private string SkipEmptyLines(string contents)
		{
			var previousPosition = 0;
			var currentPosition = contents.IndexOf("\n");
			var line = contents.Substring(previousPosition, currentPosition - previousPosition);
			while (true)
			{
				if (line != "\r")
				{
					break;
				}

				previousPosition = currentPosition + 1;
				currentPosition = contents.IndexOf("\n", previousPosition);
				line = contents.Substring(previousPosition, currentPosition - previousPosition);
			}

			return contents.Substring(previousPosition);
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
				checkBox1.Text = "Hide debug info";
			txtResults.Visible = checkBox1.Checked;
		}

		private void btnSaveHtml_Click(object sender, EventArgs e)
		{
			if (m_parseResults == null)
			{
				txtResults.AppendText("No results found - please check the input file");
				return;
			}

			var result = folderBrowserDialog1.ShowDialog();
			if (result != DialogResult.OK) return;

			var foldername = folderBrowserDialog1.SelectedPath;
			PublishResultsToHTML(m_parseResults.Item1, m_parseResults.Item2, String.Format(@"{0}\results.html", foldername));

			MessageBox.Show("HTML File (results.html) saved successfully", "Success");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string siteName = "http://bfitest.bfi.net.in/";
			string pagePath = "/tourneys/winter-national/y2012/results/team-event";
			string username = "vdevadass";
			string password = "bitspilani";
			UploadWebpages uw = new UploadWebpages(siteName, username, password, true, true);
			var m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground, 
				publishResultsCompleted, null, null, null, null);
			oldFontSize = Utilities.fontSize;
			Utilities.fontSize = 5;
			var values = new Tuple<string, string>(folderBrowserDialog1.SelectedPath, pagePath);
			m_publishResultsRunning = true;
			m_publishResultsCBW.run(values);
			MessageBox.Show("Published successfully", "Success");

		}

		private void publishResultsCompleted(bool success)
		{
			m_publishResultsRunning = false;
			Utilities.fontSize = oldFontSize;
			if (success) txtResults.Text = ("Results published succesfully at " + txtDomainName.Text);
		}
	}
}
