 

using System;
using Scada.MDSCore.Exceptions;

namespace Scada.MDSCore.Client.MDSServices
{
    /// <summary>
    /// This message is sent as return message of a MDSRemoteInvokeMessage.
    /// It is used to send return value of method invocation.
    /// It is sent by MDSServiceApplication class and received by MDSServiceProxyBase class.
    /// </summary>
    [Serializable]
    public class MDSRemoteInvokeReturnMessage
    {
        /// <summary>
        /// Return value of remote method invocation.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// If any exception occured during method invocation, this field contains Exception object.
        /// If no exception occured, this field is null.
        /// </summary>
        public MDSRemoteException RemoteException { get; set; }
    }
}
