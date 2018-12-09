using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Browser_Info
{
    /// <summary>
    /// best description for every browser history node
    /// </summary>
    public class BrowserHistory_Node
    {
            public string url;
            public string title;
            public DateTime create;
            public DateTime lastvisit;
        /// <summary>
        /// Froms the unix time.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>return unix time from windows standard time. chrome browser type save time by unixtime on db</returns>
        public static DateTime FromUnixTime(long unixTime)
        {
            try {
                unixTime = unixTime / 1000000;
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddSeconds(unixTime);
            }catch(Exception ex)
            {
                var epoch = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch;
            }
        }
    }
}
