

namespace Scada.MDSCore.Client.AppService
{
    /// <summary>
    /// Represents a MDS Application from MDSMessageProcessor perspective.
    /// This class also provides a static collection that can be used as a cache,
    /// thus, an MDSMessageProcessor/MDSClientApplicationBase can store/get application-wide objects.
    /// </summary>
    public interface IMDSApplication
    {
        /// <summary>
        /// Name of the application.
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Gets/Sets application-wide object by a string key.
        /// </summary>
        /// <param name="key">Key of the object to access it</param>
        /// <returns>Object, that is related with given key</returns>
        object this[string key] { get; set; }
    }
}
