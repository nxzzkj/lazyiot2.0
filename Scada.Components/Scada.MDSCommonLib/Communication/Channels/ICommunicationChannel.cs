

using Scada.MDSCore.Communication.Messages;

namespace Scada.MDSCore.Communication.Channels
{
    /// <summary>
    /// All Communication channels implements this interface.
    /// It is used by MDSClient and MDSController classes to communicate with MDS server.
    /// </summary>
    public interface ICommunicationChannel
    {
        /// <summary>
        /// This event is raised when the state of the communication channel changes.
        /// </summary>
        event CommunicationStateChangedHandler StateChanged;

        /// <summary>
        /// This event is raised when a MDSMessage object is received from MDS server.
        /// </summary>
        event MessageReceivedHandler MessageReceived;
        event MessageReceivedHandler ClientConnected;
        event MessageReceivedHandler ClientDisConnected;
        /// <summary>
        /// Unique identifier for this communicator in connected MDS server.
        /// This field is not set by communication channel,
        /// it is set by another classes (MDSClient) that are using
        /// communication channel. 
        /// </summary>
        long ComminicatorId { get; set; }

        /// <summary>
        /// Gets the state of communication channel.
        /// </summary>
        CommunicationStates State { get; }

        /// <summary>
        /// Communication way for this channel.
        /// This field is not set by communication channel,
        /// it is set by another classes (MDSClient) that are using
        /// communication channel. 
        /// </summary>
        CommunicationWays CommunicationWay { get; set; }
        
        /// <summary>
        /// Connects to MDS server.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects from MDS server.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sends a MDSMessage to the MDS server
        /// </summary>
        /// <param name="message">Message to be sent</param>
        void SendMessage(MDSMessage message);
        /// <summary>
        /// 存储一些变量数据
        /// </summary>
        object Tag { set; get; }
    }
}
