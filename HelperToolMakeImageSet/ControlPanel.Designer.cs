namespace HelperToolMakeImageSet
{
    partial class ControlPanel
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lstImages = new System.Windows.Forms.ListBox();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.lstObjs = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSaveImageData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstImages
            // 
            this.lstImages.FormattingEnabled = true;
            this.lstImages.ItemHeight = 16;
            this.lstImages.Location = new System.Drawing.Point(3, 213);
            this.lstImages.Name = "lstImages";
            this.lstImages.Size = new System.Drawing.Size(210, 612);
            this.lstImages.TabIndex = 0;
            this.lstImages.SelectedIndexChanged += new System.EventHandler(this.lstPictures_SelectedIndexChanged);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(3, 833);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(210, 28);
            this.btnLoadImage.TabIndex = 1;
            this.btnLoadImage.Text = "Load images";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // pnlImage
            // 
            this.pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlImage.Location = new System.Drawing.Point(215, 2);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(709, 876);
            this.pnlImage.TabIndex = 2;
            this.pnlImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.event_MouseDown);
            this.pnlImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.event_MouseMove);
            this.pnlImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.event_MouseUp);
            // 
            // lstObjs
            // 
            this.lstObjs.FormattingEnabled = true;
            this.lstObjs.ItemHeight = 16;
            this.lstObjs.Location = new System.Drawing.Point(3, 2);
            this.lstObjs.Name = "lstObjs";
            this.lstObjs.Size = new System.Drawing.Size(206, 164);
            this.lstObjs.TabIndex = 3;
            this.lstObjs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstObjs_DrawItem);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 26);
            this.button1.TabIndex = 4;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(112, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 26);
            this.button2.TabIndex = 5;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnSaveImageData
            // 
            this.btnSaveImageData.Location = new System.Drawing.Point(3, 867);
            this.btnSaveImageData.Name = "btnSaveImageData";
            this.btnSaveImageData.Size = new System.Drawing.Size(210, 28);
            this.btnSaveImageData.TabIndex = 6;
            this.btnSaveImageData.Text = "Save Data";
            this.btnSaveImageData.UseVisualStyleBackColor = true;
            this.btnSaveImageData.Click += new System.EventHandler(this.btnSaveImageData_Click);
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.ClientSize = new System.Drawing.Size(924, 1050);
            this.Controls.Add(this.btnSaveImageData);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstObjs);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.lstImages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(15, 15);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControlPanel";
            this.ShowIcon = false;
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ControlPanel_KeyDown);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnSaveImageData;

        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.ListBox lstObjs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

        private System.Windows.Forms.ListBox lstImages;
        private System.Windows.Forms.Button btnLoadImage;

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}