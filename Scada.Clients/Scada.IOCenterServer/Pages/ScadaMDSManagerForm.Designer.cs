
namespace ScadaCenterServer.Pages
{
    partial class ScadaMDSManagerForm
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ucBtnExport = new Scada.Controls.Controls.UCBtnExt();
            this.ucBtnExt2 = new Scada.Controls.Controls.UCBtnExt();
            this.ucBtnExt1 = new Scada.Controls.Controls.UCBtnExt();
            this.ucBtnAdd = new Scada.Controls.Controls.UCBtnExt();
            this.listView = new Scada.Controls.Controls.List.SCADAListView();
            this.columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMAC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ucBtnExport);
            this.splitContainer1.Panel1.Controls.Add(this.ucBtnExt2);
            this.splitContainer1.Panel1.Controls.Add(this.ucBtnExt1);
            this.splitContainer1.Panel1.Controls.Add(this.ucBtnAdd);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 36;
            this.splitContainer1.TabIndex = 0;
            // 
            // ucBtnExport
            // 
            this.ucBtnExport.BackColor = System.Drawing.Color.White;
            this.ucBtnExport.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnExport.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnExport.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnExport.BtnText = "导出授权";
            this.ucBtnExport.ConerRadius = 5;
            this.ucBtnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExport.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucBtnExport.FillColor = System.Drawing.Color.Green;
            this.ucBtnExport.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnExport.IsRadius = true;
            this.ucBtnExport.IsShowRect = true;
            this.ucBtnExport.IsShowTips = false;
            this.ucBtnExport.Location = new System.Drawing.Point(276, 0);
            this.ucBtnExport.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnExport.Name = "ucBtnExport";
            this.ucBtnExport.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtnExport.RectWidth = 1;
            this.ucBtnExport.Size = new System.Drawing.Size(92, 36);
            this.ucBtnExport.TabIndex = 3;
            this.ucBtnExport.TabStop = false;
            this.ucBtnExport.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnExport.TipsText = "";
            // 
            // ucBtnExt2
            // 
            this.ucBtnExt2.BackColor = System.Drawing.Color.White;
            this.ucBtnExt2.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnExt2.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnExt2.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnExt2.BtnText = "编辑采集站";
            this.ucBtnExt2.ConerRadius = 5;
            this.ucBtnExt2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExt2.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucBtnExt2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucBtnExt2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnExt2.IsRadius = true;
            this.ucBtnExt2.IsShowRect = true;
            this.ucBtnExt2.IsShowTips = false;
            this.ucBtnExt2.Location = new System.Drawing.Point(184, 0);
            this.ucBtnExt2.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnExt2.Name = "ucBtnExt2";
            this.ucBtnExt2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtnExt2.RectWidth = 1;
            this.ucBtnExt2.Size = new System.Drawing.Size(92, 36);
            this.ucBtnExt2.TabIndex = 2;
            this.ucBtnExt2.TabStop = false;
            this.ucBtnExt2.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnExt2.TipsText = "";
            this.ucBtnExt2.BtnClick += new System.EventHandler(this.ucBtnExt2_BtnClick);
            // 
            // ucBtnExt1
            // 
            this.ucBtnExt1.BackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnExt1.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnText = "删除采集站";
            this.ucBtnExt1.ConerRadius = 5;
            this.ucBtnExt1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExt1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucBtnExt1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.ucBtnExt1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnExt1.IsRadius = true;
            this.ucBtnExt1.IsShowRect = true;
            this.ucBtnExt1.IsShowTips = false;
            this.ucBtnExt1.Location = new System.Drawing.Point(92, 0);
            this.ucBtnExt1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnExt1.Name = "ucBtnExt1";
            this.ucBtnExt1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtnExt1.RectWidth = 1;
            this.ucBtnExt1.Size = new System.Drawing.Size(92, 36);
            this.ucBtnExt1.TabIndex = 1;
            this.ucBtnExt1.TabStop = false;
            this.ucBtnExt1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnExt1.TipsText = "";
            this.ucBtnExt1.BtnClick += new System.EventHandler(this.ucBtnExt1_BtnClick);
            // 
            // ucBtnAdd
            // 
            this.ucBtnAdd.BackColor = System.Drawing.Color.White;
            this.ucBtnAdd.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnAdd.BtnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnAdd.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnAdd.BtnText = "添加采集站";
            this.ucBtnAdd.ConerRadius = 5;
            this.ucBtnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucBtnAdd.FillColor = System.Drawing.Color.Green;
            this.ucBtnAdd.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnAdd.IsRadius = true;
            this.ucBtnAdd.IsShowRect = true;
            this.ucBtnAdd.IsShowTips = false;
            this.ucBtnAdd.Location = new System.Drawing.Point(0, 0);
            this.ucBtnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnAdd.Name = "ucBtnAdd";
            this.ucBtnAdd.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.ucBtnAdd.RectWidth = 1;
            this.ucBtnAdd.Size = new System.Drawing.Size(92, 36);
            this.ucBtnAdd.TabIndex = 0;
            this.ucBtnAdd.TabStop = false;
            this.ucBtnAdd.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnAdd.TipsText = "";
            this.ucBtnAdd.BtnClick += new System.EventHandler(this.ucBtnAdd_BtnClick);
            // 
            // listView
            // 
            this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listView.AllowDrop = true;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTitle,
            this.columnName,
            this.columnIP,
            this.columnMAC});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView.ForeColor = System.Drawing.SystemColors.MenuText;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(800, 410);
            this.listView.TabIndex = 8;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnTitle
            // 
            this.columnTitle.Text = "采集站名称";
            this.columnTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnTitle.Width = 322;
            // 
            // columnName
            // 
            this.columnName.Text = "标识";
            this.columnName.Width = 251;
            // 
            // columnIP
            // 
            this.columnIP.Text = "采集站IP地址";
            this.columnIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnIP.Width = 222;
            // 
            // columnMAC
            // 
            this.columnMAC.Text = "采集站物理标识";
            // 
            // ScadaMDSManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScadaMDSManagerForm";
            this.Text = "采集站授权管理";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Scada.Controls.Controls.UCBtnExt ucBtnAdd;
        private Scada.Controls.Controls.UCBtnExt ucBtnExt2;
        private Scada.Controls.Controls.UCBtnExt ucBtnExt1;
        private Scada.Controls.Controls.List.SCADAListView listView;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnTitle;
        private System.Windows.Forms.ColumnHeader columnIP;
        private Scada.Controls.Controls.UCBtnExt ucBtnExport;
        private System.Windows.Forms.ColumnHeader columnMAC;
    }
}