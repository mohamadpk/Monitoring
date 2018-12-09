using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info
{
    /// <summary>
    /// best description for cookies
    /// </summary>
    public class BrowserCookie_Node
    {
        public string baseDomain;
        public string name;
        public string value;
        public DateTime expiry;
        public DateTime creationTime;
        public DateTime lastAccessed;
    }
}
