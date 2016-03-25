using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using zControlsC;




namespace zControlsC.Charts
{
    public partial class zLineChart : UserControl
    {

    #region "Initializer"

        public zLineChart()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            this.BackColor = Color.White;
            
            SetChartArea();

        }


    #endregion

    #region "Declarations"

        //An image of the chart
        Bitmap _image;
        Graphics _imageGraphics;

        //Graphics Paths
        GraphicsPath[] _gpLine;
        GraphicsPath[] _gpStandardDeviationLine;
        GraphicsPath[] _gpPlot;
        GraphicsPath[] _gpLegend;
        GraphicsPath[] _gpLegendImage;
        GraphicsPath[] _gpLegendCheckBox;
        GraphicsPath[] _gpXAxis;
        GraphicsPath[] _gpTip;

        bool[] _lineIsVisible;


        //Line Collections
        ArrayList _lines = new ArrayList();
        ArrayList _linesSecondary = new ArrayList();
        ArrayList _linesStandardDeviation = new ArrayList();

        ArrayList _plots = new ArrayList();

        ArrayList _tips = new ArrayList();


        //Chart
        Color _borderColor = Color.Black;
        PlotValueFormatEnum _plotValueFormat = PlotValueFormatEnum.Number;
        string _xAxis_formatString = "";

        bool _autoMinMaxIncrement = false;

        
        //Title
        string      _title          = "Title";
        string      _subTitle       = "Subtitle";
        bool        _showTitle      = true;
        bool        _showSubTitle   = true;
        Color       _titleColor     = Color.Black;
        Color       _subTitleColor  = Color.Black;
        float       _titleSize      = 22;
        float       _subTitleSize   = 16;
        

        //X-Axis
        string      _xAxisName              = "X-Axis";
        Color       _xAxisFontColor         = Color.Black;
        float       _xAxisFontSize          = 13;
        FontStyle   _xAxisFontStyle         = FontStyle.Regular;
        FontStyle   _xAxisMouseOverFontStyle         = FontStyle.Underline;
        Color _xAxisMouseOverBackColor = Color.Blue;
        Color       _xAxisNameFontColor     = Color.Black;
        float       _xAxisNameFontSize      = 14;
        FontStyle   _xAxisNameFontStyle     = FontStyle.Regular;
        Color       _xAxisBorderColor       = Color.Black;
        int         _xAxisItemCount         = 0;
        bool _showXAxis = true;


        //Y-Axis
        string      _yAxisName            = "Y-Axis";
        Color       _yAxisNameFontColor   = Color.Black;
        float       _yAxisNameFontSize    = 13;
        Color       _yAxisFontColor       = Color.Black;
        float       _yAxisFontSize        = 10;
        decimal       _yAxisMinimum         = 92;
        decimal       _yAxisMaximum         = 98;
        decimal       _yAxisIncrement       = 1;
        int         _yAxisDecimalPlaces = 2;
        


        //Y-Axis Secondary
        bool        _showSecondary_Y_Axis          = false;
        string      _yAxisSecondaryName            = "Secondary Y-Axis";
        Color       _yAxisSecondaryNameFontColor   = Color.Black;
        float       _yAxisSecondaryNameFontSize    = 13;
        Color       _yAxisSecondaryFontColor       = Color.Black;
        float       _yAxisSecondaryFontSize        = 10;
        decimal       _yAxisSecondaryMinimum         = 92;
        decimal       _yAxisSecondaryMaximum         = 100;
        decimal       _yAxisSecondaryIncrement       = 1;
        int _yAxisSecondaryDecimalPlaces = 2;


        //Legend
        bool        _showLegend                 = true;
        Color       _legendFontColor            = Color.Black;
        Color       _legendFontColorMouseOver   = Color.Black;
        FontStyle   _legendFontStyle            = FontStyle.Regular;
        FontStyle   _legendFontStyleMouseOver   = FontStyle.Underline;
        float       _legendFontSize             = 14;
        int         _legendOpacity              = 255;
        bool        _showLineCheckBoxes             = true;
        int _legendOffset = 0;


        //Standard Deviation Lines
        bool _showStandardDeviationLines = true;



        //Grid lines
        bool _showGridLines = true;
        Color _gridLineColor = Color.Gray;


        //Mouse Coordinates
        float _x;
        float _y;
        bool _mouseClicked = false;


        
        //Plots
        int _plotMouseOverGrowAmount = 4;

        string[] _pArray;

        //Left Margin
        int _leftMargin = 90;

        //Chart Area Rectangle
        Rectangle _chartArea;
        RectangleF _chartAreaF;



        public enum PlotValueFormatEnum
        {
            Currency,
            Number
        }


    #endregion

    #region "Properties"

        [Category("Chart")]
        public Image Image
        {
            get { return _image; }
        }

        [Category("Chart")]
        public PlotValueFormatEnum PlotValueFormat
        {
            get { return _plotValueFormat; }
            set
            {
                _plotValueFormat = value;
                if (value == PlotValueFormatEnum.Currency)
                {
                    _xAxis_formatString = "$";
                }
                else
                {
                    _xAxis_formatString = "";
                }
                this.Invalidate();
            }
        }

        [Category("Chart")]
        public bool AutoMinMaxIncrement
        {
            get { return _autoMinMaxIncrement; }
            set
            {
                _autoMinMaxIncrement = value;
                this.Invalidate();
            }
        }


    #region "Title"

        [Category("Title"), DefaultValue("Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.Invalidate();
            }
        }

        [Category("Title"), DefaultValue("Subtitle")]
        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                this.Invalidate();
            }
        }

        [Category("Title"), DefaultValue(true)]
        public bool ShowTitle
        {
            get { return _showTitle; }
            set
            {
                _showTitle = value;
                SetChartArea();
                this.Invalidate();
            }

        }

        [Category("Title"), DefaultValue(true)]
        public bool ShowSubTitle
        {
            get { return _showSubTitle; }
            set
            {
                _showSubTitle = value;
                SetChartArea();
                this.Invalidate();
            }

        }

        [Category("Title")]
        public Color TitleColor
        {
            get { return _titleColor; }
            set
            {
                _titleColor = value;
                this.Invalidate();
            }

        }

        [Category("Title")]
        public Color SubTitleColor
        {
            get { return _subTitleColor; }
            set
            {
                _subTitleColor = value;
                this.Invalidate();
            }

        }

        [Category("Title"), DefaultValue(22)]
        public float TitleSize
        {
            get { return _titleSize; }
            set
            {
                _titleSize = value;
                SetChartArea();
                this.Invalidate();
            }

        }

        [Category("Title"), DefaultValue(16)]
        public float SubTitleSize
        {
            get { return _subTitleSize; }
            set
            {
                _subTitleSize = value;
                SetChartArea();
                this.Invalidate();
            }

        }


    #endregion

    #region "X-Axis"

        [Category("X-Axis"), DefaultValue("")]
        public string XAxisName
        {
            get { return _xAxisName; }
            set
            {
                _xAxisName = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis"), DefaultValue("")]
        public string XAxis_formatString
        {
            get { return _xAxis_formatString; }
            set
            {
                _xAxis_formatString = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis")]
        public Color XAxisFontColor
        {
            get { return _xAxisFontColor; }
            set
            {
                _xAxisFontColor = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis"), DefaultValue(13)]
        public float XAxisFontSize
        {
            get { return _xAxisFontSize; }
            set
            {
                _xAxisFontSize = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis"), DefaultValue(FontStyle.Regular)]
        public FontStyle XAxisFontStyle
        {
            get { return _xAxisFontStyle; }
            set
            {
                _xAxisFontStyle = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis"), DefaultValue(FontStyle.Underline)]
        public FontStyle XAxisMouseOverFontStyle
        {
            get { return _xAxisMouseOverFontStyle; }
            set
            {
                _xAxisMouseOverFontStyle = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis")]
        public Color XAxisMouseOverBackColor
        {
            get { return _xAxisMouseOverBackColor; }
            set
            {
                _xAxisMouseOverBackColor = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis")]
        public Color XAxisNameFontColor
        {
            get { return _xAxisNameFontColor; }
            set
            {
                _xAxisNameFontColor = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis"), DefaultValue(14)]
        public float XAxisNameFontSize
        {
            get { return _xAxisNameFontSize; }
            set
            {
                _xAxisNameFontSize = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis"), DefaultValue(FontStyle.Regular)]
        public FontStyle XAxisNameFontStyle
        {
            get { return _xAxisNameFontStyle; }
            set
            {
                _xAxisNameFontStyle = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis")]
        public Color XAxisBorderColor
        {
            get { return _xAxisBorderColor; }
            set
            {
                _xAxisBorderColor = value;
                this.Invalidate();
            }
        }

        [Category("X-Axis")]
        public bool ShowXAxis
        {
            get { return _showXAxis; }
            set
            {
                _showXAxis = value;
                this.Invalidate();
            }
        }


    #endregion

    #region "Y-Axis"

        [Category("Y-Axis"), DefaultValue("Y-Axis")]
        public string YAxisName
        {
            get { return _yAxisName; }
            set
            {
                _yAxisName = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis")]
        public Color YAxisNameFontColor
        {
            get { return _yAxisNameFontColor; }
            set
            {
                _yAxisNameFontColor = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis"), DefaultValue(13)]
        public float YAxisNameFontSize
        {
            get { return _yAxisNameFontSize; }
            set
            {
                _yAxisNameFontSize = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis")]
        public Color YAxisFontColor
        {
            get { return _yAxisFontColor; }
            set
            {
                _yAxisFontColor = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis"), DefaultValue(10)]
        public float YAxisFontSize
        {
            get { return _yAxisFontSize; }
            set
            {
                _yAxisFontSize = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis"), DefaultValue(92)]
        public decimal YAxisMinimum
        {
            get { return _yAxisMinimum; }
            set
            {
                _yAxisMinimum = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis"), DefaultValue(98)]
        public decimal YAxisMaximum
        {
            get { return _yAxisMaximum; }
            set
            {
                _yAxisMaximum = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis"), DefaultValue(1)]
        public decimal YAxisIncrement
        {
            get { return _yAxisIncrement; }
            set
            {
                _yAxisIncrement = value;
                this.Invalidate();
            }
        }

        [Category("Y-Axis"), DefaultValue(2)]
        public int YAxisDecimalPlaces
        {
            get { return _yAxisDecimalPlaces; }
            set
            {
                _yAxisDecimalPlaces = value;
                this.Invalidate();
            }
        }


    #endregion

    #region "Y-Axis Secondary"

        [Category("Secondary Y-Axis")]
        public bool ShowSecondaryYAxis
        {
            get { return _showSecondary_Y_Axis; }
            set
            {
                _showSecondary_Y_Axis = value;
                SetChartArea();
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue("Secondary Y-Axis")]
        public string YAxisSecondaryName
        {
            get { return _yAxisSecondaryName; }
            set
            {
                _yAxisSecondaryName = value;
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis")]
        public Color YAxisSecondaryNameFontColor
        {
            get { return _yAxisSecondaryNameFontColor; }
            set
            {
                _yAxisSecondaryNameFontColor = value;
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue(13)]
        public float YAxisSecondaryNameFontSize
        {
            get { return _yAxisSecondaryNameFontSize; }
            set
            {
                _yAxisSecondaryNameFontSize = value;
                SetChartArea();
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis")]
        public Color YAxisSecondaryFontColor
        {
            get { return _yAxisSecondaryFontColor; }
            set
            {
                _yAxisSecondaryFontColor = value;
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue(10)]
        public float YAxisSecondaryFontSize
        {
            get { return _yAxisSecondaryFontSize; }
            set
            {
                _yAxisSecondaryFontSize = value;
                SetChartArea();
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue(92)]
        public decimal YAxisSecondaryMinimum
        {
            get { return _yAxisSecondaryMinimum; }
            set
            {
                _yAxisSecondaryMinimum = value;
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue(98)]
        public decimal YAxisSecondaryMaximum
        {
            get { return _yAxisSecondaryMaximum; }
            set
            {
                _yAxisSecondaryMaximum = value;
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue(1)]
        public decimal YAxisSecondaryIncrement
        {
            get { return _yAxisSecondaryIncrement; }
            set
            {
                _yAxisSecondaryIncrement = value;
                this.Invalidate();
            }
        }

        [Category("Secondary Y-Axis"), DefaultValue(2)]
        public int YAxisSecondaryDecimalPlaces
        {
            get { return _yAxisSecondaryDecimalPlaces; }
            set
            {
                _yAxisSecondaryDecimalPlaces = value;
                this.Invalidate();
            }
        }

    #endregion

    #region "Legend"

        [Category("Legend"), DefaultValue(true)]
        public bool ShowLegend
        {
            get { return _showLegend; }
            set
            {
                _showLegend = value;
                SetChartArea();
                this.Invalidate();
            }
        }

        [Category("Legend")]
        public Color LegendFontColor
        {
            get { return _legendFontColor; }
            set
            {
                _legendFontColor = value;
                this.Invalidate();
            }
        }

        [Category("Legend")]
        public Color LegendMouseOverFontColor
        {
            get { return _legendFontColorMouseOver; }
            set
            {
                _legendFontColorMouseOver = value;
                this.Invalidate();
            }
        }

        [Category("Legend"), DefaultValue(FontStyle.Regular)]
        public FontStyle LegendFontStyle
        {
            get { return _legendFontStyle; }
            set
            {
                _legendFontStyle = value;
                this.Invalidate();
            }
        }

        [Category("Legend"), DefaultValue(FontStyle.Underline)]
        public FontStyle LegendMouseOverFontStyle
        {
            get { return _legendFontStyleMouseOver; }
            set
            {
                _legendFontStyleMouseOver = value;
                this.Invalidate();
            }
        }

        [Category("Legend"), DefaultValue(14)]
        public float LegendFontSize
        {
            get { return _legendFontSize; }
            set
            {
                _legendFontSize = value;
                SetChartArea();
                this.Invalidate();
            }
        }
        [Category("Legend"), DefaultValue(true)]
        public bool ShowLineCheckBoxes
        {
            get { return _showLineCheckBoxes; }
            set
            {
                _showLineCheckBoxes = value;
                SetChartArea();
                this.Invalidate();
            }
        }


        [Category("Legend"), DefaultValue(0)]
        public int LegendOffset
        {
            get { return _legendOffset; }
            set
            {
                _legendOffset = value;
                SetChartArea();
                this.Invalidate();
            }
        }




    #endregion

    #region "Standard Deviation"

        [Category("StandardDeviation"), DefaultValue(true)]
        public bool ShowStandardDeviationLines
        {
            get { return _showStandardDeviationLines; }
            set
            {
                _showStandardDeviationLines = value;
                this.Invalidate();
            }
        }


    #endregion

    #region "Grid Lines"

        [Category("Grid Lines"), DefaultValue(true)]
        public bool ShowGridLines
        {
            get { return _showGridLines; }
            set
            {
                _showGridLines = value;
                this.Invalidate();
            }
        }

        [Category("Grid Lines")]
        public Color GridLineColor
        {
            get { return _gridLineColor; }
            set
            {
                _gridLineColor = value;
                this.Invalidate();
            }
        }




    #endregion

    #region "Collections"

        [Category("Collections")]
        public ArrayList Lines
        {
            get { return _lines; }
        }

        [Category("Collections")]
        public ArrayList SecondaryLines
        {
            get { return _linesSecondary; }
        }

    #endregion


    #endregion

    #region "Public Methods"

        public void ClearLines()
        {
            //Un-subscribe from each line's animating event
            foreach (zLine line in _lines)
            {
                line.LineAnimating -= new zLine.LineAnimatingHandler(LineAnimating);
            }

            _lines.Clear();
            _linesSecondary.Clear();
            _linesStandardDeviation.Clear();

            _xAxisItemCount = 0;
        }

        public void BalanceLines()
        {
            _plots.Clear();

            foreach (zLine line in _lines)
            {
                foreach (zLine.Plot plot in line.Plots)
                {
                    int i = _plots.IndexOf(plot.Name);
                    if (i == -1)
                    {
                        zLinePlotInfo plotInfo = new zLinePlotInfo(plot.Name);
                        _plots.Add(plotInfo);
                    }
                }
            }

            //_plots.Sort();

            
            foreach (zLine line in _lines)
            {
                foreach (zLinePlotInfo plotInfo in _plots)
                {
                    int j = line.Plots.IndexOf(plotInfo.PlotName);
                    if (j == -1)
                    {
                        line.AddPlot(plotInfo.PlotName);                        
                    }
                }

                line.Sort();

                //foreach (zLine.Plot plot in line.Plots)
                //{
                //    MessageBox.Show(line.Name + " - " + plot.Name);
                //}

            }
            

            CreateGraphicsPath();

        }

        public void AddLine(params zLine[] line)
        {
            foreach(zLine l in line)
            {
                _lines.Add(l);

                if (l.PlotCount >= _xAxisItemCount)
                {
                    _xAxisItemCount = l.PlotCount;
                    _pArray = new string[_xAxisItemCount + 1];
                    int x = 1;
                    foreach (zLine.Plot plot in l.Plots)
                    {
                        _pArray[x] = plot.Name;
                        x++;
                    }
                }

                CreateGraphicsPath();

                SetMinMaxIncrement();
                
                //Subscribe to the line's animating event
                l.LineAnimating += new zLine.LineAnimatingHandler(LineAnimating);                

            }

            
            this.Invalidate();
        }

        public void SetMinMaxIncrement()
        {
            if (_autoMinMaxIncrement)
            {
                decimal min = 0;
                decimal max = 0;

                //Loop through each plot in each line and return the highest value and lowest value
                //This includes the standard deviation lines
                foreach (zLine l in _lines)
                {
                    foreach (zLine.Plot p in l.Plots)
                    {
                        if (max == 0) { max = p.Value; }
                        else if (p.Value > max) { max = p.Value; }

                        if (min == 0) { min = p.Value; }
                        else if (p.Value < min) { min = p.Value; }
                    }
                }

                foreach (zLine l in _linesStandardDeviation)
                {
                    foreach (zLine.Plot p in l.Plots)
                    {
                        if (max == 0) { max = p.Value; }
                        else if (p.Value > max) { max = p.Value; }

                        if (min == 0) { min = p.Value; }
                        else if (p.Value < min) { min = p.Value; }
                    }
                }

                decimal increment = 1;
                int diff = 1;
                //max = (decimal)Conversion.Conversion.RoundUp(max);
                //min = (decimal)Conversion.Conversion.RoundDown(min);


                diff = (int)((max - min));

                if (diff <= 0)
                {
                    increment = (decimal)((max - min)) / 8;
                }
                else
                {
                    for (int i = 1; i <= 1000; i++)
                    {
                        if ((diff <= i) && (diff >= i - 1))
                        {
                            increment = i * 0.20M;
                            break;
                        }
                    }
                }

                


                //if (increment <= 0)
                //{
                //    increment = 1;
                //}

                max = max + increment;
                min = min - increment;

                _yAxisIncrement = increment;
                _yAxisMaximum = max;
                _yAxisMinimum = min;


                //increment = (float)Conversion.Conversion.RoundDown(increment);

            }
        }

        public void AddStandardDeviationLine(zLine line)
        {
            decimal min = _yAxisMinimum;
            decimal max = _yAxisMaximum;

            decimal p = 0;
            int i = 0;
            foreach (zLine.Plot plot in line.Plots)
            {
                if (i == 0)
                {
                    p = plot.Value;
                }
                else
                {
                    if (plot.Value != p)
                    {
                        throw new Exception("All standard deviation plots must have the same value");
                    }
                }
                i++;
            }

            

            _linesStandardDeviation.Add(line);
        }

        public void AddStandardDeviationLine(decimal value, Color lineColor, DashStyle lineDashStyle, float lineThickness)
        {
            
            zLine line = new zLine("SD");
            line.LineColor = lineColor;
            line.LineDashStyle = lineDashStyle;
            line.LineThickness = lineThickness;
            line.LineType = zLine.LineTypes.Wavy;
            

            for (int i=0; i <= _xAxisItemCount - 1; i++)
            {
                line.AddPlot("SD", value);
            }

            _linesStandardDeviation.Add(line);

            SetMinMaxIncrement();

        }

        public void SetAllLineThickness(float thickness)
        {
            foreach (zLine line in _lines)
            {
                line.LineThickness = thickness;
            }

            foreach (zLine line in _linesSecondary)
            {
                line.LineThickness = thickness;
            }
        }

        public void HideLine(string lineName, LineTypeEnum type)
        {
            int i = 0;
            switch (type)
            {
                case LineTypeEnum.Primary:
                    {
                        i = _lines.IndexOf(lineName);
                        break;
                    }
                case LineTypeEnum.Secondary:
                    {
                        i = _linesSecondary.IndexOf(lineName);
                        break;
                    }
            }

            if (i != -1)
            {
                HideLine(i, type);
                this.Invalidate();
            }
        }

        public void HideLine(int index, LineTypeEnum type)
        {
            switch (type)
            {
                case LineTypeEnum.Primary:
                    {
                        break;
                    }
                case LineTypeEnum.Secondary:
                    {
                        index += _lines.Count - 1;

                        break;
                    }
            }

            try
            {
                _lineIsVisible[index] = false;
                this.Invalidate();
            }
            catch (Exception)
            {

            }

        }

        public void ShowLine(string lineName, LineTypeEnum type)
        {
            int i = 0;
            switch (type)
            {
                case LineTypeEnum.Primary:
                    {
                        i = _lines.IndexOf(lineName);
                        break;
                    }
                case LineTypeEnum.Secondary:
                    {
                        i = _linesSecondary.IndexOf(lineName);
                        break;
                    }
            }

            if (i != -1)
            {
                ShowLine(i, type);
                this.Invalidate();
            }
        }

        public void ShowLine(int index, LineTypeEnum type)
        {
            switch (type)
            {
                case LineTypeEnum.Primary:
                    {
                        break;
                    }
                case LineTypeEnum.Secondary:
                    {
                        index += _lines.Count - 1;

                        break;
                    }
            }
            try
            {
                _lineIsVisible[index] = true;
                this.Invalidate();
            }
            catch (Exception)
            {

            }

        }

        public enum LineTypeEnum
        {
            Primary,
            Secondary            
        }


    #endregion

    #region "Private Methods"


        private void LineAnimating(object sender, LineAnimatingEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            this.Invalidate();
            
        }

        private void SetChartArea()
        {
            //Create a new bitmap
            _image = new Bitmap(this.Width, this.Height);

            //Create a Graphics object from the new bitmap
            _imageGraphics = Graphics.FromImage(_image);
            _imageGraphics.SmoothingMode = SmoothingMode.HighQuality;
            _imageGraphics.CompositingMode = CompositingMode.SourceCopy;
            _imageGraphics.CompositingQuality = CompositingQuality.HighQuality;



            int topMargin = 0;
            float topMarginF;

            int width = 0;
            float widthF;

            int height = 0;
            float heightF;

            //Compute the top margin
            if(_showSubTitle)
            {
                topMargin = (int)(35 + _titleSize + _subTitleSize);
                topMarginF = (float)(35 + _titleSize + _subTitleSize);
            }
            else
            {
                topMargin = (int)(30 + _titleSize);
                topMarginF = (float)(30 + _titleSize);
            }


            //compute the legend margin
            if(_showLegend)
            {
                    width = (int)(this.Width - (_leftMargin * 2) - 90);
                    widthF = (float)(this.Width - (_leftMargin * 2) - 90);
            }
            else
            {
                    width = (int)(this.Width - (_leftMargin * 2));
                    widthF = (float)(this.Width - (_leftMargin * 2));
            }

            //60 pixel margin for the secondary axis
            if (_showSecondary_Y_Axis)
            {
                width = width - 60;
                widthF = widthF - 60;
            }


            height = (int)(this.Height - topMargin - 125);
            heightF = (float)(this.Height - topMarginF - 125);



            //Create the new Rectangles representing the chart area
            _chartArea = new Rectangle((int)_leftMargin, topMargin, width, height);
            _chartAreaF = new RectangleF((float)_leftMargin, topMarginF, widthF, heightF);

        }

        private void CreateGraphicsPath()
        {
            int plots = 0;
            foreach (zLine line in _lines)
            {
                plots += line.PlotCount;
            }

            int plotsSecondary = 0;
            foreach (zLine line in _linesSecondary)
            {
                plotsSecondary += line.PlotCount;
            }

            _gpLine = new GraphicsPath[_lines.Count + _linesSecondary.Count];
            _lineIsVisible = new bool[_lines.Count + _linesSecondary.Count];
            _gpPlot = new GraphicsPath[plots + plotsSecondary];
            _gpLegend = new GraphicsPath[_lines.Count + _linesSecondary.Count];
            _gpLegendImage = new GraphicsPath[_lines.Count + _linesSecondary.Count];
            _gpLegendCheckBox = new GraphicsPath[_lines.Count + _linesSecondary.Count];
            _gpXAxis = new GraphicsPath[_xAxisItemCount + 1];


            for (int i = 0; i <= (_lines.Count + _linesSecondary.Count - 1); i++)
            {
                _gpLine[i] = new GraphicsPath();
                _lineIsVisible[i] = true;
                _gpLegend[i] = new GraphicsPath();
                _gpLegendImage[i] = new GraphicsPath();
                _gpLegendCheckBox[i] = new GraphicsPath();
            }

            for (int i = 0; i <= (plots + plotsSecondary - 1); i++)
            {
                _gpPlot[i] = new GraphicsPath();
            }

            for (int i = 0; i <= _xAxisItemCount; i++)
            {
                _gpXAxis[i] = new GraphicsPath();
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Disposing)
            {
                return;
            }

            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.CompositingMode = CompositingMode.SourceOver;
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }
            catch (Exception ex)
            {

            }

            

            Graphics g = e.Graphics;


            PaintBackground(ref g, ref _imageGraphics);
            try
            {
                PaintCopyright(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {
                
            }
            
            
            try
            {
                PaintTitle(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {
                
            }
            try
            {
                PaintXAxisHighlight(ref g);
            }
            catch (Exception)
            {

            }

            try
            {
                PaintGridLines(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {

            }


            try
            {
                PaintStandardDeviationLines(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {

            }

            try
            {
                PaintLines(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {

            }


            try
            {
                PaintPlots(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {

            }

            try
            {
                PaintLegend(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {

            }


            try
            {
                PaintXAxis(ref g, ref _imageGraphics);
            }
            catch (Exception ex)
            {

            }



            try
            {
                PaintTip(ref g);
            }
            catch (Exception ex)
            {

            }


            _mouseClicked = false;          


        }


        protected virtual void PaintBackground(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//
            g.Clear(this.BackColor);
            bitmap.Clear(this.BackColor);
        }

        protected void PaintCopyright(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//
            //g.DrawString("© 2010-" + DateTime.Now.Year.ToString() + " zLineChart ~ Michael Zomparelli",
            //             new Font(this.Font.FontFamily,
            //             9, 
            //             FontStyle.Regular,
            //             GraphicsUnit.Pixel),
            //             new SolidBrush(Color.Black),
            //             new PointF(5, this.Height - 10));
            g.DrawString("zLineChart ~ Michael Zomparelli",
                         new Font(this.Font.FontFamily,
                         9,
                         FontStyle.Regular,
                         GraphicsUnit.Pixel),
                         new SolidBrush(Color.Black),
                         new PointF(5, this.Height - 10));
            bitmap.DrawString("zLineChart ~ Michael Zomparelli",
                         new Font(this.Font.FontFamily,
                         9,
                         FontStyle.Regular,
                         GraphicsUnit.Pixel),
                         new SolidBrush(Color.Black),
                         new PointF(5, this.Height - 10));
        }

        protected virtual void PaintTitle(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            
            Font titleFont = new Font(this.Font.FontFamily, _titleSize, FontStyle.Bold, GraphicsUnit.Pixel);
            Font subTitleFont = new Font(this.Font.FontFamily, _subTitleSize, FontStyle.Bold, GraphicsUnit.Pixel);

            Brush titleBrush = new SolidBrush(_titleColor);
            Brush subTitleBrush = new SolidBrush(_subTitleColor);

            //Draw the title
            if (_showTitle)
            {
                g.DrawString(_title, titleFont, titleBrush, this.Width / 2, 10, sf);
                bitmap.DrawString(_title, titleFont, titleBrush, this.Width / 2, 10, sf);
            }

            //Draw the sub title
            if (_showSubTitle)
            {
                g.DrawString(_subTitle, subTitleFont, subTitleBrush, this.Width / 2, 15 + _titleSize, sf);
                bitmap.DrawString(_subTitle, subTitleFont, subTitleBrush, this.Width / 2, 15 + _titleSize, sf);
            }

            //Dispose
            sf.Dispose();
            titleFont.Dispose();
            subTitleFont.Dispose();
            titleBrush.Dispose();
            subTitleBrush.Dispose();

        }

        protected virtual void PaintGridLines(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            

            //Draw the chart area border
            g.DrawRectangle(new Pen(_gridLineColor, 1), _chartArea);
            bitmap.DrawRectangle(new Pen(_gridLineColor, 1), _chartArea);


            decimal min       = _yAxisMinimum;
            decimal max       = _yAxisMaximum;
            decimal nMax      = (max - min);
            decimal increment = _yAxisIncrement;
            float hLines = 0;
            if ((increment != 0) && (nMax != 0))
            {
                hLines = _chartArea.Height / ((float)(nMax / increment));
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;

            Brush axisTextBrush = new SolidBrush(_yAxisFontColor);
            Font axisTextFont = new Font(this.Font.FontFamily, _yAxisFontSize, FontStyle.Regular, GraphicsUnit.Pixel);

            //This loop paints the lines and draws the Y-Axis value range
            for (int i = 0; i <= (int)hLines; i++)
            {
                if (max - (increment * i) <= min)
                {
                    break;
                }
                else
                {
                    float line = (float)(Math.Round(_chartArea.Y + (hLines * i), 0));

                    if ((i != 0) && (i != (int)hLines))
                    {
                        //float value = (float)(Math.Round(max - (increment * i), _yAxisDecimalPlaces));
                        decimal value = (decimal)(max - (increment * i));

                        if (_showGridLines)
                        {
                            g.DrawLine(new Pen(_gridLineColor, 1), _chartArea.X - 5, line + 1, _chartArea.X + _chartArea.Width, line + 1);
                            bitmap.DrawLine(new Pen(_gridLineColor, 1), _chartArea.X - 5, line + 1, _chartArea.X + _chartArea.Width, line + 1);
                        }
                        g.DrawString(_xAxis_formatString + Math.Round(value, _yAxisDecimalPlaces).ToString(),
                                     axisTextFont, 
                                     axisTextBrush, 
                                     g.ClipBounds.X + _chartArea.X - 8, 
                                     (float)(Math.Round(line - (axisTextFont.Size / 2), 5)), 
                                     sf);

                        bitmap.DrawString(_xAxis_formatString + Math.Round(value, 
                                          _yAxisDecimalPlaces).ToString(), 
                                          axisTextFont, 
                                          axisTextBrush, 
                                          g.ClipBounds.X + _chartArea.X - 8, 
                                          (float)(Math.Round(line - (axisTextFont.Size / 2), 5)), 
                                          sf);
                    }
                    else if (i == 0)
                    {
                        g.DrawLine(new Pen(_gridLineColor, 1), _chartArea.X - 5, line + 1, _chartArea.X, line + 1);
                        bitmap.DrawLine(new Pen(_gridLineColor, 1), _chartArea.X - 5, line + 1, _chartArea.X, line + 1);
                        g.DrawString(_xAxis_formatString + Math.Round(max, _yAxisDecimalPlaces).ToString(), axisTextFont, axisTextBrush, g.ClipBounds.X + _chartArea.X - 8, (float)(Math.Round(line - (axisTextFont.Size / 2), 5)), sf);
                        bitmap.DrawString(_xAxis_formatString + Math.Round(max, _yAxisDecimalPlaces).ToString(), axisTextFont, axisTextBrush, g.ClipBounds.X + _chartArea.X - 8, (float)(Math.Round(line - (axisTextFont.Size / 2), 5)), sf);
                    }
                }
            }

            //Draw the min line and text
            g.DrawLine(new Pen(_gridLineColor, 1), _chartArea.X - 5, _chartArea.Y + _chartArea.Height, _chartArea.X, _chartArea.Y + _chartArea.Height);
            bitmap.DrawLine(new Pen(_gridLineColor, 1), _chartArea.X - 5, _chartArea.Y + _chartArea.Height, _chartArea.X, _chartArea.Y + _chartArea.Height);
            g.DrawString(_xAxis_formatString + Math.Round(min, _yAxisDecimalPlaces).ToString(), axisTextFont, axisTextBrush, g.ClipBounds.X + _chartArea.X - 8, (float)(Math.Round(_chartArea.Y + _chartArea.Height - (axisTextFont.Size / 2), 5)), sf);
            bitmap.DrawString(_xAxis_formatString + Math.Round(min, _yAxisDecimalPlaces).ToString(), axisTextFont, axisTextBrush, g.ClipBounds.X + _chartArea.X - 8, (float)(Math.Round(_chartArea.Y + _chartArea.Height - (axisTextFont.Size / 2), 5)), sf);


            Brush axisNameTextBrush = new SolidBrush(_yAxisNameFontColor);
            Font axisNameTextFont = new Font(this.Font.FontFamily, _yAxisNameFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            sf.FormatFlags = StringFormatFlags.DirectionVertical;
            sf.Alignment = StringAlignment.Center;

            Matrix m = new Matrix();
            //PointF pt = new PointF(g.ClipBounds.X + _leftMargin - 40, g.ClipBounds.Y + _chartArea.Y + (_chartArea.Height / 2));
            m.Rotate(180);
            
            g.Transform = m;
            
            //Draw the Y-Axis Name
            g.DrawString(_yAxisName, axisNameTextFont, axisNameTextBrush, g.ClipBounds.X + g.ClipBounds.Width - _leftMargin + 65, g.ClipBounds.Y + _chartArea.Height + 130 - ((_chartArea.Height / 2)), sf);
            bitmap.DrawString(_yAxisName, axisNameTextFont, axisNameTextBrush, g.ClipBounds.X + g.ClipBounds.Width - _leftMargin + 65, g.ClipBounds.Y + _chartArea.Height + 130 - ((_chartArea.Height / 2)), sf);

            //g.DrawString(_yAxisName, axisNameTextFont, axisNameTextBrush, g.ClipBounds.X + _leftMargin - 40, g.ClipBounds.Y + _chartArea.Y + (_chartArea.Height / 2), sf);
            //bitmap.DrawString(_yAxisName, axisNameTextFont, axisNameTextBrush, g.ClipBounds.X + _leftMargin - 40, g.ClipBounds.Y + _chartArea.Y + (_chartArea.Height / 2), sf);

            g.ResetTransform();
            m.Dispose();

            //Draw the secondary Y-Axis value range
            if (_showSecondary_Y_Axis)
            {

                Brush axisSecondaryTextBrush = new SolidBrush(_yAxisSecondaryFontColor);
                Font axisSecondaryTextFont = new Font(this.Font.FontFamily, _yAxisSecondaryFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                StringFormat sff = new StringFormat();
                sff.Alignment = StringAlignment.Near;
                

                min = _yAxisSecondaryMinimum;
                max = _yAxisSecondaryMaximum;
                increment = _yAxisSecondaryIncrement;
                nMax = (max - min);
                hLines = _chartArea.Height / ((float)(nMax / increment));

                for (int i = 0; i <= (int)hLines; i++)
                {
                    if (max - (increment * i) <= min)
                    {
                        break;
                    }
                    else
                    {
                        int line = (int)(_chartArea.Y + (hLines * i));

                        if ((i != 0) && (i != (int)hLines))
                        {
                            decimal value = (decimal)(max - (increment * i));

                            g.DrawString(_xAxis_formatString + Math.Round(value, _yAxisSecondaryDecimalPlaces).ToString(), axisSecondaryTextFont, axisSecondaryTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 8, (int)(line - (axisSecondaryTextFont.Size / 2)), sff);
                            bitmap.DrawString(_xAxis_formatString + Math.Round(value, _yAxisSecondaryDecimalPlaces).ToString(), axisSecondaryTextFont, axisSecondaryTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 8, (int)(line - (axisSecondaryTextFont.Size / 2)), sff);
                        }
                        else if (i == 0)
                        {
                            g.DrawString(_xAxis_formatString + Math.Round(max, _yAxisSecondaryDecimalPlaces).ToString(), axisSecondaryTextFont, axisSecondaryTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 8, (int)(line - (axisSecondaryTextFont.Size / 2)), sff);
                            bitmap.DrawString(_xAxis_formatString + Math.Round(max, _yAxisSecondaryDecimalPlaces).ToString(), axisSecondaryTextFont, axisSecondaryTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 8, (int)(line - (axisSecondaryTextFont.Size / 2)), sff);
                        }
                    }

                }

                //Draw min and text
                g.DrawString(_xAxis_formatString + Math.Round(min, _yAxisSecondaryDecimalPlaces).ToString(), axisSecondaryTextFont, axisSecondaryTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 8, (int)(_chartArea.Y + _chartArea.Height - (axisSecondaryTextFont.Size / 2)), sff);
                bitmap.DrawString(_xAxis_formatString + Math.Round(min, _yAxisSecondaryDecimalPlaces).ToString(), axisSecondaryTextFont, axisSecondaryTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 8, (int)(_chartArea.Y + _chartArea.Height - (axisSecondaryTextFont.Size / 2)), sff);

                Brush axisSecondaryNameTextBrush = new SolidBrush(_yAxisSecondaryNameFontColor);
                Font axisSecondaryNameTextFont = new Font(this.Font.FontFamily, _yAxisSecondaryNameFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                sff.FormatFlags = StringFormatFlags.DirectionVertical;
                sff.Alignment = StringAlignment.Center;


                //Draw the secondary Y-Axis Name
                g.DrawString(_yAxisSecondaryName, axisSecondaryNameTextFont, axisSecondaryNameTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 50, g.ClipBounds.Y + _chartArea.Y + (_chartArea.Height / 2), sff);
                bitmap.DrawString(_yAxisSecondaryName, axisSecondaryNameTextFont, axisSecondaryNameTextBrush, g.ClipBounds.X + _chartArea.X + _chartArea.Width + 50, g.ClipBounds.Y + _chartArea.Y + (_chartArea.Height / 2), sff);

                sff.Dispose();

                axisSecondaryTextBrush.Dispose();
                axisSecondaryTextFont.Dispose();
                axisSecondaryNameTextBrush.Dispose();
                axisSecondaryNameTextFont.Dispose();

            }



            //Dispose
            sf.Dispose();
            axisTextBrush.Dispose();
            axisTextFont.Dispose();
            axisNameTextBrush.Dispose();
            axisNameTextFont.Dispose();

            
        }

        protected virtual void PaintLines(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            if (_lines.Count < 1) { return; }

            decimal min = _yAxisMinimum;
            decimal max = _yAxisMaximum;
            decimal nMax = (max - min);
            decimal increment = _yAxisIncrement;

            float vLines = _chartArea.Width / _xAxisItemCount;

            float hLines = 0;
            if ((increment != 0) && (nMax != 0))
            {
                hLines = _chartArea.Height / ((float)(nMax / increment));
            }

            int iPath = 0;

            //Loop through each line
            foreach (zLine line in _lines)
            {
                if (!_lineIsVisible[iPath]) { iPath++; continue; }

                bool mouseIsOver = false;   //this will be used soon
                zLine.Plot plot;
                Point lastPlot = new Point(0, 0);
                Point currentPlot = new Point(0, 0);

                bool lastPlotWasNull = false;

                //Create a Point array for as many plots we have in this array
                Point[] points = new Point[_xAxisItemCount];
                //if (line.PlotCount > _xAxisItemCount)
                //{
                //    points = new Point[_xAxisItemCount];
                //}
                //else
                //{
                //    points = new Point[line.PlotCount];
                //}

                //If the mouse is in this line's path then set mouseIsOver to true

                float lineThickness = line.LineThickness;

                if ((_gpLegend[iPath].IsVisible(_x, _y)))
                {
                    mouseIsOver = true;
                    lineThickness += 2;
                }

                _gpLine[iPath].Reset();

                //loop through each plot in this line
                for (int i = 1; i <= _xAxisItemCount; i++)
                {
                    
                    float xx = vLines * i;

                    if (line.PlotCount < i)
                    {
                        break;
                    }

                    plot = (zLine.Plot)line.Plots[i - 1];
                    float plotX;
                    float plotY;
                    //float pixelsFromBottom = (hLines * ((plot.Value - min) / increment)) - g.ClipBounds.Y + _chartArea.Height;
                    float pixelsFromBottom = (float)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height - zConversion.PixelsFromChartValue(plot.Value, max, min, _chartArea.Height));
                    plotX = (xx - (vLines / 2)) + g.ClipBounds.X + _chartArea.X;
                    plotY = pixelsFromBottom;
                    currentPlot = new Point((int)plotX, (int)plotY);

                    //If this line type is wavy then save the points so we can draw a curve.
                    //Otherwise connect each plot now
                    if (line.LineType == zLine.LineTypes.Wavy)
                    {
                        if ((!plot.IsNullPlot) && (!lastPlotWasNull))
                        {
                            points[i - 1] = new Point((int)plotX, (int)plotY);
                        }
                        
                    }
                    else
                    {
                        //Connect this plot with the previous one by drawing a line
                        //Only get the previous plot if i > 1
                        if (i > 1)
                        {

                            Pen pen = new Pen(line.LineColor, lineThickness);
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;

                            if ((!plot.IsNullPlot) && (!lastPlotWasNull))
                            {
                                _gpLine[iPath].AddLine(lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);

                                g.DrawLine(pen, lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);
                                bitmap.DrawLine(pen, lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);
                            }
                            

                            pen.Dispose();
                            
                        }

                        lastPlot = new Point(currentPlot.X, currentPlot.Y);

                    }

                    if (plot.IsNullPlot)
                    {
                        lastPlotWasNull = true;
                    }
                    else
                    {
                        lastPlotWasNull = false;
                    }

                }

                //Now we can draw a curved line if this line's line type is wavy
                if (line.LineType == zLine.LineTypes.Wavy)
                {
                    Pen pen = new Pen(line.LineColor, lineThickness);
                    pen.EndCap = LineCap.Round;
                    pen.StartCap = LineCap.Round;
                    pen.LineJoin = LineJoin.Round;

                    _gpLine[iPath].Reset();
                    _gpLine[iPath].AddCurve(points);

                    g.DrawCurve(pen, points);
                    bitmap.DrawCurve(pen, points);
                }

                iPath++;

            }


            //Now we do it all over again with the secondary axis
            if (_showSecondary_Y_Axis)
            {

                min = _yAxisSecondaryMinimum;
                max = _yAxisSecondaryMaximum;
                nMax = (max - min);
                increment = _yAxisSecondaryIncrement;

                vLines = _chartArea.Width / _xAxisItemCount;
                hLines = _chartArea.Height / ((float)(nMax / increment));

                foreach (zLine line in _linesSecondary)
                {
                    if (!_lineIsVisible[iPath]) { iPath++; continue; }

                    float lineThickness = line.LineThickness;
                    bool mouseIsOver = false;   //this will be used soon
                    zLine.Plot plot;
                    Point lastPlot = new Point(0, 0);
                    Point currentPlot = new Point(0, 0);

                    bool lastPlotWasNull = false;
                    bool currentPlotIsNull = false;

                    //Create a Point array for as many plots we have in this array
                    Point[] points;
                    if (line.PlotCount - 1 > _xAxisItemCount)
                    {
                        points = new Point[_xAxisItemCount];
                    }
                    else
                    {
                        points = new Point[line.PlotCount - 1];
                    }

                    //If the mouse is in this line's path then set mouseIsOver to true
                    if (_gpLegend[iPath].IsVisible(_x, _y))
                    {
                        mouseIsOver = true;
                        lineThickness += 2;
                    }

                    _gpLine[iPath].Reset();

                    //loop through each plot in this line
                    for (int i = 1; i <= _xAxisItemCount; i++)
                    {
                        float xx = vLines * i;

                        if (line.PlotCount < i)
                        {
                            break;
                        }

                        plot = (zLine.Plot)line.Plots[i - 1];
                        float plotX;
                        float plotY;
                        float pixelsFromBottom = (int)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height - zConversion.PixelsFromChartValue(plot.Value, max, min, _chartArea.Height));
                        plotX = (xx - (vLines / 2)) + g.ClipBounds.X + _chartArea.X;
                        plotY = pixelsFromBottom;
                        currentPlot = new Point((int)plotX, (int)plotY);

                        //If this line type is wavy then save the points so we can draw a curve.
                        //Otherwise connect each plot now
                        if (line.LineType == zLine.LineTypes.Wavy)
                        {
                            if (!plot.IsNullPlot)
                            {
                                points[i - 1] = new Point((int)plotX, (int)plotY);
                            }
                        }
                        else
                        {
                            //Connect this plot with the previous one by drawing a line
                            //Only get the previous plot if i > 1
                            if (i > 1)
                            {
                                Pen pen = new Pen(line.LineColor, lineThickness);
                                pen.StartCap = LineCap.Round;
                                pen.EndCap = LineCap.Round;

                                if ((!plot.IsNullPlot) && (!lastPlotWasNull))
                                {
                                    _gpLine[iPath].AddLine(lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);

                                    g.DrawLine(pen, lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);
                                    bitmap.DrawLine(pen, lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);
                                }

                                pen.Dispose();

                                if (plot.IsNullPlot) 
                                { 
                                    lastPlotWasNull = true; 
                                }
                                else
                                {
                                    lastPlotWasNull = false;
                                }
                            }

                            lastPlot = new Point(currentPlot.X, currentPlot.Y);

                        }

                    }

                    //Now we can draw a curved line if this line's line type is wavy
                    if (line.LineType == zLine.LineTypes.Wavy)
                    {
                        Pen pen = new Pen(line.LineColor, lineThickness);
                        pen.LineJoin = LineJoin.Round;

                        _gpLine[iPath].Reset();
                        _gpLine[iPath].AddCurve(points);

                        g.DrawCurve(pen, points);
                        bitmap.DrawCurve(pen, points);
                    }

                    iPath++;

                }

            }




        }

        protected virtual void PaintStandardDeviationLines(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            if (!_showStandardDeviationLines)
            {
                return;
            }

            if (_linesStandardDeviation.Count < 1) { return; }

            decimal min = _yAxisMinimum;
            decimal max = _yAxisMaximum;
            decimal nMax = (max - min);
            decimal increment = _yAxisIncrement;
            float vLines = _chartArea.Width / _xAxisItemCount;
            float hLines = 0;
            if ((increment != 0) && (nMax != 0))
            {
                hLines = _chartArea.Height / ((float)(nMax / increment));
            }

            //int iPath = 0;

            //Loop through each line
            foreach (zLine line in _linesStandardDeviation)
            {

                bool mouseIsOver = false;   //this will be used soon
                zLine.Plot plot;
                Point lastPlot = new Point(0, 0);
                Point currentPlot = new Point(0, 0);

                //Create a Point array for as many plots we have in this array
                Point[] points = new Point[_xAxisItemCount];

                //If the mouse is in this line's path then set mouseIsOver to true

                float lineThickness = line.LineThickness;

                //loop through each plot in this line
                for (int i = 1; i <= _xAxisItemCount; i++)
                {
                    float xx = vLines * i;

                    if (line.PlotCount < i)
                    {
                        break;
                    }

                    plot = (zLine.Plot)line.Plots[i - 1];
                    float plotX;
                    float plotY;
                    //float pixelsFromBottom = (hLines * ((plot.Value - min) / increment)) - g.ClipBounds.Y + _chartArea.Height;
                    float pixelsFromBottom = (int)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height - zConversion.PixelsFromChartValue(plot.Value, max, min, _chartArea.Height));
                    plotX = (xx - (vLines / 2)) + g.ClipBounds.X + _chartArea.X;
                    plotY = pixelsFromBottom;
                    currentPlot = new Point((int)plotX, (int)plotY);

                    //If this line type is wavy then save the points so we can draw a curve.
                    //Otherwise connect each plot now
                    if (line.LineType == zLine.LineTypes.Wavy)
                    {
                        points[i - 1] = new Point((int)plotX, (int)plotY);
                    }
                    else
                    {
                        //Connect this plot with the previous one by drawing a line
                        //Only get the previous plot if i > 1
                        if (i > 1)
                        {

                            Pen pen = new Pen(line.LineColor, lineThickness);
                            pen.DashStyle = line.LineDashStyle;
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;

                            //_gpLine[iPath].AddLine(lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);

                            g.DrawLine(pen, lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);
                            bitmap.DrawLine(pen, lastPlot.X, lastPlot.Y, currentPlot.X, currentPlot.Y);

                            pen.Dispose();
                        }

                        lastPlot = new Point(currentPlot.X, currentPlot.Y);

                    }

                }

                //Now we can draw a curved line if this line's line type is wavy
                if (line.LineType == zLine.LineTypes.Wavy)
                {
                    Pen pen = new Pen(line.LineColor, lineThickness);
                    pen.DashStyle = line.LineDashStyle;
                    pen.LineJoin = LineJoin.Round;

                    //_gpLine[iPath].Reset();
                    //_gpLine[iPath].AddCurve(points);

                    g.DrawCurve(pen, points);
                    bitmap.DrawCurve(pen, points);
                }

                //iPath++;

            }

        }


        protected virtual void PaintPlots(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            if (_lines.Count < 1) { return; }

            _tips.Clear();
            decimal min = _yAxisMinimum;
            decimal max = _yAxisMaximum;
            decimal nMax = (max - min);
            decimal increment = _yAxisIncrement;

            float vLines = _chartArea.Width / _xAxisItemCount;

            float hLines = 0;
            if ((increment != 0) && (nMax != 0))
            {
                hLines = _chartArea.Height / ((float)(nMax / increment));
            }

            int iPath = 0;
            int iPathLine = 0;
            bool mouseOnPlot = false;

            //***********************************//
            //FOR EACH LINE
            foreach (zLine line in _lines)
            {
                if (!_lineIsVisible[iPathLine]) { iPathLine++; continue; }

                zLine.Plot plot;
                Point lastPlot = new Point(0, 0);
                Point currentPlot = new Point(0, 0);

                Color[] color = new Color[1];
                color[0] = line.PlotColor;

                //***********************************//
                //FOR EACH PLOT
                for (int i = 1; i <= _xAxisItemCount; i++)
                {

                    float xx = vLines * i;

                    if (line.PlotCount < i) { break; }

                    plot = (zLine.Plot)line.Plots[i - 1];
                    float plotX;
                    float plotY;
                    float pixelsFromBottom = (int)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height - zConversion.PixelsFromChartValue(plot.Value, max, min, _chartArea.Height));

                    plotX = (xx - (vLines / 2)) + g.ClipBounds.X + _chartArea.X;
                    plotY = pixelsFromBottom;
                    currentPlot = new Point((int)plotX, (int)plotY);

                    float plotSize = line.PlotSize;

                    Rectangle rectPlot = new Rectangle((int)(plotX - (plotSize / 2)), (int)(plotY - (plotSize / 2)), (int)plotSize, (int)plotSize);
                    Rectangle rectDot = new Rectangle((int)plotX, (int)plotY, 3, 3);
                    Rectangle rectNone = new Rectangle((int)plotX, (int)plotY, 1, 1);

                    float centerPoint = 0.3125F;
                    float nPlotSize = plotSize;

                    if ((plot.Value >= min) && (plot.Value <= max)) //Don't plot off the chart
                    {
                        //*****************//
                        //MOUSEOVER PLOT
                        if (_gpPlot[iPath].IsVisible(_x, _y))   //mouse is over this plot
                        {
                            if (_mouseClicked)
                            {
                                //Raise MouseClick event for this plot
                                _mouseClicked = false;
                                if (Plot_MouseClick != null)
                                {
                                    Plot_MouseClick(this, new PlotMouseEventArgs(line.Name, plot.Name, plot.Value, line.LineColor, line.PlotColor, line.PlotShape));
                                }
                                
                            }

                            mouseOnPlot = true;
                            zLineTipInfo info = new zLineTipInfo(line.Name, plot.Name, plot.Value, line.LineColor, line.PlotColor, line.PlotShape);
                            _tips.Add(info);

                            //Raise MouseMove event for this plot
                            if (Plot_MouseMove != null)
                            {
                                Plot_MouseMove(this, new PlotMouseEventArgs(line.Name, plot.Name, plot.Value, line.LineColor, line.PlotColor, line.PlotShape));
                            }
                            
                            //Becasue the mouse is over this plot we need to adjust our rectangle and centerPoint                            
                            centerPoint = 0.625F;
                            nPlotSize = plotSize + _plotMouseOverGrowAmount;
                            rectPlot = new Rectangle((int)(plotX - (nPlotSize / 2)), (int)(plotY - (nPlotSize / 2)), (int)(nPlotSize), (int)(nPlotSize));
                        }
                        //END MOUSEOVER PLOT
                        //*****************//

                        _gpPlot[iPath].Reset();


                        //*******************//
                        //DRAW SHAPE
                        switch (line.PlotShape)
                        {
                            //*******************//
                            //DRAW CIRCLE
                            case zLine.PlotShapes.Circle:
                                //Circle
                                if (!plot.IsNullPlot)
                                {
                                    _gpPlot[iPath].AddEllipse(rectPlot);

                                    PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                    b.SurroundColors = color;
                                    b.CenterColor = Color.White;

                                    g.FillEllipse(b, rectPlot);
                                    g.DrawEllipse(new Pen(line.PlotColor, 1), rectPlot);

                                    bitmap.FillEllipse(b, rectPlot);
                                    bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectPlot);

                                    b.Dispose();
                                }
                                break;
                            //END DRAW CIRCLE
                            //*******************//
                            //*******************//
                            //DRAW DIAMOND
                            case zLine.PlotShapes.Diamond:
                                //Diamond
                                if (!plot.IsNullPlot)
                                {
                                    nPlotSize = mouseOnPlot ? (plotSize * 1.8F) * 2 : (plotSize * 1.8F);

                                    PointF[] pts = Drawing.Shapes.Diamond(new PointF(plotX, plotY), new SizeF(nPlotSize, nPlotSize));

                                    //PointF[] pts = new PointF[4];
                                    //pts[0] = new PointF(plotX, plotY - (nPlotSize / 2));
                                    //pts[1] = new PointF(plotX - (nPlotSize / 2) + (nPlotSize / 4), plotY);
                                    //pts[2] = new PointF(plotX, plotY - (nPlotSize / 2) + nPlotSize);
                                    //pts[3] = new PointF(plotX - (nPlotSize / 2) + ((nPlotSize / 4) * 2) + ((nPlotSize / 4) * 1), plotY);

                                    _gpPlot[iPath].AddPolygon(pts);

                                    PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                    b = new PathGradientBrush(_gpPlot[iPath]);
                                    b.SurroundColors = color;
                                    b.CenterColor = Color.White;
                                    b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                    g.FillPolygon(b, pts);
                                    g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                    bitmap.FillPolygon(b, pts);
                                    bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                    pts = null;
                                    b.Dispose();
                                }
                                break;
                            //END DRAW DIAMOND
                            //*******************//
                            //*******************//
                            //DRAW DOT
                            case zLine.PlotShapes.Dot:
                                //Dot
                                if (!plot.IsNullPlot)
                                {
                                    _gpPlot[iPath].AddEllipse(new Rectangle(rectDot.X - 7, rectDot.Y - 7, rectDot.Width + 10, rectDot.Height + 10));

                                    g.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectDot);
                                    g.DrawEllipse(new Pen(line.PlotColor, 1), rectDot);

                                    bitmap.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectDot);
                                    bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectDot);
                                }
                                break;
                            //END DRAW DOT
                            //*******************//
                            //*******************//
                            //DRAW SQUARE
                            case zLine.PlotShapes.Square:
                                //Square
                                if (!plot.IsNullPlot)
                                {
                                    _gpPlot[iPath].AddRectangle(rectPlot);

                                    PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                    b = new PathGradientBrush(_gpPlot[iPath]);
                                    b.SurroundColors = color;
                                    b.CenterColor = Color.White;
                                    b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                    g.FillRectangle(b, rectPlot);
                                    g.DrawRectangle(new Pen(line.PlotColor, 1), rectPlot);

                                    bitmap.FillRectangle(b, rectPlot);
                                    bitmap.DrawRectangle(new Pen(line.PlotColor, 1), rectPlot);

                                    b.Dispose();
                                }
                                break;
                            //END DRAW SQUARE
                            //*******************//
                            //*******************//
                            //DRAW TRIANGLE
                            case zLine.PlotShapes.Triangle:
                                //Triangle
                                if (!plot.IsNullPlot)
                                {
                                    PointF[] pts = Drawing.Shapes.Triangle(new PointF(plotX, plotY), new SizeF(nPlotSize, nPlotSize));
                                    //PointF[] pts = new PointF[3];
                                    //pts[0] = new PointF(plotX, plotY - (nPlotSize / 2));
                                    //pts[1] = new PointF(plotX - (nPlotSize / 2), plotY - (nPlotSize / 2) + nPlotSize);
                                    //pts[2] = new PointF(plotX - (nPlotSize / 2) + nPlotSize, plotY - (nPlotSize / 2) + nPlotSize);

                                    _gpPlot[iPath].AddPolygon(pts);

                                    PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                    b = new PathGradientBrush(_gpPlot[iPath]);
                                    b.SurroundColors = color;
                                    b.CenterColor = Color.White;
                                    b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                    g.FillPolygon(b, pts);
                                    g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                    bitmap.FillPolygon(b, pts);
                                    bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                    pts = null;
                                    b.Dispose();
                                }
                                break;
                            //END DRAW TRIANGLE
                            //*******************//
                            //*******************//
                            //DRAW CROSS
                            case zLine.PlotShapes.Cross:
                                //Cross
                                if (!plot.IsNullPlot)
                                {
                                    PointF[] pts = Drawing.Shapes.Cross(new PointF(plotX, plotY), new SizeF(nPlotSize, nPlotSize));
                                    //PointF[] pts = new PointF[3];
                                    //pts[0] = new PointF(plotX, plotY - (nPlotSize / 2));
                                    //pts[1] = new PointF(plotX - (nPlotSize / 2), plotY - (nPlotSize / 2) + nPlotSize);
                                    //pts[2] = new PointF(plotX - (nPlotSize / 2) + nPlotSize, plotY - (nPlotSize / 2) + nPlotSize);

                                    _gpPlot[iPath].AddPolygon(pts);

                                    PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                    b = new PathGradientBrush(_gpPlot[iPath]);
                                    b.SurroundColors = color;
                                    b.CenterColor = Color.White;
                                    b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                    g.FillPolygon(b, pts);
                                    g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                    bitmap.FillPolygon(b, pts);
                                    bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                    pts = null;
                                    b.Dispose();
                                }
                                break;
                            //END DRAW CROSS
                            //*******************//
                            //*******************//
                            //DRAW NONE
                            case zLine.PlotShapes.None:
                                //None
                                if (!plot.IsNullPlot)
                                {
                                    _gpPlot[iPath].AddEllipse(new Rectangle(rectNone.X - 7, rectNone.Y - 7, rectNone.Width + 10, rectNone.Height + 10));

                                    g.FillEllipse(new LinearGradientBrush(rectNone, line.PlotColor, line.PlotColor, LinearGradientMode.Vertical), rectNone);
                                    g.DrawEllipse(new Pen(line.PlotColor, 1), rectNone);

                                    bitmap.FillEllipse(new LinearGradientBrush(rectNone, line.PlotColor, line.PlotColor, LinearGradientMode.Vertical), rectNone);
                                    bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectNone);
                                }
                                break;
                            //END DRAW NONE
                            //*******************//
                        }
                        //END DRAW SHAPE
                        //*******************//

                    }

                    iPath++;    //increment iPath to the next Graphic path
                    lastPlot = new Point(currentPlot.X, currentPlot.Y); //record the last plot so we can connect the next plot to it

                    
                }
                //END FOR EACH PLOT
                //***********************************//

                iPathLine++;
            }
            //END FOR EACH LINE
            //***********************************//



            //Now paint the plots for the secondary axis
            if (_showSecondary_Y_Axis)
            {

                min = _yAxisSecondaryMinimum;
                max = _yAxisSecondaryMaximum;
                nMax = (max - min);
                increment = _yAxisSecondaryIncrement;

                vLines = _chartArea.Width / _xAxisItemCount;
                hLines = _chartArea.Height / ((float)(nMax / increment));

                foreach (zLine line in _linesSecondary)
                {
                    if (!_lineIsVisible[iPathLine]) { iPathLine++; continue; }

                    zLine.Plot plot;
                    Point lastPlot = new Point(0, 0);
                    Point currentPlot = new Point(0, 0);

                    Color[] color = new Color[0];
                    color[0] = line.PlotColor;

                    for (int i = 1; i <= _xAxisItemCount; i++)
                    {

                        float xx = vLines * i;

                        if (line.PlotCount < i) { break; }

                        plot = (zLine.Plot)line.Plots[i - 1];
                        float plotX;
                        float plotY;
                        float pixelsFromBottom = (int)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height - zConversion.PixelsFromChartValue(plot.Value, max, min, _chartArea.Height));

                        plotX = (xx - (vLines / 2)) + g.ClipBounds.X + _chartArea.X;
                        plotY = pixelsFromBottom;
                        currentPlot = new Point((int)plotX, (int)plotY);

                        float plotSize = line.PlotSize;

                        Rectangle rectPlot = new Rectangle((int)(plotX - (plotSize / 2)), (int)(plotY - (plotSize / 2)), (int)plotSize, (int)plotSize);
                        Rectangle rectDot = new Rectangle((int)plotX, (int)plotY, 3, 3);
                        Rectangle rectNone = new Rectangle((int)plotX, (int)plotY, 1, 1);

                        float centerPoint = 0.3125F;
                        float nPlotSize = plotSize;

                        if (plot.IsNullPlot) { continue; }  //don't plot null plots

                        if ((plot.Value >= min) && (plot.Value <= max)) //Don't plot off the chart
                        {
                            if (_gpPlot[iPath].IsVisible(_x, _y))   //mouse is over this plot
                            {
                                mouseOnPlot = true;
                                //TODO: Show tool tip
                                zLineTipInfo info = new zLineTipInfo(line.Name, plot.Name, plot.Value, line.LineColor, line.PlotColor, line.PlotShape);
                                _tips.Add(info);


                                //Raise MouseMove event for this plot
                                if (Plot_MouseMove != null)
                                {
                                    Plot_MouseMove(this, new PlotMouseEventArgs(line.Name, plot.Name, plot.Value, line.LineColor, line.PlotColor, line.PlotShape));
                                }

                                if (_mouseClicked)
                                {
                                    //Raise MouseClick event for this plot
                                    if (Plot_MouseClick != null)
                                    {
                                        Plot_MouseClick(this, new PlotMouseEventArgs(line.Name, plot.Name, plot.Value, line.LineColor, line.PlotColor, line.PlotShape));
                                    }
                                    _mouseClicked = false;
                                }

                                //Becasue the mouse is over this plot we need to adjust our rectangle and centerPoint                            
                                centerPoint = 0.625F;
                                nPlotSize = plotSize + _plotMouseOverGrowAmount;
                                rectPlot = new Rectangle((int)(plotX - (nPlotSize / 2)), (int)(plotY - (nPlotSize / 2)), (int)(nPlotSize), (int)(nPlotSize));
                            }

                            _gpPlot[iPath].Reset();

                            switch (line.PlotShape)
                            {
                                case zLine.PlotShapes.Circle:
                                    //Circle
                                    if (!plot.IsNullPlot)
                                    {
                                        _gpPlot[iPath].AddEllipse(rectPlot);

                                        PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                        b.SurroundColors = color;
                                        b.CenterColor = Color.White;

                                        g.FillEllipse(b, rectPlot);
                                        g.DrawEllipse(new Pen(line.PlotColor, 1), rectPlot);

                                        bitmap.FillEllipse(b, rectPlot);
                                        bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectPlot);

                                        b.Dispose();
                                    }
                                    break;
                                case zLine.PlotShapes.Diamond:
                                    //Diamond
                                    if (!plot.IsNullPlot)
                                    {
                                        nPlotSize = mouseOnPlot ? (plotSize * 1.8F) * 2 : (plotSize * 1.8F);

                                        PointF[] pts = new PointF[4];
                                        pts[0] = new PointF(plotX, plotY - (nPlotSize / 2));
                                        pts[1] = new PointF(plotX - (nPlotSize / 2) + (nPlotSize / 4), plotY);
                                        pts[2] = new PointF(plotX, plotY - (nPlotSize / 2) + nPlotSize);
                                        pts[3] = new PointF(plotX - (nPlotSize / 2) + ((nPlotSize / 4) * 2) + ((nPlotSize / 4) * 1), plotY);

                                        _gpPlot[iPath].AddPolygon(pts);

                                        PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                        b = new PathGradientBrush(_gpPlot[iPath]);
                                        b.SurroundColors = color;
                                        b.CenterColor = Color.White;
                                        b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                        g.FillPolygon(b, pts);
                                        g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                        bitmap.FillPolygon(b, pts);
                                        bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                        pts = null;
                                        b.Dispose();
                                    }
                                    break;
                                case zLine.PlotShapes.Dot:
                                    //Dot
                                    if (!plot.IsNullPlot)
                                    {
                                        _gpPlot[iPath].AddEllipse(new Rectangle(rectDot.X - 7, rectDot.Y - 7, rectDot.Width + 10, rectDot.Height + 10));

                                        g.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectDot);
                                        g.DrawEllipse(new Pen(line.PlotColor, 1), rectDot);

                                        bitmap.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectDot);
                                        bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectDot);
                                    }
                                    break;
                                case zLine.PlotShapes.Square:
                                    //Square
                                    if (!plot.IsNullPlot)
                                    {
                                        _gpPlot[iPath].AddRectangle(rectPlot);

                                        PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                        b = new PathGradientBrush(_gpPlot[iPath]);
                                        b.SurroundColors = color;
                                        b.CenterColor = Color.White;
                                        b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                        g.FillRectangle(b, rectPlot);
                                        g.DrawRectangle(new Pen(line.PlotColor, 1), rectPlot);

                                        bitmap.FillRectangle(b, rectPlot);
                                        bitmap.DrawRectangle(new Pen(line.PlotColor, 1), rectPlot);

                                        b.Dispose();
                                    }
                                    break;
                                case zLine.PlotShapes.Triangle:
                                    //Triangle
                                    if (!plot.IsNullPlot)
                                    {
                                        PointF[] pts = new PointF[3];
                                        pts[0] = new PointF(plotX, plotY - (nPlotSize / 2));
                                        pts[1] = new PointF(plotX - (nPlotSize / 2), plotY - (nPlotSize / 2) + nPlotSize);
                                        pts[2] = new PointF(plotX - (nPlotSize / 2) + nPlotSize, plotY - (nPlotSize / 2) + nPlotSize);

                                        _gpPlot[iPath].AddPolygon(pts);

                                        PathGradientBrush b = new PathGradientBrush(_gpPlot[iPath]);
                                        b = new PathGradientBrush(_gpPlot[iPath]);
                                        b.SurroundColors = color;
                                        b.CenterColor = Color.White;
                                        b.CenterPoint = new PointF(plotX - (line.PlotSize * centerPoint), plotY - (line.PlotSize * centerPoint));

                                        g.FillPolygon(b, pts);
                                        g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                        bitmap.FillPolygon(b, pts);
                                        bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                                        pts = null;
                                        b.Dispose();
                                    }
                                    break;
                                case zLine.PlotShapes.Cross:
                                    //Cross
                                    //TODO: Draw Cross
                                    throw new NotImplementedException("The cross plot has not been implemented yet.");
                                    break;
                                case zLine.PlotShapes.None:
                                    //None
                                    if (!plot.IsNullPlot)
                                    {
                                        _gpPlot[iPath].AddEllipse(new Rectangle(rectNone.X - 7, rectNone.Y - 7, rectNone.Width + 10, rectNone.Height + 10));

                                        g.FillEllipse(new LinearGradientBrush(rectNone, line.PlotColor, line.PlotColor, LinearGradientMode.Vertical), rectNone);
                                        g.DrawEllipse(new Pen(line.PlotColor, 1), rectNone);

                                        bitmap.FillEllipse(new LinearGradientBrush(rectNone, line.PlotColor, line.PlotColor, LinearGradientMode.Vertical), rectNone);
                                        bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectNone);
                                    }
                                    break;
                            }

                        }

                        iPath++;
                        lastPlot = new Point(currentPlot.X, currentPlot.Y);


                    }

                    iPathLine++;
                }



            }

            if (!mouseOnPlot)
            {
                
            }

            

        }

        protected virtual void PaintLegend(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            if (!_showLegend)
            {
                return;
            }

            float lastRect = g.ClipBounds.Y + _chartArea.Y;

            int iW = (int)(g.ClipBounds.X + _chartArea.X + _chartArea.Width + 55 + _legendOffset);

            int iPath = 0;

            SolidBrush b = new SolidBrush(Color.FromArgb(_legendOpacity, _legendFontColor.R, _legendFontColor.G, _legendFontColor.B));
            PathGradientBrush plotBrush;            
            

            foreach (zLine line in _lines)
            {

                b = new SolidBrush(Color.FromArgb(_legendOpacity, _legendFontColor.R, _legendFontColor.G, _legendFontColor.B));

                Color[] color = new Color[1];
                color[0] = line.PlotColor;

                FontStyle fs = _legendFontStyle;

                if ((_gpLegend[iPath].IsVisible(_x, _y)))
                {
                    if (!line.IsAnimating)
                    {
                        line.ThrobbingLineStart();
                    }
                    
                    fs = _legendFontStyleMouseOver;
                    b = new SolidBrush(Color.FromArgb(_legendOpacity, _legendFontColorMouseOver.R, _legendFontColorMouseOver.G, _legendFontColorMouseOver.B));

                    //Raise mouse move event
                    if (Legend_MouseMove != null)
                    {
                        Legend_MouseMove(this, new LegendMouseEventArgs(line.Name, line.LineColor, line.PlotColor, line.PlotShape));
                    }

                    //if the mouse was clicked then raise the mouse click event
                    if(_mouseClicked)
                    {
                        _mouseClicked = false;
                        if(Legend_MouseClick != null)
                        {
                            Legend_MouseClick(this, new LegendMouseEventArgs(line.Name, line.LineColor, line.PlotColor, line.PlotShape));
                        }                        
                    }
                }
                else
                {
                    line.ThrobbingLineStop();
                }

                _gpLegend[iPath].Reset();
                _gpLegendImage[iPath].Reset();

                SizeF s  = g.MeasureString(line.Name + ".", new Font(this.Font.FontFamily, _legendFontSize, fs, GraphicsUnit.Pixel));

                Rectangle rectPlot = new Rectangle((int)(iW), (int)lastRect, (int)_legendFontSize, (int)_legendFontSize);
                Rectangle rectDot = new Rectangle((int)(iW + (int)(_legendFontSize / 2)), (int)lastRect + (int)(_legendFontSize / 2), (int)3, (int)3);
                Rectangle rectNone = new Rectangle((int)(iW + (int)(_legendFontSize / 2)), (int)lastRect + (int)(_legendFontSize / 2), (int)1, (int)1);

                //Draw the ckeckboxes
                if (_showLineCheckBoxes)
                {
                    Point p = new Point((int)(iW - 22), (int)(lastRect));
                    System.Windows.Forms.VisualStyles.CheckBoxState check = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;

                    if (_gpLegendCheckBox[iPath].IsVisible(_x, _y))
                    {
                        if (_mouseClicked)
                        {
                            _lineIsVisible[iPath] = !_lineIsVisible[iPath];
                            _mouseClicked = false;
                        }
                    }

                    if (_lineIsVisible[iPath])
                    {
                        if (_gpLegendCheckBox[iPath].IsVisible(_x, _y))
                        {
                            check = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedHot;
                        }
                        else
                        {
                            check = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
                        }
                    }
                    else
                    {
                        if (_gpLegendCheckBox[iPath].IsVisible(_x, _y))
                        {
                            check = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedHot;
                        }
                        else
                        {
                            check = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
                        }
                    }
                    _gpLegendCheckBox[iPath].Reset();

                    _gpLegendCheckBox[iPath].AddRectangle(new Rectangle((int)(iW - 22), (int)(lastRect), 13, 13));

                    System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(g, p, check);

                }

                
                float centerPoint = 0.3125F;

                //g.FillRectangle(new SolidBrush(Color.FromArgb(_legendOpacity, line.LineColor.R, line.LineColor.G, line.LineColor.B)), new Rectangle((int)(iW), (int)lastRect, (int)_legendFontSize, (int)_legendFontSize));
                //g.DrawRectangle(Pens.Black, new Rectangle((int)(iW), (int)lastRect, (int)_legendFontSize, (int)_legendFontSize));

                //bitmap.FillRectangle(new SolidBrush(Color.FromArgb(_legendOpacity, line.LineColor.R, line.LineColor.G, line.LineColor.B)), new Rectangle((int)(iW), (int)lastRect, (int)_legendFontSize, (int)_legendFontSize));
                //bitmap.DrawRectangle(Pens.Black, new Rectangle((int)(iW), (int)lastRect, (int)_legendFontSize, (int)_legendFontSize));

                //Draw the line for the legend
                g.DrawLine(new Pen(line.LineColor, line.LineThickness), (int)(rectPlot.X - (_legendFontSize / 2)), (int)(rectPlot.Y + (_legendFontSize / 2)), (int)(rectPlot.X + rectPlot.Width + (_legendFontSize / 2)), (int)(rectPlot.Y + (_legendFontSize / 2)));
                bitmap.DrawLine(new Pen(line.LineColor, line.LineThickness), (int)(rectPlot.X - (_legendFontSize / 2)), (int)(rectPlot.Y + (_legendFontSize / 2)), (int)(rectPlot.X + rectPlot.Width + (_legendFontSize / 2)), (int)(rectPlot.Y + (_legendFontSize / 2)));

                //Draw the plot shape
                switch (line.PlotShape)
                {
                    case zLine.PlotShapes.Circle:
                        {
                            _gpLegendImage[iPath].AddEllipse(rectPlot);
                            plotBrush = new PathGradientBrush(_gpLegendImage[iPath]);
                            plotBrush.SurroundColors = color;
                            plotBrush.CenterColor = Color.White;
                            
                            g.FillEllipse(plotBrush, rectPlot);
                            g.DrawEllipse(new Pen(line.PlotColor, 1), rectPlot);

                            bitmap.FillEllipse(plotBrush, rectPlot);
                            bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectPlot);

                            break;
                        }
                    case zLine.PlotShapes.Cross:
                        {

                            break;
                        }
                    case zLine.PlotShapes.Diamond:
                        {
                            PointF[] pts = new PointF[4];
                            //pts[0] = new PointF(rectPlot.X, rectPlot.Y - (_legendFontSize / 2));
                            //pts[1] = new PointF(rectPlot.X - (_legendFontSize / 2) + (_legendFontSize / 4), rectPlot.Y);
                            //pts[2] = new PointF(rectPlot.X, rectPlot.Y - (_legendFontSize / 2) + _legendFontSize);
                            //pts[3] = new PointF(rectPlot.X - (_legendFontSize / 2) + ((_legendFontSize / 4) * 2) + ((_legendFontSize / 4) * 1), rectPlot.Y);
                            pts[0] = new PointF(rectPlot.X + (_legendFontSize / 2), rectPlot.Y);
                            pts[1] = new PointF(rectPlot.X  + ((_legendFontSize / 4) * 3), rectPlot.Y + (_legendFontSize / 2));
                            pts[2] = new PointF(rectPlot.X + (_legendFontSize / 2), rectPlot.Y + _legendFontSize);
                            pts[3] = new PointF(rectPlot.X + (_legendFontSize / 4), rectPlot.Y + (_legendFontSize / 2));

                            _gpPlot[iPath].AddPolygon(pts);

                            plotBrush = new PathGradientBrush(_gpLegendImage[iPath]);
                            plotBrush.SurroundColors = color;
                            plotBrush.CenterColor = Color.White;
                            plotBrush.CenterPoint = new PointF(rectPlot.X - (_legendFontSize * centerPoint), rectPlot.Y - (_legendFontSize * centerPoint));

                            g.FillPolygon(plotBrush, pts);
                            g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                            bitmap.FillPolygon(plotBrush, pts);
                            bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                            pts = null;

                            break;
                        }
                    case zLine.PlotShapes.Dot:
                        {
                            _gpLegendImage[iPath].AddEllipse(new Rectangle(rectDot.X - 7, rectDot.Y - 7, rectDot.Width + 10, rectDot.Height + 10));

                            g.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectDot);
                            g.DrawEllipse(new Pen(line.PlotColor, 1), rectDot);

                            bitmap.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectDot);
                            bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectDot);

                            break;
                        }
                    case zLine.PlotShapes.None:
                        {
                            _gpLegendImage[iPath].AddEllipse(new Rectangle(rectNone.X - 7, rectDot.Y - 7, rectNone.Width + 10, rectNone.Height + 10));

                            g.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectNone);
                            g.DrawEllipse(new Pen(line.PlotColor, 1), rectNone);

                            bitmap.FillEllipse(new LinearGradientBrush(rectDot, Color.White, line.PlotColor, LinearGradientMode.Vertical), rectNone);
                            bitmap.DrawEllipse(new Pen(line.PlotColor, 1), rectNone);

                            break;
                        }
                    case zLine.PlotShapes.Square:
                        {
                            _gpLegendImage[iPath].AddRectangle(rectPlot);

                            plotBrush = new PathGradientBrush(_gpLegendImage[iPath]);
                            plotBrush.SurroundColors = color;
                            plotBrush.CenterColor = Color.White;
                            plotBrush.CenterPoint = new PointF(rectPlot.X - (_legendFontSize * centerPoint), rectPlot.Y - (_legendFontSize * centerPoint));

                            g.FillRectangle(plotBrush, rectPlot);
                            g.DrawRectangle(new Pen(line.PlotColor, 1), rectPlot);

                            bitmap.FillRectangle(plotBrush, rectPlot);
                            bitmap.DrawRectangle(new Pen(line.PlotColor, 1), rectPlot);

                            break;
                        }
                    case zLine.PlotShapes.Triangle:
                        {
                            Point[] pts = new Point[3];
                            //pts[0] = new PointF(rectPlot.X, rectPlot.Y + (_legendFontSize / 2));
                            //pts[1] = new PointF(rectPlot.X - (_legendFontSize / 2), rectPlot.Y - (_legendFontSize / 2) + _legendFontSize);
                            //pts[2] = new PointF(rectPlot.X - (_legendFontSize / 2) + _legendFontSize, rectPlot.Y - (_legendFontSize / 2) + _legendFontSize);
                            pts[0] = new Point((int)(rectPlot.X + (_legendFontSize / 2)), (int)(rectPlot.Y));
                            pts[1] = new Point((int)(rectPlot.X + _legendFontSize), (int)(rectPlot.Y + _legendFontSize));
                            pts[2] = new Point((int)(rectPlot.X), (int)(rectPlot.Y + _legendFontSize));

                            _gpLegendImage[iPath].AddPolygon(pts);

                            plotBrush = new PathGradientBrush(_gpPlot[iPath]);
                            plotBrush.SurroundColors = color;
                            plotBrush.CenterColor = Color.White;
                            plotBrush.CenterPoint = new PointF(rectPlot.X - (_legendFontSize * centerPoint), rectPlot.Y - (_legendFontSize * centerPoint));

                            g.FillPolygon(plotBrush, pts);
                            g.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                            bitmap.FillPolygon(plotBrush, pts);
                            bitmap.DrawPolygon(new Pen(line.PlotColor, 1), pts);

                            pts = null;

                            break;
                        }
                }

                

                _gpLegend[iPath].AddRectangle(new Rectangle((int)(iW - (_legendFontSize / 2)), (int)lastRect, (int)(s.Width + (int)(_legendFontSize + 10)), (int)_legendFontSize));

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;


                g.DrawString(line.Name, new Font(this.Font.FontFamily, _legendFontSize, fs, GraphicsUnit.Pixel), b, (int)(iW + _legendFontSize + 10), (int)(lastRect + (_legendFontSize / 2)), sf);
                bitmap.DrawString(line.Name, new Font(this.Font.FontFamily, _legendFontSize, fs, GraphicsUnit.Pixel), b, (int)(iW + _legendFontSize + 10), (int)(lastRect + (_legendFontSize / 2)), sf);
                
                
                lastRect += ((int)_legendFontSize + 6);

                iPath++;
                
            }
            

            //TODO: Draw the secondary legend


            //Dispose
            b.Dispose();
            

        }

        protected virtual void PaintXAxis(ref Graphics g, ref Graphics bitmap)
        {
            //Paint the control graphics "g" and the Bitmap "bitmap" exactly the same
            //*************************************************************************//

            if (_lines.Count < 1) { return; }
            
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;

            Brush backBrush = new SolidBrush(this.BackColor);
            Brush textBrush = new SolidBrush(_xAxisFontColor);

            float height = this.Height - (g.ClipBounds.Y + _chartArea.Y + _chartArea.Height) - 10;

            Rectangle rectAxis = new Rectangle((int)(g.ClipBounds.X + _chartArea.X), (int)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height + 1), (int)_chartArea.Width, (int)(height - 55));
            float vLines = rectAxis.Width / _xAxisItemCount;

            //g.FillRectangle(backBrush, rectAxis);


            for(int i = 0; i <= _xAxisItemCount; i++)
            {
                float x = vLines * i;
                float plotX;
                float plotY;
                float pixelsFromBottom = 20;

                //plotX = (int)((x - (vLines / 2)) + rectAxis.X + rectAxis.Width / (_xAxisItemCount * 2));
                //float plotXText = (int)((x - vLines) + rectAxis.X + (rectAxis.Width / (_xAxisItemCount * 2)));

                plotX = (int)((x - (vLines / 2)) + rectAxis.X + (vLines / 2));
                float plotXText = (int)((x - (vLines)) + rectAxis.X + (vLines / 2));


                plotY = this.Height - pixelsFromBottom;

                g.DrawLine(new Pen(_xAxisBorderColor, 1), (int)plotX, (int)rectAxis.Y, (int)(plotX + 65), (int)(plotY - 40));
                bitmap.DrawLine(new Pen(_xAxisBorderColor, 1), (int)plotX, (int)rectAxis.Y, (int)(plotX + 65), (int)(plotY - 40));

                Point[] pts = new Point[4];
                pts[0] = new Point((int)(plotX - vLines), (int)rectAxis.Y);
                pts[1] = new Point((int)(plotX), (int)rectAxis.Y);
                pts[2] = new Point((int)(plotX + 65), (int)(plotY - 40));
                pts[3] = new Point((int)(plotX + 65 - vLines), (int)(plotY - 40));


                Font f = new Font(this.Font.FontFamily, _xAxisFontSize, _xAxisFontStyle, GraphicsUnit.Pixel);
                
                if (i != 0)
                {
                    plotY = this.Height - 88;
                    StringFormat sff = new StringFormat();

                        sff.FormatFlags = StringFormatFlags.DirectionVertical;
                        sff.Alignment = StringAlignment.Near;
                        plotXText -= 9;

                    try
                    {
                        Matrix m = new Matrix();
                        PointF pt = new PointF(plotXText, plotY - 35);
                        m.RotateAt(-45, pt);
                        
                        g.Transform = m;
                        bitmap.Transform = m;
                        _gpXAxis[i - 1].Reset();
                        _gpXAxis[i - 1].Transform(g.Transform);
                        _gpXAxis[i - 1].AddPolygon(pts);

                        if (_gpXAxis[i - 1].IsVisible(_x, _y))
                        {
                            f = new Font(this.Font.FontFamily, _xAxisFontSize, _xAxisMouseOverFontStyle, GraphicsUnit.Pixel);
                            
                        }

                        
                        Point p = new Point((int)plotXText, (int)(plotY - 20));
                        SizeF s = g.MeasureString(_pArray[i], f);
                        //_gpXAxis[i - 1].AddRectangle(new Rectangle((int)(p.X + 5), (int)(p.Y - 10), (int)(s.Width - 10), (int)(f.Size + 35)));
                                                
                        g.DrawString(_pArray[i], f, textBrush, plotXText, plotY - 20, sff);
                        bitmap.DrawString(_pArray[i], f, textBrush, plotXText, plotY - 20, sff);

                        g.ResetTransform();
                        bitmap.ResetTransform();
                        _gpXAxis[i - 1].Transform(g.Transform);

                        m.Dispose();
                    }
                    catch (Exception)
                    {

                    }

                }

            }

            //Dispose
            textBrush.Dispose();
            backBrush.Dispose();


        }

        protected virtual void PaintXAxisHighlight(ref Graphics g)
        {

            Brush backBrush = new SolidBrush(Color.FromArgb(255, _xAxisMouseOverBackColor.R, _xAxisMouseOverBackColor.G, _xAxisMouseOverBackColor.B));

            float height = this.Height - (g.ClipBounds.Y + _chartArea.Y + _chartArea.Height) - 10;

            Rectangle rectAxis = new Rectangle((int)(g.ClipBounds.X + _chartArea.X), (int)(g.ClipBounds.Y + _chartArea.Y + _chartArea.Height + 1), (int)_chartArea.Width, (int)(height - 55));
            float vLines = rectAxis.Width / _xAxisItemCount;

            for (int i = 0; i <= _xAxisItemCount; i++)
            {
                float x = vLines * i;
                float plotX;
                float plotY;

                plotX = (int)((x - (vLines / 2)) + g.ClipBounds.X + _chartArea.X + (vLines / 2));

                plotY = g.ClipBounds.Y + _chartArea.Y;                

                        if (_gpXAxis[i].IsVisible(_x, _y))
                        {
                            RectangleF r = new RectangleF(plotX, plotY, vLines, _chartArea.Height);
                            g.FillRectangle(backBrush, r);
                            g.FillPath(backBrush, _gpXAxis[i]);

                            backBrush.Dispose();
                            
                            
                        }
            }

        }

        protected virtual void PaintTip(ref Graphics g)
        {
            int tipWidth = 100;
            int tipHeight = 80;
            int shadowLoc = 8;

                //Currently we have a place for up to 4 tips at once. We can add as many as we need, but we have to figure out where we want them
                for (int x = 0; x <= _tips.Count - 1; x++)
                {

                    Rectangle rectTip = new Rectangle((int)(_x + 10), (int)(_y + 20), (int)tipWidth, (int)tipHeight);
                    Rectangle rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                    switch (x)
                    {
                        case 0:
                            {
                                rectTip = new Rectangle((int)(_x + 10), (int)(_y + 20), (int)tipWidth, (int)tipHeight);
                                rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                                break;
                            }
                        case 1:
                            {
                                rectTip = new Rectangle((int)(_x - 5 - tipWidth), (int)(_y + 20), (int)tipWidth, (int)tipHeight);
                                rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                                break;
                            }
                        case 2:
                            {
                                rectTip = new Rectangle((int)(_x  - 5 - tipWidth), (int)(_y - 5 - tipHeight), (int)tipWidth, (int)tipHeight);
                                rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                                break;
                            }
                        case 3:
                            {
                                rectTip = new Rectangle((int)(_x + 10), (int)(_y - 5 - tipHeight), (int)tipWidth, (int)tipHeight);
                                rectShadow = new Rectangle(rectTip.X + shadowLoc, rectTip.Y + shadowLoc, rectTip.Width, rectTip.Height);

                                break;
                            }
                        default:
                            {

                                break;
                            }
                    }

                    if(x > 3)
                    {
                        return;
                    }

                    zLineTipInfo info = (zLineTipInfo)_tips[x];


                    Brush bg = new SolidBrush(Color.Black);
                    //Brush shadow = new SolidBrush(Color.FromArgb(150, Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B));
                    Brush shadow = new SolidBrush(Color.FromArgb(150, info.LineColor.R, info.LineColor.G, info.LineColor.B));

                    Brush border = new SolidBrush(info.LineColor);
                    Brush text = new SolidBrush(Color.White);
                    Font f = new Font(this.Font.FontFamily, 13, FontStyle.Bold, GraphicsUnit.Pixel);

                    Rectangle rect = new Rectangle(rectTip.X, rectTip.Y, rectTip.Width, rectTip.Height);
                    //rectTip = new Rectangle(rectTip.X + 1, rectTip.Y + 1, rectTip.Width, rectTip.Height);
                    rect.Inflate(3, 3);

                    GraphicsPath pTip = RoundedRectangle(_x, _y, rectTip, 10, 4);
                    
                    GraphicsPath pTipInflated = RoundedRectangle(_x, _y, rect, 10, 4);
                    
                    GraphicsPath pShadow = RoundedRectangle(_x, _y, rectShadow, 10, 4);

                    Region rTip1 = new Region(pTipInflated);
                    Region rTip = new Region(pTip);
                    Region rShadow = new Region(pShadow);
                    
                    g.FillRegion(shadow, rShadow);
                    g.FillRegion(border, rTip1);
                    g.FillRegion(bg, rTip);

                    
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;

                    g.DrawString(info.LineName, f, text, rectTip.X + 2, rectTip.Y + 8, sf);
                    g.DrawString(info.PlotName, f, text, rectTip.X + 2, rectTip.Y + f.Size * 3, sf);
                    g.DrawString(_xAxis_formatString + Math.Round(info.PlotValue, 2).ToString(), f, text, rectTip.X + 2, rectTip.Y + f.Size * 4 + 5, sf);


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

        public void SaveChartImage()
        {
            //_image = new Bitmap(this.Width, this.Height, _imageGraphics);
            _image.Save(@"C:\Chart.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }


        private GraphicsPath RoundedRectangle(float x, float y, Rectangle rect, int cornerSize, int borderSize)
        {
            GraphicsPath p = new GraphicsPath();

            int i = cornerSize;
            int j = borderSize;

            p.StartFigure();
            //Upper Left Corner
            p.AddArc(new Rectangle((int)(rect.X + -1), (int)(rect.Y + -1), i, i), 180, 90);

            //Top Line
            p.AddLine(rect.X + j, rect.Y + 0, rect.X + rect.Width - j, rect.Y + 0);

            //Upper Right Corner
            p.AddArc(new Rectangle((int)(rect.X + rect.Width - i), (int)(rect.Y + -1), i, i), 270, 90);

            //Right Line
            p.AddLine(rect.X + rect.Width, rect.Y + j, rect.X + rect.Width, rect.Y + rect.Height - j);

            //Lower Right Corner
            p.AddArc(new Rectangle((int)(rect.X + rect.Width - i), (int)(rect.Y + rect.Height - i), i, i), 0, 90);

            //Bottom Line
            p.AddLine(rect.X + j, rect.Y + rect.Height, rect.X + rect.Width - j, rect.Y + rect.Height);

            //Lower Left Corner
            p.AddArc(new Rectangle((int)(rect.X + -1), (int)(rect.Y + rect.Height - i), i, i), 90, 90);

            //Left Line
            p.AddLine(rect.X + 0, rect.Y + j, rect.X + 0, rect.Y + rect.Height - j);

            p.CloseFigure();

            return p;
        }


        protected override void OnResize(EventArgs e)
        {
            try
            {
                base.OnResize(e);

                this.BorderStyle = BorderStyle.None;
                SetChartArea();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                
                
            }
            

        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            _x = 0;
            _y = 0;
            _mouseClicked = false;
            this.Invalidate();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            _x = e.X;
            _y = e.Y;
            //_mouseClicked = false;
            this.Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            _x = e.X;
            _y = e.Y;
            _mouseClicked = true;
            this.Invalidate();
        }

        private void zLineChart_Load(object sender, EventArgs e)
        {

            if (this.DesignMode)
            {
                //_yAxisMinimum = 80;
                //_yAxisMaximum = 100;
                //_yAxisIncrement = 2;

                //_yAxisSecondaryMinimum = 3000;
                //_yAxisSecondaryMaximum = 10000;
                //_yAxisSecondaryIncrement = 2000;

                //_legendOffset = 35;

                //_gridLineColor = Color.DarkGray;

                zLine line = new zLine("Blue");
                line.LineColor = Color.Blue;
                line.LineThickness = 3;
                line.LineType = zLine.LineTypes.Wavy;
                line.PlotColor = Color.Blue;
                line.PlotShape = zLine.PlotShapes.Circle;
                line.PlotSize = 9;

                for (int i = 1; i <= 6; i++)
                {
                    //Random r = new Random();
                    line.AddPlot("Month " + i.ToString(), (decimal)zRandom.RandomNumber(90, 100));
                }

                zLine line1 = new zLine("Green");
                line1.LineColor = Color.Green;
                line1.LineThickness = 3;
                line1.LineType = zLine.LineTypes.Wavy;
                line1.PlotColor = Color.Green;
                line1.PlotShape = zLine.PlotShapes.None;
                line1.PlotSize = 9;

                for (int i = 1; i <= 6; i++)
                {
                    //Random r = new Random();
                    line1.AddPlot("Month " + i.ToString(), (decimal)zRandom.RandomNumber(90, 100));
                }

                zLine line2 = new zLine("Orange");
                line2.LineColor = Color.Orange;
                line2.LineThickness = 3;
                line2.LineType = zLine.LineTypes.Sharp;
                line2.PlotColor = Color.Orange;
                line2.PlotShape = zLine.PlotShapes.None;
                line2.PlotSize = 9;

                for (int i = 1; i <= 6; i++)
                {
                    //Random r = new Random();
                    line2.AddPlot("Month " + i.ToString(), (decimal)zRandom.RandomNumber(90, 100));
                }


                AddLine(line);
                AddLine(line1);
                AddLine(line2);

                AddStandardDeviationLine(91, Color.Red, DashStyle.Dash, 1);
                AddStandardDeviationLine(97, Color.Red, DashStyle.Dash, 1);

                //_showSecondary_Y_Axis = true;
            }

            
            


        }


    #endregion

    #region "Events"

        public delegate void Plot_MouseEventHandler(object sender, PlotMouseEventArgs e);
        public event Plot_MouseEventHandler Plot_MouseMove;
        public event Plot_MouseEventHandler Plot_MouseClick;


        public delegate void Legend_MouseEventHandler(object sender, LegendMouseEventArgs e);
        public event Legend_MouseEventHandler Legend_MouseMove;
        public event Legend_MouseEventHandler Legend_MouseClick;


    #endregion       

    #region "Hidden"

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void AdjustFormScrollbars(bool displayScrollbars)
        {
            base.AdjustFormScrollbars(displayScrollbars);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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


        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override bool CanEnableIme
        {
            get
            {
                return base.CanEnableIme;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override bool CanRaiseEvents
        {
            get
            {
                return base.CanRaiseEvents;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return base.CreateAccessibilityInstance();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override ControlCollection CreateControlsInstance()
        {
            return base.CreateControlsInstance();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void CreateHandle()
        {
            base.CreateHandle();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override CreateParams CreateParams
        {
            get
            {
                return base.CreateParams;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override ImeMode DefaultImeMode
        {
            get
            {
                return base.DefaultImeMode;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override Padding DefaultMargin
        {
            get
            {
                return base.DefaultMargin;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override Size DefaultMaximumSize
        {
            get
            {
                return base.DefaultMaximumSize;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override Size DefaultMinimumSize
        {
            get
            {
                return base.DefaultMinimumSize;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override Padding DefaultPadding
        {
            get
            {
                return base.DefaultPadding;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override Size DefaultSize
        {
            get
            {
                return base.DefaultSize;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void DestroyHandle()
        {
            base.DestroyHandle();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                return base.DisplayRectangle;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Focused
        {
            get
            {
                return base.Focused;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override AccessibleObject GetAccessibilityObjectById(int objectId)
        {
            return base.GetAccessibilityObjectById(objectId);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override Size GetPreferredSize(Size proposedSize)
        {
            return base.GetPreferredSize(proposedSize);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
        {
            return base.GetScaledBounds(bounds, factor, specified);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override object GetService(Type service)
        {
            return base.GetService(service);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void InitLayout()
        {
            base.InitLayout();
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override bool IsInputChar(char charCode)
        {
            return base.IsInputChar(charCode);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override bool IsInputKey(Keys keyData)
        {
            return base.IsInputKey(keyData);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                return base.LayoutEngine;
            }
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
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

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void NotifyInvalidate(Rectangle invalidatedArea)
        {
            base.NotifyInvalidate(invalidatedArea);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnAutoSizeChanged(EventArgs e)
        {
            base.OnAutoSizeChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnAutoValidateChanged(EventArgs e)
        {
            base.OnAutoValidateChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnBackgroundImageLayoutChanged(EventArgs e)
        {
            base.OnBackgroundImageLayoutChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnCausesValidationChanged(EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnChangeUICues(UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

       

    #endregion

    }

    class zLinePlotInfo : IComparable
    {

        string _plotName = "";

        public string PlotName
        {
            get { return _plotName; }
        }

        public zLinePlotInfo(string plotName)
        {
            _plotName = plotName;
        }

        public override bool Equals(object obj)
        {
            return _plotName.Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return _plotName.GetHashCode();
        }

        public override string ToString()
        {
            return _plotName.ToString();
        }


        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {

            return _plotName.CompareTo(obj.ToString());
            
        }

        #endregion
    }

    class zLineTipInfo
    {
        string _lineName;
        string _plotName;
        decimal _plotValue;
        Color _lineColor;
        Color _plotColor;
        zLine.PlotShapes _plotShape;


        public string LineName
        {
            get { return _lineName; }
        }

        public string PlotName
        {
            get { return _plotName; }
        }

        public decimal PlotValue
        {
            get { return _plotValue; }
        }

        public Color LineColor
        {
            get { return _lineColor; }
        }

        public Color PlotColor
        {
            get { return _plotColor; }
        }

        public zLine.PlotShapes PlotShape
        {
            get { return _plotShape; }
        }


        public zLineTipInfo(string lineName, string plotName, decimal plotValue, Color lineColor, Color plotColor, zLine.PlotShapes plotShape)
        {
            _lineName = lineName;
            _plotName = plotName;
            _plotValue = plotValue;
            _lineColor = lineColor;
            _plotColor = plotColor;
            _plotShape = plotShape;
        }


    }

    public class PlotMouseEventArgs : EventArgs
    {
        string _LineName = "";
        string _PlotName = "";
        decimal _PlotValue;

        Color _LineColor;
        Color _PlotColor;
        zLine.PlotShapes _PlotShape;

        public string LineName
        {
            get { return _LineName; }
        }

        public string PlotName
        {
            get { return _PlotName; }
        }

        public decimal PlotValue
        {
            get { return _PlotValue; }
        }

        public Color LineColor
        {
            get { return _LineColor; }
        }

        public Color PlotColor
        {
            get { return _PlotColor; }
        }

        public zLine.PlotShapes PlotShape
        {
            get{ return _PlotShape; }
        }

        public PlotMouseEventArgs(string lineName, string plotName, decimal plotValue, Color lineColor, Color plotColor, zLine.PlotShapes plotShape)
        {
            _LineName = lineName;
            _PlotName = plotName;
            _PlotValue = plotValue;
            _LineColor = lineColor;
            _PlotColor = plotColor;
            _PlotShape = plotShape;
        }

    }

    public class LegendMouseEventArgs : EventArgs
    {
        string _LineName = "";
        Color _LineColor;
        Color _PlotColor;
        zLine.PlotShapes _PlotShape;


        public string LineName
        {
            get { return _LineName; }
        }

        public Color LineColor
        {
            get{ return _LineColor; }
        }

        public Color PlotColor
        {
            get { return _PlotColor; }
        }

        public zLine.PlotShapes PlotShape
        {
            get { return _PlotShape; }
        }


        public LegendMouseEventArgs(string lineName, Color lineColor, Color plotColor, zLine.PlotShapes plotShape)
        {
            _LineName = lineName;
            _LineColor = lineColor;
            _PlotColor = plotColor;
            _PlotShape = plotShape;
        }

    }

}
