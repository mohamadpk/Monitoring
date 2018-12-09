using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.WiFi_Logger
{
    public class _m_WiFi_Watcher_Node
    {
        int _Id;
        string _Name;
        string _Description;
        string _GUID;
        string _Physical_address;
        string _State;
        string _SSID;
        string _BSSID;
        string _Network_type;
        string _Radio_type;
        string _Authentication;
        string _Cipher;
        string _Connection_mode;
        string _Channel;
        string _Receive_rate;
        string _Transmit_rate;
        string _Signal;
        string _Profile;
        string _Hosted_network_status;
        DateTime _date;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }
        public string Physical_address
        {
            get { return _Physical_address; }
            set { _Physical_address = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string SSID
        {
            get { return _SSID; }
            set { _SSID = value; }
        }
        public string BSSID
        {
            get { return _BSSID; }
            set { _BSSID = value; }
        }
        public string Network_type
        {
            get { return _Network_type; }
            set { _Network_type = value; }
        }
        public string Radio_type
        {
            get { return _Radio_type; }
            set { _Radio_type = value; }
        }
        public string Authentication
        {
            get { return _Authentication; }
            set { _Authentication = value; }
        }
        public string Cipher
        {
            get { return _Cipher; }
            set { _Cipher = value; }
        }
        public string Connection_mode
        {
            get { return _Connection_mode; }
            set { _Connection_mode = value; }
        }
        public string Channel
        {
            get { return _Channel; }
            set { _Channel = value; }
        }
        public string Receive_rate
        {
            get { return _Receive_rate; }
            set { _Receive_rate = value; }
        }
        public string Transmit_rate
        {
            get { return _Transmit_rate; }
            set { _Transmit_rate = value; }
        }
        public string Signal
        {
            get { return _Signal; }
            set { _Signal = value; }
        }
        public string Profile
        {
            get { return _Profile; }
            set { _Profile = value; }
        }
        public string Hosted_network_status
        {
            get { return _Hosted_network_status; }
            set { _Hosted_network_status = value; }
        }
        public DateTime date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}
