using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using zControlsC;
using System.Windows.Forms;

namespace zGIS
{
    [Serializable()]
    public class zMapChart_P2P_Line
    {

        private string Name = "";

        public string Value = "";
        public string Text = "";
        private PointF _PointA;
        private PointF _PointB;
        public Color Color = Color.Black;
        public bool DisplayValues = false;
        public DisplayValuePlacement ValuePlacement = DisplayValuePlacement.LineCenter;
        public DisplayValueStyle ValueStyle = DisplayValueStyle.ColoredCircle;
        public Color ValueStyleColor = Color.Green;
        public Color ValueStyleFillColor = Color.Green;
        public float Width = 1;
        public Color FontColor = Color.Black;
        public int FontSize = 10;
        public FontStyle FontStyle = FontStyle.Regular;
        private int animationSpeed = 5;
        private AnimationSpeeds animationSpeedType = AnimationSpeeds.Points;

        private bool animateOnce = false;


        System.Timers.Timer animation = new System.Timers.Timer(100);
        DisplayValuePlacementAnimationDirection animationDirection = DisplayValuePlacementAnimationDirection.PointA_PointB;
        int animationPointIndex = 0;
        PointF[] linePoints;


        public AnimationSpeeds AnimationSpeedType
        {
            get { return animationSpeedType; }
            set
            {
                animationSpeedType = value;
                GetAllPoints();
            }
        }

        public int AnimationSpeed
        {
            get { return animationSpeed; }
            set
            {
                animationSpeed = value;
                GetAllPoints();
            }
        }

        public PointF PointA
        {
            get { return _PointA; }
            set
            {
                _PointA = value;
                if (HasPoints)
                {
                    GetAllPoints();
                }
            }
        }

        public PointF PointB
        {
            get { return _PointB; }
            set
            {
                _PointB = value;
                if (HasPoints)
                {
                    GetAllPoints();
                }
            }
        }

        public bool HasPoints
        {
            get
            {
                if (_PointA != null && _PointB != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public enum AnimationSpeeds
        {
            ConstantSpeed,
            Points,
        }

        public enum DisplayValuePlacementAnimationDirection
        {
            PointA_PointB,
            PointB_PointA
        }

        public enum DisplayValuePlacement
        {
            PointA,
            PointB,
            LineCenter,
            Animate
        }

        public enum DisplayValueStyle
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
            Icon
        }

        public zMapChart_P2P_Line(string name, PointF pointA, PointF pointB)
        {

            Name = name;
            _PointA = pointA;
            _PointB = pointB;
            Initialize();
        }

        

        public zMapChart_P2P_Line(string name)
        {

            Name = name;
            Initialize();
        }

        public zMapChart_P2P_Line(string name, PointF pointA, PointF pointB, Color lineColor)
        {

            Name = name;
            _PointA = pointA;
            _PointB = pointB;
            Color = lineColor;
            Initialize();
        }

        public zMapChart_P2P_Line(string name, PointF pointA, PointF pointB, Color lineColor, string text)
        {

            Name = name;
            _PointA = pointA;
            _PointB = pointB;
            Color = lineColor;
            Text = text;
            Initialize();
        }

        public zMapChart_P2P_Line(string name, PointF pointA, PointF pointB, Color lineColor, string text, string value)
        {

            Name = name;
            _PointA = pointA;
            _PointB = pointB;
            Color = lineColor;
            Text = text;
            Value = value;
            Initialize();
        }

        private void Initialize()
        {
            animation.Elapsed += new System.Timers.ElapsedEventHandler(animate_Elapsed);
        }

        private void GetAllPoints()
        {
            switch (animationSpeedType)
            {
                case AnimationSpeeds.ConstantSpeed:
                    double len = zFunctions.LineLength(PointA, PointB);
                    len = len * 10;
                    linePoints = zFunctions.PointsAlongLine(_PointA, _PointB, (int)(len / AnimationSpeed));
                    break;
                case AnimationSpeeds.Points:
                    linePoints = zFunctions.PointsAlongLine(_PointA, _PointB, AnimationSpeed);
                    break;
                default:
                    linePoints = zFunctions.PointsAlongLine(_PointA, _PointB, AnimationSpeed);
                    break;
            }

             
        }

        void animate_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ValuePlacement != DisplayValuePlacement.Animate)
            {
                return;
            }
            
            switch (animationDirection)
            {
                case DisplayValuePlacementAnimationDirection.PointA_PointB:
                    animationPointIndex++;
                    if (animationPointIndex >= linePoints.GetLength(0))
                    {
                        if (animateOnce)
                        {
                            animationPointIndex = linePoints.GetLength(0) - 1;
                            animation.Stop();
                        }
                        else
                        {
                            animationPointIndex = 0;
                        }
                        
                    }
                    break;
                case DisplayValuePlacementAnimationDirection.PointB_PointA:
                    animationPointIndex--;
                    if (animationPointIndex <= 0)
                    {
                        if (animateOnce)
                        {
                            animationPointIndex = 0;
                            animation.Stop();
                        }
                        else
                        {
                            animationPointIndex = linePoints.GetLength(0) - 1;
                        }
                        
                    }
                    break;
                default:
                    break;
            }

        }

        public double Lengh()
        {
            return zFunctions.LineLength(PointA, PointB);
        }

        public void Animate(DisplayValuePlacementAnimationDirection direction)
        {
            ValuePlacement = DisplayValuePlacement.Animate;
            DisplayValues = true;
            animationDirection = direction;
            switch (animationDirection)
            {
                case DisplayValuePlacementAnimationDirection.PointA_PointB:
                    animationPointIndex = 0;
                    break;
                case DisplayValuePlacementAnimationDirection.PointB_PointA:
                    animationPointIndex = linePoints.GetLength(0) - 1;
                    break;
                default:
                    break;
            }
            animateOnce = false;
            animation.Start();
        }

        public void AnimateOnce(DisplayValuePlacementAnimationDirection direction)
        {
            ValuePlacement = DisplayValuePlacement.Animate;
            DisplayValues = true;
            animationDirection = direction;
            switch (animationDirection)
            {
                case DisplayValuePlacementAnimationDirection.PointA_PointB:
                    animationPointIndex = 0;
                    break;
                case DisplayValuePlacementAnimationDirection.PointB_PointA:
                    animationPointIndex = linePoints.GetLength(0) - 1;
                    break;
                default:
                    break;
            }
            animateOnce = true;
            animation.Start();
        }

        public PointF AnimationPoint()
        {
            try
            {
                if (animateOnce)
                {
                    if (animation.Enabled)
                    {
                        return linePoints[animationPointIndex];
                    }
                    else
                    {
                        switch (animationDirection)
                        {
                            case DisplayValuePlacementAnimationDirection.PointA_PointB:
                                return PointB;
                            case DisplayValuePlacementAnimationDirection.PointB_PointA:
                                return PointA;
                            default:
                                return PointA;
                        }
                    }
                }
                else
                {
                    return linePoints[animationPointIndex];
                }
                
            }
            catch (Exception)
            {
                return new PointF(0, 0);
                //throw;
            }
            
        }

        public PointF Center()
        {
            return zFunctions.LineCenterPoint(_PointA, _PointB);            
        }

        


    }
}
