using Scada.Model;
using IOManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOManager.Controls
{
    public class IOCommunicationNode : TreeNode
    {
        private int mDeviceNumber = 0;
        public int DeviceNumber
        {
            get
            {
                mDeviceNumber++;
                return mDeviceNumber;
            }
            set
            {
                mDeviceNumber = value;
            }
        }
        public IO_COMMUNICATION Communication = null;
        ContextMenu mContextMenu = null;
        private bool mEnableContextMenu = true;
        public bool EnableContextMenu
        {
            set
            {
                mEnableContextMenu = value;
                if (value)
                {
                    this.ContextMenu = mContextMenu;

                }
                else
                {
                    this.ContextMenu = null;
                }
            }
            get { return mEnableContextMenu; }
        }
        public void ChangedNode()
        {
         
            this.Text = Communication.IO_COMM_LABEL+"["+ Communication.IO_COMM_NAME + "]";
            this.ToolTipText = "ID="+Communication.IO_COMM_ID+" "+ Communication.IO_COMM_REMARK;
        
        }
        public override string ToString()
        {
            return Text.ToString();
        }
        public void AddChildenNode(IODeviceNode node)
        {
            this.Nodes.Add(node);
            node.ChangedNode();
           
        }
        private   void TreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (TreeView.SelectedNode is IOCommunicationNode)
            {
                //编辑通讯节点
                  IOManagerUIManager.EditIOCommunicationNode(this);
            }
        }

        public IOCommunicationNode()
        {
            Communication = new IO_COMMUNICATION();
               mContextMenu = new ContextMenu();
            mContextMenu.MenuItems.Add(new MenuItem("删除通讯通道")
            {
                Tag = 1
            });
            mContextMenu.MenuItems.Add(new MenuItem("修改通讯通道") { Tag = 2 });
            mContextMenu.MenuItems.Add(new MenuItem("新增设备") { Tag = 3 });
            mContextMenu.MenuItems.Add(new MenuItem("粘贴设备") { Tag = 5 });
            mContextMenu.MenuItems.Add(new MenuItem("模拟器配置") { Tag = 4 });
            mContextMenu.MenuItems[0].Click += IOCommunicationNode_Click;
            mContextMenu.MenuItems[1].Click += IOCommunicationNode_Click;
            mContextMenu.MenuItems[2].Click += IOCommunicationNode_Click;
            mContextMenu.MenuItems[3].Click += IOCommunicationNode_Click;
            mContextMenu.MenuItems[4].Click += IOCommunicationNode_Click;

            this.ContextMenu = mContextMenu;
            Communication.IO_COMM_ID = GUIDToNormalID.GuidToLongID().ToString();

            this.SelectedImageIndex = 1;
            this.StateImageIndex = 1;
            this.ImageIndex = 1;
            ChangedNode();
        }

        private    void IOCommunicationNode_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch(item.Tag.ToString())
            {
                case "1":
                    if (MessageBox.Show(this.TreeView.FindForm(), "是否要删除" + this.Communication.IO_COMM_LABEL + "通讯通道?", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Remove();
                    }
                    break;
                case "2":
                      IOManagerUIManager.EditIOCommunicationNode(this);
                    break;
                case "3":
                      IOManagerUIManager.CreateIODeviceNode();
                    break;
                case "4":
                    {
                        //模拟器的实现
                        IOManagerUIManager.EditIOCommunicationSimulatorNode(this);
                    }
                    break;
                case "5":
                    {
                        //粘贴设备

                      

                        IO_DEVICE newDevice = IOManagerUIManager.PasteIODeviceNode(this.Communication.IO_SERVER_ID,this.Communication.IO_COMM_ID);
                        if(newDevice!=null)
                        {
                            IODeviceNode newNode = new IODeviceNode();
                            newNode.Device = newDevice;
                            newNode.ChangedNode();
                            this.Nodes.Insert(0, newNode);
                        }
                    }
                    break;
               
            }
        
        }

        
    }
}
