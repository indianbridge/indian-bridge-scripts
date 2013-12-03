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
using IndianBridge.Common;

namespace BFIAddTourney
{
    public partial class BFIAddTourney : Form
    {
        AddTourneys addTourneys;
        TourneyList tourneyList = null;
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
            tourneyList = vc.tourneyList;
            loadTourneyNames(tourneyList);
        }

        private void loadTourneyNames(TourneyList tourneyList)
        {
            foreach (TourneyPageInfo pageInfo in tourneyList.content)
            {
                tourneyNamesCombobox.Items.Add(pageInfo.title);
            }
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
            AddPagesList addPagesList = new AddPagesList();
            addPagesList.parentPageID = Convert.ToInt32(tourneyList.content[tourneyNamesCombobox.SelectedIndex].id);
            addPagesList.tourneyYear = Convert.ToInt32(tourneyYearCombobox.Text);
            addPagesList.tourneyPages = new List<NewPageInfo>();
            string message = "Creating " + tourneyNamesCombobox.Text + " for year " + tourneyYearCombobox.Text + " with following pages :"+Environment.NewLine;
            foreach (DataGridViewRow row in pageNamesDataGridView.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string value = (string)row.Cells[0].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        NewPageInfo item = new NewPageInfo();
                        item.title = value;
                        message += value + ", ";
                        item.content = "";
                        addPagesList.tourneyPages.Add(item);
                    }
                }
            }
            string content = Utilities.JsonSerialize<AddPagesList>(addPagesList);
            DialogResult dResult = MessageBox.Show(message,"Are you Sure?",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dResult == DialogResult.Yes)
            {
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(message+Environment.NewLine);
                try
                {
                    splitContainer1.Enabled = false;
                    string json_result = addTourneys.addTourney(content);
                    AddTourneyPageReturnValue returnValue = Utilities.JsonDeserialize<AddTourneyPageReturnValue>(json_result);
                    if (returnValue.error) {
                        string errorMessage = "Errors when adding tourney pages : "+Environment.NewLine+returnValue.message;
                        Utilities.showErrorMessage(errorMessage);
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.AppendText(errorMessage + Environment.NewLine);
                    }
                    else {
                        string completedMessage = "Completed adding tourney and pages";
                        MessageBox.Show(completedMessage,"Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        richTextBox1.SelectionColor = Color.Black;
                        richTextBox1.AppendText(completedMessage + Environment.NewLine);
                        foreach(AddTourneyPageInfo pageInfo in returnValue.content) {
                            if (pageInfo.error)
                            {
                                richTextBox1.SelectionColor = Color.Red;
                                richTextBox1.AppendText(pageInfo.message + Environment.NewLine);
                            }
                            else
                            {
                                richTextBox1.SelectionColor = Color.Green;
                                richTextBox1.AppendText(pageInfo.message + Environment.NewLine);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception : " + ex.Message);
                }
                splitContainer1.Enabled = true;
            }
        }

    }
}
