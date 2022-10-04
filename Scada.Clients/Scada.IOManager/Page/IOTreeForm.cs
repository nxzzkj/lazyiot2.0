using IOManager.Controls;
using Scada.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOManager.Page
{

     
    /*----------------------------------------------------------------
    // Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
    // 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
    // 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
    // 请大家尊重作者的劳动成果，共同促进行业健康发展。
    // 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
    // 创建者：马勇
    //----------------------------------------------------------------*/
    public partial class IOTreeForm : DockContent, ICobaltTab
    {
        public IOTreeForm()
        {
            InitializeComponent();
          
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public TreeNode SelectedNode
        {
            get { return this.IoTree.SelectedNode; }
        }
        public void AddMainNode(TreeNode tn)
        {
            if (this.IsHandleCreated)
            {
                this.IoTree.BeginInvoke(new EventHandler(delegate
            {
                IoTree.Nodes.Add(tn);

            }));
            }

          
        }
        public void AddChilndenNode(TreeNode tn, TreeNode Ptn)
        {
            if (this.IsHandleCreated)
            {
                this.IoTree.BeginInvoke(new EventHandler(delegate
            {
                Ptn.Nodes.Add(tn);

            }));
            }

      
        }
        public void BeginIOTreeUpdate()
        {
            this.IoTree.BeginUpdate();
        }
        public void EndIOTreeUpdate()
        {
            this.IoTree.EndUpdate();
        }
        public void ClearNode()
        {
            if (this.IsHandleCreated)
            {
                this.IoTree.BeginInvoke(new EventHandler(delegate
            {
                IoTree.Nodes.Clear();


            }));
            }
           
        }
        public IOTreeForm(Mediator m)
        {
            InitializeComponent();
            mediator = m;
            Control.CheckForIllegalCrossThreadCalls = false;
            this.machineTrainingTree.InitTree("",this.IoTree);
        }
        public IOServerNode FindServerTreeNode(Scada.Model.IO_SERVER Server)
        {
            for(int i=0;i<this.IoTree.Nodes.Count;i++)
            {
                if(this.IoTree.Nodes[i] is IOServerNode)
                {
                    IOServerNode sNode = this.IoTree.Nodes[i] as IOServerNode;
                    if (sNode.Server == Server)
                        return sNode;

                }
            }
            return null;
        }
        public IOCommunicationNode FindCommunicationTreeNode(Scada.Model.IO_SERVER Server, Scada.Model.IO_COMMUNICATION Communication)
        {
 
            for (int i = 0; i < this.IoTree.Nodes.Count; i++)
            {
                if (this.IoTree.Nodes[i] is IOServerNode)
                {
                    IOServerNode sNode = this.IoTree.Nodes[i] as IOServerNode;
                    if (sNode.Server == Server)
                    {
                        for (int j = 0; j < sNode.Nodes.Count; j++)
                        {
                            if (sNode.Nodes[j] is IOCommunicationNode)
                            {
                                IOCommunicationNode commNode = sNode.Nodes[j] as IOCommunicationNode;
                                if (commNode.Communication == Communication)
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
        public IODeviceNode FindDeviceTreeNode(Scada.Model.IO_SERVER Server, Scada.Model.IO_COMMUNICATION Communication,Scada.Model.IO_DEVICE Device)
        {
            for (int i = 0; i < this.IoTree.Nodes.Count; i++)
            {
                if (this.IoTree.Nodes[i] is IOServerNode)
                {
                    IOServerNode sNode = this.IoTree.Nodes[i] as IOServerNode;
                    if (sNode.Server == Server)
                    {
                        for (int j = 0; j < sNode.Nodes.Count; j++)
                        {
                            if (sNode.Nodes[j] is IOCommunicationNode)
                            {
                                IOCommunicationNode commNode = sNode.Nodes[j] as IOCommunicationNode;
                                if (commNode.Communication == Communication)
                                {
                                    for (int c = 0; c < commNode.Nodes.Count; c++)
                                    {
                                        if (commNode.Nodes[c] is IODeviceNode)
                                        {
                                            IODeviceNode deviceNode = commNode.Nodes[c] as IODeviceNode;
                                            if (deviceNode.Device == Device)
                                            {
                                                return deviceNode;

                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }

                }
            }
            return null;
        }
        private Mediator mediator = null;
        private string identifier;
        public TabTypes TabType
        {
            get
            {
                return TabTypes.Project;
            }
        }
        public string TabIdentifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
            }
        }

        #region 机器学习训练操作
        public void AddMachineMainNode(TreeNode tn)
        {
            if (this.IsHandleCreated)
            {
                this.machineTrainingTree.BeginInvoke(new EventHandler(delegate
                {
                    machineTrainingTree.Nodes.Add(tn);

                }));
            }


        }
        public void AddMachineChilndenNode(TreeNode tn, TreeNode Ptn)
        {
            if (this.IsHandleCreated)
            {
                if(Ptn!=null)
                {
                    this.machineTrainingTree.BeginInvoke(new EventHandler(delegate
                    {
                        Ptn.Nodes.Add(tn);

                    }));
                }
                else
                {
                    this.machineTrainingTree.BeginInvoke(new EventHandler(delegate
                    {
                        machineTrainingTree.Nodes[0].Nodes.Add(tn);

                    }));
                }
            
            }







        }
        public void ClearMachineNode()
        {
            if (this.IsHandleCreated)
            {
                this.machineTrainingTree.BeginInvoke(new EventHandler(delegate
                {
                    machineTrainingTree.Nodes.Clear();


                }));
            }

        }
        #endregion

        private void toolStripTrainingAdd_Click(object sender, EventArgs e)
        {
            AddTrainModel();
        }

        private void toolStripTrainingDel_Click(object sender, EventArgs e)
        {
            DeleteTrainModel();
        }

        private void toolStripTrainingEdit_Click(object sender, EventArgs e)
        {
            EditTrainModel();

        }
        public void  AddTrainModel()
        {
            this.machineTrainingTree.AddTrainModel();
        }
        public void EditTrainModel()
        {
            this.machineTrainingTree.EditTrainModel();
        }
        public void DeleteTrainModel()
        {
            this.machineTrainingTree.DeleteTrainModel();
        }
    }
}
