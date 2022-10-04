

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

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCBtnExt.
    /// Implements the <see cref="Scada.Controls.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCControlBase" />
    [DefaultEvent("BtnClick")]

    public partial class UCBtnExt : UCControlBase
    {
        #region 字段属性
        /// <summary>
        /// 是否显示角标
        /// </summary>
        /// <value><c>true</c> if this instance is show tips; otherwise, <c>false</c>.</value>
        [Description("是否显示角标"), Category("自定义")]
        public bool IsShowTips
        {
            get
            {
                return this.lblTips.Visible;
            }
            set
            {
                this.lblTips.Visible = value;
            }
        }
        /// <summary>
        /// 角标文字
        /// </summary>
        /// <value>The tips text.</value>
        [Description("角标文字"), Category("自定义")]
        public string TipsText
        {
            get
            {
                return this.lblTips.Text;
            }
            set
            {
                this.lblTips.Text = value;
            }
        }

        /// <summary>
        /// The BTN back color
        /// </summary>
        private Color _btnBackColor = Color.White;
        /// <summary>
        /// 按钮背景色
        /// </summary>
        /// <value>The color of the BTN back.</value>
        [Description("按钮背景色"), Category("自定义")]
        public Color BtnBackColor
        {
            get { return _btnBackColor; }
            set
            {
                _btnBackColor = value;
                this.BackColor = value;
            }
        }

        /// <summary>
        /// The BTN fore color
        /// </summary>
        private Color _btnForeColor = Color.White;
        /// <summary>
        /// 按钮字体颜色
        /// </summary>
        /// <value>The color of the BTN fore.</value>
        [Description("按钮字体颜色"), Category("自定义")]
        public virtual Color BtnForeColor
        {
            get { return _btnForeColor; }
            set
            {
                _btnForeColor = value;
                this.lbl.ForeColor = value;
            }
        }

        /// <summary>
        /// The BTN font
        /// </summary>
        private Font _btnFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        /// <summary>
        /// 按钮字体
        /// </summary>
        /// <value>The BTN font.</value>
        [Description("按钮字体"), Category("自定义")]
        public Font BtnFont
        {
            get { return _btnFont; }
            set
            {
                _btnFont = value;
                this.lbl.Font = value;
            }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        [Description("按钮点击事件"), Category("自定义")]
        public event EventHandler BtnClick;

        /// <summary>
        /// The BTN text
        /// </summary>
        private string _btnText;
        /// <summary>
        /// 按钮文字
        /// </summary>
        /// <value>The BTN text.</value>
        [Description("按钮文字"), Category("自定义")]
        public virtual string BtnText
        {
            get { return _btnText; }
            set
            {
                _btnText = value;
                lbl.Text = value;
            }
        }

        /// <summary>
        /// The m tips color
        /// </summary>
        private Color m_tipsColor = Color.FromArgb(232, 30, 99);
        /// <summary>
        /// 角标颜色
        /// </summary>
        /// <value>The color of the tips.</value>
        [Description("角标颜色"), Category("自定义")]
        public Color TipsColor
        {
            get { return m_tipsColor; }
            set { m_tipsColor = value; }
        }
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="UCBtnExt" /> class.
        /// </summary>
        public UCBtnExt()
        {
            InitializeComponent();
            this.TabStop = false;
            lblTips.Paint += lblTips_Paint;
        }

        /// <summary>
        /// Handles the Paint event of the lblTips control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        void lblTips_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SetGDIHigh();
            e.Graphics.FillEllipse(new SolidBrush(m_tipsColor), new Rectangle(0, 0, lblTips.Width - 1, lblTips.Height - 1));
            System.Drawing.SizeF sizeEnd = e.Graphics.MeasureString(TipsText, lblTips.Font);

            e.Graphics.DrawString(TipsText, lblTips.Font, new SolidBrush(lblTips.ForeColor), new PointF((lblTips.Width - sizeEnd.Width) / 2, (lblTips.Height - sizeEnd.Height) / 2 + 1));
        }

        /// <summary>
        /// Handles the MouseDown event of the lbl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.BtnClick != null)
                BtnClick(this, e);
        }
    }
}
