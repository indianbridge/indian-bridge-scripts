using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using IndianBridge.Common;
using FtpLib;
using IndianBridge.WordpressAPIs;
using WordpressAPIs;

namespace CSVParser
{
	public partial class Form1 : Form
	{
		private Tuple<DataTable, string[]> m_parseResults;
		private CustomBackgroundWorker m_publishResultsCBW = null;
		private string m_resultsFolderPath;
		private double oldFontSize;
		private string m_username, m_password, m_site, m_ftpSite, m_ftpUsername, m_ftpPassword;
		TourneyList m_tourneyList;

		public Form1()
		{
			InitializeComponent();

			lblTitle.Visible = txtTitle.Visible = label1.Visible =
						txtFileName.Visible = label3.Visible = cmbStyling.Visible = lblPreview.Visible = txtPreview.Visible = false;

			btnPublish.Enabled = false;
			btnLogin.Enabled = false;

			tabSaveResults.Enabled = tabPublishBulletin.Enabled = tabPublishResults.Enabled = loadingPicture.Visible = false;

			var usernameFromConfig = ConfigurationManager.AppSettings["username"];
			var rootFolderFromConfig = ConfigurationManager.AppSettings["rootFolderDefault"];
			m_site = ConfigurationManager.AppSettings["site"];
			m_ftpSite = ConfigurationManager.AppSettings["ftpSite"];
			m_ftpUsername = ConfigurationManager.AppSettings["ftpUsername"];
			m_ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];

			if (!String.IsNullOrEmpty(usernameFromConfig))
				m_username = txtUserName.Text = usernameFromConfig;

			if (!String.IsNullOrEmpty(rootFolderFromConfig))
			{
				m_resultsFolderPath = folderBrowserDialog1.SelectedPath = folderBrowserDialog2.SelectedPath = rootFolderFromConfig;
				btnLogin.Enabled = true;
			}
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
				txtPreview.AppendText(Utilities.WriteRowResult(row.ItemArray));
			}
		}

		private void PublishResultsToHTML(DataTable results, IEnumerable<string> columnNames, string fileName)
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

		private Tuple<bool, string> GetTourneys(string site, string username, string password)
		{
			var addTourneys = new AddTourneys(site, username, password);
			var jsonResult = addTourneys.getTourneys();

			try
			{
				m_tourneyList = Utilities.JsonDeserialize<TourneyList>(jsonResult);
			}
			catch (Exception)
			{
				return null;
			}


			foreach (TourneyPageInfo pageInfo in m_tourneyList.content)
			{
				tourneyNamesCombobox.Items.Add(pageInfo.title);
				cmbTourneyName.Items.Add(pageInfo.title);
			}

			var currentYear = DateTime.Today.Year;
			tourneyYearCombobox.Items.Clear();
			tourneyYearCombobox.Items.Add(currentYear - 1);
			tourneyYearCombobox.Items.Add(currentYear);
			tourneyYearCombobox.Items.Add(currentYear + 1);
			tourneyYearCombobox.SelectedIndex = 1;

			cmbYear.Items.Clear();
			cmbYear.Items.Add(currentYear - 1);
			cmbYear.Items.Add(currentYear);
			cmbYear.Items.Add(currentYear + 1);
			cmbYear.SelectedIndex = 1;

			return new Tuple<bool, string>(m_tourneyList.error, m_tourneyList.message);
		}

		private void publishResultsCompleted(bool success)
		{
			Utilities.fontSize = oldFontSize;
		}

		private void buttonPublishBulletin_Click(object sender, EventArgs e)
		{
			var source = openFileDialog2.FileName;
			var fileName = openFileDialog2.SafeFileName;
			var destinationFileName = txtCaption.Text == String.Empty ? fileName : txtCaption.Text.Replace(" ", "_") +
				fileName.Substring(fileName.LastIndexOf("."));

			var destination = String.Format("{0}/{1}", getBulletinsPagePath(), destinationFileName);
			using (var ftp = new FtpConnection(m_ftpSite, m_ftpUsername, m_ftpPassword))
			{
				try
				{
					ftp.Open();
					ftp.Login();
					ftp.PutFile(source, destination);
					MessageBox.Show("Done");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

		}

		private void authenticate_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtUserName.Text))
			{
				MessageBox.Show("Username is required!");
				return;
			}
			if (string.IsNullOrWhiteSpace(txtPassword.Text))
			{
				MessageBox.Show("Password is required!");
				return;
			}

			loadingPicture.Enabled = true;
			loadingPicture.Visible = true;
			loadingPicture.BringToFront();

			this.Refresh();

			var result = GetTourneys(m_site, txtUserName.Text, txtPassword.Text);
			loadingPicture.Visible = false;

			if (result != null && !result.Item1)
			{
				tabPublishBulletin.Enabled = tabPublishResults.Enabled = tabSaveResults.Enabled = true;
				tabSaveResults.Select();
				tabControl1.TabPages.Remove(tabCredentials);
				m_username = txtUserName.Text;
				m_password = txtPassword.Text;
			}
			else
			{
				MessageBox.Show(String.Format("Sorry, the credentials you entered are invalid. {0}",
					result != null ? result.Item2 + "Error message: " + Environment.NewLine : String.Empty));
			}

		}

		private void btnRootFolder_Click_1(object sender, EventArgs e)
		{
			var result = folderBrowserDialog2.ShowDialog();
			if (result != DialogResult.OK) return;
			m_resultsFolderPath = folderBrowserDialog2.SelectedPath;

			label7.Visible = label8.Visible = true;

			folderBrowserDialog1.SelectedPath = m_resultsFolderPath;

			btnLogin.Enabled = true;
		}

		private void btnSelectFile_Click_1(object sender, EventArgs e)
		{
			var fileContents = String.Empty;
			// Show the dialog and get result.
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK) // Test result.
			{
				var file = openFileDialog1.FileName;
				fileContents = File.ReadAllText(file);
			}

			if (!String.IsNullOrEmpty(fileContents))
			{
				ParseFile(fileContents);

				lblTitle.Visible =
					txtTitle.Visible = label1.Visible = btnSaveHtml.Visible =
						txtFileName.Visible = label3.Visible = cmbStyling.Visible = lblPreview.Visible = txtPreview.Visible = true;

				txtTitle.Text = txtFileName.Text = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.LastIndexOf("."));

				cmbStyling.SelectedIndex = 0;
			}
		}

		private void btnSaveHtml_Click_1(object sender, EventArgs e)
		{
			if (m_parseResults == null)
			{
				txtStatus.AppendText("No results found - please check the input file");
				return;
			}

			var result = folderBrowserDialog1.ShowDialog();
			if (result != DialogResult.OK) return;

			var foldername = folderBrowserDialog1.SelectedPath;
			var fileName = String.IsNullOrEmpty(txtFileName.Text) ? "results.html" : String.Format("{0}.html", txtFileName.Text);
			PublishResultsToHTML(m_parseResults.Item1, m_parseResults.Item2, String.Format(@"{0}\{1}", foldername, fileName));

			MessageBox.Show("Results file saved successfully", "Success");
		}

		private void txtFileName_TextChanged(object sender, EventArgs e)
		{
		}

		private void btnPublish_Click_1(object sender, EventArgs e)
		{
			var pagePath = getResultsPagePath();

			statusStrip1.Visible = true;
			txtStatus.Clear();

			txtStatus.Visible = lblStatus.Visible = true;

			var uw = new UploadWebpages(m_site, m_username, m_password, true, true) { ForceUpload = true };

			var m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground,
				publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, txtStatus);

			oldFontSize = Utilities.fontSize;
			Utilities.fontSize = 5;
			var values = new Tuple<string, string>(m_resultsFolderPath, pagePath);
			m_publishResultsCBW.run(values);
		}

		private string getResultsPagePath()
		{
			var selectedTourneyName = tourneyNamesCombobox.SelectedItem.ToString();
			var selectedTourney = m_tourneyList.content.Find(x => x.title == selectedTourneyName);
			return String.Format("/{0}/y{1}/results", selectedTourney.directory, tourneyYearCombobox.SelectedItem);
		}

		private string getBulletinsPagePath()
		{
			var selectedTourneyName = cmbTourneyName.SelectedItem.ToString();
			var selectedTourney = m_tourneyList.content.Find(x => x.title == selectedTourneyName);
			return String.Format("/{0}/{1}/bulletins", selectedTourney.directory.Replace("tourneys/", String.Empty), cmbYear.SelectedItem);
		}

		private void tourneyNamesCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(tourneyNamesCombobox.Text))
			{
				btnPublish.Enabled = true;
			}
		}

		private void txtFileName_Click(object sender, EventArgs e)
		{
			txtFileName.Select();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			var result = openFileDialog2.ShowDialog();
			if (result != DialogResult.OK) // Test result.
			{
				MessageBox.Show("Please select a valid file to upload");
			}
		}
	}
}
