using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using FuelAdvance.PreviewHandlerPack.PreviewHandlers.ComInterop;

namespace FuelAdvance.PreviewHandlerPack.PreviewHandlers
{
	public abstract class StreamBasedPreviewHandler : PreviewHandler, IInitializeWithStream
	{
		private IStream stream;
		
		void IInitializeWithStream.Initialize(IStream pstream, uint grfMode)
		{
			stream = pstream;
		}

		protected override void Load(PreviewHandlerControl c)
		{
			c.Load(new ReadOnlyIStreamStream(stream));
		}

		private class ReadOnlyIStreamStream : Stream
		{
			IStream stream;

			public ReadOnlyIStreamStream(IStream stream)
			{
				if (stream == null) throw new ArgumentNullException("stream");
				this.stream = stream;
			}

			protected override void Dispose(bool disposing)
			{
				stream = null;
				base.Dispose(disposing);
			}

			private void ThrowIfDisposed() { if (stream == null) throw new ObjectDisposedException(GetType().Name); }

			public unsafe override int Read(byte[] buffer, int offset, int count)
			{
				ThrowIfDisposed();

				if (buffer == null) throw new ArgumentNullException("buffer");
				if (offset < 0) throw new ArgumentNullException("offset");
				if (count < 0) throw new ArgumentNullException("count");

				var bytesRead = 0;
				if (count > 0)
				{
					var ptr = new IntPtr(&bytesRead);
					if (offset == 0)
					{
						if (count > buffer.Length) throw new ArgumentOutOfRangeException("count");
						stream.Read(buffer, count, ptr);
					}
					else
					{
						var tempBuffer = new byte[count];
						stream.Read(tempBuffer, count, ptr);
						if (bytesRead > 0) Array.Copy(tempBuffer, 0, buffer, offset, bytesRead);
					}
				}
				return bytesRead;
			}

			public override bool CanRead { get { return stream != null; } }
			public override bool CanSeek { get { return stream != null; } }
			public override bool CanWrite { get { return false; } }

			public override long Length
			{
				get
				{
					ThrowIfDisposed();
					const int STATFLAG_NONAME = 1;
					STATSTG stats;
					stream.Stat(out stats, STATFLAG_NONAME);
					return stats.cbSize;
				}
			}

			public override long Position
			{
				get
				{
					ThrowIfDisposed();
					return Seek(0, SeekOrigin.Current);
				}
				set
				{
					ThrowIfDisposed();
					Seek(value, SeekOrigin.Begin);
				}
			}

			public override unsafe long Seek(long offset, SeekOrigin origin)
			{
				ThrowIfDisposed();
				long pos = 0;
				var posPtr = new IntPtr(&pos);
				stream.Seek(offset, (int)origin, posPtr);
				return pos;
			}

			public override void Flush() { ThrowIfDisposed(); }

			public override void SetLength(long value)
			{
				ThrowIfDisposed();
				throw new NotSupportedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				ThrowIfDisposed();
				throw new NotSupportedException();
			}
		}
	}
}