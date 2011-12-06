using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.ResultsCreation;

namespace PairsResultsCreator
{
    public partial class EventInformationDisplay : Form
    {
        public EventInformation m_eventInformation;
        public EventInformationDisplay(EventInformation eventInformation)
        {
            InitializeComponent();
            m_eventInformation = eventInformation;
            EventName.Text = m_eventInformation.eventName;
            EventDatePicker.Value = m_eventInformation.eventDate;
            ACBLSummary.Text = (m_eventInformation.isACBLSummary ? "YES" : "NO");
            ScoringType.Text = (m_eventInformation.isIMP ? "IMP" : "MP");
            NumberOfDirections.Text = (m_eventInformation.hasDirectionField ? "1" : "2");
            DatabaseFileName.Text = m_eventInformation.databaseFileName;
            WebpagesDirectory.Text = m_eventInformation.webpagesDirectory;
        }

        private void UpdateEventInformationButton_Click(object sender, EventArgs e)
        {
            m_eventInformation.eventName = EventName.Text;
            m_eventInformation.eventDate = EventDatePicker.Value;
            m_eventInformation.databaseFileName = DatabaseFileName.Text;
            m_eventInformation.webpagesDirectory = WebpagesDirectory.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ChangeDatabaseButton_Click(object sender, EventArgs e)
        {
            if (SelectDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                DatabaseFileName.Text = SelectDatabaseFileDialog.FileName;
            }

        }

        private void WebpagesDirectoryChangeButton_Click(object sender, EventArgs e)
        {
            if (SelectWebpagesDirectoryDialog.ShowDialog() == DialogResult.OK)
            {
                WebpagesDirectory.Text = General.constructWebpagesDirectory(SelectWebpagesDirectoryDialog.SelectedPath, EventName.Text, EventDatePicker.Value);
            }
        }


    }
}
