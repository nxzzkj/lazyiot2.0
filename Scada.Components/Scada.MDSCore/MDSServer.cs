

using Scada.MDSCore.Communication;
using Scada.MDSCore.Communication.Events;
using Scada.MDSCore.Organization;
using Scada.MDSCore.Organization.Routing;
using Scada.MDSCore.Settings;
using Scada.MDSCore.Storage;
using Scada.MDSCore.Threading;

namespace Scada.MDSCore
{

    /// <summary>
    /// 创建一个MDS服务
    /// </summary>
    public class MDSServer : IRunnable
    {
        public event CommunicatorConnectedHandler ApplicationConnected;
        /// <summary>
        /// 应用程序退出
        /// </summary>
        public event CommunicatorDisconnectedHandler ApplicationDisConnected;
        /// <summary>
        /// Settings.
        /// </summary>
        private   MDSSettings _settings;
        public MDSSettings Settings
        {
            set { _settings = value; }
            get { return _settings; }
        }
    
        /// <summary>
        /// Storage manager used for database operations.
        /// </summary>
        private readonly IStorageManager _storageManager;

        /// <summary>
        /// Routing table.
        /// </summary>
        private readonly RoutingTable _routingTable;

        /// <summary>
        /// A Graph consist of server nodes. It also holds references to MDSAdjacentServer objects.
        /// </summary>
        private readonly MDSServerGraph _serverGraph;

        /// <summary>
        /// Reference to all MDS Managers. It contains communicators to all instances of MDS manager.
        /// </summary>
        private  MDSController _mdsManager;
        public MDSController Manager
        {
            set { _mdsManager = value; }
            get { return _mdsManager; }
        }
        /// <summary>
        /// List of applications
        /// </summary>
        private   MDSClientApplicationList _clientApplicationList;
        public MDSClientApplicationList  ClientApplicationList
        {
            set { _clientApplicationList = value; }
            get { return _clientApplicationList; }
        }
        /// <summary>
        /// Communication layer.
        /// </summary>
        private readonly CommunicationLayer _communicationLayer;

        /// <summary>
        /// Organization layer.
        /// </summary>
        private readonly OrganizationLayer _organizationLayer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MDSServer()
        {
            _settings = MDSSettings.Instance;
            _serverGraph = new MDSServerGraph();
            _clientApplicationList = new MDSClientApplicationList();
            _mdsManager = new MDSController("MDSController");
         
            _storageManager = StorageManagerFactory.CreateStorageManager();
            _routingTable = new RoutingTable();
            _communicationLayer = new CommunicationLayer();
            _organizationLayer = new OrganizationLayer(_communicationLayer, _storageManager, _routingTable, _serverGraph, _clientApplicationList, _mdsManager);
            _mdsManager.OrganizationLayer = _organizationLayer;
            _mdsManager.CommunicatorConnected += _mdsManager_CommunicatorConnected;
            _mdsManager.CommunicatorDisconnected += _mdsManager_CommunicatorDisconnected;
        }
        //有通讯对象链接
        private void _mdsManager_CommunicatorDisconnected(object sender, Communication.Events.CommunicatorDisconnectedEventArgs e)
        {
            if(ApplicationDisConnected!=null)
            ApplicationDisConnected(sender, e);
        }
        /// <summary>
        /// 通讯对象断开链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mdsManager_CommunicatorConnected(object sender, Communication.Events.CommunicatorConnectedEventArgs e)
        {
            
            if (ApplicationConnected != null)
                ApplicationConnected(sender, e);
        }

        /// <summary>
        /// Starts the MDS server.
        /// </summary>
        public void Start()
        {
            _storageManager.Start();
            CorrectDatabase();
            _communicationLayer.Start();
            _organizationLayer.Start();
        }

        /// <summary>
        /// Stops the MDS server.
        /// </summary>
        /// <param name="waitToStop">True, if caller thread must be blocked until MDS server stops.</param>
        public void Stop(bool waitToStop)
        {
            _communicationLayer.Stop(waitToStop);
            _organizationLayer.Stop(waitToStop);
            _storageManager.Stop(waitToStop);
        }

        /// <summary>
        /// Waits stopping of MDS server.
        /// </summary>
        public void WaitToStop()
        {
            _communicationLayer.WaitToStop();
            _organizationLayer.WaitToStop();
            _storageManager.WaitToStop();
        }

        /// <summary>
        /// Checks and corrects database records if needed.
        /// </summary>
        private void CorrectDatabase()
        {
            if (_settings["CheckDatabaseOnStartup"] != "true")
            {
                return;
            }

            //If Server graph is changed, records in storage engine (database) may be wrong, therefore, they must be updated 
            var nextServersList = _serverGraph.GetNextServersForDestServers();
            foreach (var nextServerItem in nextServersList)
            {
                _storageManager.UpdateNextServer(nextServerItem.Key, nextServerItem.Value);
            }
        }
    }
}
