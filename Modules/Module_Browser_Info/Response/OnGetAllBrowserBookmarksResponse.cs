using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info.Response
{
    /// <summary>
    /// node for get bookmark list and history list info from all browser used on Module_Browser_Info main class and serialized for server
    /// </summary>
    public class OnGetAllBrowserBookmarksResponse
    {
        /// <summary>
        /// The list of bookmark and history 
        /// </summary>
        public List<BookmarkInfo> browserbookmarks = new List<BookmarkInfo>();
        /// <summary>
        /// class is involved the browser history node(url,title,create,lastvisit) and target browser
        /// </summary>
        public class BookmarkInfo
        {
            public string TargetBrowserName;
            public BrowserHistory_Node bookmarkNode;
        }
        /// <summary>
        /// The errors if happened
        /// </summary>
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
