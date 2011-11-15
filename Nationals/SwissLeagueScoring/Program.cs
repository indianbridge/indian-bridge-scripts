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

namespace SwissLeagueScoring
{
    class Program
    {
        static SpreadSheetAPI spreadsheetAPI = null;
        static NameValueCollection configParameters;
        static bool debug;
        static int roundsCompleted;

        static void Main(string[] args)
        {
            Initialize();
            Process();
        }

        static NameValueCollection ReadConfigParameters()
        {
            return new NameValueCollection(ConfigurationManager.AppSettings);
        }

        private static void Initialize()
        {
            configParameters = ReadConfigParameters();
            debug = Convert.ToBoolean(configParameters["DebugMode"]);
            spreadsheetAPI = new SpreadSheetAPI(configParameters["GoogleSpreadsheetName"], configParameters["Username"], configParameters["Password"], debug);

            string roundsFileName = String.Format(@"{0}\RoundsCompleted.txt", configParameters["InputFolder"]);
            bool success;
            string content = Utility.ReadFile(roundsFileName, out success, true);
            roundsCompleted = String.IsNullOrEmpty(content) ? 1 : Convert.ToInt32(content) + 1;
        }

        private static void Process()
        {
            DataTable scores = ReadCSVFile();
            spreadsheetAPI.updateScores(roundsCompleted, scores, debug);
            string roundsFileName = String.Format(@"{0}\RoundsCompleted.txt", configParameters["InputFolder"]);
            Utility.WriteFile(roundsFileName, String.Format("{0}", roundsCompleted));
            //foreach (DataRow row in scores.Rows)
            //{
            //    Console.Write("Table #: " + row[0] + "; ");
            //    Console.Write("Home Team #: " + row[1] + "; ");
            //    Console.Write("Away Team #: " + row[2] + "; ");
            //    Console.Write("Home Team Score: " + row[3] + "; ");
            //    Console.WriteLine("Away Team Score: " + row[4] + "; ");
            //    Console.WriteLine();
            //}
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
                    dataElements = SplitCSVDoubleQuotes(recordData, ",");

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

        /// <summary>
        /// Method split the line with specified delimeter
        /// </summary>
        /// <param name="line">line to split</param>
        /// <param name="delimiter">delimeter character</param>
        /// <returns></returns>
        public static string[] SplitCSVDoubleQuotes(string line, string delimiter)
        {
            string inputLine = line;
            if (inputLine.IndexOf("\"") < 0)
                return inputLine.Split(Convert.ToChar(delimiter));

            List<string> values = new List<string>();
            int quoteIndex, valueLength;

            bool addEmptyString = false;

            while (inputLine.Length > 0)
            {
                quoteIndex = inputLine.IndexOf("\"");
                if (quoteIndex == 0)
                {
                    inputLine = inputLine.Substring(1);
                    valueLength = inputLine.IndexOf("\"", 0);
                }
                else
                {
                    valueLength = inputLine.IndexOf(delimiter);
                }

                if (valueLength < 0)
                    valueLength = inputLine.Length;

                string valueToAdd = inputLine.Substring(0, valueLength);

                inputLine = inputLine.Substring(valueLength).Trim();

                if ((inputLine.Length > 0) && (inputLine.Substring(0, 1) == "\"")) inputLine = inputLine.Substring(1).Trim();

                if (inputLine.IndexOf(delimiter) == 0)
                {
                    inputLine = inputLine.Substring(inputLine.IndexOf(delimiter) + 1).Trim();
                    if (inputLine.Length <= 1) addEmptyString = true;
                }
                else if (inputLine.IndexOf(delimiter) > 0)
                {
                    valueToAdd = String.Format("{0}\"{1}", valueToAdd, inputLine.Substring(0, inputLine.IndexOf(delimiter)));
                    inputLine = inputLine.Substring(inputLine.IndexOf(delimiter) + 1).Trim();
                }
                else
                {
                    string trimQuotes = inputLine.Trim(Convert.ToChar("\"")).Trim();
                    if (trimQuotes != String.Empty) valueToAdd = String.Format("{0}\"{1}", valueToAdd, trimQuotes);
                    inputLine = String.Empty;
                }

                values.Add(valueToAdd);
            }

            if (addEmptyString) values.Add(String.Empty);


            return values.ToArray();

        }
    }
}
