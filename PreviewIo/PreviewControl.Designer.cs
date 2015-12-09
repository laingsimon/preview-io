namespace PreviewIo
{
	partial class PreviewControl
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
			this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.itmPrint = new System.Windows.Forms.ToolStripMenuItem();
			this.itmCentreImage = new System.Windows.Forms.ToolStripMenuItem();
			this.picPreview = new System.Windows.Forms.PictureBox();
			this.pnlScroller = new System.Windows.Forms.Panel();
			this.mnuContext.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
			this.pnlScroller.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuContext
			// 
			this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmPrint,
            this.itmCentreImage});
			this.mnuContext.Name = "mnuContext";
			this.mnuContext.Size = new System.Drawing.Size(146, 48);
			// 
			// itmPrint
			// 
			this.itmPrint.Name = "itmPrint";
			this.itmPrint.Size = new System.Drawing.Size(145, 22);
			this.itmPrint.Text = "Print";
			this.itmPrint.Click += new System.EventHandler(this.itmPrint_Click);
			// 
			// itmCentreImage
			// 
			this.itmCentreImage.Checked = true;
			this.itmCentreImage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.itmCentreImage.Name = "itmCentreImage";
			this.itmCentreImage.Size = new System.Drawing.Size(145, 22);
			this.itmCentreImage.Text = "Centre Image";
			this.itmCentreImage.Click += new System.EventHandler(this.itmCentreImage_Click);
			// 
			// picPreview
			// 
			this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picPreview.Location = new System.Drawing.Point(0, 0);
			this.picPreview.Margin = new System.Windows.Forms.Padding(0);
			this.picPreview.Name = "picPreview";
			this.picPreview.Size = new System.Drawing.Size(150, 150);
			this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPreview.TabIndex = 1;
			this.picPreview.TabStop = false;
			// 
			// pnlScroller
			// 
			this.pnlScroller.AutoScroll = true;
			this.pnlScroller.Controls.Add(this.picPreview);
			this.pnlScroller.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlScroller.Location = new System.Drawing.Point(0, 0);
			this.pnlScroller.Name = "pnlScroller";
			this.pnlScroller.Size = new System.Drawing.Size(150, 150);
			this.pnlScroller.TabIndex = 2;
			// 
			// PreviewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.mnuContext;
			this.Controls.Add(this.pnlScroller);
			this.Name = "PreviewControl";
			this.mnuContext.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
			this.pnlScroller.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip mnuContext;
		private System.Windows.Forms.ToolStripMenuItem itmPrint;
		private System.Windows.Forms.PictureBox picPreview;
		private System.Windows.Forms.Panel pnlScroller;
		private System.Windows.Forms.ToolStripMenuItem itmCentreImage;
	}
}
