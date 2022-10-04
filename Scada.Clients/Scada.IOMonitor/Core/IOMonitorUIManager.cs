using Scada.Business;

using Scada.Controls;
using Scada.Controls.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;
using IOMonitor.Controls;
using IOMonitor.Forms;
using System.Windows.Threading;
using Scada.Controls.Forms;
using System.Diagnostics;
using Scada.Model;


 
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
    /// <summary>
    /// 系统管理类
    /// </summary>
    public abstract class IOMonitorUIManager
    {/// <summary>
     /// 模拟器
     /// </summary>


        #region 属性
        private static MonitorForm mForm = null;
        /// <summary>
        /// 系统服务
        /// </summary>
        public static IOMonitorManager MonitorManager = null;
        public static MonitorForm MainForm
        {
            set
            {
                mForm = value;
                mediator = value.mediator;

            }
            get { return mForm; }
        }

        public static Mediator mediator = null;


        #endregion
        #region 初始化窗体信息
        public static void InitMonitorMainForm(MonitorForm form)
        {
            ControlHelper.FreezeControl(form, true);
            MainForm = form;
            mediator = new Mediator(form);
            mediator.DockPanel = form.dockPanel;
            mediator.parent = form;
            mediator = new Mediator(form);
            mediator.DockPanel = form.dockPanel;

            //读取发布工程的数据库          
            form.WindowState = FormWindowState.Maximized;
            form.FormClosed += Form_FormClosed;
            mediator.OpenLogForm();

            mediator.OpenIOStatusForm();
            mediator.OpenIOPropertiesForm();
            mediator.OpenIOMonitorForm();

            //加载IO树
            mediator.IOStatusForm.LoadTreeStatus();

            //开启日志功能
            Scada.Logger.Logger.GetInstance().Run();
            //将系统监视的事件和日志输出到日志窗体


            IOMonitorProjectManager.OnDataBaseExceptionHanped += Monitor_ExceptionHanped;
            IOMonitorProjectManager.OnDataBaseLoged += IOMonitor_MakeLog;

        }

        #region 日志输出到主日志界面上

        private static void Monitor_ExceptionHanped(Exception ex)
        {
            AppendLogItem(ex.Message);

        }

        private static void IOMonitor_MakeLog(string msg)
        {
            AppendLogItem(msg);

        }
        #endregion

        private static void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            ApplicationExit();
        }




        
        public static void RefreshDeviceTreeNodeStatus(List<Scada.Model.IO_DEVICE> devices)
        {
            if (devices == null|| devices.Count<=0)
                return;
            if (mediator.IOStatusForm.IsHandleCreated)
            {
               

                    foreach (Scada.Model.IO_DEVICE device in devices)
                    {
                        TreeNode[] ioDeviceNode =mediator.IOStatusForm.IoTreeStatus.Nodes.Find(device.IO_DEVICE_ID, true);
                        if (ioDeviceNode != null && ioDeviceNode.Length > 0)
                        {if (ioDeviceNode[0] is IoDeviceTreeNode deviceNode)
                            {
                                 
                                deviceNode.ChangedStatus();
                            }
                           
                        }

                    }

                 
            }
         
        }
        public static void RefreshCommunicationTreeNodeStatus(Scada.Model.IO_COMMUNICATION comm)
        {
            if (comm == null)
                return;
            if (mediator.IOStatusForm.IsHandleCreated)
            {
                mediator.IOStatusForm.IoTreeStatus.BeginInvoke(new EventHandler(delegate
                {
                    if (mediator.IOStatusForm.IoTreeStatus.Nodes.Find(comm.IO_COMM_ID, true).Length > 0)
                    {
                        IoCommunicationTreeNode treeNode = (IoCommunicationTreeNode)mediator.IOStatusForm.IoTreeStatus.Nodes.Find(comm.IO_COMM_ID, true).First();
                        treeNode.ChangedStatus(comm.IO_COMM_STATUS == 1 ? true:false) ;
                       
                    }
                    
                }));
            }
        }

        //功能树点击事件

        #endregion
        #region 异常处理
        /// <summary>
        /// 异常信息在日志端显示
        /// </summary>
        /// <param name="ex"></param>
        public static void DisplyException(Exception ex)
        {

            Scada.Logger.Logger.GetInstance().Debug(ex.Message);
        }


        #endregion
        #region 主窗体进度条
        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="MaxValue"></param>
        public static void InitProgress(int MaxValue)
        {

            MainForm.ProgressBar.Value = 0;
            MainForm.ProgressBar.Maximum = MaxValue;
            MainForm.ProgressBar.Text = "";
        }
        public static void EndProgress()
        {

            MainForm.ProgressBar.Value = 0;
            MainForm.ProgressBar.Maximum = 100;
            MainForm.ProgressBar.Text = "";
        }
        /// <summary>
        /// 进度
        /// </summary>
        public static void SetProgress()
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
        #region 发布工程管理

        public static void LoadProject(TreeView tree)
        {



            AppendLogItem("正在加载采集站工程......");
            IOMonitorProjectManager.InitBaseModel();
            AppendLogItem("正在加载驱动信息......");
            IOMonitorManager.InitMonitor();
            AppendLogItem("正在加载IO树......");

            try
            {
                if (tree.Parent.IsHandleCreated)
                {
                    int num = IOMonitorProjectManager.IOCommunications.Count * IOMonitorProjectManager.IODevices.Count;
                    InitProgress(num);
                    IoServerTreeNode serverNode = new IoServerTreeNode();
                    serverNode.Server = IOMonitorProjectManager.IOServer;
                    serverNode.InitNode();
                    List<Scada.Model.IO_COMMUNICATION> serverComms = IOMonitorProjectManager.IOCommunications.FindAll(x => x.IO_SERVER_ID == IOMonitorProjectManager.IOServer.SERVER_ID);
                    for (int c = 0; c < serverComms.Count; c++)//通道
                    {
                        IoCommunicationTreeNode commNode = new IoCommunicationTreeNode();
                        commNode.Communication = serverComms[c];
                        commNode.Server = IOMonitorProjectManager.IOServer;
                        commNode.InitNode();
                        List<Scada.Model.IO_DEVICE> commDevices = IOMonitorProjectManager.IODevices.FindAll(x => x.IO_SERVER_ID == IOMonitorProjectManager.IOServer.SERVER_ID && x.IO_COMM_ID == serverComms[c].IO_COMM_ID);
                        for (int d = 0; d < commDevices.Count; d++)//设备
                        {
                            IoDeviceTreeNode deviceNode = new IoDeviceTreeNode();
                            deviceNode.Device = commDevices[d];
                            deviceNode.Server = IOMonitorProjectManager.IOServer;
                            deviceNode.Communication = serverComms[c];
                            deviceNode.InitNode();
                            commNode.Nodes.Add(deviceNode);
                            SetProgress();
                        }
                        SetProgress();
                        serverNode.Nodes.Add(commNode);
                    }


                    serverNode.Expand();

                    EndProgress();

                    tree.BeginInvoke(new EventHandler(delegate
                    {


                        tree.BeginUpdate();
                        tree.Nodes.Clear();
                        tree.Nodes.Add(serverNode);
                        tree.EndUpdate();

                    }));

                }

            }
            catch (Exception exm)
            {
                DisplyException(exm);
                EndProgress();
            }
        }

        public static void LoadSimulatorTree(TreeView tree)
        {





            TaskHelper.Factory.StartNew(() =>
            {
                tree.BeginUpdate();
                try
                {
                    if (tree.Parent.IsHandleCreated)
                    {

                        tree.BeginInvoke(new EventHandler(delegate
                        {



                            tree.Nodes.Clear();

                            int num = IOMonitorProjectManager.IOCommunications.Count * IOMonitorProjectManager.IODevices.Count;
                            InitProgress(num);
                            IoServerTreeNode serverNode = new IoServerTreeNode();
                            serverNode.Server = IOMonitorProjectManager.IOServer;
                            serverNode.InitNode();
                            List<Scada.Model.IO_COMMUNICATION> serverComms = IOMonitorProjectManager.IOCommunications.FindAll(x => x.IO_SERVER_ID == IOMonitorProjectManager.IOServer.SERVER_ID);
                            for (int c = 0; c < serverComms.Count; c++)//通道
                            {
                                IoCommunicationTreeNode commNode = new IoCommunicationTreeNode();
                                commNode.Communication = serverComms[c];
                                commNode.Server = IOMonitorProjectManager.IOServer;
                                commNode.InitNode();
                                commNode.Checked = serverComms[c].SimulatorCheckedStatus;

                                List<Scada.Model.IO_DEVICE> commDevices = IOMonitorProjectManager.IODevices.FindAll(x => x.IO_SERVER_ID == IOMonitorProjectManager.IOServer.SERVER_ID && x.IO_COMM_ID == serverComms[c].IO_COMM_ID);
                                for (int d = 0; d < commDevices.Count; d++)//设备
                                {
                                    IoDeviceTreeNode deviceNode = new IoDeviceTreeNode();
                                    deviceNode.Device = commDevices[d];
                                    deviceNode.Server = IOMonitorProjectManager.IOServer;
                                    deviceNode.Communication = serverComms[c];
                                    deviceNode.InitNode();
                                    deviceNode.Checked = commDevices[d].SimulatorCheckedStatus;
                                    commNode.Nodes.Add(deviceNode);
                                    SetProgress();
                                }
                                SetProgress();
                                serverNode.Nodes.Add(commNode);
                            }


                            serverNode.Expand();

                            EndProgress();
                            tree.Nodes.Add(serverNode);


                        }));
                    }

                }
                catch (Exception exm)
                {
                    DisplyException(exm);
                    FrmDialog.ShowDialog(MainForm, exm.Message);
                    EndProgress();
                }
                tree.EndUpdate();

            });
        }


        #endregion
        //写入操作和异常错误等日志
        public static void AppendLogItem(string msg)
        {
            try
            {
                if (mediator == null)
                    return;
                mediator.IOMonitorLogForm.AppendLogItem(msg);
                Scada.Logger.Logger.GetInstance().Info(msg);
            }
            catch
            {

            }

        }
        public static void AppendSendCommand(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, Scada.Model.IO_PARA para, Scada.Model.IO_COMMANDS command)
        {
            try
            {
                if (mediator == null)
                    return;
                mediator.IOMonitorForm.InsertMonitorCommandListView(server, communication, device, para, command);
                Scada.Logger.Logger.GetInstance().Info(command.GetCommandString());
            }
            catch
            {

            }

        }
        /// <summary>
        /// 显示最近产生的报警
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        #region 加载IO属性
        public static void SetIOPara(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION comm, Scada.Model.IO_DEVICE device, Scada.Model.IO_PARA para)
        {
            if (mediator.IOMonitorForm.IsHandleCreated)
            {
                mediator.IOMonitorForm.BeginInvoke(new EventHandler(delegate
            {
                mediator.IOPropertiesForm.SetPara(server, comm, device, para);
            }));
            }
        }

        #endregion
        #region 采集站采集数据实时显示

        public static void MonitorIODataShowView(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION comm, Scada.Model.IO_DEVICE device)
        {
            if (mediator.IOMonitorForm.IsHandleCreated)
            {
                mediator.IOMonitorForm.BeginInvoke(new EventHandler(delegate
            {
                if (mediator.IOMonitorForm.Device != null && mediator.IOMonitorForm.Device.IO_COMM_ID == device.IO_COMM_ID && mediator.IOMonitorForm.Device.IO_DEVICE_ID == device.IO_DEVICE_ID && mediator.IOMonitorForm.Device.IO_SERVER_ID == device.IO_SERVER_ID)
                {
                    mediator.IOMonitorForm.SetIOValue(server, comm, device);
                }

             
            }));
            }
        }
        /// <summary>
        /// 显示更新报警界面的数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        public static void MonitorIODataAlarmShowView(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION comm, Scada.Model.IO_DEVICE device, IO_PARAALARM alarm, string uploadresult)
        {
            if (mediator.IOMonitorForm.IsHandleCreated && mediator.IOMonitorForm.InvokeRequired)
            {

                mediator.IOMonitorForm.InsertMonitorAlarmListView(server, comm, device, alarm, uploadresult);


            }
        }
        /// <summary>
        /// 显示上传到数据中心结果日志的显示
        /// </summary>
        /// <param name="server"></param>
        /// <param name="communication"></param>
        /// <param name="device"></param>
        /// <param name="uploadresult"></param>
        public static void ShowMonitorUploadListView(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION communication, Scada.Model.IO_DEVICE device, string uploadresult)
        {

            mediator.IOMonitorForm.ShowMonitorUploadListView(server, communication, device, uploadresult);

        }
        #endregion
        #region 退出系统采集
        public static void ApplicationExit()
        {
            try
            {

                GC.Collect();
                IOMonitorManager.Close();
                Application.ExitThread();
                ScadaProcessManager.KillCurrentProcess();
               
                Application.Exit();
            }
            catch
            {

            }
        }
        #endregion


    }
}
