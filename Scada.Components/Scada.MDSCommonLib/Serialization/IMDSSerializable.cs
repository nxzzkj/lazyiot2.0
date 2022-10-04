

namespace Scada.MDSCore.Serialization
{
    /// <summary>
    /// This interface is implemented by all classes that can be serialized/deserialized by MDS Serialization.
    /// </summary>
    public interface IMDSSerializable
    {
        /// <summary>
        /// This method serializes the object.
        /// </summary>
        /// <param name="serializer">Used to serialize object</param>
        void Serialize(IMDSSerializer serializer);

        /// <summary>
        /// This method deserializes the object.
        /// </summary>
        /// <param name="deserializer">Used to deserialize object</param>
        void Deserialize(IMDSDeserializer deserializer);
    }
}
