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
        const string TABLE_START_TAG = "<TABLE";
        const string TABLE_END_TAG = "</TABLE>";
        string m_inputFolder, m_runningScoreFileName, m_runningScoreRoot, m_boardResultFont;
        static DataTable m_roundResults;
        static Dictionary<int, DateTime> m_lastModifiedTimes = new Dictionary<int, DateTime>();
        #endregion

        #region Properties

        public DataTable RoundResults
        {
            get
            {
                return m_roundResults;
            }
        }

        #endregion

        #region Public Methods

        public MagicInterface(string folderName, string runningScoreFileName, 
            string runningScoreRoot, string boardResultFont, bool boardResultFontBold)
        {
            m_inputFolder = folderName;
            m_runningScoreFileName = runningScoreFileName;
            m_runningScoreRoot = runningScoreRoot;
            if (boardResultFontBold)
                m_boardResultFont = String.Format("<{0}><b>", boardResultFont);
            else
                m_boardResultFont = String.Format("<{0}>", boardResultFont);
        }

        public DataTable GetRunningScores(int numberOfMatchesPerRound, out int roundInProgress, out NameValueCollection nameNumberMapping)
        {
            string rowText, result = String.Empty;
            int roundStartPosition, roundEndPosition;
            int rank = 1, prevRank = 1, pairNumber;
            Decimal cumulativeScore, roundScore, penalty;
            bool success;
            string pairName, percentScore, penaltyText;
            DataRow row;
            nameNumberMapping = new NameValueCollection();

            int rankStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["RankStartPosition"]);
            int rankStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["RankStringLength"]);
            int pairNumberStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["PairNumberStartPosition"]);
            int pairNumberStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["PairNumberStringLength"]);
            int cumulativeScoreStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["CumulativeScoreStartPosition"]);
            int cumulativeScoreStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["CumulativeScoreStringLength"]);
            int percentScoreStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["PercentScoreStartPosition"]);
            int percentScoreStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["PercentScoreStringLength"]);
            int roundScoreStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["RoundScoreStartPosition"]);
            int roundScoreStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["RoundScoreStringLength"]);
            int pairNameStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["PairNameStartPosition"]);
            int pairNameStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["PairNameStringLength"]);
            int penaltyStartPosition = Convert.ToInt16(ConfigurationManager.AppSettings["PenaltyStartPosition"]);
            int penaltyStringLength = Convert.ToInt16(ConfigurationManager.AppSettings["PenaltyStringLength"]);

            // Prior to retrieving running scores for a round, initialize the Results datatable
            InitializeDataTables();

            string fileName = String.Format(@"{0}\{1}.htm", m_inputFolder, m_runningScoreFileName);
            string fileData = Utility.ReadFile(fileName, out success);

            roundStartPosition = fileData.IndexOf("<B>Round") + 8;
            roundEndPosition = fileData.IndexOf("<br>", roundStartPosition);
            roundInProgress = Convert.ToInt32(fileData.Substring(roundStartPosition, roundEndPosition - roundStartPosition).Trim());

            #region Get Running scores
            StreamReader streamReader = new StreamReader(fileName);
            bool scoreDataFound = false;

            while (!streamReader.EndOfStream)
            {
                // Read in a line of the file data
                rowText = streamReader.ReadLine();

                if (!scoreDataFound)
                {
                    if (rowText.Contains("<PRE>"))
                    {
                        scoreDataFound = true;

                        // skip past 3 more lined
                        for (int i = 0; i < 4; i++)
                        {
                            streamReader.ReadLine();
                        }
                    }
                }
                else
                {
                    if (rowText.Contains("</PRE>"))
                    {
                        // End of the road
                        break;
                    }
                    else
                    {
                        // skip past empty lines
                        if (String.IsNullOrEmpty(rowText.Trim())) continue;

                        // This is a line of score
                        // Rank may be missing if there's a tie
                        if (!int.TryParse(rowText.Substring(rankStartPosition, rankStringLength).Trim(), out rank)) rank = prevRank;
                        pairNumber = Convert.ToInt16(rowText.Substring(pairNumberStartPosition, pairNumberStringLength).Trim());
                        if (!decimal.TryParse(rowText.Substring(cumulativeScoreStartPosition, cumulativeScoreStringLength).Trim(), out cumulativeScore)) cumulativeScore = 0;
                        percentScore = rowText.Substring(percentScoreStartPosition, percentScoreStringLength).Trim();
                        if (!decimal.TryParse(rowText.Substring(roundScoreStartPosition, roundScoreStringLength).Trim(), out roundScore)) roundScore = 0;
                        pairName = rowText.Substring(pairNameStartPosition, pairNameStringLength).Trim();

                        // Penalty may not be there so we have to try-catch the value
                        try
                        {
                            penaltyText = rowText.Substring(penaltyStartPosition, penaltyStringLength).Trim();
                            if (!decimal.TryParse(penaltyText, out penalty)) penalty = 0;
                        }
                        catch (Exception)
                        {
                            penalty = 0;
                        }

                        row = m_roundResults.NewRow();
                        row["Rank"] = rank;
                        row["PairNumber"] = pairNumber;
                        row["CumulativeScore"] = cumulativeScore;
                        row["RoundScore"] = roundScore;
                        row["PairName"] = pairName;
                        row["Penalty"] = penalty;
                        row["PercentScore"] = percentScore;
                        m_roundResults.Rows.Add(row);
                        nameNumberMapping.Add(pairNumber.ToString(), pairName);

                        prevRank = rank;
                    }
                }

            }

            #endregion

            return m_roundResults;
        }

        public string GetBoardResults(int boardNumber, NameValueCollection pairNames,
            int numberOfTables, int roundInProgress, int numberOfBoardsPerRound, out bool hasNewResults)
        {
            bool success; DateTime boardLastUpdatedTime;
            // The file is always numbered from 1 to numberOfBoardsPerRound in each round
            string fileName = String.Format(@"{0}\{1}-{2}.htm", m_inputFolder, m_runningScoreFileName, boardNumber.ToString());
            int actualboardNumber = boardNumber + (roundInProgress - 1) * numberOfBoardsPerRound;

            hasNewResults = true;

            // Only create new board results if the corresponding magic file has been updated in the interim
            DateTime lastModifiedTime = File.GetLastWriteTime(fileName);
            if (m_lastModifiedTimes.ContainsKey(actualboardNumber))
            {
                boardLastUpdatedTime = m_lastModifiedTimes[actualboardNumber];
                if (lastModifiedTime.CompareTo(boardLastUpdatedTime) <= 0)
                {
                    hasNewResults = false;
                    return String.Empty;
                }
                else
                {
                    m_lastModifiedTimes[actualboardNumber] = lastModifiedTime;
                }
            }
            else
            {
                m_lastModifiedTimes.Add(actualboardNumber, lastModifiedTime);
            }

            string html = Utility.ReadFile(fileName, out success, true);
            string backLinkText = GetBackToRunningScoresLinktext();
            string navigationLinksText = GetNavigationLinksText(boardNumber, roundInProgress, String.Empty);

            if (!success)
            {
                return String.Format(@"{2}<b>Page Updated {0}</b><br/><br/><b>No scores yet</b><br/><br/><br/>{1}",
                    Utility.GetTimeStamp(), backLinkText, navigationLinksText, actualboardNumber.ToString());
            }

            int startPosition = html.IndexOf("<center>") + 8;
            int endPosition = html.IndexOf("</center>");
            string tableText = html.Substring(startPosition, endPosition - startPosition);

            string boardResultsTable;
            string imagesRootUrl = ConfigurationManager.AppSettings["ImagesRoot"];
            string suitSymbolsSuffix = ConfigurationManager.AppSettings["SuitSymbolsSuffix"];

            startPosition = tableText.LastIndexOf(TABLE_START_TAG, StringComparison.InvariantCultureIgnoreCase);
            endPosition = tableText.IndexOf(TABLE_END_TAG, startPosition, StringComparison.InvariantCultureIgnoreCase);

            boardResultsTable = tableText.Substring(startPosition, endPosition + 8 - startPosition);

            tableText = tableText.Replace(boardResultsTable, ReplacePairNumbersWithNames(boardNumber, boardResultsTable, pairNames, numberOfTables, roundInProgress));
            tableText = tableText.Replace(String.Format("src='clubs{0}'", suitSymbolsSuffix), String.Format("src={0}clubs-large.gif", imagesRootUrl));
            tableText = tableText.Replace(String.Format("src='diamonds{0}'", suitSymbolsSuffix), String.Format("src={0}diamonds-large.gif", imagesRootUrl));
            tableText = tableText.Replace(String.Format("src='hearts{0}'", suitSymbolsSuffix), String.Format("src={0}hearts-large.gif", imagesRootUrl));
            tableText = tableText.Replace(String.Format("src='spades{0}'", suitSymbolsSuffix), String.Format("src={0}spades-large.gif", imagesRootUrl));
            tableText = tableText.Replace(" color=white>", ">");

            tableText = String.Format(@"<b>Page Updated {0}</b><br/><br/>{1}<br/><br/>{2}", Utility.GetTimeStamp(), backLinkText, tableText);

            return tableText;
        }

        #endregion

        #region Private Methods

        private void InitializeDataTables()
        {
            m_roundResults = new DataTable();
            m_roundResults.Columns.Add("Rank", typeof(System.Int16));
            m_roundResults.Columns.Add("PairNumber", typeof(System.Int16));
            m_roundResults.Columns.Add("CumulativeScore", typeof(System.Decimal));
            m_roundResults.Columns.Add("PercentScore", typeof(System.String));
            m_roundResults.Columns.Add("RoundScore", typeof(System.Decimal));
            m_roundResults.Columns.Add("PairName", typeof(System.String));
            m_roundResults.Columns.Add("Penalty", typeof(System.Decimal));
        }

        private string ReplacePairNumbersWithNames(int boardNumber, string boardResultsHtml,
            NameValueCollection pairNames, int numberOfTables, int roundInProgress)
        {
            string rowText, cellText, content, newCellText, result, linksText, tableNumber = String.Empty;
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

                    // 10 cells in each row
                    while (cellIndex < 10)
                    {
                        cellStartPosition = rowText.IndexOf("<td", cellEndPosition, StringComparison.InvariantCultureIgnoreCase);
                        cellEndPosition = rowText.IndexOf("</td>", cellStartPosition, StringComparison.InvariantCultureIgnoreCase);
                        cellText = rowText.Substring(cellStartPosition, cellEndPosition + 5 - cellStartPosition);

                        contentStartPosition = cellText.IndexOf(m_boardResultFont, StringComparison.InvariantCultureIgnoreCase) + m_boardResultFont.Length;
                        contentEndPosition = cellText.IndexOf("</font>", contentStartPosition, StringComparison.InvariantCultureIgnoreCase);
                        content = cellText.Substring(contentStartPosition, contentEndPosition - contentStartPosition);

                        // Track the completed boards by table
                        if (cellIndex == 0)
                        {
                            tableNumber = content.Replace("\t", "-");
                        }

                        // We replace pair numbers with pair names in the 2nd and 3rd cells
                        if (cellIndex == 1 || cellIndex == 2)
                        {
                            if (content != "--")
                            {
                                newCellText = cellText.Replace(String.Format("{0}{1}</font>", m_boardResultFont, content), String.Format("{0}{1}</font>", m_boardResultFont, pairNames[content]));
                                boardResultsHtml = boardResultsHtml.Replace(cellText, newCellText);
                            }
                        }
                        cellIndex++;
                    }
                }
                rowIndex++;
            }

            string note = "<table width=300 border=0><tr><td align=right><b>Note: Scores below are in matchpoints</a></b></td></tr></table>";

            linksText = GetNavigationLinksText(boardNumber, roundInProgress, note);

            result = linksText + boardResultsHtml.Replace("size=4", "size=2").Replace("width=57 align=center", "width=82 align=center");
            return result;
        }

        private string GetNavigationLinksText(int boardNumber, int roundInProgress, string note)
        {
            int numberOfBoardsPerRound = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfBoardsPerRound"]);
            string imagesRootUrl = ConfigurationManager.AppSettings["ImagesRoot"];
            string linksText = String.Empty;
            int actualboardNumber = boardNumber + (roundInProgress - 1) * numberOfBoardsPerRound;

            if (boardNumber == 1)
            {
                linksText = String.Format("<table><tr><td><table border=0><tr align=right border=0><td>&nbsp;</td><td><a href=board-{0}><img src={1}buttonNext.png></a></td></tr></table></td><td>{2}</td></tr></table>", actualboardNumber + 1, imagesRootUrl, note);
            }
            else if (boardNumber % numberOfBoardsPerRound == 0)
            {
                linksText = String.Format("<table><tr><td><table border=0><tr align=right border=0><td><a href=board-{0}><img src={1}buttonPrev.png></a></td><td>&nbsp;</td></tr></table></td><td>{2}</td></tr></table>", actualboardNumber - 1, imagesRootUrl, note);
            }
            else
            {
                linksText = String.Format("<table><tr><td><table border=0><tr align=right border=0><td><a href=board-{0}><img src={2}buttonPrev.png></a></td><td><a href=board-{1}><img src={2}buttonNext.png></a></td></tr></table></td><td>{3}</td></tr></table>", actualboardNumber - 1, actualboardNumber + 1, imagesRootUrl, note);
            }

            return linksText;
        }

        private string GetBackToRunningScoresLinktext()
        {
            string link = String.Empty;
            link = String.Format("<a href='../../{0}'>Back to Running Scores</a>", m_runningScoreRoot);
            return link;
        }

        #endregion
    }
}
