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

        public PointF Center;
        public float ZoomFactor = 1;

        public ViewController(RectangleF rectangle, PointF center)
        {
            Rectangle = rectangle;
            Center = center;
        }



        public void Zoom(object sender, MouseEventArgs args)
        {
            float zoomFactor = (float)Math.Exp(args.Delta / 1000f);
            PointF corner = Rectangle.Location;
            PointF distance = new PointF(corner.X - Center.X, corner.Y - Center.Y);
            Rectangle = new RectangleF(Center.X + distance.X * zoomFactor,
                                       Center.Y + distance.Y * zoomFactor,
                                       Rectangle.Width * zoomFactor,
                                       Rectangle.Height * zoomFactor);
            ZoomFactor *= zoomFactor;
        }
        public void Move(int x, int y)
        {
            Rectangle = new RectangleF(Rectangle.X + 10 * x, Rectangle.Y + 10 * y, Rectangle.Width, Rectangle.Height);
        }
    }
}
