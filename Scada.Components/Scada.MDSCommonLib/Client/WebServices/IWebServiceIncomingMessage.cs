
using Scada.MDSCore.Communication.Messages;

namespace Scada.MDSCore.Client.WebServices
{
    /// <summary>
    /// Represents an incoming data message to a MDS Web Service from MDS server.
    /// </summary>
    public interface IWebServiceIncomingMessage
    {
        #region Properties

        /// <summary>
        /// Gets the unique identifier for this message.
        /// </summary>
        string MessageId { get; }

        /// <summary>
        /// Name of the first source server of the message.
        /// </summary>
        string SourceServerName { get; }

        /// <summary>
        /// Name of the sender application of the message.
        /// </summary>
        string SourceApplicationName { get; }

        /// <summary>
        /// The source communication channel's (Communicator's) unique Id.
        /// When more than one communicator of an application is connected same MDS server
        /// at the same time, this field may be used to indicate a spesific communicator.
        /// This field is set by MDS automatically.
        /// </summary>
        long SourceCommunicatorId { get; }

        /// <summary>
        /// Name of the final destination server of the message.
        /// </summary>
        string DestinationServerName { get; }

        /// <summary>
        /// Name of the final destination application of the message.
        /// </summary>
        string DestinationApplicationName { get; }

        /// <summary>
        /// Passed servers of message until now, includes source and destination servers.
        /// </summary>
        ServerTransmitReport[] PassedServers { get; }

        /// <summary>
        /// Essential application message data that is received.
        /// </summary>
        byte[] MessageData { get; }

        /// <summary>
        /// Transmit rule of message.
        /// This is important because it determines persistence and transmit time of message.
        /// Default: StoreAndForward.
        /// </summary>
        MessageTransmitRules TransmitRule { get; }

        #endregion

        #region Methods

        IWebServiceResponseMessage CreateResponseMessage();

        IWebServiceOutgoingMessage CreateResponseDataMessage();

        #endregion
    }
}
