using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndianBridge.Common;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace IndianBridgeScorer
{
    public class SwissDrawCreation
    {
        private SwissTeamEventInfo m_swissTeamEventInfo;
        private SwissTeamScoringParameters m_swissTeamScoringParameters;
        private string m_databaseFileName;
        private int m_numberOfTeamsLeft;
        private int m_numberOfTeamsOriginal;
        private int m_numberOfMatchesLeft;
        private int m_numberOfMatchesOriginal;
        private int m_drawRoundNumber;
        private int m_scoreRoundNumber;
        private DataRow[] m_dataRows;
        private int[] m_teamNumber;
        private bool[] m_assigned;
        DataTable m_namesTable;
        DataTable m_scoresTable;
        DataTable m_computedScoresTable;

        private DataTable getTable(string tableName)
        {
            return AccessDatabaseUtilities.getDataTable(m_databaseFileName, tableName);
        }

        public SwissDrawCreation(string databaseFileName, SwissTeamEventInfo swissTeamEventInfo,
            SwissTeamScoringParameters swissTeamScoringParameters)
        {
            m_databaseFileName = databaseFileName;
            m_swissTeamEventInfo = swissTeamEventInfo;
            m_swissTeamScoringParameters = swissTeamScoringParameters;
            m_namesTable = getTable(Constants.TableName.EventNames);
            m_scoresTable = getTable(Constants.TableName.EventScores);
            m_computedScoresTable = getTable(Constants.TableName.EventComputedScores);
        }

        public void createRandomDraw(int drawRoundNumber)
        {
            m_drawRoundNumber = drawRoundNumber;
            m_scoreRoundNumber = 0;
            getTeamsNumberWise();
            createDraw();
        }

        private void getTeamsNumberWise()
        {
            string filterExpression = "Withdraw_Round is null OR WithDraw_Round > " + m_drawRoundNumber;
            string sort = "Team_Number ASC";
            DataRow[] dRows = m_namesTable.Select(filterExpression, sort);
            m_teamNumber = new int[dRows.Length + 1];
            m_teamNumber[0] = 0;
            m_assigned = new bool[dRows.Length + 1];
            int count = 1;
            foreach (DataRow dRow in dRows)
            {
                m_teamNumber[count] = AccessDatabaseUtilities.getIntValue(dRow, "Team_Number");
                m_assigned[count] = false;
                count++;
            }
        }

        private int getIntValue(DataRow dRow, string columnName)
        {
            return AccessDatabaseUtilities.getIntValue(dRow, columnName);
        }

        public string checkDrawForErrors(int drawRoundNumber)
        {
            m_drawRoundNumber = drawRoundNumber;
            string filter = "Round_Number = " + m_drawRoundNumber;
            string sort = "Table_Number ASC";
            m_dataRows = m_scoresTable.Select(filter, sort);
            string message = "";
            Object value;
            foreach (DataRow dRow in m_dataRows)
            {
                value = dRow["Team_1_Number"];
                if (value == DBNull.Value)
                {
                    Utilities.appendToMessage(ref message, "Team 1 Number at Table Number " + getIntValue(dRow, "Table_Number") + " is empty");
                }
                else
                {
                    int team1Number = getIntValue(dRow, "Team_1_Number");
                    if (team1Number >= 0)
                    {
                        if (team1Number != 0)
                        {
                            DataRow foundRow = m_namesTable.Rows.Find(team1Number);
                            if (foundRow == null) Utilities.appendToMessage(ref message, "Team 1 Number at Table Number " + getIntValue(dRow, "Table_Number") + " does not match any team numbers specified in the Names Database");
                        }
                        else if (m_numberOfTeamsLeft % 2 == 0)
                        {
                            Utilities.appendToMessage(ref message, "Bye team number (0) has been specified as Team 1 at Table Number " + getIntValue(dRow, "Table_Number") + " even though the number of teams left are an even number.");
                        }
                    }
                }
                value = dRow["Team_2_Number"];
                if (value == DBNull.Value)
                {
                    Utilities.appendToMessage(ref message, "Team 2 Number at Table Number " + getIntValue(dRow, "Table_Number") + " is empty");
                }
                else
                {
                    int team2Number = (int)dRow["Team_2_Number"];
                    if (team2Number >= 0)
                    {
                        if (team2Number != 0)
                        {
                            DataRow foundRow = m_namesTable.Rows.Find(team2Number);
                            if (foundRow == null) Utilities.appendToMessage(ref message, "Team 2 Number at Table Number " + getIntValue(dRow, "Table_Number") + " does not match any team numbers specified in the Names Database");
                        }
                        else if (m_numberOfTeamsLeft % 2 == 0)
                        {
                            Utilities.appendToMessage(ref message, "Bye team number (0) has been specified as Team 2 at Table Number " + getIntValue(dRow, "Table_Number") + " even though the number of teams left are an even number.");
                        }
                    }
                }
            }
            return message;
        }
        private int doesMatchExist(int currentRoundNumber, int team1Number, int team2Number)
        {
            DataRow[] dRows = m_scoresTable.Select("Round_Number < " + currentRoundNumber + " AND Team_1_Number = " + team1Number + " AND Team_2_Number = " + team2Number);
            if (dRows.Length > 0) return (int)dRows[0]["Round_Number"];
            return -1;
        }

        struct Occurences
        {
            public int count;
            public List<int> team1Occurence;
            public List<int> team2Occurence;
        };

        public string checkDrawForWarnings(int drawRoundNumber)
        {
            m_drawRoundNumber = drawRoundNumber;
            string message = "";
            Dictionary<int, Occurences> occurences = new Dictionary<int, Occurences>();
            Occurences byeOccurence;
            byeOccurence.count = 0;
            byeOccurence.team1Occurence = new List<int>();
            byeOccurence.team2Occurence = new List<int>();
            occurences.Add(0, byeOccurence);
            foreach (DataRow dRow in m_namesTable.Rows)
            {
                int teamNumber = getIntValue(dRow, "Team_Number");
                Occurences occurence;
                occurence.count = 0;
                occurence.team1Occurence = new List<int>();
                occurence.team2Occurence = new List<int>();
                occurences.Add(teamNumber, occurence);
            }
            string filterExpression = "Round_Number = " + m_drawRoundNumber;
            string sort = "Table_Number ASC";
            m_dataRows = m_scoresTable.Select(filterExpression, sort);
            foreach (DataRow dRow in m_dataRows)
            {
                int team1Number = getIntValue(dRow, "Team_1_Number");
                int team2Number = getIntValue(dRow, "Team_2_Number");
                int tableNumber = getIntValue(dRow, "Table_Number");
                if (team1Number != -1 && team2Number != -1)
                {
                    int previousRound = doesMatchExist(m_drawRoundNumber, team1Number, team2Number);
                    if (previousRound != -1)
                    {
                        message += Environment.NewLine + team1Number + " and " + team2Number + " have already played in round " + previousRound + " and are matched in this round at Table Number " + tableNumber;
                    }
                    if (!occurences.ContainsKey(team1Number))
                    {
                        message += Environment.NewLine + "Team 1 : " + team1Number + " at Table Number " + tableNumber + " is not a valid team number";
                    }
                    else
                    {
                        Occurences value = occurences[team1Number];
                        value.count++;
                        value.team1Occurence.Add(tableNumber);
                        occurences[team1Number] = value;
                    }
                    if (!occurences.ContainsKey(team2Number))
                    {
                        message += Environment.NewLine + "Team 2 : " + team2Number + " at Table Number " + tableNumber + " is not a valid team number";
                    }
                    else
                    {
                        Occurences value = occurences[team2Number];
                        value.count++;
                        value.team2Occurence.Add(tableNumber);
                        occurences[team2Number] = value;
                    }
                }
            }
            filterExpression = "Withdraw_Round is null OR WithDraw_Round > " + drawRoundNumber;
            sort = "Team_Number ASC";
            foreach (DataRow dRow in m_namesTable.Select(filterExpression, sort))
            {
                int i = getIntValue(dRow, "Team_Number");
                if (occurences[i].count == 0)
                {
                    message += Environment.NewLine + "Team Number " + i + " is not included in the draw.";
                }
                else if (occurences[i].count > 1)
                {
                    message += Environment.NewLine + "Team Number " + i + " appears more than once in draw (as Team 1 in Rows : ";
                    foreach (int number in occurences[i].team1Occurence)
                    {
                        message += "" + (number) + " ";
                    }
                    message += " as Team 2 in Rows : ";
                    foreach (int number in occurences[i].team2Occurence)
                    {
                        message += "" + (number) + " ";
                    }
                    message += ")";
                }
            }
            if (m_numberOfTeamsLeft % 2 != 0)
            {
                int i = 0;
                if (occurences[i].count == 0)
                {
                    message += Environment.NewLine + "No team has a bye even though there are an odd number of teams left.";
                }
                else if (occurences[i].count > 1)
                {
                    message += Environment.NewLine + "Bye has been specified more than once in draw (as Team 1 in Rows : ";
                    foreach (int number in occurences[i].team1Occurence)
                    {
                        message += "" + (number) + " ";
                    }
                    message += " as Team 2 in Rows : ";
                    foreach (int number in occurences[i].team2Occurence)
                    {
                        message += "" + (number) + " ";
                    }
                    message += ")";
                }
            }
            else
            {
                int i = 0;
                if (occurences[i].count > 0)
                {
                    message += Environment.NewLine + "Bye has been specified even though there are even number of teams left. Bye is specified as (as Team 1 in Rows : ";
                    foreach (int number in occurences[i].team1Occurence)
                    {
                        message += "" + (number) + " ";
                    }
                    message += " as Team 2 in Rows : ";
                    foreach (int number in occurences[i].team2Occurence)
                    {
                        message += "" + (number) + " ";
                    }
                    message += ")";
                }

            }
            return message;
        }

        private void getTeamsScoreWise()
        {
            string sort = "Rank_After_Round_" + m_scoreRoundNumber + " ASC, Tiebreaker_After_Round_" + m_scoreRoundNumber + " DESC";
            DataRow[] dRows = m_computedScoresTable.Select("", sort);
            int count = 0;
            foreach (DataRow dRow in dRows)
            {
                int team = AccessDatabaseUtilities.getIntValue(dRow, "Team_Number");
                if (!LocalUtilities.hasWithdrawn(m_databaseFileName, team, m_drawRoundNumber))
                {
                    count++;
                }
            }
            m_teamNumber = new int[count + 1];
            m_teamNumber[0] = 0;
            m_assigned = new bool[count + 1];
            count = 1;
            foreach (DataRow dRow in dRows)
            {
                int team = AccessDatabaseUtilities.getIntValue(dRow, "Team_Number");
                if (!LocalUtilities.hasWithdrawn(m_databaseFileName, team, m_drawRoundNumber))
                {
                    m_teamNumber[count] = team;
                    m_assigned[count] = false;
                    count++;
                }
            }
        }

        public void createDrawBasedOnScore(int drawRoundNumber, int scoreRoundNumber)
        {
            m_drawRoundNumber = drawRoundNumber;
            m_scoreRoundNumber = scoreRoundNumber;
            getTeamsScoreWise();
            createDraw();
        }

        private void createDraw()
        {
            m_numberOfTeamsLeft = LocalUtilities.teamsLeft(m_databaseFileName, m_drawRoundNumber);
            m_numberOfTeamsOriginal = m_swissTeamEventInfo.NumberOfTeams;
            m_numberOfMatchesLeft = (m_numberOfTeamsLeft / 2) + m_numberOfTeamsLeft % 2;
            m_numberOfMatchesOriginal = (m_numberOfTeamsOriginal / 2) + m_numberOfTeamsOriginal % 2;
            string filter = "Round_Number = " + m_drawRoundNumber;
            string sort = "Table_Number ASC";
            m_dataRows = m_scoresTable.Select(filter, sort);
            Debug.Assert(m_dataRows.Length == m_numberOfMatchesOriginal, "Number of matches for round number " + m_drawRoundNumber + " is " + m_dataRows.Length + " but it should be " + m_numberOfMatchesOriginal);
            assignLeftTeams();
            assignWithDrawnTeams();
        }

        private void assignLeftTeams()
        {
            if (!createMatch(m_drawRoundNumber, 1, m_numberOfMatchesLeft, m_assigned, m_teamNumber))
            {
                MessageBox.Show("Unable to generate random draw for round : " + m_drawRoundNumber + Environment.NewLine + "Please generate by hand and enter directly.", "Random Draw Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.EventScores);
            }
        }

        private int findFirstUnassigned(int startIndex, bool[] assigned)
        {
            for (int i = startIndex; i < assigned.Length; ++i)
            {
                if (!assigned[i]) return i;
            }
            if (m_numberOfTeamsLeft % 2 == 1 && !assigned[0]) return 0;
            return -1;
        }

        private int findOpponent(int drawRoundNumber, int index1, int startIndex, bool[] assigned, int[] teamNumber)
        {
            bool flag = true;
            int index2 = startIndex;
            int team1 = teamNumber[index1];
            while (flag)
            {
                if (index2 == 0) return -1;
                index2 = findFirstUnassigned(index2 + 1, assigned);
                if (index2 == -1) return -1;
                int team2 = teamNumber[index2];
                if (team1 == 0)
                {
                    DataRow[] dRows = m_scoresTable.Select("Round_Number < " + drawRoundNumber + " AND Team_1_Number = 0 AND Team_2_Number = " + (team2));
                    if (dRows.Length == 0) return index2;
                }
                else if (team2 == 0)
                {
                    DataRow[] dRows = m_scoresTable.Select("Round_Number < " + drawRoundNumber + " AND Team_1_Number = " + (team1) + " AND Team_2_Number = 0");
                    if (dRows.Length == 0) return index2;
                }
                else
                {
                    DataRow[] dRows = m_scoresTable.Select("Round_Number < " + drawRoundNumber + " AND Team_1_Number = " + (team1) + " AND Team_2_Number = " + (team2));
                    if (dRows.Length == 0) return index2;
                }
            }
            return -1;
        }

        private bool createMatch(int drawRoundNumber, int matchNumber, int numberOfMatches, bool[] assigned, int[] teamNumber)
        {
            if (matchNumber > numberOfMatches) return true;
            int index1 = findFirstUnassigned(1, assigned);
            Debug.Assert(index1 != -1, "For Match Number : " + matchNumber + " unable to find unassigned team");
            int team1 = teamNumber[index1];
            assigned[index1] = true;
            int index2 = index1;
            bool[] localAssigned = new bool[assigned.Length];
            Array.Copy(assigned, localAssigned, assigned.Length);
            while (true)
            {
                index2 = findOpponent(drawRoundNumber, index1, index2, assigned, teamNumber);
                if (index2 == -1)
                {
                    return false;
                }
                int team2 = teamNumber[index2];
                assigned[index2] = true;
                m_dataRows[matchNumber - 1]["Team_1_Number"] = team1;
                m_dataRows[matchNumber - 1]["Team_2_Number"] = team2;
                if (team1 == 0) m_dataRows[matchNumber - 1]["Team_2_VPs"] = m_swissTeamScoringParameters.ByeScore;
                if (team2 == 0) m_dataRows[matchNumber - 1]["Team_1_VPs"] = m_swissTeamScoringParameters.ByeScore;
                bool flag = createMatch(drawRoundNumber, matchNumber + 1, numberOfMatches, assigned, teamNumber);
                if (flag) return true;
                Array.Copy(localAssigned, assigned, assigned.Length);
            }
        }

        private void assignWithDrawnTeams()
        {
            for (int i = m_numberOfMatchesLeft; i < m_numberOfMatchesOriginal; ++i)
            {
                m_dataRows[i]["Team_1_Number"] = -1;
                m_dataRows[i]["Team_2_Number"] = -1;
            }
        }
    }
}
