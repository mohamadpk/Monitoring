using System;
using System.Collections.Generic;


namespace Module_File_Manager
{
    class Program
    {
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        static void Main(string[] args)
        {
            ////FileInfo fi = new FileInfo(@"C:\Users\mohamadpk\Desktop\1\2\3");
            ////DirectoryInfo di = fi.Directory;
            ////int i = 9;
            List<string> testpdf = new List<string>();
            testpdf.Add(@"C:\Users\mohamadpk\Downloads\Documents\Developing Large Web Applications.pdf");
            testpdf.Add(@"C:\Users\mohamadpk\Downloads\Documents\Handle_Commitment.pdf");
            testpdf.Add(@"C:\Users\mohamadpk\Downloads\Documents\rahnama.pdf");
            Content_Searcher._m_File_Manager_Content_Searcher.Content_Searcher(testpdf, " ﺪﻤﺤﻣ",Content_Searcher._m_File_Manager_Content_Searcher.Enum_direction.RtoL);

            ////Content_Searcher._m_File_Manager_Content_Searcher.Content_Searcher(testpdf, Reverse("بسمه تعالی"));
            ////Content_Searcher._m_File_Manager_Content_Searcher.Content_Searcher(testpdf, Reverse("آبان"));
            ////"ﺍﺪﺧ ﻡﺎﻧ ﻪﺑ "
            //Tree_Watcher._m_File_Manager_Watcher watcher = new Tree_Watcher._m_File_Manager_Watcher();
            //watcher.FilterFolder.Add("C:\\Users\\MOHAMA~1\\AppData\\Local\\Temp");
            //watcher.FilterFolder.Add("C:\\Windows\\System32\\LogFiles\\WMI\\RtBackup");
            //watcher.FilterFolder.Add("C:\\Users\\mohamadpk\\AppData\\Local\\Microsoft\\ApplicationInsights");
            //watcher.FilterFolder.Add("C:\\Windows\\Prefetch");
            //watcher.FilterFolder.Add("C:\\Windows\\System32\\config");
            //watcher.FilterFolder.Add("C:\\Users\\mohamadpk\\AppData");
            //watcher.FilterFolder.Add("C:\\Windows\\ServiceProfiles\\LocalService\\AppData");
            //watcher.FilterFolder.Add("C:\\Users\\mohamadpk\\Documents\\Visual Studio 2010\\Projects\\Module_File_Manager4\\Module_File_Manager\\bin\\Debug");
            //watcher.FilterFolder.Add("C:\\Windows\\Temp");


            ////watcher.FilterFolder.Add("c:\\hasan\\kachal");
            //watcher.StartWatching();
            ////Tree_Creator._m_File_Manager_Tree_FileTree_DB fmtdb = new Tree_Creator._m_File_Manager_Tree_FileTree_DB();
            ////fmtdb.GetParentFromPath(@"C:\Users\mohamadpk\Documents\Visual Studio 2010\Projects\Module_File_Manager2\packages");

            ////_m_File_Manager_Watcher watcher = new _m_File_Manager_Watcher();

            ////_s_Child_Cmd scc = new _s_Child_Cmd();



            ////_m_File_Manager mfm = new _m_File_Manager();
            ////////////////////////////
            ////JObject Drives=mfm.OnDrives();
            ////////////////////////////
            ////mfm.OnGetTree();
            ////mfm.OnSearch("c:\\xampp", "*.php", 1, "length <= 99999 And length >= 1438");
            ////mfm.OnSearch("c:\\xampp", "*.php", 1,"true");
            ////////////////////////////
            ////mfm.OnExecute(@"C:\Users\mohamadpk\Desktop\5.txt");
            ////////////////////////////
            ////// string Files_Folders = mfm.OnDir("c:\\");
            ////////////////////////////
            ////mfm.OnSetAttributes(@"C:\Users\mohamadpk\Desktop\New folder", 2);


            ////string[] dirs = Directory.GetDirectories(@"c:\");


            //_s_Child_Cmd scc = new _s_Child_Cmd();
            //scc.OnExecuteCmd("msinfo32 /nfo info.nfo");
        }
    }
}
