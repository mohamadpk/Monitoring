using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Process_Manager.Process_Executer.Respose
{
    public class OnExecuteProcessResponse
    {
        public string Target_Executable;
        public string Output;
        public int Pid;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
