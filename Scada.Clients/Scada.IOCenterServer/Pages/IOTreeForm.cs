using Scada.Controls;
using Scada.Model;
using ScadaCenterServer.Controls;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
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
namespace ScadaCenterServer.Pages
{
    public partial class IOTreeForm : DockForm
    {
        public IOTreeForm(Mediator m) : base(m)
        {

            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.CloseButton = false;
        }
        public IOTreeForm()
        {

            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
   
        public Task CommunicationStatus(IO_COMMUNICATION comm, bool status)
        {
            var treeTask = TaskHelper.Factory.StartNew(() => {
                try
                {
                    if (ioTree.IsHandleCreated && ioTree.InvokeRequired)
                    {
                        ioTree.BeginInvoke(new EventHandler(delegate
                        {
                            ioTree.BeginUpdate();
                            if (this.ioTree.Nodes.Count < 0)
                            {
                                return;
                            }

                            for (int i = 0; i < this.ioTree.Nodes.Count; i++)
                            {

                                TreeNode[] tn = this.ioTree.Nodes[i].Nodes.Find(comm.IO_COMM_ID.ToString(), false);
                                if (tn.Length == 1)
                                {
                                    IoCommunicationTreeNode commNode = tn[0] as IoCommunicationTreeNode;
                                    if (commNode != null)
                                    {


                                        if (status)
                                        {
                                            commNode.ForeColor = Color.Green;
                                            commNode.Tag = null;
                                            commNode.ImageIndex = 3;
                                            commNode.StateImageIndex = 3;

                                        }
                                        else
                                        {
                                            commNode.ForeColor = Color.Black;

                                            commNode.ImageIndex = 3;
                                            commNode.StateImageIndex = 3;

                                        }

                                    }

                                }
                            }
                            ioTree.EndUpdate();
                        }));
                    }
                }
                catch
                {

                }
            });
            return treeTask;
        }


        public override TabTypes TabType
        {
            get
            {
                return TabTypes.IOCatalog;
            }
        }
        public IoServerTreeNode GetServerNode(string server_id)
        {
            if (this.ioTree.Nodes.Count > 0)
            {
                TreeNode[] fNodes = this.ioTree.Nodes[0].Nodes.Find(server_id, false);
                if (fNodes.Length > 0)
                {
                    if (fNodes[0] is IoServerTreeNode)
                    {
                        return fNodes[0] as IoServerTreeNode;
                    }
                }

            }

            return null;
        }
        private void 实时数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode Node = ioTree.SelectedNode as IoDeviceTreeNode;
                this.mediator.RealQueryWorkForm.InitDevice(Node.Server, Node.Communication, Node.Device);
            }

        }
        /// <summary>
        /// 当前实时设备状态改变信息
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public void DeviceStatus(List<IO_DEVICE> devices)
        {
            ioTree.BeginInvoke(new EventHandler(delegate
            {
                devices.ForEach(delegate (IO_DEVICE device)
                {
                    try
                    {
                        TreeNode[] mNodes = this.ioTree.Nodes[0].Nodes.Find(device.IO_DEVICE_ID.ToString(), true);
                        if (mNodes != null && mNodes.Length > 0)
                        {
                            if (mNodes[0] is IoDeviceTreeNode deviceNode)
                            {
                                deviceNode.Device.IO_DEVICE_STATUS = device.IO_DEVICE_STATUS;
                                if (deviceNode.Device.IO_DEVICE_STATUS == 1)
                                {
                                    deviceNode.ForeColor = Color.Green;
                                    deviceNode.Parent.ForeColor = Color.Green;
                                    deviceNode.Parent.Parent.ForeColor = Color.Green;
                                    deviceNode.SelectedImageIndex = 5;
                                    deviceNode.ImageIndex = 5;

                                    deviceNode.Tag = null;
                                }

                                else
                                {
                                    deviceNode.ForeColor = Color.Red;
                                    deviceNode.SelectedImageIndex = 4;
                                    deviceNode.ImageIndex = 4;
                                    deviceNode.Tag = DateTime.Now.ToString("yyyy-MM-dd");

                                }
                            }

                        }
                    }
                    catch
                    {

                    }

                });

            }));

        }
        public void ServerStatus(IO_SERVER server, bool status)
        {
            this.ioTree.BeginInvoke(new EventHandler(delegate
            {
            
                if (this.ioTree.Nodes.Count < 0)
                {
                    return;
                }
                ioTree.BeginUpdate();
                for (int i = 0; i < this.ioTree.Nodes[0].Nodes.Count; i++)
                {

                    TreeNode[] tn = this.ioTree.Nodes[0].Nodes.Find(server.SERVER_ID.ToString(), false);
                    if (tn.Length == 1)
                    {
                        IoServerTreeNode serverNode = tn[0] as IoServerTreeNode;
                        if (serverNode != null)
                        {


                            if (status)
                            {
                                serverNode.ForeColor = Color.Green;
                                serverNode.Tag = null;
                                serverNode.ImageIndex = 2;
                                serverNode.StateImageIndex = 2;
                                serverNode.Text = server.SERVER_NAME + "已上线";
                            }
                            else
                            {
                                serverNode.ForeColor = Color.Red;
                                serverNode.Tag = DateTime.Now.ToString("yyyy-MM-dd");
                                serverNode.ImageIndex = 1;
                                serverNode.StateImageIndex = 1;
                                serverNode.Text = server.SERVER_NAME + "未上线";
                            }

                        }

                    }
                }
                ioTree.EndUpdate();
            }));

        }

        private void 历史查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode Node = ioTree.SelectedNode as IoDeviceTreeNode;
                this.mediator.HistoryQueryWorkForm.InitDevice(Node.Server, Node.Communication, Node.Device);
            }

        }

        private void 历史报警ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode Node = ioTree.SelectedNode as IoDeviceTreeNode;
                this.mediator.HistoryAlarmQueryWorkForm.InitDevice(Node.Server, Node.Communication, Node.Device);
            }
        }

        private void 下置查询toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode Node = ioTree.SelectedNode as IoDeviceTreeNode;
                this.mediator.SendCommandQueryWorkForm.InitDevice(Node.Server, Node.Communication, Node.Device);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode Node = ioTree.SelectedNode as IoDeviceTreeNode;
                this.mediator.AlarmConfigQueryWorkForm.InitDevice(Node.Server, Node.Communication, Node.Device);
            }
        }

        private void 统计查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IoDeviceTreeNode Node = ioTree.SelectedNode as IoDeviceTreeNode;
                this.mediator.HistoryStaticsQueryWorkForm.InitDevice(Node.Server, Node.Communication, Node.Device);
            }
        }
        private void 下置命令toolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (ioTree.SelectedNode != null && ioTree.SelectedNode is IoDeviceTreeNode)
            {
                IOCenterManager.SendCommand(ioTree);
            }
             
        }
    }
}
