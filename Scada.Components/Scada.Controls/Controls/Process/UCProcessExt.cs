

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
    /// Class UCProcessExt.
    /// Implements the <see cref="Scada.Controls.Controls.UCControlBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCControlBase" />
    public partial class UCProcessExt : UCControlBase
    {
        /// <summary>
        /// The value
        /// </summary>
        private int _value = 0;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return this._value; }
            set
            {
                if (value < 0)
                    return;
                this._value = value;
                SetValue();
            }
        }

        /// <summary>
        /// The maximum value
        /// </summary>
        private int maxValue = 100;

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value <= 0)
                    return;
                maxValue = value;
                SetValue();
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        private void SetValue()
        {
            double dbl = (double)_value / (double)maxValue;
            this.panel1.Width = (int)(this.Width * dbl);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCProcessExt" /> class.
        /// </summary>
        public UCProcessExt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the SizeChanged event of the ProcessExt control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void ProcessExt_SizeChanged(object sender, EventArgs e)
        {
            SetValue();
        }

        /// <summary>
        /// Steps this instance.
        /// </summary>
        public void Step()
        {
            Value++;
        }
    }
}
