namespace ScadaCenterServer.Controls
{
    partial class HistoryMachineTrainSearch
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.ucBtnSearch = new Scada.Controls.Controls.UCBtnExt();
            this.hsComboBoxService = new ScadaCenterServer.Controls.HsComboBox(this.components);
            this.comboBoxMachineTrain = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 184F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 252F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.dateStart, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.dateEnd, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.ucBtnSearch, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.hsComboBoxService, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxMachineTrain, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(958, 30);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // dateStart
            // 
            this.dateStart.CalendarFont = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateStart.CustomFormat = "yyyy-MM-dd";
            this.dateStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateStart.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateStart.Location = new System.Drawing.Point(439, 3);
            this.dateStart.Name = "dateStart";
            this.dateStart.ShowUpDown = true;
            this.dateStart.Size = new System.Drawing.Size(124, 23);
            this.dateStart.TabIndex = 24;
            // 
            // dateEnd
            // 
            this.dateEnd.CalendarFont = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateEnd.CustomFormat = "yyyy-MM-dd";
            this.dateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateEnd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateEnd.Location = new System.Drawing.Point(569, 3);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.ShowUpDown = true;
            this.dateEnd.Size = new System.Drawing.Size(124, 23);
            this.dateEnd.TabIndex = 26;
            // 
            // ucBtnSearch
            // 
            this.ucBtnSearch.BackColor = System.Drawing.Color.Transparent;
            this.ucBtnSearch.BtnBackColor = System.Drawing.Color.Transparent;
            this.ucBtnSearch.BtnFont = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBtnSearch.BtnForeColor = System.Drawing.Color.White;
            this.ucBtnSearch.BtnText = "查询";
            this.ucBtnSearch.ConerRadius = 5;
            this.ucBtnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucBtnSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.ucBtnSearch.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnSearch.ForeColor = System.Drawing.Color.White;
            this.ucBtnSearch.IsRadius = true;
            this.ucBtnSearch.IsShowRect = false;
            this.ucBtnSearch.IsShowTips = false;
            this.ucBtnSearch.Location = new System.Drawing.Point(696, 0);
            this.ucBtnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.ucBtnSearch.Name = "ucBtnSearch";
            this.ucBtnSearch.RectColor = System.Drawing.Color.Gainsboro;
            this.ucBtnSearch.RectWidth = 1;
            this.ucBtnSearch.Size = new System.Drawing.Size(47, 30);
            this.ucBtnSearch.TabIndex = 27;
            this.ucBtnSearch.TabStop = false;
            this.ucBtnSearch.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnSearch.TipsText = "";
            this.ucBtnSearch.BtnClick += new System.EventHandler(this.ucBtnExt13_BtnClick);
            // 
            // hsComboBoxService
            // 
            this.hsComboBoxService.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.hsComboBoxService.CheckedListBox = null;
            this.hsComboBoxService.CtlType = ScadaCenterServer.Controls.HsComboBox.TypeC.TreeView;
            this.hsComboBoxService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsComboBoxService.DropDownHeight = 1;
            this.hsComboBoxService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hsComboBoxService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hsComboBoxService.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hsComboBoxService.ForeColor = System.Drawing.SystemColors.WindowText;
            this.hsComboBoxService.FormattingEnabled = true;
            this.hsComboBoxService.IntegralHeight = false;
            this.hsComboBoxService.ItemHeight = 19;
            this.hsComboBoxService.Location = new System.Drawing.Point(3, 3);
            this.hsComboBoxService.Name = "hsComboBoxService";
            this.hsComboBoxService.Size = new System.Drawing.Size(178, 27);
            this.hsComboBoxService.TabIndex = 20;
            this.hsComboBoxService.SelectedIndexChanged += new System.EventHandler(this.hsComboBoxService_SelectedIndexChanged);
            // 
            // comboBoxMachineTrain
            // 
            this.comboBoxMachineTrain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMachineTrain.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxMachineTrain.FormattingEnabled = true;
            this.comboBoxMachineTrain.Location = new System.Drawing.Point(187, 3);
            this.comboBoxMachineTrain.Name = "comboBoxMachineTrain";
            this.comboBoxMachineTrain.Size = new System.Drawing.Size(246, 28);
            this.comboBoxMachineTrain.TabIndex = 28;
            this.comboBoxMachineTrain.SelectedIndexChanged += new System.EventHandler(this.comboBoxMachineTrain_SelectedIndexChanged);
            // 
            // HistoryMachineTrainSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "HistoryMachineTrainSearch";
            this.Size = new System.Drawing.Size(1238, 30);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private Scada.Controls.Controls.UCBtnExt ucBtnSearch;
        private HsComboBox hsComboBoxService;
        private System.Windows.Forms.ComboBox comboBoxMachineTrain;
    }
}
