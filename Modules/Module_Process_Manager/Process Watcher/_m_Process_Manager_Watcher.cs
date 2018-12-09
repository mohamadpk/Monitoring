using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace Module_Process_Manager.Process_Watcher
{
    class _m_Process_Manager_Watcher
    {
        /// <summary>
        /// Adds the process to black list.
        /// </summary>
        /// <param name="BlackProcessName">Name of the black process.</param>
        public void AddProcessToBlackList(string BlackProcessName)
        {
            mpmwd.AddBlackProcessToDB(BlackProcessName);
            BlackProcessList.Add(BlackProcessName);  
        }
        /// <summary>
        /// Removes the process from black list.
        /// </summary>
        /// <param name="BlackProcessName">Name of the black process.</param>
        /// <returns></returns>
        public bool RemoveProcessFromBlackList(string BlackProcessName)
        {
            mpmwd.RemoveBlackProcessFromDB(BlackProcessName);
            return BlackProcessList.Remove(BlackProcessName);
        }
        /// <summary>
        /// The keep running publicly on this class
        /// when a _m_Process_Manager_Watcher object created and StartWatching called on a new thread
        /// the new thread work StartWatching end after create watcher this bool variable 
        /// keep running a while on end of StartWatching function
        /// for kill the thread set the keep running to false is enough
        /// </summary>
        public bool keepRunning = true;
        /// <summary>
        /// The black process list contain list every process we must kill it
        /// </summary>
        public List<string> BlackProcessList;
        /// <summary>
        /// The MPMWD is object of process watcher database publicly on this class
        /// use at _m_Process_Manager_Watcher for init
        /// use at AddProcessToBlackList for add a process name to black process list
        /// use at RemoveProcessFromBlackList for remove a process name from black process list
        /// </summary>
        _m_Process_Manager_Watcher_DB mpmwd = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Process_Manager_Watcher"/> class.
        /// </summary>
        public _m_Process_Manager_Watcher()
        {
            mpmwd=new _m_Process_Manager_Watcher_DB();
            BlackProcessList = mpmwd.GetAllProcessName();
        }
        /// <summary>
        /// Starts the watching.
        /// </summary>
        public void StartWatching()
        {
            try
            {
                ManagementEventWatcher ProcessStartTrace = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
                ProcessStartTrace.EventArrived += ProcessStartTrace_Event;
                ProcessStartTrace.Start();
                ManagementEventWatcher ProcessStopTrace = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
                ProcessStopTrace.EventArrived += ProcessStopTrace_Event;
                ProcessStopTrace.Start();

                while (keepRunning)
                {
                    ///<description>
                    ///when application is form base this work for anti freez message queue 
                    ///on command base program this line is no mean
                    ///</description>
                    Thread.Sleep(100);
                    System.Windows.Forms.Application.DoEvents();
                }
                ProcessStartTrace.Stop();
                ProcessStopTrace.Stop();
            }catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// Handles the Event event of the ProcessStopTrace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArrivedEventArgs"/> instance containing the event data.</param>
        private void ProcessStopTrace_Event(object sender, EventArrivedEventArgs e)
        {
            //return killed process report if it is killed from server
            //TODO complete when main module started
        }
        /// <summary>
        /// Handles the Event event of the ProcessStartTrace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArrivedEventArgs"/> instance containing the event data.</param>
        private void ProcessStartTrace_Event(object sender, EventArrivedEventArgs e)
        {
            string NewProcessName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            foreach (string BlackProcess in BlackProcessList)
            {
                if(BlackProcess.ToLower()==NewProcessName.ToLower())
                {
                    _m_Process_Manager.OnKillProcess(int.Parse(e.NewEvent.Properties["ProcessID"].Value.ToString()));
                }
            }
        }
    }
}
