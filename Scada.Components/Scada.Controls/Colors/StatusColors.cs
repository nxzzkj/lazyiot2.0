

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
    /// 状态颜色
    /// </summary>
    public class StatusColors
    {
        /// <summary>
        /// The primary
        /// </summary>
        private static Color _Primary = ColorTranslator.FromHtml("#409eff");

        /// <summary>
        /// Gets or sets the primary.
        /// </summary>
        /// <value>The primary.</value>
        public static Color Primary
        {
            get { return _Primary; }
            internal set { _Primary = value; }
        }
        /// <summary>
        /// The success
        /// </summary>
        private static Color _Success = ColorTranslator.FromHtml("#67c23a");

        /// <summary>
        /// Gets or sets the success.
        /// </summary>
        /// <value>The success.</value>
        public static Color Success
        {
            get { return _Success; }
            internal set { _Success = value; }
        }
        /// <summary>
        /// The warning
        /// </summary>
        private static Color _Warning = ColorTranslator.FromHtml("#e6a23c");

        /// <summary>
        /// Gets or sets the warning.
        /// </summary>
        /// <value>The warning.</value>
        public static Color Warning
        {
            get { return _Warning; }
            internal set { _Warning = value; }
        }
        /// <summary>
        /// The danger
        /// </summary>
        private static Color _Danger = ColorTranslator.FromHtml("#f56c6c");

        /// <summary>
        /// Gets or sets the danger.
        /// </summary>
        /// <value>The danger.</value>
        public static Color Danger
        {
            get { return _Danger; }
            internal set { _Danger = value; }
        }
        /// <summary>
        /// The information
        /// </summary>
        private static Color _Info = ColorTranslator.FromHtml("#909399");

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public static Color Info
        {
            get { return _Info; }
            internal set { _Info = value; }
        }
    }
}
