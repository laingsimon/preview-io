using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PreviewIo.UnitTests
{
	[TestFixture]
	public class IntegrationTests
	{
		[Test]
		public async Task ShouldBeAbleToGenerateAPreview()
		{
			var settings = new PreviewSettings
			{
				RenderingFormat = ImageFormat.Png,
				Resolution = new Size(100, 100)
			};

			using (var fileStream = File.OpenRead(@"..\..\..\Sample file.xml"))
			{
				var drawing = new Drawing(fileStream);
				var previewGeneratorFactory = new HttpPreviewGeneratorFactory(settings);
				var generator = previewGeneratorFactory.Create();

				var preview = await drawing.GeneratePreview(generator, CancellationToken.None);

				var data = new byte[preview.Length];
				preview.Read(data, 0, data.Length);
				Assert.That(data.Length, Is.GreaterThan(1)); //more than 1 byte in the response!
			}
		}
	}
}
