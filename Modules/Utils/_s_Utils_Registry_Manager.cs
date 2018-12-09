using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class _s_Utils_Registry_Manager
    {
        public static string[] ReadAllKey(ref RegistryKey KeyObject,RegistryKey Root, string KeyAdress)
        {
            try {
                    KeyObject = Root.OpenSubKey(KeyAdress);
                    return KeyObject.GetSubKeyNames();

                }catch(Exception ex)
                {
                    return null;
                }
        }

        public static string GetRootStringFromFullPath(string RegistryFullPath)
        {
            int IndexFirstBackSlash = RegistryFullPath.IndexOf("\\");
            string Root = RegistryFullPath.Substring(0, IndexFirstBackSlash);
            return Root;
        }
        public static string GetPathStringFromFullPath(string RegistryFullPath)
        {
            int IndexFirstBackSlash = RegistryFullPath.IndexOf("\\");
            string KeyPath = RegistryFullPath.Substring(IndexFirstBackSlash + 1);
            return KeyPath;
        }
        public static RegistryKey ExtractRootFromRegistryFullPath(string RegistryFullPath)
        {
            //Microsoft.Win32.Registry.DynData;
            //Microsoft.Win32.Registry.PerformanceData;
            string Root = GetRootStringFromFullPath(RegistryFullPath);
            switch (Root.ToUpper())
            {
                case ("HKEY_CLASSES_ROOT"):
                    {
                        return Microsoft.Win32.Registry.ClassesRoot;
                    }
                case ("HKEY_CURRENT_USER"):
                    {
                        return Microsoft.Win32.Registry.CurrentUser;
                    }
                case ("HKEY_LOCAL_MACHINE"):
                    {
                        return Microsoft.Win32.Registry.LocalMachine;
                    }
                case ("HKEY_USERS"):
                    {
                        return Microsoft.Win32.Registry.Users;
                    }
                case ("HKEY_CURRENT_CONFIG"):
                    {
                        return Microsoft.Win32.Registry.CurrentConfig;
                    }
            }

            return null;
        }

        public static RegistryKey StringRegistryFullPathToRegistryKey(string RegistryFullPath)
        {
            RegistryKey RegistryRoot = ExtractRootFromRegistryFullPath(RegistryFullPath);
            string KeyPath = GetPathStringFromFullPath(RegistryFullPath);
            RegistryKey KeyObject = RegistryRoot.OpenSubKey(KeyPath);
            return KeyObject;
        }
        public static void DeleteKey(string KeyToDelete)
        {
            RegistryKey RegistryRoot = ExtractRootFromRegistryFullPath(KeyToDelete);
            RegistryRoot.DeleteSubKey(GetPathStringFromFullPath(KeyToDelete));
        }
    }
}
