

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
    /// Class UCTimeLine.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCTimeLine : UserControl
    {
        /// <summary>
        /// The line color
        /// </summary>
        private Color lineColor = TextColors.Light;

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        /// <value>The color of the line.</value>
        [Description("连接线颜色"), Category("自定义")]
        public Color LineColor
        {
            get { return lineColor; }
            set
            {
                lineColor = value;
                Invalidate();
            }
        }
        /// <summary>
        /// The title font
        /// </summary>
        private Font titleFont = new Font("微软雅黑", 14f);

        /// <summary>
        /// Gets or sets the title font.
        /// </summary>
        /// <value>The title font.</value>
        [Description("标题字体"), Category("自定义")]
        public Font TitleFont
        {
            get { return titleFont; }
            set
            {
                titleFont = value;
                ReloadItems();
            }
        }

        /// <summary>
        /// The title forcolor
        /// </summary>
        private Color titleForcolor = TextColors.MoreDark;

        /// <summary>
        /// Gets or sets the title forcolor.
        /// </summary>
        /// <value>The title forcolor.</value>
        [Description("标题颜色"), Category("自定义")]
        public Color TitleForcolor
        {
            get { return titleForcolor; }
            set
            {
                titleForcolor = value;
                ReloadItems();
            }
        }

        /// <summary>
        /// The details font
        /// </summary>
        private Font detailsFont = new Font("微软雅黑", 10);

        /// <summary>
        /// Gets or sets the details font.
        /// </summary>
        /// <value>The details font.</value>
        [Description("详情字体"), Category("自定义")]
        public Font DetailsFont
        {
            get { return detailsFont; }
            set
            {
                detailsFont = value;
                ReloadItems();
            }
        }

        /// <summary>
        /// The details forcolor
        /// </summary>
        private Color detailsForcolor = TextColors.Light;

        /// <summary>
        /// Gets or sets the details forcolor.
        /// </summary>
        /// <value>The details forcolor.</value>
        [Description("详情颜色"), Category("自定义")]
        public Color DetailsForcolor
        {
            get { return detailsForcolor; }
            set
            {
                detailsForcolor = value;
                ReloadItems();
            }
        }

        /// <summary>
        /// The items
        /// </summary>
        TimeLineItem[] items;

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [Description("项列表"), Category("自定义")]
        public TimeLineItem[] Items
        {
            get { return items; }
            set
            {
                items = value;
                ReloadItems();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCTimeLine"/> class.
        /// </summary>
        public UCTimeLine()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
            items = new TimeLineItem[0];
            if (ControlHelper.IsDesignMode())
            {
                items = new TimeLineItem[4];
                for (int i = 0; i < 4; i++)
                {
                    items[i] = new TimeLineItem()
                    {
                        Title = DateTime.Now.AddMonths(-1 * (3 - i)).ToString("yyyy年MM月"),
                        Details = DateTime.Now.AddMonths(-1 * (3 - i)).ToString("yyyy年MM月") + "发生了一件大事，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，咔嚓一声打了一个炸雷，然后王二麻子他爹王咔嚓出生了。"
                    };
                }
                ReloadItems();
            }
        }

        /// <summary>
        /// Reloads the items.
        /// </summary>
        private void ReloadItems()
        {
            try
            {
                ControlHelper.FreezeControl(this, true);
                this.Controls.Clear();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        FlowLayoutPanel panelTitle = new FlowLayoutPanel();
                        panelTitle.Dock = DockStyle.Top;
                        panelTitle.AutoScroll = false;
                        panelTitle.Padding = new System.Windows.Forms.Padding(5);
                        panelTitle.Name = "title_" + Guid.NewGuid().ToString();

                        Label lblTitle = new Label();
                        lblTitle.Dock = DockStyle.Top;
                        lblTitle.AutoSize = true;
                        lblTitle.Font = titleFont;
                        lblTitle.ForeColor = titleForcolor;
                        lblTitle.Text = item.Title;
                        lblTitle.SizeChanged += item_SizeChanged;
                        panelTitle.Controls.Add(lblTitle);
                        this.Controls.Add(panelTitle);
                        panelTitle.BringToFront();


                        FlowLayoutPanel panelDetails = new FlowLayoutPanel();
                        panelDetails.Dock = DockStyle.Top;
                        panelDetails.AutoScroll = false;
                        panelDetails.Padding = new System.Windows.Forms.Padding(5);
                        panelDetails.Name = "details_" + Guid.NewGuid().ToString();
                        Label lblDetails = new Label();
                        lblDetails.AutoSize = true;
                        lblDetails.Dock = DockStyle.Top;
                        lblDetails.Font = detailsFont;
                        lblDetails.ForeColor = detailsForcolor;
                        lblDetails.Text = item.Details;
                        lblDetails.SizeChanged += item_SizeChanged;
                        panelDetails.Controls.Add(lblDetails);
                        this.Controls.Add(panelDetails);
                        panelDetails.BringToFront();

                    }
                }
            }
            finally
            {
                ControlHelper.FreezeControl(this, false);
            }
        }

        /// <summary>
        /// Handles the SizeChanged event of the item control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void item_SizeChanged(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Parent.Height = lbl.Height + 10;
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
            var lst = this.Controls.ToArray().Where(p => p.Name.StartsWith("title_")).ToList();
            for (int i = 0; i < lst.Count; i++)
            {
                //画圆
                g.DrawEllipse(new Pen(new SolidBrush(lineColor)), new Rectangle(7, lst[i].Location.Y + 10, 16, 16));
                //划线
                if (i != lst.Count - 1)
                {
                    g.DrawLine(new Pen(new SolidBrush(lineColor)), new Point(7 + 8, lst[i].Location.Y + 10 - 2), new Point(7 + 8, lst[i + 1].Location.Y + 10 + 16 + 2));
                }
            }
        }
    }

    /// <summary>
    /// Class TimeLineItem.
    /// </summary>
    public class TimeLineItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; set; }
        public override string ToString()
        {
            return " 时间="+Title+",信息="+ Details;
        }
    }
}
