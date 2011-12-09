using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using IndianBridge.GoogleAPIs;

namespace IndianBridge.ResultsManager
{
    public class ResultsDatabase
    {
        private DataSet m_dataSet = new DataSet();
        private List<string> m_tableNames = new List<string>();
        private Hashtable m_accessDataAdapters = new Hashtable();
        private string m_databaseFileName = "";
        public ResultsDatabase(List<string> tableNames)
        {
            m_tableNames = tableNames;
        }

        public void loadDataFromGoogleSpreadsheet(string spreadsheetName,List<string> sheetNames)
        {
            string username = "indianbridge.dummy@gmail.com";
            string password = "kibitzer";
            SpreadSheetAPI ssa = new SpreadSheetAPI(username: username, password: password, spreadsheetname: spreadsheetName);
            foreach(string sheetName in sheetNames) ssa.getValues(sheetName, m_dataSet);
            saveChangesToAccessDatabase();
        }

        public void saveChangesToAccessDatabase()
        {
            foreach (string tableName in m_tableNames)
            {
                OleDbDataAdapter da = (OleDbDataAdapter)m_accessDataAdapters[tableName];
                da.Update(m_dataSet, tableName);
            }
        }

        public void loadAccessDatabase(string databaseFileName) 
        {
            m_databaseFileName = databaseFileName;
#if USINGPROJECTSYSTEM
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+filePath;
#else
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
#endif
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            foreach (string tableName in m_tableNames) loadTableFromAccessDatabase(connection, tableName);
            connection.Close();
            connection = null;
        }

        private void loadTableFromAccessDatabase(OleDbConnection connection, string tableName) 
        {
            Trace.WriteLine("Loading Table : " + tableName + " from " + m_databaseFileName);
            Trace.Indent();
            string sql = "SELECT * From " + tableName;
            OleDbDataAdapter da = new OleDbDataAdapter(sql, connection);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
            da.UpdateCommand = cb.GetUpdateCommand();
            da.Fill(m_dataSet, tableName);
            m_dataSet.Tables[tableName].PrimaryKey = getPrimaryKeys(connection: connection, tableName: tableName);
            m_accessDataAdapters.Add(tableName,da);
            Trace.Unindent();
        }

        public DataColumn[] getPrimaryKeys(OleDbConnection connection,String tableName)
        {
            var primaryKeyStrings = new List<string>();


            DataTable mySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                                        new Object[] { null, null, tableName });


            // following is a lengthy form of the number '3' :-)
            int columnOrdinalForName = mySchema.Columns["COLUMN_NAME"].Ordinal;

            foreach (DataRow r in mySchema.Rows)
            {
                string keyName = r.ItemArray[columnOrdinalForName].ToString();
                Debug.WriteLine("Table : " + tableName + ", key : " + keyName);
                primaryKeyStrings.Add(keyName);
            }

            DataColumn[] primaryKeys = new DataColumn[primaryKeyStrings.Count];
            for (int i = 0; i < primaryKeyStrings.Count; ++i)
            {
                primaryKeys[i] = m_dataSet.Tables[tableName].Columns[primaryKeyStrings[i]];
            }
            return primaryKeys;
        }
    }
}
