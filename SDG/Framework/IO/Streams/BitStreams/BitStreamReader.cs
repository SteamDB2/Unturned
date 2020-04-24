using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	public class BitStreamReader
	{
		public BitStreamReader(Stream newStream)
		{
			this.stream = newStream;
			this.reset();
		}

		public Stream stream { get; protected set; }

		private byte buffer { get; set; }

		private byte bitIndex { get; set; }

		private byte bitsAvailable { get; set; }

		public void readBit(ref byte data)
		{
			this.readBits(ref data, 1);
		}

		public void readBits(ref byte data, byte length)
		{
			if (this.bitIndex == 8 && this.bitsAvailable == 0)
			{
				this.fillBuffer();
			}
			if (length > this.bitsAvailable)
			{
				byte b = length - this.bitsAvailable;
				this.readBits(ref data, this.bitsAvailable);
				data = (byte)(data << (int)b);
				this.readBits(ref data, b);
			}
			else
			{
				byte b2 = 8 - length - this.bitIndex;
				byte b3 = (byte)(255 >> (int)(8 - length));
				data |= (byte)(this.buffer >> (int)b2 & (int)b3);
				this.bitIndex += length;
				this.bitsAvailable -= length;
			}
		}

		private void fillBuffer()
		{
			this.buffer = (byte)this.stream.ReadByte();
			this.bitIndex = 0;
			this.bitsAvailable = 8;
		}

		public void reset()
		{
			this.buffer = 0;
			this.bitIndex = 8;
			this.bitsAvailable = 0;
		}
	}
}
