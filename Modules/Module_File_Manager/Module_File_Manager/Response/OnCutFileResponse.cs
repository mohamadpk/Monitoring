using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnCutFileResponse
    {
        public string Target_Source;
        public string Target_Destination;
        public bool Sucsess;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
