 
using System;

namespace Scada.MDSCore.Client.MDSServices
{
    /// <summary>
    /// This message is sent to invoke a method of an application that implements MDSService API.
    /// It is sent by MDSServiceProxyBase class and received by MDSServiceApplication class.
    /// </summary>
    [Serializable]
    public class MDSRemoteInvokeMessage
    {
        /// <summary>
        /// Name of the target service class.
        /// </summary>
        public string ServiceClassName { get; set; }

        /// <summary>
        /// Method of remote application to invoke.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Parameters of method.
        /// </summary>
        public object[] Parameters { get; set; }
    }
}
