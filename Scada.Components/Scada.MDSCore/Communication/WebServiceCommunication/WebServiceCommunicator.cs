 

using System;
using System.Reflection;
using log4net;
using Scada.MDSCore.Communication;
using Scada.MDSCore.Communication.Messages;
using Scada.MDSCore.Exceptions;
 
using Scada.MDSCore.Serialization;
using Scada.MDSCore.Threading;

namespace Scada.MDSCore.Communication.WebServiceCommunication
{
    /// <summary>
    /// This class is used to communicate with a ASP.NET Web Service.
    /// </summary>
    public class WebServiceCommunicator : CommunicatorBase
    {
        /// <summary>
        /// Reference to logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// URL of web service.
        /// </summary>
        private readonly string _url;

        /// <summary>
        /// This queue is used to make web service calls sequentially.
        /// </summary>
        private readonly QueueProcessorThread<MDSDataTransferMessage> _outgoingMessageQueue;

        /// <summary>
        /// Creates a new WebServiceCommunicator object.
        /// </summary>
        /// <param name="url">URL of web service</param>
        /// <param name="comminicatorId">Communicator Id</param>
        public WebServiceCommunicator(string url, long comminicatorId)
            : base(comminicatorId)
        {
            _url = url;
            CommunicationWay = CommunicationWays.SendAndReceive;
            _outgoingMessageQueue = new QueueProcessorThread<MDSDataTransferMessage>();
            _outgoingMessageQueue.ProcessItem += OutgoingMessageQueue_ProcessItem;
        }

        /// <summary>
        /// Waits communicator to stop.
        /// </summary>
        public override void WaitToStop()
        {
            _outgoingMessageQueue.WaitToStop();
        }

        /// <summary>
        /// Prepares communication objects and starts outgoing message queue.
        /// </summary>
        protected override void StartCommunicaiton()
        {
            _outgoingMessageQueue.Start();
        }

        /// <summary>
        /// Starts outgoing message queue.
        /// </summary>
        /// <param name="waitToStop">True, to block caller thread until this object stops</param>
        protected override void StopCommunicaiton(bool waitToStop)
        {
            _outgoingMessageQueue.Stop(waitToStop);
        }

        /// <summary>
        /// This method is used to add a message to outgoing messages queue.
        /// It is called by CommunicatorBase.
        /// </summary>
        /// <param name="message">Message to send</param>
        protected override void SendMessageInternal(MDSMessage message)
        {
            if (message.MessageTypeId != MDSMessageFactory.MessageTypeIdMDSDataTransferMessage)
            {
                return;
            }

            _outgoingMessageQueue.Add(message as MDSDataTransferMessage);
        }

        /// <summary>
        /// This method is called to process a outgoing message.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void OutgoingMessageQueue_ProcessItem(object sender, ProcessQueueItemEventArgs<MDSDataTransferMessage> e)
        {
            try
            {
                //SendMessageToWebService(e.ProcessItem);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Makes web service call, receives result and raises MessageReceived event.
        /// </summary>
        /// <param name="message"></param>
        
    }
}
