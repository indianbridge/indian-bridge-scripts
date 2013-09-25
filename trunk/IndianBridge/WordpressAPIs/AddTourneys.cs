using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace WordpressAPIs
{
    public class TourneyInfo
    {
        public int parentPageID = 0;
        public int tourneyYear = 2013;
        public string tourneyPages = "";
    }

    public interface AddTourneysInterface
    {
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addTourney")]
        string addTourney(int blogId, string username, string password, TourneyInfo tourneyInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.getTourneys")]
        string getTourneys(int blogId, string username, string password);
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
                return m_interface.getTourneys(1, m_userName, m_password);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addTourney(TourneyInfo tourneyInfo)
        {
            try
            {
                return m_interface.addTourney(1, m_userName, m_password,tourneyInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
