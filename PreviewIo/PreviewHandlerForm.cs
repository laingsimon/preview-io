﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PreviewIo
{
	internal class PreviewHandlerForm : Form
	{
		private readonly PreviewContext _context;

		public PreviewHandlerForm(PreviewContext context)
		{
			_context = context;
			context.PreviewRequired += _PreviewRequired;
			context.ViewPortChanged += _ViewPortChanged;

			InitializeComponent();
		}

		private void _ViewPortChanged(object sender, EventArgs e)
		{
			_InvokeOnUiThread(_UpdateSize);
		}

		private void _PreviewRequired(object sender, EventArgs e)
		{
			if (!_context.DisplayPreview)
				return;

			_InvokeOnUiThread(_UpdatePreview);
		}

		private void _InvokeOnUiThread(MethodInvoker method)
		{
			if (Created)
				Invoke(method);
		}

		private void _UpdateSize()
		{
			Bounds = _context.ViewPort;
		}

		private async void _UpdatePreview()
		{
			try
			{
				_UpdateSize();
				_Reset();

				if (!_context.FileStream.IsDrawing())
				{
					_ReplaceControl(new XmlControl(_context.FileStream.ReadAsString()));
					return;
				}

				var drawing = new Drawing(_context.FileStream);
				var previewGeneratorFactory = new HttpPreviewGeneratorFactory(_context.Settings);
				var generator = previewGeneratorFactory.Create();

				_context.DrawingSize = await drawing.GetSize(new SizeExtractor(), _context.TokenSource.Token);
				var previewSize = _context.GetPreviewSize();
				var preview = await drawing.GeneratePreview(generator, previewSize, _context.TokenSource.Token);

				try
				{
					var image = Image.FromStream(preview);
					_ReplaceControl(new PreviewControl(image, _context));
				}
				catch (Exception exc)
				{
					using (var reader = new StreamReader(preview))
					{
						var responseBody = reader.ReadToEnd();
						throw new InvalidOperationException("Invalid image data returned: " + responseBody, exc);
					}
				}
			}
			catch (OperationCanceledException)
			{ }
			catch (Exception exc)
			{
				_ReplaceControl(new ErrorControl(exc));
			}
		}

		public void Reset()
		{
			_InvokeOnUiThread(_Reset);
		}

		private void _Reset()
		{
			_ReplaceControl(new LoadingControl());
		}

		private void _ReplaceControl(UserControl control)
		{
			Controls.Clear();
			Controls.Add(control);
			control.Dock = DockStyle.Fill;
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			//
			// PreviewHandlerForm
			//
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.Name = "PreviewHandlerForm";
			this.ResumeLayout(false);

		}
	}
}
