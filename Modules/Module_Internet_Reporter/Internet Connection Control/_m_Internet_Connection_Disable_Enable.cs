using Module_Internet_Reporter.Internet_Connection_Control.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Module_Internet_Reporter.Internet_Connection_Control
{
    public class _m_Internet_Connection_Disable_Enable
    {

        /// <summary>
        /// The miccw is object of internet connection watcher publiclly on this class
        /// use at OnStartInternetConnectionWatcher for init
        /// use at OnStopInternetConnectionWatcher for un init
        /// use at OnAddFilterRuleToInternetConnectionWatcher for add a new rule
        /// a object of _m_Internet_Connection_Disable_Enable_Node class to Rule list
        /// use at OnRemoveRuleFromInternetConnectionWatcher for remove a folder address from filtered folder list
        /// </summary>
        static Internet_Connection_Control._m_Internet_Connection_Control_Watcher miccw = null;
        /// <summary>
        /// Called when [start internet connection watcher].
        /// </summary>
        /// <returns>OnStartInternetConnectionWatcherResponse object.if successful return a "InternetConnetionWatcherStarted" Message if File watcher is already running return "InternetConnetionWatcherStartedBefore" message else return error</returns>
        public static OnStartInternetConnectionWatcherResponse OnStartInternetConnectionWatcher()
        {
            OnStartInternetConnectionWatcherResponse ROnStartInternetConnectionWatcherResponse = new OnStartInternetConnectionWatcherResponse();
            try
            {
                if (miccw == null)
                {
                    miccw = new Internet_Connection_Control._m_Internet_Connection_Control_Watcher();
                    Thread InternetConnectionWatcherThread = new Thread(new ThreadStart(miccw.StartWatching));
                    InternetConnectionWatcherThread.Start();
                    ROnStartInternetConnectionWatcherResponse.Description = "InternetConnetionWatcherStarted";
                }
                else
                {
                    ROnStartInternetConnectionWatcherResponse.Description = "InternetConnetionWatcherStartedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStartInternetConnectionWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartInternetConnectionWatcherResponse;
        }

        /// <summary>
        /// Called when [stop internet connection watcher].
        /// </summary>
        /// <returns>OnStopInternetConnectionWatcherResponse object.if successful return a "InternetConnetionWatcherStoped" Message if File watcher is already stoped or not created ergo return "InternetConnetionWatcherStopedBefore" message else return error</returns>
        public static OnStopInternetConnectionWatcherResponse OnStopInternetConnectionWatcher()
        {
            OnStopInternetConnectionWatcherResponse ROnStopInternetConnectionWatcherResponse = new OnStopInternetConnectionWatcherResponse();
            try
            {
                if (miccw != null)
                {
                    miccw.keepRunning = false;
                    miccw = null;
                    ROnStopInternetConnectionWatcherResponse.Description = "InternetConnetionWatcherStoped";
                }
                else
                {
                    ROnStopInternetConnectionWatcherResponse.Description = "InternetConnetionWatcherStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopInternetConnectionWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopInternetConnectionWatcherResponse;
        }

        /// <summary>
        /// Called when [add rule to internet connection watcher].
        /// </summary>
        /// <param name="RuleToAdd">The new rule.</param>
        /// <returns>OnAddRuleToInternetConnectionWatcherResponse object.if successful return a "NewInternetConnectionRuleAdded" Message if File watcher is stoped or not created ergo return "InternetConnectionWatcherIsNotRunning" message else return error</returns>
        public static OnAddRuleToInternetConnectionWatcherResponse OnAddRuleToInternetConnectionWatcher(_m_Internet_Connection_Disable_Enable_Node RuleToAdd)
        {
            OnAddRuleToInternetConnectionWatcherResponse ROnAddRuleToInternetConnectionWatcherResponse = new OnAddRuleToInternetConnectionWatcherResponse();
            try
            {
                if (miccw != null)
                {
                    miccw.AddRuleToList(RuleToAdd);
                    ROnAddRuleToInternetConnectionWatcherResponse.Description = "NewInternetConnectionRuleAdded";
                }
                else
                {
                    ROnAddRuleToInternetConnectionWatcherResponse.Description = "InternetConnectionWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnAddRuleToInternetConnectionWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnAddRuleToInternetConnectionWatcherResponse;
        }
        /// <summary>
        /// Called when [remove rule from internet connection watcher].
        /// </summary>
        /// <param name="FilterdFolder">The filterd folder.</param>
        /// <returns>OnRemoveFilterFromFileWatcherResponse object.if successful return a "FilteredFolderRemoved" Message and if the old filter is not find for remove return "FilteredFolderNotFindToRemoved" message if File watcher is stoped or not created ergo return "FileWatcherIsNotRunning" message else return error</returns>
        public static OnRemoveRuleFromInternetConnectionWatcherResponse OnRemoveRuleFromInternetConnectionWatcher(_m_Internet_Connection_Disable_Enable_Node RuleToRemove)
        {
            OnRemoveRuleFromInternetConnectionWatcherResponse ROnRemoveRuleFromInternetConnectionWatcherResponse = new OnRemoveRuleFromInternetConnectionWatcherResponse();
            try
            {
                if (miccw != null)
                {
                    if (miccw.RemoveRuleFromList(RuleToRemove))
                    {
                        ROnRemoveRuleFromInternetConnectionWatcherResponse.Description = "InternetConnetionRuleRemoved";
                    }
                    else
                    {
                        ROnRemoveRuleFromInternetConnectionWatcherResponse.Description = "InternetConnetionRuleNotFindToRemoved";
                    }

                }
                else
                {
                    ROnRemoveRuleFromInternetConnectionWatcherResponse.Description = "InternetConnetionWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnRemoveRuleFromInternetConnectionWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnRemoveRuleFromInternetConnectionWatcherResponse;
        }



        /// <summary>
        /// Called when [disable internet connection].
        /// </summary>
        /// <param name="NetConnectionId">The net connection identifier.</param>
        /// <returns>OnDisableInternetConnectionResponse</returns>
        public static OnDisableInternetConnectionResponse OnDisableInternetConnection(string NetConnectionId)
        {
            OnDisableInternetConnectionResponse ROnDisableInternetConnectionResponse = new OnDisableInternetConnectionResponse();
            ROnDisableInternetConnectionResponse.NetConnectionId = NetConnectionId;
            try {
                ManagementObjectSearcher searchProcedure = GetNetConnectionIdList();
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    if (((string)item["NetConnectionId"]) == NetConnectionId)
                    { 
                        item.InvokeMethod("Disable", null);
                        ROnDisableInternetConnectionResponse.FoundAndInvoke =true;
                        break;
                    }
                }
            }catch(Exception ex)
            {
                ROnDisableInternetConnectionResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnDisableInternetConnectionResponse;
        }
        /// <summary>
        /// Called when [enable internet connection].
        /// </summary>
        /// <param name="NetConnectionId">The net connection identifier.</param>
        /// <returns>OnEnableInternetConnectionResponse</returns>
        public static OnEnableInternetConnectionResponse OnEnableInternetConnection(string NetConnectionId)
        {
            OnEnableInternetConnectionResponse ROnEnableInternetConnectionResponse = new OnEnableInternetConnectionResponse();
            ROnEnableInternetConnectionResponse.NetConnectionId = NetConnectionId;
            try {
                ManagementObjectSearcher searchProcedure = GetNetConnectionIdList();
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    if (((string)item["NetConnectionId"]) == NetConnectionId)
                    {
                        item.InvokeMethod("Enable", null);
                        ROnEnableInternetConnectionResponse.FoundAndInvoke = true;
                        break;
                    }
                }
            }catch(Exception ex)
            {
                ROnEnableInternetConnectionResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnEnableInternetConnectionResponse;
        }
        /// <summary>
        /// Called when [disable all internet connection].
        /// </summary>
        public static OnDisableAllInternetConnectionResponse OnDisableAllInternetConnection()
        {
            OnDisableAllInternetConnectionResponse ROnDisableAllInternetConnectionResponse = new OnDisableAllInternetConnectionResponse();
            try {
                ManagementObjectSearcher searchProcedure = GetNetConnectionIdList();
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    item.InvokeMethod("Disable", null);
                    ROnDisableAllInternetConnectionResponse.FoundAndInvoke=true;
                }
            }catch(Exception ex)
            {
                ROnDisableAllInternetConnectionResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnDisableAllInternetConnectionResponse;
        }
        /// <summary>
        /// Called when [enable all internet connection].
        /// </summary>
        public static OnEnableAllInternetConnectionResponse OnEnableAllInternetConnection()
        {
            OnEnableAllInternetConnectionResponse ROnEnableAllInternetConnectionResponse = new OnEnableAllInternetConnectionResponse();
            try {
                ManagementObjectSearcher searchProcedure = GetNetConnectionIdList();
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    item.InvokeMethod("Enable", null);
                    ROnEnableAllInternetConnectionResponse.FoundAndInvoke = true;
                }
            }catch(Exception ex)
            {
                ROnEnableAllInternetConnectionResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnEnableAllInternetConnectionResponse;
        }
        /// <summary>
        /// Called when [get net connection identifier names].
        /// </summary>
        /// <returns> return list of "Ethernet0" like strings</returns>
        public static OnGetNetConnectionIdNamesResponse OnGetNetConnectionIdNames()
        {
            OnGetNetConnectionIdNamesResponse ROnGetNetConnectionIdNamesResponse = new OnGetNetConnectionIdNamesResponse();
            try {
                ManagementObjectSearcher searchProcedure = GetNetConnectionIdList();
                foreach (ManagementObject item in searchProcedure.Get())
                {
                    ROnGetNetConnectionIdNamesResponse.GetNetConnectionIds.Add((string)item["NetConnectionId"]);
                }
            }catch(Exception ex)
            {
                ROnGetNetConnectionIdNamesResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnGetNetConnectionIdNamesResponse;
        }

        /// <summary>
        /// Gets the net connection identifier list.
        /// </summary>
        /// <returns></returns>
        private static ManagementObjectSearcher GetNetConnectionIdList()
        {
            SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
            return searchProcedure;
        }
    }
}
