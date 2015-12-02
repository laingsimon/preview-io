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

		public async Task<Stream> GeneratePreview(IPreviewGenerator generator, CancellationToken token)
		{
			return await generator.GeneratePreview(_drawingContent, token);
		}
	}
}
