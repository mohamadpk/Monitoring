using Module_USB_Watching.USB_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Module_USB_Watching
{
    public class _m_USB_Watcher
    {
            //TODO disable enable rule
            //TODO get and send db log to server
            public void AddRuleToList(_m_USB_Watcher_Disable_Enable_Node RuleToAdd)
            {
                mUSBwdeDB.AddRuleToDB(RuleToAdd);
                RuleList.Add(RuleToAdd);
            }
            public bool RemoveRuleFromList(_m_USB_Watcher_Disable_Enable_Node RuleToRemove)
            {
                mUSBwdeDB.RemoveRuleFromDB(RuleToRemove);
                return RuleList.Remove(RuleToRemove);
            }

            public bool keepRunning = true;

            _m_USB_Watcher_Disable_Enable_DB mUSBwdeDB;
            List<_m_USB_Watcher_Disable_Enable_Node> RuleList;
            public _m_USB_Watcher()
            {
                mUSBwdeDB = new _m_USB_Watcher_Disable_Enable_DB();
                RuleList = mUSBwdeDB.GetAllRules();
            }
            public void StartWatching()
            {
                while (keepRunning)
                {
                    LogAllUSBToDB();
                    System.Threading.Thread.Sleep(1000);
                    DateTime NowTime = DateTime.Now;
                    foreach (_m_USB_Watcher_Disable_Enable_Node rule in RuleList)
                    {
                        int StartTimeCompareResualt = NowTime.CompareTo(rule.StartTime);
                        int EndTileCompareResualt = NowTime.CompareTo(rule.EndTime);
                        if (StartTimeCompareResualt >= 0 && EndTileCompareResualt <= 0 && rule.RuleStatus == true)
                        {
                            if (rule.Action == false && rule.DeviceId != "All")
                            {
                            _m_USB_Watcher_Disable_Enable.OnDisableUSB(rule.DeviceId);
                            }
                            else//DeviceId==All
                            {
                                if (rule.Action == false)
                                {
                                _m_USB_Watcher_Disable_Enable.OnDisableAllUSB();
                                }
                            }
                        }
                    }
                }
            }

        List<DeviceInfo> oldDevices;
        public void LogAllUSBToDB()
        {
            List<DeviceInfo> USBList= _m_USB_Watcher_Disable_Enable.OnGetUsbList();
            if (oldDevices == null)
            {
                oldDevices = Devices.GetConnectedUSBDevices("", "");
            }
            else
            {
                List<DeviceInfo> newDevices = Devices.GetConnectedUSBDevices("", "");
                List<DeviceInfo> differenceDevices = newDevices.Where(p => !oldDevices.Any(l => p.DevicePath == l.DevicePath)).ToList();
                //add diffrence to db
                if(differenceDevices.Count>0)
                {
                    _m_USB_Watcher_Log_DB mUSBwlDB = new _m_USB_Watcher_Log_DB();
                    foreach(DeviceInfo devi in differenceDevices)
                    {
                        mUSBwlDB.AddDeviceInfoToDB(devi);
                    }
                    
                }
                oldDevices = newDevices;
            }

        }

    }
}
