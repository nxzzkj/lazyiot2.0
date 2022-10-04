
namespace Modbus.ModbusService
{
    partial class Modbus_Serial_IP_Simulator_Device
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
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            this.SuspendLayout();
            // 
            // nudMax
            // 
            this.nudMax.Location = new System.Drawing.Point(111, 12);
            this.nudMax.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.nudMax.Minimum = new decimal(new int[] {
            50000,
            0,
            0,
            -2147483648});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(120, 21);
            this.nudMax.TabIndex = 8;
            this.nudMax.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "模拟最大值:";
            // 
            // nudMin
            // 
            this.nudMin.Location = new System.Drawing.Point(111, 39);
            this.nudMin.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.nudMin.Minimum = new decimal(new int[] {
            50000,
            0,
            0,
            -2147483648});
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(120, 21);
            this.nudMin.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "模拟最小值:";
            // 
            // Modbus_TCP_Simulator_Device
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudMin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudMax);
            this.Controls.Add(this.label2);
            this.Name = "Modbus_TCP_Simulator_Device";
            this.Size = new System.Drawing.Size(452, 172);
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMin;
        private System.Windows.Forms.Label label1;
    }
}
