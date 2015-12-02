using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("DOS Preview Handler", ".bat;.cmd", "{004b9672-e3e5-11db-9706-00e08161165f}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.DosPreviewHandler")]
	[Guid("f4ec7f30-e3e4-11db-9706-00e08161165f")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class DosPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("DOS");
		}
	}
}