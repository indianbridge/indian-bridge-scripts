using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace WordpressAPIs
{
    public class AddPagesList
    {
        public int parentPageID = 0;
        public int tourneyYear = 2013;
        public List<NewPageInfo> tourneyPages;
    }

    public class NewPageInfo
    {
        public string title = "";
        public string content = "";
    }

    public class TourneyInfo
    {
        public string username = "";
        public string password = "";
        public string content = "";
    }

    public class TourneyList
    {
        public bool error { get; set; }
        public string message { get; set; }
        public List<TourneyPageInfo> content { get; set; }
    }

    public class AddTourneyPageReturnValue
    {
        public bool error { get; set; }
        public string message { get; set; }
        public List<AddTourneyPageInfo> content { get; set; }
    }

    public class AddTourneyPageInfo
    {
        public bool error { get; set; }
        public string message { get; set; }
    }

    public class TourneyPageInfo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string directory { get; set; }
    }

    public class EditCredentials
    {
        public string username = "";
        public string password = "";
    }

    public interface AddTourneysInterface
    {
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addTourney")]
        string addTourney(TourneyInfo tourneyInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.getTourneys")]
        string getTourneys(EditCredentials credentials);
    }

    public class AddTourneys
    {
        public XmlRpcClientProtocol m_clientProtocol;
        public AddTourneysInterface m_interface;
        private string m_siteName = null;
        private string m_xmlrpcURL = null;
        private string m_userName = null;
        private string m_password = null;
        public AddTourneys(string sitename, string username, string password)
        {
            // Initialize member variables
            this.m_siteName = sitename;
            this.m_userName = username;
            this.m_password = password;
            this.m_xmlrpcURL = m_siteName.TrimEnd(new[] { '/', '\\' }) + "/xmlrpc.php";

            m_interface = (AddTourneysInterface)XmlRpcProxyGen.Create(typeof(AddTourneysInterface));
            m_clientProtocol = (XmlRpcClientProtocol)m_interface;
            m_clientProtocol.Url = this.m_xmlrpcURL;

        }

        public string getTourneys()
        {
            try
            {
                EditCredentials credentials = new EditCredentials();
                credentials.username = m_userName;
                credentials.password = m_password;
                return m_interface.getTourneys(credentials);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addTourney(string content)
        {
            try
            {
                TourneyInfo tourneyInfo = new TourneyInfo();
                tourneyInfo.username = m_userName;
                tourneyInfo.password = m_password;
                tourneyInfo.content = content;
                return m_interface.addTourney(tourneyInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
