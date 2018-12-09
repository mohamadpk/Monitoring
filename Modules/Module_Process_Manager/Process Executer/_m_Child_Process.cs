using Module_Process_Manager.Process_Executer.Respose;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Utils;

namespace Module_Process_Manager.Process_Executer
{
    /// <summary>
    /// called from _m_Process_Manager class and OnExecuteProcess function
    /// </summary>
    public class _m_Child_Process
    {
        /// <summary>
        /// The command output 
        /// string out put of stdout if process is set cmd type on ProcessOutputType
        /// </summary>
        static string _Cmd_Output;
        /// <summary>
        /// The child process identifier
        /// </summary>
        static int _ChildProcessId;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Child_Process"/> class.
        /// </summary>
        public _m_Child_Process()
        {
            _Cmd_Output = "";
        }


        /// <summary>
        /// A native win 32 api parameter call by EnumThreadWindows when seen a thread over a process at one win32 snap.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="lParam">The l parameter.</param>
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        /// A native win 32 api Enum the thread windows.
        /// </summary>
        /// <param name="dwThreadId">The dw thread identifier.</param>
        /// <param name="lpfn">The LPFN.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn,IntPtr lParam);
        /// <summary>
        /// Enumerates the process window handles.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <returns>return enumartion object of all process handle with search for window hwnd on all thread of process</returns>
        static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id,(hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }
        /// <summary>
        /// A native win 32 api Shows or Hide or max size and min size set to the window.
        /// </summary>
        /// <param name="hwnd">The HWND. window handle</param>
        /// <param name="nCmdShow">The n command show.the option how to show window</param>
        /// <returns></returns>
        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);
        /// <summary>
        /// The sw hide parameter for ShowWindow native win 32 api set hide status to window
        /// </summary>
        private const int SW_HIDE = 0;

        /// <summary>
        /// Handles the Elapsed event of the Timer_For_Hide_Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void Timer_For_Hide_Window_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try {
                foreach (var handle in EnumerateProcessWindowHandles(_ChildProcessId))
                {
                    ShowWindow((int)handle, SW_HIDE);
                }
            }catch(Exception ex)
            {

            }
        }



        /// <summary>
        /// Specified output type of started process
        /// </summary>
        public enum Enum_ProcessOutputType { Cmd, File };
        /// <summary>
        /// Specified how to set parameter for started process
        /// </summary>
        public enum Enum_ProcessParametersType { Argument, Stdin };
        /// <summary>
        /// Executes the child process.
        /// </summary>
        /// <param name="Target_Executable">The target executable.</param>
        /// <param name="Parametrs">The parametrs.</param>
        /// <param name="ProcessParametersType">Type of the process parameters.</param>
        /// <param name="NoWindow">if set to <c>true</c> [no window].</param>
        /// <param name="FullHide">if set to <c>true</c> [full hide].</param>
        /// <param name="ReturnOutput">if set to <c>true</c> [return output].</param>
        /// <param name="ProcessOutputType">Type of the process output.</param>
        /// <param name="OutputFile">The output file.</param>
        /// <returns></returns>
        public OnExecuteProcessResponse ExecuteChildProcess(string Target_Executable, string Parametrs, Enum_ProcessParametersType ProcessParametersType = Enum_ProcessParametersType.Argument, bool NoWindow = true, bool FullHide = true, bool ReturnOutput = true, Enum_ProcessOutputType ProcessOutputType = Enum_ProcessOutputType.File, string OutputFile = "output.txt")
        {
            OnExecuteProcessResponse ROnExecuteProcessResponse = new OnExecuteProcessResponse();
            ROnExecuteProcessResponse.Target_Executable = Target_Executable;

            //declare here for stop it after end of work on try or catch
            System.Timers.Timer timer_for_hide_window=null;
            try
            {
                ProcessStartInfo ExecutableStartInfo = new ProcessStartInfo();
                ExecutableStartInfo.FileName = Target_Executable;
                ExecutableStartInfo.RedirectStandardOutput = true;
                ExecutableStartInfo.RedirectStandardError = true;
                ExecutableStartInfo.RedirectStandardInput = true;
                ExecutableStartInfo.UseShellExecute = false;
                if (ProcessParametersType == Enum_ProcessParametersType.Argument)
                    ExecutableStartInfo.Arguments = Parametrs;

                if (NoWindow)
                {
                    ExecutableStartInfo.CreateNoWindow = true;
                    ExecutableStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }


                Process ChildProcess = new Process();
                ChildProcess.StartInfo = ExecutableStartInfo;
                ChildProcess.OutputDataReceived += ChildProcess_OutputDataReceived;
                ChildProcess.EnableRaisingEvents = true;
                ChildProcess.Start();
                ChildProcess.BeginOutputReadLine();
                ChildProcess.BeginErrorReadLine();

                if (FullHide)
                {
                    _ChildProcessId = ChildProcess.Id;
                    timer_for_hide_window = new System.Timers.Timer();
                    timer_for_hide_window.Interval = 1;
                    timer_for_hide_window.Elapsed += Timer_For_Hide_Window_Elapsed;
                    timer_for_hide_window.Start();
                }




                if (ProcessParametersType == Enum_ProcessParametersType.Stdin)
                    ChildProcess.StandardInput.WriteLine(Parametrs);

                



                if (ReturnOutput)
                {
                    if (ProcessOutputType == Enum_ProcessOutputType.Cmd)
                    {
                        ChildProcess.StandardInput.WriteLine("exit");
                    }

                    ChildProcess.WaitForExit();
                    ROnExecuteProcessResponse.Pid= ChildProcess.Id;

                    if (ProcessOutputType == Enum_ProcessOutputType.Cmd)
                    {
                        ROnExecuteProcessResponse.Output = _Cmd_Output;
                    }
                    if (ProcessOutputType == Enum_ProcessOutputType.File)
                    {
                        ROnExecuteProcessResponse.Output = System.IO.File.ReadAllText(OutputFile);
                    }
                }


                
                if(timer_for_hide_window!=null)
                timer_for_hide_window.Stop();

            }
            catch (Exception ex)
            {
                if (timer_for_hide_window != null)
                    timer_for_hide_window.Stop();

                ROnExecuteProcessResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnExecuteProcessResponse;
        }

        /// <summary>
        /// Handles the OutputDataReceived event of the ChildProcess control.
        /// Collecting of cmd type process output
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        private static void ChildProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _Cmd_Output += e.Data + "\r\n";
        }
    }
}
