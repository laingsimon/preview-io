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
		private readonly PreviewContext _context;
		private readonly Image _originalPreview;

		public PreviewControl(Image preview, PreviewContext context)
		{
			_originalPreview = preview;
			_context = context;
			InitializeComponent();
			picPreview.Image = _ResizePreviewImageToActualSize(preview, context.DrawingSize);
			picPreview.Size = preview.Size;
		}

		private static Image _ResizePreviewImageToActualSize(Image preview, Size? drawingSize)
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
		private void itmCentreImage_Click(object sender, EventArgs e)
			// ReSharper restore InconsistentNaming
		{
			itmCentreImage.Checked = !itmCentreImage.Checked;

			pnlScroller.AutoScrollMinSize = itmCentreImage.Checked
				? Size.Empty
				: picPreview.Image.Size;

			pnlScroller.AutoScroll = !itmCentreImage.Checked;
		}
	}
}
