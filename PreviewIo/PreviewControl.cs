using System.Drawing;
using System.Windows.Forms;

namespace PreviewIo
{
	internal partial class PreviewControl : UserControl
	{
		private readonly PreviewContext _context;

		public PreviewControl(Image preview, PreviewContext context)
		{
			_context = context;
			InitializeComponent();
			picPreview.Image = preview;
		}
	}
}
