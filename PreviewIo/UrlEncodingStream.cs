using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PreviewIo
{
	internal class UrlEncodingStream : Stream
	{
		private readonly Stream _dataStream;
		private readonly bool _readOnly;

		public UrlEncodingStream(Stream underlyingStream, bool readOnly)
		{
			_readOnly = readOnly;

			if (readOnly)
			{
				var base64Data = new StreamReader(underlyingStream).ReadToEnd();
				_dataStream = new MemoryStream(HttpUtility.UrlDecodeToBytes(base64Data), false);
			}
			else
			{
				_dataStream = underlyingStream;
			}
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!_readOnly)
				throw new InvalidOperationException();

			return _dataStream.BeginRead(buffer, offset, count, callback, state);
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (_readOnly)
				throw new InvalidOperationException();

			return _dataStream.BeginWrite(buffer, offset, count, callback, state);
		}

		public override bool CanRead
		{
			get { return _readOnly; }
		}

		public override bool CanSeek
		{
			get { return _dataStream.CanSeek; }
		}

		public override bool CanTimeout
		{
			get { return _dataStream.CanTimeout; }
		}

		public override bool CanWrite
		{
			get { return !_readOnly; }
		}

		public override void Close()
		{
			_dataStream.Close();
		}

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			if (!_readOnly)
				throw new InvalidOperationException();

			return _dataStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			if (!_readOnly)
				throw new InvalidOperationException();

			return _dataStream.EndRead(asyncResult);
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (_readOnly)
				throw new InvalidOperationException();

			base.EndWrite(asyncResult);
		}

		public override void Flush()
		{
			_dataStream.Flush();
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return _dataStream.FlushAsync(cancellationToken);
		}

		public override long Length
		{
			get { return _dataStream.Length; }
		}

		public override long Position
		{
			get { return _dataStream.Position; }
			set { _dataStream.Position = value; }
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!_readOnly)
				throw new InvalidOperationException();

			return _dataStream.Read(buffer, offset, count);
		}

		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (!_readOnly)
				throw new InvalidOperationException();

			return _dataStream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		public override int ReadByte()
		{
			if (!_readOnly)
				throw new InvalidOperationException();

			return _dataStream.ReadByte();
		}

		public override int ReadTimeout
		{
			get { return _dataStream.ReadTimeout; }
			set { _dataStream.ReadTimeout = value; }
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _dataStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			if (_readOnly)
				throw new InvalidOperationException();

			_dataStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (_readOnly)
				throw new InvalidOperationException();

			_dataStream.Write(buffer, offset, count);
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (_readOnly)
				throw new InvalidOperationException();

			return _dataStream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		public override void WriteByte(byte value)
		{
			if (_readOnly)
				throw new InvalidOperationException();

			_dataStream.WriteByte(value);
		}

		public override int WriteTimeout
		{
			get { return _dataStream.WriteTimeout; }
			set { _dataStream.WriteTimeout = value; }
		}
	}
}
