namespace zControlsC.UI_Controls
{
    partial class zQueryFilter
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbCriteria = new System.Windows.Forms.ComboBox();
            this.lblDash = new System.Windows.Forms.Label();
            this.txtCriteria2 = new System.Windows.Forms.TextBox();
            this.btnKill = new System.Windows.Forms.PictureBox();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.cmbOperator = new System.Windows.Forms.ComboBox();
            this.txtCriteria = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnKill)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCriteria
            // 
            this.cmbCriteria.FormattingEnabled = true;
            this.cmbCriteria.Location = new System.Drawing.Point(240, 3);
            this.cmbCriteria.Name = "cmbCriteria";
            this.cmbCriteria.Size = new System.Drawing.Size(145, 21);
            this.cmbCriteria.TabIndex = 17;
            this.cmbCriteria.Visible = false;
            // 
            // lblDash
            // 
            this.lblDash.BackColor = System.Drawing.Color.Transparent;
            this.lblDash.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDash.Location = new System.Drawing.Point(306, 7);
            this.lblDash.Name = "lblDash";
            this.lblDash.Size = new System.Drawing.Size(14, 12);
            this.lblDash.TabIndex = 16;
            this.lblDash.Text = "-";
            this.lblDash.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDash.Visible = false;
            // 
            // txtCriteria2
            // 
            this.txtCriteria2.Location = new System.Drawing.Point(321, 3);
            this.txtCriteria2.Name = "txtCriteria2";
            this.txtCriteria2.Size = new System.Drawing.Size(64, 20);
            this.txtCriteria2.TabIndex = 15;
            this.txtCriteria2.Visible = false;
            // 
            // btnKill
            // 
            this.btnKill.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKill.Image = global::zControlsC.Properties.Resources.Red_X;
            this.btnKill.Location = new System.Drawing.Point(391, 3);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(17, 20);
            this.btnKill.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnKill.TabIndex = 14;
            this.btnKill.TabStop = false;
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // cmbField
            // 
            this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(3, 3);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(131, 21);
            this.cmbField.TabIndex = 11;
            this.cmbField.SelectedIndexChanged += new System.EventHandler(this.cmbField_SelectedIndexChanged);
            // 
            // cmbOperator
            // 
            this.cmbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperator.FormattingEnabled = true;
            this.cmbOperator.Items.AddRange(new object[] {
            "=",
            "<>",
            ">",
            "<",
            "Between Dates",
            "Like",
            "Is Null",
            "Is Not Null"});
            this.cmbOperator.Location = new System.Drawing.Point(140, 3);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(94, 21);
            this.cmbOperator.TabIndex = 12;
            this.cmbOperator.SelectedIndexChanged += new System.EventHandler(this.cmbOperator_SelectedIndexChanged);
            // 
            // txtCriteria
            // 
            this.txtCriteria.Location = new System.Drawing.Point(240, 3);
            this.txtCriteria.Name = "txtCriteria";
            this.txtCriteria.Size = new System.Drawing.Size(145, 20);
            this.txtCriteria.TabIndex = 13;
            // 
            // zQueryFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbCriteria);
            this.Controls.Add(this.lblDash);
            this.Controls.Add(this.txtCriteria2);
            this.Controls.Add(this.btnKill);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.cmbOperator);
            this.Controls.Add(this.txtCriteria);
            this.Name = "zQueryFilter";
            this.Size = new System.Drawing.Size(414, 27);
            ((System.ComponentModel.ISupportInitialize)(this.btnKill)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox cmbCriteria;
        internal System.Windows.Forms.Label lblDash;
        internal System.Windows.Forms.TextBox txtCriteria2;
        internal System.Windows.Forms.PictureBox btnKill;
        internal System.Windows.Forms.ComboBox cmbField;
        internal System.Windows.Forms.ComboBox cmbOperator;
        internal System.Windows.Forms.TextBox txtCriteria;
    }
}
