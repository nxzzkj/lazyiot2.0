

using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages
{
    /// <summary>
    /// This message is used to acknowledge/reject a message and to send a MDSDataTransferMessage in same message object.
    /// It is used in web services.
    /// </summary>
    public class MDSDataTransferResponseMessage : MDSMessage
    {
        /// <summary>
        /// MessageTypeId of message.
        /// It is used to serialize/deserialize message.
        /// </summary>
        public override int MessageTypeId
        {
            get { return MDSMessageFactory.MessageTypeIdMDSDataTransferResponseMessage; }
        }

        /// <summary>
        /// This field is used to acknowledge/reject to an incoming message.
        /// </summary>
        public MDSOperationResultMessage Result { get; set; }

        /// <summary>
        /// This field is used to send a new message.
        /// </summary>
        public MDSDataTransferMessage Message { get; set; }

        /// <summary>
        /// Serializes this message.
        /// </summary>
        /// <param name="serializer">Serializer used to serialize objects</param>
        public override void Serialize(IMDSSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteObject(Result);
            serializer.WriteObject(Message);
        }

        /// <summary>
        /// Deserializes this message.
        /// </summary>
        /// <param name="deserializer">Deserializer used to deserialize objects</param>
        public override void Deserialize(IMDSDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Result = deserializer.ReadObject(() => new MDSOperationResultMessage());
            Message = deserializer.ReadObject(() => new MDSDataTransferMessage());
        }
    }
}
