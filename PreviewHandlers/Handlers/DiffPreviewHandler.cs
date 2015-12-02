using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("Diff Preview Handler", ".diff;.patch", "{ac48f4e7-e3eb-11db-9706-00e08161165f}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.DiffPreviewHandler")]
	[Guid("a4b69d14-e3eb-11db-9706-00e08161165f")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class DiffPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("Diff");
		}
	}
}