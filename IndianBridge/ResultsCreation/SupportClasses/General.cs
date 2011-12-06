using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using IndianBridge.Common;

namespace IndianBridge.ResultsCreation
{
    public struct EventInformation
    {
        public String rawText;
        public bool isACBLSummary;
        public bool isIMP;
        public bool hasDirectionField;
        public String eventName;
        public DateTime eventDate;
        public String databaseFileName;
        public String webpagesDirectory;
    }

    public struct DatabaseParameters
    {
        public DataSet m_ds;
        public System.Data.OleDb.OleDbDataAdapter m_daEventInformation;
        public OleDbCommandBuilder m_cbEventInformation;
        public System.Data.OleDb.OleDbDataAdapter m_daPairInformation;
        public OleDbCommandBuilder m_cbPairInformation;
        public System.Data.OleDb.OleDbDataAdapter m_daPairWiseScores;
        public OleDbCommandBuilder m_cbPairWiseScores;
        public System.Data.OleDb.OleDbDataAdapter m_daBoardWiseScores;
        public OleDbCommandBuilder m_cbBoardWiseScores;
    }
    public class General
    {
        public static EventInformation m_eventInformation;
        public static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public static String makeIdentifier_(String variableName)
        {
            Regex re = new Regex(@"\W");
            return re.Replace(variableName, "-");
        }
        public static String constructDatabaseFileName(String rootFolder, String eventName, DateTime eventDate)
        {
            return Path.Combine(rootFolder, "Databases",eventDate.ToString("yyyy-MM-dd") + "_" + makeIdentifier_(eventName) + ".mdb");
        }
        public static String constructWebpagesDirectory(String rootFolder, String eventName, DateTime eventDate)
        {
            return Path.Combine(rootFolder, "Webpages",eventDate.ToString("yyyy-MM-dd") + "_" + makeIdentifier_(eventName));
        }

        public static EventInformation getEventInformation_(string text)
        {
            m_eventInformation = General.createDefaultEventInformation();
            m_eventInformation.rawText = text;
            String patternString = "Summary\\s*for\\s*Pair";
            if (!Utilities.containsPattern_(text, patternString))
            {
                m_eventInformation.isACBLSummary = false;
                return m_eventInformation;
            }
            else m_eventInformation.isACBLSummary = true;
            patternString = "BRD\\s*DLR\\s*VUL\\s*DIR\\s*VS\\s*RESULT\\s*SCORE\\s*DATUM";
            if (Utilities.containsPattern_(text, patternString)) { setSummaryInfo_(isIMP: true, hasDirectionField: true); return m_eventInformation; }
            patternString = "BRD\\s*DLR\\s*VUL\\s*VS\\s*RESULT\\s*SCORE\\s*DATUM";
            if (Utilities.containsPattern_(text, patternString)) { setSummaryInfo_(isIMP: true, hasDirectionField: false); return m_eventInformation; }
            patternString = "BRD\\s*DLR\\s*VUL\\s*DIR\\s*VS\\s*RESULT\\s*SCORE";
            if (Utilities.containsPattern_(text, patternString)) { setSummaryInfo_(isIMP: false, hasDirectionField: true); return m_eventInformation; }
            patternString = "BRD\\s*DLR\\s*VUL\\s*VS\\s*RESULT\\s*SCORE";
            if (Utilities.containsPattern_(text, patternString)) { setSummaryInfo_(isIMP: false, hasDirectionField: false); return m_eventInformation; }
            return m_eventInformation;
        }



        private static void setSummaryInfo_(bool isIMP, bool hasDirectionField)
        {
            m_eventInformation.isIMP = isIMP;
            m_eventInformation.hasDirectionField = hasDirectionField;
            getEventNameAndDate_(m_eventInformation.rawText);
        }

        private static void getEventNameAndDate_(string text)
        {
            String[] lines = text.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return;
            String eventNameAndDateLine = lines[2];
            String[] tokens = eventNameAndDateLine.Split(',');
            if (tokens.Length < 2) { m_eventInformation.eventName = eventNameAndDateLine; return; }
            String[] subTokens = tokens[0].Split(' ');
            String eventDateString = subTokens[subTokens.Length - 2] + " " + subTokens[subTokens.Length - 1] + ", " + tokens[1];
            if (subTokens.Length > 2)
            {
                m_eventInformation.eventDate = DateTime.Parse(eventDateString);
                m_eventInformation.eventName = "";
                for (int i = 0; i < subTokens.Length - 2; ++i)
                {
                    m_eventInformation.eventName += ((i == 0 ? "" : " ") + subTokens[i]);
                }
            }
            else
            {
                m_eventInformation.eventName = eventNameAndDateLine;
            }
        }

        public static DatabaseParameters createDefaultDatabaseParameters()
        {
            DatabaseParameters dp = new DatabaseParameters();
            dp.m_ds = new DataSet();
            dp.m_daEventInformation = null;
            dp.m_cbEventInformation = null;
            dp.m_daPairInformation = null;
            dp.m_cbPairInformation = null;
            dp.m_daPairWiseScores = null;
            dp.m_cbPairWiseScores = null;
            dp.m_daBoardWiseScores = null;
            dp.m_cbBoardWiseScores = null;
            return dp;
        }



        public static EventInformation createDefaultEventInformation()
        {
            EventInformation eventInformation = new EventInformation();
            eventInformation.rawText = "";
            eventInformation.isACBLSummary = false;
            eventInformation.isIMP = false;
            eventInformation.hasDirectionField = false;
            eventInformation.eventName = "Unknown";
            eventInformation.eventDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            eventInformation.databaseFileName = "";
            return eventInformation;
        }
        public static String NORTH_SOUTH_SECTION_NAME = "North-South";
        public static String EAST_WEST_SECTION_NAME = "East-West";
        public static String SINGLE_SECTION_NAME = "Section-A";

        public static string findOtherSectionName(String sectionName)
        {
            if (sectionName.Equals(NORTH_SOUTH_SECTION_NAME, StringComparison.OrdinalIgnoreCase)) return EAST_WEST_SECTION_NAME;
            if (sectionName.Equals(EAST_WEST_SECTION_NAME, StringComparison.OrdinalIgnoreCase)) return NORTH_SOUTH_SECTION_NAME;
            return sectionName;
        }

        public static void loadEventInformation(DatabaseParameters databaseParameters, String databaseFileName, out EventInformation eventInformation)
        {
            if (databaseParameters.m_ds.Tables["Event_Information"].Rows.Count < 1) throw new Exception("Event Information not found in Database!!!");
            DataRow row = databaseParameters.m_ds.Tables["Event_Information"].Rows[0];
            eventInformation.rawText = "";
            eventInformation.isACBLSummary = (bool)row["ACBL_Summary"];
            eventInformation.hasDirectionField = (bool)row["Has_Direction_Field"];
            eventInformation.isIMP = ((String)row["Scoring_Type"]).Equals("IMP", StringComparison.OrdinalIgnoreCase);
            eventInformation.eventName = "" + row["Event_Name"];
            eventInformation.eventDate = DateTime.Parse("" + row["Event_Date"]);
            eventInformation.databaseFileName = databaseFileName;
            eventInformation.webpagesDirectory = constructWebpagesDirectory(Directory.GetCurrentDirectory(), eventInformation.eventName, eventInformation.eventDate);
        }

        public static void loadDatabaseInformation(String databaseFileName, out DatabaseParameters databaseParameters)
        {
            databaseParameters = createDefaultDatabaseParameters();
#if USINGPROJECTSYSTEM
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+filePath;
#else
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
#endif
            OleDbConnection m_myAccessConn = new OleDbConnection(strAccessConn);
            m_myAccessConn.Open();
            string sql = "SELECT * From Event_Information";
            databaseParameters.m_daEventInformation = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            databaseParameters.m_cbEventInformation = new OleDbCommandBuilder(databaseParameters.m_daEventInformation);
            databaseParameters.m_daEventInformation.Fill(databaseParameters.m_ds, "Event_Information");
            sql = "SELECT * From Pair_Information";
            databaseParameters.m_daPairInformation = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            databaseParameters.m_cbPairInformation = new OleDbCommandBuilder(databaseParameters.m_daPairInformation);
            databaseParameters.m_daPairInformation.Fill(databaseParameters.m_ds, "Pair_Information");
            databaseParameters.m_ds.Tables["Pair_Information"].PrimaryKey = new DataColumn[] { databaseParameters.m_ds.Tables["Pair_Information"].Columns["Section_Name"], databaseParameters.m_ds.Tables["Pair_Information"].Columns["Pair_Number"] };
            sql = "SELECT * From Pair_Wise_Scores";
            databaseParameters.m_daPairWiseScores = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            databaseParameters.m_cbPairWiseScores = new OleDbCommandBuilder(databaseParameters.m_daPairWiseScores);
            databaseParameters.m_daPairWiseScores.Fill(databaseParameters.m_ds, "Pair_Wise_Scores");
            DataTable table = databaseParameters.m_ds.Tables["Pair_Wise_Scores"];
            table.PrimaryKey = new DataColumn[] { table.Columns["Section_Name"], table.Columns["Pair_Number"], table.Columns["Board_Number"] };
            sql = "SELECT * From Board_Wise_Scores";
            databaseParameters.m_daBoardWiseScores = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            databaseParameters.m_cbBoardWiseScores = new OleDbCommandBuilder(databaseParameters.m_daBoardWiseScores);
            databaseParameters.m_daBoardWiseScores.Fill(databaseParameters.m_ds, "Board_Wise_Scores");
            table = databaseParameters.m_ds.Tables["Board_Wise_Scores"];
            table.PrimaryKey = new DataColumn[] { table.Columns["Board_Number"], table.Columns["Section_Name"], table.Columns["Pair_Number"] };
            m_myAccessConn.Close();
            m_myAccessConn = null;
        }
    }
}
