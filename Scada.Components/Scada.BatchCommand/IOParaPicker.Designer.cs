using System.Windows.Forms;

namespace Scada.BatchCommand
{
    partial class IOParaPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOParaPicker));
            this.panel = new System.Windows.Forms.Panel();
            this.groupBoxBool = new System.Windows.Forms.GroupBox();
            this.nbBoolValue = new System.Windows.Forms.NumericUpDown();
            this.cbBoolOp = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPara = new Scada.BatchCommand.IOParaTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.IO_TreeView = new Scada.BatchCommand.IOFlowTree();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.nudDefaultValue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBoolExpression = new System.Windows.Forms.Button();
            this.listBoxExpressions = new System.Windows.Forms.ListBox();
            this.butBoolExpressDel = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            this.groupBoxBool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbBoolValue)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultValue)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel.Controls.Add(this.butBoolExpressDel);
            this.panel.Controls.Add(this.listBoxExpressions);
            this.panel.Controls.Add(this.groupBoxBool);
            this.panel.Controls.Add(this.groupBox1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(225, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(481, 270);
            this.panel.TabIndex = 1;
            // 
            // groupBoxBool
            // 
            this.groupBoxBool.Controls.Add(this.buttonBoolExpression);
            this.groupBoxBool.Controls.Add(this.nudDefaultValue);
            this.groupBoxBool.Controls.Add(this.label2);
            this.groupBoxBool.Controls.Add(this.nbBoolValue);
            this.groupBoxBool.Controls.Add(this.cbBoolOp);
            this.groupBoxBool.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBool.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxBool.Location = new System.Drawing.Point(0, 57);
            this.groupBoxBool.Name = "groupBoxBool";
            this.groupBoxBool.Size = new System.Drawing.Size(481, 48);
            this.groupBoxBool.TabIndex = 15;
            this.groupBoxBool.TabStop = false;
            this.groupBoxBool.Text = "下置值设置";
            // 
            // nbBoolValue
            // 
            this.nbBoolValue.DecimalPlaces = 2;
            this.nbBoolValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.nbBoolValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nbBoolValue.Location = new System.Drawing.Point(79, 22);
            this.nbBoolValue.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.nbBoolValue.Minimum = new decimal(new int[] {
            -727379969,
            232,
            0,
            -2147483648});
            this.nbBoolValue.Name = "nbBoolValue";
            this.nbBoolValue.Size = new System.Drawing.Size(120, 26);
            this.nbBoolValue.TabIndex = 14;
            // 
            // cbBoolOp
            // 
            this.cbBoolOp.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbBoolOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBoolOp.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbBoolOp.FormattingEnabled = true;
            this.cbBoolOp.Items.AddRange(new object[] {
            "=",
            ">",
            "<",
            ">=",
            "<="});
            this.cbBoolOp.Location = new System.Drawing.Point(3, 22);
            this.cbBoolOp.Name = "cbBoolOp";
            this.cbBoolOp.Size = new System.Drawing.Size(76, 28);
            this.cbBoolOp.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbPara);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 57);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IO配置";
            // 
            // tbPara
            // 
            this.tbPara.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPara.Location = new System.Drawing.Point(76, 18);
            this.tbPara.Name = "tbPara";
            this.tbPara.ReadOnly = true;
            this.tbPara.Size = new System.Drawing.Size(399, 26);
            this.tbPara.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IO参数:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel);
            this.splitContainer1.Panel1.Controls.Add(this.IO_TreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer1.Panel2.Controls.Add(this.btCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btOK);
            this.splitContainer1.Panel2.Controls.Add(this.labelInfo);
            this.splitContainer1.Size = new System.Drawing.Size(706, 310);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 2;
            // 
            // IO_TreeView
            // 
            this.IO_TreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.IO_TreeView.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IO_TreeView.FullRowSelect = true;
            this.IO_TreeView.ImageIndex = 0;
            this.IO_TreeView.ImageList = this.imageList;
            this.IO_TreeView.Location = new System.Drawing.Point(0, 0);
            this.IO_TreeView.Name = "IO_TreeView";
            this.IO_TreeView.SelectedImageIndex = 0;
            this.IO_TreeView.Size = new System.Drawing.Size(225, 270);
            this.IO_TreeView.TabIndex = 0;
            this.IO_TreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.IO_TreeView_NodeMouseDoubleClick);
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
            // btCancel
            // 
            this.btCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCancel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btCancel.Image = ((System.Drawing.Image)(resources.GetObject("btCancel.Image")));
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCancel.Location = new System.Drawing.Point(62, 0);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(62, 36);
            this.btCancel.TabIndex = 10;
            this.btCancel.Text = "取消";
            this.btCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOK
            // 
            this.btOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btOK.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btOK.Image = ((System.Drawing.Image)(resources.GetObject("btOK.Image")));
            this.btOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOK.Location = new System.Drawing.Point(0, 0);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(62, 36);
            this.btOK.TabIndex = 9;
            this.btOK.Text = "保存";
            this.btOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.ForeColor = System.Drawing.Color.Red;
            this.labelInfo.Location = new System.Drawing.Point(142, 8);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(0, 20);
            this.labelInfo.TabIndex = 8;
            // 
            // nudDefaultValue
            // 
            this.nudDefaultValue.DecimalPlaces = 2;
            this.nudDefaultValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudDefaultValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudDefaultValue.Location = new System.Drawing.Point(253, 22);
            this.nudDefaultValue.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.nudDefaultValue.Minimum = new decimal(new int[] {
            -727379969,
            232,
            0,
            -2147483648});
            this.nudDefaultValue.Name = "nudDefaultValue";
            this.nudDefaultValue.Size = new System.Drawing.Size(127, 26);
            this.nudDefaultValue.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(199, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "写入值:";
            // 
            // buttonBoolExpression
            // 
            this.buttonBoolExpression.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonBoolExpression.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonBoolExpression.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonBoolExpression.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonBoolExpression.Location = new System.Drawing.Point(380, 22);
            this.buttonBoolExpression.Name = "buttonBoolExpression";
            this.buttonBoolExpression.Size = new System.Drawing.Size(62, 23);
            this.buttonBoolExpression.TabIndex = 17;
            this.buttonBoolExpression.Text = "插入";
            this.buttonBoolExpression.UseVisualStyleBackColor = true;
            this.buttonBoolExpression.Click += new System.EventHandler(this.buttonBoolExpression_Click);
            // 
            // listBoxExpressions
            // 
            this.listBoxExpressions.FormattingEnabled = true;
            this.listBoxExpressions.ItemHeight = 12;
            this.listBoxExpressions.Location = new System.Drawing.Point(0, 111);
            this.listBoxExpressions.Name = "listBoxExpressions";
            this.listBoxExpressions.Size = new System.Drawing.Size(475, 124);
            this.listBoxExpressions.TabIndex = 18;
            // 
            // butBoolExpressDel
            // 
            this.butBoolExpressDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.butBoolExpressDel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butBoolExpressDel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butBoolExpressDel.Location = new System.Drawing.Point(3, 241);
            this.butBoolExpressDel.Name = "butBoolExpressDel";
            this.butBoolExpressDel.Size = new System.Drawing.Size(62, 26);
            this.butBoolExpressDel.TabIndex = 19;
            this.butBoolExpressDel.Text = "删除";
            this.butBoolExpressDel.UseVisualStyleBackColor = true;
            this.butBoolExpressDel.Click += new System.EventHandler(this.butBoolExpressDel_Click);
            // 
            // IOParaPicker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitContainer1);
            this.Name = "IOParaPicker";
            this.Size = new System.Drawing.Size(706, 310);
            this.panel.ResumeLayout(false);
            this.groupBoxBool.ResumeLayout(false);
            this.groupBoxBool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbBoolValue)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private IOFlowTree IO_TreeView;
        private System.Windows.Forms.Panel panel;
        private IOParaTextBox tbPara;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button btOK;
        private ImageList imageList;
        private Button btCancel;
        private GroupBox groupBoxBool;
        private NumericUpDown nbBoolValue;
        private ComboBox cbBoolOp;
        private GroupBox groupBox1;
        private Label label2;
        private NumericUpDown nudDefaultValue;
        private Button buttonBoolExpression;
        private Button butBoolExpressDel;
        private ListBox listBoxExpressions;
    }
}
