using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IndianBridge.GoogleAPIs;
using IndianBridge.Common;
using System.IO;

namespace IndianBridgeScorer
{
    public partial class DownloadTourney : Form
    {
        private DocsAPI m_da;
        public string selectedTourney = "";
        public DownloadTourney()
        {
            InitializeComponent();
        }

        private void tourneysImported(bool success)
        {
            this.Enabled = true;
            this.Opacity = 1;
            m_le.Close();
            tourneysListCombobox.Items.AddRange(m_da.listOfFolders.ToArray());
            if (tourneysListCombobox.Items.Count > 0) tourneysListCombobox.SelectedIndex = 0;
        }

        LoadingEvents m_le;
        private void DownloadTourney_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Opacity = 0.75;
            m_le = new LoadingEvents("Loading Online Tourney List");
            m_le.StartPosition = FormStartPosition.CenterParent;
            m_da = new DocsAPI("indianbridge.dummy@gmail.com", "kibitzer");
            CustomBackgroundWorker cbw = new CustomBackgroundWorker("List Online Tourneys", m_da.getListOfFoldersInBackground, tourneysImported, null, null, null, null);
            cbw.run(Path.Combine("Indian Bridge Scorer", "Tourneys"));
            m_le.ShowDialog(this);
        }

        private void downloadCompleted(bool success)
        {
            ChangeEnabled(true);
            if (!success)
            {
                string directory = Path.Combine(Constants.getTourneysFolder(), tourneysListCombobox.Text);
                Directory.Delete(directory, true);
                return;
            }
            selectedTourney = tourneysListCombobox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void downloadTourneyButton_Click(object sender, EventArgs e)
        {
            string directory = Path.Combine(Constants.getTourneysFolder(), tourneysListCombobox.Text);
            if (Directory.Exists(directory))
            {
                if (MessageBox.Show("A directory already exists at " + directory + Environment.NewLine + "If you download online tourney it will overwrite existing contents.Do you want to overwrite?", "Directory Exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            }
            ChangeEnabled(false);
            statusStrip1.Enabled = true;
            operationCancelButton.Enabled = true;
            Dictionary<string, string> fileFilters = new Dictionary<string, string>();
            fileFilters.Add(".ini", "text/plain");
            fileFilters.Add(".mdb", "application/msaccess");
            CustomBackgroundWorker m_cbw = new CustomBackgroundWorker("Download from Google Docs", m_da.DownloadGoogleDocsToDirectoryInBackground, downloadCompleted, operationStatus,
                    operationProgressBar, operationCancelButton, null);
            Tuple<string, string, Dictionary<string, string>> values = new Tuple<string, string, Dictionary<string, string>>(Path.Combine("Indian Bridge Scorer", "Tourneys", tourneysListCombobox.Text), Constants.getTourneysFolder(), fileFilters);
            m_cbw.run(values);
        }

        void ChangeEnabled(bool enabled)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = enabled;
            }
        } 

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
