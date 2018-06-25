using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FromViewer
{
    class ViewController
    {
        public RectangleF Rectangle
        {
            get;
            //TODO: make private again
            set;
        }

        public ViewController(RectangleF rectangle)
        {
            Rectangle = rectangle;
        }



        public void Zoom(object sender, MouseEventArgs args)
        {
            float zoomFactor = (float)Math.Exp(args.Delta/1000f);
            PointF center = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);
            PointF newCorner = new PointF(center.X - Rectangle.Width * zoomFactor / 2, center.Y - Rectangle.Height * zoomFactor / 2);
            Rectangle = new RectangleF(Rectangle.X - Rectangle.Width * (zoomFactor - 1) / 2,
                                       Rectangle.Y - Rectangle.Height * (zoomFactor - 1) / 2,
                                       Rectangle.Width * zoomFactor,
                                       Rectangle.Height * zoomFactor);
        }
        public void Move(int x, int y)
        {
            Rectangle = new RectangleF(Rectangle.X + 10 * x, Rectangle.Y + 10 * y, Rectangle.Width, Rectangle.Height);
        }
    }
}
