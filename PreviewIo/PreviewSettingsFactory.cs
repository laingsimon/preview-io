using System.Drawing;
using System.Drawing.Imaging;

namespace PreviewIo
{
	internal class PreviewSettingsFactory
	{
		public PreviewSettings CreateSettings()
		{
			//TODO: Read from configuration file

			return new PreviewSettings
			{
				Resolution = new Size(1000, 1000),
				RenderingFormat = ImageFormat.Png
			};
		}
	}
}
