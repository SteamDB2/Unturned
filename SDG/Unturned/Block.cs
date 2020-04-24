using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Block
	{
		public Block(int prefix, byte[] contents)
		{
			this.reset(prefix, contents);
		}

		public Block(byte[] contents)
		{
			this.reset(contents);
		}

		public Block(int prefix)
		{
			this.reset(prefix);
		}

		public Block()
		{
			this.reset();
		}

		private static object[] getObjects(int index)
		{
			object[] array = Block.objects[index];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = null;
			}
			return array;
		}

		public string readString()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				string @string = Encoding.UTF8.GetString(this.block, this.step + 1, (int)this.block[this.step]);
				this.step = this.step + 1 + (int)this.block[this.step];
				return @string;
			}
			return string.Empty;
		}

		public bool readBoolean()
		{
			if (this.block != null && this.step <= this.block.Length - 1)
			{
				bool result = BitConverter.ToBoolean(this.block, this.step);
				this.step++;
				return result;
			}
			return false;
		}

		public bool[] readBooleanArray()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				bool[] array = new bool[(int)this.readUInt16()];
				ushort num = (ushort)Mathf.CeilToInt((float)array.Length / 8f);
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					for (byte b = 0; b < 8; b += 1)
					{
						if ((int)(num2 * 8 + (ushort)b) >= array.Length)
						{
							break;
						}
						array[(int)(num2 * 8 + (ushort)b)] = ((this.block[this.step + (int)num2] & Types.SHIFTS[(int)b]) == Types.SHIFTS[(int)b]);
					}
				}
				this.step += (int)num;
				return array;
			}
			return new bool[0];
		}

		public byte readByte()
		{
			if (this.block != null && this.step <= this.block.Length - 1)
			{
				byte result = this.block[this.step];
				this.step++;
				return result;
			}
			return 0;
		}

		public byte[] readByteArray()
		{
			if (this.block != null && this.step < this.block.Length)
			{
				byte[] array;
				if (this.longBinaryData)
				{
					int num = this.readInt32();
					if (num >= 30000)
					{
						return new byte[0];
					}
					array = new byte[num];
				}
				else
				{
					array = new byte[(int)this.block[this.step]];
					this.step++;
				}
				try
				{
					Buffer.BlockCopy(this.block, this.step, array, 0, array.Length);
				}
				catch
				{
				}
				this.step += array.Length;
				return array;
			}
			return new byte[0];
		}

		public short readInt16()
		{
			if (this.block != null && this.step <= this.block.Length - 2)
			{
				short result = BitConverter.ToInt16(this.block, this.step);
				this.step += 2;
				return result;
			}
			return 0;
		}

		public ushort readUInt16()
		{
			if (this.block != null && this.step <= this.block.Length - 2)
			{
				ushort result = BitConverter.ToUInt16(this.block, this.step);
				this.step += 2;
				return result;
			}
			return 0;
		}

		public int readInt32()
		{
			if (this.block != null && this.step <= this.block.Length - 4)
			{
				int result = BitConverter.ToInt32(this.block, this.step);
				this.step += 4;
				return result;
			}
			return 0;
		}

		public int[] readInt32Array()
		{
			ushort num = this.readUInt16();
			int[] array = new int[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				int num3 = this.readInt32();
				array[(int)num2] = num3;
			}
			return array;
		}

		public uint readUInt32()
		{
			if (this.block != null && this.step <= this.block.Length - 4)
			{
				uint result = BitConverter.ToUInt32(this.block, this.step);
				this.step += 4;
				return result;
			}
			return 0u;
		}

		public float readSingle()
		{
			if (this.block != null && this.step <= this.block.Length - 4)
			{
				float result = BitConverter.ToSingle(this.block, this.step);
				this.step += 4;
				return result;
			}
			return 0f;
		}

		public long readInt64()
		{
			if (this.block != null && this.step <= this.block.Length - 8)
			{
				long result = BitConverter.ToInt64(this.block, this.step);
				this.step += 8;
				return result;
			}
			return 0L;
		}

		public ulong readUInt64()
		{
			if (this.block != null && this.step <= this.block.Length - 8)
			{
				ulong result = BitConverter.ToUInt64(this.block, this.step);
				this.step += 8;
				return result;
			}
			return 0UL;
		}

		public ulong[] readUInt64Array()
		{
			ushort num = this.readUInt16();
			ulong[] array = new ulong[(int)num];
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				ulong num3 = this.readUInt64();
				array[(int)num2] = num3;
			}
			return array;
		}

		public CSteamID readSteamID()
		{
			return new CSteamID(this.readUInt64());
		}

		public Guid readGUID()
		{
			GuidBuffer guidBuffer = default(GuidBuffer);
			guidBuffer.Read(this.readByteArray(), 0);
			return guidBuffer.GUID;
		}

		public Vector3 readUInt16RVector3()
		{
			byte b = this.readByte();
			double num = (double)this.readUInt16() / 65535.0;
			double num2 = (double)this.readUInt16() / 65535.0;
			byte b2 = this.readByte();
			double num3 = (double)this.readUInt16() / 65535.0;
			num = (double)(b * Regions.REGION_SIZE) + num * (double)Regions.REGION_SIZE - 4096.0;
			num2 = num2 * 2048.0 - 1024.0;
			num3 = (double)(b2 * Regions.REGION_SIZE) + num3 * (double)Regions.REGION_SIZE - 4096.0;
			return new Vector3((float)num, (float)num2, (float)num3);
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

		public object read(Type type)
		{
			if (type == Types.STRING_TYPE)
			{
				return this.readString();
			}
			if (type == Types.BOOLEAN_TYPE)
			{
				return this.readBoolean();
			}
			if (type == Types.BOOLEAN_ARRAY_TYPE)
			{
				return this.readBooleanArray();
			}
			if (type == Types.BYTE_TYPE)
			{
				return this.readByte();
			}
			if (type == Types.BYTE_ARRAY_TYPE)
			{
				return this.readByteArray();
			}
			if (type == Types.INT16_TYPE)
			{
				return this.readInt16();
			}
			if (type == Types.UINT16_TYPE)
			{
				return this.readUInt16();
			}
			if (type == Types.INT32_TYPE)
			{
				return this.readInt32();
			}
			if (type == Types.INT32_ARRAY_TYPE)
			{
				return this.readInt32Array();
			}
			if (type == Types.UINT32_TYPE)
			{
				return this.readUInt32();
			}
			if (type == Types.SINGLE_TYPE)
			{
				return this.readSingle();
			}
			if (type == Types.INT64_TYPE)
			{
				return this.readInt64();
			}
			if (type == Types.UINT64_TYPE)
			{
				return this.readUInt64();
			}
			if (type == Types.UINT64_ARRAY_TYPE)
			{
				return this.readUInt64Array();
			}
			if (type == Types.STEAM_ID_TYPE)
			{
				return this.readSteamID();
			}
			if (type == Types.GUID_TYPE)
			{
				return this.readGUID();
			}
			if (type == Types.VECTOR3_TYPE)
			{
				if (this.useCompression)
				{
					return this.readUInt16RVector3();
				}
				return this.readSingleVector3();
			}
			else
			{
				if (type == Types.COLOR_TYPE)
				{
					return this.readColor();
				}
				Debug.LogError("Failed to read type: " + type);
				return null;
			}
		}

		public object[] read(int offset, Type type_0)
		{
			object[] array = Block.getObjects(0);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			return array;
		}

		public object[] read(int offset, Type type_0, Type type_1)
		{
			object[] array = Block.getObjects(1);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			return array;
		}

		public object[] read(Type type_0, Type type_1)
		{
			return this.read(0, type_0, type_1);
		}

		public object[] read(int offset, Type type_0, Type type_1, Type type_2)
		{
			object[] array = Block.getObjects(2);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			return array;
		}

		public object[] read(Type type_0, Type type_1, Type type_2)
		{
			return this.read(0, type_0, type_1, type_2);
		}

		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3)
		{
			object[] array = Block.getObjects(3);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			return array;
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return this.read(0, type_0, type_1, type_2, type_3);
		}

		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			object[] array = Block.getObjects(4);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			if (offset < 5)
			{
				array[4] = this.read(type_4);
			}
			return array;
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4);
		}

		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			object[] array = Block.getObjects(5);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			if (offset < 5)
			{
				array[4] = this.read(type_4);
			}
			if (offset < 6)
			{
				array[5] = this.read(type_5);
			}
			return array;
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5);
		}

		public object[] read(int offset, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			object[] array = Block.getObjects(6);
			if (offset < 1)
			{
				array[0] = this.read(type_0);
			}
			if (offset < 2)
			{
				array[1] = this.read(type_1);
			}
			if (offset < 3)
			{
				array[2] = this.read(type_2);
			}
			if (offset < 4)
			{
				array[3] = this.read(type_3);
			}
			if (offset < 5)
			{
				array[4] = this.read(type_4);
			}
			if (offset < 6)
			{
				array[5] = this.read(type_5);
			}
			if (offset < 7)
			{
				array[6] = this.read(type_6);
			}
			return array;
		}

		public object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return this.read(0, type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		public object[] read(int offset, params Type[] types)
		{
			object[] array = new object[types.Length];
			for (int i = offset; i < types.Length; i++)
			{
				array[i] = this.read(types[i]);
			}
			return array;
		}

		public object[] read(params Type[] types)
		{
			return this.read(0, types);
		}

		public void writeString(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			Block.buffer[this.step] = (byte)bytes.Length;
			this.step++;
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += bytes.Length;
		}

		public void writeBoolean(bool value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Block.buffer[this.step] = bytes[0];
			this.step++;
		}

		public void writeBooleanArray(bool[] values)
		{
			this.writeUInt16((ushort)values.Length);
			ushort num = (ushort)Mathf.CeilToInt((float)values.Length / 8f);
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				Block.buffer[this.step + (int)num2] = 0;
				for (byte b = 0; b < 8; b += 1)
				{
					if ((int)(num2 * 8 + (ushort)b) >= values.Length)
					{
						break;
					}
					if (values[(int)(num2 * 8 + (ushort)b)])
					{
						byte[] array = Block.buffer;
						int num3 = this.step + (int)num2;
						array[num3] |= Types.SHIFTS[(int)b];
					}
				}
			}
			this.step += (int)num;
		}

		public void writeByte(byte value)
		{
			Block.buffer[this.step] = value;
			this.step++;
		}

		public void writeByteArray(byte[] values)
		{
			if (this.longBinaryData)
			{
				this.writeInt32(values.Length);
			}
			else
			{
				Block.buffer[this.step] = (byte)values.Length;
				this.step++;
			}
			if (values.Length < 30000)
			{
				Buffer.BlockCopy(values, 0, Block.buffer, this.step, values.Length);
				this.step += values.Length;
				return;
			}
		}

		public void writeInt16(short value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 2;
		}

		public void writeUInt16(ushort value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 2;
		}

		public void writeInt32(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 4;
		}

		public void writeInt32Array(int[] values)
		{
			this.writeUInt16((ushort)values.Length);
			ushort num = 0;
			while ((int)num < values.Length)
			{
				this.writeInt32(values[(int)num]);
				num += 1;
			}
		}

		public void writeUInt32(uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 4;
		}

		public void writeSingle(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 4;
		}

		public void writeInt64(long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 8;
		}

		public void writeUInt64(ulong value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Buffer.BlockCopy(bytes, 0, Block.buffer, this.step, bytes.Length);
			this.step += 8;
		}

		public void writeUInt64Array(ulong[] values)
		{
			this.writeUInt16((ushort)values.Length);
			ushort num = 0;
			while ((int)num < values.Length)
			{
				this.writeUInt64(values[(int)num]);
				num += 1;
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
			this.writeByteArray(GuidBuffer.GUID_BUFFER);
		}

		public void writeUInt16RVector3(Vector3 value)
		{
			double num = (double)value.x + 4096.0;
			double num2 = (double)value.y + 1024.0;
			double num3 = (double)value.z + 4096.0;
			byte value2 = (byte)(num / (double)Regions.REGION_SIZE);
			byte value3 = (byte)(num3 / (double)Regions.REGION_SIZE);
			num %= (double)Regions.REGION_SIZE;
			num2 %= 2048.0;
			num3 %= (double)Regions.REGION_SIZE;
			num /= (double)Regions.REGION_SIZE;
			num2 /= 2048.0;
			num3 /= (double)Regions.REGION_SIZE;
			this.writeByte(value2);
			this.writeUInt16((ushort)(num * 65535.0));
			this.writeUInt16((ushort)(num2 * 65535.0));
			this.writeByte(value3);
			this.writeUInt16((ushort)(num3 * 65535.0));
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

		public void write(object objects)
		{
			Type type = objects.GetType();
			if (type == Types.STRING_TYPE)
			{
				this.writeString((string)objects);
			}
			else if (type == Types.BOOLEAN_TYPE)
			{
				this.writeBoolean((bool)objects);
			}
			else if (type == Types.BOOLEAN_ARRAY_TYPE)
			{
				this.writeBooleanArray((bool[])objects);
			}
			else if (type == Types.BYTE_TYPE)
			{
				this.writeByte((byte)objects);
			}
			else if (type == Types.BYTE_ARRAY_TYPE)
			{
				this.writeByteArray((byte[])objects);
			}
			else if (type == Types.INT16_TYPE)
			{
				this.writeInt16((short)objects);
			}
			else if (type == Types.UINT16_TYPE)
			{
				this.writeUInt16((ushort)objects);
			}
			else if (type == Types.INT32_TYPE)
			{
				this.writeInt32((int)objects);
			}
			else if (type == Types.INT32_ARRAY_TYPE)
			{
				this.writeInt32Array((int[])objects);
			}
			else if (type == Types.UINT32_TYPE)
			{
				this.writeUInt32((uint)objects);
			}
			else if (type == Types.SINGLE_TYPE)
			{
				this.writeSingle((float)objects);
			}
			else if (type == Types.INT64_TYPE)
			{
				this.writeInt64((long)objects);
			}
			else if (type == Types.UINT64_TYPE)
			{
				this.writeUInt64((ulong)objects);
			}
			else if (type == Types.UINT64_ARRAY_TYPE)
			{
				this.writeUInt64Array((ulong[])objects);
			}
			else if (type == Types.STEAM_ID_TYPE)
			{
				this.writeSteamID((CSteamID)objects);
			}
			else if (type == Types.GUID_TYPE)
			{
				this.writeGUID((Guid)objects);
			}
			else if (type == Types.VECTOR3_TYPE)
			{
				if (this.useCompression)
				{
					this.writeUInt16RVector3((Vector3)objects);
				}
				else
				{
					this.writeSingleVector3((Vector3)objects);
				}
			}
			else if (type == Types.COLOR_TYPE)
			{
				this.writeColor((Color)objects);
			}
			else
			{
				Debug.LogError("Failed to write type: " + type);
			}
		}

		public void write(object object_0, object object_1)
		{
			this.write(object_0);
			this.write(object_1);
		}

		public void write(object object_0, object object_1, object object_2)
		{
			this.write(object_0, object_1);
			this.write(object_2);
		}

		public void write(object object_0, object object_1, object object_2, object object_3)
		{
			this.write(object_0, object_1, object_2);
			this.write(object_3);
		}

		public void write(object object_0, object object_1, object object_2, object object_3, object object_4)
		{
			this.write(object_0, object_1, object_2, object_3);
			this.write(object_4);
		}

		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			this.write(object_0, object_1, object_2, object_3, object_4);
			this.write(object_5);
		}

		public void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			this.write(object_0, object_1, object_2, object_3, object_4, object_5);
			this.write(object_6);
		}

		public void write(params object[] objects)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				this.write(objects[i]);
			}
		}

		public byte[] getBytes(out int size)
		{
			if (this.block == null)
			{
				size = this.step;
				return Block.buffer;
			}
			size = this.block.Length;
			return this.block;
		}

		public byte[] getHash()
		{
			if (this.block == null)
			{
				return Hash.SHA1(Block.buffer);
			}
			return Hash.SHA1(this.block);
		}

		public void reset(int prefix, byte[] contents)
		{
			this.step = prefix;
			this.block = contents;
		}

		public void reset(byte[] contents)
		{
			this.step = 0;
			this.block = contents;
		}

		public void reset(int prefix)
		{
			this.step = prefix;
			this.block = null;
		}

		public void reset()
		{
			this.step = 0;
			this.block = null;
		}

		public static readonly int BUFFER_SIZE = 65535;

		public static byte[] buffer = new byte[Block.BUFFER_SIZE];

		private static object[][] objects = new object[][]
		{
			new object[1],
			new object[2],
			new object[3],
			new object[4],
			new object[5],
			new object[6],
			new object[7]
		};

		public bool useCompression;

		public bool longBinaryData;

		public int step;

		public byte[] block;
	}
}
