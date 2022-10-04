

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCDataGridView.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    partial class UCDataGridView
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panHead = new System.Windows.Forms.Panel();
            this.panColumns = new System.Windows.Forms.TableLayoutPanel();
            this.panHeadLeft = new System.Windows.Forms.Panel();
            this.ucSplitLine_H1 = new Scada.Controls.Controls.UCSplitLine_H();
            this.panRow = new System.Windows.Forms.Panel();
            this.panHead.SuspendLayout();
            this.SuspendLayout();
            // 
            // panHead
            // 
            this.panHead.Controls.Add(this.panColumns);
            this.panHead.Controls.Add(this.panHeadLeft);
            this.panHead.Controls.Add(this.ucSplitLine_H1);
            this.panHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHead.Location = new System.Drawing.Point(0, 0);
            this.panHead.Name = "panHead";
            this.panHead.Size = new System.Drawing.Size(1061, 40);
            this.panHead.TabIndex = 0;
            // 
            // panColumns
            // 
            this.panColumns.ColumnCount = 1;
            this.panColumns.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panColumns.Location = new System.Drawing.Point(0, 0);
            this.panColumns.Name = "panColumns";
            this.panColumns.RowCount = 1;
            this.panColumns.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panColumns.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panColumns.Size = new System.Drawing.Size(1061, 39);
            this.panColumns.TabIndex = 1;
            // 
            // panHeadLeft
            // 
            this.panHeadLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panHeadLeft.Location = new System.Drawing.Point(0, 0);
            this.panHeadLeft.Name = "panHeadLeft";
            this.panHeadLeft.Size = new System.Drawing.Size(0, 39);
            this.panHeadLeft.TabIndex = 2;
            // 
            // ucSplitLine_H1
            // 
            this.ucSplitLine_H1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ucSplitLine_H1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucSplitLine_H1.Location = new System.Drawing.Point(0, 39);
            this.ucSplitLine_H1.Name = "ucSplitLine_H1";
            this.ucSplitLine_H1.Size = new System.Drawing.Size(1061, 1);
            this.ucSplitLine_H1.TabIndex = 0;
            this.ucSplitLine_H1.TabStop = false;
            // 
            // panRow
            // 
            this.panRow.AutoScroll = true;
            this.panRow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panRow.Location = new System.Drawing.Point(0, 40);
            this.panRow.Name = "panRow";
            this.panRow.Size = new System.Drawing.Size(1061, 525);
            this.panRow.TabIndex = 1;
            // 
            // UCDataGridView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panRow);
            this.Controls.Add(this.panHead);
            this.Name = "UCDataGridView";
            this.Size = new System.Drawing.Size(1061, 565);
            this.panHead.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The pan head
        /// </summary>
        private System.Windows.Forms.Panel panHead;
        /// <summary>
        /// The pan columns
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel panColumns;
        /// <summary>
        /// The uc split line h1
        /// </summary>
        private UCSplitLine_H ucSplitLine_H1;
        /// <summary>
        /// The pan row
        /// </summary>
        private System.Windows.Forms.Panel panRow;
        /// <summary>
        /// The pan head left
        /// </summary>
        private System.Windows.Forms.Panel panHeadLeft;

    }
}
