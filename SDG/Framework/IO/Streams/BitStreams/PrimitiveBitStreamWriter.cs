using System;
using System.IO;

namespace SDG.Framework.IO.Streams.BitStreams
{
	public class PrimitiveBitStreamWriter : BitStreamWriter
	{
		public PrimitiveBitStreamWriter(Stream newStream) : base(newStream)
		{
		}

		public void writeByte(byte data)
		{
			base.writeBits(data, 8);
		}

		public void writeInt16(short data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeInt16(short data, byte length)
		{
			if (length == 16)
			{
				this.writeInt16(data);
			}
			else if (length > 8)
			{
				base.writeBits((byte)(data >> 8), length - 8);
				this.writeByte((byte)data);
			}
			else if (length == 8)
			{
				this.writeByte((byte)data);
			}
			else
			{
				base.writeBits((byte)data, length);
			}
		}

		public void writeUInt16(ushort data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeUInt16(ushort data, byte length)
		{
			if (length == 16)
			{
				this.writeUInt16(data);
			}
			else if (length > 8)
			{
				base.writeBits((byte)(data >> 8), length - 8);
				this.writeByte((byte)data);
			}
			else if (length == 8)
			{
				this.writeByte((byte)data);
			}
			else
			{
				base.writeBits((byte)data, length);
			}
		}

		public void writeInt32(int data)
		{
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeInt32(int data, byte length)
		{
			if (length == 32)
			{
				this.writeInt32(data);
			}
			else if (length > 24)
			{
				base.writeBits((byte)(data >> 24), length - 8);
				this.writeByte((byte)(data >> 16));
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
			}
			else if (length == 24)
			{
				this.writeByte((byte)(data >> 16));
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
			}
			else if (length > 16)
			{
				base.writeBits((byte)(data >> 16), length - 8);
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
			}
			else if (length == 16)
			{
				this.writeByte((byte)(data >> 8));
				this.writeByte((byte)data);
			}
			else if (length > 8)
			{
				base.writeBits((byte)(data >> 8), length - 8);
				this.writeByte((byte)data);
			}
			else if (length == 8)
			{
				this.writeByte((byte)data);
			}
			else
			{
				base.writeBits((byte)data, length);
			}
		}
	}
}
