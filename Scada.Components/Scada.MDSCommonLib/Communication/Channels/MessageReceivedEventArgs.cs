 

using System;
using Scada.MDSCore.Communication.Messages;

namespace Scada.MDSCore.Communication.Channels
{
    /// <summary>
    /// A delegate to create events by Communication Channels, when a MDSMessage received from MDS server.
    /// </summary>
    /// <param name="sender">The object which raises event</param>
    /// <param name="e">Event arguments</param>
    public delegate void MessageReceivedHandler(ICommunicationChannel sender, MessageReceivedEventArgs e);

    /// <summary>
    /// Stores message informations.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received message from MDS server.
        /// </summary>
        public MDSMessage Message { get; set; }
    }
}
