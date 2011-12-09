using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using IndianBridge.Common;

namespace IndianBridge.ResultsManager
{

    public class TeamsScorer
    {
        //private TeamsEventInformation m_eventInformation;
        //private TeamsDatabaseParameters m_databaseParameters;

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

        public void doRanking_(TeamsDatabaseParameters dp, int roundNumber)
        {
            DataTable table = dp.m_ds.Tables["Names_"];
            string totalColumnName = "Total";
            string rankColumnName = "Rank";
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow dRow = table.Rows[i];
                DataTable table2 = dp.m_ds.Tables["Scores_"];
                string filterExpression1 = "Team_1_Number = " + dRow["Team_Number"]+" AND Round_Number <= "+roundNumber;
                string filterExpression2 = "Team_2_Number = " + dRow["Team_Number"] + " AND Round_Number <= " + roundNumber;
                double sum = Utilities.getDoubleValue("" + table2.Compute("Sum(Team_1_VPs)+Sum(Team_1_Adjustment)", filterExpression1)) +
                    Utilities.getDoubleValue("" + table2.Compute("Sum(Team_2_VPs)+Sum(Team_2_Adjustment)", filterExpression2));
                dRow[totalColumnName] = sum;
            }
            string filterExpression = "";
            String sortCriteria = "Total DESC";
            DataRow[] foundRows = table.Select(filterExpression, sortCriteria);
            int rank = 1;
            double previousValue = 0;
            for (int i = 0; i < foundRows.Length; ++i)
            {
                DataRow dRow = foundRows[i];
                double currentValue = Utilities.getDoubleValue(""+dRow["Total"]);
                if (i > 0 && currentValue != previousValue) rank++;
                previousValue = currentValue;
                dRow[rankColumnName] = rank;
            }
            dp.m_daNames.Update(dp.m_ds, "Names_");
        }*/



    }
}
