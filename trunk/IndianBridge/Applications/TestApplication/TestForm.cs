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
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Column1");
            column.DataType = Type.GetType("System.String");
            column.AllowDBNull = false;
            table.Columns.Add(column);
            column = new DataColumn("Column2");
            column.DataType = Type.GetType("System.String");
            column.AllowDBNull = false;
            column.DefaultValue = "Default";
            table.Columns.Add(column);
            column = new DataColumn("Column3");
            column.DataType = Type.GetType("System.String");
            column.AllowDBNull = true;
            table.Columns.Add(column);
            DataRow row = table.NewRow();
            //row["Column1"] = "Value1";
            table.Rows.Add(row);
            /*if (openExcelFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                string filePath = openExcelFileDialog.FileName;
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                if (filePath.EndsWith(".xls"))
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (filePath.EndsWith(".xlsx"))
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    MessageBox.Show("Unknown Extension in file : " + filePath);
                    return;
                }
                DataSet result = excelReader.AsDataSet();
                excelReader.Close();
                string csvData = "";
                int row_no = 0;
                int ind = 0;
                while (row_no < result.Tables[ind].Rows.Count) // ind is the index of table
                // (sheet name) which you want to convert to csv
                {
                    for (int i = 0; i < result.Tables[ind].Columns.Count; i++)
                    {
                        csvData += result.Tables[ind].Rows[row_no][i].ToString() + ",";
                    }
                    row_no++;
                    csvData += System.Environment.NewLine;
                }
                textBox1.Text = csvData;
                MessageBox.Show("Parsed");
            }*/
        }
    }
}
