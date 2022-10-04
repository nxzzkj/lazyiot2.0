

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
using System.Drawing.Drawing2D;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class UCDropDownBtn.
    /// Implements the <see cref="Scada.Controls.Controls.UCBtnImg" />
    /// </summary>
    /// <seealso cref="Scada.Controls.Controls.UCBtnImg" />
    [DefaultEvent("BtnClick")]
    public partial class UCDropDownBtn : UCBtnImg
    {
        /// <summary>
        /// The FRM anchor
        /// </summary>
        Forms.FrmAnchor _frmAnchor;
        /// <summary>
        /// The drop panel height
        /// </summary>
        private int _dropPanelHeight = -1;
        /// <summary>
        /// 按钮点击事件
        /// </summary>
        public new event EventHandler BtnClick;
        /// <summary>
        /// 下拉框高度
        /// </summary>
        /// <value>The height of the drop panel.</value>
        [Description("下拉框高度"), Category("自定义")]
        public int DropPanelHeight
        {
            get { return _dropPanelHeight; }
            set { _dropPanelHeight = value; }
        }
        /// <summary>
        /// The BTNS
        /// </summary>
        private string[] btns;
        /// <summary>
        /// 需要显示的按钮文字
        /// </summary>
        /// <value>The BTNS.</value>
        [Description("需要显示的按钮文字"), Category("自定义")]
        public string[] Btns
        {
            get { return btns; }
            set { btns = value; }
        }
     
 
        /// <summary>
        /// 按钮字体颜色
        /// </summary>
        /// <value>The color of the BTN fore.</value>
        [Description("按钮字体颜色"), Category("自定义")]
        public override Color BtnForeColor
        {
            get
            {
                return base.BtnForeColor;
            }
            set
            {
                base.BtnForeColor = value;
                Bitmap bit = new Bitmap(12, 10);
                Graphics g = Graphics.FromImage(bit);
                g.SetGDIHigh();
                GraphicsPath path = new GraphicsPath();
                path.AddLines(new Point[] 
                {
                    new Point(1,1),
                    new Point(11,1),
                    new Point(6,10),
                    new Point(1,1)
                });
                g.FillPath(new SolidBrush(value), path);
                g.Dispose();
                this.lbl.Image = bit;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCDropDownBtn" /> class.
        /// </summary>
        public UCDropDownBtn()
        {
            InitializeComponent();
            IsShowTips = false;
            this.lbl.ImageAlign = ContentAlignment.MiddleRight;
            base.BtnClick += UCDropDownBtn_BtnClick;
        }

        /// <summary>
        /// Handles the BtnClick event of the UCDropDownBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void UCDropDownBtn_BtnClick(object sender, EventArgs e)
        {
            if (_frmAnchor == null || _frmAnchor.IsDisposed || _frmAnchor.Visible == false)
            {

                if (Btns != null && Btns.Length > 0)
                {
                    int intRow = 0;
                    int intCom = 1;
                    var p = this.PointToScreen(this.Location);
                    while (true)
                    {
                        int intScreenHeight = Screen.PrimaryScreen.Bounds.Height;
                        if ((p.Y + this.Height + Btns.Length / intCom * 50 < intScreenHeight || p.Y - Btns.Length / intCom * 50 > 0)
                            && (_dropPanelHeight <= 0 ? true : (Btns.Length / intCom * 50 <= _dropPanelHeight)))
                        {
                            intRow = Btns.Length / intCom + (Btns.Length % intCom != 0 ? 1 : 0);
                            break;
                        }
                        intCom++;
                    }
                    UCTimePanel ucTime = new UCTimePanel();
                    ucTime.IsShowBorder = true;
                    int intWidth = this.Width / intCom;

                    Size size = new Size(intCom * intWidth, intRow * 50);
                    ucTime.Size = size;
                    ucTime.FirstEvent = true;
                    ucTime.SelectSourceEvent += ucTime_SelectSourceEvent;
                    ucTime.Row = intRow;
                    ucTime.Column = intCom;

                    List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
                    foreach (var item in Btns)
                    {
                        lst.Add(new KeyValuePair<string, string>(item, item));
                    }
                    ucTime.Source = lst;

                    _frmAnchor = new Forms.FrmAnchor(this, ucTime);
                    _frmAnchor.Load += (a, b) => { (a as Form).Size = size; };

                    _frmAnchor.Show(this.FindForm());

                }
            }
            else
            {
                _frmAnchor.Close();
            }
        }
        /// <summary>
        /// Handles the SelectSourceEvent event of the ucTime control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void ucTime_SelectSourceEvent(object sender, EventArgs e)
        {
            if (_frmAnchor != null && !_frmAnchor.IsDisposed && _frmAnchor.Visible)
            {
                _frmAnchor.Close();

                if (BtnClick != null)
                {
                    BtnClick(sender.ToString(), e);
                }
            }
        }
    }
}
