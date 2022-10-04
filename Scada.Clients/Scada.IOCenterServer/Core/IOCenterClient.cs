


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using Scada.DBUtility;
using Scada.IOStructure;
using Scada.MachineTraining;
using Scada.MDSCore;
using Scada.MDSCore.Client;
using Scada.MDSCore.Communication.Messages;
using Scada.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaCenterServer.Core
{
    public delegate void RefreshAiTrainDeviceListView(string appname, string datetime, string server, string communication, string device, string taskName, string Algorithm, string AlgorithmType, string PredictedLabel, string Score, string ColumnValue, string Columns);
    public delegate void RefreshReeiveDeviceListView(string appname, string datetime, string server, string communication, string device, string msg, bool result);
    public delegate void RefreshReeiveAlarmListView(string appname, string server, string communication, string device, IO_PARAALARM alarm, bool result);
    public delegate void RefreshEventListView(string application, string eventdate, string eventname, string server, string communication, string device, string msg);
    public delegate void RefreshCommandListView(string appname, string server, string communication, string device, string para, IO_COMMANDS command);
    public delegate void RefreshMainTreeDeviceStatus(List<IO_DEVICE> devices);
    public delegate void RefreshMainTreeServerStatus(IO_SERVER server, bool status, string mac);
    public delegate void RefreshMainTreeCommunicationStatus(IO_COMMUNICATION comm, bool status);
    /// <summary>
    /// 定义一个应用程序客户都通信
    /// </summary>
    public class IOCenterClient : IDisposable
    {
        private MDSConfig mDSServerConfig = null;
        public IOCenterMonitor MDSServer { set; get; }
        public MDSClient MDSClient { set; get; }
  
        public string ApplicationName { set; get; }
        /// <summary>
        /// 定义一个数据存储和接收的缓存，influxdb用于批量插入
        /// </summary>
        public static IOCenterCacheManager RealCache = null;

        /// <summary>
        /// 异常报错返回
        /// </summary>
        Func<string, string, Task> ClientException;
        /// <summary>
        /// 正常的日志输出
        /// </summary>
        Func<string, string, Task> ClientNormalWriteLog;
        /// <summary>
        /// 用户登录
        /// </summary>
        Func<SplitPackageMessage, Task> UserLogin;
        #region IO工程发布相关
        /// <summary>
        /// 申请发布工程
        /// </summary>
        Func<SplitPackageMessage, Task> ApplyPublishProject;
        Func<SplitPackageMessage, Task> StartPublishProject;
        /// <summary>
        /// 发布工程中
        /// </summary>
        Func<SplitPackageMessage, Task> PublishingProject;
        /// <summary>
        /// 发布工程结束
        /// </summary>
        Func<SplitPackageMessage, Task> PublishedProjectSuccess;
        Func<SplitPackageMessage, Task> PublishedProjectFail;
        #endregion
        #region 流程发布相关操作
        //流程发布相关
        /// <summary>
        /// 申请发布工程
        /// </summary>
        Func<SplitPackageMessage, Task> ApplyPublishFlow;
        Func<SplitPackageMessage, Task> StartPublishFlow;
        /// <summary>
        /// 发布工程中
        /// </summary>
        Func<SplitPackageMessage, Task> PublishingFlow;
    
        /// <summary>
        /// 发布工程结束
        /// </summary>
        Func<SplitPackageMessage, Task> PublishedFlowSuccess;
        Func<SplitPackageMessage, Task> PublishedFlowFailt;
        #endregion
        Func<SplitPackageMessage, Task> UpdateMonitorAlarmResult;
        Func<SplitPackageMessage, Task> ReceiveRealData;
        Func<SplitPackageMessage, Task> ReceiveAlarmData;
        Func<SplitPackageMessage, Task> ReceiveEventData;
        Func<SplitPackageMessage, Task> ReceiveScadaStatusData;
        Func<SplitPackageMessage, IOutgoingMessage, Task> SendCommand;


        Scada.Business.IO_ALARM_CONFIG alarmConfigBll = new Scada.Business.IO_ALARM_CONFIG();
        Scada.Business.IO_COMMUNICATION communicationBll = new Scada.Business.IO_COMMUNICATION();
        Scada.Business.IO_DEVICE deviceBll = new Scada.Business.IO_DEVICE();
        Scada.Business.IO_PARA paraBll = new Scada.Business.IO_PARA();
        Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
        Scada.Business.ScadaMachineTrainingModel trainBll = new Scada.Business.ScadaMachineTrainingModel();
        Scada.Business.ScadaMachineTrainingCondition trainConditionBll = new Scada.Business.ScadaMachineTrainingCondition();
        Scada.Business.BatchCommandTaskModel batchCommandTaskBll = new Scada.Business.BatchCommandTaskModel();
        
        /// <summary>
        /// 定义一个接收工程发布数据的临时存储器
        /// </summary>
        Dictionary<string, MDSReceiveLargePackObject> TemporaryByteBuffers = new Dictionary<string, MDSReceiveLargePackObject>();
        Dictionary<string, MDSReceiveLargePackObject> TemporaryFlowByteBuffers = new Dictionary<string, MDSReceiveLargePackObject>();

        public event RefreshReeiveDeviceListView OnRefreshReeiveDeviceListView;
        public event RefreshReeiveAlarmListView OnRefreshReeiveAlarmListView;
        public event RefreshEventListView OnRefreshEventListView;
        public event RefreshCommandListView OnRefreshCommandListView;
        public event RefreshMainTreeDeviceStatus OnRefreshMainTreeDeviceStatus;
        public event RefreshMainTreeServerStatus OnRefreshMainTreeServerStatus;
        public event RefreshMainTreeCommunicationStatus OnRefreshMainTreeCommunicationStatus;

        public event RefreshAiTrainDeviceListView OnRefreshAiTrainDeviceListView;

        public IOCenterClient(string appName, string ip, int port, IOCenterMonitor server)
        {

            MDSServer = server;
            ApplicationName = appName;
            if (MDSClient != null)
            {
                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;
            }

            MDSClient = new MDSClient(ApplicationName, ip, port);
            MDSClient.MessageReceived += MDSClient_MessageReceived;
            mDSServerConfig = new MDSConfig();

            UserLogin = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        #region 处理采集站程序登录
                        TcpData tcpData = new TcpData();

                        tcpData.BytesToTcpItem(pack.Datas);
                        if (!tcpData.IsInvalid)
                        {
                            return;
                        }

                        STATION_TCP_INFO loginInfo = new STATION_TCP_INFO();
                        try
                        {

                            loginInfo.IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                            loginInfo.PASSWROD = tcpData.GetItemValue("PASSWROD");
                            loginInfo.USER = tcpData.GetItemValue("USER");
                            loginInfo.RESULT = tcpData.GetItemValue("RESULT");
                            loginInfo.FUNCTION = tcpData.GetItemValue("FUNCTION");
                            if (loginInfo.USER.Trim() == IOCenterManager.IOProject.ServerConfig.User && loginInfo.PASSWROD.Trim() == IOCenterManager.IOProject.ServerConfig.Password)
                            {

                                loginInfo.RESULT = "true";
                                loginInfo.MSG = "登录成功";
                                RealCache.Push(new EventCacheObject()
                                {
                                    Event = new ScadaEventModel()
                                    {
                                        SERVER_ID = pack.ServerId,
                                        Event = ScadaEvent.采集站登录.ToString(),
                                        Content = "登录成功! 用户 " + loginInfo.USER.Trim() + " 客户端类型 " + loginInfo.FUNCTION

                                    }

                                });
                            }
                            else
                            {
                                loginInfo.RESULT = "false";
                                loginInfo.MSG = "登录失败,账户或者密码不能为空 账户密码不正确";
                                RealCache.Push(new EventCacheObject()
                                {
                                    Event = new ScadaEventModel()
                                    {
                                        SERVER_ID = pack.ServerId,
                                        Event = ScadaEvent.采集站登录.ToString(),
                                        Content = "登录失败，密码或者账号名称不正确! 用户 " + loginInfo.USER.Trim() + " 客户端类型 " + loginInfo.FUNCTION

                                    }

                                });

                            }
                        }
                        catch (Exception ex)
                        {
                            loginInfo.RESULT = "false";
                            loginInfo.MSG = "登录失败 " + ex.Message;

                        }


                        if (loginInfo != null)
                        {
                            byte[] resultbyte = tcpData.StringToTcpByte(loginInfo.GetCommandString());
                            this.Send(resultbyte, pack.ServerId, pack.SourceApplicationName, MDSCommandType.登录反馈, pack.ClientType);
                        }


                        tcpData.Dispose();
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            ///异常报错输出
            ClientException = (string app, string msg) =>
            {

                try
                {
                    
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        if (IOCenterManager.QueryFormManager != null)
                        {
                            IOCenterManager.QueryFormManager.AddLog(app + " " + msg);
                        }

                        Scada.Logger.Logger.GetInstance().Debug(msg);
                    });
                    return task;
                }
                catch
                {

                    return null;
                }
            };
            ///正常日志输出
            ClientNormalWriteLog = (string app, string msg) =>
            {

                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        if (IOCenterManager.QueryFormManager != null)
                        {
                            IOCenterManager.QueryFormManager.AddLog(app + " " + msg);
                        }

                        Scada.Logger.Logger.GetInstance().Info(msg);
                    });
                    return task;
                }
                catch
                {

                    return null;
                }
            };
            ApplyPublishProject = (SplitPackageMessage pack) =>
            {
                try
                {

                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        try
                        {
                            #region 发布工程反馈
                            TcpData tcpData = new TcpData();
                            tcpData.BytesToTcpItem(pack.Datas);

                            if (!tcpData.IsInvalid)
                            {
                                return;
                            }

                            try
                            {
                                string IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                                string RESULT = tcpData.GetItemValue("RESULT");
                                string MSG = tcpData.GetItemValue("MSG");
                                if (IO_SERVER_ID != "" && IO_SERVER_ID == pack.ServerId)//发布的工程和网卡是一致的
                                {
                                    tcpData.GetItem("RESULT").Value = "true";
                                }
                                else
                                {
                                    tcpData.GetItem("RESULT").Value = "false";
                                    tcpData.GetItem("MSG").Value = "发布的工程只能在对应的采集站发布";
                                }

                            }
                            catch
                            {
                                tcpData.GetItem("RESULT").Value = "false";
                                tcpData.GetItem("MSG").Value = "发布的工程只能在对应的采集站发布";


                            }
                            if (tcpData != null)
                            {
                                tcpData.ChangedToBytes();
                                MDSReceiveLargePackObject tempPack = null;
                                if (TemporaryByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                                {
                                    tempPack.TemporaryByteRun = false;
                                    tempPack.Clear();
                                    TemporaryByteBuffers.Remove(pack.ServerId);


                                }

                                tempPack = new MDSReceiveLargePackObject();
                                TemporaryByteBuffers.Add(pack.ServerId, tempPack);
                                tempPack.ServerID = pack.ServerId;
                                tempPack.ReadTimeout = () => {



                                    int sendIndex = tempPack.GetSendIndex();
                                    if (sendIndex > 0)
                                    {
                                        TcpData sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = sendIndex.ToString() });//请求接收的
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在接收客户端流程图工程，请耐心等待......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程数据传输进度, ScadaClientType.IoManager);
                                    }
                                    ClientNormalWriteLog(pack.SourceApplicationName, "接收采集站流程图 第" + ((tempPack.TemporaryByteBuffer.Count + 1)) + "条数据");

                                };
                                tempPack.ReadTimeoutEnd = () => {



                                    TcpData sendData = new TcpData();
                                    sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "接收采集站流程图超时失败" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                    sendData.ChangedToBytes();
                                    ClientNormalWriteLog(pack.SourceApplicationName, "流程发布失败，客户度未上传工程ID，无法更新工程");
                                    this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);
                                    ClientNormalWriteLog(pack.SourceApplicationName, "接收采集站流程图超时失败");
                                    tempPack.Clear();

                                };



                                Send(tcpData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程请求反馈, ScadaClientType.IoManager);
                            }


                            tcpData.Dispose();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            ClientException(pack.SourceApplicationName, "ERR10021" + ex.Message);
                        }
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };

            StartPublishProject = (SplitPackageMessage pack) =>
            {
                try
                {

                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        MDSReceiveLargePackObject tempPack = null;
                        string IO_SERVER_ID = pack.ServerId;
                        if (!pack.IsInvalid)
                        {

                            return;
                        }
                        if (!TemporaryByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                        {
                            if (ClientNormalWriteLog != null)
                            {
                                ClientNormalWriteLog(pack.SourceApplicationName, "发布工程失败,采集站节点不明确");
                            }
                        }
                        if (tempPack == null)
                        {
                            return;
                        }
                        #region 上传数据开始
                        TcpData sendData = new TcpData();
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);
                        if (tcpData == null)
                        {
                            sendData = new TcpData();
                            sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                            sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "采集站工程发布失败，未上传工程ID，无法更新工程" });
                            sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                            sendData.ChangedToBytes();
                            ClientNormalWriteLog(pack.SourceApplicationName, "采集站工程发布失败，客户度未上传工程ID，无法更新工程");
                            this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);

                            tempPack.Clear();

                            //解析字符串失败
                            return;
                        }


                 
                        string NUMBER = tcpData.GetItemValue("NUMBER");
                        string BYTENUMBER = tcpData.GetItemValue("BYTENUMBER");

                        tempPack.TemporaryCounter = int.Parse(NUMBER);
                        tempPack.TemporaryBytesKey = IO_SERVER_ID;
                        tempPack.TemporaryByteRun = true;
                        //下发命令获取第一组数据
                        sendData = new TcpData();
                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = (1).ToString() });//请求接收的
                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在接收客户端流程图工程，请耐心等待......" });
                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                        sendData.ChangedToBytes();
                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程数据传输进度, ScadaClientType.IoManager);

                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            PublishingProject = (SplitPackageMessage pack) =>
            {
                try
                {
                    //通知采集站开始发布数据
                    var task = Task.Factory.StartNew(() =>
                    {
                        MDSReceiveLargePackObject tempPack = null;
                        string IO_SERVER_ID = pack.ServerId;
                        if (IO_SERVER_ID == "" || !pack.IsInvalid)
                        {
                            TcpData sendData = new TcpData();
                            sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                            sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "IO_SERVER_ID 不能为空" });
                            sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                            sendData.ChangedToBytes();
                            this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);
                            return;

                        }

                        if (!TemporaryByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                        {
                            if (ClientNormalWriteLog != null)
                            {
                                ClientNormalWriteLog(pack.SourceApplicationName, "发布工程失败,采集站节点不明确");
                            }
                        }
                        if (tempPack == null)
                        {
                            return;
                        }


                        #region 接收客户度上传的数据 

                     

                        if (!tempPack.IsReceiveEnd())//表示数据还未接收完
                        {
                            if (pack.Datas.Length > 4)
                            {
                                byte[] packIndexBytes = pack.Datas.Take(4).ToArray();//获取前两个字节，并得到index索引
                                int packIndex = BitConverter.ToInt32(packIndexBytes, 0);

                                bool res = tempPack.AddReceivePack(pack.Datas.Skip(4).ToArray(), packIndex);

                                int sendIndex = tempPack.GetSendIndex();
                                if (sendIndex > 0)
                                {
                                    TcpData sendData = new TcpData();
                                    sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = sendIndex.ToString() });//请求接收的
                                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在接请求接收第" + (tempPack.TemporaryByteBuffer.Count + 1).ToString() + "条数据，请耐心等待......" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                    sendData.ChangedToBytes();
                                    this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程数据传输进度, ScadaClientType.FlowDesign);
                                }
                                ClientNormalWriteLog(pack.SourceApplicationName, "已经接收采集站工程 " + pack.SourceApplicationName + " 第" + (tempPack.TemporaryByteBuffer.Count) + "条数据");




                            }
                        }


                     
                        //并列执行
                        if (tempPack.IsReceiveEnd())
                        {
                            tempPack.TemporaryByteRun = false;
                            ClientNormalWriteLog(pack.SourceApplicationName, "已经全部接收采集站 " + pack.SourceApplicationName + "  " + tempPack.TemporaryByteBuffer.Count + "条数据,");
                            TcpData sendData = new TcpData();
                            sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "服务器已经全部接收采" + tempPack.TemporaryByteBuffer.Count.ToString() });
                            sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "数据中心正在解析发布数据，请耐心等待....." });
                            sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                            sendData.ChangedToBytes();
                            this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.上传数据完成, ScadaClientType.IoManager);
                            Scada.Business.PublishServer mPublishServer = new Scada.Business.PublishServer();
                            #region 读取字节数据，并做转换处理
                            ClientNormalWriteLog(pack.SourceApplicationName, "正在执行发布更新中.....");
                            //备份数据
                            ConcurrentBag<IO_SERVER> Servers = new ConcurrentBag<Scada.Model.IO_SERVER>();
                            ConcurrentBag<IO_DEVICE> Devices = new ConcurrentBag<Scada.Model.IO_DEVICE>();
                            ConcurrentBag<IO_COMMUNICATION> Communications = new ConcurrentBag<Scada.Model.IO_COMMUNICATION>();
                            ConcurrentStack<IO_PARA> Paras = new ConcurrentStack<Scada.Model.IO_PARA>();
                            ConcurrentStack<IO_ALARM_CONFIG> ParasConfigs = new ConcurrentStack<Scada.Model.IO_ALARM_CONFIG>();
                            ConcurrentBag<ScadaMachineTrainingModel> Trains = new ConcurrentBag<Scada.Model.ScadaMachineTrainingModel>();
                            ConcurrentBag<ScadaMachineTrainingCondition> TrainConditions = new ConcurrentBag<Scada.Model.ScadaMachineTrainingCondition>();
                            ConcurrentBag<BatchCommandTaskModel> BatchCommandTasks = new ConcurrentBag<Scada.Model.BatchCommandTaskModel>();
                            int num = tempPack.TemporaryByteBuffer.Count;
                        
                            object receiveObject = null;
                            byte[] resBytes;
                            ConcurrentStack<IO_PARA> tempPara = new ConcurrentStack<IO_PARA>();
                            ConcurrentStack<IO_ALARM_CONFIG> tempConfig = new ConcurrentStack<IO_ALARM_CONFIG>();
                            for(int i=1;i<=tempPack.TemporaryCounter; i++)
                            {

                                try
                                {

                                    #region 反序列化IO表参数
                                  
                                    resBytes= tempPack.GetIndexBytes(i);
                              
                                    if (resBytes == null || resBytes.Length <= 0)
                                        continue;
                                    receiveObject = ObjectSerialize.BytesToObjectBinaryFormatter(resBytes);
                                    if (receiveObject != null)
                                    {
                                        if (receiveObject is IO_SERVER)
                                        {
                                            IO_SERVER serverstation = receiveObject as IO_SERVER;
                                            serverstation.SERVER_ID = IO_SERVER_ID;
                                            //保证采集站不能重复
                                            if (!Servers.Contains(serverstation))
                                                Servers.Add(serverstation);

                                        }
                                        else if (receiveObject is IO_COMMUNICATION)
                                        {
                                            IO_COMMUNICATION comm = receiveObject as IO_COMMUNICATION;
                                            comm.IO_SERVER_ID = IO_SERVER_ID;
                                            //保证通讯通道不能重复
                                             if (!Communications.Contains(comm))
                                                Communications.Add(comm);

                                        }
                                        else if (receiveObject is IO_DEVICE)
                                        {
                                            IO_DEVICE device = receiveObject as IO_DEVICE;
                                            device.IO_SERVER_ID = IO_SERVER_ID;
                                            if (!Devices.Contains(device))
                                            {
                                                Devices.Add(device);
                                                Paras.PushRange(device.IOParas.ToArray());
                                                for(int p=0;p< device.IOParas.Count; p++)
                                                {
                                                    ParasConfigs.Push(device.IOParas[p].AlarmConfig);
                                                }
                                               
                                               
                                                
                                            }
                                        }
                                        else if (receiveObject is ScadaMachineTrainingModel)
                                        {
                                            ScadaMachineTrainingModel train = receiveObject as ScadaMachineTrainingModel;
                                            train.SERVER_ID = IO_SERVER_ID;
                                         
                                            for (int c = 0; c < train.Conditions.Count; c++)
                                            {
                                                train.Conditions[c].SERVER_ID = IO_SERVER_ID;
                                                train.Conditions[c].TaskId = train.Id;
                                                train.Conditions[c].MarkDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                train.Conditions[c].StartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                train.Conditions[c].EndDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss");
                                                train.Conditions[c].DataLength = 0;
                                                if (!TrainConditions.Contains(train.Conditions[c]))
                                                    TrainConditions.Add(train.Conditions[c]);

                                            }
                                            if (!Trains.Contains(train))
                                                Trains.Add(train);
                                        }
                                        else if (receiveObject is BatchCommandTaskModel)
                                        {
                                            BatchCommandTaskModel command = receiveObject as BatchCommandTaskModel;
                                            command.SERVER_ID = IO_SERVER_ID;
                                            //保证通讯通道不能重复
                                            if (!BatchCommandTasks.Contains(command))
                                                BatchCommandTasks.Add(command);

                                        }
                                    }
                                    else
                                    {
                                        ClientNormalWriteLog(pack.SourceApplicationName, "发布工程失败,数据对象类型转换失败");

                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "服务器更新工程失败" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);
                                        if (tempPack.TemporaryByteBuffer != null)
                                            tempPack.Clear();
                                      
                                        return;
                                    }
                                    #endregion
                                    //通知客户端已经接收数据了
                                    ClientNormalWriteLog(pack.SourceApplicationName, "正在执行发布更新中.....更新进度" + (i).ToString());
                                    TaskHelper.Factory.StartNew(() =>
                                    {
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = (Convert.ToDouble(i) / num * 100).ToString("0.0") + "%" + num });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在更新采集站工程，请耐心等待......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);
                                    });

                                }
                                catch
                                {

                                    continue;
                                }
                               
                            }
                          
                            #endregion
                            #region 清理缓存
                            if (tempPack.TemporaryByteBuffer != null)
                                tempPack.Clear();
                    
                            GC.Collect();
                            #endregion
                            #region 初始化服务器
                            try
                            {

                                IO_SERVER NewServer = null;

                                Servers.TryTake(out NewServer);
                                if (NewServer != null)
                                {
                                
                                    NewServer.CENTER_IP = LocalIp.GetLocalIp();

                                    sendData = new TcpData();
                                    sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在清理原始发布数据......" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                    sendData.ChangedToBytes();
                                    this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);
                                    mPublishServer.ClearServers(NewServer.SERVER_ID);
                                    if (serverBll.Add(NewServer))
                                    {
                                        List<Task> tasks = new List<Task>();
                                        //发布成功后删除旧数据
                                        ClientNormalWriteLog(pack.SourceApplicationName, "正在清理旧的IO表信息......");
                                        ClientNormalWriteLog(pack.SourceApplicationName, "等待更新通讯通道......");
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在更新通讯通道节点数据......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);
                                    
                                        communicationBll.Add(Communications);
                                     
                                        ClientNormalWriteLog(pack.SourceApplicationName, "通讯更新完成!");
                                        ClientNormalWriteLog(pack.SourceApplicationName, "等待更新设备信息......");
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在更新所有设备节点数据......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);
                                     
                                            deviceBll.Add(Devices);
                              
                                        ClientNormalWriteLog(pack.SourceApplicationName, "更新设备信息完成!");

                                        ClientNormalWriteLog(pack.SourceApplicationName, "等待更新IO表及其预警配置信息......");
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在更新IO点及其报警配置节点数据,共计有" + Paras.Count() + "个IO点，需要等待一段时间......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);

                                     
                                        paraBll.Add(Paras, ParasConfigs);
                                       
                                        ClientNormalWriteLog(pack.SourceApplicationName, "更新IO报警配置完成!");
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在更新机器训练模型" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);

                                        ClientNormalWriteLog(pack.SourceApplicationName, "等待更新机器训练模型......");
                                        
                                            trainBll.Add(Trains);
                                  
                                        ClientNormalWriteLog(pack.SourceApplicationName, "等待更新机器训练模型信息......");
                                      
                                            trainConditionBll.Add(TrainConditions);
                                   
                                        ClientNormalWriteLog(pack.SourceApplicationName, "更新机器训练模型完成！");
                                  
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "请耐心等待数据更新任务完成" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);
                                        ClientNormalWriteLog(pack.SourceApplicationName, "等待更新自动控制命令组信息......");
                                        //更新自动控制命令
                                         
                                            batchCommandTaskBll.Add(BatchCommandTasks);
                                        
                                        ClientNormalWriteLog(pack.SourceApplicationName, "更新自动控制命令完成完成！");
                                         
                                      
 
                                        ClientNormalWriteLog(pack.SourceApplicationName, "开始正在释放相关内存空间......");
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "开始正在释放相关内存空间" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);
                                       
                                      
                                        Servers = null;
                                        Communications = null;
                                        Devices = null;
                                        Paras = null;
                                        ParasConfigs = null;
                                          Trains = null;
                                        BatchCommandTasks = null;
                                        ClientNormalWriteLog(pack.SourceApplicationName, "开始正在释放相关内存空间......");                                    
                                        ClientNormalWriteLog(pack.SourceApplicationName, "释放相关内存空间完成");
                                        ClientNormalWriteLog(pack.SourceApplicationName, "采集站工程发布成功，准备重新启动服务器!");

                                        tempPack.Clear();
                                    
                                        ClientNormalWriteLog(pack.SourceApplicationName, "重新初始化服务器!");
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在初始化数据中心服务器，请耐心等待......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);

                                        IOCenterManager.IOProject.PublishReloadProject(IO_SERVER_ID, pack.SourceApplicationName);
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "100%" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "数据中心服务器初始化成功待" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程进度, ScadaClientType.IoManager);

                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "true" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "服务器更新工程工程成功" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程成功, ScadaClientType.IoManager);
                                        //重新加载指定的工程
                                        ClientNormalWriteLog(pack.SourceApplicationName, "服务器更新成功!");
                                        this.TemporaryByteBuffers.Remove("IO_SERVER_ID");
                                        return;

                                    }
                                    else
                                    {
                                     
                                        Servers = null;
                                      
                                        Communications = null;
                                      
                                        Devices = null;
                                   
                                        Paras = null;
                                  
                                        Trains = null;
                                        sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "服务器更新工程失败" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);

                                        tempPack.Clear();
                                        this.TemporaryByteBuffers.Remove("IO_SERVER_ID");
                                  
                                        return;
                                    }
                                }
                                else
                                {
                                   
                                     Servers = null;
                                  
                                    Communications = null;
                                 
                                    Devices = null;
                                   
                                    Paras = null;
                                  
                                    Trains = null;
                                }


                            }
                            catch (Exception ex)
                            {
                               
                                Servers = null;
                                 
                                Communications = null;
                                
                                Devices = null;
                              
                                Paras = null;
                             
                                Trains = null;
                                ClientException(pack.SourceApplicationName, "发布工程失败 ERR33022" + ex.Message);

                                sendData = new TcpData();
                                sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                                sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "服务器更新工程失败" });
                                sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                                this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.发布工程失败, ScadaClientType.IoManager);
                                tempPack.Clear();
                            
                                return;
                            }

                            #endregion


                        }
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            PublishedProjectSuccess = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        #region 上传数据成功
                        ClientNormalWriteLog(pack.SourceApplicationName, "采集站数据已经全部发送完毕!");

                        #endregion
                        //保存事件
                        RealCache.Push(new EventCacheObject()
                        {
                            Event = new ScadaEventModel()
                            {
                                SERVER_ID = pack.ServerId,
                                Event = ScadaEvent.采集站发布成功.ToString()

                            }

                        });
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    //保存事件
                    RealCache.Push(new EventCacheObject()
                    {
                        Event = new ScadaEventModel()
                        {
                            SERVER_ID = pack.ServerId,
                            Event = ScadaEvent.采集站发布失败.ToString(),
                            Content = ex.Message

                        }

                    });
                    return null;
                }
            };
            PublishedProjectFail = (SplitPackageMessage pack) =>
            {
                try
                {
                    /// 
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        #region  
                        MDSReceiveLargePackObject tempPack = null;
                        string IO_SERVER_ID = pack.ServerId;
                        if (IO_SERVER_ID == "" || !pack.IsInvalid)
                        {

                            if (!TemporaryByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                            {
                                if (ClientNormalWriteLog != null)
                                {
                                    ClientNormalWriteLog(pack.SourceApplicationName, "发布工程失败,采集站节点不明确");
                                }
                            }
                            if (tempPack == null)
                            {
                                return;
                            }

                            tempPack.Clear();
                    
                            return;

                        }

                        ClientNormalWriteLog(pack.SourceApplicationName, "采集站" + pack.SourceApplicationName + "发布工程失败!");
                        RealCache.Push(new EventCacheObject()
                        {
                            Event = new ScadaEventModel()
                            {
                                SERVER_ID = pack.ServerId,
                                Event = ScadaEvent.采集站发布失败.ToString()


                            }

                        });
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            ApplyPublishFlow = (SplitPackageMessage pack) =>
            {
                try
                {
                    //流程发布申请
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        try
                        {


                            #region 流程发布请求反馈
                            TcpData tcpData = new TcpData();
                            tcpData.BytesToTcpItem(pack.Datas);
                            if (!tcpData.IsInvalid)
                            {
                                return;
                            }

                            MDSReceiveLargePackObject tempPack = null;

                            if (TemporaryFlowByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                            {
                                tempPack.TemporaryByteRun = false;
                                tempPack.Clear();
                                TemporaryFlowByteBuffers.Remove(pack.ServerId);


                            }
                            
                                tempPack = new MDSReceiveLargePackObject();
                                TemporaryFlowByteBuffers.Add(pack.ServerId, tempPack);
                                tempPack.ServerID = pack.ServerId;
                                tempPack.ReadTimeout = () => {

                                 

                                    int sendIndex = tempPack.GetSendIndex();
                                    if (sendIndex > 0)
                                    {
                                        TcpData sendData = new TcpData();
                                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = sendIndex.ToString() });//请求接收的
                                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在接收客户端流程图工程，请耐心等待......" });
                                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布传输进度, ScadaClientType.FlowDesign);
                                    }
                                    ClientNormalWriteLog(pack.SourceApplicationName, "接收采集站流程图 第" + ((tempPack.TemporaryByteBuffer.Count + 1)) + "条数据");

                                };
                                tempPack.ReadTimeoutEnd = () => {



                                    TcpData sendData = new TcpData();
                                    sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "接收采集站流程图超时失败" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                    sendData.ChangedToBytes();
                                    ClientNormalWriteLog(pack.SourceApplicationName, "流程发布失败，客户度未上传工程ID，无法更新工程");
                                    this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布失败, ScadaClientType.FlowDesign);
                                    ClientNormalWriteLog(pack.SourceApplicationName, "接收采集站流程图超时失败");
                                    tempPack.Clear();

                                };
                              
                            
                            try
                            {
                                string IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                                string PROJECTID = tcpData.GetItemValue("PROJECTID");
                                string RESULT = tcpData.GetItemValue("RESULT");
                                string MSG = tcpData.GetItemValue("MSG");
                                if (IO_SERVER_ID != "" && IO_SERVER_ID == pack.ServerId)
                                {
                                    tcpData.GetItem("RESULT").Value = "true";
                                    tcpData.GetItem("MSG").Value = "数据中心允许发布流程，请进一步操作";
                                }
                                else
                                {
                                    tcpData.GetItem("RESULT").Value = "false";
                                    tcpData.GetItem("MSG").Value = "发布的流程设计器工程只能在对应的采集站发布";
                                }

                            }
                            catch
                            {
                                tcpData.GetItem("RESULT").Value = "false";
                                tcpData.GetItem("MSG").Value = "发布的流程设计器工程只能在对应的采集站发布";


                            }


                            if (tcpData != null)
                            {
                                tcpData.ChangedToBytes();
                                this.Send(tcpData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布请求反馈, ScadaClientType.FlowDesign);

                            }


                            tcpData.Dispose();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            ClientException(pack.SourceApplicationName, "ERR10021" + ex.Message);
                        }

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            StartPublishFlow = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///开始发布流程
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        MDSReceiveLargePackObject tempPack = null;

                        if (!pack.IsInvalid)
                        {

                            return;
                        }
                        if (!TemporaryFlowByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                        {
                            if (ClientNormalWriteLog != null)
                            {
                                ClientNormalWriteLog(pack.SourceApplicationName, "发布工程失败,采集站节点不明确");
                            }
                        }
                        if (tempPack == null)
                        {
                            return;
                        }
                        #region 上传数据开始
                        TcpData sendData = new TcpData();
                        TcpData tcpData = new TcpData();

                        tcpData.BytesToTcpItem(pack.Datas);
                        if (tcpData == null)
                        {
                            sendData = new TcpData();
                            sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                            sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "流程发布失败，未上传工程ID，无法发布流程" });
                            sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                            sendData.ChangedToBytes();
                            ClientNormalWriteLog(pack.SourceApplicationName, "流程发布失败，客户度未上传工程ID，无法更新工程");
                            this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布失败, ScadaClientType.FlowDesign);
                            tempPack.Clear();
                            return;
                        }
                        //
                        string IO_SERVER_ID = pack.ServerId;
                        string NUMBER = tcpData.GetItemValue("NUMBER");
                        string BYTENUMBER = tcpData.GetItemValue("BYTENUMBER");
          
                        tempPack.TemporaryCounter = int.Parse(NUMBER);
                        tempPack.TemporaryBytesKey = IO_SERVER_ID;
                        tempPack.TemporaryByteRun = true;
                        //下发命令获取第一组数据
                        sendData = new TcpData();
                        sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = (1).ToString() });//请求接收的
                        sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在接收客户端流程图工程，请耐心等待......" });
                        sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                        sendData.ChangedToBytes();
                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布传输进度, ScadaClientType.FlowDesign);
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            PublishingFlow = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = Task.Factory.StartNew(() =>
                    {
                        string IO_SERVER_ID = pack.ServerId;

                     
                        if (IO_SERVER_ID == "" || !pack.IsInvalid)
                        {
                            TcpData sendData = new TcpData();
                            sendData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "false" });
                            sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "IO_SERVER_ID 不能为空" });
                            sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = IO_SERVER_ID });
                            sendData.ChangedToBytes();
                            this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布失败, ScadaClientType.FlowDesign);
                            return;

                        }
                        MDSReceiveLargePackObject tempPack = null;
                        if (!TemporaryFlowByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                        {
                            if (ClientNormalWriteLog != null)
                            {
                                ClientNormalWriteLog(pack.SourceApplicationName, "发布工程失败,采集站节点不明确");
                            }
                        }
                        if (tempPack == null)
                        {
                            return;
                        }

                        #region 接收客户度上传的数据 

                        if (!tempPack.IsReceiveEnd())//表示数据还未接收完
                        {
                            if (pack.Datas.Length > 4)
                            {
                                byte[] packIndexBytes = pack.Datas.Take(4).ToArray();//获取前两个字节，并得到index索引
                                int packIndex = BitConverter.ToInt32(packIndexBytes, 0);

                                bool res = tempPack.AddReceivePack(pack.Datas.Skip(4).ToArray(), packIndex);

                                int sendIndex = tempPack.GetSendIndex();
                                if (sendIndex > 0)
                                {
                                    TcpData sendData = new TcpData();
                                    sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = sendIndex.ToString() });//请求接收的
                                    sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "正在接收客户端流程图工程，请耐心等待......" });
                                    sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                    sendData.ChangedToBytes();
                                    this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布传输进度, ScadaClientType.FlowDesign);
                                }
                                ClientNormalWriteLog(pack.SourceApplicationName, "接收采集站流程图 第" + ((tempPack.TemporaryByteBuffer.Count + 1)) + "条数据");




                            }
                        }
                     
                            //并列执行
                            if (tempPack.IsReceiveEnd())
                            {
                                tempPack.TemporaryByteRun = false;
                                TcpData sendData = new TcpData();
                                sendData.Items.Add(new TcpDataItem() { Key = "PROCESS", Value = "服务器已经全部接收流程图" + tempPack.TemporaryByteBuffer.Count.ToString() });
                                sendData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "流程图正在解析发布，请耐心等待....." });
                                sendData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = "" });
                                sendData.ChangedToBytes();
                                List<byte> allbytes = new List<byte>();
                                for (int i = 1; i <= tempPack.TemporaryCounter; i++)
                                {
                                    byte[] datas = tempPack.GetIndexBytes(i);
                                    if (datas != null && datas.Length > 0)
                                    {
                                        allbytes.AddRange(datas);
                                    }

                                }

                               
                                ClientNormalWriteLog(pack.SourceApplicationName, "已经接收采集站流程图   " + tempPack.TemporaryByteBuffer.Count + "条数据,");
                                try
                                {
                                    string tempid = GUIDToNormalID.GuidToLongID();
                                    StreamWriter sw = new StreamWriter(Application.StartupPath + "/temp/flowtemp" + tempid + ".svg", true, Encoding.UTF8);
                                    sw.Write(System.Text.Encoding.Default.GetString(allbytes.ToArray()));
                                    sw.Close();
                                    ScadaFlowProject projectModel = null;
                                    List<ScadaFlowView> Views = new List<ScadaFlowView>();
                                    ScadaFlowView view = null;
                                    StringBuilder viewSvg = new StringBuilder();
                                    StreamReader sr = new StreamReader(Application.StartupPath + "/temp/flowtemp" + tempid + ".svg", Encoding.UTF8);
                                    while (!sr.EndOfStream)
                                    {
                                        var strLine = sr.ReadLine().Trim();
                                        if (strLine == "")
                                        {
                                            continue;
                                        }

                                        if (strLine.IndexOf("--PROJ") == 0)
                                        {
                                            projectModel = new ScadaFlowProject();
                                            projectModel.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
                                            projectModel.ServerID = pack.ServerId;
                                            projectModel.Title = strLine.Split('#')[2];
                                            projectModel.ProjectId = strLine.Split('#')[1];
                                            Views = new List<ScadaFlowView>();
                                        }
                                        else if (strLine.IndexOf("--VIEW") == 0)
                                        {
                                            if (projectModel != null)
                                            {
                                                view = new ScadaFlowView();
                                                view.ViewSb = new StringBuilder();
                                                view.ProjectId = projectModel.ProjectId.Trim();
                                                view.IsIndex = strLine.Split('#')[3].Trim();
                                                view.ViewTitle = strLine.Split('#')[2].Trim();
                                                view.ViewId = strLine.Split('#')[1].Trim();
                                                view.ViewSVG = "";
                                                Views.Add(view);
                                            }

                                        }
                                        else if (strLine.IndexOf("--ENDVIEW") == 0)
                                        {
                                            if (projectModel != null && view != null)
                                            {
                                                view.ViewSVG = view.ViewSb.ToString();
                                                view = new ScadaFlowView();
                                            }

                                        }
                                        else if (strLine.IndexOf("--USER") == 0)//获取用户工程中的USER
                                        {
                                            if (projectModel != null)
                                            {
                                                //对用户密码进行加密
                                                string nikename = strLine.Split('#')[1].Trim();
                                                string username = strLine.Split('#')[2].Trim();
                                                string password = strLine.Split('#')[3].Trim();
                                                string read = strLine.Split('#')[4].Trim();
                                                string write = strLine.Split('#')[5].Trim();
                                                string str = "{nikename:'" + nikename + "',username:'" + username + "',password:'" + DESEncrypt.Encrypt(password) + "',read:'" + read + "',write:'" + write + "'}";
                                                projectModel.FlowUser += projectModel.FlowUser == "" ? str : "," + str;

                                            }

                                        }
                                        else if (strLine.IndexOf("--ENDUSER") == 0)
                                        {
                                            if (projectModel != null && view != null)
                                            {
                                                view.ViewSVG = view.ViewSb.ToString();
                                                view = new ScadaFlowView();
                                            }

                                        }
                                        else
                                        {
                                            if (view != null)
                                                view.ViewSb.AppendLine(strLine);
                                        }

                                    }
                                    sr.Close();


                                    Scada.Business.ScadaFlowProject projectBll = new Scada.Business.ScadaFlowProject();
                                    Scada.Business.ScadaFlowView viewBll = new Scada.Business.ScadaFlowView();
                                    bool res = false;
                                    if (Views.Count > 0)
                                    {
                                        if (projectBll.Exists(projectModel.ProjectId.Trim()))
                                        {
                                            res = projectBll.UpdateFromProjectId(projectModel);
                                        }
                                        else
                                        {
                                            res = projectBll.Add(projectModel) > 0 ? true : false;
                                        }
                                        if (res)
                                        {
                                            viewBll.Delete(projectModel.ProjectId.Trim());
                                            for (int i = 0; i < Views.Count; i++)
                                            {
                                                if (Views[i].ViewSVG.Trim() != "")
                                                {
                                                    viewBll.Add(Views[i]);
                                                }
                                            }
                                        }
                                        ClientNormalWriteLog(pack.SourceApplicationName, "流程图发布完毕! ");
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布成功, ScadaClientType.FlowDesign);


                                    }
                                    else
                                    {
                                        ClientNormalWriteLog(pack.SourceApplicationName, "流程图发布失败  没有要发布的视图");
                                        sendData.GetItem("MSG").Value = "没有要发布的视图";
                                        sendData.ChangedToBytes();
                                        this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布失败, ScadaClientType.FlowDesign);


                                    }

                                }
                                catch (Exception emx)
                                {
                                    ClientNormalWriteLog(pack.SourceApplicationName, "流程图发布失败  " + emx.Message);
                                    sendData.GetItem("MSG").Value = emx.Message;
                                    sendData.ChangedToBytes();
                                    this.Send(sendData.Datas, pack.ServerId, pack.SourceApplicationName, MDSCommandType.流程发布失败, ScadaClientType.FlowDesign);

                                }
                            }
                     


                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            PublishedFlowSuccess = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        #region 上传数据成功
                        ClientNormalWriteLog(pack.SourceApplicationName, "采集站流程图已经全部发送完毕!");
                        RealCache.Push(new EventCacheObject()
                        {
                            Event = new ScadaEventModel()
                            {
                                SERVER_ID = pack.ServerId,
                                Event = ScadaEvent.流程图发布成功.ToString()


                            }

                        });
                        #endregion

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    RealCache.Push(new EventCacheObject()
                    {
                        Event = new ScadaEventModel()
                        {
                            SERVER_ID = pack.ServerId,
                            Event = ScadaEvent.流程图发布失败.ToString(),
                            Content = ex.Message


                        }

                    });
                    return null;
                }
            };
            PublishedFlowFailt = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        #region 上传数据失败

                        MDSReceiveLargePackObject tempPack = null;
                        string IO_SERVER_ID = pack.ServerId;
                        if (IO_SERVER_ID == "" || !pack.IsInvalid)
                        {

                            if (!TemporaryFlowByteBuffers.TryGetValue(pack.ServerId, out tempPack))
                            {
                                if (ClientNormalWriteLog != null)
                                {
                                    ClientNormalWriteLog(pack.SourceApplicationName, "发布流程失败,采集站节点不明确");
                                }
                            }
                            if (tempPack == null)
                            {
                                return;
                            }

                            tempPack.Clear();
              
                            return;

                        }
                        ClientNormalWriteLog(pack.SourceApplicationName, "采集站流程图上传失败");
                        RealCache.Push(new EventCacheObject()
                        {
                            Event = new ScadaEventModel()
                            {
                                SERVER_ID = pack.ServerId,
                                Event = ScadaEvent.流程图发布失败.ToString(),
                                Content = "采集站流程图上传失败"


                            }

                        });
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            UpdateMonitorAlarmResult = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///更新采集站端发送的报警
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        {

                            #region 更新采集站报警反馈
                            TcpData tcpData = new TcpData();

                            tcpData.BytesToTcpItem(pack.Datas);
                            if (tcpData == null)
                            {
                                //解析字符串失败
                                return;
                            }
                            IO_ALARM_CONFIG alarmConfig = new IO_ALARM_CONFIG();
                            try
                            {
                                alarmConfig.IO_ALARM_LEVEL = tcpData.GetItemValue("IO_ALARM_LEVEL");
                                alarmConfig.IO_ALARM_NUMBER = int.Parse(tcpData.GetItemValue("IO_ALARM_NUMBER"));
                                alarmConfig.IO_ALARM_TYPE = tcpData.GetItemValue("IO_ALARM_TYPE");
                                alarmConfig.IO_COMM_ID = tcpData.GetItemValue("IO_COMM_ID");
                                alarmConfig.IO_CONDITION = tcpData.GetItemValue("IO_CONDITION");
                                alarmConfig.IO_DEVICE_ID = tcpData.GetItemValue("IO_DEVICE_ID");
                                alarmConfig.IO_ENABLE_MAX = int.Parse(tcpData.GetItemValue("IO_ENABLE_MAX"));
                                alarmConfig.IO_ENABLE_MAXMAX = int.Parse(tcpData.GetItemValue("IO_ENABLE_MAXMAX"));
                                alarmConfig.IO_ENABLE_MIN = int.Parse(tcpData.GetItemValue("IO_ENABLE_MIN"));
                                alarmConfig.IO_ENABLE_MINMIN = int.Parse(tcpData.GetItemValue("IO_ENABLE_MINMIN"));
                                alarmConfig.IO_ID = tcpData.GetItemValue("IO_ID");
                                alarmConfig.IO_MAXMAX_TYPE = tcpData.GetItemValue("IO_MAXMAX_TYPE");
                                alarmConfig.IO_MAXMAX_VALUE = int.Parse(tcpData.GetItemValue("IO_MAXMAX_VALUE"));
                                alarmConfig.IO_MAX_TYPE = tcpData.GetItemValue("IO_MAX_TYPE");
                                alarmConfig.IO_MAX_VALUE = int.Parse(tcpData.GetItemValue("IO_MAX_VALUE"));
                                alarmConfig.IO_MINMIN_TYPE = tcpData.GetItemValue("IO_MINMIN_TYPE");
                                alarmConfig.IO_MINMIN_VALUE = int.Parse(tcpData.GetItemValue("IO_MINMIN_VALUE"));
                                alarmConfig.IO_MIN_TYPE = tcpData.GetItemValue("IO_MIN_TYPE");
                                alarmConfig.IO_MIN_VALUE = int.Parse(tcpData.GetItemValue("IO_MIN_VALUE"));
                                alarmConfig.IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                                alarmConfig.UPDATE_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                alarmConfig.UPDATE_RESULT = "true";
                                alarmConfig.UPDATE_UID = "";
                            }
                            catch (Exception ex)
                            {
                                ClientException(pack.SourceApplicationName, "ERROR30102  " + ex.Message);
                                alarmConfig = null;
                            }
                            try
                            {
                                if (alarmConfig != null)
                                {
                                    IO_COMMUNICATION communication = null;
                                    IO_DEVICE device = null;
                                    IO_PARA para = null;

                                    communication = IOCenterManager.IOProject.Communications.Find(x => x.IO_COMM_ID.Trim() == alarmConfig.IO_COMM_ID.Trim());
                                    if (communication != null)
                                    {
                                        device = communication.Devices.Find(x => x.IO_DEVICE_ID.Trim() == alarmConfig.IO_DEVICE_ID.Trim());
                                    }

                                    if (device != null)
                                    {
                                        para = device.IOParas.Find(x => x.IO_ID.Trim() == alarmConfig.IO_ID.Trim());
                                    }

                                    if (alarmConfigBll.UserResultUpdate(alarmConfig))
                                    {
                                        try
                                        {


                                            IOCenterManager.InfluxDbManager.DbWrite_AlarmConfigPoints(alarmConfig.IO_SERVER_ID, alarmConfig.IO_COMM_ID, alarmConfig, DateTime.Now);
                                            ClientNormalWriteLog(pack.SourceApplicationName, "管理员更新 IO ID " + alarmConfig.IO_ID + "报警配置成功! ");

                                            alarmConfig = null;
                                        }
                                        catch (Exception ex)
                                        {
                                            ClientException(pack.SourceApplicationName, "ERR12134" + ex.Message);
                                        }
                                    }
                                    else
                                    {

                                        ClientNormalWriteLog(pack.SourceApplicationName, "管理员更新 IO ID " + alarmConfig.IO_ID + "报警配置失败! ");
                                        alarmConfig = null;
                                    }



                                }
                            }
                            catch (Exception ex)
                            {
                                ClientException(pack.SourceApplicationName, "ERROR40105  " + ex.Message);
                                alarmConfig = null;
                            }


                            tcpData.Dispose();
                            #endregion
                        }
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            ReceiveRealData = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        //必须保证采集站已经是发布的，否则不能接收传递的数据


                        RealTransform(pack);

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };

            ReceiveEventData = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        //必须保证采集站已经是发布的，否则不能接收传递的数据


                        EventTransform(pack);

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50015  " + ex.Message);
                    }
                    return null;
                }
            };

            ReceiveAlarmData = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        //必须保证采集站已经是发布的，否则不能接收传递的数据
                        AlarmTransform(pack);

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };
            ReceiveScadaStatusData = (SplitPackageMessage pack) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        //必须保证采集站已经是发布的，否则不能接收传递的数据
                        ScadaStatusTransform(pack);

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };


            SendCommand = (SplitPackageMessage pack, IOutgoingMessage returnWebMessage) =>
            {
                try
                {
                    ///客户端登录的实现
                    var task = TaskHelper.Factory.StartNew(() =>
                    {
                        #region 下置命令
                        try
                        {

                            TcpData tcpData = new TcpData();
                            tcpData.BytesToTcpItem(pack.Datas);
                            string serverId = tcpData.GetItemValue("IO_SERVER_ID");

                            ClientNormalWriteLog(pack.SourceApplicationName, "Web端向采集站" + mDSServerConfig.MonitorAppPrefix + "_" + serverId + "发送下置命令" + tcpData.TcpItemToString() + ",消息通知");
                            //处理UI端显示信息


                            IO_COMMANDS command = new IO_COMMANDS()
                            {
                                COMMAND_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                COMMAND_ID = tcpData.GetItemValue("COMMAND_ID"),
                                COMMAND_RESULT = "Web下置通知",
                                COMMAND_USER = tcpData.GetItemValue("COMMAND_USER"),
                                COMMAND_VALUE = tcpData.GetItemValue("COMMAND_VALUE"),
                                IO_COMM_ID = tcpData.GetItemValue("IO_COMM_ID"),
                                IO_DEVICE_ID = tcpData.GetItemValue("IO_DEVICE_ID"),
                                IO_ID = tcpData.GetItemValue("IO_ID"),
                                IO_LABEL = tcpData.GetItemValue("IO_LABEL"),
                                IO_NAME = tcpData.GetItemValue("IO_NAME"),
                                IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID")
                            };
                            IO_COMMUNICATION common = IOCenterManager.IOProject.Communications.Find(x => x.IO_SERVER_ID.Trim().ToLower() == serverId.Trim().ToLower() && x.IO_COMM_ID == command.IO_COMM_ID);
                            IO_DEVICE device = IOCenterManager.IOProject.Devices.Find(x => x.IO_SERVER_ID.Trim().ToLower() == serverId.Trim().ToLower() && x.IO_COMM_ID == command.IO_COMM_ID && x.IO_DEVICE_ID == command.IO_DEVICE_ID);
                            if (common != null && device != null && OnRefreshCommandListView != null)
                            {

                                OnRefreshCommandListView(pack.SourceApplicationName, command.IO_SERVER_ID, common.IO_COMM_NAME + "[" + common.IO_COMM_LABEL + "]", device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]", command.IO_NAME + "[" + command.IO_LABEL + "]", command);
                            }



                        }
                        catch (Exception emx)
                        {
                            ClientException(pack.SourceApplicationName, emx.Message);
                        }
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException(pack.SourceApplicationName, "ERROR50012  " + ex.Message);
                    }
                    return null;
                }
            };

            //数据读写缓存
            RealCache = new IOCenterCacheManager();
            ///批量写入实时数据库每次最多3000条
            RealCache.InsertInfluxdb = (List<ReceiveCacheObject> result) =>
            {
                if (IOCenterManager.InfluxDbManager == null)
                    return false;
                try
                {
                    ///批量执行influxdb的写入，一次写入1000条数据
                    List<IO_DEVICE> devices = new List<IO_DEVICE>();
                    result.ForEach(delegate (ReceiveCacheObject p)
                    {

                        devices.Add(p.device);
                    });
                    if (devices.Count > 0)
                    {
                        IOCenterManager.InfluxDbManager.DbWrite_RealPoints(devices);
                    }
                    for (int i = 0; i < devices.Count; i++)
                    {
                        devices[i].Dispose();
                        devices[i] = null;

                    }
                    devices.Clear();
                    devices = null;
                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].Dispose();
                        result[i] = null;
                    }

                    result.Clear();
                    result = null;
                    return true;



                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }

            };

            //批量写入报警数据
            RealCache.InsertAlarmInfluxdb = (List<AlarmCacheObject> result) =>
            {
                if (IOCenterManager.InfluxDbManager == null)
                    return false;

                try
                {
                    ///批量执行influxdb的写入，一次写入1000条数据
                    List<IO_PARAALARM> alarms = new List<IO_PARAALARM>();
                    result.ForEach(delegate (AlarmCacheObject p)
                    {
                        if (p.Alarm != null)
                        {
                            alarms.Add(p.Alarm);
                        }
                    });
                    if (alarms.Count > 0)
                    {
                        IOCenterManager.InfluxDbManager.DbWrite_AlarmPoints(alarms);
                    }

                    for (int i = 0; i < alarms.Count; i++)
                    {
                        alarms[i].Dispose();
                        alarms[i] = null;

                    }
                    alarms.Clear();
                    alarms = null;
                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].Dispose();
                        result[i] = null;
                    }
                    result.Clear();
                    result = null;
                    return true;



                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }

            };

            //批量写入事件数据
            RealCache.InsertEventInfluxdb = (List<EventCacheObject> result) =>
            {
                if (IOCenterManager.InfluxDbManager == null)
                    return false;
                try
                { ///批量执行influxdb的写入，一次写入1000条数据
                    List<ScadaEventModel> events = new List<ScadaEventModel>();
                    result.ForEach(delegate (EventCacheObject p)
                    {
                        if (p.Event != null)
                        {
                            events.Add(p.Event);
                        }
                    });
                    if (events.Count > 0)
                    {
                        IOCenterManager.InfluxDbManager.DbWrite_EventPoints(events);
                    }
                    for (int i = 0; i < events.Count; i++)
                    {
                        events[i].Dispose();
                        events[i] = null;

                    }
                    events.Clear();
                    events = null;

                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].Dispose();
                        result[i] = null;
                    }
                    result.Clear();
                    result = null;

                    return true;

                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }

            };
            //批量写入状态数据
            RealCache.InsertStatusInfluxdb = (List<ScadaStatusCacheObject> result) =>
            {
                if (IOCenterManager.InfluxDbManager == null)
                    return false;
                try
                {
                    //定时从缓存区上传数据
                    ///批量执行influxdb的写入，一次写入1000条数据

                    if (result.Count > 0)
                    {
                        IOCenterManager.InfluxDbManager.DbWrite_StatusPoints(result);
                    }


                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].Dispose();
                        result[i] = null;
                    }
                    result.Clear();
                    result = null;
                    return true;



                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }

            };
            ///批量写入机器学习预测数据

            RealCache.InsertForecastInfluxdb = (List<ScadaMachineTrainingForecastCacheObject> result) =>
            {
                if (IOCenterManager.InfluxDbManager == null)
                    return false;
                if (result == null)
                    return false;
                try
                {
                    //定时从缓存区上传数据
                    ///批量执行influxdb的写入，一次写入1000条数据

                    if (result!=null&&result.Count > 0)
                    {
                      IOCenterManager.InfluxDbManager.DbWrite_ForeastPoints(result).GetAwaiter() ;
                        TaskHelper.Factory.StartNew(() =>
                        {
                            if (OnRefreshAiTrainDeviceListView != null)
                            {

                                result.ForEach(delegate (ScadaMachineTrainingForecastCacheObject cacheObject)
                                {

                                    IO_COMMUNICATION communication = IOCenterManager.IOProject.Communications.Find(x => x.IO_SERVER_ID == cacheObject.MachineTrainingForecast.SERVER_ID
                                    && x.IO_COMM_ID == cacheObject.MachineTrainingForecast.COMM_ID);
                                    IO_DEVICE device = IOCenterManager.IOProject.Devices.Find(x => x.IO_SERVER_ID == cacheObject.MachineTrainingForecast.SERVER_ID
                                    && x.IO_COMM_ID == cacheObject.MachineTrainingForecast.COMM_ID && x.IO_DEVICE_ID == cacheObject.MachineTrainingForecast.DEVICE_ID);
                                    if (communication != null && device != null)
                                    {
                                        OnRefreshAiTrainDeviceListView(cacheObject.MachineTrainingForecast.SERVER_ID, cacheObject.MachineTrainingForecast.PredictedDate.ToString("yyyy-MM-dd HH:mm:ss"), cacheObject.MachineTrainingForecast.SERVER_NAME,
                                        communication.IO_COMM_NAME, device.IO_DEVICE_NAME, cacheObject.MachineTrainingForecast.TaskName, cacheObject.MachineTrainingForecast.Algorithm, cacheObject.MachineTrainingForecast.AlgorithmType
                                        , cacheObject.MachineTrainingForecast.PredictedLabel, cacheObject.MachineTrainingForecast.Score, cacheObject.MachineTrainingForecast.FeaturesValue, cacheObject.MachineTrainingForecast.FeaturesName);
                                    }
                                });

                            }


                        });

                    }
                 

                    return true;



                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }

            };
            RealCache.InsertBatchCommandInfluxdb = (List<BatchCommandCacheObject> result) =>
            {
                List<BatchCommandInfluxDBModel> models = new List<BatchCommandInfluxDBModel>();
                result.ForEach(delegate (BatchCommandCacheObject cache) {
                    models.Add((BatchCommandInfluxDBModel)cache);
                });
                if (IOCenterManager.InfluxDbManager == null)
                    return false;
                try
                {
                    //定时从缓存区上传数据
                    ///批量执行influxdb的写入，一次写入1000条数据

                    if (result.Count > 0)
                    {
                        IOCenterManager.InfluxDbManager.DbWrite_BatchCommandPoints(models);
                    }


                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].Dispose();
                        result[i] = null;
                    }
                    result.Clear();
                    result = null;
                    models.Clear();
                    models = null;
                    return true;



                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }


            };

            ///写入机器训练日志
            RealCache.InsertMachineTrainInfluxdb = (List<MachineTrainCacheObject> result) =>
            {
                List<MachineTrainInfluxDBModel> models = new List<MachineTrainInfluxDBModel>();
                result.ForEach(delegate (MachineTrainCacheObject cache) {
                    models.Add((MachineTrainInfluxDBModel)cache);
                });
                if (IOCenterManager.InfluxDbManager == null)
                    return false;
                try
                {
                    //定时从缓存区上传数据
                    ///批量执行influxdb的写入，一次写入1000条数据

                    if (result.Count > 0)
                    {
                        IOCenterManager.InfluxDbManager.DbWrite_MachineTrainPoints(models);
                    }


                    for (int i = 0; i < result.Count; i++)
                    {
                        result[i].Dispose();
                        result[i] = null;
                    }
                    result.Clear();
                    result = null;
                    models.Clear();
                    models = null;
                    return true;



                }
                catch (Exception ex)
                {
                    if (ClientException != null)
                    {
                        ClientException("", "ERROR50012  " + ex.Message);
                    }
                    return false;
                }

            };

            RealCache.CacheInformation = (string msg) =>
            {
                return ClientException("数据中心缓存", msg);
            };

            RealCache.Read();
        }
        private void RealTransform(SplitPackageMessage pack)
        {

            // 单独处理每个分包数据
            #region 处理采集器端传递的实时值

         
          
            List<string> recSources = Encoding.UTF8.GetString(pack.Datas, 0, pack.Datas.Length).Split(new char[1] { '^' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (recSources.Count <= 0)
            {
                return;
            }
            for (int s = 0; s < recSources.Count; s++)
            {
                string source = recSources[s];
                try
                {
                    TcpData tcpData = new TcpData();
                    //解析上传的每个数据
                    byte[] narra = Encoding.UTF8.GetBytes(source);
                    tcpData.BytesToTcpItem(narra);
                    string TcpDataString = "";
                    bool IsInvalid = false;
                    DateTime? device_date = null;
                    long dateunix = 0;
                    string server_id = tcpData.GetItemValue("IO_SERVER_ID");
                    string communication_id = tcpData.GetItemValue("IO_COMM_ID");
                    string device_id = tcpData.GetItemValue("IO_DEVICE_ID");
                    if (server_id == "" || communication_id == "" || device_id == "")
                    {
                        tcpData.Dispose();
                        tcpData = null;
                        narra = null;
                        continue;
                    }
                    string date = tcpData.GetItemValue("DATE");
                    if (date != "" && long.TryParse(date, out dateunix))
                    {
                        device_date = UnixDateTimeConvert.ConvertIntDateTime(long.Parse(date));
                    }
                    else
                    {
                        device_date = DateTime.Now;
                    }
                    if (tcpData != null)
                    {
                        IsInvalid = tcpData.IsInvalid;
                        TcpDataString = tcpData.TcpDataString;
                    }
                    if (IsInvalid == false)
                    {
                        ClientNormalWriteLog(pack.SourceApplicationName, "数据单元无效，无法入库");
                        tcpData.Dispose();
                        tcpData = null;
                        narra = null;
                        TcpDataString = "";
                        date = "";
                        device_date = null;
                        continue;
                    }

                    if (server_id != "ERROR" && communication_id != "ERROR" && device_id != "ERROR")
                    {
                        #region  
                        IO_SERVER server = null;
                        IO_DEVICE device = null;
                        IO_COMMUNICATION communication = null;
                        lock (server_id)
                        {
                            IO_SERVER exserver = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID.Trim() == server_id.Trim());
                            IO_COMMUNICATION excommunication = IOCenterManager.IOProject.Communications.Find(x => x.IO_COMM_ID.Trim() == communication_id.Trim());
                            IO_DEVICE exdevice = excommunication != null ? excommunication.Devices.Find(x => x.IO_DEVICE_ID == device_id) : null;
                            server = exserver != null ? exserver.Copy() : null;
                            communication = excommunication != null ? excommunication.Copy() : null;
                            device = exdevice != null ? exdevice.Copy() : null;
                            if (device != null && device.IOParas != null && device.IOParas.Count > 0)
                            {
                                exdevice.IO_DEVICE_STATUS = 1;
                                device.GetedValueDate = device_date;
                                for (int i = 0; i < device.IOParas.Count; i++)
                                {
                                    string itemValue = tcpData.GetItemValue(device.IOParas[i].IO_NAME);
                                    if (itemValue != null && itemValue != "" && itemValue != "ERROR")
                                    {
                                        if (device != null)
                                        {
                                            string[] vs = itemValue.Split('|');
                                            device.IOParas[i].IORealData = new Scada.IOStructure.IOData();


                                            if (vs.Length > 0)
                                            {
                                                device.IOParas[i].IORealData.ID = itemValue.Split('|')[0];
                                            }
                                            else
                                            {
                                                device.IOParas[i].IORealData.ID = device.IOParas[i].IO_ID;
                                            }

                                            device.IOParas[i].IORealData.ServerID = device.IO_SERVER_ID;
                                            device.IOParas[i].IORealData.Date = device_date;
                                            device.IOParas[i].IORealData.ParaName = device.IOParas[i].IO_NAME;

                                            if (vs.Length > 1)
                                            {
                                                device.IOParas[i].IORealData.ParaValue = itemValue.Split('|')[1];
                                            }
                                            else
                                            {
                                                device.IOParas[i].IORealData.ParaValue = "-9999";
                                            }

                                            if (device.IOParas[i].IORealData.ParaValue.Trim() == "")
                                            {
                                                device.IOParas[i].IORealData.ParaValue = "-9999";
                                            }

                                            QualityStamp qs = QualityStamp.BAD;

                                            if (vs.Length > 2)
                                            {
                                                if (Enum.TryParse(itemValue.Split('|')[2], out qs))
                                                {
                                                    device.IOParas[i].IORealData.QualityStamp = qs;
                                                }
                                                else
                                                {
                                                    device.IOParas[i].IORealData.QualityStamp = QualityStamp.BAD;
                                                }
                                            }
                                            else
                                            {
                                                device.IOParas[i].IORealData.QualityStamp = QualityStamp.BAD;
                                            }

                                            if (device.IOParas[i].IORealData.ParaValue.Trim() == "-9999")
                                            {
                                                device.IOParas[i].IORealData.QualityStamp = QualityStamp.BAD;
                                            }
                                        }
                                    }

                                }
                                if (device != null && communication != null && server != null)
                                {
                                    //执行一个自动控制触发任务
                                    for (int i = 0; i < device.IOParas.Count; i++)
                                    {
                                        float v = -9999;
                                        if (device.IOParas[i].IORealData != null
                                        && device.IOParas[i].IO_POINTTYPE != "字符串量"
                                        && device.IOParas[i].IO_POINTTYPE != "计算值"
                                        && device.IOParas[i].IO_POINTTYPE != "关系数据库值"
                                        && !string.IsNullOrEmpty(device.IOParas[i].IORealData.ParaValue)
                                        && device.IOParas[i].IORealData.ParaValue != "-9999"
                                        &&float.TryParse(device.IOParas[i].IORealData.ParaValue,out v)
                                        )
                                        {
                                            IOCenterManager.BatchCommandTask.ExecuteIOTriggerTask(v, device.IOParas[i]);
                                        }

                                    }
                                   
                                    if (RealCache != null)
                                    {
                                        //将接收到的数据保存到缓存,通过缓存定时批量写入，每次写入不超过1000条的数据，主要为了提高效率
                                        RealCache.Push(new ReceiveCacheObject()
                                        {
                                            communication = communication,
                                            device = device,
                                            RealDate = device_date,
                                            server= server

                                        });
                                   
                                    }

                                    TaskHelper.Factory.StartNew(() =>
                                    {
                                        if (OnRefreshReeiveDeviceListView != null)
                                        {
                                            OnRefreshReeiveDeviceListView(pack.SourceApplicationName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), server.SERVER_NAME, communication.IO_COMM_NAME, device.IO_DEVICE_NAME, TcpDataString, true);

                                        }
                                    });
                                  
                                }


                            }


                        }
                        #endregion
                    }
                    //释放内存
                    tcpData.Dispose();
                    tcpData = null;
                    narra = null;
                
                    date = "";
                    device_date = null;
                    server_id = "";
                    communication_id = "";
                    device_id = "";
                }
                catch
                {
                    continue;
                }
                source = null;
            }
            #endregion
            for (int i = 0; i < recSources.Count; i++)
            {
                recSources[i] = "";
            }
            recSources.Clear();
            recSources = null;
          

        }
        private void AlarmTransform(SplitPackageMessage pack)
        {


            #region 处理采集器端传递的实时值

            List<string> resStrings = Encoding.UTF8.GetString(pack.Datas, 0, pack.Datas.Length).Split(new char[1] { '^' }, StringSplitOptions.RemoveEmptyEntries).ToList();

 
            for (int s = 0; s < resStrings.Count; s++)
            {
                string source = resStrings[s];
                TcpData tcpData = new TcpData();
                byte[] narra = Encoding.UTF8.GetBytes(source);
                tcpData.BytesToTcpItem(narra);
                string server_id = tcpData.GetItemValue("IO_SERVER_ID");
                string communication_id = tcpData.GetItemValue("IO_COMM_ID");
                string device_id = tcpData.GetItemValue("IO_DEVICE_ID");
                string TcpDataString = "";
                if (server_id == "" || communication_id == "" || device_id == "")
                {
                    tcpData.Dispose();
                    tcpData = null;
                    TcpDataString = "";
                    server_id = "";
                    communication_id = "";
                    device_id = "";
                    narra = null;
                    source = "";
                    continue;
                }

                bool IsInvalid = false;
                if (tcpData != null)
                {
                    IsInvalid = tcpData.IsInvalid;
                    TcpDataString = tcpData.TcpDataString;
                }
                if (IsInvalid == false)
                {
                    ClientNormalWriteLog(pack.SourceApplicationName, "报警数据单元无效，无法入库");
                    tcpData.Dispose();
                    tcpData = null;
                    TcpDataString = "";
                    server_id = "";
                    communication_id = "";
                    device_id = "";
                    narra = null;
                    source = "";
                    continue;
                }


                IO_PARAALARM paraAlarm = new IO_PARAALARM();
                paraAlarm.IO_ALARM_DATE = tcpData.GetItemValue("IO_ALARM_DATE").Replace("//", "#").Replace("\\", ":");
                paraAlarm.IO_COMM_ID = tcpData.GetItemValue("IO_COMM_ID");
                paraAlarm.IO_DEVICE_ID = tcpData.GetItemValue("IO_DEVICE_ID");
                paraAlarm.IO_SERVER_ID = server_id;
                paraAlarm.IO_ID = tcpData.GetItemValue("IO_ID");
                paraAlarm.IO_ALARM_LEVEL = tcpData.GetItemValue("IO_ALARM_LEVEL");
                paraAlarm.IO_ALARM_VALUE = tcpData.GetItemValue("IO_ALARM_VALUE");
                paraAlarm.IO_ALARM_TYPE = tcpData.GetItemValue("IO_ALARM_TYPE");
                paraAlarm.IO_LABEL = tcpData.GetItemValue("IO_LABEL");
                paraAlarm.IO_NAME = tcpData.GetItemValue("IO_NAME");

                DateTime alDate = DateTime.Now;
                if (!DateTime.TryParse(paraAlarm.IO_ALARM_DATE, out alDate))
                {
                    paraAlarm.IO_ALARM_DATE = alDate.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (server_id != "ERROR" && communication_id != "ERROR" && device_id != "ERROR")
                {
                    lock (server_id)
                    {
                        IO_SERVER exserver = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID.Trim() == server_id.Trim());
                    
                        #region 构造同样的三个类，主要防止多线程对原来数据修改导致错误

                        IO_COMMUNICATION excommunication = IOCenterManager.IOProject.Communications.Find(x => x.IO_COMM_ID.Trim() == communication_id.Trim());
                        IO_DEVICE existdevice = excommunication != null ? excommunication.Devices.Find(x => x.IO_DEVICE_ID == device_id) : null;
                        IO_SERVER server = exserver != null ? exserver.Copy() : null;
                        IO_COMMUNICATION communication = excommunication != null ? excommunication.Copy() : null;
                        IO_DEVICE device = existdevice != null ? existdevice.Copy() : null;
                        if (device != null && communication != null && server != null)
                        {
                            paraAlarm.DEVICE_NAME = device.IO_DEVICE_NAME;

                            if (RealCache != null && !string.IsNullOrEmpty(paraAlarm.IO_ALARM_DATE))
                            {
                                //将接收到的数据保存到缓存,通过缓存定时批量写入，每次写入不超过1000条的数据，主要为了提高效率
                                RealCache.Push(new AlarmCacheObject()
                                {
                                    communication = communication,
                                    device = device,
                                    Alarm = paraAlarm

                                });
                            }

                            TaskHelper.Factory.StartNew(() =>
                            {
                                if (OnRefreshReeiveAlarmListView != null)
                                {
                                    OnRefreshReeiveAlarmListView(pack.SourceApplicationName, server.SERVER_NAME, communication.IO_COMM_NAME, device.IO_DEVICE_NAME, paraAlarm, true);
                                }
                            });
                             

                        }
                    }
                    #endregion
                }
               
              
            }

            #endregion
             
      
            for (int i = 0; i < resStrings.Count; i++)
            {
                resStrings[i] = "";
            }
            resStrings.Clear();
            resStrings = null;
        }
        /// <summary>
        /// 接收到设备和通道状态信息
        /// </summary>
        /// <param name="pack"></param>
        private void ScadaStatusTransform(SplitPackageMessage pack)
        {


            #region 处理采集器端传递的实时值
            List<string> resStrings = Encoding.UTF8.GetString(pack.Datas, 0, pack.Datas.Length).Split(new char[1] { '^' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            
            for (int s = 0; s < resStrings.Count; s++)
            {
                string source = resStrings[s];
                TcpData tcpData = new TcpData();
                byte[] narra = Encoding.UTF8.GetBytes(source);
                tcpData.BytesToTcpItem(narra);
                if (tcpData.IsInvalid == false)
                {
                    tcpData.Dispose();
                    tcpData = null;

                    narra = null;
                    source = "";
                    continue;
                }


                ScadaStatusCacheObject status = new ScadaStatusCacheObject();
                status.COMM_ID = tcpData.GetItemValue("COMM_ID");
                status.DEVICE_ID = tcpData.GetItemValue("DEVICE_ID");
                status.SERVER_ID = tcpData.GetItemValue("SERVER_ID");
                Enum.TryParse(tcpData.GetItemValue("ScadaStatus"), out status.ScadaStatus);
                Enum.TryParse(tcpData.GetItemValue("StatusElemnt"), out status.StatusElemnt);
                IO_SERVER exserver = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID.Trim() == status.SERVER_ID.Trim());
                #region 构造同样的三个类，主要防止多线程对原来数据修改导致错误
                IO_COMMUNICATION excommunication = IOCenterManager.IOProject.Communications.Find(x => x.IO_COMM_ID.Trim() == status.COMM_ID.Trim());
                IO_DEVICE existdevice = excommunication != null ? excommunication.Devices.Find(x => x.IO_DEVICE_ID == status.DEVICE_ID) : null;
                IO_SERVER server = exserver != null ? exserver.Copy() : null;
                IO_COMMUNICATION communication = excommunication != null ? excommunication.Copy() : null;
                IO_DEVICE device = existdevice != null ? existdevice.Copy() : null;
                if (communication != null && server != null)
                {
                    if (status.StatusElemnt == ScadaStatusElemnt.Device && OnRefreshMainTreeDeviceStatus != null&& device!=null)
                    {
                        existdevice.IO_DEVICE_STATUS = (status.ScadaStatus == ScadaStatus.True ? 1 : 0);
                        RealCache.Push(new ScadaStatusCacheObject()
                        {
                           COMM_ID= device.IO_COMM_ID,
                            DEVICE_ID= device.IO_DEVICE_ID,
                             ScadaStatus= status.ScadaStatus,
                              SERVER_ID= device.IO_SERVER_ID,
                               StatusElemnt= ScadaStatusElemnt.Device

                        });
                      
                    }
                    else if (status.StatusElemnt == ScadaStatusElemnt.Communication && OnRefreshMainTreeCommunicationStatus != null&& communication!=null)
                    {
                        excommunication.IO_COMM_STATUS = status.ScadaStatus == ScadaStatus.True ? 1 : 0;
                        OnRefreshMainTreeCommunicationStatus(communication, status.ScadaStatus == ScadaStatus.True ? true : false);
                    }
                }
               
                #endregion

                

            }
          
            #endregion
            
        
            for (int i = 0; i < resStrings.Count; i++)
            {
                resStrings[i] = "";
            }
            resStrings.Clear();
            resStrings = null;
        }

        
        private void EventTransform(SplitPackageMessage pack)
        {
            #region 处理采集器端传递的实时值

            List<string> resStrings = Encoding.UTF8.GetString(pack.Datas, 0, pack.Datas.Length).Split(new char[1] { '^' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int s = 0; s < resStrings.Count; s++)
            {
                string source = resStrings[s];

                TcpData tcpData = new TcpData();
                byte[] narra = Encoding.UTF8.GetBytes(source);
                tcpData.BytesToTcpItem(narra);
                string server_id = tcpData.GetItemValue("SERVER_ID");
                string communication_id = tcpData.GetItemValue("COMM_ID");
                string device_id = tcpData.GetItemValue("DEVICE_ID");
                string TcpDataString = "";
                bool IsInvalid = false;
                if (server_id == "" || communication_id == "" || device_id == "")
                {
                    tcpData.Dispose();
                    tcpData = null;
                    TcpDataString = "";
                    narra = null;
                    source = "";
                    server_id = "";
                    communication_id = "";
                    device_id = "";
                    continue;
                }

                if (tcpData != null)
                {
                    IsInvalid = tcpData.IsInvalid;
                    TcpDataString = tcpData.TcpDataString;
                }
                if (IsInvalid == false)
                {
                    ClientNormalWriteLog(pack.SourceApplicationName, "事件数据无效，无法入库");

                    tcpData.Dispose();
                    tcpData = null;
                    TcpDataString = "";
                    narra = null;
                    source = "";
                    server_id = "";
                    communication_id = "";
                    device_id = "";

                    continue;
                }



                ScadaEventModel eventModel = null;
                eventModel = new ScadaEventModel();
                eventModel.Id = tcpData.GetItemValue("Id");
                eventModel.Date = tcpData.GetItemValue("Date").Replace("//", "#").Replace("\\", ":");
                eventModel.Event = tcpData.GetItemValue("Event");
                eventModel.SERVER_ID = tcpData.GetItemValue("SERVER_ID");
                eventModel.COMM_ID = tcpData.GetItemValue("COMM_ID");
                eventModel.COMM_NAME = tcpData.GetItemValue("COMM_NAME");
                eventModel.DEVICE_ID = tcpData.GetItemValue("DEVICE_ID");
                eventModel.DEVICE_NAME = tcpData.GetItemValue("DEVICE_NAME");
                eventModel.Content = tcpData.GetItemValue("Content");
                eventModel.IO_ID = tcpData.GetItemValue("IO_ID");
                eventModel.IO_LABEL = tcpData.GetItemValue("IO_LABEL");
                eventModel.IO_NAME = tcpData.GetItemValue("IO_NAME");
                DateTime alDate = DateTime.Now;
                if (!DateTime.TryParse(eventModel.Date, out alDate))
                {
                    eventModel.Date = alDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (string.IsNullOrEmpty(eventModel.Content))
                {
                    eventModel.Content = eventModel.Event;
                }
                if (RealCache != null && !string.IsNullOrEmpty(eventModel.Date))
                {
                    //将接收到的数据保存到缓存,通过缓存定时批量写入，每次写入不超过1000条的数据，主要为了提高效率
                    RealCache.Push(new EventCacheObject()
                    {
                        Event = eventModel

                    });
                }
                TaskHelper.Factory.StartNew(() =>
                {
                    //显示事件信息
                    if (OnRefreshEventListView != null)
                    {
                        OnRefreshEventListView(pack.SourceApplicationName, eventModel.Date, eventModel.Event, eventModel.SERVER_ID, eventModel.COMM_NAME, eventModel.DEVICE_NAME, eventModel.Content);
                    }
                });

            }
            #endregion
            
            
           
            for (int i = 0; i < resStrings.Count; i++)
            {
                resStrings[i] = "";
            }
            resStrings.Clear();
            resStrings = null;
        }
        public IOutgoingMessage CreateMessage()
        {
            return MDSClient.CreateMessage();
        }
        public void Connect()
        {

            MDSClient.Connect();
           


        }
      
        /// <summary>
        /// 处理服务器端接收到的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MDSClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            
            try
            {
                e.Message.Acknowledge();
                //如果服务器正在发布工程后重启，则暂停服务
                if (IOCenterManager.PublishRestart)
                    return;
                #region
                ///获取接收的数据包
                byte[] datas = e.Message.MessageData;
                SplitPackageMessage package = SplitPackage.RemoveIdentificationBytes(datas);
                //解析的数据无效或者不符合要求，则不进行处理
                if (!package.IsInvalid || package.Length <= 0 || package.ServerId == "")
                {
                    return;
                }
                package.DestinationApplicationName = e.Message.DestinationApplicationName;
                package.DestinationCommunicatorId = e.Message.DestinationCommunicatorId;
                package.DestinationServerName = e.Message.DestinationServerName;
                package.MessageId = e.Message.MessageId;
                package.RepliedMessageId = e.Message.RepliedMessageId;
                package.SourceApplicationName = e.Message.SourceApplicationName;
                package.SourceCommunicatorId = e.Message.SourceCommunicatorId;
                package.SourceServerName = e.Message.SourceServerName;
                #endregion
                #region 根据数据调整界面组态数中的节点状态
                //为了通用性，在开始判断的时候不要将字节数组转换成字符串进行比较，方式出现错误
                //根据MAC地址判断是否采集站上线
                if (package.ClientType == ScadaClientType.IoMonitor)//保证只获取的是监视器的远程终端信息
                {
                    IO_SERVER server = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID.Trim() == package.ServerId);
                    if (server != null && OnRefreshMainTreeServerStatus != null)
                    {
                        OnRefreshMainTreeServerStatus(server, true, package.ServerId);
                    }
                }
                #endregion
                #region  
                if (package.IsInvalid)//标识该包是有效包
                {
                    switch (package.MDSCommandType)
                    {
                        //采集站，监视器，组态统一登录
                        case MDSCommandType.登录:
                            if (UserLogin != null)
                            {
                                UserLogin(package);
                            }

                            break;
                        case MDSCommandType.更新采集站报警反馈:
                            if (UpdateMonitorAlarmResult != null)
                            {
                                UpdateMonitorAlarmResult(package);
                            }

                            break;
                        case MDSCommandType.实时值:
                            if (ReceiveRealData != null)
                            {
                                ReceiveRealData(package);
                            }
                            break;
                        case MDSCommandType.报警值:
                            {
                                if (ReceiveAlarmData != null)
                                {
                                    ReceiveAlarmData(package);
                                }

                            }
                            break;
                        case MDSCommandType.系统事件:
                            {
                                if (ReceiveEventData != null)
                                {
                                    ReceiveEventData(package);
                                }

                            }
                            break;
                        case MDSCommandType.发布工程请求:
                            if (ApplyPublishProject != null)
                            {
                                ApplyPublishProject(package);
                            }

                            break;
                        case MDSCommandType.上传数据:
                            if (PublishingProject != null)
                            {
                                PublishingProject(package);
                            }
                            break;
                        case MDSCommandType.上传数据失败:
                            if (PublishedProjectFail != null)
                            {
                                PublishedProjectFail(package);
                            }

                            break;
                        case MDSCommandType.上传数据成功:
                            if (PublishedProjectSuccess != null)
                            {
                                PublishedProjectSuccess(package);

                            }
                            break;
                        case MDSCommandType.上传数据开始:
                            if (StartPublishProject != null)
                            {
                                StartPublishProject(package);
                            }
                            break;
                        case MDSCommandType.流程发布请求:
                            if (ApplyPublishFlow != null)
                            {
                                ApplyPublishFlow(package);
                            }

                            break;
                        case MDSCommandType.流程发布准备:
                            if (StartPublishFlow != null)
                            {
                                StartPublishFlow(package);
                            }

                            break;
                        case MDSCommandType.流程发布成功:
                            if (PublishedFlowSuccess != null)
                            {
                                PublishedFlowSuccess(package);
                            }

                            break;
                        case MDSCommandType.流程发布失败:
                            if (PublishedFlowFailt != null)
                            {
                                PublishedFlowFailt(package);
                            }
                            break;
                        case MDSCommandType.流程发布数据:
                            if (PublishingFlow != null)
                            {
                                PublishingFlow(package);
                            }
                            break;
                        case MDSCommandType.下置命令:
                            {
                                var reMessage = e.Message.CreateResponseMessage();
                                if (SendCommand != null)
                                {
                                    SendCommand(package, reMessage);
                                }
                            }

                            break;
                        case MDSCommandType.通道设备状态:
                            {
                                var reMessage = e.Message.CreateResponseMessage();
                                if (ReceiveScadaStatusData != null)
                                {
                                    ReceiveScadaStatusData(package);
                                }
                            }

                            break;
                            
                        default:
                            break;
                    }
                }
                #endregion
                ///确认消息已经收到
            }
            catch
            {

            }
            finally
            {
               
            }

        }
        /// <summary>
        /// 向中心服务器发送数据
        /// </summary>
        /// <param name="datas"></param>
        public bool Send(byte[] datas, string serverid, string DestinationApplicationName, MDSCommandType commandType, ScadaClientType clientType, MessageTransmitRules rules = MessageTransmitRules.NonPersistent, int timeoutMilliseconds = 2000)
        {
            if (datas == null || datas.Length <= 0)
            {
                return false;
            }

            if (MDSClient != null)
            {
                var message = MDSClient.CreateMessage();
                ArraySegment<byte> asdatas = new ArraySegment<byte>(datas);
                message.MessageData = SplitPackage.AssembleBytes(asdatas, serverid, commandType, clientType);
                message.DestinationApplicationName = DestinationApplicationName;
                message.TransmitRule = rules;
                try
                {
                    //如果正常发送数据成功，则不会出现异常报错,否则会出现异常报错
                    message.Send(timeoutMilliseconds);
                    return true;
                }
                catch
                {
                    return false;
                }


            }
            return false;
        }
        public bool Send(byte[] datas,   string DestinationApplicationName, MessageTransmitRules rules = MessageTransmitRules.NonPersistent, int timeoutMilliseconds = 2000)
        {
            if (datas == null || datas.Length <= 0)
            {
                return false;
            }

            if (MDSClient != null)
            {
                var message = MDSClient.CreateMessage();
                message.MessageData = datas;
                message.DestinationApplicationName = DestinationApplicationName;
                message.TransmitRule = rules;
                try
                {
                    //如果正常发送数据成功，则不会出现异常报错,否则会出现异常报错
                    message.Send(timeoutMilliseconds);
                    return true;
                }
                catch
                {
                    return false;
                }


            }
            return false;
        }
        /// <summary>
        /// 发送一个数据并及时返回响应结果
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="serverid"></param>
        /// <param name="DestinationApplicationName"></param>
        /// <param name="commandType"></param>
        /// <param name="clientType"></param>
        /// <param name="rules"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <returns></returns>
        public IIncomingMessage SendAndGetResponse(byte[] datas, string serverid, string DestinationApplicationName, MDSCommandType commandType, ScadaClientType clientType, MessageTransmitRules rules = MessageTransmitRules.NonPersistent, int timeoutMilliseconds = 2000)
        {
            if (datas == null || datas.Length <= 0)
            {
                return null;
            }

            if (MDSClient != null)
            {
                var message = MDSClient.CreateMessage();
                ArraySegment<byte> asdatas = new ArraySegment<byte>(datas);
                message.MessageData = SplitPackage.AssembleBytes(asdatas, serverid, commandType, clientType);
                message.DestinationApplicationName = DestinationApplicationName;
                message.TransmitRule = rules;
                try
                {
                    //如果正常发送数据成功，则不会出现异常报错,否则会出现异常报错
                    var incomMessage = message.SendAndGetResponse(timeoutMilliseconds);
                    incomMessage.Acknowledge();//确认消息
                    return incomMessage;
                }
                catch
                {
                    return null;
                }


            }
            return null;
        }
        public void Disconnect()
        {
            if (MDSClient != null)
            {
                MDSClient.Disconnect();

            }
            if (RealCache != null)
                RealCache.Dispose();
        }

        public void Dispose()
        {
            if (MDSClient != null)
            {
                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;
            }

            if (RealCache != null)
            {
                RealCache.Dispose();
                RealCache = null;
            }

        }
    }
}
