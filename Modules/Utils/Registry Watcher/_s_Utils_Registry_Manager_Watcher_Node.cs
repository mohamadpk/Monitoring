using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Utils.Registry_Watcher
{
    public class _s_Utils_Registry_Manager_Watcher_Node
    {
        string _Root;
        public string Root
        {
            get { return _Root; }
            set { _Root = value; }
        }

        string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
    }
}
