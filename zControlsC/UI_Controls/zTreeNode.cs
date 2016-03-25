using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace zControlsC.UI_Controls
{
    //[Serializable()]
    public class zTreeNode : TreeNode
    {

        #region "Declarations"

        Image _Image;
        string _sSubText;
        Color _SubTextColor = Color.Blue;
        Color _SelectedBackColorDark = Color.Transparent;
        Color _SelectedBackColorLight = Color.Transparent;
        Color _SelectedBorderColor = Color.Transparent;


        #endregion

        #region "Properties"

        public Color SelectedBackColorDark
        {
            get { return _SelectedBackColorDark; }
            set { _SelectedBackColorDark = value; }
        }

        public Color SelectedBackColorLight
        {
            get { return _SelectedBackColorLight; }
            set { _SelectedBackColorLight = value; }
        }

        public Color SelectedBorderColor
        {
            get { return _SelectedBorderColor; }
            set { _SelectedBorderColor = value; }
        }

        public Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        public string SubText
        {
            get { return _sSubText; }
            set { _sSubText = value; }
        }

        public Color SubTextColor
        {
            get { return _SubTextColor; }
            set { _SubTextColor = value; }
        }

        #endregion

        #region "Public Methods"

        public zTreeNode()
        {
            ForeColor = Color.Black;
        }

        public zTreeNode(string text) : base(text)
        {
            this.Text = text;
            ForeColor = Color.Black;
        }

        public zTreeNode(string text, string subText) : base(text)
        {
            this.SubText = subText;
            ForeColor = Color.Black;
        }



        public zTreeNode(string text, Image icon) : base(text)
        {
            this.Image = icon;
            ForeColor = Color.Black;
        }

        public zTreeNode(string text, string subText, Image icon) : base(text)
        {
            this.SubText = subText;
            this.Image = icon;
            ForeColor = Color.Black;
        }

        public zTreeNode(string text, string subText, Color selectedColorDark, Color selectedColorLight, Color selectedColorBorder)
            : base(text)
        {
            this.SubText = subText;
            ForeColor = Color.Black;
            this.SelectedBackColorDark = selectedColorDark;
            this.SelectedBackColorLight = selectedColorLight;
            this.SelectedBorderColor = selectedColorBorder;
        }

        public zTreeNode(string text, string subText, Image icon, Color selectedColorDark, Color selectedColorLight, Color selectedColorBorder) : base(text)
        {
            this.SubText = subText;
            this.Image = icon;
            ForeColor = Color.Black;
            this.SelectedBackColorDark = selectedColorDark;
            this.SelectedBackColorLight = selectedColorLight;
            this.SelectedBorderColor = selectedColorBorder;
        }

        #endregion

        #region "Private Methods"


        #endregion

       


    }
}
