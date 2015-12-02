using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("VB.NET Code Preview Handler", ".vb", "{c6445490-8ddc-11db-96b6-005056c00008}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.VbPreviewHandler")]
	[Guid("d4b89630-8ddc-11db-96b6-005056c00008")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class VbPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("VB.NET");
		}
	}
}