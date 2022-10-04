 

using System;
using System.Runtime.Serialization;

namespace Scada.MDSCore.Exceptions
{
    /// <summary>
    /// Represents a MDS Remote Exception.
    /// This exception is used to send an exception from an application to another application.
    /// </summary>
    [Serializable]
    public class MDSRemoteException : MDSException
    {
        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSRemoteException()
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSRemoteException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            
        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MDSRemoteException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MDSRemoteException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
