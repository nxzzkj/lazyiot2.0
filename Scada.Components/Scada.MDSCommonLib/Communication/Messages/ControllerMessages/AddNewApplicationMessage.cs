
using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages.ControllerMessages
{
    /// <summary>
    /// This message is sent by MDS Manager to MDS Server to add a new Application to MDS.
    /// </summary>
    public class AddNewApplicationMessage : ControlMessage
    {
        /// <summary>
        /// Gets MessageTypeId for AddNewApplicationMessage.
        /// </summary>
        public override int MessageTypeId
        {
            get { return ControlMessageFactory.MessageTypeIdAddNewApplicationMessage; }
        }

        /// <summary>
        /// Name of the new application.
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
