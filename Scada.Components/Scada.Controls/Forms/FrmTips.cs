

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
    /// Class FrmTips.
    /// Implements the <see cref="Scada.Controls.Forms.FrmBase" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Forms.FrmBase" />
    public partial class FrmTips : FrmBase
    {
        /// <summary>
        /// The m show align
        /// </summary>
        private ContentAlignment m_showAlign = ContentAlignment.BottomLeft;

        /// <summary>
        /// 显示位置
        /// </summary>
        /// <value>The show align.</value>
        public ContentAlignment ShowAlign
        {
            get { return m_showAlign; }
            set { m_showAlign = value; }
        }

        /// <summary>
        /// The m LST tips
        /// </summary>
        private static List<FrmTips> m_lstTips = new List<FrmTips>();

        /// <summary>
        /// The m close time
        /// </summary>
        private int m_CloseTime = 0;

        /// <summary>
        /// Gets or sets the close time.
        /// </summary>
        /// <value>The close time.</value>
        public int CloseTime
        {
            get { return m_CloseTime; }
            set
            {
                m_CloseTime = value;
                if (value > 0)
                    timer1.Interval = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmTips" /> class.
        /// </summary>
        public FrmTips()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
        }

        #region 清理提示框
        /// <summary>
        /// 功能描述:清理提示框
        /// 作　　者:HZH
        /// 创建日期:2019-02-28 15:11:03
        /// 任务编号:POS
        /// </summary>
        public static void ClearTips()
        {
            for (int i = m_lstTips.Count - 1; i >= 0; i--)
            {
                FrmTips current = m_lstTips[i];
                if (!current.IsDisposed)
                {
                    current.Close();
                    current.Dispose();
                }
            }
            m_lstTips.Clear();
        }
        #endregion

        /// <summary>
        /// 重置倒计时
        /// </summary>
        public void ResetTimer()
        {
            if (m_CloseTime > 0)
            {
                timer1.Enabled = false;
                timer1.Enabled = true;
            }
        }
        /// <summary>
        /// The m last tips
        /// </summary>
        private static KeyValuePair<string, FrmTips> m_lastTips = new KeyValuePair<string, FrmTips>();

        /// <summary>
        /// Shows the tips.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        /// <param name="strMsg">The string MSG.</param>
        /// <param name="intAutoColseTime">The int automatic colse time.</param>
        /// <param name="blnShowCoseBtn">if set to <c>true</c> [BLN show cose BTN].</param>
        /// <param name="align">The align.</param>
        /// <param name="point">The point.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="size">The size.</param>
        /// <param name="state">The state.</param>
        /// <param name="color">The color.</param>
        /// <returns>FrmTips.</returns>
        public static FrmTips ShowTips(
            Form frm,
            string strMsg,
            int intAutoColseTime = 0,
            bool blnShowCoseBtn = true,
            ContentAlignment align = ContentAlignment.BottomLeft,
            Point? point = null,
            TipsSizeMode mode = TipsSizeMode.Small,
            Size? size = null,
            TipsState state = TipsState.Default)
        {

            if (m_lastTips.Key == strMsg + state && !m_lastTips.Value.IsDisposed && m_lastTips.Value.Visible)
            {
                m_lastTips.Value.ResetTimer();
                return m_lastTips.Value;
            }
            else
            {
                FrmTips frmTips = new FrmTips();
                switch (mode)
                {
                    case TipsSizeMode.Small:
                        frmTips.Size = new Size(350, 35);
                        break;
                    case TipsSizeMode.Medium:
                        frmTips.Size = new Size(350, 50);
                        break;
                    case TipsSizeMode.Large:
                        frmTips.Size = new Size(350, 65);
                        break;
                    case TipsSizeMode.None:
                        if (!size.HasValue)
                        {
                            frmTips.Size = new Size(350, 35);
                        }
                        else
                        {
                            frmTips.Size = size.Value;
                        }
                        break;
                }

                frmTips.BackColor = Color.FromArgb((int)state);

                if (state == TipsState.Default)
                {
                    frmTips.lblMsg.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    frmTips.lblMsg.ForeColor = Color.White;
                }
                switch (state)
                {
                    case TipsState.Default:
                        frmTips.pctStat.Image = Scada.Controls.Properties.Resources.alarm;
                        break;
                    case TipsState.Success:
                        frmTips.pctStat.Image = Scada.Controls.Properties.Resources.success;
                        break;
                    case TipsState.Info:
                        frmTips.pctStat.Image = Scada.Controls.Properties.Resources.alarm;
                        break;
                    case TipsState.Warning:
                        frmTips.pctStat.Image = Scada.Controls.Properties.Resources.warning;
                        break;
                    case TipsState.Error:
                        frmTips.pctStat.Image = Scada.Controls.Properties.Resources.error;
                        break;
                    default:
                        frmTips.pctStat.Image = Scada.Controls.Properties.Resources.alarm;
                        break;
                }

                frmTips.lblMsg.Text = strMsg;
                frmTips.CloseTime = intAutoColseTime;
                frmTips.btnClose.Visible = blnShowCoseBtn;


                frmTips.ShowAlign = align;
                frmTips.Owner = frm;
                FrmTips.m_lstTips.Add(frmTips);
                FrmTips.ReshowTips();
                frmTips.Show(frm);
                if (frm != null && !frm.IsDisposed)
                {
                    ControlHelper.SetForegroundWindow(frm.Handle);
                }
                //frmTips.BringToFront();
                m_lastTips = new KeyValuePair<string, FrmTips>(strMsg + state, frmTips);
                return frmTips;
            }
        }

        #region 刷新显示
        /// <summary>
        /// 功能描述:刷新显示
        /// 作　　者:HZH
        /// 创建日期:2019-02-28 15:33:06
        /// 任务编号:POS
        /// </summary>
        public static void ReshowTips()
        {
            lock (FrmTips.m_lstTips)
            {
                FrmTips.m_lstTips.RemoveAll(p => p.IsDisposed == true);
                var enumerable = from p in FrmTips.m_lstTips
                                 group p by new
                                 {
                                     p.ShowAlign
                                 };
                Size size = Screen.PrimaryScreen.Bounds.Size;
                foreach (var item in enumerable)
                {
                    List<FrmTips> list = FrmTips.m_lstTips.FindAll((FrmTips p) => p.ShowAlign == item.Key.ShowAlign);
                    for (int i = 0; i < list.Count; i++)
                    {
                        FrmTips frmTips = list[i];
                        if (frmTips.InvokeRequired)
                        {
                            frmTips.BeginInvoke(new MethodInvoker(delegate()
                            {
                                switch (item.Key.ShowAlign)
                                {
                                    case ContentAlignment.BottomCenter:
                                        frmTips.Location = new Point((size.Width - frmTips.Width) / 2, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.BottomLeft:
                                        frmTips.Location = new Point(10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.BottomRight:
                                        frmTips.Location = new Point(size.Width - frmTips.Width - 10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.MiddleCenter:
                                        frmTips.Location = new Point((size.Width - frmTips.Width) / 2, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.MiddleLeft:
                                        frmTips.Location = new Point(10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.MiddleRight:
                                        frmTips.Location = new Point(size.Width - frmTips.Width - 10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.TopCenter:
                                        frmTips.Location = new Point((size.Width - frmTips.Width) / 2, 10 + (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.TopLeft:
                                        frmTips.Location = new Point(10, 10 + (i + 1) * (frmTips.Height + 10));
                                        break;
                                    case ContentAlignment.TopRight:
                                        frmTips.Location = new Point(size.Width - frmTips.Width - 10, 10 + (i + 1) * (frmTips.Height + 10));
                                        break;
                                    default:
                                        break;
                                }
                            }));
                        }
                        else
                        {
                            switch (item.Key.ShowAlign)
                            {
                                case ContentAlignment.BottomCenter:
                                    frmTips.Location = new Point((size.Width - frmTips.Width) / 2, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.BottomLeft:
                                    frmTips.Location = new Point(10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.BottomRight:
                                    frmTips.Location = new Point(size.Width - frmTips.Width - 10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.MiddleCenter:
                                    frmTips.Location = new Point((size.Width - frmTips.Width) / 2, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.MiddleLeft:
                                    frmTips.Location = new Point(10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.MiddleRight:
                                    frmTips.Location = new Point(size.Width - frmTips.Width - 10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.TopCenter:
                                    frmTips.Location = new Point((size.Width - frmTips.Width) / 2, 10 + (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.TopLeft:
                                    frmTips.Location = new Point(10, 10 + (i + 1) * (frmTips.Height + 10));
                                    break;
                                case ContentAlignment.TopRight:
                                    frmTips.Location = new Point(size.Width - frmTips.Width - 10, 10 + (i + 1) * (frmTips.Height + 10));
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Handles the FormClosing event of the FrmTips control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs" /> instance containing the event data.</param>
        private void FrmTips_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_lastTips.Value == this)
                m_lastTips = new KeyValuePair<string, FrmTips>();
            m_lstTips.Remove(this);
            ReshowTips();

            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].IsDisposed || !Application.OpenForms[i].Visible || Application.OpenForms[i] is FrmTips)
                {
                    continue;
                }
                else
                {
                    Timer t = new Timer();
                    t.Interval = 100;
                    var frm = Application.OpenForms[i];
                    t.Tick += (a, b) =>
                    {
                        t.Enabled = false;
                        if (!frm.IsDisposed)
                            ControlHelper.SetForegroundWindow(frm.Handle);
                    };
                    t.Enabled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the FrmTips control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmTips_Load(object sender, EventArgs e)
        {
            if (m_CloseTime > 0)
            {
                this.timer1.Interval = m_CloseTime;
                this.timer1.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the Tick event of the timer1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            this.Close();
        }

        /// <summary>
        /// Handles the MouseDown event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            this.timer1.Enabled = false;
            this.Close();
        }

        /// <summary>
        /// Shows the tips success.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        /// <param name="strMsg">The string MSG.</param>
        /// <returns>FrmTips.</returns>
        public static FrmTips ShowTipsSuccess(Form frm, string strMsg)
        {
            return FrmTips.ShowTips(frm, strMsg, 3000, false, ContentAlignment.BottomCenter, null, TipsSizeMode.Large, null, TipsState.Success);
        }

        /// <summary>
        /// Shows the tips error.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        /// <param name="strMsg">The string MSG.</param>
        /// <returns>FrmTips.</returns>
        public static FrmTips ShowTipsError(Form frm, string strMsg)
        {
            return FrmTips.ShowTips(frm, strMsg, 3000, false, ContentAlignment.BottomCenter, null, TipsSizeMode.Large, null, TipsState.Error);
        }

        /// <summary>
        /// Shows the tips information.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        /// <param name="strMsg">The string MSG.</param>
        /// <returns>FrmTips.</returns>
        public static FrmTips ShowTipsInfo(Form frm, string strMsg)
        {
            return FrmTips.ShowTips(frm, strMsg, 3000, false, ContentAlignment.BottomCenter, null, TipsSizeMode.Large, null, TipsState.Info);
        }
        /// <summary>
        /// Shows the tips warning.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        /// <param name="strMsg">The string MSG.</param>
        /// <returns>FrmTips.</returns>
        public static FrmTips ShowTipsWarning(Form frm, string strMsg)
        {
            return FrmTips.ShowTips(frm, strMsg, 3000, false, ContentAlignment.BottomCenter, null, TipsSizeMode.Large, null, TipsState.Warning);
        }

        /// <summary>
        /// Handles the FormClosed event of the FrmTips control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs" /> instance containing the event data.</param>
        private void FrmTips_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }

    }

    /// <summary>
    /// Enum TipsSizeMode
    /// </summary>
    public enum TipsSizeMode
    {
        /// <summary>
        /// The small
        /// </summary>
        Small,
        /// <summary>
        /// The medium
        /// </summary>
        Medium,
        /// <summary>
        /// The large
        /// </summary>
        Large,
        /// <summary>
        /// The none
        /// </summary>
        None
    }
    /// <summary>
    /// Enum TipsState
    /// </summary>
    public enum TipsState
    {
        Default = -12542209,
        Success = -9977286,
        Info = -7299687,
        Warning = -693140,
        Error = -1097849
    }
}
