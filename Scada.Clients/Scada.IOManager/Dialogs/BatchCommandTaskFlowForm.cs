using IOManager.Core;
using Scada.BatchCommand;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
namespace IOManager.Dialogs
{
    public partial class BatchCommandTaskFlowForm : Form
    {
        public BatchCommandTaskFlowForm()
        {
            InitializeComponent();
            this.Load += BatchCommandTaskFlowForm_Load;
            this.batchCommandTaskGraph.OnShowProperties += BatchCommandTaskGraph_OnShowProperties;
        }
        public BatchCommandTaskModel CommandTaskModel = null;
        public void SetBatchCommandTask(BatchCommandTaskModel batchCommand)
        {
            this.batchCommandTaskGraph.InitMapArea();
            this.batchCommandTaskGraph.BatchCommandTask = new BatchCommandTask();
            this.batchCommandTaskGraph.BatchCommandTask.CreateCommandTaskFromDBModel(batchCommand);
        
            BatchCommandGraphEngineProject.IOServer = IOManagerUIManager.IOTreeNodeServer;
            BatchCommandGraphEngineProject.IOCommunications = IOManagerUIManager.IOTreeNodeCommunications;
            for (int i = 0; i < this.batchCommandTaskGraph.BatchCommandTask.Items.Count; i++)
            {
                BatchCommandItem commandItem = this.batchCommandTaskGraph.BatchCommandTask.Items[i];
                this.batchCommandTaskGraph.AddCommand(commandItem);
            }

        }
        private void BatchCommandTaskGraph_OnShowProperties(object sender, object[] props)
        {
            if (props.Length > 0)
                this.propertyGrid.SelectedObjects = props;
        }

        private void BatchCommandTaskFlowForm_Load(object sender, EventArgs e)
        {

        }

        private void toolStripCommandAdd_Click(object sender, EventArgs e)
        {
            string preId = "";
            if (batchCommandTaskGraph.Shapes.Count > 0)
            {

                if (batchCommandTaskGraph.Hover != null)
                    preId = batchCommandTaskGraph.Hover.BatchCommandItem.CommandID;
                else
                {
                    MessageBox.Show("请选择一个前置命令节点!");
                    return;
                }
            }
            else
            {
                preId = "";
            }
            float x = 100;
            float y = 100;
            float width = 250;
            float height = 250;
            if (batchCommandTaskGraph.Hover != null)
            {

                x = batchCommandTaskGraph.Hover.X + 20;

                y = batchCommandTaskGraph.Hover.Y + 20;
            }
            BatchCommandItemShape itemShape =     this.batchCommandTaskGraph.AddCommand(new BatchCommandItem()
            {
                CommandItemTitle = "未命名",
                PreCommandItemID = preId,
                BatchTask = this.batchCommandTaskGraph.BatchCommandTask,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                BatchCommandTaskId = this.batchCommandTaskGraph.BatchCommandTask.CommandTaskID,
                SERVER_ID = this.batchCommandTaskGraph.BatchCommandTask.SERVER_ID,
                Expand = 1

            });
            if (batchCommandTaskGraph.Hover != null)
                batchCommandTaskGraph.Hover.BatchCommandItem.NextCommandItemIDList.Add(itemShape.BatchCommandItem.CommandID);

            if (batchCommandTaskGraph.Shapes.Count > 0)
            {
                batchCommandTaskGraph.BatchCommandTask.StartCommandItemID = itemShape.BatchCommandItem.CommandID;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (batchCommandTaskGraph.Hover != null)
            {
                if(MessageBox.Show(this,"是否要删除命令及子命令?","删除命令提示",MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    batchCommandTaskGraph.Hover.Delete();
                }
              
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "是否要保存控制命令流程?", "保存提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CommandTaskModel = batchCommandTaskGraph.SaveBatchCommandTask();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
