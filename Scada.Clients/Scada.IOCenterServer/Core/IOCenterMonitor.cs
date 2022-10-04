

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.MDSCore;
using Scada.MDSCore.Communication;
using Scada.MDSCore.Communication.Events;
using Scada.MDSCore.Settings;
using Scada.Model;
using ScadaCenterServer.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaCenterServer.Core
{
    public enum MDSServerStatus
    {
        停止,
        运行,
        暂停
    }
    /// <summary>
    /// 定义mds消息服务的服务器
    /// </summary>
    public class IOCenterMonitor : IDisposable
    {
        Scada.Business.IO_ALARM_CONFIG alarmconfigBll = new Scada.Business.IO_ALARM_CONFIG();
        Scada.Business.IO_ALARM_CONFIG alarmConfigBll = new Scada.Business.IO_ALARM_CONFIG();
        Scada.Business.IO_COMMUNICATION communicationBll = new Scada.Business.IO_COMMUNICATION();
        Scada.Business.IO_DEVICE deviceBll = new Scada.Business.IO_DEVICE();
        Scada.Business.IO_PARA paraBll = new Scada.Business.IO_PARA();
        Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
        public MDSServerStatus MDSServerStatus = MDSServerStatus.停止;
        public MonitorForm ServerForm = null;

        public MDSServer Server { set; get; }
        public string ServerID { set; get; }

        /// <summary>
        /// 异常报错返回
        /// </summary>
        public Action<string> ServerException;
        /// <summary>
        /// 正常的日志输出
        /// </summary>
        public Action<string> ServerNormalWriteLog;
        public void Dispose()
        {
            if (Server != null)
            {

                Server.Stop(true);
            }
            Server = null;
        }
        private MDSConfig mDSServerConfig = null;
        public void InitMonitorForm(MonitorForm monitorForm)
        {
            ServerForm = monitorForm;
          
            
            ServerForm.FormClosing += ServerForm_FormClosing;
            IOCenterManager.ServerForm = ServerForm;
        }

     

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ServerForm.WindowState = FormWindowState.Minimized;
        }
        public string GetAppName(string ServerId)
        {
            var station = MDSSettings.Instance.Stations.Find(x => x.PhysicalMAC == ServerId);
            if (station != null)
                return mDSServerConfig.MonitorAppPrefix + "_" + station.PhysicalAddress.Replace(".", "").Replace("。", "");
            return "";
        }

        /// <summary>
        /// 初始化服务器
        /// </summary>
        public void InitServer()
        {
            try
            {

                mDSServerConfig = new MDSConfig();
                MDSSettings.Instance.ThisServer.Name = mDSServerConfig.CenterServerName;
                MDSSettings.Instance.ThisServer.Port = int.Parse(mDSServerConfig.MDSServerPort);
                MDSSettings.Instance.ThisServer.IpAddress = LocalIp.GetLocalIp();
      

                //判断主通讯是否包含数据中心的通讯客户端，如果没有则建立一个客户端
                MDSSettings.Instance.Applications.Add(new ApplicationInfoItem()
                {
                    Name = mDSServerConfig.CenterAppName,
                    StationName = mDSServerConfig.CenterServerName

                });
                MDSSettings.Instance.Applications.Add(new ApplicationInfoItem()
                {
                    Name = mDSServerConfig.WebAppPrefix,
                    StationName = mDSServerConfig.CenterServerName
                });
                //创建采集站程序，工程，流程，监视器
                foreach (var station in MDSSettings.Instance.Stations)
                {
                    //IO管理器
                    MDSSettings.Instance.Applications.Add(new ApplicationInfoItem()
                    {
                        Name = mDSServerConfig.ManagerAppPrefix + "_" + station.PhysicalAddress.Replace(".", "").Replace("。", ""),
                        StationName = station.StationName

                    });
                    //采集监视器
                    MDSSettings.Instance.Applications.Add(new ApplicationInfoItem()
                    {
                        Name = mDSServerConfig.MonitorAppPrefix + "_" + station.PhysicalAddress.Replace(".", "").Replace("。", ""),
                        StationName = station.StationName
                    });
                    //流程设计器
                    MDSSettings.Instance.Applications.Add(new ApplicationInfoItem()
                    {
                        Name = mDSServerConfig.FlowAppPrefix + "_" + station.PhysicalAddress.Replace(".", "").Replace("。", ""),
                        StationName = station.StationName
                    });
                    ///创建一个web


                }
                //直接读取配置文件MDS服务
                Server = new MDSServer();
                Server.ApplicationConnected += Server_ApplicationConnected;
                Server.ApplicationDisConnected += Server_ApplicationDisConnected;
                if (ServerNormalWriteLog != null)
                {
                    ServerNormalWriteLog("采集器消息服务创建成功!");
                }



            }
            catch (Exception emx)
            {
                if (ServerException != null)
                {
                    ServerException("消息服务初始化失败,ERROR=" + emx.Message);
                }
            }

        }
        public void RefreshServerStatus(string serverId, int status, ScadaClientType clientType, string appname, bool writemsg = false)
        {
            if (IOCenterManager.PublishRestart)
                return;
            IO_SERVER server = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID.Trim().ToLower() == serverId.Trim().ToLower());
            if (server != null)
            {

                server.SERVER_STATUS = 0;

                if (clientType == ScadaClientType.IoMonitor)
                {
                    server.SERVER_IP = "";
                    Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
                    serverBll.Update(server);
                    server.SERVER_STATUS = status;

                }



            }
            if (ServerForm != null && !ServerForm.IsDisposed)
            {
                string connStr = status == 0 ? "断开" : "连接";
                string clientMsg = "";
                if (clientType == ScadaClientType.IoMonitor)
                {
                    clientMsg = "有采集站" + connStr + "数据中心";
                }
                else if (clientType == ScadaClientType.FlowDesign)
                {
                    clientMsg = "有采集站流程设计器" + connStr + "数据中心";
                }
                else if (clientType == ScadaClientType.IoManager)
                {
                    clientMsg = "有采集站IO工程管理器" + connStr + "数据中心";
                }
                else if (clientType == ScadaClientType.WebSystem)
                {
                    clientMsg = "WebScada" + connStr + "数据中心";
                }
                if (writemsg)
                {
                    ServerNormalWriteLog(appname + " " + clientMsg);
                }
            }


            IOCenterManager.QueryFormManager.ServerStatus(server, status == 0 ? false : true);

           
        }
        private void Server_ApplicationDisConnected(object sender, CommunicatorDisconnectedEventArgs e)
        {
            if (IOCenterManager.PublishRestart)
                return;
            TaskHelper.Factory.StartNew(() => {
            try
            {

                MDSRemoteApplication remoteApp = (MDSRemoteApplication)sender;
                string clientType = remoteApp.Name.Split('_')[0];
                string serverId = remoteApp.Name.Split('_')[1];
                if (clientType == mDSServerConfig.MonitorAppPrefix)
                {
                    RefreshServerStatus(serverId, 0, ScadaClientType.IoMonitor, remoteApp.Name, true);
                }

            }
            catch (Exception emx)
            {
                if (ServerException != null)
                {
                    ServerException("消息服务初始化失败,ERROR=" + emx.Message);
                }
            }
            });
        }

        private void Server_ApplicationConnected(object sender, CommunicatorConnectedEventArgs e)
        {
           
                if (IOCenterManager.PublishRestart)
                    return;
            TaskHelper.Factory.StartNew(() =>
            {
                try
                {
                    MDSRemoteApplication remoteApp = (MDSRemoteApplication)sender;
                    string clientType = remoteApp.Name.Split('_')[0];
                    string serverId = remoteApp.Name.Split('_')[1];
                    if (clientType == mDSServerConfig.MonitorAppPrefix)
                    {
                        RefreshServerStatus(serverId, 1, ScadaClientType.IoMonitor, remoteApp.Name, true);
                    }
                }
                catch (Exception emx)
                {
                    if (ServerException != null)
                    {
                        ServerException("消息服务初始化失败,ERROR=" + emx.Message);
                    }

                }
            });
        }

        public bool Start()
        {
            if (Server != null)
            {
                try
                {

                    Server.Start();
                    MDSServerStatus = MDSServerStatus.运行;
                    return true;
                    
                }
                catch (Exception emx)
                {
                    if (ServerException != null)
                    {
                        ServerException("启动消息服务失败,ERROR=" + emx.Message);
                    }
                    return false;
                }
            }
            return false;
            
        }
        public bool Stop()
        {
            if (Server != null)
            {
                Server.Stop(true);
                MDSServerStatus = MDSServerStatus.停止;
                return true;
                
            }
            
            return false;


        }



    }
}
