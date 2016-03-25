using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zControlsC.UI_Controls
{
    //[ToolboxBitmap(@"Z:\Resource\ICO\16x16 ico\money.ico")]
    [ToolboxBitmap(typeof(Label)), DefaultEvent("OnClick")]
    public partial class zLabel : UserControl
    {

        #region "Declarations"

        StringFormat _TextFormat = new StringFormat();
        bool _IsMouseOver = false;
        bool _MouseOverColorChange = false;
        string _TextString = "zLabel";
        int _TextVerticalOffset = 0;
        int _TextHorizontalOffset = 0;

        Color _MouseOverForeColor = Color.Black;
        Font _MouseOverFont = default(Font);

        bool _UseGradientBackColor = true;

        //BackColor
        Color _BackColorLight = Color.White;
        Color _BackColorDark = Color.DarkGray;
        Color _BackColorBorder = Color.DarkGray;
        Color _MouseOverBackColorLight = Color.White;
        Color _MouseOverBackColorDark = Color.DarkGray;
        Color _MouseOverBorderColor = Color.DarkGray;

        Image _Icon = null;
        IconAlignmentEnum _IconAlignment = IconAlignmentEnum.Far;
        int _IconOffsetVertical = 0;
        int _IconOffsetHorizontal = 0;
        IconSizeEnum _IconSize = IconSizeEnum.Size5;

        bool _RoundUpperLeftCorner = true;
        bool _RoundLowerLeftCorner = true;
        bool _RoundUpperRightCorner = true;
        bool _RoundLowerRightCorner = true;
        RoundedCornersEnum _CornerSize = RoundedCornersEnum.Medium;
        int _BorderLineSize = 8;





        #endregion

        #region "Properties"

        #region "BackColor"

        public override Color   BackColor
        {
            get { return _BackColorDark; }
            set {
                _BackColorDark = value;
                this.Refresh();            
                }
        }

        public Color            BackColorLight
        {
            get { return _BackColorLight; }
            set { 
                _BackColorLight = value;
                this.Refresh();
                }
        }

        public Color            BackColorDark
        {
            get { return _BackColorDark; }
            set { 
                _BackColorDark = value;
                this.Refresh();
                }
        }

        public Color            BackColorBorder
        {
            get { return _BackColorBorder; }
            set { 
                _BackColorBorder = value;
                this.Refresh();
                }
        }

        public Color            MouseOverBackColorLight
        {
            get { return _MouseOverBackColorLight; }
            set {
                _MouseOverBackColorLight = value;
                this.Refresh();
                }
        }

        public Color            MouseOverBackColorDark
        {
            get { return _MouseOverBackColorDark; }
            set
            {
                _MouseOverBackColorDark = value;
                this.Refresh();
            }
        }

        public Color            MouseOverBorderColor
        {
            get { return _MouseOverBorderColor; }
            set
            {
                _MouseOverBorderColor = value;
                this.Refresh();
            }

        }



        #endregion

        #region "Rounded Corners"

        public bool RoundUpperLeftCorner
        {
            get { return _RoundUpperLeftCorner; }
            set
            {
                _RoundUpperLeftCorner = value;
                this.Refresh();
            }
        }

        public bool RoundLowerLeftCorner
        {
            get { return _RoundLowerLeftCorner; }
            set
            {
                _RoundLowerLeftCorner = value;
                this.Refresh();
            }
        }

        public bool RoundUpperRightCorner
        {
            get { return _RoundUpperRightCorner; }
            set
            {
                _RoundUpperRightCorner = value;
                this.Refresh();
            }
        }

        public bool RoundLowerRightCorner
        {
            get { return _RoundLowerRightCorner; }
            set { 
                _RoundLowerRightCorner = value;
                this.Refresh();
                }
        }

        public enum RoundedCornersEnum
        {
            None = 1,
            Small = 10,
            Medium = 20,
            Large = 30,
            ExtraLarge = 40
        }

        public RoundedCornersEnum CornerSize
        {
            get { return _CornerSize; }
            set { 
                _CornerSize = value;
                switch (_CornerSize)
                {
                    case RoundedCornersEnum.None:
                        _BorderLineSize = 0;
                        break;
                    case RoundedCornersEnum.Small:
                        _BorderLineSize = 4;
                        break;
                    case RoundedCornersEnum.Medium:
                        _BorderLineSize = 8;
                        break;
                    case RoundedCornersEnum.Large:
                        _BorderLineSize = 13;
                        break;
                    case RoundedCornersEnum.ExtraLarge:
                        _BorderLineSize = 17;
                        break;
                    default:
                        break;
                }
                OnSizeChanged(new EventArgs());
                this.Refresh();
                }
        }
        

        #endregion

        

        public enum IconAlignmentEnum
        {
            Near,
            Center,
            Far
        }

        public enum IconSizeEnum
        {
            Size1 = 24,
            Size2 = 23,
            Size3 = 22,
            Size4 = 21,
            Size5 = 20,
            Size6 = 19,
            Size7 = 18,
            Size8 = 17,
            Size9 = 16,
            Size10 = 15,
            Size11 = 14,
            Size12 = 13,
            Size13 = 12,
            Size14 = 11,
            Size15 = 10,
            Size16 = 9,
            Size17 = 8,
            Size18 = 7,
            Size19 = 6,
            Size20 = 5,
            Size21 = 4,
            Size22 = 3,
            Size23 = 2,
            Size24 = 1
        }

        public int IconOffsetVertical
        {
            get { return _IconOffsetVertical; }
            set { 
                _IconOffsetVertical = value;
                this.Refresh();
                }
        }

        public int IconOffsetHorizontal
        {
            get { return _IconOffsetHorizontal; }
            set { 
                _IconOffsetHorizontal = value;
                this.Refresh();
                }
        }

        public IconSizeEnum IconSize
        {
            get { return _IconSize; }
            set { 
                _IconSize = value;
                this.Refresh();
                }
        }

        public IconAlignmentEnum IconAlignment
        {
            get { return _IconAlignment; }
            set { 
                _IconAlignment = value;
                this.Refresh();
                }
        }

        [DefaultValue(null)]
        public Image Icon
        {
            get { return _Icon; }
            set {
                _Icon = value;
                this.Refresh();
                }
        }

        public bool UseGradientBackColor
        {
            get { return _UseGradientBackColor; }
            set {
                _UseGradientBackColor = value;
                this.Refresh();
                }
        }

        public int TextOffsetVertical
        {
            get { return _TextVerticalOffset; }
            set { 
                _TextVerticalOffset = value;
                this.Refresh();
                }
        }

        public int TextOffsetHorizontal
        {
            get { return _TextHorizontalOffset; }
            set { 
                _TextHorizontalOffset = value;
                this.Refresh();
                }
        }

        public string TextString
        {
            get { return _TextString; }
            set { 
                _TextString = value;
                this.Refresh();
                }
        }

        public StringAlignment TextAlign
        {
            get { return _TextFormat.Alignment; }
            set { 
                _TextFormat.Alignment = value;
                this.Refresh();
                }
        }

        public StringFormatFlags TextOrientation
        {
            get { return _TextFormat.FormatFlags; }
            set { 
                _TextFormat.FormatFlags = value;
                this.Refresh();
                }
        }




        public bool IsMouseOver
        {
            get { return _IsMouseOver; }
        }

        public bool MouseOverColorChange
        {
            get { return _MouseOverColorChange; }
            set { _MouseOverColorChange = value; }
        }

        public Color MouseOverForeColor
        {
            get { return _MouseOverForeColor; }
            set
            {
                _MouseOverForeColor = value;
                this.Refresh();
            }
        }

        public Font MouseOverFont
        {
            get { return _MouseOverFont; }
            set { 
                _MouseOverFont = value;
                this.Refresh();
                }
        }

        #endregion

        #region "Public Methods"

        //Destructor
        ~zLabel()
        {

        }

        public zLabel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion

        #region "Private Methods"

        private void CreateRegion()
        {
            GraphicsPath p = new GraphicsPath();

            int i = (int)_CornerSize;
            int j = _BorderLineSize;

            p.StartFigure();
            //Upper Left Corner
            p.AddArc(new Rectangle(-1, -1, i, i), 180, 90);

            //Top Line
            p.AddLine(j, 0, this.Width - j, 0);

            //Upper Right Corner
            p.AddArc(new Rectangle(this.Width - i, -1, i, i), 270, 90);

            //Right Line
            p.AddLine(this.Width, j, this.Width, this.Height - j);

            //Lower Right Corner
            p.AddArc(new Rectangle(this.Width - i, this.Height - i, i, i), 0, 90);

            //Bottom Line
            p.AddLine(j, this.Height, this.Width - j, this.Height);

            //Lower Left Corner
            p.AddArc(new Rectangle(-1, this.Height - i, i, i), 90, 90);

            //Left Line
            p.AddLine(0, j, 0, this.Height - j);

            p.CloseFigure();
            this.Region = new Region(p);
        }

        private void zLabel_Load(object sender, EventArgs e)
        {
            CreateRegion();
            this.Refresh();
        }

       
        #endregion

        #region "Protected Methods"

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Don't let Microsoft paint the label
            //base.OnPaintBackground(e);
            
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            //Don't let Microsoft paint the label
            //base.OnPaint(e);

            //This is the main Graphics we'll use to paint on
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //Paint the entire background transparent
            // we do this so we can round the corners
            g.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle((int)(e.ClipRectangle.X), (int)(e.ClipRectangle.Y), (int)(e.ClipRectangle.X + this.Width), (int)(e.ClipRectangle.Y + this.Height)));
            

            //The rectangle for the entire control
            int w   = (int)this.Width - 1   == 0    ? 1 : (int)this.Width - 1;
            int h   = (int)this.Height - 1  == 0    ? 1 : (int)this.Height + 6;
            int hh  = (int)this.Height - 1  == 0    ? 1 : (int)this.Height - 1;

            g.Clip.MakeInfinite();

            Rectangle rectLabel     = new Rectangle(0, 0, w, h);
            Rectangle rectBorder    = new Rectangle(0, 0, w, hh);

            //Create the pens, brushes, and fonts
            Color clrLight  = _UseGradientBackColor == true ? _BackColorLight : _BackColorDark;
            Brush brushBG   = new LinearGradientBrush(rectLabel, clrLight, _BackColorDark, LinearGradientMode.Vertical);
            Pen penBorder   = new Pen(_BackColorBorder);
            Brush brushText = new SolidBrush(this.ForeColor);
            Font fontText   = new Font(this.Font.FontFamily, this.Font.Size, this.Font.Style, GraphicsUnit.Pixel);

            //if mouse is over and the developer wants to use mouseover colors then set the pens and brushes to the mouseover colors
            if (this.Enabled)
            {
                if (_IsMouseOver)
                {
                    if (_MouseOverColorChange)
                    {
                        clrLight = _UseGradientBackColor == true ? _MouseOverBackColorLight : _MouseOverBackColorDark;
                        brushBG = new LinearGradientBrush(rectLabel, clrLight, _MouseOverBackColorDark, LinearGradientMode.Vertical);
                        penBorder = new Pen(_MouseOverBorderColor);
                        brushText = new SolidBrush(_MouseOverForeColor);

                        if (_MouseOverFont != null)
                        {
                            fontText = new Font(_MouseOverFont.FontFamily, _MouseOverFont.Size, this.Font.Style, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
            //fill the background and draw the border
            g.FillRegion(brushBG, this.Region);

            int i = (int)_CornerSize;
            int j = _BorderLineSize;

            //Draw the border using arcs for the rounded corners
            //Upper Left Corner
            g.DrawArc(penBorder, new Rectangle(0, 0, i, i), 180, 90);

            //Top Line
            g.DrawLine(penBorder, j, 0, this.Width - j, 0);

            //Upper Right Corner
            g.DrawArc(penBorder, new Rectangle(this.Width - i, 0, i, i), 270, 90);

            //Right Line
            g.DrawLine(penBorder, this.Width - 1, j, this.Width - 1, this.Height - j);

            //Lower Right Corner
            g.DrawArc(penBorder, new Rectangle(this.Width - i - 1, this.Height - i - 1, i, i), 0, 90);

            //Bottom Line
            g.DrawLine(penBorder, j, this.Height - 1, this.Width - j, this.Height - 1);

            //Lower Left Corner
            g.DrawArc(penBorder, new Rectangle(0, this.Height - i, i, i), 90, 90);

            //Left Line
            g.DrawLine(penBorder, 0, j, 0, this.Height - j);
            

            //Draw the text

                        
            if (!Enabled)
            {
                brushText = new SolidBrush(Color.Gray);
            }
            if (_TextFormat.Alignment == StringAlignment.Center)
            {
                g.DrawString(_TextString, fontText, brushText, 0 + (this.Width / 2) + _TextHorizontalOffset, 0 + (this.Height / 2) - (fontText.Size / 2) + _TextVerticalOffset, _TextFormat);
            }
            else if (_TextFormat.Alignment == StringAlignment.Far)
            {
                g.DrawString(_TextString, fontText, brushText, 0 + g.ClipBounds.Width - 2 + _TextHorizontalOffset, 0 + (this.Height / 2) - (fontText.Size / 2) + _TextVerticalOffset, _TextFormat);
            }
            else if (_TextFormat.Alignment == StringAlignment.Near)
            {
                g.DrawString(_TextString, fontText, brushText, 0 + 2 + _TextHorizontalOffset, 0 + (this.Height / 2) - (fontText.Size / 2) + _TextVerticalOffset, _TextFormat);
            }


            //Draw the icon if there is one
            if (_Icon != null)
            {
                int margin = (int)_IconSize;
                int iconSize = this.Height - margin * 2;
                Rectangle r = default(Rectangle) ;

                System.Drawing.Image icon = _Icon.GetThumbnailImage(iconSize, iconSize, null, IntPtr.Zero);

                if (!this.Enabled)
                {
                    ControlPaint.DrawImageDisabled(g, Icon, (int)(0 + margin + _IconOffsetHorizontal), margin + _IconOffsetVertical, Color.Transparent);
                    return;
                }

                switch (_IconAlignment)
                {
                    case IconAlignmentEnum.Center:
                        r = new Rectangle((int)(0 + (this.Width / 2) - (iconSize / 2) + _IconOffsetHorizontal), margin + _IconOffsetVertical, iconSize, iconSize);
                        break;

                    case IconAlignmentEnum.Far:
                        r = new Rectangle((int)(0 + this.Width - margin - iconSize - _IconOffsetHorizontal), margin + _IconOffsetVertical, iconSize, iconSize);
                        break;

                    case IconAlignmentEnum.Near:
                        r = new Rectangle((int)(0 + margin + _IconOffsetHorizontal), margin + _IconOffsetVertical, iconSize, iconSize);
                        break;

                    default:
                        break;
                }

                g.DrawImage(_Icon, r);

            }

           

        }

        
        
        protected override void OnMouseEnter(EventArgs e)
        {
            _IsMouseOver = true;
            base.OnMouseEnter(e);
            this.Refresh();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _IsMouseOver = false;
            base.OnMouseLeave(e);
            this.Refresh();
        }        

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CreateRegion();
            this.Invalidate(this.Region, true);
            //this.Refresh();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.Refresh();
        }


        #endregion

        
    }
}
