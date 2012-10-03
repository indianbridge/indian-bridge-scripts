using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IndianBridge.Common;

namespace IndianBridgeScorer
{
    public static class LocalUtilities
    {
        public static string getTeamName(string databaseFileName, string tableName, int teamNumber)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(databaseFileName, tableName);
            DataRow[] dRows = table.Select("Team_Number = " + teamNumber);
            return (dRows.Length > 0) ? (string)dRows[0]["Team_Name"] : "Unknown";
        }
    }
}
