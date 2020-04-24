using System;
using System.IO;

namespace SDG.Framework.IO.Streams
{
	public class NetworkStream
	{
		public NetworkStream(Stream newStream)
		{
			this.stream = newStream;
		}

		private Stream stream { get; set; }

		public sbyte readSByte()
		{
			return (sbyte)this.stream.ReadByte();
		}

		public byte readByte()
		{
			return (byte)this.stream.ReadByte();
		}

		public short readInt16()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			return (short)((int)b << 8 | (int)b2);
		}

		public ushort readUInt16()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			return (ushort)((int)b << 8 | (int)b2);
		}

		public int readInt32()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			byte b4 = this.readByte();
			return (int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4;
		}

		public uint readUInt32()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			byte b4 = this.readByte();
			return (uint)((int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4);
		}

		public long readInt64()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			byte b4 = this.readByte();
			byte b5 = this.readByte();
			byte b6 = this.readByte();
			byte b7 = this.readByte();
			byte b8 = this.readByte();
			return (long)((int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4 << 0 | (int)b5 << 24 | (int)b6 << 16 | (int)b7 << 8 | (int)b8);
		}

		public ulong readUInt64()
		{
			byte b = this.readByte();
			byte b2 = this.readByte();
			byte b3 = this.readByte();
			byte b4 = this.readByte();
			byte b5 = this.readByte();
			byte b6 = this.readByte();
			byte b7 = this.readByte();
			byte b8 = this.readByte();
			return (ulong)((long)((int)b << 24 | (int)b2 << 16 | (int)b3 << 8 | (int)b4 << 0 | (int)b5 << 24 | (int)b6 << 16 | (int)b7 << 8 | (int)b8));
		}

		public char readChar()
		{
			return (char)this.readUInt16();
		}

		public string readString()
		{
			ushort num = this.readUInt16();
			char[] array = new char[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				char c = this.readChar();
				array[(int)num2] = c;
			}
			return new string(array);
		}

		public void readBytes(byte[] data, ulong offset, ulong length)
		{
			this.stream.Read(data, (int)offset, (int)length);
		}

		public void writeSByte(sbyte data)
		{
			this.stream.WriteByte((byte)data);
		}

		public void writeByte(byte data)
		{
			this.stream.WriteByte(data);
		}

		public void writeInt16(short data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeUInt16(ushort data)
		{
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeInt32(int data)
		{
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeUInt32(uint data)
		{
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeInt64(long data)
		{
			this.writeByte((byte)(data >> 56));
			this.writeByte((byte)(data >> 48));
			this.writeByte((byte)(data >> 40));
			this.writeByte((byte)(data >> 32));
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeUInt64(ulong data)
		{
			this.writeByte((byte)(data >> 56));
			this.writeByte((byte)(data >> 48));
			this.writeByte((byte)(data >> 40));
			this.writeByte((byte)(data >> 32));
			this.writeByte((byte)(data >> 24));
			this.writeByte((byte)(data >> 16));
			this.writeByte((byte)(data >> 8));
			this.writeByte((byte)data);
		}

		public void writeChar(char data)
		{
			this.writeUInt16((ushort)data);
		}

		public void writeString(string data)
		{
			ushort num = (ushort)data.Length;
			char[] array = data.ToCharArray();
			this.writeUInt16(num);
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				char data2 = array[(int)num2];
				this.writeChar(data2);
			}
		}

		public void writeBytes(byte[] data, ulong offset, ulong length)
		{
			this.stream.Write(data, (int)offset, (int)length);
		}
	}
}
