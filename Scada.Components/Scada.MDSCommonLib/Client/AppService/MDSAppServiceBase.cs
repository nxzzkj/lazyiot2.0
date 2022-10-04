

namespace Scada.MDSCore.Client.AppService
{
    /// <summary>
    /// Base class for MDSMessageProcessor/MDSClientApplicationBase.
    /// </summary>
    public abstract class MDSAppServiceBase
    {
        /// <summary>
        /// Reference to the MDS Server.
        /// </summary>
        public IMDSServer Server { get; set; }

        /// <summary>
        /// Reference to this Application.
        /// </summary>
        public IMDSApplication Application { get; set; }
    }
}
