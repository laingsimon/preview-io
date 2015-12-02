using System;
using System.Windows.Forms;
using FuelAdvance.PreviewHandlerPack.PreviewHandlers;

namespace TestHarness
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Style_TextChanged(object sender, EventArgs e)
		{
			UpdateView();
		}

		private void Input_TextChanged(object sender, EventArgs e)
		{
			UpdateView();
		}		

		private void UpdateView()
		{
			Output.DocumentText = HighlightHelpers.GetHighlightedHtml(Style.Text, Input.Text);
		}
	}
}