
namespace Scada.Kernel
{
    partial class SimulatorEditDeviceParaDialog
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
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btStop = new System.Windows.Forms.Button();
            this.btStart = new System.Windows.Forms.Button();
            this.nbMax = new System.Windows.Forms.NumericUpDown();
            this.nbMin = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nbMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbMin)).BeginInit();
            this.SuspendLayout();
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(64, 12);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(224, 21);
            this.tbName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "IO名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "模拟最大值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "模拟最小值";
            // 
            // btStop
            // 
            this.btStop.Location = new System.Drawing.Point(222, 112);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(66, 23);
            this.btStop.TabIndex = 17;
            this.btStop.Text = "关闭";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(150, 112);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(66, 23);
            this.btStart.TabIndex = 16;
            this.btStart.Text = "保存";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // nbMax
            // 
            this.nbMax.Location = new System.Drawing.Point(88, 51);
            this.nbMax.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nbMax.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nbMax.Name = "nbMax";
            this.nbMax.Size = new System.Drawing.Size(120, 21);
            this.nbMax.TabIndex = 18;
            this.nbMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // nbMin
            // 
            this.nbMin.Location = new System.Drawing.Point(88, 78);
            this.nbMin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nbMin.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nbMin.Name = "nbMin";
            this.nbMin.Size = new System.Drawing.Size(120, 21);
            this.nbMin.TabIndex = 19;
            this.nbMin.Value = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            // 
            // DevicePara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 147);
            this.Controls.Add(this.nbMin);
            this.Controls.Add(this.nbMax);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevicePara";
            this.Text = "编辑IO参数";
            ((System.ComponentModel.ISupportInitialize)(this.nbMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.NumericUpDown nbMax;
        private System.Windows.Forms.NumericUpDown nbMin;
    }
}