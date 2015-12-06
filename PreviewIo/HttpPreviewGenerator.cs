using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PreviewIo
{
	internal class HttpPreviewGenerator : IPreviewGenerator
	{
		private readonly HttpClient _client;
		private readonly Uri _requestUri;
		private readonly PreviewSettings _settings;

		public HttpPreviewGenerator(PreviewSettings settings, HttpClient client, Uri requestUri)
		{
			_settings = settings;
			_client = client;
			_requestUri = requestUri;
		}

		public async Task<Stream> GeneratePreview(Stream drawingContent, Size previewSize, CancellationToken token)
		{
			var request = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "filename", "preview" },
				{ "format", _settings.RenderingFormat.ToString() },
				{ "xml", _ReadFileContent(drawingContent) },
				{ "base64", "O" },
				{ "bg", "none" },
				{ "w", previewSize.Width.ToString() },
				{ "h", previewSize.Height.ToString() },
				{ "border", "1" }
			});

			var response = await _client.PostAsync(
				_requestUri.ToString(),
				request,
				cancellationToken: token);

			return await response.Content.ReadAsStreamAsync();
		}

		private static string _ReadFileContent(Stream drawingContent)
		{
			using (var reader = new StreamReader(drawingContent))
				return reader.ReadToEnd();
		}
	}
}
