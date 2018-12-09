using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Module_File_Manager;
using Utils.Registry_Watcher;
using System.Management;
using Module_Process_Manager;
using Module_Software_Manager.Black_Software_Manager;
using Module_Software_Manager.Response;
using System.Reflection;
using System.Diagnostics;
using Module_File_Manager.Response;

namespace Module_Software_Manager
{
    /// <summary>
    /// The Software manager module main
    /// </summary>
    public class _m_Software_Manager
    {
        /// <summary>
        /// The MBSM is object of black software manager publicly on this class
        /// use at OnStartBlackSoftwareUnInstaller for init
        /// use at OnStopBlackSoftwareUnInstaller for un init
        /// use at OnAddSoftwareToBlackList for add a software display name to black list process
        /// use at OnRemoveSoftwareToBlackList for remove a software display name from black list software
        /// </summary>
        _m_Black_Software_Manager mbsm = null;
        /// <summary>
        /// A native win 32 api Regs the query information key.
        /// </summary>
        /// <param name="hkey">The hkey.</param>
        /// <param name="lpClass">The lp class.</param>
        /// <param name="lpcbClass">The LPCB class.</param>
        /// <param name="lpReserved">The lp reserved.</param>
        /// <param name="lpcSubKeys">The LPC sub keys.</param>
        /// <param name="lpcbMaxSubKeyLen">Length of the LPCB maximum sub key.</param>
        /// <param name="lpcbMaxClassLen">Length of the LPCB maximum class.</param>
        /// <param name="lpcValues">The LPC values.</param>
        /// <param name="lpcbMaxValueNameLen">Length of the LPCB maximum value name.</param>
        /// <param name="lpcbMaxValueLen">Length of the LPCB maximum value.</param>
        /// <param name="lpcbSecurityDescriptor">The LPCB security descriptor.</param>
        /// <param name="lpftLastWriteTime">The LPFT last write time.</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", EntryPoint = "RegQueryInfoKey", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        extern private static int RegQueryInfoKey(IntPtr hkey,out StringBuilder lpClass,ref int lpcbClass,int lpReserved,int lpcSubKeys,int lpcbMaxSubKeyLen,int lpcbMaxClassLen,int lpcValues,int lpcbMaxValueNameLen,int lpcbMaxValueLen,int lpcbSecurityDescriptor,out long lpftLastWriteTime);


        /// <summary>
        /// The registry uninstall keys
        /// </summary>
        static List<KeyValuePair<RegistryKey, string>> RegistryUninstallKeys = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Software_Manager"/> class.
        /// </summary>
        public _m_Software_Manager()
        {
            RegistryUninstallKeys = new List<KeyValuePair<RegistryKey, string>>();
            RegistryUninstallKeys.Add(new KeyValuePair<RegistryKey, string>(Microsoft.Win32.Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"));
            RegistryUninstallKeys.Add(new KeyValuePair<RegistryKey, string>(Microsoft.Win32.Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"));
            RegistryUninstallKeys.Add(new KeyValuePair<RegistryKey, string>(Microsoft.Win32.Registry.LocalMachine, @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"));
        }
        /// <summary>
        /// Called when [get software list].
        /// </summary>
        /// <returns>jarray object.if successful return a array of Software installed on system include important information from each one else return error</returns>
        public OnGetSoftwareListResponse OnGetSoftwareList()
        {
            OnGetSoftwareListResponse JOnGetSoftwareListResponse = new OnGetSoftwareListResponse();
            try
            {
                foreach (KeyValuePair<RegistryKey, string> RegistryUninstallKey in RegistryUninstallKeys)
                {
                    RegistryKey KeyObject = null;
                    string[] Keys = Utils._s_Utils_Registry_Manager.ReadAllKey(ref KeyObject, RegistryUninstallKey.Key, RegistryUninstallKey.Value);
                    if (Keys == null)
                        continue;
                    foreach (string SubkeyName in Keys)
                    {
                        RegistryKey SubKeyObject = null;
                        string[] SubKeys = Utils._s_Utils_Registry_Manager.ReadAllKey(ref SubKeyObject, KeyObject, SubkeyName);
                        if (SubKeyObject.GetValue("UninstallString") != null)
                        {
                            if (SubKeyObject.GetValue("SystemComponent") == null)
                            {
                                if (SubKeyObject.GetValue("ParentDisplayName") == null)
                                    if (SubKeyObject.GetValue("DisplayName") != null)
                                    {
                                        OnGetSoftwareListResponse.Software JSoftware = new OnGetSoftwareListResponse.Software();
                                        JSoftware.DisplayName = SubKeyObject.GetValue("DisplayName").ToString();
                                        JSoftware.Publisher = SubKeyObject.GetValue("Publisher")?.ToString();
                                        JSoftware.InstallDate = GetSoftwareInstallTime(SubKeyObject);

                                        JSoftware.InstallLocation = SubKeyObject.GetValue("InstallLocation")?.ToString();
                                        JSoftware.Size = SubKeyObject.GetValue("Size")?.ToString();
                                        JSoftware.DisplayVersion = SubKeyObject.GetValue("DisplayVersion")?.ToString();
                                        JSoftware.EstimatedSize = SubKeyObject.GetValue("EstimatedSize")?.ToString();
                                        JSoftware.UninstallString = SubKeyObject.GetValue("UninstallString")?.ToString();
                                        JSoftware.RegistryKeyAddress = SubKeyObject.Name;
                                        JOnGetSoftwareListResponse.software.Add(JSoftware);
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JOnGetSoftwareListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return JOnGetSoftwareListResponse;
        }

        /// <summary>
        /// Gets the software install time.
        /// </summary>
        /// <param name="SubKeyObject">The sub key object.</param>
        /// <returns>if successful return DateTime of registry key created time else return null</returns>
        private DateTime GetSoftwareInstallTime(RegistryKey SubKeyObject)
        {
            long lpftLastWriteTime;
            StringBuilder classStr = new StringBuilder(255);
            int classSize = classStr.Capacity + 1;
            RegQueryInfoKey(SubKeyObject.Handle.DangerousGetHandle(), out classStr, ref classSize, 0, 0, 0, 0, 0, 0, 0, 0, out lpftLastWriteTime);
            return DateTime.FromFileTimeUtc(lpftLastWriteTime);
        }
        /// <summary>
        /// Called when [serach software].
        /// </summary>
        /// <param name="Filter_Key">The filter key.</param>
        /// <param name="Filter_Value">The filter value.</param>
        /// <returns>json object.if successful return a array of Software installed on system by filtered include important information from each one else return error</returns>
        public OnGetSoftwareListResponse OnSerachSoftware(string Filter_Key, string Filter_Value)
        {
            OnGetSoftwareListResponse OnSerachSoftware = new OnGetSoftwareListResponse();
            try
            {              
                Response.OnGetSoftwareListResponse JSoftwares = OnGetSoftwareList();
                foreach (Response.OnGetSoftwareListResponse.Software JSoftware in JSoftwares.software)
                {
                    Type Filter_Key_Field = typeof(Module_Software_Manager.Response.OnGetSoftwareListResponse.Software);
                    FieldInfo Filter_Key_Field_Info= Filter_Key_Field.GetField(Filter_Key);
                    if(Filter_Key_Field_Info != null)
                    {
                        Type Target_Key_Field = JSoftware.GetType();
                        FieldInfo Target_Key_Field_Info = Target_Key_Field.GetField(Filter_Key);
                        object retvalue=Target_Key_Field_Info.GetValue(JSoftware);
                        if (retvalue?.ToString()== Filter_Value)
                        {
                            OnSerachSoftware.software.Add(JSoftware);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                OnSerachSoftware.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return OnSerachSoftware;
        }

        /// <summary>
        /// Called when [uninstall software].
        /// </summary>
        /// <param name="UninstallString">The uninstall string.</param>
        /// <returns>json object.if successful return a array of Software match UninstallString registry key we try to uninstall them else return error</returns>
        public OnGetSoftwareListResponse OnUninstallSoftware(string UninstallString)
        {
            OnGetSoftwareListResponse JOnUninstallSoftware = new OnGetSoftwareListResponse();
            try
            {
                _m_File_Manager mfm = new _m_File_Manager();
                OnGetSoftwareListResponse JAllSoftwareFind = OnSerachSoftware("UninstallString", UninstallString);
                JOnUninstallSoftware = JAllSoftwareFind;
                foreach (OnGetSoftwareListResponse.Software JTarget in JAllSoftwareFind.software)
                {
                    string InstallLocation = "";
                    if (JTarget.InstallLocation != null)
                    {
                        InstallLocation = JTarget.InstallLocation;

                    }
                    else//extract InstallLocation from UninstallString
                    {
                        InstallLocation = GetPathFromFullPath(UninstallString);
                    }

                    KillOpenedProcessFromPath(InstallLocation);
                    KillDesktopLNKFromPath(InstallLocation);
                    mfm.OnDeleteDirectory(InstallLocation);
                    Utils._s_Utils_Registry_Manager.DeleteKey(JTarget.RegistryKeyAddress);
                }             
            }
            catch (Exception ex)
            {
                JOnUninstallSoftware.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return JOnUninstallSoftware;
        }
        /// <summary>
        /// Kills the desktop LNK from path.
        /// </summary>
        /// <param name="InstallLocation">The install location.</param>
        /// <returns>if has lnk return true other return false</returns>
        private bool KillDesktopLNKFromPath(string InstallLocation)
        {
            bool HasLNK = false;
            string[] desktopPath = new string[2];
            desktopPath[0]=Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            desktopPath[1] = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            foreach(string desk in desktopPath)
            {
                _m_File_Manager mfm = new _m_File_Manager();
                OnDirResponse Dirs = mfm.OnDir(desk);
                foreach (OnDirResponse.FileOrDir FileOrFolder in Dirs.fileOrdir)
                {
                    //if (FileOrFolder.Path.Contains("Resizer 5"))
                    //{

                    //}
                    if (FileOrFolder.Path.EndsWith(".lnk"))
                    {
                        string TempAddress = _m_File_Manager.GetShortcutTargetFile(FileOrFolder.Path);
                        string TargetAdress = "";
                        if (TempAddress.EndsWith("\\"))
                        {
                            TargetAdress = TempAddress;
                        }
                        else
                        {
                            TargetAdress = GetPathFromFullPath(TempAddress, true);
                        }

                        if (TargetAdress == InstallLocation)
                        {
                            System.IO.File.Delete(FileOrFolder.Path);
                        }
                    }
                }
            }


            return HasLNK;
        }
        /// <summary>
        /// Kills the opened process from path.
        /// </summary>
        /// <param name="Path">The path.</param>
        /// <returns>if folder has process return true other wise return false</returns>
        private bool KillOpenedProcessFromPath(string InstallLocation)
        {
            bool HasProcess = false;
            Module_Process_Manager.Response.OnGetProcessListResponse ProcessList = _m_Process_Manager.OnGetProcessList();
            foreach(Module_Process_Manager.Response.OnGetProcessListResponse.Process prc in ProcessList.process)
            {
                try {
                    string ProcessPath = GetPathFromFullPath(prc.MainModule,true);
                    if (ProcessPath == InstallLocation)
                    {
                        _m_Process_Manager.OnKillProcess(prc.Id);
                        HasProcess = true;
                    }
                }catch(Exception ex)
                {
                    continue;
                }
            }
            return HasProcess;
        }

        /// <summary>
        /// Gets the path from full path.
        /// </summary>
        /// <param name="FullPath">The full path.</param>
        /// <returns>return string of ful path exclude file name </returns>
        private string GetPathFromFullPath(string FullPath,bool WithBaclSlash=false)
        {
            int PathLen = FullPath.LastIndexOf("\\");
            string Path = FullPath.Substring(0, PathLen);
            if(WithBaclSlash)
            {
                Path += "\\";
            }
            return Path;
        }

        /// <summary>
        /// Called when [install software].
        /// </summary>
        /// <param name="InstallString">The install string.</param>
        /// <returns>OnInstallSoftwareResponse object.if successful return a executed process data output and processid else return error</returns>
        public OnInstallSoftwareResponse OnInstallSoftware(string InstallString)
        {
            OnInstallSoftwareResponse ROnInstallSoftwareResponse = new OnInstallSoftwareResponse();
            try
            {
                ROnInstallSoftwareResponse.Description = _m_Process_Manager.OnExecuteCmd(InstallString).Output;
            }
            catch (Exception ex)
            {
                ROnInstallSoftwareResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnInstallSoftwareResponse;
        }


        /// <summary>
        /// Called when [start black software un installer].
        /// </summary>
        /// <returns>OnStartBlackSoftwareUnInstallerResponse object.if successful init object mbsm of _m_Black_Software_Manager and then start it and return "BlackSoftwareUnInstallerStarted" message if the black process manager is already running return "BlackSoftwareUnInstallerStartedBefore" message else return error</returns>
        public OnStartBlackSoftwareUnInstallerResponse OnStartBlackSoftwareUnInstaller()
        {
            OnStartBlackSoftwareUnInstallerResponse ROnStartBlackSoftwareUnInstallerResponse = new OnStartBlackSoftwareUnInstallerResponse();
            try
            {
                if (mbsm == null)
                {
                    mbsm = new _m_Black_Software_Manager();
                    mbsm.StartWatching();
                    ROnStartBlackSoftwareUnInstallerResponse.Description = "BlackSoftwareUnInstallerStarted";
                }
                else
                {
                    ROnStartBlackSoftwareUnInstallerResponse.Description = "BlackSoftwareUnInstallerStartedBefore";
                }

            }
            catch (Exception ex)
            {
                ROnStartBlackSoftwareUnInstallerResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartBlackSoftwareUnInstallerResponse;
        }

        /// <summary>
        /// Called when [stop black software un installer].
        /// </summary>
        /// <returns>OnStopBlackSoftwareUnInstallerResponse object.if successful un init object mbsm of _m_Black_Software_Manager and then stop it and return "BlackSoftwareUnInstallerStoped" message if the black fostware manager is already stoped return "BlackSoftwareUnInstallerStopedBefore" message else return error</returns>
        public OnStopBlackSoftwareUnInstallerResponse OnStopBlackSoftwareUnInstaller()
        {
            OnStopBlackSoftwareUnInstallerResponse ROnStopBlackSoftwareUnInstallerResponse = new OnStopBlackSoftwareUnInstallerResponse();
            try
            {
                if (mbsm != null)
                {
                    mbsm.keepRunning = false;
                    mbsm = null;
                    ROnStopBlackSoftwareUnInstallerResponse.Description = "BlackSoftwareUnInstallerStoped";
                }
                else
                {
                    ROnStopBlackSoftwareUnInstallerResponse.Description= "BlackSoftwareUnInstallerStopedBefore";
                }

            }
            catch (Exception ex)
            {
                ROnStopBlackSoftwareUnInstallerResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopBlackSoftwareUnInstallerResponse;
        }

        /// <summary>
        /// Called when [add software to black list].
        /// </summary>
        /// <param name="BlackSoftware">The black software.</param>
        /// <returns>OnAddSoftwareToBlackListResponse object.if successful return a "NewBlackSoftwareAdded" Message if black software manager is stoped or not created ergo return "BlackSoftwareUnInstallerIsNotRunning" message else return error</returns>
        public OnAddSoftwareToBlackListResponse OnAddSoftwareToBlackList(string BlackSoftware)
        {
            OnAddSoftwareToBlackListResponse ROnAddSoftwareToBlackListResponse = new OnAddSoftwareToBlackListResponse();
            try
            {
                if (mbsm != null)
                {
                    mbsm.AddSoftwareToBlackList(BlackSoftware);
                    ROnAddSoftwareToBlackListResponse.Description = "NewBlackSoftwareAdded";
                }
                else
                {
                    ROnAddSoftwareToBlackListResponse.Description = "BlackSoftwareUnInstallerIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnAddSoftwareToBlackListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnAddSoftwareToBlackListResponse;
        }

        /// <summary>
        /// Called when [remove software to black list].
        /// </summary>
        /// <param name="BlackSoftware">The black software.</param>
        /// <returns>OnRemoveSoftwareFromBlackListResponse object.if successful return a "BlackSoftwareRemoved" Message and if the old balack software is not find for remove return "BlackSoftwareNotFindToRemoved" message if black software manager is stoped or not created ergo return "BlackSoftwareUnInstallerIsNotRunning" message else return error</returns>
        public OnRemoveSoftwareFromBlackListResponse OnRemoveSoftwareFromBlackList(string BlackSoftware)
        {
            OnRemoveSoftwareFromBlackListResponse ROnRemoveSoftwareFromBlackListResponse = new OnRemoveSoftwareFromBlackListResponse();
            try
            {
                if (mbsm != null)
                {
                    if (mbsm.RemoveSoftwareFromBlackList(BlackSoftware))
                    {
                        ROnRemoveSoftwareFromBlackListResponse.Description = "BlackSoftwareRemoved";
                    }
                    else
                    {
                        ROnRemoveSoftwareFromBlackListResponse.Description = "BlackSoftwareNotFindToRemoved";
                    }

                }
                else
                {
                    ROnRemoveSoftwareFromBlackListResponse.Description = "BlackSoftwareUnInstallerIsNotRunning";
                }

            }
            catch (Exception ex)
            {
                ROnRemoveSoftwareFromBlackListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnRemoveSoftwareFromBlackListResponse;
        }

    }
}
