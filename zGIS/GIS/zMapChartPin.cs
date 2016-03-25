using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace zGIS
{    
    public class zMapChartPin
    {

        private string _name = "";

        public float Value = 0;
        public string Text = "";
        public PointF Point;
        public Color Color = Color.Red;
        public PinStyle Style = PinStyle.Pin;
        public Color BorderColor = Color.Black;
        public Color FontColor = Color.Black;
        public int FontSize = 12;
        public FontStyle FontStyle = FontStyle.Regular;
        public string TipText = "";
        public int PinSize = 0;



        public enum PinStyle
        {
            TextOnly,
            Circle,
            Square,
            Diamond,
            Triangle,
            Cross,
            Star,
            ColoredCircle,
            ColoredSquare,
            ColoredDiamond,
            ColoredTriangle,
            ColoredCross,
            ColoredStar,
            Pin,
            Icon
        }



        public zMapChartPin(string name, PointF point)
        {

            _name = name;
            Point = point;

        }

        public zMapChartPin(string name)
        {

            _name = name;

        }

        public zMapChartPin(string name, PointF point, Color pinColor)
        {

            _name = name;
            Point = point;
            Color = pinColor;
        }

        public zMapChartPin(string name, PointF point, Color pinColor, string text)
        {

            _name = name;
            Point = point;
            Color = pinColor;
            Text = text;
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
