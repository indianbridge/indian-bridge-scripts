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
    public class KnockoutTeamsDatabaseToWebpages
    {
        private string m_databaseFileName = "";
        private string m_webpagesRootDirectory = "";
        //private string m_prefix = "";
        private int totalNumberOfPagesToBeCreated = 0;
        private int numberOfPagesCreatedSoFar = 0;
        private BackgroundWorker m_worker;
        private bool m_runningInBackground = false;
        private string m_prefixString = "Creating Local Webpages : ";

        public KnockoutTeamsDatabaseToWebpages(string eventName, string databaseFileName, string webpagesRoot)
        {
            m_databaseFileName = databaseFileName;
            m_webpagesRootDirectory = webpagesRoot;
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
            totalNumberOfPagesToBeCreated = 1;
            string rootFolder = m_webpagesRootDirectory;
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
            createKnockoutPage();
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
        private String findColumnValue_(Match match, DataRow dRow)
        {
            String columnName = match.Value.Replace("{", "").Replace("}", "");
            return "" + dRow[columnName];
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

        
        private void writeKnockoutPage(string fileName)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            DataRow[] dRows = table.Select("", "Round_Number ASC");
            if (dRows.Length < 1) return;
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("<html><head></head><body>");
            string title = "<h2>Knockout Scores</h2>";
            string headerTemplate = title + "<br/>[_commonPageHeader]";
            sw.WriteLine(applyTemplate_(headerTemplate, null));
            bool scoresAvailable = false;
            foreach (DataRow dRow in dRows)
            {
                string html = getKnockoutRoundTable(dRow);
                if (!string.IsNullOrWhiteSpace(html))
                {
                    scoresAvailable = true;
                    sw.Write(html+Environment.NewLine);
                    sw.Write("<hr/>" + Environment.NewLine);
                }
            }
            if (!scoresAvailable)
            {
                sw.WriteLine("<h1>No Scores Available Yet!</h1>");
            }
            sw.WriteLine("</body></html>");
            sw.Close();
        }

        public String _commonPageHeader(DataRow dRow)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Utilities.INDIAN_ZONE);
            String result = "Page Updated on " + indianTime.ToString() + " IST<br/>";
            return result;
        }

        public void createKnockoutPage()
        {
            printMessage("Creating Knockout Pages");
            if (!Directory.Exists(m_webpagesRootDirectory)) Directory.CreateDirectory(m_webpagesRootDirectory);
            string fileName = Path.Combine(m_webpagesRootDirectory, "index.html");
            writeKnockoutPage(fileName);
        }

        private string getKnockoutRoundTable(DataRow dRow)
        {
            string html = "";
            int roundNumber = (int)dRow["Round_Number"];
            string roundName = (string)dRow["Round_Name"];
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
            Debug.Assert(table != null);
            if (Utilities.AllNull(table, "Total")) return "";
            int numberOfMatches = (int)Math.Pow(2,roundNumber);
            int numberOfSessions = (int)dRow["Number_Of_Sessions"];
            html+=(Utilities.makeTablePreamble_() + "<thead><tr>");
            ArrayList tableHeader = new ArrayList();
            ArrayList tableRow = new ArrayList();
            tableHeader.Add(roundName);
            html += (Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody><tr>");
            html += (Utilities.makeTablePreamble_() + "<thead><tr>");
            tableHeader = new ArrayList();
            tableHeader.Add("Match No.");
            tableHeader.Add("Team");
            tableHeader.Add("Carryover");
            for (int i = 1; i <= numberOfSessions; ++i)
            {
                tableHeader.Add("Session " + i);
            }
            tableHeader.Add("Total");
            html += (Utilities.makeTableHeader_(tableHeader) + "</tr></thead><tbody>");
            for (int i = 1; i <= numberOfMatches/2; ++i)
            {
                DataRow[] foundRows = table.Select("Match_Number = " + i);
                Debug.Assert(foundRows.Length == 2);
                html += createMatchRows(foundRows, numberOfSessions,i, i!=1);
            }
            html+=("</tbody></table>");
            return html;
        }

        private string addEmptyRows(int numberOfSessions)
        {
            string html = "";
            html += "<tr>";
            for (int i = 0; i < 4 + numberOfSessions; ++i) html += "<td style='background-color:#acf;height=1em'></td>";
            html += "</tr>" + Environment.NewLine;
            return html;
        }

        private string createMatchRows(DataRow[] dRows, int numberOfSessions, int matchNumber, bool addExtraRowInFront = false)
        {
            string html = "";
            if (addExtraRowInFront)
            {
                html += addEmptyRows(numberOfSessions);
            }
            for (int j = 0; j < 2; ++j)
            {
                html += ("<tr>");
                int otherRow = 1 - j;
                int teamNumber = (int)dRows[j]["Team_Number"];
                double total = (double)dRows[j]["Total"];
                double otherTotal = (double)dRows[otherRow]["Total"];
                if (j == 0) html += Utilities.makeTableCell_("" + matchNumber, 1, false, 2);
                int rowIndex = (total > otherTotal) ? 0 : 1;
                if (total > otherTotal) html += Utilities.makeTableCell_("<b>" + getTeamLink(teamNumber, true, true) + "</b>",rowIndex);
                else html += Utilities.makeTableCell_(getTeamLink(teamNumber, true, true), rowIndex);
                html += Utilities.makeTableCell_("" + AccessDatabaseUtilities.getDoubleValue(dRows[j], "Carryover"), rowIndex);
                for (int k = 1; k <= numberOfSessions; ++k)
                {
                    string columnName = "Session_" + k + "_Score";
                    double sessionScore = AccessDatabaseUtilities.getDoubleValue(dRows[j], columnName);
                    double otherSessionScore = AccessDatabaseUtilities.getDoubleValue(dRows[otherRow], columnName);
                    if (sessionScore > otherSessionScore) html += Utilities.makeTableCell_("<b>" + sessionScore + "</b>", rowIndex);
                    else html += Utilities.makeTableCell_("" + sessionScore, rowIndex);
                }
                if (total > otherTotal) html += Utilities.makeTableCell_("<b>" + total + "</b>", rowIndex);
                else html += Utilities.makeTableCell_("" + total, rowIndex);
                html += ("</tr>" + Environment.NewLine);
            }     
            return html;
        }

        private string getTeamLink(Object teamNumberObject, bool showNumber, bool showName)
        {
            string result = "";
            if (teamNumberObject == DBNull.Value) return "-";
            int teamNumber = (int)teamNumberObject;
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutTeams);
            DataRow[] dRows = table.Select("Team_Number = " + teamNumber);
            Debug.Assert(dRows.Length == 1);
            DataRow dRow = dRows[0];
            string originalEventName = AccessDatabaseUtilities.getStringValue(dRow,"Original_Event_Name");
            if (string.IsNullOrWhiteSpace(originalEventName))
            {
                result = "<span title='" + dRow["Member_Names"] + "' >" + (showNumber ? dRow["Team_Number"] + " " : "") + (showName ? dRow["Team_Name"] : "") + "</span>";
            }
            else
            {
                int originalTeamNumber = AccessDatabaseUtilities.getIntValue(dRow, "Original_Team_Number");
                //string webpagesRootDirectory = Path.Combine("..", "..", Constants.WebpagesFolderName, Utilities.makeIdentifier_(originalEventName));
                //string link = Path.Combine(webpagesRootDirectory, "teams", "team" + originalTeamNumber + "score.html");
                string webpagesRootDirectory = "../"+Utilities.makeIdentifier_(originalEventName);
                string link = webpagesRootDirectory+ "/teams/team" + originalTeamNumber + "score.html";
                result = "<a href='" + link + "' title='" + dRow["Member_Names"] + "'>" + (showNumber ? dRow["Team_Number"] + " " : "") + (showName ? dRow["Team_Name"] : "") + "</a>";
            }
            return result;
        }


    }
}
