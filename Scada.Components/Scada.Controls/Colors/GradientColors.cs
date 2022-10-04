

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
    /// Class GradientColors.
    /// </summary>
    public class GradientColors
    {
        /// <summary>
        /// The orange
        /// </summary>
        private static Color[] orange = new Color[] { Color.FromArgb(252, 196, 136), Color.FromArgb(243, 138, 159) };

        /// <summary>
        /// Gets the orange.
        /// </summary>
        /// <value>The orange.</value>
        public static Color[] Orange
        {
            get { return GradientColors.orange; }
            internal set { GradientColors.orange = value; }
        }
        /// <summary>
        /// The light green
        /// </summary>
        private static Color[] lightGreen = new Color[] { Color.FromArgb(210, 251, 123), Color.FromArgb(152, 231, 160) };

        /// <summary>
        /// Gets the light green.
        /// </summary>
        /// <value>The light green.</value>
        public static Color[] LightGreen
        {
            get { return GradientColors.lightGreen; }
            internal set { GradientColors.lightGreen = value; }
        }
        /// <summary>
        /// The green
        /// </summary>
        private static Color[] green = new Color[] { Color.FromArgb(138, 241, 124), Color.FromArgb(32, 190, 179) };

        /// <summary>
        /// Gets the green.
        /// </summary>
        /// <value>The green.</value>
        public static Color[] Green
        {
            get { return GradientColors.green; }
            internal set { GradientColors.green = value; }
        }
        /// <summary>
        /// The blue
        /// </summary>
        private static Color[] blue = new Color[] { Color.FromArgb(193, 232, 251), Color.FromArgb(162, 197, 253) };

        /// <summary>
        /// Gets the blue.
        /// </summary>
        /// <value>The blue.</value>
        public static Color[] Blue
        {
            get { return GradientColors.blue; }
            internal set { GradientColors.blue = value; }
        }
        /// <summary>
        /// The blue green
        /// </summary>
        private static Color[] blueGreen = new Color[] { Color.FromArgb(122, 251, 218), Color.FromArgb(16, 193, 252) };

        /// <summary>
        /// Gets the blue green.
        /// </summary>
        /// <value>The blue green.</value>
        public static Color[] BlueGreen
        {
            get { return GradientColors.blueGreen; }
            internal set { GradientColors.blueGreen = value; }
        }
        /// <summary>
        /// The light violet
        /// </summary>
        private static Color[] lightViolet = new Color[] { Color.FromArgb(248, 192, 234), Color.FromArgb(164, 142, 210) };

        /// <summary>
        /// Gets the light violet.
        /// </summary>
        /// <value>The light violet.</value>
        public static Color[] LightViolet
        {
            get { return GradientColors.lightViolet; }
            internal set { GradientColors.lightViolet = value; }
        }
        /// <summary>
        /// The violet
        /// </summary>
        private static Color[] violet = new Color[] { Color.FromArgb(185, 154, 241), Color.FromArgb(137, 124, 242) };

        /// <summary>
        /// Gets the violet.
        /// </summary>
        /// <value>The violet.</value>
        public static Color[] Violet
        {
            get { return GradientColors.violet; }
            internal set { GradientColors.violet = value; }
        }
        /// <summary>
        /// The gray
        /// </summary>
        private static Color[] gray = new Color[] { Color.FromArgb(233, 238, 239), Color.FromArgb(147, 162, 175) };

        /// <summary>
        /// Gets the gray.
        /// </summary>
        /// <value>The gray.</value>
        public static Color[] Gray
        {
            get { return GradientColors.gray; }
            internal set { GradientColors.gray = value; }
        }
    }
}
