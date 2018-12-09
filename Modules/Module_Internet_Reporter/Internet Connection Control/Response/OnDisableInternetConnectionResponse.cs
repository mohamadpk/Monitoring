using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Internet_Connection_Control.Response
{
    public class OnDisableInternetConnectionResponse
    {
        public string NetConnectionId;
        public bool FoundAndInvoke;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
