using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BridgeTeamEventScoreManager
{
    public partial class FileOverWriteConfirm : Form
    {
        public FileOverWriteConfirm(String message)
        {
            InitializeComponent();
            overwriteMessageTextBox.Text = message;
        }

        private void overwriteCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void overwriteButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
