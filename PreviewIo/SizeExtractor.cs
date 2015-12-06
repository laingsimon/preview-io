using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace PreviewIo
{
	public interface ISizeExtractor
	{
		Task<Size> ExtractSize(Stream fileContent, CancellationToken token);
	}

	public class SizeExtractor : ISizeExtractor
	{
		public async Task<Size> ExtractSize(Stream fileContent, CancellationToken token)
		{
			return await Task.Factory.StartNew<Size>(() =>
			{
				try
				{
					fileContent.Position = 0;
					var reader = new StreamReader(fileContent);
					var document = XDocument.Load(reader);

					return _ReadSizeFromDocument(document);
				}
				finally
				{
					fileContent.Position = 0;
				}
			});
		}

		private Size _ReadSizeFromDocument(XDocument document)
		{
			var rootNode = document.Root;
			var rootNodeName = rootNode.Name.LocalName;
			if (!rootNodeName.Equals("mxGraphModel"))
				rootNode = _UnCompressDocument(rootNode.Element("diagram"));

			var dx = rootNode.Attribute("dx").Value;
			var dy = rootNode.Attribute("dy").Value;

			return new Size(int.Parse(dx), int.Parse(dy));
		}

		private XElement _UnCompressDocument(XElement diagram)
		{
			var compressedData = Convert.FromBase64String(diagram.Value);
			var compressedDataStream = new MemoryStream(compressedData);
			var zipStream = new DeflateStream(compressedDataStream, CompressionMode.Decompress);

			using (var reader = new StreamReader(zipStream))
			{
				var urlEncodedXml = reader.ReadToEnd();
				var xml = HttpUtility.UrlDecode(urlEncodedXml);
				return XElement.Parse(xml);
			}
		}
	}
}
