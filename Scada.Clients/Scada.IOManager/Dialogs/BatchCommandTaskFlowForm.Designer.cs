
namespace IOManager.Dialogs
{
    partial class BatchCommandTaskFlowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchCommandTaskFlowForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripCommandAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.batchCommandTaskGraph = new Scada.BatchCommand.BatchCommandTaskGraph();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripCommandAdd,
            this.toolStripButton2,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 39);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripCommandAdd
            // 
            this.toolStripCommandAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripCommandAdd.Image")));
            this.toolStripCommandAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripCommandAdd.Name = "toolStripCommandAdd";
            this.toolStripCommandAdd.Size = new System.Drawing.Size(92, 36);
            this.toolStripCommandAdd.Text = "增加命令";
            this.toolStripCommandAdd.Click += new System.EventHandler(this.toolStripCommandAdd_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(92, 36);
            this.toolStripButton2.Text = "删除命令";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(92, 36);
            this.toolStripButton4.Text = "保存流程";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid.Location = new System.Drawing.Point(563, 39);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(237, 411);
            this.propertyGrid.TabIndex = 2;
            // 
            // batchCommandTaskGraph
            // 
            this.batchCommandTaskGraph.BatchCommandTask = null;
            this.batchCommandTaskGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.batchCommandTaskGraph.Location = new System.Drawing.Point(0, 39);
            this.batchCommandTaskGraph.Name = "batchCommandTaskGraph";
            this.batchCommandTaskGraph.Shapes = null;
            this.batchCommandTaskGraph.Size = new System.Drawing.Size(563, 411);
            this.batchCommandTaskGraph.TabIndex = 1;
            this.batchCommandTaskGraph.Text = "batchCommandTaskGraph1";
            // 
            // BatchCommandTaskFlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.batchCommandTaskGraph);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.toolStrip1);
            this.Name = "BatchCommandTaskFlowForm";
            this.Text = "任务流程编辑";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripCommandAdd;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private Scada.BatchCommand.BatchCommandTaskGraph batchCommandTaskGraph;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.PropertyGrid propertyGrid;
    }
}