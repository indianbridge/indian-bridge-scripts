using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using IndianBridge.Common;
using System.ComponentModel;

namespace IndianBridge.ResultsManager
{
    public class PairsDatabaseToWebpages
    {
        public struct Parameters
        {
            public String sortCriteria;
            public String sectionName;
            public String fileName;
            public String filterCriteria;
            public String headerTemplate;
            public String tableName;
            public OrderedDictionary columns;
        }
        public Parameters createDefaultParameters()
        {
            Parameters parameters = new Parameters();
            parameters.sortCriteria = "Event_Rank ASC";
            parameters.sectionName = null;
            parameters.fileName = "test.html";
            parameters.filterCriteria = "Pair_Number = '1'";
            parameters.headerTemplate = "Summary for";
            parameters.tableName = "Pair_Information";
            parameters.columns = new OrderedDictionary();
            return parameters;
        }
        private PairsEventInformation m_eventInformation;
        public PairsDatabaseParameters m_databaseParameters;
        private static String NORTH_SOUTH_SECTION_NAME = "North-South";
        private static String EAST_WEST_SECTION_NAME = "East-West";
        private static String SINGLE_SECTION_NAME = "Section-A";
        private HashSet<String> m_sectionNames = new HashSet<string>();
        private String m_prefix = "";
        private string m_currentPage = "";
        private int m_numberOfBoards = 0;
        private int totalNumberOfPagesToBeCreated = 0;
        private int numberOfPagesCreatedSoFar = 0;
        private BackgroundWorker m_worker;
        private bool m_runningInBackground = false;
        private string m_prefixString = "Creating Local Webpages : ";

        public PairsDatabaseToWebpages(PairsEventInformation eventInformation, PairsDatabaseParameters databaseParameters)
        {
            m_eventInformation = eventInformation;
            m_databaseParameters = databaseParameters;
        }

        private void printMessage(String message) { Trace.WriteLine(message); }

        private void reportProgress(string status)
        {
            numberOfPagesCreatedSoFar++;
            if (m_runningInBackground)
            {
                double percentage = ((double)numberOfPagesCreatedSoFar / (double)totalNumberOfPagesToBeCreated) * 100;
                m_worker.ReportProgress(Convert.ToInt32(percentage), status);
            }
        }

        private string getPrefix()
        {
            if (m_prefix == "" || m_prefix == "./") return "../" + m_currentPage + "/";
            else return m_prefix;
        }

        private string getPage(string path)
        {
            string page = path.TrimEnd('/');
            page = page.TrimEnd('\\');
            int index1 = page.LastIndexOf('/') + 1;
            int index2 = page.LastIndexOf('\\') + 1;
            page = page.Substring(index1 > index2 ? index1 : index2);
            return page;
        }

        public void createWebpagesInBackground(object sender, DoWorkEventArgs e)
        {
            m_runningInBackground = true;
            m_worker = sender as BackgroundWorker;
            numberOfPagesCreatedSoFar = 0;
            createWebpagesInternal();
        }

        public void createWebpages_()
        {
            m_runningInBackground = false;
            m_worker = null;
            createWebpagesInternal();
        }

        private void createWebpagesInternal()
        {
            String rootFolder = m_eventInformation.webpagesDirectory;
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
            if (m_eventInformation.isACBLSummary)
            {
                printMessage("Creating Section Names...");
                m_sectionNames.Clear();
                if (m_eventInformation.hasDirectionField) m_sectionNames.Add(SINGLE_SECTION_NAME);
                else { m_sectionNames.Add(NORTH_SOUTH_SECTION_NAME); m_sectionNames.Add(EAST_WEST_SECTION_NAME); }
                m_numberOfBoards = Convert.ToInt16(m_databaseParameters.m_ds.Tables["Board_Wise_Scores"].Select("Board_Number=MAX(Board_Number)")[0]["Board_Number"]);
                totalNumberOfPagesToBeCreated = 1 + m_sectionNames.Count + m_sectionNames.Count + m_numberOfBoards;
                foreach (String sectionName in m_sectionNames)
                    totalNumberOfPagesToBeCreated += (m_databaseParameters.m_ds.Tables["Pair_Information"].Select("Section_Name='" + sectionName + "'")).Length;
                Parameters parameters = createDefaultParameters();
                parameters.columns.Add("Rank", "{Event_Rank}");
                parameters.columns.Add("Pair Number", "[_makePairNumberLink]");
                parameters.columns.Add("Direction","{Section_Name}");
                parameters.columns.Add("Names", "[_makePairNamesLink]");
                parameters.columns.Add(m_eventInformation.isIMP ? "IMPs" : "MPs", "{Total_Score}");
                if (!m_eventInformation.isIMP) parameters.columns.Add("Percentage", "{Percentage}");
                parameters.sectionName = null;
                parameters.sortCriteria = "Event_Rank ASC";
                m_prefix = "./";
                parameters.fileName = Path.Combine(rootFolder, "index.html");
                parameters.filterCriteria = "";
                parameters.headerTemplate = headerify_("Combined Ranking sorted by Rank<br/>[_commonPageHeader]");
                parameters.tableName = "Pair_Information";
                printMessage("Creating Combined Ranking Leaderboard...");
                reportProgress(m_prefixString + "Creating Combined Leaderboard");
                createPage_(parameters);
                foreach (String sectionName in m_sectionNames)
                {
                    String newFolder = Path.Combine(rootFolder, Utilities.makeIdentifier_(sectionName));
                    if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);
                    parameters.columns["Rank"] = "{Session_Rank}";
                    parameters.sortCriteria = "Session_Rank ASC";
                    m_prefix = "../";
                    parameters.fileName = Path.Combine(newFolder, "leaderboard.html");
                    parameters.sectionName = sectionName;
                    parameters.filterCriteria = "Section_Name='" + parameters.sectionName + "'";
                    parameters.headerTemplate = headerify_("Ranking for " + parameters.sectionName + " sorted by Rank<br/>[_commonPageHeader]");
                    printMessage("Creating Leaderboard for " + sectionName);
                    reportProgress(m_prefixString + "Creating "+sectionName+" Leaderboard");
                    createPage_(parameters);
                }
                parameters.columns.Clear();
                parameters.columns.Add("Pair Number", "[_makePairNumberLink]");
                parameters.columns.Add("Names", "[_makePairNamesLink]");
                parameters.columns.Add("Rank", "{Session_Rank}");
                parameters.columns.Add(m_eventInformation.isIMP ? "IMPs" : "MPs", "{Total_Score}");
                if (!m_eventInformation.isIMP) parameters.columns.Add("Percentage", "{Percentage}");
                foreach (String sectionName in m_sectionNames)
                {
                    String newFolder = Path.Combine(rootFolder, Utilities.makeIdentifier_(sectionName));
                    if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);
                    parameters.sortCriteria = "Pair_Number ASC";
                    m_prefix = "../";
                    parameters.fileName = Path.Combine(newFolder, "pair_wise_scores.html");
                    parameters.sectionName = sectionName;
                    parameters.filterCriteria = "Section_Name='" + parameters.sectionName + "'";
                    parameters.headerTemplate = headerify_("Pair Wise Scores " + parameters.sectionName + "<br/>[_commonPageHeader]");
                    printMessage("Creating pair wise scores for " + sectionName);
                    reportProgress(m_prefixString + "Creating "+sectionName+ " pairwise scores");
                    createPage_(parameters);
                }
                createBoardPages_(rootFolder);
                createPairPages_(rootFolder);
            }
            else
            {
                createNonACBLSummaryPage_(Path.Combine(rootFolder, "index.html"),m_eventInformation.rawText);
            }
            printMessage("Finished");
        }
        private String headerify_(String text)
        {
            return text;
            //return "<h4>" + text + "</h4>";
        }
        private void createNonACBLSummaryPage_(String fileName, String summary)
        {
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("<html><head></head><body>");
            sw.WriteLine(summary);
            sw.WriteLine("</body></html>");
            sw.Close();
        }
        private void createPage_(Parameters parameters)
        {
            StreamWriter sw = new StreamWriter(parameters.fileName);
            sw.WriteLine("<html><head></head><body>");
            DataRow[] foundRows = m_databaseParameters.m_ds.Tables[parameters.tableName].Select(parameters.filterCriteria, parameters.sortCriteria);
            if (foundRows.Length < 1)
            {
                sw.WriteLine("<h3>Bye Table</h3>");
            }
            else
            {
                DataRow dRow = foundRows[0];
                sw.WriteLine(applyTemplate_(parameters.headerTemplate, dRow));
                sw.WriteLine(makeTablePreamble_() + "<thead><tr>");
                ArrayList tableHeader = new ArrayList();
                foreach (DictionaryEntry pair in parameters.columns)
                {
                    tableHeader.Add(applyTemplate_("" + pair.Key, dRow));
                }
                sw.WriteLine(makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
                for (int i = 0; i < foundRows.Length; ++i)
                {
                    DataRow row = foundRows[i];
                    ArrayList tableRow = new ArrayList();
                    foreach (DictionaryEntry pair in parameters.columns)
                    {
                        string value = "" + pair.Value;
                        if (value.Equals("Serial_Number", StringComparison.OrdinalIgnoreCase)) tableRow.Add("" + (i + 1));
                        else tableRow.Add(applyTemplate_(value, row));
                    }
                    sw.WriteLine("<tr>" + makeTableCell_(tableRow, i) + "</tr>");
                }
            }
            sw.WriteLine("</tbody></table></body></html>");
            sw.Close();
        }
        private void createPairPages_(String rootFolder)
        {
            foreach (String sectionName in m_sectionNames)
            {
                int numberOfPairs = (m_databaseParameters.m_ds.Tables["Pair_Information"].Select("Section_Name='" + sectionName + "'")).Length;
                String newFolder = Path.Combine(rootFolder, Utilities.makeIdentifier_(sectionName));
                if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);
                for (int i = 1; i <= numberOfPairs; ++i)
                {
                    Parameters parameters = createDefaultParameters();
                    parameters.columns.Clear();
                    parameters.columns.Add("Board", "[_makeBoardNumberLink]");
                    parameters.columns.Add("Opponent", "[_makeOpponentLink]");
                    parameters.columns.Add("Result", "{Result}");
                    if (m_eventInformation.isIMP) parameters.columns.Add("Datum", "{Datum}");
                    parameters.columns.Add(m_eventInformation.isIMP ? "IMPs" : "MPs", "{Score}");
                    parameters.sortCriteria = "Board_Number ASC";
                    m_prefix = "../";
                    parameters.fileName = Path.Combine(newFolder, "pair" + i + "score.html");
                    parameters.sectionName = sectionName;
                    parameters.filterCriteria = "Section_Name = '" + sectionName + "' AND Pair_Number='" + i + "'";
                    parameters.headerTemplate = headerify_("Summary for Pair " + i + " [_getPairInfo]<br/>[_commonPageHeader]");
                    parameters.tableName = "Pair_Wise_Scores";
                    printMessage("Creating page for pair " + i + " in " + sectionName);
                    reportProgress(m_prefixString + "Creating "+sectionName+" Pair "+i+" page");
                    createPage_(parameters);
                }

            }
        }
        private String makeTablePreamble_()
        {
            return "<table style=\'font-size:0.8em;border: 1px solid #cef;text-align: left;\'>";
        }
        private String makeTableHeader_(ArrayList text)
        {
            var retVal = "";
            foreach (String i in text) retVal += makeTableHeader_(i);
            return retVal;
        }
        private String makeTableHeader_(String text, bool usePadding = false)
        {
            return "<th style=\'font-weight: bold;background-color: #acf;border-bottom: 1px solid #cef;" + (usePadding ? "padding: 4px 5px;" : "") + "\'>" + text + "</th>";
        }
        private String makeTableCell_(ArrayList text, int row)
        {
            var retVal = "";
            foreach (String i in text) retVal += makeTableCell_(i, row);
            return retVal;
        }
        private String makeTableCell_(String text, int row, bool usePadding = false)
        {
            if (row % 2 == 0) return "<td style=\'" + (usePadding ? "padding: 4px 5px;" : "") + "background-color: #def; border-bottom: 1px solid #cef;\'>" + text + "</td>";
            else return "<td " + (usePadding ? "style=\'padding: 4px 5px;\'" : "") + ">" + text + "</td>";
        }

        private String applyTemplate_(String template, DataRow dRow)
        {
            Regex re = new Regex("{[^{}]+}");
            String result = re.Replace(template, new MatchEvaluator(delegate(Match match) { return findColumnValue_(match, dRow); }));
            re = new Regex(@"\[[^\[\]]+\]");
            return re.Replace(result, new MatchEvaluator(delegate(Match match)
            {
                Type thisType = this.GetType();
                String value = match.Value.Replace("[", "").Replace("]", "");
                MethodInfo theMethod = thisType.GetMethod(value, BindingFlags.Public | BindingFlags.Instance);
                return "" + theMethod.Invoke(this, new object[] { dRow });
            }));
        }


        private String findColumnValue_(Match match, DataRow dRow)
        {
            String columnName = match.Value.Replace("{", "").Replace("}", "");
            return "" + dRow[columnName];
        }


        public String _commonPageHeader(DataRow dRow)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Globals.INDIAN_ZONE);
            String result = "Page Updated on " + indianTime.ToString("MMMM dd, yyyy hh:mm:ss tt") + " IST<br/>";
            result += "Travellers : ";
            for (int i = 1; i <= m_numberOfBoards; ++i)
            {
                result += (i == 0 ? "" : " | ") + "<a href='" + m_prefix + "boards" + "/board" + i + "score.html'>" + i + "</a>";
            }
            result += "<br/>";
            result += "<a href='" + m_prefix + "index.html'>Combined Leaderboard</a><br/>";
            foreach (String sectionName in m_sectionNames)
            {
                result += "<a href='" + m_prefix + Utilities.makeIdentifier_(sectionName) + "/leaderboard.html'>" + sectionName + " Leaderboard</a> | <a href='" + m_prefix + Utilities.makeIdentifier_(sectionName) + "/pair_wise_scores.html'>" + sectionName + " Pair Wise Scores</a><br/>";
                result += "Personal Scores for Pair : ";
                int numberOfPairs = (m_databaseParameters.m_ds.Tables["Pair_Information"].Select("Section_Name='" + sectionName + "'")).Length;
                for (int i = 1; i <= numberOfPairs; ++i)
                {
                    result += (i == 0 ? "" : " | ") + "<a href='" + m_prefix + Utilities.makeIdentifier_(sectionName) + "/pair" + i + "score.html'>" + i + "</a>";
                }
                result += "<br/>";
            }
            result += "<br/>";
            return result;
        }

        public String _makeBoardNumberLink(DataRow dRow)
        {
            return "<a href='" + m_prefix + "boards" + "/board" + dRow["Board_Number"] + "score.html'>" + dRow["Board_Number"] + "</a>";
        }

        public String _makePairNumberLink(DataRow dRow)
        {
            return "<a href='" + m_prefix + Utilities.makeIdentifier_("" + dRow["Section_Name"]) + "/pair" + dRow["Pair_Number"] + "score.html'>" + dRow["Pair_Number"] + "</a>";
        }
        public String _getPairInfo(DataRow dRow)
        {
            String sectionName = "" + dRow["Section_Name"];
            int pairNumber = Convert.ToInt16(dRow["Pair_Number"]);
            DataTable table = m_databaseParameters.m_ds.Tables["Pair_Information"];
            String filterExpression = "Section_Name='" + sectionName + "' AND Pair_Number='" + pairNumber + "'";
            DataRow[] foundRows = table.Select(filterExpression);
            if (foundRows.Length == 1)
            {
                DataRow row = foundRows[0];
                return "(" + row["Pair_Names"] + ")<br/>Total Score : " + row["Total_Score"] + " | " + (m_eventInformation.isIMP ? "" : "Percentage : " + row["Percentage"] + " | ") + "Overall Rank : " + row["Event_Rank"] + " | Section Rank : " + row["Session_Rank"];
            }
            else if (foundRows.Length > 1)
            {
                return "Multiple Names Found.";
            }
            else return "Not Found";
        }

        public String _makePairNamesLink(DataRow dRow)
        {
            String sectionName = "" + dRow["Section_Name"];
            int pairNumber = Convert.ToInt16(dRow["Pair_Number"]);
            return "<a href='" + m_prefix + Utilities.makeIdentifier_(sectionName) + "/pair" + pairNumber + "score.html'>" + findPairNames_(sectionName, pairNumber) + "</a>";
        }

        public String _makePairLink(DataRow dRow)
        {
            String sectionName = "" + dRow["Section_Name"];
            int pairNumber = Convert.ToInt16(dRow["Pair_Number"]);
            return "<a href='" + m_prefix + Utilities.makeIdentifier_(sectionName) + "/pair" + pairNumber + "score.html'>" + pairNumber + ". " + findPairNames_(sectionName, pairNumber) + "</a>";
        }
        public String _makeOpponentLink(DataRow dRow)
        {
            String sectionName = PairsGeneral.findOtherSectionName("" + dRow["Section_Name"]);
            int pairNumber = Convert.ToInt16(dRow["Opponent"]);
            return "<a href='" + m_prefix + Utilities.makeIdentifier_(sectionName) + "/pair" + pairNumber + "score.html'>" + pairNumber + ". " + findPairNames_(sectionName, pairNumber) + "</a>";
        }



        private String findPairNames_(String sectionName, int pairNumber)
        {
            DataTable table = m_databaseParameters.m_ds.Tables["Pair_Information"];
            String filterExpression = "Section_Name='" + sectionName + "' AND Pair_Number='" + pairNumber + "'";
            DataRow[] foundRows = table.Select(filterExpression);
            if (foundRows.Length == 1)
            {
                DataRow dRow = foundRows[0];
                return "" + dRow["Pair_Names"];
            }
            else if (foundRows.Length > 1)
            {
                return "Multiple Names Found.";
            }
            else return "Not Found";
        }


        private void createBoardPages_(String rootFolder)
        {
            DataRow[] foundRows = m_databaseParameters.m_ds.Tables["Event_Information"].Select();
            DataRow dRow = foundRows[0];
            String newFolder = Path.Combine(rootFolder, "boards");
            if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);
            for (int i = 1; i <= m_numberOfBoards; ++i)
            {
                Parameters parameters = createDefaultParameters();
                parameters.columns.Clear();
                parameters.columns.Add("S.No.", "Serial_Number");
                parameters.columns.Add("NS", "[_makePairLink]");
                parameters.columns.Add("EW", "[_makeOpponentLink]");
                parameters.columns.Add("NS Result", "{Result}");
                parameters.columns.Add("EW Result", "{Opponent_Result}");
                if (m_eventInformation.isIMP) parameters.columns.Add("Datum", "{Datum}");
                parameters.columns.Add(m_eventInformation.isIMP ? "NS IMPs" : "NS MPs", "{Score}");
                parameters.columns.Add(m_eventInformation.isIMP ? "EW IMPs" : "EW MPs", "{Opponent_Score}");
                parameters.sortCriteria = "Pair_Number ASC";
                m_prefix = "../";
                parameters.fileName = Path.Combine(newFolder, "board" + i + "score.html");
                parameters.sectionName = null;
                parameters.filterCriteria = "Board_Number='" + i + "'";
                if (m_eventInformation.hasDirectionField) parameters.filterCriteria += (" AND Direction='N-S'");
                if (m_sectionNames.Count > 1) parameters.filterCriteria += (" AND Section_Name='" + NORTH_SOUTH_SECTION_NAME + "'");
                parameters.headerTemplate = headerify_("Summary for Board " + i + "<br/>" + "Dealer : {Dealer} Vul : {Vulnerability}<br/>[_commonPageHeader]");
                parameters.tableName = "Board_Wise_Scores";
                printMessage("Creating page for board " + i);
                reportProgress(m_prefixString + "Creating board "+i+" page");
                createPage_(parameters);
            }
        }
    }
}
