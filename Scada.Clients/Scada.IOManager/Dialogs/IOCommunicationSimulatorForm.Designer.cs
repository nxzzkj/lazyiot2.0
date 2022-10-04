
namespace IOManager.Dialogs
{
    partial class IOCommunicationSimulatorForm
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
            this.panelSimulator = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSimulator
            // 
            this.panelSimulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSimulator.Location = new System.Drawing.Point(0, 0);
            this.panelSimulator.Name = "panelSimulator";
            this.panelSimulator.Size = new System.Drawing.Size(986, 570);
            this.panelSimulator.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btClose);
            this.panel2.Controls.Add(this.btSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 570);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(986, 32);
            this.panel2.TabIndex = 1;
            // 
            // btClose
            // 
            this.btClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.btClose.Location = new System.Drawing.Point(75, 0);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 32);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "关闭";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btSave
            // 
            this.btSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btSave.Location = new System.Drawing.Point(0, 0);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 32);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // IOCommunicationSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 602);
            this.Controls.Add(this.panelSimulator);
            this.Controls.Add(this.panel2);
            this.Name = "IOCommunicationSimulatorForm";
            this.Text = "模拟器配置";
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSimulator;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btSave;
    }
}