using Module_USB_Watching.USB_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_USB_Watching
{
    class _m_USB_Watcher_Disable_Enable
    {
        public static void OnDisableUSB(string DeviceId)
        {
            List<DeviceInfo> USBList=  OnGetUsbList();
            foreach(DeviceInfo dviceinfo in USBList)
            {
                if(dviceinfo.DeviceID==DeviceId)
                {
                    Devices.RemoveDevices(dviceinfo.Instance);
                }
            }
        }
        public static void OnDisableAllUSB()
        {
            List<DeviceInfo> USBList = OnGetUsbList();
            foreach (DeviceInfo dviceinfo in USBList)
            {
                    Devices.RemoveDevices(dviceinfo.Instance);
            }
        }

        public static List<DeviceInfo> OnGetUsbList()
        {
            return Devices.GetConnectedUSBDevices("", "");
        }
    }
}
