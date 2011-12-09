using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;

namespace IndianBridge.GoogleAPIs
{
    public class SpreadSheetAPI
    {
        static public String APP_NAME = "SpreadsheetAPI-v0.1";
        private SpreadsheetsService m_service = null;
        private SpreadsheetEntry m_spreadsheetEntry = null;
        private string m_spreadsheetName = null;
        private Hashtable m_worksheets = new Hashtable();

        public SpreadSheetAPI(String spreadsheetname, String username, String password)
        {
            this.m_service = new SpreadsheetsService(APP_NAME);
            this.m_service.setUserCredentials(username, password);
            this.m_spreadsheetName = spreadsheetname;
            initialize();
        }

        private void initialize()
        {
            findSpreadsheet();
            getAllWorksheets();
        }

        public void getValues(string sheetName, DataSet dataSet)
        {
            var tableName = sheetName;
            if (dataSet.Tables[tableName] == null) tableName = sheetName + "_";
            if (dataSet.Tables[tableName] == null) throw new ArgumentException("Neither " + sheetName + " not " + sheetName + "_ tables could be found in the passed dataset!!!");
            DataColumnCollection columnNames = dataSet.Tables[tableName].Columns;
            WorksheetEntry entry = (WorksheetEntry)m_worksheets[sheetName];
            if (entry == null)
            {
                string message = sheetName + " not found in " + m_spreadsheetName;
                Trace.WriteLine(message);
                throw new ArgumentNullException(message);
            }
            AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery query = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed feed = m_service.Query(query);
            Trace.WriteLine("Found " + feed.Entries.Count + " rows of data in "+sheetName);
            if (feed.Entries.Count > 0)
            {
                ListEntry worksheetRow = (ListEntry)feed.Entries[0];
                ListEntry.CustomElementCollection elements = worksheetRow.Elements;
                for (int j = 0; j < feed.Entries.Count; ++j)
                {
                    worksheetRow = (ListEntry)feed.Entries[j];
                    elements = worksheetRow.Elements;
                    DataRow workRow = dataSet.Tables[tableName].NewRow();
                    for (int i = 0; i < elements.Count; ++i)
                    {
                        string value = ""+elements[i].Value;
                        Type type = columnNames[i].DataType;
                        if (type == System.Type.GetType("System.Double") && string.IsNullOrWhiteSpace(value)) value = "0";
                        if (type == System.Type.GetType("System.Int32") && string.IsNullOrWhiteSpace(value)) value = "0";
                        workRow[columnNames[i].ColumnName] = value;
                    }
                    dataSet.Tables[tableName].Rows.Add(workRow);
                }
            }
            return;
        }
        /*public NameValueCollection getInfo()
        {
            NameValueCollection parameters = new NameValueCollection();
            WorksheetEntry entry = (WorksheetEntry)m_worksheets["Info"];
            AtomLink listFeedLink = entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            ListQuery query = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed feed = m_service.Query(query);

            foreach (ListEntry worksheetRow in feed.Entries)
            {
                ListEntry.CustomElementCollection elements = worksheetRow.Elements;
                parameters.Add(elements[0].Value, elements[1].Value);
            }
            return parameters;
        }*/

        private void findSpreadsheet()
        {
            SpreadsheetQuery query = new SpreadsheetQuery();
            query.Title = m_spreadsheetName;
            Trace.WriteLine("Trying to find spreadsheet with name " + m_spreadsheetName);
            SpreadsheetFeed feed = m_service.Query(query);
            if (feed.Entries.Count > 0)
            {
                m_spreadsheetEntry = (SpreadsheetEntry)feed.Entries[0];
                Trace.WriteLine("Found spreadsheet with name " + m_spreadsheetEntry.Title.Text);
            }
            else
            {
                Trace.WriteLine("No Spreadsheet with name " + m_spreadsheetName+" could be found!!!");
                throw new ArgumentNullException("Spreadsheet with name " + m_spreadsheetName+" not found!!!");
            }
        }
        private void getAllWorksheets()
        {
            AtomLink link = m_spreadsheetEntry.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);

            WorksheetQuery query = new WorksheetQuery(link.HRef.ToString());
            WorksheetFeed feed = m_service.Query(query);

            Trace.WriteLine("Found "+feed.Entries.Count+" worksheets.");
            Trace.Indent();
            foreach (WorksheetEntry worksheet in feed.Entries)
            {
                Trace.WriteLine(worksheet.Title.Text);
                m_worksheets.Add(worksheet.Title.Text,worksheet);
            }
            Trace.Unindent();
        }
    }
}
