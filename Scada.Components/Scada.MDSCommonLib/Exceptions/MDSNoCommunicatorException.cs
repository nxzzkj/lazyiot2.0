 
using System;

namespace Scada.MDSCore.Exceptions
{
    /// <summary>
    /// This exception is thrown when there is not a communicator of a remote application.
    /// </summary>
    [Serializable]
    public class MDSNoCommunicatorException : MDSException
    {
        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSNoCommunicatorException()
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MDSNoCommunicatorException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MDSNoCommunicatorException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
