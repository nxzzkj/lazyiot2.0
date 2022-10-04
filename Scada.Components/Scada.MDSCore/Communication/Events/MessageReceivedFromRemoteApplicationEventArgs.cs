

using Scada.MDSCore.Communication.Messages;

namespace Scada.MDSCore.Communication.Events
{
    /// <summary>
    /// A delegate to create events when a message received from a remote application.
    /// </summary>
    /// <param name="sender">The object which raises event</param>
    /// <param name="e">Event arguments</param>
    public delegate void MessageReceivedFromRemoteApplicationHandler(object sender, MessageReceivedFromRemoteApplicationEventArgs e);

    /// <summary>
    /// Stores informations about received message
    /// </summary>
    public class MessageReceivedFromRemoteApplicationEventArgs
    {
        /// <summary>
        /// Remote Application.
        /// </summary>
        public MDSRemoteApplication Application { get; set; }

        /// <summary>
        /// Communicator.
        /// </summary>
        public ICommunicator Communicator { get; set; }

        /// <summary>
        /// Received message from communicator.
        /// </summary>
        public MDSMessage Message { get; set; }
    }
}
