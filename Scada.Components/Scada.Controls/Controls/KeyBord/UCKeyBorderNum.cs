

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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCKeyBorderNum.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCKeyBorderNum : UserControl
    {
        /// <summary>
        /// The use custom event
        /// </summary>
        private bool useCustomEvent = false;
        /// <summary>
        /// 是否使用自定义的事件来接收按键，当为true时将不再向系统发送按键请求
        /// </summary>
        /// <value><c>true</c> if [use custom event]; otherwise, <c>false</c>.</value>
        [Description("是否使用自定义的事件来接收按键，当为true时将不再向系统发送按键请求"), Category("自定义")]
        public bool UseCustomEvent
        {
            get { return useCustomEvent; }
            set { useCustomEvent = value; }
        }
        /// <summary>
        /// Occurs when [number click].
        /// </summary>
        [Description("数字点击事件"), Category("自定义")]
        public event EventHandler NumClick;
        /// <summary>
        /// Occurs when [backspace click].
        /// </summary>
        [Description("删除点击事件"), Category("自定义")]
        public event EventHandler BackspaceClick;
        /// <summary>
        /// Occurs when [enter click].
        /// </summary>
        [Description("回车点击事件"), Category("自定义")]
        public event EventHandler EnterClick;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCKeyBorderNum" /> class.
        /// </summary>
        public UCKeyBorderNum()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the Num control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void Num_MouseDown(object sender, MouseEventArgs e)
        {
            if (NumClick != null)
            {
                NumClick(sender, e);
            }
            if (useCustomEvent)
                return;
            Label lbl = sender as Label;
            SendKeys.Send(lbl.Tag.ToString());
        }

        /// <summary>
        /// Handles the MouseDown event of the Backspace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void Backspace_MouseDown(object sender, MouseEventArgs e)
        {
            if (BackspaceClick != null)
            {
                BackspaceClick(sender, e);
            }
            if (useCustomEvent)
                return;
            Label lbl = sender as Label;
            SendKeys.Send("{BACKSPACE}");
        }

        /// <summary>
        /// Handles the MouseDown event of the Enter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void Enter_MouseDown(object sender, MouseEventArgs e)
        {
            if (EnterClick != null)
            {
                EnterClick(sender, e);
            }
            if (useCustomEvent)
                return;
            SendKeys.Send("{ENTER}");
        }
    }
}
