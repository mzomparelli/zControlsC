using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Timers;

namespace zControlsC.Charts
{
    public class zLine : IComparable
    {

    #region "Initializer"

        public zLine(string lineName)
        {
            _name = lineName;
            tThrobbingLine.Elapsed += new ElapsedEventHandler(tThrobbingLine_Elapsed);
            tDropLine.Elapsed +=new ElapsedEventHandler(tDropLine_Elapsed);
        }       

       

    #endregion

    #region "Declarations"

        string _name = "";
        PlotShapes _plotShape = PlotShapes.None;
        Color _plotColor = Color.Black;
        float _plotSize = 6;

        Color _lineColor = Color.Black;
        float _lineThickness = 1;
        LineTypes _lineType = LineTypes.Sharp;
        DashStyle _lineDashStyle = DashStyle.Solid;


        //Used for animating
        float _lineThicknessA = 1;
        bool _up = false;
        bool _IsAnimating = false;



        ArrayList _Plots = new ArrayList();

        Timer tThrobbingLine = new Timer(100);
        Timer tDropLine = new Timer(5);
        


    #endregion

    #region "Properties"

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PlotShapes PlotShape
        {
            get { return _plotShape; }
            set { _plotShape = value; }
        }

        public Color PlotColor
        {
            get { return _plotColor; }
            set { _plotColor = value; }
        }

        public float PlotSize
        {
            get { return _plotSize; }
            set { _plotSize = value; }
        }

        public Color LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        public float LineThickness
        {
            get { return _lineThickness; }
            set
            {
                _lineThickness = value;
                _lineThicknessA = value;
            }
        }

        public LineTypes LineType
        {
            get { return _lineType; }
            set { _lineType = value; }
        }

        public DashStyle LineDashStyle
        {
            get { return _lineDashStyle; }
            set { _lineDashStyle = value; }
        }

        public int PlotCount
        {
            get { return _Plots.Count; }
        }

        public ArrayList Plots
        {
            get { return _Plots; }
        }

        public bool IsAnimating
        {
            get { return _IsAnimating; }
        }


    #endregion

    #region "Public Methods"

        public void AddPlot(string plotName, decimal plotValue)
        {
            Plot p = new Plot(plotName, plotValue);
            AddPlot(p);
        }

        public void AddPlot(Plot plot)
        {
            _Plots.Add(plot);
        }

        public void AddPlot(string plotName)
        {
            Plot p = new Plot(plotName);
            AddPlot(p);
        }

        public void InsertPlotAt(string plotName, decimal plotValue, int index)
        {
            Plot plot = new Plot(plotName, plotValue);
            InsertPlotAt(plot, index);
        }

        public void InsertPlotAt(string plotName, int index)
        {
            Plot plot = new Plot(plotName);
            InsertPlotAt(plot, index);
        }

        public void InsertPlotAt(zLine.Plot plot, int index)
        {
            if (index > PlotCount - 1)
            {
                AddPlot(plot);
            }
            else
            {
                _Plots.Insert(index, plot);
            }

            
        }

        public void MovePlot(int currentIndex, int newIndex)
        {

            Plot oldP = (Plot)Plots[currentIndex];
            Plot p = new Plot(oldP.Name, oldP.Value);
            Plots.RemoveAt(currentIndex);
            InsertPlotAt(p, newIndex);
            
            
        }

        public void ClearPlots()
        {
            _Plots.Clear();
        }

        public void ReversePlots()
        {
            _Plots.Reverse();
        }

        public void Sort()
        {
            Plots.Sort();
        }


    #endregion

    #region "Private Methods"


    #endregion

    #region "Animation"

    #region "Drop Line"

        void tDropLine_Elapsed(object sender, ElapsedEventArgs e)
        {

            //Alter the plot values and let the chart know it changed so it can redraw it
            //tAnimate.Interval = 100;

            if (_up)
            {
                tThrobbingLine.Interval = 100;
            }
            else
            {
                tThrobbingLine.Interval = 50;
            }

            if (_lineThickness == _lineThicknessA + 3)
            {
                _up = false;
                _lineThickness = _lineThicknessA + 2;
            }
            else if (_lineThickness == _lineThicknessA + 2)
            {
                if (_up)
                {
                    _lineThickness = _lineThicknessA + 3;
                }
                else
                {
                    _lineThickness = _lineThicknessA + 1;
                }
            }
            else if (_lineThickness == _lineThicknessA + 1)
            {
                if (_up)
                {
                    _lineThickness = _lineThicknessA + 2;
                }
                else
                {
                    _lineThickness = _lineThicknessA;
                }
            }
            else if (_lineThickness == _lineThicknessA)
            {
                _up = true;
                _lineThickness = _lineThicknessA + 1;
            }


            if (LineAnimating != null)
            {
                LineAnimating(this, new LineAnimatingEventArgs());
            }
        }

        internal void DropLineStart()
        {
            _lineThicknessA = _lineThickness;
            _up = false;
            tDropLine.Start();
            LineAnimating(this, new LineAnimatingEventArgs());
            _IsAnimating = true;
        }

        internal void DropLineStop()
        {
            tThrobbingLine.Stop();
            _lineThickness = _lineThicknessA;
            LineAnimating(this, new LineAnimatingEventArgs());
            _IsAnimating = false;
        }


    #endregion

    #region "Throbbing Line"

        void tThrobbingLine_Elapsed(object sender, ElapsedEventArgs e)
        {

            //Alter the plot values and let the chart know it changed so it can redraw it
            //tAnimate.Interval = 100;

            if (_up)
            {
                tThrobbingLine.Interval = 200;
            }
            else
            {
                tThrobbingLine.Interval = 200;
            }

            if (_lineThickness == _lineThicknessA + 3)
            {
                _up = false;
                _lineThickness = _lineThicknessA + 2;
            }
            else if (_lineThickness == _lineThicknessA + 2)
            {
                if (_up)
                {
                    _lineThickness = _lineThicknessA + 3;
                }
                else
                {
                    _lineThickness = _lineThicknessA + 1;
                }
            }
            else if (_lineThickness == _lineThicknessA + 1)
            {
                if (_up)
                {
                    _lineThickness = _lineThicknessA + 2;
                }
                else
                {
                    _lineThickness = _lineThicknessA;
                }
            }
            else if (_lineThickness == _lineThicknessA)
            {
                _up = true;
                _lineThickness = _lineThicknessA + 1;
            }


            if (LineAnimating != null)
            {
                LineAnimating(this, new LineAnimatingEventArgs());
            }
        }

        internal void ThrobbingLineStart()
        {
            _lineThicknessA = _lineThickness;
            _up = false;
            tThrobbingLine.Start();
            //LineAnimating(this, new LineAnimatingEventArgs());
            _IsAnimating = true;
        }

        internal void ThrobbingLineStop()
        {
            tThrobbingLine.Stop();
            _lineThickness = _lineThicknessA;
            //LineAnimating(this, new LineAnimatingEventArgs());
            _IsAnimating = false;
        }


    #endregion

    #endregion

    #region "Events"

        //The line chart must subscribe to this event in order for the animation to be visible to the user.
        //This event tells the line chart that this line is animating so the line chart can re-paint everything.
        //The subscribing of this event takes place during zLineChart.AddLine(params zLine[] line)
        public delegate void LineAnimatingHandler(object sender, LineAnimatingEventArgs e);
        public event LineAnimatingHandler LineAnimating;



    #endregion

    #region "Enums"

        public enum PlotShapes
        {
            None,
            Square,
            Circle,
            Triangle,
            Diamond,
            Cross,
            Dot
        }

        public enum LineTypes
        {
            Sharp,
            Wavy
        }




    #endregion

    #region "structs"

        public struct Plot : IComparable
        {

            public string Name;
            public decimal Value;
            public bool IsNullPlot;

            public Plot(string plotName, decimal plotValue)
            {
                Name = plotName;
                Value = plotValue;
                IsNullPlot = false;
            }

            public Plot(string plotName)
            {
                Name = plotName;
                Value = 0.0M;
                IsNullPlot = true;
            }

            public override bool Equals(object obj)
            {
                return Name.Equals(obj.ToString());
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public override string ToString()
            {
                return Name.ToString();
            }



            #region IComparable Members

            int IComparable.CompareTo(object obj)
            {
                return Name.CompareTo(obj.ToString());
            }

            #endregion
        
        }

    #endregion

    #region "Overrides"

        public override bool Equals(object obj)
        {
            return _name.Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public override string ToString()
        {
            return _name;
        }


    #endregion



        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            return _name.CompareTo(obj.ToString());

        }

    #endregion

    }


    public class LineAnimatingEventArgs : EventArgs
    {

        public LineAnimatingEventArgs()
        {

        }



    }


}
