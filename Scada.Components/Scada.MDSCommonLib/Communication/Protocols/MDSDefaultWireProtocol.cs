

using Scada.MDSCore.Communication.Messages;
using Scada.MDSCore.Exceptions;
using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Protocols
{
    /// <summary>
    /// This class is the Default Protocol that is used by MDS to communicate with other applications.
    /// A message frame is sent and received by MDSDefaultWireProtocol:
    /// 
    /// - Protocol type: 4 bytes unsigned integer. 
    ///   Must be MDSDefaultProtocolType for MDSDefaultWireProtocol.
    /// - Message type: 4 bytes integer.
    ///   Must be defined in MDSMessageFactory class.
    /// - Serialized bytes of a MDSMessage object.
    /// </summary>
    public class MDSDefaultWireProtocol : IMDSWireProtocol
    {
        /// <summary>
        /// Specific number that a message must start with.
        /// </summary>
        public const uint MDSDefaultProtocolType = 19180685;

        /// <summary>
        /// Serializes and writes a MDSMessage according to the protocol rules.
        /// </summary>
        /// <param name="serializer">Serializer to serialize message</param>
        /// <param name="message">Message to be serialized</param>
        public void WriteMessage(IMDSSerializer serializer, MDSMessage message)
        {
            //Write protocol type
            serializer.WriteUInt32(MDSDefaultProtocolType);
            
            //Write the message type
            serializer.WriteInt32(message.MessageTypeId);
            
            //Write message
            serializer.WriteObject(message);
        }

        /// <summary>
        /// Reads and constructs a MDSMessage according to the protocol rules.
        /// </summary>
        /// <param name="deserializer">Deserializer to read message</param>
        /// <returns>MDSMessage object that is read</returns>
        public MDSMessage ReadMessage(IMDSDeserializer deserializer)
        {
            //Read protocol type
            var protocolType = deserializer.ReadUInt32();
            if (protocolType != MDSDefaultProtocolType)
            {
                throw new MDSException("Wrong protocol type: " + protocolType + ".");
            }

            //Read message type
            var messageTypeId = deserializer.ReadInt32();

            //Read and return message
            return deserializer.ReadObject(() => MDSMessageFactory.CreateMessageByTypeId(messageTypeId));
        }
    }
}
