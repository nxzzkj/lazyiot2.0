

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
    /// Class UCHorizontalListItem.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [ToolboxItem(false)]
    public partial class UCHorizontalListItem : UserControl
    {
        /// <summary>
        /// Occurs when [selected item].
        /// </summary>
        public event EventHandler SelectedItem;
        /// <summary>
        /// The data source
        /// </summary>
        private KeyValuePair<string, string> _DataSource = new KeyValuePair<string, string>();
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public KeyValuePair<string, string> DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                var g = lblTitle.CreateGraphics();
                int intWidth = ControlHelper.GetStringWidth(value.Value, g, lblTitle.Font);
                g.Dispose();
                if (intWidth < 50)
                    intWidth = 50;
                this.Width = intWidth + 20;
                lblTitle.Text = value.Value;
                SetSelect(false);
            }
        }

        private Color selectedColor = Color.FromArgb(255, 77, 59);

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCHorizontalListItem" /> class.
        /// </summary>
        public UCHorizontalListItem()
        {
            InitializeComponent();
            this.Dock = DockStyle.Right;
            this.MouseDown += Item_MouseDown;
            this.lblTitle.MouseDown += Item_MouseDown;
            this.ucSplitLine_H1.MouseDown += Item_MouseDown;
        }

        /// <summary>
        /// Handles the MouseDown event of the Item control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        void Item_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectedItem != null)
                SelectedItem(this, e);
        }

        /// <summary>
        /// Sets the select.
        /// </summary>
        /// <param name="bln">if set to <c>true</c> [BLN].</param>
        public void SetSelect(bool bln)
        {
            if (bln)
            {
                lblTitle.ForeColor = selectedColor;
                ucSplitLine_H1.BackColor = selectedColor;
                ucSplitLine_H1.Visible = true;
                this.lblTitle.Padding = new Padding(0, 0, 0, 5);
            }
            else
            {
                lblTitle.ForeColor = Color.FromArgb(64, 64, 64);
                ucSplitLine_H1.Visible = false;
                this.lblTitle.Padding = new Padding(0, 0, 0, 0);
            }
        }
    }
}
