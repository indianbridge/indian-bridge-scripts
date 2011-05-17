using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


using Google.GData.Client;

namespace Upload_To_Google_Sites
{
    class Upload_To_Google_Sites
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage is Upload_To_Google_Sites sitename username password [debug_flag]. Press any key to exit...");
                Console.ReadLine();
                return;
            }
            String sitename = args[0];
            String username = args[1];
            String password = args[2];
            Boolean debug_flag = false;
            if (args.Length > 3) debug_flag = Boolean.Parse(args[3]);
            SpreadsheetTest(sitename, username, password, debug_flag);
            Console.WriteLine("Exitting.");
        }
        static void SpreadsheetTest(String spreadsheetname,String username,String password,Boolean debug_flag)
        {
            SpreadSheetAPI sa = new SpreadSheetAPI(spreadsheetname,username,password,debug_flag);
            sa.updateScores(@"C:\Documents and Settings\snarasim\My Documents\Downloads\CurrentStanding\magiccontest-o-1.htm");
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
