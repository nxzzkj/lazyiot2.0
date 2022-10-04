namespace IOManager.Dialogs
{
    partial class MachineTrainingConditionEditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbConditionName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tbConditionTitle = new System.Windows.Forms.TextBox();
            this.tbConditionRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "工况标识:";
            // 
            // tbConditionName
            // 
            this.tbConditionName.Location = new System.Drawing.Point(91, 22);
            this.tbConditionName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbConditionName.Name = "tbConditionName";
            this.tbConditionName.Size = new System.Drawing.Size(180, 23);
            this.tbConditionName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "工况名称:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "备  注:";
            // 
            // btSave
            // 
            this.btSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btSave.Location = new System.Drawing.Point(0, 0);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(46, 33);
            this.btSave.TabIndex = 16;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btClose);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 258);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 33);
            this.panel1.TabIndex = 17;
            // 
            // btClose
            // 
            this.btClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.btClose.Location = new System.Drawing.Point(46, 0);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(46, 33);
            this.btClose.TabIndex = 17;
            this.btClose.Text = "关闭";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(277, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 17);
            this.label9.TabIndex = 24;
            this.label9.Text = "(英文字符,唯一标识)";
            // 
            // tbConditionTitle
            // 
            this.tbConditionTitle.Location = new System.Drawing.Point(91, 56);
            this.tbConditionTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbConditionTitle.Name = "tbConditionTitle";
            this.tbConditionTitle.Size = new System.Drawing.Size(199, 23);
            this.tbConditionTitle.TabIndex = 25;
            // 
            // tbConditionRemark
            // 
            this.tbConditionRemark.Location = new System.Drawing.Point(91, 87);
            this.tbConditionRemark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbConditionRemark.Multiline = true;
            this.tbConditionRemark.Name = "tbConditionRemark";
            this.tbConditionRemark.Size = new System.Drawing.Size(301, 127);
            this.tbConditionRemark.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "(中文名称)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 17);
            this.label4.TabIndex = 27;
            this.label4.Text = "模型学习训练请在web端进行";
            // 
            // MachineTrainingConditionEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 291);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbConditionTitle);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbConditionRemark);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbConditionName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MachineTrainingConditionEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工况编辑";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbConditionName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbConditionTitle;
        private System.Windows.Forms.TextBox tbConditionRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}