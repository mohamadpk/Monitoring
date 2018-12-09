using Module_Internet_Reporter.Communication_Manager.Site_Manager.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.Site_Manager
{
    public class _m_SiteManager
    {
        /// <summary>
        /// Gets the hosts file address.
        /// </summary>
        /// <returns></returns>
        private static string GetHostsFileAddress()
        {
            var OSInfo = Environment.OSVersion;
            string pathpart = "hosts";
            if (OSInfo.Platform == PlatformID.Win32NT)
            {
                //is windows NT
                pathpart = "system32\\drivers\\etc\\hosts";
            }
            string hostfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), pathpart);
            return hostfile;
        }
        /// <summary>
        /// Called when [get hosts and ip].
        /// </summary>
        /// <returns></returns>
        public static OnGetHostsAndIPResponse OnGetHostsAndIP()
        {
            OnGetHostsAndIPResponse ROnGetHostsAndIPResponse = new OnGetHostsAndIPResponse();
            try { 
            string hostfile = GetHostsFileAddress();
            string[] Lines = System.IO.File.ReadAllLines(hostfile);
            foreach(string line in Lines)
            {
                if(!line.Contains("#"))
                {
                    string[] Host_And_IP = line.Split(" ".ToCharArray());
                    if(Host_And_IP.Length==2)
                    {
                        if(!ROnGetHostsAndIPResponse.hosts.ContainsKey(Host_And_IP[1]))
                            ROnGetHostsAndIPResponse.hosts.Add( Host_And_IP[1], Host_And_IP[0]);
                    }
                }
            }
            }
            catch (Exception ex)
            {
                ROnGetHostsAndIPResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnGetHostsAndIPResponse;
        }

        /// <summary>
        /// Called when [disable site].
        /// </summary>
        /// <param name="Host">The host.</param>
        /// <returns></returns>
        public OnDisableSiteResponse OnDisableSite(string Host)
        {
            OnDisableSiteResponse ROnDisableSiteResponse = new OnDisableSiteResponse();
            ROnDisableSiteResponse.Host = Host;
            try {
                string hostfile = GetHostsFileAddress();
                System.IO.File.AppendAllText(hostfile, "\r\n127.0.0.1 " + Host);
                ROnDisableSiteResponse.Description = "Succsess";
            }
            catch (Exception ex)
            {
                ROnDisableSiteResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnDisableSiteResponse;
        }
        /// <summary>
        /// Called when [enable site].
        /// </summary>
        /// <param name="Host">The host.</param>
        /// <returns></returns>
        public OnEnableSiteResponse OnEnableSite(string Host)
        {
            OnEnableSiteResponse ROnEnableSiteResponse = new OnEnableSiteResponse();
            ROnEnableSiteResponse.Host = Host;
            try
            {
            string hostfile = GetHostsFileAddress();
            string HostFileData = System.IO.File.ReadAllText(hostfile);
            HostFileData=HostFileData.Replace("\r\n127.0.0.1 " + Host, "");
            System.IO.File.WriteAllText(hostfile,HostFileData);
                ROnEnableSiteResponse.Description = "Succsess";
            }
            catch (Exception ex)
            {
                ROnEnableSiteResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnEnableSiteResponse;
        }
    }
}
