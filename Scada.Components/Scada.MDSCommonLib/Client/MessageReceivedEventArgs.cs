 

using System;

namespace Scada.MDSCore.Client
{
    /// <summary>
    /// A delegate to create events when a data transfer message received from MDS server.
    /// </summary>
    /// <param name="sender">The object which raises event</param>
    /// <param name="e">Event arguments</param>
    public delegate void MessageReceivedHandler(object sender, MessageReceivedEventArgs e);

    /// <summary>
    /// Stores message informations.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received message from MDS server.
        /// </summary>
        public IIncomingMessage Message { get; set; }
    }
}
