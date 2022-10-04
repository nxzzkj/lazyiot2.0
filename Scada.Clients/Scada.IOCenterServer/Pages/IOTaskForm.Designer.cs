
namespace ScadaCenterServer.Pages
{
    partial class IOTaskForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("机器训练任务");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOTaskForm));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("自动控制流程任务");
            this.treeViewMachineTrainTask = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.treeViewBatchCommandTask = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewMachineTrainTask
            // 
            this.treeViewMachineTrainTask.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeViewMachineTrainTask.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewMachineTrainTask.FullRowSelect = true;
            this.treeViewMachineTrainTask.HideSelection = false;
            this.treeViewMachineTrainTask.ImageIndex = 0;
            this.treeViewMachineTrainTask.ImageList = this.imageList;
            this.treeViewMachineTrainTask.Location = new System.Drawing.Point(0, 0);
            this.treeViewMachineTrainTask.Name = "treeViewMachineTrainTask";
            treeNode1.Name = "节点0";
            treeNode1.Text = "机器训练任务";
            this.treeViewMachineTrainTask.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeViewMachineTrainTask.SelectedImageIndex = 0;
            this.treeViewMachineTrainTask.Size = new System.Drawing.Size(395, 454);
            this.treeViewMachineTrainTask.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "CreateConnection.png");
            this.imageList.Images.SetKeyName(1, "Diagnostics.png");
            this.imageList.Images.SetKeyName(2, "Ai.png");
            this.imageList.Images.SetKeyName(3, "AiList.png");
            this.imageList.Images.SetKeyName(4, "End.png");
            this.imageList.Images.SetKeyName(5, "CommandTask.png");
            // 
            // treeViewBatchCommandTask
            // 
            this.treeViewBatchCommandTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewBatchCommandTask.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewBatchCommandTask.FullRowSelect = true;
            this.treeViewBatchCommandTask.HideSelection = false;
            this.treeViewBatchCommandTask.ImageIndex = 0;
            this.treeViewBatchCommandTask.ImageList = this.imageList;
            this.treeViewBatchCommandTask.Location = new System.Drawing.Point(0, 454);
            this.treeViewBatchCommandTask.Name = "treeViewBatchCommandTask";
            treeNode2.ImageIndex = 0;
            treeNode2.Name = "节点1";
            treeNode2.SelectedImageKey = "End.png";
            treeNode2.Text = "自动控制流程任务";
            this.treeViewBatchCommandTask.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeViewBatchCommandTask.SelectedImageIndex = 0;
            this.treeViewBatchCommandTask.Size = new System.Drawing.Size(395, 208);
            this.treeViewBatchCommandTask.TabIndex = 1;
            // 
            // IOTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 662);
            this.Controls.Add(this.treeViewBatchCommandTask);
            this.Controls.Add(this.treeViewMachineTrainTask);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "IOTaskForm";
            this.Text = "IOTaskForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewMachineTrainTask;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TreeView treeViewBatchCommandTask;
    }
}