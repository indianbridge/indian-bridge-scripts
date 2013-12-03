
using System;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Common;

namespace IndianBridge.Common
{
	public static class Utilities
	{
		public struct HTMLTableParameters
		{
			public string sortCriteria;
			public string sectionName;
			public string fileName;
			public string filterCriteria;
			public string headerTemplate;
			public string tableName;
			public OrderedDictionary columns;

			public HTMLTableParameters(string table="Teams")
			{
				sortCriteria = "Event_Rank ASC";
				sectionName = "";
				fileName = "test.html";
				filterCriteria = "Pair_Number = '1'";
				headerTemplate = "Summary for";
				tableName = table;
				columns = new OrderedDictionary();
			}
		}
		public static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
		public static double fontSize = 0.8;
		public static double paddingSize = 5;
		public static bool useBorder = false;

		public static Dictionary<TabControl, List<TabPage>> m_tabPages = new Dictionary<TabControl,List<TabPage>>();

		public static void appendToMessage(ref string message, string extra)
		{
			message += (string.IsNullOrWhiteSpace(message)?"":Environment.NewLine)+extra;
			return;
		}

		public static string[] arraySlice(string[] source, int start, int end)
		{
			// Handles negative ends.
			if (end > source.Length - 1)
			{
				end = source.Length - 1;
			}
			int len = end - start + 1;

			// Return new array.
			string[] res = new string[len];
			for (int i = 0; i < len; i++)
			{
				res[i] = source[i + start];
			}
			return res;
		}

		public static string getNextCode(string value) {
			int totalLength = value.Length;
			var numAlpha = new Regex("(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
			var match = numAlpha.Match(value);

			string alpha = match.Groups["Alpha"].Value;
			int alphaLength = alpha.Length;
			int num = int.Parse(match.Groups["Numeric"].Value);
			int nextNum = num + 1;
			int numLength = totalLength - alpha.Length;
			int maxNum = (int)Math.Pow(10, numLength) - 1;
			if (nextNum <= maxNum)
			{
				string newString = alpha + nextNum.ToString().PadLeft(numLength, '0');
				return newString;
			}
			else
			{
				nextNum = 1;
				if (alpha.Distinct().Count() == 1 && alpha.Distinct().ElementAt(0) == 'Z')
				{
					alpha = "".PadLeft(alphaLength + 1, 'A');
					numLength = totalLength - alpha.Length;
					string newString = alpha + nextNum.ToString().PadLeft(numLength, '0');
					return newString;
				}
				else
				{
					char[] alphaChars = alpha.ToCharArray();
					bool flag = true;
					int index = alphaChars.Length - 1;
					while (flag)
					{
						if (alphaChars[index] != 'Z')
						{
							alphaChars[index]++;
							flag = false;
						}
						index--;
					}
					alpha = new string(alphaChars);
					numLength = totalLength - alpha.Length;
					string newString = alpha + nextNum.ToString().PadLeft(numLength, '0');
					return newString;
				}

			}
		}

		public static void showBalloonNotification(string title, string text)
		{
			NotifyIcon notifyMessage = new NotifyIcon();
			notifyMessage.BalloonTipText = text;
			notifyMessage.BalloonTipTitle = title;
			notifyMessage.Icon = SystemIcons.Information;
			notifyMessage.Visible = true;
			notifyMessage.ShowBalloonTip(3);
		}

		public static void setReadOnlyAndVisibleColumns(DataGridView dgv, string[] readOnlyColumns, string[] hideColumns)
		{
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				column.ReadOnly = false;
				column.Visible = true;
			}
			if (readOnlyColumns != null)
			{
				foreach (string str in readOnlyColumns)
				{
					dgv.Columns[str].ReadOnly = true;
				}
			}
			if (hideColumns != null)
			{
				foreach (string str in hideColumns)
				{
					dgv.Columns[str].Visible = false;
				}
			}
		}

		public static void SetDataGridViewProperties(Control controlContainer)
		{
			foreach (Control control in controlContainer.Controls)
			{
				if (control is DataGridView)
				{
					DataGridView dgv = control as DataGridView;
					dgv.KeyDown += new KeyEventHandler(Utilities.handleCopyPaste);
					dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
				}
				else if (control.HasChildren) SetDataGridViewProperties(control);
			}
		}

		public static bool confirmReload(string tableName)
		{
			DialogResult result = MessageBox.Show("Are you sure? Any changes you have made to the " + tableName + " table above will be lost!", "Confirm reload!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			return result == DialogResult.Yes;
		}

		public static TourneyResults ConvertJsonOutputToTourneyResults(string json_result)
		{
			return JsonDeserialize<TourneyResults>(json_result);
		}
		
		public static Dictionary<string, string> convertJsonOutput(string json_result)
		{
			string jsonDelimiter = "~";
			Dictionary<string, string> result = new Dictionary<string, string>();
			string[] lines = json_result.Split(new string[] { jsonDelimiter }, StringSplitOptions.None);
			if (lines.Length < 3)
			{
				result["error"] = "true";
				result["message"] = lines[0];
				result["content"] = lines[0];
			}
			else
			{
				result["error"] = lines[0];
				result["message"] = lines[1];
				result["content"] = lines[2];
			}
			return result;
		}

		public static string getNewLineCharacter(string message)
		{
			string splitter = "\r\n";
			if (message.Contains(splitter)) return splitter;
			splitter = Environment.NewLine;
			if (message.Contains(splitter)) return splitter;
			return "\n";
		}

		public static void showErrorMessage(string message)
		{
			MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void showWarningessage(string message)
		{
			MessageBox.Show(message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		public static void saveTabs(TabControl tabControl)
		{
			m_tabPages[tabControl] = new List<TabPage>();
			foreach (TabPage tp in tabControl.TabPages) m_tabPages[tabControl].Add(tp);
		}

		public static void hideTabs(TabControl tabControl, List<string> tabNames = null)
		{
			foreach (TabPage tp in m_tabPages[tabControl])
			{
				if (tabNames == null || tabNames.Contains(tp.Name)) tabControl.TabPages.Remove(tp);
			}
		}

		public static void showTabs(TabControl tabControl, List<string> tabNames = null)
		{
			foreach (TabPage tp in m_tabPages[tabControl])
			{
				if (!tabControl.TabPages.Contains(tp) && (tabNames == null || tabNames.Contains(tp.Name))) tabControl.TabPages.Add(tp);
			}
		}

		public static void handleCopyPaste(object sender, KeyEventArgs e)
		{
			DataGridView dgv = sender as DataGridView;
			if (e.Control && e.KeyCode == Keys.C)
			{
				DataObject d = dgv.GetClipboardContent();
				Clipboard.SetDataObject(d);
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.V)
			{
				string s = Clipboard.GetText();
				string[] lines = s.Split('\n');
				int row = dgv.CurrentCell.RowIndex;
				int col = dgv.CurrentCell.ColumnIndex;
				foreach (string line in lines)
				{
					if (row < dgv.RowCount && line.Length > 0)
					{
						string[] cells = line.Split('\t');
						for (int i = 0; i < cells.GetLength(0); ++i)
						{
							if (col + i < dgv.ColumnCount) dgv[col + i, row].Value = Convert.ChangeType(cells[i], dgv[col + i, row].ValueType);
							else break;
						}
						row++;
					}
					else break;
				}
			}
		}


		public static bool HasNull(DataTable table, List<string> skipColumnNames = null)
		{
			foreach (DataColumn column in table.Columns)
			{
				if (skipColumnNames == null || !skipColumnNames.Contains(column.ColumnName))
				{
					if (table.Rows.OfType<DataRow>().Any(r => r.IsNull(column))) return true;
				}
			}

			return false;
		}

		public static bool AllNull(DataTable table, string columnName)
		{
			foreach (DataRow row in table.Rows)
			{
				Object value = row[columnName];
				if (value != DBNull.Value) return false;
			}

			return true;
		}

		public static string makeHtmlPrintStyle_()
		{
			string result = "";
			result += "<style>";
			result += "@media print";
			result += "{";
			result += "table { page-break-after:auto }";
			result += "tr    { page-break-inside:avoid; page-break-after:auto }";
			result += "td    { page-break-inside:avoid; page-break-after:auto }";
			result += "thead { display:table-header-group }";
			result += "tfoot { display:table-footer-group }";
			result += "}";
			result += "</style>";
			return result;
		}

		public static string makeTablePreamble_()
		{
			string borderText = (useBorder ? "border: 1px solid #000;" : "border: 1px solid #cef;");
			return "<table style=\'font-size:" + fontSize + "em;" + borderText + "text-align: left;\'>";
		}
		public static String makeTableHeader_(ArrayList text, bool usePadding = false)
		{
			var retVal = "";
			foreach (String i in text) retVal += makeTableHeader_(i, usePadding);
			return retVal;
		}
		public static String makeTableHeader_(String text, bool usePadding = false)
		{
			string borderText = (useBorder ? "border: 1px solid #000;" : "border: 1px solid #cef;");
			return "<th style=\'font-weight: bold;background-color: #acf;" + borderText + "" + (usePadding ? "padding: " + paddingSize + "px " + paddingSize + "px;" : "") + "\'>" + text + "</th>";
		}
		public static String makeTableCell_(ArrayList text, int row, bool usePadding = false)
		{
			var retVal = "";
			foreach (String i in text) retVal += makeTableCell_(i, row, usePadding,0);
			return retVal;
		}
		public static String makeTableCell_(String text, int row, bool usePadding = false, int rowSpan = 0)
		{
			string borderText = (useBorder ? "border: 1px solid #000;" : "border: 1px solid #cef;");
			string paddingText = (usePadding ? "padding: " + paddingSize + "px " + paddingSize + "px;" : "");
			if (row % 2 == 0) return "<td "+(rowSpan<1?"":"rowspan="+rowSpan+" ")+"style=\'" + paddingText + "background-color: #def;" + borderText + "\'>" + text + "</td>";
			else return "<td " + (rowSpan < 1 ? "" : "rowspan=" + rowSpan+" ") + "style=\'" + paddingText + "" + borderText + "\'>" + text + "</td>";
		}
		public static void createDatabase(string databaseFileName)
		{
			if (!File.Exists(databaseFileName))
			{
				string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databaseFileName + ";Jet OLEDB:Engine Type=5";
				ADOX.CatalogClass cat = new ADOX.CatalogClass();
				cat.Create(strAccessConn);
				cat = null;
			}
			else
			{
				MessageBox.Show(databaseFileName + " already exists. Cannot create!!!", "File Already Exists!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		public static bool getWordpressSiteComponents(string address, out string siteName, out string pagePath)
		{
			string[] tokens = address.Split('/');
			string root = tokens[2];
			siteName = tokens[0] + "/" + tokens[1] + "/" + tokens[2] + "/";
			int startToken = 3;
			if (containsPattern_(root, "localhost") || containsPattern_(root, "127.0.0.1"))
			{
				siteName += (tokens[3] + "/");
				startToken = 4;
			}
			pagePath = "";
			for (int i = startToken; i < tokens.Length; ++i)
			{
				pagePath += "/" + tokens[i];
			}
			return true;
		}

		public static string combinePath(string path1, string path2)
		{
			if (string.IsNullOrWhiteSpace(path2)) return path1.Replace('\\', '/');
			char[] trims = new char[] { '\\', '/' };
			string result = "";
			result = path1.TrimEnd(trims) + "/" + path2.TrimStart(trims);
			result = result.Replace('\\', '/');
			return result;
		}

		public static bool getGoogleSiteComponents(string address, out string siteName, out string pagePath)
		{
			if (!containsPattern_(address, "sites.google.com"))
			{
				siteName = "";
				pagePath = "";
				return false;
			}
			string[] tokens = address.Split('/');
			siteName = tokens[4];
			pagePath = "";
			for (int i = 5; i < tokens.Length; ++i)
			{
				pagePath += "/" + tokens[i];
			}
			return true;
		}

		public static String makeIdentifier_(string variableName, string separator = "-")
		{
			Regex re = new Regex(@"\W");
			return re.Replace(variableName, separator);
		}

		public static double getDoubleValue(string value)
		{
			try
			{
				return string.IsNullOrWhiteSpace(value) ? 0 : Double.Parse(value);
			}
			catch (Exception) { return 0; }
		}

		/// <summary>
		/// Converts the phrase to specified convention.
		/// </summary>
		/// <param name="phrase">The phrase to convert</param>
		/// <param name="cases">The cases.</param>
		/// <returns>string</returns>
		public static string ConvertCaseString(string phrase, Case cases = Case.PascalCase)
		{
			string[] splittedPhrase = phrase.Split(' ', '-', '.', '_');
			var sb = new StringBuilder();

			if (cases == Case.CamelCase)
			{
				sb.Append(splittedPhrase[0].ToLower());
				splittedPhrase[0] = string.Empty;
			}
			else if (cases == Case.PascalCase)
				sb = new StringBuilder();
			int count = 0;
			foreach (String s in splittedPhrase)
			{
				char[] splittedPhraseChars = s.ToCharArray();
				if (splittedPhraseChars.Length > 0)
				{
					splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToUpper().ToCharArray())[0];
				}
				if (count++ > 0) sb.Append(' ');
				sb.Append(new String(splittedPhraseChars));
			}
			return sb.ToString();
		}

		public enum Case
		{
			PascalCase,
			CamelCase
		}
		public static bool containsPattern_(String text, String pattern)
		{
			Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
			return re.IsMatch(text);
		}
		public static String compressText_(String originalText)
		{
			String text = originalText;
			text = replace(text, "\\u0020+", "\u0020");
			text = replace(text, "\\u0020\\r\\n", "\r\n");
			text = replace(text, "\\r\\n\\s+", "\r\n");
			return text;
		}

		public static String replace(String text, String expression, String replaceText)
		{
			Regex re = new Regex(expression);
			return re.Replace(text, replaceText);
		}

		public static void WriteFile(string fileName, string content)
		{
			var fileStream = new StreamWriter(fileName);
			fileStream.Write(content);
			fileStream.Close();
		}
		public static DataTable ConstructDataTable(IEnumerable<string> columnNames)
		{
			var result = new DataTable();
			foreach (var columnName in columnNames)
			{
				result.Columns.Add(columnName, Type.GetType("System.String"));
			}
			return result;
		}

		public static void AddRow(string[] rowValues, string[] columnNames, ref DataTable result)
		{
			var row = result.NewRow();
			for (var i = 0; i < columnNames.Length; i++)
			{
				row[columnNames[i]] = rowValues[i];
			}
			result.Rows.Add(row);
		}
		public static string[] GetColumns(string headerLine)
		{
			string[] columnNames = headerLine.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < columnNames.Length; ++i)
			{
				columnNames[i] = columnNames[i].Replace("\n", String.Empty);
				columnNames[i] = columnNames[i].Replace("\r", String.Empty);
			}
			return columnNames;
		}
		public static string GetHTMLTableHeader(IEnumerable<object> columnNames)
		{
			var tableHeader = columnNames.Aggregate("<tr>", (current, value) =>
				current + String.Format("<th>{0}</th>", value.ToString().Trim()));
			tableHeader += "</tr>\n";
			return tableHeader;
		}

		public static string GetHTMLRowResult(IEnumerable<object> rowValues)
		{
			var rowString = rowValues.Aggregate("<tr>", (current, value) =>
				current + String.Format("<td>{0}</td>", value.ToString().Trim()));
			rowString += "</tr>\n";
			return rowString;
		}

		public static string WriteRowResult(IEnumerable<object> rowValues)
		{
			var rowString = rowValues.Aggregate(String.Empty, (current, rowValue) => current + String.Format("{0},", rowValue.ToString().Trim()));
			rowString = rowString.TrimEnd(new[] { ',' });
			rowString = rowString.Replace("\r", String.Empty);
			return rowString + "\n";
		}

		public static T JsonDeserialize<T>(string jsonString)
		{
			var ser = new DataContractJsonSerializer(typeof(T));
			var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
			var obj = (T)ser.ReadObject(ms);
			return obj;
		}

        public static string JsonSerialize<T>(T jsonObject) {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(stream1, jsonObject);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }
	}
}
