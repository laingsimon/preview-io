using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PreviewIo
{
	internal class Drawing
	{
		private readonly Stream _drawingContent;
		private readonly FileDetail _fileDetail;

		public Drawing(Stream drawingContent, FileDetail fileDetail)
		{
			_drawingContent = drawingContent;
			_fileDetail = fileDetail;
		}

		public async Task<Size?> GetSize(ISizeExtractor sizeExtractor, CancellationToken token)
		{
			return await sizeExtractor.ExtractSize(_drawingContent, _fileDetail, token);
		}

		public async Task<Stream> GeneratePreview(IPreviewGenerator generator, Size previewSize, CancellationToken token)
		{
			return await generator.GeneratePreview(_drawingContent, _fileDetail, previewSize, token);
		}
	}
}
