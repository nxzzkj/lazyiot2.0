

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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCWave.
    /// Implements the <see cref="System.Windows.Forms.Control" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    public class UCWave : Control
    {
        /// <summary>
        /// Occurs when [on painted].
        /// </summary>
        public event PaintEventHandler OnPainted;

        /// <summary>
        /// The m wave color
        /// </summary>
        private Color m_waveColor = Color.FromArgb(255, 77, 59);

        /// <summary>
        /// Gets or sets the color of the wave.
        /// </summary>
        /// <value>The color of the wave.</value>
        [Description("波纹颜色"), Category("自定义")]
        public Color WaveColor
        {
            get { return m_waveColor; }
            set { m_waveColor = value; }
        }

        /// <summary>
        /// The m wave width
        /// </summary>
        private int m_waveWidth = 200;
        /// <summary>
        /// 为方便计算，强制使用10的倍数
        /// </summary>
        /// <value>The width of the wave.</value>
        [Description("波纹宽度（为方便计算，强制使用10的倍数）"), Category("自定义")]
        public int WaveWidth
        {
            get { return m_waveWidth; }
            set
            {
                m_waveWidth = value;
                m_waveWidth = m_waveWidth / 10 * 10;
                intLeftX = value * -1;
            }
        }

        /// <summary>
        /// The m wave height
        /// </summary>
        private int m_waveHeight = 30;
        /// <summary>
        /// 波高
        /// </summary>
        /// <value>The height of the wave.</value>
        [Description("波高"), Category("自定义")]
        public int WaveHeight
        {
            get { return m_waveHeight; }
            set { m_waveHeight = value; }
        }

        /// <summary>
        /// The m wave sleep
        /// </summary>
        private int m_waveSleep = 50;
        /// <summary>
        /// 波运行速度（运行时间间隔，毫秒）
        /// </summary>
        /// <value>The wave sleep.</value>
        [Description("波运行速度（运行时间间隔，毫秒）"), Category("自定义")]
        public int WaveSleep
        {
            get { return m_waveSleep; }
            set
            {
                if (value <= 0)
                    return;
                m_waveSleep = value;
                if (timer != null)
                {
                    timer.Enabled = false;
                    timer.Interval = value;
                    timer.Enabled = true;
                }
            }
        }

        /// <summary>
        /// The timer
        /// </summary>
        Timer timer = new Timer();
        /// <summary>
        /// The int left x
        /// </summary>
        int intLeftX = -200;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCWave" /> class.
        /// </summary>
        public UCWave()
        {
            this.Size = new Size(600, 100);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            timer.Interval = m_waveSleep;
            timer.Tick += timer_Tick;
            this.VisibleChanged += UCWave_VisibleChanged;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the UCWave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void UCWave_VisibleChanged(object sender, EventArgs e)
        {
            timer.Enabled = this.Visible;
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void timer_Tick(object sender, EventArgs e)
        {
            intLeftX -= 10;
            if (intLeftX == m_waveWidth * -2)
                intLeftX = m_waveWidth * -1;
            this.Refresh();
        }
        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SetGDIHigh();
            List<Point> lst1 = new List<Point>();
            List<Point> lst2 = new List<Point>();
            int m_intX = intLeftX;
            while (true)
            {
                lst1.Add(new Point(m_intX, 1));
                lst1.Add(new Point(m_intX + m_waveWidth / 2, m_waveHeight));

                lst2.Add(new Point(m_intX + m_waveWidth / 2, 1));
                lst2.Add(new Point(m_intX + m_waveWidth / 2 + m_waveWidth / 2, m_waveHeight));
                m_intX += m_waveWidth;
                if (m_intX > this.Width + m_waveWidth)
                    break;
            }

            GraphicsPath path1 = new GraphicsPath();
            path1.AddCurve(lst1.ToArray(), 0.5F);
            path1.AddLine(this.Width + 1, -1, this.Width + 1, this.Height);
            path1.AddLine(this.Width + 1, this.Height, -1, this.Height);
            path1.AddLine(-1, this.Height, -1, -1);

            GraphicsPath path2 = new GraphicsPath();
            path2.AddCurve(lst2.ToArray(), 0.5F);
            path2.AddLine(this.Width + 1, -1, this.Width + 1, this.Height);
            path2.AddLine(this.Width + 1, this.Height, -1, this.Height);
            path2.AddLine(-1, this.Height, -1, -1);

            g.FillPath(new SolidBrush(Color.FromArgb(220, m_waveColor.R, m_waveColor.G, m_waveColor.B)), path1);
            g.FillPath(new SolidBrush(Color.FromArgb(220, m_waveColor.R, m_waveColor.G, m_waveColor.B)), path2);

            if (OnPainted != null)
            {
                OnPainted(this, e);
            }
        }
    }
}
