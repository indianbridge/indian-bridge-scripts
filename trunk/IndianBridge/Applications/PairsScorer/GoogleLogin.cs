using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IndianBridge.Applications
{
    public partial class GoogleLogin : Form
    {
        public string m_authorizationCode;
        public GoogleLogin(Uri authUri)
        {
            InitializeComponent();
            this.webBrowser1.Url = authUri;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            m_authorizationCode = this.webBrowser1.Document.Title;
            if (m_authorizationCode.Contains('=') && m_authorizationCode.Contains("Success")) this.Close();
        }
    }
}
