namespace Collar.WinControls
{
    partial class HLSColorPicker
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
                ColorPalette.Dispose();
                BWPalette.Dispose();               
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
            this.ColorMap = new System.Windows.Forms.Panel();
            this.ColorName = new System.Windows.Forms.Label();
            this.BWMap = new System.Windows.Forms.Panel();
            this.AlphaMap = new System.Windows.Forms.Panel();
            this.ColorMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // ColorMap
            // 
            this.ColorMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorMap.Controls.Add(this.ColorName);
            this.ColorMap.Location = new System.Drawing.Point(0, 40);
            this.ColorMap.Name = "ColorMap";
            this.ColorMap.Size = new System.Drawing.Size(450, 230);
            this.ColorMap.TabIndex = 0;
            this.ColorMap.Paint += new System.Windows.Forms.PaintEventHandler(this.ColorMap_Paint);
            this.ColorMap.Resize += new System.EventHandler(this.ColorMap_Resize);
            // 
            // ColorName
            // 
            this.ColorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ColorName.AutoSize = true;
            this.ColorName.BackColor = System.Drawing.Color.Transparent;
            this.ColorName.ForeColor = System.Drawing.Color.White;
            this.ColorName.Location = new System.Drawing.Point(3, 210);
            this.ColorName.Name = "ColorName";
            this.ColorName.Size = new System.Drawing.Size(14, 13);
            this.ColorName.TabIndex = 0;
            this.ColorName.Text = "#";
            // 
            // BWMap
            // 
            this.BWMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BWMap.Location = new System.Drawing.Point(0, 20);
            this.BWMap.Name = "BWMap";
            this.BWMap.Size = new System.Drawing.Size(450, 20);
            this.BWMap.TabIndex = 1;
            this.BWMap.Paint += new System.Windows.Forms.PaintEventHandler(this.BWMap_Paint);
            // 
            // AlphaMap
            // 
            this.AlphaMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AlphaMap.Location = new System.Drawing.Point(0, 0);
            this.AlphaMap.Name = "AlphaMap";
            this.AlphaMap.Size = new System.Drawing.Size(450, 20);
            this.AlphaMap.TabIndex = 2;
            this.AlphaMap.Paint += new System.Windows.Forms.PaintEventHandler(this.AlphaMap_Paint);
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.BWMap);
            this.Controls.Add(this.ColorMap);
            this.Controls.Add(this.AlphaMap);
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(450, 271);
            this.Load += new System.EventHandler(this.ColorPicker_Load);
            this.Resize += new System.EventHandler(this.ColorPicker_Resize);
            this.ColorMap.ResumeLayout(false);
            this.ColorMap.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ColorMap;
        private System.Windows.Forms.Panel BWMap;
        private System.Windows.Forms.Panel AlphaMap;
        private System.Windows.Forms.Label ColorName;
    }
}
