﻿

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
    /// Class UCListViewItem.
    /// Implements the <see cref="Scada.Controls.Controls.UCControlBase" />
    /// Implements the <see cref="Scada.Controls.Controls.IListViewItem" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCControlBase" />
    /// <seealso cref="Scada.Controls.Controls.IListViewItem" />
    [ToolboxItem(false)]
    public partial class UCListViewItem : UCControlBase, IListViewItem
    {
        /// <summary>
        /// The m data source
        /// </summary>
        private object m_dataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        public object DataSource
        {
            get
            {
                return m_dataSource;
            }
            set
            {
                m_dataSource = value;
                lblTitle.Text = value.ToString();
            }
        }

        /// <summary>
        /// 选中项事件
        /// </summary>
        public event EventHandler SelectedItemEvent;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCListViewItem" /> class.
        /// </summary>
        public UCListViewItem()
        {
            InitializeComponent();
            lblTitle.MouseDown += lblTitle_MouseDown;
        }

        /// <summary>
        /// Handles the MouseDown event of the lblTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectedItemEvent != null)
            {
                SelectedItemEvent(this, e);
            }
        }

        /// <summary>
        /// Sets the selected.
        /// </summary>
        /// <param name="blnSelected">if set to <c>true</c> [BLN selected].</param>
        public void SetSelected(bool blnSelected)
        {
            if (blnSelected)
                this.FillColor = Color.FromArgb(255, 247, 245);
            else
                this.FillColor = Color.White;
            this.Refresh();
        }
    }
}
