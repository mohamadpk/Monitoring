using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnDirResponse
    {
        public List<FileOrDir> fileOrdir = new List<FileOrDir>();
        public class FileOrDir
        {
            public string Path;
            public Property File_Property_Info;
            public class Property
            {
                public FileAttributes Attributes;
                public string Type;
                public long Length;
                public List<AccessControl> Accesses=new List<AccessControl>();
                public class AccessControl
                {
                    public string IdentityReference;
                    public string FileSystemRights;
                    public string AccessControlType;
                    public string IsInherited;
                    public string PropagationFlags;
                    public string InheritanceFlags;
                    public string Owner;
                }

                public DateTime CreationTime;
                public bool IsReadOnly;
                public DateTime LastAccessTime;
                public DateTime LastWriteTime;
            }
        }
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}
