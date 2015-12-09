using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace PreviewIo
{
	internal class PreviewContext
	{
		public event EventHandler ViewPortChanged;
		public event EventHandler PreviewRequired;

		public Rectangle ViewPort { get; private set; }
		public bool DisplayPreview { get; private set; }
		public Stream FileStream { get; private set; }
		public PreviewSettings Settings { get; }
		public CancellationTokenSource TokenSource { get; private set; }
		public Size? DrawingSize { get; set; }

		public PreviewContext()
		{
			TokenSource = new CancellationTokenSource();
			Settings = new PreviewSettings
			{
				RenderingFormat = ImageFormat.Png,
				UpScaleForPrint = 4,
			};
		}

		public void OnViewPortChanged(Rectangle newSize)
		{
			ViewPort = newSize;

			ViewPortChanged?.Invoke(this, EventArgs.Empty);
		}

		public void OnPreviewRequired(Stream stream)
		{
			TokenSource.Cancel();
			TokenSource = new CancellationTokenSource();
			FileStream = stream;
			DisplayPreview = true;
			PreviewRequired?.Invoke(this, EventArgs.Empty);
		}

		public Size GetPreviewSize()
		{
			return _IncreaseSizeForPrint(DrawingSize) ?? ViewPort.Size;
		}

		private Size? _IncreaseSizeForPrint(Size? drawingSize)
		{
			if (drawingSize == null)
				return null;

			var size = drawingSize.Value;
			return new Size(size.Width * Settings.UpScaleForPrint, size.Height * Settings.UpScaleForPrint);
		}

	}
}
