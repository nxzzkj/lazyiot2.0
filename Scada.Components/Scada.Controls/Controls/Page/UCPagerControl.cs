

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
    /// Class UCPagerControl.
    /// Implements the <see cref="Scada.Controls.Controls.UCPagerControlBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCPagerControlBase" />
    [ToolboxItem(true)]
    public partial class UCPagerControl : UCPagerControlBase
    {
        public override int PageCount
        {
            get
            {
                return base.PageCount;
            }
            set
            {
                base.PageCount = value;
                if (PageCount <= 1)
                {
                    ShowBtn(false, false);
                }
                else
                {
                    ShowBtn(false, PageCount > 1);
                }
            }
        }

        public override int PageIndex
        {
            get
            {
                return base.PageIndex;
            }
            set
            {
                base.PageIndex = value;
                if (PageCount <= 1)
                {
                    ShowBtn(false, false);
                }
                else
                {
                    ShowBtn(false, PageCount > 1);
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UCPagerControl" /> class.
        /// </summary>
        public UCPagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            PreviousPage();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            NextPage();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            FirstPage();
        }

        /// <summary>
        /// Handles the MouseDown event of the panel4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            EndPage();
        }

        /// <summary>
        /// Shows the BTN.
        /// </summary>
        /// <param name="blnLeftBtn">if set to <c>true</c> [BLN left BTN].</param>
        /// <param name="blnRightBtn">if set to <c>true</c> [BLN right BTN].</param>
        protected override void ShowBtn(bool blnLeftBtn, bool blnRightBtn)
        {
            panel1.Visible = panel3.Visible = blnLeftBtn;
            panel2.Visible = panel4.Visible = blnRightBtn;
        }

       
    }
}
