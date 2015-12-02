using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("ASPX Code Preview Handler", ".aspx", "{b983fdec-8de0-11db-96b6-005056c00008}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.AspxPreviewHandler")]
	[Guid("c3da7028-8de0-11db-96b6-005056c00008")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class AspxPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("ASPX");
		}
	}
}