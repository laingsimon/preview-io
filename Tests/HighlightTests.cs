using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FuelAdvance.PreviewHandlerPack.PreviewHandlers;

namespace Tests
{
	[TestClass]
	public class HighlightTests
	{
		[TestMethod]
		public void CsHighlightTests()
		{
			//Running this forces the definition XML to be validated
			HighlightHelpers.GetHighlightedHtml("C#", "+");

			Assert.Inconclusive();
		}

		[TestMethod]
		public void CssHighlightTests()
		{
			//Running this forces the definition XML to be validated
			HighlightHelpers.GetHighlightedHtml("CSS", "+");

			Assert.Inconclusive();
		}

		[TestMethod]
		public void DosHighlightTests()
		{
			//Running this forces the definition XML to be validated
			HighlightHelpers.GetHighlightedHtml("DOS", "+");

			Assert.Inconclusive();
		}

		[TestMethod]
		public void RubyHighlightTests()
		{
			//Running this forces the definition XML to be validated
			HighlightHelpers.GetHighlightedHtml("Ruby", "__LINE__");

			Assert.Inconclusive();
		}
	}
}
