using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using IndianBridge.Common;

namespace BridgeMateRunningScores
{
    public class MagicInterface
    {
        #region Constants & Members
        const string pairHeadingOld = "<FONT face='Verdana, Arial, Helvetica' size=2><b><b>Pair</b></font>";
        const string pairHeadingNew = "<FONT face='Verdana, Arial, Helvetica' size=2><b><b>Pair<br>NS&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;EW</b></font>";
        const string TABLE_START_TAG = "<TABLE";
        const string TABLE_END_TAG = "</TABLE>";
        string m_inputFolder, m_runningScoreFileName, m_butlerFileName,m_runningScoreRoot;
        static DataTable m_completedBoards, m_butlerResults;
        static Dictionary<int, DateTime> m_lastModifiedTimes = new Dictionary<int, DateTime>();
        const string BOARD_RESULT_FONT_TEXT = "<FONT face='Verdana, Arial, Helvetica' size=2>";
        #endregion

        #region Properties

        public DataTable CompletedBoards
        {
            get
            {
                return m_completedBoards;
            }
        }

        public DataTable ButlerResults
        {
            get
            {
                return m_butlerResults;
            }
        }

        #endregion

        #region Public Methods

        public MagicInterface(string folderName, string runningScoreFileName, string butlerFileName, string runningScoreRoot)
        {
            m_inputFolder = folderName;
            m_runningScoreFileName = runningScoreFileName;
            m_butlerFileName = butlerFileName;
            m_runningScoreRoot = runningScoreRoot;
        }

        public NameValueCollection GetPairNames(int numberOfPairs)
        {
            NameValueCollection pairNames = new NameValueCollection();
            string rowText;

            // Get the butler table first
            string text = Utility.GetDataFromHtmlTable(String.Format(@"{0}\{1}.htm", m_inputFolder, m_butlerFileName), 0, true);

            int position, endOfRowPosition = 0;

            // Find the index of the "Count" column header
            endOfRowPosition = text.IndexOf("Name");
            position = text.IndexOf("\r\n\r\n", endOfRowPosition) + 4;
            int rank = 1;

            while (rank <= numberOfPairs)
            {
                // Find 
                endOfRowPosition = text.IndexOf("\r\n", position);

                rowText = text.Substring(position, endOfRowPosition - position);
                position = 0;
                string pairNumber = Utility.GetField(rowText, ref position);
                string team = Utility.GetField(rowText, ref position);
                string score = Utility.GetField(rowText, ref position);
                string player1 = Utility.GetField(rowText, ref position, " - ");
                position = rowText.IndexOf(" - ", position) + 3;
                string player2 = rowText.Substring(position).Trim();

                pairNames.Add(pairNumber, String.Format("{0} - {1} ({2})", player1, player2, team));
                rank++;
                position = endOfRowPosition + 2;
            }

            return pairNames;
        }

        public DataTable GetRunningScores(int numberOfMatchesPerRound, out int roundInProgress)
        {
            string rowText, result = String.Empty;
            int position, endOfRowPosition = 0, roundStartPosition, roundEndPosition, position1;
            bool success;
            string tableNumber, team1Number, team1Name, team2Number, team2Name, imp1Score, imp2Score, impScore, vpScore;
            string[] vpScores;

            // Prior to retrieving running scores for a round, initialize the Completed Boards and Butler Results datatables
            InitializeDataTables();

            string fileName = String.Format(@"{0}\{1}", m_inputFolder, m_runningScoreFileName);
            string text = Utility.GetDataFromHtmlTable(fileName, 0, true);
            string fileData = Utility.ReadFile(fileName, out success);

            // If we couldn't read the file, we quit
            if (!success)
            {
                roundInProgress = -1;
                return null;
            }

            roundStartPosition = fileData.IndexOf("<PRE>\r\nRound:") + 13;
            roundEndPosition = fileData.IndexOf("\r\n", roundStartPosition);

            roundInProgress = Convert.ToInt32(fileData.Substring(roundStartPosition, roundEndPosition - roundStartPosition).Trim());

            // Find the index of the "Team" column header
            endOfRowPosition = text.IndexOf("Score");
            int table = 1;

            DataTable currentScore = new DataTable();
            currentScore.Columns.Add("TableNumber", typeof(System.String));
            currentScore.Columns.Add("HomeTeamNumber", typeof(System.String));
            currentScore.Columns.Add("AwayTeamNumber", typeof(System.String));
            currentScore.Columns.Add("HomeTeam", typeof(System.String));
            currentScore.Columns.Add("AwayTeam", typeof(System.String));
            currentScore.Columns.Add("IMPScore", typeof(System.String));
            currentScore.Columns.Add("VPScore", typeof(System.String));

            while (table <= numberOfMatchesPerRound)
            {
                DataRow row = currentScore.NewRow();

                // Find the index of the "table" 1
                position = text.IndexOf(table.ToString(), endOfRowPosition);
                // If there's no result for this table move on to the next one
                if (position == -1)
                {
                    table++;
                    continue;
                }

                endOfRowPosition = text.IndexOf("\r\n", position);

                rowText = text.Substring(position, endOfRowPosition - position);

                position = 0;

                tableNumber = rowText.Substring(position, Utility.findWhiteSpace(rowText, position));
                team1Number = Utility.GetField(rowText, ref position);
                team1Name = Utility.GetField(rowText, ref position);
                team2Number = Utility.GetField(rowText, ref position);
                team2Name = Utility.GetField(rowText, ref position);
                imp1Score = Utility.GetField(rowText, ref position, "-");
                position = rowText.IndexOf("-", position) + 1;
                position1 = Utility.findWhiteSpace(rowText, position);
                imp2Score = rowText.Substring(position, position1 - position);
                impScore = String.Format("{0}-{1}", imp1Score, imp2Score);

                position = Utility.findWhiteSpace(rowText, position);

                // Skip past whitespace and find the score
                position += Utility.skipWhiteSpace(rowText, position);

                vpScore = rowText.Substring(position).Trim();
                vpScores = vpScore.Split(new char[] { ' ' });

                if (vpScores.Length > 1)
                {
                    vpScore = String.Format("{0}-{1}", vpScores[0].Trim(), vpScores[vpScores.Length - 1].Trim());
                }

                row["TableNumber"] = tableNumber;
                row["IMPScore"] = impScore;
                row["VPScore"] = vpScore;

                if (team1Number != "--")
                {
                    row["HomeTeam"] = Utility.ToCamelCase(team1Name);
                    row["HomeTeamNumber"] = team1Number;
                }
                else
                {
                    row["HomeTeam"] = String.Empty;
                    row["HomeTeamNumber"] = String.Empty;
                }

                if (team2Number != "--")
                {
                    row["AwayTeam"] = Utility.ToCamelCase(team2Name);
                    row["AwayTeamNumber"] = team2Number;
                }
                else
                {
                    row["AwayTeam"] = string.Empty;
                    row["AwayTeamNumber"] = String.Empty;
                }

                currentScore.Rows.Add(row);

                table++;
            }

            return currentScore;
        }

        public string GetBoardResults(int boardNumber, NameValueCollection pairNames, 
            int numberOfTables, out bool hasNewResults)
        {
            bool success; DateTime boardLastUpdatedTime;
            string fileName = String.Format(@"{0}\{1}-{2}.htm", m_inputFolder, 
                m_butlerFileName, boardNumber.ToString());

            hasNewResults = true;

            // Only create new board results if the corresponding magic file has been updated in the interim
            DateTime lastModifiedTime = File.GetLastWriteTime(fileName);
            if (m_lastModifiedTimes.ContainsKey(boardNumber))
            {
                boardLastUpdatedTime = m_lastModifiedTimes[boardNumber];
                if (lastModifiedTime.CompareTo(boardLastUpdatedTime) <= 0)
                {
                    hasNewResults = false;
                    return String.Empty;
                }
                else
                {
                    m_lastModifiedTimes[boardNumber] = lastModifiedTime;
                }
            }
            else
            {
                m_lastModifiedTimes.Add(boardNumber, lastModifiedTime);
            }

            string html = Utility.ReadFile(fileName, out success, true);
            string backLinkText = GetBackToRunningScoresLinktext();
            string navigationLinksText = GetNavigationLinksText(boardNumber, String.Empty);

            if (!success)
            {
                return String.Format(@"{2}<b>Page Updated {0}</b><br/><br/><b>No scores yet</b><br/><br/><br/>{1}", 
                    Utility.GetTimeStamp(), backLinkText, navigationLinksText, boardNumber.ToString());
            }

            int startPosition = html.IndexOf("<center>") + 8;
            int endPosition = html.IndexOf("</center>");
            string tableText = html.Substring(startPosition, endPosition - startPosition);

            string boardResultsTable;
            string imagesRootUrl = ConfigurationManager.AppSettings["ImagesRoot"];

            startPosition = tableText.LastIndexOf(TABLE_START_TAG, StringComparison.InvariantCultureIgnoreCase);
            endPosition = tableText.IndexOf(TABLE_END_TAG, startPosition, StringComparison.InvariantCultureIgnoreCase);

            boardResultsTable = tableText.Substring(startPosition, endPosition + 8 - startPosition);

            tableText = tableText.Replace(boardResultsTable, ReplacePairNumbersWithNames(boardNumber, boardResultsTable, pairNames, numberOfTables));
            //tableText = tableText.Replace("<td width=573 valign=top border=0>", "<td width=623 valign=top border=0>");
            //tableText = tableText.Replace("<table width=573 border=1 cellspacing=0 cellpadding=2 bordercolor=antiquewhite>",
            //    "<table width=623 border=1 cellspacing=0 cellpadding=2 bordercolor=antiquewhite>");
            //tableText = tableText.Replace("<td width=114 align=center", "<td width=164 align=center");
            //tableText = tableText.Replace("<table width=871 border=0>", "<table width=921 border=0>");
            tableText = tableText.Replace("src='clubs.gif'", String.Format("src={0}clubs-large.gif", imagesRootUrl));
            tableText = tableText.Replace("src='diamonds.gif'", String.Format("src={0}diamonds-large.gif", imagesRootUrl));
            tableText = tableText.Replace("src='hearts.gif'", String.Format("src={0}hearts-large.gif", imagesRootUrl));
            tableText = tableText.Replace("src='spades.gif'", String.Format("src={0}spades-large.gif", imagesRootUrl));
            tableText = tableText.Replace(pairHeadingOld, pairHeadingNew);

            tableText = String.Format(@"<b>Page Updated {0}</b><br/><br/>{1}<br/><br/>{2}", Utility.GetTimeStamp(), backLinkText, tableText);

            return tableText;
        }

        #endregion

        #region Private Methods

        private void InitializeDataTables()
        {
            m_butlerResults = new DataTable();
            m_butlerResults.Columns.Add("Pair", typeof(System.String));
            m_butlerResults.Columns.Add("Boards", typeof(System.Int16));
            m_butlerResults.Columns.Add("Score", typeof(System.Decimal));

            m_completedBoards = new DataTable();
            m_completedBoards.Columns.Add("Table", typeof(System.String));
            m_completedBoards.Columns.Add("Board", typeof(System.Int16));

        }

        private string ReplacePairNumbersWithNames(int boardNumber, string boardResultsHtml, 
            NameValueCollection pairNames, int numberOfTables)
        {
            string rowText, cellText, content, newCellText, result, linksText, tableNumber;
            string nsPair = String.Empty, ewPair = String.Empty;
            decimal nsScore=0, ewScore=0;
            bool isByeTable;
            int rowStartPosition, rowEndPosition = 0, rowIndex = 0;
            int cellStartPosition, cellEndPosition = 0, cellIndex = 0;
            int contentStartPosition, contentEndPosition = 0;

            while (rowIndex <= numberOfTables)
            {
                rowStartPosition = boardResultsHtml.IndexOf("<tr>", rowEndPosition, StringComparison.InvariantCultureIgnoreCase);
                
                // If we find fewer rows than number of tables, we just quit
                // This happens when the results at some tables haven't been entered yet
                if (rowStartPosition < 0)
                    break;

                rowEndPosition = boardResultsHtml.IndexOf("</tr>", rowStartPosition, StringComparison.InvariantCultureIgnoreCase);

                // Skip the first row (the header row)
                if (rowIndex != 0)
                {
                    cellStartPosition = 0; cellEndPosition = 0; cellIndex = 0;
                    rowText = boardResultsHtml.Substring(rowStartPosition, rowEndPosition + 5 - rowStartPosition);
                    DataRow row = m_completedBoards.NewRow();
                    isByeTable = false;

                    // 10 cells in each row
                    while (cellIndex < 10)
                    {
                        cellStartPosition = rowText.IndexOf("<td", cellEndPosition, StringComparison.InvariantCultureIgnoreCase);
                        cellEndPosition = rowText.IndexOf("</td>", cellStartPosition, StringComparison.InvariantCultureIgnoreCase);
                        cellText = rowText.Substring(cellStartPosition, cellEndPosition + 5 - cellStartPosition);

                        contentStartPosition = cellText.IndexOf(BOARD_RESULT_FONT_TEXT, StringComparison.InvariantCultureIgnoreCase) + 46;
                        contentEndPosition = cellText.IndexOf("</font>", contentStartPosition, StringComparison.InvariantCultureIgnoreCase);
                        content = cellText.Substring(contentStartPosition, contentEndPosition - contentStartPosition);

                        // Track the completed boards by table
                        if (cellIndex == 0)
                        {
                            tableNumber = content.Replace("\t", "-");
                            row["Table"] = tableNumber;
                            row["Board"] = boardNumber;
                        }

                        // We replace pair numbers with pair names in the 2nd and 3rd cells
                        if (cellIndex == 1 || cellIndex == 2)
                        {
                            if (content != "--")
                            {
                                newCellText = cellText.Replace(String.Format("{0}{1}</font>", BOARD_RESULT_FONT_TEXT, content), String.Format("{0}{1}</font>", BOARD_RESULT_FONT_TEXT, pairNames[content]));
                                boardResultsHtml = boardResultsHtml.Replace(cellText, newCellText);

                                if (cellIndex == 1)
                                {
                                    // Only add the row to completed boards if this is not a bye row
                                    m_completedBoards.Rows.Add(row);
                                    nsPair = pairNames[content];
                                }
                                else
                                {
                                    ewPair = pairNames[content];
                                }
                            }
                            else
                            {
                                isByeTable = true;
                            }
                        }

                        if (!isByeTable)
                        {
                            if (cellIndex == 8)
                            {
                                nsScore = Convert.ToDecimal(content);
                            }
                            if (cellIndex == 9)
                            {
                                ewScore = Convert.ToDecimal(content);
                            }
                        }
                        cellIndex++;
                    }

                    // Update butler results table for the 2 pairs in question
                    if (!isByeTable)
                    {
                        Utility.UpdateButlerResults(m_butlerResults, nsPair, nsScore);
                        Utility.UpdateButlerResults(m_butlerResults, ewPair, ewScore);
                    }
                }

                rowIndex++;
            }

            string note = "<table width=300 border=0><tr><td align=right><b>Note: Scores below are in <a target='_blank' href='http://en.wikipedia.org/wiki/Duplicate_bridge#IMP_scoring'>cross-imps</a></b></td></tr></table>";

            linksText = GetNavigationLinksText(boardNumber, note);

            result = linksText + boardResultsHtml.Replace("size=3", "size=2").Replace("width=57 align=center", "width=82 align=center");
            return result;
        }

        private string GetNavigationLinksText(int boardNumber, string note)
        {
            int numberOfBoardsPerRound = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfBoardsPerRound"]);
            string imagesRootUrl = ConfigurationManager.AppSettings["ImagesRoot"];
            string linksText = String.Empty;

            if (boardNumber == 1)
            {
                linksText = String.Format("<table><tr><td><table border=0><tr align=right border=0><td>&nbsp;</td><td><a href=board-{0}><img src={1}buttonNext.png></a></td></tr></table></td><td>{2}</td></tr></table>", boardNumber + 1, imagesRootUrl, note);
            }
            else if (boardNumber == numberOfBoardsPerRound)
            {
                linksText = String.Format("<table><tr><td><table border=0><tr align=right border=0><td><a href=board-{0}><img src={1}buttonPrev.png></a></td><td>&nbsp;</td></tr></table></td><td>{2}</td></tr></table>", boardNumber - 1, imagesRootUrl, note);
            }
            else
            {
                linksText = String.Format("<table><tr><td><table border=0><tr align=right border=0><td><a href=board-{0}><img src={2}buttonPrev.png></a></td><td><a href=board-{1}><img src={2}buttonNext.png></a></td></tr></table></td><td>{3}</td></tr></table>", boardNumber - 1, boardNumber + 1, imagesRootUrl, note);
            }

            return linksText;
        }

        private string GetBackToRunningScoresLinktext()
        {
            return String.Format("<a href='../../{0}'>Back to Running Scores</a>", m_runningScoreRoot);
        }

        #endregion
    }
}
