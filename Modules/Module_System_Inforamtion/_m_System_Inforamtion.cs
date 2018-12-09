using System;
using Newtonsoft.Json.Linq;
using Utils;
using Module_Process_Manager;
using Module_Process_Manager.Process_Executer.Respose;
using Module_System_Inforamtion.Response;
using System.Reflection;

namespace Module_System_Inforamtion
{
    /// <summary>
    /// The system information module main
    /// </summary>
    public class _m_System_Inforamtion
    {
        /// <summary>
        /// Called when [system information].
        /// </summary>
        /// <returns>return OnGetSystemInformationResponse contain system information data of created nfo file</returns>
        public static OnGetSystemInformationResponse OnGetSystemInformation()
        {
            OnGetSystemInformationResponse ROnGetSystemInformationResponse = new OnGetSystemInformationResponse();
            try
            {
                OnExecuteProcessResponse Execute_msinfo32 = _m_Process_Manager.OnExecuteProcess("msinfo32.exe", "/nfo info.nfo",OutputFile:"info.nfo");
                ROnGetSystemInformationResponse.Output = Execute_msinfo32.Output;
            }
            catch (Exception ex)
            {
                ROnGetSystemInformationResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnGetSystemInformationResponse;
        }


    }
}
