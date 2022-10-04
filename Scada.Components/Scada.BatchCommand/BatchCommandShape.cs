

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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.BatchCommand
{
    [Serializable]
    public class BatchCommandShape : ISerializable
    {
        public bool Expand = true;
        public RectangleF HeadItemRect = RectangleF.Empty;
        private BatchCommandItem _BatchCommandItem = null;
        public BatchCommandItem BatchCommandItem
        {
            set { _BatchCommandItem = value;
                AddProperties();
            }
            get { return _BatchCommandItem; }
        }
        protected internal virtual void InitEntity()
        {



            EID = Scada.DBUtility.GUIDToNormalID.GuidToLongID().ToString();
            mBag = new PropertyBag(this);
            mBag.GetValue += new PropertySpecEventHandler(GetPropertyBagValue);
            mBag.SetValue += new PropertySpecEventHandler(SetPropertyBagValue);
            AddProperties();

        }
        public BatchCommandShape()
        {

            IsSelected = false;
            InitEntity();
        }
        public virtual void AddProperties()
        {
            this.Bag.Properties.Clear();

            return;
        }


        protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {

        }


        protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {


            this.Invalidate();
        }
        protected BatchCommandShape(SerializationInfo info, StreamingContext context)
        {
            InitEntity();
            #region 自定义属性
            this.EID = (string)info.GetValue("EID", typeof(string));
            this.mRectangle = (RectangleF)info.GetValue("mRectangle", typeof(RectangleF));
            this.Expand = (bool)info.GetValue("Expand", typeof(bool));
            this.HeadItemRect = (RectangleF)info.GetValue("HeadItemRect", typeof(RectangleF));
            this.BatchCommandItem = (BatchCommandItem)info.GetValue("BatchCommandItem", typeof(BatchCommandItem));
            #endregion
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("EID", this.EID);
            info.AddValue("mRectangle", this.mRectangle);
            info.AddValue("Expand", this.Expand);
            info.AddValue("HeadItemRect", this.HeadItemRect);
            info.AddValue("BatchCommandItem", this.BatchCommandItem);
        }
        [NonSerialized]
        private PropertyBag mBag;

        /// <summary>
        ///  
        /// </summary>			

        public PropertyBag Bag
        {
            get { return mBag; }
            set { mBag = value; }

        }
        public string EID
        {
            set;
            get;
        }
        public bool IsResizable
        { set; get; }


        public float X
        {
            get { return this.mRectangle.X; }

            set
            {
                this.mRectangle.X = value;
            }
        }
        public float Y
        {
            get { return this.mRectangle.Y; }

            set
            {
                this.mRectangle.Y = value;
            }
        }

        public float Width
        {
            get { return this.mRectangle.Width; }

            set
            {
                this.mRectangle.Width = value;
            }
        }
        public float Height
        {
            get { return this.mRectangle.Height; }

            set
            {
                this.mRectangle.Height = value;
            }
        }
        public virtual void Offset(float x, float y)
        {

        }
        public RectSelectType SelectResult = RectSelectType.None;
        public GraphicsPath GetRoundRectPath(RectangleF rc, float r, RoundRectStyle rectStyle = RoundRectStyle.All)
        {
            float x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath gpath = new GraphicsPath();
            switch (rectStyle)
            {
                case RoundRectStyle.All:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }
                case RoundRectStyle.Bottom://底边为圆角
                    {
                        gpath.AddLine(x, y, x + w, y);
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }
                case RoundRectStyle.Top:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddLine(x + w, y + h, x, y + h);

                        break;
                    }

                case RoundRectStyle.Left:
                    {
                        gpath.AddLine(x, y + h, x, y);
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧


                        break;
                    }
                case RoundRectStyle.Right:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddLine(x + w, y, x + w, y + h);
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }

                case RoundRectStyle.Left_Bottom:
                    {
                        gpath.AddLine(x, y, x + w, y);
                        gpath.AddLine(x + w, y, x + w, y + h);
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }
                case RoundRectStyle.Left_Top:
                    {


                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddLine(x + w, y, x + w, y + h);
                        gpath.AddLine(x + w, y + h, x, y + h);
                        break;
                    }
                case RoundRectStyle.Right_Bottom:
                    {


                        gpath.AddLine(x, y + h, x, y);
                        gpath.AddLine(x, y, x + w, y);
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧


                        break;
                    }

                case RoundRectStyle.Right_Top:
                    {

                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧

                        gpath.AddLine(x + w, y + h, x, y + h);

                        gpath.AddLine(x, y + h, x, y);



                        break;
                    }

                case RoundRectStyle.Left_Bottom_NoCorner:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧
                        gpath.AddLine(x, y + h, x, y + h);

                        break;
                    }
                case RoundRectStyle.Left_Top_NoCorner:
                    {
                        gpath.AddLine(x, y, x, y);
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }

                case RoundRectStyle.Right_Bottom_NoCorner:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddLine(x + w, y + h, x + w, y + h);
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }
                case RoundRectStyle.Right_Top_NoCorner:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddLine(x + w, y, x + w, y);
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧

                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }
                case RoundRectStyle.LB_RT:
                    {

                        gpath.AddLine(x, y, x, y);
                        gpath.AddArc(x + w - r, y, r, r, 270, 90);//右上角圆弧
                        gpath.AddLine(x + w, y + h, x + w, y + h);
                        gpath.AddArc(x, y + h - r, r, r, 90, 90);//左下角圆弧

                        break;
                    }
                case RoundRectStyle.LT_RB:
                    {
                        gpath.AddArc(x, y, r, r, 180, 90);//左上角圆弧
                        gpath.AddLine(x + w, y, x + w, y);
                        gpath.AddArc(x + w - r, y + h - r, r, r, 0, 90);//右下角圆弧
                        gpath.AddLine(x, y + h, x, y + h);

                        break;
                    }

            }

            gpath.CloseFigure();//闭合
            return gpath;
        }
        public RectangleF mRectangle = RectangleF.Empty;
        public RectangleF Rectangle { set { mRectangle = value; } get { return mRectangle; } }
        public PointF MouseStart = PointF.Empty;
        public bool IsMousePress = false;
        public bool IsSelected = false;
        public virtual BatchCommandTaskGraph Site
        {
            set;
            get;
        }
        public Color SelectColor = Color.LightBlue;
        public PointF Center
        {
            get { return new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2); }
        }
        /// <summary>
        /// Occurs when the mouse is pressed on this entity
        /// </summary>
        public event MouseEventHandler OnMouseDown;

        /// <summary>
        /// Occurs when the mouse is released while above this entity
        /// </summary>
        public event MouseEventHandler OnMouseUp;

        /// <summary>
        /// Occurs when the mouse is moved while above this entity
        /// </summary>
        public event MouseEventHandler OnMouseMove;


        public virtual void RaiseMouseDown(MouseEventArgs e)
        {

            if (OnMouseDown != null) OnMouseDown(this, e);
            Invalidate();
        }

        public virtual void RaiseMouseUp(MouseEventArgs e)
        {
            if (OnMouseUp != null) OnMouseUp(this, e);
            Invalidate();
        }

        public virtual void RaiseMouseMove(MouseEventArgs e)
        {
            if (OnMouseMove != null) OnMouseMove(this, e);
            Invalidate();
        }


        public virtual void Paint(Graphics g)
        {

        }
        public virtual void Invalidate()
        {
            if (Site != null)
            {
                Site.Invalidate();
            }
        }
        public void Delete()
        {
            this.Site.RemoveShape(this);
        }
        public virtual BatchCommandShape Copy()
        {
            return null;
        }
        public virtual bool Hit(RectangleF r)
        {
            return false;
        }

        public virtual ToolStripMenuItem[] ShapeMenu()
        {
            List<ToolStripMenuItem> menuItems = new List<ToolStripMenuItem>();

            ToolStripMenuItem menu = null;
            menu = new ToolStripMenuItem("删除");
            menuItems.Add(menu);
            for(int i=0;i< menuItems.Count;i++)
            {
                menuItems[i].Click += BatchCommandShape_Click;
            }
            return menuItems.ToArray();
        }

     

        private void BatchCommandShape_Click(object sender, EventArgs e)
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
                        this.Delete();
                    }
                    break;
                case "剪贴":
                    {
                        this.Site.Cut();
                    }
                    break;
                case "复制":
                    {
                        this.Site.Copy();
                    }
                    break;
                case "粘贴":
                    {
                        this.Site.Paste();
                    }
                    break;

            }
            this.Invalidate();
        }
        //设置鼠标
        private Cursor mCursor = Cursors.Default;
        public virtual void SetCursor(Cursor cur)
        {
            mCursor = cur;
            if (Site != null)
            {
                Site.Cursor = cur;
            }
        }


        public virtual void Update(PointF p)
        {

        }
        public virtual void AddPoint(PointF p)
        {
        }

        /* *
  * 获得一个矩形中心点到矩形外某点之间的连线与矩形的交点
  * 算法思路：
  * 以矩形外的某点为坐标原点，将图形分为四个象限，每个象限分两种情况，坐标轴上单独处理
  * 1.坐标轴的情况：
  * 1.负x轴的情况
  * 2.正x轴的情况
  * 3.正y轴的情况
  * 4.负y轴的情况
  * 2.象限情况：
  * 2.1.第一象限：连线与矩形左边相交的情况；与矩形下边相交的情况
  * 2.2.第二象限：连线与矩形右边相交的情况；与矩形下边相交的情况
  * 2.3.第三象限：连线与矩形右边相交的情况；与矩形上边相交的情况
  * 2.4.第四象限：连线与矩形左边相交的情况；与矩形上边相交的情况
  *
  * */
        public  PointF GetIntersectPoint(RectangleF fromNode, PointF endPoint)
        {
            // 开始矩形的x坐标
            var x1 = fromNode.X;
            // 开始矩形的y坐标
            var y1 = fromNode.Y;
            // 结束点的x坐标
            var x2 = endPoint.X;
            // 结束点的y坐标
            var y2 = endPoint.Y;

            // 开始矩形的中心点x坐标
            var fromCenterX = x1 + fromNode.Width / 2;
            // 开始矩形的中心点的y坐标
            var fromCenterY = y1 + fromNode.Height / 2;
            // 矩形和点之间的x坐标相对距离
            var dx = Math.Abs(x1 - x2);
            // 矩形和点之间的y坐标相对距离
            var dy = Math.Abs(y1 - y2);
            // 相对距离的正切值
            var tanDYX = dy / dx;
            // 开始矩形的正切值
            var fromDYX = fromNode.Height / fromNode.Width;

            PointF returnPoint = PointF.Empty;
            // 负x轴
            if (y1 == y2 && x1 < x2)
            {
                returnPoint = new PointF(x1 + fromNode.Width, fromCenterY);
            }
            // 正x轴
            else if (y1 == y2 && x1 > x2)
            {
                returnPoint = new PointF(x1, fromCenterY);
            }
            // 正y轴
            else if (x1 == x2 && y1 < y2)
            {
                returnPoint = new PointF(fromCenterX, y1 + fromNode.Height);
            }
            // 负y轴
            else if (x1 == x2 && y1 > y2)
            {
                returnPoint = new PointF(fromCenterX, y1);
            }
            // 第一象限
            if (x1 > x2 && y1 < y2)
            {
                if (fromDYX >= tanDYX)
                {
                    returnPoint = new PointF(x1, fromCenterY + tanDYX * fromNode.Width / 2);
                }
                else
                {
                    returnPoint = new PointF(fromCenterX - dx / dy * fromNode.Height / 2, y1 + fromNode.Height);
                }
            }
            // 第二象限
            else if (x1 < x2 && y1 < y2)
            {
                //
                if (fromDYX >= tanDYX)
                {
                    returnPoint = new PointF(x1 + fromNode.Width, fromCenterY + tanDYX * fromNode.Width / 2);
                }
                else
                {
                    returnPoint = new PointF(fromCenterX + dx / dy * fromNode.Height / 2, y1 + fromNode.Height);
                }

            }
            // 第三象限
            else if (x1 < x2 && y1 > y2)
            {
                if (fromDYX >= tanDYX)
                {
                    returnPoint = new PointF(x1 + fromNode.Width, fromCenterY - tanDYX * fromNode.Width / 2);
                }
                else
                {
                    returnPoint = new PointF(fromCenterX + fromNode.Height / 2 * dx / dy, y1);
                }
            }
            // 第四象限
            else if (x1 > x2 && y1 > y2)
            {
                if (fromDYX >= tanDYX)
                {
                    returnPoint = new PointF(x1, fromCenterY - fromNode.Width / 2 * tanDYX);
                }
                else
                {
                    returnPoint = new PointF(fromCenterX - fromNode.Height / 2 * dx / dy, y1);
                }
            }
            return returnPoint;
        }

    }
}
