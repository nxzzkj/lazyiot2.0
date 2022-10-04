

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
    /// Class UCCheckBox.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    [DefaultEvent("CheckedChangeEvent")]
    public partial class UCCheckBox : UserControl
    {
        /// <summary>
        /// 选中改变事件
        /// </summary>
        [Description("选中改变事件"), Category("自定义")]
        public event EventHandler CheckedChangeEvent;
        /// <summary>
        /// 字体
        /// </summary>
        /// <value>The font.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Description("字体"), Category("自定义")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                label1.Font = value;
            }
        }

        /// <summary>
        /// The fore color
        /// </summary>
        private Color _ForeColor = Color.FromArgb(62, 62, 62);
        /// <summary>
        /// 字体颜色
        /// </summary>
        /// <value>The color of the fore.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Description("字体颜色"), Category("自定义")]
        public new Color ForeColor
        {
            get { return _ForeColor; }
            set
            {
                base.ForeColor = value;
                label1.ForeColor = value;
                _ForeColor = value;
            }
        }
        /// <summary>
        /// The text
        /// </summary>
        private string _Text = "复选框";
        /// <summary>
        /// 文本
        /// </summary>
        /// <value>The text value.</value>
        [Description("文本"), Category("自定义")]
        public string TextValue
        {
            get { return _Text; }
            set
            {
                label1.Text = value;
                _Text = value;
            }
        }
        /// <summary>
        /// The checked
        /// </summary>
        private bool _checked = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        [Description("是否选中"), Category("自定义")]
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    if (base.Enabled)
                    {
                        if (_checked)
                        {
                            panel1.BackgroundImage = Properties.Resources.checkbox1;
                        }
                        else
                        {
                            panel1.BackgroundImage = Properties.Resources.checkbox0;
                        }
                    }
                    else
                    {
                        if (_checked)
                        {
                            panel1.BackgroundImage = Properties.Resources.checkbox10;
                        }
                        else
                        {
                            panel1.BackgroundImage = Properties.Resources.checkbox00;
                        }
                    }

                    if (CheckedChangeEvent != null)
                    {
                        CheckedChangeEvent(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示控件是否可以对用户交互作出响应。
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                if (value)
                {
                    if (_checked)
                    {
                        panel1.BackgroundImage = Properties.Resources.checkbox1;
                    }
                    else
                    {
                        panel1.BackgroundImage = Properties.Resources.checkbox0;
                    }
                }
                else
                {
                    if (_checked)
                    {
                        panel1.BackgroundImage = Properties.Resources.checkbox10;
                    }
                    else
                    {
                        panel1.BackgroundImage = Properties.Resources.checkbox00;
                    }
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCCheckBox" /> class.
        /// </summary>
        public UCCheckBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void CheckBox_MouseDown(object sender, MouseEventArgs e)
        {
            Checked = !Checked;
        }
    }
}
