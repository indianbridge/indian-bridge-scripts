
using System;
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

        public static bool confirmReload(string tableName)
        {
            DialogResult result = MessageBox.Show("Are you sure? Any changes you have made to the " + tableName + " table above will be lost!", "Confirm reload!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
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


        public static bool HasNull(DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                if (table.Rows.OfType<DataRow>().Any(r => r.IsNull(column)))
                    return true;
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
            foreach (String i in text) retVal += makeTableCell_(i, row, usePadding);
            return retVal;
        }
        public static String makeTableCell_(String text, int row, bool usePadding = false)
        {
            string borderText = (useBorder ? "border: 1px solid #000;" : "border: 1px solid #cef;");
            string paddingText = (usePadding ? "padding: " + paddingSize + "px " + paddingSize + "px;" : "");
            if (row % 2 == 0) return "<td style=\'" + paddingText + "background-color: #def;" + borderText + "\'>" + text + "</td>";
            else return "<td style=\'" + paddingText + "" + borderText + "\'>" + text + "</td>";
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

        public static void getGoogleSiteComponents(string address, out string siteName, out string pagePath)
        {
            string[] tokens = address.Split('/');
            siteName = tokens[4];
            pagePath = "";
            for (int i = 5; i < tokens.Length; ++i)
            {
                pagePath += "/" + tokens[i];
            }
        }

        public static String makeIdentifier_(String variableName)
        {
            Regex re = new Regex(@"\W");
            return re.Replace(variableName, "-");
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

    }
}
