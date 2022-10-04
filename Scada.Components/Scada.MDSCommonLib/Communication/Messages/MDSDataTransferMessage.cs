﻿

using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages
{
    /// <summary>
    /// Represents a Data Transfer message. 
    /// Used to transfer real message data between client applications.
    /// </summary>
    public class MDSDataTransferMessage : MDSMessage
    {
        /// <summary>
        /// MessageTypeId of message.
        /// It is used to serialize/deserialize message.
        /// </summary>
        public override int MessageTypeId
        {
            get { return MDSMessageFactory.MessageTypeIdMDSDataTransferMessage; }
        }

        /// <summary>
        /// Name of the first source server of the message.
        /// </summary>
        public string SourceServerName { get; set; }

        /// <summary>
        /// Name of the first source application of the message.
        /// If the message is created by an MDS (source of message is not an application)
        /// then SourceApplicationName must be set to null.
        /// </summary>
        public string SourceApplicationName { get; set; }

        /// <summary>
        /// The source communication channel's (Communicator's) Id.
        /// When more than one communicator of an application is connected same MDS server
        /// at the same time, this field may be used to indicates a spesific communicator.
        /// This field is set by MDS automatically.
        /// </summary>
        public long SourceCommunicatorId { get; set; }

        /// <summary>
        /// Name of the final destination server of the message.
        /// </summary>
        public string DestinationServerName { get; set; }

        /// <summary>
        /// Name of the final destination application of the message.
        /// If the message is sent to an MDS (destination of message is not an application),
        /// then DestinationApplicationName must be set to null. 
        /// </summary>
        public string DestinationApplicationName { get; set; }

        /// <summary>
        /// Destination communication channel's (Communicator's) Id.
        /// This field is used by MDS to deliver message to a spesific communicator.
        /// When more than one communicator of an application is connected same MDS server
        /// at the same time, this field may be used to indicate a spesific communicator as receiver of message.
        /// If it is set to 0 (zero), message may be delivered to any connected communicator.
        /// If there is no communicator with DestinationCommunicatorId, message can not be delivered, so,
        /// this field can only be used to send non-persistent messages.
        /// </summary>
        public long DestinationCommunicatorId { get; set; }

        /// <summary>
        /// Passed servers of message until now, includes source and destination servers.
        /// </summary>
        public ServerTransmitReport[] PassedServers { get; set; }

        /// <summary>
        /// Essential application message data to transfer.
        /// </summary>
        public byte[] MessageData { get; set; }

        /// <summary>
        /// Transmit rule of message.
        /// This is important because it determines persistence and transmit time of message.
        /// Default: StoreAndForward.
        /// </summary>
        public MessageTransmitRules TransmitRule { get; set; }

        /// <summary>
        /// Creates a new MDSDataTransferMessage object.
        /// </summary>
        public MDSDataTransferMessage()
        {
            TransmitRule = MessageTransmitRules.StoreAndForward; //Default TransmitRule value
        }

        /// <summary>
        /// Serializes this message.
        /// </summary>
        /// <param name="serializer">Serializer used to serialize objects</param>
        public override void Serialize(IMDSSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteStringUTF8(SourceServerName);
            serializer.WriteStringUTF8(SourceApplicationName);
            serializer.WriteInt64(SourceCommunicatorId);
            serializer.WriteStringUTF8(DestinationServerName);
            serializer.WriteStringUTF8(DestinationApplicationName);
            serializer.WriteInt64(DestinationCommunicatorId);
            serializer.WriteObjectArray(PassedServers);
            serializer.WriteByteArray(MessageData);
            serializer.WriteByte((byte) TransmitRule);
        }

        /// <summary>
        /// Deserializes this message.
        /// </summary>
        /// <param name="deserializer">Deserializer used to deserialize objects</param>
        public override void Deserialize(IMDSDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            SourceServerName = deserializer.ReadStringUTF8();
            SourceApplicationName = deserializer.ReadStringUTF8();
            SourceCommunicatorId = deserializer.ReadInt64();
            DestinationServerName = deserializer.ReadStringUTF8();
            DestinationApplicationName = deserializer.ReadStringUTF8();
            DestinationCommunicatorId = deserializer.ReadInt64();
            PassedServers = deserializer.ReadObjectArray(() => new ServerTransmitReport());
            MessageData = deserializer.ReadByteArray();
            TransmitRule = (MessageTransmitRules) deserializer.ReadByte();
        }
    }
}
