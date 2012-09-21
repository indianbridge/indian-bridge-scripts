using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using IndianBridge.Common;
using System.Diagnostics;
using System.Collections;
using System.Drawing.Printing;

namespace IndianBridgeScorer
{
    struct EventParameters
    {
        string m_eventName;
        int m_numberOfTeams;
        int m_numberOfBoards;
        int m_numberOfRounds;

        public EventParameters(string eventName, int numberOfTeams, int numberOfBoards, int numberOfRounds)
        {
            m_eventName = eventName;
            m_numberOfBoards = numberOfBoards;
            m_numberOfRounds = numberOfRounds;
            m_numberOfTeams = numberOfTeams;
        }
    }
    public partial class PDScorer : Form
    {
        private EventParameters m_eventParameters = new EventParameters("", 0, 0, 0);
        private string m_databaseFileName = "";
        private string m_localWebpagesRoot = "";
        private string m_resultsWebsiteRoot = "";





        /*public static string infoTableName = "Info";
        public static string namesTableName = "Teams";
        public static string scoresTableName = "Scores";
        public static string computedScoresTableName = "ComputedScores";

        public static int numberOfTeams = 0;
        public static int numberOfBoards = 0;
        public static int numberOfRounds;
        private double m_oldFontSize;*/


        public PDScorer(string eventName, string databaseFileName, string localWebpagesRoot, string resultsWebsiteRoot)
        {
            InitializeComponent();
            m_eventParameters = new EventParameters(eventName, 0, 0, 0);
            m_databaseFileName = databaseFileName;
            m_localWebpagesRoot = localWebpagesRoot;
            m_resultsWebsiteRoot = resultsWebsiteRoot;
            initialize();
        }

        private void createPDDatabase()
        {
            AccessDatabaseUtilities.createDatabase(m_databaseFileName);

        }

        private void initialize()
        {
            // Enable copy paste in all datagridview's
            foreach (DataGridView control in this.Controls.OfType<DataGridView>())
            {
                control.KeyDown += new KeyEventHandler(Utilities.handleCopyPaste);
            }

            // Check if database already exists
            if (File.Exists(m_databaseFileName))
            {
                // load database
            }
            else
            {
                //create database
            }
        }

    }
}
