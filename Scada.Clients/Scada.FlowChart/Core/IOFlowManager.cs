using Scada.Controls.Forms;
using ScadaFlowDesign.Dialog;
using Scada.FlowGraphEngine;
using Scada.FlowGraphEngine.GraphicsMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Scada.DBUtility;
using Scada.Model;
using Scada.MDSCore;
using System.Diagnostics;
using Scada.FlowGraphEngine.GraphicsShape;
using System.ComponentModel;
using Scada.FlowGraphEngine.ScreenShot;
using System.Drawing;




/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaFlowDesign.Core
{
    public abstract class IOFlowManager
    {
        private static System.Threading.Timer ClearMemoryTimer = null;
        public static IOFlowIOProjectManager FlowDataBaseManager = null;
        public static FlowDesign FlowDesign = null;
        public static Mediator Mediator = null;
        public static List<IOFlowProject> Projects = new List<IOFlowProject>();
        public static IOFlowClient MDSClient = null;
        private static MDSConfig mDSServerConfig = null;
        public static IOMonitorConfig Config = new IOMonitorConfig();
        public static string ServerID = ComputerInfo.GetInstall().ServerID.ToString();//每个采集站ID和主板信息绑定，确保唯一
        public static event EventHandler OnConnectedServer;
        public static event EventHandler OnUserLogined;
        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        private static void ClearMemory()
        {
            Task.Run(() =>
            {
                try
                {
                    GC.Collect();

                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                    }
                }
                catch
                {

                }

            });


        }
        #endregion
        #region TCP/IP通讯
        public static void InitMDSClient(string remoteIp)
        {
            try
            {
                mDSServerConfig = new MDSConfig();
                if (MDSClient == null)
                {
                    MDSClient = new IOFlowClient(mDSServerConfig.FlowAppPrefix + "_" + IPAddressSelector.Instance().AddressIPNoPoint, remoteIp, int.Parse(mDSServerConfig.MDSServerPort));
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
                        AddLogToMainLog(msg);
                    };
                    MDSClient.OnPublishedResult = (bool res, string msg) =>
                    {

                        if (res)
                        {

                            AddLogToMainLog("流程图发布成功！");
                            MessageBox.Show(FlowDesign, "发布流程图成功!");

                            PublishObject.Clear();


                        }
                        else
                        {
                            AddLogToMainLog("流程图发布失败！");
                            MessageBox.Show(FlowDesign, "流程图发布失败!");
                        }

                    };
                    MDSClient.DisplayException = (Exception emx) =>
                    {

                        AddLogToMainLog(emx.Message);
                    };
                    MDSClient.ClientDisConnect = (string msg) =>
                    {
                        AddLogToMainLog(msg);

                    };


                    MDSClient.StartPublishProject = (bool res, string projectid) =>
                    {
                        if (res)
                        {
                            PublishObject.Clear();
                            var pubProject = Projects.Find(x => x.ProjectID == projectid);
                            if (pubProject == null)
                            {
                                AddLogToMainLog("未找到ID" + projectid + "的工程,无法发布!");
                                return;
                            }
                            bool isindex = false;
                            for (int i = 0; i < pubProject.GraphList.Count; i++)
                            {
                                isindex = pubProject.GraphList[i].Index;
                                if (isindex)
                                {
                                    break;
                                }
                            }
                            if (isindex == false)
                            {
                                AddLogToMainLog("您发布的工程没有创建主视图，无法发布。请选择主先设置主视图后在尝试发布");
                                return;
                            }
                            StringBuilder sb = new StringBuilder();
                            int ViewNum = pubProject.GraphList.Count;//当前发布的视图数量
                            sb.AppendLine("\r\n--PROJ #" + pubProject.ProjectID + "#" + pubProject.Title + "\r\n");
                            sb.AppendLine(" ");
                            for (int i = 0; i < ViewNum; i++)
                            {
                                sb.AppendLine("\r\n--VIEW #" + pubProject.GraphList[i].GID + "#" + pubProject.GraphList[i].ViewTitle + "#" + pubProject.GraphList[i].Index.ToString() + "\r\n");
                                sb.AppendLine(" ");
                                pubProject.GraphList[i].Site.IsPublish = true;
                                sb.Append(pubProject.GraphList[i].Site.ExportSVG());
                                sb.AppendLine("");
                                sb.AppendLine("\r\n--ENDVIEW\r\n");
                            }
                            //工程包含的用户数据
                            for (int i = 0; i < pubProject.FlowUsers.Count; i++)
                            {
                                sb.AppendLine("\r\n--USER #" + pubProject.FlowUsers[i].Nickname + "#" + pubProject.FlowUsers[i].UserName + "#" + pubProject.FlowUsers[i].Password.ToString() + "#" + pubProject.FlowUsers[i].Read.ToString() + "#" + pubProject.FlowUsers[i].Write.ToString() + "\r\n");
                                sb.AppendLine("\r\n--ENDUSER\r\n");
                            }

                            byte[] byteArray = System.Text.Encoding.Default.GetBytes(sb.ToString());
                            int num = byteArray.Length / PublishMaxSize;
                            if (byteArray.Length % PublishMaxSize != 0)
                            {
                                num++;
                            }

                            for (int i = 0; i < num; i++)
                            {
                                List<byte> tempBytes = new List<byte>();
                                byte[] subBytes;
                                if (i == num - 1 && byteArray.Length % PublishMaxSize != 0)
                                {
                                    subBytes = byteArray.Skip(i * PublishMaxSize).Take(byteArray.Length % PublishMaxSize).ToArray();
                                }
                                else
                                {
                                    subBytes = byteArray.Skip(i * PublishMaxSize).Take(PublishMaxSize).ToArray();
                                }


                                byte[] indexbytes = BitConverter.GetBytes(i + 1);//记录发布的索引
                                tempBytes.AddRange(indexbytes);//前四个字节是索引
                                tempBytes.AddRange(subBytes);//后面跟着数据包

                                PublishObject.Add(i + 1, tempBytes.ToArray());//前两个字节保存的是数据
                            }

                            TcpData tcpData = new TcpData();
                            tcpData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "" });
                            tcpData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "" });
                            tcpData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = FlowDataBaseManager.IOServer.SERVER_ID });
                            tcpData.Items.Add(new TcpDataItem() { Key = "NUMBER", Value = num.ToString() });//发布的总包数
                            tcpData.Items.Add(new TcpDataItem() { Key = "BYTENUMBER", Value = byteArray.Length.ToString() });

                            tcpData.ChangedToBytes();
                            AddLogToMainLog("发布视图数量" + ViewNum);
                            MDSClient.Send(tcpData.Datas, mDSServerConfig.CenterAppName, MDSCommandType.流程发布准备);


                        }
                        else
                        {
                            AddLogToMainLog("无法发布流程组态工程!");
                        }

                    };
                    MDSClient.NextPublishFlowPack = (int index, string msg) =>
                    {

                        if (index <= PublishObject.Count)
                        {
                            byte[] sendbytes = null;
                            if (PublishObject.TryGetValue(index, out sendbytes) && sendbytes.Length > 4)
                            {
                                MDSClient.Send(sendbytes.ToArray(), mDSServerConfig.CenterAppName, MDSCommandType.流程发布数据);
                                AddLogToMainLog("数据接收进度 " + (Convert.ToSingle(index) / PublishObject.Count * 100) + "%");
                            }
                            if (index == PublishObject.Count)
                            {
                                TcpData tcpData = new TcpData();
                                tcpData.Items.Add(new TcpDataItem() { Key = "RELUST", Value = "" });
                                tcpData.Items.Add(new TcpDataItem() { Key = "MSG", Value = "" });
                                tcpData.Items.Add(new TcpDataItem() { Key = "IO_SERVER_ID", Value = FlowDataBaseManager.IOServer.SERVER_ID });


                                tcpData.ChangedToBytes();
                                MDSClient.Send(tcpData.Datas, mDSServerConfig.CenterAppName, MDSCommandType.流程发布成功);
                                AddLogToMainLog("数据已经发送完成，等待服务器反馈!");
                            }

                        }
                    };

                }
                MDSClient.Connect();
                if (OnConnectedServer != null)
                {
                    OnConnectedServer(true, null);
                }
                AddLogToMainLog("连接数据中心通讯成功!");

            }
            catch (Exception emx)
            {
                AddLogToMainLog("连接数据中心通讯失败!" + emx.Message);
            }


        }

        public static void Close()
        {
            if (MDSClient != null)
            {

                MDSClient.Disconnect();
                MDSClient.Dispose();
                MDSClient = null;


            }
            Projects.Clear();
            Projects = null;
            PublishObject.Clear();
            PublishObject = null;
            if (ClearMemoryTimer != null)
            {
                ClearMemoryTimer.Dispose();
            }
            GC.Collect();
            Application.ExitThread();
            ScadaProcessManager.KillCurrentProcess();
            Application.Exit();
        }


        public static bool LoginManager(string user, string password)
        {
            STATION_TCP_INFO loginInfo = new STATION_TCP_INFO();
            loginInfo.USER = user;
            loginInfo.PASSWROD = password;
            loginInfo.IO_SERVER_ID = ServerID;
            loginInfo.IO_SERVER_IP = LocalIp.GetLocalIp();
            loginInfo.RESULT = "false";
            loginInfo.FUNCTION = "IOFlow";
            TcpData tcpData = new TcpData();
            //发送登录命令
            byte[] loginbytes = tcpData.StringToTcpByte(loginInfo.GetCommandString());
            //发送登录命令
            bool res = MDSClient.Send(loginbytes, mDSServerConfig.CenterAppName, Scada.MDSCore.MDSCommandType.登录);
            return res;
        }
        #endregion

        #region 流程发布相关

        /// <summary>
        /// 数据发布的时候一次发送的数据包最大字节，根据网络情况设置
        /// </summary>
        public static int PublishMaxSize = 512000;
        //开始发布流程图
        /// <summary>
        /// 临时保存要发布的数据合集
        /// </summary>
        private static Dictionary<int, byte[]> PublishObject = new Dictionary<int, byte[]>();



        /// <summary>
        /// 准备开始发布流程,要发布的流程工程
        /// </summary>
        public static void PublishFlowStart(IOFlowProject project)
        {
            if (FlowDataBaseManager.IOServer == null)
            {
                AddLogToMainLog("发布流程失败，您当前所在的采集站没有采集站工程！");
                MessageBox.Show(FlowDesign, "发布流程失败，您当前所在的采集站没有采集站工程!");
                return;
            }
            TcpData tcpData = new TcpData();
            string commandStr = "PROJECTID:" + project.ProjectID + "#IO_SERVER_ID:" + FlowDataBaseManager.IOServer.SERVER_ID + "#RESULT:#MSG:工程发布请求";

            byte[] publishbytes = tcpData.StringToTcpByte(commandStr);
            //发送登录命令
            MDSClient.Send(publishbytes, mDSServerConfig.CenterAppName, MDSCommandType.流程发布请求);
            AddLogToMainLog("发布流程命令请求已经下发到服务器，请耐心等待服务器进一步提示！");

        }

        #endregion

        /// <summary>
        /// 初始胡InitFlow
        /// </summary>
        /// <returns></returns>
        public static void StartFlowManager()
        {
            FlowDesign = new FlowDesign();
            Mediator = new Mediator(FlowDesign);
            FlowDesign.mediator = Mediator;
            Mediator.DockPanel = FlowDesign.DockPanel;
            //加载初始化界面
            Mediator.OpenLogForm();
            Mediator.OpenPropertiesForm();
            Mediator.OpenShapeForm();
            Mediator.OpenToolForm();


            //首先加载用户的工程树   
            IOFlowManager.LoadDataBase();

            //启动主界面
            Application.Run(FlowDesign);

            //创建垃圾定时回收

            ClearMemoryTimer = new System.Threading.Timer(delegate
            {

                ClearMemory();

            }, null, 50000, 50000);



        }
        //初始胡窗体后加载数据库中的工程数据
        public static async void LoadDataBase()
        {
            FlowDataBaseManager = new IOFlowIOProjectManager();
            FlowDataBaseManager.OnFlowDesignLogger += FlowManager_OnFlowDesignLogger;
            FlowDataBaseManager.OnFlowExceptionHanped += FlowManager_OnFlowExceptionHanped;

            FlowDataBaseManager.InitBaseModel();
            SVG_SymbolIconManager.LoadedElementInformation = (string information) =>
            {
                return Task.Run(() =>
                {
                    AddLogToMainLog(information);
                });
            };
            await SVG_SymbolIconManager.LoadSymbols();
        }
        #region 异常处理，统一都输出到主任何界面
        public static void AddLogToMainLog(string msg)
        {
            if (Mediator != null)
                Mediator.LogForm.AppendLogItem(msg);

        }
        /// <summary>
        /// 异常信息在日志端显示
        /// </summary>
        /// <param name="ex"></param>
        public static void ThrowExceptionToMain(Exception ex)
        {
            if (Mediator != null)
                Mediator.LogForm.AppendLogItem(ex.Message);

        }


        #endregion
        private static void FlowManager_OnFlowExceptionHanped(Exception ex)
        {
            ThrowExceptionToMain(ex);
        }

        private static void FlowManager_OnFlowDesignLogger(string log)
        {
            AddLogToMainLog(log);
        }

        #region 图件的保存于打开等相关操作
        public static bool LoadProject(string filename)
        {

            bool res = false;
            FileStream fs = null;
            try
            {
                IOFlowProject Project = null;
                fs = new FileStream(filename, FileMode.Open);
                fs.Seek(0, SeekOrigin.Current);
                IFormatter formatter = new BinaryFormatter();
                while (fs.Position < fs.Length)
                {
                    Project = (IOFlowProject)formatter.Deserialize(fs);

                }
                if (Project != null)
                {

                    if (Projects.Exists(x => x.ProjectID == Project.ProjectID))
                    {
                        FrmDialog.ShowDialog(null, "该工程已经被打开");
                    }
                    else
                    {
                        ProjectPasswordDialog confirmDig = new ProjectPasswordDialog(Project);
                        if (confirmDig.ShowDialog() == DialogResult.OK)
                        {


                            Project.FileFullName = filename;
                            //初始化所有图元
                            Mediator.ToolForm.InitTreeView(Project);
                            Mediator.ToolForm.InitTreeUser(Project);
                            Mediator.ToolForm.InitTreeConnections(Project);

                            Projects.Add(Project);
                            res = true;
                        }
                    }

                }

            }
            catch (Exception emx)
            {
                MessageBox.Show(emx.Message + " " + emx.InnerException);
                AddLogToMainLog(emx.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return res;

        }

        public static void OpenProject()
        {
            OpenFileDialog dig = new OpenFileDialog();
            dig.Filter = "流程图(*.flow)|*.flow";
            if (dig.ShowDialog(FlowDesign) == DialogResult.OK)
            {
                try
                {
                    if (LoadProject(dig.FileName))
                    {
                        AddLogToMainLog("打开工程成功 " + dig.FileName);
                        //写入最近打开的列表
                        StreamWriter sw = new StreamWriter(Application.StartupPath + "//Lately.log", true, Encoding.Default);
                        sw.WriteLine(dig.FileName);
                        sw.Close();

                    }
                    else
                    {
                        AddLogToMainLog("打开工程失败 " + dig.FileName);
                    }

                }
                catch (Exception ex)
                {
                    IOFlowManager.ThrowExceptionToMain(ex);

                }
            }

        }
        public static void SaveAsProject(IOFlowProject Project)
        {
            if (Project == null)
            {
                FrmDialog.ShowDialog(FlowDesign, "没有工程可保存!");
                return;
            }
            SaveFileDialog dig = new SaveFileDialog();
            dig.Filter = "流程图(*.flow)|*.flow";
            if (dig.ShowDialog(FlowDesign) == DialogResult.OK)
            {
                try
                {
                    Project.FileFullName = dig.FileName;
                    Save(Project);
                }
                catch (Exception ex)
                {
                    IOFlowManager.ThrowExceptionToMain(ex);
                }
            }
        }
        public static void SaveProject(IOFlowProject Project)
        {
            if (Project == null)
            {
                FrmDialog.ShowDialog(FlowDesign, "没有工程可保存!");
                return;
            }
            if (Project.FileFullName == "")
            {
                SaveAsProject(Project);
            }
            else
            {
                Save(Project);
            }

        }
        public static void Save(IOFlowProject Project)
        {
            Save(Project, Project.FileFullName);


        }

        public static void Save(IOFlowProject Project, string filename)
        {
            //先将文件保存到临时文件，如果保存成功则将保存文件在复制到目标文件，这样做的目的是防止用户工程出现bug导致保存失败而丢失原始工程
            string tempFile = Application.StartupPath + "//temp//flowtemp.flow";
            IFormatter formatter = new BinaryFormatter();
            FileStream fs = null;
            bool res = false;
            try
            {
                fs = new FileStream(tempFile, FileMode.Create);
                formatter.Serialize(fs, Project);
        
                res = true;
            }
            catch (Exception emx)
            {
                MessageBox.Show(FlowDesign, emx.Message);
                AddLogToMainLog("保存工程失败! " + filename);
                res = false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            if(res)
            {
                File.Copy(tempFile, filename, true);
                AddLogToMainLog("保存工程成功! " + filename);
                MessageBox.Show(FlowDesign, "保存工程成功!");

            }
         

        }
        public static void CreateNewProject()
        {

            CreateProjectDialog dig = new CreateProjectDialog();
            if (dig.ShowDialog(FlowDesign) == DialogResult.OK)
            {
                IOFlowProject Project = new IOFlowProject();
                Project.Title = dig.ProjectTitle;
                Project.Password = dig.Password;
                Project.ProjectID = GUIDToNormalID.GuidToLongID().ToString();
                Project.FileFullName = dig.FileFullName;
                Mediator.ToolForm.InitTreeView(Project);
                Mediator.ToolForm.InitTreeUser(Project);
                Mediator.ToolForm.InitTreeConnections(Project);

                Projects.Add(Project);

            }

        }
        public static FlowGraphControl Graph
        {

            get
            {

                if (Mediator.ActiveWork == null)
                    return null;
                WorkForm form = Mediator.ActiveWork as WorkForm;
                return form.GraphControl;
            }
        }
        #region 视图操作

        public static void CreateView()
        {
            Mediator.ToolForm.CreateView();

        }
        public static void DeleteView(FlowGraphControl graph)
        {
            Mediator.ToolForm.CreateView();
        }
        #endregion
        /// <summary>
        /// 发布工程
        /// </summary>
        public static void PublishProject()
        {
            Mediator.ToolForm.Publish();
        }
        public static void EditViewName()
        {
            Mediator.ToolForm.EditViewName();
        }
        /// <summary>
        /// 预览工程
        /// </summary>
        public static void ViewProject()
        {
            Mediator.ToolForm.Debug();

        }
        public static void SaveProject()
        {
            Mediator.ToolForm.SaveProject();

        }
        public static void SaveAsProject()
        {
            Mediator.ToolForm.SaveAsProject();

        }
        public static void DeleteProject()
        {
            Mediator.ToolForm.DeleteProject();

        }
        public static void DeleteView()
        {
            Mediator.ToolForm.DeleteView();

        }
        public static void PasteView()
        {
            Mediator.ToolForm.PasteView();
        }
        public static void CopyView()
        {
            Mediator.ToolForm.CopyView();
        }
        public static void OpenAllView()
        {
            Mediator.ToolForm.OpenAllView();
        }
        public static void OpenView()
        {
            Mediator.ToolForm.OpenView();
        }
        public static void ClosedAllView()
        {
            Mediator.ToolForm.ClosedAllView();
        }
        public static void DeleteDataSource()
        {
            Mediator.ToolForm.DeleteDataSource();
        }
        public static void EditDataSource()
        {
            Mediator.ToolForm.EditDataSource();
        }
        public static void EditRole()
        {
            Mediator.ToolForm.EditRole();
        }
        public static void ViewPropeitiesToOther()
        {
            Mediator.ToolForm.ViewPropeitiesToOther();
        }
        public static void ViewToTemplate()
        {
            Mediator.ToolForm.ViewToTemplate();
        }
        public static void SetProjectIndex()
        {
            Mediator.ToolForm.SetProjectIndex();
        }

        public static void CreateViewGroup()
        {
            Mediator.ToolForm.CreateViewGroup();
        }
        public static void EditProjectUserAndPassword()
        {
            Mediator.ToolForm.EditProjectUserAndPassword();
        }
        public static void DeleteViewGroup()
        {
            Mediator.ToolForm.DeleteViewGroup();
        }
        public static void EditViewGroup()
        {
            Mediator.ToolForm.EditViewGroup();
        }
        public static void AddSqlServerSource()
        {
            Mediator.ToolForm.AddSqlServerSource();
        }
        public static void AddOracleSource()
        {
            Mediator.ToolForm.AddOracleSource();
        }
        public static void AddMySqlSource()
        {
            Mediator.ToolForm.AddMySqlSource();
        }
        public static void AddSyBaseSource()
        {
            Mediator.ToolForm.AddSyBaseSource();
        }

        public static void AddSqllitSource()
        {
            Mediator.ToolForm.AddSqllitSource();
        }
        public static void Debug()
        {
            Mediator.ToolForm.Debug();

        }
        /// <summary>
        /// 截屏
        /// </summary>
        /// <param name="image"></param>
        public static void ScreenShot()
        {
            if (FlowDesign != null)
            {
                FlowDesign.Hide();
            }
            else
            {
                if (FlowDesign.ParentForm != null)
                {
                    FlowDesign.ParentForm.Hide();
                }
                else
                {
                    FlowDesign.Hide();
                }
            }
            try
            {
                FlowDesign.Cursor = Cursors.WaitCursor;
                HexScreenShot form = new HexScreenShot();
                form.Install();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.CurrentBitmap != null)
                    {
                        Rectangle cloneRect = new Rectangle(0, 0, form.CurrentBitmap.Width, form.CurrentBitmap.Height);
                        System.Drawing.Imaging.PixelFormat format = form.CurrentBitmap.PixelFormat;
                        Bitmap cloneBitmap = form.CurrentBitmap.Clone(cloneRect, format);
                        Clipboard.SetDataObject(cloneBitmap);
                        SVG_ImageShape image = new SVG_ImageShape();
                        image.Rectangle = new RectangleF(100, 100, 400, 400);
                        image.BackImage = cloneBitmap;
                        if (Graph != null)
                            Graph.AddShape(image, AddShapeType.Create);

                    }
                }
            }
            catch (Exception emx)
            {
                MessageBox.Show(FlowDesign, emx.Message);
            }
            FlowDesign.Cursor = Cursors.Default;
            if (FlowDesign != null)
            {
                FlowDesign.Show();
            }
            else
            {
                if (FlowDesign.ParentForm != null)
                {
                    FlowDesign.ParentForm.Show();
                }
                else
                {
                    FlowDesign.Show();
                }
            }
        }
        #endregion


    }
}
