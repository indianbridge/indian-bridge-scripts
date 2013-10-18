using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IndianBridge.Common
{
	public static class CSVUtilities
	{
        public static Tuple<DataTable, string[]> ParseCSV(string fileContents)
        {
            Tuple<DataTable, string[]> parseResults = null;
            string[] lines = fileContents.Split(new string[] { Utilities.getNewLineCharacter(fileContents) }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            while (index < lines.Length && string.IsNullOrWhiteSpace(lines[index])) index++;
            if (index >= lines.Length) return parseResults;
            var columnNames = Utilities.GetColumns(lines[index++]);
            var values = Utilities.ConstructDataTable(columnNames);
            while (index < lines.Length)
            {
                var currentLine = lines[index++];
                if (string.IsNullOrWhiteSpace(currentLine)) break;
                var rowValues = currentLine.Split(new[] { ',' });
                Utilities.AddRow(rowValues, columnNames, ref values);
            }

            parseResults = new Tuple<DataTable, string[]>(values, columnNames);

            return parseResults;
        }

	}
}
