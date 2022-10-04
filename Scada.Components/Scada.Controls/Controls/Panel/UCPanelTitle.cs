

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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCPanelTitle.
    /// Implements the <see cref="Scada.Controls.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCControlBase" />
    public partial class UCPanelTitle : UCControlBase, IContainerControl
    {
        /// <summary>
        /// The m int maximum height
        /// </summary>
        private int m_intMaxHeight = 0;
        /// <summary>
        /// The is can expand
        /// </summary>
        private bool isCanExpand = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is can expand.
        /// </summary>
        /// <value><c>true</c> if this instance is can expand; otherwise, <c>false</c>.</value>
        [Description("是否可折叠"), Category("自定义")]
        public bool IsCanExpand
        {
            get { return isCanExpand; }
            set
            {
                isCanExpand = value;
                if (value)
                {
                    lblTitle.Image = GetImg();
                }
                else
                {
                    lblTitle.Image = null;
                }
            }
        }
        /// <summary>
        /// The is expand
        /// </summary>
        private bool isExpand = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expand.
        /// </summary>
        /// <value><c>true</c> if this instance is expand; otherwise, <c>false</c>.</value>
        [Description("是否已折叠"), Category("自定义")]
        public bool IsExpand
        {
            get { return isExpand; }
            set
            {
                isExpand = value;
                if (value)
                {
                    m_intMaxHeight = this.Height;
                    this.Height = lblTitle.Height;
                }
                else
                {
                    this.Height = m_intMaxHeight;
                }

                if (isCanExpand)
                {
                    lblTitle.Image = GetImg();
                }
                else
                {
                    lblTitle.Image = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        [Description("边框颜色"), Category("自定义")]
        public Color BorderColor
        {
            get { return this.RectColor; }
            set
            {
                this.RectColor = value;
                this.lblTitle.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Description("面板标题"), Category("自定义")]
        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        /// <summary>
        /// 获取或设置控件的前景色。
        /// </summary>
        /// <value>The color of the fore.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override Color ForeColor
        {
            get
            {
                return this.lblTitle.ForeColor;
            }
            set
            {
                this.lblTitle.ForeColor = value;
                GetImg(true);
                if (isCanExpand)
                {
                    lblTitle.Image = GetImg();
                }
                else
                {
                    lblTitle.Image = null;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCPanelTitle" /> class.
        /// </summary>
        public UCPanelTitle()
        {
            InitializeComponent();
            this.SizeChanged += UCPanelTitle_SizeChanged;
            if (isCanExpand)
            {
                lblTitle.Image = GetImg();
            }
            else
            {
                lblTitle.Image = null;
            }
        }

        /// <summary>
        /// The bit down
        /// </summary>
        Bitmap bitDown = null;
        /// <summary>
        /// The bit up
        /// </summary>
        Bitmap bitUp = null;
        /// <summary>
        /// Gets the img.
        /// </summary>
        /// <param name="blnRefresh">if set to <c>true</c> [BLN refresh].</param>
        /// <returns>Bitmap.</returns>
        private Bitmap GetImg(bool blnRefresh = false)
        {
            if (isExpand)
            {
                if (bitDown == null || blnRefresh)
                {
                    bitDown = new Bitmap(24, 24);
                    Graphics g = Graphics.FromImage(bitDown);
                    g.SetGDIHigh();
                    GraphicsPath path = new GraphicsPath();
                    path.AddLine(3, 5, 21, 5);
                    path.AddLine(21, 5, 12, 19);
                    path.AddLine(12, 19, 3, 5);
                    g.FillPath(new SolidBrush(ForeColor), path);
                    g.Dispose();
                }
                return bitDown;
            }
            else
            {
                if (bitUp == null || blnRefresh)
                {
                    bitUp = new Bitmap(24, 24);
                    Graphics g = Graphics.FromImage(bitUp);
                    g.SetGDIHigh();
                    GraphicsPath path = new GraphicsPath();
                    path.AddLine(3, 19, 21, 19);
                    path.AddLine(21, 19, 12, 5);
                    path.AddLine(12, 5, 3, 19);
                    g.FillPath(new SolidBrush(ForeColor), path);
                    g.Dispose();
                }
                return bitUp;
            }

        }

        /// <summary>
        /// Handles the MouseDown event of the lblTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (isCanExpand)
            {
                IsExpand = !IsExpand;
            }
        }

        /// <summary>
        /// Handles the SizeChanged event of the UCPanelTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UCPanelTitle_SizeChanged(object sender, EventArgs e)
        {
            if (this.Height != lblTitle.Height)
            {
                m_intMaxHeight = this.Height;
            }
            lblTitle.Width = this.Width;
        }
    }
}
