using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using Newtonsoft.Json.Linq;
using Module_Software_Manager.Response;

namespace Module_Software_Manager.Black_Software_Manager
{
    class _m_Black_Software_Manager
    {

        /// <summary>
        /// Adds the software to black list.
        /// </summary>
        /// <param name="BlackSoftwareName">Name of the black software.</param>
        public void AddSoftwareToBlackList(string BlackSoftwareName)
        {
            mbsmd.AddBlackSoftwareToDB(BlackSoftwareName);
            BlackSoftwareList.Add(BlackSoftwareName);
        }

        /// <summary>
        /// Removes the software from black list.
        /// </summary>
        /// <param name="BlackSoftwareName">Name of the black software.</param>
        /// <returns></returns>
        public bool RemoveSoftwareFromBlackList(string BlackSoftwareName)
        {
            mbsmd.RemoveBlackSoftwareFromDB(BlackSoftwareName);
            return BlackSoftwareList.Remove(BlackSoftwareName);
        }

        /// <summary>
        /// The keep running publicly on this class
        /// when a _m_Black_Software_Manager object created and StartWatching called on a new thread
        /// the new thread work StartWatching end after create watcher this bool variable 
        /// keep running a while on end of StartWatching function
        /// for kill the thread set the keep running to false is enough
        /// </summary>
        public bool keepRunning = true;

        /// <summary>
        /// The black software list contain list every software we must uninstall it
        /// </summary>
        public List<string> BlackSoftwareList;

        /// <summary>
        /// The MBSMD is object of black software manager database publicly on this class
        /// use at _m_Black_Software_Manager for init
        /// use at AddSoftwareToBlackList for add a software display name to black software list
        /// use at RemoveSoftwareFromBlackList for remove a software display name from black software list
        /// </summary>
        _m_Black_Software_Manager_DB mbsmd = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Black_Software_Manager"/> class.
        /// </summary>
        public _m_Black_Software_Manager()
        {
            mbsmd = new _m_Black_Software_Manager_DB();
            BlackSoftwareList = mbsmd.GetAllSoftwareName();
        }

        /// <summary>
        /// Starts the watching.
        /// </summary>
        public void StartWatching()
        {
            _m_Software_Manager msm = new _m_Software_Manager();
            while (keepRunning)
            {
                System.Threading.Thread.Sleep(1000);
                OnGetSoftwareListResponse SoftWareList = msm.OnGetSoftwareList();
                foreach(OnGetSoftwareListResponse.Software SoftwareName in SoftWareList.software)
                {
                    foreach(string BlackSoftwareDisplayName in BlackSoftwareList)
                    {
                        if (SoftwareName.DisplayName == BlackSoftwareDisplayName)
                        {
                            msm.OnUninstallSoftware(SoftwareName.UninstallString);
                        }
                    }      
                }
            }
        }
    }
}
