

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
    /// Interface IPageControl
    /// </summary>
    public interface IPageControl
    {
        /// <summary>
        /// 数据源改变时发生
        /// </summary>
        event PageControlEventHandler ShowSourceChanged;
        /// <summary>
        /// 数据源
        /// </summary>
        /// <value>The data source.</value>
        List<object> DataSource { get; set; }
        /// <summary>
        /// 显示数量
        /// </summary>
        /// <value>The size of the page.</value>
        int PageSize { get; set; }
        /// <summary>
        /// 开始下标
        /// </summary>
        /// <value>The start index.</value>
        int StartIndex { get; set; }
        /// <summary>
        /// 第一页
        /// </summary>
        void FirstPage();
        /// <summary>
        /// 前一页
        /// </summary>
        void PreviousPage();
        /// <summary>
        /// 下一页
        /// </summary>
        void NextPage();
        /// <summary>
        /// 最后一页
        /// </summary>
        void EndPage();
        /// <summary>
        /// 重新加载
        /// </summary>
        void Reload();
        /// <summary>
        /// 获取当前页数据
        /// </summary>
        /// <returns>List&lt;System.Object&gt;.</returns>
        List<object> GetCurrentSource();
        /// <summary>
        /// 总页数
        /// </summary>
        /// <value>The page count.</value>
        int PageCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        /// <value>The index of the page.</value>
        int PageIndex { get; set; }
        PageModel PageModel { get; set; }
    }
}
