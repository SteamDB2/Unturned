using System;
using System.Runtime.InteropServices;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct AssetReference<T> : IAssetReference, IFormattedFileReadable, IFormattedFileWritable, IEquatable<AssetReference<T>> where T : Asset
	{
		public AssetReference(Guid GUID)
		{
			this.GUID = GUID;
		}

		public AssetReference(IAssetReference assetReference)
		{
			this.GUID = assetReference.GUID;
		}

		public Guid GUID { get; set; }

		public bool isValid
		{
			get
			{
				return this.GUID != Guid.Empty;
			}
		}

		public bool isReferenceTo(Asset asset)
		{
			return asset != null && this.GUID == asset.GUID;
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.GUID = reader.readValue<Guid>("GUID");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<Guid>("GUID", this.GUID);
			writer.endObject();
		}

		public static bool operator ==(AssetReference<T> a, AssetReference<T> b)
		{
			return a.GUID == b.GUID;
		}

		public static bool operator !=(AssetReference<T> a, AssetReference<T> b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return this.GUID.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			AssetReference<T> assetReference = (AssetReference<T>)obj;
			return this.GUID.Equals(assetReference.GUID);
		}

		public override string ToString()
		{
			return this.GUID.ToString("N");
		}

		public bool Equals(AssetReference<T> other)
		{
			return this.GUID.Equals(other.GUID);
		}

		public static AssetReference<T> invalid = new AssetReference<T>(Guid.Empty);
	}
}
