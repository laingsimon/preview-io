using System.IO;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers
{
	public abstract class StreamBasedPreviewHandlerControl : PreviewHandlerControl
	{
		public sealed override void Load(FileInfo file)
		{
			using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
				Load(fs);
		}
	}
}