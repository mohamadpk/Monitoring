using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.Port_Manager.Response
{
    public class OnDisablePortByFireWallResponse
    {
        public int Port;
        public string ruleName;
        public string Protocol;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
