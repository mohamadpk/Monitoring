using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Communication_Manager.Site_Manager.Response
{
    public class OnDisableSiteResponse
    {
        public string Host;
        public string Description;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
