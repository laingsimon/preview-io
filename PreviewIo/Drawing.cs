﻿using System;
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
			if (drawingContent == null)
				throw new ArgumentNullException("drawingContent");
			if (fileDetail == null)
				throw new ArgumentNullException("fileDetail");

			_drawingContent = drawingContent;
			_fileDetail = fileDetail;
		}

		public async Task<Size?> GetSize(ISizeExtractor sizeExtractor, CancellationToken token)
		{
			if (sizeExtractor == null)
				throw new ArgumentNullException("sizeExtractor");

			return await sizeExtractor.ExtractSize(_drawingContent, _fileDetail, token);
		}

		public async Task<Stream> GeneratePreview(IPreviewGenerator generator, Size previewSize, CancellationToken token)
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (previewSize.Width <= 0 || previewSize.Height <= 0)
				throw new ArgumentException("Preview must have a size");

			return await generator.GeneratePreview(_drawingContent, _fileDetail, previewSize, token);
		}
	}
}
