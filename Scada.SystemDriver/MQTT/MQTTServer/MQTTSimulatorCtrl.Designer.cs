
namespace MQTTnet
{
    partial class MQTTSimulatorCtrl
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
            this.cbAuto = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbServerIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.NumericUpDown();
            this.tbPeried = new System.Windows.Forms.NumericUpDown();
            this.btSave = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPeried)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.btSave);
            this.panel.Controls.Add(this.tbPeried);
            this.panel.Controls.Add(this.tbPort);
            this.panel.Controls.Add(this.cbAuto);
            this.panel.Controls.Add(this.label7);
            this.panel.Controls.Add(this.tbPassword);
            this.panel.Controls.Add(this.label6);
            this.panel.Controls.Add(this.tbUser);
            this.panel.Controls.Add(this.label5);
            this.panel.Controls.Add(this.label4);
            this.panel.Controls.Add(this.label3);
            this.panel.Controls.Add(this.tbServerIP);
            this.panel.Controls.Add(this.label2);
            this.panel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.panel.Size = new System.Drawing.Size(378, 884);
            this.panel.Controls.SetChildIndex(this.label1, 0);
            this.panel.Controls.SetChildIndex(this.label2, 0);
            this.panel.Controls.SetChildIndex(this.tbServerIP, 0);
            this.panel.Controls.SetChildIndex(this.label3, 0);
            this.panel.Controls.SetChildIndex(this.label4, 0);
            this.panel.Controls.SetChildIndex(this.label5, 0);
            this.panel.Controls.SetChildIndex(this.tbUser, 0);
            this.panel.Controls.SetChildIndex(this.label6, 0);
            this.panel.Controls.SetChildIndex(this.tbPassword, 0);
            this.panel.Controls.SetChildIndex(this.label7, 0);
            this.panel.Controls.SetChildIndex(this.cbAuto, 0);
            this.panel.Controls.SetChildIndex(this.tbPort, 0);
            this.panel.Controls.SetChildIndex(this.tbPeried, 0);
            this.panel.Controls.SetChildIndex(this.btSave, 0);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(57, -10);
            this.label1.Size = new System.Drawing.Size(0, 14);
            this.label1.Text = "";
            // 
            // cbAuto
            // 
            this.cbAuto.AutoSize = true;
            this.cbAuto.Location = new System.Drawing.Point(102, 174);
            this.cbAuto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbAuto.Name = "cbAuto";
            this.cbAuto.Size = new System.Drawing.Size(75, 21);
            this.cbAuto.TabIndex = 62;
            this.cbAuto.Text = "被动上传";
            this.cbAuto.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(190, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 17);
            this.label7.TabIndex = 61;
            this.label7.Text = "s";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(102, 143);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(102, 23);
            this.tbPassword.TabIndex = 60;
            this.tbPassword.Text = "123456";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 17);
            this.label6.TabIndex = 59;
            this.label6.Text = "密码:";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(102, 81);
            this.tbUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(191, 23);
            this.tbUser.TabIndex = 58;
            this.tbUser.Text = "admin";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 57;
            this.label5.Text = "用户名:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 55;
            this.label4.Text = "保持有效期:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 53;
            this.label3.Text = "端口号:";
            // 
            // tbServerIP
            // 
            this.tbServerIP.Location = new System.Drawing.Point(105, 19);
            this.tbServerIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbServerIP.Name = "tbServerIP";
            this.tbServerIP.Size = new System.Drawing.Size(191, 23);
            this.tbServerIP.TabIndex = 52;
            this.tbServerIP.Text = "192.168.2.7";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 51;
            this.label2.Text = "MQTT服务器:";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(102, 50);
            this.tbPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPort.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(191, 23);
            this.tbPort.TabIndex = 71;
            this.tbPort.Value = 1883;
            // 
            // tbPeried
            // 
            this.tbPeried.Location = new System.Drawing.Point(104, 112);
            this.tbPeried.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPeried.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.tbPeried.Name = "tbPeried";
            this.tbPeried.Size = new System.Drawing.Size(82, 23);
            this.tbPeried.TabIndex = 72;
            this.tbPeried.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(99, 205);
            this.btSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(87, 33);
            this.btSave.TabIndex = 73;
            this.btSave.Text = "修改配置";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // MQTTSimulatorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "MQTTSimulatorCtrl";
            this.Size = new System.Drawing.Size(1413, 884);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPeried)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox cbAuto;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbServerIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown tbPort;
        private System.Windows.Forms.NumericUpDown tbPeried;
        private System.Windows.Forms.Button btSave;
    }
}
