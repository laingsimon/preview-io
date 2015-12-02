using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("Ruby Preview Handler", ".rb;.rhtml;.rjs", "{a90c9665-e24e-11db-9706-00e08161165f}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.RubyPreviewHandler")]
	[Guid("705747ba-e24e-11db-9706-00e08161165f")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class RubyPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("Ruby");
		}
	}
}