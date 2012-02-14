
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Collections;
using System.Reflection;

namespace IndianBridge.Common
{
        public static class Utilities
        {

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
