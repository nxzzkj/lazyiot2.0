
namespace IOMonitor
{
    partial class SimulatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulatorForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlLeft = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.IoTreeStatus = new IOMonitor.Controls.IOTree();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btUnCheckAll = new System.Windows.Forms.Button();
            this.btCheckAll = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.computerInfoControl = new Scada.Controls.Controls.ComputerInfoControl();
            this.tabControlRight = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.SimulatorPanel = new System.Windows.Forms.Panel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listView = new Scada.Controls.Controls.List.SCADAListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStart = new System.Windows.Forms.ToolStripButton();
            this.toolStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlLeft.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControlRight.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(963, 489);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(963, 514);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlRight);
            this.splitContainer1.Size = new System.Drawing.Size(963, 489);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControlLeft
            // 
            this.tabControlLeft.Controls.Add(this.tabPage1);
            this.tabControlLeft.Controls.Add(this.tabPage2);
            this.tabControlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLeft.Location = new System.Drawing.Point(0, 0);
            this.tabControlLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlLeft.Name = "tabControlLeft";
            this.tabControlLeft.SelectedIndex = 0;
            this.tabControlLeft.Size = new System.Drawing.Size(220, 489);
            this.tabControlLeft.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.IoTreeStatus);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(212, 456);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IO树";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // IoTreeStatus
            // 
            this.IoTreeStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IoTreeStatus.CheckBoxes = true;
            this.IoTreeStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IoTreeStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IoTreeStatus.FullRowSelect = true;
            this.IoTreeStatus.HideSelection = false;
            this.IoTreeStatus.Location = new System.Drawing.Point(4, 5);
            this.IoTreeStatus.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.IoTreeStatus.Name = "IoTreeStatus";
            this.IoTreeStatus.Size = new System.Drawing.Size(204, 413);
            this.IoTreeStatus.TabIndex = 1;
            this.IoTreeStatus.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.IoTreeStatus_AfterCheck);
            this.IoTreeStatus.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.IoTreeStatus_NodeMouseClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btUnCheckAll);
            this.panel1.Controls.Add(this.btCheckAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 418);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 33);
            this.panel1.TabIndex = 2;
            // 
            // btUnCheckAll
            // 
            this.btUnCheckAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.btUnCheckAll.Location = new System.Drawing.Point(62, 0);
            this.btUnCheckAll.Name = "btUnCheckAll";
            this.btUnCheckAll.Size = new System.Drawing.Size(62, 33);
            this.btUnCheckAll.TabIndex = 1;
            this.btUnCheckAll.Text = "取消";
            this.btUnCheckAll.UseVisualStyleBackColor = true;
            this.btUnCheckAll.Click += new System.EventHandler(this.btUnCheckAll_Click);
            // 
            // btCheckAll
            // 
            this.btCheckAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCheckAll.Location = new System.Drawing.Point(0, 0);
            this.btCheckAll.Name = "btCheckAll";
            this.btCheckAll.Size = new System.Drawing.Size(62, 33);
            this.btCheckAll.TabIndex = 0;
            this.btCheckAll.Text = "全选";
            this.btCheckAll.UseVisualStyleBackColor = true;
            this.btCheckAll.Click += new System.EventHandler(this.btCheckAll_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.computerInfoControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(212, 463);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "资源使用";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // computerInfoControl
            // 
            this.computerInfoControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.computerInfoControl.Location = new System.Drawing.Point(4, 5);
            this.computerInfoControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.computerInfoControl.Name = "computerInfoControl";
            this.computerInfoControl.Size = new System.Drawing.Size(204, 1222);
            this.computerInfoControl.TabIndex = 2;
            // 
            // tabControlRight
            // 
            this.tabControlRight.Controls.Add(this.tabPage3);
            this.tabControlRight.Controls.Add(this.tabPage4);
            this.tabControlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlRight.Location = new System.Drawing.Point(0, 0);
            this.tabControlRight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlRight.Name = "tabControlRight";
            this.tabControlRight.SelectedIndex = 0;
            this.tabControlRight.Size = new System.Drawing.Size(738, 489);
            this.tabControlRight.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.SimulatorPanel);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Size = new System.Drawing.Size(730, 456);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "模拟设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // SimulatorPanel
            // 
            this.SimulatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SimulatorPanel.Location = new System.Drawing.Point(4, 5);
            this.SimulatorPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SimulatorPanel.Name = "SimulatorPanel";
            this.SimulatorPanel.Size = new System.Drawing.Size(722, 446);
            this.SimulatorPanel.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listView);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Size = new System.Drawing.Size(730, 463);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "系统日志";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(4, 5);
            this.listView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(722, 453);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 111;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "报告内容";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 454;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStart,
            this.toolStop,
            this.toolStripSeparator1,
            this.toolClose});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(258, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStart
            // 
            this.toolStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStart.Image")));
            this.toolStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStart.Name = "toolStart";
            this.toolStart.Size = new System.Drawing.Size(76, 22);
            this.toolStart.Text = "启动模拟";
            this.toolStart.Click += new System.EventHandler(this.toolStart_Click);
            // 
            // toolStop
            // 
            this.toolStop.Enabled = false;
            this.toolStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStop.Image")));
            this.toolStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStop.Name = "toolStop";
            this.toolStop.Size = new System.Drawing.Size(76, 22);
            this.toolStop.Text = "停止模拟";
            this.toolStop.Click += new System.EventHandler(this.toolStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolClose
            // 
            this.toolClose.Image = ((System.Drawing.Image)(resources.GetObject("toolClose.Image")));
            this.toolClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(88, 22);
            this.toolClose.Text = "关闭模拟器";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
            // 
            // SimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 514);
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SimulatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LazyOS 驱动下位机模拟器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimulatorForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SimulatorForm_FormClosed);
            this.Load += new System.EventHandler(this.SimulatorView_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlLeft.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControlRight.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStart;
        private System.Windows.Forms.ToolStripButton toolStop;
        private System.Windows.Forms.ToolStripButton toolClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControlLeft;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControlRight;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Scada.Controls.Controls.List.SCADAListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Scada.Controls.Controls.ComputerInfoControl computerInfoControl;
        public Controls.IOTree IoTreeStatus;
        private System.Windows.Forms.Panel SimulatorPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btUnCheckAll;
        private System.Windows.Forms.Button btCheckAll;
    }
}