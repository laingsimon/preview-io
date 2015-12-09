using System;
using System.Drawing;
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
		public FileDetail FileDetail { get; private set; }

		public PreviewContext()
		{
			TokenSource = new CancellationTokenSource();
			Settings = new PreviewSettings();
		}

		public void OnViewPortChanged(Rectangle newSize)
		{
			ViewPort = newSize;

			ViewPortChanged?.Invoke(this, EventArgs.Empty);
		}

		public void OnPreviewRequired(Stream stream, FileDetail fileDetail)
		{
			TokenSource.Cancel();
			TokenSource = new CancellationTokenSource();
			FileStream = stream;
			FileDetail = fileDetail;
			DisplayPreview = true;
			PreviewRequired?.Invoke(this, EventArgs.Empty);
		}

		public Size GetPreviewSize()
		{
			return _IncreaseSizeForPrint(DrawingSize) ?? Size.Empty;
		}

		private Size? _IncreaseSizeForPrint(Size? drawingSize)
		{
			if (drawingSize == null)
				return null;

			var size = drawingSize.Value;
			return new Size(size.Width * Settings.UpScaleForPrint, size.Height * Settings.UpScaleForPrint);
		}

		public void RecalculateDrawingSize(Size upscaledPreviewSize)
		{
			var actualSize = new Size(
				upscaledPreviewSize.Width / Settings.UpScaleForPrint,
				upscaledPreviewSize.Height / Settings.UpScaleForPrint);

			var previousDrawingSize = DrawingSize;
			DrawingSize = actualSize;

			if (previousDrawingSize == null)
				return;

			//work out the actual scale of the preview compared to the requested size
			var scale = new SizeF(
				((float)actualSize.Width / previousDrawingSize.Value.Width) * Settings.UpScaleForPrint,
				((float)actualSize.Height / previousDrawingSize.Value.Height) * Settings.UpScaleForPrint);

			var mostAppropriateScale = _GetMostAppropriateScale(scale);

			//reset the drawing size to that of the preview
			DrawingSize = new Size(
				upscaledPreviewSize.Width / (int)mostAppropriateScale,
				upscaledPreviewSize.Height / (int)mostAppropriateScale);
		}

		private float _GetMostAppropriateScale(SizeF scale)
		{
			var widthScale = Math.Abs(Settings.UpScaleForPrint - scale.Width);
			var heightScale = Math.Abs(Settings.UpScaleForPrint - scale.Height);

			if (widthScale < heightScale)
				return scale.Height;

			return scale.Width;
		}
	}
}
