using System;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class River
	{
		public River(string newPath)
		{
			this.path = ReadWrite.PATH + newPath;
			if (!Directory.Exists(Path.GetDirectoryName(this.path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this.path));
			}
			this.stream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			this.water = 0;
		}

		public River(string newPath, bool usePath)
		{
			this.path = newPath;
			if (usePath)
			{
				this.path = ReadWrite.PATH + this.path;
			}
			if (!Directory.Exists(Path.GetDirectoryName(this.path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this.path));
			}
			this.stream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			this.water = 0;
		}

		public River(string newPath, bool usePath, bool useCloud, bool isReading)
		{
			this.path = newPath;
			if (useCloud)
			{
				if (isReading)
				{
					this.block = ReadWrite.readBlock(this.path, useCloud, 0);
				}
				if (this.block == null)
				{
					this.block = new Block();
				}
			}
			else
			{
				if (usePath)
				{
					this.path = ReadWrite.PATH + this.path;
				}
				if (!Directory.Exists(Path.GetDirectoryName(this.path)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(this.path));
				}
				this.stream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
				this.water = 0;
			}
		}

		public string readString()
		{
			if (this.block != null)
			{
				return this.block.readString();
			}
			int count = this.stream.ReadByte();
			this.stream.Read(River.buffer, 0, count);
			return Encoding.UTF8.GetString(River.buffer, 0, count);
		}

		public bool readBoolean()
		{
			if (this.block != null)
			{
				return this.block.readBoolean();
			}
			return this.stream.ReadByte() != 0;
		}

		public byte readByte()
		{
			if (this.block != null)
			{
				return this.block.readByte();
			}
			return (byte)this.stream.ReadByte();
		}

		public byte[] readBytes()
		{
			if (this.block != null)
			{
				return this.block.readByteArray();
			}
			byte[] array = new byte[(int)this.readUInt16()];
			this.stream.Read(array, 0, array.Length);
			return array;
		}

		public short readInt16()
		{
			if (this.block != null)
			{
				return this.block.readInt16();
			}
			this.stream.Read(River.buffer, 0, 2);
			return BitConverter.ToInt16(River.buffer, 0);
		}

		public ushort readUInt16()
		{
			if (this.block != null)
			{
				return this.block.readUInt16();
			}
			this.stream.Read(River.buffer, 0, 2);
			return BitConverter.ToUInt16(River.buffer, 0);
		}

		public int readInt32()
		{
			if (this.block != null)
			{
				return this.block.readInt32();
			}
			this.stream.Read(River.buffer, 0, 4);
			return BitConverter.ToInt32(River.buffer, 0);
		}

		public uint readUInt32()
		{
			if (this.block != null)
			{
				return this.block.readUInt32();
			}
			this.stream.Read(River.buffer, 0, 4);
			return BitConverter.ToUInt32(River.buffer, 0);
		}

		public float readSingle()
		{
			if (this.block != null)
			{
				return this.block.readSingle();
			}
			this.stream.Read(River.buffer, 0, 4);
			return BitConverter.ToSingle(River.buffer, 0);
		}

		public long readInt64()
		{
			if (this.block != null)
			{
				return this.block.readInt64();
			}
			this.stream.Read(River.buffer, 0, 8);
			return BitConverter.ToInt64(River.buffer, 0);
		}

		public ulong readUInt64()
		{
			if (this.block != null)
			{
				return this.block.readUInt64();
			}
			this.stream.Read(River.buffer, 0, 8);
			return BitConverter.ToUInt64(River.buffer, 0);
		}

		public CSteamID readSteamID()
		{
			return new CSteamID(this.readUInt64());
		}

		public Guid readGUID()
		{
			if (this.block != null)
			{
				return this.block.readGUID();
			}
			GuidBuffer guidBuffer = default(GuidBuffer);
			guidBuffer.Read(this.readBytes(), 0);
			return guidBuffer.GUID;
		}

		public Vector3 readSingleVector3()
		{
			return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
		}

		public Quaternion readSingleQuaternion()
		{
			return Quaternion.Euler(this.readSingle(), this.readSingle(), this.readSingle());
		}

		public Color readColor()
		{
			return new Color((float)this.readByte() / 255f, (float)this.readByte() / 255f, (float)this.readByte() / 255f);
		}

		public void writeString(string value)
		{
			if (this.block != null)
			{
				this.block.writeString(value);
			}
			else
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				this.stream.WriteByte((byte)bytes.Length);
				this.stream.Write(bytes, 0, bytes.Length);
				this.water += 1 + bytes.Length;
			}
		}

		public void writeBoolean(bool value)
		{
			if (this.block != null)
			{
				this.block.writeBoolean(value);
			}
			else
			{
				this.stream.WriteByte((!value) ? 0 : 1);
				this.water++;
			}
		}

		public void writeByte(byte value)
		{
			if (this.block != null)
			{
				this.block.writeByte(value);
			}
			else
			{
				this.stream.WriteByte(value);
				this.water++;
			}
		}

		public void writeBytes(byte[] values)
		{
			if (this.block != null)
			{
				this.block.writeByteArray(values);
			}
			else
			{
				this.writeUInt16((ushort)values.Length);
				this.stream.Write(values, 0, values.Length);
				this.water += values.Length;
			}
		}

		public void writeInt16(short value)
		{
			if (this.block != null)
			{
				this.block.writeInt16(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 2);
				this.water += 2;
			}
		}

		public void writeUInt16(ushort value)
		{
			if (this.block != null)
			{
				this.block.writeUInt16(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 2);
				this.water += 2;
			}
		}

		public void writeInt32(int value)
		{
			if (this.block != null)
			{
				this.block.writeInt32(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 4);
				this.water += 4;
			}
		}

		public void writeUInt32(uint value)
		{
			if (this.block != null)
			{
				this.block.writeUInt32(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 4);
				this.water += 4;
			}
		}

		public void writeSingle(float value)
		{
			if (this.block != null)
			{
				this.block.writeSingle(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 4);
				this.water += 4;
			}
		}

		public void writeInt64(long value)
		{
			if (this.block != null)
			{
				this.block.writeInt64(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 8);
				this.water += 8;
			}
		}

		public void writeUInt64(ulong value)
		{
			if (this.block != null)
			{
				this.block.writeUInt64(value);
			}
			else
			{
				byte[] bytes = BitConverter.GetBytes(value);
				this.stream.Write(bytes, 0, 8);
				this.water += 8;
			}
		}

		public void writeSteamID(CSteamID steamID)
		{
			this.writeUInt64(steamID.m_SteamID);
		}

		public void writeGUID(Guid GUID)
		{
			GuidBuffer guidBuffer = new GuidBuffer(GUID);
			guidBuffer.Write(GuidBuffer.GUID_BUFFER, 0);
			this.writeBytes(GuidBuffer.GUID_BUFFER);
		}

		public void writeSingleVector3(Vector3 value)
		{
			this.writeSingle(value.x);
			this.writeSingle(value.y);
			this.writeSingle(value.z);
		}

		public void writeSingleQuaternion(Quaternion value)
		{
			Vector3 eulerAngles = value.eulerAngles;
			this.writeSingle(eulerAngles.x);
			this.writeSingle(eulerAngles.y);
			this.writeSingle(eulerAngles.z);
		}

		public void writeColor(Color value)
		{
			this.writeByte((byte)(value.r * 255f));
			this.writeByte((byte)(value.g * 255f));
			this.writeByte((byte)(value.b * 255f));
		}

		public byte[] getHash()
		{
			this.stream.Position = 0L;
			return Hash.SHA1(this.stream);
		}

		public void closeRiver()
		{
			if (this.block != null)
			{
				ReadWrite.writeBlock(this.path, true, this.block);
			}
			else
			{
				if (this.water > 0)
				{
					this.stream.SetLength((long)this.water);
				}
				this.stream.Flush();
				this.stream.Close();
				this.stream.Dispose();
			}
		}

		private static byte[] buffer = new byte[Block.BUFFER_SIZE];

		private int water;

		private string path;

		private FileStream stream;

		private Block block;
	}
}
