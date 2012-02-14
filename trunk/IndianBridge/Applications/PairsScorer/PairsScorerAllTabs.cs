using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.ResultsManager;
using IndianBridge.GoogleAPIs;
using System.Collections.Generic;
using System.Data;

namespace IndianBridge.Applications
{


    public partial class PairsScorerAllTabs : Form
    {


        private PairsEventInformation m_eventInformation = PairsGeneral.createDefaultEventInformation();
        private PairsDatabaseParameters m_databaseParameters = PairsGeneral.createDefaultDatabaseParameters();

        public PairsScorerAllTabs()
        {
            InitializeComponent();

            Globals.m_rootDirectory = Directory.GetCurrentDirectory();
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

        private void LoadSummaryButton_Click(object sender, EventArgs e)
        {
            String summaryText = Utilities.compressText_(Summary.Text);
            m_eventInformation = PairsGeneral.getEventInformation_(summaryText);
            m_eventInformation.databaseFileName = PairsGeneral.constructDatabaseFileName(Globals.m_rootDirectory, m_eventInformation.eventName, m_eventInformation.eventDate);
            m_eventInformation.webpagesDirectory = PairsGeneral.constructWebpagesDirectory(Globals.m_rootDirectory, m_eventInformation.eventName, m_eventInformation.eventDate);
            EventInformationDisplay eventInformationDisplayDialog = new EventInformationDisplay(m_eventInformation);
            eventInformationDisplayDialog.ShowDialog();
            m_eventInformation = eventInformationDisplayDialog.m_eventInformation;
            eventInformationDisplayDialog.Dispose();
            createDatabaseTab();
        }

        private void createDatabaseTab()
        {
            CD_DatabaseFileName.Text = m_eventInformation.databaseFileName;
            ControlTabs.SelectTab("CreateDatabaseTab");
        }



        private void CreateDatabaseButton_Click(object sender, EventArgs e)
        {
            m_eventInformation.databaseFileName = CD_DatabaseFileName.Text;
            if (System.IO.File.Exists(m_eventInformation.databaseFileName))
            {
                DialogResult result = MessageBox.Show(m_eventInformation.databaseFileName + " exists!!!\nShould the contents be overwritten?\nIf you would like to load an existing database then Click Cancel and Select the Load Database Tab.", "Overwrite?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    loadSummaryIntoDatabase();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(m_eventInformation.databaseFileName + " does not exist!!!\nShould a new database be created?", "Create?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    loadSummaryIntoDatabase();
                }
            }
        }

        private void loadSummaryIntoDatabase()
        {
            PairsSummaryToDatabase std = new PairsSummaryToDatabase(m_eventInformation);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(CD_Status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                std.loadSummaryIntoDatabase();
                m_databaseParameters = std.getDatabaseParameters();
                Trace.Listeners.Remove(_textBoxListener);
                CW_RootFolder.Text = m_eventInformation.webpagesDirectory;
                ControlTabs.SelectTab("CreateWebpagesTab");
            }
            catch (Exception e)
            {
                Trace.Listeners.Remove(_textBoxListener);
                throw e;
            }
        }

        private void SelectDatabaseFileButton_Click(object sender, EventArgs e)
        {
            if (LoadDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadDatabaseFileName.Text = LoadDatabaseFileDialog.FileName;
            }
        }

        private void ChangeDatabaseButton_Click(object sender, EventArgs e)
        {
            if (DatabaseChangeDialog.ShowDialog() == DialogResult.OK)
            {
                CD_DatabaseFileName.Text = PairsGeneral.constructDatabaseFileName(DatabaseChangeDialog.SelectedPath, m_eventInformation.eventName, m_eventInformation.eventDate);
            }
        }

        private void LoadDatabaseButton_Click(object sender, EventArgs e)
        {
            m_eventInformation.databaseFileName = LoadDatabaseFileName.Text;
            if (!File.Exists(m_eventInformation.databaseFileName))
            {
                MessageBox.Show(m_eventInformation.databaseFileName + " does not exist!!!!");
                return;
            }
            PairsGeneral.loadPairsDatabaseInformation(m_eventInformation.databaseFileName, out m_databaseParameters);
            PairsGeneral.loadPairsEventInformation(m_databaseParameters, m_eventInformation.databaseFileName, out m_eventInformation);
            CW_RootFolder.Text = m_eventInformation.webpagesDirectory;
            ControlTabs.SelectTab("CreateWebpagesTab");
        }

        private void CW_ChangeButton_Click(object sender, EventArgs e)
        {
            if (DatabaseChangeDialog.ShowDialog() == DialogResult.OK)
            {
                CW_RootFolder.Text = PairsGeneral.constructWebpagesDirectory(DatabaseChangeDialog.SelectedPath, m_eventInformation.eventName, m_eventInformation.eventDate);
            }

        }

        private void CW_CreateButton_Click(object sender, EventArgs e)
        {
            m_eventInformation.webpagesDirectory = CW_RootFolder.Text;
            if (Directory.Exists(m_eventInformation.webpagesDirectory))
            {
                DialogResult result = MessageBox.Show(m_eventInformation.webpagesDirectory + " exists!!!\nShould the contents be overwritten?", "Overwrite?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    createWebpages();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(m_eventInformation.databaseFileName + " does not exist!!!\nShould a new folder be created?", "Create?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    Directory.CreateDirectory(m_eventInformation.webpagesDirectory);
                    createWebpages();
                }
            }
            UW_webpagesDirectory.Text = m_eventInformation.webpagesDirectory;
            ControlTabs.SelectTab("UploadWebpagesTab");
        }

        private void createWebpages()
        {
            PairsDatabaseToWebpages dtw = new PairsDatabaseToWebpages(m_eventInformation, m_databaseParameters);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(CW_Status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                dtw.createWebpages_();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.Listeners.Remove(_textBoxListener);
                throw e;
            }

        }

        private void UploadToSites_Click(object sender, EventArgs e)
        {
            String sitename = UW_SiteName.Text;
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(UW_Status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                SitesAPI sa = new SitesAPI(sitename: sitename, username: username, password: password, replaceLinks: true, logHTTPTraffic: false);
                sa.uploadDirectory(UW_webpagesDirectory.Text, UW_RootPath.Text);
                string rootPage = "https://sites.google.com/site/" + UW_SiteName + UW_RootPath;
                MessageBox.Show("Webpages created. See <a href='" + rootPage + "'>" + rootPage + "</a>");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Trace.Listeners.Remove(_textBoxListener);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(calendarGetEvents_Status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                CalendarAPI cApi = new CalendarAPI("indianbridge.dummy@gmail.com", "kibitzer");
                SortableBindingList<IndianCalendarEvent> test = cApi.getEvents(calendarGetEvents_startDate.Value, calendarGetEvents_endDate.Value, calendarGetEvents_SearchTextbox.Text);
                this.dataGridView1.DataSource = test;
                SpreadSheetAPI ssa = new SpreadSheetAPI("Pair Results City And Event Names", "indianbridge.dummy@gmail.com", "kibitzer");
                DataTable table = ssa.getValuesFromSheet("Sheet1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Trace.Listeners.Remove(_textBoxListener);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Selected = " + dataGridView1.CurrentRow.Index);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("Selected = " + dataGridView1.CurrentRow.Index);

        }

    }
}

