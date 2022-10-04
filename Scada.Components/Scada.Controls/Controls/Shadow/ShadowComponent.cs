

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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Scada.Controls.Controls
{
    /// <summary>
    /// Class ShadowComponent.
    /// Implements the <see cref="System.ComponentModel.Component" />
    /// Implements the <see cref="System.ComponentModel.IExtenderProvider" />
    /// </summary>
    /// <seealso cref="System.ComponentModel.Component" />
    /// <seealso cref="System.ComponentModel.IExtenderProvider" />
    [ProvideProperty("ShowShadow", typeof(Control))]
    public class ShadowComponent : Component, IExtenderProvider
    {
        /// <summary>
        /// The m control cache
        /// </summary>
        Dictionary<Control, bool> m_controlCache = new Dictionary<Control, bool>();

        #region 构造函数    English:Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowComponent" /> class.
        /// </summary>
        public ShadowComponent()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowComponent" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ShadowComponent(IContainer container)
            : this()
        {
            container.Add(this);
        }
        #endregion

        /// <summary>
        /// 指定此对象是否可以将其扩展程序属性提供给指定的对象。
        /// </summary>
        /// <param name="extendee">要接收扩展程序属性的 <see cref="T:System.Object" />。</param>
        /// <returns>如果此对象可以扩展程序属性提供给指定对象，则为 true；否则为 false。</returns>
        public bool CanExtend(object extendee)
        {
            if (extendee is Control && !(extendee is Form))
                return true;
            return false;
        }

        /// <summary>
        /// Gets the show shadow.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [Browsable(true), Category("自定义属性"), Description("是否显示倒影"), DisplayName("ShowShadow"), Localizable(true)]
        public bool GetShowShadow(Control control)
        {
            if (m_controlCache.ContainsKey(control))
                return m_controlCache[control];
            else
                return false;
        }

        /// <summary>
        /// Sets the show shadow.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="isShowShadow">if set to <c>true</c> [is show shadow].</param>
        public void SetShowShadow(Control control, bool isShowShadow)
        {
            control.ParentChanged += control_ParentChanged;
            m_controlCache[control] = isShowShadow;
        }

        /// <summary>
        /// Handles the ParentChanged event of the control control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void control_ParentChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control.Parent != null && m_controlCache[control])
            {
                if (!lstPaintEventControl.Contains(control.Parent))
                {
                    lstPaintEventControl.Add(control.Parent);
                    Type type = control.Parent.GetType();
                    System.Reflection.PropertyInfo pi = type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    pi.SetValue(control.Parent, true, null);
                    control.Parent.Paint += Parent_Paint;
                }
            }
        }

        /// <summary>
        /// The LST paint event control
        /// </summary>
        List<Control> lstPaintEventControl = new List<Control>();
        /// <summary>
        /// The shadow height
        /// </summary>
        private float shadowHeight = 0.3f;

        /// <summary>
        /// Gets or sets the height of the shadow.
        /// </summary>
        /// <value>The height of the shadow.</value>
        [Browsable(true), Category("自定义属性"), Description("倒影高度，0-1"), Localizable(true)]
        public float ShadowHeight
        {
            get { return shadowHeight; }
            set { shadowHeight = value; }
        }
        /// <summary>
        /// The BLN loading
        /// </summary>
        bool blnLoading = false;
        /// <summary>
        /// Handles the Paint event of the Parent control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        void Parent_Paint(object sender, PaintEventArgs e)
        {
            if (blnLoading)
                return;           
            if (shadowHeight > 0)
            {
                var control = sender as Control;
                var lst = m_controlCache.Where(p => p.Key.Parent == control && p.Value);
                if (lst != null && lst.Count() > 0)
                {
                    blnLoading = true;
                    e.Graphics.SetGDIHigh();
                    foreach (var item in lst)
                    {
                        Control _control = item.Key;

                        using (Bitmap bit = new Bitmap(_control.Width, _control.Height))
                        {
                            _control.DrawToBitmap(bit, _control.ClientRectangle);
                            using (Bitmap bitNew = new Bitmap(bit.Width, (int)(bit.Height * shadowHeight)))
                            {
                                using (var g = Graphics.FromImage(bitNew))
                                {
                                    g.DrawImage(bit, new RectangleF(0, 0, bitNew.Width, bitNew.Height), new RectangleF(0, bit.Height - bit.Height * shadowHeight, bit.Width, bit.Height * shadowHeight), GraphicsUnit.Pixel);
                                }
                                bitNew.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                e.Graphics.DrawImage(bitNew, new Point(_control.Location.X, _control.Location.Y + _control.Height + 1));
                                Color bgColor = GetParentColor(_control);
                                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(_control.Location.X, _control.Location.Y + _control.Height + 1, bitNew.Width, bitNew.Height), Color.FromArgb(50, bgColor), bgColor, 90f);   //75f 表示角度
                                e.Graphics.FillRectangle(lgb, new Rectangle(new Point(_control.Location.X, _control.Location.Y + _control.Height + 1), bitNew.Size));
                            }
                        }
                    }
                }
            }
            blnLoading = false;
        }

        /// <summary>
        /// Gets the color of the parent.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>Color.</returns>
        private Color GetParentColor(Control c)
        {
            if (c.Parent.BackColor != Color.Transparent)
            {
                return c.Parent.BackColor;
            }
            return GetParentColor(c.Parent);
        }
    }
}
