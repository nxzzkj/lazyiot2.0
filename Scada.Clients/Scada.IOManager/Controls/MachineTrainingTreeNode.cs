
 
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
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOManager.Controls
{
    /// <summary>
    /// 机器学习任务
    /// </summary>
    public class MachineTrainingTreeNode : TreeNode
    {
        public ScadaMachineTrainingModel TrainingModel = new ScadaMachineTrainingModel();
        public MachineTrainingTreeNode()
        {
            this.ContextMenu = new ContextMenu();
            this.ContextMenu.MenuItems.Add(new MenuItem("删除任务") {  Tag="1"});
            this.ContextMenu.MenuItems.Add(new MenuItem("编辑任务") { Tag="2" });
            this.ContextMenu.MenuItems.Add(new MenuItem("添加工况") { Tag = "3" });
            this.ContextMenu.MenuItems[0].Click += MachineTrainingTreeNode_Click;
            this.ContextMenu.MenuItems[1].Click += MachineTrainingTreeNode_Click;
            this.ContextMenu.MenuItems[2].Click += MachineTrainingTreeNode_Click;
            TrainingModel.Id = GUIDToNormalID.GuidToInt();
            this.SelectedImageIndex = 1;
            this.StateImageIndex = 1;
            this.ImageIndex = 1;
        }
        public void ChangeNode()
        {
            if (TrainingModel != null)
                this.Text = TrainingModel.ToString();

        }
        /// <summary>
        /// 删除训练模型任务
        /// </summary>
        public void DeleteTrainModel()
        {
            if (MessageBox.Show(this.TreeView.FindForm(), "删除机器训练任务", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Remove();
            }

        }
        public void EditTrainModel()
        {
            MachineTrainingTree parent = this.TreeView as MachineTrainingTree;
            var model = IOManagerUIManager.MachineTrainingModelEdit(parent.IOTree, TrainingModel);
            if (model != null)
            {
                model.Id = TrainingModel.Id;
                model.SERVER_ID = TrainingModel.SERVER_ID;
                this.Text = TrainingModel.TaskName;
            }
        }
        public void AddTrainConditionModel()
        {
            MachineTrainingTree parent = this.TreeView as MachineTrainingTree;
            var model = IOManagerUIManager.MachineTrainingConditionEdit(TrainingModel, parent.IOTree);
            if (model != null)
            {
                MachineTrainingConditionTreeNode conditionNode = new MachineTrainingConditionTreeNode();
                conditionNode.TrainingConditionModel = new ScadaMachineTrainingCondition();
                conditionNode.TrainingConditionModel = model;
                conditionNode.TrainingConditionModel.TaskId = TrainingModel.Id;
                conditionNode.TrainingConditionModel.SERVER_ID = TrainingModel.SERVER_ID;
                conditionNode.Text = model.ConditionTitle + "[" + model.ConditionName + "]";
                this.Nodes.Add(conditionNode);
            }
        }
        private void MachineTrainingTreeNode_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch (item.Tag.ToString())
            {

                case "3":
                    {
                        AddTrainConditionModel();
                    }

                    break;
                case "2":
                    {
                        EditTrainModel();
                    }

                    break;
                case "1":

                    DeleteTrainModel();
                    break;
            }
        }
    }

    /// <summary>
    /// 机器学习任务所属工况
    /// </summary>
    public class MachineTrainingConditionTreeNode : TreeNode
    {
        public ScadaMachineTrainingCondition TrainingConditionModel = new ScadaMachineTrainingCondition();
        public MachineTrainingConditionTreeNode()
        {
            this.ContextMenu = new ContextMenu();
            this.ContextMenu.MenuItems.Add(new MenuItem("删除工况") { Tag = "1" });
            this.ContextMenu.MenuItems.Add(new MenuItem("编辑工况") { Tag = "2" });
        
            this.ContextMenu.MenuItems[0].Click += MachineTrainingTreeNode_Click;
            this.ContextMenu.MenuItems[1].Click += MachineTrainingTreeNode_Click;
             
            TrainingConditionModel.Id = GUIDToNormalID.GuidToInt();
           
            this.SelectedImageIndex = 2;
            this.StateImageIndex = 2;
            this.ImageIndex = 2;
        }
        public void ChangeNode()
        {
            if (TrainingConditionModel != null)
                this.Text = TrainingConditionModel.ToString();

        }
        public void  EditTrainConditionModel()
        {
            MachineTrainingTree parent = this.TreeView as MachineTrainingTree;
            MachineTrainingTreeNode trainingTreeNode = this.Parent as MachineTrainingTreeNode;
            var model = IOManagerUIManager.MachineTrainingConditionEdit(trainingTreeNode.TrainingModel, parent.IOTree, TrainingConditionModel);
            if (model != null)
            {

                model.TaskId = trainingTreeNode.TrainingModel.Id;
                model.SERVER_ID = trainingTreeNode.TrainingModel.SERVER_ID;
                model.Id = TrainingConditionModel.Id;
                this.Text = model.ConditionTitle + "[" + model.ConditionName + "]";
            }
        }
        public void DeleteTrainConditionModel()
        {
            if (MessageBox.Show(this.TreeView.FindForm(), "删除机器训练工况", "删除提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Remove();
            }

        }
        private void MachineTrainingTreeNode_Click(object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            switch (item.Tag.ToString())
            {

        
                case "2":
                    {
                        EditTrainConditionModel();
                    }

                    break;
                case "1":
                    DeleteTrainConditionModel();

                    break;
            }
        }
    }
}
