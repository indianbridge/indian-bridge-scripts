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
using System.IO;

namespace UploadFolderToWordpress
{
    public partial class UploadFolderToWordpress : Form
    {
        private bool m_publishResultsRunning = false;
        private CustomBackgroundWorker m_publishResultsCBW = null;
        public UploadFolderToWordpress()
        {
            InitializeComponent();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFolderTextbox.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void publishResultsCompleted(bool success)
        {
            m_publishResultsRunning = false;
            if (success)
            {
                MessageBox.Show("Folder uploaded Successfully!");
            }
        }

        private void publishResultsToWordpress()
        {
            if (m_publishResultsRunning)
            {
                Utilities.showErrorMessage("A Publish Results operation is already running. Wait for it to finish or Cancel it before starting another!");
                return;
            }
            string siteName =  wordpressURLTextbox.Text;
            string pagePath = wordpressPathTextbox.Text;
            string username = usernameTextbox.Text;
            string password = passwordTextbox.Text;
            UploadWebpages uw = new UploadWebpages(siteName, username, password, true);
            uw.UseTourneyTemplate = useTourneyTemplateCheckbox.Checked;
            uw.ForceUpload = true;
            String fileName = selectedFolderTextbox.Text;
            if (Directory.Exists(fileName))
            {
                m_publishResultsCBW = new CustomBackgroundWorker("Publish Results", uw.uploadDirectoryInBackground, publishResultsCompleted, publishResultsStatus, publishResultsProgressBar, cancelPublishResultsButton, null);
                Tuple<string, string> values = new Tuple<string, string>(selectedFolderTextbox.Text, pagePath);
                m_publishResultsRunning = true;
                m_publishResultsCBW.run(values);
            }
            else if (File.Exists(fileName))
            {
                try
                {
                    uw.uploadSingleFile(fileName, pagePath);
                    MessageBox.Show(fileName + " uploaded to " + siteName + pagePath + " successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Following Error occurred while uploading file" + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(fileName + " is not a valid file or folder!");
            }


        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            publishResultsToWordpress();
        }

        private void selectFolderButton_Click_1(object sender, EventArgs e)
        {
            if (selectFolderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFolderTextbox.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void selectFileToUploadButton_Click(object sender, EventArgs e)
        {
            if (selectFileOrFolderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFolderTextbox.Text = selectFileOrFolderDialog.FileName;
            }
        }

        private void postMasterpointsButton_Click(object sender, EventArgs e)
        {
            try
            {
                string siteName = wordpressURLTextbox.Text;
                string username = usernameTextbox.Text;
                string password = passwordTextbox.Text;
                UploadWebpages uw = new UploadWebpages(siteName, username, password, true);
                uw.postMasterpoints(csvContentTextbox.Text);
                MessageBox.Show("Posted Masterpoints successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Following Error occurred while posting masterpoints" + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
