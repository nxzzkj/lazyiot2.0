using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Model;

namespace Scada.BatchCommand
{
public    class IOFlowTree:TreeView
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
     
        public IOFlowTree():base()
        {

            SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            this.NodeMouseDoubleClick += IOFlowTree_NodeMouseDoubleClick;
            this.FullRowSelect = true;
            this.BeforeSelect += IOFlowTree_BeforeSelect;
         
        }
        public bool IsParaNodeSelected = false;
        private void IOFlowTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node != null)
            {
                if(IsParaNodeSelected)
                {
                    if (e.Node.GetType() != typeof(BatchCommandParaTreeNode))
                    {
                        e.Cancel = true;  //不让选中禁用节点
                    }

                }
               
            }
        }

        private void IOFlowTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Clicks == 2)
            {


                if (e.Node is BatchCommandDeviceTreeNode)
                {

                    BatchCommandServerTreeNode ServerNode = e.Node.Parent.Parent as BatchCommandServerTreeNode;
                    BatchCommandCommunicationTreeNode commNode = e.Node.Parent as BatchCommandCommunicationTreeNode;
                    BatchCommandDeviceTreeNode deviceNode = e.Node as BatchCommandDeviceTreeNode;
                    deviceNode.Nodes.Clear();
                    if (deviceNode.Device.IOParas != null)
                    {
                        for (int p = 0; p < deviceNode.Device.IOParas.Count; p++)
                        {
                            BatchCommandParaTreeNode paraNode = new BatchCommandParaTreeNode();
                          
                            paraNode.Parament = deviceNode.Device.IOParas[p];
                            paraNode.InitNode();
                            deviceNode.Nodes.Add(paraNode);
                        }

                    }
                    deviceNode.Checked = true;
                }
            }

         

        }


        public void InitTree(IO_SERVER Server ,List<IO_COMMUNICATION> Communications)
        {
            
            if (this.Nodes.Count > 0)
                return;
            if (Server == null)
                return;
            if (Communications == null)
                return;
                this.Nodes.Clear();
                BatchCommandServerTreeNode ServerNode = new BatchCommandServerTreeNode();
                ServerNode.Server = Server;
         
                ServerNode.InitNode();
               for(int i=0;i< Communications.Count;i++)
                {
                    BatchCommandCommunicationTreeNode commNode = new BatchCommandCommunicationTreeNode();
                    commNode.Communication = Communications[i];
                    commNode.Server = Server;
                    commNode.InitNode();
                    if (Communications[i].Devices!=null)
                    {
                        for (int d = 0; d < Communications[i].Devices.Count; d++)
                        {
                            BatchCommandDeviceTreeNode deviceNode = new BatchCommandDeviceTreeNode();
                            deviceNode.Device = Communications[i].Devices[d];
                            deviceNode.Server = Server;
                            deviceNode.Communication = Communications[i];
                            deviceNode.InitNode();
                            commNode.Nodes.Add(deviceNode);
                        }

                    }
                   
                    ServerNode.Nodes.Add(commNode);
                }
                this.Nodes.Add(ServerNode);

       
          

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // IOFlowTree
            // 
            this.FullRowSelect = true;
            this.LineColor = System.Drawing.Color.Maroon;
            this.ShowNodeToolTips = true;
            this.ResumeLayout(false);

        }
    }
}
