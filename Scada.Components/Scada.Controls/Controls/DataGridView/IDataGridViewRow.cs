

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
using System.Windows.Forms;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Interface IDataGridViewRow
    /// </summary>
    public interface IDataGridViewRow
    {
        /// <summary>
        /// Occurs when [row custom event].
        /// </summary>
        event DataGridViewRowCustomEventHandler RowCustomEvent;
        /// <summary>
        /// CheckBox选中事件
        /// </summary>
        event DataGridViewEventHandler CheckBoxChangeEvent;
        /// <summary>
        /// 点击单元格事件
        /// </summary>
        event DataGridViewEventHandler CellClick;
        /// <summary>
        /// 数据源改变事件
        /// </summary>
        event DataGridViewEventHandler SourceChanged;
        /// <summary>
        /// 列参数，用于创建列数和宽度
        /// </summary>
        /// <value>The columns.</value>
        List<DataGridViewColumnEntity> Columns { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is show CheckBox.
        /// </summary>
        /// <value><c>true</c> if this instance is show CheckBox; otherwise, <c>false</c>.</value>
        bool IsShowCheckBox { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        bool IsChecked { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        object DataSource { get; set; }
        /// <summary>
        /// 添加单元格元素，仅做添加控件操作，不做数据绑定，数据绑定使用BindingCells
        /// </summary>
        void ReloadCells();
        /// <summary>
        /// 绑定数据到Cell
        /// </summary>
        /// <returns>返回true则表示已处理过，否则将进行默认绑定（通常只针对有Text值的控件）</returns>
        void BindingCellData();
        /// <summary>
        /// 设置选中状态，通常为设置颜色即可
        /// </summary>
        /// <param name="blnSelected">是否选中</param>
        void SetSelect(bool blnSelected);
        /// <summary>
        /// 行高
        /// </summary>
        /// <value>The height of the row.</value>
        int RowHeight { get; set; }
    }
}
