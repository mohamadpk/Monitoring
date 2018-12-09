using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Response
{
    public class OnCheckAvailableHostOrIPResponse
    {
        /// <summary>
        /// if the agent see host or ip with ping set to true else is false
        /// </summary>
        public bool DoYouSeeHim;
        public string HostOrIP;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
