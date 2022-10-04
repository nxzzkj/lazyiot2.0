
using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages
{
    /// <summary>
    /// This class represents a message that is being transmitted between MDS server and a Controller (MDS Manager).
    /// </summary>
    public class MDSControllerMessage : MDSMessage
    {
        /// <summary>
        /// MessageTypeId for MDSControllerMessage.
        /// </summary>
        public override int MessageTypeId
        {
            get { return MDSMessageFactory.MessageTypeIdMDSControllerMessage; }
        }

        /// <summary>
        /// MessageTypeId of ControllerMessage.
        /// This field is used to deserialize MessageData.
        /// All types defined in ControlMessageFactory class.
        /// </summary>
        public int ControllerMessageTypeId { get; set; }

        /// <summary>
        /// Essential message data.
        /// This is a serialized object of a class in MDS.Communication.Messages.ControllerMessages namespace.
        /// </summary>
        public byte[] MessageData { get; set; }

        public override void Serialize(IMDSSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteInt32(ControllerMessageTypeId);
            serializer.WriteByteArray(MessageData);
        }

        public override void Deserialize(IMDSDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ControllerMessageTypeId = deserializer.ReadInt32();
            MessageData = deserializer.ReadByteArray();
        }
    }
}
