using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndianBridge.Common;
using System.IO;
using System.ComponentModel;

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
            fields.Add(new NiniField("Font_Size", "Number", "1", ""));
            fields.Add(new NiniField("Results_Website", "String", "", ""));
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
            set { 
                fontSize = value;
                NiniUtilities.setDoubleValue(m_niniFileName, Constants.FontSizeFieldName, fontSize, m_autoSave);
            }
        }

        [CategoryAttribute("Results Publish Parameters")]
        public string ResultsWebsite
        {
            get { return resultsWebsite; }
            set { 
                resultsWebsite = value;
                NiniUtilities.setStringValue(m_niniFileName, Constants.ResultsWebsiteFieldName, resultsWebsite, m_autoSave);
            }
        }

    }

    public class SwissTeamScoringParameters
    {
        private string m_niniFileName = "";
        private string m_eventName = "";
        private ScoringTypeValues scoringType = ScoringTypeValues.IMP;
        private TiebreakerMethodValues tiebreakerMethod = TiebreakerMethodValues.TeamNumber;
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
            fields.Add(new NiniField("Scoring_Type", "List", "IMP", "IMP,VP"));
            fields.Add(new NiniField("Tiebreaker_Method", "List", "Quotient", "Quotient,Team_Number"));
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
        }

        [CategoryAttribute("Scoring Parameters")]
        public ScoringTypeValues ScoringType
        {
            get { return scoringType; }
            set { 
                scoringType = value;
                NiniUtilities.setStringValue(m_niniFileName, Constants.ScoringTypeFieldName, scoringType.ToString(), m_autoSave);
            }
        }

        [CategoryAttribute("Scoring Parameters")]
        public TiebreakerMethodValues TiebreakerMethod
        {
            get { return tiebreakerMethod; }
            set { 
                tiebreakerMethod = value;
                NiniUtilities.setStringValue(m_niniFileName, Constants.TiebreakerMethodFieldName, tiebreakerMethod.ToString(), m_autoSave);
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
            fields.Add(new NiniField("Draw_For_Round", "Integer", "0", ""));
            fields.Add(new NiniField("Font_Size", "Number", "1.5", ""));
            fields.Add(new NiniField("Padding_Size", "Number", "5", ""));
            fields.Add(new NiniField("VPs_In_Separate_Column", "Boolean", "True", ""));
            fields.Add(new NiniField("Use_Border", "Boolean", "True", ""));
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
            set { 
                drawForRound = value;
                NiniUtilities.setIntValue(m_niniFileName, Constants.DrawForRoundFieldName, drawForRound, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public double FontSize
        {
            get { return fontSize; }
            set { 
                fontSize = value;
                NiniUtilities.setDoubleValue(m_niniFileName, Constants.FontSizeFieldName, fontSize, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public double PaddingSize
        {
            get { return paddingSize; }
            set { 
                paddingSize = value;
                NiniUtilities.setDoubleValue(m_niniFileName, Constants.PaddingSizeFieldName, paddingSize, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public bool VPsInSeparateColumn
        {
            get { return vpsInSeparateColumn; }
            set { 
                vpsInSeparateColumn = value;
                NiniUtilities.setBooleanValue(m_niniFileName, Constants.VPSInSeparateColumnFieldName, vpsInSeparateColumn, m_autoSave);
            }
        }

        [CategoryAttribute("Print Draw Parameters")]
        public bool UseBorder
        {
            get { return useBorder; }
            set { 
                useBorder = value;
                NiniUtilities.setBooleanValue(m_niniFileName, Constants.UseBorderFieldName, useBorder, m_autoSave);
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

        public void reset()
        {
            drawsCompleted = 0;
            roundsCompleted = 0;
            roundsScored = 0;
        }

        [CategoryAttribute("Swiss Team Event Computed Parameters")]
        public int DrawsCompleted
        {
            get { return drawsCompleted; }
            set { 
                if (value > drawsCompleted) drawsCompleted = value;
                NiniUtilities.setIntValue(m_niniFileName, Constants.DrawsCompletedFieldName, drawsCompleted, m_autoSave);
            }
        }

        [CategoryAttribute("Swiss Team Event Computed Parameters")]
        public int RoundsCompleted
        {
            get { return roundsCompleted; }
            set { 
                if (value > roundsCompleted) roundsCompleted = value;
                NiniUtilities.setIntValue(m_niniFileName, Constants.RoundsCompletedFieldName, roundsCompleted, m_autoSave);
            }
        }

        [CategoryAttribute("Swiss Team Event Computed Parameters")]
        public int RoundsScored
        {
            get { return roundsScored; }
            set { 
                if (value > roundsScored) roundsScored = value;
                NiniUtilities.setIntValue(m_niniFileName, Constants.RoundsScoredFieldName, roundsScored, m_autoSave);
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

        public void create()
        {
            List<NiniField> fields = new List<NiniField>();
            fields.Add(new NiniField(Constants.NumberOfTeamsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfRoundsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfBoardsFieldName, "Integer", "0", ""));
            fields.Add(new NiniField(Constants.NumberOfQualifiersFieldName, "Integer", "0", ""));
            NiniUtilities.createNiniFile(m_niniFileName, fields);
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
                    NiniUtilities.setIntValue(m_niniFileName, Constants.NumberOfTeamsFieldName, numberOfTeams, m_autoSave);
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
                    NiniUtilities.setIntValue(m_niniFileName, Constants.NumberOfRoundsFieldName, numberOfRounds, m_autoSave);
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
                    NiniUtilities.setIntValue(m_niniFileName, Constants.NumberOfBoardsFieldName, numberOfBoardsPerRound, m_autoSave);
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
                    NiniUtilities.setIntValue(m_niniFileName, Constants.NumberOfQualifiersFieldName, numberOfQualifiers, m_autoSave);
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
                    NiniUtilities.setStringValue(m_niniFileName, Constants.TourneyNameFieldName, tourneyName, m_autoSave);
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
                NiniUtilities.setStringValue(m_niniFileName, Constants.ResultsWebsiteFieldName, resultsWebsite, m_autoSave);
            }
        }
    }

}
