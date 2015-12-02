using System.Runtime.InteropServices;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
    [PreviewHandler("JavaScript Code Preview Handler", ".js;.as", "{1e25f416-e251-11db-9706-00e08161165f}")]
    [ProgId("FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers.JavaScriptPreviewHandler")]
    [Guid("1677000c-e251-11db-9706-00e08161165f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class JavaScriptPreviewHandler : StreamBasedPreviewHandler
    {
        protected override PreviewHandlerControl CreatePreviewHandlerControl()
        {
            return new CodePreviewHandlerControl("JavaScript");
        }
    }
}