using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Collections.Specialized;


using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Upload_To_Google_Sites
{
    class Upload_To_Google_Sites
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage is Upload_To_Google_Sites sitename username password [debug_flag]. Press any key to exit...");
                Console.ReadLine();
                return;
            }
            String sitename = args[0];
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            Boolean debug_flag = false;
            if (args.Length > 1) debug_flag = Boolean.Parse(args[1]);
            //NameValueCollection test = getTeamNames("Copy of Results_Template");
            //Console.WriteLine("Team Num = "+test["Team 3"]);
            //SitesTest(sitename, username, password, debug_flag);
            SpreadsheetTest(sitename, username, password, debug_flag);
            Console.WriteLine("Exitting.");
            Console.ReadLine();
        }

        static NameValueCollection getTeamNames(String spreadsheetname,Boolean debug_flag=false)
        {
            NameValueCollection names = new NameValueCollection();
            SpreadsheetsService service = new SpreadsheetsService("GetNames-v0.1");
            service.setUserCredentials("indianbridge.dummy@gmail.com", "kibitzer");
            SpreadsheetQuery query = new SpreadsheetQuery();
            query.Title = spreadsheetname;
            SpreadsheetFeed feed = service.Query(query);
            if (feed.Entries.Count > 0)
            {
                SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
                if (debug_flag) Console.WriteLine("Found spreadsheet with name " + spreadsheet.Title.Text);
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
                }
                AtomLink cellFeedLink = namesSheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);

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

            }
            else
            {
                String message = "No Spreadsheet with name " + spreadsheetname + " could be found!!!";
                if (debug_flag) Console.WriteLine(message);
                throw new ArgumentNullException(message);
            }


            return names;
        }
        static void SpreadsheetTest(String spreadsheetname,String username,String password,Boolean debug_flag)
        {
            SpreadSheetAPI sa = new SpreadSheetAPI(spreadsheetname,username,password,debug_flag);
            DataTable currentScore = new DataTable();
            currentScore.Columns.Add("TableNumber", typeof(System.String));
            currentScore.Columns.Add("HomeTeamNumber", typeof(System.String));
            currentScore.Columns.Add("HomeTeam", typeof(System.String));
            currentScore.Columns.Add("AwayTeamNumber", typeof(System.String));
            currentScore.Columns.Add("AwayTeam", typeof(System.String));
            currentScore.Columns.Add("IMPScore", typeof(System.String));
            currentScore.Columns.Add("VPScore", typeof(System.String));
            currentScore.Rows.Add("1", "", "", "1", "India", "BYE", "18");
            currentScore.Rows.Add("2", "5", "Delhi", "2", "Pakistan", "28-12", "22 8");
            currentScore.Rows.Add("3", "4", "Sri Lanka", "3", "Bangladesh", "6-11", "13 17");
            sa.updateScores(15,currentScore);
            //sa.updateScores(@"C:\Documents and Settings\snarasim\My Documents\Downloads\CurrentStanding\magiccontest-o-1.htm");
        }
        static void SitesTest(String sitename,String username,String password,Boolean debug_flag)
        {
            SitesAPI sa = null;
            try
            {
                sa = new SitesAPI(sitename,username,password, debug_flag);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.ToString());
            }
            String line = "yes";
            while (line != null)
            {
                try
                {
                    sa.uploadDirectory(@"C:\Documents and Settings\snarasim\My Documents\Downloads\Output", "/test");
                }
                catch (Exception e) { Console.WriteLine("Exception : " + e.ToString()); }
                Console.WriteLine("Press Ctrl+Z to quit, Enter to run again.");
                line = Console.ReadLine();
            }
        }
    }
}
