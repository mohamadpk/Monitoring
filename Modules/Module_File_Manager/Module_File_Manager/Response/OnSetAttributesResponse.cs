using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnSetAttributesResponse
    {
        public string File_Or_Directory;
        public FileAttributes Input_Attributes;
        public FileAttributes Output_Attributes;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
