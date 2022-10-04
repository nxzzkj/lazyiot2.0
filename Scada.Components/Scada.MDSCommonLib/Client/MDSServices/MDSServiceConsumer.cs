 

using System;
using Scada.MDSCore.Communication;

namespace Scada.MDSCore.Client.MDSServices
{
    public class MDSServiceConsumer : IDisposable
    {
        #region Public fields

        /// <summary>
        /// Underlying MDSClient object to send/receive MDS messages.
        /// </summary>
        internal MDSClient MdsClient { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MDSServiceApplication object with default values to connect to MDS server.
        /// </summary>
        /// <param name="applicationName">Name of the application</param>
        public MDSServiceConsumer(string applicationName)
        {
            MdsClient = new MDSClient(applicationName, CommunicationConsts.DefaultIpAddress, CommunicationConsts.DefaultMDSPort);
            Initialize();
        }

        /// <summary>
        /// Creates a new MDSServiceApplication object with default port to connect to MDS server.
        /// </summary>
        /// <param name="applicationName">Name of the application</param>
        /// <param name="ipAddress">IP address of MDS server</param>
        public MDSServiceConsumer(string applicationName, string ipAddress)
        {
            MdsClient = new MDSClient(applicationName, ipAddress, CommunicationConsts.DefaultMDSPort);
        }

        /// <summary>
        /// Creates a new MDSServiceApplication object.
        /// </summary>
        /// <param name="applicationName">Name of the application</param>
        /// <param name="ipAddress">IP address of MDS server</param>
        /// <param name="port">TCP port of MDS server</param>
        public MDSServiceConsumer(string applicationName, string ipAddress, int port) 
        {
            MdsClient = new MDSClient(applicationName, ipAddress, port);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method connects to MDS server using underlying MDSClient object.
        /// </summary>
        public void Connect()
        {
            MdsClient.Connect();
        }

        /// <summary>
        /// This method disconnects from MDS server using underlying MDSClient object.
        /// </summary>
        public void Disconnect()
        {
            MdsClient.Disconnect();
        }

        /// <summary>
        /// Disposes this object, disposes/closes underlying MDSClient object.
        /// </summary>
        public void Dispose()
        {
            MdsClient.Dispose();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes this object.
        /// </summary>
        private void Initialize()
        {
            MdsClient.CommunicationWay = CommunicationWays.Send;
        }
        
        #endregion
    }
}
