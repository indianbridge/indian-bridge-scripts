using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IndianBridge.Common;
using Nini.Config;
using IndianBridge.ResultsManager;
using IndianBridge.GoogleAPIs;

namespace IndianBridge.Applications
{
    public partial class WinterNationals2011PairsScorer : Form
    {
        private IConfigSource m_configParameters;
        public WinterNationals2011PairsScorer()
        {
            InitializeComponent();
            m_configParameters = new DotNetConfigSource(DotNetConfigSource.GetFullConfigPath());
            getFieldName("GoogleSiteName");
            Utilities.m_rootDirectory = Directory.GetCurrentDirectory();
            loadEventPageMapping();
        }

        private void getFieldName(string fieldName)
        {
           if (m_configParameters.Configs["appSettings"].Get(fieldName) == null)
            {
               GetTextInput gti = new GetTextInput(fieldName);
               gti.ShowDialog();
               m_configParameters.Configs["appSettings"].Set(fieldName,gti.getFieldValue());
               gti.Dispose();
            }
        }

        private void loadEventPageMapping()
        {
            PairsEventName.Items.Clear();
            string[] events = m_configParameters.Configs["EventPageMapping"].GetKeys();
            for (int i = 0; i < events.Length; ++i)
            {
                PairsEventName.Items.Add(events[i]);
            }
            PairsEventName.SelectedIndex = 0;
            updateGooglePage();
        }

        private void updateGooglePage()
        {
            GooglePage.Text = "https://sites.google.com/site/" + m_configParameters.Configs["appSettings"].Get("GoogleSiteName") + m_configParameters.Configs["EventPageMapping"].Get("" + PairsEventName.SelectedItem);
        }

        private string removeTrailingSlash(string text)
        {
            return text.TrimEnd('/');
        }

        private void enableAll(bool enable)
        {
            SelectSummaryFileButton.Enabled = enable;
            Summary.Enabled = enable;
            PairsEventName.Enabled = enable;

        }

        private void SelectSummaryFileButton_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectSummaryFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string text = File.ReadAllText(SelectSummaryFileDialog.FileName);
                    Summary.Text = text;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Exception when trying to read Summary file : " + exception.Message);
                }
            }
        }

        private void PairsEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGooglePage();
        }

        private void LoadSummaryButton_Click(object sender, EventArgs e)
        {
            enableAll(false);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(ProgressReport);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                PairsEventInformation eventInformation = PairsGeneral.getEventInformation_(Utilities.compressText_(Summary.Text));
                if (!eventInformation.isACBLSummary) throw new Exception("The provide summary cannot be parsed as an ACBL Summary. Only ACBL Summaries can be uploaded using this application.");
                string eventName = Utilities.makeIdentifier_("" + PairsEventName.SelectedItem);
                eventInformation.databaseFileName = Path.Combine(Utilities.m_rootDirectory, "Databases", eventName + ".mdb");
                eventInformation.webpagesDirectory = Path.Combine(Utilities.m_rootDirectory, "Webpages", eventName);
                PairsDatabaseParameters databaseParameters = PairsGeneral.createDefaultDatabaseParameters();
                ProgressReport.Clear();
                if (!Steps.GetItemChecked(0))
                {
                    PairsSummaryToDatabase std = new PairsSummaryToDatabase(eventInformation);
                    std.loadSummaryIntoDatabase();
                    databaseParameters = std.getDatabaseParameters();
                    Steps.SetItemChecked(0, true);
                    Steps.Refresh();
                }
                else
                {
                    databaseParameters = PairsGeneral.createDefaultDatabaseParameters();
                    PairsGeneral.loadPairsDatabaseInformation(eventInformation.databaseFileName, out databaseParameters);
                }
                ProgressReport.Clear();
                if (!Steps.GetItemChecked(1))
                {
                    PairsDatabaseToWebpages dtw = new PairsDatabaseToWebpages(eventInformation, databaseParameters);
                    dtw.createWebpages_();
                    Steps.SetItemChecked(1, true);
                    Steps.Refresh();
                }
                ProgressReport.Clear();
                if (!Steps.GetItemChecked(2))
                {
                    String sitename = m_configParameters.Configs["appSettings"].Get("GoogleSiteName");
                    String username = "indianbridge.dummy@gmail.com";
                    String password = "kibitzer";
                    SitesAPI sa = new SitesAPI(sitename: sitename, username: username, password: password, replaceLinks: true, logHTTPTraffic: false);
                    sa.uploadDirectory(eventInformation.webpagesDirectory, m_configParameters.Configs["EventPageMapping"].Get("" + PairsEventName.SelectedItem));
                    Steps.SetItemChecked(2, true);
                    Steps.Refresh();
                }
                MessageBox.Show("Webpages created at " + GooglePage.Text);
                ProgressReport.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message + " was thrown. Please check your files and run again!!!");
            }
            Trace.Listeners.Remove(_textBoxListener);
            enableAll(true);
        }

        private void Summary_TextChanged(object sender, EventArgs e)
        {
            Steps.SetItemChecked(0, false);
            Steps.SetItemChecked(1, false);
            Steps.SetItemChecked(2, false);
        }
    }
}
