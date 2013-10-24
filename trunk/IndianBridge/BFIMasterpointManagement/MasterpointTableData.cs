using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using IndianBridge.Common;
using Excel;

namespace BFIMasterpointManagement
{
    public static class MasterpointManagementUtilties
    {
        public static string loadUsersFile(string fileName, ref string errorMessage, ref bool error)
        {
            DataTable table = createUserDataTable();
            loadDataTable(fileName, ref table, ref errorMessage, ref error);
            if (error) return "";
            checkUserDataTable(ref table, ref errorMessage, ref error);
            if (error) return "";
            return dataTableToCSV(table);
        }

        public static string loadMasterpointsFile(string fileName, ref string errorMessage, ref bool error)
        {
            DataTable table = createMasterpointDataTable();
            loadDataTable(fileName, ref table, ref errorMessage, ref error);
            if (error) return "";
            checkMasterpointDataTable(ref table, ref errorMessage, ref error);
            if (error) return "";
            return dataTableToCSV(table);
        }

        public static string loadDeleteUsersFile(string fileName, ref string errorMessage, ref bool error)
        {
            DataTable table = createDeleteUserDataTable();
            loadDataTable(fileName, ref table, ref errorMessage, ref error);
            if (error) return "";
            checkUserDataTable(ref table, ref errorMessage, ref error);
            if (error) return "";
            return dataTableToCSV(table);
        }


        private static void checkUserDataTable(ref DataTable table, ref string errorMessage, ref bool error)
        {
            int rowNo = 2;
            foreach (DataRow row in table.Rows)
            {
                checkStringLength(row, "member_id", 8, ref errorMessage, ref error, "At Row " + rowNo + " : ");
                rowNo++;
            }
        }

        private static void checkMasterpointDataTable(ref DataTable table, ref string errorMessage, ref bool error)
        {
            int rowNo = 2;
            foreach (DataRow row in table.Rows)
            {
                checkStringLength(row, "tournament_code", 5, ref errorMessage, ref error, "At Row " + rowNo + " : ");
                checkStringLength(row, "event_code", 3, ref errorMessage, ref error, "At Row " + rowNo + " : ");
                checkStringLength(row, "event_date", 10, ref errorMessage, ref error, "At Row " + rowNo + " : ");
                checkStringLength(row, "member_id", 8, ref errorMessage, ref error, "At Row " + rowNo + " : ");
                checkDouble(row, "localpoints_earned", ref errorMessage, ref error, "At Row " + rowNo + " : ");
                checkDouble(row, "fedpoints_earned", ref errorMessage, ref error, "At Row " + rowNo + " : ");
                rowNo++;
            }
        }

        private static DataColumn createStringDataColumn(string columnName, bool allowDBNull = false)
        {
            DataColumn dColumn = new DataColumn(columnName, Type.GetType("System.String"));
            dColumn.AllowDBNull = allowDBNull;
            return dColumn;
        }
        private static DataColumn createStringDataColumn(string columnName, string defaultValue, bool allowDBNull = false)
        {
            DataColumn dColumn = new DataColumn(columnName, Type.GetType("System.String"));
            dColumn.AllowDBNull = allowDBNull;
            dColumn.DefaultValue = defaultValue;
            return dColumn;
        }
        private static DataColumn createIntDataColumn(string columnName, bool allowDBNull = false)
        {
            DataColumn dColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            dColumn.AllowDBNull = allowDBNull;
            return dColumn;
        }
        private static DataColumn createIntDataColumn(string columnName, int defaultValue, bool allowDBNull = false)
        {
            DataColumn dColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
            dColumn.AllowDBNull = allowDBNull;
            dColumn.DefaultValue = defaultValue;
            return dColumn;
        }
        private static DataColumn createDoubleDataColumn(string columnName, bool allowDBNull = false)
        {
            DataColumn dColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            dColumn.AllowDBNull = allowDBNull;
            return dColumn;
        }
        private static DataColumn createDoubleDataColumn(string columnName, double defaultValue, bool allowDBNull = false)
        {
            DataColumn dColumn = new DataColumn(columnName, Type.GetType("System.Double"));
            dColumn.AllowDBNull = allowDBNull;
            dColumn.DefaultValue = defaultValue;
            return dColumn;
        }

        private static void checkStringLength(DataRow row, string columnName, int length, ref string errorMessage, ref bool error, string prefix)
        {
            try
            {
                string value = (string)row[columnName];
                if (value.Length != length)
                {
                    Utilities.appendToMessage(ref errorMessage, prefix + " Value for " + columnName + " - " + value + " has to be exactly " + length + " characters long!");
                    error = true;
                }
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, prefix + exc.Message);
            }
        }

        private static void checkDouble(DataRow row, string columnName, ref string errorMessage, ref bool error, string prefix)
        {
            if (row[columnName].GetType() != typeof(double))
            {
                double num;
                string value = (string)row[columnName];
                bool localError = !(double.TryParse(value, out num));
                if (localError)
                {
                    error = true;
                    Utilities.appendToMessage(ref errorMessage, prefix + "Value for " + columnName + " : " + value + " is not numeric!");
                }
            }
        }

        private static void checkInt(DataRow row, string columnName, ref string errorMessage, ref bool error, string prefix)
        {
            if (row[columnName].GetType() != typeof(int))
            {
                int num;
                string value = (string)row[columnName];
                bool localError = !(int.TryParse(value, out num));
                if (localError)
                {
                    error = true;
                    Utilities.appendToMessage(ref errorMessage, prefix + "Value for " + columnName + " : " + value + " is not numeric!");
                }
            }
        }

        private static void loadDataTable(string fileName, ref DataTable table, ref string errorMessage, ref bool error)
        {
            if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
            {
                parseExcelFile(fileName, ref table, ref errorMessage, ref error);
            }
            else if (fileName.EndsWith(".txt") || fileName.EndsWith(".csv"))
            {
                parseCSVFile(fileName, ref table, ref errorMessage, ref error);
            }
            else
            {
                errorMessage = "Unknown file extension in file name : " + fileName;
                error = true;
                return;
            }
        }

        private static DataTable createUserDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(createStringDataColumn("member_id", false));
            table.Columns.Add(createStringDataColumn("first_name", false));
            table.Columns.Add(createStringDataColumn("last_name", "", true));
            table.Columns.Add(createDoubleDataColumn("total_current_lp", 0, false));
            table.Columns.Add(createDoubleDataColumn("total_current_fp", 0, false));
            return table;
        }

        private static DataTable createDeleteUserDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(createStringDataColumn("member_id", false));
            return table;
        }

        private static DataTable createMasterpointDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(createStringDataColumn("tournament_code", false));
            table.Columns.Add(createStringDataColumn("event_code", false));
            table.Columns.Add(createStringDataColumn("event_date", false));
            table.Columns.Add(createStringDataColumn("member_id", false));
            table.Columns.Add(createDoubleDataColumn("localpoints_earned", 0, false));
            table.Columns.Add(createDoubleDataColumn("fedpoints_earned", 0, false));
            return table;
        }

        private static void addRow(ref DataTable table, string[] columnNames, string[] values, ref string errorMessage, ref bool error, string prefix)
        {
            try
            {
                DataRow dRow = table.NewRow();
                for (int i = 0; i < columnNames.Length; ++i)
                {
                    if (columnNames[i] == "event_date") {
                        DateTime date = Convert.ToDateTime(values[i]);
                        dRow[columnNames[i]] = date.ToString("yyyy-MM-dd");
                    }
                    else {
                        dRow[columnNames[i]] = values[i];
                    }
                }
                table.Rows.Add(dRow);
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, prefix + exc.Message);
            }
        }

        private static string dataTableToCSV(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            string[] columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in table.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(",", fields));
            }
            return sb.ToString();
        }

        private static void parseExcelSheet(DataTable excelTable, ref DataTable table, ref string errorMessage, ref bool error, string prefix)
        {
            string[] columnNames = excelTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            int rowNo = 2;
            foreach (DataRow row in excelTable.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                addRow(ref table, columnNames, fields, ref errorMessage, ref error, prefix + " at Row " + rowNo + " : ");
                rowNo++;
            }
        }

        private static void parseExcelFile(string fileName, ref DataTable table, ref string errorMessage, ref bool error)
        {
            try
            {
                FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = fileName.EndsWith(".xls") ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet(true);
                excelReader.Close();
                for (int i = 0; i < result.Tables.Count; ++i) parseExcelSheet(result.Tables[i], ref table, ref errorMessage, ref error, "In Excel Sheet " + result.Tables[i].TableName);
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, exc.Message);
                return;
            }
        }

        private static void parseCSVFile(string fileName, ref DataTable table, ref string errorMessage, ref bool error)
        {
            try
            {
                string fileContents = File.ReadAllText(fileName);
                string[] lines = fileContents.Split(new string[] { Utilities.getNewLineCharacter(fileContents) }, StringSplitOptions.RemoveEmptyEntries);
                int index = 0;
                while (index < lines.Length && string.IsNullOrWhiteSpace(lines[index])) index++;
                if (lines.Length - index < 2)
                {
                    error = true;
                    Utilities.appendToMessage(ref errorMessage, "CSV File contents needs minimum 2 lines - one for header and atleast one for actual content!");
                    return;
                }
                // Header line
                string headerLine = lines[index++];
                string[] columnNames = headerLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                while (index < lines.Length)
                {
                    var currentLine = lines[index++];
                    if (string.IsNullOrWhiteSpace(currentLine)) break;
                    var rowValues = currentLine.Split(new[] { ',' });
                    addRow(ref table, columnNames, rowValues, ref errorMessage, ref error, "In Row " + index + " : ");
                }
            }
            catch (Exception exc)
            {
                error = true;
                Utilities.appendToMessage(ref errorMessage, exc.Message);
                return;
            }

        }

    }
}
