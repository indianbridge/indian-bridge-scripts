using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using IndianBridge.Common;
using IndianBridge.ResultsCreation;
using IndianBridge.Common.Logging;
using System.Diagnostics;
using IndianBridge.GoogleAPIs;
using Nini.Config;

namespace WinterNationals2011PairsResultsCreator
{
    public partial class WinterNationals2011PairsResultsCreator : Form
    {
        private IConfigSource m_configParameters;
        public WinterNationals2011PairsResultsCreator()
        {
            InitializeComponent();
            m_configParameters = new DotNetConfigSource(DotNetConfigSource.GetFullConfigPath());
            getFieldName("GoogleSiteName");
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
                EventInformation eventInformation = General.getEventInformation_(Utilities.compressText_(Summary.Text));
                if (!eventInformation.isACBLSummary) throw new Exception("The provide summary cannot be parsed as an ACBL Summary. Only ACBL Summaries can be uploaded using this application.");
                string eventName = General.makeIdentifier_(""+PairsEventName.SelectedItem);
                eventInformation.databaseFileName = Path.Combine(Directory.GetCurrentDirectory(), "Databases", eventName + ".mdb");
                eventInformation.webpagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Webpages", eventName);
                DatabaseParameters databaseParameters = General.createDefaultDatabaseParameters();
                ProgressReport.Clear();
                if (!Steps.GetItemChecked(0))
                {
                    SummaryToDatabase std = new SummaryToDatabase(eventInformation);
                    std.loadSummaryIntoDatabase();
                    databaseParameters = std.getDatabaseParameters();
                    Steps.SetItemChecked(0, true);
                }
                else
                {
                    databaseParameters = General.createDefaultDatabaseParameters();
                    General.loadDatabaseInformation(eventInformation.databaseFileName, out databaseParameters);
                }
                ProgressReport.Clear();
                if (!Steps.GetItemChecked(1))
                {
                    DatabaseToWebpages dtw = new DatabaseToWebpages(eventInformation, databaseParameters);
                    dtw.createWebpages_();
                    Steps.SetItemChecked(1, true);
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
