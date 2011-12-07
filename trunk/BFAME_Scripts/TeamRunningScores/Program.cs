using System;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using Upload_To_Google_Sites;
using IndianBridge.Common;

namespace BridgeMateRunningScores
{
	class Program
	{
		#region Members

		static bool isEndOfSegment = false;
		static int roundInProgress = 0;
		static int segmentInProgress = 0;
		static int totalNumberOfTeams = 0;
		static int numberOfBoardsPerRound = 0;
		static String eventName = String.Empty;
		static NameValueCollection configParameters = null;
		static NameValueCollection pairNames = null;
		static bool playOffMode = false;
		static bool isMultiSegment = false;
		static bool isMultiRound = false;

		#endregion

		#region Methods

		static void Main(string[] args)
		{
			#region Initial Setup

			long elapsedTime;
			Stopwatch stopwatch = null;
			NameValueCollection nameNumberMapping = null;
			SpreadSheetAPI spreadsheetAPI = null;
			SitesAPI sitesAPI = null;

			// Read all configuration parameters
			configParameters = ReadConfigParameters();

			bool debug = Convert.ToBoolean(configParameters["DebugMode"]);
			playOffMode = Convert.ToBoolean(configParameters["Playoffs"]);
			isMultiSegment = Convert.ToInt16(configParameters["NumberOfSegmentsPerRound"]) > 1;

			#region Initialize SpreadsheetAPI and gather information

			if (Boolean.Parse(configParameters["RunUpdateGoogleSpreadsheet"]))
			{
				Console.WriteLine("Retrieving event parameter data from spreadsheet");
				Console.WriteLine();
				try
				{
					spreadsheetAPI = new SpreadSheetAPI(configParameters["GoogleSpreadsheetName"], configParameters["Username"], configParameters["Password"], debug);
					totalNumberOfTeams = spreadsheetAPI.getNumberOfTeams();
					eventName = spreadsheetAPI.getEventName();
					numberOfBoardsPerRound = spreadsheetAPI.getNumberOfBoards();
					nameNumberMapping = spreadsheetAPI.getTeamNames(debug);
					isMultiRound = spreadsheetAPI.getNumberOfRounds() > 1;
				}
				catch (Exception)
				{
					if (debug) Console.WriteLine("Spreadsheet call failed. Using parameters from Config");
					GetEventParametersFromConfig(out totalNumberOfTeams, out eventName, out numberOfBoardsPerRound,
						out nameNumberMapping);
				}
			}
			else {
				Console.WriteLine("Retrieving event parameter data from config");
				Console.WriteLine();
				GetEventParametersFromConfig(out totalNumberOfTeams, out eventName, out numberOfBoardsPerRound, 
					out nameNumberMapping);
			}

			#endregion

			// If we are in playoffs, always read number of rounds and teams from config
			if (isMultiSegment)
			{
				numberOfBoardsPerRound = Convert.ToInt32(configParameters["NumberOfBoardsPerSegment"]);
			}

			if (playOffMode)
			{
				totalNumberOfTeams = Convert.ToInt32(configParameters["TotalNumberOfTeams"]);
			}

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
			while (true) {
				try
				{
					stopwatch = Stopwatch.StartNew();
					if (debug) Console.WriteLine("Running at " + DateTime.Now.ToString());
					// Calculate Scores and Update Files
					Console.WriteLine("Creating Butler, Running scores and Board files");
					Console.WriteLine();
					 DataTable runningScores = CreateButlerAndRunningScoresFiles(totalNumberOfTeams, nameNumberMapping, debug);

					if (Boolean.Parse(configParameters["RunUpdateGoogleSite"]))
					{
						// Upload Running Scores
						Console.WriteLine("Uploading running scores");
						Console.WriteLine();

						sitesAPI.uploadDirectory(configParameters["OutputFolder"]
							+ "\\runningscores", configParameters["GoogleRunningScoresRoot"], configParameters["BackupFolder"]);
					}

					if (isEndOfSegment)
					{
						// Upload Butler Scores at the end of each segment
						if (Boolean.Parse(configParameters["RunUpdateGoogleSite"]) && Boolean.Parse(configParameters["UseButlerScores"]))
						{
							Console.WriteLine("Uploading Butler Scores");
							Console.WriteLine();
							sitesAPI.uploadDirectory(configParameters["OutputFolder"] + "\\butlerscores",
								configParameters["GoogleButlerScoresRoot"], configParameters["BackupFolder"]);
						}

						// Only write to the spreadsheet at the end of the round - we don't track segment scores in the spreadsheet
						if (Boolean.Parse(configParameters["RunUpdateGoogleSpreadsheet"]) 
							&& segmentInProgress == Convert.ToInt16(configParameters["NumberOfSegmentsPerRound"]))
						{
							Console.WriteLine(String.Format("Uploading spreadsheet with round scores for Round {0}", roundInProgress));
							Console.WriteLine();
							GetWebScores(ref runningScores, nameNumberMapping);
							spreadsheetAPI.updateScores(roundInProgress, runningScores, debug);
						}
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

		static void GetWebScores(ref DataTable runningScores, NameValueCollection nameNumberMapping)
		{
			foreach (DataRow row in runningScores.Rows)
			{
				row["HomeTeamNumber"] = nameNumberMapping[row["HomeTeam"].ToString().Replace('_', ' ').Replace('~', '-')];
				row["AwayTeamNumber"] = nameNumberMapping[row["AwayTeam"].ToString().Replace('_', ' ').Replace('~', '-')];
			}
		}

		static void GetEventParametersFromConfig(out int totalNumberOfTeams, out string eventName,
			out int numberOfBoardsPerRound, out NameValueCollection nameNumberMapping)
		{
			totalNumberOfTeams = Convert.ToInt16(configParameters["TotalNumberOfTeams"]);
			eventName = configParameters["EventName"];
			numberOfBoardsPerRound = int.Parse(configParameters["NumberOfBoardsPerRound"]);
			nameNumberMapping = GetTeamNumbersNamesMapping();
		}

		static NameValueCollection ReadConfigParameters()
		{
			return new NameValueCollection(ConfigurationManager.AppSettings);
		}

		static DataTable CreateButlerAndRunningScoresFiles(int totalNumberOfTeams, 
			NameValueCollection nameNumberMapping, Boolean debug = false)
		{
			int roundsComputed = 0, segmentsComputed = 0;
			int numberOfMatchesPerRound = ((totalNumberOfTeams + 1) / 2);
			int numberOfPairs = 4*(totalNumberOfTeams/2);
			int numberOfTables = 2 * numberOfMatchesPerRound;
			string boardResultText, outputFileName;
			bool hasNewResults;
			DataTable cumulativeButlerScores;
			String eventFolder = String.Empty, boardsOutputFolder;
			NameValueCollection playedBoards;
			int boardNumber = 0;

			MagicInterface magicInterface = new MagicInterface(configParameters["InputFolder"], configParameters["RunningScoreFileName"],
				configParameters["ButlerFileName"], configParameters["RunningScoresFileName"], configParameters["BoardResultFont"], Convert.ToBoolean(configParameters["BoardResultFontBold"]));
			DataTable runningScores = magicInterface.GetRunningScores(((totalNumberOfTeams+1) / 2), 
				Convert.ToBoolean(configParameters["HasCarryOver"]), out roundInProgress, out segmentInProgress);

			if (roundInProgress > 0)
			{
				// Only retrieve pair names if they've not already been retrieved
				if (Boolean.Parse(configParameters["UseButlerScores"]) && (pairNames == null || pairNames.Count == 0))
				{
					pairNames = magicInterface.GetPairNames(numberOfPairs);
				}

                if (Boolean.Parse(configParameters["GetBoardResults"]))
				{
					eventFolder = String.Format(@"{0}\runningscores\{1}", configParameters["OutputFolder"], configParameters["RunningScoresFileName"]);
					if (!Directory.Exists(eventFolder)) Directory.CreateDirectory(eventFolder);

					if (isMultiRound)
					{
						boardsOutputFolder = isMultiSegment ? String.Format(@"{0}\round{1}\segment{2}", eventFolder, roundInProgress.ToString(), segmentInProgress.ToString())
							: String.Format(@"{0}\round{1}", eventFolder, roundInProgress.ToString());
					}
					else
					{
						boardsOutputFolder = isMultiSegment ? String.Format(@"{0}\segment{1}", eventFolder, segmentInProgress.ToString())
							: String.Format(@"{0}\segment1", eventFolder);
					}

					if (!Directory.Exists(boardsOutputFolder)) Directory.CreateDirectory(boardsOutputFolder);

					for (int i = 1; i <= numberOfBoardsPerRound; i++)
					{
						boardResultText = magicInterface.GetBoardResults(i, pairNames, numberOfTables, isMultiRound, isMultiSegment, out hasNewResults);

						// only update the file if results have been updated
						if (hasNewResults)
						{
							outputFileName = String.Format(@"{0}\board-{1}.html", boardsOutputFolder, i.ToString());
							Utility.WriteFile(outputFileName, boardResultText);
							// If this is a multi-segment event, also write all the boards to the Round folder
							if (isMultiSegment)
							{
								boardNumber = (segmentInProgress - 1) * numberOfBoardsPerRound + i;
								boardsOutputFolder = String.Format(@"{0}\round{1}\boards", eventFolder, roundInProgress.ToString());
								if (!Directory.Exists(boardsOutputFolder)) Directory.CreateDirectory(boardsOutputFolder);
								outputFileName = String.Format(@"{0}\board-{1}.html", boardsOutputFolder, boardNumber.ToString());
								Utility.WriteFile(outputFileName, boardResultText);
							}
						}
					}
				}

				playedBoards = GetPlayedBoards(numberOfMatchesPerRound, magicInterface.CompletedBoards);
				GenerateRunningScoresHTML(runningScores, roundInProgress, segmentInProgress, playedBoards, nameNumberMapping);

				// Perform closure actions at end of segment
				if (isEndOfSegment && Boolean.Parse(configParameters["UseButlerScores"]))
				{
					// Only re-compute results if we haven't already generated butler results for this round
					bool success;
					string butlerRoundsFileName = String.Format(@"{0}\ButlerRoundsComputed.txt", configParameters["OutputFolder"]);
					string content = Utility.ReadFile(butlerRoundsFileName, out success, true);

					string[] values = content.Split(new char[] { '-' });
					if (values.Length == 2)
					{
						roundsComputed = Convert.ToInt32(values[0]);
						segmentsComputed = Convert.ToInt32(values[1]);
					}

					// We always re-generate the butler scores (in case we are re-running the program for the same session)
					if (debug) Console.WriteLine("Generating Butler Results...");
					GenerateButlerScoresHTML(magicInterface.ButlerResults, roundInProgress, segmentInProgress, false);

					// If this is a new segment (or a new round), we will write butler results to the file system
					if (segmentInProgress != segmentsComputed || roundInProgress != roundsComputed)
					{
						cumulativeButlerScores = MergeCurrentButlerScoresIntoCumulativeResults(magicInterface.ButlerResults);
						Utility.WriteFile(butlerRoundsFileName, String.Format("{0}-{1}", roundInProgress.ToString(), segmentInProgress.ToString()));
						WriteButlerScoresToFile(cumulativeButlerScores, true);
					}
					else
					{
						cumulativeButlerScores = LoadCumulativeButlerResults();
					}

					GenerateButlerScoresHTML(cumulativeButlerScores, roundInProgress, segmentInProgress, true);

					//// If this is the final segment of the current round, update the Rounds computed file
					////if (roundInProgress > roundsComputed && segmentInProgress == Convert.ToInt16(configParameters["NumberOfSegmentsPerRound"]))
					////{
					////    roundsComputed = roundInProgress;
					////}
					////else
					////{
					////    cumulativeButlerScores = LoadCumulativeButlerResults();
					////}

					//// Always re-generate cross event butler results so that the file gets updated when any of 
					//// the butler files are re-generated
					//CreateCrossEventButlerScores(cumulativeButlerScores, roundInProgress);
				}

			}

			return runningScores;

		}

		public static void CreateCrossEventButlerScores(DataTable currentEventCumulativeResults, int roundInProgress)
		{
			string otherEventButlerFiles;
			DataTable butlerResults;
			DataTable crossEventButlerResults = CreateNewButlerResultsTable();
			if (Convert.ToBoolean(configParameters["GenerateCrossEventButlerScore"]))
			{
				otherEventButlerFiles = configParameters["CrossEventButlerFilesPath"];
				string[] butlerFiles = otherEventButlerFiles.Split(new char[] { ';' });

				foreach (string path in butlerFiles)
				{
					butlerResults = LoadCumulativeButlerResults(path);
					MergeCurrentButlerScoresIntoCumulativeResults(butlerResults, crossEventButlerResults);
				}
				MergeCurrentButlerScoresIntoCumulativeResults(currentEventCumulativeResults, crossEventButlerResults);

				GenerateButlerScoresHTML(crossEventButlerResults, roundInProgress, segmentInProgress, false, true);
			}

		}

		public static void GenerateRunningScoresHTML(DataTable runningScores, int roundInProgres, int segmentInProgress,
			NameValueCollection completedBoards, NameValueCollection teamNumbers)
		{
			bool success;
			int minPlayedBoards = numberOfBoardsPerRound, playedBoards = 0;
			string rowText, rowsText = String.Empty, linkText = String.Empty;
			string result, homeTeam, awayTeam, roundText = String.Empty, path;

			string scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RunningScoresTemplate.html"), out success);
			string rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "RowTemplate.html"), out success);
			string boardTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "BoardTemplate.html"), out success);
			string stage = configParameters["Stage"];

			if (isMultiRound)
			{
				roundText = isMultiSegment ? String.Format("Round {0}/Segment {1}", roundInProgres.ToString(), segmentInProgress.ToString()) :
					String.Format("Round {0}", roundInProgres.ToString());
			}
			else
			{
				roundText = isMultiSegment ? String.Format("Segment {0}", segmentInProgress.ToString()) : "Segment 1";
			}

			scoresTemplate = scoresTemplate.Replace("[#Stage#]", stage);
			scoresTemplate = scoresTemplate.Replace("[#SegmentNumber#]", roundText);
			scoresTemplate = scoresTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
			scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);

			int j = 0;
			
			foreach (DataRow row in runningScores.Rows) 
			{
				rowText = (j % 2) == 0 ? rowTemplate : rowTemplate.Replace("background-color:#def", "background-color:#ddd");
				rowText = rowText.Replace("[#TableNumber#]", row["TableNumber"].ToString());

				homeTeam = row["HomeTeam"].ToString().Replace('_', ' ').Replace('~', '-');
				awayTeam = row["AwayTeam"].ToString().Replace('_', ' ').Replace('~', '-');

				rowText = rowText.Replace("[#HomeTeam#]", homeTeam);
				rowText = rowText.Replace("[#AwayTeam#]", awayTeam);

				rowText = rowText.Replace("[#IMPScore#]", row["IMPScore"].ToString());
				rowText = rowText.Replace("[#VPScore#]", row["VPScore"].ToString());

				linkText = configParameters["TeamLinkTemplate"].Replace("[#TeamNumber#]", teamNumbers[homeTeam]);
				rowText = rowText.Replace("[#HomeTeamLink#]", linkText);

				linkText = configParameters["TeamLinkTemplate"].Replace("[#TeamNumber#]", teamNumbers[awayTeam]);
				rowText = rowText.Replace("[#AwayTeamLink#]", linkText);

				rowText = rowText.Replace("[#RunningScoresFileName#]", configParameters["RunningScoresFileName"]);

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
				isEndOfSegment = true;
			}

			result = scoresTemplate.Replace("[#Scores#]", rowsText);

			rowsText = String.Empty;

			if (Boolean.Parse(configParameters["GetBoardResults"]))
			{
				if (isMultiRound)
				{
					if (isMultiSegment)
					{
						path = String.Format("round{0}/segment{1}", roundInProgres, segmentInProgress);
					}
					else
					{
						path = String.Format("round{0}", roundInProgres);
					}
				}
				else
				{
					if (isMultiSegment)
					{
						path = String.Format("segment{1}", segmentInProgress);
					}
					else
					{
						path = "segment1";
					}
				}

				for (int i = 1; i <= numberOfBoardsPerRound; i++)
				{
					// Alternating backgrounds
					rowText = (i % 2) == 0 ? boardTemplate.Replace("background-color:#def", "background-color:#ddd") : boardTemplate;

					linkText = configParameters["BoardLinkTemplate"];
					linkText = linkText.Replace("[#BoardNumber#]", i.ToString());
					linkText = linkText.Replace("[#Path#]", String.Format("{0}/{1}", configParameters["RunningScoresFileName"], path));
					rowText = rowText.Replace("[#BoardLink#]", linkText);
					rowText = rowText.Replace("[#BoardNumber#]", i.ToString());
					rowsText += rowText;
				}

				result = result.Replace("[#Boards#]", rowsText);
			}
			else
			{
				result = result.Replace("[#Boards#]", "<tr><td>No board results available</td></tr>");
                // If we are not parsing board files, we assume it's the end of the segment
                isEndOfSegment = true;
			}

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
			int numPlayedBoards, tableNum;

			for (int i = 0; i < numberOfMatches; i++)
			{
				numPlayedBoards = 0;
				tableNum = Convert.ToInt32(ConfigurationManager.AppSettings["StartTableNumber"]) + i;
				// Find rows completed at table A-n (where n is the table number)
				rows = completedBoards.Select(String.Format("Table='A-{0}'", tableNum));

				foreach (DataRow row in rows) 
				{
					if (completedBoards.Select(String.Format("Table='B-{0}' AND Board='{1}'", tableNum, row["Board"].ToString())).Length > 0) 
					{
						numPlayedBoards++;
					}
				}

				playedBoards.Add(tableNum.ToString(), numPlayedBoards.ToString());
			}

			return playedBoards;
		}

		public static void WriteButlerScoresToFile(DataTable butlerScores, bool cumulative = false)
		{
			string butlerResultsFileName = String.Format(@"{0}\{1}", configParameters["OutputFolder"], String.Format("{0}ButlerResults.csv", cumulative ? "Cumulative" : String.Empty));
			string content;

			content = "Pair, Boards, Score\r\n";
			foreach (DataRow row in butlerScores.Rows)
			{
				content += String.Format("{0},{1},{2}\r\n", row["Pair"], row["Boards"], row["Score"]);
			}

			Utility.WriteFile(butlerResultsFileName, content);
		}

		// Load the Cumulative results thus far into a datatable
		// The path can be used when the cumulative results have to be loaded for a different event
		public static DataTable LoadCumulativeButlerResults(string path = "")
		{
			string butlerResultsFileName = String.IsNullOrEmpty(path) ?
				String.Format(@"{0}\CumulativeButlerResults.csv", configParameters["OutputFolder"]) : path;

			StreamReader fileStream = null;

			DataTable butlerResults = CreateNewButlerResultsTable();

			// If we couldn't read the file, we return an empty datatable
			try
			{
				fileStream = new StreamReader(butlerResultsFileName);
			}
			catch (Exception)
			{
				return butlerResults;
			}

			string row;
			string[] data;
			DataRow dataRow;
			int boards;
			decimal score;

			// Skip past the header row
			fileStream.ReadLine();

			while (true)
			{
				row = fileStream.ReadLine();
				if (String.IsNullOrEmpty(row)) break;
				data = row.Split(new char[] { ',' });

				dataRow = butlerResults.NewRow();
				dataRow["Pair"] = data[0];
				boards = Convert.ToInt16(data[1]);
				score = Convert.ToDecimal(data[2]);
				dataRow["Boards"] = boards;
				dataRow["Score"] = score;
				dataRow["AvgScore"] = score/boards;

				butlerResults.Rows.Add(dataRow);
			}

			fileStream.Close();

			return butlerResults;
		}

		public static NameValueCollection GetTeamNumbersNamesMapping()
		{
			NameValueCollection names = new NameValueCollection();
			string teamNamesFileName = String.Format(@"{0}\TeamNames.csv", configParameters["InputFolder"]);

			if (File.Exists(teamNamesFileName))
			{
				StreamReader fileStream = new StreamReader(teamNamesFileName);
				string row;
				string[] data;

				// Skip past the header row
				fileStream.ReadLine();

				while (true)
				{
					row = fileStream.ReadLine();
					if (String.IsNullOrEmpty(row)) break;
					data = row.Split(new char[] { ',' });

					names.Add(data[1], data[0]);
				}

				fileStream.Close();
			}

			return names;
		}

		private static DataTable CreateNewButlerResultsTable()
		{
			DataTable butlerResults = new DataTable();
			butlerResults.Columns.Add("Pair", typeof(System.String));
			butlerResults.Columns.Add("Boards", typeof(System.Int16));
			butlerResults.Columns.Add("Score", typeof(System.Decimal));
			butlerResults.Columns.Add("AvgScore", typeof(System.Decimal));

			return butlerResults;
		}

		public static DataTable MergeCurrentButlerScoresIntoCumulativeResults(DataTable butlerResults, 
			DataTable cumulativeResults = null)
		{
			DataTable cumulativeButlerResults = cumulativeResults ?? LoadCumulativeButlerResults();

			foreach (DataRow row in butlerResults.Rows)
			{
				Utility.UpdateButlerResults(cumulativeButlerResults, row["Pair"].ToString(),
					Convert.ToDecimal(row["Score"]), Convert.ToInt16(row["Boards"]));
			}

			return cumulativeButlerResults;
		}

		public static void GenerateButlerScoresHTML(DataTable butlerScores, int roundInProgres, int segmentInProgress,
			bool cumulative, bool acrossEvents = false)
		{
			bool success;
			string templateFileName, scoresTemplate, rowTemplate, butlerOutputFolder;
			int numberOfSegmentsPerRound = Convert.ToInt16(configParameters["NumberOfSegmentsPerRound"]);
			rowTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], "ButlerRowTemplate.html"), out success);
			if (!acrossEvents)
			{
				templateFileName = String.Format("{0}ButlerScoresTemplate.html", cumulative ? "Cumulative" : String.Empty);
				scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], templateFileName), out success);
			}
			else
			{
				templateFileName = String.Format("CrossEventButlerScoresTemplate.html");
				scoresTemplate = Utility.ReadFile(String.Format(@"{0}\{1}", configParameters["TemplateFolder"], templateFileName), out success);
			}

			string rowText, rowsText = String.Empty, linkText = String.Empty;
			int boards; decimal score; decimal avgScore;
			string result, roundText, roundLinksText = String.Empty, roundLinkTemplate, path = String.Empty;

			string stage = configParameters["Stage"];

			if (isMultiRound)
			{
				roundText = isMultiSegment ? String.Format("Round {0}/Segment {1}", roundInProgres.ToString(), segmentInProgress.ToString()) :
					String.Format("Round {0}", roundInProgres.ToString());
			}
			else
			{
				roundText = isMultiSegment ? String.Format("Segment {0}", segmentInProgress.ToString()) : "Segment 1";
			}

			scoresTemplate = scoresTemplate.Replace("[#Stage#]", stage);
			scoresTemplate = scoresTemplate.Replace("[#SegmentNumber#]", roundText);
			scoresTemplate = scoresTemplate.Replace("[#TimeStamp#]", Utility.GetTimeStamp());
			scoresTemplate = scoresTemplate.Replace("[#TournamentName#]", configParameters["TournamentName"]);
			scoresTemplate = scoresTemplate.Replace("[#EventName#]", eventName);

			scoresTemplate = scoresTemplate.Replace("[#ButlerScoresRoot#]", GetBackToCumulativeScoresLinktext(isMultiRound, isMultiSegment, configParameters["ButlerScoresFilename"].ToString()));
			int j = 0;

			foreach (DataRow row in butlerScores.Select(String.Empty, "AvgScore Desc"))
			{
				rowText = (j % 2) == 0 ? rowTemplate : rowTemplate.Replace("background-color:#def", "background-color:#ddd");
				boards = Convert.ToInt16(row["Boards"]);
				score = Convert.ToDecimal(row["Score"]);
				if (row["AvgScore"] != DBNull.Value)
				{
					avgScore = Convert.ToDecimal(row["AvgScore"]);
				}
				else
				{
					avgScore = score/boards;
				}

				rowText = rowText.Replace("[#Pair#]", row["Pair"].ToString().Replace('_', ' ').Replace('~', '-'));
				rowText = rowText.Replace("[#Boards#]", boards.ToString());
				rowText = rowText.Replace("[#TotalScore#]", score.ToString());
				rowText = rowText.Replace("[#AvgScore#]", avgScore.ToString("#.##"));
				rowsText += rowText;
				j++;
			}

			result = scoresTemplate.Replace("[#Scores#]", rowsText);

			if (cumulative)
			{
				if (isMultiRound)
				{
					path = isMultiSegment ? "round[#RoundNumber#]/segment[#SegmentNumber#]" : "round[#RoundNumber#]";
					result = result.Replace("[#RoundWiseHeaderText#]", "Round-wise Butler results");
				}
				else
				{
					path = isMultiSegment ? "segment[#SegmentNumber#]" : "segment1";
					result = result.Replace("[#RoundWiseHeaderText#]", "Segment-wise Butler results");
				}

				roundLinkTemplate = String.Format("<td><a href='{1}'</a>{0} |</td>", path, 
					configParameters["ButlerScoresFileName"] + "/" + configParameters["ButlerRoundLinkTemplate"].Replace("[#Path#]", path));

				for (int i = 1; i <= roundInProgres; i++)
				{
					for (int k = 1; k <= numberOfSegmentsPerRound; k++)
					{
						roundLinksText += roundLinkTemplate.Replace("[#SegmentNumber#]", k.ToString()).Replace("[#RoundNumber#]", i.ToString());
						// For the current round, don't generate past the current segment
						if (i == roundInProgress && k == segmentInProgress) break;
					}
				}

				result = result.Replace("[#RoundLinks#]", roundLinksText);
			}

			if (!acrossEvents)
			{
				if (cumulative)
				{
					butlerOutputFolder = configParameters["OutputFolder"] + "\\butlerscores";
					if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
					String butlerRootFolder = String.Format(@"{0}\{1}", butlerOutputFolder, configParameters["ButlerScoresFileName"]);
					if (!Directory.Exists(butlerRootFolder)) Directory.CreateDirectory(butlerRootFolder);
					string outputFileName = String.Format(@"{0}\index.html", butlerRootFolder);
					Utility.WriteFile(outputFileName, result);
				}
				else
				{
					string butlerRootFolder = configParameters["OutputFolder"] + "\\butlerscores\\" + configParameters["ButlerScoresFileName"];
					if (!Directory.Exists(butlerRootFolder)) Directory.CreateDirectory(butlerRootFolder);

					if (Convert.ToInt16(configParameters["NumberOfSegmentsPerRound"]) == 1)
					{
						butlerOutputFolder = String.Format(@"{0}\{1}", butlerRootFolder, String.Format("round{0}", roundInProgres.ToString()));
						if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
					}
					else
					{
						// If there are multiple segments per round, create a sub-folder for each segment
						butlerOutputFolder = String.Format(@"{0}\{1}\{2}", butlerRootFolder, String.Format("round{0}", roundInProgres.ToString()),
							String.Format("segment{0}", segmentInProgress.ToString()));
						if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
					}

					string outputFileName = String.Format(@"{0}\butlerscores.html", butlerOutputFolder);
					Utility.WriteFile(outputFileName, result);
				}
			}
			else
			{
				butlerOutputFolder = configParameters["OutputFolder"] + "\\butlerscores";
				if (!Directory.Exists(butlerOutputFolder)) Directory.CreateDirectory(butlerOutputFolder);
				string outputFileName = String.Format(@"{0}\CrossEventButlerScores.html", butlerOutputFolder);
				Utility.WriteFile(outputFileName, result);
			}
		}

		#endregion

		private static string GetBackToCumulativeScoresLinktext(bool isMultiRound, bool isMultiSegment, string rootFolder)
		{
			string link = String.Empty;
			if (isMultiRound)
			{
				if (isMultiSegment)
					link = String.Format("../../../{0}", rootFolder);
				else
					link = String.Format("../../{0}", rootFolder);
			}
			else
			{
				if (isMultiSegment)
					link = String.Format("../../{0}", rootFolder);
				else
					link = String.Format("../{0}", rootFolder);
			}

			return link;
		}
	}
}
