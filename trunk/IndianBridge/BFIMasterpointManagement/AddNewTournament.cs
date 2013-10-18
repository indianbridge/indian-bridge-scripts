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
using IndianBridge.Common;

namespace BFIMasterpointManagement
{
    public partial class AddNewTournament : Form
    {
        public ManageMasterpoints m_mm;
        public AddNewTournament(ManageMasterpoints mm,DataGridView tlm, string tournament_level_code)
        {
            m_mm = mm;
            InitializeComponent();
            int colIndex = tlm.Columns["tournament_level_code"].Index;
            foreach (DataGridViewRow row in tlm.Rows)
            {
                tournamentLevelCombobox.Items.Add((string)(row.Cells[colIndex].Value));
            }
            if (tournament_level_code == null) tournamentLevelCombobox.SelectedIndex = 0;
            else tournamentLevelCombobox.Text = tournament_level_code;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tournamentCodeTextbox.Text))
            {
                MessageBox.Show("Tournament Code cannot be empty string!");
                return;
            }
            if (string.IsNullOrWhiteSpace(descriptionTextbox.Text))
            {
                MessageBox.Show("Description cannot be empty string!");
                return;
            }
            if (string.IsNullOrWhiteSpace(tournamentLevelCombobox.Text))
            {
                MessageBox.Show("Tournament Level cannot be empty string!");
                return;
            }
            this.loginPanel.Enabled = false;
            this.loadingPicture.Enabled = true;
            this.loadingPicture.Visible = true;
            this.loadingPicture.BringToFront();
            this.Refresh();
            TournamentInfo tournamentInfo = new TournamentInfo();
            tournamentInfo.tournament_code = tournamentCodeTextbox.Text;
            tournamentInfo.description = descriptionTextbox.Text;
            tournamentInfo.tournament_level_code = tournamentLevelCombobox.Text;

            string json_result = m_mm.addTournament(tournamentInfo);
            Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Error when trying to add tournament because : " + result["content"], "Error adding tournament !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result["content"], "Add Tournament Success");
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
