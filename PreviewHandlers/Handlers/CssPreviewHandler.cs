using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("CSS Code Preview Handler", ".css", "{21a72198-e3df-11db-9706-00e08161165f}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.CssPreviewHandler")]
	[Guid("198ce3bc-e3df-11db-9706-00e08161165f")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class CssPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("CSS");
		}
	}
}