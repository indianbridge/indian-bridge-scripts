using System;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using IndianBridge.Common;
using System.ComponentModel;

namespace IndianBridge.ResultsManager
{
    public  class PairsSummaryToDatabase
    {
        private PairsEventInformation m_eventInformation;
        private PairsDatabaseParameters m_databaseParameters;
        private BackgroundWorker m_worker;
        private bool m_runningInBackground = false;

        public PairsDatabaseParameters getDatabaseParameters() { return m_databaseParameters; }

        public PairsSummaryToDatabase(PairsEventInformation eventInformation)
        {
            setParameters(eventInformation);
        }

        public void setParameters(PairsEventInformation eventInformation)
        {
            m_eventInformation = eventInformation;
        }

        private  void printMessage(String message) { Trace.WriteLine(message); }

        private void reportProgress(int progress,string status)
        {
            if (m_runningInBackground)
            {
                m_worker.ReportProgress(progress, status);
            }
        }

        public void loadSummaryIntoDatabaseInBackground(object sender, DoWorkEventArgs e)
        {
            m_runningInBackground = true;
            m_worker = sender as BackgroundWorker;
            loadSummaryIntoDatabaseInternal();
        }

        public void loadSummaryIntoDatabase()
        {
            m_runningInBackground = false;
            m_worker = null;
            loadSummaryIntoDatabaseInternal();
        }

        private void loadSummaryIntoDatabaseInternal()
        {
            m_databaseParameters = PairsGeneral.createDefaultDatabaseParameters();
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(m_eventInformation.databaseFileName));
            System.IO.File.Delete(m_eventInformation.databaseFileName);
            String sourceFileName = System.IO.Path.Combine(Globals.m_rootDirectory, "Databases", "PairsScoreDatabaseTemplate.mdb");
            System.IO.File.Copy(sourceFileName, m_eventInformation.databaseFileName);
            PairsGeneral.loadPairsDatabaseInformation(m_eventInformation.databaseFileName, out m_databaseParameters);
            printMessage("Updating Event Information...");
            reportProgress(0,"Reading Event Information");
            DataTable eventInfoTable = m_databaseParameters.m_ds.Tables["Event_Information"];
            eventInfoTable.Clear();
            DataRow dRow = eventInfoTable.NewRow();
            dRow["ID"] = 1;
            dRow["ACBL_Summary"] = m_eventInformation.isACBLSummary;
            dRow["Has_Direction_Field"] = m_eventInformation.hasDirectionField;
            dRow["Scoring_Type"] = m_eventInformation.isIMP ? "IMP" : "MP";
            dRow["Event_Name"] = m_eventInformation.eventName;
            dRow["Event_Date"] = m_eventInformation.eventDate.ToString();
            eventInfoTable.Rows.Add(dRow);
            printMessage("Uploading Event Information Table to Database...");
            m_databaseParameters.m_daEventInformation.Update(m_databaseParameters.m_ds, "Event_Information");
            reportProgress(10, "Processing Summaries");
            printMessage("Processing Summaries...");
            String[] lines = m_eventInformation.rawText.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
            {
                lines = m_eventInformation.rawText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            int lineNumber = getNextSummaryLine_(lines, 0);
            while (lineNumber < lines.Length && lineNumber != -1)
            {
                lineNumber = processOneSummary_(lines, lineNumber, false);
                lineNumber = getNextSummaryLine_(lines, lineNumber);
            }
            lineNumber = getNextSummaryLine_(lines, 0);
            while (lineNumber < lines.Length && lineNumber != -1)
            {
                lineNumber = processOneSummary_(lines, lineNumber, true);
                lineNumber = getNextSummaryLine_(lines, lineNumber);
            }

            reportProgress(50, "Calculating Scores");
            printMessage("Calculating Ranks...");
            // Combined Ranking
            doRanking_();
            // Section Ranking
            if (m_eventInformation.hasDirectionField) doRanking_(PairsGeneral.SINGLE_SECTION_NAME, "Session_Rank");
            else
            {
                doRanking_(PairsGeneral.NORTH_SOUTH_SECTION_NAME, "Session_Rank");
                doRanking_(PairsGeneral.EAST_WEST_SECTION_NAME, "Session_Rank");
            }
            reportProgress(75, "Saving to database");
            printMessage("Uploading Pair Information Table to Database...");
            m_databaseParameters.m_daPairInformation.Update(m_databaseParameters.m_ds, "Pair_Information");
            printMessage("Uploading Pair Wise Scores Table to Database...");
            m_databaseParameters.m_daPairWiseScores.Update(m_databaseParameters.m_ds, "Pair_Wise_Scores");
            printMessage("Uploading Board Wise Scores Table to Database...");
            m_databaseParameters.m_daBoardWiseScores.Update(m_databaseParameters.m_ds, "Board_Wise_Scores");
            printMessage("Finished Creating Database.");
        }


        private  int getMatchingLine_(String[] lines, int lineNumber, String pattern)
        {
            while (lineNumber < lines.Length)
            {
                Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
                if (re.IsMatch(lines[lineNumber])) return lineNumber;
                lineNumber++;
            }
            return -1;
        }

        private  int getNextSummaryLine_(String[] lines, int lineNumber)
        {
            return getMatchingLine_(lines, lineNumber, "summary");
        }

        private  int processOneSummary_(String[] lines, int lineNumber, bool update = false)
        {
            Regex re = new Regex("session", RegexOptions.IgnoreCase);
            String sectionName = "";
            int pairNumber = -1;
            printMessage("Processing Pair : " + lines[lineNumber]);
            lineNumber = getPairInformation_(lines, lineNumber, out sectionName, out pairNumber, update);
            if (pairNumber != -1)
            {
                lineNumber = getMatchingLine_(lines, lineNumber, "BRD\\s*DLR\\s*VUL");
                String text = "";
                if (lineNumber != -1)
                {
                    lineNumber++;
                    text = lines[lineNumber].Trim();
                }
                while (lineNumber != -1 && lineNumber < lines.Length && !re.IsMatch(text))
                {
                    processOneScoreLine_(text, sectionName, pairNumber, update);
                    lineNumber++;
                    if (lineNumber < lines.Length) text = lines[lineNumber].Trim();
                    else text = "Session";
                }
                processSessionLine_(lines, lineNumber, sectionName, pairNumber);
            }
            return lineNumber;
        }

        private  int getPairInformation_(String[] lines, int lineNumber, out String sectionName, out int pairNumber, bool update = false)
        {
            sectionName = "";
            pairNumber = -1;
            String text = lines[lineNumber++].Trim();
            String[] tokens = text.Split(new string[] { "Summary for Pair " }, StringSplitOptions.None);
            if (tokens.Length > 1)
            {
                String[] newTokens = tokens[1].Split(' ');
                sectionName = (Utilities.containsPattern_(text, PairsGeneral.NORTH_SOUTH_SECTION_NAME) ? PairsGeneral.NORTH_SOUTH_SECTION_NAME : (Utilities.containsPattern_(text, PairsGeneral.EAST_WEST_SECTION_NAME) ? PairsGeneral.EAST_WEST_SECTION_NAME : PairsGeneral.SINGLE_SECTION_NAME));
                pairNumber = newTokens.Length > 1 ? Convert.ToInt16(newTokens[1]) : -1;
                String pairNames = lines[lineNumber++].Trim();
                if (pairNumber != -1)
                {
                    printMessage((update ? "Updating" : "Adding") + " pair information - " + sectionName + ", " + pairNumber + ", " + pairNames);
                    if (!update) addPairInformationRow(sectionName, pairNumber, pairNames);
                }
            }

            return lineNumber;
        }

        private  void processOneScoreLine_(String text, string sectionName, int pairNumber, bool update = false)
        {
            String[] tokens = text.Split(' ');
            int numItems = 6;
            numItems += (m_eventInformation.isIMP ? 1 : 0);
            numItems += (m_eventInformation.hasDirectionField ? 1 : 0);
            int count = 0;
            while (count + numItems <= tokens.Length)
            {
                count = readOneScoreRecord_(tokens, count, sectionName, pairNumber, update);
            }
        }

        private  int readOneScoreRecord_(String[] tokens, int i, string sectionName, int pairNumber, bool update = false)
        {
            int boardNumber = 0;
            boardNumber = Convert.ToInt16(tokens[i++]);
            String dealer = tokens[i++];
            String vul = tokens[i++];
            String direction = m_eventInformation.hasDirectionField ? tokens[i++] : "";
            int opponent = Convert.ToInt16(tokens[i++]);
            String result = tokens[i++];
            String score = tokens[i++];
            string datum = "";
            if (score.Length > 5)
            {
                string temp = score;
                int splitPosition = score.Length - 5;
                score = temp.Substring(0, splitPosition);
                datum = temp.Substring(splitPosition);
            }
            else datum = m_eventInformation.isIMP ? tokens[i++] : "";
            addBoardWiseScoresRow(boardNumber, dealer, vul, sectionName, pairNumber, direction, opponent, result, score, datum, update);
            addPairWiseScoresRow(sectionName, pairNumber, direction, boardNumber, opponent, result, score, datum, update);
            return i;
        }

        private  void processSessionLine_(String[] lines, int lineNumber, String sectionName, int pairNumber)
        {
            String text = lines[lineNumber++].Trim();
            String[] tokens = text.Split(' ');

            double totalScore = 0;
            try
            {
                totalScore = (tokens.Length > 2 ? Convert.ToDouble(tokens[2]) : 0);
            }
            catch (System.FormatException)
            {
                totalScore = 0;
            }
            double percentage = 0;
            try
            {
                percentage = (tokens.Length > 4 ? Convert.ToDouble(tokens[4]) : 0);
            }
            catch (System.FormatException)
            {
                percentage = 0;
            }
            int sessionRank = 0;
            try
            {
                sessionRank = (tokens.Length > 7 ? Convert.ToInt16(tokens[7]) : 0);
            }
            catch (System.FormatException)
            {
                sessionRank = 0;
            }
            text = lines[lineNumber].Trim();
            tokens = text.Split(' ');
            int eventRank = 0;
            if (tokens[0].Equals("Event", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    eventRank = (tokens.Length > 2 ? Convert.ToInt16(tokens[2]) : 0);
                }
                catch (System.FormatException)
                {
                    eventRank = 0;
                }
            }
            printMessage("Processing Session Line with Total Score = " + totalScore + " and Percentage = " + percentage + " and Session Rank = " + sessionRank + ", and Event Rank = " + eventRank);
            Object[] keys = new Object[2];
            keys[0] = sectionName;
            keys[1] = pairNumber;
            DataRow dRow = m_databaseParameters.m_ds.Tables["Pair_Information"].Rows.Find(keys);
            if (dRow != null)
            {
                dRow["Total_Score"] = totalScore;
                dRow["Percentage"] = percentage;
                dRow["Session_Rank"] = sessionRank;
                dRow["Event_Rank"] = eventRank;
            }

        }
        private  void addBoardWiseScoresRow(int boardNumber, String dealer, String vul,
            String sectionName, int pairNumber, String direction, int opponent,
            String result, String score, String datum, bool update = false)
        {
            if (!update)
            {
                Object[] keys = new Object[3];
                keys[0] = boardNumber;
                keys[1] = sectionName;
                keys[2] = pairNumber;
                DataRow dRow = m_databaseParameters.m_ds.Tables["Board_Wise_Scores"].Rows.Find(keys);
                bool addFlag = false;
                if (dRow == null)
                {
                    addFlag = true;
                    dRow = m_databaseParameters.m_ds.Tables["Board_Wise_Scores"].NewRow();
                }
                dRow["Section_Name"] = sectionName;
                dRow["Pair_Number"] = pairNumber;
                dRow["Direction"] = direction;
                dRow["Board_Number"] = boardNumber;
                dRow["Opponent"] = opponent;
                dRow["Result"] = result;
                dRow["Score"] = score;
                dRow["Datum"] = datum;
                dRow["Dealer"] = dealer;
                dRow["Vulnerability"] = vul;
                if (addFlag) m_databaseParameters.m_ds.Tables["Board_Wise_Scores"].Rows.Add(dRow);
            }
            else
            {
                Object[] keys = new Object[3];
                keys[0] = boardNumber;
                keys[1] = PairsGeneral.findOtherSectionName(sectionName);
                keys[2] = opponent;
                DataRow dRow = m_databaseParameters.m_ds.Tables["Board_Wise_Scores"].Rows.Find(keys);
                if (dRow == null) return;
                dRow["Opponent_Score"] = score;
                dRow["Opponent_Result"] = result;
            }
        }

        private  void addPairWiseScoresRow(String sectionName, int pairNumber, String direction, int boardNumber,
            int opponent, String result, String score, String datum, bool update = false)
        {
            if (!update)
            {
                Object[] keys = new Object[3];
                keys[0] = sectionName;
                keys[1] = pairNumber;
                keys[2] = boardNumber;
                DataRow dRow = m_databaseParameters.m_ds.Tables["Pair_Wise_Scores"].Rows.Find(keys);
                bool addFlag = false;
                if (dRow == null)
                {
                    addFlag = true;
                    dRow = m_databaseParameters.m_ds.Tables["Pair_Wise_Scores"].NewRow();
                }
                dRow["Section_Name"] = sectionName;
                dRow["Pair_Number"] = pairNumber;
                dRow["Direction"] = direction;
                dRow["Board_Number"] = boardNumber;
                dRow["Opponent"] = opponent;
                dRow["Result"] = result;
                dRow["Score"] = score;
                dRow["Datum"] = datum;
                if (addFlag) m_databaseParameters.m_ds.Tables["Pair_Wise_Scores"].Rows.Add(dRow);
            }
            else
            {
                Object[] keys = new Object[3];
                keys[0] = PairsGeneral.findOtherSectionName(sectionName);
                keys[1] = opponent;
                keys[2] = boardNumber;
                DataRow dRow = m_databaseParameters.m_ds.Tables["Pair_Wise_Scores"].Rows.Find(keys);
                if (dRow == null) return;
                dRow["Opponent_Score"] = score;
                dRow["Opponent_Result"] = result;
            }
        }

        private  void addPairInformationRow(String sectionName, int pairNumber, String pairNames)
        {
            DataRow dRow = m_databaseParameters.m_ds.Tables["Pair_Information"].NewRow();
            dRow["Section_Name"] = sectionName;
            dRow["Pair_Number"] = pairNumber;
            dRow["Pair_Names"] = pairNames;
            dRow["Total_Score"] = 0;
            dRow["Percentage"] = 0;
            m_databaseParameters.m_ds.Tables["Pair_Information"].Rows.Add(dRow);
        }

        private  void doRanking_(String sectionName = null, String columnName = "Event_Rank")
        {
            DataTable table = m_databaseParameters.m_ds.Tables["Pair_Information"];
            String filterExpression = sectionName == null ? "" : "Section_Name='" + sectionName + "'";
            String sortCriteria = (m_eventInformation.isIMP ? "Total_Score DESC" : "Percentage DESC");
            DataRow[] foundRows = table.Select(filterExpression, sortCriteria);
            int rank = 1;
            double previousValue = 0;
            for (int i = 0; i < foundRows.Length; ++i)
            {
                DataRow dRow = foundRows[i];
                double currentValue = (m_eventInformation.isIMP ? Convert.ToDouble(dRow["Total_Score"]) : Convert.ToDouble(dRow["Percentage"]));
                if (i > 0 && currentValue != previousValue) rank++;
                previousValue = currentValue;
                dRow[columnName] = rank;
            }
        }
    }
}

