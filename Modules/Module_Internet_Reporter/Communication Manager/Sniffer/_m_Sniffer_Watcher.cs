using SharpPcap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Module_Internet_Reporter.Communication_Manager.Sniffer
{
    public class _m_Sniffer_Watcher
    {
        private Object LockForPacketOnSaveAndOnAddNew = new Object();
        public bool keepRunning = true;
        CaptureDeviceList Devices=null;
        List<Thread> CaptureThreads = new List<Thread>();
        Dictionary<ICaptureDevice,ArrayList> DevicesAndPackets;
        int MinToSaveSniff = 30;
        public void StartWatching()
        {
            DevicesAndPackets = new Dictionary<ICaptureDevice, ArrayList>();
            DateTime DateToSavePcap = DateTime.Now.AddMinutes(MinToSaveSniff);
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(1000);
                if (Devices == null)
                {
                    Devices = CaptureDeviceList.Instance;
                    foreach (ICaptureDevice Device in Devices)
                    {
                        StartDeviceSniff(Device);
                    }
                }
                else
                {
                    CaptureDeviceList NewDevices = CaptureDeviceList.Instance;
                    foreach (ICaptureDevice NewDevice in NewDevices)
                    {
                        bool DeviceIsOld = false;
                        foreach(ICaptureDevice OldDevice in Devices)
                        {
                            if (Devices.Contains(NewDevice))
                            {
                                DeviceIsOld = true;
                                break;
                            }                                
                        }
                        
                        if(!DeviceIsOld)
                        {
                            StartDeviceSniff(NewDevice);
                        }
                            
                    }
                    
                    Devices = NewDevices;
                }
                if (DateTime.Now.CompareTo(DateToSavePcap) >= 0)
                {
                    DateToSavePcap=DateToSavePcap.AddMinutes(MinToSaveSniff);
                    SavePackets();
                    
                }
                    
            }
            foreach (Thread CaptureThread in CaptureThreads)
            {
                CaptureThread.Abort();
            }
        }

        private void StartDeviceSniff(ICaptureDevice dev)
        {
            DevicesAndPackets.Add(dev, new ArrayList());
            Thread CaptureThread = new Thread(ThreadStartCapture);
            CaptureThread.Start(dev);
            CaptureThreads.Add(CaptureThread);
        }
        private void ThreadStartCapture(object thread_param_device)
        {
            ICaptureDevice device = (ICaptureDevice)thread_param_device;
            device.Open(DeviceMode.Promiscuous, 1000);

            device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            device.StartCapture();
        }
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            RawCapture temp = e.Packet;
            if(DevicesAndPackets.ContainsKey(e.Device))
            {
               ArrayList Packets= DevicesAndPackets[e.Device];
                lock(LockForPacketOnSaveAndOnAddNew)
                {
                    Packets.Add(temp);
                }
                
            }

        }

        private void SavePackets()
        {
            lock (LockForPacketOnSaveAndOnAddNew)
            {
                
                foreach (KeyValuePair<ICaptureDevice,ArrayList> dev in DevicesAndPackets)
                {
                    if (dev.Key.Started)
                    {
                        string dev_friendlyname = dev.Key.ToString();
                        dev_friendlyname = dev_friendlyname.Substring(dev_friendlyname.IndexOf("FriendlyName: "), dev_friendlyname.Length - dev_friendlyname.IndexOf("FriendlyName: ") - "FriendlyName: ".Length);
                        dev_friendlyname = dev_friendlyname.Substring("FriendlyName: ".Length, dev_friendlyname.IndexOf('\n') - "FriendlyName: ".Length);
                        string CapFileName = dev_friendlyname + DateTime.Now.ToString()+".pcap";
                        CapFileName = CleanFileName(CapFileName);
                        //CapFileName=CapFileName.Replace("/", "-");
                        //CapFileName = CapFileName.Replace(":", "_");
                        SharpPcap.LibPcap.CaptureFileWriterDevice captureFileWriter = new SharpPcap.LibPcap.CaptureFileWriterDevice((SharpPcap.LibPcap.LibPcapLiveDevice)dev.Key, CapFileName);
                        //int count = dev.Value.Count;
                        foreach (RawCapture p in dev.Value)
                        {
                            captureFileWriter.Write(p);
                        }
                    }
                }
            }
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "_"));
        }
    }
}
