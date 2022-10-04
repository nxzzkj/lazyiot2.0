

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
    /// Class UCKeyBorderAll.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [DefaultEvent("KeyDown")]
    public partial class UCKeyBorderAll : UserControl
    {
        /// <summary>
        /// The character type
        /// </summary>
        private KeyBorderCharType _charType = KeyBorderCharType.CHAR;

        /// <summary>
        /// Gets or sets the type of the character.
        /// </summary>
        /// <value>The type of the character.</value>
        [Description("默认显示样式"), Category("自定义")]
        public KeyBorderCharType CharType
        {
            get { return _charType; }
            set
            {
                _charType = value;
                if (value == KeyBorderCharType.CHAR)
                {
                    if (label37.Text.ToLower() == "abc.")
                    {
                        CharOrNum();
                    }
                }
                else
                {
                    if (label37.Text.ToLower() == "?123")
                    {
                        CharOrNum();
                    }
                }
            }
        }
        /// <summary>
        /// Occurs when [key click].
        /// </summary>
        [Description("按键点击事件"), Category("自定义")]
        public event EventHandler KeyClick;
        /// <summary>
        /// Occurs when [enter click].
        /// </summary>
        [Description("回车点击事件"), Category("自定义")]
        public event EventHandler EnterClick;
        /// <summary>
        /// Occurs when [backspace clike].
        /// </summary>
        [Description("删除点击事件"), Category("自定义")]
        public event EventHandler BackspaceClike;
        /// <summary>
        /// Occurs when [retract clike].
        /// </summary>
        [Description("收起点击事件"), Category("自定义")]
        public event EventHandler RetractClike;
        /// <summary>
        /// Initializes a new instance of the <see cref="UCKeyBorderAll" /> class.
        /// </summary>
        public UCKeyBorderAll()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the KeyDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void KeyDown_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = sender as Label;
            if (string.IsNullOrEmpty(lbl.Text))
            {
                return;
            }
            if (lbl.Text == "大写")
            {
                ToUper(true);
                lbl.Text = "小写";
            }
            else if (lbl.Text == "小写")
            {
                ToUper(false);
                lbl.Text = "大写";
            }
            else if (lbl.Text == "?123" || lbl.Text == "abc.")
            {
                CharOrNum();
            }
            else if (lbl.Text == "空格")
            {
                SendKeys.Send(" ");
            }
            else if (lbl.Text == "删除")
            {
                SendKeys.Send("{BACKSPACE}");
                if (BackspaceClike != null)
                    BackspaceClike(sender, e);
            }
            else if (lbl.Text == "回车")
            {
                SendKeys.Send("{ENTER}");
                if (EnterClick != null)
                    EnterClick(sender, e);
            }
            else if (lbl.Text == "收起")
            {
                if (RetractClike != null)
                    RetractClike(sender, e);
            }
            else
            {
                string Str = "{"+ lbl.Text + "}";
                SendKeys.Send(lbl.Text);
            }
            if (KeyClick != null)
                KeyClick(sender, e);
        }

        /// <summary>
        /// Converts to uper.
        /// </summary>
        /// <param name="bln">if set to <c>true</c> [BLN].</param>
        private void ToUper(bool bln)
        {
            foreach (Control item in this.tableLayoutPanel2.Controls)
            {
                if (item is Panel)
                {
                    foreach (Control pc in item.Controls)
                    {
                        if (pc is Label)
                        {
                            if (pc.Text == "abc.")
                                break;
                            if (bln)
                            {
                                pc.Text = pc.Text.ToUpper();
                            }
                            else
                            {
                                pc.Text = pc.Text.ToLower();
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Characters the or number.
        /// </summary>
        private void CharOrNum()
        {
            foreach (Control item in this.tableLayoutPanel2.Controls)
            {
                if (item is Panel)
                {
                    foreach (Control pc in item.Controls)
                    {
                        if (pc is Label)
                        {
                            string strTag = pc.Text;
                            pc.Text = pc.Tag.ToString();
                            pc.Tag = strTag;
                            break;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Enum KeyBorderCharType
    /// </summary>
    public enum KeyBorderCharType
    {
        /// <summary>
        /// The character
        /// </summary>
        CHAR = 1,
        /// <summary>
        /// The number
        /// </summary>
        NUMBER = 2
    }
}
