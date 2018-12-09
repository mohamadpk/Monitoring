using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnSearchInFileContentResponse
    {
        public string KeyToSearch;
        public List<string> Paths = new List<string>();
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
