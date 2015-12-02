﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace PreviewIo
{
	internal class HttpPreviewGeneratorFactory
	{
		private readonly Uri _previewUrl;
		private readonly Uri _proxyUrl;
		private readonly PreviewSettings _settings;

		public HttpPreviewGeneratorFactory(
			PreviewSettings settings,
			Uri proxyUrl = null,
			Uri previewUrl = null)
		{
			_settings = settings;
			_previewUrl = previewUrl ?? new Uri("https://exp.draw.io/ImageExport4/export", UriKind.Absolute);
			_proxyUrl = proxyUrl ?? _GetDefaultProxy(_previewUrl);
		}

		private static Uri _GetDefaultProxy(Uri previewUrl)
		{
			var proxy = WebRequest.DefaultWebProxy.GetProxy(previewUrl);
			if (proxy == previewUrl)
				return null;

			Trace.TraceInformation("Using proxy: " + proxy);
			return proxy;
		}

		public IPreviewGenerator Create()
		{
			return new HttpPreviewGenerator(
				_settings,
				new HttpClient(new HttpClientHandler
				{
					Proxy = _GetProxy(),
					UseProxy = _proxyUrl != null
				}),
				_previewUrl);
		}

		private WebProxy _GetProxy()
		{
			if (_proxyUrl == null)
				return null;

			return new WebProxy(_proxyUrl)
			{
				UseDefaultCredentials = true
			};
		}
	}
}