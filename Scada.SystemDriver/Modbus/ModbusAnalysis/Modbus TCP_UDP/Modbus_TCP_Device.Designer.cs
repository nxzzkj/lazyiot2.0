
namespace Modbus.ModbusAnalysis
{
    partial class Modbus_TCP_Device
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nbRetiresInternal = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRetries = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ndRetiresNum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbRetiresInternal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndRetiresNum)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nbRetiresInternal);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbRetries);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.ndRetiresNum);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 75);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "重试    ";
            // 
            // nbRetiresInternal
            // 
            this.nbRetiresInternal.Location = new System.Drawing.Point(104, 53);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "重试次数：";
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
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 28;
            this.label11.Text = "重试间隔(ms)：";
            // 
            // ndRetiresNum
            // 
            this.ndRetiresNum.Location = new System.Drawing.Point(104, 26);
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
            // Modbus_TCP_Device
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "Modbus_TCP_Device";
            this.Size = new System.Drawing.Size(263, 99);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nbRetiresInternal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndRetiresNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nbRetiresInternal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbRetries;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown ndRetiresNum;
    }
}
