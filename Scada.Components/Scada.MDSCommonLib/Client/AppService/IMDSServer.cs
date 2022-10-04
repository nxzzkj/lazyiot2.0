

namespace Scada.MDSCore.Client.AppService
{
    /// <summary>
    /// This interface is used by MDSMessageProcessor/MDSClientApplicationBase to perform operations on MDSServer,
    /// for example; creating messages to send.
    /// </summary>
    public interface IMDSServer
    {
        /// <summary>
        /// Creates an empty message to send.
        /// </summary>
        /// <returns>Created message</returns>
        IOutgoingMessage CreateMessage();
    }
}
