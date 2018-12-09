using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Process_Manager.Response
{
    public class OnKillProcessResponse
    {
        public int Pid;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
