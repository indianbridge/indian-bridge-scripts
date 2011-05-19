﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;

namespace Upload_To_Google_Sites
{
    public class SpreadSheetAPI
    {
        static public String APP_NAME = "BridgeTeamEventScoreManager-SpreadsheetAPI-v0.1";
        private SpreadsheetsService service = null;
        private SpreadsheetEntry spreadsheet = null;
        private string spreadsheetname = null;
        private Hashtable worksheets = new Hashtable();
        private NameValueCollection info = new NameValueCollection();
        private uint numTeams = 0;
        private uint numRounds = 0;
        private uint numMatches = 0;
        public SpreadSheetAPI(String spreadsheetname,String username, String password, Boolean debug_flag)
        {
            this.service = new SpreadsheetsService(APP_NAME);
            this.service.setUserCredentials(username, password);
            this.spreadsheetname = spreadsheetname;
            initialize(debug_flag);
        }
        private void initialize(Boolean debug)
        {
            findSpreadsheet(debug);
            getAllWorksheets(debug);
            info = getInfo(debug);
            numTeams = uint.Parse(info["Number of Teams"]);
            numRounds = uint.Parse(info["Number of Rounds"]);
            numMatches = (uint)(numTeams / 2 + (numTeams % 2 == 0 ? 0 : 1));
            if (debug)
            {
                Console.WriteLine("Event Name = " + info["Event Name"]);
                Console.WriteLine("Number of Teams = " + numTeams);
                Console.WriteLine("Number of Rounds = " + numRounds);
                Console.WriteLine("Number of Matches = " + numMatches);
            }
        }
        public NameValueCollection getInfo(Boolean debug = false)
        {
            NameValueCollection parameters = new NameValueCollection();
            WorksheetEntry entry = (WorksheetEntry)worksheets["Info"];
            AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery query = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed feed = service.Query(query);

            foreach (ListEntry worksheetRow in feed.Entries)
            {
                ListEntry.CustomElementCollection elements = worksheetRow.Elements;
                parameters.Add(elements[0].Value, elements[1].Value);
            }
            return parameters;
        }
        public NameValueCollection getTeamNames(Boolean debug_flag=false)
        {
            NameValueCollection names = new NameValueCollection();
            WorksheetEntry entry = (WorksheetEntry)worksheets["Names"];
            /*AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            AtomLink link = spreadsheet.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);
            WorksheetQuery wquery = new WorksheetQuery(link.HRef.ToString());
            wquery.Title = "Info";
            WorksheetFeed wfeed = service.Query(wquery);
            WorksheetEntry namesSheet = null;
            WorksheetEntry infoSheet = null;
            if (wfeed.Entries.Count > 0)
            {
                infoSheet = wfeed.Entries[0] as WorksheetEntry;
            }
                else
                {
                    String message = "Cannot find sheet titled Info!!!";
                    if (debug_flag) Console.WriteLine(message);
                    throw new ArgumentNullException(message);
                }

                wquery.Title = "Names";
                wfeed = service.Query(wquery);
                if (wfeed.Entries.Count > 0)
                {
                    namesSheet = wfeed.Entries[0] as WorksheetEntry;
                }
                else
                {
                    String message = "Cannot find sheet titled Names!!!";
                    if (debug_flag) Console.WriteLine(message);
                    throw new ArgumentNullException(message);
                }
                int numTeams = 0;
                AtomLink listFeedLink = infoSheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
                ListQuery iquery = new ListQuery(listFeedLink.HRef.ToString());
                ListFeed ifeed = service.Query(iquery) as ListFeed;
                foreach (ListEntry worksheetRow in ifeed.Entries)
                {
                    ListEntry.CustomElementCollection elements = worksheetRow.Elements;
                    String parameterName = elements[0].Value;
                    if (parameterName.ToLower() == "Number of Teams".ToLower())
                    {
                        numTeams = int.Parse(elements[1].Value);
                        if (debug_flag) Console.WriteLine("Num Teams = " + numTeams);
                    }
                }*/
                AtomLink cellFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);
                CellQuery cquery = new CellQuery(cellFeedLink.HRef.ToString());
                cquery.ReturnEmpty = ReturnEmptyCells.yes;
                cquery.MinimumColumn = 1;
                cquery.MaximumColumn = 2;
                cquery.MinimumRow = 2;
                cquery.MaximumRow = (uint)(numTeams+1);
                CellFeed cfeed = service.Query(cquery);
                for (int i = 0; i < cfeed.Entries.Count; i += 2)
                {
                    CellEntry nameCell = cfeed.Entries[i + 1] as CellEntry;
                    CellEntry numberCell = cfeed.Entries[i] as CellEntry;
                    names.Add(nameCell.Cell.Value,numberCell.Cell.Value);
                }
            return names;
        }
        private void findSpreadsheet(Boolean debug_flag=false)
        {
            SpreadsheetQuery query = new SpreadsheetQuery();
            query.Title = spreadsheetname;
            if (debug_flag) Console.WriteLine("Trying to find spreadsheet with name " + spreadsheetname);
            SpreadsheetFeed feed = service.Query(query);
            if (feed.Entries.Count > 0)
            {
                spreadsheet = (SpreadsheetEntry)feed.Entries[0];
                if (debug_flag) Console.WriteLine("Found spreadsheet with name " + spreadsheet.Title.Text);
            }
            else
            {
                if (debug_flag) Console.WriteLine("No Spreadsheet with name " + spreadsheetname+" could be found!!!");
                throw new ArgumentNullException("Spreadsheet with name " + spreadsheetname+" not found!!!");
            }
        }
        private void getAllWorksheets(Boolean debug_flag=false)
        {
            AtomLink link = spreadsheet.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);

            WorksheetQuery query = new WorksheetQuery(link.HRef.ToString());
            WorksheetFeed feed = service.Query(query);

            if (debug_flag) Console.WriteLine("Finding Worksheets. Found...");
            foreach (WorksheetEntry worksheet in feed.Entries)
            {
                if(debug_flag) Console.WriteLine(worksheet.Title.Text);
                worksheets.Add(worksheet.Title.Text,worksheet);
            }

        }
        public void updateScores(String filename,Boolean debug=false)
        {
            var text = File.ReadAllText(filename);
            var re = new Regex("<pre>(.*?)</pre>", RegexOptions.IgnoreCase|RegexOptions.Singleline);
            MatchCollection MatchList = re.Matches(text);
            String scores = MatchList[0].Groups[1].Value;
            re = new Regex("Round:\\s+(\\w)",RegexOptions.IgnoreCase|RegexOptions.Singleline);
            MatchList = re.Matches(scores);
            int roundNumber = int.Parse(MatchList[0].Groups[1].Value);
            char[] delimiters = new char[] { '\n','\r' };
            string[] lines = scores.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length-2 != numMatches) throw new ArgumentOutOfRangeException("Number of Scores lines " + (lines.Length - 2) + " does not correspond with number of matches " + numMatches);
            int[,] scoreData = new int[numMatches,6];
            for (int i = 2; i < lines.Length; ++i)
            {
                readOneScoreLine(lines,i,scoreData);
            }
            updateScores(roundNumber, scoreData,debug);
        }
        private void readOneScoreLine(String []lines,int row,int[,]scores)
        {
            var text = lines[row];
            scores[row - 2, 4] = 0;
            scores[row - 2, 5] = 0;
            String team1NoString = text.Substring(5, 2).Replace("-", "").Trim();
            scores[row-2,0] = ((team1NoString.Length == 0) ? 0 : int.Parse(team1NoString));
            String team2NoString = text.Substring(19, 2).Replace("-", "").Trim();
            scores[row - 2, 1] = ((team2NoString.Length == 0) ? 0 : int.Parse(team2NoString));
            String team1VPString = text.Substring(40, 3).Trim();
            scores[row - 2, 2] = ((team1VPString.Length == 0) ? 0 : int.Parse(team1VPString));
            String team2VPString = text.Substring(43).Trim();
            scores[row - 2, 3] = ((team2VPString.Length == 0) ? 0 : int.Parse(team2VPString));
            //Console.WriteLine(team1Number + " , " + team2Number + " , " + team1VPScore + " , " + team2VPScore);
        }
        public void updateScores(int roundNumber, DataTable scores,Boolean debug=false)
        {
            var totalRows = scores.Rows.Count;
            if (totalRows != numMatches) throw new ArgumentOutOfRangeException("Number of Rows in scores object should be same as number of matches!!!");
            DataRow row1 = scores.Rows[0];
            if (!row1.Table.Columns.Contains("TableNumber") || !row1.Table.Columns.Contains("HomeTeamNumber") || !row1.Table.Columns.Contains("AwayTeamNumber") || !row1.Table.Columns.Contains("HomeTeam") || !row1.Table.Columns.Contains("AwayTeam") || !row1.Table.Columns.Contains("IMPScore") || !row1.Table.Columns.Contains("VPScore")) throw new ArgumentOutOfRangeException("DataTable does not have all requisite columns!!!");

            int[,] computedScores = new int[numMatches, 6];
            uint i = 0;
            String value = "";
            foreach (DataRow row in scores.Rows)
            {
                uint j = 0;
                value = row["HomeTeamNumber"].ToString().Trim().ToLower();
                computedScores[i, j++] = ((value == "" || value == "bye" || value == "--") ? 0 : int.Parse(value));
                value = row["AwayTeamNumber"].ToString();
                computedScores[i, j++] = ((value == "" || value == "bye" || value == "--") ? 0 : int.Parse(value));
                value = row["VPScore"].ToString();
                char[] delimiters = new char[] {' ','\t','-'};
                String[] values = value.Split(delimiters);
                int index = 0;
                computedScores[i, j++] = ((computedScores[i, 0] == 0) ? 0 : int.Parse(values[index++]));
                computedScores[i, j++] = ((computedScores[i, 1] == 0) ? 0 : int.Parse(values[index++]));
                computedScores[i, j++] = 0;
                computedScores[i, j++] = 0;
                i++;
            }
            updateScores(roundNumber, computedScores,debug);
        }

        public void updateScores(int roundNumber, int[,] scores,Boolean debug=false)
        {
            if (numMatches != scores.GetUpperBound(0) + 1) throw new ArgumentOutOfRangeException("Number of Rows in scores object should be same as number of matches!!!");
            if (6 != scores.GetUpperBound(1) + 1) throw new ArgumentOutOfRangeException("Number of Columns in scores object should be 6!!!");
            WorksheetEntry entry = (WorksheetEntry)worksheets["Scores"];
            AtomLink cellFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);

            CellQuery query = new CellQuery(cellFeedLink.HRef.ToString());
            query.ReturnEmpty = ReturnEmptyCells.yes;
            query.MinimumColumn = 3;
            query.MaximumColumn = 8;
            query.MinimumRow = (uint)(2+(roundNumber-1)*numMatches);
            query.MaximumRow = (uint)(1 + (roundNumber) * numMatches);

            CellFeed feed = service.Query(query);
            AtomFeed batchFeed = new AtomFeed(feed);
            for (uint i = 0; i <= scores.GetUpperBound(0); ++i)
            {
                for (uint j = 0; j <= scores.GetUpperBound(1); ++j)
                {
                    int index = (int)(i * (scores.GetUpperBound(1)+1) + j);
                    CellEntry cellEntry = (CellEntry)feed.Entries[index];
                    cellEntry.Cell.InputValue = ""+scores[i, j];
                    cellEntry.BatchData = new GDataBatchEntryData("" + index, GDataBatchOperationType.update);
                    batchFeed.Entries.Add(cellEntry);
                }
            }
            CellFeed batchResultFeed = (CellFeed)service.Batch(batchFeed, new Uri(feed.Batch));
            updateUpdateProgress(roundNumber,debug);

        }
        private void updateUpdateProgress(int roundNumber,Boolean debug_flag=false)
        {
            WorksheetEntry entry = (WorksheetEntry)worksheets["Update Progress"];
            AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery query = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed feed = service.Query(query);

            foreach (ListEntry worksheetRow in feed.Entries)
            {
                ListEntry.CustomElementCollection elements = worksheetRow.Elements;
                String parameterName = elements[0].Value;
                if (parameterName.ToLower() == "Rounds Completed".ToLower())
                {
                    int value = int.Parse(worksheetRow.Elements[1].Value);
                    if (value > roundNumber - 1)
                    {
                        worksheetRow.Elements[1].Value = "" + (roundNumber - 1);
                        worksheetRow.Update();
                        if (debug_flag) Console.WriteLine("Changing Rounds Completed to = " + (roundNumber - 1));
                    }
                }
                else if (parameterName.ToLower() == "Draws Completed".ToLower())
                {
                    int value = int.Parse(worksheetRow.Elements[1].Value);
                    if (value > roundNumber - 1)
                    {
                        worksheetRow.Elements[1].Value = "" + (roundNumber - 1);
                        worksheetRow.Update();
                        if (debug_flag) Console.WriteLine("Changing Draws Completed to = " + (roundNumber - 1));
                    }
                }

            }
        }
        /*private void getInfo()
        {
            WorksheetEntry entry = (WorksheetEntry)worksheets["Info"];
            AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery query = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed feed = service.Query(query);

            foreach (ListEntry worksheetRow in feed.Entries)
            {
                ListEntry.CustomElementCollection elements = worksheetRow.Elements;
                String parameterName = elements[0].Value;
                if (parameterName.ToLower() == "Number of Teams".ToLower())
                {
                    numTeams = uint.Parse(elements[1].Value);
                    if(debug_flag) Console.WriteLine("Num Teams = " + numTeams);
                }
                else if (parameterName.ToLower() == "Number of Rounds".ToLower())
                {
                    numRounds = uint.Parse(elements[1].Value);
                    if (debug_flag) Console.WriteLine("Num Rounds = " + numRounds);
                }

            }
            numMatches = (uint)(numTeams / 2 + (numTeams % 2 == 0 ? 0 : 1));
            if (debug_flag) Console.WriteLine("Num Matches = " + numMatches);
        }*/


    }
}
