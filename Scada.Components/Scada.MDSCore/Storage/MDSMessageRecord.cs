 

using System;
using Scada.MDSCore.Communication.Messages;

namespace Scada.MDSCore.Storage
{
    /// <summary>
    /// Represents a message record in database/storage manager.
    /// </summary>
    public class MDSMessageRecord
    {
        /// <summary>
        /// Auto Increment ID in database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// MessageId of message.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Message object.
        /// </summary>
        public MDSDataTransferMessage Message { get; set; }

        /// <summary>
        /// Destination server.
        /// </summary>
        public string DestServer { get; set; }

        /// <summary>
        /// Next server.
        /// </summary>
        public string NextServer { get; set; }

        /// <summary>
        /// Destination application in destination server
        /// </summary>
        public string DestApplication { get; set; }

        /// <summary>
        /// Storing time of message on this server.
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Empty contructor.
        /// </summary>
        public MDSMessageRecord()
        {
            
        }

        /// <summary>
        /// Creates a MDSMessageRecord object using a MDSDataTransferMessage.
        /// </summary>
        /// <param name="message">Message object</param>
        public MDSMessageRecord(MDSDataTransferMessage message)
        {
            Message = message;
            MessageId = message.MessageId;
            DestServer = message.DestinationServerName;
            DestApplication = message.DestinationApplicationName;
            RecordDate = DateTime.Now;
        }
    }
}
