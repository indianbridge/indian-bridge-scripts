using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IndianBridgeScorer
{
    public partial class SetTourneyInfo : Form
    {
        public Boolean cancelPressed = true;
        public string tourneyName = "";
        public string resultsWebsiteRoot = "";
        public SetTourneyInfo()
        {
            InitializeComponent();
        }

        private void setupTourneyButton_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tourneyNameTextBox.Text)) {
                MessageBox.Show("Event Name cannot be empty!","Invalid Event Name",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(resultsWebsiteRootTextBox.Text)) {
                MessageBox.Show("Results website root page has not been provided!"+Environment.NewLine+"You will be asked for a website when you try to publish results for any event.","Missing Results Website!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            cancelPressed = false;
            tourneyName = tourneyNameTextBox.Text;
            resultsWebsiteRoot = resultsWebsiteRootTextBox.Text;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancelPressed = true;
            this.Close();
        }
    }
}
