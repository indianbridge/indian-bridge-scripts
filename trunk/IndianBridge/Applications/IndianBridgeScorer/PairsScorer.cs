using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.ResultsManager;
using IndianBridge.Common;
using System.IO;

namespace IndianBridgeScorer
{
    public partial class PairsScorer : Form
    {
        private Boolean resultsLoaded = false, publishEnabled = false;
        private PairsEventInformation m_eventInformation = PairsGeneral.createDefaultEventInformation();
        TourneyInformationDatabase m_tid;
        string m_databaseFileName;
        string m_eventName;
        public PairsScorer(TourneyInformationDatabase tid, string eventName, string databaseFileName)
        {
            m_tid = tid;
            m_eventName = eventName;
            m_databaseFileName = databaseFileName;
            InitializeComponent();
            DataTable table = m_tid.m_ds.Tables[TourneyInformationDatabase.tourneyInfoTableName];
            DataRow dRow = table.Rows[0];
            string websiteRoot = (string)dRow["Tourney_Results_Website"];
            websiteAddress_textBox.Text = websiteRoot + "/" + Utilities.makeIdentifier_(eventName);
        }

        private void updateButtonStatus()
        {
            publishEnabled = resultsLoaded;
            if (!resultsLoaded)
            {
                this.selectedResultsFile_textBox.Text = "";
                this.eventInfo_textBox.Text = "";
            }
            this.loadResults.BackColor = resultsLoaded ? Color.Green : Color.Red;
            this.publishResults.BackColor = publishEnabled ? Color.Green : Color.Red;
            this.publishResults.Enabled = publishEnabled;
            this.Refresh();
        }

        private void loadResults_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectSummaryFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    String text = File.ReadAllText(SelectSummaryFileDialog.FileName);
                    String summaryText = Utilities.compressText_(text);
                    m_eventInformation = PairsGeneral.getEventInformation_(summaryText);
                    m_eventInformation.databaseFileName = m_databaseFileName;
                    m_eventInformation.webpagesDirectory = Path.Combine(Path.GetDirectoryName(m_databaseFileName), "Webpages",Utilities.makeIdentifier_(m_eventName));
                }
                catch (Exception exception)
                {
                    resultsLoaded = false;
                    updateButtonStatus();
                    MessageBox.Show("Exception when trying to read Summary file : " + exception.Message);
                }
                resultsLoaded = true;
                this.selectedResultsFile_textBox.Text = SelectSummaryFileDialog.FileName;
                this.eventInfo_textBox.Text = "Event Title : " + m_eventInformation.eventName + ", Event Date : " + m_eventInformation.eventDate.ToString("MMMM dd, yyyy");
                updateButtonStatus();
            }
            else
            {
                resultsLoaded = false;
                updateButtonStatus();
            }
        }

        private void publishResults_Click(object sender, EventArgs e)
        {
            CreateAndPublishPairsResults publishResults = new CreateAndPublishPairsResults(m_eventInformation, websiteAddress_textBox.Text);
            publishResults.ShowDialog(this);
            publishResults.Dispose();
        }
    }
}
