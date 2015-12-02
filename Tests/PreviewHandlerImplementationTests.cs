using System;
using System.Runtime.InteropServices;

using FuelAdvance.PreviewHandlerPack.PreviewHandlers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests
{
	[TestClass]
	public class PreviewHandlerImplementationTests
	{
		[TestMethod]
		public void ValidateGuids()
		{
			string results = null;

			List<Guid> usedGuids = new List<Guid>();

			foreach (Type t in typeof(PreviewHandler).Assembly.GetTypes())
			{
				if (typeof(PreviewHandler).IsAssignableFrom(t))
				{
					GuidAttribute[] guidAttributes = (GuidAttribute[])t.GetCustomAttributes(typeof(GuidAttribute), true);
					foreach(GuidAttribute guidAttribute in guidAttributes)
					{
						Guid guid = new Guid(guidAttribute.Value);

						if (usedGuids.Contains(guid))
							results += string.Format("The type {0} shares the GUID {1} with another concrete class.\r\n", t.FullName, guidAttribute.Value);

						usedGuids.Add(guid);
					}

					PreviewHandlerAttribute[] previewHandlerAttributes = (PreviewHandlerAttribute[])t.GetCustomAttributes(typeof(PreviewHandlerAttribute), true);
					foreach(PreviewHandlerAttribute previewHandlerAttribute in previewHandlerAttributes)
					{
						Guid guid = new Guid(previewHandlerAttribute.AppId);

						if (usedGuids.Contains(guid))
							results += string.Format("The type {0} shares the GUID {1} with another concrete class.\r\n", t.FullName, previewHandlerAttribute.AppId);

						usedGuids.Add(guid);
					}
				}
			}

			if (results != null)
			{
				Assert.Fail(results.Trim());
			}
		}
	}
}