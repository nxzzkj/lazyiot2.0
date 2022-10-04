using Scada.DBUtility;
using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.BatchCommand
{
    [Serializable]
    public delegate void ShapeInfo(object sender, BatchCommandShape shape);
    public  class BatchCommandTaskGraph : ScrollableControl, IBatchCommandTaskGraphSite
    {
        public BatchCommandTaskGraph()
        {


            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);//双缓冲
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); //禁止擦除背景.
            this.UpdateStyles();
            Control.CheckForIllegalCrossThreadCalls = false;
            GraphAbstract = new BatchCommandTaskGraphAbstract();
            Shapes = new List<BatchCommandShape>();
            bag = new PropertyBag();
            bag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            bag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
        }
        public BatchCommandTaskGraphAbstract GraphAbstract;
        [Category("Graph"), Description("在双击实体或通过上下文时发生显示窗体"), Browsable(true)]
        public event PropertiesInfo OnShowProperties;

     
        [NonSerialized]
        protected PropertyBag bag;
        public   PropertyBag Properties
        {
            get { return bag; }

        }

        public BatchCommandTask BatchCommandTask { set { GraphAbstract.BatchCommandTask = value; } get { return GraphAbstract.BatchCommandTask; } }

        public BatchCommandTaskModel SaveBatchCommandTask()
        {
            GraphAbstract.BatchCommandTask.Items = new List<BatchCommandItem>();
            for(int i=0;i<this.Shapes.Count;i++)
            {
                GraphAbstract.BatchCommandTask.AddCommandItem(this.Shapes[i].BatchCommandItem);
            }
            return GraphAbstract.BatchCommandTask.CreateDBModelFromTask();


        }
        public List<BatchCommandShape> Shapes { set { GraphAbstract.Shapes = value; } get { return GraphAbstract.Shapes; } }

        public event BatchCommandControlInfo OnGraph;
     
        public BatchCommandMapArea MapArea = null;
        public new  Color BackColor = Color.Gray;
        public Color WorkBakcColor = Color.SteelBlue;
        public Color GridLineColor = Color.Gray;
        public float GridLineWidth = 0.5f;
        public bool ShowGrid = true;
        public int GridSize = 10;
        public BatchCommandItemShape AddCommand( BatchCommandItem commandItem)
        {
            BatchCommandItemShape batchCommand = new BatchCommandItemShape();
            batchCommand.X = commandItem.X;
            batchCommand.Y = commandItem.Y;
            batchCommand.Width = commandItem.Width;
            batchCommand.Height = commandItem.Height;
            batchCommand.Expand = commandItem.Expand == 1 ? true:false ;
            batchCommand.Site = this;
            batchCommand.BatchCommandItem = commandItem;
            batchCommand.Site = this;
            AddShape(batchCommand);
            return batchCommand;


        }
  
        public void InitMapArea(float left=0, float top=0, float right=2500, float bottom=2000)
        {

          
            AddProperties();
        

            MapArea = new BatchCommandMapArea();
            MapArea.Left = left;
            MapArea.Top = top;
            MapArea.Right = right;
            MapArea.Bottom = bottom;
            Shapes = new List<BatchCommandShape>();//绘图图元
            
               
           
 
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {

            base.OnMouseWheel(e);
        }
        protected void AddProperties()
        {
            try
            {
                
                bag.Properties.Add(new PropertySpec("图件背景色", typeof(Color), "图件属性", "图件背景色", Color.Gray));
                   bag.Properties.Add(new PropertySpec("工作区背景色", typeof(Color), "图件属性", "填充色1", Color.SteelBlue));
                bag.Properties.Add(new PropertySpec("网格线颜色", typeof(Color), "图件属性", "填充色2", Color.Gray));
                bag.Properties.Add(new PropertySpec("网格线宽度", typeof(float), "图件属性", "背景网格大小", 0.5f));

                bag.Properties.Add(new PropertySpec("背景网格大小", typeof(int), "图件属性", "背景网格大小", 20));

                bag.Properties.Add(new PropertySpec("是否显示网格", typeof(bool), "图件属性", "是否显示网格", false));
 
            }
            catch
            {

            }

        }
        /// <summary>
        /// 从PropertyBag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {   
            switch (e.Property.Name)
            {
               
                case "图件背景色": e.Value =BackColor; break;
        
                case "工作区背景色": e.Value = WorkBakcColor; break;
                case "网格线颜色": e.Value = GridLineColor; break;
                case "网格线宽度": e.Value = GridLineWidth; break;
                case "背景网格大小": e.Value = GridSize; break;
                case "是否显示网格": e.Value = ShowGrid; break;
               

            }
        }


        /// <summary>
        /// 设置属性网格的值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {


            switch (e.Property.Name)
            {
                case "图件背景色": BackColor=(Color)e.Value; break;
                case "工作区背景色": WorkBakcColor = (Color) e.Value; break;
                case "网格线颜色": GridLineColor = (Color)e.Value; break;
                case "网格线宽度": GridLineWidth = (float)e.Value; break;
                case "背景网格大小": GridSize = (int)e.Value; break;
                case "是否显示网格": ShowGrid = (bool)e.Value; break;

            }
        }


        public void RaiseShowProperties(PropertyBag props)
        {
            if (OnShowProperties != null)
                OnShowProperties(this, new object[] { props });

        }


        public void RaiseShowProperties(PropertyBag[] props)
        {
            if (OnShowProperties != null)
                OnShowProperties(this, props);

        }


        public void RaiseShowProperties(object[] props)
        {
            if (OnShowProperties != null)
                OnShowProperties(this, props);

        }

      

        #region 定义事件

        public Func<BatchCommandShape, bool> OnAddedShape;
        public Func<BatchCommandShape, bool> OnDeletedShape;
        public Func<BatchCommandShape, bool> OnCopyedShape;
        public Func<BatchCommandShape, bool> OnCutedShape;
        public Func<BatchCommandShape, bool> OnPastedShape;
 
        #endregion
       


        /// <summary>
        /// 删除选中的图元
        /// </summary>
        public void RemoveShape(BatchCommandShape shape)
        {
            this.Shapes.Remove(shape);
            for(int i=0;i< shape.BatchCommandItem.NextCommandItemIDList.Count;i++)
            {
                string id = shape.BatchCommandItem.NextCommandItemIDList[i];
                BatchCommandShape exShape = this.Shapes.Find(x => x.BatchCommandItem.CommandID == id);
                if (exShape != null)
                    RemoveShape(exShape);
            }
            if (OnDeletedShape != null)
            {
                OnDeletedShape(shape);
            }
            this.Invalidate();
        }
        public void AddShape(BatchCommandShape shape)
        {
            shape.EID = Guid.NewGuid().ToString();
            if (shape == null)
                return;
            shape.Site = this;
            this.Shapes.Add(shape);
      
            if (OnAddedShape != null)
            {
                OnAddedShape(shape);
            }
            this.Invalidate();
        }

        private BatchCommandShape HitHover(float x, float y)
        {

            RectangleF r = new RectangleF(x - 2, y - 2, 4, 4);

            for (int i = this.Shapes.Count - 1; i >= 0; i--)
            {
                BatchCommandShape elementShape = this.Shapes[i];
                if (elementShape != null)
                {
                    if (elementShape.Hit(r))
                    {
                        return elementShape;
                    }

                }

            }



            return null;
        }
        protected void OnProperties()
        {
            if (Hover != null)
                this.RaiseShowProperties(Hover.Bag);
            else
                this.RaiseShowProperties(this.Properties);

        }

        private void BatchCommandContextMentItem_Click(object sender, EventArgs e)
        {
            string text = "";
            if (sender is ToolStripMenuItem toolStripMenuItem)
            {
                text = toolStripMenuItem.Text;
            }
            else if (sender is ToolStripButton toolStripButton)
            {
                text = toolStripButton.Text;

            }
            switch (text)
            {
                case "删除":
                    {
                        if (Hover != null)
                            this.RemoveShape(Hover);
                    }
                    break;
                case "剪贴":
                    {
                        this.Cut();
                    }
                    break;
                case "复制":
                    {
                        this.Copy();
                    }
                    break;
                case "粘贴":
                    {
                        this.Paste();
                    }
                    break;
            }
        }
      
        public BatchCommandElementType BatchCommandElementType = BatchCommandElementType.NONE;
    
        protected override void OnMouseDown(MouseEventArgs me)
        {
            base.OnMouseDown(me);
         

            PointF p = new PointF(me.X - this.AutoScrollPosition.X, me.Y - this.AutoScrollPosition.Y);
            if (!MapArea.ClientRect.Contains(p))
            {
                return;
            }

         
            MouseEventArgs e = new MouseEventArgs(me.Button, me.Clicks, (int)(p.X), (int)(p.Y), me.Delta);
            Rectangle r = new Rectangle(e.X - 1, e.Y - 1, 2, 2);
            if (e.Clicks == 1 && e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < this.Shapes.Count; i++)
                {
                    this.Shapes[i].IsSelected = false;
                }

                Hover = HitHover(e.X, e.Y);
                if (Hover != null)
                {
                    Hover.RaiseMouseDown(e);
                    Update();
                }
          



            }
            else if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {

                Hover = HitHover(e.X, e.Y);
                if (Hover != null)
                {
                    Hover.RaiseMouseDown(e);
                    Update();
                }
            

            }


            else if (e.Clicks == 1 && e.Button == MouseButtons.Right)
            {
                Hover = HitHover(e.X, e.Y);
                if (Hover != null)
                {
                    Hover.RaiseMouseDown(e);
                    Update();
                }
                
                this.ContextMenuStrip = null;
                if (Hover != null)
                {
                    if (typeof(BatchCommandShape).IsInstanceOfType(Hover))
                    {
                        this.ContextMenuStrip = new ContextMenuStrip();

                        ToolStripMenuItem[] additionals = (Hover as BatchCommandShape).ShapeMenu();
                        this.ContextMenuStrip.Items.Clear();
                        if (additionals != null)
                        {
                            try
                            {
                                this.ContextMenuStrip.Items.Add("-");
                                this.ContextMenuStrip.Items.AddRange(additionals);
                            }
                            catch
                            {

                            }
                        }
                    }



                }
                else
                {
                    List<ToolStripMenuItem> menuItems = new List<ToolStripMenuItem>();

                    ToolStripMenuItem menu = null;
                    if (Hover != null)
                    {
                        menu = new ToolStripMenuItem("剪贴");
                        menuItems.Add(menu);
                        menu = new ToolStripMenuItem("复制");
                        menuItems.Add(menu);
                    }
                    if (CursurTag != null)
                    {

                        menu = new ToolStripMenuItem("粘贴");
                        menuItems.Add(menu);
                    }

                    menu = new ToolStripMenuItem("删除");
                    menuItems.Add(menu);
                    for (int i = 0; i < menuItems.Count; i++)
                    {
                        menuItems[i].Click += BatchCommandContextMentItem_Click;
                    }
                    this.ContextMenuStrip = new ContextMenuStrip();
                    this.ContextMenuStrip.Items.AddRange(menuItems.ToArray());


                }
            }

            OnProperties();
            this.Invalidate();
        
        }

       


        protected override void OnMouseMove(MouseEventArgs me)
        {


            base.OnMouseMove(me);

         

            PointF p = new PointF(me.X - this.AutoScrollPosition.X, me.Y - this.AutoScrollPosition.Y);
            if (!MapArea.ClientRect.Contains(p))
            {
                return;
            }

          
            MouseEventArgs e = new MouseEventArgs(me.Button, me.Clicks, (int)(p.X), (int)(p.Y), me.Delta);
  

            if (this.BatchCommandElementType == BatchCommandElementType.NONE)
            {
                if (Hover != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {

                        Hover.RaiseMouseMove(e);
                    }

                }
                
            }
            
            if (OnGraph != null)
            {
                string desc = "";
                if (Hover != null)
                {
                    PointF up = new PointF();
                    PointF cp = Hover.Center;
                    this.MapArea.ReturnLToU_Point(p, ref up);
                    RectangleF uf = RectangleF.Empty;
                    this.MapArea.ReturnLToU_Rectangle(Hover.Rectangle, ref uf);
                    desc = "X " + up.X + ",Y " + up.Y + " W " + uf.Width + ", H " + uf.Height;
                }
                else
                {
                    PointF up = new PointF();
                    this.MapArea.ReturnLToU_Point(p, ref up);
                    desc = "X " + up.X + ",Y " + up.Y;
                }
                OnGraph(this, desc);
            }

        }
        public BatchCommandShape Hover = null;
        protected override void OnMouseUp(MouseEventArgs me)
        {
            base.OnMouseUp(me);
    

            PointF p = new PointF(me.X - this.AutoScrollPosition.X, me.Y - this.AutoScrollPosition.Y);
            if (!MapArea.ClientRect.Contains(p))
            {
                return;
            }

          
            MouseEventArgs e = new MouseEventArgs(me.Button, me.Clicks, (int)(p.X), (int)(p.Y), me.Delta);
            Rectangle r = new Rectangle(e.X - 1, e.Y - 1, 2, 2);

            if (this.BatchCommandElementType == BatchCommandElementType.NONE)
            {
                if (Hover != null)
                {
                    Hover.RaiseMouseUp(e);
                }
               

            }
             
 
            this.Invalidate();
        }
        public BatchCommandShape CursurTag = null;
        public void Copy()
        {
            if (Hover != null)
            {
                this.Cursor = Cursors.Hand;
                CursurTag = Hover.Copy();
                this.Cursor = Cursors.Default;
            }
            else
            {
                MessageBox.Show("请选择要复制的图元");
            }


        }
        public void Cut()
        {
            if (Hover != null)
            {
                this.Cursor = Cursors.Hand;
                CursurTag = Hover;
                this.RemoveShape(Hover);
                this.Cursor = Cursors.Default;
            }
            else
            {
                MessageBox.Show("请选择要复制的图元");
            }
        }
        public void Paste()
        {
            if (CursurTag != null)
            {
                BatchCommandShape co = CursurTag as BatchCommandShape;
                co.Offset(10, 10);
                co.Site = this;
                this.AddShape(co);

                this.Invalidate();
                CursurTag = null;
            }
            else
            {
                MessageBox.Show("请选择要复制的图元");
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Control && e.KeyCode == Keys.C)//拷贝
            {
                if (Hover != null)
                {
                    this.Cursor = Cursors.Hand;
                    Cursor.Tag = Hover.Copy();
                    this.Cursor = Cursors.Default;
                }
            }
            if (e.Control && e.KeyCode == Keys.V)//拷贝
            {
                if (Cursor.Tag != null)
                {
                    BatchCommandShape co = Cursor.Tag as BatchCommandShape;
                    co.Offset(10, 10);
                    co.Site = this;
                    this.Shapes.Add(co);
                    this.Invalidate();

                }
            }




        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (MapArea == null)
                return;

        
            RectangleF b = MapArea.ClientRect;
            this.AutoScrollMinSize = new Size(Convert.ToInt32(b.Size.Width *1.5f), Convert.ToInt32(b.Size.Height * 1.5f));
            Graphics g = e.Graphics;
            GraphicsContainer state = g.BeginContainer();
            g.PageUnit = GraphicsUnit.Pixel;
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, MatrixOrder.Append);
      
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);
            PaintWork(g);
            for (int i = 0; i < this.Shapes.Count; i++)
            {
                BatchCommandShape elementShape = Shapes[i];
                if (elementShape != null)
                    elementShape.Paint(g);
            }
            g.EndContainer(state);


        }
        protected void PaintWork(Graphics pevent)
        {
            Pen myPen = new Pen(new SolidBrush(GridLineColor), GridLineWidth);
            myPen.DashStyle = DashStyle.Dash;
            Graphics g = pevent;

            g.FillRectangle(new SolidBrush(WorkBakcColor), MapArea.ClientRect);

            float x = MapArea.ClientRect.X;
            float y = MapArea.ClientRect.Y;

            int numx = Convert.ToInt32(MapArea.ClientRect.Width / this.GridSize);
            int numy = Convert.ToInt32(MapArea.ClientRect.Height / this.GridSize);

            float disx = this.GridSize;
            float dixy = this.GridSize;
            for (int i = 1; i < numx; i++)
            {
                x += disx;
                g.DrawLine(myPen, x, MapArea.ClientRect.Top, x, MapArea.ClientRect.Bottom);



            }
            for (int i = 1; i < numy; i++)
            {
                y += dixy;
                g.DrawLine(myPen, this.ClientRectangle.Left, y, MapArea.ClientRect.Right, y);



            }
            g.DrawRectangle(myPen, MapArea.ClientRect.X, MapArea.ClientRect.Y, MapArea.ClientRect.Width, MapArea.ClientRect.Height);
        }

    }
}
