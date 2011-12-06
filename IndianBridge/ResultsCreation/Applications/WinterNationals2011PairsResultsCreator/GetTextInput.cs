using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinterNationals2011PairsResultsCreator
{
    public partial class GetTextInput : Form
    {
        private string m_fieldName;
        public GetTextInput(string fieldName)
        {
            m_fieldName = fieldName;
            InitializeComponent();
            this.Text = "Get " + m_fieldName + " Value";
            GetTextInputLabel.Text = "Provide a value for " + m_fieldName;
        }
        public string getFieldValue() { return GetTextInputTextBox.Text; }

        private void GetTextInputButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GetTextInputTextBox.Text)) MessageBox.Show(m_fieldName + " is required to continue!!!");
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
