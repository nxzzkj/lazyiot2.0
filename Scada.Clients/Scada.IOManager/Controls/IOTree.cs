using Scada.Model;
using IOManager.Core;
using System;
using System.Collections.Generic;
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
namespace IOManager.Controls
{
    public class IOTree : TreeView
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == 0x0014) // 禁掉清除背景消息WM_ERASEBKGND

                return;

            base.WndProc(ref m);

        }

        public IOServerNode GetServerNode(IO_SERVER server)
        {

            for(int i=0;i<this.Nodes.Count;i++)
            {
                if(this.Nodes[i] is IOServerNode)
                {
                    IOServerNode serNode = this.Nodes[i] as IOServerNode;
                    if(serNode.Server==server)
                    {
                        return serNode;
                    }
                }
               

            }
            return null;
        }
        public IOCommunicationNode GetCommunicationNode(IO_SERVER server, IO_COMMUNICATION communication)
        {

            for (int i = 0; i < this.Nodes.Count; i++)
            {
                if (this.Nodes[i] is IOServerNode)
                {
                    IOServerNode serNode = this.Nodes[i] as IOServerNode;
                    if (serNode.Server == server)
                    {
                        for (int j = 0; j < serNode.Nodes.Count; j++)
                        {
                            if (serNode.Nodes[j] is IOCommunicationNode)
                            {
                                IOCommunicationNode commNode = serNode.Nodes[j] as IOCommunicationNode;
                                if (commNode.Communication == communication)
                                {
                                    return commNode;
                                }
                            }


                        }
                    }
                }


            }

           
            return null;
        }
        public IOTree()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer |
                  ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
       
            this.LabelEdit = false;
            this.NodeMouseDoubleClick += IOTree_NodeMouseDoubleClick;
            this.NodeMouseClick += IOTree_NodeMouseClick;
            this.HideSelection = false;
        }

        private   void IOTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Clicks != 2)
                return;
            if (e.Node is IOCommunicationNode)
            {
                //编辑通讯节点
                  IOManagerUIManager.EditIOCommunicationNode((IOCommunicationNode)e.Node);
            }
            else if (e.Node is IODeviceNode)
            {
                  IOManagerUIManager.EditIODeviceNode((IODeviceNode)e.Node);
            }
            else if (e.Node is IOServerNode)
            {
                  IOManagerUIManager.EditIOServerNode();
            }
        }

        private   void IOTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Clicks != 1)
                return;
             if (e.Node is IODeviceNode)
            {
                IOCommunicationNode comNode = e.Node.Parent as IOCommunicationNode;
                IOServerNode sNode = comNode.Parent as IOServerNode;
                IODeviceNode dNode = e.Node as IODeviceNode;
                  IOManagerUIManager.OpenDeviceParas(sNode.Server, comNode.Communication, dNode.Device);
            }
            
        }

       

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // IOTree
            // 
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ItemHeight = 28;
            this.ResumeLayout(false);

        }
    }
}
