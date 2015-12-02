using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
	[PreviewHandler("C# Code Preview Handler", ".cs", "{b5dcd61e-8dc7-11db-96b6-005056c00008}")]
	[ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.CsPreviewHandler")]
	[Guid("d474e300-8dc7-11db-96b6-005056c00008")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public sealed class CsPreviewHandler : StreamBasedPreviewHandler
	{
		protected override PreviewHandlerControl CreatePreviewHandlerControl()
		{
			return new CodePreviewHandlerControl("C#");
		}
	}
}