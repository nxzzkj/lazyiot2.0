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
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls.Forms
{
    /// <summary>
    /// Class FrmWithTitle.
    /// Implements the <see cref="Scada.Controls.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Forms.FrmBase" />
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class FrmWithTitle : FrmBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("窗体标题"), Category("自定义")]
        public string Title
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                lblTitle.Text = value;
            }
        }
        /// <summary>
        /// The is show close BTN
        /// </summary>
        private bool _isShowCloseBtn = false;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is show close BTN.
        /// </summary>
        /// <value><c>true</c> if this instance is show close BTN; otherwise, <c>false</c>.</value>
        [Description("是否显示右上角关闭按钮"), Category("自定义")]
        public bool IsShowCloseBtn
        {
            get
            {
                return _isShowCloseBtn;
            }
            set
            {
                _isShowCloseBtn = value;
                btnClose.Visible = value;
                if (value)
                {
                    btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
                    btnClose.BringToFront();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmWithTitle" /> class.
        /// </summary>
        public FrmWithTitle()
        {
            InitializeComponent();
            InitFormMove(this.lblTitle);
            this.Load += FrmWithTitle_Load;
            btnClose.Location = new Point(this.Width - btnClose.Width, btnClose.Location.Y);
            btMin.Location = new Point(this.Width - btnClose.Width - btMin.Width - 4, btnClose.Location.Y);
            btMax.Location = new Point(this.Width - btnClose.Width - btMin.Width - btMax.Width - 6, btnClose.Location.Y);
        }

        private void FrmWithTitle_Load(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
        }


        /// <summary>
        /// Handles the Shown event of the FrmWithTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmWithTitle_Shown(object sender, EventArgs e)
        {
            if (IsShowCloseBtn)
            {
                btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
                btnClose.BringToFront();
            }
        }


        /// <summary>
        /// Handles the VisibleChanged event of the FrmWithTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmWithTitle_VisibleChanged(object sender, EventArgs e)
        {
        }

        public virtual    void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmWithTitle_SizeChanged(object sender, EventArgs e)
        {
            btnClose.Location = new Point(this.Width - btnClose.Width, btnClose.Location.Y);
          
            btMin.Location = new Point(this.Width - btnClose.Width- btMin.Width-4, btnClose.Location.Y);
            btMax.Location = new Point(this.Width - btnClose.Width - btMin.Width - btMax.Width-6, btnClose.Location.Y);
        }

        private void btMax_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
          
        }

        private void lblTitle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
    }
}
