using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordpressAPIs;
using IndianBridge.Common;

namespace BFIAddTourney
{
	public partial class ValidateCredentials : Form
	{
		public AddTourneys addTourneys;
		public TourneyList tourneyList = null;
		public string m_site;
		private const string m_productionSite = "bfi.net.in";
		
		public ValidateCredentials()
		{
			InitializeComponent();
			var usernameFromConfig = ConfigurationManager.AppSettings["username"];
			m_site = ConfigurationManager.AppSettings["site"];

			if (!String.IsNullOrEmpty(usernameFromConfig))
				usernameTextbox.Text = usernameFromConfig;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
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
			addTourneys = new AddTourneys(m_site, usernameTextbox.Text, passwordTextbox.Text);
			string json_result = addTourneys.getTourneys();
			try
			{
				tourneyList = Utilities.JsonDeserialize<TourneyList>(json_result);
				loadingPicture.Visible = false;
				loginPanel.Enabled = true;
				if (tourneyList.error)
				{
					MessageBox.Show("Unable to validate username and password because : " + Environment.NewLine + tourneyList.message);
					return;
				}
				else
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
					if (m_site.Contains(m_productionSite))
					{
						MessageBox.Show(
							"Please note that you are logged in to the production site. Your changes will impact content on the official public BFI web site!",
							"Production Web site warning!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Exception : " + Environment.NewLine + ex.Message);
				loadingPicture.Visible = false;
				loginPanel.Enabled = true;
				return;
			}
		}
	}
}
