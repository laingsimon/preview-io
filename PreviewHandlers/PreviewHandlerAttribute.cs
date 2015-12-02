using System;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class PreviewHandlerAttribute : Attribute
	{
	    readonly string name;
	    readonly string extension;
	    readonly string appId;

		public PreviewHandlerAttribute(string name, string extension, string appId)
		{
			if (name == null) throw new ArgumentNullException("name");
			if (extension == null) throw new ArgumentNullException("extension");
			if (appId == null) throw new ArgumentNullException("appId");
			this.name = name;
			this.extension = extension;
			this.appId = appId;
		}

		public string Name { get { return name; } }
		public string Extension { get { return extension; } }
		public string AppId { get { return appId; } }
	}
}