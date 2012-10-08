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
    public partial class UploadTourney : Form
    {
        public UploadTourney()
        {
            InitializeComponent();
        }

        private void uploadCompleted(bool success)
        {
            if (!success)
            {
                Utilities.showErrorMessage("Tourney was not backed up to Google Docs!");
            }
            this.Close();
        }

        private void UploadTourney_Load(object sender, EventArgs e)
        {
            label1.Text = "Uploading " + Constants.CurrentTourneyName + " to Google Docs";
            Dictionary<string, string> fileFilters = new Dictionary<string, string>();
            fileFilters.Add(".ini", "text/plain");
            fileFilters.Add(".mdb", "application/msaccess");
            DocsAPI m_da = new DocsAPI("indianbridge.dummy@gmail.com", "kibitzer");
            CustomBackgroundWorker m_cbw = new CustomBackgroundWorker("Upload to Google Docs", m_da.uploadDirectoryToGoogleDocsInBackground, uploadCompleted, operationStatus,
                    operationProgressBar, operationCancelButton, null);
            Tuple<string, string, Dictionary<string, string>> values = new Tuple<string, string, Dictionary<string, string>>(Constants.getCurrentTourneyFolder(), Path.Combine("Indian Bridge Scorer", "Tourneys"), fileFilters);
            m_cbw.run(values);
        }
    }
}
