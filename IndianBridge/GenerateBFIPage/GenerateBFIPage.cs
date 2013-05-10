using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GenerateBFIPage
{
    public partial class GenerateBFIPage : Form
    {
        string[] m_startTags = { "", "[one_half]", "[one_third]", "[one_fourth]", "[one_fifth]", "[one_sixth]" };
        string[] m_stopTags = { "", "[/one_half]", "[/one_third]", "[/one_fourth]", "[/one_fifth]", "[/one_sixth]" };
        int m_numColumns = 2;
        public GenerateBFIPage()
        {
            InitializeComponent();
        }

        private DataTable readCSVFile()
        {
            DialogResult result = SelectCSVFile.ShowDialog();
            if (result != DialogResult.OK) {
                MessageBox.Show("Cancelled");
                return null;
            }
            string fileName = SelectCSVFile.FileName;
            string line;
            DataTable dt = new DataTable();
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(fileName);

                //Read the first line of text
                line = sr.ReadLine();
                foreach (string columnName in line.Split('\t'))
                {
                    dt.Columns.Add(columnName);
                }

                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    dt.Rows.Add(line.Split('\t'));
                    //Read the next line
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message);
                return null;
            }

            return dt;

        }

        private void generateState(DataTable dt)
        {
            m_numColumns = Convert.ToInt16(NumberOfColumnsComboBox.Text);
            string shortcodeText = preamble("BFI State Association Contact Information");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                shortcodeText += startTag();
                shortcodeText += getImageLink(row, "State", "Photo", "wp-content/uploads/files/india_map.gif");
                shortcodeText += clear();
                shortcodeText += getField(row, "President", "user_business.png", false, "President");
                shortcodeText += getField(row, "Secretary", "user.png", false, "Secretary");
                shortcodeText += getEmailLink(row, "Email");
                shortcodeText += getField(row, "Contact No", "mobile_phone.png", false);
                shortcodeText += stopTag();
                shortcodeText += clearEndOfRow(i);
            }
            shortcodeText += postamble();
            shortCodeTextBox.Text = shortcodeText;
        }

        private void generateDirectorsList(string title, DataTable dt)
        {
            m_numColumns = Convert.ToInt16(NumberOfColumnsComboBox.Text);
            string shortcodeText = preamble(title);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                shortcodeText += startTag();
                shortcodeText += getImageLink(row, "Member", "Photo", "wp-content/uploads/files/images/mystery-man-bpfull.jpg");
                shortcodeText += clear();
                shortcodeText += getField(row, "Tournament Level", "user.png", true);
                shortcodeText += getField(row, "Region", "map.png", false);
                shortcodeText += getEmailLink(row, "Email");
                shortcodeText += getField(row, "Phone", "mobile_phone.png", false);
                shortcodeText += getField(row, "Address", "house.png", false);
                shortcodeText += getField(row, "Bio", "help.png", false);
                shortcodeText += stopTag();
                shortcodeText += clearEndOfRow(i);
            }
            shortcodeText += postamble();
            shortCodeTextBox.Text = shortcodeText;
        }

        private void generateOfficeBearersList(DataTable dt)
        {
            m_numColumns = Convert.ToInt16(NumberOfColumnsComboBox.Text);
            string shortcodeText = preamble("BFI Executive Committee");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                shortcodeText += startTag();
                shortcodeText += getImageLink(row, "Name", "Photo", "wp-content/uploads/files/images/mystery-man-bpfull.jpg");
                shortcodeText += clear();
                shortcodeText += getField(row, "Designation", "user.png", true);
                shortcodeText += getEmailLink(row, "Email");
                shortcodeText += getField(row, "Phone", "mobile_phone.png", false);
                shortcodeText += getField(row, "Address", "house.png", false);
                shortcodeText += getField(row, "Bio", "help.png", false);
                shortcodeText += stopTag();
                shortcodeText += clearEndOfRow(i);
            }
            shortcodeText += postamble();
            shortCodeTextBox.Text = shortcodeText;
        }

        private string preamble(string title)
        {
            string shortcodeText = "<div class=\"gray-contentbox\">" + System.Environment.NewLine + "[clear h=\"20\"]" + System.Environment.NewLine;
            shortcodeText += "<h3>"+title+"</h3>" + System.Environment.NewLine;
            return shortcodeText;
        }

        private string postamble()
        {
            return System.Environment.NewLine + "[clear h=\"20\"]" + System.Environment.NewLine + System.Environment.NewLine + "</div>";
        }

        private string clearEndOfRow(int itemNumber)
        {
            return ((itemNumber + 1) % m_numColumns == 0) ? clear() : "";
        }

        private string startTag()
        {
            string currentStartTag = m_startTags[m_numColumns - 1];
            return System.Environment.NewLine + currentStartTag + System.Environment.NewLine;
        }

        private string stopTag()
        {
            string currentStopTag = m_stopTags[m_numColumns - 1];
            return System.Environment.NewLine + currentStopTag + System.Environment.NewLine;
        }

        private string clear()
        {
            return System.Environment.NewLine + "[clear]" + System.Environment.NewLine;
        }

        private string getEmailLink(DataRow row, string fieldName)
        {
            string shortcodeText = "";
            string email = row.Table.Columns.Contains(fieldName) ? (string)row[fieldName] : "";
            if (!string.IsNullOrWhiteSpace(email))
                shortcodeText += System.Environment.NewLine + "[email align=\"left\" to=\"" + email + "\"]" + email + "[/email]" + System.Environment.NewLine;
            return shortcodeText;
        }

        private string getImageLink(DataRow row, string nameFieldName, string photoFieldName, string imagePrefix)
        {
            string shortcodeText = "";
            string imageLink = row.Table.Columns.Contains(photoFieldName) ? (string)row[photoFieldName] : "";
            if (string.IsNullOrWhiteSpace(imageLink)) imageLink = websitePrefixTextBox.Text + imagePrefix;
            string name = row.Table.Columns.Contains(nameFieldName) ? (string)row[nameFieldName] : "Unknown Name";
            shortcodeText += System.Environment.NewLine + "[img align=\"left\" src=\"" + imageLink + "\" w=\"110\" h=\"80\" title=\"<strong>" + name + "</strong>\" alt=\"" + name + "\"]" + System.Environment.NewLine;
            return shortcodeText;
        }

        private string getField(DataRow row, string fieldName, string iconName, bool strong=false,string text="")
        {
            string shortcodeText = "";
            if (!row.Table.Columns.Contains(fieldName)) return "";
            string field = (string)row[fieldName];
            if (!string.IsNullOrWhiteSpace(field))
                shortcodeText += System.Environment.NewLine + "[icon name=\"" + iconName + "\"]" + (strong ? "<strong>" : "") + (string.IsNullOrWhiteSpace(text)?"":text+" : ") + field + (strong ? "</strong>" : "") + "[/icon]" + System.Environment.NewLine;
            return shortcodeText;
        }

        private void GenerateShortcodeButton_Click(object sender, EventArgs e)
        {
            DataTable dt = readCSVFile();
            if (dt != null)
            {
                generateOfficeBearersList(dt);
                shortCodeTextBox.SelectAll();
            }
        }

        private void GenerateStateButton_Click(object sender, EventArgs e)
        {
            DataTable dt = readCSVFile();
            if (dt != null)
            {
                generateState(dt);
                shortCodeTextBox.SelectAll();
            }
        }

        private void GenerateDirectorsListButton_Click(object sender, EventArgs e)
        {
            DataTable dt = readCSVFile();
            if (dt != null)
            {
                generateDirectorsList("BFI Certified Directors List", dt);
                shortCodeTextBox.SelectAll();
            }
        }

        private void GeneratePotentialDirectorsListButton_Click(object sender, EventArgs e)
        {
            DataTable dt = readCSVFile();
            if (dt != null) {
                generateDirectorsList("Potential Directors List (most of them attended Goa Seminar)", dt);
                shortCodeTextBox.SelectAll();
            }

        }

    }


}
