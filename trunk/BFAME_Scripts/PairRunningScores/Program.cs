using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Specialized;
using Upload_To_Google_Sites;
using System.Configuration;
using System.Data;
using System.Threading;
using BridgeMateRunningScores;
using System.IO;
using IndianBridge.Common;

namespace PairRunningScores
{
    class Program
    {
        #region Members

        static int roundInProgress = 0;
        static int totalNumberOfPairs = 0;
        static int numberOfBoardsPerRound = 0;
        static String eventName = String.Empty;
        static NameValueCollection configParameters = null;
        static NameValueCollection pairNames = null;

        #endregion

        static void Main(string[] args)
        {
            #region Initial Setup

            long elapsedTime;
            Stopwatch stopwatch = null;
            SitesAPI sitesAPI = null;

            // Read all configuration parameters
            configParameters = ReadConfigParameters();

            bool debug = Convert.ToBoolean(configParameters["DebugMode"]);

            numberOfBoardsPerRound = Convert.ToInt32(configParameters["NumberOfBoardsPerRound"]);
            totalNumberOfPairs = Convert.ToInt32(configParameters["TotalNumberOfPairs"]);

            // Initialize Google SitesAPI
            Console.WriteLine("Initializing connection to Google Site");
            Console.WriteLine();
            if (Boolean.Parse(configParameters["RunUpdateGoogleSite"]))
            {
                sitesAPI = new SitesAPI(configParameters["GoogleSiteName"], configParameters["Username"], configParameters["Password"], debug);
            }

            #endregion

            #region Main Loop
            // Run continuously
            while (true)
            {
                try
                {
                    stopwatch = Stopwatch.StartNew();
                    if (debug) Console.WriteLine("Running at " + DateTime.Now.ToString());
                    // Calculate Scores and Update Files
                    Console.WriteLine("Creating Running scores and Board files");
                    Console.WriteLine();
                    DataTable runningScores = CreateRunningScoresFile(totalNumberOfPairs, debug);

                    if (Boolean.Parse(configParameters["RunUpdateGoogleSite"]))
                    {
                        // Upload Running Scores
                        Console.WriteLine("Uploading running scores");
                        Console.WriteLine();

                        sitesAPI.uploadDirectory(configParameters["OutputFolder"]
                            + "\\runningscores", configParameters["GoogleRunningScoresRoot"], configParameters["BackupFolder"]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Encountered : " + e.ToString());
                }

                if (Convert.ToBoolean(configParameters["RunOnce"]))
                {
                    break;
                }

                Console.WriteLine("Done processing...");
                Console.WriteLine();
                elapsedTime = stopwatch.ElapsedMilliseconds;
                if (debug) Console.WriteLine("...Sleeping for " + (long.Parse(configParameters["UpdateFrequency"]) - elapsedTime) + " milliseconds.");
                if (elapsedTime < long.Parse(configParameters["UpdateFrequency"])) Thread.Sleep((int)(long.Parse(configParameters["UpdateFrequency"]) - elapsedTime));
            }

            #endregion

        }

        static NameValueCollection ReadConfigParameters()
        {
            return new NameValueCollection(ConfigurationManager.AppSettings);
        }

        static DataTable CreateRunningScoresFile(int totalNumberOfPairs, Boolean debug = false)
        {
            string outputFileName;
            String eventFolder, boardsOutputFolder, boardResultText;
            NameValueCollection pairNames;
            bool hasNewResults;
            int boardNumber;

            MagicInterface magicInterface = new MagicInterface(configParameters["InputFolder"], configParameters["RunningScoreFileName"],
                configParameters["RunningScoresRoot"], configParameters["BoardResultFont"], Convert.ToBoolean(configParameters["BoardResultFontBold"]));
            DataTable runningScores = magicInterface.GetRunningScores(totalNumberOfPairs / 2, out roundInProgress, out pairNames);

            if (roundInProgress > 0)
            {
                GenerateRunningScoresHTML(runningScores, roundInProgress);

                eventFolder = String.Format(@"{0}\runningscores\{1}", configParameters["OutputFolder"], configParameters["RunningScoresRoot"]);
                if (!Directory.Exists(eventFolder)) Directory.CreateDirectory(eventFolder);

                boardsOutputFolder = String.Format(@"{0}\round{1}", eventFolder, roundInProgress.ToString());

                if (!Directory.Exists(boardsOutputFolder)) Directory.CreateDirectory(boardsOutputFolder);

                for (int i = 1; i <= numberOfBoardsPerRound; i++)
                {
                    boardNumber = i + (roundInProgress - 1) * numberOfBoardsPerRound;

                    boardResultText = magicInterface.GetBoardResults(i, pairNames, totalNumberOfPairs / 2, 
                        roundInProgress, numberOfBoardsPerRound, out hasNewResults);

                    // only update the file if results have been updated
                    if (hasNewResults)
                    {
                        outputFileName = String.Format(@"{0}\board-{1}.html", boardsOutputFolder, boardNumber.ToString());
                        Utility.WriteFile(outputFileName, boardResultText);
                    }
                }
            }

            return runningScores;
        }

        public static void GenerateRunningScoresHTML(DataTable runningScores, int roundInProgres)
        {
            bool success;

            string rowText, rowsText = String.Empty, linkText = String.Empty, result;
            int boardNumber;
            string scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RunningScoresTemplate.html"), out success);
            string rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RowTemplate.html"), out success);
            string boardTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "BoardTemplate.html"), out success);
            string stage = configParameters["Stage"];
            eventName = configParameters["EventName"];

            scoresTemplate = scoresTemplate.Replace("[#Stage#]", stage);
            scoresTemplate = scoresTemplate.Replace("[#RoundNumber#]", roundInProgress.ToString());
            scoresTemplate = scoresTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
            scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);

            int j = 0;

            foreach (DataRow row in runningScores.Rows)
            {
                rowText = (j % 2) == 0 ? rowTemplate : rowTemplate.Replace("background-color:#def", "background-color:#ddd");
                rowText = rowText.Replace("[#Rank#]", row["Rank"].ToString());
                rowText = rowText.Replace("[#PairNumber#]", row["PairNumber"].ToString());
                rowText = rowText.Replace("[#PairName#]", row["PairName"].ToString());
                rowText = rowText.Replace("[#RoundScore#]", row["RoundScore"].ToString());
                rowText = rowText.Replace("[#TotalScore#]", row["CumulativeScore"].ToString());
                rowText = rowText.Replace("[#PercentScore#]", row["PercentScore"].ToString());
                rowText = rowText.Replace("[#RoundPenalty#]", row["Penalty"].ToString());

                rowsText += rowText;
                j++;
            }

            result = scoresTemplate.Replace("[#Scores#]", rowsText);

            rowsText = String.Empty;

            for (int i = 1; i <= numberOfBoardsPerRound; i++)
            {
                boardNumber = i + (roundInProgres - 1) * numberOfBoardsPerRound;
                // Alternating backgrounds
                rowText = (i % 2) == 0 ? boardTemplate.Replace("background-color:#def", "background-color:#ddd") : boardTemplate;

                linkText = configParameters["BoardLinkTemplate"];
                linkText = linkText.Replace("[#BoardNumber#]", boardNumber.ToString());
                linkText = linkText.Replace("[#RoundNumber#]", roundInProgress.ToString());
                linkText = linkText.Replace("[#Path#]", configParameters["RunningScoresRoot"]);
                rowText = rowText.Replace("[#BoardLink#]", linkText);
                rowText = rowText.Replace("[#BoardNumber#]", boardNumber.ToString());
                rowsText += rowText;
            }

            result = result.Replace("[#Boards#]", rowsText);
            String runningScoresFolder = configParameters["OutputFolder"] + "\\runningscores";
            if (!Directory.Exists(runningScoresFolder)) Directory.CreateDirectory(runningScoresFolder);
            String runningScoresRootFolder = String.Format(@"{0}\{1}", runningScoresFolder, configParameters["RunningScoresRoot"]);
            if (!Directory.Exists(runningScoresRootFolder)) Directory.CreateDirectory(runningScoresRootFolder);
            String outputFileName = String.Format(@"{0}\index.html", runningScoresRootFolder);
            Utility.WriteFile(outputFileName, result);
        }
    }
}
