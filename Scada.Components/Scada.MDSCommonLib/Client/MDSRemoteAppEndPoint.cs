

namespace Scada.MDSCore.Client
{
    public class MDSRemoteAppEndPoint
    {
        public string ServerName { get; set; }

        public string ApplicationName { get; set; }

        public long CommunicatorId { get; set; }

        public MDSRemoteAppEndPoint()
        {
            
        }

        public MDSRemoteAppEndPoint(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public MDSRemoteAppEndPoint(string serverName, string applicationName)
        {
            ServerName = serverName;
            ApplicationName = applicationName;
        }

        public MDSRemoteAppEndPoint(string serverName, string applicationName, long communicatorId)
        {
            ServerName = serverName;
            ApplicationName = applicationName;
            CommunicatorId = communicatorId;
        }
    }
}
