using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Data;
using System.Collections.Specialized;
using System.Runtime.Serialization;
// Note: When building this code, you must reference the
// System.Runtime.Serialization.Formatters.Soap.dll assembly.
using System.Runtime.Serialization.Formatters.Soap;

namespace IndianBridge.Common
{
    public class Utility
    {
        const string PRE_START_TAG = "<PRE>";
        const string PRE_END_TAG = "</PRE>";
        const string TABLE_START_TAG = "<TABLE";
        const string TABLE_END_TAG = "</TABLE>";

        public static string ToCamelCase(string input)
        {
            return String.Format("{0}{1}", input.Substring(0, 1), input.Substring(1).ToLower());
        }

        // Given row text and a start position, find the next field in the row
        public static string GetField(string rowText, ref int startPosition, string separator = " ")
        {
            startPosition = findWhiteSpace(rowText, startPosition);

            // Skip past whitespace and find the score
            startPosition += skipWhiteSpace(rowText, startPosition);

            return rowText.Substring(startPosition, rowText.IndexOf(separator, startPosition) - startPosition);
        }

        public static int skipWhiteSpace(string text, int startPosition)
        {
            int i = 0;
            // Skip past whitespace characters
            while (text.Substring(startPosition + i, 1) == " ")
            {
                i++;
            }
            return i;
        }

        public static int findWhiteSpace(string text, int startPosition)
        {
            return text.IndexOf(" ", startPosition);
        }

        public static string GetDataFromHtmlTable(string fileName, int tableIndex, bool preFormatted)
        {
            bool success;
            string html = Utility.ReadFile(fileName, out success);

            if (!success)
            {
                Console.WriteLine("GetDataFromHtmlTable failed - unable to read input file");
                return String.Empty;
            }

            string startTag, endTag;
            int startPosition, endPosition;
            startTag = preFormatted ? PRE_START_TAG : TABLE_START_TAG;
            endTag = preFormatted ? PRE_END_TAG : TABLE_END_TAG;

            // Find the first occurrence
            startPosition = html.IndexOf(startTag, StringComparison.InvariantCultureIgnoreCase);

            if (startPosition < 0) return null;
            endPosition = html.IndexOf(endTag, startPosition);

            // Then keep finding the next occurrence until we get to the correct table we want
            for (int i = 0; i < tableIndex; i++)
            {
                startPosition = html.IndexOf(startTag, endPosition, StringComparison.InvariantCultureIgnoreCase);
                endPosition = html.IndexOf(endTag, startPosition, StringComparison.InvariantCultureIgnoreCase);
            }
            endPosition += endTag.Length;

            string tableHtml = html.Substring(startPosition, endPosition - startPosition);

            return tableHtml;
        }

        public static string ReadFile(string fileName, out bool success, bool ignoreError = false)
        {
            string content = String.Empty;
            StreamReader fileStream = null;
            success = true;

            try
            {
                fileStream = new StreamReader(fileName);
                content = fileStream.ReadToEnd();
            }
            catch (Exception exc)
            {
                if (!ignoreError)
                {
                    Console.WriteLine(String.Format("Unable to read file {0}: {1}", fileName, exc.Message));
                }
                success = false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            return content;
        }

        public static void WriteFile(string fileName, string content)
        {
            StreamWriter fileStream = new StreamWriter(fileName);
            fileStream.Write(content);
            fileStream.Close();
        }

        public static string GetTimeStamp()
        {
            return String.Format("{0} IST", DateTime.Now.ToString());
        }

        public static void UpdateButlerResults(DataTable butlerResults, string pair, decimal score, int boards = 1)
        {
            // Update butler results table for the 2 pairs in question
            DataRow[] butlerRows = butlerResults.Select(String.Format("Pair='{0}'", pair));
            DataRow butlerRow;
            if (butlerRows.Length > 0)
            {
                butlerRow = butlerRows[0];
                butlerRow["Boards"] = Convert.ToInt16(butlerRow["Boards"]) + boards;
                butlerRow["Score"] = Convert.ToDecimal(butlerRow["Score"]) + score;
                butlerRow.AcceptChanges();
            }
            else
            {
                butlerRow = butlerResults.NewRow();
                butlerRow["Pair"] = pair;
                butlerRow["Boards"] = boards;
                butlerRow["Score"] = score;
                butlerResults.Rows.Add(butlerRow);
            }
        }

        // Function to serialize a NameValueCollection to a file
        public static void Serialize(NameValueCollection names, String filename, Boolean debug=false)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);

            // Construct a SoapFormatter and use it 
            // to serialize the data to the stream.
            SoapFormatter formatter = new SoapFormatter();
            try
            {
                formatter.Serialize(fs, names);
            }
            catch (SerializationException e)
            {
                if(debug) Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        // Function to Deserialize from a file to a NameValueCollection
        public static NameValueCollection Deserialize(String filename, Boolean debug = false)
        {
            // Declare the NameValueCollection reference.
            NameValueCollection names = null;

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(filename, FileMode.Open);
            try
            {
                SoapFormatter formatter = new SoapFormatter();

                // Deserialize the NameValueCollection from the file and 
                // assign the reference to the local variable.
                names = (NameValueCollection)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                if(debug) Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return names;
        }
    }
}
