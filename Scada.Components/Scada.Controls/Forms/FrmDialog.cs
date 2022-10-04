

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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Controls.Forms
{
    /// <summary>
    /// Class FrmDialog.
    /// Implements the <see cref="Scada.Controls.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Forms.FrmBase" />
    public partial class FrmDialog : FrmBase
    {
        /// <summary>
        /// The BLN enter close
        /// </summary>
        bool blnEnterClose = true;
        /// <summary>
        /// Initializes a new instance of the <see cref="FrmDialog" /> class.
        /// </summary>
        /// <param name="strMessage">The string message.</param>
        /// <param name="strTitle">The string title.</param>
        /// <param name="blnShowCancel">if set to <c>true</c> [BLN show cancel].</param>
        /// <param name="blnShowClose">if set to <c>true</c> [BLN show close].</param>
        /// <param name="blnisEnterClose">if set to <c>true</c> [blnis enter close].</param>
        private FrmDialog(
            string strMessage,
            string strTitle,
            bool blnShowCancel = false,
            bool blnShowClose = false,
            bool blnisEnterClose = true)
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            InitFormMove(this.lblTitle);
            if (!string.IsNullOrWhiteSpace(strTitle))
                lblTitle.Text = strTitle;
            lblMsg.Text = strMessage;
            if (blnShowCancel)
            {
                this.tableLayoutPanel1.ColumnStyles[1].Width = 1;
                this.tableLayoutPanel1.ColumnStyles[2].Width = 50;
            }
            else
            {
                this.tableLayoutPanel1.ColumnStyles[1].Width = 0;
                this.tableLayoutPanel1.ColumnStyles[2].Width = 0;
            }
            //btnCancel.Visible = blnShowCancel;
            //ucSplitLine_V1.Visible = blnShowCancel;
            btnClose.Visible = blnShowClose;
            blnEnterClose = blnisEnterClose;
            //if (blnShowCancel)
            //{
            //    btnOK.BtnForeColor = Color.FromArgb(255, 85, 51);
            //}
        }

        #region 显示一个模式信息框
        /// <summary>
        /// 功能描述:显示一个模式信息框
        /// 作　　者:HZH
        /// 创建日期:2019-03-04 15:49:48
        /// 任务编号:POS
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="strMessage">strMessage</param>
        /// <param name="strTitle">strTitle</param>
        /// <param name="blnShowCancel">blnShowCancel</param>
        /// <param name="isShowMaskDialog">isShowMaskDialog</param>
        /// <param name="blnShowClose">blnShowClose</param>
        /// <param name="blnIsEnterClose">if set to <c>true</c> [BLN is enter close].</param>
        /// <param name="deviationSize">大小偏移，当默认大小过大或过小时，可以进行调整（增量）</param>
        /// <returns>返回值</returns>
        public static DialogResult ShowDialog(
            IWin32Window owner,
            string strMessage,
            string strTitle = "提示",
            bool blnShowCancel = false,
            bool isShowMaskDialog = true,
            bool blnShowClose = false,
            bool blnIsEnterClose = true,
            Size? deviationSize = null)
        {
            DialogResult result = DialogResult.Cancel;
            if (owner == null || (owner is Control && (owner as Control).IsDisposed))
            {
                var frm = new FrmDialog(strMessage, strTitle, blnShowCancel, blnShowClose, blnIsEnterClose)
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    IsShowMaskDialog = isShowMaskDialog,
                    TopMost = true
                };
                if (deviationSize != null)
                {
                    frm.Width += deviationSize.Value.Width;
                    frm.Height += deviationSize.Value.Height;
                }
                result = frm.ShowDialog();
            }
            else
            {
                if (owner is Control)
                {
                    owner = (owner as Control).FindForm();
                }
                var frm = new FrmDialog(strMessage, strTitle, blnShowCancel, blnShowClose, blnIsEnterClose)
                {
                    StartPosition = (owner != null) ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen,
                    IsShowMaskDialog = isShowMaskDialog,
                    TopMost = true
                };
                if (deviationSize != null)
                {
                    frm.Width += deviationSize.Value.Width;
                    frm.Height += deviationSize.Value.Height;
                }
                result = frm.ShowDialog(owner);
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Handles the BtnClick event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnOK_BtnClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Handles the BtnClick event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnCancel_BtnClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// Handles the MouseDown event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// Does the enter.
        /// </summary>
        protected override void DoEnter()
        {
            if (blnEnterClose)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Handles the VisibleChanged event of the FrmDialog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmDialog_VisibleChanged(object sender, EventArgs e)
        {
            
        }
    }
}
