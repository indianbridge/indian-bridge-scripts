using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace IndianBridge.WordpressAPIs
{

    public interface XMLRPCInterface
    {
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.validateMasterpointCredentials")]
        string validateMasterpointCredentials(int blogId, string username, string password);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.getTableData")]
        string getTableData(int blogId, string username, string password, TableInfo tableInfo);
        /*[CookComputing.XmlRpc.XmlRpcMethod("bfi.addTableData")]
        string addTableData(int blogId, string username, string password, TableInfo tableInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.removeTableData")]
        string removeTableData(int blogId, string username, string password, TableInfo tableInfo);*/
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addTournamentLevel")]
        string addTournamentLevel(int blogId, string username, string password, TournamentLevelInfo tournamentInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addTournament")]
        string addTournament(int blogId, string username, string password, TournamentInfo tournamentInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addEvent")]
        string addEvent(int blogId, string username, string password, EventInfo eventInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addUsers")]
        string addUsers(int blogId, string username, string password, TableInfo tableInfo);
        [CookComputing.XmlRpc.XmlRpcMethod("bfi.addMasterpoints")]
        string addMasterpoints(int blogId, string username, string password, TableInfo tableInfo);
    }

    public class TableInfo
    {
        public string tableName = "";
        public string content = "";
        public string delimiter = "";
        public string where = "";
        public string orderBy = "";
        public string limit = "";
    }

    public class TournamentLevelInfo
    {
        public string tournament_level_code = "";
        public string description = "";
        public string tournament_type = "";
    }

    public class TournamentInfo
    {
        public string tournament_code = "";
        public string description = "";
        public string tournament_level_code = "";
    }

    public class EventInfo
    {
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

        public string validateMasterpointCredentials()
        {
            try
            {
                return m_interface.validateMasterpointCredentials(1, m_userName, m_password);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        public string getTableData(TableInfo tableInfo)
        {
            try
            {
                return m_interface.getTableData(1, m_userName, m_password,tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /*public string addTableData(TableInfo tableInfo)
        {
            try
            {
                return m_interface.addTableData(1, m_userName, m_password, tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string removeTableData(TableInfo tableInfo)
        {
            try
            {
                return m_interface.removeTableData(1, m_userName, m_password, tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }*/

        public string addTournamentLevel(TournamentLevelInfo tournamentLevelInfo)
        {
            try
            {
                return m_interface.addTournamentLevel(1, m_userName, m_password, tournamentLevelInfo);
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
                return m_interface.addTournament(1, m_userName, m_password, tournamentInfo);
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
                return m_interface.addEvent(1, m_userName, m_password, eventInfo);
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
                return m_interface.addUsers(1, m_userName, m_password, tableInfo);
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
                return m_interface.addMasterpoints(1, m_userName, m_password, tableInfo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
