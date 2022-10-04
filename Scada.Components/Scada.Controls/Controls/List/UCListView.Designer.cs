﻿

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
    /// Class UCListView.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    partial class UCListView
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
            this.panMain = new System.Windows.Forms.FlowLayoutPanel();
            this.panPage = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panMain
            // 
            this.panMain.AutoScroll = true;
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 0);
            this.panMain.Margin = new System.Windows.Forms.Padding(0);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(5);
            this.panMain.Size = new System.Drawing.Size(462, 319);
            this.panMain.TabIndex = 1;
            this.panMain.Resize += new System.EventHandler(this.panMain_Resize);
            // 
            // panPage
            // 
            this.panPage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panPage.Location = new System.Drawing.Point(0, 319);
            this.panPage.Name = "panPage";
            this.panPage.Size = new System.Drawing.Size(462, 44);
            this.panPage.TabIndex = 0;
            this.panPage.Visible = false;
            // 
            // UCListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panPage);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCListView";
            this.Size = new System.Drawing.Size(462, 363);
            this.Load += new System.EventHandler(this.UCListView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The pan main
        /// </summary>
        private System.Windows.Forms.FlowLayoutPanel panMain;
        /// <summary>
        /// The pan page
        /// </summary>
        private System.Windows.Forms.Panel panPage;
    }
}
