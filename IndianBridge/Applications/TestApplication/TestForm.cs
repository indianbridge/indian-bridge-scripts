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

namespace IndianBridge
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Opacity = 0.75;
            LoadingEvents le = new LoadingEvents();
            le.StartPosition = FormStartPosition.CenterParent;
            le.ShowDialog();
            this.Enabled = true;
            this.Opacity = 1;
            /*if (folderBrowserDialog1.ShowDialog() != DialogResult.Cancel)
            {
                DocsAPI da = new DocsAPI("indianbridge.dummy@gmail.com","kibitzer");
                Dictionary<string, string> fileFilters = new Dictionary<string, string>();
                fileFilters.Add(".ini", "text/plain");
                fileFilters.Add(".mdb", "application/msaccess");
                //da.downloadGoogleDocsToDirectory(Path.Combine("Indian Bridge Scorer", "Tourneys", "Test_2012_10_02"), folderBrowserDialog1.SelectedPath, fileFilters);
                m_cbw = new CustomBackgroundWorker("Upload Google Docs", da.DownloadGoogleDocsToDirectoryInBackground, uploadCompleted, operationStatus,
                        operationProgressBar, operationCancelButton, null);
                Tuple<string, string, Dictionary<string, string>> values = new Tuple<string, string, Dictionary<string, string>>(Path.Combine("Indian Bridge Scorer", "Tourneys", "Test_2012_10_02"), folderBrowserDialog1.SelectedPath, fileFilters);
                m_cbw.run(values);
            }*/

        }

        private void uploadCompleted(bool success)
        {
            MessageBox.Show("Done");
        }

        private string getMimeType(string fileName)
        {
            string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(fileExtension);
            if (rk != null && rk.GetValue("Content Type") != null)
                return rk.GetValue("Content Type").ToString();
            return null;
        }
    }
}
