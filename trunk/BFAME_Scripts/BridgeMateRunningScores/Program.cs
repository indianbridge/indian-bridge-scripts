﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using Nini.Config;
using Upload_To_Google_Sites;

namespace BridgeMateRunningScores
{
    class Program
    {
        #region Config values

        // Consolidated this into a NameValueCollection, this way we can change where we these values from.
        // For now the NameValueCollection is just ConfigurationManager.AppSettings
        /*static string eventName = ConfigurationManager.AppSettings["EventName"];
        static string configParameters["TeamLinkTemplate"] = ConfigurationManager.AppSettings["configParameters["TeamLinkTemplate"]"];
        static int Convert.ToInt16(configParameters["NumberOfBoardsPerRound"]) = Convert.ToInt16(ConfigurationManager.AppSettings["Convert.ToInt16(configParameters["NumberOfBoardsPerRound"])"]);
        static string configParameters["BoardLinkTemplate"] = ConfigurationManager.AppSettings["BoardLinkTemplate"];
        static string configParameters["ButlerRoundLinkTemplate"] = ConfigurationManager.AppSettings["ButlerRoundLinkTemplate"];
        static string configParameters["OutputFolder"] = ConfigurationManager.AppSettings["configParameters["OutputFolder"]"];
        static string configParameters["TemplateFolder"] = ConfigurationManager.AppSettings["configParameters["TemplateFolder"]"];
        static string configParameters["RunningScoreFileName"] = ConfigurationManager.AppSettings["configParameters["RunningScoreFileName"]"];
        static string configParameters["InputFolder"] = ConfigurationManager.AppSettings["configParameters["InputFolder"]"];
        static string configParameters["ButlerFileName"] = ConfigurationManager.AppSettings["configParameters["ButlerFileName"]"];
        static string configParameters["Username"] = ConfigurationManager.AppSettings["configParameters["Username"]"];
        static string configParameters["Password"] = ConfigurationManager.AppSettings["configParameters["Password"]"];
        static string configParameters["GoogleSpreadsheetName"] = ConfigurationManager.AppSettings["configParameters["GoogleSpreadsheetName"]"];
        static string configParameters["GoogleSiteName"] = ConfigurationManager.AppSettings["configParameters["GoogleSiteName"]"];
        static string configParameters["GoogleRunningScoresRoot"] = ConfigurationManager.AppSettings["configParameters["GoogleRunningScoresRoot"]"];
        static string configParameters["GoogleButlerScoresRoot"] = ConfigurationManager.AppSettings["configParameters["GoogleButlerScoresRoot"]"];
        static string configParameters["RunningScoresFileName"] = ConfigurationManager.AppSettings["configParameters["RunningScoresFileName"]"];
        static string configParameters["ButlerScoresFileName"] = ConfigurationManager.AppSettings["configParameters["ButlerScoresFileName"]"];
        static Boolean Boolean.Parse(configParameters["RunUpdateGoogleSpreadsheet"]) = Boolean.Parse(ConfigurationManager.AppSettings["Boolean.Parse(configParameters["RunUpdateGoogleSpreadsheet"])"]);
        static Boolean Boolean.Parse(configParameters["RunUpdateGoogleSite"]) = Boolean.Parse(ConfigurationManager.AppSettings["Boolean.Parse(configParameters["RunUpdateGoogleSite"])"]);
        static long long.Parse(configParameters["UpdateFrequency"]) = long.Parse(ConfigurationManager.AppSettings["long.Parse(configParameters["UpdateFrequency"])"]);*/

        #endregion

        static bool isEndOfRound = false;
        static int roundInProgress = 0;
        static int roundsComputed = 0;
        static NameValueCollection configParameters = null;
        static int totalNumberOfTeams = 0;
        static int numberOfBoardsPerRound = 0;
        static String eventName = "";

        static void Main(string[] args) 
        {
            Boolean debug = true;
            // Read all configuration parameters
            configParameters = readConfigParameters();
            
            NameValueCollection nameNumberMapping = null;
            SpreadSheetAPI spreadsheetAPI = null;
            SitesAPI sitesAPI = null;

            // Wipe out Butler files
            Utility.WriteFile(String.Format(@"{0}\CumulativeButlerResults.csv", configParameters["TemplateFolder"]), "");

            // Initialize SpreadsheetAPI and gather information
            if (Boolean.Parse(configParameters["RunUpdateGoogleSpreadsheet"]))
            {
                spreadsheetAPI = new SpreadSheetAPI(configParameters["GoogleSpreadsheetName"], configParameters["Username"], configParameters["Password"], debug);
                totalNumberOfTeams = (int)spreadsheetAPI.getNumberOfTeams();
                eventName = spreadsheetAPI.getEventName();
                numberOfBoardsPerRound = spreadsheetAPI.getNumberOfBoards();
                nameNumberMapping = spreadsheetAPI.getTeamNames(debug);
            }
            else {
                totalNumberOfTeams = Convert.ToInt16(configParameters["TotalNumberOfTeams"]);
                eventName = configParameters["EventName"];
                numberOfBoardsPerRound = int.Parse(configParameters["NumberOfBoardsPerRound"]);
                // TODO Investigate using NINI
                /*IConfigSource source = new IniConfigSource("NameNumberMapping.ini");
                String nameNumberMappingConfig = "NameNumberMapping";
                string[] keys = source.Configs[nameNumberMappingConfig].GetKeys();
                foreach (string key in keys) nameNumberMapping.Add(key, source.Configs[nameNumberMappingConfig].Get(key));*/
                nameNumberMapping = GetTeamNumbersNamesMapping();
            }

            // Initialize Google SitesAPI
            if (Boolean.Parse(configParameters["RunUpdateGoogleSite"]))
            {
                sitesAPI = new SitesAPI(configParameters["GoogleSiteName"], configParameters["Username"], configParameters["Password"], debug);
            }

            // Run continuously
            while(true) {
                try
                {
                    var stopwatch = Stopwatch.StartNew();
                    if (debug) Console.WriteLine("Running at " + DateTime.Now.ToString());
                    // Calculate Scores and Update Files
                    DataTable runningScores = createButlerAndRunningScoresFiles(totalNumberOfTeams, nameNumberMapping, debug);
                    // Upload Running Scores
                    if (Boolean.Parse(configParameters["RunUpdateGoogleSite"])) sitesAPI.uploadDirectory(configParameters["OutputFolder"] + "\\runningscores", configParameters["GoogleRunningScoresRoot"]);
                    if (isEndOfRound)
                    {
                        // Upload Butler Scores
                        if (Boolean.Parse(configParameters["RunUpdateGoogleSite"])) sitesAPI.uploadDirectory(configParameters["OutputFolder"] + "\\butlerscores", configParameters["GoogleButlerScoresRoot"]);
                        // Update Spreadsheet with round scores
                        if (Boolean.Parse(configParameters["RunUpdateGoogleSpreadsheet"])) spreadsheetAPI.updateScores(roundInProgress, runningScores, debug);
                    }
                    long elapsedTime = stopwatch.ElapsedMilliseconds;
                    if (debug) Console.WriteLine("Sleeping for " + (long.Parse(configParameters["UpdateFrequency"]) - elapsedTime) + " milliseconds.");
                    if (elapsedTime < long.Parse(configParameters["UpdateFrequency"])) Thread.Sleep((int)(long.Parse(configParameters["UpdateFrequency"]) - elapsedTime));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception Encountered : " + e.ToString());
                }
            }

        }

        static NameValueCollection readConfigParameters()
        {
            return new NameValueCollection(ConfigurationManager.AppSettings);
        }


        static DataTable createButlerAndRunningScoresFiles(int totalNumberOfTeams, NameValueCollection nameNumberMapping, Boolean debug = false)
        {
            int numberOfMatchesPerRound = ((totalNumberOfTeams + 1) / 2);
            int numberOfPairs = 4*(totalNumberOfTeams/2);
            int numberOfTables = 2 * numberOfMatchesPerRound;
            string boardResultText, outputFileName;

            MagicInterface magicInterface = new MagicInterface(configParameters["InputFolder"], configParameters["RunningScoreFileName"], configParameters["ButlerFileName"], configParameters["RunningScoresFileName"]);
            DataTable runningScores = magicInterface.GetRunningScores(((totalNumberOfTeams+1) / 2), out roundInProgress);
            if (roundInProgress > 0)
            {
                NameValueCollection pairNames = magicInterface.GetPairNames(numberOfPairs);
                String eventFolder = String.Format(@"{0}\runningscores\{1}", configParameters["OutputFolder"], configParameters["RunningScoresFileName"]);
                if (!Directory.Exists(eventFolder)) Directory.CreateDirectory(eventFolder);
                string boardsOutputFolder = String.Format(@"{0}\round{1}", eventFolder, roundInProgress.ToString());
                if (!Directory.Exists(boardsOutputFolder)) Directory.CreateDirectory(boardsOutputFolder);

                for (int i = 1; i <= numberOfBoardsPerRound; i++)
                {
                    boardResultText = magicInterface.GetBoardResults(i, pairNames, numberOfTables);
                    outputFileName = String.Format(@"{0}\board-{1}.html", boardsOutputFolder, i.ToString());
                    Utility.WriteFile(outputFileName, boardResultText);
                }

                NameValueCollection playedBoards = GetPlayedBoards(numberOfMatchesPerRound, magicInterface.CompletedBoards);
                
                GenerateRunningScoresHTML(runningScores, roundInProgress, playedBoards, nameNumberMapping);

                // Perform closure actions at end of round
                if (isEndOfRound)
                {
                    // Only re-compute results if we haven't already generated butler results for this round
                    /*bool success;
                    string butlerRoundsFileName = String.Format(@"{0}\ButlerRoundsComputed.txt", configParameters["TemplateFolder"]);
                    int roundsComputed = Convert.ToInt32(Utility.ReadFile(butlerRoundsFileName, out success));*/
                    if (roundInProgress > roundsComputed)
                    {
                        roundsComputed = roundInProgress;
                        //Utility.WriteFile(butlerRoundsFileName, roundInProgress.ToString());
                        DataTable cumulativeButlerScores = MergeCurrentButlerScoresIntoCumulativeResults(magicInterface.ButlerResults);
                        WriteButlerScoresToFile(cumulativeButlerScores, true);
                        GenerateButlerScoresHTML(cumulativeButlerScores, roundInProgress, true);
                        GenerateButlerScoresHTML(magicInterface.ButlerResults, roundInProgress, false);
                    }
                }

            }

            return runningScores;

       }

        //TODO (Sriram): This is a stub for the call to the spreadsheet to get the names-numbers mapping
        // When you update the code in Main to call that method instead, just delete this method
        public static NameValueCollection GetTeamNumbersNamesMapping()
        {
            NameValueCollection names = new NameValueCollection();
            names.Add("India", "1");
            names.Add("Pakistan", "2");
            names.Add("Bangladesh", "3");
            names.Add("Srilanka", "4");
            names.Add("Delhi", "5");

            return names;
        }

        public static void GenerateRunningScoresHTML(DataTable runningScores, int roundInProgres,
            NameValueCollection completedBoards, NameValueCollection teamNumbers)
        {
            bool success;
            int minPlayedBoards = numberOfBoardsPerRound, playedBoards = 0;
            string scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RunningScoresTemplate.html"), out success);
            string rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RowTemplate.html"), out success);
            string boardTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "BoardTemplate.html"), out success);

            string rowText, rowsText = String.Empty, linkText = String.Empty;
            string result, homeTeam, awayTeam;

            scoresTemplate = scoresTemplate.Replace("[#RoundNumber#]", roundInProgres.ToString());
            scoresTemplate = scoresTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
            scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);

            int j = 0;
            
            foreach (DataRow row in runningScores.Rows) 
            {
                rowText = (j % 2) == 0 ? rowTemplate : rowTemplate.Replace("background-color:#def", "background-color:#ddd");
                rowText = rowText.Replace("[#TableNumber#]", row["TableNumber"].ToString());

                homeTeam = row["HomeTeam"].ToString();
                awayTeam = row["AwayTeam"].ToString();

                rowText = rowText.Replace("[#HomeTeam#]", homeTeam);
                rowText = rowText.Replace("[#AwayTeam#]", awayTeam);

                rowText = rowText.Replace("[#IMPScore#]", row["IMPScore"].ToString());
                rowText = rowText.Replace("[#VPScore#]", row["VPScore"].ToString());

                linkText = configParameters["TeamLinkTemplate"].Replace("[#TeamNumber#]", teamNumbers[homeTeam]);
                rowText = rowText.Replace("[#HomeTeamLink#]", linkText);

                linkText = configParameters["TeamLinkTemplate"].Replace("[#TeamNumber#]", teamNumbers[awayTeam]);
                rowText = rowText.Replace("[#AwayTeamLink#]", linkText);

                playedBoards = Convert.ToInt16(completedBoards[row["TableNumber"].ToString()]);
                if (row["IMPScore"].ToString() != "Bye" && playedBoards < minPlayedBoards)
                {
                    minPlayedBoards = playedBoards;
                }

                rowText = rowText.Replace("[#CompletedBoards#]", playedBoards.ToString());
                rowsText += rowText;
                j++;
            }

            if (minPlayedBoards == numberOfBoardsPerRound)
            {
                isEndOfRound = true;
            }

            result = scoresTemplate.Replace("[#Scores#]", rowsText);

            rowsText = String.Empty;

            for (int i=1;i<=numberOfBoardsPerRound;i++) {
                // Alternating backgrounds
                rowText = (i % 2) == 0 ? boardTemplate.Replace("background-color:#def", "background-color:#ddd") : boardTemplate;
                linkText = configParameters["RunningScoresFileName"]+"/"+configParameters["BoardLinkTemplate"].Replace("[#RoundNumber#]", roundInProgres.ToString());
                linkText = linkText.Replace("[#BoardNumber#]", i.ToString());
                rowText = rowText.Replace("[#BoardLink#]", linkText);
                rowText = rowText.Replace("[#BoardNumber#]", i.ToString());
                rowsText += rowText;
            }

            result = result.Replace("[#Boards#]", rowsText);
            String runningScoresFolder = configParameters["OutputFolder"] + "\\runningscores";
            if (!Directory.Exists(runningScoresFolder)) Directory.CreateDirectory(runningScoresFolder);
            String runningScoresRootFolder = String.Format(@"{0}\{1}", runningScoresFolder, configParameters["RunningScoresFileName"]);
            if (!Directory.Exists(runningScoresRootFolder)) Directory.CreateDirectory(runningScoresRootFolder);
            String outputFileName = String.Format(@"{0}\index.html", runningScoresRootFolder);
            Utility.WriteFile(outputFileName, result);
        }

        // We assume that the table numbers are A-n and B-n, i.e A-1/B-1, A-2/B-2 and so on
        public static NameValueCollection GetPlayedBoards(int numberOfMatches, DataTable completedBoards) 
        {
            DataRow[] rows;
            NameValueCollection playedBoards = new NameValueCollection();
            int numPlayedBoards;

            for (int i = 1; i <= numberOfMatches; i++)
            {
                numPlayedBoards = 0;
                // Find rows completed at table A-n (where n is the table number)
                rows = completedBoards.Select(String.Format("Table='A-{0}'", i));

                foreach (DataRow row in rows) 
                {
                    if (completedBoards.Select(String.Format("Table='B-{0}' AND Board='{1}'", i, row["Board"].ToString())).Length == 1) 
                    {
                        numPlayedBoards++;
                    }
                }

                playedBoards.Add(i.ToString(), numPlayedBoards.ToString());
            }

            return playedBoards;
        }

        public static void WriteButlerScoresToFile(DataTable butlerScores, bool cumulative = false)
        {
            //string configParameters["TemplateFolder"] = ConfigurationManager.AppSettings["configParameters["TemplateFolder"]"];
            string butlerResultsFileName = String.Format(@"{0}\{1}", configParameters["TemplateFolder"], String.Format("{0}ButlerResults.csv", cumulative ? "Cumulative" : String.Empty));
            string content;

            content = "Pair, Boards, Score\r\n";
            foreach (DataRow row in butlerScores.Rows)
            {
                content += String.Format("{0},{1},{2}\r\n", row["Pair"], row["Boards"], row["Score"]);
            }

            Utility.WriteFile(butlerResultsFileName, content);
        }

        public static DataTable LoadCumulativeButlerResults()
        {
            DataTable m_butlerResults;
            //string configParameters["TemplateFolder"] = ConfigurationManager.AppSettings["configParameters["TemplateFolder"]"];
            string m_butlerResultsFileName = String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "CumulativeButlerResults.csv");
            StreamReader fileStream = new StreamReader(m_butlerResultsFileName);
            string row;
            string[] data;
            DataRow dataRow;

            // Skip past the header row
            fileStream.ReadLine();

            m_butlerResults = new DataTable();
            m_butlerResults.Columns.Add("Pair", typeof(System.String));
            m_butlerResults.Columns.Add("Boards", typeof(System.Int16));
            m_butlerResults.Columns.Add("Score", typeof(System.Decimal));

            while (true)
            {
                row = fileStream.ReadLine();
                if (String.IsNullOrEmpty(row)) break;
                data = row.Split(new char[] { ',' });

                dataRow = m_butlerResults.NewRow();
                dataRow["Pair"] = data[0];
                dataRow["Boards"] = Convert.ToInt16(data[1]);
                dataRow["Score"] = Convert.ToDecimal(data[2]);

                m_butlerResults.Rows.Add(dataRow);
            }

            fileStream.Close();

            return m_butlerResults;
        }

        public static DataTable MergeCurrentButlerScoresIntoCumulativeResults(DataTable currentButlerResults)
        {
            DataTable cumulativeButlerResults = LoadCumulativeButlerResults();

            foreach (DataRow row in currentButlerResults.Rows)
            {
                Utility.UpdateButlerResults(cumulativeButlerResults, row["Pair"].ToString(),
                    Convert.ToDecimal(row["Score"]), Convert.ToInt16(row["Boards"]));
            }

            return cumulativeButlerResults;
        }

        public static void GenerateButlerScoresHTML(DataTable butlerScores, int roundInProgres, bool cumulative)
        {
            bool success;
            string templateFileName = String.Format("{0}ButlerScoresTemplate.html", cumulative ? "Cumulative" : String.Empty);
            string scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], templateFileName), out success);
            string rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "ButlerRowTemplate.html"), out success);

            string rowText, rowsText = String.Empty, linkText = String.Empty;
            int boards; decimal score; decimal avgScore;
            string result, roundText, roundLinksText = String.Empty, roundLinkTemplate;

            roundText = cumulative ? String.Empty : String.Format(" for Round {0}", roundInProgres.ToString());
            scoresTemplate = scoresTemplate.Replace("[#RoundNumber#]", roundText);
            scoresTemplate = scoresTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
            scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);
            scoresTemplate = scoresTemplate.Replace("[#Cumulative#]", cumulative ? "Cumulative " : String.Empty);
            scoresTemplate = scoresTemplate.Replace("[#ButlerScoresRoot#]", "../../"+configParameters["ButlerScoresFilename"]);
            int j = 0;

            foreach (DataRow row in butlerScores.Select(String.Empty, "Score Desc"))
            {
                rowText = (j % 2) == 0 ? rowTemplate : rowTemplate.Replace("background-color:#def", "background-color:#ddd");
                boards = Convert.ToInt16(row["Boards"]);
                score = Convert.ToDecimal(row["Score"]);
                avgScore = score/boards;

                rowText = rowText.Replace("[#Pair#]", row["Pair"].ToString());
                rowText = rowText.Replace("[#Boards#]", boards.ToString());
                rowText = rowText.Replace("[#TotalScore#]", score.ToString());
                rowText = rowText.Replace("[#AvgScore#]", avgScore.ToString("#.##"));
                rowsText += rowText;
                j++;
            }

            result = scoresTemplate.Replace("[#Scores#]", rowsText);

            if (cumulative)
            {
                roundLinkTemplate = String.Format("<td><a href='{0}'</a>Round [#RoundNumber#]</td>", configParameters["ButlerScoresFileName"]+"/"+configParameters["ButlerRoundLinkTemplate"]);
                for (int i = 1; i <= roundInProgres; i++)
                {
                    roundLinksText += roundLinkTemplate.Replace("[#RoundNumber#]", i.ToString());
                }

                result = result.Replace("[#RoundLinks#]", roundLinksText);
            }

            if (cumulative)
            {
                string butlerOutputFolder = configParameters["OutputFolder"] + "\\butlerscores";
                if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
                String butlerRootFolder = String.Format(@"{0}\{1}",butlerOutputFolder,configParameters["ButlerScoresFileName"]);
                if(!Directory.Exists(butlerRootFolder)) Directory.CreateDirectory(butlerRootFolder);
                string outputFileName = String.Format(@"{0}\index.html",butlerRootFolder);
                Utility.WriteFile(outputFileName, result);
            }
            else
            {
                string butlerRootFolder = configParameters["OutputFolder"] + "\\butlerscores\\"+configParameters["ButlerScoresFileName"];
                if (!Directory.Exists(butlerRootFolder)) Directory.CreateDirectory(butlerRootFolder);
                string butlerOutputFolder = String.Format(@"{0}\{1}", butlerRootFolder, String.Format("round{0}", roundInProgres.ToString()));
                if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
                string outputFileName = String.Format(@"{0}\butlerscores.html", butlerOutputFolder);
                Utility.WriteFile(outputFileName, result);
            }
        }

    }
}
