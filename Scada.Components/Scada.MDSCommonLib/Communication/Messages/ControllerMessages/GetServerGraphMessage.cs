

namespace Scada.MDSCore.Communication.Messages.ControllerMessages
{
    /// <summary>
    /// This message is sent from MDS manager to MDS server to get graph of MDS servers.
    /// </summary>
    public class GetServerGraphMessage : ControlMessage
    {
        /// <summary>
        /// Gets MessageTypeId for GetServerGraphMessage.
        /// </summary>
        public override int MessageTypeId
        {
            get { return ControlMessageFactory.MessageTypeIdGetServerGraphMessage; }
        }
    }
}
