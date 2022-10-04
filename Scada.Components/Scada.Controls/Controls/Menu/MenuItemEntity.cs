

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

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class MenuItemEntity.
    /// </summary>
    [Serializable]
    public class MenuItemEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }
        /// <summary>
        /// 文字
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// The m childrens
        /// </summary>
        private List<MenuItemEntity> m_childrens = new List<MenuItemEntity>();
        /// <summary>
        /// 子节点
        /// </summary>
        /// <value>The childrens.</value>
        public List<MenuItemEntity> Childrens
        {
            get
            {
                return m_childrens ?? (new List<MenuItemEntity>());
            }
            set
            {
                m_childrens = value;
            }
        }
        /// <summary>
        /// 自定义数据源，一般用于扩展展示，比如定义节点图片等
        /// </summary>
        /// <value>The data source.</value>
        public object DataSource { get; set; }

    }
}
