namespace zControlsC.UI_Controls
{
    partial class zField
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
            this.zFieldLabel = new System.Windows.Forms.Label();
            this.zLabelTextBox = new System.Windows.Forms.TextBox();
            this.zFieldComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // zFieldLabel
            // 
            this.zFieldLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.zFieldLabel.AutoEllipsis = true;
            this.zFieldLabel.BackColor = System.Drawing.Color.Transparent;
            this.zFieldLabel.Location = new System.Drawing.Point(3, 0);
            this.zFieldLabel.Name = "zFieldLabel";
            this.zFieldLabel.Size = new System.Drawing.Size(113, 27);
            this.zFieldLabel.TabIndex = 0;
            this.zFieldLabel.Text = "Field Label:";
            this.zFieldLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // zLabelTextBox
            // 
            this.zLabelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zLabelTextBox.Location = new System.Drawing.Point(122, 4);
            this.zLabelTextBox.Name = "zLabelTextBox";
            this.zLabelTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.zLabelTextBox.Size = new System.Drawing.Size(233, 20);
            this.zLabelTextBox.TabIndex = 1;
            // 
            // zFieldComboBox
            // 
            this.zFieldComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zFieldComboBox.FormattingEnabled = true;
            this.zFieldComboBox.Location = new System.Drawing.Point(122, 2);
            this.zFieldComboBox.Name = "zFieldComboBox";
            this.zFieldComboBox.Size = new System.Drawing.Size(233, 21);
            this.zFieldComboBox.TabIndex = 2;
            // 
            // zField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zFieldLabel);
            this.Controls.Add(this.zLabelTextBox);
            this.Controls.Add(this.zFieldComboBox);
            this.Name = "zField";
            this.Size = new System.Drawing.Size(358, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label zFieldLabel;
        public System.Windows.Forms.TextBox zLabelTextBox;
        public System.Windows.Forms.ComboBox zFieldComboBox;
    }
}
