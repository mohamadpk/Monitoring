﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Response
{
    public class OnDeleteFileResponse
    {
        public string Target_File;
        public bool Sucsess;
        public Utils._s_Utils_ErrosHandling Errors = new Utils._s_Utils_ErrosHandling();
    }
}