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

        public static int teamsLeft(string databaseFileName,int roundNumber)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(databaseFileName,Constants.TableName.EventNames);
            DataRow[] rows = table.Select("Withdraw_Round is null OR WithDraw_Round > " + roundNumber);
            return rows.Length;
        }

        public static bool hasWithdrawn(string databaseFileName, int teamNumber, int roundNumber)
        {
            DataTable table = AccessDatabaseUtilities.getDataTable(databaseFileName,Constants.TableName.EventNames);
            DataRow[] foundRows = table.Select("Team_Number=" + teamNumber + " AND NOT Withdraw_Round is null AND WithDraw_Round <= " + roundNumber, "");
            return foundRows.Length > 0;
        }
    }
}
