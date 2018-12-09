using System;
using Newtonsoft.Json.Linq;


namespace Utils
{
    /// <summary>
    /// The serialize helper
    /// </summary>
    public class _s_Utils_Serialize
    {
        /// <summary>
        /// Errors the creator.
        /// </summary>
        /// <param name="JTarget">The j target.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static JObject ErrorCreator(JObject JTarget, string FunctionName, Exception ex)
        {
            JObject JError = new JObject();
            JError["Error_Message"] = ex.Message;
            JTarget[FunctionName] = JError;
            return JTarget;
        }
    }
}
