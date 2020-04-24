using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;

namespace SDG.Unturned
{
	public abstract class Asset : IDirtyable, IFormattedFileReadable, IFormattedFileWritable
	{
		public Asset()
		{
			this.name = base.GetType().Name;
		}

		public Asset(Bundle bundle, Local localization, byte[] hash)
		{
			if (bundle != null)
			{
				this.name = bundle.name;
			}
			else
			{
				this.name = "Asset_" + this.id;
			}
			this.id = 0;
			this.hash = hash;
		}

		public Asset(Bundle bundle, Data data, Local localization, ushort id)
		{
			if (bundle != null)
			{
				this.name = bundle.name;
			}
			else
			{
				this.name = "Asset_" + id;
			}
			this.id = id;
			if (data != null)
			{
				this.hash = data.hash;
				if (data.has("Asset_Origin_Override"))
				{
					this.assetOrigin = (EAssetOrigin)Enum.Parse(typeof(EAssetOrigin), data.readString("Asset_Origin_Override"), true);
				}
			}
			else
			{
				this.hash = new byte[20];
			}
		}

		public bool isDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				if (this.isDirty == value)
				{
					return;
				}
				this._isDirty = value;
				if (this.isDirty)
				{
					DirtyManager.markDirty(this);
				}
				else
				{
					DirtyManager.markClean(this);
				}
			}
		}

		public virtual string getFilePath()
		{
			string path = '/' + this.name + ".asset";
			return this.directory.getPath(path);
		}

		public void save()
		{
			string filePath = this.getFilePath();
			string directoryName = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(filePath))
			{
				IFormattedFileWriter writer = new KeyValueTableWriter(streamWriter);
				this.write(writer);
			}
		}

		public virtual void read(IFormattedFileReader reader)
		{
			if (reader == null)
			{
				return;
			}
			reader = reader.readObject();
			this.readAsset(reader);
		}

		protected virtual void readAsset(IFormattedFileReader reader)
		{
			this.id = reader.readValue<ushort>("ID");
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject("Metadata");
			writer.writeValue<Guid>("GUID", this.GUID);
			writer.writeValue<Type>("Type", base.GetType());
			writer.endObject();
			writer.beginObject("Asset");
			this.writeAsset(writer);
			writer.endObject();
		}

		protected virtual void writeAsset(IFormattedFileWriter writer)
		{
			writer.writeValue<ushort>("ID", this.id);
		}

		public AssetReference<T> getReferenceTo<T>() where T : Asset
		{
			return new AssetReference<T>(this.GUID);
		}

		public EAssetOrigin assetOrigin
		{
			get
			{
				return this._assetOrigin;
			}
			set
			{
				if (this.assetOrigin != EAssetOrigin.MISC)
				{
					return;
				}
				this._assetOrigin = value;
			}
		}

		public byte[] hash { get; protected set; }

		public virtual void clearHash()
		{
			this.hash = new byte[20];
		}

		public virtual EAssetType assetCategory
		{
			get
			{
				return EAssetType.NONE;
			}
		}

		public override string ToString()
		{
			return this.id + " - " + this.name;
		}

		protected bool _isDirty;

		public string name;

		[Inspectable("#SDG::Asset.Asset.ID.Name", null)]
		public ushort id;

		public AssetDirectory directory;

		[Inspectable("#SDG::Asset.Asset.GUID.Name", "#SDG::Asset.Asset.GUID.Tooltip")]
		public Guid GUID;

		protected EAssetOrigin _assetOrigin = EAssetOrigin.MISC;

		public string absoluteOriginFilePath;
	}
}
