﻿namespace ScadaFlowDesign.Dialog
{
    using MySql.Data.MySqlClient;
    using Scada.DBUtility;
    using System;
    using System.ComponentModel;
    using System.Data.SQLite;
    using System.Drawing;
    using System.Windows.Forms;


     
    /*----------------------------------------------------------------
    // Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
    // 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
    // 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
    // 请大家尊重作者的劳动成果，共同促进行业健康发展。
    // 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
    // 创建者：马勇
    //----------------------------------------------------------------*/
    public class SQLiteConnectionFrm : Form
    {
        private SQLite_Connection _connection;
        private IContainer components;
        private Panel panel1;
        private Panel panel2;
        private Button btCancel;
        private Button btOK;
        private Button btTest;
        private Panel panel7;
        private TextBox tbVersion;
        private Label label5;
        private Panel panel6;
        private TextBox tbPassword;
        private Label label4;
        private Panel panel3;
        private TextBox tbDataSource;
        private Label label1;
        private Panel panel8;
        private TextBox tbConnectString;
        private Label label6;
        private Button btFile;

        public SQLiteConnectionFrm()
        {
            this.InitializeComponent();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void btFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbDataSource.Text = dialog.FileName;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbDataSource.Text))
            {
                MessageBox.Show(this, "请输入数据库DataSource");
            }
            else if (string.IsNullOrEmpty(this.tbVersion.Text))
            {
                MessageBox.Show(this, "请输入数据库Version");
            }
            else
            {
                try
                {
                    SQLiteConnection connection1 = new SQLiteConnection(DESEncrypt.Decrypt(this.Connection.ConnectionString));
                    connection1.Open();
                    connection1.Close();
                    base.DialogResult = DialogResult.OK;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, "测试链接失败，错误" + exception.Message);
                }
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbDataSource.Text))
            {
                MessageBox.Show(this, "请输入数据库DataSource");
            }
            else if (string.IsNullOrEmpty(this.tbVersion.Text))
            {
                MessageBox.Show(this, "请输入数据库Version");
            }
            else
            {
                try
                {
                    MySqlConnection connection1 = new MySqlConnection(DESEncrypt.Decrypt(this.Connection.ConnectionString));
                    connection1.Open();
                    connection1.Close();
                    MessageBox.Show(this, "链接数据库成功");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, "测试链接失败，错误" + exception.Message);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tbConnectString = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btFile = new System.Windows.Forms.Button();
            this.tbDataSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btTest = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 240);
            this.panel1.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.tbConnectString);
            this.panel8.Controls.Add(this.label6);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 81);
            this.panel8.Margin = new System.Windows.Forms.Padding(4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(426, 45);
            this.panel8.TabIndex = 12;
            // 
            // tbConnectString
            // 
            this.tbConnectString.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbConnectString.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbConnectString.Location = new System.Drawing.Point(87, 0);
            this.tbConnectString.Multiline = true;
            this.tbConnectString.Name = "tbConnectString";
            this.tbConnectString.Size = new System.Drawing.Size(295, 45);
            this.tbConnectString.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 45);
            this.label6.TabIndex = 0;
            this.label6.Text = "链接字符串:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.tbVersion);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 56);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(426, 25);
            this.panel7.TabIndex = 11;
            // 
            // tbVersion
            // 
            this.tbVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbVersion.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbVersion.Location = new System.Drawing.Point(87, 0);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(295, 26);
            this.tbVersion.TabIndex = 1;
            this.tbVersion.Text = "3";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tbPassword);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 28);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(426, 28);
            this.panel6.TabIndex = 10;
            // 
            // tbPassword
            // 
            this.tbPassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbPassword.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPassword.Location = new System.Drawing.Point(87, 0);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(295, 26);
            this.tbPassword.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "password:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btFile);
            this.panel3.Controls.Add(this.tbDataSource);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(426, 28);
            this.panel3.TabIndex = 7;
            // 
            // btFile
            // 
            this.btFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.btFile.Location = new System.Drawing.Point(382, 0);
            this.btFile.Margin = new System.Windows.Forms.Padding(4);
            this.btFile.Name = "btFile";
            this.btFile.Size = new System.Drawing.Size(29, 28);
            this.btFile.TabIndex = 14;
            this.btFile.Text = "...";
            this.btFile.UseVisualStyleBackColor = true;
            this.btFile.Click += new System.EventHandler(this.btFile_Click);
            // 
            // tbDataSource
            // 
            this.tbDataSource.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbDataSource.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDataSource.Location = new System.Drawing.Point(87, 0);
            this.tbDataSource.Name = "tbDataSource";
            this.tbDataSource.ReadOnly = true;
            this.tbDataSource.Size = new System.Drawing.Size(295, 26);
            this.tbDataSource.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "DataSource:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btCancel);
            this.panel2.Controls.Add(this.btOK);
            this.panel2.Controls.Add(this.btTest);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 240);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(426, 37);
            this.panel2.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCancel.Location = new System.Drawing.Point(149, 0);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(53, 37);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "取消";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOK
            // 
            this.btOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btOK.Location = new System.Drawing.Point(94, 0);
            this.btOK.Margin = new System.Windows.Forms.Padding(4);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(55, 37);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "保存";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btTest
            // 
            this.btTest.Dock = System.Windows.Forms.DockStyle.Left;
            this.btTest.Location = new System.Drawing.Point(0, 0);
            this.btTest.Margin = new System.Windows.Forms.Padding(4);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(94, 37);
            this.btTest.TabIndex = 7;
            this.btTest.Text = "测试链接";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // SQLiteConnectionFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 277);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SQLiteConnectionFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQLite 数据库配置";
            this.panel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        public SQLite_Connection Connection
        {
            get
            {
                SQLite_Connection connection1 = new SQLite_Connection {
                    password = DESEncrypt.Encrypt(this.tbPassword.Text),
                    DataSource = DESEncrypt.Encrypt(this.tbDataSource.Text),
                    Version = DESEncrypt.Encrypt(this.tbVersion.Text)
                };
                this._connection = connection1;
                if (string.IsNullOrEmpty(this.tbConnectString.Text) || string.IsNullOrEmpty(this._connection.ConnectionString))
                {
                    this._connection.ConnectionString = DESEncrypt.Encrypt("Data Source=" + DESEncrypt.Decrypt(this._connection.DataSource) + ";Version=" + DESEncrypt.Decrypt(this._connection.Version) + (string.IsNullOrEmpty(this._connection.password) ? "" : (";Password=" + DESEncrypt.Decrypt(this._connection.password) + ";")));
                }
                return this._connection;
            }
            set
            {
                this._connection = value;
                if (this._connection != null)
                {
                    this.tbPassword.Text = DESEncrypt.Decrypt(this._connection.password);
                    this.tbVersion.Text = DESEncrypt.Decrypt(this._connection.Version.ToString());
                    this.tbDataSource.Text = DESEncrypt.Decrypt(this._connection.DataSource);
                }
            }
        }
    }
}

