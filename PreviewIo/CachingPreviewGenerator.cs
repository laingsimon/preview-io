using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PreviewIo
{
	internal class CachingPreviewGenerator : IPreviewGenerator
	{
		private readonly IPreviewGenerator _underlyingGenerator;
		private static readonly Dictionary<FileDetail, MemoryStream> _cache = new Dictionary<FileDetail, MemoryStream>();

		public CachingPreviewGenerator(IPreviewGenerator underlyingGenerator)
		{
			_underlyingGenerator = underlyingGenerator;
		}

		public async Task<Stream> GeneratePreview(Stream drawingContent, FileDetail fileDetail, Size previewSize, CancellationToken token)
		{
			if (_cache.ContainsKey(fileDetail))
				return _CopyStream(_cache[fileDetail]);

			var preview = await _underlyingGenerator.GeneratePreview(drawingContent, fileDetail, previewSize, token);
			var cachablePreview = _CopyStream(preview);
			_cache.Add(fileDetail, cachablePreview);

			return _CopyStream(cachablePreview);
		}

		private static MemoryStream _CopyStream(Stream source)
		{
			var copy = new MemoryStream();
			if (source.CanSeek)
				source.Position = 0;
			source.CopyTo(copy);

			copy.Position = 0;
			return copy;
		}

		public static void EvictFromCache(FileDetail fileDetail)
		{
			_cache.Remove(fileDetail);
		}
	}
}
