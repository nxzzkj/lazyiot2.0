
using Scada.MDSCore.Communication.Events;
using Scada.MDSCore.Threading;

namespace Scada.MDSCore.Communication
{
    /// <summary>
    /// This interface is implemented by communcation managers.
    /// </summary>
    public interface ICommunicationManager : IRunnable
    {
        /// <summary>
        /// This event is raised when a communicator is connected succesfully.
        /// </summary>
        event CommunicatorConnectedHandler CommunicatorConnected;
    }
}
