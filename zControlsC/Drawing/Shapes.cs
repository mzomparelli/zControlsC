using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace zControlsC.Drawing
{
    public static class Shapes
    {

        public static PointF[] Diamond(PointF centerPoint, SizeF size)
        {
            PointF[] diamond = new PointF[4];
            diamond[0] = new PointF(centerPoint.X, centerPoint.Y - (size.Height / 2));
            diamond[1] = new PointF(centerPoint.X + (size.Width / 2) + 4, centerPoint.Y);
            diamond[2] = new PointF(centerPoint.X, centerPoint.Y + (size.Height / 2));
            diamond[3] = new PointF(centerPoint.X - (size.Width / 2) - 4, centerPoint.Y);
            return diamond;
        }

        public static PointF[] Cross(PointF centerPoint, SizeF size)
        {
            PointF[] cross = new PointF[12];
            cross[0] = new PointF(centerPoint.X - (size.Width / 2) + (size.Width / 3), centerPoint.Y - (size.Height / 2));
            cross[1] = new PointF(centerPoint.X - (size.Width / 2) + ((size.Width / 3) * 2), centerPoint.Y - (size.Height / 2));
            cross[2] = new PointF(centerPoint.X - (size.Width / 2) + ((size.Width / 3) * 2), centerPoint.Y - (size.Height / 2) + (size.Height / 3));
            cross[3] = new PointF(centerPoint.X - (size.Width / 2) + ((size.Width / 3) * 3), centerPoint.Y - (size.Height / 2) + (size.Height / 3));
            cross[4] = new PointF(centerPoint.X - (size.Width / 2) + ((size.Width / 3) * 3), centerPoint.Y - (size.Height / 2) + ((size.Height / 3) * 2));
            cross[5] = new PointF(centerPoint.X - (size.Width / 2) + ((size.Width / 3) * 2), centerPoint.Y - (size.Height / 2) + ((size.Height / 3) * 2));
            cross[6] = new PointF(centerPoint.X - (size.Width / 2) + ((size.Width / 3) * 2), centerPoint.Y - (size.Height / 2) + ((size.Height / 3) * 3));
            cross[7] = new PointF(centerPoint.X - (size.Width / 2) + (size.Width / 3), centerPoint.Y - (size.Height / 2) + ((size.Height / 3) * 3));
            cross[8] = new PointF(centerPoint.X - (size.Width / 2) + (size.Width / 3), centerPoint.Y - (size.Height / 2) + ((size.Height / 3) * 2));
            cross[9] = new PointF(centerPoint.X - (size.Width / 2), centerPoint.Y - (size.Height / 2) + ((size.Height / 3) * 2));
            cross[10] = new PointF(centerPoint.X - (size.Width / 2), centerPoint.Y - (size.Height / 2) + (size.Height / 3));
            cross[11] = new PointF(centerPoint.X - (size.Width / 2) + (size.Width / 3), centerPoint.Y - (size.Height / 2) + (size.Height / 3));
            return cross;
        }

        public static PointF[] Star(PointF centerPoint, SizeF size)
        {
            PointF[] star = new PointF[10];
            float sin36 = (float)Math.Sin(36.0 * Math.PI / 180.0);
            float sin72 = (float)Math.Sin(72.0 * Math.PI / 180.0);
            float cos36 = (float)Math.Cos(36.0 * Math.PI / 180.0);
            float cos72 = (float)Math.Cos(72.0 * Math.PI / 180.0);
            float r = size.Width;
            float r1 = r * cos72 / cos36;
            star[0] = new PointF(centerPoint.X, centerPoint.Y - r);
            star[1] = new PointF(centerPoint.X + r1 * sin36, centerPoint.Y - r1 * cos36);
            star[2] = new PointF(centerPoint.X + r * sin72, centerPoint.Y - r * cos72);
            star[3] = new PointF(centerPoint.X + r1 * sin72, centerPoint.Y + r1 * cos72);
            star[4] = new PointF(centerPoint.X + r * sin36, centerPoint.Y + r * cos36);
            star[5] = new PointF(centerPoint.X, centerPoint.Y + r1);
            star[6] = new PointF(centerPoint.X - r * sin36, centerPoint.Y + r * cos36);
            star[7] = new PointF(centerPoint.X - r1 * sin72, centerPoint.Y + r1 * cos72);
            star[8] = new PointF(centerPoint.X - r * sin72, centerPoint.Y - r * cos72);
            star[9] = new PointF(centerPoint.X - r1 * sin36, centerPoint.Y - r1 * cos36);
            return star;
        }

        public static Rectangle Rectangle(PointF centerPoint, SizeF size)
        {
            return new Rectangle((int)(centerPoint.X - (size.Width / 2)), (int)(centerPoint.Y - (size.Height / 2)), (int)(size.Width), (int)(size.Height));
        }

        public static GraphicsPath RoundedRectangle(PointF centerPoint, SizeF size, int cornerSize, int borderSize)
        {
            GraphicsPath p = new GraphicsPath();
            Rectangle r = Rectangle(centerPoint, size);
            int i = cornerSize;
            int j = borderSize;

            p.StartFigure();
            //Upper Left Corner
            p.AddArc(new Rectangle(r.X + -1, r.Y + -1, i, i), 180, 90);

            //Top Line
            p.AddLine(r.X + j, r.Y + 0, r.X + r.Width - j, r.Y + 0);

            //Upper Right Corner
            p.AddArc(new Rectangle(r.X + r.Width - i, r.Y + -1, i, i), 270, 90);
            
            //Right Line
            p.AddLine(r.X + r.Width, r.Y + j, r.X + r.Width, r.Y + r.Height - j);

            //Lower Right Corner
            p.AddArc(new Rectangle(r.X + r.Width - i, r.Y + r.Height - i, i, i), 0, 90);

            //Bottom Line
            p.AddLine(r.X + j, r.Y + r.Height, r.X + r.Width - j, r.Y + r.Height);

            //Lower Left Corner
            p.AddArc(new Rectangle(r.X + -1, r.Y + r.Height - i, i, i), 90, 90);

            //Left Line
            p.AddLine(r.X + 0, r.Y + j, r.X + 0, r.Y + r.Height - j);

            p.CloseFigure();

            return p;
        }

        public static GraphicsPath RoundedRectangle(Rectangle r, int cornerSize, int borderSize)
        {
            GraphicsPath p = new GraphicsPath();
            int i = cornerSize;
            int j = borderSize;

            p.StartFigure();
            //Upper Left Corner
            p.AddArc(new Rectangle(r.X + -1, r.Y + -1, i, i), 180, 90);

            //Top Line
            p.AddLine(r.X + j, r.Y + 0, r.X + r.Width - j, r.Y + 0);

            //Upper Right Corner
            p.AddArc(new Rectangle(r.X + r.Width - i, r.Y + -1, i, i), 270, 90);

            //Right Line
            p.AddLine(r.X + r.Width, r.Y + j, r.X + r.Width, r.Y + r.Height - j);

            //Lower Right Corner
            p.AddArc(new Rectangle(r.X + r.Width - i, r.Y + r.Height - i, i, i), 0, 90);

            //Bottom Line
            p.AddLine(r.X + j, r.Y + r.Height, r.X + r.Width - j, r.Y + r.Height);

            //Lower Left Corner
            p.AddArc(new Rectangle(r.X + -1, r.Y + r.Height - i, i, i), 90, 90);

            //Left Line
            p.AddLine(r.X + 0, r.Y + j, r.X + 0, r.Y + r.Height - j);

            p.CloseFigure();

            return p;
        }

        public static PointF[] Triangle(PointF centerPoint, SizeF size)
        {
            PointF[] triangle = new PointF[3];
            triangle[0] = new PointF(centerPoint.X, centerPoint.Y - (size.Height / 2));
            triangle[1] = new PointF(centerPoint.X - (size.Width / 2), centerPoint.Y - (size.Height / 2) + size.Height);
            triangle[2] = new PointF(centerPoint.X - (size.Width / 2) + size.Width, centerPoint.Y - (size.Height / 2) + size.Height);
            return triangle;
        }

        public static GraphicsPath Ellipse(PointF centerPoint, SizeF size)
        {
            Rectangle r = Rectangle(centerPoint, size);
            GraphicsPath ellipse = new GraphicsPath();
            ellipse.StartFigure();
            ellipse.AddEllipse(r);
            ellipse.CloseFigure();
            return ellipse;
        }

        

    }
}
