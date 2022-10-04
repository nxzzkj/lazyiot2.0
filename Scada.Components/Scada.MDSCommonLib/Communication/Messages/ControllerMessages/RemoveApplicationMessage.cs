

using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages.ControllerMessages
{
    /// <summary>
    /// This message is sent by MDS Manager to MDS Server to remove a Application from MDS.
    /// </summary>
    public class RemoveApplicationMessage : ControlMessage
    {
        /// <summary>
        /// Gets MessageTypeId for RemoveApplicationMessage.
        /// </summary>
        public override int MessageTypeId
        {
            get { return ControlMessageFactory.MessageTypeIdRemoveApplicationMessage; }
        }

        /// <summary>
        /// Name of the removing application.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Serializes this message.
        /// </summary>
        /// <param name="serializer">Serializer used to serialize objects</param>
        public override void Serialize(IMDSSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteStringUTF8(ApplicationName);
        }

        /// <summary>
        /// Deserializes this message.
        /// </summary>
        /// <param name="deserializer">Deserializer used to deserialize objects</param>
        public override void Deserialize(IMDSDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ApplicationName = deserializer.ReadStringUTF8();
        }
    }
}
