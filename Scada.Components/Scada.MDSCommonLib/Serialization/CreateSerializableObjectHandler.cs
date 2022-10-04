

namespace Scada.MDSCore.Serialization
{
    /// <summary>
    /// This delegate is used with IMDSDeserializer to deserialize an object.
    /// It is used by IMDSDeserializer to create an instance of deserializing object.
    /// So, user of MDS serialization must supply a method that creates an empty T object.
    /// This is needed for performance reasons. Because it is slower to create object by reflection.
    /// </summary>
    /// <typeparam name="T">Type of the object to be deserialized</typeparam>
    /// <returns>An object from type T</returns>
    public delegate T CreateSerializableObjectHandler<T>() where T : IMDSSerializable;
}
