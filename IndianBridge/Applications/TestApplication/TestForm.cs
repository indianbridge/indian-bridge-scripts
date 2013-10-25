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

namespace IndianBridge
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs ea)
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

        private void button2_Click(object sender, EventArgs e)
        {
            string value = textBox1.Text;
            MessageBox.Show(Utilities.getNextCode(value));
        }
    }
}
