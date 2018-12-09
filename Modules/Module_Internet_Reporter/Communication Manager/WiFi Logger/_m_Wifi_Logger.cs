using Module_Internet_Reporter.Communication_Manager.WiFi_Logger.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Module_Internet_Reporter.Communication_Manager.WiFi_Logger
{
    public class _m_Wifi_Logger
    {
        /// <summary>
        /// The mww is object of wifi watcher publiclly on this class
        /// use at OnStartWifiWatcher for init
        /// use at OnStopWifiWatcher for un init
        /// </summary>
        static WiFi_Logger._m_WiFi_Watcher mww = null;
        /// <summary>
        /// Called when [start wifi watcher].
        /// </summary>
        /// <returns>OnStartWifiWatcherResponse object.if successful return a "WifiWatcherStarted" Message if File watcher is already running return "WifiWatcherStartedBefore" message else return error</returns>
        public static OnStartWifiWatcherResponse OnStartWifiWatcher()
        {
            OnStartWifiWatcherResponse ROnStartWifiWatcherResponse = new OnStartWifiWatcherResponse();
            try
            {
                if (mww == null)
                {
                    mww = new _m_WiFi_Watcher();
                    Thread WifiWatcherThread = new Thread(new ThreadStart(mww.StartWatching));
                    WifiWatcherThread.Start();
                    ROnStartWifiWatcherResponse.Description = "WifiWatcherStarted";
                }
                else
                {
                    ROnStartWifiWatcherResponse.Description = "WifiWatcherStartedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStartWifiWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartWifiWatcherResponse;
        }
        /// <summary>
        /// Called when [stop Wifi watcher].
        /// </summary>
        /// <returns>OnStopWifiWatcherResponse object.if successful return a "WifiWatcherStoped" Message if File watcher is already stoped or not created ergo return "WifiWatcherStopedBefore" message else return error</returns>
        public static OnStopWifiWatcherResponse OnStopWifiWatcher()
        {
            OnStopWifiWatcherResponse ROnStopWifiWatcherResponse = new OnStopWifiWatcherResponse();
            try
            {
                if (mww != null)
                {
                    mww.keepRunning = false;
                    mww = null;
                    ROnStopWifiWatcherResponse.Description = "WifiWatcherStoped";
                }
                else
                {
                    ROnStopWifiWatcherResponse.Description = "WifiWatcherStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopWifiWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopWifiWatcherResponse;
        }
    }
}
