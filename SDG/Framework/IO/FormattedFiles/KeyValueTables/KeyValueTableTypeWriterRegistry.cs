using System;
using System.Collections.Generic;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	public class KeyValueTableTypeWriterRegistry
	{
		public static void write<T>(IFormattedFileWriter output, T value)
		{
			IFormattedTypeWriter formattedTypeWriter;
			if (KeyValueTableTypeWriterRegistry.writers.TryGetValue(typeof(T), out formattedTypeWriter))
			{
				formattedTypeWriter.write(output, value);
			}
			else
			{
				output.writeValue(value.ToString());
			}
		}

		public static void write(IFormattedFileWriter output, object value)
		{
			IFormattedTypeWriter formattedTypeWriter;
			if (KeyValueTableTypeWriterRegistry.writers.TryGetValue(value.GetType(), out formattedTypeWriter))
			{
				formattedTypeWriter.write(output, value);
			}
			else
			{
				output.writeValue(value.ToString());
			}
		}

		public static void add<T>(IFormattedTypeWriter writer)
		{
			KeyValueTableTypeWriterRegistry.add(typeof(T), writer);
		}

		public static void add(Type type, IFormattedTypeWriter writer)
		{
			KeyValueTableTypeWriterRegistry.writers.Add(type, writer);
		}

		public static void remove<T>()
		{
			KeyValueTableTypeWriterRegistry.remove(typeof(T));
		}

		public static void remove(Type type)
		{
			KeyValueTableTypeWriterRegistry.writers.Remove(type);
		}

		private static Dictionary<Type, IFormattedTypeWriter> writers = new Dictionary<Type, IFormattedTypeWriter>();
	}
}
