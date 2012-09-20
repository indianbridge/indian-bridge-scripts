using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using IndianBridge.Common;

namespace IndianBridgeScorer
{
    public class TourneyInformationDatabase
    {
        private string m_databaseFileName;
        private string m_rootFolder;
        public DataSet m_ds;
        public System.Data.OleDb.OleDbDataAdapter m_daTourneyInfo;
        public OleDbCommandBuilder m_cbTourneyInfo;
        public System.Data.OleDb.OleDbDataAdapter m_daTourneyEvents;
        public OleDbCommandBuilder m_cbTourneyEvents;

        public static string tourneyInfoTableName = "TourneyInfo";
        public static string tourneyEventsTableName = "TourneyEvents";

        public TourneyInformationDatabase()
        {
            initializeDataset();
        }

        public bool eventExists(string eventName)
        {
            DataTable table = m_ds.Tables[tourneyEventsTableName];
            return table.Rows.Find(eventName) != null;
        }

        public void deleteEvent(string eventName)
        {
            DataTable table = m_ds.Tables[tourneyEventsTableName];
            DataRow dRow = table.Rows.Find(eventName);
            string eventFileName = Path.Combine(Globals.m_rootDirectory, "Tourneys", (string)dRow["Event_File"]);
            File.Delete(eventFileName);
            dRow.Delete();
            m_daTourneyEvents.Update(m_ds, tourneyEventsTableName);
        }

        public void addNewEvent(string eventName, string eventType)
        {
            DataTable table = m_ds.Tables[tourneyEventsTableName];
            DataRow dRow = table.NewRow();
            dRow["Event_Name"] = eventName;
            dRow["Event_Type"] = eventType;
            dRow["Event_File"] = Path.Combine(m_rootFolder, "Databases", Utilities.makeIdentifier_(eventName)+".mdb"); 
            table.Rows.Add(dRow);
            m_daTourneyEvents.Update(m_ds, tourneyEventsTableName);
        }

        public TourneyInformationDatabase(string rootFolder)
        {
            initializeDataset();
            setRootFolder(rootFolder);
        }

        public void setRootFolder(string rootFolder)
        {
            m_rootFolder = rootFolder;
            m_databaseFileName = Path.Combine(Globals.m_rootDirectory,"Tourneys",m_rootFolder, "Databases", "TourneyInformation.mdb");
            loadDatabase();
        }
        public string getTourneyName()
        {
            if (m_ds.Tables[tourneyInfoTableName].Rows.Count < 1) return "None";
            DataRow dRow = m_ds.Tables[tourneyInfoTableName].Rows[0];
            return (string)dRow["Tourney_Name"];
        }

        public string getTourneyResultsWebsite()
        {
            if (m_ds.Tables[tourneyInfoTableName].Rows.Count < 1) return "None";
            DataRow dRow = m_ds.Tables[tourneyInfoTableName].Rows[0];
            return (string)dRow["Tourney_Results_Website"];
        }

        public void setTourneyInfo(string tourneyName, DateTime tourneyDate, string tourneyResultsWebsite)
        {
            DataTable table = m_ds.Tables[tourneyInfoTableName];
            table.Clear();
            DataRow dRow = table.NewRow();
            dRow["Tourney_Name"] = tourneyName;
            dRow["Tourney_Start_Date"] = tourneyDate;
            dRow["Tourney_End_Date"] = tourneyDate;
            dRow["Tourney_Results_Website"] = tourneyResultsWebsite;
            table.Rows.Add(dRow);
            m_daTourneyInfo.Update(m_ds, tourneyInfoTableName);
        }

        private void initializeDataset()
        {
            m_ds = new DataSet();
            m_daTourneyEvents = null;
            m_cbTourneyEvents = null;
            m_daTourneyInfo = null;
            m_cbTourneyInfo = null;
        }

        private void loadDatabase()
        {
#if USINGPROJECTSYSTEM
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+filePath;
#else
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_databaseFileName + ";";
#endif
            OleDbConnection m_myAccessConn = new OleDbConnection(strAccessConn);
            m_myAccessConn.Open();
            string sql = "SELECT * From " + tourneyInfoTableName;
            m_daTourneyInfo = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            m_cbTourneyInfo = new OleDbCommandBuilder(m_daTourneyInfo);
            m_daTourneyInfo.Fill(m_ds, tourneyInfoTableName);
            sql = "SELECT * From " + tourneyEventsTableName;
            m_daTourneyEvents = new System.Data.OleDb.OleDbDataAdapter(sql, m_myAccessConn);
            m_cbTourneyEvents = new OleDbCommandBuilder(m_daTourneyEvents);
            m_daTourneyEvents.Fill(m_ds, tourneyEventsTableName);
            DataTable table = m_ds.Tables[tourneyEventsTableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["Event_Name"] };
            m_myAccessConn.Close();
            m_myAccessConn = null;
        }
    }
}
