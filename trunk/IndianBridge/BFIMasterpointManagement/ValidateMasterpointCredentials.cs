using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using IndianBridge.WordpressAPIs;
using IndianBridge.Common;

namespace BFIMasterpointManagement
{
	public partial class AddTournamentLevel : Form
	{
		public ManageMasterpoints mm;
		public string m_site;
		private const string m_productionSite = "bfi.net.in";

		public AddTournamentLevel()
		{
			InitializeComponent();
			var usernameFromConfig = ConfigurationManager.AppSettings["username"];
			m_site = ConfigurationManager.AppSettings["site"];

			if (!String.IsNullOrEmpty(usernameFromConfig))
				usernameTextbox.Text = usernameFromConfig;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(usernameTextbox.Text))
			{
				MessageBox.Show("Username cannot be empty string!");
				return;
			}
			if (string.IsNullOrWhiteSpace(passwordTextbox.Text))
			{
				MessageBox.Show("Password cannot be empty string!");
				return;
			}
			loginPanel.Enabled = false;
			loadingPicture.Enabled = true;
			loadingPicture.Visible = true;
			loadingPicture.BringToFront();
			this.Refresh();
			mm = new ManageMasterpoints(m_site, usernameTextbox.Text, passwordTextbox.Text);
			string json_result = mm.validateCredentials();
			Dictionary<string, string> result = Utilities.convertJsonOutput(json_result);
			bool errorStatus = Convert.ToBoolean(result["error"]);
			loadingPicture.Visible = false;
			loginPanel.Enabled = true;
			if (!errorStatus)
			{
				mm.m_session_id = result["content"];
				this.DialogResult = DialogResult.OK;
				this.Close();
				if (m_site.Contains(m_productionSite))
				{
					MessageBox.Show(
						"Please note that you are logged in to the production site. Your changes will impact content on the official public BFI web site!",
						"Production Web site warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				}
			}
			else
			{
				MessageBox.Show("Unable to validate username and password because : " + Environment.NewLine + result["message"]);
				return;
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

	}
}
