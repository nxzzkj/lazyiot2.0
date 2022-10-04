

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
    /// Class UCLEDDataTime.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCLEDDataTime : UserControl
    {
        /// <summary>
        /// The m value
        /// </summary>
        private DateTime m_value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Description("值"), Category("自定义")]
        public DateTime Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                string str = value.ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 0; i < str.Length; i++)
                {
                    if (i == 10)
                        continue;
                    ((UCLEDNum)this.tableLayoutPanel1.Controls.Find("D" + (i + 1), false)[0]).Value = str[i];
                }
            }
        }

        /// <summary>
        /// The m line width
        /// </summary>
        private int m_lineWidth = 8;

        /// <summary>
        /// Gets or sets the width of the line.
        /// </summary>
        /// <value>The width of the line.</value>
        [Description("线宽度，为了更好的显示效果，请使用偶数"), Category("自定义")]
        public int LineWidth
        {
            get { return m_lineWidth; }
            set
            {
                m_lineWidth = value;
                foreach (UCLEDNum c in this.tableLayoutPanel1.Controls)
                {
                    c.LineWidth = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置控件的前景色。
        /// </summary>
        /// <value>The color of the fore.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Description("颜色"), Category("自定义")]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                foreach (UCLEDNum c in this.tableLayoutPanel1.Controls)
                {
                    c.ForeColor = value;
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCLEDDataTime" /> class.
        /// </summary>
        public UCLEDDataTime()
        {
            InitializeComponent();
            Value = DateTime.Now;
        }
    }
}
