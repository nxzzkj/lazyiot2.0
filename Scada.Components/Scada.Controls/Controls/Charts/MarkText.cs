

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
using System.Drawing;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class MarkText.
    /// </summary>
    public class MarkText
    {
        /// <summary>
        /// The mark text offect
        /// </summary>
        public static readonly int MarkTextOffect = 5;

        /// <summary>
        /// Gets or sets the curve key.
        /// </summary>
        /// <value>The curve key.</value>
        public string CurveKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mark text.
        /// </summary>
        /// <value>The mark text.</value>
        public string Text
        {
            get;
            set;
        }

        private Color? textColor = null;

        public Color? TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }


        /// <summary>
        /// The position style
        /// </summary>
        private MarkTextPositionStyle positionStyle = MarkTextPositionStyle.Auto;

        /// <summary>
        /// Gets or sets the position style.
        /// </summary>
        /// <value>The position style.</value>
        public MarkTextPositionStyle PositionStyle
        {
            get { return positionStyle; }
            set { positionStyle = value; }
        }

        /// <summary>
        /// Calculates the index of the direction from data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="Index">The index.</param>
        /// <returns>MarkTextPositionStyle.</returns>
        public static MarkTextPositionStyle CalculateDirectionFromDataIndex(float[] data, int Index)
        {
            float num = (Index == 0) ? data[Index] : data[Index - 1];
            float num2 = (Index == data.Length - 1) ? data[Index] : data[Index + 1];
            if (num < data[Index] && data[Index] < num2)
            {
                return MarkTextPositionStyle.Left;
            }
            if (num > data[Index] && data[Index] > num2)
            {
                return MarkTextPositionStyle.Right;
            }
            if (num <= data[Index] && data[Index] >= num2)
            {
                return MarkTextPositionStyle.Up;
            }
            if (num >= data[Index] && data[Index] <= num2)
            {
                return MarkTextPositionStyle.Down;
            }
            return MarkTextPositionStyle.Up;
        }
    }
}
