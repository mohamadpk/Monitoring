using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_WebCam_Microphone_Capture.Response
{
    public class OnRecordWebcamAndVoiceCompleteResponse
    {
        public string EncryptedFileName;
        public string Pass;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
