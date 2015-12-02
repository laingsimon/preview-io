using System;
using System.Drawing;
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

			FormBorderStyle = FormBorderStyle.None;
			BackColor = SystemColors.Window;
			DoubleBuffered = true;
		}

		private void _ViewPortChanged(object sender, EventArgs e)
		{
			_InvokeOnUiThread(delegate
			{
				_UpdateSize();
			});
		}

		private void _PreviewRequired(object sender, EventArgs e)
		{
			if (!_context.DisplayPreview)
				return;

			_InvokeOnUiThread(delegate
			{
				_UpdatePreview();
			});
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
				Reset();

				var drawing = new Drawing(_context.FileStream);
				var previewGeneratorFactory = new HttpPreviewGeneratorFactory(_context.Settings);
				var generator = previewGeneratorFactory.Create();

				var preview = await drawing.GeneratePreview(generator, _context.TokenSource.Token);

				_ReplaceControl(new PreviewControl(Image.FromStream(preview), _context));
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
			_InvokeOnUiThread(delegate
			{
				_Reset();
			});
		}

		public void _Reset()
		{
			_ReplaceControl(new LoadingControl());
		}

		private void _ReplaceControl(UserControl control)
		{
			Controls.Clear();
			Controls.Add(control);
			control.Dock = DockStyle.Fill;
		}
	}
}
