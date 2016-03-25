using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using zControlsC;
using System.Runtime.Serialization;
using zControlsC;

namespace zGIS.NorthAmerica
{
    [Serializable()]
    public class StateBorder
    {

    #region "Initialize"

        public StateBorder(string stateName)
        {
            state = stateName;
            //TODO: Get rid of random value
            //Random r = new Random();
            Value = zRandom.RandomNumber(1, 100);
        }

        public StateBorder(string stateName, List<List<PointF>> points) : this(stateName)
        {
            geoBorders = points;
        }

    #endregion

    #region "Declarations"


        //List<PointF> geoBorder = new List<PointF>();
        string state;

        List<List<PointF>> geoBorders = new List<List<PointF>>();

        List<List<PointF>> _projectedBorders = new List<List<PointF>>();

        List<PointF> centerPoints = new List<PointF>();
        public Color StateColor = Color.White;
        public Color StateColorMouseOver = Color.White;
        public byte StateColorOpacity = 255;
        public byte StateColorMouseOverOpacity = 255;
        public float Value;

        [OptionalField]
        public Regions Region;

        public string Text = "";


    #endregion

    #region "Properties"

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public List<List<PointF>> GeoBorders
        {
            get { return geoBorders; }
        }


        public List<PointF> CenterPoints
        {
            get { return centerPoints; }
        }


        public enum Regions
        {
            Midwest,
            Northeast,
            South,
            West
        }

        

    #endregion

    #region "Public Methods"

        

        public void AddPoints(List<PointF> points)
        {
            geoBorders.Add(points);
        }

        public List<List<PointF>> ProjectBorder()
        {

            if (_projectedBorders == null)
            {
                _projectedBorders = new List<List<PointF>>();

                foreach (List<PointF> pointList in geoBorders)
                {

                    List<PointF> points = new List<PointF>();
                    foreach (PointF point in pointList)
                    {
                        PointF p = zConversion.Project(point);
                        points.Add(p);

                    }

                    _projectedBorders.Add(points);

                }
            }
            

            return _projectedBorders;
        }


        public override bool Equals(object obj)
        {
            return state.Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return state.GetHashCode();
        }

        public override string ToString()
        {
            return state; 
        }


    #endregion

    #region "Private Methods"


    #endregion


    }
}
