using System.Windows.Forms;
using System.Xml.Linq;

namespace PreviewIo
{
	public partial class XmlControl : UserControl
	{
		private bool _reformat = false;
		private readonly string _xml;

		public XmlControl(string xml, bool initiallyReformatted = false)
		{
			InitializeComponent();
			_reformat = initiallyReformatted;
			_xml = xml;
			_UpdateDisplay();
		}

		private void _UpdateDisplay()
		{
			if (!_reformat)
			{
				txtXml.Text = _xml;
				return;
			}

			var xDocument = XDocument.Parse(_xml);
			txtXml.Text = xDocument.ToString();
		}
	}
}
