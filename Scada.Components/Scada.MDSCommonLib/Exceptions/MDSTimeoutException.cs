 

using System;

namespace Scada.MDSCore.Exceptions
{
    /// <summary>
    /// Represents an Timeout exception.
    /// </summary>
    [Serializable]
    public class MDSTimeoutException : MDSException
    {
        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSTimeoutException()
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MDSTimeoutException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MDSTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
