using Module_Process_Manager;
using Module_Process_Manager.Process_Executer;
using Module_Process_Manager.Process_Executer.Respose;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.WiFi_Logger
{
    public class _m_WiFi_Watcher
    {
        public bool keepRunning = true;
        _m_WiFi_Watcher_DB mwwDB;
        public _m_WiFi_Watcher()
        {
            mwwDB = new _m_WiFi_Watcher_DB();
        }
        public void StartWatching()
        {
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(1000);
                OnExecuteProcessResponse ExecuteResponse = _m_Process_Manager.OnExecuteProcess("netsh.exe", "wlan show interfaces", ProcessOutputType: _m_Child_Process.Enum_ProcessOutputType.Cmd, FullHide: false);
                string Output = ExecuteResponse.Output;
                //TODO Add to db 
                if (!Output.Contains("There is no wireless interface on the system"))
                {
                    _m_WiFi_Watcher_Node mwwn = new _m_WiFi_Watcher_Node();
                    mwwn.date = DateTime.Now;
                    string[] lines = Output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] SplitedByColon = line.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
                        if (SplitedByColon.Length < 2)
                            continue;
                        string PropertyName = SplitedByColon[0].TrimStart().TrimEnd();
                        if (PropertyName != "")
                        {
                            string PropertyValue = SplitedByColon[1].TrimStart();
                            if (PropertyValue != "")
                            {
                                if (PropertyName == "Name")
                                {
                                    mwwn.Name = PropertyValue;
                                }
                                else if (PropertyName == "Description")
                                {
                                    mwwn.Description = PropertyValue;
                                }
                                else if (PropertyName == "GUID")
                                {
                                    mwwn.GUID = PropertyValue;
                                }
                                else if (PropertyName == "Physical address")
                                {
                                    mwwn.Physical_address = PropertyValue;
                                }
                                else if (PropertyName == "State")
                                {
                                    mwwn.State = PropertyValue;
                                }
                                else if (PropertyName == "SSID")
                                {
                                    mwwn.SSID = PropertyValue;
                                }
                                else if (PropertyName == "BSSID")
                                {
                                    mwwn.BSSID = PropertyValue;
                                }
                                else if (PropertyName == "Network type")
                                {
                                    mwwn.Network_type = PropertyValue;
                                }
                                else if (PropertyName == "Radio type")
                                {
                                    mwwn.Radio_type = PropertyValue;
                                }
                                else if (PropertyName == "Authentication")
                                {
                                    mwwn.Authentication = PropertyValue;
                                }
                                else if (PropertyName == "Cipher")
                                {
                                    mwwn.Cipher = PropertyValue;
                                }
                                else if (PropertyName == "Connection mode")
                                {
                                    mwwn.Connection_mode = PropertyValue;
                                }
                                else if (PropertyName == "Channel")
                                {
                                    mwwn.Channel = PropertyValue;
                                }
                                else if (PropertyName == "Receive rate (Mbps)")
                                {
                                    mwwn.Receive_rate = PropertyValue;
                                }
                                else if (PropertyName == "Transmit rate (Mbps)")
                                {
                                    mwwn.Transmit_rate = PropertyValue;
                                }
                                else if (PropertyName == "Signal")
                                {
                                    mwwn.Signal = PropertyValue;
                                }
                                else if (PropertyName == "Profile")
                                {
                                    mwwn.Profile = PropertyValue;
                                }
                                else if (PropertyName == "Hosted network status")
                                {
                                    mwwn.Hosted_network_status = PropertyValue;
                                }
                            }
                        }
                    }


                    if (!mwwDB.WifiIsRecordedOrStateChanged(mwwn.Physical_address, mwwn.State))//is new record
                    {
                        mwwDB.AddWifiToDB(mwwn);
                    }
                }
            }
        }
        public List<_m_WiFi_Watcher_Node> GetWifiList()
        {
           return mwwDB.GetAllWiFi();
        }
    }
}
