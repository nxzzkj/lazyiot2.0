using IOMonitor.Controls;
using IOMonitor.Core;
using Scada.Kernel;
using Scada.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor
{
    public partial class SimulatorForm : Form
    {
        public IOMonitorSimulatorManager SimulatorManager = null;
        public SimulatorForm()
        {
            InitializeComponent();
            
        }
        protected List<ListViewItem> LogCacheItemsSource
        {
            get;
            private set;
        }

        protected ConcurrentStack<ListViewItem> LogItemsSource
        {
            get;
            private set;
        }
        System.Threading.Timer listTimer = null;
        private void ListViewAlarm_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (this.LogCacheItemsSource == null || this.LogCacheItemsSource.Count == 0)
            {
                return;
            }

            e.Item = this.LogCacheItemsSource[e.ItemIndex];
            if (e.ItemIndex == this.LogCacheItemsSource.Count)
            {
                this.LogCacheItemsSource = null;

            }

        }
        private void toolClose_Click(object sender, EventArgs e)
        {
           
      
                this.Close();
            
        }

        private void SimulatorView_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            try
            {
                LoadTreeStatus();
                computerInfoControl.Monitour();
                SimulatorManager = new IOMonitorSimulatorManager();
                SimulatorManager.OnSimulatorLog += SimulatorManager_OnSimulatorLog;
                listView.VirtualMode = true;
                listView.VirtualListSize = 0;
                
                listView.RetrieveVirtualItem += ListViewAlarm_RetrieveVirtualItem;

                LogCacheItemsSource = new List<ListViewItem>();

                LogItemsSource = new ConcurrentStack<ListViewItem>();

                listTimer = new System.Threading.Timer(delegate
                {
                    if (IOMonitorManager.IsBackRun)
                        return;
                    TaskHelper.Factory.StartNew(() => { 
                    try
                    {
                         
                         
                         
                            //定时加载日志
                            LogCacheItemsSource = LogItemsSource.ToList();

                            if (LogItemsSource.Count > 2000)
                            {

                                LogItemsSource.Clear();
                                listView.VirtualListSize = 0;

                            }
                            else
                            {
                                listView.VirtualListSize = LogCacheItemsSource.Count;

                            }
                        
                         
                    }
                    catch
                    {

                    }
                    });
                }, null, 1000, 1000);


            }
            catch (Exception emx)
            {
                MessageBox.Show(this, emx.Message);

            }
       
        }

        private void SimulatorManager_OnSimulatorLog(string log)
        {
            if (IOMonitorManager.IsBackRun)
                return;
            AddLog(log);
            
        }
        public void AddLog(string msg)
        {
                ListViewItem lvi = new ListViewItem(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lvi.SubItems.Add(msg);
                LogItemsSource.Push(lvi);

        }

        private void toolStart_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show(this, "是否要启动驱动模拟器?", "提示",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TaskHelper.Factory.StartNew(() => { 
                try
                {
                    if (SimulatorManager != null)
                    {
                        SimulatorManager.ColseSimulator();
                        SimulatorManager.Dispose();
                      
                    }
                      
                  
                    SimulatorManager.InitSimulator(new List<IO_SERVER>() { IOMonitorProjectManager.IOServer }, IOMonitorProjectManager.IOCommunications, IOMonitorProjectManager.IODevices, IOMonitorProjectManager.CommDrivers, IOMonitorProjectManager.DeviceDrivers);

                    SimulatorManager.StartSimulator();
                    toolStart.Enabled = false;
                    toolStop.Enabled = true;
                }
                catch(Exception emx)
                {
                    toolStart.Enabled = true;
                    toolStop.Enabled = false;
                    MessageBox.Show(this, emx.Message);
                }
                });
            }
          
        }

        private void toolStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否要停止模拟器运行", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TaskHelper.Factory.StartNew(() =>
                {
                    try
                    {
                        toolStart.Enabled = true;
                        toolStop.Enabled = false;
                        SimulatorManager.ColseSimulator();
                        SimulatorManager.Dispose();
                    }
                    catch (Exception emx)
                    {
                        MessageBox.Show(this, emx.Message);
                    }
                });
            }
        }

        #region 加载目录树
        public void LoadTreeStatus()
        {
            TaskHelper.Factory.StartNew(() =>
            {
                try
                {

                    IOMonitorUIManager.LoadSimulatorTree(this.IoTreeStatus);
                  
                }
                catch (Exception ex)
                {
                    IOMonitorUIManager.DisplyException(ex);
                }
            });

        }

        #endregion

        private void IoTreeStatus_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

    

        }

        private void SimulatorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.SimulatorManager.ColseSimulator();
                this.SimulatorManager.Dispose();
            }
            catch
            {

            }
        }

        private void SimulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(this, "是否要退出模拟器", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
              


            }
        }

        private void IoTreeStatus_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node is IoCommunicationTreeNode)
                {
                    try
                    {
                        IoCommunicationTreeNode commNode = e.Node as IoCommunicationTreeNode;

                        //加载模拟器显示参数界面配置界面
                        if (commNode.Communication != null)
                        {
                            commNode.Communication.SimulatorCheckedStatus = e.Node.Checked;
                        }
                    }
                    catch (Exception ex)
                    {
                        IOMonitorUIManager.DisplyException(ex);
                    }
                }
                else if (e.Node is IoDeviceTreeNode)
                {
                    try
                    {
                        IoDeviceTreeNode devNode = e.Node as IoDeviceTreeNode;

                        //加载模拟器显示参数界面配置界面
                        if (devNode.Device != null)
                        {
                            devNode.Device.SimulatorCheckedStatus = e.Node.Checked;
                        }
                    }
                    catch (Exception ex)
                    {
                        IOMonitorUIManager.DisplyException(ex);
                    }
                }
            }
        }

        private void btCheckAll_Click(object sender, EventArgs e)
        {
            foreach(TreeNode tn in IoTreeStatus.Nodes)
            {
                foreach (TreeNode ctn in tn.Nodes)//通道
                {
                    if (ctn is IoCommunicationTreeNode commNode)
                    {
                        commNode.Communication.SimulatorCheckedStatus = true;
                        commNode.Checked = true;
                    }
                    foreach (TreeNode dtn in ctn.Nodes)//设备
                    {
                        if (dtn is IoDeviceTreeNode devNode)
                        {
                            devNode.Device.SimulatorCheckedStatus = true;
                            devNode.Checked = true;
                        }

                    }

                }

            }

        }

        private void btUnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in IoTreeStatus.Nodes)
            {
                foreach (TreeNode ctn in tn.Nodes)//通道
                {
                    if (ctn is IoCommunicationTreeNode commNode)
                    {
                        commNode.Communication.SimulatorCheckedStatus = false;
                        commNode.Checked = false;
                    }
                    foreach (TreeNode dtn in ctn.Nodes)//设备
                    {
                        if (dtn is IoDeviceTreeNode devNode)
                        {
                            devNode.Device.SimulatorCheckedStatus = false;
                            devNode.Checked = false;
                        }

                    }

                }

            }
        }

        private void IoTreeStatus_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is IoCommunicationTreeNode)
            {
                try
                {
                    IoCommunicationTreeNode commNode = e.Node as IoCommunicationTreeNode;

                    //加载模拟器显示参数界面配置界面
                    if (commNode.Communication.CommunicateDriver != null)
                    {
                        ScadaCommunicateKernel communicateKernel = commNode.Communication.CommunicateDriver as ScadaCommunicateKernel;
                        SimulatorKernelControl ctrl = communicateKernel.CreateSimulatorKernelControl();
                        ctrl.Dock = DockStyle.Fill;
                        ctrl.SetUIParameter(commNode.Communication, commNode.Communication.Devices);
                        SimulatorPanel.Controls.Clear();
                        SimulatorPanel.Controls.Add(ctrl);
                    }
                }
                catch (Exception ex)
                {
                    IOMonitorUIManager.DisplyException(ex);
                }
            }
        }
    }
}
