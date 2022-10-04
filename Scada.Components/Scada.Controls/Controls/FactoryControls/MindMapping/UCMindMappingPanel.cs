

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
    /// Class UCMindMappingPanel.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [DefaultEvent("ItemClicked")]
    public partial class UCMindMappingPanel : UserControl
    {

        /// <summary>
        /// The item context menu strip
        /// </summary>
        private ContextMenuStrip itemContextMenuStrip;

        /// <summary>
        /// Gets or sets the item context menu strip.
        /// </summary>
        /// <value>The item context menu strip.</value>
        [Description("节点关联的右键菜单"), Category("自定义")]
        public ContextMenuStrip ItemContextMenuStrip
        {
            get { return itemContextMenuStrip; }
            set
            {
                itemContextMenuStrip = value;
                this.ucMindMapping1.ItemContextMenuStrip = value;
            }
        }

        /// <summary>
        /// The item backcolor
        /// </summary>
        private Color itemBackcolor = Color.FromArgb(255, 77, 59);

        /// <summary>
        /// Gets or sets the item backcolor.
        /// </summary>
        /// <value>The item backcolor.</value>
        [Description("节点背景色，优先级小于数据源中设置的背景色"), Category("自定义")]
        public Color ItemBackcolor
        {
            get { return itemBackcolor; }
            set
            {
                itemBackcolor = value;
                this.ucMindMapping1.ItemBackcolor = value;
            }
        }
        /// <summary>
        /// The data source
        /// </summary>
        private MindMappingItemEntity dataSource;

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        [Description("数据源"), Category("自定义")]
        public MindMappingItemEntity DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;
                this.ucMindMapping1.DataSource = value;
            }
        }
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        [Description("数据源"), Category("自定义")]
        public event EventHandler ItemClicked;

        /// <summary>
        /// The line color
        /// </summary>
        private Color lineColor = Color.Black;

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        /// <value>The color of the line.</value>
        [Description("线条颜色"), Category("自定义")]
        public Color LineColor
        {
            get { return lineColor; }
            set
            {
                lineColor = value;
                this.ucMindMapping1.LineColor = value;
            }
        }


        /// <summary>
        /// Gets the select entity.
        /// </summary>
        /// <value>The select entity.</value>
        [Description("选中的数据源"), Category("自定义")]
        public MindMappingItemEntity SelectEntity
        {
            get { return ucMindMapping1.SelectEntity; }

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCMindMappingPanel" /> class.
        /// </summary>
        public UCMindMappingPanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
            ucMindMapping1.ItemClicked += ucMindMapping1_ItemClicked;
        }

        /// <summary>
        /// Handles the ItemClicked event of the ucMindMapping1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void ucMindMapping1_ItemClicked(object sender, EventArgs e)
        {
            if (ItemClicked != null)
            {
                ItemClicked(sender, e);
            }
        }
    }
}
