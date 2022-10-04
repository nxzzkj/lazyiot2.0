 

using System;
using Scada.MDSCore.Communication.Messages.ControllerMessages;

namespace Scada.MDSCore.Management
{
    /// <summary>
    /// A delegate to create events when a control message received from MDS server.
    /// </summary>
    /// <param name="sender">The object which raises event</param>
    /// <param name="e">Event arguments</param>
    public delegate void ControlMessageReceivedHandler(object sender, ControlMessageReceivedEventArgs e);

    /// <summary>
    /// Stores message informations.
    /// </summary>
    public class ControlMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received message from MDS server.
        /// </summary>
        public ControlMessage Message { get; set; }

        /// <summary>
        /// Creates a ControlMessageReceivedEventArgs object.
        /// </summary>
        /// <param name="message">Received message from MDS server</param>
        public ControlMessageReceivedEventArgs(ControlMessage message)
        {
            Message = message;
        }
    }
}
