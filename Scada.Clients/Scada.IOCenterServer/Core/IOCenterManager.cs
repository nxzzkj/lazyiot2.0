using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.Logger;
using Scada.MachineTraining;
using ScadaCenterServer.Controls;
using ScadaCenterServer.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Temporal.DbAPI;
using Temporal.Net.InfluxDb.Models.Responses;
using System.Linq;
using System.ComponentModel;
using Scada.Model;
using System.Text;
using Scada.MDSCore.Settings;
using Scada.MDSCore;
using Scada.BatchCommand;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaCenterServer.Core
{

    /// <summary>
    /// 任务管理器
    /// </summary>
    public abstract class IOCenterManager
    {
        /// <summary>
        /// 垃圾内存定时清理器
        /// </summary>

        /// <summary>
        /// 是否在后头运行,前台不显示相关信息
        /// </summary>
        public static bool IsBackRun = true;
        public static IOCenterIOProjectManager IOProject = null;
        public static IOCenterSimulatorManager SimulatorManager = null;

        public static IOCenterUIManager QueryFormManager = null;
        public static MonitorForm ServerForm = null;
        public static IOCenterInfluxdbBackupManager InfluxdbBackupManager = null;
        public static IOCenterMonitor IOCenterServer = null;
        public static IOCenterClient IOCenterClient = null;
        //机器学习相关
        public static MachineTrainManager TrainManager = null;
        public static ComputerStatusMonitor ComputerStatusMonitor;
        public static MDSConfig MDSConfig = null;
        public static IOCenterBatchCommandTaskManager BatchCommandTask;

       
        public static bool EnableLoggger
        {
            get { return Logger.GetInstance().Enable; }
            set { Logger.GetInstance().Enable = value; }
        }

        //是否要新创建influxdb连接，做测试用
        public static bool NewInfluxDB = false;
        public static InfluxDbManager InfluxDbManager
        {
            get
            {
                if (NewInfluxDB)
                {
                    InfluxDbManager mInfluxDbManager = new InfluxDbManager(IOCenterManager.IOProject.ServerConfig.influxdConfig.HttpAddress, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.DataBaseName, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.User, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.Password, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.InfluxDBVersion);
                    mInfluxDbManager.ShouldConnectInfluxDb();
                    return mInfluxDbManager;
                }
                else
                {
                    return QueryFormManager.InfluxDbManager;
                }
            }


        }
        private static System.Threading.Timer deviceStatusTimer = null;
        public static bool EnableWriterLog = true;
        private static void LogMonitor()
        {
            Scada.Logger.Logger.GetInstance().Run();
            TaskHelper.Factory.StartNew(() =>
            {
                while (EnableWriterLog)
                {
                    Thread.Sleep(1000 * 60);

                    Scada.Logger.Logger.GetInstance().WriteLog();
                }


            });
        }
        public static bool PublishRestart = false;
        /// <summary>
        /// 用户重新发布工程后重新启动相关业务
        /// </summary>
        public static void LoadSystem(IOCenterMainForm iOCenterMain)
        {
            IOCenterLoading frmLoading = new IOCenterLoading();
            frmLoading.BackgroundWorkAction = delegate ()
            {
                try
                {
                    IISExpressManager.IISExpress();
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(10, "正在配置Web服务器...");
                    IOCenterManager.IOProject.LoadProject();
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(20, "初始化查询管理目录树...");
                    IOCenterManager.QueryFormManager.LoadQueryTreeProject();
                    IOCenterManager.QueryFormManager.LoadQueryMachineTreeProject();
                    IOCenterManager.QueryFormManager.LoadQueryBatchCommandTaskTreeProject();
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(30, "正在初始化数据查询管理器...");
                    IOCenterManager.QueryFormManager.StartInfluxDBServer();

                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(40, "正在初始化时序数据库...");
                    InfluxdbBackupManager = new IOCenterInfluxdbBackupManager();
                    if (IOCenterManager.IOProject.ServerConfig.Backups.Enable)
                    {
                        InfluxdbBackupManager.Start();
                    }
                    else
                    {
                        InfluxdbBackupManager.Stop();
                    }
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "运行时序数据库...");
                    InfluxdbBackupManager.Run();
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(60, "启动网络通信服务组件...");
                    IOCenterServer.InitServer();
                    IOCenterServer.Start();//启动通讯服务
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(70, "初始化数据中心通讯组件...");
                    IOCenterClient = new IOCenterClient(MDSConfig.CenterAppName, IPAddressSelector.Instance().AddressIP, int.Parse(MDSConfig.MDSServerPort), IOCenterServer);
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(80, "正在创建数据中心链接...");
                    IOCenterClient.Connect();
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(90, "正在启动模型训练任务...");
                    TrainManager.InitMachineTrainManager(IOProject.DataBaseFileName);
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "SCADA数据中心启动成功...");


                }
                catch (Exception ex)
                {

                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "SCADA数据中心启动失败!");
                    MessageBox.Show(frmLoading, "加载资源时出现错误 " + ex.Message);
                    Application.ExitThread();
                    Application.Exit();
                }
            };
            IOProject_CenterServerLog("系统资源全部创建完成，启动服务成功");
            frmLoading.ShowDialog(iOCenterMain);

            IOCenterClient.OnRefreshEventListView += IOCenterClient_OnRefreshEventListView;
            IOCenterClient.OnRefreshMainTreeDeviceStatus += IOCenterClient_OnRefreshMainTreeDeviceStatus;
            IOCenterClient.OnRefreshMainTreeCommunicationStatus += IOCenterClient_OnRefreshMainTreeCommunicationStatus;
            IOCenterClient.OnRefreshCommandListView += IOCenterClient_OnRefreshCommandListView;
            IOCenterClient.OnRefreshReeiveAlarmListView += IOCenterClient_OnRefreshReeiveAlarmListView;
            IOCenterClient.OnRefreshMainTreeServerStatus += IOCenterClient_OnRefreshMainTreeServerStatus;
            IOCenterClient.OnRefreshReeiveDeviceListView += IOCenterClient_OnRefreshReeiveDeviceListView;
            IOCenterClient.OnRefreshAiTrainDeviceListView += IOCenterClient_AddAiTrain;

         
    
        }

        private static void IOCenterClient_OnRefreshReeiveDeviceListView(string appname, string datetime, string server, string communication, string device, string msg, bool result)
        {
            if (IOCenterServer.ServerForm != null && !IOCenterServer.ServerForm.IsDisposed)
            {
                IOCenterServer.ServerForm.AddReeiveDevice(appname, datetime, server, communication, device, msg, result);
            }
        }
        private static void IOCenterClient_AddAiTrain(string appname, string datetime, string server, string communication, string device, string taskName, string Algorithm, string AlgorithmType, string PredictedLabel, string Score, string ColumnValue, string Columns)
        {
            if (IOCenterServer.ServerForm != null && !IOCenterServer.ServerForm.IsDisposed)
            {
                IOCenterServer.ServerForm.AddAiTrain(appname, datetime, server, communication, device, taskName, Algorithm, AlgorithmType, PredictedLabel, Score, ColumnValue, Columns);
            }
        }
        private static void IOCenterClient_OnRefreshMainTreeServerStatus(Scada.Model.IO_SERVER server, bool status, string mac)
        {


            QueryFormManager.ServerStatus(server, status);
        }

        private static void IOCenterClient_OnRefreshReeiveAlarmListView(string appname, string server, string communication, string device, Scada.Model.IO_PARAALARM alarm, bool result)
        {
            if (IOCenterServer.ServerForm != null && !IOCenterServer.ServerForm.IsDisposed)
            {
                IOCenterServer.ServerForm.AddReeiveAlarm(appname, server, communication, device, alarm, result);
            }
        }

        private static void IOCenterClient_OnRefreshCommandListView(string appname, string server, string communication, string device, string para, Scada.Model.IO_COMMANDS command)
        {
            if (IOCenterServer.ServerForm != null && !IOCenterServer.ServerForm.IsDisposed)
            {
                IOCenterServer.ServerForm.AddCommand(appname, server, communication, device, para, command);
            }

        }

        private static void IOCenterClient_OnRefreshMainTreeDeviceStatus(List<Scada.Model.IO_DEVICE> devices)
        {
            TaskHelper.Factory.StartNew(() =>
            {
                QueryFormManager.DeviceStatus(devices);
            });

        }
        private static void IOCenterClient_OnRefreshMainTreeCommunicationStatus(Scada.Model.IO_COMMUNICATION comm, bool status)
        {

            QueryFormManager.CommunicationStatus(comm, status);

        }
        private static void IOCenterClient_OnRefreshEventListView(string application, string eventdate, string eventname, string server, string communication, string device, string msg)
        {
            if (IOCenterServer.ServerForm != null && !IOCenterServer.ServerForm.IsDisposed)
            {
                IOCenterServer.ServerForm.AddEvent(application, eventdate, eventname, server, communication, device, msg);
            }

        }
        public static void WriterMainFormLog(string log)
        {
            if (QueryFormManager != null)
            {
                QueryFormManager.AddLog(log);
            }
        }
        private static string GetEnumCategory<T>(T tField)
        {
            var description = string.Empty; //结果
            var inputType = tField.GetType(); //输入的类型
            var descType = typeof(CategoryAttribute); //目标查找的描述类型

            var fieldStr = tField.ToString();                //输入的字段字符串
            var field = inputType.GetField(fieldStr);        //目标字段

            var isDefined = field.IsDefined(descType, false);//判断描述是否在字段的特性
            if (isDefined)
            {
                var EnumAttributes = (CategoryAttribute[])field        //得到特性信息
                    .GetCustomAttributes(descType, false);
                description = EnumAttributes.FirstOrDefault()?.Category ?? string.Empty;
            }
            return description;
        }
        private static string GetEnumScadaMachineTrainingAlgorithm(string tField)
        {

            ScadaMachineTrainingAlgorithm res = (ScadaMachineTrainingAlgorithm)Enum.Parse(typeof(ScadaMachineTrainingAlgorithm), tField);
            return GetEnumCategory<ScadaMachineTrainingAlgorithm>(res);
        }

        public static void InitIOCenterManager()
        {
            MDSConfig = new MDSConfig();
            IOProject = new IOCenterIOProjectManager();
            IOProject.CenterServerException += IOProject_CenterServerException;
            IOProject.CenterServerLog += IOProject_CenterServerLog;
            SimulatorManager = new IOCenterSimulatorManager();//模拟器服务

            QueryFormManager = new IOCenterUIManager();//数据查询相关服务
            IOCenterServer = new IOCenterMonitor()
            {
                ServerException = (msg) =>
                {

                    IOProject_CenterServerException(msg);
                },
                ServerNormalWriteLog = (msg) =>
                {

                    IOProject_CenterServerLog(msg);
                }
            };//初始化MDS通讯服务的客户端应用程序

            //创建垃圾定时回收
            MemoryCollect.Monitor(TaskHelper.Factory);

            LogMonitor();
            ComputerStatusMonitor = new ComputerStatusMonitor();
            ComputerStatusMonitor.ComputerLocked = () =>
            {
                IsBackRun = true;

            };
            ComputerStatusMonitor.Start();
            deviceStatusTimer = new System.Threading.Timer(new TimerCallback(CheckDeviceStatus), null, 50000, 30000);

            TrainManager = new MachineTrainManager();
            TrainManager.RunStatus = true;
            TrainManager.TrainManagerLog = TrainManager.ExceptionThrow = (string msg) =>
            {

                return TaskHelper.Factory.StartNew(() =>
                {

                    WriterMainFormLog(msg);
                });

            };
            ///从时序数据库读取的预测周期内的数据
            TrainManager.ReadToTxtTrainFromInfluxdb = (DateTime currentTime, List<Scada.Model.ScadaMachineTrainingModel> trainingModels) =>
            {
                List<ScadaMachineTrainingInput> results = new List<ScadaMachineTrainingInput>();
                try
                {
                    if (IOCenterManager.InfluxDbManager == null)
                        return null;
            
                    List<IO_DEVICE> devices = new List<IO_DEVICE>();
                    trainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel train)
                    {
                        var finder = IOProject.Devices.Find(x => x.IO_DEVICE_ID == train.DEVICE_ID &&
                          x.IO_SERVER_ID == train.SERVER_ID && x.IO_COMM_ID == train.COMM_ID);

                        if (finder != null && !devices.Exists(x => x.IO_DEVICE_ID == train.DEVICE_ID &&
                               x.IO_SERVER_ID == train.SERVER_ID && x.IO_COMM_ID == train.COMM_ID))
                            devices.Add(finder);
                    });

                    var foreashData = InfluxDbManager.DbQuery_MachineTrainForeastSourceRealData(trainingModels, devices, currentTime).GetAwaiter().GetResult();
                    if (foreashData != null && foreashData.Count > 0)
                    {

                        foreashData.ForEach(delegate (IEnumerable<Serie> mseries)
                        {
                            if (mseries.Count() > 0)
                            {
                                var s = mseries.First();//获取第一条数据
                                if (s != null && s.Tag != null)
                                {
                                    Scada.Model.ScadaMachineTrainingModel trainingModel = s.Tag as Scada.Model.ScadaMachineTrainingModel;
                                    //获取首个时间
                                    int dateindex = s.Columns.IndexOf("time");
                                    if (dateindex >= 0)
                                    {
                                        for (int i = 0; i < s.Values.Count; i++)
                                        {
                                            if (dateindex >= 0)
                                            {
                                                string date = InfluxDbManager.GetInfluxdbValue(s.Values[i][dateindex]);
                                                string category = GetEnumScadaMachineTrainingAlgorithm(trainingModel.Algorithm);
                                                switch (category)
                                                {
                                                    case "二元分类":
                                                        {
                                                            #region
                                                            ScadaMachineTrainingBinaryInput input = new ScadaMachineTrainingBinaryInput()
                                                            {

                                                                COMM_ID = trainingModel.COMM_ID,
                                                                SERVER_ID = trainingModel.SERVER_ID,
                                                                DEVICE_ID = trainingModel.DEVICE_ID,
                                                                TaskId = trainingModel.Id,
                                                                Algorithm = trainingModel.Algorithm,
                                                                Properties = trainingModel.Properties.Split(','),
                                                                DateStampTime = date,
                                                                AlgorithmType = trainingModel.AlgorithmType,
                                                                TaskName = trainingModel.TaskName,
                                                                ColumnNumber = trainingModel.Properties.Split(',').Length



                                                            };

                                                            for (int c = 0; c < input.Properties.Length; c++)
                                                            {

                                                                string ioName = input.Properties[c].Trim().ToLower();
                                                                int paraindex = s.Columns.IndexOf(ioName);
                                                                if (paraindex >= 0)
                                                                {
                                                                    if (s.Values[i][paraindex] != null)
                                                                    {
                                                                        float v;
                                                                        if (float.TryParse(s.Values[i][paraindex].ToString(), out v))
                                                                        {
                                                                            switch (c)
                                                                            {
                                                                                case 0:
                                                                                    input.Column1 = v;
                                                                                    break;
                                                                                case 1:
                                                                                    input.Column2 = v;
                                                                                    break;
                                                                                case 2:
                                                                                    input.Column3 = v;
                                                                                    break;
                                                                                case 3:
                                                                                    input.Column4 = v;
                                                                                    break;
                                                                                case 5:
                                                                                    input.Column6 = v;
                                                                                    break;
                                                                                case 7:
                                                                                    input.Column8 = v;
                                                                                    break;
                                                                                case 8:
                                                                                    input.Column9 = v;
                                                                                    break;
                                                                                case 9:
                                                                                    input.Column10 = v;
                                                                                    break;
                                                                            }

                                                                        }

                                                                    }
                                                                }

                                                            }
                                                            results.Add(input);
                                                            #endregion
                                                        }
                                                        break;
                                                    case "多类分类":
                                                        {
                                                            #region
                                                            ScadaMachineTrainingMultiClassicInput input = new ScadaMachineTrainingMultiClassicInput()
                                                            {

                                                                COMM_ID = trainingModel.COMM_ID,
                                                                SERVER_ID = trainingModel.SERVER_ID,
                                                                DEVICE_ID = trainingModel.DEVICE_ID,
                                                                TaskId = trainingModel.Id,
                                                                Algorithm = trainingModel.Algorithm,
                                                                Properties = trainingModel.Properties.Split(','),
                                                                DateStampTime = date,

                                                                AlgorithmType = trainingModel.AlgorithmType,
                                                                TaskName = trainingModel.TaskName,
                                                                ColumnNumber = trainingModel.Properties.Split(',').Length


                                                            };

                                                            for (int c = 0; c < input.Properties.Length; c++)
                                                            {

                                                                string ioName = input.Properties[c].Trim().ToLower();
                                                                int paraindex = s.Columns.IndexOf(ioName);
                                                                if (paraindex >= 0)
                                                                {
                                                                    if (s.Values[i][paraindex] != null)
                                                                    {
                                                                        float v;
                                                                        if (float.TryParse(s.Values[i][paraindex].ToString(), out v))
                                                                        {
                                                                            switch (c)
                                                                            {
                                                                                case 0:
                                                                                    input.Column1 = v;
                                                                                    break;
                                                                                case 1:
                                                                                    input.Column2 = v;
                                                                                    break;
                                                                                case 2:
                                                                                    input.Column3 = v;
                                                                                    break;
                                                                                case 3:
                                                                                    input.Column4 = v;
                                                                                    break;
                                                                                case 5:
                                                                                    input.Column6 = v;
                                                                                    break;
                                                                                case 7:
                                                                                    input.Column8 = v;
                                                                                    break;
                                                                                case 8:
                                                                                    input.Column9 = v;
                                                                                    break;
                                                                                case 9:
                                                                                    input.Column10 = v;
                                                                                    break;
                                                                            }

                                                                        }

                                                                    }
                                                                }

                                                            }
                                                            results.Add(input);
                                                            #endregion
                                                        }
                                                        break;


                                                    case "异常检测":
                                                        {
                                                            #region
                                                            ScadaMachineTrainingRandomizedPcaInput input = new ScadaMachineTrainingRandomizedPcaInput()
                                                            {

                                                                COMM_ID = trainingModel.COMM_ID,
                                                                SERVER_ID = trainingModel.SERVER_ID,
                                                                DEVICE_ID = trainingModel.DEVICE_ID,
                                                                TaskId = trainingModel.Id,
                                                                Algorithm = trainingModel.Algorithm,
                                                                Properties = trainingModel.Properties.Split(','),
                                                                DateStampTime = date,
                                                                AlgorithmType = trainingModel.AlgorithmType,
                                                                TaskName = trainingModel.TaskName,
                                                                ColumnNumber = trainingModel.Properties.Split(',').Length



                                                            };

                                                            for (int c = 0; c < input.Properties.Length; c++)
                                                            {

                                                                string ioName = input.Properties[c].Trim().ToLower();
                                                                int paraindex = s.Columns.IndexOf(ioName);
                                                                if (paraindex >= 0)
                                                                {
                                                                    if (s.Values[i][paraindex] != null)
                                                                    {
                                                                        float v;
                                                                        if (float.TryParse(s.Values[i][paraindex].ToString(), out v))
                                                                        {
                                                                            switch (c)
                                                                            {
                                                                                case 0:
                                                                                    input.Column1 = v;
                                                                                    break;
                                                                                case 1:
                                                                                    input.Column2 = v;
                                                                                    break;
                                                                                case 2:
                                                                                    input.Column3 = v;
                                                                                    break;
                                                                                case 3:
                                                                                    input.Column4 = v;
                                                                                    break;
                                                                                case 5:
                                                                                    input.Column6 = v;
                                                                                    break;
                                                                                case 7:
                                                                                    input.Column8 = v;
                                                                                    break;
                                                                                case 8:
                                                                                    input.Column9 = v;
                                                                                    break;
                                                                                case 9:
                                                                                    input.Column10 = v;
                                                                                    break;
                                                                            }

                                                                        }

                                                                    }
                                                                }

                                                            }
                                                            results.Add(input);
                                                            #endregion
                                                        }
                                                        break;

                                                }

                                            }
                                        }

                                    }
                                }
                            }
                        });


                    }
             
                }
                catch(Exception emx)
                {
                    WriterMainFormLog(emx.Message);
                }
                return results;

            };
            ///提交预测数据到缓存
            TrainManager.ReceiveMachineTrainPredicte = (ScadaMachineTrainingForecast model) =>
            {
                if (model == null || string.IsNullOrEmpty(model.PredictedLabel))
                    return null;
                return TaskHelper.Factory.StartNew(() =>
                {

                    IO_COMMUNICATION comm = IOProject.Communications.Find(x => x.IO_SERVER_ID == model.SERVER_ID && x.IO_COMM_ID == model.COMM_ID);
                    if (model != null)
                        model.COMM_NAME = comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]";
                    IO_DEVICE device = IOProject.Devices.Find(x => x.IO_SERVER_ID == model.SERVER_ID && x.IO_COMM_ID == model.COMM_ID && x.IO_DEVICE_ID == model.DEVICE_ID);
                    if (device != null)
                        model.DEVICE_NAME = device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]";
                    string appName = IOCenterServer.GetAppName(model.SERVER_ID);
                  
                    //触发机器预测执行自动控制命令
                    IOCenterManager.BatchCommandTask.ExecuteMachineTrainTask(model.PredictedLabel,IOProject.MachineTrainingModels.Find(x => x.Id == model.TaskId && x.SERVER_ID == model.SERVER_ID));
                    //在监视器窗口显示预测信息

                });
            };
            ///保存机器训练成功与否的结果
            TrainManager.MachineTrainExecuteResult = (ScadaMachineTrainingModel model, bool result, string error) => {

                if (model == null)
                    return null;
                return TaskHelper.Factory.StartNew(() =>
                {
                    IO_COMMUNICATION comm = IOProject.Communications.Find(x => x.IO_SERVER_ID == model.SERVER_ID && x.IO_COMM_ID == model.COMM_ID);
                
                    IO_DEVICE device = IOProject.Devices.Find(x => x.IO_SERVER_ID == model.SERVER_ID && x.IO_COMM_ID == model.COMM_ID && x.IO_DEVICE_ID == model.DEVICE_ID);
              
                    string res = result ? "训练成功" : "训练失败";
                    //将执行日志增加到缓存
                    MachineTrainCacheObject trainCacheObject = new MachineTrainCacheObject()
                    {
                        AppName = IOCenterServer.GetAppName(model.SERVER_ID),
                        COMM_ID = comm != null ? comm.IO_COMM_ID : "",
                        COMM_NAME = comm != null ? comm.IO_COMM_NAME + "[" + comm.IO_COMM_LABEL + "]" : "",
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        DEVICE_ID = device != null ? device.IO_DEVICE_ID : "",
                        DEVICE_NAME = device != null ? device.IO_DEVICE_NAME + "[" + device.IO_DEVICE_LABLE + "]" : "",
                        Remark = error,
                        Result = res,
                        SERVER_ID = model.SERVER_ID,
                        TaskID = model.Id.ToString(),
                        TaskTitle = model.TaskName


                    };
                    IOCenterClient.RealCache.Push(trainCacheObject);

                });

            };
                    
            BatchCommandTask = new IOCenterBatchCommandTaskManager();
            BatchCommandTask.CommandItemRunResult = (BatchCommandTask batchTask, BatchCommandItem Item) => {

                return TaskHelper.Factory.StartNew(() => { });
            };
            BatchCommandTask.CommandRunErrorResult = (BatchCommandTask batchTask, BatchCommandItem Item,string error) => {

                return TaskHelper.Factory.StartNew(() => {
                    WriterMainFormLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 自动控制任务:" + batchTask.CommandTaskTitle + "下的"+ Item.CommandItemTitle + "命令异常错误 错误:" + error);
                });
            };
            BatchCommandTask.CommandTaskEnded = (BatchCommandTask batchTask) => {

                return TaskHelper.Factory.StartNew(() =>
                {

                    string tasktype = "";
                    switch (batchTask.TaskStartRunType)
                    {
                        case BatchCommandStartRunType.IOTriggerTask:
                            tasktype = "IO触发控制命令任务";
                            break;
                        case BatchCommandStartRunType.MachineTrainTask:
                            tasktype = "机器预测触发控制命令任务";
                            break;
                        case BatchCommandStartRunType.ManualTask:
                            tasktype = "手动执行控制命令任务";
                            break;
                        case BatchCommandStartRunType.TimedTask:
                            tasktype = "定时自动控制命令任务";
                            break;
                    }
                    BatchCommandCacheObject cacheObject = new BatchCommandCacheObject()
                    {
                        CreateTime = batchTask.CommandTaskCreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Remark = "",
                        Result = "执行成功",
                        SERVER_ID = batchTask.SERVER_ID,
                        TaskID = batchTask.CommandTaskID,
                        TaskTitle = batchTask.CommandTaskTitle,
                        TaskType = tasktype
                    };

                    cacheObject.AppName = IOCenterServer.GetAppName(cacheObject.SERVER_ID);
                    //向时序数据库写入操作日志
                    IOCenterServer.ServerForm.AddBatchCommand(cacheObject.AppName, cacheObject.CreateTime,
                        cacheObject.SERVER_ID, cacheObject.TaskTitle, cacheObject.TaskType, cacheObject.Result, cacheObject.Remark);

                    IOCenterClient.RealCache.Push(cacheObject);
                });
            };
            BatchCommandTask.CommandTaskError = (BatchCommandTask batchTask,string error) => {

                return TaskHelper.Factory.StartNew(() => {
                

                    string tasktype = "";
                    switch (batchTask.TaskStartRunType)
                    {
                        case BatchCommandStartRunType.IOTriggerTask:
                            tasktype = "IO触发控制命令任务";
                            break;
                        case BatchCommandStartRunType.MachineTrainTask:
                            tasktype = "机器预测触发控制命令任务";
                            break;
                        case BatchCommandStartRunType.ManualTask:
                            tasktype = "手动执行控制命令任务";
                            break;
                        case BatchCommandStartRunType.TimedTask:
                            tasktype = "定时自动控制命令任务";
                            break;
                    }
                    //向时序数据库写入操作日志
                    BatchCommandCacheObject cacheObject = new BatchCommandCacheObject()
                    {
                        CreateTime = batchTask.CommandTaskCreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Remark = error,
                        Result = "执行失败",
                        SERVER_ID = batchTask.SERVER_ID,
                        TaskID = batchTask.CommandTaskID,
                        TaskTitle = batchTask.CommandTaskTitle,
                        TaskType = tasktype
                    };
                    cacheObject.AppName = IOCenterServer.GetAppName(cacheObject.SERVER_ID);
                    IOCenterServer.ServerForm.AddBatchCommand(cacheObject.SERVER_ID, cacheObject.CreateTime,
                        cacheObject.SERVER_ID, cacheObject.TaskTitle, cacheObject.TaskType, cacheObject.Result, cacheObject.Remark);

                    IOCenterClient.RealCache.Push(cacheObject);
                });
            };
            BatchCommandTask.CommandTaskStarted = (BatchCommandTask batchTask) => {

                return TaskHelper.Factory.StartNew(() => {
                    WriterMainFormLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 自动控制任务:" + batchTask.CommandTaskTitle + "启动执行");
                });
            };
            BatchCommandTask.RunManager();
            BatchCommandTask.Start();
            TaskHelper.TaskRunException = (string exception) => {

                WriterMainFormLog(exception);
            };
            //设置系统环境变量
            ScadaEnvironment.SetVariable(AppDomain.CurrentDomain.BaseDirectory);
            ScadaEnvironment.SetVariable(IPAddressSelector.Instance().AddressIP + ":" + MDSConfig.MDSServerPort, "LAZY_IP");
            ScadaFilePermission.SetDBFilePermission(IOProject.DataBaseFileName);


        }
        private static void CheckDeviceStatus(object sender)
        {
            if (PublishRestart)
                return;
            IOCenterClient_OnRefreshMainTreeDeviceStatus(IOProject.Devices);
        }

        #region 工程浏览器日志
        /// <summary>
        /// IOProject日志写入
        /// </summary>
        /// <param name="log"></param>
        private static void IOProject_CenterServerLog(string log)
        {
            if (QueryFormManager != null)
            {
                QueryFormManager.AddLog(log);
            }
        }

        private static void IOProject_CenterServerException(string exmsg)
        {
            if (QueryFormManager != null)
            {
                QueryFormManager.DisplyException(exmsg);
            }
        }
        #endregion
        #region 监视器日志
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="log"></param>
        private static void ShowMonitorEvent(string application, string eventdate, string eventname, string server, string communication, string device, string msg)
        {
            if (IOCenterServer != null)
            {
                IOCenterServer.ServerForm.AddEvent(application, eventdate, eventname, server, communication, device, msg);
            }
        }
        private static void ShowMonitorFormLog(string stationTitle, string log)
        {
            QueryFormManager.AddLog(stationTitle + " " + log);
        }


        #endregion
        #region 监视器服务管理
        /// <summary>
        /// 当前服务器的状态
        /// </summary>
        public MDSServerStatus ServerStatus
        {
            get { return IOCenterServer.MDSServerStatus; }
        }
        /// <summary>
        /// 启动采集服务
        /// </summary>
        public static void StartServer()
        {


            if (IOCenterServer.Start())
            {
                if (QueryFormManager != null)
                {
                    QueryFormManager.AddLog("用户启动数据服务成功!");
                }
                IOCenterServer.ServerForm.ToolStripStatus.Text = "服务运行中......";
                IOCenterServer.ServerForm.ToolStripStatus.ForeColor = Color.DarkGreen;
                QueryFormManager.MainForm.ServerStarted();
            }
            else
            {

                if (QueryFormManager != null)
                {
                    QueryFormManager.AddLog("启动服务失败!");
                }
            }

            deviceStatusTimer = new System.Threading.Timer(new TimerCallback(CheckDeviceStatus), null, 50000, 30000);



        }

        public static void CloseServer()
        {

            if (IOCenterServer.Stop())
            {

                if (QueryFormManager != null)
                {
                    QueryFormManager.AddLog("用户停止数据服务成功!");
                }
                IOCenterServer.ServerForm.ToolStripStatus.Text = "服务已关闭";
                IOCenterServer.ServerForm.ToolStripStatus.ForeColor = Color.Red;
                QueryFormManager.MainForm.ServerStoped();
            }
            else
            {

                if (QueryFormManager != null)
                {
                    QueryFormManager.AddLog("用户停止数据服失败");
                }
            }
            if (deviceStatusTimer != null)
                deviceStatusTimer.Dispose();
            deviceStatusTimer = null;
        }
        #endregion

        public static void SendCommand(Controls.IOTree IOTreeView)
        {
            if (IOTreeView.SelectedNode == null)
            {
                FrmDialog.ShowDialog(QueryFormManager.MainForm, "请在IO树中选择设备节点");
                return;
            }
            if (IOTreeView.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode deviceNode = IOTreeView.SelectedNode as IoDeviceTreeNode;
                IoCommunicationTreeNode communicationTreeNode = IOTreeView.SelectedNode.Parent as IoCommunicationTreeNode;
                IoServerTreeNode serverNode = IOTreeView.SelectedNode.Parent.Parent as IoServerTreeNode;

                SendCommandForm sendCommandForm = new SendCommandForm();
                sendCommandForm.InitCommand(serverNode.Server, communicationTreeNode.Communication, deviceNode.Device);
                if (sendCommandForm.ShowDialog(QueryFormManager.MainForm) == DialogResult.OK)
                {

                }

            }
            else
            {
                FrmDialog.ShowDialog(QueryFormManager.MainForm, "请在IO树中选择设备节点");
                return;
            }
        }
        /// <summary>
        /// 下置一个命令
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static bool SendCommand(IO_COMMANDS Command)
        {
            try
            {

                Command.COMMAND_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Command.COMMAND_RESULT = "FALSE";
                Command.COMMAND_USER = IOCenterManager.IOProject.ServerConfig.User;


                MDSConfig MDSServerConfig = new MDSConfig();

                //下置命令
                byte[] datas = Encoding.UTF8.GetBytes(Command.GetCommandString());

                IOStationInfoItem station = MDSSettings.Instance.Stations.Find(x => x.PhysicalMAC.Trim().ToLower() == Command.IO_SERVER_ID.Trim().ToLower());
                if (station != null)
                {
                    var responseMessage = IOCenterManager.IOCenterClient.SendAndGetResponse(datas, Command.IO_SERVER_ID, MDSServerConfig.MonitorAppPrefix + "_" + station.PhysicalAddress.Replace(".", "").Replace("。", ""), MDSCommandType.下置命令, ScadaClientType.IoServer, Scada.MDSCore.Communication.Messages.MessageTransmitRules.StoreAndForward, 10000);
                    TcpData tcpData = new TcpData();
                    tcpData.BytesToTcpItem(SplitPackage.RemoveIdentificationBytes(responseMessage.MessageData).Datas);
                    Command.COMMAND_RESULT = tcpData.GetItemValue("COMMAND_RESULT");
                    //写入下置命令道实时数据库
                    IOCenterManager.InfluxDbManager.DbWrite_CommandPoint(Command.IO_SERVER_ID, Command.COMMAND_ID, Command, DateTime.Now);
                    //在主界面窗体中显示下置数据的日志
                    if (IOCenterManager.IOCenterServer.ServerForm != null && !IOCenterManager.IOCenterServer.ServerForm.IsDisposed)
                    {
                        IOCenterManager.IOCenterServer.ServerForm.AddCommand(responseMessage.SourceApplicationName, Command.IO_SERVER_ID, Command.IO_COMM_ID, Command.IO_DEVICE_ID, Command.IO_NAME, Command);
                    }
                }

                if (Command.COMMAND_RESULT == "FALSE")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception emx)
            {
                return false;
            }
        }
        public static void Close()
        {
            try
            {
                QueryFormManager.MainForm.WindowState = FormWindowState.Minimized;
                ServerForm.WindowState = FormWindowState.Minimized;
                IOCenterLoading frmLoading = new IOCenterLoading();
                frmLoading.BackgroundWorkAction = delegate ()
                {
                    try
                    {
                        if (deviceStatusTimer != null)
                            deviceStatusTimer.Dispose();
                        deviceStatusTimer = null;
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(0, "开始回收内存...");
                        MemoryCollect.Close();
                        IISExpressManager.Close();
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(10, "开始关闭时序数据库...");
                        if (QueryFormManager != null)
                        {
                            QueryFormManager.CloseInfluxDBServer();
                            QueryFormManager.Dispose();
                        }
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(20, "开始关闭数据中心通讯器...");

                        if (IOCenterClient != null)
                        {
                            IOCenterClient.Disconnect();
                            IOCenterClient.Dispose();
                            IOCenterClient = null;
                        }

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(40, "开始关闭服务...");
                        if (IOCenterServer != null)
                        {
                            IOCenterServer.Stop();
                            IOCenterServer.Dispose();
                            IOCenterServer = null;
                        }
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "关闭IO工程...");
                        if (IOProject != null)
                        {
                            IOProject.Dispose();
                            IOProject = null;
                        }
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(60, "关闭服务器状态检测器...");
                        if (ComputerStatusMonitor != null)
                            ComputerStatusMonitor.Close();
                        ComputerStatusMonitor = null;
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(70, "正在关闭自动控制任务...");

                        if (BatchCommandTask != null)
                        {
                            BatchCommandTask.Stop();
                            BatchCommandTask.Dispose();
                        }
                        
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(80, "正在关闭窗体");
                        try
                        {
                                Application.ExitThread();
                                QueryFormManager.MainForm.Close();
                            QueryFormManager.MainForm.Dispose();
                            ServerForm.Close();
                            ServerForm.Dispose();
                        }
                        catch
                        {

                        }
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(90, "释放机器训练模型数据");
                        if (TrainManager != null)
                            TrainManager.Dispose();
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "系统关闭完成");
                        Application.ExitThread();
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "SCADA数据中心启动失败!");
                        MessageBox.Show(frmLoading, "加载资源时出现错误 " + ex.Message);

                    }
                };

                frmLoading.ShowDialog();


            }
            catch
            {

            }

        }

    }
}
