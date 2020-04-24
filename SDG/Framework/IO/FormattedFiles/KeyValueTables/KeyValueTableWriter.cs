using System;
using System.IO;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	public class KeyValueTableWriter : IFormattedFileWriter
	{
		public KeyValueTableWriter(StreamWriter writer)
		{
			this.writer = writer;
			this.indentationCount = 0;
			this.hasWritten = false;
			this.wroteKey = false;
		}

		public virtual void writeKey(string key)
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.writeIndents();
			this.writer.Write('"');
			this.writer.Write(key);
			this.writer.Write('"');
			this.hasWritten = true;
			this.wroteKey = true;
		}

		public virtual void writeValue(string key, string value)
		{
			this.writeKey(key);
			this.writeValue(value);
		}

		public virtual void writeValue(string value)
		{
			if (this.wroteKey)
			{
				this.writer.Write(' ');
			}
			else
			{
				if (this.hasWritten)
				{
					this.writer.WriteLine();
				}
				this.writeIndents();
			}
			this.writer.Write('"');
			this.writer.Write(value);
			this.writer.Write('"');
			this.wroteKey = false;
		}

		public virtual void writeValue(string key, object value)
		{
			this.writeKey(key);
			this.writeValue(value);
		}

		public virtual void writeValue(object value)
		{
			if (value is IFormattedFileWritable)
			{
				IFormattedFileWritable formattedFileWritable = value as IFormattedFileWritable;
				formattedFileWritable.write(this);
			}
			else
			{
				KeyValueTableTypeWriterRegistry.write(this, value);
			}
		}

		public virtual void writeValue<T>(string key, T value)
		{
			this.writeKey(key);
			this.writeValue<T>(value);
		}

		public virtual void writeValue<T>(T value)
		{
			if (value is IFormattedFileWritable)
			{
				IFormattedFileWritable formattedFileWritable = value as IFormattedFileWritable;
				formattedFileWritable.write(this);
			}
			else
			{
				KeyValueTableTypeWriterRegistry.write<T>(this, value);
			}
		}

		public virtual void beginObject(string key)
		{
			this.writeKey(key);
			this.beginObject();
		}

		public virtual void beginObject()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.writeIndents();
			this.writer.Write('{');
			this.indentationCount++;
			this.hasWritten = true;
			this.wroteKey = false;
		}

		public virtual void endObject()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.indentationCount--;
			this.writeIndents();
			this.writer.Write('}');
		}

		public virtual void beginArray(string key)
		{
			this.writeKey(key);
			this.beginArray();
		}

		public virtual void beginArray()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.writeIndents();
			this.writer.Write('[');
			this.indentationCount++;
			this.hasWritten = true;
			this.wroteKey = false;
		}

		public virtual void endArray()
		{
			if (this.hasWritten)
			{
				this.writer.WriteLine();
			}
			this.indentationCount--;
			this.writeIndents();
			this.writer.Write(']');
		}

		protected virtual void writeIndents()
		{
			for (int i = 0; i < this.indentationCount; i++)
			{
				this.writer.Write('\t');
			}
		}

		protected StreamWriter writer;

		protected int indentationCount;

		protected bool hasWritten;

		protected bool wroteKey;
	}
}
