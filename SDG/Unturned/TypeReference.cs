using System;
using System.Runtime.InteropServices;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;

namespace SDG.Unturned
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct TypeReference<T> : ITypeReference, IFormattedFileReadable, IFormattedFileWritable, IEquatable<TypeReference<T>>
	{
		public TypeReference(string assemblyQualifiedName)
		{
			this.assemblyQualifiedName = assemblyQualifiedName;
		}

		public TypeReference(ITypeReference typeReference)
		{
			this.assemblyQualifiedName = typeReference.assemblyQualifiedName;
		}

		public string assemblyQualifiedName { get; set; }

		public Type type
		{
			get
			{
				return Type.GetType(this.assemblyQualifiedName);
			}
		}

		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.assemblyQualifiedName);
			}
		}

		public bool isReferenceTo(Type type)
		{
			return type != null && this.assemblyQualifiedName == type.FullName;
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.assemblyQualifiedName = reader.readValue("Type");
			this.assemblyQualifiedName = KeyValueTableTypeRedirectorRegistry.chase(this.assemblyQualifiedName);
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue("Type", this.assemblyQualifiedName);
			writer.endObject();
		}

		public static bool operator ==(TypeReference<T> a, TypeReference<T> b)
		{
			return a.assemblyQualifiedName == b.assemblyQualifiedName;
		}

		public static bool operator !=(TypeReference<T> a, TypeReference<T> b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return this.assemblyQualifiedName.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			TypeReference<T> typeReference = (TypeReference<T>)obj;
			return this.assemblyQualifiedName == typeReference.assemblyQualifiedName;
		}

		public override string ToString()
		{
			return this.assemblyQualifiedName;
		}

		public bool Equals(TypeReference<T> other)
		{
			return this.assemblyQualifiedName == other.assemblyQualifiedName;
		}

		public static TypeReference<T> invalid = new TypeReference<T>(null);
	}
}
