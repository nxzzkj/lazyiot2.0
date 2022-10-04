 

using System;

namespace Scada.MDSCore.Exceptions
{
    /// <summary>
    /// Represents an Serialization / Deserialization exception.
    /// </summary>
    [Serializable]
    public class MDSSerializationException : MDSException
    {
        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSSerializationException()
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MDSSerializationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MDSSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
