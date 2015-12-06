using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PreviewIo
{
	internal class Drawing
	{
		private readonly Stream _drawingContent;

		public Drawing(Stream drawingContent)
		{
			_drawingContent = drawingContent;
		}

		public async Task<Size?> GetSize(ISizeExtractor sizeExtractor, CancellationToken token)
		{
			return await sizeExtractor.ExtractSize(_drawingContent, token);
		}

		public async Task<Stream> GeneratePreview(IPreviewGenerator generator, Size previewSize, CancellationToken token)
		{
			return await generator.GeneratePreview(_drawingContent, previewSize, token);
		}
	}
}
