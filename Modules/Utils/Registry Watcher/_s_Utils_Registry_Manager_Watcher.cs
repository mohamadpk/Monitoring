using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using Microsoft.Win32;

namespace Utils.Registry_Watcher
{
    public class _s_Utils_Registry_Manager_Watcher
    {
        
        public _s_Utils_Registry_Manager_Watcher()
        {
        KeysToWatch = new List<_s_Utils_Registry_Manager_Watcher_Node>();
        }
        public bool keepRunning = true;
        List<_s_Utils_Registry_Manager_Watcher_Node> KeysToWatch=null;
        public delegate void DelRegistryWatcherEvent(object sender, EventArrivedEventArgs e);
        public DelRegistryWatcherEvent RegistryWatcherEvent;
        public void AddRegistryKeyToWatchList(_s_Utils_Registry_Manager_Watcher_Node RegistryNode)
        {
            
            KeysToWatch.Add(RegistryNode);
        }

        public void StartWatching()
        {
                foreach (_s_Utils_Registry_Manager_Watcher_Node KeyToWatch in KeysToWatch)
                {
                try {
                    StringBuilder query = new StringBuilder("SELECT * FROM RegistryValueChangeEvent WHERE Hive = '");
                    query.Append("HKEY_LOCAL_MACHINE");
                    query.Append("'");
                    //query.Append(" AND KeyPath = '");
                    //query.Append(KeyToWatch.Path);
                    //query.Append("'");

                    ManagementEventWatcher RegistryStartTrace = new ManagementEventWatcher(
                        new WqlEventQuery("SELECT * FROM RegistryTreeChangeEvent WHERE " +
                     "Hive = 'HKEY_LOCAL_MACHINE'" +
                     @"AND Rootpath = 'Software'"));
                    RegistryStartTrace.EventArrived += RegistryChangeTrace_EventArrived;

                    RegistryStartTrace.Start();
                }catch(Exception ex)
                { }
                }



            while (keepRunning)
            {
                ///<description>
                ///when application is form base this work for anti freez message queue 
                ///on command base program this line is no mean
                ///</description>
                System.Windows.Forms.Application.DoEvents();
            }


        }

        private void RegistryChangeTrace_EventArrived(object sender, EventArrivedEventArgs e)
        {
        //    System.Management.registry
        //    event EventHandler<RegistryKeyChangeEventArgs> RegistryKeyChangeEvent;
        //RegistryKeyChangeEvent RegValueChange = new RegistryKeyChangeEvent(e.NewEvent);
        //    RegistryWatcherEvent(sender, e);
        }
    }
}
