
namespace Scada.Kernel
{
    partial class SimulatorKernelControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewPara = new System.Windows.Forms.DataGridView();
            this.IOName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.simulatormax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.simulatormin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripPara = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditParatoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewDevice = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SimulatorParaString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批量编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPara)).BeginInit();
            this.contextMenuStripPara.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevice)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1147, 866);
            this.splitContainer1.SplitterDistance = 307;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.label1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(307, 866);
            this.panel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(134, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户自定义参数区域";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewPara);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 501);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(835, 365);
            this.groupBox2.TabIndex = 55;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "参数管理";
            // 
            // dataGridViewPara
            // 
            this.dataGridViewPara.AllowUserToAddRows = false;
            this.dataGridViewPara.AllowUserToDeleteRows = false;
            this.dataGridViewPara.AllowUserToResizeColumns = false;
            this.dataGridViewPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPara.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IOName,
            this.simulatormax,
            this.simulatormin});
            this.dataGridViewPara.ContextMenuStrip = this.contextMenuStripPara;
            this.dataGridViewPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPara.Location = new System.Drawing.Point(3, 20);
            this.dataGridViewPara.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewPara.MultiSelect = false;
            this.dataGridViewPara.Name = "dataGridViewPara";
            this.dataGridViewPara.RowTemplate.Height = 23;
            this.dataGridViewPara.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPara.Size = new System.Drawing.Size(829, 341);
            this.dataGridViewPara.TabIndex = 3;
            // 
            // IOName
            // 
            this.IOName.DataPropertyName = "IO_NAME";
            this.IOName.Frozen = true;
            this.IOName.HeaderText = "IO 名称";
            this.IOName.Name = "IOName";
            this.IOName.ReadOnly = true;
            // 
            // simulatormax
            // 
            this.simulatormax.DataPropertyName = "IO_SIMULATOR_MAX";
            this.simulatormax.HeaderText = "模拟最大值";
            this.simulatormax.Name = "simulatormax";
            this.simulatormax.ReadOnly = true;
            // 
            // simulatormin
            // 
            this.simulatormin.DataPropertyName = "IO_SIMULATOR_MIN";
            this.simulatormin.HeaderText = "模拟最小值";
            this.simulatormin.Name = "simulatormin";
            this.simulatormin.ReadOnly = true;
            // 
            // contextMenuStripPara
            // 
            this.contextMenuStripPara.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditParatoolStripMenuItem});
            this.contextMenuStripPara.Name = "contextMenuStrip";
            this.contextMenuStripPara.Size = new System.Drawing.Size(125, 26);
            // 
            // EditParatoolStripMenuItem
            // 
            this.EditParatoolStripMenuItem.Name = "EditParatoolStripMenuItem";
            this.EditParatoolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.EditParatoolStripMenuItem.Text = "编辑参数";
            this.EditParatoolStripMenuItem.Click += new System.EventHandler(this.EditParatoolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewDevice);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(835, 501);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备管理";
            // 
            // dataGridViewDevice
            // 
            this.dataGridViewDevice.AllowUserToAddRows = false;
            this.dataGridViewDevice.AllowUserToDeleteRows = false;
            this.dataGridViewDevice.AllowUserToResizeColumns = false;
            this.dataGridViewDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.SimulatorParaString});
            this.dataGridViewDevice.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDevice.Location = new System.Drawing.Point(3, 20);
            this.dataGridViewDevice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewDevice.MultiSelect = false;
            this.dataGridViewDevice.Name = "dataGridViewDevice";
            this.dataGridViewDevice.RowTemplate.Height = 23;
            this.dataGridViewDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDevice.Size = new System.Drawing.Size(829, 477);
            this.dataGridViewDevice.TabIndex = 0;
            this.dataGridViewDevice.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewDevice_CellMouseClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditDeviceToolStripMenuItem,
            this.批量编辑ToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(181, 70);
            // 
            // EditDeviceToolStripMenuItem
            // 
            this.EditDeviceToolStripMenuItem.Name = "EditDeviceToolStripMenuItem";
            this.EditDeviceToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.EditDeviceToolStripMenuItem.Text = "编辑设备";
            this.EditDeviceToolStripMenuItem.Click += new System.EventHandler(this.EditDeviceToolStripMenuItem_Click);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "IO_DEVICE_ID";
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "设备ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "IO_DEVICE_NAME";
            this.Column2.Frozen = true;
            this.Column2.HeaderText = "设备名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "IO_DEVICE_UPDATECYCLE";
            this.Column3.HeaderText = "采集周期(毫秒)";
            this.Column3.Name = "Column3";
            // 
            // SimulatorParaString
            // 
            this.SimulatorParaString.DataPropertyName = "IO_DEVICE_SIMULATOR_PARASTRING";
            this.SimulatorParaString.HeaderText = "设备模拟参数";
            this.SimulatorParaString.Name = "SimulatorParaString";
            this.SimulatorParaString.ReadOnly = true;
            this.SimulatorParaString.Width = 200;
            // 
            // 批量编辑ToolStripMenuItem
            // 
            this.批量编辑ToolStripMenuItem.Name = "批量编辑ToolStripMenuItem";
            this.批量编辑ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.批量编辑ToolStripMenuItem.Text = "批量编辑";
            this.批量编辑ToolStripMenuItem.Click += new System.EventHandler(this.批量编辑ToolStripMenuItem_Click);
            // 
            // SimulatorKernelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SimulatorKernelControl";
            this.Size = new System.Drawing.Size(1147, 866);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPara)).EndInit();
            this.contextMenuStripPara.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDevice)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Panel panel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewDevice;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewPara;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem EditDeviceToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPara;
        private System.Windows.Forms.ToolStripMenuItem EditParatoolStripMenuItem;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn IOName;
        private System.Windows.Forms.DataGridViewTextBoxColumn simulatormax;
        private System.Windows.Forms.DataGridViewTextBoxColumn simulatormin;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn SimulatorParaString;
        private System.Windows.Forms.ToolStripMenuItem 批量编辑ToolStripMenuItem;
    }
}
