using System;
using System.Data;
using System.Data.OleDb;
using IndianBridge.Common;

namespace IndianBridge.ResultsManager
{

    public struct TeamsEventInformation
    {
        public String eventName;
        public int numberOfTeams;
        public int numberOfRounds;
        public int numberOfBoards;
        public int numberOfQualifiers;
        public String databaseFileName;
        public String webpagesDirectory;
        public String googleSitesRoot;
    }
    public struct TeamsDatabaseParameters
    {
        public DataSet m_ds;
        public System.Data.OleDb.OleDbDataAdapter m_daEventInformation;
        public OleDbCommandBuilder m_cbEventInformation;
        public System.Data.OleDb.OleDbDataAdapter m_daNames;
        public OleDbCommandBuilder m_cbNames;
        public System.Data.OleDb.OleDbDataAdapter m_daScores;
        public OleDbCommandBuilder m_cbScores;
    }


    public class TeamsScorer
    {
        public static String INFO_SHEET_NAME = "Info_";
        public static String NAMES_SHEET_NAME = "Names_";
        public static String SCORE_SHEET_NAME = "Scores_";

        //private TeamsEventInformation m_eventInformation;
        private TeamsDatabaseParameters m_databaseParameters;

        public static TeamsDatabaseParameters createDefaultDatabaseParameters()
        {
            TeamsDatabaseParameters dp = new TeamsDatabaseParameters();
            dp.m_ds = new DataSet();
            dp.m_daEventInformation = null;
            dp.m_cbEventInformation = null;
            dp.m_daNames = null;
            dp.m_cbNames = null;
            dp.m_daScores = null;
            dp.m_cbScores = null;
            return dp;
        }

        public static TeamsEventInformation createDefaultEventInformation()
        {
            TeamsEventInformation eventInformation = new TeamsEventInformation();
            eventInformation.eventName = "No Name";
            eventInformation.databaseFileName = "";
            eventInformation.googleSitesRoot = "";
            eventInformation.numberOfBoards = 0;
            eventInformation.numberOfQualifiers = 0;
            eventInformation.numberOfRounds = 0;
            eventInformation.numberOfTeams = 0;
            eventInformation.webpagesDirectory = "";
            return eventInformation;
        }

        public void createLeaderboard()
        {
            IndianBridge.Common.WebpageCreationUtilities.Parameters parameters = WebpageCreationUtilities.createDefaultParameters();
            parameters.columns.Add("Rank", "{Rank}");
            parameters.columns.Add("Team Name", "{Team_Name}");
            parameters.columns.Add("Total", "{Total}");
            parameters.sectionName = null;
            parameters.sortCriteria = "Rank ASC";
            parameters.fileName = @"C:\Temp\index.html";
            parameters.filterCriteria = "";
            parameters.headerTemplate = "Leaderboard<br/>[_commonPageHeader]";
            parameters.table = m_databaseParameters.m_ds.Tables["Names_"];
            WebpageCreationUtilities.createPage_(parameters); 
        }

        public void loadTeamsDatabaseInformation(String databaseFileName)
        {
            m_databaseParameters = createDefaultDatabaseParameters();
#if USINGPROJECTSYSTEM
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+filePath;
#else
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
#endif
            OleDbConnection m_myAccessConn = new OleDbConnection(strAccessConn);
            m_myAccessConn.Open();
            string sql = "SELECT * From " + INFO_SHEET_NAME;
            m_databaseParameters.m_daEventInformation = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            m_databaseParameters.m_cbEventInformation = new OleDbCommandBuilder(m_databaseParameters.m_daEventInformation);
            m_databaseParameters.m_daEventInformation.Fill(m_databaseParameters.m_ds, INFO_SHEET_NAME);
            sql = "SELECT * From " + NAMES_SHEET_NAME;
            m_databaseParameters.m_daNames = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            m_databaseParameters.m_cbNames = new OleDbCommandBuilder(m_databaseParameters.m_daNames);
            m_databaseParameters.m_daNames.Fill(m_databaseParameters.m_ds, NAMES_SHEET_NAME);
            DataTable table = m_databaseParameters.m_ds.Tables[NAMES_SHEET_NAME];
            table.PrimaryKey = new DataColumn[] { table.Columns["Team_Number"] };
            sql = "SELECT * From " + SCORE_SHEET_NAME;
            m_databaseParameters.m_daScores = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            m_databaseParameters.m_cbScores = new OleDbCommandBuilder(m_databaseParameters.m_daScores);
            m_databaseParameters.m_daScores.Fill(m_databaseParameters.m_ds, SCORE_SHEET_NAME);
            table = m_databaseParameters.m_ds.Tables[SCORE_SHEET_NAME];
            table.PrimaryKey = new DataColumn[] { table.Columns["Round_Number"], table.Columns["Table_Number"] };
            m_myAccessConn.Close();
            m_myAccessConn = null;
        }

        public void doRanking_(int roundNumber)
        {
            TeamsDatabaseParameters dp = m_databaseParameters;
            DataTable table = dp.m_ds.Tables["Names_"];
            string totalColumnName = "Total";
            string rankColumnName = "Rank";
            string penaltyColumnName = "Penalty_";
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow dRow = table.Rows[i];
                DataTable table2 = dp.m_ds.Tables["Scores_"];
                string filterExpression1 = "Team_1_Number = " + dRow["Team_Number"] + " AND Round_Number <= " + roundNumber;
                string filterExpression2 = "Team_2_Number = " + dRow["Team_Number"] + " AND Round_Number <= " + roundNumber;
                double sum = Utilities.getDoubleValue("" + table2.Compute("Sum(Team_1_VPs)+Sum(Team_1_Adjustment)", filterExpression1)) +
                    Utilities.getDoubleValue("" + table2.Compute("Sum(Team_2_VPs)+Sum(Team_2_Adjustment)", filterExpression2));
                dRow[totalColumnName] = sum - Utilities.getDoubleValue("" + dRow[penaltyColumnName]); ;
            }
            string filterExpression = "";
            String sortCriteria = "Total DESC";
            DataRow[] foundRows = table.Select(filterExpression, sortCriteria);
            int rank = 1;
            double previousValue = 0;
            for (int i = 0; i < foundRows.Length; ++i)
            {
                DataRow dRow = foundRows[i];
                double currentValue = Utilities.getDoubleValue("" + dRow["Total"]);
                if (i > 0 && currentValue != previousValue) rank = i+1;
                previousValue = currentValue;
                dRow[rankColumnName] = rank;
            }
            dp.m_daNames.Update(dp.m_ds, "Names_");
        }


        /* public TeamsScorer(string databaseFileName)
         {
             //m_eventInformation = createDefaultTeamsEventInformation();
             //m_databaseParameters = TeamsGeneral.loadTeamsDatabaseInformation(databaseFileName);
         }

         public int getCompletedRounds(TeamsDatabaseParameters dp)
         {
             int completedRounds = 0;
             DataTable table = dp.m_ds.Tables["Scores_"];
             return completedRounds;
         }

         public bool checkRound(TeamsDatabaseParameters dp, int roundNumber, out string message)
         {
             message = "";
             int numberOfTeams = int.Parse(""+dp.m_ds.Tables["Names_"].Compute("Count(Team_Number)", ""));
             DataTable table = dp.m_ds.Tables["Scores_"];
             string filterExpression = "Round_Number = " + roundNumber;
             DataRow[] foundRows = table.Select(filterExpression);
             return false;
         }

         public int alreadyPlayed(int team1,int team2,int currentRoundNumber) {
             int roundNumber = 0;
             //DataTable table = dp.m_ds.Tables["Scores_"];
             for (int i = 1; i < currentRoundNumber; ++i)
             {
                
             }
             return roundNumber;
         }

*/



    }
}
