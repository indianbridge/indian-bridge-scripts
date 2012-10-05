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
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Microsoft.VisualBasic;

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
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                String CLIENT_ID = "853084544209.apps.googleusercontent.com";
                String CLIENT_SECRET = "KgCFuRVRohw6FO38zq_4h7em";

                // Register the authenticator and create the service
                var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description, CLIENT_ID, CLIENT_SECRET);
                var auth = new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthorization);
                var service = new DriveService(auth);

                File body = new File();
                body.Title = "My document";
                body.Description = "A test document";
                body.MimeType = "text/plain";

                byte[] byteArray = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "text/plain");
                request.Upload();

                File file = request.ResponseBody;
                MessageBox.Show("File id: " + file.Id);
            }
        }

        private static IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {
            // Get the auth URL:
            IAuthorizationState state = new AuthorizationState(new[] { "https://www.googleapis.com/auth/drive.file" });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            Process.Start(authUri.ToString());
            string authCode = Interaction.InputBox("What is the Authorization Code?", "Authorization Code");

            // Retrieve the access token by using the authorization code:
            return arg.ProcessUserAuthorization(authCode, state);
        }

    }
}
