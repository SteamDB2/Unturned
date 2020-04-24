using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	public class BitStreamWriter
	{
		public BitStreamWriter(Stream newStream)
		{
			this.stream = newStream;
			this.reset();
		}

		public Stream stream { get; protected set; }

		private byte buffer { get; set; }

		private byte bitIndex { get; set; }

		private byte bitsAvailable { get; set; }

		public void writeBit(byte data)
		{
			this.writeBits(data, 1);
		}

		public void writeBits(byte data, byte length)
		{
			if (length > this.bitsAvailable)
			{
				byte b = length - this.bitsAvailable;
				this.writeBits((byte)(data >> (int)b), this.bitsAvailable);
				this.writeBits(data, b);
			}
			else
			{
				byte b2 = 8 - length - this.bitIndex;
				byte b3 = (byte)(255 >> (int)(8 - length));
				this.buffer |= (byte)((data & b3) << (int)b2);
				this.bitIndex += length;
				this.bitsAvailable -= length;
				if (this.bitIndex == 8 && this.bitsAvailable == 0)
				{
					this.emptyBuffer();
				}
			}
		}

		private void emptyBuffer()
		{
			this.stream.WriteByte(this.buffer);
			this.reset();
		}

		public void flush()
		{
			if (this.bitsAvailable == 8)
			{
				return;
			}
			this.emptyBuffer();
		}

		public void reset()
		{
			this.buffer = 0;
			this.bitIndex = 0;
			this.bitsAvailable = 8;
		}
	}
}
