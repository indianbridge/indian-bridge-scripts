using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace IndianBridge.Common
{
    public struct DatabaseField
    {
        public string fieldName;
        public string fieldType;
        public int fieldSize;

        public DatabaseField(string name, string type, int size = 0)
        {
            fieldName = name;
            fieldType = type;
            fieldSize = size;
        }
    }

    public static class AccessDatabaseUtilities
    {
        public static Dictionary<string, OleDbDataAdapter> m_dataAdapters = new Dictionary<string, OleDbDataAdapter>();
        public static Dictionary<string, OleDbCommandBuilder> m_commandBuilders = new Dictionary<string, OleDbCommandBuilder>();
        public static DataSet m_ds = new DataSet();

        public static string makeKey_(string databaseFileName, string tableName)
        {
            return Utilities.makeIdentifier_(databaseFileName) + "_" + tableName;
        }

        public static int getIntValue(DataRow dRow, string columnName)
        {
            Object value = dRow[columnName];
            return (value == DBNull.Value ? 0 : (int)value);
        }

        public static double getDoubleValue(DataRow dRow, string columnName)
        {
            Object value = dRow[columnName];
            return (value == DBNull.Value ? 0 : (double)value);
        }

        public static string getStringValue(DataRow dRow, string columnName)
        {
            Object value = dRow[columnName];
            return (value == DBNull.Value ? "" : (string)value);
        }

        public static string getStringParameterValue(string databaseFileName, string tableName, string parameterName)
        {
            DataRow dRow = m_ds.Tables[makeKey_(databaseFileName, tableName)].Rows.Find(parameterName);
            return (dRow == null) ? null : (string)dRow["Parameter_Value"];
        }

        public static int getIntParameterValue(string databaseFileName, string tableName, string parameterName)
        {
            DataRow dRow = m_ds.Tables[makeKey_(databaseFileName, tableName)].Rows.Find(parameterName);
            return (dRow == null) ? -1 : (int)dRow["Parameter_Value"];
        }

        public static DataTable getDataTable(string databaseFileName, string tableName)
        {
            return m_ds.Tables[makeKey_(databaseFileName, tableName)];
        }

        public static void createDatabase(string databaseFileName)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";Jet OLEDB:Engine Type=5";
            ADOX.CatalogClass cat = new ADOX.CatalogClass();
            cat.Create(strAccessConn);
            cat = null;
        }


        public static DataTable loadDatabaseToTable(string databaseFileName, string tableName, string query = "")
        {
            string key = makeKey_(databaseFileName, tableName);
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            string sql = "SELECT * From " + tableName + (string.IsNullOrWhiteSpace(query)?"":" "+query);
            m_dataAdapters[key] = new OleDbDataAdapter(sql, connection);
            m_commandBuilders[key] = new OleDbCommandBuilder(m_dataAdapters[key]);
            m_dataAdapters[key].UpdateCommand = m_commandBuilders[key].GetUpdateCommand();
            if (m_ds.Tables.Contains(key))
            {
                m_ds.Tables[key].Rows.Clear();
            }
            m_dataAdapters[key].Fill(m_ds, key);
            List<string> primaryKeyStrings = getPrimaryKeys(connection, tableName);
            DataColumn[] columns = new DataColumn[primaryKeyStrings.Count];
            for (int i = 0; i < primaryKeyStrings.Count; ++i)
            {
                columns[i] = m_ds.Tables[key].Columns[primaryKeyStrings[i]];
            }
            m_ds.Tables[key].PrimaryKey = columns;
            connection.Close();
            connection = null;
            return m_ds.Tables[key];
        }

        public static void saveTableToDatabase(string databaseFileName, string tableName)
        {
            string key = makeKey_(databaseFileName, tableName);
            m_dataAdapters[key].Update(m_ds, key);
        }

        public static DataTable createTable(string databaseFileName, string tableName, bool stringParameters)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            string query = "CREATE TABLE " + tableName + "([Parameter_Name] TEXT(255) PRIMARY KEY, [Parameter_Value] ";
            query += (stringParameters?"TEXT(255)":"INTEGER");
            query += ")";
            OleDbCommand myCommand = new OleDbCommand(query, connection);
            myCommand.ExecuteNonQuery();
            connection.Close();
            connection = null;
            return loadDatabaseToTable(databaseFileName, tableName);
        }


        public static DataTable createTable(string databaseFileName, string tableName, List<DatabaseField> fields, List<string> primaryKeyFields)
        {
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            string query = "CREATE TABLE " + tableName + "(";
            foreach (DatabaseField field in fields)
            {
                query += ("[" + field.fieldName + "] " + field.fieldType + (field.fieldSize > 0 ? "(" + field.fieldSize + ")" : "") + ",");
            }
            query += ("CONSTRAINT primarykey PRIMARY KEY(");
            for (int i = 0; i < primaryKeyFields.Count; ++i)
            {
                query += (primaryKeyFields[i] + (i == primaryKeyFields.Count - 1 ? "" : ","));
            }
            query += "))";
            OleDbCommand myCommand = new OleDbCommand(query, connection);
            myCommand.ExecuteNonQuery();
            connection.Close();
            connection = null;
            return loadDatabaseToTable(databaseFileName, tableName);
        }

        public static void dropTable(string databaseFileName, string tableName)
        {
            string key = makeKey_(databaseFileName, tableName);
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            string query = "DROP TABLE " + tableName;
            OleDbCommand myCommand = new OleDbCommand(query, connection);
            myCommand.ExecuteNonQuery();
            connection.Close();
            connection = null;
            if (m_ds.Tables.Contains(key)) m_ds.Tables.Remove(key);
            if (m_dataAdapters.ContainsKey(key)) m_dataAdapters.Remove(key);
            if (m_commandBuilders.ContainsKey(key)) m_commandBuilders.Remove(key);
        }

        public static void addColumn(string databaseFileName, string tableName, List<DatabaseField> fields)
        {
            string key = makeKey_(databaseFileName, tableName);
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            foreach (DatabaseField field in fields)
            {
                string query = "ALTER TABLE " + tableName + " ADD COLUMN " + field.fieldName + " " + field.fieldType + (field.fieldSize > 0 ? "(" + field.fieldSize + ")" : "");
                OleDbCommand myCommand = new OleDbCommand(query, connection);
                myCommand.ExecuteNonQuery();
            }
            connection.Close();
            connection = null;
            loadDatabaseToTable(databaseFileName, tableName);
        }

        public static void dropColumn(string databaseFileName, string tableName, List<DatabaseField> fields)
        {
            string key = makeKey_(databaseFileName, tableName);
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";";
            OleDbConnection connection = new OleDbConnection(strAccessConn);
            connection.Open();
            foreach (DatabaseField field in fields)
            {
                string query = "ALTER TABLE " + tableName + " DROP COLUMN " + field.fieldName;
                OleDbCommand myCommand = new OleDbCommand(query, connection);
                myCommand.ExecuteNonQuery();
            }
            connection.Close();
            connection = null;
            loadDatabaseToTable(databaseFileName, tableName);
        }

        public static List<string> getPrimaryKeys(OleDbConnection connection, String tableName)
        {
            var primaryKeyStrings = new List<string>();
            DataTable mySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                                        new Object[] { null, null, tableName });
            int columnOrdinalForName = mySchema.Columns["COLUMN_NAME"].Ordinal;
            foreach (DataRow r in mySchema.Rows)
            {
                string keyName = r.ItemArray[columnOrdinalForName].ToString();
                primaryKeyStrings.Add(keyName);
            }
            return primaryKeyStrings;
        }

    }
}
