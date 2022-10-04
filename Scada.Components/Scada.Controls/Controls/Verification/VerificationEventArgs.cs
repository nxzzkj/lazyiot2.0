

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
    /// Class VerificationEventArgs.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class VerificationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the verification control.
        /// </summary>
        /// <value>The verification control.</value>
        public Control VerificationControl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [verify success].
        /// </summary>
        /// <value><c>true</c> if [verify success]; otherwise, <c>false</c>.</value>
        public bool IsVerifySuccess { get; set; }
        /// <summary>
        /// Gets or sets the verification model.
        /// </summary>
        /// <value>The verification model.</value>
        public VerificationModel VerificationModel { get; set; }
        /// <summary>
        /// 是否已处理，如果为true，则不再使用默认验证提示功能
        /// </summary>
        /// <value><c>true</c> if this instance is processed; otherwise, <c>false</c>.</value>
        public bool IsProcessed { get; set; }
        /// <summary>
        /// Gets or sets 正则表达式
        /// </summary>
        /// <value>The custom regex.</value>
        public string Regex { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VerificationEventArgs"/> is required.
        /// </summary>
        /// <value><c>true</c> if required; otherwise, <c>false</c>.</value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the error MSG.
        /// </summary>
        /// <value>The error MSG.</value>
        public string ErrorMsg { get; set; }
    }
}
