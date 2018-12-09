using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Module_Process_Manager.Process_Executer.Respose;
using System.Reflection;

namespace Module_Process_Manager.Process_Executer
{
    /// <summary>
    /// called from _m_Process_Manager class and OnExecuteCmd function
    /// </summary>
    public class _s_Child_Cmd
    {
        
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="Command">The command.</param>
        /// <returns>json object.if successful return output of executed standard stdout else return error</returns>
        public static OnExecuteProcessResponse ExecuteCmd(string Command)
        {
            OnExecuteProcessResponse ROnExecuteProcessResponse = new OnExecuteProcessResponse();
            ROnExecuteProcessResponse.Target_Executable = Command;
            try {
                ROnExecuteProcessResponse= _m_Process_Manager.OnExecuteProcess("cmd.exe", Command, ProcessParametersType: _m_Child_Process.Enum_ProcessParametersType.Stdin, ProcessOutputType: _m_Child_Process.Enum_ProcessOutputType.Cmd);
                }catch(Exception  ex)
            {
                ROnExecuteProcessResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnExecuteProcessResponse;
        }
    }
}
