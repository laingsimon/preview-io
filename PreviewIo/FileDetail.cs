using System;

namespace PreviewIo
{
	public class FileDetail
	{
		public string FileName { get; }
		public DateTime LastModified { get; }

		public FileDetail(string fileName, DateTime lastModified)
		{
			FileName = fileName;
			LastModified = lastModified;
		}

		public override int GetHashCode()
		{
			return FileName.GetHashCode() ^ LastModified.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var other = obj as FileDetail;
			if (other == null)
				return false;

			return FileName == other.FileName
				&& LastModified == other.LastModified;
		}
	}
}
