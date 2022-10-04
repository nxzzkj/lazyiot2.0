using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


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
namespace Scada.Controls.Controls
{
    [DefaultEvent("Paint")]
    public partial class GraphicalOverlayComponent : Component
    {
        public event EventHandler<PaintEventArgs> Paint;
        
        public GraphicalOverlayComponent()
        {
            InitializeComponent();
        }

        public GraphicalOverlayComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        private Control owner;
        [Browsable(true), Category("自定义属性"), Description("父控件"), Localizable(true)]    
        public Control Owner
        {
            get { return owner; }
            set
            {
                // The owner form cannot be set to null.
                if (value == null)
                    return;

                // The owner form can only be set once.
                if (owner != null)
                    return;

                // Save the form for future reference.
                owner = value;

                // Handle the form's Resize event.
                owner.Resize += new EventHandler(Form_Resize);

                // Handle the Paint event for each of the controls in the form's hierarchy.
                ConnectPaintEventHandlers(owner);
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            owner.Invalidate(true);
        }

        private void ConnectPaintEventHandlers(Control control)
        {

            Type type = control.GetType();
            System.Reflection.PropertyInfo pi = type.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi.SetValue(control, true, null);
           
            
            control.Paint -= new PaintEventHandler(Control_Paint);
            control.Paint += new PaintEventHandler(Control_Paint);

            control.ControlAdded -= new ControlEventHandler(Control_ControlAdded);
            control.ControlAdded += new ControlEventHandler(Control_ControlAdded);

            // Recurse the hierarchy.
            foreach (Control child in control.Controls)
                ConnectPaintEventHandlers(child);
        }

        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            // Connect the paint event handler for the new control.
            ConnectPaintEventHandlers(e.Control);
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            // As each control on the form is repainted, this handler is called.

            Control control = sender as Control;
            Point location;

            // Determine the location of the control's client area relative to the form's client area.
            if (control == owner)
                // The form's client area is already form-relative.
                location = control.Location;
            else
            {
                // The control may be in a hierarchy, so convert to screen coordinates and then back to form coordinates.
                location = owner.PointToClient(control.Parent.PointToScreen(control.Location));

                // If the control has a border shift the location of the control's client area.
                location += new Size((control.Width - control.ClientSize.Width) / 2, (control.Height - control.ClientSize.Height) / 2);
            }

            // Translate the location so that we can use form-relative coordinates to draw on the control.
            if (control != owner)
                e.Graphics.TranslateTransform(-location.X, -location.Y);

            // Fire a paint event.
            OnPaint(sender, e);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            // Fire a paint event.
            // The paint event will be handled in Form1.graphicalOverlay1_Paint().

            if (Paint != null)
                Paint(sender, e);
        }
    }
}

namespace System.Windows.Forms
{
    using System.Drawing;

    public static class Extensions
    {
        public static Rectangle Coordinates(this Control control)
        {
            // Extend System.Windows.Forms.Control to have a Coordinates property.
            // The Coordinates property contains the control's form-relative location.
            Rectangle coordinates;
            Form form = (Form)control.TopLevelControl;

            if (control == form)
                coordinates = form.ClientRectangle;
            else
                coordinates = form.RectangleToClient(control.Parent.RectangleToScreen(control.Bounds));

            return coordinates;
        }
    }
}
