using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using System.Diagnostics;
using IndianBridge.GoogleAPIs;

namespace IndianBridgeScorer
{
    public partial class CreateSwissTeamResults : Form
    {
        private string m_databaseFileName;
        private string m_localWebpagesRootDirectory = "";
        private string m_eventName = "";
        public string m_message = "";
        public bool m_success = false;

        public CreateSwissTeamResults(string databaseFileName, string webpagesRootDirectory, string eventName)
        {
            m_databaseFileName = databaseFileName;
            m_localWebpagesRootDirectory = webpagesRootDirectory;
            m_eventName = eventName;
            InitializeComponent();
            this.Text = "Create Local Results Webpages for " + m_eventName;
        }

        private void createWebpages()
        {
            currentOperationTitle.Text = "Creating Local webpages from results database";
            status.Text = "";
            status.Refresh();
            SwissTeamsDatabaseToWebpages tdw = new SwissTeamsDatabaseToWebpages(m_eventName,m_databaseFileName,m_localWebpagesRootDirectory);
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(status);
            Trace.Listeners.Add(_textBoxListener);
            try
            {
                tdw.createWebpages_();
                Trace.Listeners.Remove(_textBoxListener);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.Listeners.Remove(_textBoxListener);
                m_message = "Unable to load create local webpages from results database"+Environment.NewLine + e.Message;
                m_success = false ;
                return;
            }
            m_message = "Local Webpages created at " + m_localWebpagesRootDirectory;
            m_success = true;
        }

        private void PublishResults_Shown(object sender, EventArgs e)
        {
            createWebpages();
            this.Close();
        }
    }
}
