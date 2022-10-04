namespace IOMonitor
{
    partial class NetClientForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudMaxNumber = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudTimeInternal = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbAuto = new System.Windows.Forms.CheckBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbLog = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.nudBlocking = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.nudTransTimeout = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeInternal)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlocking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudMaxNumber);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudTimeInternal);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(497, 229);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "缓存配置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(428, 68);
            this.label6.TabIndex = 7;
            this.label6.Text = "每到缓存时间，系统会批量将缓存中指定最大数量的采集数据上传到数据中心。\r\n系统最大一次传输的采集数与网络状况有关。网络状况好可以采用比较大的值；\r\n网络状况差，可" +
    "减小缓存最大值；缓存值越大，则在网络比较好的情况下数据\r\n传输效率会更高，每秒的并发数量会越大。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "条";
            // 
            // nudMaxNumber
            // 
            this.nudMaxNumber.Location = new System.Drawing.Point(91, 96);
            this.nudMaxNumber.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudMaxNumber.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMaxNumber.Name = "nudMaxNumber";
            this.nudMaxNumber.Size = new System.Drawing.Size(120, 23);
            this.nudMaxNumber.TabIndex = 5;
            this.nudMaxNumber.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "缓存最大值:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(353, 34);
            this.label3.TabIndex = 3;
            this.label3.Text = "时间越小，数据延迟越小，但服务器的硬件要求越高，默认是每\r\n100毫秒的时间将采集器缓存中采集的数据批量传输至数据中心。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "ms(毫秒)";
            // 
            // nudTimeInternal
            // 
            this.nudTimeInternal.Location = new System.Drawing.Point(91, 14);
            this.nudTimeInternal.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudTimeInternal.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudTimeInternal.Name = "nudTimeInternal";
            this.nudTimeInternal.Size = new System.Drawing.Size(120, 23);
            this.nudTimeInternal.TabIndex = 1;
            this.nudTimeInternal.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "缓存时间:";
            // 
            // btSave
            // 
            this.btSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btSave.Location = new System.Drawing.Point(0, 0);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 34);
            this.btSave.TabIndex = 8;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btClose);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 456);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(899, 34);
            this.panel1.TabIndex = 1;
            // 
            // btClose
            // 
            this.btClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.btClose.Location = new System.Drawing.Point(75, 0);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 34);
            this.btClose.TabIndex = 9;
            this.btClose.Text = "关闭";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbAuto);
            this.groupBox2.Controls.Add(this.tbIP);
            this.groupBox2.Controls.Add(this.tbPwd);
            this.groupBox2.Controls.Add(this.tbUser);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(497, 175);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "登录配置";
            // 
            // cbAuto
            // 
            this.cbAuto.AutoSize = true;
            this.cbAuto.Location = new System.Drawing.Point(91, 135);
            this.cbAuto.Name = "cbAuto";
            this.cbAuto.Size = new System.Drawing.Size(99, 21);
            this.cbAuto.TabIndex = 7;
            this.cbAuto.Text = "开启自动登录";
            this.cbAuto.UseVisualStyleBackColor = true;
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(91, 96);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(185, 23);
            this.tbIP.TabIndex = 6;
            // 
            // tbPwd
            // 
            this.tbPwd.Location = new System.Drawing.Point(91, 63);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.PasswordChar = '*';
            this.tbPwd.Size = new System.Drawing.Size(185, 23);
            this.tbPwd.TabIndex = 5;
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(91, 32);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(185, 23);
            this.tbUser.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 17);
            this.label9.TabIndex = 3;
            this.label9.Text = "数据中心IP:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "账号密码:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "账号名称:";
            // 
            // cbLog
            // 
            this.cbLog.AutoSize = true;
            this.cbLog.Location = new System.Drawing.Point(12, 420);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(120, 16);
            this.cbLog.TabIndex = 8;
            this.cbLog.Text = "是否长期保存日志";
            this.cbLog.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.nudTransTimeout);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.nudBlocking);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Location = new System.Drawing.Point(504, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(395, 229);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "通信设置";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(230, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "ms(毫秒)";
            // 
            // nudBlocking
            // 
            this.nudBlocking.Location = new System.Drawing.Point(102, 20);
            this.nudBlocking.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudBlocking.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudBlocking.Name = "nudBlocking";
            this.nudBlocking.Size = new System.Drawing.Size(120, 21);
            this.nudBlocking.TabIndex = 4;
            this.nudBlocking.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 12);
            this.label11.TabIndex = 3;
            this.label11.Text = "数据传输阻塞:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(311, 36);
            this.label12.TabIndex = 6;
            this.label12.Text = "在并发情况下通信数据阻塞的时间，该值默认在100毫秒。\r\n时间越大发送数据根据网络状况延迟时间约久，\r\n值太小可能会出现丢失数据";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 143);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(293, 12);
            this.label13.TabIndex = 10;
            this.label13.Text = "当数据发送为得到确认超时的时间，默认是10000毫秒.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(230, 110);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 9;
            this.label14.Text = "ms(毫秒)";
            // 
            // nudTransTimeout
            // 
            this.nudTransTimeout.Location = new System.Drawing.Point(102, 106);
            this.nudTransTimeout.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudTransTimeout.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTransTimeout.Name = "nudTransTimeout";
            this.nudTransTimeout.Size = new System.Drawing.Size(120, 21);
            this.nudTransTimeout.TabIndex = 8;
            this.nudTransTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 108);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 12);
            this.label15.TabIndex = 7;
            this.label15.Text = "数据传输超时:";
            // 
            // NetClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 490);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cbLog);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetClientForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "网络配置";
            this.Load += new System.EventHandler(this.NetClientForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeInternal)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBlocking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudTimeInternal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudMaxNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbAuto;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.CheckBox cbLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudBlocking;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudTransTimeout;
        private System.Windows.Forms.Label label15;
    }
}