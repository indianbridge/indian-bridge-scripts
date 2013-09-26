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
			// Tracks the position within the file
			var previousPosition = 0;

			// Skip empty lines at the beginning of the file, if any
			var results = SkipEmptyLines(fileContents);

			var currentPosition = results.IndexOf("\n");
			var headerLine = results.Substring(previousPosition, currentPosition - previousPosition);

			var columnNames = Utilities.GetColumns(headerLine);
			var values = Utilities.ConstructDataTable(columnNames);

			// Skip empty lines after the header, if any
			results = SkipEmptyLines(results.Substring(currentPosition + 1));

			//txtFileContents.AppendText(results);

			previousPosition = 0;
			currentPosition = results.IndexOf("\n");

			while (true)
			{
				var currentLine = results.Substring(previousPosition, currentPosition - previousPosition);
				if (currentLine == String.Empty || currentLine == "\r") break;
				var rowValues = currentLine.Split(new[] {','});
				Utilities.AddRow(rowValues, columnNames, ref values);
				previousPosition = currentPosition + 1;
				currentPosition = results.IndexOf("\n", previousPosition);
				if (currentPosition < 0) break;
			}

			parseResults = new Tuple<DataTable, string[]>(values, columnNames);

			return parseResults;
		}

		public static string SkipEmptyLines(string contents)
		{
			var previousPosition = 0;
			var currentPosition = contents.IndexOf("\n");
			var line = contents.Substring(previousPosition, currentPosition - previousPosition);
			while (true)
			{
				if (line != "\r")
				{
					break;
				}

				previousPosition = currentPosition + 1;
				currentPosition = contents.IndexOf("\n", previousPosition);
				line = contents.Substring(previousPosition, currentPosition - previousPosition);
			}

			return contents.Substring(previousPosition);
		}
	}
}
