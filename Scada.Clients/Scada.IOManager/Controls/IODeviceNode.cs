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
  public  class IODeviceNode: TreeNode
    {
       
        public IO_DEVICE Device = null;
        ContextMenu mContextMenu = null;
        private bool mEnableContextMenu = true;
        public bool EnableContextMenu
        {
            set
            {
                mEnableContextMenu = value;
                if(value)
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
            this.Text = Device.IO_DEVICE_LABLE+"["+ Device.IO_DEVICE_NAME + "]";
            this.ToolTipText ="ID="+ Device.IO_DEVICE_ID+" Address="+Device.IO_DEVICE_ADDRESS;
          
        }

       

        public IODeviceNode()
        {
            Device = new IO_DEVICE();
               mContextMenu = new ContextMenu();
    
            mContextMenu.MenuItems.Add(new MenuItem("删除设备")
            {
                Tag =1
            });
            mContextMenu.MenuItems.Add(new MenuItem("修改设备") { Tag=2 });
            mContextMenu.MenuItems.Add(new MenuItem("复制设备") { Tag = 4 });
            mContextMenu.MenuItems.Add(new MenuItem("粘贴设备") { Tag = 5 });
            mContextMenu.MenuItems.Add(new MenuItem("编辑IO表") { Tag = 3 });
            mContextMenu.MenuItems[0].Click += DeviceNode_Click;
            mContextMenu.MenuItems[1].Click += DeviceNode_Click;
            mContextMenu.MenuItems[2].Click += DeviceNode_Click;
            mContextMenu.MenuItems[3].Click += DeviceNode_Click;
            mContextMenu.MenuItems[4].Click += DeviceNode_Click;
            this.ContextMenu = mContextMenu;
            Device.IO_DEVICE_ID = GUIDToNormalID.GuidToLongID().ToString();
            this.SelectedImageIndex = 2;
            this.StateImageIndex = 2;
            this.ImageIndex = 2;
            ChangedNode();
        }

        private   void DeviceNode_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch (item.Tag.ToString())
            {
                case "1":
                    if (MessageBox.Show(this.TreeView.FindForm(), "是否要删除" + this.Device.IO_DEVICE_LABLE + "设备?", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Remove();
                    }
                    break;
                case "2":
                      IOManagerUIManager.EditIODeviceNode(this);
                    break;
                case "3":
                    {
                        IOCommunicationNode comNode = this.Parent as IOCommunicationNode;
                        IOServerNode sNode = comNode.Parent as IOServerNode;
                          IOManagerUIManager.OpenDeviceParas(sNode.Server, comNode.Communication, this.Device);
                    }

                    break;
                case "4":
                    {
                        IOCommunicationNode comNode = this.Parent as IOCommunicationNode;
                        IOServerNode sNode = comNode.Parent as IOServerNode;
                        IOManagerUIManager.CopyIODeviceNode(this.Device);//复制设备信息
                    }

                    break;
                case "5":
                    {
                        IOCommunicationNode comNode = this.Parent as IOCommunicationNode;
                        IOServerNode sNode = comNode.Parent as IOServerNode;
                        IO_DEVICE newDevice=  IOManagerUIManager.PasteIODeviceNode(sNode.Server.SERVER_ID, comNode.Communication.IO_COMM_ID);
                        if(newDevice!=null)
                        {
                            IODeviceNode newNode = new IODeviceNode();
                            newNode.Device = newDevice;
                            newNode.ChangedNode();
                            comNode.Nodes.Insert(this.Index, newNode);
                        }
                    }

                    break;
            }
        }
    }
}
