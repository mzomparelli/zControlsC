using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace zControlsC.UI_Controls
{
    [ToolboxBitmap(@"E:\SourceCode\C#\zControlsC\zControlsC\UI_Controls\SQL.ico")]
    public partial class zQueryFilter : UserControl
    {
        

    #region "Initialize"

        public zQueryFilter()
        {
            InitializeComponent();
        }

    #endregion


    #region "Declarations"

        bool useCriteriaComboBox = false;

    #endregion

    #region "Properties"

        public string FilterField
        {
            get { return this.cmbField.Text; }
            set { this.cmbField.Text = value; }
        }

        public string FilterOperator
        {
            get { return this.cmbOperator.Text; }
            set { this.cmbOperator.Text = value; }
        }

        public string FilterCriteria
        {
            get { return this.txtCriteria.Text; }
            set { this.txtCriteria.Text = value; }
        }

        public string FilterCriteria2
        {
            get { return this.txtCriteria2.Text; }
            set { this.txtCriteria2.Text = value; }
        }

        public bool UseCriteriaComboBox
        {
            get { return useCriteriaComboBox; }
            set
            {
                useCriteriaComboBox = value;
                AdjustControls();
            }
        }




    #endregion

    #region "Public Methods"


        public void SetForNumber()
        {
            this.cmbOperator.Items.Clear();
            this.cmbOperator.Items.Add("=");
            this.cmbOperator.Items.Add("<>");
            this.cmbOperator.Items.Add(">");
            this.cmbOperator.Items.Add("<");
            this.cmbOperator.Items.Add("Is Null");
            this.cmbOperator.Items.Add("Is Not Null");
        }

        public void SetForText()
        {
            this.cmbOperator.Items.Clear();
            this.cmbOperator.Items.Add("=");
            this.cmbOperator.Items.Add("<>");
            this.cmbOperator.Items.Add(">");
            this.cmbOperator.Items.Add("<");
            this.cmbOperator.Items.Add("Like");
            this.cmbOperator.Items.Add("Is Null");
            this.cmbOperator.Items.Add("Is Not Null");
        }

        public void SetForDate()
        {
            this.cmbOperator.Items.Clear();
            this.cmbOperator.Items.Add("Between Dates");
            this.cmbOperator.Items.Add("Prompt Me");
        }

        public void AddCriteria(string Item)
        {
            this.cmbCriteria.Items.Add(Item);
        }

        public void ClearCriteriaItems()
        {
            this.cmbCriteria.Items.Clear();
        }

        public void AddField(string Item)
        {
            this.cmbField.Items.Add(Item);
        }

        public void ClearFields()
        {
            this.cmbField.Items.Clear();
        }


        public void AdjustControls()
        {
            if (this.cmbOperator.Text == "Is Null" | this.cmbOperator.Text == "Is Not Null")
            {
                this.txtCriteria.Text = "";
                this.cmbCriteria.Visible = false;
                this.txtCriteria.Enabled = false;
                this.txtCriteria.Width = 145;
                this.txtCriteria2.Visible = false;
                this.lblDash.Visible = false;
            }
            else if (this.cmbOperator.Text == "Between Dates")
            {
                this.cmbCriteria.Visible = false;
                this.txtCriteria.Enabled = true;
                this.txtCriteria2.Enabled = true;
                this.txtCriteria.Width = 64;
                this.txtCriteria2.Visible = true;
                this.txtCriteria.Text = "";
                this.txtCriteria2.Text = "";
                this.lblDash.Visible = true;
            }
            else if (this.cmbOperator.Text == "Prompt Me")
            {
                this.txtCriteria.Text = "";
                this.cmbCriteria.Visible = false;
                this.txtCriteria.Enabled = false;
                this.txtCriteria2.Enabled = false;
                this.txtCriteria.Text = "&Prompt1";
                this.txtCriteria2.Text = "&Prompt2";
                this.txtCriteria.Width = 64;
                this.txtCriteria2.Visible = true;
                this.lblDash.Visible = true;
            }
            else
            {
                if (useCriteriaComboBox)
                {
                    this.cmbCriteria.Visible = true;
                    this.txtCriteria.Enabled = true;
                    this.txtCriteria.Visible = false;
                    this.txtCriteria2.Visible = false;
                    this.lblDash.Visible = false;
                }
                else
                {
                    this.txtCriteria.Visible = true;
                    this.txtCriteria.Enabled = true;
                    this.txtCriteria.Width = 145;
                    this.txtCriteria2.Visible = false;
                    this.lblDash.Visible = false;
                    this.cmbCriteria.Visible = false;
                }

            }
        }


    #endregion

    #region "Private Methods"

        private void btnKill_Click(object sender, EventArgs e)
        {
            UserClickedX(this, new EventArgs());
        }

        private void cmbOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            AdjustControls();
        }

        private void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtCriteria.Text = "";
            this.txtCriteria2.Text = "";
            this.cmbCriteria.Text = "";
            UserChangedField(this, new EventArgs());
        }

    #endregion

    #region "Events"

        public event EventHandler UserClickedX;
        public event EventHandler UserChangedField;

    #endregion

                        

    }
}
