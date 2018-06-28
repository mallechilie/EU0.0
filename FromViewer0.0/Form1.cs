using System;
using System.Drawing;
using System.Windows.Forms;
using Enumeration;

namespace FromViewer
{
    internal partial class Viewer : Form
    {
        private ViewController controller;
        private Graphics graphics;
        private int x, y;
        private readonly MapViewer map;
        private readonly bool resizable;
        public bool wrapAround;

        public Viewer(MapViewer map, bool resizable = true)
        {
            KeyPreview = true;
            this.map = map;
            wrapAround = map.Torus;
            this.resizable = resizable;
            controller = new ViewController(new RectangleF(0, 0, ClientSize.Width, ClientSize.Height));
            InitializeComponent();
            MouseMove += DrawMouse;
            MouseWheel += controller.Zoom;
            MouseWheel += (sender, args) => { Draw(); };
            graphics = CreateGraphics();
        }


        private void ResetMap()
        {
            if (resizable)
            {
                map.Width = ClientSize.Width;
                map.Height = ClientSize.Height;
                graphics = CreateGraphics();
            }
            map?.ResetMap();
            controller.Rectangle = new RectangleF(0, 0, ClientSize.Width, ClientSize.Height);
            Draw();
        }
        protected override void OnResize(EventArgs e)
        {
            graphics = CreateGraphics();
            base.OnResize(e);
            RectangleF rect = controller.Rectangle;
            controller.Rectangle = new RectangleF(rect.X, rect.Y, Width * controller.ZoomFactor, Height * controller.ZoomFactor);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw();
        }
        private void Draw()
        {
            DrawTiles();
        }

        private void DrawMouse(Object o, MouseEventArgs mea)
        {
            x = PointToClient(Cursor.Position).X * (map.Width + 1) / ClientSize.Width;
            if (x >= map.Width || x < 0)
                return;
            y = PointToClient(Cursor.Position).Y * (map.Height + 1) / ClientSize.Height;
            if (y >= map.Height || y < 0)
                return;
        }
        

        private void DrawTiles()
        {
            Bitmap bitmap = map.GetBitmap();
            RectangleF rect = controller.Rectangle;
            if (wrapAround)
            {
                for (float x = rect.X % rect.Width - rect.Width; x < ClientSize.Width; x += rect.Width)
                    for (float y = rect.Y % rect.Height - rect.Height; y < ClientSize.Height; y += rect.Height)
                        graphics.DrawImage(bitmap, new RectangleF(x, y, rect.Width, rect.Height));
            }
            else
            {

                graphics.DrawImage(bitmap, rect);
                Brush back = new SolidBrush(BackColor);
                graphics.FillRectangle(back, new RectangleF(0, 0, rect.X, ClientSize.Height));
                graphics.FillRectangle(back, new RectangleF(rect.X + rect.Width, 0, ClientSize.Width, ClientSize.Height));
                graphics.FillRectangle(back, new RectangleF(0, 0, ClientSize.Width, rect.Y));
                graphics.FillRectangle(back, new RectangleF(0, rect.Y + rect.Height, ClientSize.Width, ClientSize.Height));
            }

            return;
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    DrawTile(x, y, map.GetColor(x, y));
        }

        private void DrawTile(int x, int y, Color color)
        {
            switch (map.TileTopology)
            {
                case TileShape.Square:
                    {
                        Rectangle rect = new Rectangle((int)((float)ClientSize.Width / (map.Width + 1) * x),
                            (int)((float)ClientSize.Height / (map.Height + 1) * y),
                            (int)((float)ClientSize.Width / (map.Width + 1)) + 1,
                            (int)((float)ClientSize.Height / (map.Height + 1)) + 1);
                        //graphics.DrawRectangle(Pens.Black, rect);
                        graphics.FillRectangle(new SolidBrush(color), rect);
                    }
                    break;
            }
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // TODO: less hacky, no override of ProcessCmdKey
            bool changed = false;
            switch (keyData)
            {
                case Keys.Right:
                    controller.Move(-1, 0);
                    changed = true;
                    break;
                case Keys.Left:
                    controller.Move(1, 0);
                    changed = true;
                    break;
                case Keys.Up:
                    controller.Move(0, 1);
                    changed = true;
                    break;
                case Keys.Down:
                    controller.Move(0, -1);
                    changed = true;
                    break;
            }
            if (changed)
                Draw();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetMap();
        }
    }
}
