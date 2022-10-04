using Scada.Controls.Controls.List;
using Scada.Controls.Controls.Page;
using Scada.Controls.Controls.SCADAChart;

namespace ScadaCenterServer.Pages
{
    partial class ScadaMachineTrainQueryWorkForm
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出CSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.search = new ScadaCenterServer.Controls.HistoryMachineTrainSearch();
            this.listView = new Scada.Controls.Controls.List.SCADAListView();
            this.columnID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnServerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCommunicationName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnParaLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnParaName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEventContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ucPagerControl = new Scada.Controls.Controls.Page.SCADAPager();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出CSVToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(124, 26);
            // 
            // 导出CSVToolStripMenuItem
            // 
            this.导出CSVToolStripMenuItem.Name = "导出CSVToolStripMenuItem";
            this.导出CSVToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.导出CSVToolStripMenuItem.Text = "导出CSV";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.search);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listView);
            this.splitContainer.Panel2.Controls.Add(this.ucPagerControl);
            this.splitContainer.Size = new System.Drawing.Size(995, 548);
            this.splitContainer.SplitterDistance = 33;
            this.splitContainer.TabIndex = 0;
            // 
            // search
            // 
            this.search.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.search.EndDate = new System.DateTime(2019, 12, 13, 3, 25, 21, 77);
            this.search.Location = new System.Drawing.Point(0, 0);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(995, 33);
            this.search.StartDate = new System.DateTime(2019, 12, 13, 3, 25, 21, 72);
            this.search.TabIndex = 0;
            this.search.SearchClick += new System.EventHandler(this.Search_SearchClick);
            this.search.SelectedIndexChanged += new System.EventHandler(this.Search_SelectedIndexChanged);
            // 
            // listView
            // 
            this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listView.AllowDrop = true;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.CheckBoxes = true;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnID,
            this.columnServerName,
            this.columnCommunicationName,
            this.columnDeviceName,
            this.columnParaLabel,
            this.columnParaName,
            this.columnEventName,
            this.columnEventContent});
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
            this.listView.Size = new System.Drawing.Size(995, 476);
            this.listView.TabIndex = 7;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnID
            // 
            this.columnID.Text = "任务ID";
            this.columnID.Width = 103;
            // 
            // columnServerName
            // 
            this.columnServerName.Text = "采集站";
            this.columnServerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnServerName.Width = 103;
            // 
            // columnCommunicationName
            // 
            this.columnCommunicationName.Text = "通道";
            this.columnCommunicationName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnCommunicationName.Width = 103;
            // 
            // columnDeviceName
            // 
            this.columnDeviceName.Text = "设备";
            this.columnDeviceName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDeviceName.Width = 103;
            // 
            // columnParaLabel
            // 
            this.columnParaLabel.Text = "任务名称";
            this.columnParaLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnParaLabel.Width = 103;
            // 
            // columnParaName
            // 
            this.columnParaName.Text = "执行时间";
            this.columnParaName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnParaName.Width = 175;
            // 
            // columnEventName
            // 
            this.columnEventName.Text = "执行结果";
            this.columnEventName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnEventName.Width = 103;
            // 
            // columnEventContent
            // 
            this.columnEventContent.Text = "备注";
            this.columnEventContent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnEventContent.Width = 180;
            // 
            // ucPagerControl
            // 
            this.ucPagerControl.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ucPagerControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucPagerControl.Location = new System.Drawing.Point(0, 476);
            this.ucPagerControl.Name = "ucPagerControl";
            this.ucPagerControl.PageCount = 0;
            this.ucPagerControl.PageIndex = 1;
            this.ucPagerControl.PageSize = 100;
            this.ucPagerControl.RecordCount = 0;
            this.ucPagerControl.Size = new System.Drawing.Size(995, 35);
            this.ucPagerControl.TabIndex = 6;
            this.ucPagerControl.OnPageIndexed += new Scada.Controls.Controls.PageChanged(this.UcPagerControl_OnPageIndexed);
            // 
            // ScadaMachineTrainQueryWorkForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(995, 548);
            this.Controls.Add(this.splitContainer);
            this.Name = "ScadaMachineTrainQueryWorkForm";
            this.Text = "机器训练日志查询";
            this.Load += new System.EventHandler(this.RealAlarmQueryWorkForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private SCADAPager ucPagerControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 导出CSVToolStripMenuItem;
        private SCADAListView listView;
        private System.Windows.Forms.ColumnHeader columnID;
        private System.Windows.Forms.ColumnHeader columnDeviceName;
        private System.Windows.Forms.ColumnHeader columnServerName;
        private System.Windows.Forms.ColumnHeader columnCommunicationName;
        private System.Windows.Forms.ColumnHeader columnParaName;
        private System.Windows.Forms.ColumnHeader columnParaLabel;
        private System.Windows.Forms.ColumnHeader columnEventContent;
        private System.Windows.Forms.ColumnHeader columnEventName;
        private Controls.HistoryMachineTrainSearch search;
    }
}