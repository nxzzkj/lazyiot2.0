namespace IOManager.Page
{
    partial class IOTreeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOTreeForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("机器学习任务", 0, 0);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.machineTrainingTree = new IOManager.Controls.MachineTrainingTree();
            this.imageListTraining = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTrainingAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripTrainingDel = new System.Windows.Forms.ToolStripButton();
            this.toolStripTrainingEdit = new System.Windows.Forms.ToolStripButton();
            this.IoTree = new IOManager.Controls.IOTree();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Server");
            this.imageList.Images.SetKeyName(1, "Communication");
            this.imageList.Images.SetKeyName(2, "Device");
            this.imageList.Images.SetKeyName(3, "IO");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.machineTrainingTree);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 713);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 102);
            this.panel1.TabIndex = 2;
            // 
            // machineTrainingTree
            // 
            this.machineTrainingTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machineTrainingTree.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.machineTrainingTree.ImageIndex = 0;
            this.machineTrainingTree.ImageList = this.imageListTraining;
            this.machineTrainingTree.ItemHeight = 28;
            this.machineTrainingTree.Location = new System.Drawing.Point(0, 25);
            this.machineTrainingTree.Name = "machineTrainingTree";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "机器学习任务";
            this.machineTrainingTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.machineTrainingTree.SelectedImageIndex = 0;
            this.machineTrainingTree.Server_ID = null;
            this.machineTrainingTree.Size = new System.Drawing.Size(297, 77);
            this.machineTrainingTree.TabIndex = 1;
            // 
            // imageListTraining
            // 
            this.imageListTraining.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTraining.ImageStream")));
            this.imageListTraining.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTraining.Images.SetKeyName(0, "open-16x16.png");
            this.imageListTraining.Images.SetKeyName(1, "Order.png");
            this.imageListTraining.Images.SetKeyName(2, "Price.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTrainingAdd,
            this.toolStripTrainingDel,
            this.toolStripTrainingEdit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(297, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTrainingAdd
            // 
            this.toolStripTrainingAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripTrainingAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripTrainingAdd.Image")));
            this.toolStripTrainingAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripTrainingAdd.Name = "toolStripTrainingAdd";
            this.toolStripTrainingAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripTrainingAdd.Text = "添加机器训练方案";
            this.toolStripTrainingAdd.Click += new System.EventHandler(this.toolStripTrainingAdd_Click);
            // 
            // toolStripTrainingDel
            // 
            this.toolStripTrainingDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripTrainingDel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripTrainingDel.Image")));
            this.toolStripTrainingDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripTrainingDel.Name = "toolStripTrainingDel";
            this.toolStripTrainingDel.Size = new System.Drawing.Size(23, 22);
            this.toolStripTrainingDel.Text = "删除方案";
            this.toolStripTrainingDel.Click += new System.EventHandler(this.toolStripTrainingDel_Click);
            // 
            // toolStripTrainingEdit
            // 
            this.toolStripTrainingEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripTrainingEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripTrainingEdit.Image")));
            this.toolStripTrainingEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripTrainingEdit.Name = "toolStripTrainingEdit";
            this.toolStripTrainingEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripTrainingEdit.Text = "编辑方案";
            this.toolStripTrainingEdit.Click += new System.EventHandler(this.toolStripTrainingEdit_Click);
            // 
            // IoTree
            // 
            this.IoTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.IoTree.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IoTree.ImageIndex = 0;
            this.IoTree.ImageList = this.imageList;
            this.IoTree.ItemHeight = 28;
            this.IoTree.Location = new System.Drawing.Point(0, 0);
            this.IoTree.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IoTree.Name = "IoTree";
            this.IoTree.SelectedImageIndex = 0;
            this.IoTree.Size = new System.Drawing.Size(297, 713);
            this.IoTree.TabIndex = 1;
            // 
            // IOTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 815);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.IoTree);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "IOTreeForm";
            this.Text = "IOTreeForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public Controls.IOTree IoTree;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripTrainingAdd;
        private System.Windows.Forms.ToolStripButton toolStripTrainingDel;
        private System.Windows.Forms.ToolStripButton toolStripTrainingEdit;
        private System.Windows.Forms.ImageList imageListTraining;
        public Controls.MachineTrainingTree machineTrainingTree;
    }
}