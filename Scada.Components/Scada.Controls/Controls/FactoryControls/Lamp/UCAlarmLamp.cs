

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
    /// Class UCAlarmLamp.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public class UCAlarmLamp : UserControl
    {
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
        /// The lampstand
        /// </summary>
        private Color lampstand = Color.FromArgb(105, 105, 105);

        /// <summary>
        /// Gets or sets the lampstand.
        /// </summary>
        /// <value>The lampstand.</value>
        [Description("灯座颜色"), Category("自定义")]
        public Color Lampstand
        {
            get { return lampstand; }
            set { lampstand = value; }
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
        /// The m rect working
        /// </summary>
        Rectangle m_rectWorking;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCAlarmLamp" /> class.
        /// </summary>
        public UCAlarmLamp()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.SizeChanged += UCAlarmLamp_SizeChanged;
            this.Size = new Size(50, 50);
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += timer_Tick;
        }

        /// <summary>
        /// Handles the SizeChanged event of the UCAlarmLamp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void UCAlarmLamp_SizeChanged(object sender, EventArgs e)
        {
            m_rectWorking = new Rectangle(10, 0, this.Width - 20, this.Height);
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
        /// 引发 <see cref="E:System.Windows.Forms.Control.Paint" /> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SetGDIHigh();

            Color c1 = lampColor[intColorIndex];
            GraphicsPath path = new GraphicsPath();
            path.AddLine(new Point(m_rectWorking.Left, m_rectWorking.Bottom), new Point(m_rectWorking.Left, m_rectWorking.Top + m_rectWorking.Width));
            path.AddArc(new Rectangle(m_rectWorking.Left, m_rectWorking.Top, m_rectWorking.Width, m_rectWorking.Width), 180f, 180f);
            path.AddLine(new Point(m_rectWorking.Right, m_rectWorking.Top + m_rectWorking.Width), new Point(m_rectWorking.Right, m_rectWorking.Bottom));
            path.CloseAllFigures();
            g.FillPath(new SolidBrush(c1), path);

            g.FillRectangle(new SolidBrush(lampstand), new Rectangle(5, m_rectWorking.Bottom - 19, this.Width - 10, 10));
            g.FillRectangle(new SolidBrush(lampstand), new Rectangle(0, m_rectWorking.Bottom - 10, this.Width, 10));
        }
    }
}
