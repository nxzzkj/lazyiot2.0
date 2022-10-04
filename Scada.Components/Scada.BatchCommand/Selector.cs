using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.BatchCommand
{
    [Serializable]
    public class BatchCommandSelector
    {



        public BatchCommandElementType DrawType = BatchCommandElementType.NONE;
        public BatchCommandShape ActiveShape = null;
        public BatchCommandTaskGraph Site = null;
        public BatchCommandSelector(PointF s, BatchCommandTaskGraph site, BatchCommandElementType dtype)
        {
            Site = site;

            DrawType = dtype;
            switch (DrawType)
            {
                case BatchCommandElementType.命令:
                    {
                        BatchCommandItemShape commandShape = new BatchCommandItemShape();
                        commandShape.Site = site;
                        commandShape.Rectangle = new RectangleF(s.X, s.Y, 100, 200);
                  
                        ActiveShape = commandShape;
                        break;
                    }
                

            }
            IsEnd = false;

        }
        public void Update(PointF p)
        {
            if (ActiveShape != null)
                ActiveShape.Update(p);
            Site.Invalidate();

        }
        public void AddPoint(PointF p)
        {
            if (ActiveShape != null)
                ActiveShape.AddPoint(p);

        }
        public void Paint(Graphics graphics)
        {
            if (ActiveShape != null)
            {
                ActiveShape.Paint(graphics);
            }
        }
        public bool IsEnd = true;
        public void DrawEnd(bool end)
        {



            if (end)
            {
                if (DrawType != BatchCommandElementType.NONE)
                    Site.AddShape(ActiveShape);
            }
            if (DrawType != BatchCommandElementType.NONE)
            {
                ActiveShape = null;
                DrawType = BatchCommandElementType.NONE;
                IsEnd = true;
            }
            

        }
    }
}
