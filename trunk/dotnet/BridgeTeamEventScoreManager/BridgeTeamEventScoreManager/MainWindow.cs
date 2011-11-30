using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace BridgeTeamEventScoreManager
{
    public partial class MainWindow : Form
    {
        private SitesAPI sa = new SitesAPI();
        OleDbConnection myAccessConn = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void updateGoogleClientLoginStatus()
        {
            if (sa.getLoginStatus())
            {
                loginStatusTextBox.Text = "Logged In.";
                loginStatusTextBox.ForeColor = Color.Green;
            }
            else
            {
                loginStatusTextBox.Text = "Not Logged In.";
                loginStatusTextBox.ForeColor = Color.Red;
            }
            if (sa.getAuthenticationStatus())
            {
                authenticationStatusTextBox.Text = "Authenticated.";
                authenticationStatusTextBox.ForeColor = Color.Green;
            }
            else
            {
                authenticationStatusTextBox.Text = "Not Authenticated.";
                authenticationStatusTextBox.ForeColor = Color.Red;
            }
            errorMessageTextBox.Text = sa.getErrorMessage();
        }

        private void statusTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginLogoutButton_Click(object sender, EventArgs e)
        {
            sa.ShowDialog();
            updateGoogleClientLoginStatus();
        }

        private void changeDatabaseButton_Click(object sender, EventArgs e)
        {
            if (selectDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedDatabaseTextBox.Text = selectDatabaseFileDialog.FileName;
                checkDatabaseValidity(selectedDatabaseTextBox.Text);
            }
        }

        private void createNewDatabaseButton_Click(object sender, EventArgs e)
        {
            String sourceFileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BridgeTeamEventScoreTemplate.mdb");
            selectDatabaseFileDialog.CheckFileExists = false;
            if (selectDatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedDatabaseTextBox.Text = selectDatabaseFileDialog.FileName;
                if (System.IO.File.Exists(selectedDatabaseTextBox.Text))
                {
                    FileOverWriteConfirm fd = new FileOverWriteConfirm(selectedDatabaseTextBox.Text + " already exists!!! OverWrite?");
                    if (fd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.Copy(sourceFileName, selectedDatabaseTextBox.Text);
                        checkDatabaseValidity(selectedDatabaseTextBox.Text);
                    }
                }
                else
                {
                    System.IO.File.Copy(sourceFileName, selectedDatabaseTextBox.Text);
                    checkDatabaseValidity(selectedDatabaseTextBox.Text);
                }
            }
            selectDatabaseFileDialog.CheckFileExists = true;
        }
        private Boolean checkDatabaseValidity(String filePath)
        {
#if USINGPROJECTSYSTEM
		string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+filePath;
#else
            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+filePath+";";
#endif
            myAccessConn = new OleDbConnection(strAccessConn);
            myAccessConn.Open();
            string strAccessSelect = "SELECT * FROM Scores";
            try
            {
                OleDbCommand cm = new OleDbCommand(strAccessSelect, myAccessConn);
                OleDbDataReader rd;
                rd = cm.ExecuteReader();
            }
            catch (Exception e)
            {
                textBox1.Text = e.ToString();
                return true;
            }
            textBox1.Text = "Success";
            return false;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}