

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
    /// Class UCProcessLineExt.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class UCProcessLineExt : UserControl
    {
        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        [Description("值变更事件"), Category("自定义")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Description("当前属性"), Category("自定义")]
        public int Value
        {
            set
            {
                ucProcessLine1.Value = value;
                Refresh();
            }
            get
            {
                return ucProcessLine1.Value;
            }
        }



        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        [Description("最大值"), Category("自定义")]
        public int MaxValue
        {
            get { return ucProcessLine1.MaxValue; }
            set
            {
                ucProcessLine1.MaxValue = value;
                Refresh();
            }
        }


        /// <summary>
        /// Gets or sets the color of the value.
        /// </summary>
        /// <value>The color of the value.</value>
        [Description("值进度条颜色"), Category("自定义")]
        public Color ValueColor
        {
            get { return ucProcessLine1.ValueColor; }
            set
            {
                ucProcessLine1.ValueColor = value;
                Refresh();
            }
        }


        /// <summary>
        /// Gets or sets the color of the value bg.
        /// </summary>
        /// <value>The color of the value bg.</value>
        [Description("值背景色"), Category("自定义")]
        public Color ValueBGColor
        {
            get { return ucProcessLine1.ValueBGColor; }
            set
            {
                ucProcessLine1.ValueBGColor = value;
                Refresh();
            }
        }


        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        [Description("边框颜色"), Category("自定义")]
        public Color BorderColor
        {
            get { return ucProcessLine1.BorderColor; }
            set
            {
                ucProcessLine1.BorderColor = value;
                Refresh();
            }
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
        [Description("值字体"), Category("自定义")]
        public override Font Font
        {
            get
            {
                return ucProcessLine1.Font;
            }
            set
            {
                ucProcessLine1.Font = value;
                Refresh();
            }
        }

        /// <summary>
        /// 获取或设置控件的前景色。
        /// </summary>
        /// <value>The color of the fore.</value>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [Description("值块颜色"), Category("自定义")]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UCProcessLineExt" /> class.
        /// </summary>
        public UCProcessLineExt()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            ucProcessLine1.ValueChanged += ucProcessLine1_ValueChanged;
        }

        /// <summary>
        /// Handles the ValueChanged event of the ucProcessLine1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        void ucProcessLine1_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SetGDIHigh();
            float fltIndex = (float)this.ucProcessLine1.Value / (float)this.ucProcessLine1.MaxValue;

            int x = (int)(fltIndex * this.ucProcessLine1.Width + this.ucProcessLine1.Location.X - 15) - 2;
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(x, 1, 30, 20);
            int cornerRadius = 2;
            path.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            path.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            path.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            path.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            path.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            path.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.Right - cornerRadius * 2 - 5, rect.Bottom);//下
            path.AddLine(rect.Right - cornerRadius * 2 - 5, 21, x + 15, ucProcessLine1.Location.Y);
            path.AddLine(x + 15, ucProcessLine1.Location.Y, rect.X + cornerRadius * 2 + 5, 21);
            path.AddLine(rect.X + cornerRadius * 2 + 5, 20, rect.X + cornerRadius * 2, rect.Bottom);//下
            path.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            path.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);//上
            path.CloseFigure();

            e.Graphics.FillPath(new SolidBrush(ForeColor), path);

            string strValue = ((float)Value / (float)MaxValue).ToString("0%");
            System.Drawing.SizeF sizeF = e.Graphics.MeasureString(strValue, Font);
            e.Graphics.DrawString(strValue, Font, new SolidBrush(Color.White), new PointF(x + (30 - sizeF.Width) / 2 + 1, (20 - sizeF.Height) / 2 + 1));
        }
    }
}
