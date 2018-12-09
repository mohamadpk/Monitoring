using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_Screen_Capture.Response
{
    public class OnUpdateReziseToResponse 
    {
        public string Description;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
        public System.Drawing.Size ReSizeTo;
    }
}
