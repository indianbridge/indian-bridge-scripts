using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IndianBridge.Common;
using System.Diagnostics;

namespace IndianBridgeScorer
{
    class SwissScoringAndRanking
    {
        private SwissTeamEventInfo m_swissTeamEventInfo;
        private SwissTeamScoringParameters m_swissTeamScoringParameters;
        private SwissTeamScoringProgressParameters m_swissTeamScoringProgressParameters;
        private string m_databaseFileName;
        DataTable m_namesTable;
        DataTable m_scoresTable;
        DataTable m_computedScoresTable;
        public SwissScoringAndRanking(string databaseFileName, SwissTeamEventInfo swissTeamEventInfo,
            SwissTeamScoringParameters swissTeamScoringParameters, SwissTeamScoringProgressParameters swissTeamScoringProgressParameters)
        {
            m_databaseFileName = databaseFileName;
            m_swissTeamEventInfo = swissTeamEventInfo;
            m_swissTeamScoringParameters = swissTeamScoringParameters;
            m_swissTeamScoringProgressParameters = swissTeamScoringProgressParameters;
            m_namesTable = getTable(Constants.TableName.EventNames);
            m_scoresTable = getTable(Constants.TableName.EventScores);
            m_computedScoresTable = getTable(Constants.TableName.EventComputedScores);
        }

        private DataTable getTable(string tableName)
        {
            return AccessDatabaseUtilities.getDataTable(m_databaseFileName, tableName);
        }

        public void recalculateScoresAndRanks(int roundsScored)
        {
            int roundsCompleted = m_swissTeamScoringProgressParameters.RoundsCompleted;
            if (roundsCompleted >= roundsScored)
            {
                for (int i = roundsScored; i <= roundsCompleted; ++i)
                {
                    doScoring(i);
                    doTieBreaker(i);
                    doRanking(i);
                }

                copyTotalAndRank(roundsCompleted);
                saveTable(Constants.TableName.EventComputedScores);
                m_swissTeamScoringProgressParameters.RoundsScored = roundsCompleted;
            }
        }

        private void saveTable(string tableName)
        {
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, tableName);
        }

        private void copyTotalAndRank(int roundNumber)
        {
            foreach (DataRow dRow in m_namesTable.Rows)
            {
                int teamNumber = getIntValue(dRow, "Team_Number");
                DataRow foundRow = m_computedScoresTable.Rows.Find(teamNumber);
                Debug.Assert(foundRow != null, "SwissScoringAndRanking.CopyTotalAndRank: Cannot find row for team : " + teamNumber + " " + Constants.TableName.EventComputedScores + " table.");
                dRow["Total_Score"] = getDoubleValue(foundRow, "Score_After_Round_" + roundNumber);
                dRow["Tiebreaker_Score"] = getDoubleValue(foundRow, "Tiebreaker_After_Round_" + roundNumber);
                dRow["Rank"] = getIntValue(foundRow, "Rank_After_Round_" + roundNumber);
            }
            saveTable(Constants.TableName.EventNames);
        }

        private int getIntValue(DataRow dRow, string columnName)
        {
            return AccessDatabaseUtilities.getIntValue(dRow, columnName);
        }

        private double getDoubleValue(DataRow dRow, string columnName)
        {
            return AccessDatabaseUtilities.getDoubleValue(dRow, columnName);
        }

        private void doScoring(int roundNumber)
        {
            DataRow[] dRows = m_scoresTable.Select("Round_Number = " + roundNumber);
            foreach (DataRow dRow in dRows)
            {
                int team1Number = getIntValue(dRow, "Team_1_Number"); ;
                int team2Number = getIntValue(dRow, "Team_2_Number");
                double team1VPs = getDoubleValue(dRow, "Team_1_VPs");
                double team2VPs = getDoubleValue(dRow, "Team_2_VPs");
                double team1Adjustment = getDoubleValue(dRow, "Team_1_VP_Adjustment");
                double team2Adjustment = getDoubleValue(dRow, "Team_2_VP_Adjustment");
                if (team1Number > 0)
                {
                    DataRow dNamesRow = m_namesTable.Rows.Find(team1Number);
                    Debug.Assert(dNamesRow != null, "Row for team number " + team1Number + " was not found in names scores table");
                    DataRow dComputedRow = m_computedScoresTable.Rows.Find(team1Number);
                    Debug.Assert(dComputedRow != null, "Row for team number " + team1Number + " was not found in computed scores table");
                    double previousScore = (roundNumber == 1) ? getDoubleValue(dNamesRow, "Carryover") : getDoubleValue(dComputedRow, "Score_After_Round_" + (roundNumber - 1));
                    dComputedRow["Score_After_Round_" + roundNumber] = previousScore + team1VPs + team1Adjustment;
                }
                if (team2Number > 0)
                {
                    DataRow dNamesRow = m_namesTable.Rows.Find(team2Number);
                    Debug.Assert(dNamesRow != null, "Row for team number " + team2Number + " was not found in names scores table");
                    DataRow dComputedRow = m_computedScoresTable.Rows.Find(team2Number);
                    Debug.Assert(dComputedRow != null, "Row for team number " + team2Number + " was not found in computed scores table");
                    double previousScore = (roundNumber == 1) ? getDoubleValue(dNamesRow, "Carryover") : getDoubleValue(dComputedRow, "Score_After_Round_" + (roundNumber - 1));
                    dComputedRow["Score_After_Round_" + roundNumber] = previousScore + team2VPs + team2Adjustment;
                }
            }
        }

        private void doTieBreaker(int roundNumber)
        {
            foreach (DataRow dRow in m_namesTable.Rows)
            {
                int teamNumber = getIntValue(dRow, "Team_Number");
                DataRow dComputedRow = m_computedScoresTable.Rows.Find(teamNumber);
                Debug.Assert(dComputedRow != null, "Row for team number " + teamNumber + " was not found in computed scores table");
                switch (m_swissTeamScoringParameters.TiebreakerMethod)
                {
                    case TiebreakerMethodValues.TeamNumber:
                        dComputedRow["Tiebreaker_After_Round_" + roundNumber] = -teamNumber;
                        break;
                    case TiebreakerMethodValues.Quotient:
                        calculateTieBreakerQuotient(dComputedRow, teamNumber, roundNumber);
                        break;
                    default:
                        throw new Exception("Unknown Tiebreaker method : " + m_swissTeamScoringParameters.TiebreakerMethod.ToString());
                }
            }
        }

        private void calculateTieBreakerQuotient(DataRow dComputedRow, int teamNumber, int roundNumber)
        {
            int count = 0;
            double tieBreakerScore = 0;
            DataRow[] dRows = m_scoresTable.Select("Round_Number <= " + roundNumber + " AND Team_1_Number = " + teamNumber);
            if (dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    int opponent = (int)dRow["Team_2_Number"];
                    if (opponent > 0)
                    {
                        double vps = getDoubleValue(dRow, "Team_1_VPs") + getDoubleValue(dRow, "Team_1_VP_Adjustment");
                        DataRow[] foundRows = m_computedScoresTable.Select("Team_Number = " + opponent);
                        Debug.Assert(foundRows.Length == 1);
                        double score = getDoubleValue(foundRows[0], "Score_After_Round_" + roundNumber);
                        tieBreakerScore += (score * vps);
                        count++;
                    }
                }
            }
            dRows = m_scoresTable.Select("Round_Number <= " + roundNumber + " AND Team_2_Number = " + teamNumber);

            if (dRows.Length > 0)
            {
                foreach (DataRow dRow in dRows)
                {
                    int opponent = (int)dRow["Team_1_Number"];
                    if (opponent > 0)
                    {
                        double vps = getDoubleValue(dRow, "Team_2_VPs") + getDoubleValue(dRow, "Team_2_VP_Adjustment");
                        DataRow[] foundRows = m_computedScoresTable.Select("Team_Number = " + opponent);
                        Debug.Assert(foundRows.Length == 1);
                        double score = getDoubleValue(foundRows[0], "Score_After_Round_" + roundNumber);
                        tieBreakerScore += (score * vps);
                        count++;
                    }
                }
            }
            dComputedRow["Tiebreaker_After_Round_" + roundNumber] = (count > 0) ? tieBreakerScore / count : 0;
        }

        private void doRanking(int roundNumber)
        {
            DataRow[] foundRows = m_computedScoresTable.Select("", "Score_After_Round_" + roundNumber + " DESC, Tiebreaker_After_Round_" + roundNumber + " DESC");
            int rank = 1;
            double previousValue = 0;
            double previousTiebreaker = 0;
            string rankColumnName = "Rank_After_Round_" + roundNumber;
            int count = 0;
            for (int i = 0; i < foundRows.Length; ++i)
            {
                DataRow dRow = foundRows[i];
                if (LocalUtilities.hasWithdrawn(m_databaseFileName, getIntValue(dRow, "Team_Number"), roundNumber))
                {
                    dRow[rankColumnName] = -1;
                }
                else
                {
                    double currentValue = getDoubleValue(dRow, "Score_After_Round_" + roundNumber);
                    double currentTiebreaker = getDoubleValue(dRow, "Tiebreaker_After_Round_" + roundNumber);
                    if (count > 0 && (currentValue != previousValue || currentTiebreaker != previousTiebreaker)) rank = count + 1;
                    previousValue = currentValue;
                    previousTiebreaker = currentTiebreaker;
                    dRow[rankColumnName] = rank;
                    count++;
                }
            }
        }
    }
}
