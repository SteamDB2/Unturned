using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamPacker
	{
		public static int step
		{
			get
			{
				return SteamPacker.block.step;
			}
			set
			{
				SteamPacker.block.step = value;
			}
		}

		public static bool useCompression
		{
			get
			{
				return SteamPacker.block.useCompression;
			}
			set
			{
				SteamPacker.block.useCompression = value;
			}
		}

		public static bool longBinaryData
		{
			get
			{
				return SteamPacker.block.longBinaryData;
			}
			set
			{
				SteamPacker.block.longBinaryData = value;
			}
		}

		public static object read(Type type)
		{
			return SteamPacker.block.read(type);
		}

		public static object[] read(Type type_0, Type type_1)
		{
			return SteamPacker.block.read(type_0, type_1);
		}

		public static object[] read(Type type_0, Type type_1, Type type_2)
		{
			return SteamPacker.block.read(type_0, type_1, type_2);
		}

		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3)
		{
			return SteamPacker.block.read(type_0, type_1, type_2, type_3);
		}

		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			return SteamPacker.block.read(type_0, type_1, type_2, type_3, type_4);
		}

		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			return SteamPacker.block.read(type_0, type_1, type_2, type_3, type_4, type_5);
		}

		public static object[] read(Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			return SteamPacker.block.read(type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		public static object[] read(params Type[] types)
		{
			return SteamPacker.block.read(types);
		}

		public static void openRead(int prefix, byte[] bytes)
		{
			SteamPacker.block.reset(prefix, bytes);
		}

		public static void closeRead()
		{
		}

		public static void write(object objects)
		{
			SteamPacker.block.write(objects);
		}

		public static void write(object object_0, object object_1)
		{
			SteamPacker.block.write(object_0, object_1);
		}

		public static void write(object object_0, object object_1, object object_2)
		{
			SteamPacker.block.write(object_0, object_1, object_2);
		}

		public static void write(object object_0, object object_1, object object_2, object object_3)
		{
			SteamPacker.block.write(object_0, object_1, object_2, object_3);
		}

		public static void write(object object_0, object object_1, object object_2, object object_3, object object_4)
		{
			SteamPacker.block.write(object_0, object_1, object_2, object_3, object_4);
		}

		public static void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			SteamPacker.block.write(object_0, object_1, object_2, object_3, object_4, object_5);
		}

		public static void write(object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			SteamPacker.block.write(object_0, object_1, object_2, object_3, object_4, object_5, object_6);
		}

		public static void write(params object[] objects)
		{
			SteamPacker.block.write(objects);
		}

		public static void openWrite(int prefix)
		{
			SteamPacker.block.reset(prefix);
		}

		public static byte[] closeWrite(out int size)
		{
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0, object object_1)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0, object_1);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0, object object_1, object object_2)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0, object_1, object_2);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0, object object_1, object object_2, object object_3)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0, object_1, object_2, object_3);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0, object object_1, object object_2, object object_3, object object_4)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0, object_1, object_2, object_3, object_4);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0, object object_1, object object_2, object object_3, object object_4, object object_5)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0, object_1, object_2, object_3, object_4, object_5);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, object object_0, object object_1, object object_2, object object_3, object object_4, object object_5, object object_6)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(object_0, object_1, object_2, object_3, object_4, object_5, object_6);
			return SteamPacker.block.getBytes(out size);
		}

		public static byte[] getBytes(int prefix, out int size, params object[] objects)
		{
			SteamPacker.block.reset(prefix);
			SteamPacker.block.write(objects);
			return SteamPacker.block.getBytes(out size);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(0, type_0);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0, Type type_1)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0, type_1);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(type_0, type_1);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0, Type type_1, Type type_2)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0, type_1, type_2);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(type_0, type_1, type_2);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0, Type type_1, Type type_2, Type type_3)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0, type_1, type_2, type_3);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(type_0, type_1, type_2, type_3);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0, type_1, type_2, type_3, type_4);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(type_0, type_1, type_2, type_3, type_4);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0, type_1, type_2, type_3, type_4, type_5);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(type_0, type_1, type_2, type_3, type_4, type_5);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, Type type_0, Type type_1, Type type_2, Type type_3, Type type_4, Type type_5, Type type_6)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (type_0 == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, type_0, type_1, type_2, type_3, type_4, type_5, type_6);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(type_0, type_1, type_2, type_3, type_4, type_5, type_6);
		}

		public static object[] getObjects(CSteamID steamID, int offset, int prefix, byte[] bytes, params Type[] types)
		{
			SteamPacker.block.reset(offset + prefix, bytes);
			if (types[0] == Types.STEAM_ID_TYPE)
			{
				object[] array = SteamPacker.block.read(1, types);
				array[0] = steamID;
				return array;
			}
			return SteamPacker.block.read(types);
		}

		public static Block block = new Block();
	}
}
