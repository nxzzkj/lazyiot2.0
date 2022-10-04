

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using Scada.DBUtility;
using Scada.Kernel;
using Scada.MDSCore;
using Scada.MDSCore.Client;
using Scada.MDSCore.Communication.Messages;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOMonitor.Core
{
    /// <summary>
    /// 定义一个应用程序客户都通信
    /// </summary>
    public class IOMonitorClient : IDisposable
    {
        public MDSClient MDSClient { set; get; }
        public string ApplicationName { set; get; }
        public string ServerID { set; get; }
        public MDSConfig MDSServerConfig = null;
        /// <summary>
        /// 异常报错返回
        /// </summary>
        public Action<Exception> DisplayException;
        /// <summary>
        /// 正常的日志输出
        /// </summary>
        public Action<string> NormalWriteLog;
        /// <summary>
        /// 用户登录
        /// </summary>
        public Action<bool,string> OnUserLogined;
        /// <summary>
        /// 接收到服务器端更新采集站报警配置的信息
        /// </summary>
          Func<SplitPackageMessage, Task> ReceiveUpdateAlarm;
        /// <summary>
        /// 处理用户登录反馈
        /// </summary>
        Func<SplitPackageMessage, Task> UserLoginFeedback;
        /// <summary>
        /// 接收到服务器端下置命令的信息
        /// </summary>

          Func<SplitPackageMessage, MessageReceivedEventArgs, Task> ReceiveSendCommand;
        public Action<string> ClientDisConnect;
        /// <summary>
        /// 客户端连接到服务器的事件
        /// </summary>
        public Action<string> ClientConnected;
        Scada.Business.IO_ALARM_CONFIG alarmconfigBll = new Scada.Business.IO_ALARM_CONFIG();

        public bool ClientConnectedStatus = false;
         

        public IOMonitorClient(string appName, string ip, int port)
        {
            ApplicationName = appName;

            if (MDSClient != null)
            {
                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;
            }
           
            MDSClient = new MDSClient(ApplicationName, ip, port);
            MDSClient.DataMessageTimeout = IOMonitorManager.MonitorConfig.DataMessageTimeout;
        
            MDSClient.MessageReceived += MDSClient_MessageReceived;
            MDSClient.ClientDisConnect += (MDSClient client) =>
            {

                if (ClientDisConnect != null)
                    ClientDisConnect("采集站工程管理器与服务器连接断开了");

                ClientConnectedStatus = false;

            };
            MDSClient.ClientConnect += (MDSClient client) =>
            {
                ClientConnectedStatus = true;

                if (ClientConnected != null)
                    ClientConnected("采集站连接服务器成功");

                IOMonitorManager.SendScadaEvent(ScadaEvent.采集站启动, new ScadaEventModel() {  Content= "采集站连接服务器成功" });

            };
            //处理登录反馈
            UserLoginFeedback = (SplitPackageMessage pack) =>
            {
                var task =  TaskHelper.Factory.StartNew(() =>
                {
                    try
                    {
                        #region 处理采集器端登录
                        TcpData tcpData = new TcpData();

                        tcpData.BytesToTcpItem(pack.Datas);
                        if (tcpData == null)
                        {
                            if (OnUserLogined != null)
                            {
                                OnUserLogined(false, "登录失败!");
                            }
                            return;
                        }
                        STATION_TCP_INFO loginInfo = new STATION_TCP_INFO();
                        try
                        {
                            loginInfo.IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                            loginInfo.IO_SERVER_IP = tcpData.GetItemValue("IO_SERVER_IP");
                            loginInfo.PASSWROD = tcpData.GetItemValue("PASSWROD");
                            loginInfo.USER = tcpData.GetItemValue("USER");
                            loginInfo.RESULT = tcpData.GetItemValue("RESULT");
                            loginInfo.MSG = tcpData.GetItemValue("MSG");
                            loginInfo.FUNCTION = tcpData.GetItemValue("FUNCTION");
                            //判断是否已经存在有发布的采集站工程

                            if (loginInfo.RESULT == "true")
                            {
                                if (OnUserLogined != null)
                                {
                                    OnUserLogined(true, loginInfo.MSG);
                                }
                           
                            }
                            else
                            {

                                if (OnUserLogined != null)
                                {
                                    OnUserLogined(false, loginInfo.MSG);
                                }
                           
                            }
                        }
                        catch
                        {
                            if (OnUserLogined != null)
                            {
                                OnUserLogined(false, "登录失败");
                            }
                          
                            return;
                        }

                        tcpData.Dispose();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        if (DisplayException != null)
                            DisplayException(ex);
                     
                    }
                });
                return task;
            };
            ReceiveUpdateAlarm = (SplitPackageMessage pack) =>
            {
                var task =  TaskHelper.Factory.StartNew(() =>
                {
                    #region 更新采集站报警
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
                        alarmConfig.UPDATE_DATE = "";
                        alarmConfig.UPDATE_RESULT = "";
                        alarmConfig.UPDATE_UID = "";
                    }
                    catch (Exception ex)
                    {
                        DisplayException(new Exception("ERR70034" + ex.Message));
                        alarmConfig = null;
                    }

                    if (alarmConfig != null)
                    {
                        if (alarmconfigBll.Update(alarmConfig))
                        {

                            //如果采集站更新成功
                            this.Send(tcpData.Datas, MDSServerConfig.CenterAppName, MDSCommandType.更新采集站报警反馈);
                            IO_DEVICE device = IOMonitorProjectManager.IODevices.Find(x => x.IO_DEVICE_ID == alarmConfig.IO_DEVICE_ID);
                            if (device != null)
                            {
                                IO_PARA para = device.IOParas.Find(x => x.IO_ID == alarmConfig.IO_ID);
                                if (para != null)
                                {
                                    para.AlarmConfig = alarmConfig;
                                    AddLogToMainLog("管理员更新" + device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]//" + para.IO_NAME + "[" + para.IO_LABEL + "]" + "报警配置成功! ");
                                }
                            }
                        }
                        else
                        {
                            AddLogToMainLog("管理员更新" + alarmConfig.IO_ID + "报警配置失败! ");
                        }

                    }



                    tcpData.Dispose();
                    #endregion
                });
                return task;
            };
            //接收到用户设置的下置命令的参数

            ReceiveSendCommand = (SplitPackageMessage pack, MessageReceivedEventArgs e) =>
            {
                var task =  TaskHelper.Factory.StartNew(() =>
                {
                    try
                    {
                        //接收到用户下置命令数据
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);
                        IO_COMMANDS command = new IO_COMMANDS()
                        {
                            COMMAND_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            COMMAND_ID = tcpData.GetItemValue("COMMAND_ID"),
                            COMMAND_RESULT = tcpData.GetItemValue("COMMAND_RESULT"),
                            COMMAND_USER = tcpData.GetItemValue("COMMAND_USER"),
                            COMMAND_VALUE = tcpData.GetItemValue("COMMAND_VALUE"),
                            IO_COMM_ID = tcpData.GetItemValue("IO_COMM_ID"),
                            IO_DEVICE_ID = tcpData.GetItemValue("IO_DEVICE_ID"),
                            IO_ID = tcpData.GetItemValue("IO_ID"),
                            IO_LABEL = tcpData.GetItemValue("IO_LABEL"),
                            IO_NAME = tcpData.GetItemValue("IO_NAME"),
                            IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID")
                        };
                        command.COMMAND_RESULT = "FALSE";

                        Scada.Model.IO_COMMUNICATION _COMMUNICATION = IOMonitorProjectManager.IOCommunications.Find(x => x.IO_COMM_ID == command.IO_COMM_ID && command.IO_SERVER_ID == x.IO_SERVER_ID);
                        Scada.Model.IO_DEVICE device = IOMonitorProjectManager.IODevices.Find(x => x.IO_COMM_ID == command.IO_COMM_ID && command.IO_SERVER_ID == x.IO_SERVER_ID && x.IO_DEVICE_ID == command.IO_DEVICE_ID);

                        if (_COMMUNICATION != null && device != null)
                        {
                            Scada.Model.IO_PARA para = device.IOParas.Find(x => x.IO_COMM_ID == command.IO_COMM_ID && command.IO_SERVER_ID == x.IO_SERVER_ID && x.IO_DEVICE_ID == command.IO_DEVICE_ID && x.IO_ID == command.IO_ID);
                            if (para == null)
                            {
                                AddLogToMainLog(device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "] 设备下参数 " + para.IO_ID + " " + para.IO_LABEL + " " + para.IO_NAME + " 参数不存在");
                                return;
                            }
                            if (_COMMUNICATION.DriverInfo == null)
                            {
                                AddLogToMainLog("请在采集站中设置该通讯通道驱动!");
                                IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel() { Content = "请在采集站中设置该通讯通道驱动" });
                                return;
                            }
                            try
                            {
                                if (_COMMUNICATION.CommunicateDriver == null)
                                {
                                    AddLogToMainLog("请在采集站中设置该通讯通道驱动!");
                                    IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel() { Content = "请在采集站中设置该通讯通道驱动" });
                                    return;
                                }
                                else
                                    ((ScadaCommunicateKernel)_COMMUNICATION.CommunicateDriver).IsCreateControl = false;

                                ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)_COMMUNICATION.CommunicateDriver;
                                driverDll.IsCreateControl = false;
                                driverDll.SetUIParameter(_COMMUNICATION.IO_COMM_PARASTRING);
                           
                               // driverDll.InitKernel(MonitorDataBaseModel.IOServer, _COMMUNICATION, _COMMUNICATION.Devices, _COMMUNICATION.DriverInfo);
                                try
                                {
                                    ScadaResult res = driverDll.SendCommand(IOMonitorProjectManager.IOServer, _COMMUNICATION, device, para, command.COMMAND_VALUE);

                                    if (res.Result)
                                    {
                                        command.COMMAND_RESULT = "TRUE";
                                        string msg = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "] 设备下参数 " + para.IO_ID + " " + para.IO_LABEL + " " + para.IO_NAME + " 已经下置成功,下置值" + command.COMMAND_VALUE;
                                        AddLogToMainLog(msg);
                                        IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令成功, new ScadaEventModel() { Content = msg });
                                    }
                                    else
                                    {
                                        command.COMMAND_RESULT = "FALSE";
                                        string msg = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "] 设备下参数 " + para.IO_ID + " " + para.IO_LABEL + " " + para.IO_NAME + " 已经下置失败,下置值" + command.COMMAND_VALUE;
                                        AddLogToMainLog(msg);
                                        IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel() { Content = msg });
                                    }
                                    //创建一个返回消息
                                    var resPack = e.Message.CreateResponseMessage();
                                    byte[] resbytes = SplitPackage.AssembleBytes(Encoding.UTF8.GetBytes(command.GetCommandString()), ServerID, MDSCommandType.下置反馈, ScadaClientType.IoMonitor);
                                    //向web工程端口发送下置命令确认消息
                                    if(pack.ClientType== ScadaClientType.IoServer)
                                    {
                                        resPack.DestinationApplicationName = MDSServerConfig.CenterAppName;
                                    }
                                    else
                                    {
                                        resPack.DestinationApplicationName = MDSServerConfig.WebAppPrefix;
                                    }
                               
                                    resPack.TransmitRule = MessageTransmitRules.StoreAndForward;
                                    resPack.MessageData = resbytes;
                                    resPack.Send();
                                    //将日志写入监视器的命令监视内
                                    IOMonitorUIManager.AppendSendCommand(IOMonitorProjectManager.IOServer, _COMMUNICATION, device, para, command);
                                }
                                catch (Exception ex)
                                {
                                    IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel() { Content = ex.Message });
                                    ThrowExceptionToMain(new Exception("ERROR600002" + ex.Message));
                                }
                            }
                            catch (Exception ex)
                            {
                                IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel() { Content = ex.Message });
                                ThrowExceptionToMain(new Exception("ERROR600001" + ex.Message));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        IOMonitorManager.SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel() { Content = ex.Message });
                        ThrowExceptionToMain(new Exception("ERR10030" + ex.Message));
                    }
                });
                return task;
            };
        }
        public void Connect()
        {
            try
            {
                MDSClient.Connect();
            }
            catch
            {

            }
       
        }
        /// <summary>
        /// 处理服务器端接收到的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MDSClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ClientConnectedStatus = true;
            e.Message.Acknowledge();
            SplitPackageMessage package = null;
            try
            {
                ///获取接收的数据包
                byte[] datas = e.Message.MessageData;
                package= SplitPackage.RemoveIdentificationBytes(datas);
                //解析的数据无效或者不符合要求，则不进行处理
                if (!package.IsInvalid || package.Length <= 0 || package.ServerId == "")
                {

                    return;
                }
            }
            catch
            {

            }
            if (package == null)
                return;
       
            #region 
            package.DestinationApplicationName = e.Message.DestinationApplicationName;
            package.DestinationCommunicatorId = e.Message.DestinationCommunicatorId;
            package.DestinationServerName = e.Message.DestinationServerName;
            package.MessageId = e.Message.MessageId;
            package.RepliedMessageId = e.Message.RepliedMessageId;
            package.SourceApplicationName = e.Message.SourceApplicationName;
            package.SourceCommunicatorId = e.Message.SourceCommunicatorId;
            package.SourceServerName = e.Message.SourceServerName;
            #endregion
            if (package.IsInvalid)//标识该包是有效包,并处理接收到的数据
            {
                switch (package.MDSCommandType)
                {
                    case MDSCommandType.登录反馈:
                        {
                 
                            if (UserLoginFeedback != null)
                            {
                                UserLoginFeedback(package);
                                
                            }
                       
                        }
                        break;
                    case MDSCommandType.更新采集站报警:
                        {
                       
                            if (ReceiveUpdateAlarm != null)
                            {
                                ReceiveUpdateAlarm(package);
                            }
                         
                        }
                        break;
                    case MDSCommandType.下置命令:
                        {
                            if (ReceiveSendCommand != null)
                                ReceiveSendCommand(package,e);
                        }
                        break;
                  
                }
            }
            
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="DestinationApplicationName"></param>
        /// <param name="commandType"></param>
        /// <param name="rules"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <returns></returns>
        public bool Send(byte[] datas, string DestinationApplicationName, MDSCommandType commandType, MessageTransmitRules rules = MessageTransmitRules.NonPersistent )
        {
            int timeoutMilliseconds = IOMonitorManager.MonitorConfig.SendBlockTime;
            if (MDSClient != null)
            {
                var message = MDSClient.CreateMessage();

                ArraySegment<byte> asdatas = new ArraySegment<byte>(datas);


                message.MessageData = SplitPackage.AssembleBytes(asdatas, ServerID, commandType, ScadaClientType.IoMonitor);
                message.DestinationApplicationName = DestinationApplicationName;
                message.TransmitRule = rules;
                try
                {
                    //如果正常发送数据成功，则不会出现异常报错,否则会出现异常报错
                    message.Send(timeoutMilliseconds);
                    return true;
                }
                catch (Exception emx)
                {

                    return false;
                }


            }
            return false;
        }
        public bool Send(byte[] datas, MDSCommandType commandType, MessageTransmitRules rules = MessageTransmitRules.NonPersistent)
        {
            int timeoutMilliseconds = IOMonitorManager.MonitorConfig.SendBlockTime;
            if (MDSClient != null)
            {
                var message = MDSClient.CreateMessage();

                ArraySegment<byte> asdatas = new ArraySegment<byte>(datas);


                message.MessageData = SplitPackage.AssembleBytes(asdatas, ServerID, commandType, ScadaClientType.IoMonitor);
                message.DestinationApplicationName = MDSServerConfig.CenterAppName;
                message.TransmitRule = rules;
                try
                {
                    
                    //等待10毫秒
                    //如果正常发送数据成功，则不会出现异常报错,否则会出现异常报错
                    if (message.MessageData!=null && message.MessageData.Length>0)
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
        public IOutgoingMessage CreateMessage()
        {
            return MDSClient.CreateMessage();
        }
        public void Disconnect()
        {
            if (MDSClient != null)
            {
                MDSClient.Disconnect();
            
            }
        }
        private void AddLogToMainLog(string msg)
        {
            if(NormalWriteLog != null)
            {
                NormalWriteLog(msg);
            }
        }
        private void ThrowExceptionToMain(Exception emx)
        {
            if(DisplayException!=null)
            {
                DisplayException(emx);
            }
        }
        public void Dispose()
        {
            if (MDSClient != null)
            {
                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;
            }
            alarmconfigBll = null;
            MDSServerConfig = null;
        }


    }
}
