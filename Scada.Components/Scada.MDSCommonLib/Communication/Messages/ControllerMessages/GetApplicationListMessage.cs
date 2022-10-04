

namespace Scada.MDSCore.Communication.Messages.ControllerMessages
{
    /// <summary>
    /// This message is sent from MDS manager to MDS server to get list of all client applications.
    /// </summary>
    public class GetApplicationListMessage : ControlMessage
    {
        /// <summary>
        /// Gets MessageTypeId for GetApplicationListMessage.
        /// </summary>
        public override int MessageTypeId
        {
            get { return ControlMessageFactory.MessageTypeIdGetApplicationListMessage; }
        }
    }
}
