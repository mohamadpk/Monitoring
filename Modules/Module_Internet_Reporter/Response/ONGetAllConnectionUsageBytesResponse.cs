using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Response
{
    public class ONGetAllConnectionUsageBytesResponse
    {
        public List<Adapter> Adapters = new List<Adapter>();
        public class Adapter
        {
            public ulong Sent;
            public ulong Recv;
            public string AdapterName;
        }
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
