using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PreviewIo
{
	internal partial class PreviewControl : UserControl
	{
		private readonly PreviewContext _context;

		public PreviewControl(Image preview, PreviewContext context)
		{
			_context = context;
			InitializeComponent();
			picPreview.Image = preview;
			picPreview.Size = preview.Size;
		}

		private void itmPrint_Click(object sender, System.EventArgs e)
		{
			try
			{
				var tempFile = Path.ChangeExtension(Path.GetTempFileName(), ".png");
				picPreview.Image.Save(tempFile, ImageFormat.Png);

				var process = Process.Start(new ProcessStartInfo
				{
					FileName = tempFile,
					UseShellExecute = true,
					Verb = "print"
				});
				process.EnableRaisingEvents = true;
				process.Exited += (s, args) => _DeleteTempFile(tempFile);
			}
			catch (System.Exception exc)
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

		private static void _DeleteTempFile(string filePath)
		{
			try
			{
				File.Delete(filePath);
			}
			catch { }
		}

		private void itmCentreImage_Click(object sender, System.EventArgs e)
		{
			itmCentreImage.Checked = !itmCentreImage.Checked;

			pnlScroller.AutoScrollMinSize = itmCentreImage.Checked
				? Size.Empty
				: picPreview.Image.Size;

			pnlScroller.AutoScroll = !itmCentreImage.Checked;
		}
	}
}
