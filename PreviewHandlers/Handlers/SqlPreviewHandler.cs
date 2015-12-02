using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("SQL Code Preview Handler", ".sql", "{5b977a6a-8de0-11db-96b6-005056c00008}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.SqlPreviewHandler")]
	[Guid("303ab58a-8de0-11db-96b6-005056c00008")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class SqlPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("SQL");
		}
	}
}