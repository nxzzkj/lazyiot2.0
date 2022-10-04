

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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCPanelQuote.
    /// Implements the <see cref="System.Windows.Forms.Panel" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Panel" />
    public class UCPanelQuote : Panel
    {
        /// <summary>
        /// The border color
        /// </summary>
        private Color borderColor = LineColors.Light;

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        [Description("边框颜色"), Category("自定义")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// The left color
        /// </summary>
        private Color leftColor = StatusColors.Danger;

        /// <summary>
        /// Gets or sets the color of the left.
        /// </summary>
        /// <value>The color of the left.</value>
        [Description("左侧颜色"), Category("自定义")]
        public Color LeftColor
        {
            get { return leftColor; }
            set
            {
                leftColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCPanelQuote"/> class.
        /// </summary>
        public UCPanelQuote()
            : base()
        {
            Padding = new Padding(5, 1, 1, 1);
        }

        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SetGDIHigh();

            e.Graphics.DrawLines(new Pen(borderColor), new Point[] 
            { 
                new Point(e.ClipRectangle.Left,e.ClipRectangle.Top),
                new Point(e.ClipRectangle.Right-1,e.ClipRectangle.Top),
                new Point(e.ClipRectangle.Right-1,e.ClipRectangle.Bottom-1),
                new Point(e.ClipRectangle.Left,e.ClipRectangle.Bottom-1),
                new Point(e.ClipRectangle.Left,e.ClipRectangle.Top)
            });

            e.Graphics.FillRectangle(new SolidBrush(leftColor), new Rectangle(0, 0, 5, this.Height));
        }
    }
}
