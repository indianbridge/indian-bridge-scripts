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
    public partial class GetGoogleWebsiteRoot : Form
    {
        public string websiteRoot = "";
        public GetGoogleWebsiteRoot(string eventName)
        {
            InitializeComponent();
            websiteRootTextbox.Text = "https://sites.google.com/site/" + eventName + "/results";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            websiteRoot = websiteRootTextbox.Text;
            this.Close();
        }

    }
}
