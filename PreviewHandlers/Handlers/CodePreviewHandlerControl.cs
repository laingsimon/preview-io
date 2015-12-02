using System.IO;
using System.Windows.Forms;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers.Handlers
{
    public class CodePreviewHandlerControl : StreamBasedPreviewHandlerControl
    {
        readonly string definition = string.Empty;

        public CodePreviewHandlerControl(string definition)
        {
            this.definition = definition;
        }

        public override void Load(Stream stream)
        {
            var sourceHtml = HighlightHelpers.GetHighlightedHtml(definition, stream);

            var webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                DocumentText = sourceHtml
            };
            Controls.Add(webBrowser);
        }
    }
}