using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndianBridge.Common;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace IndianBridgeScorer
{
    public enum ScoringTypeValues
    {
        IMP,
        VP
    };
    public enum TiebreakerMethodValues
    {
        Quotient,
        TeamNumber
    };

    public class KnockoutSessions
    {
        private string m_niniFileName = "";
        private string m_databaseFileName = "";
        private string m_eventName = "";
        private bool m_autoSave = false;
        private int numberOfTeams = 8;
        private int oldNumberOfTeams = 8;
        private Dictionary<int, int> m_oldNumberOfSessions = new Dictionary<int, int>();
        private bool calledFromNumberOfTeams = false;
        private bool calledFromNumberOfRounds = false;
        private PropertyGrid m_grid = null;

        [CategoryAttribute("Knockout Parameters"), DescriptionAttribute("Number of Teams in the Knockout. Number of Rounds will be updated automatically")]
        public int NumberOfTeams
        {
            get { return numberOfTeams; }
            set
            {
                calledFromNumberOfTeams = true;
                double rounds = Math.Log(value, 2);
                if (rounds % 1 != 0)
                {
                    Utilities.showErrorMessage("Number of Teams has to be a power of 2 like, 2,4,8,16 etc. Other values cannot be handled by software.");
                    return;
                }
                numberOfTeams = value;
                NiniUtilities.setValue(m_niniFileName, Constants.NumberOfTeamsFieldName, numberOfTeams, m_autoSave);
                if (!calledFromNumberOfRounds)
                {
                    NumberOfRounds = Convert.ToInt32(rounds);
                    update();
                }
                calledFromNumberOfTeams = false;
                if (m_grid != null) m_grid.Refresh();
            }
        }
        private int numberOfRounds = 3;
        private int oldNumberOfRounds = 3;

        [CategoryAttribute("Knockout Parameters"), DescriptionAttribute("Number of Rounds in the Knockout. Number of Teams will be updated automatically")]
        public int NumberOfRounds
        {
            get { return numberOfRounds; }
            set
            {
                calledFromNumberOfRounds = true;
                numberOfRounds = value;
                NiniUtilities.setValue(m_niniFileName, Constants.NumberOfRoundsFieldName, numberOfRounds, m_autoSave);
                if (!calledFromNumberOfTeams)
                {
                    NumberOfTeams = Convert.ToInt32(Math.Pow(2, value));
                    update();
                }
                calledFromNumberOfRounds = false;
                if (m_grid != null) m_grid.Refresh();
            }
        }
        public KnockoutSessions(string eventName, string niniFileName, string databaseFileName, bool autoSave = false) :
            this(null, eventName, niniFileName, databaseFileName, autoSave) { }

        public KnockoutSessions(PropertyGrid grid, string eventName, string niniFileName, string databaseFileName, bool autoSave = false)
        {
            m_grid = grid;
            if (m_grid != null) m_grid.SelectedObject = this;
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            m_databaseFileName = databaseFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            if (!File.Exists(m_databaseFileName))
            {
                AccessDatabaseUtilities.createDatabase(m_databaseFileName);
                createTeamsTable();
                createSessionsTable();
            }
            load();
        }

        public void updateSessions()
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            foreach (DataRow dRow in table.Rows)
            {
                List<string> removeColumns = null;
                int roundNumber = (int)dRow["Round_Number"];
                int numberOfSessions = (int)dRow["Number_Of_Sessions"];
                if (numberOfSessions > m_oldNumberOfSessions[roundNumber])
                {
                    List<DatabaseField> fields = new List<DatabaseField>();
                    for (int i = m_oldNumberOfSessions[roundNumber] + 1; i <= numberOfSessions; ++i)
                    {
                        fields.Add(new DatabaseField("Session_" + i + "_Score", "NUMBER"));
                    }
                    AccessDatabaseUtilities.addColumn(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber, fields);
                }
                else if (m_oldNumberOfSessions[roundNumber] > numberOfSessions)
                {
                    removeColumns = new List<string>();
                    List<DatabaseField> fields = new List<DatabaseField>();
                    for (int i = numberOfSessions + 1; i <= m_oldNumberOfSessions[roundNumber]; ++i)
                    {
                        fields.Add(new DatabaseField("Session_" + i + "_Score", "NUMBER"));
                        removeColumns.Add("Session_" + i + "_Score");
                    }
                    AccessDatabaseUtilities.dropColumn(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber, fields);
                }
                m_oldNumberOfSessions[roundNumber] = numberOfSessions;
                AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber, "", removeColumns);
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutSessions);
        }

        public void update()
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            DataTable teamsTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutTeams);
            if (numberOfRounds > oldNumberOfRounds)
            {
                for (int i = oldNumberOfRounds + 1; i <= numberOfRounds; ++i)
                {
                    DataRow dRow = table.NewRow();
                    string sessionName = (Constants.KnockoutSessionNames.Length >= i ? Constants.KnockoutSessionNames[i - 1] : "Round_of_" + Math.Pow(2, i));
                    dRow["Round_Number"] = i;
                    dRow["Round_Name"] = sessionName;
                    dRow["Number_Of_Sessions"] = 3;
                    table.Rows.Add(dRow);
                    createRoundScoresTable(i);
                }
                AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutSessions);
                for (int i = oldNumberOfTeams + 1; i <= numberOfTeams; ++i)
                {
                    DataRow dRow = teamsTable.NewRow();
                    string sessionName = (Constants.KnockoutSessionNames.Length >= i ? Constants.KnockoutSessionNames[i - 1] : "Round_of_" + Math.Pow(2, i));
                    dRow["Team_Number"] = i;
                    dRow["Team_Name"] = "Team " + i;
                    teamsTable.Rows.Add(dRow);
                }
                AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutTeams);
            }
            else if (numberOfRounds < oldNumberOfRounds)
            {
                for (int i = numberOfRounds + 1; i <= oldNumberOfRounds; ++i)
                {
                    table.Rows.Find(i).Delete();
                    AccessDatabaseUtilities.dropTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + i);
                    m_oldNumberOfSessions.Remove(i);
                }
                AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutSessions);
                for (int i = numberOfTeams + 1; i <= oldNumberOfTeams; ++i)
                {
                    teamsTable.Rows.Find(i).Delete();
                }
                AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutTeams);
            }
            oldNumberOfRounds = numberOfRounds;
            oldNumberOfTeams = numberOfTeams;
        }


        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.NumberOfTeamsFieldName, "Integer", "8", ""));
            fields.Add(new NiniField(Constants.NumberOfRoundsFieldName, "Integer", "3", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        private void createSessionsTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Round_Number", "INTEGER"));
            fields.Add(new DatabaseField("Round_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Number_Of_Sessions", "INTEGER"));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Round_Number");
            DataTable table = AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.KnockoutSessions, fields, primaryKeys);
            for (int i = 1; i <= numberOfRounds; i++)
            {
                DataRow dRow = table.NewRow();
                string sessionName = (Constants.KnockoutSessionNames.Length >= i ? Constants.KnockoutSessionNames[i - 1] : "Round_of_" + Convert.ToInt32(Math.Pow(2, i)));
                dRow["Round_Number"] = i;
                dRow["Round_Name"] = sessionName;
                dRow["Number_Of_Sessions"] = 3;
                table.Rows.Add(dRow);
                createRoundScoresTable(i);
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutSessions);
        }

        private void createTeamsTable()
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Member_Names", "TEXT", 255));
            fields.Add(new DatabaseField("Original_Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Original_Event_Name", "TEXT", 255));
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Team_Number");
            DataTable table = AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.KnockoutTeams, fields, primaryKeys);
            for (int i = 1; i <= numberOfTeams; i++)
            {
                DataRow dRow = table.NewRow();
                string sessionName = (Constants.KnockoutSessionNames.Length >= i ? Constants.KnockoutSessionNames[i - 1] : "Round_of_" + Math.Pow(2, i));
                dRow["Team_Number"] = i;
                dRow["Team_Name"] = "Team " + i;
                table.Rows.Add(dRow);
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutTeams);
        }

        public void initializeMatches()
        {
            DataTable teamsTable = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutTeams);
            for (int i = 1; i <= numberOfRounds; ++i)
            {
                DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + i);
                int totalTeams = Convert.ToInt32(Math.Pow(2, i));
                int numberOfMatches = totalTeams / 2;
                for (int j = 1; j <= numberOfMatches; ++j)
                {
                    DataRow team1Row = teamsTable.Rows[j - 1];
                    DataRow team2Row = teamsTable.Rows[totalTeams - j];
                    DataRow[] dRows = table.Select("Match_Number = " + j);
                    Debug.Assert(dRows.Length == 2);
                    dRows[0]["Team_Number"] = AccessDatabaseUtilities.getIntValue(team1Row, "Team_Number");
                    dRows[0]["Team_Name"] = AccessDatabaseUtilities.getStringValue(team1Row, "Team_Name");
                    dRows[1]["Team_Number"] = AccessDatabaseUtilities.getIntValue(team2Row, "Team_Number");
                    dRows[1]["Team_Name"] = AccessDatabaseUtilities.getStringValue(team2Row, "Team_Name");
                }
                AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + i);
            }
        }

        private void createRoundScoresTable(int roundNumber)
        {
            List<DatabaseField> fields = new List<DatabaseField>();
            fields.Add(new DatabaseField("Match_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Number", "INTEGER"));
            fields.Add(new DatabaseField("Team_Name", "TEXT", 255));
            fields.Add(new DatabaseField("Carryover", "NUMBER"));
            fields.Add(new DatabaseField("Total", "NUMBER"));
            DataTable table = AccessDatabaseUtilities.getDataTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            DataRow dRow = table.Rows.Find(roundNumber);
            int numberOfSessions = (int)dRow["Number_Of_Sessions"];
            m_oldNumberOfSessions[roundNumber] = numberOfSessions;
            for (int i = 1; i <= numberOfSessions; ++i)
            {
                fields.Add(new DatabaseField("Session_" + i + "_Score", "NUMBER"));
            }
            List<string> primaryKeys = new List<string>();
            primaryKeys.Add("Match_Number");
            primaryKeys.Add("Team_Number");
            DataTable scoresTable = AccessDatabaseUtilities.createTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber, fields, primaryKeys);
            int numberOfMatches = Convert.ToInt32(Math.Pow(2, roundNumber - 1));
            for (int i = 1; i <= numberOfMatches; ++i)
            {
                DataRow dScoresRow = scoresTable.NewRow();
                dScoresRow["Match_Number"] = i;
                int teamNumber = i;
                dScoresRow["Team_Number"] = i;
                dScoresRow["Team_Name"] = LocalUtilities.getTeamName(m_databaseFileName, Constants.TableName.KnockoutTeams, i);
                scoresTable.Rows.Add(dScoresRow);
                dScoresRow = scoresTable.NewRow();
                dScoresRow["Match_Number"] = i;
                teamNumber = 2 * numberOfMatches - (i - 1);
                dScoresRow["Team_Number"] = teamNumber;
                dScoresRow["Team_Name"] = LocalUtilities.getTeamName(m_databaseFileName, Constants.TableName.KnockoutTeams, teamNumber);
                scoresTable.Rows.Add(dScoresRow);
            }
            AccessDatabaseUtilities.saveTableToDatabase(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + roundNumber);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            numberOfTeams = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfTeamsFieldName);
            numberOfRounds = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfRoundsFieldName);
            DataTable table = AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.KnockoutSessions);
            foreach (DataRow dRow in table.Rows)
            {
                int roundNumber = (int)dRow["Round_Number"];
                int numberOfSessions = (int)dRow["Number_Of_Sessions"];
                m_oldNumberOfSessions[roundNumber] = numberOfSessions;
            }
            AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.KnockoutTeams);
            for (int i = 1; i <= numberOfRounds; i++)
                AccessDatabaseUtilities.loadDatabaseToTable(m_databaseFileName, Constants.TableName.KnockoutScores + "_" + i);
        }

    }

    public class ResultsPublishParameters
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private double fontSize = 1.5;
        private string resultsWebsite = "";
        private bool m_autoSave = false;
        public ResultsPublishParameters(string eventName, string niniFileName, bool autoSave = false)
        {
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.FontSizeFieldName, "Number", "1", ""));
            fields.Add(new NiniField(Constants.ResultsWebsiteFieldName, "String", "", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            fontSize = NiniUtilities.getDoubleValue(m_niniFileName, Constants.FontSizeFieldName);
            resultsWebsite = NiniUtilities.getStringValue(m_niniFileName, Constants.ResultsWebsiteFieldName);
        }


        [CategoryAttribute("Results Publish Parameters")]
        public double FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                NiniUtilities.setValue(m_niniFileName, Constants.FontSizeFieldName, fontSize, m_autoSave);
            }
        }

        [CategoryAttribute("Results Publish Parameters")]
        public string ResultsWebsite
        {
            get { return resultsWebsite; }
            set
            {
                resultsWebsite = value;
                NiniUtilities.setValue(m_niniFileName, Constants.ResultsWebsiteFieldName, resultsWebsite, m_autoSave);
            }
        }

    }

    public class SwissTeamScoringParameters
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private ScoringTypeValues scoringType = ScoringTypeValues.IMP;
        private TiebreakerMethodValues tiebreakerMethod = TiebreakerMethodValues.TeamNumber;
        private int byeScore = 18;
        private bool m_autoSave = false;

        public SwissTeamScoringParameters(string eventName, string niniFileName, bool autoSave = false)
        {
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.ScoringTypeFieldName, "List", "IMP", "IMP,VP"));
            fields.Add(new NiniField(Constants.TiebreakerMethodFieldName, "List", "Quotient", "Quotient,Team_Number"));
            fields.Add(new NiniField(Constants.ByeScoreFieldName, "Number", "18", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            scoringType = (ScoringTypeValues)Enum.Parse(typeof(ScoringTypeValues), NiniUtilities.getStringValue(m_niniFileName, Constants.ScoringTypeFieldName), true);
            tiebreakerMethod = (TiebreakerMethodValues)Enum.Parse(typeof(TiebreakerMethodValues), NiniUtilities.getStringValue(m_niniFileName, Constants.TiebreakerMethodFieldName), true);
            byeScore = NiniUtilities.getIntValue(m_niniFileName, Constants.ByeScoreFieldName, 18);
        }

        [CategoryAttribute("Scoring Parameters")]
        public ScoringTypeValues ScoringType
        {
            get { return scoringType; }
            set
            {
                scoringType = value;
                NiniUtilities.setValue(m_niniFileName, Constants.ScoringTypeFieldName, scoringType.ToString(), m_autoSave);
            }
        }

        [CategoryAttribute("Scoring Parameters")]
        public TiebreakerMethodValues TiebreakerMethod
        {
            get { return tiebreakerMethod; }
            set
            {
                tiebreakerMethod = value;
                NiniUtilities.setValue(m_niniFileName, Constants.TiebreakerMethodFieldName, tiebreakerMethod.ToString(), m_autoSave);
            }
        }
        [CategoryAttribute("Scoring Parameters")]
        public int ByeScore
        {
            get { return byeScore; }
            set
            {
                byeScore = value;
            }
        }
    }



    public class SwissTeamPrintDrawParameters
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private int drawForRound = 0;
        private double fontSize = 1.5;
        private double paddingSize = 5;
        private bool vpsInSeparateColumn = true;
        private bool useBorder = true;
        private bool m_autoSave = false;

        public SwissTeamPrintDrawParameters(string eventName, string niniFileName, bool autoSave = false)
        {
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.DrawForRoundFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.FontSizeFieldName, "Number", "1.5", ""));
            fields.Add(new NiniField(Constants.PaddingSizeFieldName, "Number", "5", ""));
            fields.Add(new NiniField(Constants.VPSInSeparateColumnFieldName, "Boolean", "True", ""));
            fields.Add(new NiniField(Constants.UseBorderFieldName, "Boolean", "True", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            drawForRound = NiniUtilities.getIntValue(m_niniFileName, Constants.DrawForRoundFieldName);
            fontSize = NiniUtilities.getDoubleValue(m_niniFileName, Constants.FontSizeFieldName);
            paddingSize = NiniUtilities.getDoubleValue(m_niniFileName, Constants.PaddingSizeFieldName);
            vpsInSeparateColumn = NiniUtilities.getBooleanValue(m_niniFileName, Constants.VPSInSeparateColumnFieldName);
            useBorder = NiniUtilities.getBooleanValue(m_niniFileName, Constants.UseBorderFieldName);
        }

        [CategoryAttribute("Print Draw Parameters")]
        public int DrawForRound
        {
            get { return drawForRound; }
            set
            {
                drawForRound = value;
                NiniUtilities.setValue(m_niniFileName, Constants.DrawForRoundFieldName, drawForRound, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public double FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                NiniUtilities.setValue(m_niniFileName, Constants.FontSizeFieldName, fontSize, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public double PaddingSize
        {
            get { return paddingSize; }
            set
            {
                paddingSize = value;
                NiniUtilities.setValue(m_niniFileName, Constants.PaddingSizeFieldName, paddingSize, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public bool VPsInSeparateColumn
        {
            get { return vpsInSeparateColumn; }
            set
            {
                vpsInSeparateColumn = value;
                NiniUtilities.setValue(m_niniFileName, Constants.VPSInSeparateColumnFieldName, vpsInSeparateColumn, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public bool UseBorder
        {
            get { return useBorder; }
            set
            {
                useBorder = value;
                NiniUtilities.setValue(m_niniFileName, Constants.UseBorderFieldName, useBorder, m_autoSave);
            }
        }
    }


    public class SwissTeamScoringProgressParameters
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private int drawsCompleted = 0;
        private int roundsCompleted = 0;
        private int roundsScored = 0;
        private bool m_autoSave = false;

        public SwissTeamScoringProgressParameters(string eventName, string niniFileName, bool autoSave = false)
        {
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.DrawsCompletedFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.RoundsCompletedFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.RoundsScoredFieldName, "Integer", "0", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            drawsCompleted = NiniUtilities.getIntValue(m_niniFileName, Constants.DrawsCompletedFieldName);
            roundsCompleted = NiniUtilities.getIntValue(m_niniFileName, Constants.RoundsCompletedFieldName);
            roundsScored = NiniUtilities.getIntValue(m_niniFileName, Constants.RoundsScoredFieldName);
        }

        public void reset(int newNumberOfRounds)
        {
            if (drawsCompleted > newNumberOfRounds) drawsCompleted = newNumberOfRounds;
            if (roundsCompleted > newNumberOfRounds) roundsCompleted = newNumberOfRounds;
            if (roundsScored > newNumberOfRounds) roundsScored = newNumberOfRounds;
        }

        [CategoryAttribute("Swiss Team Event Computed Parameters")]
        public int DrawsCompleted
        {
            get { return drawsCompleted; }
            set
            {
                if (value > drawsCompleted) drawsCompleted = value;
                NiniUtilities.setValue(m_niniFileName, Constants.DrawsCompletedFieldName, drawsCompleted, m_autoSave);
            }
        }

        [CategoryAttribute("Swiss Team Event Computed Parameters")]
        public int RoundsCompleted
        {
            get { return roundsCompleted; }
            set
            {
                if (value > roundsCompleted) roundsCompleted = value;
                NiniUtilities.setValue(m_niniFileName, Constants.RoundsCompletedFieldName, roundsCompleted, m_autoSave);
            }
        }

        [CategoryAttribute("Swiss Team Event Computed Parameters")]
        public int RoundsScored
        {
            get { return roundsScored; }
            set
            {
                if (value > roundsScored) roundsScored = value;
                NiniUtilities.setValue(m_niniFileName, Constants.RoundsScoredFieldName, roundsScored, m_autoSave);
            }
        }
    }

    public class PDEventInfo
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private int numberOfTeams = 0;
        private int numberOfRounds = 0;
        private int numberOfBoardsPerRound = 0;

        private bool m_autoSave = false;

        [BrowsableAttribute(false)]
        public int previousNumberOfTeams = 0;
        [BrowsableAttribute(false)]
        public int previousNumberOfRounds = 0;
        [BrowsableAttribute(false)]
        public int previousNumberOfBoardsPerRound = 0;
        [BrowsableAttribute(false)]
        public int totalNumberOfBoards = 0;
        [BrowsableAttribute(false)]
        public bool hasPhantomTable = false;
        [BrowsableAttribute(false)]
        public int numberOfTables = 0;

        public PDEventInfo(string eventName, string niniFileName, bool autoSave = false)
        {
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public bool isValid()
        {
            return numberOfTeams >= 2 && NumberOfRounds >= 2 && numberOfBoardsPerRound > 0;
        }

        public bool hasChanged()
        {
            return (previousNumberOfTeams != numberOfTeams) ||
                (previousNumberOfRounds != numberOfRounds) ||
                (previousNumberOfBoardsPerRound != numberOfBoardsPerRound);
        }

        public bool wasNonZero()
        {
            return previousNumberOfTeams != 0 || previousNumberOfRounds != 0 || previousNumberOfBoardsPerRound != 0;
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.NumberOfTeamsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfRoundsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfBoardsFieldName, "Integer", "0", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
            previousNumberOfRounds = numberOfRounds;
            previousNumberOfTeams = numberOfTeams;
            previousNumberOfBoardsPerRound = numberOfBoardsPerRound;
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            numberOfTeams = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfTeamsFieldName);
            hasPhantomTable = (numberOfTeams % 2 == 0);
            numberOfTables = numberOfTeams + (hasPhantomTable ? 1 : 0);
            numberOfRounds = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfRoundsFieldName);
            numberOfBoardsPerRound = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfBoardsFieldName);
            previousNumberOfRounds = numberOfRounds;
            previousNumberOfTeams = numberOfTeams;
            previousNumberOfBoardsPerRound = numberOfBoardsPerRound;
            totalNumberOfBoards = numberOfTables * numberOfBoardsPerRound;
        }

        [CategoryAttribute("PD Event Setup Parameters"), DescriptionAttribute("Number of Teams in the Event")]
        public int NumberOfTeams
        {
            get { return numberOfTeams; }
            set
            {
                if (value % 2 == 0)
                {
                    hasPhantomTable = true;
                    Utilities.showWarningessage("A phantom table will be automatically added to make odd number of tables.");
                }
                else hasPhantomTable = false;
                if (value < 2)
                {
                    Utilities.showErrorMessage("Number of Teams (" + value + ") has to be atleast 2!");
                }
                else
                {
                    numberOfTeams = value;
                    numberOfTables = numberOfTeams + (hasPhantomTable ? 1 : 0);
                    totalNumberOfBoards = numberOfTables * numberOfBoardsPerRound;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfTeamsFieldName, numberOfTeams, m_autoSave);
                }
            }
        }

        [CategoryAttribute("PD Event Setup Parameters"), DescriptionAttribute("Number of Rounds in the Event")]
        public int NumberOfRounds
        {
            get { return numberOfRounds; }
            set
            {
                if (value < 2 || value >= numberOfTables)
                {
                    Utilities.showErrorMessage("Number of Rounds (" + value + ") has to be atleast 2 and less than equal to number of tables (" + numberOfTables + ")!");
                }
                else
                {
                    numberOfRounds = value;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfRoundsFieldName, numberOfRounds, m_autoSave);
                }
            }
        }

        [CategoryAttribute("PD Event Setup Parameters"), DescriptionAttribute("Number of Boards to be played in each Round")]
        public int NumberOfBoardsPerRound
        {
            get { return numberOfBoardsPerRound; }
            set
            {
                if (value < 1)
                {
                    Utilities.showErrorMessage("Number of BoardsPerRound (" + value + ") has to be atleast 1!");
                }
                else
                {
                    numberOfBoardsPerRound = value;
                    totalNumberOfBoards = numberOfTables * numberOfBoardsPerRound;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfBoardsFieldName, numberOfBoardsPerRound, m_autoSave);
                }
            }
        }
    }

    public class SwissTeamEventInfo
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private int numberOfTeams = 0;
        private int numberOfRounds = 0;
        private int numberOfBoardsPerRound = 0;
        private int numberOfQualifiers = 0;
        private bool m_autoSave = false;

        [BrowsableAttribute(false)]
        public int previousNumberOfTeams = 0;
        [BrowsableAttribute(false)]
        public int previousNumberOfRounds = 0;

        public SwissTeamEventInfo(string eventName, string niniFileName, bool autoSave = false)
        {
            m_eventName = eventName;
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public bool isValid()
        {
            return numberOfTeams >= 2 && NumberOfRounds >= 2 && numberOfQualifiers >= 0;
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.NumberOfTeamsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfRoundsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfBoardsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfQualifiersFieldName, "Integer", "0", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
            previousNumberOfRounds = numberOfRounds;
            previousNumberOfTeams = numberOfTeams;
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            numberOfTeams = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfTeamsFieldName);
            numberOfRounds = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfRoundsFieldName);
            numberOfBoardsPerRound = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfBoardsFieldName);
            numberOfQualifiers = NiniUtilities.getIntValue(m_niniFileName, Constants.NumberOfQualifiersFieldName);
            previousNumberOfRounds = numberOfRounds;
            previousNumberOfTeams = numberOfTeams;
        }

        [CategoryAttribute("Swiss Team Event Setup Parameters"), DescriptionAttribute("Number of Teams in the Swiss League")]
        public int NumberOfTeams
        {
            get { return numberOfTeams; }
            set
            {
                if (value < 2)
                {
                    Utilities.showErrorMessage("Number of Teams (" + value + ") has to be atleast 2!");
                }
                else
                {
                    numberOfTeams = value;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfTeamsFieldName, numberOfTeams, m_autoSave);
                }
            }
        }

        [CategoryAttribute("Swiss Team Event Setup Parameters"), DescriptionAttribute("Number of Rounds in the Round Robin")]
        public int NumberOfRounds
        {
            get { return numberOfRounds; }
            set
            {
                if (value < 2)
                {
                    Utilities.showErrorMessage("Number of Rounds (" + value + ") has to be atleast 2!");
                }
                else
                {
                    numberOfRounds = value;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfRoundsFieldName, numberOfRounds, m_autoSave);
                }
            }
        }

        [CategoryAttribute("Swiss Team Event Setup Parameters"), DescriptionAttribute("Number of Boards to be played in each Round! This is used to load the appropriate VP Scale to convert IMPS to VPs. Set this to 0 to ignore and enter VPs directly.")]
        public int NumberOfBoardsPerRound
        {
            get { return numberOfBoardsPerRound; }
            set
            {
                if (value < 0)
                {
                    Utilities.showErrorMessage("Number of BoardsPerRound (" + value + ") has to be atleast 0!");
                }
                else
                {
                    numberOfBoardsPerRound = value;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfBoardsFieldName, numberOfBoardsPerRound, m_autoSave);
                }
            }
        }

        [CategoryAttribute("Swiss Team Event Setup Parameters"), DescriptionAttribute("Number of Qualifiers for the next round")]
        public int NumberOfQualifiers
        {
            get { return numberOfQualifiers; }
            set
            {
                if (value < 0 || value > numberOfTeams)
                {
                    Utilities.showErrorMessage("Number of Qualifiers (" + value + ") has to be atleast 0 and less than number of teams (" + numberOfTeams + ")!");
                }
                else
                {
                    numberOfQualifiers = value;
                    NiniUtilities.setValue(m_niniFileName, Constants.NumberOfQualifiersFieldName, numberOfQualifiers, m_autoSave);
                }
            }
        }
    }

    public class TourneyInfo
    {

        private string m_niniFileName;
        private string tourneyName = "";
        private string resultsWebsite = "";
        private bool m_autoSave = false;

        public TourneyInfo(string niniFileName, bool autoSave = false)
        {
            m_autoSave = autoSave;
            m_niniFileName = niniFileName;
            if (!File.Exists(m_niniFileName))
            {
                create();
            }
            load();
        }

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.TourneyNameFieldName, "String", "Unknown"));
            fields.Add(new NiniField(Constants.ResultsWebsiteFieldName, "String", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
        }

        public void save()
        {
            NiniUtilities.saveNiniConfig(m_niniFileName);
        }

        public void load()
        {
            NiniUtilities.loadNiniConfig(m_niniFileName);
            tourneyName = NiniUtilities.getStringValue(m_niniFileName, Constants.TourneyNameFieldName, "Unknown");
            resultsWebsite = NiniUtilities.getStringValue(m_niniFileName, Constants.ResultsWebsiteFieldName, "");
        }

        [CategoryAttribute("Tourney Setup Parameters"), DescriptionAttribute("Name of your tourney. You will not be able to change this later so choose carefully. The tourney name will determine the folder in which all data will be stored.")]
        public string TourneyName
        {
            get { return tourneyName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Utilities.showErrorMessage("Tourney Name cannot be null!");
                }
                else
                {
                    tourneyName = value;
                    NiniUtilities.setValue(m_niniFileName, Constants.TourneyNameFieldName, tourneyName, m_autoSave);
                }
            }
        }

        [CategoryAttribute("Tourney Setup Parameters"), DescriptionAttribute("The website where results should be published. Just specify the root of the results page. A separate sub page will be created for each event (using the name of the event).")]
        public string ResultsWebsite
        {
            get { return resultsWebsite; }
            set
            {
                resultsWebsite = value;
                NiniUtilities.setValue(m_niniFileName, Constants.ResultsWebsiteFieldName, resultsWebsite, m_autoSave);
            }
        }
    }

}
