 
using System;
using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages
{
    /// <summary>
    /// Abstract class of all message classes.
    /// All messages transmiting on MDS must be derrived from this class.
    /// </summary>
    public abstract class MDSMessage : IMDSSerializable
    {
        /// <summary>
        /// MessageTypeId of message.
        /// It is used to serialize/deserialize message.
        /// </summary>
        public abstract int MessageTypeId { get; }
        
        /// <summary>
        /// Unique ID for this message.
        /// Thiss will be a GUID if it is not set.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// If this message is a reply for another message then RepliedMessageId contains first message's MessageId
        /// else RepliedMessageId is null default.
        /// </summary>
        public string RepliedMessageId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MDSMessage()
        {
            MessageId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Serializes this message.
        /// </summary>
        /// <param name="serializer">Serializer used to serialize objects</param>
        public virtual void Serialize(IMDSSerializer serializer)
        {
            serializer.WriteStringUTF8(MessageId);
            serializer.WriteStringUTF8(RepliedMessageId);
        }

        /// <summary>
        /// Deserializes this message.
        /// </summary>
        /// <param name="deserializer">Deserializer used to deserialize objects</param>
        public virtual void Deserialize(IMDSDeserializer deserializer)
        {
            MessageId = deserializer.ReadStringUTF8();
            RepliedMessageId = deserializer.ReadStringUTF8();
        }
    }
}
