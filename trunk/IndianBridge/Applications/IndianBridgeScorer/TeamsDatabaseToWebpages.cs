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

namespace IndianBridgeScorer
{
    public class TeamsDatabaseToWebpages
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
            parameters.fileName = "test.html";
            parameters.filterCriteria = "Pair_Number = '1'";
            parameters.headerTemplate = "Summary for";
            parameters.tableName = "Pair_Information";
            parameters.columns = new OrderedDictionary();
            return parameters;
        }

        private DataSet m_ds = null;
        private string m_webpagesRootDirectory = "";
        private String m_prefix = "";
        private int m_roundsCompleted = 0;
        private int m_drawsCompleted = 0;
        private int m_numberOfTeams = 0;
        private int m_numberOfRounds = 0;

        public TeamsDatabaseToWebpages(DataSet ds, string webpagesRootDirectory)
        {
            m_ds = ds;
            m_webpagesRootDirectory = webpagesRootDirectory;
            if (m_ds.Tables[TeamScorer.infoTableName].Rows.Count >= 1)
            {
                DataRow dRow = m_ds.Tables[TeamScorer.infoTableName].Rows[0];
                m_numberOfTeams = (int)dRow["Number_Of_Teams"];
                m_numberOfRounds = (int)dRow["Number_Of_Rounds"];
                m_roundsCompleted = (int)dRow["Rounds_Completed"];
                m_drawsCompleted = (int)dRow["Draws_Completed"];
            }

        }

        private void printMessage(String message) { Trace.WriteLine(message); }

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
            string title = (m_roundsCompleted == m_numberOfRounds) ? "<h2>Final Leaderboard</h2>" : "<h2>Leaderboard after Round " + m_roundsCompleted+"</h2>";
            string headerTemplate = headerify_(title + "<br/>[_commonPageHeader]");
            if (m_roundsCompleted < 1)
            {
                StreamWriter sw = new StreamWriter(Path.Combine(m_webpagesRootDirectory, "index.html"));
                sw.WriteLine("<html><head></head><body>");
                sw.WriteLine(applyTemplate_(headerTemplate, null));
                sw.WriteLine("<h1>No Scores Available Yet!</h1>");
                sw.WriteLine("</body></html>");
                sw.Close();
                return;
            }
            Parameters parameters = createDefaultParameters();
            parameters.columns.Add("Rank", "{Rank_After_Round_" + m_roundsCompleted + "}");
            parameters.columns.Add("No.", "[_makeTeamNumberLink]");
            parameters.columns.Add("Name", "[_makeTeamNameLink]");
            parameters.columns.Add("VPs", "{Score_After_Round_" + m_roundsCompleted + "}");
            parameters.columns.Add("Tiebreaker Quotient", "{Tiebreaker_After_Round_" + m_roundsCompleted + "}");
            parameters.sortCriteria = "Rank_After_Round_" + m_roundsCompleted + " ASC";
            m_prefix = "../";
            parameters.fileName = Path.Combine(rootFolder, "index.html");
            parameters.filterCriteria = "";
            parameters.headerTemplate = headerTemplate;
            parameters.tableName = TeamScorer.computedScoresTableName;          
            createPage_(parameters);
            m_prefix = "./";
            parameters.fileName = Path.Combine(m_webpagesRootDirectory, "index.html");
            createPage_(parameters);
        }

        private void createNamesPage()
        {
            printMessage("Creating Team Names...");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "names");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);

            Parameters parameters = createDefaultParameters();
            parameters.columns.Add("No.", "[_makeTeamNumberLink]");
            parameters.columns.Add("Name", "[_makeTeamNameLink]");
            parameters.columns.Add("Members", "{Member_Names}");
            parameters.sortCriteria = "Team_Number ASC";
            m_prefix = "../";
            parameters.fileName = Path.Combine(rootFolder, "index.html");
            parameters.filterCriteria = "";
            parameters.headerTemplate = headerify_("<h2>Team Compositions</h2>" + "<br/>[_commonPageHeader]");
            parameters.tableName = TeamScorer.namesTableName;
            createPage_(parameters);
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

        private string getTeamMemberNames(DataRow dRow)
        {
            DataRow[] dRows = m_ds.Tables[TeamScorer.namesTableName].Select("Team_Number = " + (int)dRow["Team_Number"]);
            Debug.Assert(dRows.Length == 1);
            return (string)dRows[0]["Member_Names"];
        }

        private void writeKnockoutPage(string fileName)
        {
            DataRow[] dRows = m_ds.Tables[TeamScorer.knockoutSessionsTableName].Select("", "Round_Number ASC");
            if (dRows.Length < 1) return;
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("<html><head></head><body>");
            string title = "<h2>Knockout Scores</h2>";
            string headerTemplate = headerify_(title + "<br/>[_commonPageHeader]");
            sw.WriteLine(applyTemplate_(headerTemplate, null));
            bool scoresAvailable = false;
            foreach (DataRow dRow in dRows)
            {
                string html = getKnockoutRoundTable(dRow);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    scoresAvailable = true;
                    sw.Write(html);
                }
            }
            if (!scoresAvailable)
            {
                sw.WriteLine("<h1>No Scores Available Yet!</h1>");
            }
            sw.WriteLine("</body></html>");
            sw.Close();
        }

        public void createKnockoutPage()
        {
            printMessage("Creating Knockout Pages");
            String rootFolder = Path.Combine(m_webpagesRootDirectory, "knockout");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
            m_prefix = "../";
            string fileName = Path.Combine(rootFolder, "index.html");
            writeKnockoutPage(fileName);
            m_prefix = "./";
            fileName = Path.Combine(m_webpagesRootDirectory, "index.html");
            writeKnockoutPage(fileName);
        }

        private string getKnockoutRoundTable(DataRow dRow)
        {
            string html = "";
            string roundName = (string)dRow["Round"];
            DataTable table = m_ds.Tables[roundName];
            Debug.Assert(table != null);
            if (Utilities.AllNull(table, "Total_IMPs")) return "";
            int numberOfMatches = (int)Math.Pow(2,(int)dRow["Round_Number"]);
            int numberOfSessions = (int)dRow["Number_Of_Sessions"];
            html+=(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            ArrayList tableRow = new ArrayList();
            tableHeader.Add(roundName);
            html += (Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            for (int i = 1; i <= numberOfMatches/2; ++i)
            {
                tableRow.Clear();
                DataRow[] foundRows = table.Select("Match_Number = " + i);
                Debug.Assert(foundRows.Length == 2);
                tableRow.Add(createMatchTable(foundRows,numberOfSessions));
                html += ("<tr>" + Utilities.makeTableCell_(tableRow, 1) + "</tr>");
            }
            html+=("</tbody></table>");
            return html;
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
                tableRow.Add(""+getValue(dRows[j], "Carryover"));
                for (int k = 1; k <= numberOfSessions; ++k)
                {
                    string columnName = "Session_" + k + "_Score";
                    double sessionScore = getValue(dRows[j],columnName);
                    double otherSessionScore = getValue(dRows[otherRow], columnName);
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
            DataRow[] foundRows = m_ds.Tables[TeamScorer.namesTableName].Select("Team_Number = "+teamNumber);
            Debug.Assert(foundRows.Length == 1);
            DataRow dRow = foundRows[0];
            string headerTemplate = headerify_("<h2>Scores for " + teamNumber + " : {Team_Name} ({Member_Names})</h2>" + "<br/>[_commonPageHeader]");
            sw.WriteLine(applyTemplate_(headerTemplate, dRow));

            sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Add("Round");
            tableHeader.Add("Opponent");
            tableHeader.Add("VPs");
            tableHeader.Add("Adjustment");
            tableHeader.Add("Cumulative Score After Round");
            tableHeader.Add("Rank After Round");
            sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            for (int i = 1; i <= m_roundsCompleted; ++i)
            {
                ArrayList tableRow = new ArrayList();
                tableRow.Add(getRoundNumberLink(i));
                foundRows = m_ds.Tables[TeamScorer.scoresTableName].Select("Round_Number = " + i + " AND Team_1_Number = " + teamNumber);
                if (foundRows.Length > 0)
                {
                    dRow = foundRows[0];
                    int opponent = (int)dRow["Team_2_Number"];
                    if (opponent == 0 || opponent > m_numberOfTeams) tableRow.Add("BYE");
                    else tableRow.Add("" + getTeamLink(dRow["Team_2_Number"],true,true));
                    tableRow.Add("" + getValue(dRow,"Team_1_VPs"));
                    tableRow.Add("" + getValue(dRow,"Team_1_VP_Adjustment"));
                }
                else
                {
                    foundRows = m_ds.Tables[TeamScorer.scoresTableName].Select("Round_Number = " + i + " AND Team_2_Number = " + teamNumber);
                    if (foundRows.Length < 1)
                    {
                        tableRow.Add("-");
                        tableRow.Add("-");
                        tableRow.Add("-");
                    }
                    else
                    {
                        dRow = foundRows[0];
                        int opponent = (int)dRow["Team_1_Number"];
                        if (opponent == 0 || opponent > m_numberOfTeams) tableRow.Add("BYE");
                        else tableRow.Add("" + getTeamLink(dRow["Team_1_Number"], true, true));
                        tableRow.Add("" + getValue(dRow,"Team_2_VPs"));
                        tableRow.Add("" + getValue(dRow,"Team_2_VP_Adjustment"));
                    }
                }
                foundRows = m_ds.Tables[TeamScorer.computedScoresTableName].Select("Team_Number = " + teamNumber);
                Debug.Assert(foundRows.Length <= 1);
                if (foundRows.Length < 1)
                {
                    tableRow.Add("-");
                    tableRow.Add("-");
                }
                else
                {
                    dRow = foundRows[0];
                    string columnName = "Score_After_Round_"+i;
                    if (dRow.Table.Columns.Contains(columnName)) tableRow.Add(""+getValue(dRow,columnName));
                    else tableRow.Add("-");
                    columnName = "Rank_After_Round_"+i;
                    if (dRow.Table.Columns.Contains(columnName)) tableRow.Add("" + getValue(dRow, columnName));
                    else tableRow.Add("-");
                }
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
            }
            for (int i = m_roundsCompleted + 1; i <= m_numberOfRounds; ++i)
            {
                ArrayList tableRow = new ArrayList();
                tableRow.Add("" + i);
                tableRow.Add("-");
                tableRow.Add("-");
                tableRow.Add("-");
                tableRow.Add("-");
                tableRow.Add("-");
                sw.WriteLine("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
            }
            sw.WriteLine("</tbody></table>");
            sw.WriteLine("</body></html>");
            sw.Close();
        }

        private double getValue(DataRow dRow, string columnName)
        {
            Object value = dRow[columnName];
            return (value == DBNull.Value) ? 0 : (double)value;
        }

        private String headerify_(String text)
        {
            return text;
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
            DataRow[] foundRows = m_ds.Tables[TeamScorer.scoresTableName].Select("Round_Number = " + roundNumber, "Table_Number ASC");
            DataRow dRow = (foundRows.Length>0)?foundRows[0]:null;
            string headerTemplate = headerify_("<h2>Scores for Round " + roundNumber + "</h2>" + "<br/>[_commonPageHeader]");
            sw.WriteLine(applyTemplate_(headerTemplate, dRow));
            if (roundNumber > m_roundsCompleted)
            {
                if (roundNumber <= m_drawsCompleted)
                {
                    sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
                    ArrayList tableHeader = new ArrayList();
                    tableHeader.Clear();
                    tableHeader.Add("Round " + roundNumber + " Draw");
                    sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
                    sw.WriteLine("<tr><td>");
                    sw.WriteLine(createRoundTable(roundNumber, foundRows,false));
                    sw.WriteLine("</td></tr></tbody></table>");
                    sw.WriteLine("</tbody></table>");
                }
                else sw.WriteLine("<h2>No Draw or Score published yet!</h2>");
            }
            else
            {
                sw.WriteLine(Utilities.makeTablePreamble_() + "<thead><tr>");
                ArrayList tableHeader = new ArrayList();
                tableHeader.Clear();
                tableHeader.Add("Round "+roundNumber+" Score");
                tableHeader.Add("Cumulative Score and Ranking after Round " + roundNumber);
                sw.WriteLine(Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
                sw.WriteLine("<tr><td>");
                sw.WriteLine(createRoundTable(roundNumber,foundRows));
                sw.WriteLine("</td><td>");
                Parameters parameters = createDefaultParameters();
                parameters.columns.Add("Rank", "{Rank_After_Round_" + roundNumber + "}");
                parameters.columns.Add("No.", "[_makeTeamNumberLink]");
                parameters.columns.Add("Name", "[_makeTeamNameLink]");
                parameters.columns.Add("VPs", "{Score_After_Round_" + roundNumber + "}");
                parameters.columns.Add("Tiebreaker Quotient", "{Tiebreaker_After_Round_" + roundNumber + "}");
                parameters.sortCriteria = "Rank_After_Round_" + roundNumber + " ASC";
                parameters.filterCriteria = "";
                parameters.headerTemplate = headerTemplate;
                parameters.tableName = TeamScorer.computedScoresTableName;          

                sw.WriteLine(createTable_(parameters));
                sw.WriteLine("</td></tr></tbody></table>");
                sw.WriteLine("</tbody></table>");
            }
            sw.WriteLine("</body></html>");
            sw.Close();
        }

        private string createRoundTable(int roundNumber, DataRow[] foundRows, bool showVPs= true)
        {
            string result = "";
            result += (Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            tableHeader.Clear();
            tableHeader.Add("Table");
            tableHeader.Add("Team 1");
            if (showVPs)
            {
                tableHeader.Add("Team 1 VPs");
                tableHeader.Add("Team 2 VPs");
            }
            tableHeader.Add("Team 2");
            result += (Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            int i = 0;
            foreach (DataRow dRow in foundRows)
            {
                ArrayList tableRow = new ArrayList();
                tableRow.Add("" + (int)dRow["Table_Number"]);
                string team1string = getTeamLink(dRow["Team_1_Number"], true, true);
                tableRow.Add(team1string);
                string team2string = getTeamLink(dRow["Team_2_Number"], true, true);
                if (showVPs)
                {
                    if (team1string == "BYE")
                    {
                        tableRow.Add("BYE");
                    }
                    else
                    {
                        double team1vps = getValue(dRow, "Team_1_VPs") + getValue(dRow, "Team_1_VP_Adjustment");
                        tableRow.Add("" + team1vps);
                    }
                    if (team2string == "BYE")
                    {
                        tableRow.Add("BYE");
                    }
                    else
                    {
                        double team2vps = getValue(dRow, "Team_2_VPs") + getValue(dRow, "Team_2_VP_Adjustment");
                        tableRow.Add("" + team2vps);
                    }
                }
                tableRow.Add(team2string);
                result += ("<tr>" + Utilities.makeTableCell_(tableRow, i++) + "</tr>");
            }
            result += "</tbody></table>";
            return result;

        }

        public void createWebpages_()
        {
            String rootFolder = m_webpagesRootDirectory;
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
            createKnockoutPage();
            createLeaderboard();
            createNamesPage();
            createTeamPages();
            createRoundPages();
        }

        private string createTable_(Parameters parameters)
        {
            string result = "";
            DataRow[] foundRows = m_ds.Tables[parameters.tableName].Select(parameters.filterCriteria, parameters.sortCriteria);
            DataRow dRow = (foundRows.Length < 1) ? null : foundRows[0];

            result += (Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            foreach (DictionaryEntry pair in parameters.columns)
            {
                tableHeader.Add(applyTemplate_("" + pair.Key, dRow));
            }
            result += (Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
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
                result += ("<tr>" + Utilities.makeTableCell_(tableRow, i) + "</tr>");
            }
            result += ("</tbody></table>");
            return result;
        }

        private void createPage_(Parameters parameters)
        {
            StreamWriter sw = new StreamWriter(parameters.fileName);
            sw.WriteLine("<html><head></head><body>");
            DataRow[] foundRows = m_ds.Tables[parameters.tableName].Select(parameters.filterCriteria, parameters.sortCriteria);
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


        private String findColumnValue_(Match match, DataRow dRow)
        {
            String columnName = match.Value.Replace("{", "").Replace("}", "");
            return "" + dRow[columnName];
        }


        public String _commonPageHeader(DataRow dRow)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Utilities.INDIAN_ZONE);
            String result = "Page Updated on " + indianTime.ToString() + " IST<br/>";
            result += "<a href='" + m_prefix + "knockout/index.html'>Knockout</a>";
            result += " | <a href='" + m_prefix + "leaderboard/index.html'>Round Robin Leaderboard</a>";
            result += " | <a href='" + m_prefix + "names/index.html'>Team Compositions</a><br/>";
            result += "Team Scores : ";
            for (int i = 1; i <= m_numberOfTeams; ++i)
            {
                result += (i == 1 ? "" : " | ") + "<a href='" + m_prefix + "teams" + "/team" + i + "score.html'>" + i + "</a>";
            }
            result += "<br/>";
            result += "Round Scores : ";
            for (int i = 1; i <= m_numberOfRounds; ++i)
            {
                result += (i == 1 ? "" : " | ") + "<a href='" + m_prefix + "rounds" + "/round" + i + "score.html'>" + i + "</a>";
            }
            return result;
        }

        private string getTeamLink(Object teamNumberObject, bool showNumber, bool showName)
        {
            string result = "";
            if (teamNumberObject == DBNull.Value) return "-";
            int teamNumber = (int)teamNumberObject;
            if (teamNumber <= 0 || teamNumber > m_numberOfTeams) return "BYE";
            DataRow[] dRows = m_ds.Tables[TeamScorer.namesTableName].Select("Team_Number = " + teamNumber);
            Debug.Assert(dRows.Length == 1);
            DataRow dRow = dRows[0];
            result = "<a href='" + m_prefix + "teams" + "/team" + dRow["Team_Number"] + "score.html'>" + (showNumber?dRow["Team_Number"]+" ":"") + (showName?dRow["Team_Name"]:"") + "</a>";
            return result;
        }

        private string getRoundNumberLink(int roundNumber)
        {
            return "<a href='" + m_prefix + "rounds" + "/round" + roundNumber + "score.html'>" + roundNumber + "</a>";
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
