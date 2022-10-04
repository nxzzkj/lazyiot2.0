using Scada.FlowGraphEngine;
using Scada.FlowGraphEngine.GraphicsShape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
    
    public class SymbolListView : ListView
    {
        public event EventHandler OnSelectSymbol;
        public IndustrySymbolGroup FieldSymbol = null;
        public SymbolListView() :
            base()
        {
            this.View = View.LargeIcon;
            this.Font = new Font("宋体",12);
            

            this.GridLines = true;
            this.MultiSelect = false;
            this.OwnerDraw = true;
         
            this.Columns.Add("符号");
            this.DrawItem += SymbolListView_DrawItem;

            this.MouseDoubleClick += SymbolListView_MouseDoubleClick;
            this.MouseDown += SymbolListView_MouseDown;
          
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);//双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true); //禁止擦除背景.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); //禁止擦除背景.
            this.UpdateStyles();
         

        }
 

        private void SymbolListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Pen selectPen = Pens.Black;
        
            if (e.Item.Selected)
            {
                selectPen = Pens.Red;
            }
            Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
         
          
            Graphics g = e.Graphics;
            ScadaIndustrySymbol_Shape symbol = (ScadaIndustrySymbol_Shape)e.Item.Tag;

 
            if (symbol.Paths != null && symbol.Paths.Count > 0&& symbol.Bmp== null)
            {
                symbol.Bmp = new Bitmap(e.Bounds.Width, e.Bounds.Height);
                RectangleF mr = new RectangleF(0,0, e.Bounds.Width, e.Bounds.Height);
                Graphics mg = Graphics.FromImage(symbol.Bmp);
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddRectangle(r);
                symbol.Paint("", mg, mr, 0,new SVG_Color(Color.Red), SVT_IndustryFillType.Original, null);
                mg.Dispose();
                g.DrawImage(symbol.Bmp, e.Bounds.X, e.Bounds.Y);


            }
            else
            {
                g.DrawImage(symbol.Bmp, e.Bounds.X, e.Bounds.Y);
            }
          

            g.DrawRectangle(selectPen, r);

        }

       

        private void SymbolListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {


                ListViewItem liv = this.GetItemAt(e.X, e.Y);
                if (liv != null)
                {
                    SelectSymbol = (ScadaIndustrySymbol_Shape)liv.Tag;
                    if (liv != null)
                    {

                        if (OnSelectSymbol != null)
                        {
                            OnSelectSymbol(SelectSymbol, e);
                        }
                    }

                }
                else
                {
                    this.Parent.Cursor = Cursors.Default;
                }
            }
            else
            {
                this.Parent.Cursor = Cursors.Default;
                SelectSymbol = null;
            }
        }

        private void SymbolListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Parent.Cursor = Cursors.Default;
            SelectSymbol = null;
        }
     
    
        public ScadaIndustrySymbol_Shape SelectSymbol = null;
        public void InitItems()
        {
            if (FieldSymbol == null)
                return;
            
            this.Items.Clear();
            int num = 1;
            foreach(ScadaIndustrySymbol_Shape s in FieldSymbol.Shapes)
            {
                ListViewItem listViewItem = new ListViewItem(num.ToString());
                listViewItem.Font = new Font("宋体", 12);
                listViewItem.Tag = s;
                
                this.Items.Add(listViewItem);
                num++;
            }
           
        }
 

      

    }
}
