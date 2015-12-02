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

		public PreviewContext()
		{
			TokenSource = new CancellationTokenSource();
			Settings = new PreviewSettings
			{
				RenderingFormat = ImageFormat.Png,
				Resolution = new Size(1000, 1000)
			};
		}

		public void OnViewPortChanged(Rectangle newSize)
		{
			ViewPort = newSize;
			Settings.Resolution = new Size(newSize.Width * 2, newSize.Height * 2);

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
	}
}
