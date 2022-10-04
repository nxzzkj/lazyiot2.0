

namespace Scada.MDSCore.Client.WebServices
{
    /// <summary>
    /// Represents an outgoing data message from a MDS web service to MDS server.
    /// </summary>
    public interface IWebServiceOutgoingMessage
    {
        #region Properties

        /// <summary>
        /// Essential application message data to be sent.
        /// </summary>
        byte[] MessageData { get; set; }

        #endregion
    }
}
