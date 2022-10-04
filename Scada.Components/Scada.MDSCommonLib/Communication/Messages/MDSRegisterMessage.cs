
using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Messages
{
    /// <summary>
    /// Register Message. A MDSRegisterMessage object is used to register a MDS server as an Application or MDS server.
    /// </summary>
    public class MDSRegisterMessage : MDSMessage
    {
        /// <summary>
        /// MessageTypeId of message.
        /// It is used to serialize/deserialize message.
        /// </summary>
        public override int MessageTypeId
        {
            get { return MDSMessageFactory.MessageTypeIdMDSRegisterMessage; }
        }

        /// <summary>
        /// Communicator type (MDS server, Application or Controller). 
        /// </summary>
        public CommunicatorTypes CommunicatorType { get; set; }

        /// <summary>
        /// Communication way for this communicator (SEND, RECEIVE or BOTH)
        /// </summary>
        public CommunicationWays CommunicationWay { get; set; }

        /// <summary>
        /// Name of the communicator. 
        /// If CommunicatorType is a MDS, than this is server's name,
        /// if CommunicatorType is an Application, than this is application's name,
        /// if CommunicatorType is a Controller, than this is an arbitrary string represents controller.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Password to connect to MDS associated with Name and CommunicatorType.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Creates a new MDSRegisterMessage object.
        /// </summary>
        public MDSRegisterMessage()
        {
            CommunicationWay = CommunicationWays.Send;
        }

        /// <summary>
        /// Serializes this message.
        /// </summary>
        /// <param name="serializer">Serializer used to serialize objects</param>
        public override void Serialize(IMDSSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteByte((byte)CommunicatorType);
            serializer.WriteByte((byte)CommunicationWay);
            serializer.WriteStringUTF8(Name);
            serializer.WriteStringUTF8(Password);
        }

        /// <summary>
        /// Deserializes this message.
        /// </summary>
        /// <param name="deserializer">Deserializer used to deserialize objects</param>
        public override void Deserialize(IMDSDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            CommunicatorType = (CommunicatorTypes) deserializer.ReadByte();
            CommunicationWay = (CommunicationWays) deserializer.ReadByte();
            Name = deserializer.ReadStringUTF8();
            Password = deserializer.ReadStringUTF8();
        }
    }
}
