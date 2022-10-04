namespace Modbus.ModbusService
{
    partial class Modbus_Serial_Ctrl
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
            this.label10 = new System.Windows.Forms.Label();
            this.nbPackOffset = new System.Windows.Forms.NumericUpDown();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbRTSEnable = new System.Windows.Forms.CheckBox();
            this.cbStopbits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.comboSeriePort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCheck = new System.Windows.Forms.ComboBox();
            this.groupSerial = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nbWriteTimeout = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.nbReadTimeout = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ndWriteBufferSize = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.ndReadBuffSize = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cbDTREnable = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbHandshake = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nbPackOffset)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupSerial.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbWriteTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbReadTimeout)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndWriteBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndReadBuffSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "偏移间隔：";
            // 
            // nbPackOffset
            // 
            this.nbPackOffset.Location = new System.Drawing.Point(76, 173);
            this.nbPackOffset.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nbPackOffset.Name = "nbPackOffset";
            this.nbPackOffset.Size = new System.Drawing.Size(42, 21);
            this.nbPackOffset.TabIndex = 20;
            this.nbPackOffset.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // cbDataBits
            // 
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cbDataBits.Location = new System.Drawing.Point(78, 112);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(160, 20);
            this.cbDataBits.TabIndex = 21;
            this.cbDataBits.Text = "8";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbDTREnable);
            this.groupBox1.Controls.Add(this.cbRTSEnable);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(248, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 38);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "RTS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(105, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "DTR";
            // 
            // cbRTSEnable
            // 
            this.cbRTSEnable.AutoSize = true;
            this.cbRTSEnable.Location = new System.Drawing.Point(58, 20);
            this.cbRTSEnable.Name = "cbRTSEnable";
            this.cbRTSEnable.Size = new System.Drawing.Size(15, 14);
            this.cbRTSEnable.TabIndex = 13;
            this.cbRTSEnable.UseVisualStyleBackColor = true;
            // 
            // cbStopbits
            // 
            this.cbStopbits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStopbits.FormattingEnabled = true;
            this.cbStopbits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.cbStopbits.Location = new System.Drawing.Point(76, 143);
            this.cbStopbits.Name = "cbStopbits";
            this.cbStopbits.Size = new System.Drawing.Size(166, 20);
            this.cbStopbits.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "停止位：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "数据位：";
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "50",
            "75",
            "100",
            "150",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400"});
            this.cbBaudRate.Location = new System.Drawing.Point(78, 50);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(160, 20);
            this.cbBaudRate.TabIndex = 3;
            this.cbBaudRate.Text = "19200";
            // 
            // comboSeriePort
            // 
            this.comboSeriePort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSeriePort.FormattingEnabled = true;
            this.comboSeriePort.Location = new System.Drawing.Point(78, 20);
            this.comboSeriePort.Name = "comboSeriePort";
            this.comboSeriePort.Size = new System.Drawing.Size(160, 20);
            this.comboSeriePort.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "校验：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "波特率：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "串口：";
            // 
            // cbCheck
            // 
            this.cbCheck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheck.FormattingEnabled = true;
            this.cbCheck.Items.AddRange(new object[] {
            "无",
            "偶校验",
            "奇校验",
            "常1",
            "常0"});
            this.cbCheck.Location = new System.Drawing.Point(78, 80);
            this.cbCheck.Name = "cbCheck";
            this.cbCheck.Size = new System.Drawing.Size(160, 20);
            this.cbCheck.TabIndex = 4;
            // 
            // groupSerial
            // 
            this.groupSerial.Controls.Add(this.label9);
            this.groupSerial.Controls.Add(this.cbHandshake);
            this.groupSerial.Controls.Add(this.groupBox5);
            this.groupSerial.Controls.Add(this.groupBox2);
            this.groupSerial.Controls.Add(this.cbCheck);
            this.groupSerial.Controls.Add(this.label1);
            this.groupSerial.Controls.Add(this.label2);
            this.groupSerial.Controls.Add(this.label3);
            this.groupSerial.Controls.Add(this.comboSeriePort);
            this.groupSerial.Controls.Add(this.cbBaudRate);
            this.groupSerial.Controls.Add(this.label4);
            this.groupSerial.Controls.Add(this.label5);
            this.groupSerial.Controls.Add(this.cbStopbits);
            this.groupSerial.Controls.Add(this.groupBox1);
            this.groupSerial.Controls.Add(this.cbDataBits);
            this.groupSerial.Controls.Add(this.nbPackOffset);
            this.groupSerial.Controls.Add(this.label10);
            this.groupSerial.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupSerial.Location = new System.Drawing.Point(0, 0);
            this.groupSerial.Name = "groupSerial";
            this.groupSerial.Size = new System.Drawing.Size(504, 273);
            this.groupSerial.TabIndex = 33;
            this.groupSerial.TabStop = false;
            this.groupSerial.Text = "串口通讯";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nbWriteTimeout);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.nbReadTimeout);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Location = new System.Drawing.Point(254, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 75);
            this.groupBox2.TabIndex = 45;
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ndWriteBufferSize);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.ndReadBuffSize);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Location = new System.Drawing.Point(248, 104);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(237, 66);
            this.groupBox5.TabIndex = 46;
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "写缓存：";
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
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 9;
            this.label11.Text = "读缓存：";
            // 
            // cbDTREnable
            // 
            this.cbDTREnable.AutoSize = true;
            this.cbDTREnable.Location = new System.Drawing.Point(143, 20);
            this.cbDTREnable.Name = "cbDTREnable";
            this.cbDTREnable.Size = new System.Drawing.Size(15, 14);
            this.cbDTREnable.TabIndex = 14;
            this.cbDTREnable.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 47;
            this.label9.Text = "握手协议：";
            // 
            // cbHandshake
            // 
            this.cbHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHandshake.FormattingEnabled = true;
            this.cbHandshake.Items.AddRange(new object[] {
            "None",
            "XOnXOff",
            "RequestToSend",
            "RequestToSendXOnXOff"});
            this.cbHandshake.Location = new System.Drawing.Point(78, 209);
            this.cbHandshake.Name = "cbHandshake";
            this.cbHandshake.Size = new System.Drawing.Size(166, 20);
            this.cbHandshake.TabIndex = 48;
            // 
            // Modbus_Serial_Ctrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupSerial);
            this.Name = "Modbus_Serial_Ctrl";
            this.Size = new System.Drawing.Size(504, 296);
            ((System.ComponentModel.ISupportInitialize)(this.nbPackOffset)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupSerial.ResumeLayout(false);
            this.groupSerial.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbWriteTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbReadTimeout)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndWriteBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndReadBuffSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nbPackOffset;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbRTSEnable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbStopbits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.ComboBox comboSeriePort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCheck;
        private System.Windows.Forms.GroupBox groupSerial;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nbWriteTimeout;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nbReadTimeout;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown ndWriteBufferSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown ndReadBuffSize;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbDTREnable;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbHandshake;
    }
}
