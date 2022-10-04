

using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages
{
    /// <summary>
    /// This message is used to change Communication Way of a communicator while it is connected to the MDS server.
    /// Thus, for example, a receiver may change it's communication way to only Send and it does not get messages
    /// anymore but can send messages.
    /// </summary>
    public class MDSChangeCommunicationWayMessage : MDSMessage
    {
        /// <summary>
        /// MessageTypeId of message.
        /// It is used to serialize/deserialize message.
        /// </summary>
        public override int MessageTypeId
        {
            get { return MDSMessageFactory.MessageTypeIdMDSChangeCommunicationWayMessage; }
        }

        /// <summary>
        /// New communication way.
        /// </summary>
        public CommunicationWays NewCommunicationWay { get; set; }

        /// <summary>
        /// Serializes this message.
        /// </summary>
        /// <param name="serializer">Serializer used to serialize objects</param>
        public override void Serialize(IMDSSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteByte((byte) NewCommunicationWay);
        }

        /// <summary>
        /// Deserializes this message.
        /// </summary>
        /// <param name="deserializer">Deserializer used to deserialize objects</param>
        public override void Deserialize(IMDSDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            NewCommunicationWay = (CommunicationWays) deserializer.ReadByte();
        }
    }
}
