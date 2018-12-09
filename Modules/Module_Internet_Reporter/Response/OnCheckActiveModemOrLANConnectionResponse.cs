using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_Internet_Reporter.Response
{
    public class OnCheckActiveModemOrLANConnectionResponse
    {
        public bool ActiveModemOrLAN;
        public int Description;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
