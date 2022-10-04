

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
    /// Class UCTimePanel.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    partial class UCTimePanel
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
            this.panMain = new System.Windows.Forms.TableLayoutPanel();
            this.ucSplitLine_V1 = new Scada.Controls.Controls.UCSplitLine_V();
            this.ucSplitLine_V2 = new Scada.Controls.Controls.UCSplitLine_V();
            this.ucSplitLine_H1 = new Scada.Controls.Controls.UCSplitLine_H();
            this.ucSplitLine_H2 = new Scada.Controls.Controls.UCSplitLine_H();
            this.SuspendLayout();
            // 
            // panMain
            // 
            this.panMain.ColumnCount = 1;
            this.panMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(1, 1);
            this.panMain.Name = "panMain";
            this.panMain.RowCount = 1;
            this.panMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panMain.Size = new System.Drawing.Size(99, 228);
            this.panMain.TabIndex = 0;
            // 
            // ucSplitLine_V1
            // 
            this.ucSplitLine_V1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucSplitLine_V1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucSplitLine_V1.Location = new System.Drawing.Point(0, 0);
            this.ucSplitLine_V1.Name = "ucSplitLine_V1";
            this.ucSplitLine_V1.Size = new System.Drawing.Size(1, 230);
            this.ucSplitLine_V1.TabIndex = 0;
            this.ucSplitLine_V1.TabStop = false;
            this.ucSplitLine_V1.Visible = false;
            // 
            // ucSplitLine_V2
            // 
            this.ucSplitLine_V2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucSplitLine_V2.Dock = System.Windows.Forms.DockStyle.Right;
            this.ucSplitLine_V2.Location = new System.Drawing.Point(100, 0);
            this.ucSplitLine_V2.Name = "ucSplitLine_V2";
            this.ucSplitLine_V2.Size = new System.Drawing.Size(1, 230);
            this.ucSplitLine_V2.TabIndex = 1;
            this.ucSplitLine_V2.TabStop = false;
            this.ucSplitLine_V2.Visible = false;
            // 
            // ucSplitLine_H1
            // 
            this.ucSplitLine_H1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucSplitLine_H1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSplitLine_H1.Location = new System.Drawing.Point(1, 0);
            this.ucSplitLine_H1.Name = "ucSplitLine_H1";
            this.ucSplitLine_H1.Size = new System.Drawing.Size(99, 1);
            this.ucSplitLine_H1.TabIndex = 0;
            this.ucSplitLine_H1.TabStop = false;
            this.ucSplitLine_H1.Visible = false;
            // 
            // ucSplitLine_H2
            // 
            this.ucSplitLine_H2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucSplitLine_H2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucSplitLine_H2.Location = new System.Drawing.Point(1, 229);
            this.ucSplitLine_H2.Name = "ucSplitLine_H2";
            this.ucSplitLine_H2.Size = new System.Drawing.Size(99, 1);
            this.ucSplitLine_H2.TabIndex = 2;
            this.ucSplitLine_H2.TabStop = false;
            this.ucSplitLine_H2.Visible = false;
            // 
            // UCTimePanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.ucSplitLine_H2);
            this.Controls.Add(this.ucSplitLine_H1);
            this.Controls.Add(this.ucSplitLine_V2);
            this.Controls.Add(this.ucSplitLine_V1);
            this.Name = "UCTimePanel";
            this.Size = new System.Drawing.Size(101, 230);
            this.Load += new System.EventHandler(this.UCTimePanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The pan main
        /// </summary>
        private System.Windows.Forms.TableLayoutPanel panMain;
        /// <summary>
        /// The uc split line v1
        /// </summary>
        private UCSplitLine_V ucSplitLine_V1;
        /// <summary>
        /// The uc split line v2
        /// </summary>
        private UCSplitLine_V ucSplitLine_V2;
        /// <summary>
        /// The uc split line h1
        /// </summary>
        private UCSplitLine_H ucSplitLine_H1;
        /// <summary>
        /// The uc split line h2
        /// </summary>
        private UCSplitLine_H ucSplitLine_H2;
    }
}
