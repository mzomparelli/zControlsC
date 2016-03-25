using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using zControlsC;
using zControlsC.GIS.NorthAmerica;
using System.Runtime.InteropServices;
using zControlsC.Drawing;
using zControlsC.Properties;
using System.IO;
using vb = Microsoft.VisualBasic;

namespace zControlsC.Charts
{
    public partial class zMapChart : UserControl, IDisposable
    {
        

    #region "Initialize"

        public zMapChart()
        {
            InitializeComponent();

           
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            ReDrawTimer.Interval = 50;
            ReDrawTimer.Tick += new EventHandler(ReDrawTimer_Tick);
            ReDrawTimer.Start();
            bitmap = new Bitmap(this.Width, this.Height);
            gBitmap = Graphics.FromImage(bitmap);
            gBitmap.SmoothingMode = SmoothingMode.AntiAlias;
            gBitmap.CompositingQuality = CompositingQuality.HighSpeed;

            //Load the states and zipcodes
            //LoadStates();
            //LoadZipcodes();           
            

        }

        public void LoadMapFiles()
        {
            CreateFiles();
            //this.StateGeoCodes = Serialize.DeSerializeCompressed<List<StateBorder>>(Application.StartupPath + "\\NorthAmerica_States3.geo");
            //this._Zip3CenterPoints = Serialize.DeSerializeCompressed<List<Zip3CenterPoint>>(Application.StartupPath + "\\US_3Zip_CenterPoints.geo");
            //this._Zip3GeoCodes = Serialize.DeSerializeCompressed<List<Zip3Border>>(Application.StartupPath + "\\US_3Zip.geo");
        }

        private void CreateFiles()
        {
            string dllLoc = Application.StartupPath + "\\C1.C1Zip.2.dll";

            if (!File.Exists(dllLoc))
            {
                byte[] b = Resources.C1_C1Zip_2;
                FileStream tempFile = File.Create(dllLoc);
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }

            string fName = Application.StartupPath + "\\NorthAmerica_States3.geo";
            if (!File.Exists(fName))
            {
                byte[] b = Resources.NorthAmerica_States3;
                FileStream tempFile = File.Create(fName);
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }

            fName = Application.StartupPath + "\\US_3Zip_CenterPoints.geo";
            if (!File.Exists(fName))
            {
                byte[] b = Resources.US_3Zip_CenterPoints;
                FileStream tempFile = File.Create(fName);
                tempFile.Write(b, 0, b.Length);
                tempFile.Close();
                tempFile.Dispose();
                tempFile = null;
            }


        }

        void ReDrawTimer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

    #endregion


    #region "Declarations"

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        private Bitmap bitmap;
        private Graphics gBitmap;

        List<PointF[]> geoStates = new List<PointF[]>();
        List<PointF[]> projectedStates = new List<PointF[]>();

        List<PointF[]> geoZips = new List<PointF[]>();
        List<PointF[]> projectedZips = new List<PointF[]>();

        List<int> hoverPath = new List<int>();
        List<zMapChartPin> hoverPins = new List<zMapChartPin>();
        List<zMapChart_P2P_Line> hoverLines = new List<zMapChart_P2P_Line>();

        //GraphicsPaths
        GraphicsPath[] gpState;
        GraphicsPath[] gpZip;
        GraphicsPath[] gpPin;
        GraphicsPath[] gpLine;

        List<StateBorder> _StateGeoCodes;
        List<Zip3Border> _Zip3GeoCodes;
        List<Zip3CenterPoint> _Zip3CenterPoints;

        List<zMapChartPin> _pins = new List<zMapChartPin>();
        List<zMapChart_P2P_Line> _lines = new List<zMapChart_P2P_Line>();

        private RectangleF viewport = RectangleF.FromLTRB(
                        -14783105.0f,
                        -4093599.0f,
                        -7563799.0f,
                        -3959886.5f);


        PointF geographicCoordinate;
        PointF projectedCoordinate;


        //Matrix
        public Matrix transformMap = new Matrix();
        public Matrix transformNormal = new Matrix();

        //Graphics
        public Graphics Graphics;


        //Colors
        private SolidBrush brush = new SolidBrush(Color.Green);
        private Pen pen = new Pen(Color.Black, 0.001f);


        //Mouse Coordinates
        private float x;
        private float y;

        public LocationType MouseCoords;

        private Boolean _drawStates = true;
        private Boolean _drawZip3 = false;

        private Timer ReDrawTimer = new Timer();

        private bool invalidating = false;
            


    #endregion

    #region "Properties"

        public List<zMapChart_P2P_Line> Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }

        public List<Zip3CenterPoint> Zip3CenterPoints
        {
            get { return _Zip3CenterPoints; }
        }

        public Boolean DrawStates
        {
            get { return _drawStates; }
            set { _drawStates = value; }
        }

        public Boolean DrawZip3
        {
            get { return _drawZip3; }
            set { _drawZip3 = value; }
        }

        public List<StateBorder> StateGeoCodes
        {
            get { return _StateGeoCodes; }
            set 
            { 
                _StateGeoCodes = value;
                if (_StateGeoCodes != null)
                {
                    gpState = new GraphicsPath[_StateGeoCodes.Count];
                    int i = 0;
                    foreach (StateBorder state in _StateGeoCodes)
                    {
                        gpState[i] = new GraphicsPath();
                        i++;
                    }
                }
            }

        }

        public List<Zip3Border> Zip3GeoCodes
        {
            get { return _Zip3GeoCodes; }
            set
            {
                _Zip3GeoCodes = value;
                if (_Zip3GeoCodes != null)
                {
                    gpZip = new GraphicsPath[_Zip3GeoCodes.Count];
                    int i = 0;
                    foreach (Zip3Border state in _Zip3GeoCodes)
                    {
                        gpZip[i] = new GraphicsPath();
                        i++;
                    }
                }
            }

        }



        public struct LocationType
        {
            public PointF Pixel;
            public PointF LatLong;
            public PointF Projected;
        }

    #endregion

    #region "Public Methods"

        public PointF Zip3Center(string zip3)
        {
            if (zip3.Length < 3)
            {
                return new PointF(0, 0);
            }

            foreach (Zip3CenterPoint z in this._Zip3CenterPoints)
            {
                if (z.Zip3 == vb.Strings.Left(zip3, 3))
                {
                    return z.CenterPoint;
                }
            }


            return new PointF(0, 0);
            
        }



        public virtual void ZoomIn()
        {
            float zoomWidthAmount = -viewport.Width * 0.10f;
            float zoomHeightAmount = -viewport.Height * 0.10f;
            viewport.Inflate(zoomWidthAmount, zoomHeightAmount);
            SetStatusBar();
            CenterViewport(new PointF(geographicCoordinate.X, geographicCoordinate.Y));
            TransformToMap();
            //Invalidate();
        }

        public virtual void ZoomOut()
        {
            float zoomWidthAmount = viewport.Width * 0.10f;
            float zoomHeightAmount = viewport.Height * 0.10f;
            viewport.Inflate(zoomWidthAmount, zoomHeightAmount);
            SetStatusBar();
            CenterViewport(new PointF(geographicCoordinate.X, geographicCoordinate.Y));
            TransformToMap();
            //Invalidate();
        }

        public virtual void PanUp()
        {
            float zoomHeightAmount = (-viewport.Height * 0.10f);
            viewport.Offset(0.0f, zoomHeightAmount);
            SetStatusBar();
            TransformToMap();
        }

        public virtual void PanDown()
        {
            float zoomHeightAmount = (viewport.Height * 0.10f);
            viewport.Offset(0.0f, zoomHeightAmount);
            SetStatusBar();
            TransformToMap();
        }

        public virtual void PanLeft()
        {
            float zoomHeightAmount = (-viewport.Width * 0.10f);
            viewport.Offset(zoomHeightAmount, 0.0f);
            SetStatusBar();
            TransformToMap();
        }

        public virtual void PanRight()
        {
            float zoomHeightAmount = (viewport.Width * 0.10f);
            viewport.Offset(zoomHeightAmount, 0.0f);
            SetStatusBar();
            TransformToMap();
        }

        public Point GeoToPixel(PointF coords)
        {
            PointF[] p = new PointF[1];
            p[0] = coords;
            p[0] = zConversion.Project(p[0]);
            transformMap.TransformPoints(p);
            return new Point((int)Math.Round(p[0].X, 0), (int)Math.Round(p[0].Y, 0));
        }

        public PointF PixelToGeo(Point pixel)
        {
            Matrix reverseTransform = transformMap.Clone();
            reverseTransform.Invert();
            Point[] projectedCoordinates = new Point[] { pixel };
            reverseTransform.TransformPoints(projectedCoordinates);
            PointF geographicCoordinate = zConversion.Deproject(projectedCoordinates[0]);
            return geographicCoordinate;
        }

        public virtual void CenterViewport(float latitude, float longitude)
        {
            CenterViewport(new PointF(latitude, longitude));
        }

        public virtual void CenterViewport(PointF centerLatLongCoord)
        {
            PointF pLT = zConversion.Deproject(new PointF(viewport.Left, viewport.Top));
            PointF pS = zConversion.Deproject(viewport.Size.ToPointF());

            float x1 = pLT.X + (pS.X / 2);
            float y1 = -pLT.Y - (pS.Y / 2);

            float x2 = centerLatLongCoord.X;
            float y2 = centerLatLongCoord.Y;

            PointF offset = new PointF(x1 - x2, y1 - y2);
            offset = zConversion.Project(offset);
            //viewport.Location = new PointF(viewport.Location.X + -offset.X, viewport.Location.Y + offset.Y);
            viewport.Offset(new PointF(-offset.X, offset.Y));
            TransformToMap();
            //PointF pixel = GeoToPixel(centerCoord);

            //The next block of code is used to determine where to put the mouse.
            //I need to add the locations of every container until we find the root container which is a form class.
            int pixelX = 0;
            int pixelY = 0;

            try
            {
                pixelX += this.Parent.Location.X;
                pixelY += this.Parent.Location.Y;

                pixelX += this.Parent.Parent.Location.X;
                pixelY += this.Parent.Parent.Location.Y;

                pixelX += this.Parent.Parent.Parent.Location.X;
                pixelY += this.Parent.Parent.Parent.Location.Y;

                pixelX += this.Parent.Parent.Parent.Parent.Location.X;
                pixelY += this.Parent.Parent.Parent.Parent.Location.Y;

                pixelX += this.Parent.Parent.Parent.Parent.Parent.Location.X;
                pixelY += this.Parent.Parent.Parent.Parent.Parent.Location.Y;

                pixelX += this.Parent.Parent.Parent.Parent.Parent.Parent.Location.X;
                pixelY += this.Parent.Parent.Parent.Parent.Parent.Parent.Location.Y;
            }
            catch
            {

            }

            Point p = GeoToPixel(centerLatLongCoord);
            Point pixel = new Point(pixelX + this.Location.X + 4 + p.X, pixelY + this.Location.Y + 30 + p.Y);
            SetCursorPos(pixel.X, pixel.Y);
            //Invalidate();


        }

        public void Add_P2P_Line(zMapChart_P2P_Line line)
        {
            _lines.Add(line);
            int i = 0;
            gpLine = new GraphicsPath[_lines.Count];
            foreach (zMapChart_P2P_Line l in _lines)
            {
                gpLine[i] = new GraphicsPath();
                i++;
            }
        }

        public void AddPin(zMapChartPin pin)
        {
            _pins.Add(pin);
            int i = 0;
            gpPin = new GraphicsPath[_pins.Count];
            foreach (zMapChartPin p in _pins)
            {
                gpPin[i] = new GraphicsPath();
                i++;
            }
        }

        public void SaveImage(string fileName, System.Drawing.Imaging.ImageFormat format)
        {
            bitmap.Save(fileName, format);
        }
        

    #endregion

    #region "Private Methods"

        private void TransformToMap()
        {
            AdjustViewportAspectRatio();
            transformMap.Reset();
            transformMap.Translate(-viewport.X, viewport.Y, MatrixOrder.Append);
            transformMap.Scale(this.Width / viewport.Width, this.Height / -viewport.Height, MatrixOrder.Append);
        }

        private void AdjustViewportAspectRatio()
        {
            float pixelAspectRatio = (float)(this.Width) / this.Height;
            float projectedAspectRatio = (viewport.Width) / viewport.Height;

            // Is the Form's aspect ratio larger than the viewport's ratio?
            if (pixelAspectRatio > projectedAspectRatio)
            {
                // Yes.  Increase the width of the viewport
                viewport.Inflate(
                    ((pixelAspectRatio * viewport.Height) - viewport.Width) / 2,
                    0);
            }
            // Is the viewport's aspect ratio larger than the form's ratio?
            else if (pixelAspectRatio < projectedAspectRatio)
            {
                // Yes.  Increase the height of the viewport
                viewport.Inflate(
                    0,
                    ((viewport.Width / pixelAspectRatio) - viewport.Height) / 2);
            }

            
            //Add 12.5% to the height to make the map closer to actual proportions
            viewport.Inflate(
                    0,
                    -viewport.Height * 0.125f);
        }


        private void DrawStatesEx(ref PaintEventArgs e)
        {

            if (_drawStates == false)
            {
                return;
            }

            int path = 0;

            e.Graphics.Transform = transformMap;
            gBitmap.Transform = transformMap;

            SolidBrush brush = new SolidBrush(Color.Green);
            SolidBrush brushWhite = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black, 0.001f);

            if (_StateGeoCodes != null)
            {
                StateBorder HoverState = null;
                foreach (StateBorder state in _StateGeoCodes)
                {

                    gpState[path].Transform(transformMap);



                    if (gpState[path].IsVisible(MouseCoords.Pixel.X, MouseCoords.Pixel.Y))
                    {
                        //save this state for last
                        HoverState = state;
                    }
                    else
                    {
                        brush = new SolidBrush(Color.FromArgb(state.StateColorOpacity, state.StateColor.R, state.StateColor.G, state.StateColor.B));
                        pen.Color = Color.Black;
                    }

                    gpState[path].Reset();

                    foreach (List<PointF> points in state.ProjectBorder())
                    {
                        bool draw = false;
                        foreach (PointF p in points)
                        {
                            if ((p.X > viewport.Left) && (p.X < viewport.Right) && (p.Y > viewport.Top) && (p.X < viewport.Bottom))
                            {
                                draw = true;
                            }
                        }
                        if (draw)
                        {
                            if (points.Count > 2)
                            {
                                gpState[path].AddPolygon(points.ToArray());
                                //We fill with white first to allow for proper opacity of the state color
                                e.Graphics.FillPolygon(brushWhite, points.ToArray());
                                e.Graphics.FillPolygon(brush, points.ToArray());
                                e.Graphics.DrawPolygon(pen, points.ToArray());

                                //Draw to the image
                                gBitmap.FillPolygon(brushWhite, points.ToArray());
                                gBitmap.FillPolygon(brush, points.ToArray());
                                gBitmap.DrawPolygon(pen, points.ToArray());
                            }
                        }

                    }

                    path++;
                }

                if (HoverState != null)
                {
                    brush = new SolidBrush(Color.FromArgb(HoverState.StateColorMouseOverOpacity, HoverState.StateColorMouseOver.R, HoverState.StateColorMouseOver.G, HoverState.StateColorMouseOver.B));
                    pen.Color = Color.Black;
                    foreach (List<PointF> points in HoverState.ProjectBorder())
                    {
                        if (points.Count > 2)
                        {
                            //We fill with white first to allow for proper opacity of the state color
                            e.Graphics.FillPolygon(brushWhite, points.ToArray());
                            e.Graphics.FillPolygon(brush, points.ToArray());
                            e.Graphics.DrawPolygon(pen, points.ToArray());

                            //Draw to the image
                            gBitmap.FillPolygon(brushWhite, points.ToArray());
                            gBitmap.FillPolygon(brush, points.ToArray());
                            gBitmap.DrawPolygon(pen, points.ToArray());
                        }
                    }
                }

            }
        }

        private void DrawZips(ref PaintEventArgs e)
        {

            if (_drawZip3 == false)
            {
                return;
            }

            int path = 0;

            e.Graphics.Transform = transformMap;
            gBitmap.Transform = transformMap;

            SolidBrush brush = new SolidBrush(Color.Green);
            Pen pen = new Pen(Color.Black, 0.001f);

            if (_Zip3GeoCodes != null)
            {
                Zip3Border HoverState = null;
                foreach (Zip3Border zip in _Zip3GeoCodes)
                {

                    try
                    {
                        
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        
                    }

                    //gpZip[path].Transform(transformMap);
                    //if (gpZip[path].IsVisible(MouseCoords.Pixel.X, MouseCoords.Pixel.Y))
                    //{
                        //save this state for last
                    //    HoverState = zip;
                    //}
                    //else
                    //{
                        brush = new SolidBrush(zip.ZipColor);
                        pen.Color = Color.Black;
                    //}

                    //gpZip[path].Reset();

                    foreach (List<PointF> points in zip.ProjectBorder())
                    {
                        bool draw = false;
                        foreach (PointF p in points)
                        {
                            if ((p.X > viewport.Left) && (p.X < viewport.Right) && (p.Y > viewport.Top) && (p.X < viewport.Bottom))
                            {
                                draw = true;
                            }
                        }
                        if (draw)
                        {
                            if (points.Count > 2)
                            {
                                //gpZip[path].AddPolygon(points.ToArray());
                                e.Graphics.FillPolygon(brush, points.ToArray());
                                e.Graphics.DrawPolygon(pen, points.ToArray());

                                //Draw to the image
                                gBitmap.FillPolygon(brush, points.ToArray());
                                gBitmap.DrawPolygon(pen, points.ToArray());
                            }
                        }

                    }

                    path++;

                    
                }

                if (HoverState != null)
                {
                    brush = new SolidBrush(HoverState.ZipColor);
                    pen.Color = Color.Black;
                    foreach (List<PointF> points in HoverState.ProjectBorder())
                    {
                        if (points.Count > 2)
                        {
                            e.Graphics.FillPolygon(brush, points.ToArray());
                            e.Graphics.DrawPolygon(pen, points.ToArray());

                            //Draw to the image
                            gBitmap.FillPolygon(brush, points.ToArray());
                            gBitmap.DrawPolygon(pen, points.ToArray());
                        }
                    }
                }

            }
        }

        private void DrawPin(ref Graphics g, zMapChartPin pin, int pinPath, bool setGP)
        {

            //Don't try and draw pins that don't have points
            if (pin.Point == null)
            {
                return;
            }

            if (setGP)
            {
                if (gpPin[pinPath].IsVisible(MouseCoords.Pixel.X, MouseCoords.Pixel.Y))
                {
                    hoverPins.Add(pin);
                    hoverPath.Add(pinPath);
                    return;
                }
            }

            g.Transform = transformNormal;
            gBitmap.Transform = transformNormal;

            

            PointF point = GeoToPixel(pin.Point);

            Font textFont = new Font(this.Font.FontFamily, pin.FontSize, pin.FontStyle, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            
            SizeF textLen = g.MeasureString(pin.Text, textFont);
            PointF pText = new PointF(point.X - 1, point.Y - (textLen.Height / 2));

            
            PointF[] diamond;
            PointF[] cross;
            PointF[] star;
            Rectangle rectangle;
            PointF[] triangle;

            textLen = new SizeF(textLen.Width + 10 + (10 * pin.PinSize), textLen.Height + 10 + (10 * pin.PinSize));

            PathGradientBrush b;
            Color[] c;


            //what are we drawing
            switch (pin.Style)
            {
                case zMapChartPin.PinStyle.Circle:
                    rectangle = Shapes.Rectangle(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddEllipse(rectangle);
                    }
                    g.DrawEllipse(new Pen(pin.BorderColor, 1), rectangle);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawEllipse(new Pen(pin.BorderColor, 1), rectangle);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.Cross:
                    cross = Shapes.Cross(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(cross);
                    }
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), cross);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), cross);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.Diamond:
                    diamond = Shapes.Diamond(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(diamond);
                    }
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), diamond);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), diamond);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.Square:
                    rectangle = Shapes.Rectangle(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddRectangle(rectangle);
                    }
                    g.DrawRectangle(new Pen(pin.BorderColor, 1), rectangle);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawRectangle(new Pen(pin.BorderColor, 1), rectangle);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.Triangle:
                    triangle = Shapes.Triangle(new PointF(point.X, point.Y - 7), textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(triangle);
                    }
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), triangle);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), triangle);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.Star:
                    star = Shapes.Star(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(star);
                    }
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), star);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), star);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.ColoredCircle:
                    rectangle = Shapes.Rectangle(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddEllipse(rectangle);
                    }
                    if (setGP)
                    {
                        g.FillEllipse(new SolidBrush(pin.Color), rectangle);
                        gBitmap.FillEllipse(new SolidBrush(pin.Color), rectangle);
                    }
                    else
                    {
                        b = new PathGradientBrush(gpPin[pinPath]);
                        c = new Color[1];
                        c[0] = pin.Color;
                        b.SurroundColors = c;
                        b.CenterColor = Color.White;
                        g.FillEllipse(b, rectangle);
                        gBitmap.FillEllipse(b, rectangle);
                    }
                    
                    g.DrawEllipse(new Pen(pin.BorderColor, 1), rectangle);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawEllipse(new Pen(pin.BorderColor, 1), rectangle);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf); 
                    break;
                case zMapChartPin.PinStyle.ColoredCross:
                    cross = Shapes.Cross(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(cross);
                    }
                    if (setGP)
                    {
                        g.FillPolygon(new SolidBrush(pin.Color), cross);
                        gBitmap.FillPolygon(new SolidBrush(pin.Color), cross);
                    }
                    else
                    {
                        b = new PathGradientBrush(gpPin[pinPath]);
                        c = new Color[1];
                        c[0] = pin.Color;
                        b.SurroundColors = c;
                        b.CenterColor = Color.White;
                        g.FillPolygon(b, cross);
                        gBitmap.FillPolygon(b, cross);
                    }

                    g.DrawPolygon(new Pen(pin.BorderColor, 1), cross);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), cross);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.ColoredDiamond:
                    diamond = Shapes.Diamond(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(diamond);
                    }
                    if (setGP)
                    {
                        g.FillPolygon(new SolidBrush(pin.Color), diamond);
                        gBitmap.FillPolygon(new SolidBrush(pin.Color), diamond);
                    }
                    else
                    {
                        b = new PathGradientBrush(gpPin[pinPath]);
                        c = new Color[1];
                        c[0] = pin.Color;
                        b.SurroundColors = c;
                        b.CenterColor = Color.White;
                        g.FillPolygon(b, diamond);
                        gBitmap.FillPolygon(b, diamond);
                    }
                    
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), diamond);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), diamond);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.ColoredSquare:
                    rectangle = Shapes.Rectangle(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddRectangle(rectangle);
                    }
                    if (setGP)
                    {
                        g.FillRectangle(new SolidBrush(pin.Color), rectangle);
                        gBitmap.FillRectangle(new SolidBrush(pin.Color), rectangle);
                    }
                    else
                    {
                        b = new PathGradientBrush(gpPin[pinPath]);
                        c = new Color[1];
                        c[0] = pin.Color;
                        b.SurroundColors = c;
                        b.CenterColor = Color.White;
                        g.FillRectangle(b, rectangle);
                        gBitmap.FillRectangle(b, rectangle);
                    }
                    
                    g.DrawRectangle(new Pen(pin.BorderColor, 1), rectangle);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawRectangle(new Pen(pin.BorderColor, 1), rectangle);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.ColoredTriangle:
                    triangle = Shapes.Triangle(new PointF(point.X, point.Y - 7), textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(triangle);
                    }
                    if (setGP)
                    {
                        g.FillPolygon(new SolidBrush(pin.Color), triangle);
                        gBitmap.FillPolygon(new SolidBrush(pin.Color), triangle);
                    }
                    else
                    {
                        b = new PathGradientBrush(gpPin[pinPath]);
                        c = new Color[1];
                        c[0] = pin.Color;
                        b.SurroundColors = c;
                        b.CenterColor = Color.White;
                        g.FillPolygon(b, triangle);
                        gBitmap.FillPolygon(b, triangle);
                    }
                    
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), triangle);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), triangle);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.ColoredStar:
                    star = Shapes.Star(point, textLen);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddPolygon(star);
                    }
                    if (setGP)
                    {
                        g.FillPolygon(new SolidBrush(pin.Color), star);
                        gBitmap.FillPolygon(new SolidBrush(pin.Color), star);
                    }
                    else
                    {
                        b = new PathGradientBrush(gpPin[pinPath]);
                        c = new Color[1];
                        c[0] = pin.Color;
                        b.SurroundColors = c;
                        b.CenterColor = Color.White;
                        g.FillPolygon(b, star);
                        gBitmap.FillPolygon(b, star);
                    }
                    
                    g.DrawPolygon(new Pen(pin.BorderColor, 1), star);
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);

                    gBitmap.DrawPolygon(new Pen(pin.BorderColor, 1), star);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    break;
                case zMapChartPin.PinStyle.TextOnly:
                    g.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    gBitmap.DrawString(pin.Text, textFont, new SolidBrush(pin.FontColor), pText, sf);
                    rectangle = new Rectangle((int)(point.X - 6), (int)(point.Y - 22), 12, 12);
                    if (setGP)
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddEllipse(rectangle);
                    }
                    break;
                case zMapChartPin.PinStyle.Pin:
                    rectangle = new Rectangle((int)(point.X - 6), (int)(point.Y - 22), 12, 12);
                    if (setGP) 
                    {
                        gpPin[pinPath].Reset();
                        gpPin[pinPath].AddEllipse(rectangle);                        
                    }

                    b = new PathGradientBrush(gpPin[pinPath]);
                    c = new Color[1];
                    c[0] = pin.Color;
                    b.SurroundColors = c;
                    b.CenterColor = Color.White;

                    pen.Color = Color.Black;

                    g.DrawLine(pen, point, new Point((int)(point.X), (int)(point.Y - 10)));
                    g.FillEllipse(b, rectangle);
                    g.DrawEllipse(new Pen(Color.Black, 1), rectangle);

                    gBitmap.DrawLine(pen, point, new Point((int)(point.X), (int)(point.Y - 10)));
                    gBitmap.FillEllipse(b, rectangle);
                    gBitmap.DrawEllipse(new Pen(Color.Black, 1), rectangle);
                    //b.Dispose();
                    break;
                default:
                    break;
            }

           
            
        }

        private void DrawPins(ref PaintEventArgs e)
        {


            Graphics g = e.Graphics;
            //Paint the pins
            int pinPath = 0;


            //Clear the hoverPins
            hoverPins.Clear();
            hoverPath.Clear();

            foreach (zMapChartPin pin in _pins)
            {               
                DrawPin(ref g, pin, pinPath, true);
                pinPath++;
            }

            //Draw the pins that the mouse is over
            int i = 0;
            foreach (zMapChartPin pin in hoverPins)
            {
                
                DrawPin(ref g, pin, hoverPath[i], false);
                i++;
            }
        }

        private void DrawLines(ref PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Transform = transformNormal;
            gBitmap.Transform = transformMap;
            //Paint the pins
            int linePath = 0;

            List<int> iPath = new List<int>();


            foreach (zMapChart_P2P_Line line in _lines)
            {
                //Don't try and draw a line unless both points are available
                if (line.HasPoints == false)
                {
                    continue;
                }

                gpLine[linePath].Reset();
                gpLine[linePath].Transform(transformNormal);
                gpLine[linePath].AddLine(GeoToPixel(line.PointA), GeoToPixel(line.PointB));                
                pen.Color = line.Color;
                pen.Width = line.Width;
                graphics.DrawLine(pen, GeoToPixel(line.PointA), GeoToPixel(line.PointB));
                gBitmap.DrawLine(pen, GeoToPixel(line.PointA), GeoToPixel(line.PointB));
                linePath++;
            }


            linePath = 0;

            //now that we have drawn the lines we need to draw the values
            foreach (zMapChart_P2P_Line line in _lines)
            {
                if (line.DisplayValues)
                {
                    //where are we drawing
                    PointF point = new PointF();

                    switch (line.ValuePlacement)
                    {
                        case zMapChart_P2P_Line.DisplayValuePlacement.LineCenter:
                            point = line.Center();
                            break;
                        case zMapChart_P2P_Line.DisplayValuePlacement.PointA:
                            point = line.PointA;
                            break;
                        case zMapChart_P2P_Line.DisplayValuePlacement.PointB:
                            point = line.PointB;
                            break;
                        case zMapChart_P2P_Line.DisplayValuePlacement.Animate:
                            point = line.AnimationPoint();
                            break;
                        default:
                            break;
                    }

                    point = GeoToPixel(point);
                    //point.Y = point.Y - (line.FontSize / 2);

                    

                    //declare the fonts and brushes
                    Brush textBrush = new SolidBrush(line.FontColor);
                    Font textFont = new Font(this.Font.FontFamily, line.FontSize, line.FontStyle, GraphicsUnit.Pixel);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;

                    SizeF textLen = e.Graphics.MeasureString(line.Value, textFont);

                    Rectangle rectText = new Rectangle((int)(point.X - (textLen.Width / 2)), (int)(point.Y - (textLen.Height / 2)), (int)textLen.Width, (int)textLen.Height);

                    textLen = new SizeF(textLen.Width + 10, textLen.Height + 10);

                    PointF[] diamond;
                    PointF[] cross;
                    PointF[] star;
                    Rectangle rectangle;
                    PointF[] triangle;


                    //what are we drawing
                    switch (line.ValueStyle)
                    {
                        case zMapChart_P2P_Line.DisplayValueStyle.Circle:
                            rectangle = Shapes.Rectangle(point, textLen);
                            graphics.DrawEllipse(new Pen(line.ValueStyleColor, line.Width), rectangle);
                            gBitmap.DrawEllipse(new Pen(line.ValueStyleColor, line.Width), rectangle);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.Cross:
                            cross = Shapes.Cross(point, textLen);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), cross);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), cross);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.Diamond:
                            diamond = Shapes.Diamond(point, textLen);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), diamond);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), diamond);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.Square:
                            rectangle = Shapes.Rectangle(point, textLen);
                            graphics.DrawRectangle(new Pen(line.ValueStyleColor, line.Width), rectangle);
                            gBitmap.DrawRectangle(new Pen(line.ValueStyleColor, line.Width), rectangle);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.Triangle:
                            triangle = Shapes.Triangle(new PointF(point.X, point.Y - 7), textLen);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), triangle);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), triangle);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.Star:
                            star = Shapes.Star(point, textLen);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), star);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), star);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.ColoredCircle:
                            rectangle = Shapes.Rectangle(point, textLen);
                            graphics.FillEllipse(new SolidBrush(line.ValueStyleFillColor), rectangle);
                            graphics.DrawEllipse(new Pen(line.ValueStyleColor, line.Width), rectangle);

                            gBitmap.FillEllipse(new SolidBrush(line.ValueStyleFillColor), rectangle);
                            gBitmap.DrawEllipse(new Pen(line.ValueStyleColor, line.Width), rectangle);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.ColoredCross:
                            cross = Shapes.Cross(point, textLen);
                            graphics.FillPolygon(new SolidBrush(line.ValueStyleFillColor), cross);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), cross);

                            gBitmap.FillPolygon(new SolidBrush(line.ValueStyleFillColor), cross);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), cross);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.ColoredDiamond:
                            diamond = Shapes.Diamond(point, textLen);
                            graphics.FillPolygon(new SolidBrush(line.ValueStyleFillColor), diamond);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), diamond);

                            gBitmap.FillPolygon(new SolidBrush(line.ValueStyleFillColor), diamond);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), diamond);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.ColoredSquare:
                            rectangle = Shapes.Rectangle(point, textLen);
                            graphics.FillRectangle(new SolidBrush(line.ValueStyleFillColor), rectangle);
                            graphics.DrawRectangle(new Pen(line.ValueStyleColor, line.Width), rectangle);

                            gBitmap.FillRectangle(new SolidBrush(line.ValueStyleFillColor), rectangle);
                            gBitmap.DrawRectangle(new Pen(line.ValueStyleColor, line.Width), rectangle);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.ColoredTriangle:
                            triangle = Shapes.Triangle(new PointF(point.X, point.Y - 7), textLen);
                            graphics.FillPolygon(new SolidBrush(line.ValueStyleFillColor), triangle);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), triangle);

                            gBitmap.FillPolygon(new SolidBrush(line.ValueStyleFillColor), triangle);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), triangle);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.ColoredStar:
                            star = Shapes.Star(point, textLen);
                            graphics.FillPolygon(new SolidBrush(line.ValueStyleFillColor), star);
                            graphics.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), star);

                            gBitmap.FillPolygon(new SolidBrush(line.ValueStyleFillColor), star);
                            gBitmap.DrawPolygon(new Pen(line.ValueStyleColor, line.Width), star);
                            break;
                        case zMapChart_P2P_Line.DisplayValueStyle.TextOnly:
                            //Nothing needs to be done here
                            break;
                        default:
                            break;
                    }

                    PointF p = new PointF(point.X - 1, point.Y - (textLen.Height / 2) + 4);
                    graphics.DrawString(line.Value, textFont, textBrush, p, sf);
                    //gBitmap.DrawString(line.Value, textFont, textBrush, p, sf);

                }

                linePath++;
            }

        }

        private void DrawPinTips(ref PaintEventArgs e)
        {
            e.Graphics.Transform = transformNormal;

            int tipWidth = 125;
            int tipHeight = 60;
            int shadowLoc = 8;

            //Currently we have a place for up to 4 tips at once. We can add as many as we need, but we have to figure out where we want them
            for (int x = 0; x <= hoverPins.Count - 1; x++)
            {

                Rectangle rectTip = new Rectangle(this.Width - 140, 5, (int)tipWidth, (int)tipHeight);
                Rectangle rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                switch (x)
                {
                    case 0:
                        {
                            //rectTip = new Rectangle(this.Width - 130, (int)(MouseCoords.Pixel.Y + 20), (int)tipWidth, (int)tipHeight);
                            //rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                            break;
                        }
                    default:
                        {
                            rectTip = new Rectangle(this.Width - 140, 80 * x, (int)tipWidth, (int)tipHeight);
                            rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);
                            break;
                        }
                }

                if (x > 3)
                {
                    return;
                }

                int y = x == 0 ? 5 : 80 * x;

                zMapChartPin info = (zMapChartPin)hoverPins[x];


                Brush bg = new SolidBrush(Color.Black);
                Brush shadow = new SolidBrush(Color.FromArgb(150, Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B));

                Brush border = new SolidBrush(Color.White);
                Brush text = new SolidBrush(Color.Gold);
                Font f = new Font(this.Font.FontFamily, 12, FontStyle.Bold, GraphicsUnit.Pixel);

                Rectangle rect = new Rectangle(rectTip.X, rectTip.Y, rectTip.Width, rectTip.Height);
                //rectTip = new Rectangle(rectTip.X + 1, rectTip.Y + 1, rectTip.Width, rectTip.Height);
                rect.Inflate(2, 2);

                GraphicsPath pTip = Shapes.RoundedRectangle(rectTip, 10, 4);

                GraphicsPath pTipInflated = Shapes.RoundedRectangle(rect, 10, 4);

                GraphicsPath pShadow = Shapes.RoundedRectangle(rectShadow, 10, 4);

                Region rTip1 = new Region(pTipInflated);
                Region rTip = new Region(pTip);
                Region rShadow = new Region(pShadow);

                e.Graphics.FillRegion(shadow, rShadow);
                e.Graphics.FillRegion(border, rTip1);
                e.Graphics.FillRegion(bg, rTip);


                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;

                e.Graphics.DrawString(info.Name, f, text, rectTip.X + 2, rectTip.Y + 6, sf);
                e.Graphics.DrawString(info.TipText, f, text, rectTip.X + 2, rectTip.Y + 18, sf);
                //e.Graphics.DrawString(info.Value.ToString(), f, text, rectTip.X + 2, rectTip.Y + 28, sf);
                //g.DrawString(info.PlotName, f, text, rectTip.X + 2, rectTip.Y + 18, sf);
                //g.DrawString(formatString + Math.Round(info.PlotValue, 2).ToString(), f, text, rectTip.X + 2, rectTip.Y + 31, sf);


                //Dispose
                bg.Dispose();
                shadow.Dispose();
                text.Dispose();
                f.Dispose();
                pTip.Dispose();
                pShadow.Dispose();
                rTip.Dispose();
                rShadow.Dispose();


            }
        }

        private void DrawCoordinates(ref PaintEventArgs e)
        {
            e.Graphics.Transform = transformNormal;
            Brush text = new SolidBrush(Color.White);
            Font f = new Font(this.Font.FontFamily, 10, FontStyle.Regular, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;

            e.Graphics.DrawString("N: " + MouseCoords.LatLong.Y.ToString() + "  W: " + MouseCoords.LatLong.X.ToString(), f, text, 5, this.Height - 40, sf);
            e.Graphics.DrawString("X: " + MouseCoords.Pixel.X.ToString() + "  Y: " + MouseCoords.Pixel.Y.ToString(), f, text, 5, this.Height - 30, sf);


            //Dispose
            text.Dispose();
            f.Dispose();


        }

        private void SetStatusBar()
        {
                        
            //PointF p = Conversion.Conversion.Deproject(new PointF(viewport.Left, viewport.Top));
            //PointF pp = Conversion.Conversion.Deproject(new PointF(viewport.Right, viewport.Bottom));
            //PointF pSize = Conversion.Conversion.Deproject(viewport.Size.ToPointF());
            

            //lblWidth.Text = pSize.X.ToString();
            //lblHeight.Text = pSize.Y.ToString();

            //lblLeft.Text = (-p.X).ToString();
            //lblTop.Text = (-p.Y).ToString();
            //lblRight.Text = (-pp.X).ToString();
            //lblBottom.Text = (-pp.Y).ToString();
            //lblGeo.Text = "W: " + MouseCoords.LatLong.X.ToString() + ", N: " + MouseCoords.LatLong.Y.ToString();
            //PointF ThePoint = GeoToPixel(geographicCoordinate);
            //lblPixel.Text = "X: " + MouseCoords.Pixel.X.ToString() + ", Y: " + MouseCoords.Pixel.Y.ToString();
            
        }

        private void zMapChart_Load(object sender, EventArgs e)
        {
            Graphics = this.CreateGraphics();
            TransformToMap();
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Graphics.Transform = transformMap;

            
            
        }

        private void zMapChart_MouseClick(object sender, MouseEventArgs e)
        {
            //CenterViewport(new PointF(geographicCoordinate.X, geographicCoordinate.Y));
        }

        private void zMapChart_SizeChanged(object sender, EventArgs e)
        {
            TransformToMap();
        }

    #endregion

    #region "Override"

        protected override void OnPaint(PaintEventArgs e)
        {
            if (invalidating)
            {
                return;
            }
            invalidating = true;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            gBitmap.Clear(Color.Blue);

            DrawStatesEx(ref e);

            DrawZips(ref e);

            

            DrawLines(ref e);
            DrawPins(ref e);
            DrawPinTips(ref e);
            DrawCoordinates(ref e);
            base.OnPaint(e);
            invalidating = false;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }



        
        protected override void OnKeyUp(KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Left:
                    PanLeft();
                    break;
                case Keys.Up:
                    PanUp();
                    break;
                case Keys.Right:
                    PanRight();
                    break;
                case Keys.Down:
                    PanDown();
                    break;

            }

            //Invalidate();


        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ZoomOut();
            }
            else
            {
                ZoomIn();
            }

            //this.Invalidate();

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            
            x = e.X;
            y = e.Y;
            geographicCoordinate = PixelToGeo(new Point((int)x, (int)y));

            LocationType l = new LocationType();
            l.LatLong = geographicCoordinate;
            l.Projected = projectedCoordinate;
            l.Pixel = new Point((int)x, (int)y);
            MouseCoords = l;
            //SetStatusBar();

            Application.DoEvents();
            //base.OnMouseMove(e);
            //ReDraw();
            //Invalidate();

        }



        
    #endregion



        #region IDisposable Members

        void IDisposable.Dispose()
        {
            this.transformMap.Dispose();
            this.transformNormal.Dispose();
            this.brush.Dispose();
            this.pen.Dispose();
            this.Dispose();
        }

        #endregion

        


        #region "Hidden Methods"



        [EditorBrowsable(EditorBrowsableState.Never)]
        public string AccessibleDefaultActionDescription
        {
            get { return base.AccessibleDefaultActionDescription; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccessibleObject AccessibilityObject
        {
            get { return base.AccessibilityObject; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Control ActiveControl
        {
            get { return base.ActiveControl; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void AdjustFormScrollbars(bool displayScrollbars)
        {
            base.AdjustFormScrollbars(displayScrollbars);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                base.AllowDrop = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override AnchorStyles Anchor
        {
            get
            {
                return base.Anchor;
            }
            set
            {
                base.Anchor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                base.AutoScroll = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Point AutoScrollOffset
        {
            get
            {
                return base.AutoScrollOffset;
            }
            set
            {
                base.AutoScrollOffset = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override AutoValidate AutoValidate
        {
            get
            {
                return base.AutoValidate;
            }
            set
            {
                base.AutoValidate = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override BindingContext BindingContext
        {
            get
            {
                return base.BindingContext;
            }
            set
            {
                base.BindingContext = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool CanEnableIme
        {
            get
            {
                return base.CanEnableIme;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool CanRaiseEvents
        {
            get
            {
                return base.CanRaiseEvents;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return base.CreateAccessibilityInstance();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override ControlCollection CreateControlsInstance()
        {
            return base.CreateControlsInstance();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void CreateHandle()
        {
            base.CreateHandle();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override CreateParams CreateParams
        {
            get
            {
                return base.CreateParams;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Cursor DefaultCursor
        {
            get
            {
                return base.DefaultCursor;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override ImeMode DefaultImeMode
        {
            get
            {
                return base.DefaultImeMode;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Padding DefaultMargin
        {
            get
            {
                return base.DefaultMargin;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Size DefaultMaximumSize
        {
            get
            {
                return base.DefaultMaximumSize;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Size DefaultMinimumSize
        {
            get
            {
                return base.DefaultMinimumSize;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Padding DefaultPadding
        {
            get
            {
                return base.DefaultPadding;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Size DefaultSize
        {
            get
            {
                return base.DefaultSize;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void DestroyHandle()
        {
            base.DestroyHandle();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                return base.DisplayRectangle;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool DoubleBuffered
        {
            get
            {
                return base.DoubleBuffered;
            }
            set
            {
                base.DoubleBuffered = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override AccessibleObject GetAccessibilityObjectById(int objectId)
        {
            return base.GetAccessibilityObjectById(objectId);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Size GetPreferredSize(Size proposedSize)
        {
            return base.GetPreferredSize(proposedSize);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
        {
            return base.GetScaledBounds(bounds, factor, specified);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override object GetService(Type service)
        {
            return base.GetService(service);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override ImeMode ImeModeBase
        {
            get
            {
                return base.ImeModeBase;
            }
            set
            {
                base.ImeModeBase = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitLayout()
        {
            base.InitLayout();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool IsInputChar(char charCode)
        {
            return base.IsInputChar(charCode);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool IsInputKey(Keys keyData)
        {
            return base.IsInputKey(keyData);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                return base.LayoutEngine;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Size MaximumSize
        {
            get
            {
                return base.MaximumSize;
            }
            set
            {
                base.MaximumSize = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
            set
            {
                base.MinimumSize = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void NotifyInvalidate(Rectangle invalidatedArea)
        {
            base.NotifyInvalidate(invalidatedArea);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnAutoSizeChanged(EventArgs e)
        {
            base.OnAutoSizeChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnAutoValidateChanged(EventArgs e)
        {
            base.OnAutoValidateChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnBackgroundImageLayoutChanged(EventArgs e)
        {
            base.OnBackgroundImageLayoutChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnCausesValidationChanged(EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnChangeUICues(UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnCursorChanged(EventArgs e)
        {
            base.OnCursorChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            base.OnGiveFeedback(gfbevent);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            base.OnHelpRequested(hevent);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnImeModeChanged(EventArgs e)
        {
            base.OnImeModeChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnMarginChanged(EventArgs e)
        {
            base.OnMarginChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnNotifyMessage(Message m)
        {
            base.OnNotifyMessage(m);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            base.OnParentBackgroundImageChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentBindingContextChanged(EventArgs e)
        {
            base.OnParentBindingContextChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentCursorChanged(EventArgs e)
        {
            base.OnParentCursorChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentEnabledChanged(EventArgs e)
        {
            base.OnParentEnabledChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentFontChanged(EventArgs e)
        {
            base.OnParentFontChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentForeColorChanged(EventArgs e)
        {
            base.OnParentForeColorChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentRightToLeftChanged(EventArgs e)
        {
            base.OnParentRightToLeftChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnParentVisibleChanged(EventArgs e)
        {
            base.OnParentVisibleChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnPrint(PaintEventArgs e)
        {
            base.OnPrint(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
        {
            base.OnQueryContinueDrag(qcdevent);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnRegionChanged(EventArgs e)
        {
            base.OnRegionChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bitmap = new Bitmap(this.Width, this.Height);
            gBitmap = Graphics.FromImage(bitmap);
            gBitmap.SmoothingMode = SmoothingMode.AntiAlias;
            gBitmap.CompositingQuality = CompositingQuality.HighSpeed;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnStyleChanged(EventArgs e)
        {
            base.OnStyleChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool PreProcessMessage(ref Message msg)
        {
            return base.PreProcessMessage(ref msg);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessDialogChar(char charCode)
        {
            return base.ProcessDialogChar(charCode);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            return base.ProcessDialogKey(keyData);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessKeyEventArgs(ref Message m)
        {
            return base.ProcessKeyEventArgs(ref m);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessKeyMessage(ref Message m)
        {
            return base.ProcessKeyMessage(ref m);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessKeyPreview(ref Message m)
        {
            return base.ProcessKeyPreview(ref m);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessMnemonic(char charCode)
        {
            return base.ProcessMnemonic(charCode);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ProcessTabKey(bool forward)
        {
            return base.ProcessTabKey(forward);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Refresh()
        {
            base.Refresh();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetBackColor()
        {
            base.ResetBackColor();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetCursor()
        {
            base.ResetCursor();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetFont()
        {
            base.ResetFont();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetForeColor()
        {
            base.ResetForeColor();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetRightToLeft()
        {
            base.ResetRightToLeft();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void ResetText()
        {
            base.ResetText();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ScaleChildren
        {
            get
            {
                return base.ScaleChildren;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ScaleCore(float dx, float dy)
        {
            base.ScaleCore(dx, dy);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Point ScrollToControl(Control activeControl)
        {
            return base.ScrollToControl(activeControl);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Select(bool directed, bool forward)
        {
            base.Select(directed, forward);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void SetClientSizeCore(int x, int y)
        {
            base.SetClientSizeCore(x, y);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ShowFocusCues
        {
            get
            {
                return base.ShowFocusCues;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool ShowKeyboardCues
        {
            get
            {
                return base.ShowKeyboardCues;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override Size SizeFromClientSize(Size clientSize)
        {
            return base.SizeFromClientSize(clientSize);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void UpdateDefaultButton()
        {
            base.UpdateDefaultButton();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ValidateChildren()
        {
            return base.ValidateChildren();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ValidateChildren(ValidationConstraints validationConstraints)
        {
            return base.ValidateChildren(validationConstraints);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string AccessibleDescription
        {
            get { return base.AccessibleDescription; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string AccessibleName
        {
            get { return base.AccessibleName; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccessibleRole AccessibleRole
        {
            get { return base.AccessibleRole; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public SizeF AutoScaleDimensions
        {
            get { return base.AutoScaleDimensions; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Point AutoScrollPosition
        {
            get { return base.AutoScrollPosition; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Bottom
        {
            get { return base.Bottom; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle Bounds
        {
            get { return base.Bounds; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanFocus
        {
            get { return base.CanFocus; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CanSelect
        {
            get { return base.CanSelect; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Capture
        {
            get { return base.Capture; }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool CausesValidation
        {
            get { return base.CausesValidation; }
        }

        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle ClientRectangle
        {
            get { return base.ClientRectangle; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size ClientSize
        {
            get { return base.ClientSize; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CompanyName
        {
            get { return base.CompanyName; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IContainer Container
        {
            get { return base.Container; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ContainsFocus
        {
            get { return base.ContainsFocus; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ControlCollection Controls
        {
            get { return base.Controls; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Created
        {
            get { return base.Created; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public SizeF CurrentAutoScaleDimensions
        {
            get { return base.CurrentAutoScaleDimensions; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ControlBindingsCollection DataBindings
        {
            get { return base.DataBindings; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Color DefaultBackColor
        {
            get { return Color.Black; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Font DefaultFont
        {
            get { return null; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Color DefaultForeColor
        {
            get { return Color.Black; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Disposing
        {
            get { return base.Disposing; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Enabled
        {
            get { return base.Enabled; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IntPtr Handle
        {
            get { return base.Handle; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasChildren
        {
            get { return base.HasChildren; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public HScrollProperties HorizontalScroll
        {
            get { return base.HorizontalScroll; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ImeMode ImeMode
        {
            get { return base.ImeMode; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool InvokeRequired
        {
            get { return base.InvokeRequired; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsAccessible
        {
            get { return base.IsAccessible; }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDisposed
        {
            get { return base.IsDisposed; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsHandleCreated
        {
            get{return base.IsHandleCreated;}
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsMirrored
        {
            get { return base.IsMirrored; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Left
        {
            get { return base.Left; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Padding Margin
        {
            get{return base.Margin;}

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Padding Padding
        {
            get { return base.Padding; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size PreferredSize
        {
            get { return base.PreferredSize; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ProductName
        {
            get { return base.ProductName; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ProductVersion
        {
            get { return base.ProductVersion; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ImeMode PropagatingImeMode
        {
            get { return ImeMode.Inherit; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool RecreatingHandle
        {
            get { return base.RecreatingHandle; }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Right
        {
            get { return base.Right; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public int Top
        {
            get { return base.Top; }
        }





        #endregion


    }

    public class zMapChartMouseEventArgs : EventArgs
    {
        string _Name = "";
        decimal _Value;
        Color _Color;
        System.Windows.Forms.MouseEventArgs _MouseArgs;

        public string Name
        {
            get { return _Name; }
        }

        public decimal Value
        {
            get { return _Value; }
        }

        public Color StateColor
        {
            get { return _Color; }
        }

        public System.Windows.Forms.MouseEventArgs MouseArgs
        {
            get { return _MouseArgs; }
        }

        public zMapChartMouseEventArgs(string Name, decimal Value, Color Color, System.Windows.Forms.MouseEventArgs mouseArgs)
        {
            _Name = Name;
            _Value = Value;
            _Color = Color;
            _MouseArgs = mouseArgs;
        }

    }

    public class StateMouseEventArgs : zMapChartMouseEventArgs
    {


        public StateMouseEventArgs(string Name, decimal Value, Color Color, System.Windows.Forms.MouseEventArgs mouseArgs)
            : base(Name, Value, Color, mouseArgs)
        {

        }

    }

    public class ZipMouseEventArgs : zMapChartMouseEventArgs
    {

        public ZipMouseEventArgs(string Name, decimal Value, Color Color, System.Windows.Forms.MouseEventArgs mouseArgs)
            : base(Name, Value, Color, mouseArgs)
        {

        }

    }
}
