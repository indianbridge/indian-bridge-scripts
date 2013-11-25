using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FtpLib;
using IndianBridge.Common;
using IndianBridge.WordpressAPIs;
using WordpressAPIs;

namespace CSVParser
{
	public partial class Form1 : Form
	{
		private Tuple<DataTable, string[]> m_parseResults;
		private CustomBackgroundWorker m_publishResultsCBW = null;
		private bool m_publishResultsRunning = false;
		private string m_resultsFolderPath;
		private double oldFontSize;
		string[] tourneyNames;
		string[] tourneyPageIDs;

		public Form1()
		{
			InitializeComponent();
			this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
			txtFileContents.Visible = lblContents.Visible = btnSaveHtml.Visible = txtTitle.Visible = label3.Visible = cmbStyling.Visible =
				lblTitle.Visible = label1.Visible = txtFileName.Visible = label2.Visible = txtPath.Visible = btnPublish.Visible = false;
		}

		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			var fileContents = String.Empty;
			// Show the dialog and get result.
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK) // Test result.
			{
				var file = openFileDialog1.FileName;
				fileContents = File.ReadAllText(file);
			}

			ParseFile(fileContents);

			btnSaveHtml.Visible = txtTitle.Visible = label3.Visible = cmbStyling.Visible =
				lblTitle.Visible = label1.Visible = txtFileName.Visible = true;

			txtFileName.Text = openFileDialog1.SafeFileName;

			cmbStyling.SelectedIndex = 0;
		}

		private void ParseFile(string fileContents)
		{
			m_parseResults = CSVUtilities.ParseCSV(fileContents);

			var columnsOutput = m_parseResults.Item2.Aggregate(String.Empty,
				(current, column) => current + String.Format("{0} ,", column));

			columnsOutput = columnsOutput.TrimEnd(new[] {','});

			// Show the preview
			PublishResults(m_parseResults.Item1);
		}

		private void PublishResults(DataTable results)
		{
			foreach (DataRow row in results.Rows)
			{
				txtFileContents.AppendText(Utilities.WriteRowResult(row.ItemArray));
			}
		}

		private void PublishResultsToHTML(DataTable results, IEnumerable<string> columnNames,
			string fileName = @"C:\results.html")
		{
			var tableContainerClass = cmbStyling.SelectedItem.ToString();
			var titleRow = !String.IsNullOrWhiteSpace(txtTitle.Text)
				? String.Format(
					"<table><tr><td width=\"90%\"><h2>{0}</h2></td><td width=\"8%\" style=\"vertical-align: middle;align:right\"><a href=\"..\">Up one level</a></td></tr></table>",
					txtTitle.Text)
				: "<table><tr><td width=\"90%\"></td><td width=\"8%\" style=\"vertical-align: middle;align:right\"><a href=\"..\">Up one level</a></td></tr></table>";

			var htmlHeader = String.Format("{0}<div class=\"{1}\"><table class=\"stripeme\"><thead>\n", titleRow,
				tableContainerClass);
			var htmlString = htmlHeader + Utilities.GetHTMLTableHeader(columnNames) + "</thead><tbody>";
			htmlString = results.Rows.Cast<DataRow>().Aggregate(htmlString, (current, row) =>
				current + Utilities.GetHTMLRowResult(row.ItemArray));
			htmlString += "\n</tbody></table></div></html>";
			Utilities.WriteFile(fileName, htmlString);
		}

		private void GetTourneys()
		{
			var addTourneys = new AddTourneys("http://bfitest.bfi.net.in/", "vdevadass", "bitspilani");
			var jsonResult = addTourneys.getTourneys();
			var result = Utilities.ConvertJsonOutputToTourneyResults(jsonResult);

			var tourneyList = result.content;
			var tokens = tourneyList.Split('#');
			tourneyNames = tokens[0].Split(',');
			tourneyPageIDs = tokens[1].Split(',');
			tourneyNamesCombobox.Items.AddRange(tourneyNames);
			tourneyNamesCombobox.SelectedIndex = 0;

			var currentYear = DateTime.Today.Year;
			tourneyYearCombobox.Items.Clear();
			tourneyYearCombobox.Items.Add(currentYear);
			tourneyYearCombobox.Items.Add(currentYear + 1);
			tourneyYearCombobox.SelectedIndex = 0;
		}

		private void btnSaveHtml_Click(object sender, EventArgs e)
		{
			if (m_parseResults == null)
			{
				txtFileContents.AppendText("No results found - please check the input file");
				return;
			}

			var result = folderBrowserDialog1.ShowDialog();
			if (result != DialogResult.OK) return;

			var foldername = folderBrowserDialog1.SelectedPath;
			var fileName = String.IsNullOrEmpty(txtFileName.Text) ? "results.html" : String.Format("{0}.html", txtFileName.Text);
			PublishResultsToHTML(m_parseResults.Item1, m_parseResults.Item2, String.Format(@"{0}\{1}", foldername, fileName));

			MessageBox.Show("Results file saved successfully", "Success");
		}

		private void btnPublish_Click(object sender, EventArgs e)
		{
			// TODO : Use form values (or use login form)
			//string siteName = "http://127.0.0.1/bfi/";
			string siteName = "http://bfitest.bfi.net.in/";
			string username = "vdevadass";
			string password = "bitspilani";

			string pagePath = txtPath.Text; // /tourneys/winter-national/y2012/results/team-event
			if (!pagePath.StartsWith("/"))
				pagePath = String.Format("/{0}", pagePath);

			statusStrip1.Visible = true;
			txtFileContents.Clear();

			var uw = new UploadWebpages(siteName, username, password, true, true) {ForceUpload = true};

			var m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground, 
				publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, txtFileContents);

			oldFontSize = Utilities.fontSize;
			Utilities.fontSize = 5;
			var values = new Tuple<string, string>(m_resultsFolderPath, pagePath);
			m_publishResultsRunning = true;
			m_publishResultsCBW.run(values);
		}

		private void publishResultsCompleted(bool success)
		{
			m_publishResultsRunning = false;
			Utilities.fontSize = oldFontSize;
		}

		private void btnUploadResults_Click(object sender, EventArgs e)
		{
			pnlResults.Visible = true;
			pnlBulletin.Visible = false;
			pnlMenu.Visible = false;
			btnPublish.Visible = label2.Visible = txtPath.Visible = txtFileContents.Visible = lblContents.Visible = true;
			GetTourneys();
		}

		private void btnMainMenu_Click(object sender, EventArgs e)
		{
			pnlResults.Visible = false;
			pnlBulletin.Visible = false;
			pnlMenu.Visible = true;
		}

		private void btnUploadBulletin_Click(object sender, EventArgs e)
		{
			pnlResults.Visible = false;
			pnlBulletin.Visible = true;
			pnlMenu.Visible = false;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string _remoteHost = "ftp.bfitest.net.in";
			string _remoteUser = "bfi@bfitest.net.in";
			string _remotePass = "bfi";
			string source = @"C:\Users\snarasim\Downloads\test.pdf";
			string destination = "/test.pdf";
			using (FtpConnection ftp = new FtpConnection(_remoteHost, _remoteUser, _remotePass))
			{
				try
				{
					ftp.Open(); // Open the FTP connection 
					ftp.Login(); // Login using previously provided credentials
					ftp.PutFile(source, destination);
					MessageBox.Show("Done");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

		}

		private void btnRootFolder_Click(object sender, EventArgs e)
		{
			var result = folderBrowserDialog2.ShowDialog();
			if (result != DialogResult.OK) return;
			m_resultsFolderPath = folderBrowserDialog2.SelectedPath;

			panel1.Visible = panel2.Visible = label7.Visible = label8.Visible = true;

			folderBrowserDialog1.SelectedPath = m_resultsFolderPath;
		}

		private void txtTitle_TextChanged(object sender, EventArgs e)
		{
			txtTitle.Select();
		}

	}
}
