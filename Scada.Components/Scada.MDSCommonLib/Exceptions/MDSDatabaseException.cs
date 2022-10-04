 

using System;

namespace Scada.MDSCore.Exceptions
{
    /// <summary>
    /// Represents a Database exception.
    /// </summary>
    [Serializable]
    public class MDSDatabaseException : MDSException
    {
        /// <summary>
        /// Executed query text
        /// </summary>
        public string QueryText { set;  get; }

        /// <summary>
        /// Contstructor.
        /// </summary>
        public MDSDatabaseException()
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public MDSDatabaseException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Contstructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public MDSDatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
