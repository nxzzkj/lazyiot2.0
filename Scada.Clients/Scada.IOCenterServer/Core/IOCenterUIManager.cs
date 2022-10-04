using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.IOStructure;
using Scada.Logger;
using Scada.Model;
using ScadaCenterServer.Controls;
using ScadaCenterServer.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Temporal.DbAPI;
using Temporal.Net.InfluxDb.Models.Responses;


 
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
    public delegate void CenterServerExceptionHappened(string exmsg);
    public delegate void CenterServerLogHappened(string log);
    public class IOCenterUIManager : ScadaTask
    {


        public IOCenterMainForm MainForm = null;
        public Mediator Mediator = null;
        public IOTreeForm IOTreeForm
        { get { return Mediator.IOTreeForm; } }
        public void ServerStatus(Scada.Model.IO_SERVER server, bool status)
        {
            IOTreeForm.ServerStatus(server, status);
        }
        public void DeviceStatus(List<Scada.Model.IO_DEVICE> devices)
        {
            IOTreeForm.DeviceStatus( devices);
        }
        public void CommunicationStatus(Scada.Model.IO_COMMUNICATION comm, bool status)
        {
            IOTreeForm.CommunicationStatus( comm, status);
        }
        public event CenterServerExceptionHappened CenterServerException;
        public event CenterServerLogHappened CenterServerLog;
        public Random MainRandom = new Random();
        /// <summary>
        /// 当当前对象释放的时候
        /// </summary>
        public override void Dispose()
        {
            CloseInfluxDBServer();
            base.Dispose();

            MainRandom = null;
            InfluxDbManager = null;


        }
       
        /// <summary>
        /// 异常信息在日志端显示
        /// </summary>
        /// <param name="ex"></param>
        public void DisplyException(Exception ex)
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = true;
                }
            }
            if (CenterServerException != null)
            {
                CenterServerException(ex.Message);
            }
            Scada.Logger.Logger.GetInstance().Debug(ex.Message);
        }
        public void DisplyException(string exmsg)
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = true;
                }
            }
            if (CenterServerException != null)
            {
                CenterServerException(exmsg);
            }
            Scada.Logger.Logger.GetInstance().Debug(exmsg);
        }
        public void AddLog(string log)
        {
            if (CenterServerLog != null)
            {
                CenterServerLog(log);
            }
            Scada.Logger.Logger.GetInstance().Info(log);
        }
        /// <summary>
        /// 初始化窗体任务
        /// </summary>
        public void InitQueryForm(IOCenterMainForm form)
        {
            MainForm = form;
            //加载数据中心数据库Sqlit
            Mediator = new Mediator(form);
            Mediator.DockPanel = form.dockPanel;
            Mediator.OpenIOPropeitesForm();
            Mediator.OpenIOTreeForm();
            Mediator.OpenIOTaskForm();
            Mediator.OpenOperatorLogForm();
            CenterServerException += IOCenterServerFormManager_CenterServerException;
            CenterServerLog += IOCenterServerFormManager_CenterServerLog;

        }


        #region 日志和错误异常
        private void IOCenterServerFormManager_CenterServerLog(string log)
        {
            Mediator.OperatorLogForm.AppendLogItem(log);
        }

        private void IOCenterServerFormManager_CenterServerException(string exmsg)
        {
            Mediator.OperatorLogForm.AppendLogItem(exmsg);
            Logger.GetInstance().Debug(exmsg);
        }
        #endregion
        #region 时序列数据库的管理
        public Process influxdbApplication;

        public InfluxDbManager InfluxDbManager = null;
        private void InstallInfluxDB()
        {
            string str = System.Windows.Forms.Application.StartupPath + "\\influxdb\\influxdbstart.bat";

            string strDirPath = System.IO.Path.GetDirectoryName(str);
            string strFilePath = System.IO.Path.GetFileName(str);

            string targetDir = string.Format(strDirPath);//this is where mybatch.bat lies
            influxdbApplication = new Process();
            influxdbApplication.StartInfo.WorkingDirectory = targetDir;
            influxdbApplication.StartInfo.FileName = strFilePath;

            influxdbApplication.StartInfo.CreateNoWindow = true;
            influxdbApplication.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            influxdbApplication.Start();
        }
        //启动实时数据库
        public void StartInfluxDBServer()
        {
            if (IOCenterManager.IOProject.ServerConfig == null)
                return;

                try
                {
                Process[] influx = Process.GetProcessesByName("influxd");
                if (influx.Length > 0)
                {

                    influx[0].Kill();
                }
                AddLog("正在启动时序数据库服务.....");

                    //安装实时数据库

                    InstallInfluxDB();

                    AddLog("时序数据库服务已经启动完成");
              
                    InfluxDbManager = new InfluxDbManager(IOCenterManager.IOProject.ServerConfig.influxdConfig.HttpAddress, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.DataBaseName, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.User, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.Password, IOCenterManager.IOProject.ServerConfig.InfluxDBGlobal.InfluxDBVersion);
                    //此处要判断实时数据库是否已经启动，如果没有启动一直等待数据库服务器启动
                    InfluxDbManager.InfluxException += InfluxDbManager_InfluxException;

                    int num = 0;
                    while (true)
                    {
                        if (num > 6)
                        {
                            break;
                        }
                        AddLog("正在启动实时数据服务......" + (num + 1) + "秒");
                        Thread.Sleep(1000);
                        num++;
                    }
                    while (true)
                    {
                        Process[] influxs = Process.GetProcessesByName("influxd");
                        if (influxs.Length > 0)
                        {

                            break;
                        }
                        AddLog("正在启动实时数据服务......");
                        //每1秒循环判断一次实时数据库是否在启动中
                        Thread.Sleep(1000);
                    }
                    AddLog("SCADA实时数据库准备就绪!");
                    //连接实时数据库
                    AddLog("准备连接SCADA实时数据库......");
                    InfluxDbManager.ShouldConnectInfluxDb();
                    AddLog("连接SCADA实时数据库已经完成!");
         
                    //监视实时数据库状态，如果实时数据库因为某些原因被关闭，则要求及时自动将实时数据库重新驱动
                    AddLog("启动监视服务.......!");
                    MonitorInfluxDB();
            
                    AddLog("启动监视服务成功!");
                IsClose = false;
            }
                catch (Exception ex)
                {
                    DisplyException(ex);
                    AddLog("系统服务启动失败，请尝试重新启动服务!");

                }
         
        }
        /// <summary>
        /// influx数据库报错异常返回
        /// </summary>
        /// <param name="ex"></param>
        private void InfluxDbManager_InfluxException(Exception ex)
        {
            DisplyException(ex);
        }
        private bool IsClose = true;
        //关闭时序数据库
        public void CloseInfluxDBServer()
        {

            try
            {
                IsClose = true;
                AddLog("正在关闭时序数据库服务.....");
                if (influxdbApplication != null)
                {
                    influxdbApplication.Kill();
                }

                Process[] influx = Process.GetProcessesByName("influxd");
                for (int i = 0; i < influx.Length; i++)
                {
                    influx[i].CloseMainWindow();
                    influx[i].Kill();
                }
                if (influxdbApplication != null)
                {
                    influxdbApplication.Close();
                    influxdbApplication.Dispose();
                    influxdbApplication = null;
                }

                AddLog("时序数据库服务已经关闭");
            }
            catch (Exception ex)
            {
                DisplyException(ex);
            }

        }
        //监视influxdb数据库，如果数据库被退出了，则要重新启动
        private void MonitorInfluxDB()
        {
            var influxTask = TaskHelper.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (IsClose)
                        break;
                    Process[] influx = Process.GetProcessesByName("influxd");
                    if (influx.Length <= 0)
                    {
                        AddLog("时序数据库服务未知原因而关闭");
                        CloseInfluxDBServer();
                        AddLog("准备重新启动时序数据库服务......");
                        Thread.Sleep(2000);
                        StartInfluxDBServer();

                        break;
                    }
                    //每10秒循环判断一次实时数据库是否在启动中
                    Thread.Sleep(30000);
                }
            });

        }
        #endregion
        #region 主窗体进度条
        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="MaxValue"></param>
        public void InitProgress(int MaxValue)
        {

            MainForm.ProgressBar.Value = 0;
            MainForm.ProgressBar.Maximum = MaxValue;
            MainForm.ProgressBar.Text = "";
        }
        public void EndProgress()
        {

            MainForm.ProgressBar.Value = 0;
            MainForm.ProgressBar.Maximum = 100;
            MainForm.ProgressBar.Text = "";
        }
        /// <summary>
        /// 进度
        /// </summary>
        public void SetProgress()
        {

            if (MainForm.ProgressBar.Value + 1 == MainForm.ProgressBar.Maximum)
            {
                MainForm.ProgressBar.Value++;
                EndProgress();
                MainForm.ProgressBar.Text = "任务完成";
            }
            else
            {
                MainForm.ProgressBar.Value++;
                MainForm.ProgressBar.Text = (MainForm.ProgressBar.Value * 100.0f / MainForm.ProgressBar.Maximum).ToString("0.0");
            }

        }
        #endregion
        #region IO树操作管理
        private Task  LoadQueryTreeProject(TreeView tree)
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                try
                {
                    TreeNode mainNode = new TreeNode();
                    int num = IOCenterManager.IOProject.Servers.Count + IOCenterManager.IOProject.Communications.Count + IOCenterManager.IOProject.Devices.Count;

                    mainNode.ImageIndex = 0;
                    mainNode.SelectedImageIndex = 0;
                    mainNode.Text = PubConstant.Product;
                    InitProgress(num);
                    ///加载采集站
                    for (int i = 0; i < IOCenterManager.IOProject.Servers.Count; i++)
                    {

                        IoServerTreeNode serverNode = new IoServerTreeNode();
                        serverNode.Server = IOCenterManager.IOProject.Servers[i];
                        serverNode.InitNode();
                        AddLog("创建采集站" + serverNode.Server.SERVER_ID+"树节点");
                        List<Scada.Model.IO_COMMUNICATION> serverComms = IOCenterManager.IOProject.Communications.FindAll(x => x.IO_SERVER_ID == IOCenterManager.IOProject.Servers[i].SERVER_ID);
                        for (int c = 0; c < serverComms.Count; c++)//通道
                        {
                            IoCommunicationTreeNode commNode = new IoCommunicationTreeNode();
                            commNode.Communication = serverComms[c];
                            commNode.Server = IOCenterManager.IOProject.Servers[i];
                            commNode.InitNode();
                            AddLog("创建采集站"+ serverComms[c].IO_SERVER_ID+"下的通道" + commNode.Name + "树节点");
                            List<Scada.Model.IO_DEVICE> commDevices = IOCenterManager.IOProject.Devices.FindAll(x => x.IO_SERVER_ID == IOCenterManager.IOProject.Servers[i].SERVER_ID && x.IO_COMM_ID == serverComms[c].IO_COMM_ID);
                            for (int d = 0; d < commDevices.Count; d++)//设备
                            {
                                IoDeviceTreeNode deviceNode = new IoDeviceTreeNode();
                                deviceNode.Device = commDevices[d];
                                deviceNode.Server = IOCenterManager.IOProject.Servers[i];
                                deviceNode.Communication = serverComms[c];
                                //挂载右键菜单
                                deviceNode.ContextMenuStrip = Mediator.IOTreeForm.contextMenuStrip;
                                deviceNode.InitNode();
                                commNode.Nodes.Add(deviceNode);
                                AddLog("创建采集站" + serverComms[c].IO_SERVER_ID + "下的" + commNode.Name +"通道设备" + deviceNode.Name + "树节点");
                                SetProgress();
                            }
                            SetProgress();
                            serverNode.Nodes.Add(commNode);
                        }

                        mainNode.Nodes.Add(serverNode);
                        SetProgress();
                    }
                    tree.BeginInvoke(new EventHandler(delegate
                    {
                        mainNode.ExpandAll();
                        tree.BeginUpdate();
                        tree.Nodes.Clear();
                        tree.Nodes.Add(mainNode);
                        EndProgress();
                        tree.EndUpdate();
                        AddLog("加载采集站目录树完成");
                    }));
                }
                catch (Exception exm)
                {
                    DisplyException(exm);
                    EndProgress();
                }

            });
        }

        private Task LoadQueryMachineTreeProject(TreeView tree)
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                try
                {
                    TreeNode mainNode = new TreeNode();
                   
                    mainNode.ImageIndex = 0;
                    mainNode.SelectedImageIndex = 0;
                    mainNode.Text = "机器训练模型任务";
                 
                    ///加载采集站
                    for (int i = 0; i < IOCenterManager.IOProject.Servers.Count; i++)
                    {

                        TaskServerNode serverNode = new TaskServerNode();
                        serverNode.Server = IOCenterManager.IOProject.Servers[i];
                        serverNode.InitNode();
                     
                        List<Scada.Model.ScadaMachineTrainingModel> trainComms = IOCenterManager.IOProject.MachineTrainingModels.FindAll(x => x.SERVER_ID == IOCenterManager.IOProject.Servers[i].SERVER_ID);
                        for (int c = 0; c < trainComms.Count; c++)//通道
                        {
                            MachineTrainTreeNode trainNode = new MachineTrainTreeNode();
                            trainNode.MachineTrainingModel = trainComms[c];
                            trainNode.InitNode();
            
                          
                            serverNode.Nodes.Add(trainNode);
                        }

                        mainNode.Nodes.Add(serverNode);
                  
                    }
                    tree.BeginInvoke(new EventHandler(delegate
                    {
                        mainNode.ExpandAll();
                        tree.BeginUpdate();
                        tree.Nodes.Clear();
                        tree.Nodes.Add(mainNode);
                        EndProgress();
                        tree.EndUpdate();
                    
                    }));
                }
                catch (Exception exm)
                {
                    DisplyException(exm);
            
                }

            });
        }
        private Task LoadQueryBatchCommandTaskTreeProject(TreeView tree)
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                try
                {
                    TreeNode mainNode = new TreeNode();

                    mainNode.ImageIndex = 1;
                    mainNode.SelectedImageIndex = 1;
                    mainNode.Text = "自动控制流程任务";

                    ///加载采集站
                    for (int i = 0; i < IOCenterManager.IOProject.Servers.Count; i++)
                    {

                        TaskServerNode serverNode = new TaskServerNode();
                        serverNode.Server = IOCenterManager.IOProject.Servers[i];
                        serverNode.InitNode();

                        List<Scada.Model.BatchCommandTaskModel> batchComms = IOCenterManager.IOProject.BatchCommandTasks.FindAll(x => x.SERVER_ID == IOCenterManager.IOProject.Servers[i].SERVER_ID);
                        for (int c = 0; c < batchComms.Count; c++)//通道
                        {
                            BatchCommandTreeNode batchNode = new BatchCommandTreeNode();
                            batchNode.BatchCommandTask = batchComms[c];
                            batchNode.InitNode();
                            batchComms[c].Items.ForEach(delegate (Scada.Model.BatchCommandTaskItemModel item) {

                                BatchCommandItemTaskNode itemNode = new BatchCommandItemTaskNode();
                                itemNode.BatchCommandTaskItem = item;

                                batchNode.Nodes.Add(itemNode);
                            });
                            serverNode.Nodes.Add(batchNode);
                        }

                        mainNode.Nodes.Add(serverNode);

                    }
                    tree.BeginInvoke(new EventHandler(delegate
                    {
                        mainNode.ExpandAll();
                        tree.BeginUpdate();
                        tree.Nodes.Clear();
                        tree.Nodes.Add(mainNode);
                        EndProgress();
                        tree.EndUpdate();

                    }));
                }
                catch (Exception exm)
                {
                    DisplyException(exm);

                }

            });
        }

        public Task LoadQueryTreeProject(TreeView tree, string ServerID)
        {
            return TaskHelper.Factory.StartNew(() =>
            {
                IoServerTreeNode serverNode = new IoServerTreeNode();
                IO_SERVER server = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID == ServerID);
                if (server != null)
                {
                
                    serverNode.Server = server;
                    serverNode.InitNode();
                    AddLog("创建新采集站节点");
                  
                    List<Scada.Model.IO_COMMUNICATION> serverComms = IOCenterManager.IOProject.Communications.FindAll(x => x.IO_SERVER_ID == server.SERVER_ID);
                    for (int c = 0; c < serverComms.Count; c++)//通道
                    {
                        IoCommunicationTreeNode commNode = new IoCommunicationTreeNode();
                        commNode.Communication = serverComms[c];
                        commNode.Server = server;
                        commNode.InitNode();
                        AddLog("创建新通道" + serverComms[c].IO_COMM_NAME + "节点");
                        List<Scada.Model.IO_DEVICE> commDevices = IOCenterManager.IOProject.Devices.FindAll(x => x.IO_SERVER_ID == server.SERVER_ID && x.IO_COMM_ID == serverComms[c].IO_COMM_ID);
                        for (int d = 0; d < commDevices.Count; d++)//设备
                        {
                           
                            IoDeviceTreeNode deviceNode = new IoDeviceTreeNode();
                            deviceNode.Device = commDevices[d];
                            deviceNode.Server = server;
                            deviceNode.Communication = serverComms[c];
                            //挂载右键菜单
                            deviceNode.ContextMenuStrip = Mediator.IOTreeForm.contextMenuStrip;
                            deviceNode.InitNode();
                            commNode.Nodes.Add(deviceNode);
                            AddLog("创建新通道下网关设备" + deviceNode.Name + "节点");

                        }

                        serverNode.Nodes.Add(commNode);
                    }
                    serverNode.ExpandAll();
                }
                try
                {
                    tree.BeginInvoke(new EventHandler(delegate
                    {
                    
                        tree.BeginUpdate();
                        //删除指定的server节点
                        tree.Nodes[0].Nodes.RemoveByKey(ServerID);
                        //增加新的发布节点
                        tree.Nodes[0].Nodes.Add(serverNode);
                        tree.EndUpdate();
                        AddLog("加载" + ServerID + "采集站工程成功");

                    }));

                }
                catch (Exception exm)
                {
                    DisplyException(exm);
                    EndProgress();
                }
            });
        }
        public Task LoadQueryMachineTreeTask(TreeView tree, string ServerID)
        {
            return TaskHelper.Factory.StartNew(() =>
            {
            
                IO_SERVER server = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID == ServerID);
                #region 加载机器训练模型
                {


                    TaskServerNode serverTrainNode = new TaskServerNode();
                    if (server != null)
                    {

                        serverTrainNode.Server = server;
                        serverTrainNode.InitNode();


                        List<Scada.Model.ScadaMachineTrainingModel> serverMachineTrasinModels = IOCenterManager.IOProject.MachineTrainingModels.FindAll(x => x.SERVER_ID == server.SERVER_ID);
                        for (int c = 0; c < serverMachineTrasinModels.Count; c++)//通道
                        {
                            MachineTrainTreeNode trainNode = new MachineTrainTreeNode();
                            trainNode.MachineTrainingModel = serverMachineTrasinModels[c];

                            trainNode.InitNode();


                            serverTrainNode.Nodes.Add(trainNode);
                        }
                        serverTrainNode.ExpandAll();
                    }
                    try
                    {
                        tree.BeginInvoke(new EventHandler(delegate
                        {

                            tree.BeginUpdate();
                        //删除指定的server节点
                        tree.Nodes[0].Nodes.RemoveByKey(ServerID);
                        //增加新的发布节点
                        tree.Nodes[0].Nodes.Add(serverTrainNode);
                            tree.EndUpdate();


                        }));

                    }
                    catch (Exception exm)
                    {
                        DisplyException(exm);
                        EndProgress();
                    }
                }
                #endregion
                
            });
        }
        public Task LoadQueryBatchCommandTaskTreeTask(TreeView tree, string ServerID)
        {
            return TaskHelper.Factory.StartNew(() =>
            {

                IO_SERVER server = IOCenterManager.IOProject.Servers.Find(x => x.SERVER_ID == ServerID);
                #region 加载自动控制任务模型
                {


                    TaskServerNode serverBatchNode = new TaskServerNode();
                    if (server != null)
                    {

                        serverBatchNode.Server = server;
                        serverBatchNode.InitNode();


                        List<Scada.Model.BatchCommandTaskModel>  batchCommandTaskModels = IOCenterManager.IOProject.BatchCommandTasks.FindAll(x => x.SERVER_ID == server.SERVER_ID);
                        for (int c = 0; c < batchCommandTaskModels.Count; c++)//通道
                        {
                            BatchCommandTreeNode taskNode = new BatchCommandTreeNode();
                            taskNode.BatchCommandTask = batchCommandTaskModels[c];
                            taskNode.InitNode();
                            batchCommandTaskModels[c].Items.ForEach(delegate (Scada.Model.BatchCommandTaskItemModel item) {

                                BatchCommandItemTaskNode batchCommandItemTaskNode = new BatchCommandItemTaskNode();
                                batchCommandItemTaskNode.BatchCommandTaskItem = item;
                                batchCommandItemTaskNode.InitNode();
                                taskNode.Nodes.Add(batchCommandItemTaskNode);
                            });
                            serverBatchNode.Nodes.Add(taskNode);
                        }
                        serverBatchNode.ExpandAll();
                    }
                    try
                    {
                        tree.BeginInvoke(new EventHandler(delegate
                        {

                            tree.BeginUpdate();
                            //删除指定的server节点
                            tree.Nodes[0].Nodes.RemoveByKey(ServerID);
                            //增加新的发布节点
                            tree.Nodes[0].Nodes.Add(serverBatchNode);
                            tree.EndUpdate();


                        }));

                    }
                    catch (Exception exm)
                    {
                        DisplyException(exm);
                        EndProgress();
                    }
                }
                #endregion

            });
        }


        public Task LoadQueryTreeProject()
        {
            
               return LoadQueryTreeProject(Mediator.IOTreeForm.ioTree);
        
        }
        public Task LoadQueryMachineTreeProject()
        {

            return LoadQueryMachineTreeProject(Mediator.IOTaskForm.MachineTrainTaskTree);

        }
        public Task LoadQueryBatchCommandTaskTreeProject()
        {

            return LoadQueryBatchCommandTaskTreeProject(Mediator.IOTaskForm.BatchCommandTaskTree);

        }
        /// <summary>
        /// 加载指定的Server工程
        /// </summary>
        /// <param name="ServerID"></param>
        public Task LoadQueryTreeProject(string ServerID)
        {
             
             return   LoadQueryTreeProject(Mediator.IOTreeForm.ioTree, ServerID);
          
        }
        public Task LoadQueryMachineTreeTask(string ServerID)
        {

            return LoadQueryMachineTreeTask(Mediator.IOTaskForm.MachineTrainTaskTree, ServerID);

        }
        public Task LoadQueryBatchCommandTaskTreeTask(string ServerID)
        {

            return LoadQueryBatchCommandTaskTreeTask(Mediator.IOTaskForm.BatchCommandTaskTree, ServerID);

        }
        #endregion
        #region 读取某个设备的实时值
        public async void ReadRealDevice(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device)
        {
            try
            {
                if (InfluxDbManager != null)
                {
                    var result = await InfluxDbManager.DbQuery_Real(server, communication, device);
                    if (result != null && result.Count() > 0)
                    {
                        Serie s = result.Last();
                        if (s != null && s.Values.Count > 0)
                        {
                            for (int i = 0; i < device.IOParas.Count; i++)
                            {
                                device.IOParas[i].IORealData = new Scada.IOStructure.IOData();
                            }
                            var values = s.Values[s.Values.Count - 1];
                            for (int i = 0; i < s.Columns.Count; i++)
                            {
                                if (s.Columns[i].Split('_')[0].ToLower() == "field")
                                {
                                    string paraname = s.Columns[i].Replace("field_", "").Replace("_value", "").Replace("_datetime", "").Replace("_qualitystamp", "");
                                    Scada.Model.IO_PARA para = device.IOParas.Find(x => x.IO_NAME.Trim().ToLower() == paraname.ToLower());
                                    if (para != null)
                                    {
                                        int length = s.Columns[i].Split('_').Length;
                                        if (s.Columns[i].Split('_')[length - 1].ToLower() == "value")
                                        {
                                            para.IORealData.ParaValue = InfluxDbManager.GetInfluxdbValue(values[i]).ToString();
                                        }
                                        if (s.Columns[i].Split('_')[length - 1].ToLower() == "datetime")
                                        {
                                            para.IORealData.Date = Convert.ToDateTime(InfluxDbManager.GetInfluxdbValue(values[i]).ToString());
                                        }
                                        if (s.Columns[i].Split('_')[length - 1].ToLower() == "qualitystamp")
                                        {
                                            para.IORealData.QualityStamp = (QualityStamp)Enum.Parse(typeof(QualityStamp), InfluxDbManager.GetInfluxdbValue(values[i]).ToString());
                                        }
                                    }
                                }

                            }


                        }


                    }

                }
            }
            catch (Exception emx)
            {
                AddLog("ERROR=50001" + emx.Message);
            }


        }

        #endregion
        #region 读取某个设备的指定时间段的历史数据
        public async Task<InfluxDBHistoryData> ReadHistoryDevice(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_History(server, communication, device, SDate, EDate, PageSize, PageIndex);

                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个设备的指定时间段的统计数据
        public async Task<InfluxDBHistoryData> ReadHistoryStaticsDevice(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex, string selected, string timespan)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_RealDataHistoryStatics(server, communication, device, SDate, EDate, PageSize, PageIndex, selected, timespan);

                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个设备的指定时间段的报警
        public async Task<InfluxDBHistoryData> ReadHistoryAlarmDevice(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, DateTime SDate, DateTime EDate, string AlarmType, string AlarmLevel, int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_Alarms(server, communication, device, SDate, EDate, AlarmType, AlarmLevel, PageSize, PageIndex);

                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个设备的指定时间段的下置结果
        public async Task<InfluxDBHistoryData> ReadHistoryCommandsDevice(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_Commands(server, communication, device, SDate, EDate, PageSize, PageIndex);

                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个设备的指定时间段的用户修改报警配置的信息
        public async Task<InfluxDBHistoryData> ReadHistoryAlarmConfigsDevice(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_AlarmConfigs(server, communication, device, SDate, EDate, PageSize, PageIndex);

                return result;

            }
            return null;

        }

        #endregion
        #region 读取数据库备份日志
        public async Task<InfluxDBHistoryData> ReadBackupHistory(int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_Backup(PageSize, PageIndex);

                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个设备的指定时间段的事件
        public async Task<InfluxDBHistoryData> ReadHistoryEvent(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, DateTime SDate, DateTime EDate, string EventType,int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_Events(server, communication, device, SDate, EDate, EventType, PageSize, PageIndex);
                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个任务的指定时间段的执行记录
        public async Task<InfluxDBHistoryData> ReadHistoryMachineTrain(Scada.Model.IO_SERVER server, Scada.Model.ScadaMachineTrainingModel train,   DateTime SDate, DateTime EDate,  int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_MachineTrain(train, SDate, EDate, PageSize, PageIndex);
                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个任务的指定时间段的预测数据
        public async Task<InfluxDBHistoryData> ReadHistoryMachineTrainForeast(Scada.Model.IO_SERVER server, Scada.Model.ScadaMachineTrainingModel train, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_MachineTrainForeast(train, SDate, EDate, PageSize, PageIndex);
                return result;

            }
            return null;

        }

        #endregion
        #region 读取某个命令任务的指定时间段的执行记录
        public async Task<InfluxDBHistoryData> ReadHistoryBatchCommandTask(Scada.Model.IO_SERVER server, Scada.Model.BatchCommandTaskModel commandTask, DateTime SDate, DateTime EDate, int PageSize, int PageIndex)
        {
            if (InfluxDbManager != null)
            {
                var result = await InfluxDbManager.DbQuery_BatchCommandTask(commandTask, SDate, EDate, PageSize, PageIndex);
                return result;

            }
            return null;

        }

        #endregion
    }
}
