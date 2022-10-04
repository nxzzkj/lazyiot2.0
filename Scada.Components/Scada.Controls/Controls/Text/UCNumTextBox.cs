

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
    /// Class UCNumTextBox.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [DefaultEvent("NumChanged")]
    public partial class UCNumTextBox : UserControl
    {
        /// <summary>
        /// Occurs when [show key border event].
        /// </summary>
        [Description("弹出输入键盘时发生"), Category("自定义")]
        public event EventHandler ShowKeyBorderEvent;
        /// <summary>
        /// Occurs when [hide key border event].
        /// </summary>
        [Description("关闭输入键盘时发生"), Category("自定义")]
        public event EventHandler HideKeyBorderEvent;
        /// <summary>
        /// Occurs when [number changed].
        /// </summary>
        [Description("数字改变时发生"), Category("自定义")]
        public event EventHandler NumChanged;
        /// <summary>
        /// 输入类型
        /// </summary>
        /// <value>The type of the input.</value>
        [Description("输入类型"), Category("自定义")]
        public TextInputType InputType
        {
            get
            {
                return txtNum.InputType;
            }
            set
            {
                if (value == TextInputType.NotControl)
                {
                    return;
                }
                txtNum.InputType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is number can input.
        /// </summary>
        /// <value><c>true</c> if this instance is number can input; otherwise, <c>false</c>.</value>
        [Description("数字是否可手动编辑"), Category("自定义")]
        public bool IsNumCanInput
        {
            get
            {
                return txtNum.Enabled;
            }
            set
            {
                txtNum.Enabled = value;
            }
        }
        /// <summary>
        /// 当InputType为数字类型时，能输入的最大值
        /// </summary>
        /// <value>The maximum value.</value>
        [Description("当InputType为数字类型时，能输入的最大值。")]
        public decimal MaxValue
        {
            get
            {
                return this.txtNum.MaxValue;
            }
            set
            {
                this.txtNum.MaxValue = value;
            }
        }
        /// <summary>
        /// 当InputType为数字类型时，能输入的最小值
        /// </summary>
        /// <value>The minimum value.</value>
        [Description("当InputType为数字类型时，能输入的最小值。")]
        public decimal MinValue
        {
            get
            {
                return this.txtNum.MinValue;
            }
            set
            {
                this.txtNum.MinValue = value;
            }
        }
        /// <summary>
        /// The key board type
        /// </summary>
        private KeyBoardType keyBoardType = KeyBoardType.UCKeyBorderNum;
        /// <summary>
        /// Gets or sets the type of the key board.
        /// </summary>
        /// <value>The type of the key board.</value>
        [Description("键盘样式"), Category("自定义")]
        public KeyBoardType KeyBoardType
        {
            get { return keyBoardType; }
            set { keyBoardType = value; }
        }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        [Description("数值"), Category("自定义")]
        public decimal Num
        {
            get { return txtNum.Text.ToDecimal(); }
            set { txtNum.Text = value.ToString(); }
        }
        /// <summary>
        /// 获取或设置控件显示的文字的字体。
        /// </summary>
        /// <value>The font.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Description("字体"), Category("自定义")]
        public new Font Font
        {
            get
            {
                return txtNum.Font;
            }
            set
            {
                txtNum.Font = value;
            }
        }

        /// <summary>
        /// Occurs when [add click].
        /// </summary>
        [Description("增加按钮点击事件"), Category("自定义")]
        public event EventHandler AddClick;
        /// <summary>
        /// Occurs when [minus click].
        /// </summary>
        [Description("减少按钮点击事件"), Category("自定义")]
        public event EventHandler MinusClick;

        private decimal increment = 1;
        [Description("递增量，大于0的数值"), Category("自定义")]
        public decimal Increment
        {
            get { return increment; }
            set
            {
                if (value <= 0)
                    return;
                increment = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCNumTextBox" /> class.
        /// </summary>
        public UCNumTextBox()
        {
            InitializeComponent();
            txtNum.TextChanged += txtNum_TextChanged;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtNum control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void txtNum_TextChanged(object sender, EventArgs e)
        {
            if (NumChanged != null)
            {
                NumChanged(txtNum.Text.ToString(), e);
            }
        }
        /// <summary>
        /// The m FRM anchor
        /// </summary>
        Forms.FrmAnchor m_frmAnchor;
        /// <summary>
        /// Handles the MouseDown event of the txtNum control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void txtNum_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsNumCanInput)
            {
                if (KeyBoardType != Scada.Controls.Controls.KeyBoardType.Null)
                {
                    switch (keyBoardType)
                    {
                        case KeyBoardType.UCKeyBorderAll_EN:

                            UCKeyBorderAll keyAll = new UCKeyBorderAll();
                            keyAll.RetractClike += (a, b) => { m_frmAnchor.Hide(); };
                            keyAll.EnterClick += (a, b) => { m_frmAnchor.Hide(); };
                            m_frmAnchor = new Forms.FrmAnchor(this, keyAll);
                            m_frmAnchor.VisibleChanged += m_frmAnchor_VisibleChanged;

                            m_frmAnchor.Show(this.FindForm());
                            break;
                        case KeyBoardType.UCKeyBorderNum:

                            UCKeyBorderNum keyNum = new UCKeyBorderNum();
                            keyNum.EnterClick += (a, b) => { m_frmAnchor.Hide(); };
                            m_frmAnchor = new Forms.FrmAnchor(this, keyNum);
                            m_frmAnchor.VisibleChanged += m_frmAnchor_VisibleChanged;
                            m_frmAnchor.Show(this.FindForm());
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the VisibleChanged event of the m_frmAnchor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void m_frmAnchor_VisibleChanged(object sender, EventArgs e)
        {
            if (m_frmAnchor.Visible)
            {
                if (ShowKeyBorderEvent != null)
                {
                    ShowKeyBorderEvent(this, null);
                }
            }
            else
            {
                if (HideKeyBorderEvent != null)
                {
                    HideKeyBorderEvent(this, null);
                }
            }
        }

        /// <summary>
        /// Numbers the add click.
        /// </summary>
        public void NumAddClick()
        {
            btnAdd_MouseDown(null, null);
        }

        /// <summary>
        /// Numbers the minus click.
        /// </summary>
        public void NumMinusClick()
        {
            btnMinus_MouseDown(null, null);
        }

        /// <summary>
        /// Handles the MouseDown event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void btnAdd_MouseDown(object sender, MouseEventArgs e)
        {
            if (AddClick != null)
            {
                AddClick(this, e);
            }
            decimal dec = this.txtNum.Text.ToDecimal();
            dec += increment;
            txtNum.Text = dec.ToString();

        }

        /// <summary>
        /// Handles the MouseDown event of the btnMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void btnMinus_MouseDown(object sender, MouseEventArgs e)
        {
            if (MinusClick != null)
            {
                MinusClick(this, e);
            }
            decimal dec = this.txtNum.Text.ToDecimal();
            dec -= increment; ;
            txtNum.Text = dec.ToString();
        }

        /// <summary>
        /// Handles the Load event of the UCNumTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UCNumTextBox_Load(object sender, EventArgs e)
        {
            this.txtNum.BackColor = this.BackColor;
        }

        /// <summary>
        /// Handles the FontChanged event of the txtNum control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void txtNum_FontChanged(object sender, EventArgs e)
        {
            txtNum.Location = new Point(txtNum.Location.X, (this.Height - txtNum.Height) / 2);
        }

        /// <summary>
        /// Handles the BackColorChanged event of the UCNumTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UCNumTextBox_BackColorChanged(object sender, EventArgs e)
        {
            Color c = this.BackColor;
            Control control = this;
            while (c == Color.Transparent)
            {
                control = control.Parent;
                if (control == null)
                    break;
                c = control.BackColor;
            }
            if (c == Color.Transparent)
                return;
            txtNum.BackColor = c;
        }
    }
}
