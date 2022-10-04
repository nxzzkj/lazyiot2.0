

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
using System.Drawing;
using System.Linq;
using System.Text;

namespace Scada.Controls
{
    /// <summary>
    /// Class LineColors.
    /// </summary>
    public class LineColors
    {
        /// <summary>
        /// The more light
        /// </summary>
        private static Color _MoreLight = ColorTranslator.FromHtml("#f2f6fc");

        /// <summary>
        /// Gets the more light.
        /// </summary>
        /// <value>The more light.</value>
        public static Color MoreLight
        {
            get { return _MoreLight; }
            internal set { _MoreLight = value; }
        }
        /// <summary>
        /// The light
        /// </summary>
        private static Color _Light = ColorTranslator.FromHtml("#ebeef5");

        /// <summary>
        /// Gets the light.
        /// </summary>
        /// <value>The light.</value>
        public static Color Light
        {
            get { return _Light; }
            internal set { _Light = value; }
        }
        /// <summary>
        /// The dark
        /// </summary>
        private static Color _Dark = ColorTranslator.FromHtml("#e4e7ed");

        /// <summary>
        /// Gets the dark.
        /// </summary>
        /// <value>The dark.</value>
        public static Color Dark
        {
            get { return _Dark; }
            internal set { _Dark = value; }
        }
        /// <summary>
        /// The more dark
        /// </summary>
        private static Color _MoreDark = ColorTranslator.FromHtml("#dcdfe6");

        /// <summary>
        /// Gets the more dark.
        /// </summary>
        /// <value>The more dark.</value>
        public static Color MoreDark
        {
            get { return _MoreDark; }
            internal set { _MoreDark = value; }
        }
    }
}
