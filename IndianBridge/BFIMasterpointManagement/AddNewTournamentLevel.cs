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
            if (string.IsNullOrWhiteSpace(typeTextbox.Text))
            {
                MessageBox.Show("Tournament Type cannot be empty string!");
                return;
            }
            this.loginPanel.Enabled = false;
            this.loadingPicture.Enabled = true;
            this.loadingPicture.Visible = true;
            this.loadingPicture.BringToFront();
            this.Refresh();
            TournamentLevelInfo tournamentLevelInfo = new TournamentLevelInfo();
            tournamentLevelInfo.tournament_level_code = levelCodeTextbox.Text;
            tournamentLevelInfo.description = descriptionTextbox.Text;
            tournamentLevelInfo.tournament_type = typeTextbox.Text;
            string json_result = m_mm.addTournamentLevel(tournamentLevelInfo);
            Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            if (errorStatus)
            {
                MessageBox.Show("Error when trying to add Tournament Level because : " + result["content"], "Error adding Table Data !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(result["content"], "Add Tournament Level Success");
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
