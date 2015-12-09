using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PreviewIo
{
	internal partial class PreviewControl : UserControl
	{
		private const float _maxZoom = 2f;
		private const float _minZoom = 0.3f;

		private readonly PreviewContext _context;
		private readonly Image _originalPreview;
		private float? _currentZoom;
		private bool _ctrlPressed;
		private bool _shouldPan;
		private Point _panOrigin;
		private Point _scrollOrigin;

		public PreviewControl(Image preview, PreviewContext context)
		{
			_originalPreview = preview;
			_context = context;
			InitializeComponent();
			picPreview.Image = _ResizePreviewImageToSize(preview, context.DrawingSize);
			picPreview.Size = preview.Size;

			itmZoomIn.Enabled = context.DrawingSize != null;
			itmZoomOut.Enabled = context.DrawingSize != null;
			_UpdateDrawingDetails();
		}

		private Image _ResizePreviewImageToZoom(Image preview, float zoom)
		{
			var drawingSize = _context.DrawingSize.Value;
			var newSize = new Size((int)(drawingSize.Width * zoom), (int)(drawingSize.Height * zoom));
			return _ResizePreviewImageToSize(preview, newSize);
		}

		private static Image _ResizePreviewImageToSize(Image preview, Size? drawingSize)
		{
			if (drawingSize == null)
				return preview;

			var newSize = drawingSize.Value;
			var image = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppArgb);
			using (var graphics = Graphics.FromImage(image))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.CompositingQuality = CompositingQuality.HighQuality;

				graphics.DrawImage(preview, new Rectangle(Point.Empty, newSize));
			}

			return image;
		}

		// ReSharper disable InconsistentNaming
		private void itmPrint_Click(object sender, EventArgs e)
		// ReSharper restore InconsistentNaming
		{
			try
			{
				var tempFile = _CreateTempFile();

				_originalPreview.Save(tempFile, ImageFormat.Png);

				var process = Process.Start(new ProcessStartInfo
				{
					FileName = tempFile,
					UseShellExecute = true,
					Verb = "print"
				});
				process.EnableRaisingEvents = true;
				process.Exited += (s, args) => _DeleteTempFile(tempFile);
			}
			catch (Exception exc)
			{
				if (ParentForm == null)
					throw;

				var parentForm = ParentForm; //we have to store a reference to the ParentForm as it will be removed when this control is remove from it (by Controls.Clear())
				parentForm.Controls.Clear();
				parentForm.Controls.Add(new ErrorControl(exc)
				{
					Dock = DockStyle.Fill
				});
			}
		}

		private static string _CreateTempFile()
		{
			try
			{
				return Path.ChangeExtension(Path.GetTempFileName(), ".png");
			}
			catch (Exception exc)
			{
				throw new IOException("Could not create temporary file for printing - " + exc.Message);
			}
		}

		private static void _DeleteTempFile(string filePath)
		{
			try
			{
				File.Delete(filePath);
			}
			// ReSharper disable EmptyGeneralCatchClause
			catch { }
			// ReSharper restore EmptyGeneralCatchClause
		}

		// ReSharper disable InconsistentNaming
		private void itmFitImage_Click(object sender, EventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (_currentZoom != null)
				picPreview.Image = _ResizePreviewImageToSize(_originalPreview, _context.DrawingSize);

			itmFitImage.Checked = !itmFitImage.Checked;

			pnlScroller.AutoScrollMinSize = itmFitImage.Checked
				? Size.Empty
				: picPreview.Image.Size;

			pnlScroller.AutoScroll = !itmFitImage.Checked;
			picPreview.SizeMode = itmFitImage.Checked
				? PictureBoxSizeMode.Zoom
				: PictureBoxSizeMode.CenterImage;
			_currentZoom = null;
			picPreview_MouseUp(null, null);
			_UpdateDrawingDetails();
		}

		// ReSharper disable InconsistentNaming
		private void itmZoomIn_Click(object sender, EventArgs e)
		// ReSharper restore InconsistentNaming
		{
			_currentZoom = (_currentZoom ?? 1f) + 0.1f;
			_UpdateZoom();
		}

		// ReSharper disable InconsistentNaming
		private void itmZoomOut_Click(object sender, EventArgs e)
		// ReSharper restore InconsistentNaming
		{
			_currentZoom = (_currentZoom ?? 1f) - 0.1f;
			_UpdateZoom();
		}

		private void _UpdateZoom()
		{
			picPreview.Image = _ResizePreviewImageToZoom(_originalPreview, _currentZoom.Value);
			itmZoomIn.Enabled = _currentZoom.Value <= _maxZoom;
			itmZoomOut.Enabled = _currentZoom.Value >= _minZoom;
			itmFitImage.Checked = false;
			pnlScroller.AutoScrollMinSize = picPreview.Image.Size;
			pnlScroller.AutoScroll = true;
			picPreview.SizeMode = PictureBoxSizeMode.CenterImage;
			_UpdateDrawingDetails();
		}

		// ReSharper disable InconsistentNaming
		private void PreviewControl_KeyDown(object sender, KeyEventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (e.Control)
				_ctrlPressed = true;
		}

		// ReSharper disable InconsistentNaming
		private void PreviewControl_KeyUp(object sender, KeyEventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (e.Control)
				_ctrlPressed = false;
		}

		// ReSharper disable InconsistentNaming
		private void PreviewControl_Scroll(object sender, ScrollEventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (!_ctrlPressed)
				return;

			float? zoomAdjustment = null;

			switch (e.Type)
			{
				case ScrollEventType.LargeIncrement:
					zoomAdjustment = 0.2f;
					break;
				case ScrollEventType.LargeDecrement:
					zoomAdjustment = -0.2f;
					break;
				case ScrollEventType.SmallIncrement:
					zoomAdjustment = 0.1f;
					break;
				case ScrollEventType.SmallDecrement:
					zoomAdjustment = -0.1f;
					break;
			}

			if (zoomAdjustment == null)
				return;

			var newZoomStart = (_currentZoom + 0.1f) ?? 1f;
			var newZoom = newZoomStart + zoomAdjustment.Value;

			_currentZoom = Math.Min(Math.Max(newZoom, _minZoom), _maxZoom);
			_UpdateZoom();
		}

		private void _UpdateDrawingDetails()
		{
			var zoom = _currentZoom.HasValue
				? string.Format(" (x{0:n0}%)", _currentZoom.Value * 100)
				: "";

			itmDrawingDetails.Text = string.Format(
				"{0} x {1}{2}",
				_originalPreview.Size.Width,
				_originalPreview.Size.Height,
				zoom);
		}

		// ReSharper disable InconsistentNaming
		private void picPreview_MouseDown(object sender, MouseEventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (!pnlScroller.AutoScroll || e.Button != MouseButtons.Left)
				return;

			picPreview.Cursor = Cursors.SizeAll;
			_shouldPan = true;
			_panOrigin = new Point(
				e.Location.X + pnlScroller.AutoScrollPosition.X,
				e.Location.Y + pnlScroller.AutoScrollPosition.Y);
			_scrollOrigin = pnlScroller.AutoScrollPosition;
		}

		// ReSharper disable InconsistentNaming
		private void picPreview_MouseUp(object sender, MouseEventArgs e)
		// ReSharper restore InconsistentNaming
		{
			picPreview.Cursor = Cursors.Default;
			_shouldPan = false;
			_panOrigin = Point.Empty;
			_scrollOrigin = Point.Empty;
		}

		// ReSharper disable InconsistentNaming
		private void picPreview_MouseMove(object sender, MouseEventArgs e)
		// ReSharper restore InconsistentNaming
		{
			if (!_shouldPan)
				return;

			var currentLocation = new Point(
				e.Location.X + pnlScroller.AutoScrollPosition.X,
				e.Location.Y + pnlScroller.AutoScrollPosition.Y);

			var movement = new Point(
				currentLocation.X - _panOrigin.X,
				currentLocation.Y - _panOrigin.Y);

			var newScroll = new Point(
				0 - _scrollOrigin.X - movement.X,
				0 - _scrollOrigin.Y - movement.Y);

			pnlScroller.AutoScrollPosition = newScroll;
		}
	}
}
