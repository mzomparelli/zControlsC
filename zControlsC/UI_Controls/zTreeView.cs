using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using zControlsC.Properties;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Drawing2D;



namespace zControlsC.UI_Controls
{
    [ToolboxBitmap(typeof(TreeView))]
    public class zTreeView : System.Windows.Forms.TreeView

    {

        
        #region Component Designer generated code

        private System.ComponentModel.Container components = null;

        private void InitializeComponent()
        {
            this.HideSelection = false;
            this.LabelEdit = false;
            this.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            this.Font = new Font(this.Font.FontFamily, 12, this.Font.Style, GraphicsUnit.Pixel);
            this.ShowPlusMinus = false;
            
            //this.DrawNode += new DrawTreeNodeEventHandler(zTreeView_DrawNode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region "Declarations"

        Color _selectedDarkColor = Color.Gold;
        Color _selectedLightColor = Color.White;
        Color _selectedBorderColor = Color.Gray;

        Color _BranchLineColor = Color.Gray;
        System.Drawing.Drawing2D.DashStyle _BranchLineStyle = System.Drawing.Drawing2D.DashStyle.Dot;

        bool _DrawIcons = false;

        Image _ExpandIcon = null;
        Image _CollapseIcon = null;




        #endregion

        #region "Enums"

        public enum zTreeViewExpandedIconSet
        {
            Round,
            Simple
        }


        #endregion

        #region "Properties"

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                Font f = new Font(value.FontFamily, value.Size, value.Style, GraphicsUnit.Pixel);
                base.Font = f;
            }
        }


        public Color SelectedDarkColor
        {
            get { return _selectedDarkColor; }
            set { _selectedDarkColor = value; }
        }

        public Color SelectedLightColor
        {
            get { return _selectedLightColor; }
            set { _selectedLightColor = value; }
        }

        public Color SelectedBorderColor
        {
            get { return _selectedBorderColor; }
            set { _selectedBorderColor = value; }
        }

        public Color BranchLineColor
        {
            get { return _BranchLineColor; }
            set { 
                _BranchLineColor = value;
                this.Invalidate();
                }
        }

        public System.Drawing.Drawing2D.DashStyle BranchLineStyle
        {
            get { return _BranchLineStyle; }
            set
            {
                _BranchLineStyle = value;
                this.Invalidate();
            }
        }

        [DefaultValue(null)]
        public Image ExpandIcon
        {
            get { return _ExpandIcon; }
            set { 
                _ExpandIcon = value;
                this.Refresh();
                }
        }

        [DefaultValue(null)]
        public Image CollpaseIcon
        {
            get { return _CollapseIcon; }
            set { 
                _CollapseIcon = value;
                this.Refresh();
                }
        }


        #endregion

        #region "Public Methods"

        

        public zTreeView() : base()
        {
            InitializeComponent();
        }

        public zTreeView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region "Private Methods"

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.Invalidate();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            this.Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);            
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
                base.OnNodeMouseClick(e); 
                       
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
                base.OnAfterSelect(e);
                this.Refresh();
        }

        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            base.OnAfterCollapse(e);
            this.Refresh();
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
            this.Refresh();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Get the node where the user clicked
            zTreeNode clickedNode = (zTreeNode)this.GetNodeAt(e.X, e.Y);

            if (clickedNode == null)
            {
                return;
            }

            if (NodeBounds(clickedNode).Contains(e.X, e.Y)) //User clicked the node text
            {
                //select the node
                this.SelectedNode = clickedNode;
            }
            else if (clickedNode.Nodes.Count > 0 && ExpandCollapseBounds(clickedNode).Contains(e.X, e.Y))   //User clicked the Expand/Collpase
            {
                //Expand the node
                if (clickedNode.IsExpanded) { clickedNode.Collapse(); } else { clickedNode.Expand(); }
            }
            else
            {
                //do the default
                base.OnMouseDown(e);
            }

            //base.OnMouseDown(e);

        }


        private Rectangle NodeBounds(zTreeNode node)
        {
            Graphics g = this.CreateGraphics();
            Font fontRegular = new Font(this.Font.FontFamily, this.Font.Size - 1, this.Font.Style, GraphicsUnit.Pixel);

            SizeF stringLength = g.MeasureString(node.Text, fontRegular);
            int sw = (int)stringLength.Width + 2;
            int sh = (int)stringLength.Height;

            CheckForIcons();
            int iconSize = node.Bounds.Height - 6;
            int textOffest = _DrawIcons == true ? iconSize + 6 : 0;     //If wee need to draw icons then we need to make room
            Rectangle bounds = new Rectangle(node.Bounds.X + textOffest, node.Bounds.Y, sw, node.Bounds.Height - 1);
            return bounds;

        }

        private Rectangle ExpandCollapseBounds(zTreeNode node)
        {
            CheckForIcons();
            int iconSize = node.Bounds.Height - 6;
            Rectangle bounds = new Rectangle(node.Bounds.X - 23, node.Bounds.Y + 3, iconSize, iconSize);
            return bounds;

        }

        private int iCount = 0;
        private void CheckForIcons()
        {
            //Check if we need to make room to draw the icons
            if (iCount > 0)
            {
                return;
            }
            if (!_DrawIcons)
            {
                foreach (zTreeNode n in this.Nodes)
                {
                    if (n.Image != null)
                    {
                        _DrawIcons = true;
                        break;
                    }
                }
            }

            iCount++;
        }

        
        
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            //Check if we need to make room to draw the icons
            CheckForIcons();

            TreeView tv = (TreeView)this;

            e.DrawDefault = false;
            //Get the current node from the args
            zTreeNode theNode = (zTreeNode)e.Node;            

            //Declare a string format
            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoWrap;
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;            

            //Specify the colors
            Color clrDark;
            Color clrLight;
            Color clrBorder;

            //If the individual node has a selection color then use it
            //Otherwise use the treeview selected color
            if (theNode.SelectedBackColorDark == Color.Transparent) { clrDark = this.SelectedDarkColor; }
            else { clrDark = theNode.SelectedBackColorDark; }

            if (theNode.SelectedBackColorLight == Color.Transparent) { clrLight = this.SelectedLightColor; }
            else { clrLight = theNode.SelectedBackColorLight; }

            if (theNode.SelectedBorderColor == Color.Transparent) { clrBorder = this.SelectedBorderColor; }
            else { clrBorder = theNode.SelectedBorderColor; }

            
            //Specify the fonts for the selected and regular nodes
            Font fontSelected = new Font(this.Font.FontFamily, this.Font.Size - 1, this.Font.Style, GraphicsUnit.Pixel);
            Font fontRegular = new Font(this.Font.FontFamily, this.Font.Size - 1, this.Font.Style, GraphicsUnit.Pixel);
            Font fontSubTextRegular = new Font(this.Font.FontFamily, this.Font.Size - 1, this.Font.Style, GraphicsUnit.Pixel);
            Font fontSubTextSelected = new Font(this.Font.FontFamily, this.Font.Size - 1, this.Font.Style, GraphicsUnit.Pixel);

            //Specify the rectangles for the Icon
            int iconSize = e.Node.Bounds.Height - 6;
            Rectangle rectIcon = new Rectangle(e.Node.Bounds.X + 3, e.Bounds.Y + 3, iconSize, iconSize);

            string strCount = theNode.SubText;
            string strNode = theNode.Text;

            System.Drawing.Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            
            SolidBrush nodeBrush = new SolidBrush(theNode.ForeColor);
            SolidBrush countBrush = new SolidBrush(theNode.SubTextColor);

            //Dim rectNode As Rectangle = rectSelParent  
            SizeF stringLength = g.MeasureString(theNode.Text, fontRegular);
            SizeF stringLength1 = g.MeasureString(theNode.SubText, fontRegular);

            int sw = (int)stringLength.Width + 2;
            int sh = (int)stringLength.Height;

            SizeF subtextLength = g.MeasureString(theNode.SubText, fontRegular);
            int sw1 = (int)stringLength1.Width + 2;
            int sh1 = (int)stringLength1.Height;

            Rectangle rectNode = new Rectangle(e.Bounds.X, e.Bounds.Y, sw, e.Bounds.Height - 1);

            int textOffest = _DrawIcons == true ? iconSize + 6 : 0;     //If wee need to draw icons then we need to make room

            Rectangle rectCount = new Rectangle(e.Node.Bounds.X + sw + textOffest, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2) - (this.Font.Size / 2) - 1), sw1, (int)this.Font.Size + 1);
            Rectangle rectSelCount = new Rectangle(e.Node.Bounds.X + sw + textOffest, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2) - (this.Font.Size / 2) - 1), e.Node.Bounds.Width + 5, (int)this.Font.Size + 1);

            Rectangle rectText = new Rectangle(e.Node.Bounds.X + textOffest, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2) - (this.Font.Size / 2) - 1), sw, (int)this.Font.Size + 1);
            Rectangle rectSelText = new Rectangle(e.Node.Bounds.X + textOffest, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2) - (this.Font.Size / 2) - 1), sw, (int)this.Font.Size + 1);

            Rectangle rectFull = new Rectangle(e.Node.Bounds.X + textOffest, e.Node.Bounds.Y, sw, e.Node.Bounds.Height - 1);
            Rectangle rectSelFull = new Rectangle(e.Node.Bounds.X + textOffest, e.Node.Bounds.Y, sw, e.Node.Bounds.Height - 1);

            

            Pen pen;
                                   
            if (theNode.IsSelected) //Paint the node selected
            {
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(rectNode, clrLight, clrDark, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                pen = new Pen(clrBorder);

                g.FillRectangle(brush, rectSelFull);
                g.DrawRectangle(pen, rectSelFull);

                if (_DrawIcons)
                {
                    if (theNode.Image != null) { e.Graphics.DrawImage(theNode.Image, rectIcon); }
                }
                g.DrawString(strCount, fontSubTextSelected, countBrush, rectSelCount);
                g.DrawString(strNode, fontSelected, nodeBrush, rectSelText, sf);
            }
            else    //Paint the node normal
            {
                pen = new Pen(this.BackColor);
                g.DrawRectangle(pen, e.Bounds);

                if (_DrawIcons)
                {
                    if (theNode.Image != null) { e.Graphics.DrawImage(theNode.Image, rectIcon); }
                }
                g.DrawString(strCount, fontSubTextRegular, countBrush, rectCount);
                g.DrawString(strNode, fontRegular, nodeBrush, rectText, sf);
            }            
            
                //Draw the node connecting lines (brnaches)
                switch (theNode.Level)
                {
                    case 0: //Node level 0                    
                        
                        //Draw the connecting lines
                        Pen penLine = new Pen(_BranchLineColor);
                        penLine.DashStyle = _BranchLineStyle;

                        int ih = e.Node.Index == 0 ? 0 : (e.Node.Bounds.Height / 2);
                        int o = theNode.Image == null ? textOffest : 0;

                        Point ptLine_1A = new Point(e.Node.Bounds.X - 3 + o, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)));
                        Point ptLine_1B = new Point(e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)));

                        Point ptLine_2A = new Point(e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)));
                        Point ptLine_2B = new Point(e.Node.Bounds.X - 10, e.Node.Bounds.Y - ih);

                        g.DrawLine(penLine, ptLine_1A, ptLine_1B);
                        g.DrawLine(penLine, ptLine_2A, ptLine_2B);

                        break;
                    case 1: //Node level 1

                        //Draw the connecting lines
                        Pen penLine1 = new Pen(_BranchLineColor);
                        penLine1.DashStyle = _BranchLineStyle;
                        o = theNode.Image == null ? textOffest : 0;
                        g.DrawLine(penLine1, e.Node.Bounds.X - 3 + o, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)));
                        
                        if (e.Node.Parent.FirstNode.Text == e.Node.Text)
                        {
                            g.DrawLine(penLine1, e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 10, e.Node.Bounds.Y - 2);
                        }
                        else
                        {
                            g.DrawLine(penLine1, e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 10, e.Node.Bounds.Y - (e.Node.Bounds.Height / 2));
                        }

                        g.DrawLine(penLine1, e.Node.Bounds.X - 29, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 29, e.Node.Bounds.Y - (e.Node.Bounds.Height / 2));

                        
                        break;

                    case 2: //Node level 2

                        //Draw the connecting lines
                        o = theNode.Image == null ? textOffest : 0;
                        penLine1 = new Pen(_BranchLineColor);
                        penLine1.DashStyle = _BranchLineStyle;
                        g.DrawLine(penLine1, e.Node.Bounds.X - 3 + o, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)));

                        if (e.Node.Parent.FirstNode.Text == e.Node.Text)
                        {
                            g.DrawLine(penLine1, e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 10, e.Node.Bounds.Y - 2);
                        }
                        else
                        {
                            g.DrawLine(penLine1, e.Node.Bounds.X - 10, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 10, e.Node.Bounds.Y - (e.Node.Bounds.Height / 2));
                        }
                        if (e.Node.Parent.LastNode.Text != e.Node.Text)
                        {
                            g.DrawLine(penLine1, e.Node.Bounds.X - 29, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 29, e.Node.Bounds.Y - (e.Node.Bounds.Height / 2));
                        }

                        g.DrawLine(penLine1, e.Node.Bounds.X - 48, (int)(e.Node.Bounds.Y + (e.Node.Bounds.Height / 2)), e.Node.Bounds.X - 48, e.Node.Bounds.Y - (e.Node.Bounds.Height / 2));

                        break;

                    case 3: //Node level 3

                        break;

                    case 4: //Node level 4

                        break;

                    case 5: //Node level 5
                        
                        break;

                    case 6: //Node level 6

                        break;
                    default:    //default should never be used

                        break;

                }

                //paint the expand/collapse icon
                if (theNode.Nodes.Count > 0)
                {
                    Image collpase  = _CollapseIcon == null ? Resources._12       : _CollapseIcon;
                    Image expand    = _ExpandIcon == null   ? Resources._11   : _ExpandIcon;


                    Rectangle rectImage1 = new Rectangle(e.Node.Bounds.X - 23, e.Node.Bounds.Y + 3, iconSize, iconSize);
                    if (theNode.IsExpanded)
                    {
                        g.DrawImage(collpase, rectImage1);
                    }
                    else
                    {
                        g.DrawImage(expand, rectImage1);
                    }

                } 

            nodeBrush.Dispose();
            countBrush.Dispose();
        }


        

        #endregion

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
        }


    }

   


}
