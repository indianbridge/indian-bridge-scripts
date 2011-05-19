using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Configuration;
using Upload_To_Google_Sites;

namespace BridgeMateRunningScores
{
    class Program
    {
        #region Config values

        // TODO (Sriram): Add folders (for running scores files and butler files), google site root, spreadsheet name, 
        // username password (of google account with access to site/spreadsheet) to config
        static int totalNumberOfTeams = Convert.ToInt16(ConfigurationManager.AppSettings["TotalNumberOfTeams"]);
        static string eventName = ConfigurationManager.AppSettings["EventName"];
        static string teamLinkTemplate = ConfigurationManager.AppSettings["TeamLinkTemplate"];
        static int numberOfBoardsPerRound = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfBoardsPerRound"]);
        static string boardLinkTemplate = ConfigurationManager.AppSettings["BoardLinkTemplate"];
        static string butlerRoundLinkTemplate = ConfigurationManager.AppSettings["ButlerRoundLinkTemplate"];
        static string outputFolder = ConfigurationManager.AppSettings["OutputFolder"];
        static string templateFolder = ConfigurationManager.AppSettings["TemplateFolder"];
        static string runningScoreFileName = ConfigurationManager.AppSettings["RunningScoreFileName"];
        static string inputFolder = ConfigurationManager.AppSettings["InputFolder"];
        static string butlerFileName = ConfigurationManager.AppSettings["ButlerFileName"];
        static string username = ConfigurationManager.AppSettings["Username"];
        static string password = ConfigurationManager.AppSettings["Password"];
        static string googleSpreadsheetName = ConfigurationManager.AppSettings["GoogleSpreadsheetName"];
        static string googleSiteName = ConfigurationManager.AppSettings["GoogleSiteName"];
        static string googleRunningScoresRoot = ConfigurationManager.AppSettings["GoogleRunningScoresRoot"];
        static string googleButlerScoresRoot = ConfigurationManager.AppSettings["GoogleButlerScoresRoot"];
        static string runningScoresFilename = ConfigurationManager.AppSettings["RunningScoresFileName"];
        static string butlerScoresFilename = ConfigurationManager.AppSettings["ButlerScoresFileName"];
        static Boolean doUpdates = Boolean.Parse(ConfigurationManager.AppSettings["DoUpdates"]);

        #endregion

        static bool isEndOfRound = false;

        static void Main(string[] args)
        {
            int numberOfMatchesPerRound = ((totalNumberOfTeams + 1) / 2);
            int numberOfPairs = 4*(totalNumberOfTeams/2);
            int numberOfTables = 2 * numberOfMatchesPerRound;
            int roundInProgress;
            string boardResultText, outputFileName;

            MagicInterface magicInterface = new MagicInterface(inputFolder, runningScoreFileName, butlerFileName);
            DataTable runningScores = magicInterface.GetRunningScores(((totalNumberOfTeams+1) / 2), out roundInProgress);

            if (roundInProgress > 0)
            {
                NameValueCollection pairNames = magicInterface.GetPairNames(numberOfPairs);
                String eventFolder = String.Format(@"{0}\runningscores\{1}", outputFolder, runningScoresFilename);
                if (!Directory.Exists(eventFolder)) Directory.CreateDirectory(eventFolder);
                string boardsOutputFolder = String.Format(@"{0}\round{1}", eventFolder, roundInProgress.ToString());
                if (!Directory.Exists(boardsOutputFolder)) 
                {
                    Directory.CreateDirectory(boardsOutputFolder);
                }

                for (int i = 1; i <= numberOfBoardsPerRound; i++)
                {
                    boardResultText = magicInterface.GetBoardResults(i, pairNames, numberOfTables);
                    outputFileName = String.Format(@"{0}\board-{1}.html", boardsOutputFolder, i.ToString());
                    Utility.WriteFile(outputFileName, boardResultText);
                }

                NameValueCollection playedBoards = GetPlayedBoards(numberOfMatchesPerRound, magicInterface.CompletedBoards);

                Boolean debug = true;
                SpreadSheetAPI sp = new SpreadSheetAPI(googleSpreadsheetName, username, password, debug);
                //TODO (Sriram): Substitute this call with a call to your spreadsheet function
                NameValueCollection teamNumbers = sp.getTeamNames(debug);
                //NameValueCollection teamNumbers = GetTeamNumbersNamesMapping();
                GenerateRunningScoresHTML(runningScores, roundInProgress, playedBoards, teamNumbers);

                // Perform closure actions at end of round
                if (isEndOfRound)
                {
                    // Only re-compute results if we haven't already generated butler results for this round
                    bool success;
                    string butlerRoundsFileName = String.Format(@"{0}\ButlerRoundsComputed.txt", templateFolder);
                    int roundsComputed = Convert.ToInt32(Utility.ReadFile(butlerRoundsFileName, out success));
                    if (roundInProgress > roundsComputed)
                    {
                        Utility.WriteFile(butlerRoundsFileName, roundInProgress.ToString());
                        DataTable cumulativeButlerScores = MergeCurrentButlerScoresIntoCumulativeResults(magicInterface.ButlerResults);
                        WriteButlerScoresToFile(cumulativeButlerScores, true);
                        GenerateButlerScoresHTML(cumulativeButlerScores, roundInProgress, true);
                        GenerateButlerScoresHTML(magicInterface.ButlerResults, roundInProgress, false);
                    }
                }

                if (isEndOfRound && doUpdates)
                {
                    SitesAPI sites = new SitesAPI(googleSiteName, username, password, debug);
                    sp.updateScores(roundInProgress,runningScores, debug);
                    sites.uploadDirectory(outputFolder + "\\runningscores", googleRunningScoresRoot);
                    sites.uploadDirectory(outputFolder + "\\butlerscores", googleButlerScoresRoot);
                }
                // TODO (Sriram): Make your API calls here
                // Easiest thing to do is to add your project to this solution and then add a project reference
                // SitesAPI = new SitesAPI (values for Running scores from config)
                // SitesAPI.UploadRunningScores()

                // if (isEndOfRound) {
                // SitesAPI.UploadRoundResults(runningScores)
                // }

                // SitesAPI = new SitesAPI (values for Butler results from config)
                // if (isEndOfRound) {
                // SitesAPI.UploadButlerResults(runningScores)
                // }
            }
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
            string scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", templateFolder, "RunningScoresTemplate.html"), out success);
            string rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", templateFolder, "RowTemplate.html"), out success);
            string boardTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", templateFolder, "BoardTemplate.html"), out success);

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

                linkText = teamLinkTemplate.Replace("[#TeamNumber#]", teamNumbers[homeTeam]);
                rowText = rowText.Replace("[#HomeTeamLink#]", linkText);

                linkText = teamLinkTemplate.Replace("[#TeamNumber#]", teamNumbers[awayTeam]);
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
                linkText = boardLinkTemplate.Replace("[#RoundNumber#]", roundInProgres.ToString());
                linkText = linkText.Replace("[#BoardNumber#]", i.ToString());
                rowText = rowText.Replace("[#BoardLink#]", linkText);
                rowText = rowText.Replace("[#BoardNumber#]", i.ToString());
                rowsText += rowText;
            }

            result = result.Replace("[#Boards#]", rowsText);
            String runningScoresFolder = outputFolder + "\\runningscores";
            if (!Directory.Exists(runningScoresFolder)) Directory.CreateDirectory(runningScoresFolder);
            string outputFileName = String.Format(@"{0}\{1}.html", runningScoresFolder, runningScoresFilename);
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
            string templateFolder = ConfigurationManager.AppSettings["TemplateFolder"];
            string butlerResultsFileName = String.Format(@"{0}\{1}", templateFolder, String.Format("{0}ButlerResults.csv", cumulative ? "Cumulative" : String.Empty));
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
            string templateFolder = ConfigurationManager.AppSettings["TemplateFolder"];
            string m_butlerResultsFileName = String.Format(@"{0}\{1}", templateFolder, "CumulativeButlerResults.csv");
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
            string scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", templateFolder, templateFileName), out success);
            string rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", templateFolder, "ButlerRowTemplate.html"), out success);

            string rowText, rowsText = String.Empty, linkText = String.Empty;
            int boards; decimal score; decimal avgScore;
            string result, roundText, roundLinksText = String.Empty, roundLinkTemplate;

            roundText = cumulative ? String.Empty : String.Format(" for Round {0}", roundInProgres.ToString());
            scoresTemplate = scoresTemplate.Replace("[#RoundNumber#]", roundText);
            scoresTemplate = scoresTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
            scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);
            scoresTemplate = scoresTemplate.Replace("[#Cumulative#]", cumulative ? "Cumulative " : String.Empty);
            scoresTemplate = scoresTemplate.Replace("[#ButlerScoresRoot#]", "../" + butlerScoresFilename);
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
                roundLinkTemplate = String.Format("<td><a href='{0}'</a>Round [#RoundNumber#]</td>", butlerRoundLinkTemplate);
                for (int i = 1; i <= roundInProgres; i++)
                {
                    roundLinksText += roundLinkTemplate.Replace("[#RoundNumber#]", i.ToString());
                }

                result = result.Replace("[#RoundLinks#]", roundLinksText);
            }

            if (cumulative)
            {
                string butlerOutputFolder = outputFolder + "\\butlerscores";
                if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
                string outputFileName = String.Format(@"{0}\" + butlerScoresFilename + ".html" , butlerOutputFolder);
                Utility.WriteFile(outputFileName, result);
            }
            else
            {
                string butlerRootFolder = outputFolder + "\\butlerscores\\"+butlerScoresFilename;
                if (!Directory.Exists(butlerRootFolder)) Directory.CreateDirectory(butlerRootFolder);
                string butlerOutputFolder = String.Format(@"{0}\{1}", butlerRootFolder, String.Format("round{0}", roundInProgres.ToString()));
                if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
                string outputFileName = String.Format(@"{0}\butlerscores.html.html", butlerOutputFolder);
                Utility.WriteFile(outputFileName, result);
            }
        }

    }
}
