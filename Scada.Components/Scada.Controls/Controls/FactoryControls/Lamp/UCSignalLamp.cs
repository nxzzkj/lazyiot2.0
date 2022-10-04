

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
    /// Class UCSignalLamp.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public class UCSignalLamp : UserControl
    {
        /// <summary>
        /// The is show border
        /// </summary>
        private bool isShowBorder = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show border.
        /// </summary>
        /// <value><c>true</c> if this instance is show border; otherwise, <c>false</c>.</value>
        [Description("是否显示边框"), Category("自定义")]
        public bool IsShowBorder
        {
            get { return isShowBorder; }
            set
            {
                isShowBorder = value;
                Refresh();
            }
        }

        /// <summary>
        /// The lamp color
        /// </summary>
        private Color[] lampColor = new Color[] { Color.FromArgb(255, 77, 59) };

        /// <summary>
        /// Gets or sets the color of the lamp.
        /// </summary>
        /// <value>The color of the lamp.</value>
        [Description("灯颜色，当需要闪烁时，至少需要2个及以上颜色，不需要闪烁则至少需要1个颜色"), Category("自定义")]
        public Color[] LampColor
        {
            get { return lampColor; }
            set
            {
                if (value == null || value.Length <= 0)
                    return;
                lampColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// The is highlight
        /// </summary>
        private bool isHighlight = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is highlight.
        /// </summary>
        /// <value><c>true</c> if this instance is highlight; otherwise, <c>false</c>.</value>
        [Description("是否高亮显示"), Category("自定义")]
        public bool IsHighlight
        {
            get { return isHighlight; }
            set
            {
                isHighlight = value;
                Refresh();
            }
        }

        /// <summary>
        /// The twinkle speed
        /// </summary>
        private int twinkleSpeed = 0;

        /// <summary>
        /// Gets or sets the twinkle speed.
        /// </summary>
        /// <value>The twinkle speed.</value>
        [Description("闪烁间隔时间（毫秒），当为0时不闪烁"), Category("自定义")]
        public int TwinkleSpeed
        {
            get { return twinkleSpeed; }
            set
            {
                if (value < 0)
                    return;
                twinkleSpeed = value;
                if (value == 0 || lampColor.Length <= 1)
                {
                    timer.Enabled = false;
                }
                else
                {
                    intColorIndex = 0;
                    timer.Interval = value;
                    timer.Enabled = true;
                }
                Refresh();
            }
        }
        /// <summary>
        /// The timer
        /// </summary>
        Timer timer;
        /// <summary>
        /// The int color index
        /// </summary>
        int intColorIndex = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCSignalLamp" /> class.
        /// </summary>
        public UCSignalLamp()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Size = new Size(50, 50);
            this.SizeChanged += UCSignalLamp_SizeChanged;
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += timer_Tick;
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void timer_Tick(object sender, EventArgs e)
        {
            intColorIndex++;
            if (intColorIndex >= lampColor.Length)
                intColorIndex = 0;
            Refresh();
        }
        /// <summary>
        /// Handles the SizeChanged event of the UCSignalLamp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void UCSignalLamp_SizeChanged(object sender, EventArgs e)
        {
            var maxSize = Math.Min(this.Width, this.Height);
            if (this.Width != maxSize)
                this.Width = maxSize;
            if (this.Height != maxSize)
                this.Height = maxSize;
        }

        /// <summary>
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SetGDIHigh();           

            Color c1 = lampColor[intColorIndex];
            g.FillEllipse(new SolidBrush(c1), new Rectangle(this.ClientRectangle.Location, this.ClientRectangle.Size - new Size(1, 1)));

            if (isHighlight)
            {
                GraphicsPath gp = new GraphicsPath();

                Rectangle rec = new Rectangle(5, 5, this.Width - 10 - 1, this.Height - 10 - 1);
                gp.AddEllipse(rec);

                Color[] surroundColor = new Color[] { c1 };
                PathGradientBrush pb = new PathGradientBrush(gp);
                pb.CenterColor = Color.White;
                pb.SurroundColors = surroundColor;
                g.FillPath(pb, gp);
            }

            if (isShowBorder)
            {
                g.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 2), new Rectangle(4, 4, this.Width - 1 - 8, this.Height - 1 - 8));
            }
        }
    }
}
