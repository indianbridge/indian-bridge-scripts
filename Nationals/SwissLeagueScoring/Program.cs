using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using Upload_To_Google_Sites;
using System.Collections.Specialized;
using IndianBridge.Common;
using System.Diagnostics;
using System.Threading;

namespace SwissLeagueScoring
{
    class Program
    {
        #region Members
        static SpreadSheetAPI spreadsheetAPI = null;
        static NameValueCollection configParameters;
        static bool debug;
        static int roundCompleted;
        static int roundForDraw;
        #endregion

        #region Common
        static void Main(string[] args)
        {
            configParameters = ReadConfigParameters();

            if (configParameters["ScoringMode"] == "CSV")
            {
                ProcessCSVScoring();
            }
            else
            {
                if (args.Length > 0 && args[0] == "showresults")
                {
                    long elapsedTime;
                    Stopwatch stopwatch = null;

                    Console.WriteLine("Generating results file...");
                    // We don't want to change any of the data here - just read the magic contest file 
                    // and generate the results file

                    while (true)
                    {
                        stopwatch = Stopwatch.StartNew();
                        Console.WriteLine("Running at " + DateTime.Now.ToString());

                        // Read and write results...
                        ProcessMagicResults();
                        // ... then sleep for 2 mins
                        elapsedTime = stopwatch.ElapsedMilliseconds;
                        Console.WriteLine("...Sleeping for " + (long.Parse(configParameters["UpdateFrequency"]) - elapsedTime) + " milliseconds.");
                        if (elapsedTime < long.Parse(configParameters["UpdateFrequency"])) Thread.Sleep((int)(long.Parse(configParameters["UpdateFrequency"]) - elapsedTime));
                    }
                }
                else
                {
                    ProcessMagicScoring();
                }
            }
        }

        static NameValueCollection ReadConfigParameters()
        {
            return new NameValueCollection(ConfigurationManager.AppSettings);
        }

        #endregion

        #region Magic

        private static void ProcessMagicResults()
        {
            string inputFolder = configParameters["InputFolder"];
            string drawFileName = configParameters["MagicDrawFileName"];

            Console.WriteLine("Reading results...");
            DataTable results = ReadMagicResults(inputFolder);

            // Write out the draw file
            Console.WriteLine("Writing results file...");
            GenerateResultsHTML(results);
        }

        private static void ProcessMagicScoring()
        {
            bool uploadScores = Convert.ToBoolean(configParameters["UploadToSpreadsheet"]);
            string inputFolder = configParameters["InputFolder"];
            string drawFileName = configParameters["MagicDrawFileName"];
            Dictionary<int, int> scores;

            Console.WriteLine("Reading draw...");
            DataTable draw = ReadMagicDraw(inputFolder);

            Console.WriteLine("Reading scores...");
            scores = ReadMagicScores(inputFolder);

            // Write out the draw file
            Console.WriteLine("Writing draw file...");
            GenerateDrawHTML(draw, scores);

            // Upload to spreadsheet if asked to do so, but only if a round has been completed
            if (uploadScores && roundCompleted > 0)
            {
                Console.WriteLine("Initializing spreadsheet...");
                spreadsheetAPI = new SpreadSheetAPI(configParameters["GoogleSpreadsheetName"], configParameters["Username"], configParameters["Password"], debug);

                Console.WriteLine(String.Format("Reading draw for round {0}...", roundCompleted));
                DataTable prevDraw = LoadPreviousDraw();

                Console.WriteLine("Merging draw and scores...");
                MergeDrawAndScores(prevDraw, scores);

                Console.WriteLine("Uploading scores to spreadsheet...");
                spreadsheetAPI.updateScores(roundCompleted, prevDraw, debug);
            }

            // Write the current draw and cumulative scores to a file so we can use it later
            Console.WriteLine("Writing draw and scores to file...");
            WriteScoresToFile(scores);
            WriteDrawToFile(draw);
        }

        static Dictionary<int, int> ReadMagicScores(string inputFolder)
        {
            int teamNumberStartPosition = int.Parse(configParameters["TeamNumberStartPosition"]);
            int teamNumberStringLength = int.Parse(configParameters["TeamNumberStringLength"]);
            int teamScoreStartPosition = int.Parse(configParameters["TeamScoreStartPosition"]);
            int teamScoreStringLength = int.Parse(configParameters["TeamScoreStringLength"]);
            string scoreFileName = configParameters["MagicScoreFileName"];
            string numberOfTeams = String.Empty;

            int teamNumber, teamScore, position = 0;

            Dictionary<int, int> VPScore = new Dictionary<int, int>();
            string fileName = String.Format(@"{0}\{1}", inputFolder, scoreFileName);

            string recordData;
            StreamReader streamReader = new StreamReader(fileName);
            bool scoreDataFound = false;

            while (!streamReader.EndOfStream)
            {
                // Read in a line of the file data
                recordData = streamReader.ReadLine();

                if (!scoreDataFound)
                {
                    if (recordData.Contains("<PRE>"))
                    {
                        scoreDataFound = true;
                        
                        // read the next line after the "PRE" which contains the round number
                        recordData = streamReader.ReadLine();
                        position = Utility.findWhiteSpace(recordData, 0);
                        numberOfTeams = recordData.Substring(0, position);
                        try
                        {
                            roundCompleted = int.Parse(recordData.Replace(".", String.Empty).Replace(String.Format(
                                "{0} teams Number of rounds:", numberOfTeams), String.Empty).Trim());
                        }
                        catch (Exception)
                        {
                            // If there was an error, it means that this is the very first round
                            roundCompleted = 0;
                        }

                        // skip past 3 more lined
                        for (int i = 0; i < 3; i++)
                        {
                            streamReader.ReadLine();
                        }
                    }
                }
                else
                {
                    if (recordData.Contains("</PRE>"))
                    {
                        // End of the road
                        break;
                    }
                    else
                    {
                        // This is a line of score
                        teamNumber = Convert.ToInt16(recordData.Substring(teamNumberStartPosition, teamNumberStringLength));
                        teamScore = Convert.ToInt16(recordData.Substring(teamScoreStartPosition, teamScoreStringLength));
                        VPScore.Add(teamNumber, teamScore);
                    }
                }
            }

            return VPScore;
        }

        static DataTable ReadMagicDraw(string inputFolder)
        {
            DataTable results = CreateMagicScoreTable();

            string scoreFileName = configParameters["MagicDrawFileName"];

            int homeTeamNumber = 0, awayTeamNumber = 0, tableNumber, position;
            string homeTeamName = String.Empty, awayTeamName = String.Empty;
            DataRow row;

            string fileName = String.Format(@"{0}\{1}", inputFolder, scoreFileName);

            string recordData;
            StreamReader streamReader = new StreamReader(fileName);
            bool scoreDataFound = false;

            while (!streamReader.EndOfStream)
            {
                // Read in a line of the file data
                recordData = streamReader.ReadLine();

                if (!scoreDataFound)
                {
                    if (recordData.Contains("<PRE>"))
                    {
                        scoreDataFound = true;
                        // read the next line after the "PRE" which contains the round number
                        recordData = streamReader.ReadLine();
                        roundForDraw = int.Parse(recordData.Replace("Round:", String.Empty).Trim());
                        // skip past one more line
                        streamReader.ReadLine();
                    }
                }
                else
                {
                    if (recordData.Contains("</PRE>"))
                    {
                        // End of the road
                        break;
                    }
                    else
                    {
                        position = 0;
                        // skip past empty lines
                        if (String.IsNullOrEmpty(recordData.Trim())) continue;

                        tableNumber = Convert.ToInt16(Utility.GetField(recordData, ref position));
                        if (!int.TryParse(Utility.GetField(recordData, ref position), out homeTeamNumber))
                        {
                            homeTeamNumber = 0;
                            // We've to do this anyway to move the cursor to the next field
                            homeTeamName = Utility.GetField(recordData, ref position);
                            homeTeamName = "BYE";
                        }
                        else
                        {
                            homeTeamName = Utility.GetField(recordData, ref position);
                        }

                        if (!int.TryParse(Utility.GetField(recordData, ref position), out awayTeamNumber))
                        {
                            awayTeamNumber = 0;
                            awayTeamName = "BYE";
                        }
                        else
                        {
                            awayTeamName = Utility.GetField(recordData, ref position);
                        }

                        row = results.NewRow();
                        row["TableNumber"] = tableNumber;
                        row["HomeTeamNumber"] = homeTeamNumber;
                        row["AwayTeamNumber"] = awayTeamNumber;
                        row["HomeTeamName"] = homeTeamName.Replace('_', ' ').Replace('~', '-');
                        row["AwayTeamName"] = awayTeamName.Replace('_', ' ').Replace('~', '-');
                        results.Rows.Add(row);
                    }
                }
        }

            return results;
        }

        static DataTable ReadMagicResults(string inputFolder)
        {
            DataTable results = CreateMagicResultsTable();

            string scoreFileName = configParameters["MagicDrawFileName"];

            int homeTeamNumber = 0, awayTeamNumber = 0, tableNumber, position;
            string homeTeamName = String.Empty, awayTeamName = String.Empty, impScore1, impScore2, vpScore;
            string[] vpScores;
            DataRow row;

            string fileName = String.Format(@"{0}\{1}", inputFolder, scoreFileName);

            string recordData;
            StreamReader streamReader = new StreamReader(fileName);
            bool scoreDataFound = false;

            while (!streamReader.EndOfStream)
            {
                // Read in a line of the file data
                recordData = streamReader.ReadLine();

                if (!scoreDataFound)
                {
                    if (recordData.Contains("<PRE>"))
                    {
                        scoreDataFound = true;
                        // read the next line after the "PRE" which contains the round number
                        recordData = streamReader.ReadLine();
                        roundForDraw = int.Parse(recordData.Replace("Round:", String.Empty).Trim());
                        // skip past one more line
                        streamReader.ReadLine();
                    }
                }
                else
                {
                    if (recordData.Contains("</PRE>"))
                    {
                        // End of the road
                        break;
                    }
                    else
                    {
                        position = 0;
                        // skip past empty lines
                        if (String.IsNullOrEmpty(recordData.Trim())) continue;

                        tableNumber = Convert.ToInt16(Utility.GetField(recordData, ref position));
                        if (!int.TryParse(Utility.GetField(recordData, ref position), out homeTeamNumber))
                        {
                            homeTeamNumber = 0;
                            // We've to do this anyway to move the cursor to the next field
                            homeTeamName = Utility.GetField(recordData, ref position);
                            homeTeamName = "BYE";
                        }
                        else
                        {
                            homeTeamName = Utility.GetField(recordData, ref position);
                        }

                        if (!int.TryParse(Utility.GetField(recordData, ref position), out awayTeamNumber))
                        {
                            awayTeamNumber = 0;
                            awayTeamName = "BYE";
                        }
                        else
                        {
                            awayTeamName = Utility.GetField(recordData, ref position);
                        }

                        // Find the last index of "-" which will give us the imp score
                        position = recordData.LastIndexOf("-");
                        if (position == -1)
                        {
                            vpScores = new string[2];
                            impScore1 = impScore2 = vpScores[0] = vpScores[1] = "0";
                        }
                        else
                        {
                            position = position - 4;

                            impScore1 = Utility.GetField(recordData, ref position, "-");
                            // Skip beyond the '-'
                            position = recordData.IndexOf("-", position) + 1;
                            impScore2 = recordData.Substring(position, 3).Trim();

                            position += 4;

                            // Skip past whitespace and find the VP score
                            position += Utility.skipWhiteSpace(recordData, position);

                            vpScore = recordData.Substring(position).Trim();
                            vpScores = vpScore.Split(new char[] { ' ' });
                            if (vpScores.Length == 3) vpScores[1] = vpScores[2];
                        }

                        row = results.NewRow();
                        row["TableNumber"] = tableNumber;
                        row["HomeTeamNumber"] = homeTeamNumber;
                        row["AwayTeamNumber"] = awayTeamNumber;
                        row["HomeTeamName"] = homeTeamName.Replace('_', ' ').Replace('~', '-');
                        row["AwayTeamName"] = awayTeamName.Replace('_', ' ').Replace('~', '-');
                        row["HomeTeamIMPScore"] = homeTeamNumber == 0 ? 0 : Convert.ToInt16(impScore1);
                        row["AwayTeamIMPScore"] = awayTeamNumber == 0 ? 0 : Convert.ToInt16(impScore2);
                        row["HomeTeamVPScore"] = homeTeamNumber == 0 ? 0 : Convert.ToInt16(vpScores[0]);
                        row["AwayTeamVPScore"] = awayTeamNumber == 0 ? 0 : Convert.ToInt16(vpScores[1]);
                        results.Rows.Add(row);
                    }
                }
            }

            return results;
        }

        static void MergeDrawAndScores(DataTable draw, Dictionary<int, int> scores)
        {
            int homeTeamNumber, awayTeamNumber;
            // Load cumulative scores up to now
            Dictionary<int, int> prevScores = LoadPreviousScores();
            foreach (DataRow row in draw.Rows)
            {
                homeTeamNumber = int.Parse(row["HomeTeamNumber"].ToString());
                awayTeamNumber = int.Parse(row["AwayTeamNumber"].ToString());

                // Make sure the scores table has the score for the team (it may be missing if the team has dropped out)
                if (homeTeamNumber != 0 && scores.ContainsKey(homeTeamNumber))
                    // Score for this round is current cumulative score - previous cumulative score 
                    row["HomeTeamScore"] = scores[homeTeamNumber] - prevScores[homeTeamNumber];
                else
                    row["HomeTeamScore"] = 0;

                if (awayTeamNumber != 0 && scores.ContainsKey(awayTeamNumber))
                    row["AwayTeamScore"] = scores[awayTeamNumber] - prevScores[awayTeamNumber];
                else
                    row["AwayTeamScore"] = 0;

                row.AcceptChanges();
            }
        }

        private static DataTable CreateMagicScoreTable()
        {
            DataTable scoreTable = new DataTable();
            scoreTable.Columns.Add("TableNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamName", Type.GetType("System.String"));
            scoreTable.Columns.Add("AwayTeamName", Type.GetType("System.String"));

            return scoreTable;
        }

        private static DataTable CreateMagicResultsTable()
        {
            DataTable scoreTable = new DataTable();
            scoreTable.Columns.Add("TableNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamIMPScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamIMPScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamVPScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamVPScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamName", Type.GetType("System.String"));
            scoreTable.Columns.Add("AwayTeamName", Type.GetType("System.String"));

            return scoreTable;
        }

        public static void GenerateDrawHTML(DataTable draw, Dictionary<int, int> scores)
        {
            bool success;
            string scoresTemplate, rowTemplate;
            rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RowTemplate.html"), out success);
            scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "DrawTemplate.html"), out success);

            string rowText, rowsText = String.Empty, linkText = String.Empty;
            int homeTeamNumber, awayTeamNumber, tableNumber, homeTeamScore, awayTeamScore;
            string result, homeTeamName, awayTeamName;

            string stage = configParameters["Stage"];
            string eventName = configParameters["EventName"];
            string backgroundColor = configParameters["ItemBackgroundColor"];
            string alternatingItemBackgroundColor = configParameters["AlternatingItemBackgroundColor"];

            scoresTemplate = scoresTemplate.Replace("[#DrawRoundNumber#]", roundForDraw.ToString());
            scoresTemplate = scoresTemplate.Replace("[#ScoreHeader#]", roundCompleted > 0 ?
                                String.Format("(Scores as of Round {0})", roundCompleted.ToString()) : String.Empty);
            scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);
            scoresTemplate = scoresTemplate.Replace("[#Stage#]", stage);
            int j = 0;

            foreach (DataRow row in draw.Select(String.Empty, "TableNumber"))
            {
                rowText = (j % 2) == 0 ? rowTemplate.Replace("[#BackgroundColor#]", backgroundColor) : rowTemplate.Replace("[#BackgroundColor#]", alternatingItemBackgroundColor);
                tableNumber = Convert.ToInt16(row["TableNumber"]);
                homeTeamNumber = Convert.ToInt16(row["HomeTeamNumber"]);
                awayTeamNumber = Convert.ToInt16(row["AwayTeamNumber"]);

                if (scores.ContainsKey(homeTeamNumber))
                {
                    homeTeamScore = scores[homeTeamNumber];
                    homeTeamName = row["HomeTeamName"].ToString();
                }
                else
                {
                    homeTeamScore = 0;
                    homeTeamName = homeTeamNumber == 0 ? "BYE" : String.Format("{0} (Withdrawn)", row["HomeTeamName"].ToString());
                }
                if (scores.ContainsKey(awayTeamNumber))
                {
                    awayTeamScore = scores[awayTeamNumber];
                    awayTeamName = row["AwayTeamName"].ToString();
                }
                else
                {
                    awayTeamScore = 0;
                    awayTeamName = awayTeamNumber == 0 ? "BYE" : String.Format("{0} (Withdrawn)", row["AwayTeamName"].ToString());
                }

                rowText = rowText.Replace("[#TableNumber#]", tableNumber.ToString());
                rowText = rowText.Replace("[#HomeTeamNumber#]", homeTeamNumber.ToString());
                rowText = rowText.Replace("[#HomeTeamName#]", homeTeamName);
                rowText = rowText.Replace("[#HomeTeamScore#]", roundCompleted > 0 ?
                           String.Format("({0})", homeTeamScore.ToString()) : String.Empty);
                rowText = rowText.Replace("[#AwayTeamNumber#]", awayTeamNumber.ToString());
                rowText = rowText.Replace("[#AwayTeamName#]", awayTeamName.ToString());
                rowText = rowText.Replace("[#AwayTeamScore#]", roundCompleted > 0 ?
                           String.Format("({0})", awayTeamScore.ToString()) : String.Empty);
                rowsText += rowText;
                j++;
            }

            result = scoresTemplate.Replace("[#Scores#]", rowsText);

            string outputFolder = configParameters["OutputFolder"] + "\\draws" + "\\Round " + roundForDraw.ToString();
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            string outputFileName = String.Format(@"{0}\Draw.html", outputFolder);
            Utility.WriteFile(outputFileName, result);
        }

        public static void GenerateResultsHTML(DataTable draw)
        {
            bool success;
            string resultsTemplate, rowTemplate;
            rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "ResultsRowTemplate.html"), out success);
            resultsTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "ResultsTemplate.html"), out success);

            string rowText, rowsText = String.Empty, linkText = String.Empty;
            int homeTeamNumber, awayTeamNumber, tableNumber, homeTeamIMPScore, awayTeamIMPScore, homeTeamVPScore, awayTeamVPScore;
            string result, homeTeamName, awayTeamName;

            string stage = configParameters["Stage"];
            string eventName = configParameters["EventName"];
            string backgroundColor = configParameters["ItemBackgroundColor"];
            string alternatingItemBackgroundColor = configParameters["AlternatingItemBackgroundColor"];

            resultsTemplate = resultsTemplate.Replace("[#DrawRoundNumber#]", roundForDraw.ToString());
            resultsTemplate = resultsTemplate.Replace("[#EventName#]", eventName);
            resultsTemplate = resultsTemplate.Replace("[#Stage#]", stage);
            resultsTemplate = resultsTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
            int j = 0;

            foreach (DataRow row in draw.Select(String.Empty, "TableNumber"))
            {
                rowText = (j % 2) == 0 ? rowTemplate.Replace("[#BackgroundColor#]", backgroundColor) : rowTemplate.Replace("[#BackgroundColor#]", alternatingItemBackgroundColor);
                tableNumber = Convert.ToInt16(row["TableNumber"]);
                homeTeamNumber = Convert.ToInt16(row["HomeTeamNumber"]);
                awayTeamNumber = Convert.ToInt16(row["AwayTeamNumber"]);

                homeTeamName = row["HomeTeamName"].ToString();
                homeTeamIMPScore = Convert.ToInt16(row["HomeTeamIMPScore"]);
                homeTeamVPScore = Convert.ToInt16(row["HomeTeamVPScore"]);

                awayTeamName = row["AwayTeamName"].ToString();
                awayTeamIMPScore = Convert.ToInt16(row["AwayTeamIMPScore"]);
                awayTeamVPScore = Convert.ToInt16(row["AwayTeamVPScore"]);

                rowText = rowText.Replace("[#TableNumber#]", tableNumber.ToString());
                rowText = rowText.Replace("[#HomeTeamNumber#]", homeTeamNumber.ToString());
                rowText = rowText.Replace("[#HomeTeamName#]", homeTeamName);
                rowText = rowText.Replace("[#HomeTeamIMPScore#]", homeTeamIMPScore.ToString());
                rowText = rowText.Replace("[#HomeTeamVPScore#]", homeTeamVPScore.ToString());
                rowText = rowText.Replace("[#AwayTeamNumber#]", awayTeamNumber.ToString());
                rowText = rowText.Replace("[#AwayTeamName#]", awayTeamName.ToString());
                rowText = rowText.Replace("[#AwayTeamIMPScore#]", awayTeamIMPScore.ToString());
                rowText = rowText.Replace("[#AwayTeamVPScore#]", awayTeamVPScore.ToString());
                rowsText += rowText;
                j++;
            }

            result = resultsTemplate.Replace("[#Scores#]", rowsText);

            string outputFolder = configParameters["OutputFolder"] + "\\results" + "\\Round " + roundForDraw.ToString();
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            string outputFileName = String.Format(@"{0}\Results.html", outputFolder);
            Utility.WriteFile(outputFileName, result);
        }

        public static void WriteDrawToFile(DataTable draw)
        {
            string drawFolder = configParameters["OutputFolder"] + "\\SavedDraws";
            if (!Directory.Exists(drawFolder)) Directory.CreateDirectory(drawFolder);

            string drawFileName = String.Format(@"{0}\DrawRound{1}.csv", drawFolder, roundForDraw);
            string content;

            content = "Table, Team1, Team2\r\n";
            foreach (DataRow row in draw.Rows)
            {
                content += String.Format("{0},{1},{2}\r\n", row["TableNumber"], row["HomeTeamNumber"], row["AwayTeamNumber"]);
            }

            Utility.WriteFile(drawFileName, content);
        }

        // Load the previous draw
        public static DataTable LoadPreviousDraw()
        {
            string drawFolder = configParameters["OutputFolder"] + "\\SavedDraws";
            string drawFileName = String.Format(@"{0}\DrawRound{1}.csv", drawFolder, roundCompleted);

            StreamReader fileStream = null;

            DataTable draw = CreateScoreTable();

            // If we can't read the file, we return an empty datatable
            try
            {
                fileStream = new StreamReader(drawFileName);
            }
            catch (Exception)
            {
                return draw;
            }

            string row;
            string[] data;
            DataRow dataRow;

            // Skip past the header row
            fileStream.ReadLine();

            while (true)
            {
                row = fileStream.ReadLine();
                if (String.IsNullOrEmpty(row)) break;
                data = row.Split(new char[] { ',' });

                dataRow = draw.NewRow();
                dataRow["TableNumber"] = Convert.ToInt16(data[0]);
                dataRow["HomeTeamNumber"] = Convert.ToInt16(data[1]);
                dataRow["AwayTeamNumber"] = Convert.ToInt16(data[2]);

                draw.Rows.Add(dataRow);
            }

            fileStream.Close();

            return draw;
        }

        public static void WriteScoresToFile(Dictionary<int, int> scores)
        {
            string scoresFolder = configParameters["OutputFolder"] + "\\SavedScores";
            if (!Directory.Exists(scoresFolder)) Directory.CreateDirectory(scoresFolder);

            string scoresFileName = String.Format(@"{0}\CumulativeScores.csv", scoresFolder);
            string content;

            content = "TeamNumber, Score\r\n";
            foreach (int key in scores.Keys)
            {
                content += String.Format("{0},{1}\r\n", key, scores[key]);
            }

            Utility.WriteFile(scoresFileName, content);
        }

        // Load the previous draw
        public static Dictionary<int, int> LoadPreviousScores()
        {
            string scoresFolder = configParameters["OutputFolder"] + "\\SavedScores";
            string drawFileName = String.Format(@"{0}\CumulativeScores.csv", scoresFolder);

            StreamReader fileStream = null;

            Dictionary<int, int> scores = new Dictionary<int, int>();

            // If we can't read the file, we return an empty list
            try
            {
                fileStream = new StreamReader(drawFileName);
            }
            catch (Exception)
            {
                return scores;
            }

            string row;
            string[] data;

            // Skip past the header row
            fileStream.ReadLine();

            while (true)
            {
                row = fileStream.ReadLine();
                if (String.IsNullOrEmpty(row)) break;
                data = row.Split(new char[] { ',' });

                scores.Add(Convert.ToInt16(data[0]), Convert.ToInt16(data[1]));
            }

            fileStream.Close();

            return scores;
        }

        #endregion

        #region CSV
        private static void Initialize()
        {
            debug = Convert.ToBoolean(configParameters["DebugMode"]);

            string roundsFileName = String.Format(@"{0}\roundCompleted.txt", configParameters["InputFolder"]);
            bool success;
            string content = Utility.ReadFile(roundsFileName, out success, true);
            roundCompleted = String.IsNullOrEmpty(content) ? 1 : Convert.ToInt32(content) + 1;
        }

        private static void ProcessCSVScoring()
        {
            Initialize();
            DataTable scores = ReadCSVFile();
            spreadsheetAPI.updateScores(roundCompleted, scores, debug);
            string roundsFileName = String.Format(@"{0}\roundCompleted.txt", configParameters["InputFolder"]);
            Utility.WriteFile(roundsFileName, String.Format("{0}", roundCompleted));
        }

        private static DataTable ReadCSVFile()
        {
            string recordData;
            string[] dataElements;
            string fileName = String.Format(@"{0}\SwissLeagueScore.csv", configParameters["InputFolder"]);
            StreamReader streamReader = new StreamReader(fileName);
            List<string> invalidFileData = new List<string>();
            DataTable scores = CreateScoreTable();
            DataRow row;
            int rowCount = 1;

            while (!streamReader.EndOfStream)
            {
                // Read in a line of the file data
                recordData = streamReader.ReadLine();

                // check the line is not empty
                if (recordData.Trim() != String.Empty)
                {
                    dataElements = Utility.SplitCSVDoubleQuotes(recordData, ",");

                    // Check data elements length
                    // and add it to the list for further process
                    if (dataElements.Length != 4)
                        // add invalid row 
                        invalidFileData.Add(recordData);
                    else
                    {
                        row = scores.NewRow();
                        row[0] = rowCount++;
                        for (int i = 1; i < 5; i++)
                            row[i] = dataElements[i-1];
                        scores.Rows.Add(row);
                    }
                }
            }

            if (invalidFileData.Count > 0)
                Console.WriteLine("Invalid data found");
            return scores;
        }

        private static DataTable CreateScoreTable()
        {
            DataTable scoreTable = new DataTable();
            scoreTable.Columns.Add("TableNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamNumber", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("HomeTeamScore", Type.GetType("System.Int16"));
            scoreTable.Columns.Add("AwayTeamScore", Type.GetType("System.Int16"));

            return scoreTable;
        }
        #endregion
    }
}
