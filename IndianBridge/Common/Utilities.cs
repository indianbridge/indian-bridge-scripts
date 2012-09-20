
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

namespace IndianBridge.Common
{
    public static class Utilities
    {

        public static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public static double fontSize = 0.8;

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

        public static String makeTablePreamble_()
        {
            return "<table style=\'font-size:"+fontSize+"em;border: 1px solid #cef;text-align: left;\'>";
        }
        public static String makeTableHeader_(ArrayList text, bool usePadding = false)
        {
            var retVal = "";
            foreach (String i in text) retVal += makeTableHeader_(i,usePadding);
            return retVal;
        }
        public static String makeTableHeader_(String text, bool usePadding = false)
        {
            return "<th style=\'font-weight: bold;background-color: #acf;border-bottom: 1px solid #cef;" + (usePadding ? "padding: 4px 5px;" : "") + "\'>" + text + "</th>";
        }
        public static String makeTableCell_(ArrayList text, int row, bool usePadding = false)
        {
            var retVal = "";
            foreach (String i in text) retVal += makeTableCell_(i, row,usePadding);
            return retVal;
        }
        public static String makeTableCell_(String text, int row, bool usePadding = false)
        {
            if (row % 2 == 0) return "<td style=\'" + (usePadding ? "padding: 4px 5px;" : "") + "background-color: #def; border-bottom: 1px solid #cef;\'>" + text + "</td>";
            else return "<td " + (usePadding ? "style=\'padding: 4px 5px;\'" : "") + ">" + text + "</td>";
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
