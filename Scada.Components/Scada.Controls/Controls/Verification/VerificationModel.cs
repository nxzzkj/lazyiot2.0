

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

namespace Scada.Controls.Controls
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public enum VerificationModel
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无"), VerificationAttribute()]
        None = 1,
        /// <summary>
        /// 任意字母数字下划线
        /// </summary>
        [Description("任意字母数字下划线"), VerificationAttribute(@"^[a-zA-Z_0-1]*$", "请输入任意字母数字下划线")]
        AnyChar = 2,
        /// <summary>
        /// 任意数字
        /// </summary>
        [Description("任意数字"), VerificationAttribute(@"^[\-\+]?\d+(\.\d+)?$", "请输入任意数字")]
        Number = 4,
        /// <summary>
        /// 非负数
        /// </summary>
        [Description("非负数"), VerificationAttribute(@"^(\+)?\d+(\.\d+)?$", "请输入非负数")]
        UnsignNumber = 8,
        /// <summary>
        /// 正数
        /// </summary>
        [Description("正数"), VerificationAttribute(@"(\+)?([1-9][0-9]*(\.\d{1,2})?)|(0\.\d{1,2})", "请输入正数")]
        PositiveNumber = 16,
        /// <summary>
        /// 整数
        /// </summary>
        [Description("整数"), VerificationAttribute(@"^[\+\-]?\d+$", "请输入整数")]
        Integer = 32,
        /// <summary>
        /// 非负整数
        /// </summary>
        [Description("非负整数"), VerificationAttribute(@"^(\+)?\d+$", "请输入非负整数")]
        UnsignIntegerNumber = 64,
        /// <summary>
        /// 正整数
        /// </summary>
        [Description("正整数"), VerificationAttribute(@"^[0-9]*[1-9][0-9]*$", "请输入正整数")]
        PositiveIntegerNumber = 128,
        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱"), VerificationAttribute(@"^(([0-9a-zA-Z]+)|([0-9a-zA-Z]+[_.0-9a-zA-Z-]*[0-9a-zA-Z]+))@([a-zA-Z0-9-]+[.])+([a-zA-Z]{2}|net|NET|com|COM|gov|GOV|mil|MIL|org|ORG|edu|EDU|int|INT)$", "请输入正确的邮箱地址")]
        Email = 256,
        /// <summary>
        /// 手机
        /// </summary>
        [Description("手机"), VerificationAttribute(@"^(\+?86)?1\d{10}$", "请输入正确的手机号")]
        Phone = 512,
        /// <summary>
        /// IP
        /// </summary>
        [Description("IP"), VerificationAttribute(@"(?=(\b|\D))(((\d{1,2})|(1\d{1,2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{1,2})|(2[0-4]\d)|(25[0-5]))(?=(\b|\D))", "请输入正确的IP地址")]
        IP = 1024,
        /// <summary>
        /// Url
        /// </summary>
        [Description("Url"), VerificationAttribute(@"^[a-zA-z]+://(//w+(-//w+)*)(//.(//w+(-//w+)*))*(//?//S*)?$", "请输入正确的网址")]
        URL = 2048,
        /// <summary>
        /// 身份证号
        /// </summary>
        [Description("身份证号"), VerificationAttribute(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$", "请输入正确的身份证号")]
        IDCardNo = 4096,
        /// <summary>
        /// 正则验证
        /// </summary>
        [Description("自定义正则表达式"), VerificationAttribute()]
        Custom = 8192,
    }
}
