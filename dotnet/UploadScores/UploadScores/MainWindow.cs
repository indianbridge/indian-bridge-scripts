using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using Google.GData.Tools;

namespace UploadScores
{

    public partial class MainWindow : Form
    {
        private SitesService service = null;
        public MainWindow()
        {
            InitializeComponent();
            GoogleClientLogin loginDialog = new GoogleClientLogin(new SpreadsheetsService("Spreadsheet-GData-Sample-App"), "youremailhere@gmail.com");
            if (loginDialog.ShowDialog() == DialogResult.OK)
            {
                credentials.Text = "Logged in Successfully";
                service = new SitesService("APPNAME");
                service.setUserCredentials("indianbridge@gmail.com","merabharatmahan");
            }

            else this.Close();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
    class SitesService : MediaService
    {
        public const string GSitesService = "jotspot";
        public const string SITES_NAMESPACE = "http://schemas.google.com/sites/2008";
        public const string KIND_SCHEME = "http://schemas.google.com/g/2005#kind";
        public const string ATTACHMENT_TERM = SITES_NAMESPACE + "#attachment";
        public const string WEBPAGE_TERM = SITES_NAMESPACE + "#webpage";
        public const string FILECABINET_TERM = SITES_NAMESPACE + "#filecabinet";
        public const string PARENT_REL = SITES_NAMESPACE + "#parent";

        public SitesService(string applicationName) : base(GSitesService, applicationName) { }
    }
}
