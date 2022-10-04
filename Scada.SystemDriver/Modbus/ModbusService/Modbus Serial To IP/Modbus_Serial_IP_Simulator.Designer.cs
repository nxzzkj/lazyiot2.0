
namespace Modbus.ModbusService
{
    partial class Modbus_Serial_IP_Simulator
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
            this.label2 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.nudUpdateCycle = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRegisterNum = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegisterNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.nudStart);
            this.panel.Controls.Add(this.label5);
            this.panel.Controls.Add(this.nudRegisterNum);
            this.panel.Controls.Add(this.label4);
            this.panel.Controls.Add(this.label3);
            this.panel.Controls.Add(this.nudUpdateCycle);
            this.panel.Controls.Add(this.btSave);
            this.panel.Controls.Add(this.label2);
            this.panel.Controls.SetChildIndex(this.label1, 0);
            this.panel.Controls.SetChildIndex(this.label2, 0);
            this.panel.Controls.SetChildIndex(this.btSave, 0);
            this.panel.Controls.SetChildIndex(this.nudUpdateCycle, 0);
            this.panel.Controls.SetChildIndex(this.label3, 0);
            this.panel.Controls.SetChildIndex(this.label4, 0);
            this.panel.Controls.SetChildIndex(this.nudRegisterNum, 0);
            this.panel.Controls.SetChildIndex(this.label5, 0);
            this.panel.Controls.SetChildIndex(this.nudStart, 0);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 542);
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据更新周期";
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(157, 128);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 28);
            this.btSave.TabIndex = 5;
            this.btSave.Text = "修改配置";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // nudUpdateCycle
            // 
            this.nudUpdateCycle.Location = new System.Drawing.Point(112, 28);
            this.nudUpdateCycle.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudUpdateCycle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudUpdateCycle.Name = "nudUpdateCycle";
            this.nudUpdateCycle.Size = new System.Drawing.Size(120, 23);
            this.nudUpdateCycle.TabIndex = 6;
            this.nudUpdateCycle.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "秒";
            // 
            // nudRegisterNum
            // 
            this.nudRegisterNum.Location = new System.Drawing.Point(112, 57);
            this.nudRegisterNum.Maximum = new decimal(new int[] {
            39999,
            0,
            0,
            0});
            this.nudRegisterNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRegisterNum.Name = "nudRegisterNum";
            this.nudRegisterNum.Size = new System.Drawing.Size(120, 23);
            this.nudRegisterNum.TabIndex = 9;
            this.nudRegisterNum.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "寄存器数量";
            // 
            // nudStart
            // 
            this.nudStart.Location = new System.Drawing.Point(112, 86);
            this.nudStart.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStart.Name = "nudStart";
            this.nudStart.Size = new System.Drawing.Size(120, 23);
            this.nudStart.TabIndex = 11;
            this.nudStart.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "起始地址";
            // 
            // Modbus_TCP_Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Modbus_TCP_Simulator";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegisterNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudUpdateCycle;
        private System.Windows.Forms.NumericUpDown nudRegisterNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudStart;
        private System.Windows.Forms.Label label5;
    }
}
