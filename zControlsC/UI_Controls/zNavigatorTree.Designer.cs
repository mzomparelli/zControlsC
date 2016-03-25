using System.Windows.Forms;

namespace zControlsC.UI_Controls
{
    partial class zNavigatorTree
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
            this.components = new System.ComponentModel.Container();
            this.timerCollapse = new System.Windows.Forms.Timer(this.components);
            this.timerExpand = new System.Windows.Forms.Timer(this.components);
            this.tree = new zControlsC.UI_Controls.zTreeView(this.components);
            this.Group = new zControlsC.UI_Controls.zLabel();
            this.SuspendLayout();
            // 
            // timerCollapse
            // 
            this.timerCollapse.Tick += new System.EventHandler(this.timerCollapse_Tick);
            // 
            // timerExpand
            // 
            this.timerExpand.Tick += new System.EventHandler(this.timerExpand_Tick);
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.BranchLineColor = System.Drawing.Color.Gray;
            this.tree.BranchLineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.tree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tree.HideSelection = false;
            this.tree.Location = new System.Drawing.Point(0, 37);
            this.tree.Name = "tree";
            this.tree.SelectedBorderColor = System.Drawing.Color.Gray;
            this.tree.SelectedDarkColor = System.Drawing.Color.Gold;
            this.tree.SelectedLightColor = System.Drawing.Color.White;
            this.tree.ShowPlusMinus = false;
            this.tree.Size = new System.Drawing.Size(235, 153);
            this.tree.TabIndex = 1;
            // 
            // Group
            // 
            this.Group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Group.BackColorBorder = System.Drawing.Color.DarkGray;
            this.Group.BackColorDark = System.Drawing.Color.DimGray;
            this.Group.BackColorLight = System.Drawing.Color.White;
            this.Group.CornerSize = zControlsC.UI_Controls.zLabel.RoundedCornersEnum.None;
            this.Group.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Group.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Group.IconAlignment = zControlsC.UI_Controls.zLabel.IconAlignmentEnum.Near;
            this.Group.IconOffsetHorizontal = 0;
            this.Group.IconOffsetVertical = 0;
            this.Group.IconSize = zControlsC.UI_Controls.zLabel.IconSizeEnum.Size9;
            this.Group.Location = new System.Drawing.Point(0, 0);
            this.Group.Margin = new System.Windows.Forms.Padding(6);
            this.Group.MouseOverBackColorDark = System.Drawing.Color.Silver;
            this.Group.MouseOverBackColorLight = System.Drawing.Color.White;
            this.Group.MouseOverBorderColor = System.Drawing.Color.Silver;
            this.Group.MouseOverColorChange = true;
            this.Group.MouseOverFont = null;
            this.Group.MouseOverForeColor = System.Drawing.Color.Black;
            this.Group.Name = "Group";
            this.Group.RoundLowerLeftCorner = true;
            this.Group.RoundLowerRightCorner = true;
            this.Group.RoundUpperLeftCorner = true;
            this.Group.RoundUpperRightCorner = true;
            this.Group.Size = new System.Drawing.Size(235, 37);
            this.Group.TabIndex = 0;
            this.Group.TextAlign = System.Drawing.StringAlignment.Center;
            this.Group.TextOffsetHorizontal = 0;
            this.Group.TextOffsetVertical = -4;
            this.Group.TextString = "Group";
            this.Group.UseGradientBackColor = true;
            this.Group.Click += new System.EventHandler(this.Group_Click);
            this.Group.Resize += new System.EventHandler(this.Group_Resize);
            // 
            // zNavigatorTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tree);
            this.Controls.Add(this.Group);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "zNavigatorTree";
            this.Size = new System.Drawing.Size(235, 190);
            this.Load += new System.EventHandler(this.zNavigatorTree_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private zLabel Group;
        private zTreeView tree;
        private Timer timerExpand;
        private Timer timerCollapse;

    }
}
