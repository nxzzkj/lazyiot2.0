namespace ScadaFlowDesign.Dialog
{
    partial class FlowUserManagerForm
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
            this.tbNikeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbWrite = new System.Windows.Forms.CheckBox();
            this.cbRead = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbNikeName
            // 
            this.tbNikeName.Location = new System.Drawing.Point(92, 23);
            this.tbNikeName.Name = "tbNikeName";
            this.tbNikeName.Size = new System.Drawing.Size(346, 26);
            this.tbNikeName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "  昵    称：";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(92, 56);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(346, 26);
            this.tbUserName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "  用户名 ：";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(92, 90);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(346, 26);
            this.tbPassword.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "  密    码：";
            // 
            // cbWrite
            // 
            this.cbWrite.AutoSize = true;
            this.cbWrite.Checked = true;
            this.cbWrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWrite.Location = new System.Drawing.Point(154, 135);
            this.cbWrite.Name = "cbWrite";
            this.cbWrite.Size = new System.Drawing.Size(56, 24);
            this.cbWrite.TabIndex = 3;
            this.cbWrite.Text = "可写";
            this.cbWrite.UseVisualStyleBackColor = true;
            // 
            // cbRead
            // 
            this.cbRead.AutoSize = true;
            this.cbRead.Checked = true;
            this.cbRead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRead.Location = new System.Drawing.Point(92, 135);
            this.cbRead.Name = "cbRead";
            this.cbRead.Size = new System.Drawing.Size(56, 24);
            this.cbRead.TabIndex = 2;
            this.cbRead.Text = "可读";
            this.cbRead.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(14, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 26);
            this.label4.TabIndex = 1;
            this.label4.Text = "  系统权限：";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(22, 177);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 35);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "确定";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(105, 177);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 35);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "取消";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // FlowUserManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 215);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbNikeName);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbWrite);
            this.Controls.Add(this.cbRead);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlowUserManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
      
        private System.Windows.Forms.Label label1;
  
        private System.Windows.Forms.Label label2;
 
        private System.Windows.Forms.Label label3;
 
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbNikeName;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.CheckBox cbWrite;
        private System.Windows.Forms.CheckBox cbRead;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
    }
}