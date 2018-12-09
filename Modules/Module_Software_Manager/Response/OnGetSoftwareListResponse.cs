using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Software_Manager.Response
{
    public class OnGetSoftwareListResponse
    {
        public List<Software> software = new List<Software>();
        public class Software
        {
            public string DisplayName;
            public string Publisher;
            public DateTime InstallDate;
            public string InstallLocation;
            public string Size;
            public string DisplayVersion;
            public string EstimatedSize;
            public string UninstallString;
            public string RegistryKeyAddress;
        }
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
