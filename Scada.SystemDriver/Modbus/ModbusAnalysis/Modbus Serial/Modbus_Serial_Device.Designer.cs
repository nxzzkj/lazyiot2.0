namespace Modbus.ModbusAnalysis
{
    partial class Modbus_Serial_Device
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
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.nbRetiresInternal = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.cbRetries = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.ndRetiresNum = new System.Windows.Forms.NumericUpDown();
            this.cbModels = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbRetiresInternal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndRetiresNum)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.nbRetiresInternal);
            this.groupBox8.Controls.Add(this.label21);
            this.groupBox8.Controls.Add(this.cbRetries);
            this.groupBox8.Controls.Add(this.label22);
            this.groupBox8.Controls.Add(this.ndRetiresNum);
            this.groupBox8.Location = new System.Drawing.Point(3, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(245, 75);
            this.groupBox8.TabIndex = 42;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "重试    ";
            // 
            // nbRetiresInternal
            // 
            this.nbRetiresInternal.Location = new System.Drawing.Point(94, 49);
            this.nbRetiresInternal.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nbRetiresInternal.Name = "nbRetiresInternal";
            this.nbRetiresInternal.Size = new System.Drawing.Size(111, 21);
            this.nbRetiresInternal.TabIndex = 27;
            this.nbRetiresInternal.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 24);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 12);
            this.label21.TabIndex = 29;
            this.label21.Text = "重试次数：";
            // 
            // cbRetries
            // 
            this.cbRetries.AutoSize = true;
            this.cbRetries.Location = new System.Drawing.Point(35, 0);
            this.cbRetries.Name = "cbRetries";
            this.cbRetries.Size = new System.Drawing.Size(15, 14);
            this.cbRetries.TabIndex = 15;
            this.cbRetries.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 53);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(89, 12);
            this.label22.TabIndex = 28;
            this.label22.Text = "重试间隔(ms)：";
            // 
            // ndRetiresNum
            // 
            this.ndRetiresNum.Location = new System.Drawing.Point(94, 22);
            this.ndRetiresNum.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ndRetiresNum.Name = "ndRetiresNum";
            this.ndRetiresNum.Size = new System.Drawing.Size(51, 21);
            this.ndRetiresNum.TabIndex = 16;
            this.ndRetiresNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // cbModels
            // 
            this.cbModels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModels.FormattingEnabled = true;
            this.cbModels.Items.AddRange(new object[] {
            "RTU",
            "ASCII"});
            this.cbModels.Location = new System.Drawing.Point(97, 84);
            this.cbModels.Name = "cbModels";
            this.cbModels.Size = new System.Drawing.Size(128, 20);
            this.cbModels.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 43;
            this.label5.Text = "通讯协议：";
            // 
            // Modbus_Serial_Device
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbModels);
            this.Controls.Add(this.groupBox8);
            this.Name = "Modbus_Serial_Device";
            this.Size = new System.Drawing.Size(259, 208);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbRetiresInternal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndRetiresNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.NumericUpDown nbRetiresInternal;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox cbRetries;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown ndRetiresNum;
        private System.Windows.Forms.ComboBox cbModels;
        private System.Windows.Forms.Label label5;
    }
}
