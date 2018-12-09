using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.WiFi_Logger
{
    class _m_WiFi_Watcher_DB
    {
        SQLiteConnection dbConnection;

        public _m_WiFi_Watcher_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=WiFi_Watcher.sqlite;Version=3;");
            dbConnection.Open();
        }

        public bool WifiIsRecordedOrStateChanged(string Physical_address,string State)
        {
            
            string sql = "select * from WiFiInfo WHERE Physical_address == @Physical_address and State==@State";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("Physical_address", DbType.String).Value = Physical_address;
            cmd.Parameters.Add("State", DbType.String).Value = State;
            SQLiteDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<_m_WiFi_Watcher_Node> GetAllWiFi()
        {
            string sql = "select * from WiFiInfo";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<_m_WiFi_Watcher_Node> AllWiFi = new List<_m_WiFi_Watcher_Node>();
            while (reader.Read())
            {
                AllWiFi.Add(ReaderToWiFi(reader));
            }
            return AllWiFi;
        }
        private _m_WiFi_Watcher_Node ReaderToWiFi(SQLiteDataReader Reader)
        {
            _m_WiFi_Watcher_Node mwwn = new _m_WiFi_Watcher_Node();
            mwwn.Name = Reader["Name"].ToString();
            mwwn.Description = Reader["Description"].ToString();
            mwwn.GUID = Reader["GUID"].ToString();
            mwwn.Physical_address = Reader["Physical_address"].ToString();
            mwwn.State = Reader["State"].ToString();
            mwwn.SSID = Reader["SSID"].ToString();
            mwwn.BSSID = Reader["BSSID"].ToString();
            mwwn.Network_type = Reader["Network_type"].ToString();
            mwwn.Radio_type = Reader["Radio_type"].ToString();
            mwwn.Authentication = Reader["Authentication"].ToString();
            mwwn.Cipher = Reader["Cipher"].ToString();
            mwwn.Connection_mode = Reader["Connection_mode"].ToString();
            mwwn.Channel = Reader["Channel"].ToString();
            mwwn.Receive_rate = Reader["Receive_rate"].ToString();
            mwwn.Transmit_rate = Reader["Transmit_rate"].ToString();
            mwwn.Signal = Reader["Signal"].ToString();
            mwwn.Profile = Reader["Profile"].ToString();
            mwwn.Hosted_network_status = Reader["Hosted_network_status"].ToString();
            mwwn.date = DateTime.Parse(Reader["date"].ToString());
            return mwwn;
        }
        public void AddWifiToDB(_m_WiFi_Watcher_Node WifiNode)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into WiFiInfo (Name,Description,GUID,Physical_address,State,SSID,BSSID,Network_type,Radio_type,Authentication,Cipher,Connection_mode,Channel,Receive_rate,Transmit_rate,Signal,Profile,Hosted_network_status,date)"+
                " values (@Name,@Description,@GUID,@Physical_address,@State,@SSID,@BSSID,@Network_type,@Radio_type,@Authentication,@Cipher,@Connection_mode,@Channel,@Receive_rate,@Transmit_rate,@Signal,@Profile,@Hosted_network_status,@date)";
            cmd.Parameters.Add("Name", DbType.String).Value = WifiNode.Name;
            cmd.Parameters.Add("Description", DbType.String).Value = WifiNode.Description;
            cmd.Parameters.Add("GUID", DbType.String).Value = WifiNode.GUID;
            cmd.Parameters.Add("Physical_address", DbType.String).Value = WifiNode.Physical_address;
            cmd.Parameters.Add("State", DbType.String).Value = WifiNode.State;
            cmd.Parameters.Add("SSID", DbType.String).Value = WifiNode.SSID;
            cmd.Parameters.Add("BSSID", DbType.String).Value = WifiNode.BSSID;
            cmd.Parameters.Add("Network_type", DbType.String).Value = WifiNode.Network_type;
            cmd.Parameters.Add("Radio_type", DbType.String).Value = WifiNode.Radio_type;
            cmd.Parameters.Add("Authentication", DbType.String).Value = WifiNode.Authentication;
            cmd.Parameters.Add("Cipher", DbType.String).Value = WifiNode.Cipher;
            cmd.Parameters.Add("Connection_mode", DbType.String).Value = WifiNode.Connection_mode;
            cmd.Parameters.Add("Channel", DbType.String).Value = WifiNode.Channel;
            cmd.Parameters.Add("Receive_rate", DbType.String).Value = WifiNode.Receive_rate;
            cmd.Parameters.Add("Transmit_rate", DbType.String).Value = WifiNode.Transmit_rate;
            cmd.Parameters.Add("Signal", DbType.String).Value = WifiNode.Signal;
            cmd.Parameters.Add("Profile", DbType.String).Value = WifiNode.Profile;
            cmd.Parameters.Add("Hosted_network_status", DbType.String).Value = WifiNode.Hosted_network_status;
            cmd.Parameters.Add("date", DbType.DateTime).Value = WifiNode.date;
            cmd.ExecuteNonQuery();
        }
    }
}
