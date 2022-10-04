 

using System.Collections.Generic;
using Scada.MDSCore.Communication;
using Scada.MDSCore.Communication.Messages;
using Scada.MDSCore.Storage;

namespace Scada.MDSCore.Organization
{
    /// <summary>
    /// Represents a Client Application that can send and received messages to/from this server.
    /// </summary>
    public class MDSClientApplication : MDSPersistentRemoteApplicationBase
    {
        #region Public properties

        /// <summary>
        /// Communicator type for Client applications.
        /// </summary>
        public override CommunicatorTypes CommunicatorType
        {
            get { return CommunicatorTypes.Application; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MDSClientApplication.
        /// </summary>
        /// <param name="name">Unique name of the application</param>
        public MDSClientApplication(string name)
            : base(name)
        {
            //No action
        }

        #endregion

        #region Protected / Overrive methods

        /// <summary>
        /// Gets messages from database to be sent to this application.
        /// </summary>
        /// <param name="minId">Minimum Id of message record to get (minId included)</param>
        /// <param name="maxCount">Maximum number of records to get</param>
        /// <returns>List of messages</returns>
        protected override List<MDSMessageRecord> GetWaitingMessages(int minId, int maxCount)
        {
            return StorageManager.GetWaitingMessagesOfApplication(Settings.ThisServerName, Name, minId, maxCount);
        }

        /// <summary>
        /// Gets Id of last incoming message that will be sent to this application.
        /// </summary>
        /// <returns>Id of last incoming message</returns>
        protected override int GetMaxWaitingMessageId()
        {
            return StorageManager.GetMaxWaitingMessageIdOfApplication(Settings.ThisServerName, Name);
        }

        /// <summary>
        /// Finds Next server for a message.
        /// </summary>
        /// <returns>Next server</returns>
        protected override string GetNextServerForMessage(MDSDataTransferMessage message)
        {
            //Next server and destination server is same (this server) because message is being delivered to application now.
            return message.DestinationServerName;
        }

        #endregion
    }
}
