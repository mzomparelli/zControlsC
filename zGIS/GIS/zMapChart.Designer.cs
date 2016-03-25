namespace zGIS
{
    partial class zMapChart
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
            this.SuspendLayout();
            // 
            // zMapChart
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Blue;
            this.DoubleBuffered = true;
            this.Name = "zMapChart";
            this.Size = new System.Drawing.Size(761, 453);
            this.Load += new System.EventHandler(this.zMapChart_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zMapChart_MouseClick);
            this.SizeChanged += new System.EventHandler(this.zMapChart_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
