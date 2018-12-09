using Module_Internet_Reporter.Communication_Manager.Sniffer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Module_Internet_Reporter.Communication_Manager.Sniffer
{
    public class _m_Sniffer
    {
        /// <summary>
        /// The sms is object of sniffer watcher publiclly on this class
        /// use at OnStartSnifferWatcher for init
        /// use at OnStopSnifferWatcher for un init
        /// </summary>
        static Sniffer._m_Sniffer_Watcher sms = null;
        /// <summary>
        /// Called when [start sniffer watcher].
        /// </summary>
        /// <returns>OnStartSnifferWatcherResponse object.if successful return a "SnifferWatcherStarted" Message if File watcher is already running return "SnifferWatcherStartedBefore" message else return error</returns>
        public static OnStartSnifferWatcherResponse OnStartSnifferWatcher()
        {
            OnStartSnifferWatcherResponse ROnStartSnifferWatcherResponse = new OnStartSnifferWatcherResponse();
            try
            {
                if (sms == null)
                {
                    sms = new Sniffer._m_Sniffer_Watcher();
                    Thread SnifferWatcherThread = new Thread(new ThreadStart(sms.StartWatching));
                    SnifferWatcherThread.Start();
                    ROnStartSnifferWatcherResponse.Description = "SnifferWatcherStarted";
                }
                else
                {
                    ROnStartSnifferWatcherResponse.Description = "SnifferWatcherStartedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStartSnifferWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartSnifferWatcherResponse;
        }
        /// <summary>
        /// Called when [stop sniffer watcher].
        /// </summary>
        /// <returns>OnStopWifiWatcherResponse object.if successful return a "WifiWatcherStoped" Message if File watcher is already stoped or not created ergo return "WifiWatcherStopedBefore" message else return error</returns>
        public static OnStopSnifferWatcherResponse OnStopSnifferWatcher()
        {
            OnStopSnifferWatcherResponse ROnStopSnifferWatcherResponse = new OnStopSnifferWatcherResponse();
            try
            {
                if (sms != null)
                {
                    sms.keepRunning = false;
                    sms = null;
                    ROnStopSnifferWatcherResponse.Description = "SnifferWatcherStoped";
                }
                else
                {
                    ROnStopSnifferWatcherResponse.Description = "SnifferWatcherStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopSnifferWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopSnifferWatcherResponse;
        }
    }
}
