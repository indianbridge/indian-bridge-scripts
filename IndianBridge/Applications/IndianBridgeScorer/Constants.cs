using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IndianBridge.Common;

namespace IndianBridgeScorer
{
    public static class Constants
    {

        // Path Constants
        public static string RootFolder = Directory.GetCurrentDirectory();
        public static string TourneysFolderName = "Tourneys";
        public static string DatabasesFolderName = "Databases";
        public static string WebpagesFolderName = "Webpages";
        public static string TourneyInformationFileName = "TourneyInfo.ini";
        public static string TourneyEventsFileName = "TourneyEvents.mdb";
        public static string EventInformationFileName = "EventInfo.ini";
        public static string EventScoringProgressParametersFileName = "EventScoringProgressParameters.ini";
        public static string EventScoresFileName = "EventScores.mdb";
        public static string SwissTeamEventPrintDrawParametersFileName = "SwissTeamPrintDrawParameters.ini";
        public static string SwissTeamEventPrintDrawFileName = "DrawForPrinting.html";
        public static string SwissTeamEventScoringParametersFileName = "SwissTeamEventScoringParameters.ini";
        public static string ResultsPublishParametersFileName = "ResultsPublishParameters.ini";
        public static string KnockoutInfoFileName = "KnockoutInfo.ini";
        public static string KnockoutScoresFileName = "KnockoutScores.mdb";

        // Event Type Constants
        public static class EventType {
            public static string TeamsSwissLeague = "TeamsSwissLeague";
            public static string TeamsKnockout = "TeamsKnockout";
            public static string Pairs = "Pairs";
            public static string PD = "PD";
        }


        // Table Name Constants
        public static class TableName
        {
            public static string TourneyEvents = "Tourney_Events";
            public static string EventNames = "Teams";
            public static string EventScores = "Scores";
            public static string EventComputedScores = "ComputedScores";
            public static string VPScale = "VPScales";
            public static string KnockoutSessions = "KnockoutSessions";
            public static string KnockoutTeams = "KnockoutTeams";
            public static string KnockoutScores = "KnockoutScores";
        }


        // Information Constants
        public static string CurrentTourneyFolderName = "";
        public static string CurrentTourneyName = "";
        public static string CurrentTourneyResultsWebsite = "";

        // Computed Path Constants
        // General
        public static string getRootDatabaseFolder() { return Path.Combine(RootFolder, DatabasesFolderName); }
        public static string getRootTourneyInformationFile() { return Path.Combine(getRootDatabaseFolder(), TourneyInformationFileName); }
        public static string getTourneysFolder() { return getFolder(Path.Combine(RootFolder, TourneysFolderName)); }

        // Current Tourney
        public static string getCurrentTourneyFolder() { return getFolder(Path.Combine(getTourneysFolder(), CurrentTourneyFolderName)); }
        public static string getCurrentTourneyDatabasesFolder() { return getFolder(Path.Combine(getCurrentTourneyFolder(),DatabasesFolderName)); }
        public static string getCurrentTourneyWebpagesFolder() { return getFolder(Path.Combine(getCurrentTourneyFolder(),WebpagesFolderName)); }
        public static string getCurrentTourneyInformationFileName() { return Path.Combine(getCurrentTourneyDatabasesFolder(), TourneyInformationFileName); }
        public static string getCurrentTourneyEventsFileName() { return Path.Combine(getCurrentTourneyDatabasesFolder(), TourneyEventsFileName); }

        // Event
        public static string getEventDatabasesFolder(string eventName) { return getFolder(Path.Combine(getCurrentTourneyDatabasesFolder(), Utilities.makeIdentifier_(eventName))); }
        public static string getEventScoresFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), EventScoresFileName); }
        public static string getEventInformationFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), EventInformationFileName); }
        public static string getEventScoringProgressParametersFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), EventScoringProgressParametersFileName); }
        public static string getSwissTeamPrintDrawParametersFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), SwissTeamEventPrintDrawParametersFileName); }
        public static string getSwissTeamPrintDrawFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), SwissTeamEventPrintDrawFileName); }
        public static string getSwissTeamScoringParametersFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), SwissTeamEventScoringParametersFileName); }
        public static string getResultsPublishParametersFileName(string eventName) { return Path.Combine(getEventDatabasesFolder(eventName), ResultsPublishParametersFileName); }
        public static string getEventWebpagesFolder(string eventName) { return getFolder(Path.Combine(getCurrentTourneyWebpagesFolder(), Utilities.makeIdentifier_(eventName))); }
        public static string getEventResultsWebsite(string eventName) { 
            if (string.IsNullOrWhiteSpace(CurrentTourneyResultsWebsite)) return "";
            return CurrentTourneyResultsWebsite+"/"+Utilities.makeIdentifier_(eventName);
        }
        public static string getKnockoutEventInfoFileName(string eventName) { return Path.Combine(getFolder(getEventDatabasesFolder(eventName)), KnockoutInfoFileName); }
        public static string getKnockoutEventScoresFileName(string eventName) { return Path.Combine(getFolder(getEventDatabasesFolder(eventName)), KnockoutScoresFileName); }

        // Field Names
        public static string TourneyNameFieldName = "Tourney_Name";
        public static string ResultsWebsiteFieldName = "Results_Website";
        public static string EventNameFieldName = "Event_Name";
        public static string NumberOfTeamsFieldName = "Number_Of_Teams";
        public static string NumberOfRoundsFieldName = "Number_Of_Rounds";
        public static string NumberOfBoardsFieldName = "Number_Of_Boards";
        public static string NumberOfQualifiersFieldName = "Number_Of_Qualifiers";
        public static string DrawsCompletedFieldName = "Draws_Completed";
        public static string RoundsCompletedFieldName = "Rounds_Completed";
        public static string RoundsScoredFieldName = "Rounds_Scored";
        public static string DrawForRoundFieldName = "Draw_For_Round";
        public static string FontSizeFieldName = "Font_Size";
        public static string PaddingSizeFieldName = "Padding_Size";
        public static string VPSInSeparateColumnFieldName = "VPs_In_Separate_Column";
        public static string UseBorderFieldName = "Use_Border";
        public static string ScoringTypeFieldName = "Scoring_Type";
        public static string TiebreakerMethodFieldName = "Tiebreaker_Method";
        
        // Helpers
        public static string getFolder(string folder)
        {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            return folder; 
        }
        public static string generateTourneyFolder(string tourneyName)
        {
            DateTime eventDate = DateTime.Now;
            return eventDate.ToString("yyyy_MM_dd") + "-" + Utilities.makeIdentifier_(tourneyName, "_");
        }
        public static string[] KnockoutSessionNames = new string[] { "Finals", "Semi_Finals", "Quarter_Finals", "Pre_Quarter_Finals" };

    }
}
