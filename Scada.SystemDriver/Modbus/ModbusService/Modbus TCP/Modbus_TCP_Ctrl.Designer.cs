namespace Modbus.ModbusService
{
    partial class Modbus_TCP_Ctrl
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
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ndWriteBufferSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.ndReadBuffSize = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textIP = new System.Windows.Forms.TextBox();
            this.ndLocalPort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nbWriteTimeout = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.nbReadTimeout = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndWriteBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndReadBuffSize)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndLocalPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbWriteTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbReadTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "TCP/IP",
            "UDP/IP"});
            this.comboBoxMode.Location = new System.Drawing.Point(261, 41);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(168, 20);
            this.comboBoxMode.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(259, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "通讯协议:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ndWriteBufferSize);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.ndReadBuffSize);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Location = new System.Drawing.Point(236, 77);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(206, 66);
            this.groupBox5.TabIndex = 44;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "缓存配置";
            // 
            // ndWriteBufferSize
            // 
            this.ndWriteBufferSize.Location = new System.Drawing.Point(75, 39);
            this.ndWriteBufferSize.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ndWriteBufferSize.Name = "ndWriteBufferSize";
            this.ndWriteBufferSize.Size = new System.Drawing.Size(79, 21);
            this.ndWriteBufferSize.TabIndex = 12;
            this.ndWriteBufferSize.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "写缓存：";
            // 
            // ndReadBuffSize
            // 
            this.ndReadBuffSize.Location = new System.Drawing.Point(75, 14);
            this.ndReadBuffSize.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ndReadBuffSize.Name = "ndReadBuffSize";
            this.ndReadBuffSize.Size = new System.Drawing.Size(79, 21);
            this.ndReadBuffSize.TabIndex = 10;
            this.ndReadBuffSize.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "读缓存：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textIP);
            this.groupBox4.Controls.Add(this.ndLocalPort);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(247, 68);
            this.groupBox4.TabIndex = 43;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "设备网络";
            // 
            // textIP
            // 
            this.textIP.Location = new System.Drawing.Point(76, 15);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(159, 21);
            this.textIP.TabIndex = 13;
            // 
            // ndLocalPort
            // 
            this.ndLocalPort.Location = new System.Drawing.Point(76, 44);
            this.ndLocalPort.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.ndLocalPort.Name = "ndLocalPort";
            this.ndLocalPort.Size = new System.Drawing.Size(159, 21);
            this.ndLocalPort.TabIndex = 10;
            this.ndLocalPort.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(8, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "设备端口：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(20, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "设备IP：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nbWriteTimeout);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.nbReadTimeout);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Location = new System.Drawing.Point(7, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 75);
            this.groupBox2.TabIndex = 41;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "读写设置";
            // 
            // nbWriteTimeout
            // 
            this.nbWriteTimeout.Location = new System.Drawing.Point(124, 41);
            this.nbWriteTimeout.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nbWriteTimeout.Name = "nbWriteTimeout";
            this.nbWriteTimeout.Size = new System.Drawing.Size(79, 21);
            this.nbWriteTimeout.TabIndex = 12;
            this.nbWriteTimeout.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 12);
            this.label12.TabIndex = 11;
            this.label12.Text = "写入超时时间(ms)：";
            // 
            // nbReadTimeout
            // 
            this.nbReadTimeout.Location = new System.Drawing.Point(124, 14);
            this.nbReadTimeout.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nbReadTimeout.Name = "nbReadTimeout";
            this.nbReadTimeout.Size = new System.Drawing.Size(79, 21);
            this.nbReadTimeout.TabIndex = 10;
            this.nbReadTimeout.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(113, 12);
            this.label13.TabIndex = 9;
            this.label13.Text = "读取超时时间(ms)：";
            // 
            // Modbus_TCP_Ctrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Name = "Modbus_TCP_Ctrl";
            this.Size = new System.Drawing.Size(454, 214);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndWriteBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndReadBuffSize)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndLocalPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbWriteTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbReadTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown ndWriteBufferSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown ndReadBuffSize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.NumericUpDown ndLocalPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nbWriteTimeout;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nbReadTimeout;
        private System.Windows.Forms.Label label13;
    }
}
