using System;
using System.Runtime.InteropServices;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ContentReference<T> : IContentReference, IFormattedFileReadable, IFormattedFileWritable, IEquatable<ContentReference<T>> where T : Object
	{
		public ContentReference(string newName, string newPath)
		{
			this.name = newName;
			this.path = newPath;
		}

		public ContentReference(IContentReference contentReference)
		{
			this.name = contentReference.name;
			this.path = contentReference.path;
		}

		public string name { get; set; }

		public string path { get; set; }

		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.name) && !string.IsNullOrEmpty(this.path);
			}
		}

		public bool isReferenceTo(ContentFile file)
		{
			return file != null && this.name == file.rootDirectory.name && this.path == file.path;
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.name = reader.readValue("Name");
			this.path = reader.readValue("Path");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue("Name", this.name);
			writer.writeValue("Path", this.path);
			writer.endObject();
		}

		public static bool operator ==(ContentReference<T> a, ContentReference<T> b)
		{
			return a.name == b.name && a.path == b.path;
		}

		public static bool operator !=(ContentReference<T> a, ContentReference<T> b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return this.name.GetHashCode() ^ this.path.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ContentReference<T> contentReference = (ContentReference<T>)obj;
			return this.name == contentReference.name && this.path == contentReference.path;
		}

		public override string ToString()
		{
			return "#" + this.name + "::" + this.path;
		}

		public bool Equals(ContentReference<T> other)
		{
			return this.name == other.name && this.path == other.path;
		}

		public static ContentReference<T> invalid = new ContentReference<T>(null, null);
	}
}
