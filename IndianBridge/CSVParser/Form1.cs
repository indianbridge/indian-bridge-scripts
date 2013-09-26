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

			ParseFile(fileContents);

			txtFileContents.Visible = lblContents.Visible = btnSaveHtml.Visible = txtTitle.Visible = lblTitle.Visible = true;
		}

		private void ParseFile(string fileContents)
		{
			try
			{
				m_parseResults = CSVUtilities.ParseCSV(fileContents);

				txtResults.AppendText("\n");
				txtResults.AppendText("Columns: ");
				var columnsOutput = m_parseResults.Item2.Aggregate(String.Empty,
				                                          (current, column) => current + String.Format("{0} ,", column));

				columnsOutput = columnsOutput.TrimEnd(new[] {','});
				txtResults.AppendText(columnsOutput);

				// Show the preview
				PublishResults(m_parseResults.Item1);
			}
			catch (Exception exc)
			{
				txtResults.AppendText("\n");
				txtResults.AppendText(exc.Message);
				txtResults.AppendText("\n");
				txtResults.AppendText(exc.StackTrace);
			}
		}

		private void PublishResults(DataTable results)
		{
			foreach (DataRow row in results.Rows)
			{
				txtFileContents.AppendText(Utilities.WriteRowResult(row.ItemArray));
			}
		}

		private void PublishResultsToHTML(DataTable results, IEnumerable<string> columnNames, string fileName = @"C:\results.html")
		{
			const string tableContainerClass = "datagrid";
			var htmlHeader = String.Format("<html><head><title></title>{0}</head><h2>{1}</h2><div class=\"{2}\"><table class=\"stripeme\"><thead>\n", txtTitle.Text,txtTitle.Text,tableContainerClass);
			var htmlString = htmlHeader + Utilities.GetHTMLTableHeader(columnNames) + "</thead><tbody>";
			htmlString = results.Rows.Cast<DataRow>().Aggregate(htmlString, (current, row) =>
				current + Utilities.GetHTMLRowResult(row.ItemArray));
			htmlString += "\n</tbody></table><div></html>";
			Utilities.WriteFile(fileName, htmlString);
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
