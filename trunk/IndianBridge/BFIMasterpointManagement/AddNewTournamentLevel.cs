using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.WordpressAPIs;
using System.Web.Script.Serialization;

namespace BFIMasterpointManagement
{
    public partial class AddNewTournamentLevel : Form
    {
        public ManageMasterpoints m_mm;
        public string newRowCSV;
        public AddNewTournamentLevel(ManageMasterpoints mm)
        {
            m_mm = mm;
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(levelCodeTextbox.Text))
            {
                MessageBox.Show("Level Code cannot be empty string!");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextbox.Text))
            {
                MessageBox.Show("Description cannot be empty string!");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextbox.Text))
            {
                MessageBox.Show("Description cannot be empty string!");
                return;
            }
            this.loginPanel.Enabled = false;
            this.loadingPicture.Enabled = true;
            this.loadingPicture.Visible = true;
            this.loadingPicture.BringToFront();
            this.Refresh();
            TableInfo tableInfo = new TableInfo();
            tableInfo.tableName = "bfi_tournament_level_master";
            string delimiter = ",";
            string content = "tournament_level_code" + delimiter + "description" + delimiter + "tournament_type"+Environment.NewLine;
            newRowCSV = levelCodeTextbox.Text + delimiter + descriptionTextbox.Text + delimiter + typeTextbox.Text;
            content += newRowCSV;
            tableInfo.content = content;
            tableInfo.delimiter = delimiter;
            string json_result = m_mm.addTableData(tableInfo);
            var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
            Dictionary<string, string> result = serializer.Deserialize<Dictionary<string, string>>(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Error when trying to add table data because : " + result["content"], "Error adding Table Data !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result["content"], "Add Table Data Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            this.loadingPicture.Visible = false;
            this.loginPanel.Enabled = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
