using Scada.Kernel;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scada.Model;
using MQTTnet.Client;
using System.Security.Authentication;

namespace MQTTnet
{
    public class MQTTTimer
    {
        public System.Threading.Timer Timer = null;
        public string ClientID = "";
        public  bool IsStop
        {
            get;
            set;
        }
        public void Close()
        {
            ClientID = "";
            if (Timer != null)
            {


                Timer.Dispose();
                Timer = null;
            }
        }
        public void Stop()
        {
            IsStop = true;
        }
        public void Continue()
        {
            IsStop = false;
        }
        
    }
    /// <summary>
    /// MQTT协议接口的实现
    /// </summary>
    public class MQTTServer : ScadaCommunicateKernel
    {
        private int ConnunicationTimeout = 60;
        private string UserName = "";
        private string Password = "";
        private MqttQualityOfServiceLevel MessageQulity =  MqttQualityOfServiceLevel.AtMostOnce;
        private string WillFlag = "";
        private string MqttDataType = "";
        /// <summary>
        /// Mqtt服务器的IP
        /// </summary>
        private string ServerIP = "127.0.0.1";
        private bool EaableAnonymousAuthentication = false;
 
        private string Certificate = "";
        private string MqttCommunicatonModel = "TCP";
        private SslProtocols SslProtocol = SslProtocols.None;
        public int mMaxPendingMessagesPerClient = 100;
        public bool mEnableKeepSessions = true;
        public MqttPendingMessagesOverflowStrategy mMqttPendingMessagesOverflowStrategy = MqttPendingMessagesOverflowStrategy.DropOldestQueuedMessage;






        List<MQTTTimer> MqttClientTimes = new List<MQTTTimer>();
        private Timer simulatorSendTime = null;


        /// <summary>
        /// MQtt服务器的端口号
        /// </summary>
        public int ServerPort = 1883;
        private const string mGuid = "CEA4530C-05DB-42FE-A8DC-A04EEEC79AF0";
        /// <summary>
        /// 驱动唯一标识，采用系统GUID分配
        /// </summary>
        public override string GUID
        {
            get
            {
                return mGuid;
            }


        }
        private string mTitle = "MQTT物联网通讯";
        public override string Title
        {
            get
            {
                return mTitle;
            }

            set
            {
                mTitle = value;
            }
        }
        /// <summary>
        /// 初始化驱动
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="ioDevices"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        protected override bool InitCommunicateKernel(IO_SERVER server, IO_COMMUNICATION communication, List<IO_DEVICE> ioDevices, SCADA_DRIVER driver)
        {
            try
            { 
                ParaPack communicatePack = new ParaPack(communication.IO_COMM_PARASTRING);
                if (communication.IO_COMM_PARASTRING != null && communication.IO_COMM_PARASTRING != "")
                {
                    this.ServerIP = communicatePack.GetValue("服务器IP");
                    this.ServerPort = int.Parse(communicatePack.GetValue("端口号"));
                    this.UserName = communicatePack.GetValue("用户名");
                    this.Password = communicatePack.GetValue("密码");
                    this.EaableAnonymousAuthentication = int.Parse(communicatePack.GetValue("开启匿名验证")) == 0 ? false : true;
                    this.Certificate = communicatePack.GetValue("SSL证书");
                    string msgqulity = communicatePack.GetValue("消息质量");
                    switch (msgqulity)
                    {
                        case "QoS 0 最多分发一次":
                            MessageQulity = MqttQualityOfServiceLevel.AtMostOnce;
                            break;
                        case "QoS 1 至少分发一次":
                            MessageQulity = MqttQualityOfServiceLevel.AtLeastOnce;
                            break;
                        case "QoS 2 只分发一次":
                            MessageQulity = MqttQualityOfServiceLevel.ExactlyOnce;
                            break;
                    }
                    this.WillFlag = communicatePack.GetValue("遗愿标志");
                    this.MqttDataType = communicatePack.GetValue("数据格式");
                   
                    this.ConnunicationTimeout = string.IsNullOrEmpty(communicatePack.GetValue("连接超时")) ? 60 : int.Parse(communicatePack.GetValue("连接超时"));

                    this.mEnableKeepSessions = communicatePack.GetValue("启用持久会话") == "1" ? true : false;
                    this.mMaxPendingMessagesPerClient = string.IsNullOrEmpty(communicatePack.GetValue("最大挂起消息数")) ? 100 : int.Parse(communicatePack.GetValue("最大挂起消息数"));
                    this.MqttCommunicatonModel = communicatePack.GetValue("通讯模式") == "" ? "TCP" : communicatePack.GetValue("通讯模式");
                    this.mMqttPendingMessagesOverflowStrategy = string.IsNullOrEmpty(communicatePack.GetValue("溢出策略")) ? MqttPendingMessagesOverflowStrategy.DropOldestQueuedMessage
                        : (MqttPendingMessagesOverflowStrategy)Enum.Parse(typeof(MqttPendingMessagesOverflowStrategy), communicatePack.GetValue("溢出策略"));
                    this.SslProtocol = string.IsNullOrEmpty(communicatePack.GetValue("SSL证书")) ? SslProtocols.None
                        : (SslProtocols)Enum.Parse(typeof(SslProtocols), communicatePack.GetValue("SSL证书"));
                    this.mEnableKeepSessions = communicatePack.GetValue("启用持久会话") == "1" ? true : false;

                }

                if (IsCreateControl)
                {
                    CommunicationControl = new MQTTServerCtrl();
                    if (communication != null && communication.IO_COMM_PARASTRING != "")
                        CommunicationControl.SetUIParameter(communication.IO_COMM_PARASTRING);
                }
            }
            catch (Exception emx)
            {
                this.CommunicationException(emx.Message);
                return false;
            }
            return true;

        }
        #region 发送命令部分

        protected override ScadaResult IOSendCommand(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value)
        {
            try
            {
                ParaPack deviceparaPack = new ParaPack(device.IO_DEVICE_PARASTRING);
                ParaPack paraPack = new ParaPack(para.IO_PARASTRING);
                ///向指定客户端的指定设备发送下置命令的数据
                Task.Run(async () =>
                {




                    string cmdJson = "{\"uid\":\"" + device.IO_DEVICE_ADDRESS + "\",\"paraname\":\"" + para.IO_NAME + "\",\"value\":\"" + value + "\",\"jsonname\":\"" + paraPack.GetValue("IO标识") + "\",\"defaultvalue\":\"" + paraPack.GetValue("命令默认值") + "\"}";
                    string topic = deviceparaPack.GetValue("下置命令主题").Trim();    // 将字符串转换为字符数组

                    MqttApplicationMessage mqttApplicationMessage = new MqttApplicationMessage()
                    {
                        Topic = topic,
                        QualityOfServiceLevel = MessageQulity,
                        Retain = true,
                        Payload = Scada.Kernel.ScadaHexByteOperator.StringToAsciiByte(cmdJson)

                    };
                    await this._mqttServer.PublishAsync(mqttApplicationMessage);

                    mqttApplicationMessage.Dispose();
                    mqttApplicationMessage = null;
                });
                return new ScadaResult();
            }
            catch (Exception emx)
            {
                return new ScadaResult(false, emx.Message);
            }
        }


        #endregion
        #region 模拟器部分

//        可以使用 `var options = new MqttClientOptions();` 直接构建一个options。你也可以通过参数构建器 `var options = new MqttClientOptionsBuilder();` 使代码更简洁美观。

//构建器的函数说明：

//函数名 功能说明
//Build 构建配置参数
//WithAuthentication 允许使用不同的身份验证模式
//WithCleanSession 将客户端与MQTT干净会话支持一起使用
//WithClientId 设置客户端ID
//WithCommunicationTimeout 设置通信超时
//WithCredentials 设置登录凭证
//WithExtendedAuthenticationExchangeHandler 以自定义方式处理身份验证
//WithKeepAlivePeriod 设置保持有效期
//WithKeepAliveSendInterval 设置保活的发送间隔
//WithMaximumPacketSize 设置最大数据包大小
//WithNoKeepAlive 不要使用保持活动状态
//WithProtocolVersion 设置MQTT协议版本
//WithProxy 设置代理
//WithTls 客户端使用SSL/TLS
//WithTopicAliasMaximum   允许最大数量的主题别名
//WithReceiveMaximum  允许最大数量的已接收数据包
//WithRequestProblemInformation   显示请求问题信息
//WithRequestResponseInformation  显示请求响应问题信息
//WithSessionExpiryInterval   一段时间后终止会话
//WithTcpServer   告诉客户端（通过TCP）连接到哪个MQTT代理。
//WithWebSocketServer 告诉客户端（通过WebSocket）连接到哪个MQTT代理
//WithWillMessage 告诉客户端最后一条消息将被发送。
//WithWillDelayInterval 告诉客户端最后一个消息得延迟间隔

        /// <summary>
        /// 模拟器部分
        /// </summary>
        /// <param name="times"></param>
        /// <param name="IsSystem"></param>
        public override void Simulator(IO_COMMUNICATION communicate)
        {
     
            SimulatorControl = new MQTTSimulatorCtrl();
            SimulatorCommunication = communicate;
            MqttClients = new List<IMqttClient>();
            MqttOptions = new List<MqttClientOptions>();

        }

        protected override Task SimulatorClose()
        {
            return Task.Run(() =>
            {
                try
                {
                    SimulatorStatus = false;
                    if (simulatorSendTime != null)
                        simulatorSendTime.Dispose();
                    simulatorSendTime = null;

                    if (MqttClients == null)
                        return;
                    for (int i = 0; i < MqttClients.Count; i++)
                    {
                        if (MqttClients.Count > 0)
                        {
                            try
                            {
                                MqttClients[i].DisconnectAsync();

                                MqttClients[i].Dispose();
                                MqttClients[i] = null;
                                
                            }
                            catch
                            {
                                continue;
                            }
                        }

                    }
                    MqttClients.Clear();
                    MqttClients = null;
                    if (MqttOptions != null)
                        MqttOptions.Clear();
                    MqttOptions = null;
                    MqttClients = new List<IMqttClient>();
                    MqttOptions = new List<MqttClientOptions>();
                }
                catch
                {

                }
               
            });

        }
        private Random random = new Random();
        public SimulatorMqttJson MqttJson = null;
        public List<IMqttClient> MqttClients { set; get; }
        private MqttQualityOfServiceLevel MqttQualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce;

        List<MqttClientOptions> MqttOptions { set; get; }

        protected override Task SimulatorStart()
        {
            return Task.Run(() =>
            {
                SimulatorStatus = false;
                if (string.IsNullOrEmpty(SimulatorCommunication.IO_COMM_SIMULATOR_PARASTRING))
                {
                    SimulatorAppendLog("没有配置相关MQTT服务器参数");
                    return;
                }


                ParaPack paraPack = new ParaPack(SimulatorCommunication.IO_COMM_SIMULATOR_PARASTRING);
                try
                {
                    if (paraPack.Count > 0)
                    {
                        MqttJson = new SimulatorMqttJson()
                        {
                            ServerIP = paraPack.GetValue("MQTT服务器"),
                            HeartPeried = paraPack.GetValue("保持有效期"),
                            PassiveActive = paraPack.GetValue("被动上传") == "1" ? true : false,
                            Password = paraPack.GetValue("密码"),
                            Port = paraPack.GetValue("端口号"),
                            User = paraPack.GetValue("用户名")


                        };
                        foreach (IO_DEVICE device in SimulatorCommunication.Devices)
                        {
                            if (!device.SimulatorCheckedStatus)
                                continue;
                            SimulatorMqttJsonObject deviceJson = new SimulatorMqttJsonObject();
                            ParaPack devicePack = new ParaPack(device.IO_DEVICE_SIMULATOR_PARASTRING);
                            deviceJson.device.ClientID = devicePack.GetValue("MQTT客户端ID");
                            deviceJson.device.name = device.IO_DEVICE_NAME;
                            deviceJson.device.uid = device.IO_DEVICE_ADDRESS;
                            deviceJson.device.UpdateCycle = device.IO_DEVICE_UPDATECYCLE;//保存的是毫秒数据
                            deviceJson.device.DataTopic = devicePack.GetValue("数据订阅主题");
                            deviceJson.device.CommandTopic = devicePack.GetValue("下置命令主题");
                            deviceJson.device.UpdateCycleTopic = devicePack.GetValue("循环周期主题");
                            foreach (IO_PARA para in device.IOParas)
                            {
                                SimulatorMqttJsonPara paraJson = new SimulatorMqttJsonPara();
                                paraJson.name = para.IO_NAME;
                                paraJson.SimulatorMax = para.IO_SIMULATOR_MAX;
                                paraJson.SimulatorMin = para.IO_SIMULATOR_MIN;
                                deviceJson.paras.Add(paraJson);
                            }

                            MqttJson.Devices.Add(deviceJson);
                        }

                    }
                }
                catch (Exception emx)
                {
                    SimulatorAppendLog(emx.Message);
                    MqttJson = null;
                }
                if (MqttJson == null)
                {
                    SimulatorAppendLog("没有配置相关MQTT服务器参数");
                    return;

                }
                SimulatorStatus = true;
                CreateSimulatorMQTTClient(paraPack).Wait();
                Task.Run(() =>
                {
                    try
                    {

                        for (int i = 0; i < MqttClients.Count; i++)
                        {
                            try
                            {

                                MqttClients[i].ConnectAsync(MqttOptions[i]);
                                SimulatorAppendLog(MqttJson.Devices[i].Name + " 创建mqtt客户端成功");

                            }
                            catch
                            {

                                continue;
                            }


                        }

                    }
                    catch (Exception emx)
                    {
                        SimulatorAppendLog(emx.Message);
                    }
                }).Wait();
                simulatorSendTime = new Timer(new TimerCallback(delegate
                {
                    if (!SimulatorStatus)
                        return;
                    DateTime current = DateTime.Now;
                    int num = MqttClients.Count / 10000;

                    if (MqttClients.Count % 10000 != 0)
                        num++;
                    for (int i = 0; i < num; i++)
                    {
                        int iindex = i;
                        Task.Run(() =>
                        {
                            for (int j = 0; j < MqttClients.Count; j++)
                            {
                                int index = iindex * 10000 + j;
                                if (index < MqttClients.Count)
                                {
                                    IMqttClient mqttClient = MqttClients[index];
                                    var device = MqttJson.Devices[index];
                                    if (mqttClient != null && mqttClient.IsConnected)
                                    {
                                        if (current > device.LastTime.AddMilliseconds(device.UpdateCycle))
                                        {
                                            Task.Run(() =>
                                            {
                                                SimulatorPublicRealDataJson(mqttClient, device);
                                            });
                                        }
                                    }
                                }



                            }
                        });
                    }
                    //创建定时数据发送任务
                    Thread.Sleep(1000);

                }), null, 60000, 1000);
            });
           

        }
        private Task CreateSimulatorMQTTClient(ParaPack paraPack)
        {
           return  Task.Run(() => { 
            try
            {
                
              
                MqttClients = new List<IMqttClient>();
                MqttOptions = new List<MqttClientOptions>();
                for (int i = 0; i < MqttJson.Devices.Count; i++)
                {


                    try
                    {
                        string cleintID = MqttJson.Devices[i].device.ClientID;
                        MqttClientOptions option = new MqttClientOptions() { ClientId = cleintID };
                        option.ChannelOptions = new MqttClientTcpOptions()
                        {
                            Server = MqttJson.ServerIP,
                            Port = Convert.ToInt32(MqttJson.Port),
                             BufferSize=1024
                        };
                        
                        option.Credentials = new MqttClientCredentials()
                        {
                            Username = MqttJson.User,
                            Password = MqttJson.Password
                             
                        };
                      
                        option.CleanSession = false;
                          
                        option.KeepAlivePeriod = TimeSpan.FromMinutes(30000);//保持有效期
                        option.KeepAliveSendInterval = TimeSpan.FromMinutes(30000);//保持发送间隔
                        MqttOptions.Add(option);

                        IMqttClient MqttClient = new MqttFactory().CreateMqttClient();
                        ///接收到数据

                        MqttClient.ApplicationMessageReceived += (sender, args) =>
                        {

                            if (args.ClientId == null || args.ClientId == "")
                                return;
                            if (args.ApplicationMessage.Payload == null || args.ApplicationMessage.Payload.Length <= 0)
                                return;
                            IMqttClient mqttClient = (IMqttClient)sender;
                            Task.Run(() =>
                            {
                                SimulatorMqttJsonObject device = MqttJson.Devices.Find(x => x.ClientID == args.ClientId);
                                if (device != null && args.ApplicationMessage.Topic.Trim() == device.device.UpdateCycleTopic)//用户上位机读取数据的间隔,是秒,如果是客户端是被动，则服务器端是主动，此处则要求发布一次数据
                                {

                                    ///获取当前的json字符串
                                    string json = args.ApplicationMessage.ConvertPayloadToString();
                                    //将json对象转换为c#对象
                                    MQTTPassiveSubTopicObject subTopicObject = ScadaHexByteOperator.JsonToObject<MQTTPassiveSubTopicObject>(json);

                                    if (subTopicObject != null && device != null && device.UID.Trim() == subTopicObject.uid.Trim())
                                    {
                                        SimulatorPublicRealDataJson(mqttClient, device);

                                    }
                                    json = "";
                                    subTopicObject.Dispose();
                                }
                                else if (device != null && args.ApplicationMessage.Topic.Trim() == device.device.CommandTopic)//用户上位机下置数据
                                {

                                    SimulatorAppendLog(device.device.name+"向服务器端发送一条数据");
                                }

                            });


                        };
                       
                        MqttClient.Connected += (sender, args) =>
                        {
                            IMqttClient mqttClient = (IMqttClient)sender;
                            SimulatorMqttJsonObject device = MqttJson.Devices.Find(x => x.ClientID == mqttClient.Options.ClientId);

                            Task.Run(() => {
                              

                             
                                try
                                {
                                    if (device != null)
                                    {
                                        MqttClient.UnsubscribeAsync(device.device.CommandTopic);
                                        MqttClient.SubscribeAsync(device.device.CommandTopic, MqttQualityOfServiceLevel);//服务器端下置命令的主题

                                        if (MqttJson.PassiveActive)//被动提交数据
                                        {
                                            //订阅时间更新主题，这个主题是服务器端定时刷新通知客户端发布一次数据
                                            MqttClient.SubscribeAsync(device.device.UpdateCycleTopic, MqttQualityOfServiceLevel);//服务器端设置了更新数据周期后通知到客户端


                                        }
                                    }
                                }
                                catch
                                {

                                }
                         
                            SimulatorAppendLog(device.device.name + "链接上服务器成功");
                            });


                        };
                        MqttClient.Disconnected +=   (sender, args) =>
                        {
                            SimulatorAppendLog("客户端与服务器断开链接" + (args.Exception != null ? args.Exception.Message : ",尝试重新链接"));
                            if(SimulatorStatus)
                            {
                             
                                MqttClient.ConnectAsync(option);
                                Thread.Sleep(30000);
                            }
                         



                        };                     
                        MqttClients.Add(MqttClient);
                       


                    }
                    catch (Exception emx)
                    {
                        SimulatorAppendLog("创建MQTT模拟失败!   " + emx.Message);
                        continue;
                    }
                     
                }
            }
            catch (Exception emx)
            {
                SimulatorAppendLog(emx.Message);
            }
            });
        }
        private void SimulatorPublicRealDataJson(IMqttClient MqttClient, SimulatorMqttJsonObject device)
        {
       
            string clientid = MqttClient.Options.ClientId;
             
            if (clientid == null || clientid == ""|| device.device==null)
                return;
            device.LastTime = DateTime.Now;
            try
            {
                if (MqttClient != null && MqttClient.IsConnected)
                {
                
                       //构造一个对象
                       Random random = new Random();

                    for (int p = 0; p < device.paras.Count; p++)
                    {
                        int v = random.Next(device.paras[p].SimulatorMin, device.paras[p].SimulatorMax);
                        string dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        device.paras[p].data.Clear();
                        device.paras[p].data.Add(dateString);//日期
                        device.paras[p].data.Add(v.ToString());//值
                        device.paras[p].data.Add("无");//单位

                    }
                   
                    random = null;
                    string json = ScadaHexByteOperator.ObjectToJson(device);
                    //发布订阅的数据
                    MqttApplicationMessage mqttApplicationMessage = new MqttApplicationMessage()
                    {
                        Payload = Encoding.UTF8.GetBytes(json),
                        QualityOfServiceLevel = MqttQualityOfServiceLevel,
                        Retain = false,
                        Topic = device.device.DataTopic
                        

                    };
                    MqttClient.PublishAsync(mqttApplicationMessage);
                    json = "";
                    SimulatorAppendLog(clientid +"客户端 " +device.Name + " 模拟一条数据");
                }

            }
            catch (Exception emx)
            {
                SimulatorAppendLog(emx.Message);
                return;
            }

        }
        #endregion
        #region 服务器管理部分
        IMqttServer _mqttServer = null;
//        可以使用 `var options = new MqttServerOptions();` 直接构建一个options。你也可以通过参数构建器 `var options = new MqttServerOptionsBuilder();` 使代码更简洁美观。

//构建器的函数说明：

//函数名 功能说明
//Build 构建配置参数
//WithApplicationMessageInterceptor 允许处理来自客户端的所有已发布消息
//WithClientId 服务端发布消息时使用的ClientId
//WithConnectionBacklog 设置要保留的连接数
//WithConnectionValidator 验证连接
//WithDefaultCommunicationTimeout 设置默认的通信超时
//WithDefaultEndpoint 使用默认端点
//WithDefaultEndpointBoundIPAddress 使用默认端点IPv4地址
//WithDefaultEndpointBoundIPV6Address 使用默认端点IPv6地址
//WithDefaultEndpointPort 使用默认端点端口
//WithEncryptedEndpoint 使用加密的端点
//WithEncryptedEndpointBoundIPAddress 使用加密的端点IPv4地址
//WithEncryptedEndpointBoundIPV6Address 使用加密的端点IPv6地址
//WithEncryptedEndpointPort 使用加密的端点端口
//WithEncryptionCertificate 使用证书进行SSL连接
//WithEncryptionSslProtocol 使用SSL协议级别
//WithMaxPendingMessagesPerClient 每个客户端允许最多未决消息
//WithPersistentSessions 保持会话
//WithStorage 使用存储
//WithSubscriptionInterceptor 允许处理来自客户端的所有订阅
//WithoutDefaultEndpoint 禁用默认端点
//WithoutEncryptedEndpoint 禁用默认(SSL)端点

        protected override Task Start()
        {
            return Task.Run(() =>
            {
                if (null != _mqttServer)
                {
                    return;
                }
                ///相关参数不能为空
                if (this.IOServer == null || this.IOCommunication == null)
                    return;

                var optionBuilder =
                    new MqttServerOptionsBuilder();

         
                if(this.MqttCommunicatonModel=="TCP")
                {
                    optionBuilder.WithDefaultEndpoint();
                    optionBuilder.WithDefaultEndpointPort(ServerPort);
                    optionBuilder.WithoutEncryptedEndpoint();
                    if (!String.IsNullOrEmpty(ServerIP))
                    {
                        optionBuilder.WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ServerIP));
                    }
                }
               
                else
                {
                    optionBuilder.WithEncryptedEndpoint();
                    optionBuilder.WithEncryptedEndpointPort(ServerPort);
                    optionBuilder.WithoutDefaultEndpoint();
                    if (!String.IsNullOrEmpty(ServerIP))
                    {
                        optionBuilder.WithEncryptedEndpointBoundIPAddress(IPAddress.Parse(ServerIP));
                    }

                }
                optionBuilder.WithConnectionBacklog(this.mMaxPendingMessagesPerClient);
                optionBuilder.WithDefaultCommunicationTimeout(TimeSpan.FromSeconds( this.ConnunicationTimeout));
                if(!string.IsNullOrEmpty(this.Certificate))
                optionBuilder.WithEncryptionCertificate(Encoding.UTF8.GetBytes(this.Certificate));

                optionBuilder.WithEncryptionSslProtocol(this.SslProtocol);
                if(this.mEnableKeepSessions)
                optionBuilder.WithPersistentSessions();
             


               var options = optionBuilder.Build();
                //连接验证
                (options as MqttServerOptions).ConnectionValidator += context =>
                {
                    string clientId = "";
                    Task.Run(() =>
                    {
                        try
                        {
                            ParaPack paraPack = new ParaPack(this.IOCommunication.IO_COMM_PARASTRING);

                            if (context.ClientId.Length < 2)
                            {
                                context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;
                                return;
                            }
                      
                            if (EaableAnonymousAuthentication)
                            {
                                if (!context.Username.Equals(this.UserName))
                                {
                                    context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                                    return;
                                }
                                if (!context.Password.Equals(this.Password))
                                {
                                    context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                                    return;
                                }
                            }
                            bool isValidCleint = false;

                            for (int i = 0; i < IODevices.Count; i++)
                            {

                                ParaPack deviceparaPack = new ParaPack(IODevices[i].IO_DEVICE_PARASTRING);
                                clientId = deviceparaPack.GetValue("MQTT连接ID号").Trim();    // 将字符串转换为字符数组
                                if (clientId.Trim() == context.ClientId.Trim())
                                {
                                    isValidCleint = true;
                                    IODevices[i].Tag = clientId.Trim();//标记对应的客户端ID号

                                }

                            }
                            if (isValidCleint)
                                context.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
                            else
                                context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;
                        }
                        catch
                        {
                            context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;
                        }
                        if (string.IsNullOrEmpty(clientId))
                        {
                            context.ReturnCode = MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;
                        }
                    });
                };


                _mqttServer = new MqttFactory().CreateMqttServer();


                //开始连接
                _mqttServer.ClientConnected += (sender, args) =>
                {

                    if (args.ClientId == null || args.ClientId == "")
                        return;


                    IO_DEVICE device = this.IODevices.Find(x => x.Tag.ToString().Trim() == args.ClientId.Trim());
                    if (device == null)
                        return;


                    Task.Run(() =>
                    {

                        ParaPack commPack = new ParaPack(this.IOCommunication.IO_COMM_PARASTRING);
                        ParaPack devicePack = new ParaPack(device.IO_DEVICE_PARASTRING);
                        #region 通用MQTT解析
                        {

                            //客户端连接上后发布订阅数据
                            List<TopicFilter> topicFilters = new List<TopicFilter>();

                            this.DeviceOnline(this.IOServer, this.IOCommunication, device, null);
                            this.SetOneDeviceStatus(this.IOServer, IOCommunication, device);
                            string clientId = devicePack.GetValue("MQTT连接ID号").Trim();    // 将字符串转换为字符数组
                            device.Tag = clientId;
                            if (clientId.Trim() == args.ClientId.Trim())
                            {
                                //订阅数据

                                TopicFilter topicFilter = new TopicFilter(devicePack.GetValue("数据订阅主题").Trim(), MessageQulity);
                                topicFilters.Add(topicFilter);
                                try
                                {
                                _mqttServer.SubscribeAsync(args.ClientId.Trim(), topicFilters);
                                }
                                catch (Exception)
                                {
                                    this.CommunicationException("ERROR=MQTTNet_20006,发布订阅主题失败 ");
                                }

                            }
                            else
                            {
                                this.CommunicationException("ERROR=MQTTNet_20006,MQTT ID与设备配置ID不匹配 ");
                            }
                            //定时向客户端发布一个读取数据的订阅
                            if (commPack.GetValue("接收方式") == "主动")
                            {
                                lock (MqttClientTimes)
                                {


                                    MQTTTimer cleintMqtt = new MQTTTimer()
                                    {
                                        ClientID = args.ClientId
                                    };
                                    MqttClientTimes.Add(cleintMqtt);

                                    cleintMqtt.Timer = new Timer(delegate
                                    {
                                        if (!cleintMqtt.IsStop)
                                        {
                                            try
                                            {

                                                _mqttServer.PublishAsync(
                                              new MqttApplicationMessage()
                                              {
                                                  QualityOfServiceLevel = MessageQulity,
                                                  Retain = false,
                                                  Topic = devicePack.GetValue("循环周期主题").Trim(),
                                                  Payload = Encoding.UTF8.GetBytes("{\"uid\":\"" + devicePack.GetValue("设备ID编码").Trim() + "\",\"updatecycle\":\"" + device.IO_DEVICE_UPDATECYCLE + "\",\"topic\":\"" + devicePack.GetValue("数据订阅主题").Trim() + "\"}")

                                              }

                                              );



                                            }
                                            catch (Exception)
                                            {
                                                this.CommunicationException("ERROR=MQTTNet_20006,发布订阅主题失败 ");
                                            }
                                            this.SimulatorAppendLog(device.IO_DEVICE_NAME + "服务器端更新周期" + device.IO_DEVICE_UPDATECYCLE + "毫秒");
                                        }

                                    }, args, 1000, device.IO_DEVICE_UPDATECYCLE);
                                }


                            }

                        }
                        #endregion
                    });
                };
                ///断开连接
                _mqttServer.ClientDisconnected += (sender, args) =>
                {
                    Task.Run(() =>
                    {

                        IO_DEVICE device = this.IODevices.Find(x => x.Tag.ToString().Trim() == args.ClientId.Trim());
                        if (device != null)
                        {

                            this.DeviceOffline(this.IOServer, IOCommunication, device, null);
                            this.SetOneDeviceStatus(this.IOServer, IOCommunication, device);
                        }

                    });

                };
                ///接收到订阅主题的数据数据

                _mqttServer.ApplicationMessageReceived += (sender, args) =>
                {
                    Task.Run(() =>
                    {
                      
                        if (args.ClientId == null || args.ClientId.Trim() == "")
                            return;

                        if (args.ApplicationMessage.Payload == null || args.ApplicationMessage.Payload.Length <= 0)
                        {
                            this.CommunicationException("接收的数据为空");

                            return;
                        }
                        ParaPack commPack = new ParaPack(this.IOCommunication.IO_COMM_PARASTRING);

                        try
                        {


                            string cleintId = args.ClientId.Trim();
                            //将接收到的数据发送到实际的对应的解析数据库中
                            string strs = ScadaHexByteOperator.UTF8ByteToString(args.ApplicationMessage.Payload);
                            CommonMqttJsonObject mqttJsonObject = ScadaHexByteOperator.JsonToObject<CommonMqttJsonObject>(strs);

                            if (mqttJsonObject == null)
                                return;
                            IO_DEVICE device    = this.IODevices.Find(x => x.IO_DEVICE_ADDRESS == mqttJsonObject.device.uid.Trim()&& (new ParaPack(x.IO_DEVICE_PARASTRING)).GetValue("MQTT连接ID号").Trim() == cleintId.Trim());
                            if (device != null)
                            {
                                #region
                              
                                if (args.ApplicationMessage.Topic.Trim() == (new ParaPack(device.IO_DEVICE_PARASTRING)).GetValue("数据订阅主题").Trim())
                                {
                                    ScadaDeviceKernel scadaDeviceKernel = device.DeviceDrive as ScadaDeviceKernel;
                                    device.IOParas.ForEach(delegate (IO_PARA para) {
                                        para.IORealData=scadaDeviceKernel.AnalysisData(this.IOServer, IOCommunication, device, para, args.ApplicationMessage.Payload,DateTime.Now, mqttJsonObject);

                                    });
                                   
                                    this.ReceiveData(this.IOServer, IOCommunication, device, args.ApplicationMessage.Payload, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), mqttJsonObject);
                                }
                                #endregion
                            }


                        }
                        catch
                        {
                            return;
                        }

                    });
                };
                ///订阅主题

                _mqttServer.ClientSubscribedTopic += (sender, args) =>
                {


                };
                ///取消订阅主题
                _mqttServer.ClientUnsubscribedTopic += (sender, args) =>
                {

                };

                _mqttServer.Started += (sender, args) =>
                {
                    base.CommunicationOnline();
                };

                _mqttServer.Stopped += (sender, args) =>
                {

                    base.CommunicationOffline();

                };

                _mqttServer.StartAsync(options);

                this.CommunctionStartChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "启动服务");
            });
        }

        protected override Task Close()
        {
            return Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < MqttClientTimes.Count; i++)
                    {
                        MqttClientTimes[i].Close();
                    }
                    MqttClientTimes.Clear();
                    _mqttServer.StopAsync();
                    _mqttServer = null;
                    this.CommunctionCloseChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "关闭网络服务");
                }
                catch (Exception emx)
                {
                    this.CommunicationException("ERROR=MQTTNet_10006," + emx.Message);
                    this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "关闭网络服务失败");
                }
            });
        }
        protected override void Continue()
        {
            
            try
            {
                for (int i = 0; i < MqttClientTimes.Count; i++)
                {
                    MqttClientTimes[i].Continue();
                }
                this.CommunctionContinueChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "继续网络服务");
            }
            catch (Exception emx)
            {
                this.CommunicationException("ERROR=MQTTNet_10003," + emx.Message);
                this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "继续网络服务失败");
            }
        }
        protected override void Pause()
        {
            try
            {
                for (int i = 0; i < MqttClientTimes.Count; i++)
                {
                    MqttClientTimes[i].Stop();
                }
                this.CommunctionPauseChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "暂停网络服务");
            }
            catch (Exception emx)
            {
                this.CommunicationException("ERROR=MQTTNet_10005," + emx.Message);
                this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "暂停网络服务失败");
            }
        }
        protected override Task Stop()
        {
            return Task.Run(() =>
            {
                try
                {
                    if (MqttClientTimes != null)
                    {
                        for (int i = 0; i < MqttClientTimes.Count; i++)
                        {
                            MqttClientTimes[i].Stop();
                        }
                        MqttClientTimes.Clear();
                    }


                    if (_mqttServer != null)
                        _mqttServer.StopAsync();
                    this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "停止网络服务");


                }
                catch (Exception emx)
                {
                    this.CommunicationException("ERROR=MQTTNet_10003," + emx.Message);
                    this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "停止网络服务失败");
                }
            });


        }
        private MQTTServerCtrl mQTTServerCtrl = null;

        public override CommunicationKernelControl CommunicationControl
        {
            set
            {
                if(mQTTServerCtrl==null)
                {
                    mQTTServerCtrl = (MQTTServerCtrl)value;
                }
              
                mQTTServerCtrl.SetUIParameter(this.IOCommunication.IO_COMM_PARASTRING);
            }
            get { return mQTTServerCtrl; }
        }

        #endregion

        #region 实现创建配置界面
        public override SimulatorDeviceEditControl CreateSimulatorDeviceEdit()
        {
            return new MQTTSimulatorDeviceEditCtrl();
        }
        public override SimulatorKernelControl CreateSimulatorKernelControl()
        {
            return new MQTTSimulatorCtrl();
        }
        public override CommunicationKernelControl CreateCommunicationKernelControl()
        {
            return new MQTTServerCtrl();
        }
        #endregion

    }
}
