

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
using System.Drawing;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class DataGridViewColumnEntity.
    /// </summary>
    public class DataGridViewColumnEntity
    {
        /// <summary>
        /// Gets or sets the head text.
        /// </summary>
        /// <value>The head text.</value>
        public string HeadText { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the type of the width.
        /// </summary>
        /// <value>The type of the width.</value>
        public System.Windows.Forms.SizeType WidthType { get; set; }
        /// <summary>
        /// Gets or sets the data field.
        /// </summary>
        /// <value>The data field.</value>
        public string DataField { get; set; }
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        public Func<object, string> Format { get; set; }
        /// <summary>
        /// The text align
        /// </summary>
        private ContentAlignment _TextAlign = ContentAlignment.MiddleCenter;
        /// <summary>
        /// Gets or sets the text align.
        /// </summary>
        /// <value>The text align.</value>
        public ContentAlignment TextAlign { get { return _TextAlign; } set { _TextAlign = value; } }
        /// <summary>
        /// 自定义的单元格控件，一个实现IDataGridViewCustomCell的Control
        /// </summary>
        /// <value>The custom cell.</value>
        private Type customCellType = null;
        public Type CustomCellType
        {
            get
            {
                return customCellType;
            }
            set
            {
                if (!typeof(IDataGridViewCustomCell).IsAssignableFrom(value) || !value.IsSubclassOf(typeof(System.Windows.Forms.Control)))
                    throw new Exception("行控件没有实现IDataGridViewCustomCell接口");
                customCellType = value;
            }
        }
    }
}
