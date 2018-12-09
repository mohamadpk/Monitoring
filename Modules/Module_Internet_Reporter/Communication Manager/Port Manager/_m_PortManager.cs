using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Module_Process_Manager;
using Module_Process_Manager.Process_Executer;
using System.Diagnostics;
using Utils;
using Module_Internet_Reporter.Communication_Manager.Port_Manager.Response;
using System.Reflection;
using Module_Process_Manager.Process_Executer.Respose;

namespace Module_Internet_Reporter.Communication_Manager.Port_Manager
{
    public class _m_PortManager
    {
        /// <summary>
        /// The Method That Parse The NetStat Output
        /// And Returns A List Of Port Objects
        /// </summary>
        /// <returns>all port  list by prosses name and protocol</returns>
        public static OnGetNetStatPortsResponse OnGetNetStatPorts()
        {
            OnGetNetStatPortsResponse ROnGetNetStatPortsResponse = new OnGetNetStatPortsResponse();
            try
            {
                OnExecuteProcessResponse  ExecuteResponse = _m_Process_Manager.OnExecuteProcess("netstat.exe", "-a -n -o", ProcessOutputType: _m_Child_Process.Enum_ProcessOutputType.Cmd, FullHide:false);

                string Output = ExecuteResponse.Output;
                string[] Connections = Regex.Split(Output, "\r\n");
                foreach (string Connection in Connections)
                {
                    string[] tokens = Regex.Split(Connection, "\\s+");
                    if (tokens.Length > 4 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP")))
                    {
                        OnGetNetStatPortsResponse.Connection connection = new OnGetNetStatPortsResponse.Connection();
                        string localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                        connection.LocalAddress = localAddress;
                        connection.Protocol= localAddress.Contains("1.1.1.1") ? String.Format("{0}v6", tokens[1]) : String.Format("{0}v4", tokens[1]);
                        connection.RemoteAddress = tokens[3];
                        connection.Port= localAddress.Split(':')[1];
                        connection.ProcessName = tokens[1] == "UDP" ? LookupProcess(Convert.ToInt16(tokens[4])) : LookupProcess(Convert.ToInt16(tokens[5]));
                        ROnGetNetStatPortsResponse.Connections.Add(connection);
                    }
                }


            }
            catch (Exception ex)
            {
                ROnGetNetStatPortsResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnGetNetStatPortsResponse;
        }
        /// <summary>
        /// this function use for take name of prossess  from ID
        /// </summary>
        /// <param name = "pid" > prosses ID</param>
        /// <returns>Prossess name</returns>
        public static string LookupProcess(int pid)
        {
            string procName;
            try { procName = Process.GetProcessById(pid).ProcessName; }
            catch (Exception) {
                procName = "";
            }
            return procName;
        }

        /// <summary>
        /// Called when [disable port by fire wall].
        /// </summary>
        /// <param name="ruleName">Name of the rule.</param>
        /// <param name="dir">The dir.</param>
        /// <param name="Protocol">The protocol.</param>
        /// <param name="Port">The port.</param>
        /// <param name="Profile">The profile.</param>
        /// <returns></returns>
        public OnDisablePortByFireWallResponse OnDisablePortByFireWall(string ruleName, string dir, string Protocol, int Port, string Profile)
        {
            OnDisablePortByFireWallResponse ROnDisablePortByFireWallResponse = new OnDisablePortByFireWallResponse();
            ROnDisablePortByFireWallResponse.Port = Port;
            ROnDisablePortByFireWallResponse.Protocol = Protocol;
            ROnDisablePortByFireWallResponse.ruleName = ruleName;

            try
            {
                string CommandToExecute = String.Format("advfirewall firewall add rule name=\"{0}\" dir={1} action=block protocol={2} localport={3} profile={4}", ruleName, dir, Protocol, Port, Profile);

                _m_Process_Manager.OnExecuteProcess("netsh.exe", CommandToExecute, ProcessOutputType: _m_Child_Process.Enum_ProcessOutputType.Cmd, FullHide: false);
            }
            catch (Exception ex)
            {
                ROnDisablePortByFireWallResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnDisablePortByFireWallResponse;
        }
        /// <summary>
        /// Called when [enable port by fire wall].
        /// </summary>
        /// <param name="ruleName">Name of the rule.</param>
        /// <param name="dir">The dir.</param>
        /// <param name="Protocol">The protocol.</param>
        /// <param name="Port">The port.</param>
        /// <param name="Profile">The profile.</param>
        /// <returns></returns>
        public OnEnablePortByFireWallResponse OnEnablePortByFireWall(string ruleName, string dir, string Protocol, int Port, string Profile)
        {
            OnEnablePortByFireWallResponse ROnEnablePortByFireWallResponse = new OnEnablePortByFireWallResponse();
            ROnEnablePortByFireWallResponse.Port = Port;
            ROnEnablePortByFireWallResponse.Protocol = Protocol;
            ROnEnablePortByFireWallResponse.ruleName = ruleName;
            try
            {
                string CommandToExecute = String.Format("advfirewall firewall add rule name=\"{0}\" dir={1} action=allow protocol={2} localport={3} profile={4}", ruleName, dir, Protocol, Port, Profile);
                _m_Process_Manager.OnExecuteProcess("netsh.exe", CommandToExecute, ProcessOutputType: _m_Child_Process.Enum_ProcessOutputType.Cmd, FullHide: false);
            }
            catch (Exception ex)
            {
                ROnEnablePortByFireWallResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnEnablePortByFireWallResponse;
        }
    }
}
