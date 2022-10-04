 
using System;
using System.Runtime.Serialization;

namespace Scada.MDSCore.Exceptions
{
    /// <summary>
    /// Represents a MDS Exception.
    /// This is the base class for exceptions that are thrown by MDS system.
    /// </summary>
    [Serializable]
    public class MDSException : Exception
    {
        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSException()
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MDSException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MDSException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
