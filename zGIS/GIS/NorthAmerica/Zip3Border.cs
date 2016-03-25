using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace zGIS.NorthAmerica
{
    [Serializable()]
    public class Zip3Border : IDeserializationCallback
    {

        #region "Initialize"

        public Zip3Border(string zip3)
        {
            _Zip3 = zip3;
        }

        public Zip3Border(string zip3, List<List<PointF>> points)
        {
            _Zip3 = zip3;
            geoBorders = points;
        }

        #endregion

        #region "Declarations"

        //List<PointF> geoBorder = new List<PointF>();
        string _Zip3;

        List<List<PointF>> geoBorders = new List<List<PointF>>();

        List<List<PointF>> _projectedBorders = new List<List<PointF>>();


        List<PointF> centerPoints = new List<PointF>();

        public Color ZipColor = Color.White;
        public float Value;



        #endregion

        #region "Properties"

        public string Zip3
        {
            get { return _Zip3; }
        }

        public List<List<PointF>> GeoBorders
        {
            get { return geoBorders; }
        }

        public List<PointF> CenterPoints
        {
            get { return centerPoints; }
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
            return _Zip3.Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return _Zip3.GetHashCode();
        }

        public override string ToString()
        {
            return _Zip3; ;
        }


        #endregion

        #region "Private Methods"


        #endregion

        #region "Protected"

        

        #endregion



        #region IDeserializationCallback Members

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            //foreach (List<PointF> poly in geoBorders)
            //{
            //    int i = poly.Count - 1;

            //    //MessageBox.Show(poly[0].X.ToString() + "\n" + poly[i].X.ToString());
                
            //    if ((poly[0].X != poly[i].X) && (poly[0].X != poly[i].X))
            //    {
            //        PointF p = new PointF(poly[0].X, poly[0].Y);
            //        poly.Add(p);
            //    }

            //    //MessageBox.Show(poly[0].X.ToString() + "\n" + poly[i + 1].X.ToString());
            //}
        }

        #endregion
    }
}
