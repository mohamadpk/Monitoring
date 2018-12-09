using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info.Response
{
    /// <summary>
    /// node for get addons list info from all browser used on Module_Browser_Info main class and serialized for server
    /// </summary>
    public class OnGetAllBrowserAddonListResponse
    {
        /// <summary>
        /// The list of addon info
        /// </summary>
        public List<AddonInfo> addoninfos = new List<AddonInfo>();
        /// <summary>
        /// class is involved the addon name and target browser
        /// </summary>
        public class AddonInfo
        {
            public string TargetBrowserName;
            public string AddonName;
        }
        /// <summary>
        /// The errors if happened
        /// </summary>
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
