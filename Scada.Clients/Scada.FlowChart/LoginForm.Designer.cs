﻿namespace ScadaFlowDesign
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.ucBtnClose = new Scada.Controls.Controls.UCBtnExt();
            this.ucBtLogin = new Scada.Controls.Controls.UCBtnExt();
            this.labelInfo = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPhysicalAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ucBtnClose
            // 
            this.ucBtnClose.BackColor = System.Drawing.Color.White;
            this.ucBtnClose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucBtnClose.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnClose.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnClose.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnClose.BtnText = " 关 闭 ";
            this.ucBtnClose.ConerRadius = 5;
            this.ucBtnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucBtnClose.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnClose.IsRadius = true;
            this.ucBtnClose.IsShowRect = true;
            this.ucBtnClose.IsShowTips = false;
            this.ucBtnClose.Location = new System.Drawing.Point(581, 463);
            this.ucBtnClose.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnClose.Name = "ucBtnClose";
            this.ucBtnClose.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucBtnClose.RectWidth = 2;
            this.ucBtnClose.Size = new System.Drawing.Size(92, 34);
            this.ucBtnClose.TabIndex = 3;
            this.ucBtnClose.TabStop = false;
            this.ucBtnClose.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnClose.TipsText = "";
            this.ucBtnClose.BtnClick += new System.EventHandler(this.ucBtnClose_BtnClick);
            // 
            // ucBtLogin
            // 
            this.ucBtLogin.BackColor = System.Drawing.Color.White;
            this.ucBtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucBtLogin.BtnBackColor = System.Drawing.Color.White;
            this.ucBtLogin.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtLogin.BtnForeColor = System.Drawing.Color.White;
            this.ucBtLogin.BtnText = "登录管理";
            this.ucBtLogin.ConerRadius = 5;
            this.ucBtLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtLogin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucBtLogin.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtLogin.IsRadius = true;
            this.ucBtLogin.IsShowRect = true;
            this.ucBtLogin.IsShowTips = false;
            this.ucBtLogin.Location = new System.Drawing.Point(485, 463);
            this.ucBtLogin.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtLogin.Name = "ucBtLogin";
            this.ucBtLogin.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtLogin.RectWidth = 2;
            this.ucBtLogin.Size = new System.Drawing.Size(92, 34);
            this.ucBtLogin.TabIndex = 2;
            this.ucBtLogin.TabStop = false;
            this.ucBtLogin.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtLogin.TipsText = "";
            this.ucBtLogin.BtnClick += new System.EventHandler(this.ucBtLogin_BtnClick);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.BackColor = System.Drawing.Color.Transparent;
            this.labelInfo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.ForeColor = System.Drawing.Color.White;
            this.labelInfo.Location = new System.Drawing.Point(215, 164);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(131, 21);
            this.labelInfo.TabIndex = 4;
            this.labelInfo.Text = "请确保网络正常1";
            // 
            // tbUser
            // 
            this.tbUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUser.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbUser.Location = new System.Drawing.Point(223, 197);
            this.tbUser.Multiline = true;
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(245, 40);
            this.tbUser.TabIndex = 6;
            this.tbUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbPassword
            // 
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPassword.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPassword.Location = new System.Drawing.Point(223, 269);
            this.tbPassword.Multiline = true;
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(245, 40);
            this.tbPassword.TabIndex = 7;
            this.tbPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIP
            // 
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIP.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIP.Location = new System.Drawing.Point(223, 374);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(245, 34);
            this.txtIP.TabIndex = 17;
            this.txtIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(10, 468);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 21);
            this.label1.TabIndex = 24;
            this.label1.Text = "采集站物理标识:";
            // 
            // tbPhysicalAddress
            // 
            this.tbPhysicalAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPhysicalAddress.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPhysicalAddress.Location = new System.Drawing.Point(142, 463);
            this.tbPhysicalAddress.Name = "tbPhysicalAddress";
            this.tbPhysicalAddress.Size = new System.Drawing.Size(340, 34);
            this.tbPhysicalAddress.TabIndex = 23;
            this.tbPhysicalAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(676, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPhysicalAddress);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.ucBtnClose);
            this.Controls.Add(this.ucBtLogin);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "流程设计管理登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Scada.Controls.Controls.UCBtnExt ucBtLogin;
        private Scada.Controls.Controls.UCBtnExt ucBtnClose;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPhysicalAddress;
    }
}