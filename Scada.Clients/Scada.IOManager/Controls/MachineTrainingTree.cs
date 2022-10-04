
 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
//在调试过程中如果发现相关的bug或者代码错误等问题可直接微信联系作者。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using IOManager.Core;
using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOManager.Controls
{
  public   class MachineTrainingTree: TreeView
    {
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
        public IOTree IOTree = null;
        public void InitTree(string ServerId,IOTree ioTree)
        {
            Server_ID = ServerId;
            IOTree = ioTree;
               MainNode = new TreeNode();
            MainNode.Text = "机器学习任务";
            MainNode.SelectedImageIndex = 0;
            MainNode.ImageIndex = 0;
            MainNode.StateImageIndex = 0;
            MainNode.ExpandAll();
            this.ContextMenu = new ContextMenu();
            this.ContextMenu.MenuItems.Add(new MenuItem("添加机器学习任务") { Tag = "1" });


            this.ContextMenu.MenuItems[0].Click += MachineTrainingTree_Click;
            this.Nodes.Clear();
            this.Nodes.Add(MainNode);
        }

        public string Server_ID { set; get; }

        private void MachineTrainingTree_Click(object sender, EventArgs e)
        {
            AddTrain();


        }
        private void AddTrain()
        {
            var model = IOManagerUIManager.MachineTrainingModelEdit(this.IOTree);
            if (model != null)
            {
                model.SERVER_ID = Server_ID;
                model.Id = GUIDToNormalID.GuidToInt();
                MachineTrainingTreeNode machineTrainingTree = new MachineTrainingTreeNode();
                machineTrainingTree.TrainingModel = model;
                machineTrainingTree.Text = model.TaskName;
                machineTrainingTree.ChangeNode();
                machineTrainingTree.ExpandAll();
              
                MainNode.Nodes.Add(machineTrainingTree);
            }
        }
        #region 节点相关操作
        public void DeleteTrainModel()
        {
            if (this.SelectedNode is MachineTrainingTreeNode trainNode)
            {
                trainNode.DeleteTrainModel();
            }
            else if (this.SelectedNode is MachineTrainingConditionTreeNode conditionNode)
            {
                conditionNode.DeleteTrainConditionModel();
            }

        }
        public void AddTrainModel()
        {
            if (this.SelectedNode is MachineTrainingTreeNode trainNode)
            {
                trainNode.AddTrainConditionModel();
            }
            else  
            {
                AddTrain();
            }
        }
        public void EditTrainModel()
        {
            if (this.SelectedNode is MachineTrainingTreeNode trainNode)
            {
                trainNode.EditTrainModel();
            }
            else if (this.SelectedNode is MachineTrainingConditionTreeNode conditionNode)
            {
                conditionNode.EditTrainConditionModel();
            }

        }
            #endregion
            TreeNode MainNode = null;
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
        public MachineTrainingTree()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer |
                  ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            this.LabelEdit = false;
          
        }

     
 
    }
}
