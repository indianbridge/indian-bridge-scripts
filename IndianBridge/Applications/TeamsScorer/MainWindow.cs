

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.ResultsManager;

namespace IndianBridge.Applications
{
    public partial class MainWindow : Form
    {
        private String mDatabaseFileName;
        public MainWindow()
        {
            InitializeComponent();
            Globals.m_rootDirectory = Directory.GetCurrentDirectory();
        }



        private void RunButton_Click(object sender, EventArgs e)
        {
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(Status);
            Trace.Listeners.Add(_textBoxListener);
            ResultsDatabase rd = new ResultsDatabase(new List<string>() { "Info_", "Names_", "Scores_" });
            String sourceFileName = System.IO.Path.Combine(Globals.m_rootDirectory, "Databases", "TeamsScoreDatabaseTemplate.mdb");
            String destinationFileName = System.IO.Path.Combine(Globals.m_rootDirectory, "Databases", "Test" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".mdb");
            mDatabaseFileName = destinationFileName;
            System.IO.File.Copy(sourceFileName, destinationFileName);
            rd.loadAccessDatabase(destinationFileName);
            rd.loadDataFromGoogleSpreadsheet("Mohanlal Bhartiya Memorial Grand Prix 2012 Team of Four",
                new List<string>() { "Info","Names","Scores"});
            Trace.Listeners.Remove(_textBoxListener);
            MessageBox.Show("Finished");
        }

        private void LoadDatabaseButton_Click(object sender, EventArgs e)
        {
            if (LoadDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                /*IndianBridge.ResultsManager.TeamsScorer scorer = new IndianBridge.ResultsManager.TeamsScorer();
                scorer.loadTeamsDatabaseInformation(LoadDatabaseFileDialog.FileName);
                scorer.doRanking_(6);
                scorer.createLeaderboard();*/
                MessageBox.Show("Finished");

            }
        }

    }
}
