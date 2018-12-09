using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnExecuteResponse
    {
        public string Target_File;
        public string Arguments;
        public int Pid;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
