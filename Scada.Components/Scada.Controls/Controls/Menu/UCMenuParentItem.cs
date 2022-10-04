

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
    /// 父类节点
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// Implements the <see cref="Scada.Controls.Controls.IMenuItem" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="Scada.Controls.Controls.IMenuItem" />
    [ToolboxItem(false)]
    public partial class UCMenuParentItem : UserControl, IMenuItem
    {
        /// <summary>
        /// Occurs when [selected item].
        /// </summary>
        public event EventHandler SelectedItem;

        /// <summary>
        /// The m data source
        /// </summary>
        private MenuItemEntity m_dataSource;
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public MenuItemEntity DataSource
        {
            get
            {
                return m_dataSource;
            }
            set
            {
                m_dataSource = value;
                if (value != null)
                {
                    lblTitle.Text = value.Text;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCMenuParentItem" /> class.
        /// </summary>
        public UCMenuParentItem()
        {
            InitializeComponent();
            lblTitle.MouseDown += lblTitle_MouseDown;
        }

        /// <summary>
        /// 设置样式
        /// </summary>
        /// <param name="styles">key:属性名称,value:属性值</param>
        /// <exception cref="System.Exception">菜单元素设置样式异常</exception>
        /// <exception cref="Exception">菜单元素设置样式异常</exception>
        public void SetStyle(Dictionary<string, object> styles)
        {
            Type t = this.GetType();
            foreach (var item in styles)
            {
                var pro = t.GetProperty(item.Key);
                if (pro != null && pro.CanWrite)
                {
                    try
                    {
                        pro.SetValue(this, item.Value, null);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("菜单元素设置样式异常", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 设置选中样式
        /// </summary>
        /// <param name="blnSelected">是否选中</param>
        public void SetSelectedStyle(bool? blnSelected)
        {
            if (blnSelected.HasValue)
            {
                if (blnSelected.Value)
                {
                    this.lblTitle.Image = Properties.Resources.sanjiao1;
                }
                else
                {
                    this.lblTitle.Image = Properties.Resources.sanjiao2;
                }
            }
            else
            {
                this.lblTitle.Image = null;
            }
        }

        /// <summary>
        /// Handles the MouseDown event of the lblTitle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectedItem != null)
            {
                SelectedItem(this, e);
            }
        }
    }
}
