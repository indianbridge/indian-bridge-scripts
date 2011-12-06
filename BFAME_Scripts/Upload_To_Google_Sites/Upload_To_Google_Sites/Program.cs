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
using System.Text.RegularExpressions;

namespace Upload_To_Google_Sites
{
    class Upload_To_Google_Sites
    {
        static void Main(string[] args)
        {
            /*MediaService service = new MediaService("jotspot", "test");
            service.setUserCredentials("indianbridge.dummy@gmail.com", "kibitzer");
            String url = "https://sites.google.com/feeds/content/site/srirambridgetest?path=/open";
            AtomEntry entry = service.Get(url);
            if (entry == null)
            {
                Console.WriteLine("Not Found");
                return;
            }
            AtomContent newContent = new AtomContent();
            newContent.Type = "html";
            newContent.Content = "Updated Content";
            entry.Content = newContent;
            string title = IndianBridge.Common.Utility.ToCamelCase("open");
            entry.Title.Text = title;
            service.Update(entry);*/


            /*if (args.Length < 1)
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
            SitesTest(sitename, username, password, debug_flag);
            //SpreadsheetTest(sitename, username, password, debug_flag);
            Console.WriteLine("Exitting.");
            Console.ReadLine();*/

           /* String sitename = "winternationals2011test";
            String username = "indianbridge.dummy@gmail.com";
            String password = "kibitzer";
            Boolean debug_flag = true;

            SitesTest(sitename, username, password, debug_flag);*/
            string html = File.ReadAllText(@"C:\Users\snarasim\Documents\Bridge\indian-bridge-scripts\IndianBridge\ResultsCreation\Applications\WinterNationals2011PairsResultsCreator\bin\Webpages\Holkar-Pairs\boards\board1score.html");
            string url = @"https://sites.google.com/feeds/content/site/winternationals2011test?Path=/results/pairs/Holkar-Pairs/boards/board1score";
            string newHTML = replaceLinks(html, url);
            Console.WriteLine(newHTML);
            /*string input = "This is an example body of HTML with <a href  ='./index.htm'></a>3 links"+ 
                "<a href= http://www.one.com/mike/test.html title=test>link</a> more text"+
                "here <a href = \"http://www.TWO.com/2/index.htm\" target=_blank>link</a>"+
                "now for a <em>relative link</em>. <a href=/csa.html>CSA</a>. Mike Golding";
            Console.WriteLine(Regex.Replace(input, @"(\./index\.html)|(\./index\.htm)", "MAYA", RegexOptions.IgnoreCase));*/
            /*Regex re = new Regex(@"<a\s+href[^<]*</a>", RegexOptions.IgnoreCase);
            Regex re2 = new Regex("http",RegexOptions.IgnoreCase);
            string output = re.Replace(input, new MatchEvaluator(
                delegate(Match match)
                {
                    Console.WriteLine(match.Value);
                    string result = match.Value;
                    if(re2.IsMatch(result)) return result;
                    else return result.Replace(".html", "").Replace(".htm", "");
                }));

            Console.WriteLine(output);*/
            
        }

        private static string getParentPage(string path)
        {
            string parentPage = path.TrimEnd('/').TrimEnd('\\');
            int index1 = parentPage.LastIndexOf('/');
            int index2 = parentPage.LastIndexOf('\\');
            int index = index1 > index2 ? index1 : index2;
            return parentPage.Substring(0, index);
        }
        private static string getPage(string path)
        {
            return path.TrimEnd('/').TrimEnd('\\');
        }


        private static string replaceLinks(string html, string pathUrl)
        {
            string url = Regex.Replace(pathUrl, @"\?Path=", "", RegexOptions.IgnoreCase);
            url = Regex.Replace(url, @"feeds/content/", "", RegexOptions.IgnoreCase);
            Regex re = new Regex(@"<a\s+href[^<]*</a>", RegexOptions.IgnoreCase);
            Regex re2 = new Regex("http", RegexOptions.IgnoreCase);
            return re.Replace(html, new MatchEvaluator(
            delegate(Match match)
            {
                string result = match.Value;
                if (re2.IsMatch(result)) return result;
                else
                {
                    result = Regex.Replace(result, @"(\.\./)|(\.\.\\)", getParentPage(url) + "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\./)|(\.\\)", getPage(url) + "/", RegexOptions.IgnoreCase);
                    //string parentPage = "";
                    //string page = getPage(url, out parentPage);
                    result = Regex.Replace(result, @"(index\.html)|(index\.htm)", "", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(\.html)|(\.htm)", "", RegexOptions.IgnoreCase);
                    result = result.TrimEnd('/').TrimEnd('\\');
                    return result;
                }
            }));
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
            sa.updateScores(4,currentScore);
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
                    sa.uploadDirectory(@"C:\Users\snarasim\Documents\Bridge\runningscores", "/sriram_test");
                }
                catch (Exception e) { Console.WriteLine("Exception : " + e.ToString()); }
                Console.WriteLine("Press Ctrl+Z to quit, Enter to run again.");
                line = Console.ReadLine();
            }
        }
    }
}
