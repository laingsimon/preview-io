using System.Drawing;
using System.Drawing.Imaging;

namespace PreviewIo
{
	internal class PreviewSettings
	{
		public Size Resolution { get; set; }
		public ImageFormat RenderingFormat { get; set; }

		public PreviewSettings()
		{
			Resolution = new Size(1000, 1000);
			RenderingFormat = ImageFormat.Png;
		}
	}
}
