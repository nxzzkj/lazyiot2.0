namespace MQTTnet
{
    partial class MQTTServerCtrl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbEnableUser = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbWill = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbMessage = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.cbReceiveMethod = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbEnableKeepSessions = new System.Windows.Forms.CheckBox();
            this.comboMqttPendingMessagesOverflowStrategy = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.nudConnectTimeout = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.nudMaxPendingMessagesPerClient = new System.Windows.Forms.NumericUpDown();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.rbTCP = new System.Windows.Forms.RadioButton();
            this.rbTSL = new System.Windows.Forms.RadioButton();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.cbSSLVersion = new System.Windows.Forms.ComboBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.tbSSLCertificate = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConnectTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPendingMessagesPerClient)).BeginInit();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nudPort);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtIp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 30);
            this.panel1.TabIndex = 0;
            // 
            // nudPort
            // 
            this.nudPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudPort.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudPort.Location = new System.Drawing.Point(306, 0);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(103, 26);
            this.nudPort.TabIndex = 3;
            this.nudPort.Value = new decimal(new int[] {
            1883,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(245, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口号:";
            // 
            // txtIp
            // 
            this.txtIp.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtIp.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIp.Location = new System.Drawing.Point(85, 0);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(160, 26);
            this.txtIp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbPwd);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.tbUser);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(0, 135);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(484, 30);
            this.panel2.TabIndex = 1;
            // 
            // tbPwd
            // 
            this.tbPwd.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbPwd.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPwd.Location = new System.Drawing.Point(306, 0);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(160, 26);
            this.tbPwd.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(245, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "密   码:";
            // 
            // tbUser
            // 
            this.tbUser.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbUser.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbUser.Location = new System.Drawing.Point(85, 0);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(160, 26);
            this.tbUser.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "用 户 名:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbEnableKeepSessions);
            this.panel3.Controls.Add(this.cbEnableUser);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 165);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(484, 30);
            this.panel3.TabIndex = 2;
            // 
            // cbEnableUser
            // 
            this.cbEnableUser.AutoSize = true;
            this.cbEnableUser.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbEnableUser.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbEnableUser.Location = new System.Drawing.Point(85, 0);
            this.cbEnableUser.Name = "cbEnableUser";
            this.cbEnableUser.Size = new System.Drawing.Size(112, 30);
            this.cbEnableUser.TabIndex = 1;
            this.cbEnableUser.Text = "启用用户验证";
            this.cbEnableUser.UseVisualStyleBackColor = true;
            this.cbEnableUser.CheckedChanged += new System.EventHandler(this.cbEnableUser_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 30);
            this.label6.TabIndex = 0;
            this.label6.Text = "            ";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cbWill);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.cbMessage);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 195);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(484, 30);
            this.panel5.TabIndex = 4;
            // 
            // cbWill
            // 
            this.cbWill.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbWill.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWill.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbWill.FormattingEnabled = true;
            this.cbWill.Items.AddRange(new object[] {
            "Will Flag",
            "Will QoS",
            "Will Retain Flag"});
            this.cbWill.Location = new System.Drawing.Point(330, 0);
            this.cbWill.Name = "cbWill";
            this.cbWill.Size = new System.Drawing.Size(136, 28);
            this.cbWill.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(245, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 30);
            this.label8.TabIndex = 2;
            this.label8.Text = "遗愿标志:";
            // 
            // cbMessage
            // 
            this.cbMessage.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbMessage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMessage.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMessage.FormattingEnabled = true;
            this.cbMessage.Items.AddRange(new object[] {
            "QoS 0 最多分发一次",
            "QoS 1 至少分发一次",
            "QoS 2 只分发一次"});
            this.cbMessage.Location = new System.Drawing.Point(85, 0);
            this.cbMessage.Name = "cbMessage";
            this.cbMessage.Size = new System.Drawing.Size(160, 28);
            this.cbMessage.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 30);
            this.label9.TabIndex = 0;
            this.label9.Text = "消息质量:";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.comboMqttPendingMessagesOverflowStrategy);
            this.panel8.Controls.Add(this.label12);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 225);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(484, 30);
            this.panel8.TabIndex = 7;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.cbReceiveMethod);
            this.panel9.Controls.Add(this.label13);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 255);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(484, 30);
            this.panel9.TabIndex = 8;
            // 
            // cbReceiveMethod
            // 
            this.cbReceiveMethod.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbReceiveMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReceiveMethod.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbReceiveMethod.FormattingEnabled = true;
            this.cbReceiveMethod.Items.AddRange(new object[] {
            "主动",
            "被动"});
            this.cbReceiveMethod.Location = new System.Drawing.Point(85, 0);
            this.cbReceiveMethod.Name = "cbReceiveMethod";
            this.cbReceiveMethod.Size = new System.Drawing.Size(381, 28);
            this.cbReceiveMethod.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.Dock = System.Windows.Forms.DockStyle.Left;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 30);
            this.label13.TabIndex = 0;
            this.label13.Text = "接收方式:   ";
            // 
            // cbEnableKeepSessions
            // 
            this.cbEnableKeepSessions.AutoSize = true;
            this.cbEnableKeepSessions.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbEnableKeepSessions.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbEnableKeepSessions.Location = new System.Drawing.Point(197, 0);
            this.cbEnableKeepSessions.Name = "cbEnableKeepSessions";
            this.cbEnableKeepSessions.Size = new System.Drawing.Size(112, 30);
            this.cbEnableKeepSessions.TabIndex = 2;
            this.cbEnableKeepSessions.Text = "启用持久会话";
            this.cbEnableKeepSessions.UseVisualStyleBackColor = true;
            // 
            // comboMqttPendingMessagesOverflowStrategy
            // 
            this.comboMqttPendingMessagesOverflowStrategy.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboMqttPendingMessagesOverflowStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMqttPendingMessagesOverflowStrategy.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboMqttPendingMessagesOverflowStrategy.FormattingEnabled = true;
            this.comboMqttPendingMessagesOverflowStrategy.Items.AddRange(new object[] {
            "DropOldestQueuedMessage",
            "DropNewMessage"});
            this.comboMqttPendingMessagesOverflowStrategy.Location = new System.Drawing.Point(85, 0);
            this.comboMqttPendingMessagesOverflowStrategy.Name = "comboMqttPendingMessagesOverflowStrategy";
            this.comboMqttPendingMessagesOverflowStrategy.Size = new System.Drawing.Size(381, 28);
            this.comboMqttPendingMessagesOverflowStrategy.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Left;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 30);
            this.label12.TabIndex = 3;
            this.label12.Text = "溢出策略:   ";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.nudMaxPendingMessagesPerClient);
            this.panel10.Controls.Add(this.label16);
            this.panel10.Controls.Add(this.label15);
            this.panel10.Controls.Add(this.nudConnectTimeout);
            this.panel10.Controls.Add(this.label14);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 285);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(484, 30);
            this.panel10.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Left;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 30);
            this.label14.TabIndex = 0;
            this.label14.Text = "连接超时:   ";
            // 
            // nudConnectTimeout
            // 
            this.nudConnectTimeout.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudConnectTimeout.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudConnectTimeout.Location = new System.Drawing.Point(85, 0);
            this.nudConnectTimeout.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudConnectTimeout.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudConnectTimeout.Name = "nudConnectTimeout";
            this.nudConnectTimeout.Size = new System.Drawing.Size(103, 26);
            this.nudConnectTimeout.TabIndex = 4;
            this.nudConnectTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.Dock = System.Windows.Forms.DockStyle.Left;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(188, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 30);
            this.label15.TabIndex = 5;
            this.label15.Text = "(秒)";
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Left;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(223, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(122, 30);
            this.label16.TabIndex = 6;
            this.label16.Text = "最大挂起消息数:";
            // 
            // nudMaxPendingMessagesPerClient
            // 
            this.nudMaxPendingMessagesPerClient.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudMaxPendingMessagesPerClient.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudMaxPendingMessagesPerClient.Location = new System.Drawing.Point(345, 0);
            this.nudMaxPendingMessagesPerClient.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudMaxPendingMessagesPerClient.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaxPendingMessagesPerClient.Name = "nudMaxPendingMessagesPerClient";
            this.nudMaxPendingMessagesPerClient.Size = new System.Drawing.Size(127, 26);
            this.nudMaxPendingMessagesPerClient.TabIndex = 7;
            this.nudMaxPendingMessagesPerClient.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.rbTSL);
            this.panel11.Controls.Add(this.rbTCP);
            this.panel11.Controls.Add(this.label17);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 100);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(484, 35);
            this.panel11.TabIndex = 10;
            // 
            // label17
            // 
            this.label17.Dock = System.Windows.Forms.DockStyle.Left;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(85, 35);
            this.label17.TabIndex = 1;
            this.label17.Text = "通讯模式:";
            // 
            // rbTCP
            // 
            this.rbTCP.AutoSize = true;
            this.rbTCP.Checked = true;
            this.rbTCP.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbTCP.Location = new System.Drawing.Point(85, 0);
            this.rbTCP.Name = "rbTCP";
            this.rbTCP.Size = new System.Drawing.Size(65, 35);
            this.rbTCP.TabIndex = 2;
            this.rbTCP.TabStop = true;
            this.rbTCP.Text = "TCP模式";
            this.rbTCP.UseVisualStyleBackColor = true;
            this.rbTCP.CheckedChanged += new System.EventHandler(this.rbTCP_CheckedChanged);
            // 
            // rbTSL
            // 
            this.rbTSL.AutoSize = true;
            this.rbTSL.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbTSL.Location = new System.Drawing.Point(150, 0);
            this.rbTSL.Name = "rbTSL";
            this.rbTSL.Size = new System.Drawing.Size(65, 35);
            this.rbTSL.TabIndex = 3;
            this.rbTSL.Text = "TLS模式";
            this.rbTSL.UseVisualStyleBackColor = true;
            this.rbTSL.CheckedChanged += new System.EventHandler(this.rbTSL_CheckedChanged);
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.cbSSLVersion);
            this.panel12.Controls.Add(this.label18);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 30);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(484, 35);
            this.panel12.TabIndex = 11;
            // 
            // label18
            // 
            this.label18.Dock = System.Windows.Forms.DockStyle.Left;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(0, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(85, 35);
            this.label18.TabIndex = 1;
            this.label18.Text = "SSL版本:";
            // 
            // cbSSLVersion
            // 
            this.cbSSLVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbSSLVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSSLVersion.Enabled = false;
            this.cbSSLVersion.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbSSLVersion.FormattingEnabled = true;
            this.cbSSLVersion.Items.AddRange(new object[] {
            "None",
            "Ssl2",
            "Ssl3",
            "Tls",
            "Tls11",
            "Tls12"});
            this.cbSSLVersion.Location = new System.Drawing.Point(85, 0);
            this.cbSSLVersion.Name = "cbSSLVersion";
            this.cbSSLVersion.Size = new System.Drawing.Size(381, 28);
            this.cbSSLVersion.TabIndex = 3;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.tbSSLCertificate);
            this.panel13.Controls.Add(this.label19);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 65);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(484, 35);
            this.panel13.TabIndex = 12;
            // 
            // label19
            // 
            this.label19.Dock = System.Windows.Forms.DockStyle.Left;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(0, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(85, 35);
            this.label19.TabIndex = 1;
            this.label19.Text = "SSL证书:";
            // 
            // tbSSLCertificate
            // 
            this.tbSSLCertificate.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbSSLCertificate.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbSSLCertificate.Location = new System.Drawing.Point(85, 0);
            this.tbSSLCertificate.Name = "tbSSLCertificate";
            this.tbSSLCertificate.Size = new System.Drawing.Size(387, 26);
            this.tbSSLCertificate.TabIndex = 2;
            // 
            // MQTTServerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.panel1);
            this.Name = "MQTTServerCtrl";
            this.Size = new System.Drawing.Size(484, 342);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudConnectTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPendingMessagesPerClient)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbEnableUser;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbMessage;
        private System.Windows.Forms.ComboBox cbWill;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.ComboBox cbReceiveMethod;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox cbEnableKeepSessions;
        private System.Windows.Forms.ComboBox comboMqttPendingMessagesOverflowStrategy;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudConnectTimeout;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown nudMaxPendingMessagesPerClient;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.RadioButton rbTSL;
        private System.Windows.Forms.RadioButton rbTCP;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cbSSLVersion;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbSSLCertificate;
    }
}
