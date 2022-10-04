using Scada.Controls.Controls;
using Scada.Controls.Controls.List;
using ScadaCenterServer.Controls;

namespace ScadaCenterServer
{
    partial class MonitorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorForm));
            this.label3 = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.tabControlMonitor = new Scada.Controls.Controls.TabControlExt();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ucLog = new Scada.Controls.Controls.UCCheckBox();
            this.listViewReport = new Scada.Controls.Controls.List.SCADAListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEventNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEventServerHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEventCommunicationHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEventDeviceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ucSplitLine_H4 = new Scada.Controls.Controls.UCSplitLine_H();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ucReceive = new Scada.Controls.Controls.UCCheckBox();
            this.listViewReceive = new Scada.Controls.Controls.List.SCADAListView();
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ucSplitLine_H3 = new Scada.Controls.Controls.UCSplitLine_H();
            this.cbSendCommandSize = new Scada.Controls.Controls.SCADAPageCombox();
            this.ucbSendCommand = new Scada.Controls.Controls.UCCheckBox();
            this.listViewSendCommand = new Scada.Controls.Controls.List.SCADAListView();
            this.columnDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnserver = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columndevice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columniopara = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnvalue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnresult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnuser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ucSplitLine_H2 = new Scada.Controls.Controls.UCSplitLine_H();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.ucEnableAlarm = new Scada.Controls.Controls.UCCheckBox();
            this.listViewAlarm = new Scada.Controls.Controls.List.SCADAListView();
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ucSplitLine_H5 = new Scada.Controls.Controls.UCSplitLine_H();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ucCheckBoxAi = new Scada.Controls.Controls.UCCheckBox();
            this.machineTrainListView = new Scada.Controls.Controls.List.SCADAListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader28 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader29 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader30 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ucCheckBoxBatchCommand = new Scada.Controls.Controls.UCCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.listViewBatchCommand = new Scada.Controls.Controls.List.SCADAListView();
            this.columnHeader32 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader33 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader34 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader35 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader36 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader37 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControlMonitor.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1180, 28);
            this.label3.TabIndex = 7;
            this.label3.Text = "采集器采集接收数据:";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(280, 25);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(0, 12);
            this.labelIP.TabIndex = 15;
            // 
            // tabControlMonitor
            // 
            this.tabControlMonitor.Controls.Add(this.tabPage4);
            this.tabControlMonitor.Controls.Add(this.tabPage3);
            this.tabControlMonitor.Controls.Add(this.tabPage5);
            this.tabControlMonitor.Controls.Add(this.tabPage1);
            this.tabControlMonitor.Controls.Add(this.tabPage2);
            this.tabControlMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMonitor.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlMonitor.IsShowCloseBtn = false;
            this.tabControlMonitor.ItemSize = new System.Drawing.Size(0, 30);
            this.tabControlMonitor.Location = new System.Drawing.Point(0, 0);
            this.tabControlMonitor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControlMonitor.Name = "tabControlMonitor";
            this.tabControlMonitor.SelectedIndex = 0;
            this.tabControlMonitor.Size = new System.Drawing.Size(1194, 571);
            this.tabControlMonitor.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.ucLog);
            this.tabPage4.Controls.Add(this.listViewReport);
            this.tabPage4.Controls.Add(this.ucSplitLine_H4);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage4.Size = new System.Drawing.Size(1186, 533);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "系统事件";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ucLog
            // 
            this.ucLog.BackColor = System.Drawing.Color.Transparent;
            this.ucLog.Checked = true;
            this.ucLog.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucLog.Location = new System.Drawing.Point(91, 2);
            this.ucLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucLog.Name = "ucLog";
            this.ucLog.Padding = new System.Windows.Forms.Padding(1);
            this.ucLog.Size = new System.Drawing.Size(105, 24);
            this.ucLog.TabIndex = 19;
            this.ucLog.TextValue = "实时显示";
            // 
            // listViewReport
            // 
            this.listViewReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnEventNameHeader,
            this.columnEventServerHeader,
            this.columnEventCommunicationHeader,
            this.columnEventDeviceHeader,
            this.columnHeader4});
            this.listViewReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewReport.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewReport.FullRowSelect = true;
            this.listViewReport.GridLines = true;
            this.listViewReport.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewReport.HideSelection = false;
            this.listViewReport.Location = new System.Drawing.Point(3, 31);
            this.listViewReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listViewReport.Name = "listViewReport";
            this.listViewReport.Size = new System.Drawing.Size(1180, 500);
            this.listViewReport.TabIndex = 12;
            this.listViewReport.UseCompatibleStateImageBehavior = false;
            this.listViewReport.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "事件时间";
            this.columnHeader3.Width = 137;
            // 
            // columnEventNameHeader
            // 
            this.columnEventNameHeader.Text = "事件类型";
            this.columnEventNameHeader.Width = 137;
            // 
            // columnEventServerHeader
            // 
            this.columnEventServerHeader.Text = "采集站";
            this.columnEventServerHeader.Width = 137;
            // 
            // columnEventCommunicationHeader
            // 
            this.columnEventCommunicationHeader.Text = "通道";
            this.columnEventCommunicationHeader.Width = 137;
            // 
            // columnEventDeviceHeader
            // 
            this.columnEventDeviceHeader.Text = "设备";
            this.columnEventDeviceHeader.Width = 137;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "事件内容";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 415;
            // 
            // ucSplitLine_H4
            // 
            this.ucSplitLine_H4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ucSplitLine_H4.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSplitLine_H4.Location = new System.Drawing.Point(3, 30);
            this.ucSplitLine_H4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucSplitLine_H4.Name = "ucSplitLine_H4";
            this.ucSplitLine_H4.Size = new System.Drawing.Size(1180, 1);
            this.ucSplitLine_H4.TabIndex = 14;
            this.ucSplitLine_H4.TabStop = false;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label7.Location = new System.Drawing.Point(3, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1180, 28);
            this.label7.TabIndex = 13;
            this.label7.Text = "系统事件日志:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Size = new System.Drawing.Size(1186, 533);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "数据监视";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(3, 2);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.ucReceive);
            this.splitContainer3.Panel1.Controls.Add(this.listViewReceive);
            this.splitContainer3.Panel1.Controls.Add(this.ucSplitLine_H3);
            this.splitContainer3.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.cbSendCommandSize);
            this.splitContainer3.Panel2.Controls.Add(this.ucbSendCommand);
            this.splitContainer3.Panel2.Controls.Add(this.listViewSendCommand);
            this.splitContainer3.Panel2.Controls.Add(this.ucSplitLine_H2);
            this.splitContainer3.Panel2.Controls.Add(this.label5);
            this.splitContainer3.Size = new System.Drawing.Size(1180, 529);
            this.splitContainer3.SplitterDistance = 355;
            this.splitContainer3.SplitterWidth = 3;
            this.splitContainer3.TabIndex = 0;
            // 
            // ucReceive
            // 
            this.ucReceive.BackColor = System.Drawing.Color.Transparent;
            this.ucReceive.Checked = true;
            this.ucReceive.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucReceive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucReceive.Location = new System.Drawing.Point(140, 0);
            this.ucReceive.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucReceive.Name = "ucReceive";
            this.ucReceive.Padding = new System.Windows.Forms.Padding(1);
            this.ucReceive.Size = new System.Drawing.Size(127, 24);
            this.ucReceive.TabIndex = 18;
            this.ucReceive.TextValue = "实时显示";
            // 
            // listViewReceive
            // 
            this.listViewReceive.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewReceive.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12,
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader2,
            this.columnHeader11,
            this.columnHeader6,
            this.columnHeader13});
            this.listViewReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewReceive.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewReceive.FullRowSelect = true;
            this.listViewReceive.GridLines = true;
            this.listViewReceive.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewReceive.HideSelection = false;
            this.listViewReceive.Location = new System.Drawing.Point(0, 29);
            this.listViewReceive.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listViewReceive.Name = "listViewReceive";
            this.listViewReceive.Size = new System.Drawing.Size(1180, 326);
            this.listViewReceive.TabIndex = 12;
            this.listViewReceive.UseCompatibleStateImageBehavior = false;
            this.listViewReceive.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "采集站IP";
            this.columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "采集时间";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 110;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "设备";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 144;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "通道";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 161;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "采集站";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "采集值";
            this.columnHeader6.Width = 305;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "入库结果";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ucSplitLine_H3
            // 
            this.ucSplitLine_H3.BackColor = System.Drawing.Color.Gray;
            this.ucSplitLine_H3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSplitLine_H3.Location = new System.Drawing.Point(0, 28);
            this.ucSplitLine_H3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucSplitLine_H3.Name = "ucSplitLine_H3";
            this.ucSplitLine_H3.Size = new System.Drawing.Size(1180, 1);
            this.ucSplitLine_H3.TabIndex = 8;
            this.ucSplitLine_H3.TabStop = false;
            // 
            // cbSendCommandSize
            // 
            this.cbSendCommandSize.BackColor = System.Drawing.Color.Transparent;
            this.cbSendCommandSize.BackColorExt = System.Drawing.SystemColors.ControlLightLight;
            this.cbSendCommandSize.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSendCommandSize.ConerRadius = 5;
            this.cbSendCommandSize.DropPanelHeight = -1;
            this.cbSendCommandSize.FillColor = System.Drawing.SystemColors.ControlLightLight;
            this.cbSendCommandSize.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbSendCommandSize.IsRadius = true;
            this.cbSendCommandSize.IsShowRect = true;
            this.cbSendCommandSize.ItemWidth = 70;
            this.cbSendCommandSize.Location = new System.Drawing.Point(269, 0);
            this.cbSendCommandSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbSendCommandSize.Name = "cbSendCommandSize";
            this.cbSendCommandSize.RectColor = System.Drawing.SystemColors.ControlLightLight;
            this.cbSendCommandSize.RectWidth = 1;
            this.cbSendCommandSize.SelectedIndex = 0;
            this.cbSendCommandSize.SelectedValue = "100";
            this.cbSendCommandSize.Size = new System.Drawing.Size(154, 24);
            this.cbSendCommandSize.Source = ((System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>)(resources.GetObject("cbSendCommandSize.Source")));
            this.cbSendCommandSize.TabIndex = 18;
            this.cbSendCommandSize.TextValue = "显示最近100条";
            this.cbSendCommandSize.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            // 
            // ucbSendCommand
            // 
            this.ucbSendCommand.BackColor = System.Drawing.Color.Transparent;
            this.ucbSendCommand.Checked = true;
            this.ucbSendCommand.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucbSendCommand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucbSendCommand.Location = new System.Drawing.Point(172, 0);
            this.ucbSendCommand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucbSendCommand.Name = "ucbSendCommand";
            this.ucbSendCommand.Padding = new System.Windows.Forms.Padding(1);
            this.ucbSendCommand.Size = new System.Drawing.Size(113, 26);
            this.ucbSendCommand.TabIndex = 17;
            this.ucbSendCommand.TextValue = "实时显示";
            // 
            // listViewSendCommand
            // 
            this.listViewSendCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewSendCommand.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDateTime,
            this.columnserver,
            this.columnContent,
            this.columndevice,
            this.columniopara,
            this.columnvalue,
            this.columnresult,
            this.columnuser});
            this.listViewSendCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewSendCommand.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewSendCommand.FullRowSelect = true;
            this.listViewSendCommand.GridLines = true;
            this.listViewSendCommand.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSendCommand.HideSelection = false;
            this.listViewSendCommand.Location = new System.Drawing.Point(0, 31);
            this.listViewSendCommand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listViewSendCommand.Name = "listViewSendCommand";
            this.listViewSendCommand.Size = new System.Drawing.Size(1180, 140);
            this.listViewSendCommand.TabIndex = 11;
            this.listViewSendCommand.UseCompatibleStateImageBehavior = false;
            this.listViewSendCommand.View = System.Windows.Forms.View.Details;
            // 
            // columnDateTime
            // 
            this.columnDateTime.Text = "下置时间";
            this.columnDateTime.Width = 137;
            // 
            // columnserver
            // 
            this.columnserver.Text = "采集站";
            this.columnserver.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnserver.Width = 120;
            // 
            // columnContent
            // 
            this.columnContent.Text = "通道";
            this.columnContent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnContent.Width = 100;
            // 
            // columndevice
            // 
            this.columndevice.Text = "设备";
            this.columndevice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columndevice.Width = 99;
            // 
            // columniopara
            // 
            this.columniopara.Text = "IO参数";
            this.columniopara.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columniopara.Width = 102;
            // 
            // columnvalue
            // 
            this.columnvalue.Text = "下置值";
            this.columnvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnvalue.Width = 105;
            // 
            // columnresult
            // 
            this.columnresult.Text = "下置结果";
            this.columnresult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnresult.Width = 118;
            // 
            // columnuser
            // 
            this.columnuser.Text = "操作用户";
            // 
            // ucSplitLine_H2
            // 
            this.ucSplitLine_H2.BackColor = System.Drawing.Color.Gray;
            this.ucSplitLine_H2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSplitLine_H2.Location = new System.Drawing.Point(0, 30);
            this.ucSplitLine_H2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucSplitLine_H2.Name = "ucSplitLine_H2";
            this.ucSplitLine_H2.Size = new System.Drawing.Size(1180, 1);
            this.ucSplitLine_H2.TabIndex = 10;
            this.ucSplitLine_H2.TabStop = false;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1180, 30);
            this.label5.TabIndex = 9;
            this.label5.Text = "SCADA下置命令发送日志:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.ucEnableAlarm);
            this.tabPage5.Controls.Add(this.listViewAlarm);
            this.tabPage5.Controls.Add(this.ucSplitLine_H5);
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Location = new System.Drawing.Point(4, 34);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1186, 533);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "报警监视";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // ucEnableAlarm
            // 
            this.ucEnableAlarm.BackColor = System.Drawing.Color.Transparent;
            this.ucEnableAlarm.Checked = true;
            this.ucEnableAlarm.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucEnableAlarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucEnableAlarm.Location = new System.Drawing.Point(78, -1);
            this.ucEnableAlarm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucEnableAlarm.Name = "ucEnableAlarm";
            this.ucEnableAlarm.Padding = new System.Windows.Forms.Padding(1);
            this.ucEnableAlarm.Size = new System.Drawing.Size(105, 27);
            this.ucEnableAlarm.TabIndex = 34;
            this.ucEnableAlarm.TextValue = "实时显示";
            // 
            // listViewAlarm
            // 
            this.listViewAlarm.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewAlarm.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader23,
            this.columnHeader14,
            this.columnHeader17,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader22});
            this.listViewAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewAlarm.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewAlarm.FullRowSelect = true;
            this.listViewAlarm.GridLines = true;
            this.listViewAlarm.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewAlarm.HideSelection = false;
            this.listViewAlarm.Location = new System.Drawing.Point(0, 31);
            this.listViewAlarm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listViewAlarm.Name = "listViewAlarm";
            this.listViewAlarm.Size = new System.Drawing.Size(1186, 502);
            this.listViewAlarm.TabIndex = 12;
            this.listViewAlarm.UseCompatibleStateImageBehavior = false;
            this.listViewAlarm.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "数据IP";
            this.columnHeader23.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "报警时间";
            this.columnHeader14.Width = 137;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "IO参数";
            this.columnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader17.Width = 102;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "报警值";
            this.columnHeader20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader20.Width = 102;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "报警类型";
            this.columnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader21.Width = 102;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "报警等级";
            this.columnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader18.Width = 102;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "采集站";
            this.columnHeader19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader19.Width = 118;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "通道";
            this.columnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader15.Width = 100;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "设备";
            this.columnHeader16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader16.Width = 99;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "入库结果";
            this.columnHeader22.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader22.Width = 102;
            // 
            // ucSplitLine_H5
            // 
            this.ucSplitLine_H5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ucSplitLine_H5.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSplitLine_H5.Location = new System.Drawing.Point(0, 30);
            this.ucSplitLine_H5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucSplitLine_H5.Name = "ucSplitLine_H5";
            this.ucSplitLine_H5.Size = new System.Drawing.Size(1186, 1);
            this.ucSplitLine_H5.TabIndex = 33;
            this.ucSplitLine_H5.TabStop = false;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1186, 30);
            this.label10.TabIndex = 28;
            this.label10.Text = "报警监视:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ucCheckBoxAi);
            this.tabPage1.Controls.Add(this.machineTrainListView);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1186, 533);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "AI预测";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ucCheckBoxAi
            // 
            this.ucCheckBoxAi.BackColor = System.Drawing.Color.Transparent;
            this.ucCheckBoxAi.Checked = true;
            this.ucCheckBoxAi.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucCheckBoxAi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucCheckBoxAi.Location = new System.Drawing.Point(85, -1);
            this.ucCheckBoxAi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucCheckBoxAi.Name = "ucCheckBoxAi";
            this.ucCheckBoxAi.Padding = new System.Windows.Forms.Padding(1);
            this.ucCheckBoxAi.Size = new System.Drawing.Size(105, 27);
            this.ucCheckBoxAi.TabIndex = 35;
            this.ucCheckBoxAi.TextValue = "实时显示";
            // 
            // machineTrainListView
            // 
            this.machineTrainListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.machineTrainListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26,
            this.columnHeader27,
            this.columnHeader28,
            this.columnHeader29,
            this.columnHeader30});
            this.machineTrainListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machineTrainListView.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.machineTrainListView.FullRowSelect = true;
            this.machineTrainListView.GridLines = true;
            this.machineTrainListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.machineTrainListView.HideSelection = false;
            this.machineTrainListView.Location = new System.Drawing.Point(0, 30);
            this.machineTrainListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.machineTrainListView.Name = "machineTrainListView";
            this.machineTrainListView.Size = new System.Drawing.Size(1186, 503);
            this.machineTrainListView.TabIndex = 29;
            this.machineTrainListView.UseCompatibleStateImageBehavior = false;
            this.machineTrainListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "采集站";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "预测时间";
            this.columnHeader8.Width = 137;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "预测工况";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader9.Width = 102;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "输入值";
            this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader10.Width = 102;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "输入参数";
            this.columnHeader24.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader24.Width = 102;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "预测分数";
            this.columnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader25.Width = 102;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "通道";
            this.columnHeader26.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader26.Width = 118;
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "设备";
            this.columnHeader27.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader27.Width = 100;
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "训练任务";
            this.columnHeader28.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader28.Width = 99;
            // 
            // columnHeader29
            // 
            this.columnHeader29.Text = "算法";
            this.columnHeader29.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader29.Width = 102;
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "算法分类";
            this.columnHeader30.Width = 118;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1186, 30);
            this.label1.TabIndex = 30;
            this.label1.Text = "AI预测:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewBatchCommand);
            this.tabPage2.Controls.Add(this.ucCheckBoxBatchCommand);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1186, 533);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "自动控制任务";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ucCheckBoxBatchCommand
            // 
            this.ucCheckBoxBatchCommand.BackColor = System.Drawing.Color.Transparent;
            this.ucCheckBoxBatchCommand.Checked = true;
            this.ucCheckBoxBatchCommand.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucCheckBoxBatchCommand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ucCheckBoxBatchCommand.Location = new System.Drawing.Point(61, -1);
            this.ucCheckBoxBatchCommand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucCheckBoxBatchCommand.Name = "ucCheckBoxBatchCommand";
            this.ucCheckBoxBatchCommand.Padding = new System.Windows.Forms.Padding(1);
            this.ucCheckBoxBatchCommand.Size = new System.Drawing.Size(105, 27);
            this.ucCheckBoxBatchCommand.TabIndex = 36;
            this.ucCheckBoxBatchCommand.TextValue = "实时显示";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1186, 30);
            this.label2.TabIndex = 32;
            this.label2.Text = "控制任务:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "network%20harddrive.ico");
            this.imageList1.Images.SetKeyName(1, "comm2.png");
            this.imageList1.Images.SetKeyName(2, "comm1.png");
            this.imageList1.Images.SetKeyName(3, "RAID.ico");
            this.imageList1.Images.SetKeyName(4, "wifi2.png");
            this.imageList1.Images.SetKeyName(5, "wifi.png");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel1,
            this.ToolStripStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 571);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1194, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(20, 17);
            this.toolStripStatusLabel2.Text = "   ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel1.Image")));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(108, 17);
            this.toolStripStatusLabel1.Text = "服务运行状态：";
            // 
            // ToolStripStatus
            // 
            this.ToolStripStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.ToolStripStatus.Name = "ToolStripStatus";
            this.ToolStripStatus.Size = new System.Drawing.Size(86, 17);
            this.ToolStripStatus.Text = "服务运行中......";
            // 
            // listViewBatchCommand
            // 
            this.listViewBatchCommand.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewBatchCommand.AllowDrop = true;
            this.listViewBatchCommand.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewBatchCommand.CheckBoxes = true;
            this.listViewBatchCommand.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader33,
            this.columnHeader32,
            this.columnHeader34,
            this.columnHeader35,
            this.columnHeader36,
            this.columnHeader37});
            this.listViewBatchCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBatchCommand.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewBatchCommand.ForeColor = System.Drawing.SystemColors.MenuText;
            this.listViewBatchCommand.FullRowSelect = true;
            this.listViewBatchCommand.GridLines = true;
            this.listViewBatchCommand.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBatchCommand.HideSelection = false;
            this.listViewBatchCommand.Location = new System.Drawing.Point(0, 30);
            this.listViewBatchCommand.MultiSelect = false;
            this.listViewBatchCommand.Name = "listViewBatchCommand";
            this.listViewBatchCommand.ShowItemToolTips = true;
            this.listViewBatchCommand.Size = new System.Drawing.Size(1186, 503);
            this.listViewBatchCommand.TabIndex = 37;
            this.listViewBatchCommand.UseCompatibleStateImageBehavior = false;
            this.listViewBatchCommand.View = System.Windows.Forms.View.Details;
            this.listViewBatchCommand.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListViewBatchCommand_RetrieveVirtualItem);
            // 
            // columnHeader32
            // 
            this.columnHeader32.Text = "任务名称";
            this.columnHeader32.Width = 139;
            // 
            // columnHeader33
            // 
            this.columnHeader33.Text = "采集站";
            this.columnHeader33.Width = 122;
            // 
            // columnHeader34
            // 
            this.columnHeader34.Text = "执行时间";
            this.columnHeader34.Width = 144;
            // 
            // columnHeader35
            // 
            this.columnHeader35.Text = "执行结果";
            this.columnHeader35.Width = 133;
            // 
            // columnHeader36
            // 
            this.columnHeader36.Text = "任务类型";
            this.columnHeader36.Width = 139;
            // 
            // columnHeader37
            // 
            this.columnHeader37.Text = "执行备注";
            this.columnHeader37.Width = 159;
            // 
            // MonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 593);
            this.Controls.Add(this.tabControlMonitor);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MonitorForm";
            this.Text = "采集监视器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.tabControlMonitor.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label label5;
        private Scada.Controls.Controls.UCSplitLine_H ucSplitLine_H3;
        private Scada.Controls.Controls.UCSplitLine_H ucSplitLine_H2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnDateTime;
        private System.Windows.Forms.ColumnHeader columnContent;
        private Scada.Controls.Controls.TabControlExt tabControlMonitor;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columndevice;
        private System.Windows.Forms.ColumnHeader columniopara;
        private System.Windows.Forms.ColumnHeader columnvalue;
        private System.Windows.Forms.ColumnHeader columnresult;
        public SCADAListView listViewReceive;
        public SCADAListView listViewReport;
        public SCADAListView listViewSendCommand;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TabPage tabPage5;
        public SCADAListView listViewAlarm;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.ColumnHeader columnserver;
        private System.Windows.Forms.ColumnHeader columnuser;
       // private Scada.Controls.Controls.ComputerInfoControl computerInfoControl;
        private SCADAPageCombox  cbSendCommandSize;
        private Scada.Controls.Controls.UCCheckBox ucbSendCommand;
        private System.Windows.Forms.Label label7;
        private Scada.Controls.Controls.UCSplitLine_H ucSplitLine_H4;
        private Scada.Controls.Controls.UCSplitLine_H ucSplitLine_H5;
        private System.Windows.Forms.Label label10;
        private UCCheckBox ucReceive;
        private UCCheckBox ucLog;
        private UCCheckBox ucEnableAlarm;

        /// <summary>
        /// 定义事件报告列
        /// </summary>
        private System.Windows.Forms.ColumnHeader columnEventServerHeader;
        private System.Windows.Forms.ColumnHeader columnEventNameHeader;
        private System.Windows.Forms.ColumnHeader columnEventCommunicationHeader;
        private System.Windows.Forms.ColumnHeader columnEventDeviceHeader;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        public System.Windows.Forms.ToolStripStatusLabel ToolStripStatus;
        private System.Windows.Forms.TabPage tabPage1;
        private UCCheckBox ucCheckBoxAi;
        public SCADAListView machineTrainListView;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader26;
        private System.Windows.Forms.ColumnHeader columnHeader27;
        private System.Windows.Forms.ColumnHeader columnHeader28;
        private System.Windows.Forms.ColumnHeader columnHeader29;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader columnHeader30;
        private System.Windows.Forms.TabPage tabPage2;
        private UCCheckBox ucCheckBoxBatchCommand;
        private System.Windows.Forms.Label label2;
        private SCADAListView listViewBatchCommand;
        private System.Windows.Forms.ColumnHeader columnHeader32;
        private System.Windows.Forms.ColumnHeader columnHeader33;
        private System.Windows.Forms.ColumnHeader columnHeader34;
        private System.Windows.Forms.ColumnHeader columnHeader35;
        private System.Windows.Forms.ColumnHeader columnHeader36;
        private System.Windows.Forms.ColumnHeader columnHeader37;
    }
}