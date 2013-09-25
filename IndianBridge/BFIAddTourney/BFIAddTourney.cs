using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordpressAPIs;
using System.Web.Script.Serialization;

namespace BFIAddTourney
{
    public partial class BFIAddTourney : Form
    {
        AddTourneys addTourneys;
        string[] tourneyNames;
        string[] tourneyPageIDs;
        public BFIAddTourney()
        {
            InitializeComponent();
        }

        private void validateCredentials(bool closeOnError)
        {
            ValidateCredentials vc = new ValidateCredentials();
            vc.StartPosition = FormStartPosition.CenterParent;
            if (vc.ShowDialog(this) == DialogResult.Cancel)
            {
                if (closeOnError) this.Close();
                return;
            }
            addTourneys = vc.addTourneys;
            loadTourneyNames(vc.tourneyList);
        }

        private void loadTourneyNames(string tourneyList)
        {
            string[] tokens = tourneyList.Split('#');
            tourneyNames = tokens[0].Split(',');
            tourneyPageIDs = tokens[1].Split(',');
            tourneyNamesCombobox.Items.AddRange(tourneyNames);
            tourneyNamesCombobox.SelectedIndex = 0;
            pageNamesDataGridView.Rows.Add("Information");
            pageNamesDataGridView.Rows.Add("Programme");
            pageNamesDataGridView.Rows.Add("People");
            pageNamesDataGridView.Rows.Add("Results");
            pageNamesDataGridView.Rows.Add("Bulletins");

        }

        private void BFIAddTourney_Load(object sender, EventArgs e)
        {
            validateCredentials(true);
            int currentYear = DateTime.Today.Year;
            tourneyYearCombobox.Items.Clear();
            tourneyYearCombobox.Items.Add(currentYear);
            tourneyYearCombobox.Items.Add(currentYear + 1);
            tourneyYearCombobox.SelectedIndex = 0;
        }

        private void createPagesButton_Click(object sender, EventArgs e)
        {
            string pageNames = "";
            string message = "Creating " + tourneyNamesCombobox.Text + " for year " + tourneyYearCombobox.Text + " with following pages :"+Environment.NewLine;
            foreach (DataGridViewRow row in pageNamesDataGridView.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string value = (string)row.Cells[0].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        pageNames += value+",";
                        message += value+Environment.NewLine;
                    }
                }
            }
            pageNames = pageNames.TrimEnd(',');
            DialogResult dResult = MessageBox.Show(message,"Are you Sure?",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dResult == DialogResult.Yes)
            {
                splitContainer1.Enabled = false;
                TourneyInfo tourneyInfo = new TourneyInfo();
                tourneyInfo.parentPageID = Convert.ToInt32(tourneyPageIDs[tourneyNamesCombobox.SelectedIndex]);
                tourneyInfo.tourneyYear = Convert.ToInt32(tourneyYearCombobox.Text);
                tourneyInfo.tourneyPages = pageNames;
                string json_result = addTourneys.addTourney(tourneyInfo);
                var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
                Dictionary<string, string> result = serializer.Deserialize<Dictionary<string, string>>(json_result);
                bool errorStatus = Convert.ToBoolean(result["error"]);
                if (errorStatus) {
                    MessageBox.Show("Errors when adding tourney : "+Environment.NewLine+result["message"],"Errors!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else {
                    MessageBox.Show("Successfully added tourney and pages : "+Environment.NewLine+result["message"],"Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                splitContainer1.Enabled = true;
            }
        }
    }
}
