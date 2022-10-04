using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.Model;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
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
namespace ScadaCenterServer.Controls
{
    public partial class Search : UserControl
    {
        public IO_DEVICE Device = null;
        public IO_SERVER Server = null;
        public IO_COMMUNICATION Communication = null;
        public event EventHandler SearchClick;
        public event EventHandler SelectedIndexChanged = null;




        public Search()
        {
            InitializeComponent();
            this.Load += Search_Load;
        }

        private void Search_Load(object sender, EventArgs e)
        {
            //第一次打开要加载目录树结构
            hsComboBoxDevices.SelectedIndexChanged += hsComboBox_SelectedIndexChanged;

        }



        public void SetSelectItem(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device)
        {
            Device = device;
            Communication = communication;
            Server = server;
            TreeNode[] finders = this.hsComboBoxDevices.TreeView.Nodes.Find(device.IO_DEVICE_ID, true);
            if (finders.Length > 0)
            {
                IoDeviceTreeNode node = finders[0] as IoDeviceTreeNode;
                this.hsComboBoxDevices.ShowItem(node);

            }
        }

        /// <summary>
        /// 首先要加载树结构
        /// </summary>
        public async void LoadTreeProject()
        {

            if (this.hsComboBoxDevices.TreeView.Nodes.Count <= 0)
            {
                await TaskHelper.Factory.StartNew(() =>
                {
                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke(new EventHandler(delegate
                        {

                            try
                            {
                                this.hsComboBoxDevices.TreeView.Nodes.Clear();

                                int num = IOCenterManager.IOProject.Servers.Count + IOCenterManager.IOProject.Communications.Count + IOCenterManager.IOProject.Devices.Count;
                                TreeNode mainNode = new TreeNode();
                                mainNode.ImageIndex = 0;
                                mainNode.SelectedImageIndex = 0;
                                mainNode.Text = PubConstant.Product;

                                ///加载采集站
                                for (int i = 0; i < IOCenterManager.IOProject.Servers.Count; i++)
                                {

                                    IoServerTreeNode serverNode = new IoServerTreeNode();
                                    serverNode.Server = IOCenterManager.IOProject.Servers[i];
                                    serverNode.InitNode();
                                    List<Scada.Model.IO_COMMUNICATION> serverComms = IOCenterManager.IOProject.Communications.FindAll(x => x.IO_SERVER_ID == IOCenterManager.IOProject.Servers[i].SERVER_ID);
                                    for (int c = 0; c < serverComms.Count; c++)//通道
                                    {
                                        IoCommunicationTreeNode commNode = new IoCommunicationTreeNode();
                                        commNode.Communication = serverComms[c];
                                        commNode.Server = IOCenterManager.IOProject.Servers[i];
                                        commNode.InitNode();
                                        List<Scada.Model.IO_DEVICE> commDevices = IOCenterManager.IOProject.Devices.FindAll(x => x.IO_SERVER_ID == IOCenterManager.IOProject.Servers[i].SERVER_ID && x.IO_COMM_ID == serverComms[c].IO_COMM_ID);
                                        for (int d = 0; d < commDevices.Count; d++)//设备
                                        {
                                            IoDeviceTreeNode deviceNode = new IoDeviceTreeNode();
                                            deviceNode.Device = commDevices[d];
                                            deviceNode.Server = IOCenterManager.IOProject.Servers[i];
                                            deviceNode.Communication = serverComms[c];
                                            //挂载右键菜单

                                            deviceNode.InitNode();
                                            commNode.Nodes.Add(deviceNode);

                                        }

                                        serverNode.Nodes.Add(commNode);
                                    }

                                    mainNode.Nodes.Add(serverNode);

                                }
                                mainNode.Expand();
                                this.hsComboBoxDevices.TreeView.Nodes.Add(mainNode);


                            }
                            catch
                            {

                            }
                        }));
                    }
                });
            }
        }
        private void hsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (hsComboBoxDevices.SelectedItem != null)
            {

                IoDeviceTreeNode node = hsComboBoxDevices.SelectedItem as IoDeviceTreeNode;
                //如果选择的还是同一个设备，就不做处理
                if (this.Device != node.Device)
                {

                    this.Server = node.Server;
                    this.Communication = node.Communication;
                    this.Device = node.Device;

                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(sender, e);
                    }
                }

            }

        }
        private void ucBtn_Search_BtnClick(object sender, EventArgs e)
        {
            if (hsComboBoxDevices.SelectedItem == null)
            {
                FrmDialog.ShowDialog(this, "请选择要查询历史数据的设备节点!", "提示");
                return;


            }
            if (dateStart.Value > dateEnd.Value)
            {

                FrmDialog.ShowDialog(this, "开始时间必须小于结束时间!", "提示");
                return;
            }
            if ((dateEnd.Value - dateStart.Value).Days >= 31)
            {

                FrmDialog.ShowDialog(this, "您选择的时间段太长，时间段不能超过31天!", "提示");
                return;
            }

            IoDeviceTreeNode node = hsComboBoxDevices.SelectedItem as IoDeviceTreeNode;
            if (SearchClick != null)
            {
                this.Server = ((IoServerTreeNode)node.Parent.Parent).Server;
                this.Communication = ((IoCommunicationTreeNode)node.Parent).Communication;
                SearchClick(node.Device, e);
            }
        }
        public DateTime StartDate
        {
            get { return dateStart.Value; }
            set
            {
                dateStart.Value = value;
            }
        }
        public DateTime EndDate
        {
            get { return dateEnd.Value; }
            set
            {
                dateEnd.Value = value;
            }
        }
    }
}
