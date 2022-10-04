

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
    /// Class UCHorizontalList.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    partial class UCHorizontalList
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
            this.panMain = new System.Windows.Forms.Panel();
            this.panList = new System.Windows.Forms.Panel();
            this.panRight = new System.Windows.Forms.Panel();
            this.panLeft = new System.Windows.Forms.Panel();
            this.panMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.panList);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(46, 0);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(422, 53);
            this.panMain.TabIndex = 3;
            // 
            // panList
            // 
            this.panList.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.panList.BackColor = System.Drawing.Color.Transparent;
            this.panList.Location = new System.Drawing.Point(0, 0);
            this.panList.Name = "panList";
            this.panList.Size = new System.Drawing.Size(401, 53);
            this.panList.TabIndex = 0;
            // 
            // panRight
            // 
            this.panRight.BackgroundImage = global::Scada.Controls.Properties.Resources.chevron_right;
            this.panRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panRight.Location = new System.Drawing.Point(468, 0);
            this.panRight.Name = "panRight";
            this.panRight.Size = new System.Drawing.Size(46, 53);
            this.panRight.TabIndex = 2;
            this.panRight.Visible = false;
            this.panRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panRight_MouseDown);
            // 
            // panLeft
            // 
            this.panLeft.BackgroundImage = global::Scada.Controls.Properties.Resources.chevron_left;
            this.panLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panLeft.Location = new System.Drawing.Point(0, 0);
            this.panLeft.Name = "panLeft";
            this.panLeft.Size = new System.Drawing.Size(46, 53);
            this.panLeft.TabIndex = 1;
            this.panLeft.Visible = false;
            this.panLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panLeft_MouseDown);
            // 
            // UCHorizontalList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panRight);
            this.Controls.Add(this.panLeft);
            this.Name = "UCHorizontalList";
            this.Size = new System.Drawing.Size(514, 53);
            this.panMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The pan left
        /// </summary>
        private System.Windows.Forms.Panel panLeft;
        /// <summary>
        /// The pan right
        /// </summary>
        private System.Windows.Forms.Panel panRight;
        /// <summary>
        /// The pan main
        /// </summary>
        private System.Windows.Forms.Panel panMain;
        /// <summary>
        /// The pan list
        /// </summary>
        private System.Windows.Forms.Panel panList;
    }
}
