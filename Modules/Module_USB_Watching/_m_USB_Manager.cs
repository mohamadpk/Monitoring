using Module_USB_Watching.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Module_USB_Watching
{
    public class _m_USB_Manager
    {
        /// <summary>
        /// The mUSBw is object of usb watcher publicly on this class
        /// use at OnStartUSBWatcher for init
        /// use at OnStopUSBWatcher for un init
        /// use at OnAddUSBToBlackList for add a usb name to black list usb
        /// use at OnRemoveUSBFromBlackList for remove a usb name from black list usb
        /// </summary>
        static _m_USB_Watcher mUSBw = null;
        /// <summary>
        /// Called when [start usb watcher].
        /// </summary>
        /// <returns>OnStartUSBWatcherResponse object.if successful init object mUSBw of _m_USB_Watcher and then start it and return "USBWatcherStarted" message if the usb watcher is already running return "USBWatcherStartedBefore" message else return error</returns>
        public static OnStartUSBWatcherResponse OnStartUSBWatcher()
        {
            OnStartUSBWatcherResponse ROnStartUSBWatcherResponse = new OnStartUSBWatcherResponse();
            try
            {
                if (mUSBw == null)
                {
                    mUSBw = new _m_USB_Watcher();
                    Thread USBWatcherThread = new Thread(new ThreadStart(mUSBw.StartWatching));
                    USBWatcherThread.Start();
                    ROnStartUSBWatcherResponse.Description = "USBWatcherStarted";
                }
                else
                {
                    ROnStartUSBWatcherResponse.Description = "USBWatcherStartedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStartUSBWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartUSBWatcherResponse;
        }

        /// <summary>
        /// Called when [stop process watcher].
        /// </summary>
        /// <returns>OnStopUSBWatcherResponse object.if successful un init object mUSBw of _m_USB_Watcher and then stop it and return "USBWatcherStoped" message if the process watcher is already stoped return "USBWatcherStopedBefore" message else return error</returns>
        public static OnStopUSBWatcherResponse OnStopUSBWatcher()
        {
            OnStopUSBWatcherResponse ROnStopUSBWatcherResponse = new OnStopUSBWatcherResponse();
            try
            {
                if (mUSBw != null)
                {
                    mUSBw.keepRunning = false;
                    mUSBw = null;
                    ROnStopUSBWatcherResponse.Description = "USBWatcherStoped";
                }
                else
                {
                    ROnStopUSBWatcherResponse.Description = "USBWatcherStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopUSBWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopUSBWatcherResponse;
        }

        /// <summary>
        /// Called when [add usb to black list].
        /// </summary>
        /// <param name="BlackUSB">The black process.</param>
        /// <returns>OnAddUSBToBlackListResponse object.if successful return a "NewBlackUSBAdded" Message if process watcher is stoped or not created ergo return "USBWatcherIsNotRunning" message else return error</returns>
        public static OnAddUSBToBlackListResponse OnAddUSBToBlackList(_m_USB_Watcher_Disable_Enable_Node RuleToAdd)
        {
            OnAddUSBToBlackListResponse ROnAddUSBToBlackListResponse = new OnAddUSBToBlackListResponse();
            try
            {
                if (mUSBw != null)
                {
                    mUSBw.AddRuleToList(RuleToAdd);
                    ROnAddUSBToBlackListResponse.Description = "NewBlackUSBAdded";
                }
                else
                {
                    ROnAddUSBToBlackListResponse.Description = "USBWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnAddUSBToBlackListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnAddUSBToBlackListResponse;
        }
        /// <summary>
        /// Called when [remove usb from black list].
        /// </summary>
        /// <param name="BlackUSB">The black usb.</param>
        /// <returns>OnRemoveUSBFromBlackListResponse object.if successful return a "BlackProcessRemoved" Message and if the old balack process is not find for remove return "BlackProcessNotFindToRemoved" message if process watcher is stoped or not created ergo return "ProcessWatcherIsNotRunning" message else return error</returns>
        public static OnRemoveUSBFromBlackListResponse OnRemoveUSBFromBlackList(_m_USB_Watcher_Disable_Enable_Node RuleToRemove)
        {
            OnRemoveUSBFromBlackListResponse ROnRemoveUSBFromBlackListResponse = new OnRemoveUSBFromBlackListResponse();
            try
            {
                if (mUSBw != null)
                {
                    if (mUSBw.RemoveRuleFromList(RuleToRemove))
                    {
                        ROnRemoveUSBFromBlackListResponse.Description = "BlackUSBRemoved";
                    }
                    else
                    {
                        ROnRemoveUSBFromBlackListResponse.Description = "BlackUSBNotFindToRemoved";
                    }

                }
                else
                {
                    ROnRemoveUSBFromBlackListResponse.Description = "USBWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnRemoveUSBFromBlackListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnRemoveUSBFromBlackListResponse;
        }
    }
}
