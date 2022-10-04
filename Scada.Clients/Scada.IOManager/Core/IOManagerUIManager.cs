
using Scada.IOStructure;
using Scada.Business;
using IOManager.Controls;
using IOManager.Dialogs;

using Scada.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;
using System.Diagnostics;
using System.Net;
using Scada.Model;

using Scada.Kernel;
using Scada.MDSCore;
using Scada.MDSCore.Communication.Messages;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOManager.Core
{
    /// <summary>
    /// 主窗体任务
    /// </summary>
    public abstract class IOManagerUIManager
    {
        private static MDSConfig mDSServerConfig = null;
        public static event EventHandler OnConnectedServer;
        public static event EventHandler OnUserLogined;
        public static List<BatchCommandListItem> BatchCommandTasks = new List<BatchCommandListItem>();
        private static void PublishObjectAAdd(object sender)
        {
            PublishObject.Add(PublishObject.Count+1, sender);

        }
        public static void InitMDSClient(string remoteIp)
        {
            try
            {
                mDSServerConfig = new MDSConfig();
                if (MDSClient == null)
                {
                    MDSClient = new IOManagerClient(mDSServerConfig.ManagerAppPrefix + "_" + IPAddressSelector.Instance().AddressIPNoPoint, remoteIp, int.Parse(mDSServerConfig.MDSServerPort));
                 
                    MDSClient.ServerID = ServerID;
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
                        AddLog(msg);
                    };
                    MDSClient.OnPublishedResult = (bool res, string msg) =>
                    {
                        if (MainForm != null)
                        {
                            for (int i = 0; i < MainForm.Controls.Count; i++)
                            {
                                MainForm.Controls[i].Enabled= true;
                            }
                        }

                        if (res)
                        {
                            MessageBox.Show(MainForm, msg);
                            AddLog(msg);
                            AddLog("工程发布成功,正在启动采集服务器......");
                            
                            PublishObject.Clear();
                            try
                            {
                                Process[] processes = Process.GetProcessesByName("IOMonitor");
                                foreach (Process p in processes)
                                {
                                    p.Kill();
                                }
                            }
                            catch
                            {

                            }
                            try
                            {
                                //复制到发布目录
                                File.Copy(Project, Application.StartupPath + "//IOProject/Station.station", true);
                            }
                            catch
                            {
                                AddLog("备份工程失败");

                            }

                        }
                        else
                        {
                            try
                            {
                                
                                PublishObject.Clear();
                                MessageBox.Show(MainForm, msg);
                            }
                            catch (Exception emx)
                            {
                                AddLog(emx.Message);
                            }
                        }
                        AddLog(msg);
                    };
                    MDSClient.DisplayException = (Exception emx) =>
                    {

                        AddLog(emx.Message);
                    };
                    MDSClient.ClientDisConnect = (string msg) =>
                      {
                          AddLog(msg);

                      };

                    MDSClient.OnReStartMonitor = (bool res, string msg) =>
                    {
                        try
                        {
                            //重新启动监视服务器
                            if (MainForm != null)
                            {
                                for (int i = 0; i < MainForm.Controls.Count; i++)
                                {
                                    MainForm.Controls[i].Enabled = true;
                                }
                            }
                            Process.Start("IOMonitor.exe", Config.User + " " + Config.Password);
                        }
                        catch (Exception emx)
                        {
                            AddLog("启动监视器失败，请手动启动IoMonitor.exe");
                        }
                    };
                    MDSClient.StartPublishProject = (bool res) =>
                      {

                          if (res)
                          {
                              PublishObject.Clear();

                              AddLog("复制当前工程到发布目录......");
                              File.Copy(Project, Application.StartupPath + "//Publish/Publish.station", true);
                              AddLog("与服务器进行通信，准备上传工程,需要耐心等待一段时间......");
                              try
                              {


                                  AddLog("准备发布数据中......");
                                  InitProgress(100);
                                 
                                  #region 发布数据

                                  AddLog("正在准备要发布的数据......");
                                  //直接读取树节点的信息并发布
                                  int allnum = mediator.IOTreeForm.IoTree.GetNodeCount(true);
                                  InitProgress(allnum);
                                  for (int i = 0; i < mediator.IOTreeForm.IoTree.Nodes.Count; i++)
                                  {
                                      if (mediator.IOTreeForm.IoTree.Nodes[i] is IOServerNode)
                                      {
                                          IOServerNode sNode = mediator.IOTreeForm.IoTree.Nodes[i] as IOServerNode;
                                          Scada.Model.IO_SERVER server = sNode.Server;
                                          if (server.SERVER_ID == null || server.SERVER_ID == "")
                                          {
                                              server.SERVER_IP = LocalIp.GetLocalIp();

                                          }
                                          server.SERVER_ID = ServerID;
                                          server.SERVER_IP = LocalIp.GetLocalIp();
                                          PublishObjectAAdd(server);
                                          for (int j = 0; j < sNode.Nodes.Count; j++)
                                          {
                                              if (sNode.Nodes[j] is IOCommunicationNode)
                                              {
                                                  IOCommunicationNode cNode = sNode.Nodes[j] as IOCommunicationNode;
                                                  Scada.Model.IO_COMMUNICATION Communication = cNode.Communication;
                                                  Communication.IO_SERVER_ID = ServerID;
                                                  if (Communication.IO_COMM_ID == null || Communication.IO_COMM_ID == "")
                                                  {
                                                      Communication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();
                                                  }
                                                  if (cNode.Nodes.Count > 0)
                                                      PublishObjectAAdd(Communication);

                                                  for (int d = 0; d < cNode.Nodes.Count; d++)
                                                  {
                                                      if (cNode.Nodes[d] is IODeviceNode)
                                                      {
                                                          IODeviceNode dNode = cNode.Nodes[d] as IODeviceNode;
                                                          Scada.Model.IO_DEVICE Device = dNode.Device;
                                                          if (Device.IOParas == null || Device.IOParas.Count <= 0)
                                                              continue;
                                                          Device.IO_SERVER_ID = ServerID;
                                                          Device.IO_COMM_ID = Communication.IO_COMM_ID;
                                                          if (Device.IO_DEVICE_ID == null || Device.IO_DEVICE_ID == "")
                                                          {
                                                              Device.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID().ToString();
                                                          }

                                                          for (int p = 0; p < Device.IOParas.Count; p++)
                                                          {
                                                              Device.IOParas[p].AlarmConfig.IO_SERVER_ID = ServerID;
                                                              Device.IOParas[p].IO_SERVER_ID = ServerID;

                                                          }
                                                          PublishObjectAAdd(Device);
                                                          SetProgress();
                                                      }

                                                  }
                                                  SetProgress();
                                              }


                                          }
                                          SetProgress();

                                      }

                                  }
                                  ///要发布的机器学习的相关方案
                                  for (int i = 0; i < mediator.IOTreeForm.machineTrainingTree.Nodes[0].Nodes.Count; i++)
                                  {
                                      MachineTrainingTreeNode sNode = mediator.IOTreeForm.machineTrainingTree.Nodes[0].Nodes[i] as MachineTrainingTreeNode;
                                      Scada.Model.ScadaMachineTrainingModel TrainModel = sNode.TrainingModel;
                                      TrainModel.Conditions = new List<Scada.Model.ScadaMachineTrainingCondition>();
                                      for (int j = 0; j < sNode.Nodes.Count; j++)
                                      {
                                          MachineTrainingConditionTreeNode cNode = sNode.Nodes[j] as MachineTrainingConditionTreeNode;
                                          Scada.Model.ScadaMachineTrainingCondition ConditionModel = cNode.TrainingConditionModel;
                                          TrainModel.Conditions.Add(ConditionModel);
                                      }

                                      PublishObjectAAdd(TrainModel);
                                  }

                                  //自动控制命令发布
                                  for (int i = 0; i <BatchCommandTasks.Count; i++)
                                  {
                                      Scada.Model.BatchCommandTaskModel batchCommand = BatchCommandTasks[i].Task;

                                      PublishObjectAAdd(batchCommand);
                                  }
                                  #endregion

                                  TcpData data = new TcpData();
                                  data.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = ServerID });
                                  data.Items.Add(new TcpDataItem() { Key = "NUMBER", Value = PublishObject.Count().ToString() });
                                  data.ChangedToBytes();
                                  MDSClient.Send(data.Datas, mDSServerConfig.CenterAppName, MDSCommandType.上传数据开始);
                                  AddLog("准备开始上传数据请求......");

                              }
                              catch (Exception ex)
                              {
                                  TcpData data = new TcpData();
                                  data.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = ServerID });
                                  data.Items.Add(new TcpDataItem() { Key = "IO_SERVER", Value = "0" });
                                  data.Items.Add(new TcpDataItem() { Key = "IO_COMMUNICATION", Value = "0" });
                                  data.Items.Add(new TcpDataItem() { Key = "IO_DEVICE", Value = "0" });
                                  data.Items.Add(new TcpDataItem() { Key = "IO_PARA", Value = "0" });
                                  data.Items.Add(new TcpDataItem() { Key = "IO_ALARM_CONFIG", Value = "0" });
                                  data.Items.Add(new TcpDataItem() { Key = "COMM_DRIVER", Value = "0" });
                                  data.Items.Add(new TcpDataItem() { Key = "DEVICE_DRIVER", Value = "0" });
                                  data.ChangedToBytes();
                                  MDSClient.Send(data.Datas, mDSServerConfig.CenterAppName, MDSCommandType.上传数据失败);
                                  AddLog("上传数据失败，请再次重新发布工程,原因" + ex.Message);
                                  MessageBox.Show(MainForm, "发布工程失败");
                              }
                          }
                          else
                          {
                              AddLog("发布失败  请重新尝试发布");
                              MessageBox.Show(MainForm, "发布失败  请重新尝试发布");
                          }

                      };
                    MDSClient.NextPublishProjectPack = (int index, string msg) =>
                    {

                        if (index <= PublishObject.Count)
                        {
                            try
                            {
                                object sendobj = null;
                                if(PublishObject.TryGetValue(index,out sendobj)&& sendobj!=null)
                                {
                                    List<byte> allBytes = new List<byte>();
                                    byte[] sendbytes = ObjectSerialize.ObjectToBytesBinaryFormatter(sendobj);
                                    byte[] indexBytes = BitConverter.GetBytes(index);
                                    allBytes.AddRange(indexBytes);
                                    allBytes.AddRange(sendbytes);
                                    MDSClient.Send(allBytes.ToArray(), mDSServerConfig.CenterAppName, MDSCommandType.上传数据, MessageTransmitRules.StoreAndForward, 2000);
                                    Thread.Sleep(10);
                                    AddLog("已经发送第" + index + "条数据");

                                }
                             
                            }
                            catch (Exception ex)
                            {

                                byte[] sendbytes = ObjectSerialize.ObjectToBytesBinaryFormatter(PublishObject.ElementAt(index - 1).Value);
                                MDSClient.Send(sendbytes, mDSServerConfig.CenterAppName, MDSCommandType.上传数据失败);
                                AddLog("上传数据失败" + ex.Message);
                            }
                        }

                    };

                }
                MDSClient.Connect();
                if (OnConnectedServer != null)
                {
                    OnConnectedServer(true, null);
                }
                AddLog("连接数据中心通讯成功!");

            }
            catch (Exception emx)
            {
                AddLog("连接数据中心通讯失败!" + emx.Message);
            }

        }

        #region 登录管理系统，要与服务器进行连接
        /// <summary>
        /// 登录管理系统，要与服务器进行连接
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public static bool LoginManager(string user, string password)
        {

            STATION_TCP_INFO loginInfo = new STATION_TCP_INFO();
            loginInfo.USER = user;
            loginInfo.PASSWROD = password;
            loginInfo.IO_SERVER_ID = ServerID;
            loginInfo.IO_SERVER_IP = LocalIp.GetLocalIp();
            loginInfo.RESULT = "false";
            loginInfo.FUNCTION = "IOManager";
            TcpData tcpData = new TcpData();
            byte[] loginbytes = tcpData.StringToTcpByte(loginInfo.GetCommandString());
            //发送登录命令
            bool res = MDSClient.Send(loginbytes, mDSServerConfig.CenterAppName, Scada.MDSCore.MDSCommandType.登录);
            return res;
        }
        #endregion
        #region 属性定义
        //当前加载的工程
        public static string Project = "";
         

        public static Scada.Model.IO_SERVER IOTreeNodeServer
        {
            get {

                for (int i = 0; i < mediator.IOTreeForm.IoTree.Nodes.Count; i++)
                {
                    if (mediator.IOTreeForm.IoTree.Nodes[i] is IOServerNode)
                    {
                        IOServerNode sNode = mediator.IOTreeForm.IoTree.Nodes[i] as IOServerNode;
                        Scada.Model.IO_SERVER server = sNode.Server;

                        return server;
 

                    }

                }
                return null;
            }
        }

        public static List<Scada.Model.ScadaMachineTrainingModel> MachineTrainingModels
        {
            get
            {
                List<Scada.Model.ScadaMachineTrainingModel> models = new List<Scada.Model.ScadaMachineTrainingModel>();
                for (int i = 0; i < mediator.IOTreeForm.machineTrainingTree.Nodes[0].Nodes.Count; i++)
                {
                    if (mediator.IOTreeForm.machineTrainingTree.Nodes[0].Nodes[i] is MachineTrainingTreeNode machineTrainingTreeNode)
                    {

                        models.Add(machineTrainingTreeNode.TrainingModel);


                    }

                }
                return models;
            }
        }
        public static List<Scada.Model.IO_COMMUNICATION> IOTreeNodeCommunications
        {
            get
            {

                List<Scada.Model.IO_COMMUNICATION> Communications = new List<Scada.Model.IO_COMMUNICATION>();
               
                for (int i = 0; i < mediator.IOTreeForm.IoTree.Nodes.Count; i++)
                {
                    if (mediator.IOTreeForm.IoTree.Nodes[i] is IOServerNode)
                    {
                        IOServerNode sNode = mediator.IOTreeForm.IoTree.Nodes[i] as IOServerNode;
                       

                        for (int j = 0; j < sNode.Nodes.Count; j++)
                        {
                            if (sNode.Nodes[j] is IOCommunicationNode)
                            {
                                IOCommunicationNode cNode = sNode.Nodes[j] as IOCommunicationNode;
                                Scada.Model.IO_COMMUNICATION Communication = cNode.Communication;
                                Communication.IO_SERVER_ID = ServerID;
                                if (Communication.IO_COMM_ID == null || Communication.IO_COMM_ID == "")
                                {
                                    Communication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();
                                }
                                Communication.Devices = new List<Scada.Model.IO_DEVICE>();
                                Communications.Add(Communication);
                                for (int d = 0; d < cNode.Nodes.Count; d++)
                                {
                                    if (cNode.Nodes[d] is IODeviceNode)
                                    {
                                        IODeviceNode dNode = cNode.Nodes[d] as IODeviceNode;
                                        Scada.Model.IO_DEVICE Device = dNode.Device;
                                        Device.IO_SERVER_ID = ServerID;

                                        Device.IO_COMM_ID = Communication.IO_COMM_ID;
                                        Communication.Devices.Add(Device);
                                    }


                                }
                            }


                        }
                   

                    }





                }
                return Communications;
            }
        }
        //当前绑定的窗体

        private static System.Windows.Forms.ToolStripProgressBar progressBar = null;
        private static System.Windows.Forms.ToolStripStatusLabel progressStatus = null;
        private static IOMainForm mForm = null;
        public static string ServerID = ComputerInfo.GetInstall().ServerID.ToString();//每个采集站ID和主板信息绑定，确保唯一
        public static IOMonitorConfig Config = new IOMonitorConfig();

        public static IOManagerClient MDSClient = null;

        public static IOMainForm MainForm
        {
            set
            {
                mForm = value;
                mediator = value.mediator;
                progressBar = mForm.progressBar;
                progressStatus = mForm.progressStatus;
            }
            get { return mForm; }
        }
        public static Mediator mediator = null;
        #endregion
        #region 主窗体进度条
        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="MaxValue"></param>
        public static void InitProgress(int MaxValue)
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = true;
                }
            }
            progressBar.Value = 0;
            progressBar.Maximum = MaxValue;
            progressStatus.Text = "正在准备任务中......";
        }
        public static void SetProgressMax(int MaxValue)
        {


            progressBar.Maximum = MaxValue;

        }
        public static void EndProgress()
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = true;
                }
            }
            progressBar.Value = 0;
            progressBar.Maximum = 100;
            progressStatus.Text = "";
        }
        /// <summary>
        /// 进度
        /// </summary>
        public static void SetProgress()
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = false;
                }
            }
            if (progressBar.Value + 1 == progressBar.Maximum)
            {
                progressBar.Value++;

                progressStatus.Text = "任务完成";
                EndProgress();

            }
            else
            {
                progressBar.Value++;
                progressStatus.Text = (progressBar.Value * 100.0f / progressBar.Maximum).ToString("0.0");
            }

        }
        public static void SetProgress(int value)
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = false;
                }
            }
            progressBar.Value = value;

        }
        #endregion
        #region 主窗体日志写入
        private static void AddLog(string msg)
        {
            if (mediator != null && mediator.IOLogForm != null)
                mediator.IOLogForm.AppendText(DateTime.Now.ToString("yyyy-MM-dd") + "  " + msg);
        }
        #endregion
        #region 工程管理
        public static void Close()
        {
            if (MDSClient != null)
            {
                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;
            }
            Application.ExitThread();
            ScadaProcessManager.KillCurrentProcess();
        }
        //未完成的发布任务,需要进行网络通信将本采集站的数据库发送到数据服务中心
        public static async Task PublisProject()
        {
            if (Project == null || Project == "")
                return;
            ///删除发布数据的缓存
            PublishObject.Clear();
            //先保存当前工程
            if (MessageBox.Show(MainForm, "发布前是否保存工程", "保存提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var task = SaveProject();
                task.Wait();
            }

            if (MessageBox.Show(MainForm, "是否发布此采集站工程,\r\n如果没有保存请在退出系统时保存工程，\r\n发布需要等待一段时间！", "发布提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TcpData publishUnit = new TcpData();
                publishUnit.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = ServerID });//要发布的工程ID
                publishUnit.Items.Add(new TcpDataItem() { Key = "RESULT", Value = "false" });//返回的结果
                publishUnit.Items.Add(new TcpDataItem() { Key = "MSG", Value = "" });//返回的结果信息
                publishUnit.ChangedToBytes();
                MDSClient.Send(publishUnit.Datas, mDSServerConfig.CenterAppName, MDSCommandType.发布工程请求);
                AddLog("正在请求数据服务中心,请耐心等待！.......!");

            }

        }


        /// <summary>
        /// 临时保存要发布的数据合集
        /// </summary>
        private static Dictionary<int, Object> PublishObject = new Dictionary<int, Object>();


        public static void LoadProject()
        {
            try
            {



                OpenFileDialog dialog = new OpenFileDialog();
                dialog.FileName = @"C:\";
                dialog.Filter = "SCADA IO表(*.station)|*.station";
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    Project = dialog.FileName;
                    DbHelperSQLite.connectionString = "Data Source=" + Project + ";Version=3;";
                    var task = InitProject();
                    task.Wait();

                }
            }
            catch (Exception emx)
            {
                DisplayException(emx);
            }
        }
        public static void LoadProject(string filename)
        {
            try
            {

                Project = filename;
                DbHelperSQLite.connectionString = "Data Source=" + Project;
                var task = InitProject();
                task.Wait();

            }
            catch (Exception emx)
            {
                DisplayException(emx);
            }
        }
        public static string ipToLong(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            long value = long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
            return value.ToString();
        }
        /// <summary>
        /// IP地址转换为数字
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        static string LongToip(long ip)
        {
            long IntIp = ip;
            StringBuilder sb = new StringBuilder();
            sb.Append(IntIp >> 0x18 & 0xff).Append(".");
            sb.Append(IntIp >> 0x10 & 0xff).Append(".");
            sb.Append(IntIp >> 0x8 & 0xff).Append(".");
            sb.Append(IntIp & 0xff);
            return sb.ToString();

        }
        private static string OldServerID = "";
        /// <summary>
        /// 初始加载工程数据
        /// </summary>
        private static Task InitProject()
        {
          
            var task = Task.Run(() =>
              {
                  try
                  {

                      InitProgress(100);

                      Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
                      Scada.Model.IO_SERVER server = serverBll.GetModel();

                      if (server == null)
                      {
                          server = new Scada.Model.IO_SERVER();
                          server.SERVER_IP = LocalIp.GetLocalIp();
                          server.SERVER_ID = ServerID;//将IP地址转换为数字形式
                      }
                      string oldServerid = server.SERVER_ID;
                      OldServerID = oldServerid;
                      bool isChanged = false;
                      if (server.SERVER_ID != ServerID)
                      {
                          if (MessageBox.Show(MainForm, "您加载的采集站工程并不是由该采集站服务器上创建，如果要加载将进行本地化转换！是否要加载工程?", "工程加载提醒", MessageBoxButtons.OKCancel) == DialogResult.OK)
                          {
                              isChanged = true;
                          }
                          else
                          {
                              return;
                          }
                      }
                      server.SERVER_ID = ServerID;
                      server.SERVER_IP = LocalIp.GetLocalIp();
                      server.CENTER_IP = IOManagerUIManager.Config.RemoteIP;
                      IOServerNode serverNode = new IOServerNode(server, Project);

                      serverNode.Server = server;

                      serverNode.ChangedNode();


                      mediator.IOLogForm.AppendText("加载采集站 " + oldServerid + " " + server.SERVER_NAME);
                      //加载通道
                      mediator.IOLogForm.AppendText("准备加载通道信息......");
                      Scada.Business.IO_COMMUNICATION commBll = new Scada.Business.IO_COMMUNICATION();
                      Scada.Business.ScadaMachineTrainingModel trainBll = new Scada.Business.ScadaMachineTrainingModel();
                      Scada.Business.ScadaMachineTrainingCondition conditionBll = new Scada.Business.ScadaMachineTrainingCondition();
                      Scada.Business.BatchCommandTaskModel commandTaskBll = new Scada.Business.BatchCommandTaskModel();
                      Scada.Business.BatchCommandTaskItemModel commandTaskItemBll = new Scada.Business.BatchCommandTaskItemModel();
                      List<Scada.Model.IO_COMMUNICATION> comms = commBll.GetModelList(" IO_SERVER_ID='" + oldServerid + "'");
                      if (comms != null)
                          comms = comms.OrderBy(t => t.IO_COMM_NAME).ToList();

                      mediator.IOLogForm.AppendText("加载通道信息成功!");
                      mediator.IOLogForm.AppendText("准备加载设备与IO点表信息......");
                      Scada.Business.IO_DEVICE deviceBll = new Scada.Business.IO_DEVICE();
                      List<Scada.Model.IO_DEVICE> devies = deviceBll.GetModelList(" IO_SERVER_ID='" + oldServerid + "'");
                      mediator.IOLogForm.AppendText("加载设备与IO点表信息成功!");
                      List<Scada.Model.ScadaMachineTrainingModel> trainModels = trainBll.GetModelList(" SERVER_ID='" + oldServerid + "'");

                      List<Scada.Model.BatchCommandTaskModel> commandTaskModels = commandTaskBll.GetModelList(" SERVER_ID='" + oldServerid + "'");
                      List<Scada.Model.BatchCommandTaskItemModel> commandTaskItemsModels = commandTaskItemBll.GetModelList(" SERVER_ID='" + oldServerid + "'");

                      int allnum = comms.Count + devies.Count;
                      InitProgress(allnum);
                      mediator.IOLogForm.AppendText("准备构建IO树......!");
                      comms.ForEach(delegate (Scada.Model.IO_COMMUNICATION c)
                      {

                          IOCommunicationNode commNode = new IOCommunicationNode();
                          commNode.Communication = c;
                          List<Scada.Model.IO_DEVICE> comm_devices = devies.FindAll(x => x != null && x.IO_COMM_ID == c.IO_COMM_ID && x.IO_SERVER_ID == c.IO_SERVER_ID);

                          if (comm_devices != null)
                              comm_devices = comm_devices.OrderBy(t => t.IO_DEVICE_NAME).ToList();
                          List<Scada.Model.ScadaMachineTrainingModel> trainingCommModel = trainModels.FindAll(x => x.COMM_ID == commNode.Communication.IO_COMM_ID);
                          if (isChanged)
                          {
                              c.IO_SERVER_ID = server.SERVER_ID;
                              commNode.Communication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();
                              for (int t = 0; t < trainingCommModel.Count; t++)
                              {
                                  trainingCommModel[t].COMM_ID = commNode.Communication.IO_COMM_ID;
                              }

                          }


                          commNode.ChangedNode();
                          mediator.IOLogForm.AppendText(" 加载完成通讯通道 " + c.IO_COMM_NAME + " " + c.IO_COMM_LABEL);
                          SetProgress();

                          commNode.DeviceNumber = comm_devices.Count;
                          comm_devices.ForEach(delegate (Scada.Model.IO_DEVICE d)
                          {

                              IODeviceNode deviceNode = new IODeviceNode();
                              deviceNode.Device = d;
                              List<Scada.Model.ScadaMachineTrainingModel> trainingDeviceModel = trainingCommModel.FindAll(x => x.DEVICE_ID == deviceNode.Device.IO_DEVICE_ID);
                              if (isChanged)
                              {

                                  d.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID().ToString();
                                  for (int t = 0; t < trainingDeviceModel.Count; t++)
                                  {
                                      trainingDeviceModel[t].COMM_ID = deviceNode.Device.IO_DEVICE_ID;
                                  }

                              }
                              deviceNode.Device.IO_SERVER_ID = server.SERVER_ID;
                              deviceNode.Device.IO_COMM_ID = commNode.Communication.IO_COMM_ID;
                              for (int dv = 0; dv < deviceNode.Device.IOParas.Count; dv++)
                              {
                                  deviceNode.Device.IOParas[dv].IO_SERVER_ID = server.SERVER_ID;
                                  deviceNode.Device.IOParas[dv].IO_COMM_ID = c.IO_COMM_ID;
                                  deviceNode.Device.IOParas[dv].IO_DEVICE_ID = d.IO_DEVICE_ID;
                                  if (isChanged)
                                  {

                                      deviceNode.Device.IOParas[dv].IO_ID = GUIDToNormalID.GuidToLongID().ToString();

                                  }
                                  deviceNode.Device.IOParas[dv].AlarmConfig.IO_ID = deviceNode.Device.IOParas[dv].IO_ID;
                                  deviceNode.Device.IOParas[dv].AlarmConfig.IO_SERVER_ID = server.SERVER_ID;
                                  deviceNode.Device.IOParas[dv].AlarmConfig.IO_COMM_ID = deviceNode.Device.IOParas[dv].IO_COMM_ID;
                                  deviceNode.Device.IOParas[dv].AlarmConfig.IO_DEVICE_ID = deviceNode.Device.IOParas[dv].IO_DEVICE_ID;
                              }
                              deviceNode.ChangedNode();
                              commNode.Nodes.Add(deviceNode);


                              mediator.IOLogForm.AppendText(" 加载完成通讯通道 " + c.IO_COMM_NAME + " 下设备 " + d.IO_DEVICE_NAME + " " + d.IO_DEVICE_LABLE);
                              SetProgress();
                          });
                          serverNode.Nodes.Add(commNode);
                      });
                      mediator.IOTreeForm.ClearNode();
                      mediator.IOTreeForm.IoTree.BeginUpdate();
                      mediator.IOTreeForm.AddMainNode(serverNode);
                      mediator.IOTreeForm.IoTree.EndUpdate();
                      mediator.IOLogForm.AppendText("加载机器学习训练模型......!");

                      trainModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel train)
                      {
                          MachineTrainingTreeNode treeNode = new MachineTrainingTreeNode();
                          treeNode.TrainingModel = train;
                          treeNode.TrainingModel.SERVER_ID = server.SERVER_ID;
                          if (isChanged)
                          {
                              treeNode.TrainingModel.Id = GUIDToNormalID.GuidToInt();

                              Thread.Sleep(100);
                          }
                          treeNode.ChangeNode();
                          mediator.IOTreeForm.AddMachineChilndenNode(treeNode, null);
                          //加载工况节点
                          List<Scada.Model.ScadaMachineTrainingCondition> conditionModels = conditionBll.GetModelList(" SERVER_ID='" + oldServerid + "' and TaskId='" + train.Id + "'");
                          conditionModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingCondition contition)
                          {

                              MachineTrainingConditionTreeNode conditionNode = new MachineTrainingConditionTreeNode();
                              conditionNode.TrainingConditionModel = contition;
                              conditionNode.TrainingConditionModel.SERVER_ID = server.SERVER_ID;
                              if (isChanged)
                              {
                                  conditionNode.TrainingConditionModel.Id = GUIDToNormalID.GuidToInt();
                                  conditionNode.TrainingConditionModel.TaskId = treeNode.TrainingModel.Id;

                                  Thread.Sleep(100);
                              }
                              conditionNode.ChangeNode();
                              mediator.IOTreeForm.AddMachineChilndenNode(conditionNode, treeNode);
                          });
                      });
                      mediator.IOLogForm.AppendText("准备加载自动控制任务!");
                      BatchCommandTasks = new List<BatchCommandListItem>();
                      commandTaskModels.ForEach(delegate (Scada.Model.BatchCommandTaskModel c)
                      {
                          c.Items = new List<Scada.Model.BatchCommandTaskItemModel>();
                          c.Items = commandTaskItemsModels.FindAll(x => x.SERVER_ID == oldServerid && x.CommandTaskID == c.Id);
                          if (isChanged)
                          {
                              c.SERVER_ID = server.SERVER_ID;
                              for (int n = 0; n < c.Items.Count; n++)
                              {
                                  c.Items[n].SERVER_ID = c.SERVER_ID;
                              }
                          }
                          BatchCommandTasks.Add(new BatchCommandListItem()
                          {
                              Task = c


                          });

                      });
                      mediator.IOLogForm.AppendText("加载自动控制任务完成!");
                      //加载自动控制任务
                      mediator.BatchCommandTaskForm.RefreshBatchCommandTask();

                      MainForm.Text = PubConstant.Product + "  " + Project;
                      mediator.IOLogForm.AppendText("加载工程成功 " + Project);
                      EndProgress();
                  }
                  catch (Exception ex)
                  {
                      EndProgress();
                      DisplayException(ex);
                  }
                  EndProgress();
              });

            return task;

        }
        //新建立工程
        public static void CreateProject()
        {



            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = @"C:\" + DateTime.Today.ToString("yyyyMMdd") + ".station";
            dialog.Filter = "SCADA IO表(*.station)|*.station";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Copy(Application.StartupPath + "/db/IOConfig.station", dialog.FileName, true);
                    Project = dialog.FileName;
                    DbHelperSQLite.connectionString = "Data Source=" + Project;
                    var task = InitProject();
                    task.Wait();
                    mediator.IOLogForm.AppendText("创建工程成功 " + dialog.FileName);


                }
                catch (Exception emx)
                {
                    DisplayException(emx);
                }
            }

        }

        //保存工程
        public static Task SaveProject()
        {

            PublishServer mPublishServer = new PublishServer();
            return Task.Factory.StartNew(a =>
              {
                  //保存文件之前将数据先备份下防止意外导致数据丢失


                  Scada.Business.IO_SERVER serverBll = new Scada.Business.IO_SERVER();
                  Scada.Business.IO_PARA paraBll = new Scada.Business.IO_PARA();
                  Scada.Business.IO_DEVICE deviceBll = new Scada.Business.IO_DEVICE();
                  Scada.Business.IO_COMMUNICATION commBll = new Scada.Business.IO_COMMUNICATION();
                  Scada.Business.ScadaMachineTrainingModel trainBll = new Scada.Business.ScadaMachineTrainingModel();
                  Scada.Business.ScadaMachineTrainingCondition conditionBll = new Scada.Business.ScadaMachineTrainingCondition();
                  Scada.Business.BatchCommandTaskModel batchCommandBll = new Scada.Business.BatchCommandTaskModel();
                  Scada.Model.IO_SERVER myserver = serverBll.GetModel();
                  //备份结束后在加载数据
                  try
                  {

                      int allnum = mediator.IOTreeForm.IoTree.GetNodeCount(true);
                      //当前所有的IO点表

                      List<Scada.Model.IO_DEVICE> Devices = new List<Scada.Model.IO_DEVICE>();
                      Scada.Model.IO_SERVER Server = null;
                      List<Scada.Model.IO_COMMUNICATION> Communications = new List<Scada.Model.IO_COMMUNICATION>();
                      List<Scada.Model.IO_PARA> Paras = new List<Scada.Model.IO_PARA>();
                      List<Scada.Model.IO_ALARM_CONFIG> ParaConfigs = new List<Scada.Model.IO_ALARM_CONFIG>();
                      InitProgress(allnum + 10);
                      for (int i = 0; i < mediator.IOTreeForm.IoTree.Nodes.Count; i++)
                      {
                          if (mediator.IOTreeForm.IoTree.Nodes[i] is IOServerNode)
                          {
                              IOServerNode sNode = mediator.IOTreeForm.IoTree.Nodes[i] as IOServerNode;
                              Scada.Model.IO_SERVER server = sNode.Server;
                              if (server.SERVER_ID == null || server.SERVER_ID == "")
                              {
                                  server.SERVER_IP = LocalIp.GetLocalIp();

                              }
                              server.SERVER_ID = ServerID;
                              Server = server;

                              for (int j = 0; j < sNode.Nodes.Count; j++)
                              {
                                  if (sNode.Nodes[j] is IOCommunicationNode)
                                  {
                                      IOCommunicationNode cNode = sNode.Nodes[j] as IOCommunicationNode;
                                      Scada.Model.IO_COMMUNICATION Communication = cNode.Communication;
                                      Communication.IO_SERVER_ID = ServerID;
                                      if (Communication.IO_COMM_ID == null || Communication.IO_COMM_ID == "")
                                      {
                                          Communication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();
                                      }

                                      Communications.Add(Communication);
                                      for (int d = 0; d < cNode.Nodes.Count; d++)
                                      {
                                          if (cNode.Nodes[d] is IODeviceNode)
                                          {
                                              IODeviceNode dNode = cNode.Nodes[d] as IODeviceNode;
                                              Scada.Model.IO_DEVICE Device = dNode.Device;
                                              Device.IO_SERVER_ID = ServerID;

                                              Device.IO_COMM_ID = Communication.IO_COMM_ID;
                                              if (Device.IO_DEVICE_ID == null || Device.IO_DEVICE_ID == "")
                                              {
                                                  Device.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID().ToString();
                                              }

                                              for (int p = 0; p < Device.IOParas.Count; p++)
                                              {

                                                  Device.IOParas[p].IO_SERVER_ID = ServerID;
                                                  Device.IOParas[p].IO_COMM_ID = Communication.IO_COMM_ID;
                                                  Device.IOParas[p].IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                                  Device.IOParas[p].AlarmConfig.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                                  Device.IOParas[p].AlarmConfig.IO_COMM_ID = Communication.IO_COMM_ID;
                                                  Device.IOParas[p].AlarmConfig.IO_SERVER_ID = ServerID;
                                                  if (Device.IOParas[p].IO_ID == null || Device.IOParas[p].IO_ID == "")
                                                  {
                                                      Device.IOParas[p].IO_ID = GUIDToNormalID.GuidToLongID().ToString();

                                                  }
                                                  Device.IOParas[p].AlarmConfig.IO_ID = Device.IOParas[p].IO_ID;
                                                  ParaConfigs.Add(Device.IOParas[p].AlarmConfig);
                                              }
                                              Devices.Add(Device);
                                              Paras.AddRange(Device.IOParas);

                                              SetProgress();
                                          }


                                      }
                                      SetProgress();
                                  }


                              }
                              SetProgress();

                          }





                      }

                      //Sqlit视图有问题，必须先建立条件之外的数据后在插入建立视图相关的数据，否则数据视图会非常多
                      serverBll.Clear();
                      Server.SERVER_IP = LocalIp.GetLocalIp();
                      Server.CENTER_IP = IOManagerUIManager.Config.RemoteIP;
                      serverBll.Add(Server);
                      mediator.IOLogForm.AppendText("准备保存通道信息......");
                      commBll.Clear();
                      commBll.Add(Communications);
                      mediator.IOLogForm.AppendText("保存通道信息成功!");
                      mediator.IOLogForm.AppendText("准备保存设备信息......");
                      deviceBll.Clear();
                      deviceBll.Add(Devices);
                      mediator.IOLogForm.AppendText("保存设备信息成功!");
                      mediator.IOLogForm.AppendText("准备保存IO点表及其预警配置信息......");
                      paraBll.Clear();
                      paraBll.Add(Paras, ParaConfigs);

                      mediator.IOLogForm.AppendText("保存IO点表保存成功!");
                      mediator.IOLogForm.AppendText("准备保存机器学习训练模型!");
                      List<Scada.Model.ScadaMachineTrainingModel> MachineTrains = new List<Scada.Model.ScadaMachineTrainingModel>();
                      List<Scada.Model.ScadaMachineTrainingCondition> Conditions = new List<Scada.Model.ScadaMachineTrainingCondition>();
                      for (int i = 0; i < mediator.IOTreeForm.machineTrainingTree.Nodes[0].Nodes.Count; i++)
                      {
                          MachineTrainingTreeNode trainingTreeNode = mediator.IOTreeForm.machineTrainingTree.Nodes[0].Nodes[i] as MachineTrainingTreeNode;
                          trainingTreeNode.TrainingModel.SERVER_ID = Server.SERVER_ID;
                          trainingTreeNode.TrainingModel.SERVER_NAME = Server.SERVER_NAME;
                          MachineTrains.Add(trainingTreeNode.TrainingModel);
                          for (int j = 0; j < trainingTreeNode.Nodes.Count; j++)
                          {
                              MachineTrainingConditionTreeNode conditionNode = trainingTreeNode.Nodes[j] as MachineTrainingConditionTreeNode;
                              conditionNode.TrainingConditionModel.SERVER_ID = Server.SERVER_ID;
                              conditionNode.TrainingConditionModel.TaskId = trainingTreeNode.TrainingModel.Id;
                              conditionNode.TrainingConditionModel.SERVER_NAME = Server.SERVER_NAME;
                              Conditions.Add(conditionNode.TrainingConditionModel);
                          }
                      }
                      trainBll.Clear(ServerID);
                      trainBll.Clear(OldServerID);
                      trainBll.Add(MachineTrains);
                      conditionBll.Clear(ServerID);
                      conditionBll.Clear(OldServerID);
                      conditionBll.Add(Conditions);
                      EndProgress();

                      //删除老的数据 也就是标记为1的数据
                      mediator.IOLogForm.AppendText("正在保存自动控制任务");
                      List<Scada.Model.BatchCommandTaskModel> batchCommandTasks = new List<Scada.Model.BatchCommandTaskModel>();
                      BatchCommandTasks.ForEach(delegate (BatchCommandListItem listItem) {

                          batchCommandTasks.Add(listItem.Task);
                      });
                      batchCommandBll.Clear(ServerID);
                      batchCommandBll.Clear(OldServerID);


                      batchCommandBll.Add(batchCommandTasks);
                      batchCommandTasks.Clear();
                      batchCommandTasks = null;
                      mediator.IOLogForm.AppendText("工程保存完成");
                      Devices.Clear();
                      Devices = null;
                      Paras.Clear();
                      Paras = null;
                      Communications.Clear();
                      Communications = null;

                      MachineTrains.Clear();
                      MachineTrains = null;

                      //保存用户最近使用的路径,系统默认直接加载
                      IOMonitorConfig config = new IOMonitorConfig();
                      config.Project = Project;
                      config.WriteConfig();
                      //执行数据库的压缩
                      DbHelperSQLite.Compress();


                  }
                  catch (Exception emx)
                  {
                      //出错误，要恢复之前的老数据
                      if (!mPublishServer.Recovery(Project))
                      {

                          return;
                      }


                      //删除老的数据 也就是标记为1的数据

                      DisplayException(emx);
                  }
              }, mPublishServer);
        }

        public static List<Scada.Model.IO_COMMUNICATION> GetIOProject()
        {
            Scada.Model.IO_SERVER Server = null;
            List<Scada.Model.IO_COMMUNICATION> Communications = new List<Scada.Model.IO_COMMUNICATION>();
            if (mediator.IOTreeForm.IoTree.Nodes.Count > 0)
            {



                try
                {





                    for (int i = 0; i < mediator.IOTreeForm.IoTree.Nodes.Count; i++)
                    {
                        if (mediator.IOTreeForm.IoTree.Nodes[i] is IOServerNode)
                        {
                            IOServerNode sNode = mediator.IOTreeForm.IoTree.Nodes[i] as IOServerNode;
                            Scada.Model.IO_SERVER server = sNode.Server;
                            if (server.SERVER_ID == null || server.SERVER_ID == "")
                            {
                                server.SERVER_IP = LocalIp.GetLocalIp();

                            }
                            server.SERVER_ID = ServerID;
                            Server = server;

                            for (int j = 0; j < sNode.Nodes.Count; j++)
                            {
                                if (sNode.Nodes[j] is IOCommunicationNode)
                                {
                                    IOCommunicationNode cNode = sNode.Nodes[j] as IOCommunicationNode;
                                    Scada.Model.IO_COMMUNICATION Communication = cNode.Communication;
                                    Communication.IO_SERVER_ID = ServerID;
                                    if (Communication.IO_COMM_ID == null || Communication.IO_COMM_ID == "")
                                    {
                                        Communication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();
                                    }
                                    Communication.Devices = new List<Scada.Model.IO_DEVICE>() ;
                                    Communications.Add(Communication);
                                    for (int d = 0; d < cNode.Nodes.Count; d++)
                                    {
                                        if (cNode.Nodes[d] is IODeviceNode)
                                        {
                                            IODeviceNode dNode = cNode.Nodes[d] as IODeviceNode;
                                            Scada.Model.IO_DEVICE Device = dNode.Device;
                                            Device.IO_SERVER_ID = ServerID;

                                            Device.IO_COMM_ID = Communication.IO_COMM_ID;
                                            if (Device.IO_DEVICE_ID == null || Device.IO_DEVICE_ID == "")
                                            {
                                                Device.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID().ToString();
                                            }
                                            Communication.Devices.Add(Device);
                                            for (int p = 0; p < Device.IOParas.Count; p++)
                                            {

                                                Device.IOParas[p].IO_SERVER_ID = ServerID;
                                                Device.IOParas[p].IO_COMM_ID = Communication.IO_COMM_ID;
                                                Device.IOParas[p].IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                                Device.IOParas[p].AlarmConfig.IO_DEVICE_ID = Device.IO_DEVICE_ID;
                                                Device.IOParas[p].AlarmConfig.IO_COMM_ID = Communication.IO_COMM_ID;
                                                Device.IOParas[p].AlarmConfig.IO_SERVER_ID = ServerID;
                                                if (Device.IOParas[p].IO_ID == null || Device.IOParas[p].IO_ID == "")
                                                {
                                                    Device.IOParas[p].IO_ID = GUIDToNormalID.GuidToLongID().ToString();

                                                }
                                                Device.IOParas[p].AlarmConfig.IO_ID = Device.IOParas[p].IO_ID;



                                            }




                                        }


                                    }

                                }


                            }


                        }





                    }

                    //Sqlit视图有问题，必须先建立条件之外的数据后在插入建立视图相关的数据，否则数据视图会非常多




                }
                catch (Exception emx)
                {


                    DisplayException(emx);
                }
            }
            return Communications;


        }
        public static void SaveAsProject()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = @"C:\";
            dialog.Filter = "SCADA IO表(*.station)|*.station";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {


                    //此处要先保存点表工程
                    var task = SaveProject();
                    task.Wait();
                    //复制
                    File.Copy(Project, dialog.FileName);
                    mediator.IOLogForm.AppendText("另存工程成功 " + dialog.FileName);


                }
                catch (Exception emx)
                {
                    DisplayException(emx);
                }

            }
        }
        #endregion
        #region 异常处理
        public static void DisplayException(Exception ex)
        {
            if (MainForm != null)
            {
                for (int i = 0; i < MainForm.Controls.Count; i++)
                {
                    MainForm.Controls[i].Enabled = true;
                }
            }

            if (ex == null) throw new ArgumentNullException("ex");

            Logger.GetInstance().Debug(ex.Message);
            mediator.IOLogForm.AppendText("程序错误 " + ex.Message);
        }
        #endregion
        #region TreeView  相关处理
        /// <summary>
        ///将通信驱动加载到下拉列表中
        /// </summary>
        /// <param name="cbx"></param>
        /// <returns></returns>
        public static void IOCommunicationDriveCombox(ComboBox cbx)
        {
            try
            {


                Scada.Business.SCADA_DRIVER driverBll = new Scada.Business.SCADA_DRIVER();
                List<Scada.Model.SCADA_DRIVER> Drives = driverBll.GetModelList("");
                cbx.Items.Clear();
                for (int i = 0; i < Drives.Count; i++)
                {
                    cbx.Items.Add(Drives[i]);

                }
            }
            catch (Exception emx)
            {
                DisplayException(emx);
            }

        }
        public static void IODeviceDriveCombox(ComboBox cbx, string driverId)
        {
            try
            {

                Scada.Business.SCADA_DRIVER comdriverBll = new Scada.Business.SCADA_DRIVER();
                Scada.Model.SCADA_DRIVER comDriver = comdriverBll.GetModel(driverId);
                //此处进行了修改(2021-8-8)，可以指定用户选择某个协议，也可列表出所有协议
                ScadaCommunicateKernel communicateKernel = IOManagerUIManager.CreateCommunicateDriver(comDriver);
                if (comDriver != null)
                {




                    Scada.Business.SCADA_DEVICE_DRIVER driverBll = new Scada.Business.SCADA_DEVICE_DRIVER();
                    List<Scada.Model.SCADA_DEVICE_DRIVER> Drives = driverBll.GetModelList(" Dll_GUID='" + comDriver.GUID + "'");
                    cbx.Items.Clear();


                    if (communicateKernel != null && communicateKernel.AssignDeviceProtocols.Count != 0)
                    {
                        //加载dll下的所有设备解析驱动
                        for (int i = 0; i < Drives.Count; i++)
                        {
                            ScadaDeviceKernel deviceKernel = IOManagerUIManager.CreateDeviceDrive(Drives[i]);
                            if (deviceKernel != null && communicateKernel.AssignDeviceProtocols.Exists(x => x == deviceKernel.GUID))
                            {
                                cbx.Items.Add(Drives[i]);
                                deviceKernel.Dispose();
                                deviceKernel = null;
                            }


                        }
                    }
                    else if (communicateKernel != null && (communicateKernel.AssignDeviceProtocols.Count == 0 || communicateKernel.AssignDeviceProtocols == null))
                    {
                        for (int i = 0; i < Drives.Count; i++)
                        {
                            cbx.Items.Add(Drives[i]);

                        }
                    }

                }

            }
            catch (Exception emx)
            {
                DisplayException(emx);

            }

        }
        /// <summary>
        /// 新建通道
        /// </summary>
        /// <returns></returns>
        public static void CreateIOCommunicationNode()
        {
            try
            {


                if (mediator.IOTreeForm.SelectedNode != null && mediator.IOTreeForm.SelectedNode is IOServerNode)
                {
                    IOServerNode serverNode = mediator.IOTreeForm.SelectedNode as IOServerNode;
                    IOCommunicationForm form = new IOCommunicationForm();
                    form.Server = serverNode.Server;
                    form.InitForm();
                    if (form.ShowDialog(mForm) == DialogResult.OK)
                    {

                        InsertIOCommunicationNode(form.Server, form.Comunication);
                        mediator.IOLogForm.AppendText("创建通道" + form.Comunication.IO_COMM_NAME + "成功");
                    }
                }
            }
            catch (Exception emx)
            {
                DisplayException(emx);
            }

        }
        /// <summary>
        /// 编辑通讯节点
        /// </summary>
        /// <param name="commNode"></param>
        /// <returns></returns>
        public static void EditIOCommunicationNode(IOCommunicationNode commNode)
        {
            try
            {


                if (commNode != null && commNode is IOCommunicationNode)
                {
                    IOServerNode serverNode = commNode.Parent as IOServerNode;
                    IOCommunicationForm form = new IOCommunicationForm();
                    form.Server = serverNode.Server;
                    form.Comunication = commNode.Communication;
                    form.InitForm();
                    if (form.ShowDialog(mForm) == DialogResult.OK)
                    {

                        InsertIOCommunicationNode(form.Server, form.Comunication);
                        mediator.IOLogForm.AppendText("编辑通道" + form.Comunication.IO_COMM_NAME + "成功");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }
        /// <summary>
        /// 插入通信节点
        /// </summary>
        /// <param name="server"></param>
        /// <param name="Communication"></param>
        /// <returns></returns>
        public static void InsertIOCommunicationNode(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION Communication)
        {
            IOCommunicationNode commNode = mediator.IOTreeForm.FindCommunicationTreeNode(server, Communication);
            if (commNode == null)
            {
                IOServerNode serverNode = mediator.IOTreeForm.FindServerTreeNode(server);
                if (serverNode != null)
                {
                    commNode = new IOCommunicationNode();
                    commNode.Communication = Communication;
                    serverNode.AddChildenNode(commNode);


                }
                serverNode.Expand();

            }
            else
            {
                commNode.ChangedNode();
            }
        }
        public static void CreateIODeviceNode()
        {
            try
            {
                if (mediator.IOTreeForm.SelectedNode != null && mediator.IOTreeForm.SelectedNode is IOCommunicationNode)
                {
                    IOCommunicationNode commNode = mediator.IOTreeForm.SelectedNode as IOCommunicationNode;
                    IOServerNode serverNode = commNode.Parent as IOServerNode;
                    IODeviceForm form = new IODeviceForm();
                    form.Server = serverNode.Server;
                    form.Communication = commNode.Communication;
                    form.InitForm();
                    if (form.ShowDialog(mForm) == DialogResult.OK)
                    {
                        InsertIODeviceNode(form.Server, form.Communication, form.Device);
                        mediator.IOLogForm.AppendText("创建设备" + form.Device.IO_DEVICE_NAME + "成功");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }
        /// <summary>
        /// 编辑通讯节点
        /// </summary>
        /// <param name="commNode"></param>
        /// <returns></returns>
        public static void EditIODeviceNode(IODeviceNode deviceNode)
        {
            try
            {
                if (deviceNode != null && deviceNode is IODeviceNode)
                {
                    IOCommunicationNode commNode = deviceNode.Parent as IOCommunicationNode;
                    IOServerNode serverNode = commNode.Parent as IOServerNode;

                    IODeviceForm form = new IODeviceForm();
                    form.Server = serverNode.Server;
                    form.Communication = commNode.Communication;
                    form.Device = deviceNode.Device;
                    form.InitForm();
                    if (form.ShowDialog(mForm) == DialogResult.OK)
                    {

                        InsertIODeviceNode(form.Server, form.Communication, form.Device);
                        mediator.IOLogForm.AppendText("编辑设备" + form.Device.IO_DEVICE_NAME + "成功");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }
        private static Scada.Model.IO_DEVICE CopyDevice = null;
        /// <summary>
        /// 复制一个设备节点
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <param name="targetServerId"></param>
        /// <param name="TargetCommunicationId"></param>
        /// <returns></returns>
        public static Scada.Model.IO_DEVICE CopyIODeviceNode(Scada.Model.IO_DEVICE sourceDevice)
        {
            Scada.Model.IO_DEVICE newDevice = sourceDevice.Copy();
            newDevice.IO_DEVICE_ID = "";
            newDevice.IO_COMM_ID = "";
            newDevice.IO_SERVER_ID = "";
            for (int i = 0; i < newDevice.IOParas.Count; i++)
            {
                newDevice.IOParas[i].IO_ID = "";
                newDevice.IOParas[i].IO_DEVICE_ID = "";
                newDevice.IOParas[i].IO_COMM_ID = "";
                newDevice.IOParas[i].IO_SERVER_ID = "";
                if (newDevice.IOParas[i].AlarmConfig != null)
                {
                    newDevice.IOParas[i].AlarmConfig.IO_DEVICE_ID = "";
                    newDevice.IOParas[i].AlarmConfig.IO_COMM_ID = "";
                    newDevice.IOParas[i].AlarmConfig.IO_SERVER_ID = "";
                    newDevice.IOParas[i].AlarmConfig.IO_ID = "";

                }
            }
            CopyDevice = newDevice;
            return newDevice;
        }
        public static Scada.Model.IO_DEVICE PasteIODeviceNode(Scada.Model.IO_DEVICE sourceDevice, string TargetServerId, string TargetCommunicationID)
        {

            sourceDevice.IO_COMM_ID = TargetCommunicationID;
            sourceDevice.IO_SERVER_ID = TargetServerId;
            sourceDevice.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID();
            for (int i = 0; i < sourceDevice.IOParas.Count; i++)
            {
                sourceDevice.IOParas[i].IO_DEVICE_ID = sourceDevice.IO_DEVICE_ID;
                sourceDevice.IOParas[i].IO_COMM_ID = TargetCommunicationID;
                sourceDevice.IOParas[i].IO_SERVER_ID = TargetServerId;
                sourceDevice.IOParas[i].IO_ID = GUIDToNormalID.GuidToLongID();
                if (sourceDevice.IOParas[i].AlarmConfig != null)
                {
                    sourceDevice.IOParas[i].AlarmConfig.IO_DEVICE_ID = sourceDevice.IO_DEVICE_ID;
                    sourceDevice.IOParas[i].AlarmConfig.IO_COMM_ID = TargetCommunicationID;
                    sourceDevice.IOParas[i].AlarmConfig.IO_SERVER_ID = TargetServerId;
                    sourceDevice.IOParas[i].AlarmConfig.IO_ID = sourceDevice.IOParas[i].IO_ID;

                }
            }
            sourceDevice.IO_DEVICE_NAME = sourceDevice.IO_DEVICE_NAME + "_副本";
            return sourceDevice;
        }
        public static Scada.Model.IO_DEVICE PasteIODeviceNode(string TargetServerId, string TargetCommunicationID)
        {
            if (CopyDevice != null)
                return PasteIODeviceNode(CopyDevice.Copy(), TargetServerId, TargetCommunicationID);
            return null;



        }
        /// <summary>
        /// 插入通信节点
        /// </summary>
        /// <param name="server"></param>
        /// <param name="Communication"></param>
        /// <returns></returns>
        public static void InsertIODeviceNode(Scada.Model.IO_SERVER server, Scada.Model.IO_COMMUNICATION Communication, Scada.Model.IO_DEVICE Device)
        {
            IOServerNode serverNode = mediator.IOTreeForm.FindServerTreeNode(server);
            IOCommunicationNode commNode = mediator.IOTreeForm.FindCommunicationTreeNode(server, Communication);
            IODeviceNode deviceNode = mediator.IOTreeForm.FindDeviceTreeNode(server, Communication, Device);
            if (deviceNode == null)
            {

                if (serverNode != null && commNode != null)
                {
                    deviceNode = new IODeviceNode();
                    deviceNode.Device = Device;
                    commNode.AddChildenNode(deviceNode);
                    commNode.Expand();
                }


            }
            else
            {
                deviceNode.ChangedNode();
            }
        }
        //编辑采集站节点
        public static void EditIOServerNode()
        {
            try
            {
                if (mediator.IOTreeForm.SelectedNode is IOServerNode)
                {
                    IOServerForm form = new IOServerForm();
                    IOServerNode serverNode = mediator.IOTreeForm.SelectedNode as IOServerNode;
                    form.Server = serverNode.Server;


                    if (form.ShowDialog(mForm) == DialogResult.OK)
                    {
                        serverNode.Server = form.Server;
                        serverNode.ChangedNode();
                        mediator.IOLogForm.AppendText("编辑站点" + form.Server.SERVER_NAME + "成功");
                    }

                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }

        #endregion
        #region 加载通讯设备驱动Dll
        private static object CreateObject(string fullname, string dllname)
        {
            try
            {
                Assembly assm = Assembly.LoadFrom(Application.StartupPath + "\\" + dllname + ".dll");//第一步：通过程序集名称加载程序集
                object objType = assm.CreateInstance(fullname, true);// 第二步：通过命名空间+类名创建类的实例。
                return objType;


            }
            catch (System.Exception ex)
            {
                DisplayException(ex);
                return null;
            }

        }

        /// <summary>
        /// 获取驱动信息程序集信息
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static DllInfo GetDriverAssembly(string fullname)
        {
            DllInfo dllInfo = new DllInfo();
            try
            {
                Assembly assembly = Assembly.LoadFrom(fullname);

                foreach (Attribute attr in Attribute.GetCustomAttributes(assembly))
                {

                    if (attr.GetType() == typeof(AssemblyTitleAttribute))
                    {
                        dllInfo.Title = ((AssemblyTitleAttribute)attr).Title;
                    }
                    else if (attr.GetType() == typeof(AssemblyDescriptionAttribute))
                    {
                        dllInfo.Description = ((AssemblyDescriptionAttribute)attr).Description;

                    }
                    else if (attr.GetType() == typeof(AssemblyCompanyAttribute))
                    {
                        dllInfo.Company = ((AssemblyCompanyAttribute)attr).Company;
                    }
                    else if (attr.GetType() == typeof(AssemblyVersionAttribute))
                    {
                        dllInfo.Version = ((AssemblyVersionAttribute)attr).Version;
                    }
                    else if (attr.GetType() == typeof(GuidAttribute))
                    {
                        dllInfo.GUID = ((GuidAttribute)attr).Value;
                    }
                }
                dllInfo.FillName = Path.GetFileNameWithoutExtension(fullname);
                dllInfo.FullName = assembly.FullName;
                Type[] types = assembly.GetTypes();
                FileInfo fn = new FileInfo(fullname);
                for (int i = 0; i < types.Length; i++)
                {
                    if (types[i].BaseType == null)
                        continue;
                    if ((types[i].BaseType != null && types[i].BaseType == typeof(ScadaCommunicateKernel)) || (types[i].BaseType.BaseType != null && types[i].BaseType.BaseType == typeof(ScadaCommunicateKernel)))
                    {
                        ScadaCommunicateKernel obj = (ScadaCommunicateKernel)Activator.CreateInstance(types[i]);

                        DriverInfo driverInfo = new DriverInfo();
                        driverInfo.ClassName = types[i].Name;
                        driverInfo.FullName = types[i].FullName;
                        driverInfo.Title = obj.Title;
                        driverInfo.Guid = obj.GUID;
                        driverInfo.DllGuid = dllInfo.GUID;
                        dllInfo.CommDrivers.Add(driverInfo);

                    }
                    if ((types[i].BaseType != null && types[i].BaseType == typeof(ScadaDeviceKernel)) || (types[i].BaseType.BaseType != null && types[i].BaseType.BaseType == typeof(ScadaDeviceKernel)))
                    {
                        ScadaDeviceKernel obj = (ScadaDeviceKernel)Activator.CreateInstance(types[i]);

                        DriverInfo driverInfo = new DriverInfo();
                        driverInfo.ClassName = types[i].Name;
                        driverInfo.FullName = types[i].FullName;
                        driverInfo.Title = obj.Title;
                        driverInfo.Guid = obj.GUID;
                        driverInfo.DllGuid = dllInfo.GUID;
                        dllInfo.DeviceDrivers.Add(driverInfo);

                    }
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

            return dllInfo;

        }
        /// <summary>
        /// 创建设备驱动
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ScadaDeviceKernel CreateDeviceDrive(Scada.Model.SCADA_DEVICE_DRIVER driveModel)
        {
            try
            {
                ScadaDeviceKernel river = (ScadaDeviceKernel)CreateObject(driveModel.DeviceFullName, driveModel.Dll_Name);

                return river;
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
            return null;
        }
        /// <summary>
        /// 创建通讯驱动
        /// </summary>
        /// <param name="commModel"></param>
        /// <returns></returns>
        public static ScadaCommunicateKernel CreateCommunicateDriver(Scada.Model.SCADA_DRIVER commModel)
        {
            if (commModel == null)
                return null;
            try
            {
                ScadaCommunicateKernel river = (ScadaCommunicateKernel)CreateObject(commModel.CommunicationFullName, commModel.FillName);

                return river;
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
            return null;
        }


        #endregion
        #region 通讯驱动管理
        /// <summary>
        /// 增加驱动
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static void AddDrive()
        {
            try
            {
                OpenFileDialog dig = new OpenFileDialog();
                dig.Filter = "通讯驱动(*.dll)|*.dll";
                if (dig.ShowDialog(MainForm) == DialogResult.OK)
                {
                    DllInfo dllInfo = GetDriverAssembly(dig.FileName);

                    if (dllInfo == null)
                    {
                        mediator.IOLogForm.AppendText("您加载的不是本系统的驱动，请检测驱动接口是否正确");
                        MessageBox.Show("您加载的不是本系统的驱动，请检测驱动接口是否正确");

                    }
                    else
                    {
                        Scada.Business.SCADA_DRIVER DriverBll = new Scada.Business.SCADA_DRIVER();

                        bool res = false;
                        for (int i = 0; i < dllInfo.CommDrivers.Count; i++)
                        {
                            try
                            {


                                DriverInfo info = dllInfo.CommDrivers[i];

                                Scada.Model.SCADA_DRIVER commDriver = new Scada.Model.SCADA_DRIVER();
                                commDriver.Id = GUIDToNormalID.GuidToLongID(Guid.Parse(info.Guid)).ToString();
                                commDriver.DeviceName = "";
                                commDriver.Company = dllInfo.Company;
                                commDriver.Description = dllInfo.Description;
                                commDriver.IsSystem = 0;
                                commDriver.UpdateTime = DateTime.Now.ToString();
                                commDriver.CreateTime = DateTime.Now.ToString();
                                commDriver.Anthor = dllInfo.Company;
                                commDriver.CommunicationFullName = info.FullName;
                                commDriver.CommunicationName = info.ClassName;
                                commDriver.Title = info.Title;
                                commDriver.Version = dllInfo.Version;
                                commDriver.GUID = dllInfo.GUID;
                                commDriver.Namespace = info.FullName;
                                commDriver.ClassifyId = 12;//用户自定义驱动默认在12扩展驱动目录下
                                //dll名称
                                commDriver.FillName = dllInfo.FillName;

                                Scada.Model.SCADA_DRIVER existDriver = DriverBll.GetModel(commDriver.Id);


                                if (existDriver == null)
                                {
                                    res = DriverBll.Add(commDriver);

                                }
                                else
                                {
                                    res = DriverBll.Update(commDriver);
                                }
                                string oldconn = DbHelperSQLite.connectionString;
                                DbHelperSQLite.connectionString = "Data Source = " + Application.StartupPath + "/db/IOConfig.station";
                                Scada.Model.SCADA_DRIVER existOldDriver = DriverBll.GetModel(commDriver.Id);
                                if (existOldDriver == null)
                                {
                                    res = DriverBll.Add(commDriver);

                                }
                                else
                                {
                                    res = DriverBll.Update(commDriver);
                                }
                                DbHelperSQLite.connectionString = oldconn;

                            }
                            catch (Exception emx)
                            {
                                AddLog(emx.Message);
                                res = false;
                            }

                        }



                        //更新驱动
                        if (res)
                        {
                            //添加设备驱动
                            Scada.Business.SCADA_DEVICE_DRIVER DeviceDriverBll = new Scada.Business.SCADA_DEVICE_DRIVER();
                            for (int i = 0; i < dllInfo.DeviceDrivers.Count; i++)
                            {
                                try
                                {


                                    DriverInfo info = dllInfo.DeviceDrivers[i];

                                    Scada.Model.SCADA_DEVICE_DRIVER deviceDriver = new Scada.Model.SCADA_DEVICE_DRIVER();
                                    deviceDriver.Id = GUIDToNormalID.GuidToLongID(Guid.Parse(info.Guid)).ToString();
                                    deviceDriver.DeviceName = info.ClassName;
                                    deviceDriver.Dll_GUID = dllInfo.GUID;
                                    deviceDriver.DeviceFullName = info.FullName;
                                    deviceDriver.Title = info.Title;
                                    deviceDriver.Namespace = info.FullName;

                                    deviceDriver.Dll_Name = dllInfo.FillName;
                                    deviceDriver.FillName = dllInfo.FillName;
                                    deviceDriver.Dll_Title = dllInfo.Title;
                                    Scada.Model.SCADA_DEVICE_DRIVER existDriver = DeviceDriverBll.GetModel(deviceDriver.Id);


                                    if (existDriver == null)
                                    {
                                        string oldconn = DbHelperSQLite.connectionString;
                                        DbHelperSQLite.connectionString = "Data Source = " + Application.StartupPath + "/db/IOConfig.station";
                                        DeviceDriverBll.Add(deviceDriver);
                                        DbHelperSQLite.connectionString = oldconn;
                                        DeviceDriverBll.Add(deviceDriver);
                                    }
                                    else
                                    {
                                        string oldconn = DbHelperSQLite.connectionString;
                                        DbHelperSQLite.connectionString = "Data Source = " + Application.StartupPath + "/db/IOConfig.station";
                                        DeviceDriverBll.Update(deviceDriver);
                                        DbHelperSQLite.connectionString = oldconn;
                                        DeviceDriverBll.Update(deviceDriver);
                                    }
                                }
                                catch (Exception emx)
                                {
                                    AddLog(emx.Message);
                                    res = false;
                                }

                            }


                            //实际驱动的调用目录
                            File.Copy(dig.FileName, Application.StartupPath + "/" + dllInfo.FillName + ".dll", true);
                            //驱动的备份
                            File.Copy(dig.FileName, Application.StartupPath + "/Drivers/" + dllInfo.FillName + ".dll", true);

                            MessageBox.Show(MainForm, "保存成功");
                            AddLog("新增驱动" + dllInfo.Title.ToString() + "成功");

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }
        public static void LoadDriver(TreeView tree, ContextMenuStrip contextMenuStrip)
        {
            #region 加载驱动和目录
            Scada.Business.Classify_DRIVER claBll = new Scada.Business.Classify_DRIVER();
            Scada.Business.SCADA_DRIVER driverBll = new Scada.Business.SCADA_DRIVER();
            List<Scada.Model.Classify_DRIVER> cls = claBll.GetModelList("");
            tree.Nodes.Clear();
            for (int i = 0; i < cls.Count; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Text = cls[i].ClassifyName;
                tn.Tag = cls[i].Id;
                tn.Name = cls[i].Id.ToString();
                tree.Nodes.Add(tn);
                List<Scada.Model.SCADA_DRIVER> drivers = driverBll.GetModelList(" ClassifyId=" + cls[i].Id + " ");
                for (int j = 0; j < drivers.Count; j++)
                {

                    TreeNode drivertn = new TreeNode();
                    drivertn.Text = drivers[j].Title;
                    drivertn.Tag = drivers[j].Id;
                    drivertn.Name = "device_" + drivers[j].Id;
                    if (drivers[j].IsSystem == 0)
                        drivertn.ContextMenuStrip = contextMenuStrip;
                    tn.Nodes.Add(drivertn);
                }
            }

            #endregion
        }
        /// <summary>
        /// 删除某个驱动
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static void DeleteDrive(TreeNode lv)
        {
            try
            {
                if (MessageBox.Show(MainForm, "删除正在使用的驱动非常危险,是否要删除选中的驱动?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Scada.Business.SCADA_DRIVER commDriverBll = new Scada.Business.SCADA_DRIVER();

                    if (commDriverBll.Delete(lv.Tag.ToString()))
                    {

                        AddLog("删除" + lv.Text + "驱动成功");
                    }

                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }
        #endregion  
        #region 日志管理部分
        //导出日志

        public static void ExportLog()
        {
            SaveFileDialog dig = new SaveFileDialog();
            dig.Filter = "文本文件(*.txt)|*.txt";
            if (dig.ShowDialog(MainForm) == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(dig.FileName, false, Encoding.Default);
                List<string> sb = mediator.IOLogForm.GetLogContent();
                InitProgress(sb.Count);
                for (int i = 0; i < sb.Count; i++)
                {
                    SetProgress();
                    sw.WriteLine(sb[i]);
                }
                sw.Close();
            }
        }
        #endregion
        #region IO点表管理
        public static void OpenDeviceParas(Scada.Model.IO_SERVER Server, Scada.Model.IO_COMMUNICATION Communication, Scada.Model.IO_DEVICE Device)
        {
            try
            {
                mediator.IOParaForm.InitListView(Server, Communication, Device);
                mediator.IOLogForm.AppendText("编辑设备" + Server.SERVER_NAME + "\\" + Communication.IO_COMM_NAME + "\\" + Device.IO_DEVICE_NAME + "下的IO点");
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }
        public static void EditDevicePara(Scada.Model.IO_SERVER Server, Scada.Model.IO_COMMUNICATION Communication, Scada.Model.IO_DEVICE Device, Scada.Model.IO_PARA Para)
        {
            try
            {

                IOParaForm form = new IOParaForm();
                form.Server = Server;
                form.Comunication = Communication;
                form.Device = Device;
                form.Para = Para;
                form.InitForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    mediator.IOLogForm.AppendText("在" + Device.IO_DEVICE_NAME + "设备上创建" + form.Para.IO_NAME + "成功!");
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }
        #endregion
        #region 通道模拟器参数配置
        public static void EditIOCommunicationSimulatorNode(IOCommunicationNode commNode)
        {
            try
            {
                if (commNode != null && commNode is IOCommunicationNode)
                {
                    if (commNode.Communication.CommunicateDriver == null)
                    {
                        commNode.Communication.CommunicateDriver = CreateCommunicateDriver(commNode.Communication.DriverInfo);
                    }
                    List<Scada.Model.IO_DEVICE> devices = new List<Scada.Model.IO_DEVICE>();
                    for (int i = 0; i < commNode.Nodes.Count; i++)
                    {
                        if (commNode.Nodes[i].GetType() == typeof(IODeviceNode))
                        {
                            IODeviceNode deviceNode = commNode.Nodes[i] as IODeviceNode;
                            devices.Add(deviceNode.Device);
                        }

                    }
                    IOCommunicationSimulatorForm simulatorForm = new IOCommunicationSimulatorForm(commNode.Communication, devices);
                    if (simulatorForm.ShowDialog() == DialogResult.OK)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

        }
        #endregion

        #region 机器学习相关
        /// <summary>
        /// 编辑机器学习任务
        /// </summary>
        public static Scada.Model.ScadaMachineTrainingModel MachineTrainingModelEdit(IOTree ioTree, Scada.Model.ScadaMachineTrainingModel scadaMachineTraining = null)
        {
            MachineTrainingEditForm editForm = new MachineTrainingEditForm(ioTree, scadaMachineTraining);
            if (editForm.ShowDialog(mForm) == DialogResult.OK)
            {
                editForm.MachineTrainingModel.SERVER_ID = ServerID;
                editForm.MachineTrainingModel.SERVER_NAME = "";
                if (editForm.MachineTrainingModel.Id == 0)
                {
                    editForm.MachineTrainingModel.Id = GUIDToNormalID.GuidToInt();
                }
                return editForm.MachineTrainingModel;
            }
            return null;
        }
        /// <summary>
        /// 编辑任务工况
        /// </summary>
        public static Scada.Model.ScadaMachineTrainingCondition MachineTrainingConditionEdit(Scada.Model.ScadaMachineTrainingModel scadaMachineTraining, IOTree ioTree, Scada.Model.ScadaMachineTrainingCondition condition = null)
        {
            MachineTrainingConditionEditForm editForm = new MachineTrainingConditionEditForm(ioTree, condition);
            if (editForm.ShowDialog(mForm) == DialogResult.OK)
            {
                editForm.ConditionModel.SERVER_ID = ServerID;
                editForm.ConditionModel.SERVER_NAME = "";
                editForm.ConditionModel.TaskId = scadaMachineTraining.Id;
                if (editForm.ConditionModel.Id == 0)
                {
                    editForm.ConditionModel.Id = GUIDToNormalID.GuidToInt();
                }
                return editForm.ConditionModel;
            }
            return null;
        }

        public static void AddMachineTrain()
        {
            mediator.IOTreeForm.AddTrainModel();
        }
        public static void EditMachineTrain()
        {
            mediator.IOTreeForm.EditTrainModel();
        }
        public static void DeleteMachineTrain()
        {
            mediator.IOTreeForm.DeleteTrainModel();
        }
        #endregion
        #region 自动控制相关
        public static void EditBatchCommandTaskForm()
        {
            try
            {
                BatchCommandListItem taskModel = mediator.BatchCommandTaskForm.SelectItem;
 
                if (taskModel == null)
                    return;
                BatchCommandTaskEditForm taskForm = new BatchCommandTaskEditForm();
                taskForm.InitForm();
                taskForm.TaskModel = taskModel.Task;
                if (taskForm.ShowDialog(mediator.BatchCommandTaskForm) == DialogResult.OK)
                {
                    taskForm.TaskModel.SERVER_ID = ServerID;

                    BatchCommandListItem listItem = BatchCommandTasks.Find(x => x.Task == taskForm.TaskModel);
                    if (listItem != null)
                    {
                        listItem.Task = taskForm.TaskModel;
                    }


                }
            }
            catch (Exception emx)
            {
                MessageBox.Show(mediator.BatchCommandTaskForm, emx.Message);

            }
        }
        public static void AddBatchCommandTaskForm()
        {
            try
            {
                BatchCommandTaskEditForm taskForm = new BatchCommandTaskEditForm();
                taskForm.InitForm();
                if (taskForm.ShowDialog(mediator.BatchCommandTaskForm) == DialogResult.OK)
                {
                    taskForm.TaskModel.Id = GUIDToNormalID.GuidToLongID();
                    taskForm.TaskModel.SERVER_ID = ServerID;
                    BatchCommandTasks.Add(new BatchCommandListItem()
                    {
                        Task = taskForm.TaskModel

                    });
                    mediator.BatchCommandTaskForm.RefreshBatchCommandTask();

                }
            }
            catch (Exception emx)
            {
                MessageBox.Show(mediator.BatchCommandTaskForm, emx.Message);

            }
        }
        /// <summary>
        /// 编辑任务流程
        /// </summary>
        /// <param name="taskModel"></param>
        public static void EditBatchCommandTaskFlowForm()
        {
            BatchCommandListItem taskModel = mediator.BatchCommandTaskForm.SelectItem;
            if (taskModel == null)
                return;
            BatchCommandTaskFlowForm taskFlowForm = new BatchCommandTaskFlowForm();
            taskFlowForm.Text = taskModel.Task.CommandTaskTitle + "-控制流程编辑";
            taskFlowForm.SetBatchCommandTask(taskModel.Task);
            if (taskFlowForm.ShowDialog(IOManagerUIManager.mediator.BatchCommandTaskForm) == DialogResult.OK)
            {
                taskModel.Task = taskFlowForm.CommandTaskModel;
            }
        }
        public static void DeleteBatchCommandTaskForm()
        {
            BatchCommandListItem taskModel = mediator.BatchCommandTaskForm.SelectItem;
            if (taskModel == null)
                return;
            if (MessageBox.Show(IOManagerUIManager.mediator.BatchCommandTaskForm, "是否要删除任务?", "删除提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
               
                    IOManagerUIManager.BatchCommandTasks.Remove(taskModel);
                IOManagerUIManager.mediator.BatchCommandTaskForm.RefreshBatchCommandTask();


            }
        }
        #endregion
    }
}
