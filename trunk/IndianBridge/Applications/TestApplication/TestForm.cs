using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;
using Microsoft.VisualBasic;
using IndianBridge.GoogleAPIs;
using IndianBridge.Common;
using System.IO;
using System.Net;
using FtpLib;
using Excel;
using System.Text.RegularExpressions;
using IndianBridge.WordpressAPIs;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace IndianBridge
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

        }

        private void sendViaFTP()
        {
            string _remoteHost = "ftp.bfi.net.in";
            string _remoteUser = "bfi@bfi.net.in";
            string _remotePass = "bfi";
            string source = @"C:\Users\snarasim\Downloads\test.pdf";
            string destination = "/test.pdf";
            using (FtpConnection ftp = new FtpConnection(_remoteHost, _remoteUser, _remotePass))
            {
                try
                {
                    ftp.Open(); // Open the FTP connection 
                    ftp.Login(); // Login using previously provided credentials
                    ftp.PutFile(source, destination);
                    MessageBox.Show("Done");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void sendFileViaFTPButton_Click(object sender, EventArgs e)
        {
            sendViaFTP();
        }

        private void connectToSQLButton_Click(object sender, EventArgs e)
        {
            try
            {
                string server = "localhost";
                string database = "bfinem7l_bfi";
                string uid = "indianbridge";
                string password = "kibitzer";
                string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                MySqlConnection myConnection = new MySqlConnection(connectionString);
                myConnection.Open();
                MySqlDataReader myReader = null;
                MySqlCommand myCommand = new MySqlCommand("select * from bfi_event_master",myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    textBox1.AppendText(myReader["event_code"].ToString()+Environment.NewLine);
                    textBox1.AppendText(myReader["description"].ToString()+Environment.NewLine);
                }
                myConnection.Close();
            }
            catch (Exception ex)
            {
                Utilities.showErrorMessage(ex.ToString());
            }
        }

    }
}
