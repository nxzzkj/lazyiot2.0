

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

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class NavigationMenuItem.
    /// </summary>
    public class NavigationMenuItem : NavigationMenuItemBase
    {
        /// <summary>
        /// The items
        /// </summary>
        private NavigationMenuItem[] items;
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [Description("子项列表")]
        public NavigationMenuItem[] Items
        {
            get { return items; }
            set
            {
                items = value;
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        item.ParentItem = this;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has split lint at top.
        /// </summary>
        /// <value><c>true</c> if this instance has split lint at top; otherwise, <c>false</c>.</value>
        [Description("是否在此项顶部显示一个分割线")]
        public bool HasSplitLintAtTop { get; set; }

        /// <summary>
        /// Gets the parent item.
        /// </summary>
        /// <value>The parent item.</value>
        [Description("父节点")]
        public NavigationMenuItem ParentItem { get; private set; }
    }


}
