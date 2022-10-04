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
    public class IOServerNode : TreeNode
    {
        private int mCommunicationNumber = 0;
        public int CommunicationNumber
        {
            get
            {
                mCommunicationNumber++;
                return mCommunicationNumber;
            }
        }
        public override string ToString()
        {
            return Text.ToString();
        }

        public IO_SERVER Server = null;
        public string Project = "";
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
            this.Text = LocalIp.GetLocalIp();
            this.ToolTipText = Server.SERVER_REMARK;
        
        }
        public void AddChildenNode(IOCommunicationNode node)
        {
            this.Nodes.Add(node);
            node.ChangedNode();
         

        }
        public IOServerNode(IO_SERVER mServer,string mProject)
        {
            Server = mServer;
            Project = mProject;
            mContextMenu = new ContextMenu();
            mContextMenu.MenuItems.Add(new MenuItem("新建通道")
            {
                Tag = 1
            });
            mContextMenu.MenuItems.Add(new MenuItem("编辑采集站")
            {
                Tag = 2
            });
            mContextMenu.MenuItems.Add(new MenuItem("删除采集站工程")
            {
                Tag = 3
            });
            mContextMenu.MenuItems[0].Click += IOServerNode_Click;
            mContextMenu.MenuItems[1].Click += IOServerNode_Click;
            this.ContextMenu = mContextMenu;
            if (Server.SERVER_ID==null || Server.SERVER_ID=="")
            {
         
                Server.SERVER_IP = LocalIp.GetLocalIp();
                Server.SERVER_ID= IOManagerUIManager.ipToLong(Server.SERVER_IP);
            }
           
            this.Text = LocalIp.GetLocalIp();
            ///当前加载工程的文件路径
            this.Tag = Project;
         
            this.SelectedImageIndex = 0;
            this.StateImageIndex = 0;
            this.ImageIndex = 0;
            this.ExpandAll();
         

        }
    

        

        private   void IOServerNode_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch(item.Tag.ToString())
            {
                case "1":
                      IOManagerUIManager.CreateIOCommunicationNode();
                    break;
                case "2":
                      IOManagerUIManager.EditIOServerNode();
                    
                    break;
                case "3":
                    if(MessageBox.Show(this.TreeView.FindForm(),"删除采集站工程","删除提示",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        this.Remove();
                    }
                 

                    break;
            }
           
        }
    }
}
