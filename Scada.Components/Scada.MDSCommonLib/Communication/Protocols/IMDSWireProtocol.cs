

using Scada.MDSCore.Communication.Messages;
using Scada.MDSCore.Serialization;

namespace Scada.MDSCore.Communication.Protocols
{
    /// <summary>
    /// This interface is used to Write/Read messages according to a Wire/Communication Protocol.
    /// </summary>
    public interface IMDSWireProtocol
    {
        /// <summary>
        /// Serializes and writes a MDSMessage according to the protocol rules.
        /// </summary>
        /// <param name="serializer">Serializer to serialize message</param>
        /// <param name="message">Message to be serialized</param>
        void WriteMessage(IMDSSerializer serializer, MDSMessage message);

        /// <summary>
        /// Reads and constructs a MDSMessage according to the protocol rules.
        /// </summary>
        /// <param name="deserializer">Deserializer to read message</param>
        /// <returns>MDSMessage object that is read</returns>
        MDSMessage ReadMessage(IMDSDeserializer deserializer);
    }
}
