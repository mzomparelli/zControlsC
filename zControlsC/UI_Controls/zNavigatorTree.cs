using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zControlsC.UI_Controls
{
    [ToolboxBitmap(@"E:\Resource\ICO\16x16 ico\money.ico")]
    public partial class zNavigatorTree : UserControl, IDisposable
    {       

        #region "Declarations"       

        //Group Variables - Used Internally Only - Soon to be Public
        private int _GroupHeight = 37;

        //Tree Variables - this needs to be created in the treeview class then removed from here
        private Color _TreeSelectedItemForeColor    = Color.Black;
        

        //Global Variables
        private zNavigatorTreeState _State = zNavigatorTreeState.Expanded;
        private bool _IsAnimating = false;
        private int _AnimationSpeed = 5;    //The Expand/Collapse animation speed
        private int _Width;                 //Store the width of the control
        private int _Height;                //Store the height of the control



        #endregion

        #region "Enums"

        public enum zNavigatorTreeState
        {
            Collapsed,
            Expanded
        }

        #endregion

        #region "Properties"

        [Category("Appearance")]
        public int AnimationSpeed
        {
            get { return _AnimationSpeed; }
            set { _AnimationSpeed = value; }
        }

        [Category("Behavior")]
        public bool IsAnimating
        {
            get { return _IsAnimating; }
        }

        [Category("Behavior")]
        public zNavigatorTreeState State
        {
            get { return _State; }
            set { 
                _State = value;
                if (_State == zNavigatorTreeState.Collapsed) { this.Collapse(); } else { this.Expand(); }
                }
        }

        
        #region "Tree Properties"

        [Category("TreeView")]
        public Color TreeSelectedItemBackColor
        {
            get { return tree.SelectedDarkColor; }
            set { tree.SelectedDarkColor = value; }
        }

        [Category("TreeView")]
        public Color TreeSelectedItemForeColor
        {
            get { return _TreeSelectedItemForeColor; }
            set { _TreeSelectedItemForeColor = value; }
        }

        [Category("TreeView")]
        public Color TreeSelectedItemBorderColor
        {
            get { return tree.SelectedBorderColor; }
            set { tree.SelectedBorderColor = value; }
        }

        [Category("TreeView")]
        public Color TreeBranchColor
        {
            get { return tree.BranchLineColor; }
            set { tree.BranchLineColor = value; }
        }

        [Category("TreeView")]
        public System.Drawing.Drawing2D.DashStyle TreeBranchStyle
        {
            get { return tree.BranchLineStyle; }
            set { tree.BranchLineStyle = value; }
        }

        [Category("TreeView")]
        [DefaultValue(null)]
        public Image TreeExpandIcon
        {
            get { return tree.ExpandIcon; }
            set { tree.ExpandIcon = value; }
        }

        [Category("TreeView")]
        [DefaultValue(null)]
        public Image TreeCollapseIcon
        {
            get { return tree.CollpaseIcon; }
            set { tree.CollpaseIcon = value; }
        }

        [Category("TreeView")]
        [DefaultValue(12)]
        public int TreeFontSize
        {
            get { return (int)tree.Font.Size; }
            set { tree.Font = new Font(tree.Font.FontFamily, value, tree.Font.Style, GraphicsUnit.Pixel); }
        }

        #endregion

            #region "Group Properties"

        [Category("Group Label")]
        public string GroupText
        {
            get { return Group.TextString; }
            set { Group.TextString = value; }
        }

        [Category("Group Label")]
        public Color GroupBackColor
        {
            get { return Group.BackColorDark; }
            set { Group.BackColorDark = value; }
        }

        [Category("Group Label")]
        public Color GroupForeColor
        {
            get { return Group.ForeColor; }
            set { Group.ForeColor = value; }
        }

        [Category("Group Label")]
        public Color GroupBackColorMouseOver
        {
            get { return Group.MouseOverBackColorDark; }
            set { Group.MouseOverBackColorDark = value; }
        }

        [Category("Group Label")]
        public Color GroupForeColorMouseOver
        {
            get { return Group.MouseOverForeColor; }
            set { Group.MouseOverForeColor = value; }
        }

        [Category("Group Label")]
        public Color GroupBorderColor
        {
            get { return Group.BackColorBorder; }
            set { Group.BackColorBorder = value; }
        }

        [Category("Group Label")]
        public Color GroupBorderColorMouseOver
        {
            get { return Group.MouseOverBorderColor; }
            set { Group.MouseOverBorderColor = value; }
        }

        [Category("Group Label")]
        public Font GroupFont
        {
            get { return Group.Font; }
            set { Group.Font = value; }
        }

        [Category("Group Label")]
        public int GroupTextOffsetVertical
        {
            get { return Group.TextOffsetVertical; }
            set { Group.TextOffsetVertical = value; }
        }

        [Category("Group Label")]
        public int GroupTextOffsetHorizontal
        {
            get { return Group.TextOffsetHorizontal; }
            set { Group.TextOffsetHorizontal = value; }
        }

        [Category("Group Label")]
        public StringAlignment GroupTextAlignment
        {
            get { return Group.TextAlign; }
            set { Group.TextAlign = value; }
        }

        [Category("Group Label")]
        public Image GroupIcon
        {
            get { return Group.Icon; }
            set {Group.Icon = value; }
        }

        [Category("Group Label")]
        public zLabel.IconSizeEnum GroupIconSize
        {
            get { return Group.IconSize; }
            set { Group.IconSize = value; }
        }

        [Category("Group Label")]
        public zLabel.IconAlignmentEnum GroupIconAlignment
        {
            get { return Group.IconAlignment; }
            set { Group.IconAlignment = value; }
        }

        [Category("Group Label")]
        public int GroupIconOffsetHorizontal
        {
            get { return Group.IconOffsetHorizontal; }
            set { Group.IconOffsetHorizontal = value; }
        }

        [Category("Group Label")]
        public int GroupIconOffsetVertical
        {
            get { return Group.IconOffsetVertical; }
            set { Group.IconOffsetVertical = value; }
        }

        #endregion

        #endregion

        #region "Public Methods"

        public zNavigatorTree()
        {
            InitializeComponent();
        }

        public void Collapse()
        {
            TreeBeginUpdate();
            timerExpand.Stop();
            timerCollapse.Interval = _AnimationSpeed;
            timerCollapse.Start();
            this.Refresh();
        }

        public void Expand()
        {
            TreeBeginUpdate();
            timerCollapse.Stop();
            timerExpand.Interval = _AnimationSpeed;
            timerExpand.Start();
            this.Refresh();
        }

        public void AddNode(zTreeNode n)
        {
            this.tree.Nodes.Add(n);
        }

        public void ClearNodes()
        {
            this.tree.Nodes.Clear();
        }

        public void TreeBeginUpdate()
        {
            tree.BeginUpdate();
        }

        public void TreeEndUpdate()
        {
            tree.EndUpdate();
        }

        #endregion        

        #region "Private Methods"

        //Animate the collapsing of the control
        private void timerCollapse_Tick(object sender, EventArgs e)
        {
            _IsAnimating = true;
            tree.Scrollable = false;
            if (this.Height == _GroupHeight)
            {
                _State = zNavigatorTreeState.Collapsed;
                timerCollapse.Stop();
                _IsAnimating = false;
                tree.Scrollable = true;
                TreeEndUpdate();
                return;
            }

            if (this.Height - 10 >= _GroupHeight) { this.Height -= 10; }
            else if (this.Height - 5 >= _GroupHeight) { this.Height -= 5; }
            else { this.Height -= 1; }
        }

        //Animate the expanding of the control
        private void timerExpand_Tick(object sender, EventArgs e)
        {
            _IsAnimating = true;
            tree.Scrollable = false;
            int h = this.DesignMode ? 200 : _Height;

            if (this.Height >= h)
            {
                _State = zNavigatorTreeState.Expanded;
                timerExpand.Stop();
                _IsAnimating = false;
                tree.Scrollable = true;
                TreeEndUpdate();
                return;
            }

            if (this.Height + 10 <= h) { this.Height += 10; }
            else if (this.Height + 5 <= h) { this.Height += 5; }
            else { this.Height += 1; }
        }

        //Load Event
        private void zNavigatorTree_Load(object sender, EventArgs e)
        {
            _Height = _State == zNavigatorTreeState.Expanded ? this.Height : 200;
        }

        //Make the Group button expand/collpase the control
        private void Group_Click(object sender, EventArgs e)
        {
            if (_State == zNavigatorTreeState.Collapsed)
            {
                Expand();
            }
            else if (_State == zNavigatorTreeState.Expanded)
            {
                Collapse();
            }
        }

        //control was resized - store the width and height
        private void Group_Resize(object sender, EventArgs e)
        {
            if (!_IsAnimating)  //don't store the width and height if the control is animating
            {
                _Height = _State == zNavigatorTreeState.Expanded ? this.Height : 200;
                _Width = this.Width;
                return;
            }

            
        }


        #endregion

        #region "Events"

        #region "TreeView Events"

        //Forward the TreeView Click event
        public event EventHandler TreeViewClick
        {
            add { this.tree.Click += value; }
            remove { this.tree.Click -= value; }
        }

        //Forward the TreeView DoubleClick event
        public event EventHandler TreeViewDoubleClick
        {
            add { this.tree.DoubleClick += value; }
            remove { this.tree.DoubleClick -= value; }
        }

        //Forward the TreeView DragDrop event
        public event DragEventHandler TreeViewDragDrop
        {
            add { this.tree.DragDrop += value; }
            remove { this.tree.DragDrop -= value; }
        }

        //Forward the TreeView DragEnter event
        public event DragEventHandler TreeViewDragEnter
        {
            add { this.tree.DragEnter += value; }
            remove { this.tree.DragEnter -= value; }
        }

        //Forward the TreeView DragLeave event
        public event EventHandler TreeViewDragLeave
        {
            add { this.tree.DragLeave += value; }
            remove { this.tree.DragLeave -= value; }
        }

        //Forward the TreeView ItemDrag event
        public event ItemDragEventHandler TreeViewItemDrag
        {
            add { this.tree.ItemDrag += value; }
            remove { this.tree.ItemDrag -= value; }
        }

        //Forward the TreeView MouseClick event
        public event MouseEventHandler TreeViewMouseClick
        {
            add { this.tree.MouseClick += value; }
            remove { this.tree.MouseClick -= value; }
        }

        //Forward the TreeView MouseDoubleClick event
        public event MouseEventHandler TreeViewMouseDoubleClick
        {
            add { this.tree.MouseDoubleClick += value; }
            remove { this.tree.MouseDoubleClick -= value; }
        }

        //Forward the TreeView MouseDown event
        public event MouseEventHandler TreeViewMouseDown
        {
            add { this.tree.MouseDown += value; }
            remove { this.tree.MouseDown -= value; }
        }

        //Forward the TreeNode MouseClick event
        public event TreeNodeMouseClickEventHandler TreeNodeMouseClick
        {
            add { this.tree.NodeMouseClick += value; }
            remove { this.tree.NodeMouseClick -= value; }
        }

        //Forward the TreeNode MouseDoubleClick event
        public event TreeNodeMouseClickEventHandler TreeNodeMouseDoubleClick
        {
            add { this.tree.NodeMouseDoubleClick += value; }
            remove { this.tree.NodeMouseDoubleClick -= value; }
        }

        //Forward the TreeNode MouseHover event
        public event TreeNodeMouseHoverEventHandler TreeNodeMouseHover
        {
            add { this.tree.NodeMouseHover += value; }
            remove { this.tree.NodeMouseHover -= value; }
        }


        #endregion



        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            tree.Dispose();
            Group.Dispose();
        }

        #endregion

        #region "Hidden Properties"

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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


        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override bool CanEnableIme
        {
            get
            {
                return base.CanEnableIme;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override bool CanRaiseEvents
        {
            get
            {
                return base.CanRaiseEvents;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override CreateParams CreateParams
        {
            get
            {
                return base.CreateParams;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override Cursor DefaultCursor
        {
            get
            {
                return base.DefaultCursor;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override ImeMode DefaultImeMode
        {
            get
            {
                return base.DefaultImeMode;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override System.Windows.Forms.Layout.LayoutEngine LayoutEngine
        {
            get
            {
                return base.LayoutEngine;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override bool ScaleChildren
        {
            get
            {
                return base.ScaleChildren;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override bool ShowFocusCues
        {
            get
            {
                return base.ShowFocusCues;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        protected override bool ShowKeyboardCues
        {
            get
            {
                return base.ShowKeyboardCues;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        #endregion

        
    }
}
