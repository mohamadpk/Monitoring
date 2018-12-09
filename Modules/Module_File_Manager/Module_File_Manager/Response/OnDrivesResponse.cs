using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnDrivesResponse
    {
        public List<Drive> Drives = new List<Drive>();
        public class Drive
        {
            public string Name;
            public string DriveFormat;
            public long AvailableFreeSpace;
            public DriveType Drivetype;
            public bool IsReady;
            public long TotalFreeSpace;
            public long TotalSize;
            public string VolumeLabel;
        }

        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
