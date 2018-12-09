using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.Port_Manager.Response
{
    public class OnGetNetStatPortsResponse
    {
        public List<Connection> Connections = new List<Connection>();
        public class Connection {
            public string LocalAddress;
            public string Protocol;
            public string RemoteAddress;
            public string Port;
            public string ProcessName;
        }
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
