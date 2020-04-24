using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	public class KeyValueTableTypeReaderRegistry
	{
		public static T read<T>(IFormattedFileReader input)
		{
			IFormattedTypeReader formattedTypeReader;
			if (KeyValueTableTypeReaderRegistry.readers.TryGetValue(typeof(T), out formattedTypeReader))
			{
				object obj = formattedTypeReader.read(input);
				if (obj == null)
				{
					return default(T);
				}
				return (T)((object)obj);
			}
			else
			{
				if (!typeof(T).IsEnum)
				{
					Debug.LogError("Failed to find reader for: " + typeof(T));
					return default(T);
				}
				string value = input.readValue();
				if (string.IsNullOrEmpty(value))
				{
					return default(T);
				}
				return (T)((object)Enum.Parse(typeof(T), value, true));
			}
		}

		public static object read(IFormattedFileReader input, Type type)
		{
			IFormattedTypeReader formattedTypeReader;
			if (KeyValueTableTypeReaderRegistry.readers.TryGetValue(type, out formattedTypeReader))
			{
				object obj = formattedTypeReader.read(input);
				if (obj == null)
				{
					return type.getDefaultValue();
				}
				return obj;
			}
			else
			{
				if (!type.IsEnum)
				{
					Debug.LogError("Failed to find reader for: " + type);
					return type.getDefaultValue();
				}
				string value = input.readValue();
				if (string.IsNullOrEmpty(value))
				{
					return type.getDefaultValue();
				}
				return Enum.Parse(type, value, true);
			}
		}

		public static void add<T>(IFormattedTypeReader reader)
		{
			KeyValueTableTypeReaderRegistry.add(typeof(T), reader);
		}

		public static void add(Type type, IFormattedTypeReader reader)
		{
			KeyValueTableTypeReaderRegistry.readers.Add(type, reader);
		}

		public static void remove<T>()
		{
			KeyValueTableTypeReaderRegistry.remove(typeof(T));
		}

		public static void remove(Type type)
		{
			KeyValueTableTypeReaderRegistry.readers.Remove(type);
		}

		private static Dictionary<Type, IFormattedTypeReader> readers = new Dictionary<Type, IFormattedTypeReader>();
	}
}
