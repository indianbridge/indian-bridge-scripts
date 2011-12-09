

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.ResultsManager;

namespace TeamsScorer
{
    public partial class MainWindow : Form
    {
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
            System.IO.File.Copy(sourceFileName, destinationFileName);
            rd.loadAccessDatabase(destinationFileName);
            rd.loadDataFromGoogleSpreadsheet("Winter Nationals 2011 RUIA Gold Pre-Quarters",
                new List<string>() { "Info","Names","Scores"});
            Trace.Listeners.Remove(_textBoxListener);
            /*string spreadsheetName = "Winter Nationals 2011 RUIA Gold Pre-Quarters";
            TextBoxTraceListener _textBoxListener = new TextBoxTraceListener(Status);
            Trace.Listeners.Add(_textBoxListener);
            SpreadSheetAPI ssa = new SpreadSheetAPI(username:username,password:password,spreadsheetname:spreadsheetName);
            TeamsDatabaseParameters db;
            TeamsGeneral.loadTeamsDatabaseInformation(destinationFileName,out db);
            Trace.Listeners.Remove(_textBoxListener);*/
            MessageBox.Show("Finished");
        }

        private void LoadDatabaseButton_Click(object sender, EventArgs e)
        {
            /*if (LoadDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                TeamsDatabaseParameters db;
                TeamsGeneral.loadTeamsDatabaseInformation(LoadDatabaseFileDialog.FileName, out db);
                TeamsGeneral.doRanking_(db, 2);
                MessageBox.Show("Finished");

            }*/
        }

    }
}
