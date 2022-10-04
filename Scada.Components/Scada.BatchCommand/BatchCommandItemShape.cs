

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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.BatchCommand
{
    [Serializable]
    public class BatchCommandItemShape : BatchCommandShape
    {
        public BatchCommandItemShape() : base()
        {

        }
        protected BatchCommandItemShape(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {

            this.ItemTextColor = (Color)info.GetValue("ItemTextColor", typeof(Color));
            this.ItemTextFont = (Font)info.GetValue("ItemTextFont", typeof(Font));
            this.HeadBackColor = (Color)info.GetValue("HeadBackColor", typeof(Color));
            this.HeadForeColor = (Color)info.GetValue("HeadForeColor", typeof(Color));
            this.HeadBorderColor = (Color)info.GetValue("HeadBorderColor", typeof(Color));
            this.HeadBorderWidth = (float)info.GetValue("HeadBorderWidth", typeof(float));
            this.HeadTextColor = (Color)info.GetValue("HeadTextColor", typeof(Color));
            this.HeadTextFont = (Font)info.GetValue("HeadTextFont", typeof(Font));
            this.BackColor = (Color)info.GetValue("BackColor", typeof(Color));
            this.ForeColor = (Color)info.GetValue("ForeColor", typeof(Color));
            this.BorderColor = (Color)info.GetValue("BorderColor", typeof(Color));
            this.BorderWidth = (float)info.GetValue("BorderWidth", typeof(float));
            this.Radious = (float)info.GetValue("Radious", typeof(float));
         

            this.ItemRect = (RectangleF)info.GetValue("ItemRect", typeof(RectangleF));
            this.ConnectorLineWidth = (float)info.GetValue("ItemRect", typeof(float));
            this.ConnectorLineColor = (Color)info.GetValue("ConnectorLineColor", typeof(Color));

        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ItemTextColor", this.ItemTextColor);
            info.AddValue("ItemTextFont", this.ItemTextFont);
            info.AddValue("HeadBackColor", this.HeadBackColor);
            info.AddValue("HeadForeColor", this.HeadForeColor);
            info.AddValue("HeadBorderColor", this.HeadBorderColor);
            info.AddValue("HeadBorderWidth", this.HeadBorderWidth);
            info.AddValue("HeadTextColor", this.HeadTextColor);
            info.AddValue("HeadTextFont", this.HeadTextFont);
            info.AddValue("BackColor", this.BackColor);
            info.AddValue("ForeColor", this.ForeColor);
            info.AddValue("BorderColor", this.BorderColor);
            info.AddValue("BorderWidth", this.BorderWidth);
            info.AddValue("Radious", this.Radious);
          
            info.AddValue("ItemRect", this.ItemRect);
            info.AddValue("ConnectorLineColor", this.ConnectorLineColor);
            info.AddValue("ConnectorLineWidth", this.ConnectorLineWidth);



        }
       
        private Color ItemTextColor = Color.Black;
        private Font ItemTextFont = new Font("微软雅黑", 9);
        private Color HeadBackColor = Color.Gray;
        private Color HeadForeColor = Color.WhiteSmoke;
        private Color HeadBorderColor = Color.Black;
        private float HeadBorderWidth = 2;
        private Color HeadTextColor = Color.Black;
        private Font HeadTextFont = new Font("微软雅黑", 18);
        private Color BackColor = Color.Gray;
        private Color ForeColor = Color.WhiteSmoke;
        private Color BorderColor = Color.Black;
        private float BorderWidth = 2;

        private Color ConnectorLineColor = Color.Red;
        private float ConnectorLineWidth = 10;


        float Radious = 10;
        /// <summary>
        /// 处于折叠还是展开状态
        /// </summary>
       
   
        RectangleF ItemRect = RectangleF.Empty;
        public override void Paint(Graphics g)
        {



            mRectangle.X = (mRectangle.X <= mRectangle.Right) ? mRectangle.X : mRectangle.Right;
            mRectangle.Y = (mRectangle.Y <= mRectangle.Bottom) ? mRectangle.Y : mRectangle.Bottom;
            mRectangle.Width = mRectangle.Right - mRectangle.X;
            if (mRectangle.Width < 30) mRectangle.Width = 30;
            mRectangle.Height = mRectangle.Bottom - Rectangle.Y;
            if (mRectangle.Height < 30) mRectangle.Height = 30;
            //计算每个子项区域
            SizeF titSize = g.MeasureString("测试", HeadTextFont);
            //计算标题域
            HeadItemRect = new RectangleF(this.X, this.Y, this.Width, titSize.Height + titSize.Height / 3);
            ItemRect = new RectangleF(this.X, this.Y + HeadItemRect.Height, this.Width, this.Height - HeadItemRect.Height);

            //刷新状态
            this.BatchCommandItem.RefreshItemType();
         
            if (Expand)
            {
                using (GraphicsPath path = base.GetRoundRectPath(this.Rectangle, Radious, RoundRectStyle.Top))
                {
                  
                    Color fillcolor = this.BackColor;
                    using (LinearGradientBrush brush = new LinearGradientBrush(ItemRect, this.BackColor, this.ForeColor, LinearGradientMode.Vertical))
                    {
                        g.FillPath(brush, path);
                    }
                    //绘制边框
                    g.DrawPath(new Pen(BorderColor, BorderWidth), path);
                    SizeF txtSize = g.MeasureString(BatchCommandItem.CommandContent, ItemTextFont);
                    g.DrawString(BatchCommandItem.CommandContent, ItemTextFont, new SolidBrush(ItemTextColor), ItemRect, new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });


                }

            }

            using (GraphicsPath titlePath = base.GetRoundRectPath(HeadItemRect, Radious, RoundRectStyle.Top))
            {
               
                Color titlefillcolor = HeadBackColor;
                using (LinearGradientBrush brush = new LinearGradientBrush(HeadItemRect, this.HeadBackColor, this.HeadForeColor, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, titlePath);
                }
                g.DrawPath(new Pen(HeadBorderColor, HeadBorderWidth), titlePath);

                titSize = g.MeasureString(string.IsNullOrEmpty(BatchCommandItem.CommandItemTitle) ? "[请输入标题]" : BatchCommandItem.CommandItemTitle, HeadTextFont);
                g.DrawString(string.IsNullOrEmpty(BatchCommandItem.CommandItemTitle) ? "[请输入标题]" : BatchCommandItem.CommandItemTitle, HeadTextFont, new SolidBrush(HeadTextColor), HeadItemRect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                //绘制标题图标
                RectangleF iconRect = new RectangleF()
                {
                    X = HeadItemRect.X + HeadItemRect.Width / 2 - titSize.Width / 2 - titSize.Height,
                    Y = HeadItemRect.Y + HeadItemRect.Height / 2 - titSize.Height / 2,
                    Height = titSize.Height,
                    Width = titSize.Height

                };
                if (iconRect.X <= this.X)
                    iconRect.X = this.X;
                if (BatchCommandItem.Icon != null)
                {

                    g.DrawImage(BatchCommandItem.Icon, iconRect);
                }


            }
            if (!string.IsNullOrEmpty(this.BatchCommandItem.PreCommandItemID))
            {
            


                BatchCommandShape commandShape = this.Site.Shapes.Find(x => x.BatchCommandItem.CommandID == this.BatchCommandItem.PreCommandItemID);
                if (commandShape != null)
                {
                    PointF scp;
                    PointF ecp;
                    PointF sP;
                    PointF eP;
                    RectangleF preRect = commandShape.Rectangle;
                    if (!commandShape.Expand)
                    {
                        preRect = commandShape.HeadItemRect;
                    }
                    RectangleF currRect = this.Rectangle;
                    if (!this.Expand)
                    {
                        currRect = this.HeadItemRect;
                    }
                    scp = new PointF(preRect.X + preRect.Width / 2, preRect.Y + preRect.Height / 2);
                    ecp = new PointF(currRect.X + currRect.Width / 2, currRect.Y + currRect.Height / 2);
                    sP = GetIntersectPoint(preRect, ecp);
                    eP = GetIntersectPoint(currRect, scp);
                    Pen linBorderPen = new Pen(Color.Black, ConnectorLineWidth + 2);
                    g.DrawLine(linBorderPen, sP, eP);
                    LinearGradientBrush linearGradientBrush = new LinearGradientBrush(sP, eP, ConnectorLineColor, this.BackColor);
                     Pen linPen = new Pen(linearGradientBrush, ConnectorLineWidth);
                    linPen.DashStyle = DashStyle.Solid;
                    linPen.EndCap = LineCap.Triangle;
                    g.DrawLine(linPen, sP, eP);
                }


            }

            if(BatchCommandItem!=null)
            {
                BatchCommandItem.X = this.X;
                BatchCommandItem.Y = this.Y;
                BatchCommandItem.Width = this.Width;
                BatchCommandItem.Height = this.Height;
                BatchCommandItem.Expand = Expand ? 1 : 0;

            }
            if (IsSelected)
            {
                if (Expand)
                {
                    using (Pen bPen = new Pen(new SolidBrush(Color.Gray)))
                    {

                        bPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        bPen.Width = 2;
                        g.DrawRectangle(bPen, this.X, this.Y, this.Width, this.Height);
                        using (SolidBrush fillBrush = new SolidBrush(Color.LightBlue))
                        {
                            g.FillEllipse(fillBrush, this.X - 2, this.Y - 2, 4, 4);
                            g.FillEllipse(fillBrush, this.X - 2, this.Y + this.Height - 2, 4, 4);
                            g.FillEllipse(fillBrush, this.X + this.Width - 2, this.Y - 2, 4, 4);
                            g.FillEllipse(fillBrush, this.X + this.Width - 2, this.Y + this.Height - 2, 4, 4);

                        }

                    }


                }
                else
                {
                    using (Pen bPen = new Pen(new SolidBrush(Color.Gray)))
                    {

                        bPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        bPen.Width = 2;
                        g.DrawRectangle(bPen, HeadItemRect.X, HeadItemRect.Y, HeadItemRect.Width, HeadItemRect.Height);
                        using (SolidBrush fillBrush = new SolidBrush(Color.LightBlue))
                        {
                            g.FillEllipse(fillBrush, HeadItemRect.X - 2, HeadItemRect.Y - 2, 4, 4);
                            g.FillEllipse(fillBrush, HeadItemRect.X - 2, HeadItemRect.Y + HeadItemRect.Height - 2, 4, 4);
                            g.FillEllipse(fillBrush, HeadItemRect.X + HeadItemRect.Width - 2, HeadItemRect.Y - 2, 4, 4);
                            g.FillEllipse(fillBrush, HeadItemRect.X + HeadItemRect.Width - 2, HeadItemRect.Y + HeadItemRect.Height - 2, 4, 4);

                        }

                    }


                }


            }



        }
        public override ToolStripMenuItem[] ShapeMenu()
        {
            ToolStripMenuItem[] olds = base.ShapeMenu();
            ToolStripMenuItem[] menus = new ToolStripMenuItem[olds.Length + 1];
            for (int i = 0; i < olds.Length; i++)
            {
                menus[i] = olds[i];

            }
            menus[olds.Length ] = new ToolStripMenuItem() { Text = "添加下置命令" };
            menus[olds.Length ].Click += BatchCommandItemShape_Click;
            return menus;
        }

        private void BatchCommandItemShape_Click(object sender, EventArgs e)
        {

            BatchCommandItem item = new BatchCommandItem()
            {
                CommandItemTitle = "未命名",
                PreCommandItemID = this.BatchCommandItem.CommandID,
                BatchTask = this.Site.BatchCommandTask,
                X = this.X + 20,
                Y = this.Y + 20,
                Width = this.Width,
                Height = this.Height,
                BatchCommandTaskId = this.BatchCommandItem.BatchCommandTaskId,
                SERVER_ID = this.BatchCommandItem.SERVER_ID,
                Expand=1
                 

            };


            BatchCommandItemShape batchCommand = this.Site.AddCommand(item);
            this.BatchCommandItem.NextCommandItemIDList.Add(batchCommand.BatchCommandItem.CommandID);
            batchCommand.BatchCommandItem.PreCommandItemID = this.BatchCommandItem.CommandID;
            this.Invalidate();
        }

        public override void RaiseMouseDown(MouseEventArgs e)
        {



            RectangleF f = new RectangleF(e.X - 5, e.Y - 5, 10, 10);
            SetCursor(Cursors.Cross);
            IsResizable = true;

            mStartPoint.X = e.X;
            mStartPoint.Y = e.Y;




        }
        public override void RaiseMouseMove(MouseEventArgs e)
        {
            PointF mEndPoint = new PointF(e.X, e.Y);

            if (IsResizable)
            {

                switch (SelectResult)
                {
                    case RectSelectType.Rect:
                        {


                            mRectangle.X += mEndPoint.X - mStartPoint.X;
                            mRectangle.Y += mEndPoint.Y - mStartPoint.Y;


                            mStartPoint.X = e.X;
                            mStartPoint.Y = e.Y;
                            Invalidate();
                            break;
                        }
                    case RectSelectType.LB:
                        {
                            mRectangle.X = mEndPoint.X;
                            mRectangle.Height += mEndPoint.Y - mStartPoint.Y;
                            mRectangle.Width -= mEndPoint.X - mStartPoint.X;

                            mStartPoint.X = e.X;
                            mStartPoint.Y = e.Y;
                            Invalidate();
                            break;
                        }
                    case RectSelectType.LT:
                        {
                            mRectangle.X = mEndPoint.X;
                            mRectangle.Y = mEndPoint.Y;
                            mRectangle.Height -= mEndPoint.Y - mStartPoint.Y;
                            mRectangle.Width -= mEndPoint.X - mStartPoint.X;


                            mStartPoint.X = e.X;
                            mStartPoint.Y = e.Y;
                            Invalidate();
                            break;
                        }
                    case RectSelectType.RT:
                        {

                            mRectangle.Y = mEndPoint.Y;
                            mRectangle.Height -= mEndPoint.Y - mStartPoint.Y;
                            mRectangle.Width += mEndPoint.X - mStartPoint.X;

                            mStartPoint.X = e.X;
                            mStartPoint.Y = e.Y;
                            Invalidate();
                            break;
                        }
                    case RectSelectType.RB:
                        {


                            mRectangle.Height += mEndPoint.Y - mStartPoint.Y;
                            mRectangle.Width += mEndPoint.X - mStartPoint.X;

                            mStartPoint.X = e.X;
                            mStartPoint.Y = e.Y;
                            Invalidate();
                            break;
                        }

                }

            }
            mStartPoint = mEndPoint;
        }

        public override void RaiseMouseUp(MouseEventArgs e)
        {
            if (IsResizable)
            {

                Invalidate();
                SetCursor(System.Windows.Forms.Cursors.Default);
                SelectResult = RectSelectType.None;
                IsResizable = false;
            }
        }
        public PointF mStartPoint = new PointF();

        public override bool Hit(RectangleF r)
        {


            IsSelected = false;
            PointF[] pts = new PointF[4];
            if (Expand)
            {
                pts[0] = new PointF(Rectangle.Left, Rectangle.Top);
                pts[1] = new PointF(Rectangle.Left, Rectangle.Bottom);
                pts[2] = new PointF(Rectangle.Right, Rectangle.Bottom);
                pts[3] = new PointF(Rectangle.Right, Rectangle.Top);


            }
            else
            {
                pts[0] = new PointF(HeadItemRect.Left, HeadItemRect.Top);
                pts[1] = new PointF(HeadItemRect.Left, HeadItemRect.Bottom);
                pts[2] = new PointF(HeadItemRect.Right, HeadItemRect.Bottom);
                pts[3] = new PointF(HeadItemRect.Right, HeadItemRect.Top);
            }


            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddPolygon(pts);



                IsSelected = false;


                if (path.IsVisible(r.X + r.Width / 2, r.Y + r.Height / 2))
                {

                    IsSelected = true;
                    SelectResult = RectSelectType.Rect;
                }
                if (r.IntersectsWith(new RectangleF(pts[0].X - 2, pts[0].Y - 2, 4, 4)))
                {
                    IsSelected = true;
                    SelectResult = RectSelectType.LT;
                }
                if (r.IntersectsWith(new RectangleF(pts[1].X - 2, pts[1].Y - 2, 4, 4)))
                {
                    IsSelected = true;
                    SelectResult = RectSelectType.LB;
                }
                if (r.IntersectsWith(new RectangleF(pts[2].X - 2, pts[2].Y - 2, 4, 4)))
                {
                    IsSelected = true;
                    SelectResult = RectSelectType.RB;
                }
                if (r.IntersectsWith(new RectangleF(pts[3].X - 2, pts[3].Y - 2, 4, 4)))
                {
                    IsSelected = true;
                    SelectResult = RectSelectType.RT;
                }
            }


            if (IsSelected && !Expand)
            {
                SelectResult = RectSelectType.Rect;
            }

            return IsSelected;
        }
        public override void AddProperties()
        {
            Bag.Properties.Clear();
            Bag.Properties.Add(new PropertySpec("展开折叠", typeof(bool), "图形属性", "展开折叠.", this.Expand));
            Bag.Properties.Add(new PropertySpec("内容颜色", typeof(Color), "图形属性", "内容颜色.", this.ItemTextColor));
            Bag.Properties.Add(new PropertySpec("内容字体", typeof(Font), "图形属性", "内容字体.", this.ItemTextFont));
            Bag.Properties.Add(new PropertySpec("标题背景", typeof(Color), "图形属性", "标题背景.", this.HeadBackColor));
            Bag.Properties.Add(new PropertySpec("标题前景色", typeof(Color), "图形属性", "标题前景色.", this.HeadForeColor));
            Bag.Properties.Add(new PropertySpec("标题颜色", typeof(Color), "图形属性", "标题颜色.", this.HeadTextColor));
            Bag.Properties.Add(new PropertySpec("标题字体", typeof(Font), "图形属性", "标题字体.", this.HeadTextFont));
            Bag.Properties.Add(new PropertySpec("标题边框颜色", typeof(Color), "图形属性", "标题边框颜色.", this.HeadBorderColor));
            Bag.Properties.Add(new PropertySpec("标题边框宽度", typeof(float), "图形属性", "标题边框宽度.", this.HeadBorderWidth));

            Bag.Properties.Add(new PropertySpec("图元背景", typeof(Color), "图形属性", "图元背景.", this.BackColor));
            Bag.Properties.Add(new PropertySpec("图元前景色", typeof(Color), "图形属性", "图元前景色.", this.ForeColor));
            Bag.Properties.Add(new PropertySpec("圆角半径", typeof(int), "图形属性", "圆角半径.", this.Radious));
            Bag.Properties.Add(new PropertySpec("连接线颜色", typeof(Color), "图形属性", "连接线颜色", this.ConnectorLineColor));
            Bag.Properties.Add(new PropertySpec("连接线宽度", typeof(float), "图形属性", "连接线宽度.", this.ConnectorLineWidth));
            if(this.BatchCommandItem!=null)
            { 
            Bag.Properties.Add(new PropertySpec("命令标题", typeof(string), "命令参数", "命令标题.", this.BatchCommandItem.CommandItemTitle));
            Bag.Properties.Add(new PropertySpec("创建时间", typeof(DateTime), "命令参数", "创建时间.", this.BatchCommandItem.CommandCreateTime));
            Bag.Properties.Add(new PropertySpec("如何执行", typeof(BatchCommandItemExecuteTime), "命令参数", "如何执行.", this.BatchCommandItem.CommandExecuteTime));
            Bag.Properties.Add(new PropertySpec("执行方式", typeof(BatchCommandItemExecuteType), "命令参数", "执行方式.", this.BatchCommandItem.CommandExecuteType));
            Bag.Properties.Add(new PropertySpec("延迟时间", typeof(int), "命令参数", "延迟时间(0 表示立即执行)", this.BatchCommandItem.Delayed.TimeSpan));
            Bag.Properties.Add(new PropertySpec("延迟时间类型", typeof(ExecuteTimeType), "命令参数", "延迟时间类型.", this.BatchCommandItem.Delayed.TimeType));
 
            Bag.Properties.Add(new PropertySpec("下置条件设置", typeof(BatchCommandItemWriteValue), "命令参数", "下置条件设置.", this.BatchCommandItem.FixedValue));
            Bag.Properties.Add(new PropertySpec("备注", typeof(string), "命令参数", "备注.", this.BatchCommandItem.Remark));
            Bag.Properties.Add(new PropertySpec("采集站", typeof(string), "命令参数", "采集站.", this.BatchCommandItem.SERVER_ID));
            Bag.Properties.Add(new PropertySpec("下置IO参数绑定", typeof(BachCommand_IOPara), "命令参数", "下置IO参数绑定.", this.BatchCommandItem.IOParaCommand, typeof(IOParaPickerUIEditor), typeof(TypeConverter)));
            Bag.Properties.Add(new PropertySpec("命令启动IO参数绑定", typeof(BachCommand_IOPara), "命令参数", "下置IO参数绑定.", this.BatchCommandItem.IOTriggerParaValue, typeof(IOParaPickerUIEditor), typeof(TypeConverter)  ));
            }
        }

        protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "命令标题":
                    e.Value = this.BatchCommandItem.CommandItemTitle; break;
                case "延迟时间":
                    e.Value = this.BatchCommandItem.Delayed.TimeSpan; break;
                case "延迟时间类型":
                    e.Value = this.BatchCommandItem.Delayed.TimeType; break;
                case "创建时间":
                    e.Value = this.BatchCommandItem.CommandCreateTime; break;
                case "如何执行":
                    e.Value = this.BatchCommandItem.CommandExecuteTime; break;
                case "执行方式":
                    e.Value = this.BatchCommandItem.CommandExecuteType; break;
        
                case "下置条件设置":
                    e.Value = this.BatchCommandItem.FixedValue; break;
                case "备注":
                    e.Value = this.BatchCommandItem.Remark; break;
                case "采集站":
                    e.Value = this.BatchCommandItem.SERVER_ID; break;
                case "下置IO参数绑定":
                    e.Value = this.BatchCommandItem.IOParaCommand; break;
                case "命令启动IO参数绑定":
                    e.Value = this.BatchCommandItem.IOTriggerParaValue; break;

            }
            switch (e.Property.Name)
            {
                case "连接线颜色":
                    e.Value = this.ConnectorLineColor; break;
                case "连接线宽度":
                    e.Value = this.ConnectorLineWidth; break;

                case "展开折叠":
                    e.Value = this.Expand; break;


                case "内容颜色":
                    e.Value = this.ItemTextColor; break;

                case "内容字体":
                    e.Value = this.ItemTextFont; break;

                case "标题背景":
                    e.Value = this.HeadBackColor; break;

                case "标题前景色":
                    e.Value = this.HeadForeColor; break;

                case "标题颜色":
                    e.Value = this.HeadTextColor; break;

                case "标题字体":
                    e.Value = this.HeadTextFont; break;

                case "标题边框颜色":
                    e.Value = this.HeadBorderColor; break;

                case "标题边框宽度":
                    e.Value = this.HeadBorderWidth; break;


                case "图元背景":
                    e.Value = this.BackColor; break;
                case "图元前景色":
                    e.Value = this.ForeColor; break;
                case "圆角半径":
                    e.Value = this.Radious; break;


            }
        }
        protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "命令标题":
                     this.BatchCommandItem.CommandItemTitle = (string)e.Value; break;
                case "创建时间":
                    this.BatchCommandItem.CommandCreateTime = (DateTime)e.Value; break;
                case "如何执行":
                   this.BatchCommandItem.CommandExecuteTime = (BatchCommandItemExecuteTime)e.Value; break;
                case "执行方式":
                      this.BatchCommandItem.CommandExecuteType = (BatchCommandItemExecuteType)e.Value; break;
     
                case "下置条件设置":
                      this.BatchCommandItem.FixedValue = (float)e.Value; break;
                case "备注":
                      this.BatchCommandItem.Remark = (string)e.Value; break;
                case "采集站":
                    this.BatchCommandItem.SERVER_ID = (string)e.Value; break;
                case "下置IO参数绑定":
                    this.BatchCommandItem.IOParaCommand = (BachCommand_IOPara)e.Value; break;
                case "命令启动IO参数绑定":
                   this.BatchCommandItem.IOTriggerParaValue = (BachCommand_IOPara)e.Value; break;

                case "延迟时间":
                     this.BatchCommandItem.Delayed.TimeSpan= (int)e.Value; break;
                case "延迟时间类型":
                    this.BatchCommandItem.Delayed.TimeType= (ExecuteTimeType)e.Value; break;

            }
            switch (e.Property.Name)
            {
                case "连接线颜色":
                    ConnectorLineColor = (Color)e.Value; break;
                case "连接线宽度":
                    ConnectorLineWidth = (float)e.Value; break;

                case "展开折叠":
                    this.Expand = (bool)e.Value; break;



                case "内容颜色":
                    e.Value = this.ItemTextColor = (Color)e.Value; break;

                case "内容字体":
                    e.Value = this.ItemTextFont = (Font)e.Value; break;

                case "标题背景":
                    e.Value = this.HeadBackColor = (Color)e.Value; break;

                case "标题前景色":
                    e.Value = this.HeadForeColor = (Color)e.Value; break;

                case "标题颜色":
                    e.Value = this.HeadTextColor = (Color)e.Value; break;

                case "标题字体":
                    e.Value = this.HeadTextFont = (Font)e.Value; break;

                case "标题边框颜色":
                    e.Value = this.HeadBorderColor = (Color)e.Value; break;

                case "标题边框宽度":
                    e.Value = this.HeadBorderWidth = (float)e.Value; break;


                case "图元背景":
                    e.Value = this.BackColor = (Color)e.Value; break;
                case "图元前景色":
                    e.Value = this.ForeColor = (Color)e.Value; break;
                case "圆角半径":
                    e.Value = this.Radious = (float)e.Value; break;


            }
            this.Invalidate();
        }

    }

}
