using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using IndianBridge.Common;
using System.Windows.Forms;
using System.Drawing;

namespace IndianBridge.Common
{
    public class CustomBackgroundWorker
    {
        private BackgroundWorker m_worker = null;
        private string m_workerName = "";
        public delegate void DoWorkHandlerDelegate(object o,DoWorkEventArgs e);
        private DoWorkHandlerDelegate m_doWorkHandler = null;
        private delegate void RunWorkerCompletedEventHandlerDelegate(object o, RunWorkerCompletedEventArgs e);
        private RunWorkerCompletedEventHandlerDelegate m_runWorkerCompletedHandler = null;
        public delegate void RunAfterCompletionDelegate(bool success);
        private RunAfterCompletionDelegate m_runAfterCompletion = null;
        private delegate void ProgressChangedEventHandlerDelegate(object o, ProgressChangedEventArgs e);
        private ProgressChangedEventHandlerDelegate m_progressChangedHandler = null;
        private ToolStripStatusLabel m_statusLabelT = null;
        private ToolStripProgressBar m_statusProgressBarT = null;
        private ToolStripButton m_statusButtonT = null;
        private Label m_statusLabel = null;
        private ProgressBar m_statusProgressBar = null;
        private Button m_statusButton = null;
        private TextBox m_statusTextbox = null;
        private bool m_useToolStrip = false;
        public enum WorkerStatus { COMPLETED, ERROR, CANCELLED };
        public WorkerStatus m_status;

        public CustomBackgroundWorker(string workerName,DoWorkHandlerDelegate doWorkHandler, RunAfterCompletionDelegate runAfterCompletion,
            ToolStripStatusLabel statusLabel, ToolStripProgressBar statusProgressBar, ToolStripButton statusButton, TextBox statusTextbox)
        {
            m_workerName = workerName;
            m_doWorkHandler = doWorkHandler;
            m_runAfterCompletion = runAfterCompletion;
            m_progressChangedHandler = progressChangedHandler;
            m_runWorkerCompletedHandler = workerCompletedHandler;
            m_statusLabelT = statusLabel;
            m_statusProgressBarT = statusProgressBar;
            m_statusButtonT = statusButton;
            m_statusTextbox = statusTextbox;
            m_useToolStrip = true;
        }

        // Last field is added to avoid ambiguity when nulls are passed
        public CustomBackgroundWorker(string workerName, DoWorkHandlerDelegate doWorkHandler, RunAfterCompletionDelegate runAfterCompletion,
            Label statusLabel, ProgressBar statusProgressBar, Button statusButton, TextBox statusTextbox,bool dummy)
        {
            m_workerName = workerName;
            m_doWorkHandler = doWorkHandler;
            m_runAfterCompletion = runAfterCompletion;
            m_progressChangedHandler = progressChangedHandler;
            m_runWorkerCompletedHandler = workerCompletedHandler;
            m_statusLabel = statusLabel;
            m_statusProgressBar = statusProgressBar;
            m_statusButton = statusButton;
            m_statusTextbox = statusTextbox;
            m_useToolStrip = false;
        }

        private void setStatus(int percentage, string message, string detailedMessage, string buttonText)
        {
            if (!m_useToolStrip && m_statusButton != null) m_statusButton.Text = buttonText;
            else if (m_useToolStrip && m_statusButtonT != null) m_statusButtonT.Text = buttonText;
            setStatus(percentage, message, detailedMessage);
        }

        private void setStatus(int percentage, string message, string detailedMessage)
        {
            if (!m_useToolStrip)
            {
                if (m_statusLabel != null)
                {
                    if (percentage == 0) m_statusLabel.ForeColor = Color.Red;
                    else if (percentage == 100) m_statusLabel.ForeColor = Color.Green;
                    else m_statusLabel.ForeColor = Color.Orange;
                    m_statusLabel.Text = message;
                }
                if (m_statusProgressBar != null) m_statusProgressBar.Value = percentage;
                if (m_statusTextbox != null) m_statusTextbox.AppendText(detailedMessage+Environment.NewLine);
            }
            else
            {
                if (m_statusLabelT != null)
                {
                    if (percentage == 0) m_statusLabelT.ForeColor = Color.Red;
                    else if (percentage == 100) m_statusLabelT.ForeColor = Color.Green;
                    else m_statusLabelT.ForeColor = Color.Orange;
                    m_statusLabelT.Text = message;
                }
                if (m_statusProgressBarT != null) m_statusProgressBarT.Value = percentage;
                if (m_statusTextbox != null) m_statusTextbox.AppendText(detailedMessage+Environment.NewLine);
            }
        }

        private void progressChangedHandler(object sender, ProgressChangedEventArgs e)
        {
            string message = m_workerName + " : "+e.UserState as string;
            string detailedMessage = m_workerName+" : In Progress - Completed " + e.ProgressPercentage + "%" + Environment.NewLine + message;
            setStatus(e.ProgressPercentage, message, detailedMessage, "Cancel "+m_workerName);
        }

        private void workerCompletedHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            int percentage = 0;
            string message = "", detailedMessage = "";
            if ((e.Cancelled == true))
            {
                m_status = WorkerStatus.CANCELLED;
                percentage = 0;
                message = m_workerName+" : Cancelled";
                detailedMessage = "Cancelled by user after only " + ((m_statusProgressBar == null)?"unknown":""+m_statusProgressBar.Value) + " % completed!";
                if (m_runAfterCompletion != null) m_runAfterCompletion(false);
            }

            else if (e.Error != null)
            {
                m_status = WorkerStatus.ERROR;
                percentage = 0;
                message = m_workerName+" : Error";
                detailedMessage = "Following error(s) noted after " + ((m_statusProgressBar == null) ? "unknown" : "" + m_statusProgressBar.Value) + "% completed!";
                detailedMessage += Environment.NewLine + e.Error.Message;
                if (m_statusTextbox == null) Utilities.showErrorMessage(detailedMessage);
                if (m_runAfterCompletion != null) m_runAfterCompletion(false);
            }
            else
            {
                m_status = WorkerStatus.COMPLETED;
                percentage = 100;
                message = m_workerName+" : Success";
                detailedMessage = (m_workerName+" completed successfully");
                if (m_runAfterCompletion != null) m_runAfterCompletion(true);
            }
            setStatus(percentage, message, detailedMessage, "");
        }


        public void run(object value = null)
        {
            if (m_worker == null || !m_worker.IsBusy)
            {
                m_worker = new BackgroundWorker();
                m_worker.WorkerReportsProgress = true;
                m_worker.WorkerSupportsCancellation = true;
                m_worker.DoWork += new DoWorkEventHandler(m_doWorkHandler);
                m_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_runWorkerCompletedHandler);
                m_worker.ProgressChanged += new ProgressChangedEventHandler(m_progressChangedHandler);
                if (m_statusButton != null) m_statusButton.Click += new EventHandler(cancelOperation);
                setStatus(0, m_workerName + " : Starting...", m_workerName + " : Starting...", "Cancel " + m_workerName);
                m_worker.RunWorkerAsync(value);
            }
            else
            {
                Utilities.showErrorMessage("A " + m_workerName + " operation is already running! Please wait for it to complete or press cancel button to stop it immediately before starting again.");
            }
        }

        private void cancelOperation(object sender, EventArgs e)
        {
            m_statusButton.Text = "Cancelling";
            if (m_worker != null && m_worker.IsBusy) m_worker.CancelAsync();
        }
    }
}
