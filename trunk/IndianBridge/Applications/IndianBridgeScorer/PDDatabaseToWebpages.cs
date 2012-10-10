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

namespace IndianBridgeScorer
{
    public class PDDatabaseToWebpages
    {
        private string m_databaseFileName = "";
        private string m_webpagesRootDirectory = "";
        private string m_prefix = "";
        private int m_numberOfTeams = 0;
        private int m_numberOfRounds = 0;
        private int m_numberOfBoardsPerRound = 0;
        private int m_totalNumberOfBoards = 0;
        private int totalNumberOfPagesToBeCreated = 0;
        private int numberOfPagesCreatedSoFar = 0;
        private BackgroundWorker m_worker;
        private bool m_runningInBackground = false;
        private string m_prefixString = "Creating Local Webpages : ";

        public PDDatabaseToWebpages(string eventName, string databaseFileName, string webpagesRoot)
        {
            setParameters(eventName, databaseFileName, webpagesRoot);
        }

        public void setParameters(string eventName, string databaseFileName, string webpagesRoot)
        {
            m_databaseFileName = databaseFileName;
            m_webpagesRootDirectory = webpagesRoot;
            string mainNiniFileName = Constants.getEventInformationFileName(eventName);
            m_numberOfTeams = NiniUtilities.getIntValue(mainNiniFileName, Constants.NumberOfTeamsFieldName);
            m_numberOfRounds = NiniUtilities.getIntValue(mainNiniFileName, Constants.NumberOfRoundsFieldName);
            m_numberOfBoardsPerRound = NiniUtilities.getIntValue(mainNiniFileName, Constants.NumberOfBoardsFieldName);
            m_totalNumberOfBoards = m_numberOfTeams * m_numberOfBoardsPerRound;
        }

        public void createWebpagesInBackground(object sender,DoWorkEventArgs e)
        {
            m_runningInBackground = true;
            m_worker = sender as BackgroundWorker;
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
            numberOfPagesCreatedSoFar = 0;
            totalNumberOfPagesToBeCreated = 1 + 1 + m_numberOfTeams + m_numberOfRounds + m_totalNumberOfBoards;
            string rootFolder = m_webpagesRootDirectory;
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
            createLeaderboard();
            createNamesPage();
            createTeamPages();
            createRoundPages();
            createBoardPages();
        }

        private void createBoardPages()
        {
            printMessage("Creating Board Pages");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "boards");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);

            for (int i = 1; i <= m_totalNumberOfBoards; ++i)
            {
                createBoardPage(i);
            }
        }

        private void createBoardPage(int boardNumber)
        {
            printMessage("Creating Board Page for Round Number : " + boardNumber);
            m_prefix = "../";
            StreamWriter sw = new StreamWriter(Path.Combine(m_webpagesRootDirectory, "boards", "board" + boardNumber + "score.html"));
            sw.WriteLine("<html><head></head><body>");
            string headerTemplate = "<h2>Scores for Board " + boardNumber + "</h2>" + "<br/>[_commonPageHeader]";
            sw.WriteLine(applyTemplate_(headerTemplate, null));
            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Add("NS Team");
            tableHeader.Add("EW Team");
            tableHeader.Add("Round");
            tableHeader.Add("NS_Score");
            tableHeader.Add("EW_Score");
            tableHeader.Add("NS MPs");
            tableHeader.Add("EW MPs");
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            DataRow[] foundRows = getTable(Constants.TableName.EventScores).Select("Board_Number = " + boardNumber, "Round_Number ASC");
            int i = 0;
            foreach (DataRow dRow in foundRows)
            {
                ArrayList tableRow = new ArrayList();
                if (dRow["NS_Team_Number"] != DBNull.Value) tableRow.Add(getTeamLink((int)dRow["NS_Team_Number"], true, true));
                else tableRow.Add("-");
                if (dRow["EW_Team_Number"] != DBNull.Value) tableRow.Add(getTeamLink((int)dRow["EW_Team_Number"], true, true));
                else tableRow.Add("-");
                if (dRow["Round_Number"] != DBNull.Value) tableRow.Add(getRoundNumberLink((int)dRow["Round_Number"]));
                else tableRow.Add("-");
                if (dRow["NS_Score"] != DBNull.Value) tableRow.Add(""+(double)dRow["NS_Score"]);
                else tableRow.Add("-");
                if (dRow["EW_Score"] != DBNull.Value) tableRow.Add(""+(double)dRow["EW_Score"]);
                else tableRow.Add("-");
                if (dRow["NS_MPs"] != DBNull.Value) tableRow.Add(""+(double)dRow["NS_MPs"]);
                else tableRow.Add("-");
                if (dRow["EW_MPs"] != DBNull.Value) tableRow.Add(""+(double)dRow["EW_MPs"]);
                else tableRow.Add("-");
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
                i++;
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
            reportProgress("Created Round " + boardNumber + " Score Page");
        }


        private void printMessage(String message) { 
            Trace.WriteLine(message);   
        }

        private void reportProgress(string title)
        {
            numberOfPagesCreatedSoFar++;
            if (m_runningInBackground)
            {
                double percentage = ((double)numberOfPagesCreatedSoFar / (double)totalNumberOfPagesToBeCreated) * 100;
                m_worker.ReportProgress(Convert.ToInt32(percentage), m_prefixString+ "Created " + title+ " Page");
            }
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

        private void createLeaderboard()
        {
            printMessage("Creating Leaderboard...");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "leaderboard");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
            string title = "<h2>Leaderboard</h2>";
            string headerTemplate = title + "<br/>[_commonPageHeader]";
            Utilities.HTMLTableParameters parameters = new Utilities.HTMLTableParameters("");
            parameters.columns.Add("Rank", "{Rank}");
            parameters.columns.Add("No.", "[_makeTeamNumberLink]");
            parameters.columns.Add("Name", "[_makeTeamNameLink]");
            parameters.columns.Add("Total_Score", "{Total_Score}");
            parameters.columns.Add("Boards_Played", "{Boards_Played}");
            parameters.columns.Add("Percentage_Score", "{Percentage_Score}");
            parameters.sortCriteria = "Rank ASC";
            m_prefix = "../";
            parameters.fileName = Path.Combine(rootFolder, "index.html");
            parameters.filterCriteria = "";
            parameters.headerTemplate = headerTemplate;
            parameters.tableName = Constants.TableName.EventNames;          
            createPage_(parameters);
            m_prefix = "./";
            parameters.fileName = Path.Combine(m_webpagesRootDirectory, "index.html");
            createPage_(parameters);
            reportProgress("Created Leaderboard Page");
        }

        private void createNamesPage()
        {
            printMessage("Creating Team Names...");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "names");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);

            Utilities.HTMLTableParameters parameters = new Utilities.HTMLTableParameters("");
            parameters.columns.Add("No.", "[_makeTeamNumberLink]");
            parameters.columns.Add("Name", "[_makeTeamNameLink]");
            parameters.columns.Add("Members", "{Member_Names}");
            parameters.sortCriteria = "Team_Number ASC";
            m_prefix = "../";
            parameters.fileName = Path.Combine(rootFolder, "index.html");
            parameters.filterCriteria = "";
            parameters.headerTemplate = "<h2>Team Compositions</h2>" + "<br/>[_commonPageHeader]";
            parameters.tableName = Constants.TableName.EventNames;
            createPage_(parameters);
            reportProgress("Created Names Page");
        }

        private void createTeamPages() {
            printMessage("Creating Team Pages");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "teams");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);

            for (int i = 1; i <= m_numberOfTeams; ++i)
            {
                createTeamPage(i);
            }
        }

        private DataTable getTable(string tableName) { return AccessDatabaseUtilities.getDataTable(m_databaseFileName, tableName); }
        private double getDoubleValue(DataRow dRow, string columnName) { return AccessDatabaseUtilities.getDoubleValue(dRow, columnName); }
        private string getTeamMemberNames(DataRow dRow)
        {
            DataRow[] dRows = getTable(Constants.TableName.EventNames).Select("Team_Number = " + (int)dRow["Team_Number"]);
            Debug.Assert(dRows.Length == 1);
            return (string)dRows[0]["Member_Names"];
        }

        private string createMatchTable(DataRow[] dRows, int numberOfSessions)
        {
            string html = "";
            html+=(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            ArrayList tableRow = new ArrayList();
            tableHeader.Add("Team");
            tableHeader.Add("Carryover");
            for (int i = 1; i <= numberOfSessions; ++i)
            {
                tableHeader.Add("Session " + i);
            }
            tableHeader.Add("Total");
            html += (Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            for (int j = 0; j < 2; ++j)
            {
                int otherRow = 1 - j;
                tableRow.Clear();
                int teamNumber = (int)dRows[j]["Team_Number"];
                double total = (double)dRows[j]["Total_IMPs"];
                double otherTotal = (double)dRows[otherRow]["Total_IMPs"];
                if (total > otherTotal) tableRow.Add("<b>" + getTeamLink(teamNumber, true, true) + "</b>");
                else tableRow.Add(getTeamLink(teamNumber, true, true));
                tableRow.Add(""+getDoubleValue(dRows[j], "Carryover"));
                for (int k = 1; k <= numberOfSessions; ++k)
                {
                    string columnName = "Session_" + k + "_Score";
                    double sessionScore = getDoubleValue(dRows[j],columnName);
                    double otherSessionScore = getDoubleValue(dRows[otherRow], columnName);
                    if (sessionScore > otherSessionScore) tableRow.Add("<b>" + sessionScore + "</b>");
                    else tableRow.Add("" + sessionScore);
                }
                if (total > otherTotal) tableRow.Add("<b>" + total + "</b>");
                else tableRow.Add(""+total);
                html += ("<tr>" + Utilities.makeTableCell_(tableRow, (total>otherTotal)?0:1) + "</tr>");
            }
            html+=("</tbody></table>");
            return html;
        }

        private void createTeamPage(int teamNumber)
        {
            printMessage("Creating Team Page for Team Number : " + teamNumber);
            m_prefix = "../";
            StreamWriter sw = new StreamWriter(Path.Combine(m_webpagesRootDirectory, "teams","team" + teamNumber + "score.html"));
            sw.WriteLine("<html><head></head><body>");
            DataRow dRow = getTable(Constants.TableName.EventNames).Rows.Find(teamNumber);
            string headerTemplate = "<h2>Scores for " + teamNumber + " : {Team_Name} ({Member_Names})</h2>";
            headerTemplate += "<h3>Total Score : {Total_Score}, Boards Played : {Boards_Played}, Percentage Score : {Percentage_Score}, Rank : {Rank}</h3>";
            sw.WriteLine(applyTemplate_(headerTemplate, dRow));
            headerTemplate = "<br/>[_commonPageHeader]";
            sw.WriteLine(applyTemplate_(headerTemplate, dRow));

            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Add("Board");
            tableHeader.Add("Opponent");
            tableHeader.Add("NS_Score");
            tableHeader.Add("EW_Score");
            tableHeader.Add("MPs");
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            for (int i = 1; i <= m_totalNumberOfBoards; ++i)
            {
                ArrayList tableRow = new ArrayList();
                tableRow.Add(getBoardNumberLink(i));
                DataRow[] nsRows = getTable(Constants.TableName.EventScores).Select("Board_Number = " + i + " AND NS_Team_Number = " + teamNumber);
                DataRow[] ewRows = getTable(Constants.TableName.EventScores).Select("Board_Number = " + i + " AND EW_Team_Number = " + teamNumber);
                if (nsRows.Length > 0 && nsRows[0]["EW_Team_Number"] != DBNull.Value)
                {
                    tableRow.Add(getTeamLink(nsRows[0]["EW_Team_Number"], true, true));
                }
                else if (ewRows.Length > 0 && ewRows[0]["NS_Team_Number"] != DBNull.Value)
                {
                    tableRow.Add(getTeamLink(ewRows[0]["NS_Team_Number"], true, true));
                }
                else tableRow.Add("-");
                if (nsRows.Length > 0 && nsRows[0]["NS_Score"] != DBNull.Value)
                {
                    tableRow.Add("" + nsRows[0]["NS_Score"]);
                }
                else tableRow.Add("-");
                if (ewRows.Length > 0 && ewRows[0]["NS_Score"] != DBNull.Value)
                {
                    tableRow.Add("" + ewRows[0]["EW_Score"]);
                }
                else tableRow.Add("-");
                if (nsRows.Length > 0 && nsRows[0]["NS_MPs"] != DBNull.Value)
                {
                    tableRow.Add("" + nsRows[0]["NS_MPs"]);
                }
                else tableRow.Add("-");
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
            reportProgress("Created Team "+teamNumber+" Score Page");
        }

        private void createRoundPages()
        {
            printMessage("Creating Round Pages");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "rounds");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);

            for (int i = 1; i <= m_numberOfRounds; ++i)
            {
                createRoundPage(i);
            }
        }

        private void createRoundPage(int roundNumber)
        {
            printMessage("Creating Round Page for Round Number : " + roundNumber);
            m_prefix = "../";
            StreamWriter sw = new StreamWriter(Path.Combine(m_webpagesRootDirectory, "rounds", "round" + roundNumber + "score.html"));
            sw.WriteLine("<html><head></head><body>");
            string headerTemplate = "<h2>Scores for Round " + roundNumber + "</h2>" + "<br/>[_commonPageHeader]";
            sw.WriteLine(applyTemplate_(headerTemplate, null));
            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Add("NS Team");
            tableHeader.Add("EW Team");
            tableHeader.Add("Board");
            tableHeader.Add("NS_Score");
            tableHeader.Add("EW_Score");
            tableHeader.Add("NS MPs");
            tableHeader.Add("EW MPs");
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            DataRow[] foundRows = getTable(Constants.TableName.EventScores).Select("Round_Number = " + roundNumber, "NS_Team_Number ASC");
            int i = 0;
            foreach (DataRow dRow in foundRows)
            {
                ArrayList tableRow = new ArrayList();
                if (dRow["NS_Team_Number"] != DBNull.Value) tableRow.Add(getTeamLink((int)dRow["NS_Team_Number"],true,true));
                else tableRow.Add("-");
                if (dRow["EW_Team_Number"] != DBNull.Value) tableRow.Add(getTeamLink((int)dRow["EW_Team_Number"],true,true));
                else tableRow.Add("-");
                if (dRow["Board_Number"] != DBNull.Value) tableRow.Add(getBoardNumberLink((int)dRow["Board_Number"]));
                else tableRow.Add("-");
                if (dRow["NS_Score"] != DBNull.Value) tableRow.Add(""+(double)dRow["NS_Score"]);
                else tableRow.Add("-");
                if (dRow["EW_Score"] != DBNull.Value) tableRow.Add(""+(double)dRow["EW_Score"]);
                else tableRow.Add("-");
                if (dRow["NS_MPs"] != DBNull.Value) tableRow.Add(""+(double)dRow["NS_MPs"]);
                else tableRow.Add("-");
                if (dRow["EW_MPs"] != DBNull.Value) tableRow.Add(""+(double)dRow["EW_MPs"]);
                else tableRow.Add("-");
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
                i++;
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
            reportProgress("Created Round "+roundNumber+" Score Page");
        }
        private void createPage_(Utilities.HTMLTableParameters parameters)
        {
            StreamWriter sw = new StreamWriter(parameters.fileName);
            sw.WriteLine("<html><head></head><body>");
            DataRow[] foundRows = getTable(parameters.tableName).Select(parameters.filterCriteria, parameters.sortCriteria);
            DataRow dRow = (foundRows.Length < 1) ? null : foundRows[0];
            sw.WriteLine(applyTemplate_(parameters.headerTemplate, dRow));

            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            foreach (DictionaryEntry pair in parameters.columns)
            {
                tableHeader.Add(applyTemplate_("" + pair.Key, dRow));
            }
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            for (int i = 0; i < foundRows.Length; ++i)
            {
                DataRow row = foundRows[i];
                ArrayList tableRow = new ArrayList();
                foreach (DictionaryEntry pair in parameters.columns)
                {
                    string value = "" + pair.Value;
                    if (value.Equals("Serial_Number", StringComparison.OrdinalIgnoreCase)) tableRow.Add("" + i);
                    else tableRow.Add(applyTemplate_(value, row));
                }
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
        }

        private string applyTemplate_(String template, DataRow dRow)
        {
            string result = "";
            Regex re;
            if (dRow != null)
            {
                re = new Regex("{[^{}]+}");
                result = re.Replace(template, new MatchEvaluator(delegate(Match match) { return findColumnValue_(match, dRow); }));
            }
            else result = template;
            re = new Regex(@"\[[^\[\]]+\]");
            return re.Replace(result, new MatchEvaluator(delegate(Match match)
            {
                Type thisType = this.GetType();
                String value = match.Value.Replace("[", "").Replace("]", "");
                MethodInfo theMethod = thisType.GetMethod(value, BindingFlags.Public | BindingFlags.Instance);
                return "" + theMethod.Invoke(this, new object[] { dRow });
            }));
        }


        private string findColumnValue_(Match match, DataRow dRow)
        {
            String columnName = match.Value.Replace("{", "").Replace("}", "");
            if (dRow[columnName] == DBNull.Value) return "0";
            return "" + dRow[columnName];
        }

        private string getTeamMemberNames(int teamNumber)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.EventNames);
            DataRow dRow = table.Rows.Find(teamNumber);
            return ""+dRow["Member_Names"];
        }


        public String _commonPageHeader(DataRow dRow)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Utilities.INDIAN_ZONE);
            String result = "Page Updated on " + indianTime.ToString() + " IST<br/>";
            result += "<a href='" + m_prefix + "leaderboard/index.html'>Round Robin Leaderboard</a>";
            result += " | <a href='" + m_prefix + "names/index.html'>Team Compositions</a><br/>";
            result += "Team Scores : ";
            for (int i = 1; i <= m_numberOfTeams; ++i)
            {

                result += (i == 1 ? "" : " | ") + "<a href='" + m_prefix + "teams" + "/team" + i + "score.html'  title = '" + getTeamMemberNames(i) + "'>" + i + "</a>";
            }
            result += "<br/>";
            result += "Round Scores : ";
            for (int i = 1; i <= m_numberOfRounds; ++i)
            {
                result += (i == 1 ? "" : " | ") + "<a href='" + m_prefix + "rounds" + "/round" + i + "score.html'>" + i + "</a>";
            }
            result += "<br/>";
            result += "Board Scores : ";
            for (int i = 1; i <= m_totalNumberOfBoards; ++i)
            {
                result += (i == 1 ? "" : " | ") + "<a href='" + m_prefix + "boards" + "/board" + i + "score.html'>" + i + "</a>";
            }
            return result;
        }

        private string getTeamLink(Object teamNumberObject, bool showNumber, bool showName)
        {
            string result = "";
            if (teamNumberObject == DBNull.Value) return "-";
            int teamNumber = (int)teamNumberObject;
            if (teamNumber <= 0 || teamNumber > m_numberOfTeams) return "BYE";
            DataRow dRow = getTable(Constants.TableName.EventNames).Rows.Find(teamNumber);
            result = "<a href='" + m_prefix + "teams" + "/team" + dRow["Team_Number"] + "score.html'  title='" + dRow["Member_Names"] + "'>" + (showNumber ? dRow["Team_Number"] + " " : "") + (showName ? dRow["Team_Name"] : "") + "</a>";
            return result;
        }

        private string getRoundNumberLink(int roundNumber)
        {
            return "<a href='" + m_prefix + "rounds" + "/round" + roundNumber + "score.html'>" + roundNumber + "</a>";
        }

        private string getBoardNumberLink(int roundNumber)
        {
            return "<a href='" + m_prefix + "boards" + "/board" + roundNumber + "score.html'>" + roundNumber + "</a>";
        }

        public String _makeTeamNumberLink(DataRow dRow)
        {
            return getTeamLink(dRow["Team_Number"], true, false);
        }

        public String _makeTeamNameLink(DataRow dRow)
        {
            return getTeamLink(dRow["Team_Number"], false, true);
        }

        public String _makeRoundNumberLink(DataRow dRow)
        {
            return "<a href='" + m_prefix + "rounds" + "/round" + dRow["Round_Number"] + "score.html'>" + dRow["Round_Number"] + "</a>";
        }
    }
}
