using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Home_assistant
{
    public class RoundButton : Button
    {
        public Color BorderColor { get; set; } = Color.DimGray;
        public int BorderRadius { get; set; } = 20;
        public int BorderSize { get; set; } = 1;
        public Color HoverBackColor { get; set; } = Color.LightGray;

        // 🔧 Nieuw: originele achtergrondkleur bewaren
        public Color DefaultBackColor { get; set; }

        public RoundButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.White;
            DefaultBackColor = BackColor;
            ForeColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -BorderSize, -BorderSize);

            int radius = BorderRadius;

            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, radius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, radius))
            using (Pen penSurface = new Pen(this.Parent.BackColor, BorderSize))
            using (Pen penBorder = new Pen(BorderColor, BorderSize))
            {
                Region = new Region(pathSurface);
                pevent.Graphics.DrawPath(penSurface, pathSurface);
                pevent.Graphics.DrawPath(penBorder, pathBorder);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.BackColor = HoverBackColor;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = DefaultBackColor;
            this.Invalidate();
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
