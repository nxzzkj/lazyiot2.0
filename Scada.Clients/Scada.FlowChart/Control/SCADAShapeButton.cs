
using Scada.FlowGraphEngine;
using ScadaFlowDesign.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaFlowDesign.Control
{
  public   class SCADAShapeButton:PictureBox
    {
        public string ShapeName
        {
            set;get;
        }
          
        public SCADAShapeButton()
        {

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);//双缓冲

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); //禁止擦除背景.
            this.UpdateStyles();
            this.MouseDown += SCADAShapeButton_MouseDown;
            this.MouseUp += SCADAShapeButton_MouseUp;
            this.Paint += SCADAShapeButton_Paint;
            this.MouseDoubleClick += SCADAShapeButton_MouseDoubleClick;

        }

        private void SCADAShapeButton_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            selected = false;
            this.Parent.Cursor = Cursors.Default;
        }

        private void SCADAShapeButton_Paint(object sender, PaintEventArgs e)
        {
        
            if(selected)
            {
                e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Black,4),e.ClipRectangle);
            }
        }

        private bool selected = false;

        private void SCADAShapeButton_MouseUp(object sender, MouseEventArgs e)
        {
            selected = false;
            this.Invalidate();
        }
     

        private void SCADAShapeButton_MouseDown(object sender, MouseEventArgs e)
        {
            selected = true;
            this.Invalidate();
            if(this.Image!=null)
            {
                Bitmap bmp = (Bitmap)this.Image;
                this.FindForm().Cursor = new Cursor(bmp.GetHicon());
            }
        
        }
        public IntPtr GetCursor()
        {
            
            return Cursors.Default.Handle;
        }

        private ShapeElement mShapeElement = ShapeElement.None;
        public ShapeElement ShapeElement
        {
            set { mShapeElement = value; }
            get { return mShapeElement; }
        }
       
    }
}
