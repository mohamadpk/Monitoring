using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Utils;
using System.Management;
using Module_Internet_Reporter.Response;
using System.Reflection;
using System.Threading;
using Module_Internet_Reporter.Internet_Connection_Control;

namespace Module_Internet_Reporter
{
    public class _m_Internet_Checker
    {
        /// <summary>
        /// Internets the state of the get connected.
        /// </summary>
        /// <param name="Description">The description.</param>
        /// <param name="ReservedValue">The reserved value.</param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        /// <summary>
        /// Called when [check available host or ip].
        /// </summary>
        /// <param name="HostOrIP">The host.</param>
        /// <returns>OnCheckAvailableHostOrIPResponse </returns>
        public OnCheckAvailableHostOrIPResponse OnCheckAvailableHostOrIP(string HostOrIP)
        {
            OnCheckAvailableHostOrIPResponse ROnCheckAvailableHostOrIPResponse = new OnCheckAvailableHostOrIPResponse();
            ROnCheckAvailableHostOrIPResponse.HostOrIP = HostOrIP;
            Ping p = new Ping();
            try
            {
                
                PingReply reply = p.Send(HostOrIP, 3000);
                if (reply.Status == IPStatus.Success)
                {
                    ROnCheckAvailableHostOrIPResponse.DoYouSeeHim = true;

                }
                else
                {
                    ROnCheckAvailableHostOrIPResponse.DoYouSeeHim = false;
                }

            }
            catch (Exception ex)
            {
                ROnCheckAvailableHostOrIPResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnCheckAvailableHostOrIPResponse;
        }
        /// <summary>
        /// Called when [check active modem or lan connection].
        /// </summary>
        /// <returns>OnCheckActiveModemOrLANConnectionResponse </returns>
        public OnCheckActiveModemOrLANConnectionResponse OnCheckActiveModemOrLANConnection()
        {
            OnCheckActiveModemOrLANConnectionResponse ROnCheckActiveModemOrLANConnectionResponse = new OnCheckActiveModemOrLANConnectionResponse();
            int Desc;//see https://msdn.microsoft.com/en-us/library/windows/desktop/aa384702(v=vs.85).aspx
            try
            {
                ROnCheckActiveModemOrLANConnectionResponse.ActiveModemOrLAN = InternetGetConnectedState(out Desc, 0);
                ROnCheckActiveModemOrLANConnectionResponse.Description = Desc;
            }
            catch (Exception ex)
            {
                ROnCheckActiveModemOrLANConnectionResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnCheckActiveModemOrLANConnectionResponse;
        }

        /// <summary>
        /// Ons the get all connection usage bytes.
        /// </summary>
        /// <returns>ONGetAllConnectionUsageBytesResponse </returns>
        public ONGetAllConnectionUsageBytesResponse ONGetAllConnectionUsageBytes()
        {
            ONGetAllConnectionUsageBytesResponse RONGetAllConnectionUsageBytesResponse = new ONGetAllConnectionUsageBytesResponse();
            try {
                ManagementScope PowerShellNameSpaceScope = new ManagementScope("\\\\.\\ROOT\\StandardCimv2");
                SelectQuery wmiQuery = new SelectQuery("SELECT * From MSFT_NetAdapterStatisticsSettingData");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(PowerShellNameSpaceScope, wmiQuery);
                ManagementObjectCollection adaptersCollection = searcher.Get();
                foreach (ManagementObject Madapter in adaptersCollection)
                {
                    ONGetAllConnectionUsageBytesResponse.Adapter Adapter = new ONGetAllConnectionUsageBytesResponse.Adapter();
                    Adapter.AdapterName = (string)Madapter["Name"];
                    Adapter.Recv = System.UInt64.Parse(Madapter["ReceivedUnicastBytes"].ToString());
                    Adapter.Sent = System.UInt64.Parse(Madapter["SentUnicastBytes"].ToString());
                    RONGetAllConnectionUsageBytesResponse.Adapters.Add(Adapter);
                }
            }catch(Exception ex)
            {
                RONGetAllConnectionUsageBytesResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);

            }
            return RONGetAllConnectionUsageBytesResponse;
        }

    }
}
