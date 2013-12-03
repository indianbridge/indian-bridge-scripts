using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordpressAPIs;
using IndianBridge.Common;

namespace BFIAddTourney
{
    public partial class ValidateCredentials : Form
    {
        public AddTourneys addTourneys;
        public TourneyList tourneyList = null;
        public ValidateCredentials()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTextbox.Text))
            {
                MessageBox.Show("Username cannot be empty string!");
                return;
            }
            if (string.IsNullOrWhiteSpace(passwordTextbox.Text))
            {
                MessageBox.Show("Password cannot be empty string!");
                return;
            }
            loginPanel.Enabled = false;
            loadingPicture.Enabled = true;
            loadingPicture.Visible = true;
            loadingPicture.BringToFront();
            this.Refresh();
            addTourneys = new AddTourneys("http://127.0.0.1/bfi", usernameTextbox.Text, passwordTextbox.Text);
            string json_result = addTourneys.getTourneys();
            try
            {
                tourneyList = Utilities.JsonDeserialize<TourneyList>(json_result);
                loadingPicture.Visible = false;
                loginPanel.Enabled = true;
                if (tourneyList.error)
                {
                    MessageBox.Show("Unable to validate username and password because : " + Environment.NewLine + tourneyList.message);
                    return;
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + Environment.NewLine + ex.Message);
                loadingPicture.Visible = false;
                loginPanel.Enabled = true;
                return;
            }
        }
    }
}
