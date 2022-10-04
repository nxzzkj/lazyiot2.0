using Scada.Controls.Forms;
using Scada.DBUtility;
using Scada.Model;
using ScadaCenterServer.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaCenterServer.Controls
{
    public partial class HistoryEventSearch : UserControl
    {
        public HistoryEventSearch()
        {
            InitializeComponent();
            this.Load += RealAlarmSearch_Load;
        }

        private void RealAlarmSearch_Load(object sender, EventArgs e)
        {

            List<KeyValuePair<string, string>> lstAlarnTypeCom = new List<KeyValuePair<string, string>>();
            lstAlarnTypeCom.Add(new KeyValuePair<string, string>("", "全部类型"));

            foreach (ScadaEvent scadaEvent in Enum.GetValues(typeof(ScadaEvent)))
            {
                lstAlarnTypeCom.Add(new KeyValuePair<string, string>(scadaEvent.ToString(), scadaEvent.ToString()));
            }


            ucEventType.Source = lstAlarnTypeCom;
            ucEventType.SelectedIndex = 0;
            hsComboBoxDevices.IsSelectbaseNode = false;
        }



        public IO_DEVICE Device = null;
        public IO_SERVER Server = null;
        public IO_COMMUNICATION Communication = null;
        public event EventHandler SearchClick;
        public event EventHandler SelectedIndexChanged = null;
 
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

        private void hsComboBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hsComboBoxDevices.SelectedItem != null)
            {
                if(hsComboBoxDevices.SelectedItem is IoDeviceTreeNode node)
                {
                    this.Server = node.Server;
                    this.Communication = node.Communication;
                    this.Device = node.Device;

                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(node, e);
                    }
                }
                else if (hsComboBoxDevices.SelectedItem is IoCommunicationTreeNode cnode)
                {
                    this.Server = cnode.Server;
                    this.Communication = cnode.Communication;
                    this.Device = null;

                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(cnode, e);
                    }
                }
                else if (hsComboBoxDevices.SelectedItem is IoServerTreeNode snode)
                {
                    this.Server = snode.Server;
                    this.Communication = null;
                    this.Device = null;

                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(snode, e);
                    }
                }

            }
        }

        private void ucBtnExt13_BtnClick(object sender, EventArgs e)
        {
            if (hsComboBoxDevices.SelectedItem == null)
            {
                FrmDialog.ShowDialog(this, "请选择要查询数据的设备节点!", "提示");
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

            if (hsComboBoxDevices.SelectedItem is IoDeviceTreeNode node)
            {
                this.Server = node.Server;
                this.Communication = node.Communication;
                this.Device = node.Device;

                if (SearchClick != null)
                {
                    SearchClick(node.Device, e);
                }
            }
            else if (hsComboBoxDevices.SelectedItem is IoCommunicationTreeNode cnode)
            {
                this.Server = cnode.Server;
                this.Communication = cnode.Communication;
                this.Device = null;
                if (SearchClick != null)
                {
                  
                    SearchClick(cnode.Communication, e);
                }
            }
            else if (hsComboBoxDevices.SelectedItem is IoServerTreeNode snode)
            {
                this.Server = snode.Server;
                this.Communication = null;
                this.Device = null;

                if (SearchClick != null)
                {
                
                    SearchClick(snode.Server, e);
                }
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
        public string EventType
        {
            get { return string.IsNullOrEmpty(this.ucEventType.SelectedValue)==false?this.ucEventType.SelectedValue:""; }
        }
       
    }
}
