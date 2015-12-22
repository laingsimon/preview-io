using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PreviewIo
{
	internal class Base64Stream : Stream
	{
		private readonly MemoryStream _dataStream;
		private readonly bool _readOnly;
		private readonly Stream _underlyingStream;

		public Base64Stream(string base64String)
		{
			_dataStream = new MemoryStream(Convert.FromBase64String(base64String), false);
			_readOnly = true;
		}

		public Base64Stream(Stream underlyingStream, bool readOnly)
		{
			_readOnly = readOnly;

			if (readOnly)
			{
				var base64Data = new StreamReader(underlyingStream).ReadToEnd();
				_dataStream = new MemoryStream(Convert.FromBase64String(base64Data), false);
			}
			else
			{
				_underlyingStream = underlyingStream;
				_dataStream = new MemoryStream();
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
			_Flush();

			_dataStream.SetLength(0);
			_dataStream.Close();

			if (_underlyingStream != null)
				_underlyingStream.Close();
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
		{ }

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(0);
		}

		private void _Flush()
		{
			if (_readOnly)
				return;

			var base64String = Convert.ToBase64String(_dataStream.ToArray());
			var base64Data = Encoding.UTF8.GetBytes(base64String);

			_underlyingStream.Write(base64Data, 0, base64Data.Length);
			_underlyingStream.Flush();

			_dataStream.Position = 0;
			_dataStream.SetLength(0);
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
