﻿using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PreviewIo
{
	internal interface IPreviewGenerator
	{
		Task<Stream> GeneratePreview(Stream drawingContent, FileDetail fileDetail, Size previewSize, CancellationToken token);
	}
}
