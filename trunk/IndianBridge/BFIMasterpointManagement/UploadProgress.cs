using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.Common;
using IndianBridge.WordpressAPIs;

namespace BFIMasterpointManagement
{
    public partial class UploadProgress : Form
    {
        public string statusText;
        public bool errorFound;
        public string m_content;
        ManageMasterpoints m_mm;
        int m_incrementSize = 25;
        enum Operation {
            UploadUsers,
            UploadMasterpoints,
            DeleteUsers
        };
        Operation m_currentOperation;
        public UploadProgress(ManageMasterpoints mm)
        {
            m_mm = mm;                     
            InitializeComponent();
        }


        private void startOperation(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int maxLinesToUploadPerTry = m_incrementSize;
            string newLineCharacter = Utilities.getNewLineCharacter(m_content);
            string[] lines = m_content.Split(new string[] { newLineCharacter }, StringSplitOptions.RemoveEmptyEntries);
            worker.ReportProgress(0, new Tuple<bool,string>(false,"Total Records to processed = " + (lines.Length - 1)+Environment.NewLine));
            string headerLine = lines[0];
            int processedLines = 1;
            while (processedLines < lines.Length)
            {
                int startIndex = processedLines;
                int endIndex = startIndex + maxLinesToUploadPerTry - 1;
                if (endIndex >= lines.Length) endIndex = lines.Length - 1;
                processedLines = endIndex + 1;
                int progress = (int)Math.Floor(startIndex * 100.0 / (lines.Length - 1));
                worker.ReportProgress(progress, new Tuple<bool,string>(false,"Processing entries from " + startIndex + " to " + endIndex + Environment.NewLine));
                TableInfo tableInfo = new TableInfo();
                string[] partialContent = Utilities.arraySlice(lines, startIndex, endIndex);
                tableInfo.content = headerLine + newLineCharacter + string.Join(newLineCharacter, partialContent);
                tableInfo.delimiter = ",";
                try
                {
                    string json_result ="";
                    switch(m_currentOperation) {
                        case Operation.UploadUsers:
                        json_result = m_mm.addUsers(tableInfo);
                        break;
                        case Operation.UploadMasterpoints:
                        json_result = m_mm.addMasterpoints(tableInfo);
                        break;
                        case Operation.DeleteUsers:
                        json_result = m_mm.deleteUsers(tableInfo);
                        break;
                        default:
                        MessageBox.Show("Upload Progress Unknown Operation");
                        return;
                    }
                    progress = (int)Math.Floor(endIndex*100.0 / (lines.Length - 1));
                    Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
                    bool errorStatus = Convert.ToBoolean(result["error"]);
                    result["content"] = result["content"].Replace(Utilities.getNewLineCharacter(result["content"]), System.Environment.NewLine);
                    worker.ReportProgress(progress, new Tuple<bool,string>(errorStatus,result["message"] + Environment.NewLine + result["content"]));
                }
                catch (Exception ex)
                {
                    progress = (int)Math.Floor(endIndex * 100.0 / (lines.Length - 1));
                    worker.ReportProgress(endIndex / lines.Length, new Tuple<bool,string>(true,ex.Message));
                }
            }
        }

        private void operationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            uploadProgressBar.Value = 100;
            statusMessageTextbox.AppendText("Completed Processing");
            statusText = statusMessageTextbox.Text;
            MessageBox.Show("Completed Processing. See Textbox for details");
            this.Close();
        }

        private void operationProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            uploadProgressBar.Value = e.ProgressPercentage;
            Tuple<bool, string> value = e.UserState as Tuple<bool, string>;
            errorFound = errorFound | value.Item1;
            if (errorFound) statusMessageTextbox.ForeColor = Color.Red;
            else statusMessageTextbox.ForeColor = Color.Green;
            statusMessageTextbox.AppendText(value.Item2);
        }

        public void uploadUsers(string content, int incrementSize = 25)
        {
            m_content = content;
            m_incrementSize = incrementSize;
            m_currentOperation = Operation.UploadUsers;
            statusMessageTextbox.Text = "Upload Users"+Environment.NewLine;
            this.ShowDialog();
        }

        public void deleteUsers(string content, int incrementSize = 25)
        {
            m_content = content;
            m_incrementSize = incrementSize;
            m_currentOperation = Operation.DeleteUsers;
            statusMessageTextbox.Text = "Delete Users" + Environment.NewLine;
            this.ShowDialog();
        }

        public void uploadMasterpoints(string content, int incrementSize = 25)
        {
            m_content = content;
            m_incrementSize = incrementSize;
            m_currentOperation = Operation.UploadMasterpoints;
            statusMessageTextbox.Text = "Upload Masterpoints" + Environment.NewLine;
            this.ShowDialog();
        }

        private void runOperation()
        {
            errorFound = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(startOperation);
            bw.ProgressChanged += new ProgressChangedEventHandler(operationProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(operationCompleted);
            bw.RunWorkerAsync();
        }

        private void UploadProgress_Load(object sender, EventArgs e)
        {
            runOperation();
        }

    }
}
