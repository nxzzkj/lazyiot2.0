

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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCPond.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public class UCPond : UserControl
    {
        /// <summary>
        /// The maximum value
        /// </summary>
        private decimal maxValue = 100;

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        [Description("最大值"), Category("自定义")]
        public decimal MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value < m_value)
                    return;
                maxValue = value;
                Refresh();
            }
        }

        /// <summary>
        /// The m value
        /// </summary>
        private decimal m_value = 0;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Description("值"), Category("自定义")]
        public decimal Value
        {
            get { return m_value; }
            set
            {
                if (value < 0)
                    return;
                if (value > maxValue)
                    m_value = maxValue;
                else
                    m_value = value;
                Refresh();
            }
        }

        /// <summary>
        /// The wall color
        /// </summary>
        private Color wallColor = Color.FromArgb(255, 77, 59);

        /// <summary>
        /// Gets or sets the color of the wall.
        /// </summary>
        /// <value>The color of the wall.</value>
        [Description("池壁颜色"), Category("自定义")]
        public Color WallColor
        {
            get { return wallColor; }
            set
            {
                wallColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// The wall width
        /// </summary>
        private int wallWidth = 2;

        /// <summary>
        /// Gets or sets the width of the wall.
        /// </summary>
        /// <value>The width of the wall.</value>
        [Description("池壁宽度"), Category("自定义")]
        public int WallWidth
        {
            get { return wallWidth; }
            set
            {
                if (value <= 0)
                    return;
                wallWidth = value;
                Refresh();
            }
        }

        /// <summary>
        /// The liquid color
        /// </summary>
        private Color liquidColor = Color.FromArgb(3, 169, 243);

        /// <summary>
        /// Gets or sets the color of the liquid.
        /// </summary>
        /// <value>The color of the liquid.</value>
        [Description("液体颜色"), Category("自定义")]
        public Color LiquidColor
        {
            get { return liquidColor; }
            set { liquidColor = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCPond" /> class.
        /// </summary>
        public UCPond()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Size = new Size(150, 50);
        }
        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Height <= 0)
                return;
            var g = e.Graphics;
            g.SetGDIHigh();
            int intHeight = (int)(m_value / maxValue * this.Height);
            if (intHeight != 0)
            {
                g.FillRectangle(new SolidBrush(liquidColor), new Rectangle(0, this.Height - intHeight, this.Width, intHeight));
            }
            //划边
            g.FillRectangle(new SolidBrush(wallColor), 0, 0, wallWidth, this.Height);
            g.FillRectangle(new SolidBrush(wallColor), 0, this.Height - wallWidth, this.Width, wallWidth);
            g.FillRectangle(new SolidBrush(wallColor), this.Width - wallWidth-1, 0, wallWidth, this.Height);
        }
    }
}
