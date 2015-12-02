namespace TestHarness
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.Style = new System.Windows.Forms.TextBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.Input = new System.Windows.Forms.TextBox();
			this.Output = new System.Windows.Forms.WebBrowser();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Style
			// 
			this.Style.Location = new System.Drawing.Point(12, 12);
			this.Style.Name = "Style";
			this.Style.Size = new System.Drawing.Size(141, 20);
			this.Style.TabIndex = 0;
			this.Style.Text = "DOS";
			this.Style.TextChanged += new System.EventHandler(this.Style_TextChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 38);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.Input);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.Output);
			this.splitContainer1.Size = new System.Drawing.Size(502, 400);
			this.splitContainer1.SplitterDistance = 200;
			this.splitContainer1.TabIndex = 1;
			// 
			// Input
			// 
			this.Input.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Input.Location = new System.Drawing.Point(0, 0);
			this.Input.Multiline = true;
			this.Input.Name = "Input";
			this.Input.Size = new System.Drawing.Size(502, 200);
			this.Input.TabIndex = 0;
			this.Input.Text = resources.GetString("Input.Text");
			this.Input.TextChanged += new System.EventHandler(this.Input_TextChanged);
			// 
			// Output
			// 
			this.Output.AllowWebBrowserDrop = false;
			this.Output.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Output.Location = new System.Drawing.Point(0, 0);
			this.Output.MinimumSize = new System.Drawing.Size(20, 20);
			this.Output.Name = "Output";
			this.Output.Size = new System.Drawing.Size(502, 196);
			this.Output.TabIndex = 0;
			this.Output.WebBrowserShortcutsEnabled = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(526, 450);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.Style);
			this.Name = "Form1";
			this.Text = "Form1";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox Style;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox Input;
		private System.Windows.Forms.WebBrowser Output;
	}
}

