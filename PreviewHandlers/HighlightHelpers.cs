using System.IO;
using Wayloop.Highlight;
using Wayloop.Highlight.Engines;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers
{
    public static class HighlightHelpers
    {
        public static string GetHighlightedHtml(string definition, Stream stream)
        {
            //Read the source code in
            string sourceText;
            using (var reader = new StreamReader(stream))
                sourceText = reader.ReadToEnd();

            return GetHighlightedHtml(definition, sourceText);
        }

        public static string GetHighlightedHtml(string definition, string source)
        {
            var engine = new HtmlEngine();
            var highlighter = new Highlighter(engine);
            var sourceHtml = highlighter.Highlight(definition, source);

            sourceHtml = string.Format("<pre>{0}</pre>", sourceHtml);

            return sourceHtml;
        }
    }
}