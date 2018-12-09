using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info.Response
{
    /// <summary>
    /// list of all installed browser on system used on Module_Browser_Info main class and serialized for server
    /// </summary>
    public class OnGetBrowserListResponse
    {
        public List<string> BrowserList = new List<string>();
        /// <summary>
        /// The errors if happened
        /// </summary>
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
