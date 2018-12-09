using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utils;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Module_Process_Manager.Process_Executer;
using System.Management;
using System.Threading;
using Module_Process_Manager.Response;
using System.Reflection;
using Module_Process_Manager.Process_Executer.Respose;

namespace Module_Process_Manager
{
    /// <summary>
    /// The process manager module main
    /// </summary>
    public class _m_Process_Manager
    {
        /// <summary>
        /// The MPMW is object of process watcher publicly on this class
        /// use at OnStartProcessWatcher for init
        /// use at OnStopProcessWatcher for un init
        /// use at OnAddProcessToBlackList for add a process name to black list process
        /// use at OnRemoveProcessFromBlackList for remove a process name from black list process
        /// </summary>
        static Process_Watcher._m_Process_Manager_Watcher mpmw = null;


        /// <summary>
        /// A native win 32 api Opens the process token.
        /// </summary>
        /// <param name="ProcessHandle">The process handle.</param>
        /// <param name="DesiredAccess">The desired access.</param>
        /// <param name="TokenHandle">The token handle.</param>
        /// <returns>return bool for standard success or not and return out TokenHandle from process handle </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        /// <summary>
        /// A native win 32 api Closes the handle.
        /// </summary>
        /// <param name="HandleOfObject">The handle of object.</param>
        /// <returns>return bool if object pointer is closed</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr HandleOfObject);

        /// <summary>
        /// Called when [execute process].
        /// call direct the static ExecuteChildProcess function at _m_Child_Process
        /// </summary>
        /// <param name="Target_Executable">The target executable.</param>
        /// <param name="Parametrs">The parametrs.</param>
        /// <param name="ProcessParametersType">Type of the process parameters.</param>
        /// <param name="NoWindow">if set to <c>true</c> [no window].</param>
        /// <param name="FullHide">if set to <c>true</c> [full hide].</param>
        /// <param name="ReturnOutput">if set to <c>true</c> [return output].</param>
        /// <param name="ProcessOutputType">Type of the process output.</param>
        /// <param name="OutputFile">The output file.</param>
        /// <returns>json object.if successful return output of executed process else return error</returns>
        public static OnExecuteProcessResponse OnExecuteProcess(string Target_Executable,string Parametrs, _m_Child_Process.Enum_ProcessParametersType ProcessParametersType = _m_Child_Process.Enum_ProcessParametersType.Argument,bool NoWindow=true,bool FullHide=true,bool ReturnOutput=true, _m_Child_Process.Enum_ProcessOutputType ProcessOutputType = _m_Child_Process.Enum_ProcessOutputType.File,string OutputFile="output.txt")
        {
            _m_Child_Process mcp = new _m_Child_Process(); 
            return mcp.ExecuteChildProcess(Target_Executable,Parametrs,ProcessParametersType,NoWindow,FullHide,ReturnOutput,ProcessOutputType,OutputFile);
        }
        /// <summary>
        /// Called when [execute command].
        /// call direct the static ExecuteCmd function at _m_Child_Process
        /// write command directly to standard stdin
        /// </summary>
        /// <param name="Command">The command.</param>
        /// <returns>json object.if successful return output of executed standard stdout else return error</returns>
        public static OnExecuteProcessResponse OnExecuteCmd(string Command)
        {
            return _s_Child_Cmd.ExecuteCmd(Command);
        }

        /// <summary>
        /// Called when [get process list].
        /// </summary>
        /// <returns>OnGetProcessListResponse object.if successful return a array of process running on target include important information from each one else return error</returns>
        public static OnGetProcessListResponse OnGetProcessList()
        {
                OnGetProcessListResponse JOnGetProcessList = new OnGetProcessListResponse();
                Process[] ProcessList = Process.GetProcesses();
            Utils._s_Utils_ErrosHandling ErrorHandling = new Utils._s_Utils_ErrosHandling();
            foreach (Process process in ProcessList)
            {
                OnGetProcessListResponse.Process JProcess = new OnGetProcessListResponse.Process();
                
                ErrorHandling.OnErrorResumeNext(
                 () => { JProcess.Id = process.Id; },
                 () => { JProcess.ProcessOwner = GetProcessUser(process); },
                 () => { JProcess.ProcessName = process.ProcessName; },
                  () => { JProcess.BasePriority = process.BasePriority; },
                   () => { JProcess.MainModule = process.MainModule.FileName; },
                  () => { JProcess.MainWindowHandle = process.MainWindowHandle; },
                  () => { JProcess.StartTime = process.StartTime; },
                   () => { JOnGetProcessList.process.Add(JProcess); }
                );
            }
            JOnGetProcessList.Errors = ErrorHandling;
            return JOnGetProcessList;
        }
        /// <summary>
        /// Gets the process user.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>if successful return owner of process else return null</returns>
        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                System.Security.Principal.WindowsIdentity wi = new System.Security.Principal.WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }
        /// <summary>
        /// Called when [kill process].
        /// </summary>
        /// <param name="ProcessIdToKill">The process identifier to kill.</param>
        /// <returns>OnKillProcessResponse object</returns>
        public static OnKillProcessResponse OnKillProcess(int ProcessIdToKill)
        {
            OnKillProcessResponse ROnKillProcessResponse = new OnKillProcessResponse();
            ROnKillProcessResponse.Pid = ProcessIdToKill;//response to server for what process you try to kill 
            try
            {
                Process ProcessToKill = Process.GetProcessById(ProcessIdToKill);
                ProcessToKill.Kill();
                
            }
            catch (Exception ex)
            {
                ROnKillProcessResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnKillProcessResponse;
        }

        /// <summary>
        /// Called when [start process watcher].
        /// </summary>
        /// <returns>OnStartProcessWatcherResponse object.if successful init object mpmw of _m_Process_Manager_Watcher and then start it and return "ProcessWatcherStarted" message if the process watcher is already running return "ProcessWatcherStartedBefore" message else return error</returns>
        public static OnStartProcessWatcherResponse OnStartProcessWatcher()
        {
            OnStartProcessWatcherResponse ROnStartProcessWatcherResponse = new OnStartProcessWatcherResponse();
            try
            {
                if (mpmw == null)
                {
                    mpmw =new  Process_Watcher._m_Process_Manager_Watcher();
                    Thread ProcessWatcherThread = new Thread(new ThreadStart(mpmw.StartWatching));
                    ProcessWatcherThread.Start();
                    ROnStartProcessWatcherResponse.Description= "ProcessWatcherStarted";
                }
                else
                {
                    ROnStartProcessWatcherResponse.Description = "ProcessWatcherStartedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStartProcessWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartProcessWatcherResponse;
        }

        /// <summary>
        /// Called when [stop process watcher].
        /// </summary>
        /// <returns>OnStopProcessWatcherResponse object.if successful un init object mpmw of _m_Process_Manager_Watcher and then stop it and return "ProcessWatcherStoped" message if the process watcher is already stoped return "ProcessWatcherStopedBefore" message else return error</returns>
        public static OnStopProcessWatcherResponse OnStopProcessWatcher()
        {
            OnStopProcessWatcherResponse ROnStopProcessWatcherResponse = new OnStopProcessWatcherResponse();
            try
            {
                if (mpmw != null)
                {
                    mpmw.keepRunning = false;
                    mpmw = null;
                    ROnStopProcessWatcherResponse.Description = "ProcessWatcherStoped";
                }
                else
                {
                    ROnStopProcessWatcherResponse.Description = "ProcessWatcherStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopProcessWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopProcessWatcherResponse;
        }

        /// <summary>
        /// Called when [add process to black list].
        /// </summary>
        /// <param name="BlackProcess">The black process.</param>
        /// <returns>OnAddProcessToBlackListResponse object.if successful return a "NewBlackProcessAdded" Message if process watcher is stoped or not created ergo return "ProcessWatcherIsNotRunning" message else return error</returns>
        public static OnAddProcessToBlackListResponse OnAddProcessToBlackList(string BlackProcess)
        {
            OnAddProcessToBlackListResponse ROnAddProcessToBlackListResponse = new OnAddProcessToBlackListResponse();
            try
            {
                if (mpmw != null)
                {
                    mpmw.AddProcessToBlackList(BlackProcess);
                    ROnAddProcessToBlackListResponse.Description = "NewBlackProcessAdded";
                }
                else
                {
                    ROnAddProcessToBlackListResponse.Description = "ProcessWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnAddProcessToBlackListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnAddProcessToBlackListResponse;
        }
        /// <summary>
        /// Called when [remove process from black list].
        /// </summary>
        /// <param name="BlackProcess">The black process.</param>
        /// <returns>OnRemoveProcessFromBlackListResponse object.if successful return a "BlackProcessRemoved" Message and if the old balack process is not find for remove return "BlackProcessNotFindToRemoved" message if process watcher is stoped or not created ergo return "ProcessWatcherIsNotRunning" message else return error</returns>
        public static OnRemoveProcessFromBlackListResponse OnRemoveProcessFromBlackList(string BlackProcess)
        {
            OnRemoveProcessFromBlackListResponse ROnRemoveProcessFromBlackListResponse = new OnRemoveProcessFromBlackListResponse();
            try
            {
                if (mpmw != null)
                {
                    if (mpmw.RemoveProcessFromBlackList(BlackProcess))
                    {
                        ROnRemoveProcessFromBlackListResponse.Description = "BlackProcessRemoved";
                    }
                    else
                    {
                        ROnRemoveProcessFromBlackListResponse.Description = "BlackProcessNotFindToRemoved";
                    }

                }
                else
                {
                    ROnRemoveProcessFromBlackListResponse.Description = "ProcessWatcherIsNotRunning";
                }              
            }
            catch (Exception ex)
            {
                ROnRemoveProcessFromBlackListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnRemoveProcessFromBlackListResponse;
        }
    }
}
