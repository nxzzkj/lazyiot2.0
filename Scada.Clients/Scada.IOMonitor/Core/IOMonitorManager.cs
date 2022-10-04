using Scada.DBUtility;
using Scada.IOStructure;
using Scada.Kernel;
using Scada.TriggerAlarm;
 
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Scada.MDSCore;



/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Core
{


    //监视采集主任务
    public abstract class IOMonitorManager
    {
        public static LimitedTaskScheduler TaskScheduler = new LimitedTaskScheduler(50);
        /// <summary>
        /// 是否在后头运行,前台不显示相关信息
        /// </summary>
        public static bool IsBackRun = true;
        private static MDSConfig mDSServerConfig = null;
        public static IOMonitorClient MDSClient = null;
        public static string ServerID = ComputerInfo.GetInstall().ServerID.ToString();//每个采集站ID和主板信息绑定，确保唯一

        /// <summary>
        /// 采集数据缓存
        /// </summary>
        private static IOMonitorCacheManager receiveRealCache = null;
        public static IOMonitorCacheManager RealCache
        {
            get { return receiveRealCache; }
        }


        #region
        public static bool EnableWriterLog
        {
            get { return Scada.Logger.Logger.GetInstance().Enable; }
            set { Scada.Logger.Logger.GetInstance().Enable = value; }
        }
        public static event EventHandler OnConnectedServer;
        public static event EventHandler OnUserLogined;


        //日志定时保存
        private static void MonitorLogRun()
        {


            TaskHelper.Factory.StartNew(() =>
            {
                while (EnableWriterLog)
                {
                    Thread.Sleep(1000 * 120);

                    Scada.Logger.Logger.GetInstance().WriteLog();
                }


            });
        }

        #endregion

        public static event MonitorOperator OnMonitorOperator;
        /// <summary>
        /// 加载系统配置
        /// </summary>
        public static IOMonitorConfig MonitorConfig = null;
        //当前采集站的主要对象
        private static IO_SERVER IOServer = null;
        //创建读取数据的子任务

        private static Scada.Business.SCADA_DRIVER commDriverBll = null;

        /// <summary>
        /// 创建下发数据命令的子任务
        /// </summary>

        private static TaskOperator TaskOperator = TaskOperator.关闭;


        //异常报错事件
        private static event MonitorException OnMonitorException;
        //常规日志处理事件
        private static event MonitorLog OnMonitorLog;

        //接收数据并处理后返回的事件
        private static event MonitorReceive OnMonitorRealData;
        private static event MonitorMakeAlarm OnMonitorAlarm;
        public static event MonitorUploadListView OnMonitorUploadListView;

        public static IOMonitorStatusManager StatusMonitor;
        public static ComputerStatusMonitor ComputerStatusMonitor;


        #region 日志显示的的信息都统一增加日志窗体中去
        private static Task AddLogToMainLog(string log)
        {
            return TaskHelper.Factory.StartNew(() => {
            if (OnMonitorLog != null)
            {
                OnMonitorLog(log);
            }
            Scada.Logger.Logger.GetInstance().Info(log);
            });
        }
        private static Task ThrowExceptionToMain(Exception emx)
        {
            return TaskHelper.Factory.StartNew(() => {
                if (OnMonitorException != null)
            {
                OnMonitorException(emx);
            }
            ///写如到本地日志
            Scada.Logger.Logger.GetInstance().Debug(emx.Message);
            });

        }
        private static Task ThrowExceptionUploadCenter(Exception emx)
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                ThrowExceptionToMain(emx);
                ///上传异常事件到数据中心
                SendScadaEvent(ScadaEvent.采集站异常, new ScadaEventModel()
                {
                    Content = emx.Message.Replace("#", " ").Replace("|", " ").Replace("^", " ")
                });
            });
        }
        private static Task ThrowAnalysisKernelException(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, Exception emx)
        {
            return TaskHelper.Factory.StartNew(() =>
            {
                ThrowExceptionToMain(emx);
                ///上传异常事件到数据中心
                ScadaEventModel eventModel = new ScadaEventModel()
                {
                    SERVER_ID = server.SERVER_ID,
                    Content = emx.Message.Replace("#", " ").Replace("|", " ").Replace("^", " ")

                };
                if (device != null)
                {
                    eventModel.DEVICE_ID = device.IO_DEVICE_ID;
                    eventModel.DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]";

                }

                if (comm != null)
                {
                    eventModel.COMM_ID = comm.IO_COMM_ID;
                    eventModel.COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]";

                }
                if (para != null)
                {
                    eventModel.IO_ID = para.IO_ID;
                    eventModel.IO_LABEL = para.IO_LABEL;
                    eventModel.IO_NAME = para.IO_NAME;
                }
                SendScadaEvent(ScadaEvent.通道驱动异常, eventModel);
            });
        }
        #endregion
        public static bool MDSClientConnectedStatus
        {
            get
            {
                if (MDSClient != null)
                {
                    bool res = MDSClient.ClientConnectedStatus;
                    if (res == false)
                        AddLogToMainLog("采集站与数据中心网络链接断开，正在尝试重新链接！");
                    return res;
                }
                else
                    return false;

            }
        }

        /// <summary>
        /// 由于用户在登录前需要TCP与服务器通讯，所以在登录前要创建改服务
        /// </summary>
        /// <returns></returns>
        public static void InitMDSClient(string remoteIp)
        {
            try
            {
                if (mDSServerConfig == null)
                    mDSServerConfig = new MDSConfig();
                if (MDSClient == null)
                {

                    MDSClient = new IOMonitorClient(mDSServerConfig.MonitorAppPrefix + "_" + IPAddressSelector.Instance().AddressIPNoPoint, remoteIp, int.Parse(mDSServerConfig.MDSServerPort));
                    MDSClient.ServerID = ServerID;
                    MDSClient.MDSServerConfig = mDSServerConfig;
                    MDSClient.OnUserLogined = async (bool res, string msg) =>
                    {
                        //处理用户登录反馈的消息
                        if (OnUserLogined != null)
                        {
                            OnUserLogined(res, null);
                        }

                    };
                    MDSClient.NormalWriteLog = (string msg) =>
                    {
                        AddLogToMainLog(msg);
                    };
                    MDSClient.ClientConnected = (string msg) =>
                    {
                        AddLogToMainLog(msg);
                        SendScadaEvent(ScadaEvent.通道开启, new ScadaEventModel()
                        {
                            Content = "通道开启",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

                        });
                    };

                    MDSClient.DisplayException = (Exception emx) =>
                    {

                        ThrowExceptionToMain(emx);
                        SendScadaEvent(ScadaEvent.采集站异常, new ScadaEventModel()
                        {
                            Content = emx.Message,
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

                        });
                    };
                    MDSClient.ClientDisConnect = (string msg) =>
                    {
                        AddLogToMainLog(msg);

                        SendScadaEvent(ScadaEvent.采集站关闭, new ScadaEventModel()
                        {
                            Content = "采集站关闭",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

                        });
                    };
                    MDSClient.Connect();
                    if (OnConnectedServer != null)
                    {
                        OnConnectedServer(true, null);
                    }
                    AddLogToMainLog("连接数据中心通讯成功!");



                }

            }
            catch (Exception emx)
            {
                AddLogToMainLog("连接数据中心通讯失败!" + emx.Message);
            }

        }
        public static void CloseMDSClient()
        {
            try
            {
                if (mDSServerConfig != null)
                    mDSServerConfig = null;
                if (MDSClient != null)
                {
                    MDSClient.Disconnect();
                    MDSClient.Dispose();
                }
                MDSClient = null;
            }
            catch
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scadaEvent"></param>
        /// <param name="eventModel"></param>

        public static void SendScadaEvent(ScadaEvent scadaEvent, ScadaEventModel eventModel)
        {
            if (receiveRealCache == null)
                return;
            eventModel.SERVER_ID = ServerID;
            receiveRealCache.Push(new EventCacheObject() { DataString = IOMonitorRealDataUploadDBUtility.GetScadaEventCacheStriung(scadaEvent, eventModel) });


        }

        public static void InitMonitor()
        {

            OnMonitorRealData += IOMonitorManager_OnMonitorRealData;
            OnMonitorAlarm += IOMonitorManager_OnMonitorAlarm;
            OnMonitorException += IOMonitorManager_OnMonitorException;
            OnMonitorLog += IOMonitorManager_OnMonitorLog;
            OnMonitorUploadListView += IOMonitorManager_OnMonitorUploadListView;
            commDriverBll = new Scada.Business.SCADA_DRIVER();
            //开启日志保存服务
            MonitorLogRun();
            int num = IOMonitorProjectManager.ProgressMaxNum + 4;
            //创建垃圾定时回收
            MemoryCollect.Monitor(TaskHelper.Factory);


            #region 读取当前采集站工程数据
            TaskOperator = TaskOperator.关闭;
            IOServer = IOMonitorProjectManager.IOServer;
            //创建驱动模块
            for (int i = 0; i < IOMonitorProjectManager.IOCommunications.Count; i++)
            {
                if (IOMonitorProjectManager.IOCommunications[i].Devices == null || IOMonitorProjectManager.IOCommunications[i].Devices.Count <= 0)
                    continue;

                if (IOMonitorProjectManager.IOCommunications[i].DriverInfo == null)
                {
                    AddLogToMainLog("创建通道" + IOMonitorProjectManager.IOCommunications[i].IO_COMM_NAME.ToString() + @"[" + IOMonitorProjectManager.IOCommunications[i].IO_COMM_LABEL + @"]驱动失败,请在采集站中设置该通讯通道驱动!");
                    continue;
                }
                try
                {

                    if (IOMonitorProjectManager.IOCommunications[i].CommunicateDriver == null)
                        continue;
                    else
                        ((ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver).IsCreateControl = false;
                    AddLogToMainLog("创建通道" + IOMonitorProjectManager.IOCommunications[i].IO_COMM_NAME.ToString() + @"[" + IOMonitorProjectManager.IOCommunications[i].IO_COMM_LABEL + @"]驱动成功!");

                    ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver;
                    driverDll.IsCreateControl = false;
                    driverDll.SetUIParameter(IOMonitorProjectManager.IOCommunications[i].IO_COMM_PARASTRING);

                    driverDll.InitKernel(IOMonitorProjectManager.IOServer, IOMonitorProjectManager.IOCommunications[i], IOMonitorProjectManager.IOCommunications[i].Devices, IOMonitorProjectManager.IOCommunications[i].DriverInfo);

                    driverDll.CommunctionClose += CDriverDll_CommunctionClose;
                    driverDll.CommunctionContinue += CDriverDll_CommunctionContinue;
                    driverDll.CommunctionPause += CDriverDll_CommunctionPause;
                    driverDll.CommunctionStart += CDriverDll_CommunctionStart;
                    driverDll.CommunctionStop += CDriverDll_CommunctionStop;
                    driverDll.DeviceSended += CDriverDll_DeviceSended;
                    driverDll.DeviceStatusChanged += CDriverDll_DeviceStatusChanged;
                    driverDll.CommunicationStatusChanged += DriverDll_CommunicationStatusChanged;
                    driverDll.OnKernelException += CDriverDll_Communication_Exception;
                    driverDll.KernelDataReceived = (IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, byte[] receivedatas, string date, object sender) =>
                    {
                        //处理接收的数据


                        return DataReceived(server, comm, device, receivedatas, date, sender);


                    };
                    driverDll.OnKernelLog += CDriverDll_KernelLog;

                    AddLogToMainLog("准备创建该通道下的设备驱动.....");
                    for (int d = 0; d < IOMonitorProjectManager.IOCommunications[i].Devices.Count; d++)
                    {
                        Scada.Model.IO_DEVICE device = IOMonitorProjectManager.IOCommunications[i].Devices[d];
                        if (device.IOParas == null || device.IOParas.Count <= 0)
                            continue;
                        try
                        {
                            if (IOMonitorProjectManager.IOCommunications[i].Devices[d].DriverInfo == null)
                            {
                                AddLogToMainLog("创建设备" + device.IO_DEVICE_LABLE.ToString() + @"[" + device.IO_DEVICE_NAME + @"]驱动失败,请在采集站中设置该设备驱动!");
                                continue;
                            }
                            ((ScadaDeviceKernel)device.DeviceDrive).IsCreateControl = false;
                            ((ScadaDeviceKernel)device.DeviceDrive).Exception += CDriverDll_Device_Exception;
                            ((ScadaDeviceKernel)device.DeviceDrive).InitKernel(IOMonitorProjectManager.IOServer, IOMonitorProjectManager.IOCommunications[i], device, null, device.DriverInfo);
                        }
                        catch (Exception ex)
                        {
                            ThrowAnalysisKernelException(IOServer, IOMonitorProjectManager.IOCommunications[i], device, null, new Exception("创建设备" + IOMonitorProjectManager.IOCommunications[i].Devices[d].IO_DEVICE_LABLE.ToString() + @"[" + IOMonitorProjectManager.IOCommunications[i].Devices[d].IO_DEVICE_NAME + @"]驱动失败,!错误原因:" + ex.Message));

                        }
                    }

                }
                catch (Exception ex)
                {
                    ThrowAnalysisKernelException(IOServer, IOMonitorProjectManager.IOCommunications[i], null, null, new Exception("创建通道" + IOMonitorProjectManager.IOCommunications[i].IO_COMM_NAME.ToString() + @"[" + IOMonitorProjectManager.IOCommunications[i].IO_COMM_LABEL + @"]驱动失败,!错误原因:" + ex.Message));

                }

            }
            #endregion

            receiveRealCache = new IOMonitorCacheManager(IOMonitorManager.MonitorConfig.CacheInterval, IOMonitorManager.MonitorConfig.CacheMaxNumber);
            //批量上传实时数据
            receiveRealCache.WillUpload = (List<ReceiveCacheObject> result) =>
            {
                try
                {
                    //定时从缓存区上传数据
                    var analysisTask = TaskHelper.Factory.StartNew(() =>
                    {


                        IOMonitorRealDataUploadDBUtility.UploadReal(result);
                        Thread.CurrentThread.Priority = ThreadPriority.Highest;

                    });
                    return analysisTask;



                }
                catch
                {
                    return null;
                }
            };
            ///批量上传报警数据
            receiveRealCache.WillUploadAlarm = (List<AlarmCacheObject> result) =>
            {
                try
                {
                    //定时从缓存区上传数据
                    var analysisTask = TaskHelper.Factory.StartNew(() =>
                    {
                        IOMonitorRealDataUploadDBUtility.UploadAlarm(result);
                        Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
                    });
                    return analysisTask;



                }
                catch
                {
                    return null;
                }
            };
            //批量上传事件数据
            receiveRealCache.WillUploadEvent = (List<EventCacheObject> result) =>
            {
                try
                {
                    //定时从缓存区上传数据
                    var analysisTask = TaskHelper.Factory.StartNew(() =>
                    {
                        IOMonitorRealDataUploadDBUtility.UploadEvent(result);
                        Thread.CurrentThread.Priority = ThreadPriority.Normal;
                    });
                    return analysisTask;



                }
                catch
                {
                    return null;
                }
            };

            //批量上传通讯和设备状态
            receiveRealCache.WillUploadScadaStatus = (List<ScadaStatusCacheObject> result) =>
            {
                try
                {
                    //定时从缓存区上传数据
                    var analysisTask = TaskHelper.Factory.StartNew(() =>
                    {
                        IOMonitorRealDataUploadDBUtility.UploadScadaStatus(result);
                        Thread.CurrentThread.Priority = ThreadPriority.Normal;
                    });
                    return analysisTask;



                }
                catch
                {
                    return null;
                }
            };
            receiveRealCache.CacheInformation = (string msg) =>
            {
                var analysisTask = TaskHelper.Factory.StartNew(async () =>
                {

                    // AddLogToMainLog(msg);

                });
                return analysisTask;
            };
            receiveRealCache.Read();

            StatusMonitor = new IOMonitorStatusManager();
            StatusMonitor.Monitor = () =>
            {


                return TaskHelper.Factory.StartNew(() =>
                {
                    try
                    {
                        IOMonitorProjectManager.IOCommunications.ForEach(delegate (IO_COMMUNICATION comm)
                        {
                            ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)comm.CommunicateDriver;
                            if (comm.CommunicateDriver != null)
                            {
                                driverDll.CheckDeviceStatus();
                            }


                            IOMonitorManager.RealCache.Push(new ScadaStatusCacheObject()
                            {
                                COMM_ID = comm.IO_COMM_ID,
                                DEVICE_ID = "",
                                SERVER_ID = comm.IO_SERVER_ID,
                                ScadaStatus = comm.IO_COMM_STATUS == 1 ? ScadaStatus.True : ScadaStatus.False,
                                StatusElemnt = ScadaStatusElemnt.Communication

                            });
                            comm.Devices.ForEach(delegate (IO_DEVICE device)
                            {


                                IOMonitorManager.RealCache.Push(new ScadaStatusCacheObject()
                                {

                                    COMM_ID = device.IO_COMM_ID,
                                    DEVICE_ID = device.IO_DEVICE_ID,
                                    SERVER_ID = device.IO_SERVER_ID,
                                    ScadaStatus = device.IO_DEVICE_STATUS == 1 ? ScadaStatus.True : ScadaStatus.False,
                                    StatusElemnt = ScadaStatusElemnt.Device


                                });
                            });

                        });
                    }
                    catch
                    {

                    }
                });
            };

            ComputerStatusMonitor = new ComputerStatusMonitor();
            //电脑处于锁定状态
            ComputerStatusMonitor.ComputerLocked = () =>
            {
                IsBackRun = true;
            };
            ComputerStatusMonitor.ComputerUnLocked = () =>
            {

            };
            Start();
            StatusMonitor.Start();
            ComputerStatusMonitor.Start();
            TaskHelper.TaskRunException = (string exception) =>
            {

                AddLogToMainLog(exception);
            };
            //设置系统环境变量 
            ScadaEnvironment.SetVariable(AppDomain.CurrentDomain.BaseDirectory, "LAZY_MONITOR_PATH");
            ScadaFilePermission.SetDBFilePermission(IOMonitorProjectManager.DataBaseFileName.Split('=')[1]);

        }

        private static void DriverDll_CommunicationStatusChanged(IO_SERVER server, IO_COMMUNICATION comm, object tag)
        {
            if (tag != null)
            {
                if (tag.ToString() == "1" || tag.ToString().ToLower() == "true")
                {
                    AddLogToMainLog("通道" + comm.IO_COMM_NAME + " 启动 ");
                    //设备上线不需要通知，如果下线，则在系统日志中进行显示
                    comm.IO_COMM_STATUS = 1;
                    IOMonitorUIManager.RefreshCommunicationTreeNodeStatus(comm);
                    SendScadaEvent(ScadaEvent.通道开启, new ScadaEventModel()
                    {
                        COMM_ID = comm.IO_COMM_ID,
                        COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                        SERVER_ID = server.SERVER_ID,


                    });



                }
                else
                {
                    AddLogToMainLog("通道" + comm.IO_COMM_NAME + " 关闭 ");
                    comm.IO_COMM_STATUS = 0;
                    IOMonitorUIManager.RefreshCommunicationTreeNodeStatus(comm);
                    SendScadaEvent(ScadaEvent.通道关闭, new ScadaEventModel()
                    {
                        COMM_ID = comm.IO_COMM_ID,
                        COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                        SERVER_ID = server.SERVER_ID,


                    });

                }

            }
        }

        private static void IOMonitorManager_OnMonitorUploadListView(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, string uploadresult)
        {
            IOMonitorUIManager.ShowMonitorUploadListView(server, communication, device, uploadresult);
        }

        private static void IOMonitorManager_OnMonitorLog(string log)
        {
            IOMonitorUIManager.AppendLogItem(log);
        }

        private static void IOMonitorManager_OnMonitorException(Exception ex)
        {
            IOMonitorUIManager.AppendLogItem(ex.Message);
            SendScadaEvent(ScadaEvent.采集站异常, new ScadaEventModel()
            {
                Content = ex.Message.Replace("#", " ").Replace("|", " ").Replace("^", " ")
            });
        }

        private static void IOMonitorManager_OnMonitorAlarm(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARAALARM arlarm)
        {

            IOMonitorUIManager.MonitorIODataAlarmShowView(server, comm, device, arlarm, "上传成功");

        }

        private static void IOMonitorManager_OnMonitorRealData(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, byte[] sourceBytes)
        {

            IOMonitorUIManager.MonitorIODataShowView(server, comm, device);

        }

        private static void StationMDSServer_CenterServerLog(string log)
        {


            AddLogToMainLog(log);

        }

        private static void StationMDSServer_CenterServerException(Exception ex)
        {

            ThrowExceptionToMain(ex);
            SendScadaEvent(ScadaEvent.采集站异常, new ScadaEventModel()
            {
                Content = ex.Message.Replace("#", " ").Replace("|", " ").Replace("^", " ")
            });


        }




        #region 设备驱动返回的异常信息
        private static void CDriverDll_Device_Exception(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, string msg)
        {
            if (comm == null || device == null)
                return;
            ThrowExceptionToMain(new Exception(ScadaEvent.设备驱动异常 + " " + msg));
            SendScadaEvent(ScadaEvent.设备驱动异常, new ScadaEventModel()
            {
                COMM_ID = comm.IO_COMM_ID,
                COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                SERVER_ID = server.SERVER_ID,
                DEVICE_ID = device.IO_DEVICE_ID,
                DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]",
                Content = msg

            });
            msg = "";
        }
        #endregion
        #region 通讯驱动返回的各种数据事件 
        /// <summary>
        /// 返回要在日志窗体中显示的数据
        /// </summary>
        /// <param name="msg"></param>
        private static void CDriverDll_KernelLog(string msg)
        {


            AddLogToMainLog(msg);
            msg = "";


        }

        /// <summary>
        /// 返回服务器端接收的数据,此处主要将一个设备下的所有IO表数据统一获取后在一次性上传,
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para">为null,此参数不传递,根据用户驱动需要</param>
        /// <param name="receivedatas">接收的全部数据，要求在驱动中进行一次读取后返回</param>

        private static Task DataReceived(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, byte[] receivedatas, string date, object sender)
        {

            return TaskHelper.Factory.StartNew(() =>
            {
                //解析数据
                if (device != null && device.DeviceDrive != null)
                {

                    //清理已经接收完成的数据
                    for (int i = 0; i < device.IOParas.Count; i++)
                    {
                        device.IOParas[i].IORealData = null;
                    }
                    device.GetedValueDate = DateTime.Now;
                    device.ReceiveBytes = receivedatas;
                    #region 循环解析实时数据接收的每个参数

                    ScadaDeviceKernel Driver = (ScadaDeviceKernel)device.DeviceDrive;
                    for (int i = 0; i < device.IOParas.Count; i++)
                    {
                        #region 解析开关量 模拟量 字符常量 数据
                        if (device.IOParas[i].IO_POINTTYPE == "模拟量" || device.IOParas[i].IO_POINTTYPE == "开关量" || device.IOParas[i].IO_POINTTYPE == "字符串量")
                        {

                            Driver.InitKernel(server, comm, device, device.IOParas[i], device.DriverInfo);
                            IOData recdata = Driver.AnalysisData(server, comm, device, device.IOParas[i], receivedatas, Convert.ToDateTime(date), sender);
                            if (recdata != null)
                            {
                                device.IOParas[i].IORealData = recdata;
                            }
                        }

                        #endregion
                        #region 解析关系数据库值
                        try
                        {
                            if (device.IOParas[i].IO_POINTTYPE == "关系数据库值" && device.IOParas[i].IO_DATASOURCE.Trim() != "")
                            {

                                IOMonitorRelationalDatabase rlation = new IOMonitorRelationalDatabase(device.IOParas[i].IO_DATASOURCE);
                                string sql = rlation.GetSql();
                                string conn = rlation.ConnectString;
                                switch (rlation.Database_Type)
                                {
                                    case "SQL Server":
                                        {
                                            try
                                            {
                                                if (conn != "" && sql != "")
                                                {
                                                    DbHelperSQL sqlHealp = new DbHelperSQL();
                                                    sqlHealp.connectionString = conn;
                                                    DataSet ds = sqlHealp.Query(sql);
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        device.IOParas[i].IORealData = new IOData();
                                                        device.IOParas[i].IORealData.CommunicationID = device.IO_COMM_ID;
                                                        device.IOParas[i].IORealData.ServerID = device.IO_SERVER_ID;
                                                        device.IOParas[i].IORealData.ID = device.IO_DEVICE_ID;
                                                        device.IOParas[i].IORealData.ParaName = device.IOParas[i].IO_NAME;
                                                        device.IOParas[i].IORealData.ParaString = device.IOParas[i].IO_PARASTRING;
                                                        device.IOParas[i].IORealData.ParaValue = ds.Tables[0].Rows[0]["value"].ToString();
                                                        device.IOParas[i].IORealData.QualityStamp = QualityStamp.GOOD;
                                                        device.IOParas[i].IORealData.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["datetime"].ToString());
                                                    }

                                                }


                                            }
                                            catch
                                            {
                                                device.IOParas[i].IORealData = null;
                                            }
                                        }
                                        break;
                                    case "ORACLE":
                                        {
                                            try
                                            {
                                                if (conn != "" && sql != "")
                                                {
                                                    DbHelperOra oracleHealp = new DbHelperOra();
                                                    oracleHealp.connectionString = conn;
                                                    DataSet ds = oracleHealp.Query(sql);
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        device.IOParas[i].IORealData = new IOData();
                                                        device.IOParas[i].IORealData.CommunicationID = device.IO_COMM_ID;
                                                        device.IOParas[i].IORealData.ServerID = device.IO_SERVER_ID;
                                                        device.IOParas[i].IORealData.ID = device.IO_DEVICE_ID;
                                                        device.IOParas[i].IORealData.ParaName = device.IOParas[i].IO_NAME;
                                                        device.IOParas[i].IORealData.ParaString = device.IOParas[i].IO_PARASTRING;
                                                        device.IOParas[i].IORealData.ParaValue = ds.Tables[0].Rows[0]["value"].ToString();
                                                        device.IOParas[i].IORealData.QualityStamp = QualityStamp.GOOD;
                                                        device.IOParas[i].IORealData.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["datetime"].ToString());
                                                    }

                                                }

                                            }
                                            catch
                                            {
                                                device.IOParas[i].IORealData = null;
                                            }
                                        }
                                        break;
                                    case "MySql":
                                        {
                                            try
                                            {
                                                if (conn != "" && sql != "")
                                                {
                                                    DbHelperMySQL mysqlHealp = new DbHelperMySQL();
                                                    mysqlHealp.connectionString = conn;
                                                    DataSet ds = mysqlHealp.Query(sql);
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        device.IOParas[i].IORealData = new IOData();
                                                        device.IOParas[i].IORealData.CommunicationID = device.IO_COMM_ID;
                                                        device.IOParas[i].IORealData.ServerID = device.IO_SERVER_ID;
                                                        device.IOParas[i].IORealData.ID = device.IO_DEVICE_ID;
                                                        device.IOParas[i].IORealData.ParaName = device.IOParas[i].IO_NAME;
                                                        device.IOParas[i].IORealData.ParaString = device.IOParas[i].IO_PARASTRING;
                                                        device.IOParas[i].IORealData.ParaValue = ds.Tables[0].Rows[0]["value"].ToString();
                                                        device.IOParas[i].IORealData.QualityStamp = QualityStamp.GOOD;
                                                        device.IOParas[i].IORealData.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["datetime"].ToString());
                                                        double d = 0;
                                                        if (double.TryParse(device.IOParas[i].IORealData.ParaValue, out d))
                                                        {
                                                            device.IOParas[i].IORealData.DataType = typeof(double);
                                                        }
                                                        else
                                                        {
                                                            device.IOParas[i].IORealData.DataType = typeof(string);

                                                        }
                                                    }

                                                }
                                            }
                                            catch
                                            {
                                                device.IOParas[i].IORealData = null;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        catch (Exception emx)
                        {
                            ThrowExceptionToMain(new Exception("数据接收关系数据库解析异常-" + emx.Message));
                        }
                        #endregion
                        #region 解析计算值包含公式计算的

                        if (device.IOParas[i].IO_POINTTYPE == "计算值")
                        {
                            if (device.IOParas[i].IO_FORMULA.Trim() != "")
                            {
                                device.IOParas[i].IORealData = new IOData();
                                try
                                {
                                    device.IOParas[i].IORealData.QualityStamp = QualityStamp.GOOD;
                                    device.IOParas[i].IORealData.Date = DateTime.Now;
                                    device.IOParas[i].IORealData.ParaName = device.IOParas[i].IO_NAME;
                                    device.IOParas[i].IORealData.DataType = typeof(double);
                                    //替换关键字为数值
                                    string formula = device.IOParas[i].IO_FORMULA;
                                    foreach (IO_PARA para in device.IOParas)
                                    {
                                        if (device.IOParas[i].IO_POINTTYPE != "字符串量" && device.IOParas[i].IO_POINTTYPE != "计算值" && device.IOParas[i].IORealData != null)
                                        {
                                            if (device.IOParas[i].IORealData.QualityStamp == QualityStamp.GOOD && device.IOParas[i].IORealData.ParaValue != "-9999" && device.IOParas[i].IORealData.ParaValue != "")
                                            {
                                                formula = formula.Replace(device.IOParas[i].IO_NAME, device.IOParas[i].IORealData.ParaValue);
                                            }

                                        }
                                    }
                                    if (formula != "")
                                    {
                                        //解析数学公式
                                        device.IOParas[i].IORealData.ParaValue = AnalyzeCalculate.Calculate(device.IOParas[i].IO_FORMULA);
                                        double d = 0;
                                        if (double.TryParse(device.IOParas[i].IORealData.ParaValue, out d))
                                        {
                                            device.IOParas[i].IORealData.DataType = typeof(double);
                                        }
                                        else
                                        {
                                            device.IOParas[i].IORealData.DataType = typeof(string);

                                        }
                                    }

                                    else
                                    {
                                        device.IOParas[i].IORealData.ParaValue = "-9999";
                                        device.IOParas[i].IORealData.QualityStamp = QualityStamp.BAD;
                                    }
                                }
                                catch
                                {
                                    device.IOParas[i].IORealData.QualityStamp = QualityStamp.BAD;
                                    device.IOParas[i].IORealData.Date = DateTime.Now;
                                    device.IOParas[i].IORealData.ParaName = device.IOParas[i].IO_NAME;
                                }

                            }
                            else
                            {
                                device.IOParas[i].IORealData = null;

                            }
                        }

                        #endregion
                        #region 进行量程转换  
                        if (device.IOParas[i].IO_POINTTYPE == "模拟量")
                        {

                            if (device.IOParas[i].IORealData != null && device.IOParas[i].IO_ENABLERANGECONVERSION == 1 && device.IOParas[i].IORealData.QualityStamp == QualityStamp.GOOD)
                            {
                                if (device.IOParas[i].IORealData.ParaValue != "" && device.IOParas[i].IORealData.ParaValue != "-9999")
                                {
                                    string value = ConvertParaTypeValue(device.IOParas[i].GetParaValueType(), device.IOParas[i].IORealData.ParaValue, double.Parse(device.IOParas[i].IO_RANGEMAX), double.Parse(device.IOParas[i].IO_RANGEMIN), double.Parse(device.IOParas[i].IO_MAXVALUE), double.Parse(device.IOParas[i].IO_MINVALUE));
                                    device.IOParas[i].IORealData.ParaValue = value;
                                }
                            }
                        }
                        #endregion
                        #region 常量值  

                        if (device.IOParas[i].IO_POINTTYPE == "常量值")
                        {
                            device.IOParas[i].IORealData = new IOData()
                            {
                                CommunicationID = device.IOParas[i].IO_COMM_ID,
                                DataType = typeof(string),
                                Date = device.GetedValueDate,
                                ParaName = device.IOParas[i].IO_NAME,
                                ParaString = device.IOParas[i].IO_PARASTRING,
                                ParaValue = device.IOParas[i].IO_INITALVALUE,
                                QualityStamp = QualityStamp.GOOD,
                                ServerID = device.IOParas[i].IO_SERVER_ID
                            };

                        }


                        #endregion

                    }
                    Driver = null;
                    #endregion
                    #region 将解析后的数据上传到数据中心服务器上
                    try
                    {
                        //将接收到的数据保存到实时缓存,主要用于批量上传,提高传输效率
                        receiveRealCache.Push(new ReceiveCacheObject() { DataString = IOMonitorRealDataUploadDBUtility.GetRealDataCacheString(device) });

                        TaskHelper.Factory.StartNew(() =>
                        {

                        //此次采用代理事件模式，否则会界面卡顿
                        if (OnMonitorUploadListView != null)
                                OnMonitorUploadListView(server, comm, device, "提交入库");
                        });
                        TaskHelper.Factory.StartNew(() =>
                        {
                        //显示实时数据
                        if (OnMonitorRealData != null)
                            {
                                OnMonitorRealData(server, comm, device, receivedatas);

                            }
                        });

                    }
                    catch (Exception emx)
                    {
                        ThrowExceptionToMain(new Exception("数据接收解析异常-" + emx.Message));


                    }
                    #endregion
                    #region 解析报警并上传报警信息
                    try
                    {
                        List<IO_PARAALARM> res = new List<IO_PARAALARM>();
                        //将接收到的数据保存到实时缓存,主要用于批量上传,提高传输效率
                        receiveRealCache.Push(new AlarmCacheObject() { DataString = IOMonitorRealDataUploadDBUtility.GetAlarmCacheString(device, out res) });
                        for (int i = 0; i < res.Count; i++)
                        {
                            if (OnMonitorAlarm != null)
                            {
                                OnMonitorAlarm(server, comm, device, res[i]);
                            }
                        }
                         


                    }
                    catch (Exception emx)
                    {
                        ThrowExceptionToMain(new Exception("数据接收报警处理异常-" + emx.Message));

                    }
                    #endregion
                    #region 修改设备状态值
                    IO_DEVICE sourceDevice = IOMonitorProjectManager.IODevices.Find(x => x.IO_DEVICE_ID == device.IO_DEVICE_ID);
                    if (sourceDevice != null)
                    {
                        sourceDevice.IO_DEVICE_STATUS = 1;
                        sourceDevice.Last_Online_Time = DateTime.Now;
                    }
                    #endregion
                }
                //释放内存
            });

        }
        #region 量程转换
        /// <summary>
        /// 量程变换，将下位机传输回来的数据进行数据转换
        /// </summary>
        /// <param name="strtype"></param>
        /// <param name="svalue"></param>
        /// <param name="rangemax"></param>
        /// <param name="rangemin"></param>
        /// <param name="valuemax"></param>
        /// <param name="valuemin"></param>
        /// <returns></returns>
        private  static  string ConvertParaTypeValue(Type strtype, string svalue, double rangemax, double rangemin, double valuemax, double valuemin)
        {
            try
            {
                //量程上限EULO+（裸数据上限PVRAWHI-裸数据下限PVRAWLO）*（量程上限EUHI-量程下限EULO）/（裸数据PVRAW-裸数据下限PVRAWLO）
                double value = -9999;
                if (double.TryParse(svalue, out value))
                {


                    svalue = (rangemin + (value - valuemin) * (rangemax - rangemin) / (valuemax - valuemin)).ToString();
                }
                else
                {
                    return "-9999";
                }

                if (strtype == typeof(sbyte))
                {
                    return Convert.ToSByte(svalue).ToString();
                }
                else if (typeof(byte) == strtype)
                {
                    return Convert.ToInt16(svalue).ToString();
                }
                else if (typeof(UInt16) == strtype)
                {
                    return Convert.ToUInt16(svalue).ToString();
                }
                else if (typeof(Int32) == strtype)
                {
                    return Convert.ToInt32(svalue).ToString();
                }
                else if (typeof(UInt32) == strtype)
                {
                    return Convert.ToUInt32(svalue).ToString();
                }
                else if (typeof(Int64) == strtype)
                {
                    return Convert.ToInt64(svalue).ToString();
                }
                else if (typeof(UInt64) == strtype)
                {
                    return Convert.ToUInt64(svalue).ToString();
                }
                else if (typeof(Single) == strtype)
                {
                    return Convert.ToSingle(svalue).ToString();
                }
                else if (typeof(Double) == strtype)
                {
                    return Convert.ToDouble(svalue).ToString();
                }
                else
                {
                    return value.ToString();
                }
            }
            catch
            {
                return "-9999";
            }


        }
        #endregion
        /// <summary>
        /// 返回通讯端口出现的异常错误
        /// </summary>
        /// <param name="msg"></param>

        private static void CDriverDll_Communication_Exception(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device,string msg)
        {
           

                ThrowExceptionToMain(new Exception("ERROR600005" + msg));
                SendScadaEvent(ScadaEvent.通道驱动异常, new ScadaEventModel()
                {
                    COMM_ID = comm.IO_COMM_ID,
                    COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                    SERVER_ID = server.SERVER_ID,
                    DEVICE_ID = device.IO_DEVICE_ID,
                    DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]",
                     Content= msg

                });
            
        }
        /// <summary>
        /// 设备状态变化返回的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="tag"></param>

        private static void CDriverDll_DeviceStatusChanged(IO_SERVER server, IO_COMMUNICATION comm, List<IO_DEVICE> devices)
        {

            if (devices == null || devices.Count <= 0)
                return;
            TaskHelper.Factory.StartNew(() =>
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    IO_DEVICE device = devices[i];
                    if (device.IO_DEVICE_STATUS == 1)
                    {
                        AddLogToMainLog("设备" + device.IO_DEVICE_NAME + " 上线 ");
                        SendScadaEvent(ScadaEvent.设备上线, new ScadaEventModel()
                        {
                            COMM_ID = comm.IO_COMM_ID,
                            COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                            SERVER_ID = server.SERVER_ID,
                            DEVICE_ID = device.IO_DEVICE_ID,
                            DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]"

                        });
                        ///向服务器发送设备上线童子
                        receiveRealCache.Push(new ScadaStatusCacheObject()
                        {

                            COMM_ID = comm.IO_COMM_ID,
                            DEVICE_ID = device.IO_DEVICE_ID,
                            ScadaStatus = ScadaStatus.True,
                            SERVER_ID = server.SERVER_ID,
                            StatusElemnt = ScadaStatusElemnt.Device
                        });
                    }
                    else
                    {
                        AddLogToMainLog("设备" + device.IO_DEVICE_NAME + " 下线 ");

                        SendScadaEvent(ScadaEvent.设备下线, new ScadaEventModel()
                        {
                            COMM_ID = comm.IO_COMM_ID,
                            COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                            SERVER_ID = server.SERVER_ID,
                            DEVICE_ID = device.IO_DEVICE_ID,
                            DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]"

                        });
                        receiveRealCache.Push(new ScadaStatusCacheObject()
                        {

                            COMM_ID = comm.IO_COMM_ID,
                            DEVICE_ID = device.IO_DEVICE_ID,
                            ScadaStatus = ScadaStatus.False,
                            SERVER_ID = server.SERVER_ID,
                            StatusElemnt = ScadaStatusElemnt.Device
                        });
                    }

                }
                IOMonitorUIManager.RefreshDeviceTreeNodeStatus(devices);
            });
        }
        /// <summary>
        /// 本通讯通道内设备发送数据后返回的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>

        private static   void CDriverDll_DeviceSended(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value, bool result)
        {
           

                if (result)
                {
                    AddLogToMainLog("下置" + device.IO_DEVICE_NAME + "设备" + para.IO_NAME + "IO点值" + value + "成功!");
                    SendScadaEvent(ScadaEvent.下置命令成功, new ScadaEventModel()
                    {
                        COMM_ID = comm.IO_COMM_ID,
                        COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                        SERVER_ID = server.SERVER_ID,
                        DEVICE_ID = device.IO_DEVICE_ID,
                        DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]",
                        IO_ID = para.IO_ID,
                        IO_LABEL = para.IO_LABEL,
                        IO_NAME = para.IO_NAME,
                        Content = "设置值=" + value
                    }) ;
                }
                else
                {
                    AddLogToMainLog("下置" + device.IO_DEVICE_NAME + "设备" + para.IO_NAME + "IO点值" + value + "失败!");
                    SendScadaEvent(ScadaEvent.下置命令失败, new ScadaEventModel()
                    {
                        COMM_ID = comm.IO_COMM_ID,
                        COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                        SERVER_ID = server.SERVER_ID,
                        DEVICE_ID = device.IO_DEVICE_ID,
                        DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]",
                        IO_ID = para.IO_ID,
                        IO_LABEL = para.IO_LABEL,
                        IO_NAME = para.IO_NAME,
                        Content = "设置值=" + value
                    });
                }

            
          

        }
        /// <summary>
        /// 本通讯通道内设备上线后返回的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="tag"></param>


        /// <summary>
        /// 通讯通道被停止后返回的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="tag"></param>

        private static void CDriverDll_CommunctionStop(IO_SERVER server, IO_COMMUNICATION comm, object tag)
        {


            AddLogToMainLog(comm.IO_COMM_NAME + "通讯通道被关闭!");
            SendScadaEvent(ScadaEvent.通道关闭, new ScadaEventModel()
            {
                COMM_ID = comm.IO_COMM_ID,
                COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                SERVER_ID = server.SERVER_ID
            });
            receiveRealCache.Push(new ScadaStatusCacheObject()
            {

                COMM_ID = comm.IO_COMM_ID,
                ScadaStatus = ScadaStatus.False,
                SERVER_ID = server.SERVER_ID,
                StatusElemnt = ScadaStatusElemnt.Communication
            });
        }
        /// <summary>
        /// 通讯通道启动服务返回的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="tag"></param>

        private static void CDriverDll_CommunctionStart(IO_SERVER server, IO_COMMUNICATION comm, object tag)
        {
            

                AddLogToMainLog(comm.IO_COMM_NAME + "通讯通道启动!");
                SendScadaEvent(ScadaEvent.通道开启, new ScadaEventModel()
                {
                    COMM_ID = comm.IO_COMM_ID,
                    COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                    SERVER_ID = server.SERVER_ID
                });
            receiveRealCache.Push(new ScadaStatusCacheObject()
            {

                COMM_ID = comm.IO_COMM_ID,
                ScadaStatus = ScadaStatus.True,
                SERVER_ID = server.SERVER_ID,
                StatusElemnt = ScadaStatusElemnt.Communication
            });
        }
        /// <summary>
        /// 通讯通道被暂停返回的事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="tag"></param>

        private static void CDriverDll_CommunctionPause(IO_SERVER server, IO_COMMUNICATION comm, object tag)
        {
           
                AddLogToMainLog(comm.IO_COMM_NAME + "通讯通道暂停!");
                SendScadaEvent(ScadaEvent.通道关闭, new ScadaEventModel()
                {
                    COMM_ID = comm.IO_COMM_ID,
                    COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                    SERVER_ID = server.SERVER_ID
                });
            receiveRealCache.Push(new ScadaStatusCacheObject()
            {

                COMM_ID = comm.IO_COMM_ID,
                ScadaStatus = ScadaStatus.False,
                SERVER_ID = server.SERVER_ID,
                StatusElemnt = ScadaStatusElemnt.Communication
            });
        }
        /// <summary>
        /// 通讯通道暂停后继续服务的返回事件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="tag"></param>

        //通讯通道继续
        private static void CDriverDll_CommunctionContinue(IO_SERVER server, IO_COMMUNICATION comm, object tag)
        {
            

                AddLogToMainLog(comm.IO_COMM_NAME + "通讯通道继续服务!");
                SendScadaEvent(ScadaEvent.通道开启, new ScadaEventModel()
                {
                    COMM_ID = comm.IO_COMM_ID,
                    COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                    SERVER_ID = server.SERVER_ID
                });
            receiveRealCache.Push(new ScadaStatusCacheObject()
            {

                COMM_ID = comm.IO_COMM_ID,
                ScadaStatus = ScadaStatus.True,
                SERVER_ID = server.SERVER_ID,
                StatusElemnt = ScadaStatusElemnt.Communication
            });
        }

        //通讯驱动被关闭后120秒后重新再连接
        private static void CDriverDll_CommunctionClose(IO_SERVER server, IO_COMMUNICATION comm, object tag)
        {


            AddLogToMainLog(comm.IO_COMM_NAME + "通讯通道已关闭!");
            SendScadaEvent(ScadaEvent.通道关闭, new ScadaEventModel()
            {
                COMM_ID = comm.IO_COMM_ID,
                COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]",
                SERVER_ID = server.SERVER_ID
            });
            receiveRealCache.Push(new ScadaStatusCacheObject()
            {

                COMM_ID = comm.IO_COMM_ID,
                ScadaStatus = ScadaStatus.False,
                SERVER_ID = server.SERVER_ID,
                StatusElemnt = ScadaStatusElemnt.Communication
            });
        }
        #endregion
        #region 当然任务的启动停止暂停的方法

        //开始任务
        public static Task Start()
        {
            return TaskHelper.Factory.StartNew(() => { 
            //创建通信子任务
            for (int i = 0; i < IOMonitorProjectManager.IOCommunications.Count; i++)
            {
                if (IOMonitorProjectManager.IOCommunications[i].CommunicateDriver != null)
                {
                        try
                        {
                            ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver;
                            //创建主任务
                            var task = driverDll.StartServer();
                        }
                        catch (Exception emx)
                        {
                            ThrowExceptionUploadCenter(emx);
                        }
                }


            }
            TaskOperator = TaskOperator.运行;
            if (OnMonitorOperator != null)
            {
                OnMonitorOperator(TaskOperator);
            }
            AddLogToMainLog("启动采集服务");
            SendScadaEvent(ScadaEvent.采集站启动, new ScadaEventModel()
            {

            });
            });
        }
        public static Task LoginManager(string user, string password)
        {
            return TaskHelper.Factory.StartNew(() =>
            {
                STATION_TCP_INFO loginInfo = new STATION_TCP_INFO();
                loginInfo.USER = user;
                loginInfo.PASSWROD = password;
                loginInfo.IO_SERVER_ID = IOMonitorProjectManager.ServerID;
                loginInfo.IO_SERVER_IP = "";
                loginInfo.RESULT = "false";
                loginInfo.FUNCTION = "IOMonitor";
                TcpData tcpData = new TcpData();
                byte[] loginbytes = tcpData.StringToTcpByte(loginInfo.GetCommandString());
                //发送登录命令
                if (MDSClient != null)
                    MDSClient.Send(loginbytes, mDSServerConfig.CenterAppName, MDSCommandType.登录);
            });
        }
     
        public static  Task Suspend()
        {
            return TaskHelper.Factory.StartNew(() => {

                for (int i = 0; i < IOMonitorProjectManager.IOCommunications.Count; i++)
                {

                    //此处不用线程，在具体实现中用户采用线程
                    if (IOMonitorProjectManager.IOCommunications[i].CommunicateDriver != null)
                    {
                        ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver;
                        driverDll.PauseServer();
                        

                    }


                }
            TaskOperator = TaskOperator.暂停;
            if (OnMonitorOperator != null)
            {
                OnMonitorOperator(TaskOperator);
            }
            AddLogToMainLog("暂停采集服务");


            });
            AddLogToMainLog("启动采集服务");
            SendScadaEvent(ScadaEvent.采集站关闭, new ScadaEventModel()
            {

            });

        }
        public static  Task  Continue()
        {
            return TaskHelper.Factory.StartNew(() => {
            for (int i = 0; i < IOMonitorProjectManager.IOCommunications.Count; i++)
            {

                //此处不用线程，在具体实现中用户采用线程
                if (IOMonitorProjectManager.IOCommunications[i].CommunicateDriver != null)
                {
                        ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver;
                    driverDll.ContinueServer();

                }


            }
            TaskOperator = TaskOperator.运行;
            if (OnMonitorOperator != null)
            {
                OnMonitorOperator(TaskOperator);
            }
            AddLogToMainLog("继续采集服务");
            });
            SendScadaEvent(ScadaEvent.采集站启动, new ScadaEventModel()
            {

            });
        }
        public static Task Stop()
        {
            return TaskHelper.Factory.StartNew(() =>
            {
                try
                {
                    if (IOMonitorProjectManager.IOCommunications != null)
                        for (int i = 0; i < IOMonitorProjectManager.IOCommunications.Count; i++)
                        {

                            //此处不用线程，在具体实现中用户采用线程
                            if (IOMonitorProjectManager.IOCommunications[i].CommunicateDriver != null)
                            {
                                ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver;
                                driverDll.StopServer();
                                receiveRealCache.Push(new ScadaStatusCacheObject
                                {
                                    COMM_ID = IOMonitorProjectManager.IOCommunications[i].IO_COMM_ID,
                                    SERVER_ID = IOMonitorProjectManager.IOCommunications[i].IO_SERVER_ID,
                                    ScadaStatus = ScadaStatus.False,
                                    StatusElemnt = ScadaStatusElemnt.Communication

                                });
                                for (int d = 0; d < IOMonitorProjectManager.IOCommunications[i].Devices.Count; d++)
                                {
                                    receiveRealCache.Push(new ScadaStatusCacheObject
                                    {
                                        COMM_ID = IOMonitorProjectManager.IOCommunications[i].IO_COMM_ID,
                                        SERVER_ID = IOMonitorProjectManager.IOCommunications[i].IO_SERVER_ID,
                                        DEVICE_ID = IOMonitorProjectManager.IOCommunications[i].Devices[d].IO_DEVICE_ID,
                                        ScadaStatus = ScadaStatus.False,

                                        StatusElemnt = ScadaStatusElemnt.Device

                                    });

                                }
                            }


                        }
                    TaskOperator = TaskOperator.停止;
                    if (OnMonitorOperator != null)
                    {
                        OnMonitorOperator(TaskOperator);
                    }
                    AddLogToMainLog("停止采集服务");
                }
                catch (Exception emx)
                { ThrowExceptionToMain(emx); }
            });
            SendScadaEvent(ScadaEvent.采集站关闭, new ScadaEventModel()
            {

            });

        }
        //关闭并释放任务，该任务要求初始化所有任务
        public static Task Close()
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                try
                {
             
                    if (IOMonitorProjectManager.IOCommunications != null)
                    {


                        for (int i = 0; i < IOMonitorProjectManager.IOCommunications.Count; i++)
                        {

                            //此处不用线程，在具体实现中用户采用线程
                            if (IOMonitorProjectManager.IOCommunications[i].CommunicateDriver != null)
                            {
                                ScadaCommunicateKernel driverDll = (ScadaCommunicateKernel)IOMonitorProjectManager.IOCommunications[i].CommunicateDriver;
                                driverDll.CommunctionClose -= CDriverDll_CommunctionClose;
                                driverDll.CommunctionContinue -= CDriverDll_CommunctionContinue;
                                driverDll.CommunctionPause -= CDriverDll_CommunctionPause;
                                driverDll.CommunctionStart -= CDriverDll_CommunctionStart;
                                driverDll.CommunctionStop -= CDriverDll_CommunctionStop;

                                driverDll.DeviceSended -= CDriverDll_DeviceSended;
                                driverDll.DeviceStatusChanged -= CDriverDll_DeviceStatusChanged;
                                driverDll.OnKernelException -= CDriverDll_Communication_Exception;
                                driverDll.KernelDataReceived = null;


                                driverDll.OnKernelLog -= CDriverDll_KernelLog;
                                driverDll.StopServer();
                                receiveRealCache.Push(new ScadaStatusCacheObject { 
                                 COMM_ID= IOMonitorProjectManager.IOCommunications[i].IO_COMM_ID,
                                  SERVER_ID= IOMonitorProjectManager.IOCommunications[i].IO_SERVER_ID,
                                   ScadaStatus= ScadaStatus.False,
                                    StatusElemnt= ScadaStatusElemnt.Communication

                                });
                                for (int d = 0; d < IOMonitorProjectManager.IOCommunications[i].Devices.Count; d++)
                                {
                                    receiveRealCache.Push(new ScadaStatusCacheObject
                                    {
                                        COMM_ID = IOMonitorProjectManager.IOCommunications[i].IO_COMM_ID,
                                        SERVER_ID = IOMonitorProjectManager.IOCommunications[i].IO_SERVER_ID,
                                         DEVICE_ID= IOMonitorProjectManager.IOCommunications[i].Devices[d].IO_DEVICE_ID,
                                        ScadaStatus = ScadaStatus.False,

                                        StatusElemnt = ScadaStatusElemnt.Device

                                    });

                                }
                                

                            }


                        }
                        IOMonitorRealDataUploadDBUtility.UploadScadaStatus(receiveRealCache.PopScadaStatus());
                        Thread.Sleep(500);
                    }
                    TaskOperator = TaskOperator.关闭;
                    if (OnMonitorOperator != null)
                    {
                        OnMonitorOperator(TaskOperator);
                    }
                    AddLogToMainLog("停止并并关闭采集服务");
                    MemoryCollect.Close();

                    if (MDSClient != null)
                    {
                        MDSClient.Disconnect();
                        MDSClient.Dispose();
                        MDSClient = null;
                    }

                    if (receiveRealCache != null)
                    {
                        receiveRealCache.Dispose();
                        receiveRealCache = null;
                    }
                    if (StatusMonitor != null)
                        StatusMonitor.Dispose();
                    StatusMonitor = null;

                    if (ComputerStatusMonitor != null)
                        ComputerStatusMonitor.Close();
                    ComputerStatusMonitor = null;
                }
                catch (Exception emx)
                { ThrowExceptionToMain(emx); }

            });
             
        }

        #endregion
        


    }
}
