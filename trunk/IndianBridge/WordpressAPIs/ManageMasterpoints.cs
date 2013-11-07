using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace IndianBridge.WordpressAPIs
{

    public interface XMLRPCInterface
    {
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.validateCredentials")]
        string validateCredentials(Credentials credentials);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.invalidateCredentials")]
        string invalidateCredentials(Session session);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.validateMasterpointCredentials")]
        string validateMasterpointCredentials(int blogId, string username, string password);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.getTableData")]
        string getTableData(TableInfo tableInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addTournamentLevel")]
        string addTournamentLevel(TournamentLevelInfo tournamentInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addTournament")]
        string addTournament(TournamentInfo tournamentInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addEvent")]
        string addEvent(EventInfo eventInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addUsers")]
        string addUsers(TableInfo tableInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.deleteUsers")]
        string deleteUsers(TableInfo tableInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.transferUsers")]
        string transferUsers(TableInfo tableInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addMasterpoints")]
        string addMasterpoints(TableInfo tableInfo);
    }

    public class Credentials
    {
        public string username = "";
        public string password = "";
        public string delimiter = "~";
    }

    public class Session
    {
        public string session_id = "";
        public string delimiter = "~";
    }

    public class TableInfo
    {
        public string session_id = "";
        public string table_name = "";
        public string content = "";
        public string delimiter = "~";
        public string where = "";
        public string orderBy = "";
        public string limit = "";
    }

    public class TournamentLevelInfo
    {
        public string session_id = "";
        public string tournament_level_code = "";
        public string description = "";
        public string tournament_type = "";
    }

    public class TournamentInfo
    {
        public string session_id = "";
        public string tournament_code = "";
        public string description = "";
        public string tournament_level_code = "";
    }

    public class EventInfo
    {
        public string session_id = "";
        public string event_code = "";
        public string description = "";
    }

    public class ManageMasterpoints
    {
        public XmlRpcClientProtocol m_clientProtocol;
        public XMLRPCInterface m_interface;
        private string m_siteName = null;
        private string m_xmlrpcURL = null;
        private string m_userName = null;
        public string m_session_id = "";
        private string m_delimiter = "~";

        public string UserName
        {
          get { return m_userName; }
          set { m_userName = value; }
        }

        private string m_password = null;


        public ManageMasterpoints(string sitename, string username, string password)
        {
            // Initialize member variables
            this.m_siteName = sitename;
            this.m_userName = username;
            this.m_password = password;
            this.m_xmlrpcURL = m_siteName.TrimEnd(new[] { '/', '\\' }) + "/xmlrpc.php";

            m_interface = (XMLRPCInterface)XmlRpcProxyGen.Create(typeof(XMLRPCInterface));
            m_clientProtocol = (XmlRpcClientProtocol)m_interface;
            m_clientProtocol.Url = this.m_xmlrpcURL;

        }

        public string validateCredentials()
        {
            try
            {
                Credentials credentials = new Credentials();
                credentials.username = m_userName;
                credentials.password = m_password;
                credentials.delimiter = m_delimiter;
                return m_interface.validateCredentials(credentials);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string invalidateCredentials()
        {
            try
            {
                Session session = new Session();
                session.session_id = m_session_id;
                session.delimiter = m_delimiter;
                return m_interface.invalidateCredentials(session);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string getTableData(string table_name)
        {
            try
            {
                TableInfo tableInfo = new TableInfo();
                tableInfo.session_id = m_session_id;
                tableInfo.delimiter = m_delimiter;
                tableInfo.table_name = table_name;
                return m_interface.getTableData(tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addTournamentLevel(TournamentLevelInfo tournamentLevelInfo)
        {
            try
            {
                tournamentLevelInfo.session_id = m_session_id;
                return m_interface.addTournamentLevel(tournamentLevelInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addTournament(TournamentInfo tournamentInfo)
        {
            try
            {
                tournamentInfo.session_id = m_session_id;
                return m_interface.addTournament(tournamentInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addEvent(EventInfo eventInfo)
        {
            try
            {
                eventInfo.session_id = m_session_id;
                return m_interface.addEvent(eventInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addUsers(TableInfo tableInfo)
        {
            try
            {
                tableInfo.session_id = m_session_id;
                return m_interface.addUsers(tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string deleteUsers(TableInfo tableInfo)
        {
            try
            {
                tableInfo.session_id = m_session_id;
                return m_interface.deleteUsers(tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string transferUsers(TableInfo tableInfo)
        {
            try
            {
                tableInfo.session_id = m_session_id;
                return m_interface.transferUsers(tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string addMasterpoints(TableInfo tableInfo)
        {
            try
            {
                tableInfo.session_id = m_session_id;
                return m_interface.addMasterpoints(tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
