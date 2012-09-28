using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using System.IO;

namespace IndianBridgeScorer
{
    public partial class SetTourneyInfo : Form
    {
        public SetTourneyInfo()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            TourneyInfo tourneyInfo = new TourneyInfo(Constants.getRootTourneyInformationFile(), true);
            tourneyInfoPropertyGrid.SelectedObject = tourneyInfo;
        }

        private void setupTourneyButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
