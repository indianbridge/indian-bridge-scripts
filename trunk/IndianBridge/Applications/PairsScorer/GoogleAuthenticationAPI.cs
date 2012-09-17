using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using System.Diagnostics;
using System.Threading;

namespace IndianBridge.Applications
{
    public partial class GoogleAuthenticationAPI : Form
    {
        public OAuth2Authenticator<NativeApplicationClient> m_auth;
        private NativeApplicationClient m_provider;
        private CalendarService m_service;

        public GoogleAuthenticationAPI()
        {
            InitializeComponent();
        }

        private IAuthorizationState GetAuthorization(NativeApplicationClient arg)
        {

            // Get the auth URL:
            IAuthorizationState state = new AuthorizationState(new[] { CalendarService.Scopes.Calendar.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            Uri authUri = arg.RequestUserAuthorization(state);

            // Request authorization from the user (by opening a browser window):
            GoogleLogin gl = new GoogleLogin(authUri);
            gl.ShowDialog();
            String[] tokens = gl.m_authorizationCode.Split('=');
            String authCode = tokens[1];
            // Retrieve the access token by using the authorization code:
            return arg.ProcessUserAuthorization(authCode, state);
        }

        private void run_Button_Click(object sender, EventArgs e)
        {
            //Calendar calendar = null;
            try
            {
                m_provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
                m_provider.ClientIdentifier = "475623449109-og3q1tijq7kuu82i8dnfjcjklhb32g47.apps.googleusercontent.com";
                m_provider.ClientSecret = "OdlJ_oaKeWQXdXEpkMsjF3HF";
                m_auth = new OAuth2Authenticator<NativeApplicationClient>(m_provider, GetAuthorization);
                m_service = new CalendarService(m_auth);
                EventsResource.ListRequest listRequest = m_service.Events.List("indianbridge@gmail.com");
                listRequest.MaxResults = 1;
                listRequest.Q = "authentication";
                listRequest.TimeMax = "1981-01-01T23:59:59+05:30";
                Events events = listRequest.Fetch();

                //calendar = m_service.Calendars.Get("indianbridge@gmail.com").Fetch();
                this.authorizationCode_Textbox.Text = "Number of Items = " + events.Items.Count;
                Event ev = events.Items[0];
                this.authorizationCode_Textbox.Text += "   \nSummary : " + ev.Summary;
                this.authorizationCode_Textbox.Text += "   \nDescription : " + ev.Description;
                try
                {

                    if (ev.Description.EndsWith("x")) ev.Description = ev.Description.Substring(0, ev.Description.Length - 1);
                    else ev.Description += "x";
                    m_service.Events.Update(ev, "indianbridge@gmail.com", ev.Id).Fetch();
                    //m_service.Calendars.Update(calendar, calendar.Id).Fetch();
                    this.authorizationCode_Textbox.Text += "\nSucess!!!";
                }
                catch (Exception ex)
                {
                    this.authorizationCode_Textbox.Text += "After Update" + ex.Message;
                }
            }
            catch (Exception ex)
            {
                this.authorizationCode_Textbox.Text += "Before Update"+ex.Message;
            }
        }

    }
}
