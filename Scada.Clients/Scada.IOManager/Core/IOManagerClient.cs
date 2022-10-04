

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 

using Scada.DBUtility;
using Scada.MDSCore;
using Scada.MDSCore.Client;
using Scada.MDSCore.Communication.Messages;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOManager.Core
{
    /// <summary>
    /// 定义一个应用程序客户都通信
    /// </summary>
    public class IOManagerClient : IDisposable
    {

        public MDSClient MDSClient { set; get; }
        public string ApplicationName { set; get; }
        public string ServerID { set; get; }
     
        /// <summary>
        /// 异常报错返回
        /// </summary>
        public Action<Exception> DisplayException;
        /// <summary>
        /// 正常的日志输出
        /// </summary>
        public Action<string> NormalWriteLog;
        public Action<bool, string> OnPublishedResult;
        public Action<bool, string> OnReStartMonitor;
        /// <summary>
        /// 发布下一个数据包
        /// </summary>
        public Action<int, string> NextPublishProjectPack;
        /// <summary>
        /// 开始发布工程
        /// </summary>
        public Action<bool> StartPublishProject;
        public Action<string > ClientDisConnect;
        /// <summary>
        /// 用户登录成功与否的标识
        /// </summary>
        public Func<bool,string,Task> OnUserLogined;

        /// <summary>
        /// 处理用户登录反馈
        /// </summary>
          Func<SplitPackageMessage, Task> OnUserLoginFeedback;
        /// <summary>
        /// 发布工程反馈
        /// </summary>
          Func<SplitPackageMessage, Task> PublishProjectedFeedback;
        /// <summary>
        /// 发布工程成功
        /// </summary>
          Func<SplitPackageMessage, Task> PublishProjectedSuccess;
        /// <summary>
        /// 发布工程失败
        /// </summary>
          Func<SplitPackageMessage, Task> PublishProjectedFail;
        /// <summary>
        /// 发布工程的进度
        /// </summary>
          Func<SplitPackageMessage, Task> PublishProjectedProcess;
        /// <summary>
        /// 发布工程数据传输进度
        /// </summary>
          Func<SplitPackageMessage, Task> PublishProjectedDataTrans;


        public IOManagerClient(string appName, string ip, int port)
        {
            ApplicationName = appName;

            if (MDSClient != null)
            {
                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;
            }

            MDSClient = new MDSClient(ApplicationName, ip, port);

            MDSClient.MessageReceived += MDSClient_MessageReceived;
            MDSClient.ClientDisConnect += (MDSClient client) =>
            {

                if (ClientDisConnect != null)
                    ClientDisConnect("采集站工程管理器与服务器连接断开了");
             
            };
            //处理登录反馈
            OnUserLoginFeedback = (SplitPackageMessage pack) =>
            {
                var task = Task.Factory.StartNew(() =>
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
                        if(DisplayException!=null)
                        DisplayException( ex);
                    }
                });
                return task;
            };
            PublishProjectedFeedback = (SplitPackageMessage pack) =>
            {
                try
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        #region 发布工程反馈
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);
                        if (tcpData == null)
                        {
                            if (OnPublishedResult != null)
                            {
                                OnPublishedResult(false, "发布失败");
                            }
                            return;
                        }

                        try
                        {
                            string IO_SERVER_ID = tcpData.GetItemValue("IO_SERVER_ID");
                            string RESULT = tcpData.GetItemValue("RESULT");
                            string MSG = tcpData.GetItemValue("MSG");
                            //判断是否已经存在有发布的采集站工程

                            if (RESULT == "true")
                            {
                                if (StartPublishProject != null)
                                {
                                    StartPublishProject(true);
                                }
                            }
                            else
                            {

                                if (OnPublishedResult != null)
                                {
                                    OnPublishedResult(false,MSG);
                                }
                            }
                        }
                        catch
                        {
                            if (OnPublishedResult != null)
                            {
                                OnPublishedResult(false, "发布失败");
                            }
                            return;
                        }

                        tcpData.Dispose();
                        #endregion
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    DisplayException(new Exception("ERR70034" + ex.Message));
                    return null;
                }
            };
            PublishProjectedSuccess = (SplitPackageMessage pack) =>
            {
                try
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        NormalWriteLog("发布采集站工程成功,请重新启动采集服务!");
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);
                        if (OnPublishedResult != null)
                        {
                            OnPublishedResult(true, tcpData.GetItemValue("MSG"));
                        }
                        ///发布成功后重新启动采集站监视器
                        if (this.OnReStartMonitor != null)
                        {
                            OnReStartMonitor(true, "发布采集站工程成功,请重新启动采集服务");
                        }
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    DisplayException(new Exception("ERR70034" + ex.Message));
                    return null;
                }
            };
            PublishProjectedFail = (SplitPackageMessage pack) =>
            {
                try
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);
                        if (OnPublishedResult != null)
                        {
                            OnPublishedResult(false, tcpData.GetItemValue("MSG"));
                        }
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    DisplayException(new Exception("ERR70034" + ex.Message));
                    return null;
                }
            };
            PublishProjectedProcess = (SplitPackageMessage pack) =>
            {
                try
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);

                        if (tcpData == null)
                        {
                            return;
                        }
                        //显示服务器端更新进度
                        WriteNormalLog(tcpData.GetItemValue("MSG") + "  " + tcpData.GetItemValue("PROCESS"));

                    });
                    return task;
                }
                catch (Exception ex)
                {
                    DisplayException(new Exception("ERR70034" + ex.Message));
                    return null;
                }
            };
            PublishProjectedDataTrans = (SplitPackageMessage pack) =>
            {
                try
                {
                    var task = Task.Factory.StartNew(() =>
                    {
                        TcpData tcpData = new TcpData();
                        tcpData.BytesToTcpItem(pack.Datas);
                        if (tcpData == null)
                        {
                            return;
                        }
                        int index = int.Parse(tcpData.GetItemValue("PROCESS"));//请求的数据
                                                                               //显示服务器端更新进度
                        WriteNormalLog(tcpData.GetItemValue("MSG") + "  正在发送第" + tcpData.GetItemValue("PROCESS") + "条数据");
                        if (NextPublishProjectPack != null)
                        {
                            NextPublishProjectPack(index, tcpData.GetItemValue("MSG"));
                        }
                    });
                    return task;
                }
                catch (Exception ex)
                {
                    DisplayException(new Exception("ERR70034" + ex.Message));
                    return null;
                }
            };






        }
        public IOutgoingMessage CreateMessage()
        {
            return MDSClient.CreateMessage();
        }
        public void Connect()
        {
            if (MDSClient != null)
            {
                MDSClient.Connect();
            }


        }
        public void WriteNormalLog(string msg)
        {
            if (NormalWriteLog != null)
                NormalWriteLog(msg);
        }
        /// <summary>
        /// 处理服务器端接收到的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MDSClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            e.Message.Acknowledge();
            ///获取接收的数据包
            byte[] datas = e.Message.MessageData;
            SplitPackageMessage package = SplitPackage.RemoveIdentificationBytes(datas);
            //解析的数据无效或者不符合要求，则不进行处理
            if (!package.IsInvalid || package.Length <= 0 || package.ServerId == "")
            {
            
                return;
            }         
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
            if (package.IsInvalid)//标识该包是有效包
            {
                switch (package.MDSCommandType)
                {
                    case MDSCommandType.登录反馈:
                        if (OnUserLoginFeedback != null)
                            OnUserLoginFeedback(package);
                        break;
                    case MDSCommandType.发布工程请求反馈:
                        if (PublishProjectedFeedback != null)
                            PublishProjectedFeedback(package);
                        break;
                    case MDSCommandType.发布工程失败:
                        if (PublishProjectedFail != null)
                            PublishProjectedFail(package);
                        break;
                    case MDSCommandType.发布工程成功:
                        if (PublishProjectedSuccess != null)
                            PublishProjectedSuccess(package);
                        break;
                    case MDSCommandType.发布工程进度:
                        if (PublishProjectedProcess != null)
                            PublishProjectedProcess(package);
                        break;
                    case MDSCommandType.发布工程数据传输进度:
                        if (PublishProjectedDataTrans != null)
                            PublishProjectedDataTrans(package);
                        break;
                }
            }

        }
        public bool Send(byte[] datas,string DestinationApplicationName, MDSCommandType commandType, MessageTransmitRules rules = MessageTransmitRules.NonPersistent, int timeoutMilliseconds = 1000)
        {
            if (MDSClient != null)
            {
                var message = MDSClient.CreateMessage();

                ArraySegment<byte> asdatas = new ArraySegment<byte>(datas);


                message.MessageData = SplitPackage.AssembleBytes(asdatas, ServerID, commandType, ScadaClientType.IoManager);
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
        public void Disconnect()
        {
            if (MDSClient != null)
            {
                MDSClient.Disconnect();

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
        }



    }
}
