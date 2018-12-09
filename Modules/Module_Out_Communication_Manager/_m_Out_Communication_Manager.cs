using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module_Out_Communication_Manager.HubProxyExtension;
using System.Threading;
using Utils;

namespace Module_Out_Communication_Manager
{
    public class _m_Out_Communication_Manager
    {
        public bool ContinueTryConnect = true;
        private bool IsTryConnectWorked = false;
        public _s_HubsManager HubsManager;
        public HubConnection connection;
        private static string _Target = "http://192.168.1.103:1051";

        public _m_Out_Communication_Manager()
        {
            connection = new HubConnection(GetMyServerAddress());
            HubsManager = new _s_HubsManager();
            HubsManager.AddHub("CommandHub");
            HubsManager.AddHubsToConnection(connection);
            connection.Received += Connection_Received;
            connection.Closed += Connection_Closed;
            connection.Reconnecting += Connection_Reconnecting;
            connection.Reconnected += Connection_Reconnected;
            connection.StateChanged += Connection_StateChanged;
            connection.Headers.Add("Nick", GetMyNick());
        }
        public static string GetMyServerAddress()
        {
            string ServerAddressFile = "ServerAddress.txt";
            string ServerAddress = "";
            if (System.IO.File.Exists(ServerAddressFile))
            {
                ServerAddress = System.IO.File.ReadAllText(ServerAddressFile).Trim();
                if (ServerAddress.Length >0)
                {
                    return ServerAddress;
                }
                else
                {
                    ServerAddress = _Target;
                }
            }
            else
            {
                ServerAddress = _Target;
            }
            return ServerAddress;
        }
        public static string GetMyNick()
        {
            string NickFile = "Nick.txt";
            int NickLen = 10;
            string MyNick = "";
            if (System.IO.File.Exists(NickFile))
            {
                MyNick = System.IO.File.ReadAllText(NickFile).Trim();
                if (MyNick.Length == NickLen)
                {
                    return MyNick;
                }
                else
                {
                    MyNick = _s_Utils_Random.RandomString(NickLen);
                    System.IO.File.WriteAllText(NickFile, MyNick);
                    return MyNick;
                }
            }
            else
            {
                MyNick = _s_Utils_Random.RandomString(NickLen);
                System.IO.File.WriteAllText(NickFile, MyNick);
                return MyNick;
            }
            return MyNick;
        }

        public bool Connect()
        {
            try
            {
                connection.Start().Wait(6000);//60000 =1 min
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        private void Connection_StateChanged(StateChange obj)
        {

        }

        private void Connection_Reconnected()
        {
            
        }

        private void Connection_Reconnecting()
        {
            
        }

        private void Connection_Closed()
        {
            Connect();
        }


        private void Connection_Received(string obj)
        {

        }
    }
}
