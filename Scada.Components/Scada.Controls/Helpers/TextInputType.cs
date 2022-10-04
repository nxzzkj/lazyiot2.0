

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
using System.Linq;
using System.Text;

namespace Scada.Controls
{
    /// <summary>
    /// 功能描述:文本控件输入类型
    /// 作　　者:HZH
    /// 创建日期:2019-02-28 10:09:00
    /// </summary>
    public enum TextInputType
    {
        /// <summary>
        /// 不控制输入
        /// </summary>
        [Description("不控制输入")]
        NotControl = 1,
        /// <summary>
        /// 任意数字
        /// </summary>
        [Description("任意数字")]
        Number = 2,
        /// <summary>
        /// 非负数
        /// </summary>
        [Description("非负数")]
        UnsignNumber = 4,
        /// <summary>
        /// 正数
        /// </summary>
        [Description("正数")]
        PositiveNumber = 8,
        /// <summary>
        /// 整数
        /// </summary>
        [Description("整数")]
        Integer = 16,
        /// <summary>
        /// 非负整数
        /// </summary>
        [Description("非负整数")]
        PositiveInteger = 32,
        /// <summary>
        /// 正则验证
        /// </summary>
        [Description("正则验证")]
        Regex = 64
    }
}
