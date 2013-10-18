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
    public partial class ValidateCredentials : Form
    {
        public AddTourneys addTourneys;
        public string tourneyList = "";
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
            Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
            bool errorStatus = Convert.ToBoolean(result["error"]);
            loadingPicture.Visible = false;
            loginPanel.Enabled = true;
            if (!errorStatus)
            {
                this.DialogResult = DialogResult.OK;
                tourneyList = result["content"];
                this.Close();
            }
            else
            {
                MessageBox.Show("Unable to validate username and password because : " + Environment.NewLine + result["message"]);
                return;
            }
        }
    }
}
