

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using ScadaCenterServer.Core;
using System;
using System.Windows.Forms;

namespace ScadaCenterServer.Controls
{
    public class BatchCommandTreeNode : TreeNode, INode
    {
        public Scada.Model.BatchCommandTaskModel  BatchCommandTask = null;

        public void InitNode()
        {
            if (BatchCommandTask != null)
            {
                this.Text = BatchCommandTask.CommandTaskTitle;
                this.Name = BatchCommandTask.Id.ToString();
            }
            this.SelectedImageIndex = 2;
            this.ImageIndex = 2;
            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.Items.Add("命令执行记录查询");
            this.ContextMenuStrip.Items[0].Click += MachineTrainTreeNode_Click;
           
        }

        private void MachineTrainTreeNode_Click(object sender, EventArgs e)
        {
            if (BatchCommandTask == null)
                return;
            ToolStripItem toolStripItem = sender as ToolStripItem;
            switch (toolStripItem.Text)
            {
                case "命令执行记录查询":
                    IOCenterManager.QueryFormManager.Mediator.OpenScadaBatchCommandTaskForm();
                    IOCenterManager.QueryFormManager.Mediator.ScadaBatchCommandTaskForm.InitTreeProject();
                    IOCenterManager.QueryFormManager.Mediator.ScadaBatchCommandTaskForm.SetBatchCommandTaskModel(BatchCommandTask);
                    break;
             
            }

        }
    }
}
