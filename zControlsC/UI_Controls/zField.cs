using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zControlsC.UI_Controls
{
    public partial class zField : UserControl
    {
        public zField()
        {
            InitializeComponent();
        }

    #region "Declarations"

        private string databaseFieldName = "";
        private bool isActive = false;
        private FieldStyle fieldDisplayStyle = FieldStyle.TextBox;

    #endregion

    #region "Properties"

        #region "ComboBox"

        public ComboBoxStyle ComboBoxDropDownStyle
        {
            get { return this.zFieldComboBox.DropDownStyle; }
            set { this.zFieldComboBox.DropDownStyle = value; }
        }

        public bool ComboBoxEnabled
        {
            get { return this.zFieldComboBox.Enabled; }
            set { this.zFieldComboBox.Enabled = value; }
        }

        public ComboBox.ObjectCollection ComboBoxItems
        {
            get { return this.zFieldComboBox.Items; }
        }

        public int ComboBoxMaxDropDownItems
        {
            get { return this.zFieldComboBox.MaxDropDownItems; }
            set { this.zFieldComboBox.MaxDropDownItems = value; }
        }

        public bool ComboBoxSorted
        {
            get { return this.zFieldComboBox.Sorted; }
            set { this.zFieldComboBox.Sorted = value; }
        }

        public object ComboBoxTag
        {
            get { return this.zFieldComboBox.Tag; }
            set { this.zFieldComboBox.Tag = value; }
        }

        public string ComboBoxText
        {
            get { return this.zFieldComboBox.Text; }
            set { this.zFieldComboBox.Text = value; }
        }

        #endregion

        #region "TextBox"

        public string TextBoxText
        {
            get { return this.zLabelTextBox.Text; }
            set { this.zLabelTextBox.Text = value; }
        }

        public int TextBoxMaxChar
        {
            get { return this.zLabelTextBox.MaxLength; }
            set { this.zLabelTextBox.MaxLength = value; }
        }

        public CharacterCasing TextBoxCharCasing
        {
            get { return this.zLabelTextBox.CharacterCasing; }
            set { this.zLabelTextBox.CharacterCasing = value; }
        }

        public bool TextBoxEnabled
        {
            get { return this.zLabelTextBox.Enabled; }
            set { this.zLabelTextBox.Enabled = value; }
        }

        public char TextBoxPasswordChar
        {
            get { return this.zLabelTextBox.PasswordChar; }
            set { this.zLabelTextBox.PasswordChar = value; }
        }

        public bool TextBoxReadOnly
        {
            get { return this.zLabelTextBox.ReadOnly; }
            set { this.zLabelTextBox.ReadOnly = value; }
        }

        public object TextBoxTag
        {
            get { return this.zLabelTextBox.Tag; }
            set { this.zLabelTextBox.Tag = value; }
        }

        public HorizontalAlignment TextBoxAlignment
        {
            get { return this.zLabelTextBox.TextAlign; }
            set { this.zLabelTextBox.TextAlign = value; }
        }

        public bool TextBoxMultiLine
        {
            get { return this.zLabelTextBox.Multiline; }
            set { 
                this.zLabelTextBox.Multiline = value;
                if (value == false)
                {
                    this.Size = new Size(this.Size.Width, 20);
                }
                else
                {
                    if (this.fieldDisplayStyle == FieldStyle.TextBox)
                    {
                        this.zLabelTextBox.Location = new Point(122, 3);
                        this.zLabelTextBox.Size = new Size(this.zLabelTextBox.Size.Width, 20);
                    }
                    else if (this.fieldDisplayStyle == FieldStyle.TextFill)
                    {
                        this.zLabelTextBox.Location = new Point(0, 0);
                        this.zLabelTextBox.Size = new Size(this.zLabelTextBox.Size.Width, this.zLabelTextBox.Size.Height);
                    }
                }
            }
        }

        #endregion

        #region "Label"

        public string FieldLabel
        {
            get { return this.zFieldLabel.Text; }
            set { this.zFieldLabel.Text = value; }
        }

        public bool FieldLabelAutoEllipsis
        {
            get { return this.zFieldLabel.AutoEllipsis; }
            set { this.zFieldLabel.AutoEllipsis = value; }
        }

        public int FieldLabelWidth
        {
            get { return this.zFieldLabel.Width; }
            set
            {
                this.zFieldLabel.Width = value;
                //If the display style is text fill then don't do anything
                if (this.fieldDisplayStyle == FieldStyle.TextFill) { return; }

                int i = this.zFieldLabel.Size.Width - value;
                //Resize the label
                this.zFieldLabel.Size = new Size(value, this.zFieldLabel.Size.Height);
                //Resize and re-position the textbox
                this.zLabelTextBox.Location = new Point(this.zLabelTextBox.Location.X - i, this.zLabelTextBox.Location.Y);
                this.zLabelTextBox.Size = new Size(this.zLabelTextBox.Size.Width + i, this.zLabelTextBox.Size.Height);
                //Resize and re-position the combobox
                this.zFieldComboBox.Location = new Point(this.zFieldComboBox.Location.X - i, this.zFieldComboBox.Location.Y);
                this.zFieldComboBox.Size = new Size(this.zFieldComboBox.Size.Width + i, 21);

                this.Invalidate();
            }
        }




        #endregion

        #region "Colors"

        public Color FieldLabelForeColor
        {
            get { return this.zFieldLabel.ForeColor; }
            set { this.zFieldLabel.ForeColor = value; }
        }

        public Color FieldLabelBackColor
        {
            get { return this.zFieldLabel.BackColor; }
            set { this.zFieldLabel.BackColor = value; }
        }

        public Color TextBoxForeColor
        {
            get { return this.zLabelTextBox.ForeColor; }
            set { this.zLabelTextBox.ForeColor = value; }
        }

        public Color TextBoxBackColor
        {
            get { return this.zLabelTextBox.BackColor; }
            set { this.zLabelTextBox.BackColor = value; }
        }

        public Color ComboBoxForeColor
        {
            get { return this.zFieldComboBox.ForeColor; }
            set { this.zFieldComboBox.ForeColor = value; }
        }

        public Color ComboBoxBackColor
        {
            get { return this.zFieldComboBox.BackColor; }
            set { this.zFieldComboBox.BackColor = value; }
        }



        #endregion

        public string DatabaseFieldName
        {
            get { return databaseFieldName; }
            set { databaseFieldName = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public FieldStyle FieldDisplayStyle
        {
            get { return fieldDisplayStyle; }
            set
            {
                fieldDisplayStyle = value;
                if (value == FieldStyle.ComboBox)
                {
                    this.zLabelTextBox.Visible = false;
                    this.zFieldComboBox.Visible = true;
                    this.zFieldLabel.Visible = true;

                    this.zFieldComboBox.Location = new Point(122, 2);
                    this.zFieldComboBox.Size = new Size(233, 21);
                    this.Size = new Size(358, 25);
                    this.zFieldLabel.Size = new Size(113, 25);

                }
                else if (value == FieldStyle.TextBox)
                {
                    this.zLabelTextBox.Visible = true;
                    this.zFieldComboBox.Visible = false;
                    this.zFieldLabel.Visible = true;

                    this.TextBoxMultiLine = false;
                    this.zLabelTextBox.Location = new Point(122, 3);
                    this.zLabelTextBox.Size = new Size(233, 21);
                    this.Size = new Size(358, 25);
                    this.zFieldLabel.Size = new Size(113, 25);

                }
                else if (value == FieldStyle.TextFill)
                {
                    this.zLabelTextBox.Visible = true;
                    this.zFieldComboBox.Visible = false;
                    this.zFieldLabel.Visible = false;

                    this.TextBoxMultiLine = true;
                    this.zLabelTextBox.Location = new Point(0, 0);
                    this.zLabelTextBox.Size = new Size(this.Size.Width, this.Size.Height);

                }
            }
        }

    #endregion

        #region "Enum and Structs"

        public enum FieldStyle
        {
            TextBox,
            ComboBox,
            TextFill
        }




        #endregion

        #region "Public Methods"


        #endregion

        #region "Private Methods"

        
        #endregion

        #region "Override Methods"

        public override string Text
        {
            get
            {
                switch (fieldDisplayStyle)
                {
                    case FieldStyle.ComboBox:
                        return this.zFieldComboBox.Text;
                    case FieldStyle.TextBox:
                        return this.zLabelTextBox.Text;
                    case FieldStyle.TextFill:
                        return this.zLabelTextBox.Text;
                    default:
                        return "";
                }
            }
            set
            {
                base.Text = value;
                switch (fieldDisplayStyle)
                {
                    case FieldStyle.ComboBox:
                        this.zFieldComboBox.Text = value;
                        break;
                    case FieldStyle.TextBox:
                        this.zLabelTextBox.Text = value;
                        break;
                    case FieldStyle.TextFill:
                        this.zLabelTextBox.Text = value;
                        break;
                }
            }
        }

       

        #endregion



    }


}
