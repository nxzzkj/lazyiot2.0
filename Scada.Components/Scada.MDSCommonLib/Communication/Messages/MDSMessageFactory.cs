

using Scada.MDSCore.Exceptions;

namespace Scada.MDSCore.Communication.Messages
{
    public static class MDSMessageFactory
    {
        public const int MessageTypeIdMDSDataTransferMessage = 1;
        public const int MessageTypeIdMDSOperationResultMessage = 2;
        public const int MessageTypeIdMDSPingMessage = 3;
        public const int MessageTypeIdMDSRegisterMessage = 4;
        public const int MessageTypeIdMDSChangeCommunicationWayMessage = 5;
        public const int MessageTypeIdMDSControllerMessage = 6;
        public const int MessageTypeIdMDSDataTransferResponseMessage = 7;

        public static MDSMessage CreateMessageByTypeId(int messageTypeId)
        {
            switch (messageTypeId)
            {
                case MessageTypeIdMDSDataTransferMessage:
                    return new MDSDataTransferMessage();
                case MessageTypeIdMDSOperationResultMessage:
                    return new MDSOperationResultMessage();
                case MessageTypeIdMDSPingMessage:
                    return new MDSPingMessage();
                case MessageTypeIdMDSRegisterMessage:
                    return new MDSRegisterMessage();
                case MessageTypeIdMDSChangeCommunicationWayMessage:
                    return new MDSChangeCommunicationWayMessage();
                case MessageTypeIdMDSControllerMessage:
                    return new MDSControllerMessage();
                case MessageTypeIdMDSDataTransferResponseMessage:
                    return new MDSDataTransferResponseMessage();
                default:
                    throw new MDSException("Unknown MessageTypeId: " + messageTypeId);
            }
        }
    }
}
