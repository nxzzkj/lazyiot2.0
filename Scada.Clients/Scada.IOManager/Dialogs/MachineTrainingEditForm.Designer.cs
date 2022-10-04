namespace IOManager.Dialogs
{
    partial class MachineTrainingEditForm
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
            this.tbTaskName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btClose = new System.Windows.Forms.Button();
            this.tbTaskRemark = new System.Windows.Forms.TextBox();
            this.cbTaskAlgorithm = new System.Windows.Forms.ComboBox();
            this.nudTaskTrainingCycle = new System.Windows.Forms.NumericUpDown();
            this.nudTaskForecastPriod = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelDesc = new System.Windows.Forms.Label();
            this.cbTaskProperties = new IOManager.Controls.ComboBoxCheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbCommunication = new System.Windows.Forms.ComboBox();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxClass = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbTrue = new System.Windows.Forms.TextBox();
            this.tbFalse = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbDetection5 = new System.Windows.Forms.TextBox();
            this.tbDetection6 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tbDetection7 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbDetection8 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tbDetection9 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbDetection10 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTaskTrainingCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTaskForecastPriod)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBoxClass.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务名称:";
            // 
            // tbTaskName
            // 
            this.tbTaskName.Location = new System.Drawing.Point(91, 22);
            this.tbTaskName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTaskName.Name = "tbTaskName";
            this.tbTaskName.Size = new System.Drawing.Size(413, 23);
            this.tbTaskName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "算法:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "训练周期:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "预测周期:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "属  性:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 318);
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
            this.panel1.Location = new System.Drawing.Point(0, 451);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(929, 33);
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
            // tbTaskRemark
            // 
            this.tbTaskRemark.Location = new System.Drawing.Point(91, 266);
            this.tbTaskRemark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTaskRemark.Multiline = true;
            this.tbTaskRemark.Name = "tbTaskRemark";
            this.tbTaskRemark.Size = new System.Drawing.Size(430, 127);
            this.tbTaskRemark.TabIndex = 11;
            // 
            // cbTaskAlgorithm
            // 
            this.cbTaskAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTaskAlgorithm.FormattingEnabled = true;
            this.cbTaskAlgorithm.Location = new System.Drawing.Point(91, 87);
            this.cbTaskAlgorithm.Name = "cbTaskAlgorithm";
            this.cbTaskAlgorithm.Size = new System.Drawing.Size(413, 25);
            this.cbTaskAlgorithm.TabIndex = 18;
            this.cbTaskAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cbTaskAlgorithm_SelectedIndexChanged);
            // 
            // nudTaskTrainingCycle
            // 
            this.nudTaskTrainingCycle.Location = new System.Drawing.Point(92, 123);
            this.nudTaskTrainingCycle.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudTaskTrainingCycle.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTaskTrainingCycle.Name = "nudTaskTrainingCycle";
            this.nudTaskTrainingCycle.Size = new System.Drawing.Size(315, 23);
            this.nudTaskTrainingCycle.TabIndex = 20;
            this.nudTaskTrainingCycle.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nudTaskForecastPriod
            // 
            this.nudTaskForecastPriod.Location = new System.Drawing.Point(92, 152);
            this.nudTaskForecastPriod.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nudTaskForecastPriod.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTaskForecastPriod.Name = "nudTaskForecastPriod";
            this.nudTaskForecastPriod.Size = new System.Drawing.Size(315, 23);
            this.nudTaskForecastPriod.TabIndex = 21;
            this.nudTaskForecastPriod.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(413, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 22;
            this.label7.Text = "(次/分钟)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(413, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 17);
            this.label8.TabIndex = 23;
            this.label8.Text = "(次/分钟)";
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Location = new System.Drawing.Point(89, 49);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(92, 17);
            this.labelDesc.TabIndex = 24;
            this.labelDesc.Text = "请选择算法模型";
            // 
            // cbTaskProperties
            // 
            this.cbTaskProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTaskProperties.FormattingEnabled = true;
            this.cbTaskProperties.Location = new System.Drawing.Point(49, 47);
            this.cbTaskProperties.Name = "cbTaskProperties";
            this.cbTaskProperties.Size = new System.Drawing.Size(375, 25);
            this.cbTaskProperties.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(266, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 17);
            this.label9.TabIndex = 27;
            this.label9.Text = "设备:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 17);
            this.label10.TabIndex = 28;
            this.label10.Text = "通道:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 17);
            this.label11.TabIndex = 29;
            this.label11.Text = "IO点:";
            // 
            // cbCommunication
            // 
            this.cbCommunication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommunication.FormattingEnabled = true;
            this.cbCommunication.Location = new System.Drawing.Point(49, 16);
            this.cbCommunication.Name = "cbCommunication";
            this.cbCommunication.Size = new System.Drawing.Size(209, 25);
            this.cbCommunication.TabIndex = 30;
            this.cbCommunication.SelectedIndexChanged += new System.EventHandler(this.cbCommunication_SelectedIndexChanged);
            // 
            // cbDevice
            // 
            this.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Location = new System.Drawing.Point(297, 16);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(127, 25);
            this.cbDevice.TabIndex = 31;
            this.cbDevice.SelectedIndexChanged += new System.EventHandler(this.cbDevice_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbCommunication);
            this.groupBox1.Controls.Add(this.cbDevice);
            this.groupBox1.Controls.Add(this.cbTaskProperties);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(91, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 78);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // groupBoxClass
            // 
            this.groupBoxClass.Controls.Add(this.label12);
            this.groupBoxClass.Controls.Add(this.label13);
            this.groupBoxClass.Controls.Add(this.tbTrue);
            this.groupBoxClass.Controls.Add(this.tbFalse);
            this.groupBoxClass.Location = new System.Drawing.Point(555, 22);
            this.groupBoxClass.Name = "groupBoxClass";
            this.groupBoxClass.Size = new System.Drawing.Size(363, 90);
            this.groupBoxClass.TabIndex = 33;
            this.groupBoxClass.TabStop = false;
            this.groupBoxClass.Text = "二元分类设置";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 17);
            this.label12.TabIndex = 28;
            this.label12.Text = "True(是)显示文本:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 17);
            this.label13.TabIndex = 31;
            this.label13.Text = "False(否)显示文本:";
            // 
            // tbTrue
            // 
            this.tbTrue.Location = new System.Drawing.Point(117, 28);
            this.tbTrue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTrue.Name = "tbTrue";
            this.tbTrue.Size = new System.Drawing.Size(180, 23);
            this.tbTrue.TabIndex = 29;
            this.tbTrue.Text = "是";
            // 
            // tbFalse
            // 
            this.tbFalse.Location = new System.Drawing.Point(119, 59);
            this.tbFalse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbFalse.Name = "tbFalse";
            this.tbFalse.Size = new System.Drawing.Size(180, 23);
            this.tbFalse.TabIndex = 30;
            this.tbFalse.Text = "否";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.tbDetection10);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.tbDetection9);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.tbDetection8);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.tbDetection7);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.tbDetection5);
            this.groupBox2.Controls.Add(this.tbDetection6);
            this.groupBox2.Location = new System.Drawing.Point(555, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 215);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "异常检测分级指标";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 17);
            this.label14.TabIndex = 28;
            this.label14.Text = "异常值<0.5";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 62);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 17);
            this.label15.TabIndex = 31;
            this.label15.Text = "异常值0.5~0.6";
            // 
            // tbDetection5
            // 
            this.tbDetection5.Location = new System.Drawing.Point(117, 28);
            this.tbDetection5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDetection5.Name = "tbDetection5";
            this.tbDetection5.Size = new System.Drawing.Size(180, 23);
            this.tbDetection5.TabIndex = 29;
            this.tbDetection5.Text = "正常";
            // 
            // tbDetection6
            // 
            this.tbDetection6.Location = new System.Drawing.Point(119, 59);
            this.tbDetection6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDetection6.Name = "tbDetection6";
            this.tbDetection6.Size = new System.Drawing.Size(180, 23);
            this.tbDetection6.TabIndex = 30;
            this.tbDetection6.Text = "疑似异常";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 93);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(87, 17);
            this.label16.TabIndex = 33;
            this.label16.Text = "异常值0.6~0.7";
            // 
            // tbDetection7
            // 
            this.tbDetection7.Location = new System.Drawing.Point(119, 90);
            this.tbDetection7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDetection7.Name = "tbDetection7";
            this.tbDetection7.Size = new System.Drawing.Size(180, 23);
            this.tbDetection7.TabIndex = 32;
            this.tbDetection7.Text = "轻度异常";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(5, 124);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(87, 17);
            this.label17.TabIndex = 35;
            this.label17.Text = "异常值0.7~0.8";
            // 
            // tbDetection8
            // 
            this.tbDetection8.Location = new System.Drawing.Point(119, 121);
            this.tbDetection8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDetection8.Name = "tbDetection8";
            this.tbDetection8.Size = new System.Drawing.Size(180, 23);
            this.tbDetection8.TabIndex = 34;
            this.tbDetection8.Text = "中度异常";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(5, 155);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(87, 17);
            this.label18.TabIndex = 37;
            this.label18.Text = "异常值0.8~0.9";
            // 
            // tbDetection9
            // 
            this.tbDetection9.Location = new System.Drawing.Point(119, 152);
            this.tbDetection9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDetection9.Name = "tbDetection9";
            this.tbDetection9.Size = new System.Drawing.Size(180, 23);
            this.tbDetection9.TabIndex = 36;
            this.tbDetection9.Text = "重度异常";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(5, 186);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 17);
            this.label19.TabIndex = 39;
            this.label19.Text = "异常值0.9~1";
            // 
            // tbDetection10
            // 
            this.tbDetection10.Location = new System.Drawing.Point(119, 183);
            this.tbDetection10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbDetection10.Name = "tbDetection10";
            this.tbDetection10.Size = new System.Drawing.Size(180, 23);
            this.tbDetection10.TabIndex = 38;
            this.tbDetection10.Text = "严重异常";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(552, 376);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(308, 34);
            this.label20.TabIndex = 36;
            this.label20.Text = "该值计算结果在0-1范围内，小于0.5表示数据是正常的。\r\n大于0.5以上表示数据出现异常。";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(560, 117);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(164, 17);
            this.label21.TabIndex = 37;
            this.label21.Text = "二元分类只分类是否两个类别";
            // 
            // MachineTrainingEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 484);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxClass);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelDesc);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudTaskForecastPriod);
            this.Controls.Add(this.nudTaskTrainingCycle);
            this.Controls.Add(this.cbTaskAlgorithm);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbTaskRemark);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbTaskName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MachineTrainingEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "机器学习任务编辑";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTaskTrainingCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTaskForecastPriod)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxClass.ResumeLayout(false);
            this.groupBoxClass.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTaskName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.TextBox tbTaskRemark;
        private System.Windows.Forms.ComboBox cbTaskAlgorithm;
        private System.Windows.Forms.NumericUpDown nudTaskTrainingCycle;
        private System.Windows.Forms.NumericUpDown nudTaskForecastPriod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelDesc;
        private Controls.ComboBoxCheckBox cbTaskProperties;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbCommunication;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxClass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbTrue;
        private System.Windows.Forms.TextBox tbFalse;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbDetection5;
        private System.Windows.Forms.TextBox tbDetection6;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tbDetection9;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbDetection8;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbDetection7;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbDetection10;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
    }
}