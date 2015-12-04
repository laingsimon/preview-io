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
			this.picPreview = new System.Windows.Forms.PictureBox();
			this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.itmPrint = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
			this.mnuContext.SuspendLayout();
			this.SuspendLayout();
			// 
			// picPreview
			// 
			this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picPreview.Location = new System.Drawing.Point(0, 0);
			this.picPreview.Name = "picPreview";
			this.picPreview.Size = new System.Drawing.Size(150, 150);
			this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picPreview.TabIndex = 0;
			this.picPreview.TabStop = false;
			// 
			// mnuContext
			// 
			this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmPrint});
			this.mnuContext.Name = "mnuContext";
			this.mnuContext.Size = new System.Drawing.Size(100, 26);
			// 
			// itmPrint
			// 
			this.itmPrint.Name = "itmPrint";
			this.itmPrint.Size = new System.Drawing.Size(99, 22);
			this.itmPrint.Text = "Print";
			this.itmPrint.Click += new System.EventHandler(this.itmPrint_Click);
			// 
			// PreviewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ContextMenuStrip = this.mnuContext;
			this.Controls.Add(this.picPreview);
			this.Name = "PreviewControl";
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
			this.mnuContext.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picPreview;
		private System.Windows.Forms.ContextMenuStrip mnuContext;
		private System.Windows.Forms.ToolStripMenuItem itmPrint;
	}
}
