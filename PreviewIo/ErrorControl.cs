using System;
using System.Windows.Forms;

namespace PreviewIo
{
	internal partial class ErrorControl : UserControl
	{
		public ErrorControl(Exception exception)
		{
			InitializeComponent();
			txtMessage.Text = exception.ToString();

			txtMessage.Text += "\r\n\r\nLog:\r\n" + Logging.ReadLog();
		}
	}
}
