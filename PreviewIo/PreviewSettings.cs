using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace PreviewIo
{
	internal class PreviewSettings
	{
		public Size Resolution { get; set; }
		public ImageFormat RenderingFormat { get; set; }

		public PreviewSettings()
		{
			Resolution = _ReadSize(ConfigurationManager.AppSettings["size"], new Size(1000, 1000));
			RenderingFormat = _ReadImageFormat(ConfigurationManager.AppSettings["format"], ImageFormat.Png);
		}

		private static ImageFormat _ReadImageFormat(string value, ImageFormat defaultFormat)
		{
			if (string.IsNullOrEmpty(value))
				return defaultFormat;

			var property = typeof(ImageFormat).GetProperty(value, BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.IgnoreCase);
			if (property == null)
				return defaultFormat;

			return (ImageFormat)property.GetValue(null);
		}

		private static Size _ReadSize(string value, Size defaultSize)
		{
			if (string.IsNullOrEmpty(value))
				return defaultSize;

			int scale;
			if (!int.TryParse(value, out scale))
				return defaultSize;

			return new Size(scale, scale);
		}
	}
}
