using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Process_Manager.Response
{
    public class OnGetProcessListResponse
    {
        public List<Process> process = new List<Process>();
        public class Process
        {
            public int Id;
            public string ProcessOwner;
            public string ProcessName;
            public int BasePriority;
            public string MainModule;
            public IntPtr MainWindowHandle;
            public DateTime StartTime;
        }
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
