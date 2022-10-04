using System;
using System.Drawing;

namespace Scada.BatchCommand
{
    [Serializable]
    /// <summary>
    /// 定义一个符号库的绘图坐标区域，主要是为了兼容resform
    /// </summary>
    public class BatchCommandMapArea
    {
        public float Left = 0;
        public float Top = 0;
        public float Right = 1000;
        public float Bottom = 1000;
        /// <summary>
        /// 坐标系原点
        /// </summary>
        public PointF ClientCenter
        {
            get {
                return new PointF(ClientRect.X+ ClientRect.Width/2, ClientRect.Y+ ClientRect.Height/2);
            }
        }
        public RectangleF ClientRect
        {
            get
            {
                RectangleF r = new RectangleF();
                r.X = 1;
                r.Y = 1;
                r.Width = 1500;
                r.Height = 1500;
                return r;

            }
        }

        public float Width
        {
            get { return Math.Abs(Right - Left); }

        }

        public float Height
        {
            get { return Math.Abs(Bottom - Top); }

        }
        #region 用户坐标转逻辑坐标

        /// <summary>
        /// 用户坐标转换为屏幕坐标
        /// </summary>
        /// <param name="ux"></param>
        /// <param name="uy"></param>
        /// <param name="lx"></param>
        /// <param name="ly"></param>
        public void ReturnUToL(float ux, float uy, ref float lx, ref float ly)
        {
            ReturnUToL_X(ux, ref lx);
            ReturnUToL_Y(uy, ref ly);
        }
        //x方向转换
        private void ReturnUToL_X(float ux, ref float lx)
        {
            lx = this.ClientRect.X + (ux - this.Left) / (this.Right - this.Left) * this.ClientRect.Width;

        }
        /// <summary>
        /// Y方向转换
        /// </summary>
        /// <param name="uy"></param>
        /// <param name="ly"></param>
        private void ReturnUToL_Y(float uy, ref float ly)
        {
            ly = this.ClientRect.Y + (this.Top - uy) / (this.Top - this.Bottom) * this.ClientRect.Height; ;

        }
        public void ReturnUToL_Point(PointF up, ref PointF lp)
        {
            float ux = up.X;
            float uy = up.Y;
            float lx = lp.X;
            float ly = lp.Y;
            ReturnUToL(ux, uy, ref lx, ref ly);
            lp = new PointF(lx, ly);
        }
        public void ReturnUToL_Rectangle(RectangleF up, ref RectangleF lp)
        {

          
            float ux = up.X;
            float uy = up.Y;
            float lx = lp.X;
            float ly = lp.Y;
            ReturnUToL(ux, uy, ref lx, ref ly);
            lp.X = lx;
            lp.Y = ly;
            ux = up.Right;
            uy = up.Bottom;
            lx = lp.Right;
            ly = lp.Bottom;
            ReturnUToL(ux, uy, ref lx, ref ly);
            lp.Width = lx - lp.X;
            lp.Height = Math.Abs(ly - lp.Y);
          
        }
        /// <summary>
        /// 求取用户坐标上的一段距离转换为逻辑坐标距离X方向
        /// </summary>
        /// <param name="distance"></param>
        public float ReturnUToL_LineX(float distance)
        {
            float x = this.Left;
            float x2 = this.Left + distance;
            float lx = 0;
            float lx2 = 0;
            ReturnUToL_X(x, ref lx);
            return Math.Abs(lx2 - lx);
        }
        /// <summary>
        /// 求取用户坐标上的一段距离转换为逻辑坐标距离Y方向
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public float ReturnUToL_LineY(float distance)
        {
            float y = this.Bottom;
            float y2 = this.Bottom + distance;
            float ly = 0;
            float ly2 = 0;
            ReturnUToL_Y(y, ref ly);
            return Math.Abs(ly2 - ly);
        }
        #endregion
        #region 逻辑坐标转用户坐标

        /// <summary>
        /// 求取逻辑坐标上的一段距离转换为屏幕坐标距离
        /// </summary>
        /// <param name="distance"></param>
        public float ReturnLToU_LineX(float distance)
        {
            float lx = distance;

            float lx2 = 0;
            ReturnLToU_X(0, ref lx);
            ReturnLToU_X(distance, ref lx2);
            return Math.Abs(lx2 - lx);
        }
        public float ReturnLToU_LineY(float distance)
        {
            float ly = distance;
            float ly2 = 0;
            ReturnLToU_Y(distance, ref ly);
            ReturnLToU_Y(0, ref ly2);
            return Math.Abs(ly2 - ly);
        }
        /// <summary>
        /// 屏幕坐标转换为用户坐标
        /// </summary>
        /// <param name="lx"></param>
        /// <param name="ly"></param>
        /// <param name="ux"></param>
        /// <param name="uy"></param>
        public void ReturnLToU(float lx, float ly, ref float ux, ref float uy)
        {
            ReturnLToU_X(lx, ref ux);
            ReturnLToU_Y(ly, ref uy);

        }
        public void ReturnLToU_Point(PointF lp, ref PointF up)
        {
            float ux = up.X;
            float uy = up.Y;
            float lx = lp.X;
            float ly = lp.Y;
            ReturnLToU(lx, ly, ref ux, ref uy);
            up = new PointF(ux, uy);
        }
        public void ReturnLToU_Rectangle(RectangleF lp, ref RectangleF up)
        {

          
            float ux = up.X;
            float uy = up.Y;
            float lx = lp.X;
            float ly = lp.Y;
            ReturnLToU(lx, ly, ref ux, ref uy);
            up.X = ux;
            up.Y = uy;
            ux = up.Right;
            uy = up.Bottom;
            lx = lp.Right;
            ly = lp.Bottom;
            ReturnLToU(lx, ly, ref ux, ref uy);
            up.Width = ux - up.X;
            up.Height = uy - up.Y;
        }
        private void ReturnLToU_X(float lx, ref float ux)
        {
            ux = ((lx - this.ClientRect.X) / this.ClientRect.Width) * (this.Right - this.Left) + this.Left;

        }
        private void ReturnLToU_Y(float ly, ref float uy)
        {
            uy = ((ly - this.ClientRect.Y) / this.ClientRect.Height) * (this.Top - this.Bottom) - this.Top;
            uy = -uy;
        }
        #endregion

       
    }
}
