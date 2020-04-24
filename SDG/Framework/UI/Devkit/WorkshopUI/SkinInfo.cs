using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class SkinInfo : IFormattedFileReadable, IFormattedFileWritable
	{
		public SkinInfo()
		{
			this.albedoPath = new InspectableFilePath("*.png");
			this.metallicPath = new InspectableFilePath("*.png");
			this.emissionPath = new InspectableFilePath("*.png");
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Albedo_Path.Name", null)]
		public InspectableFilePath albedoPath
		{
			get
			{
				return this._albedoPath;
			}
			set
			{
				this._albedoPath = value;
				this.triggerChanged();
			}
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Metallic_Path.Name", null)]
		public InspectableFilePath metallicPath
		{
			get
			{
				return this._metallicPath;
			}
			set
			{
				this._metallicPath = value;
				this.triggerChanged();
			}
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Emission_Path.Name", null)]
		public InspectableFilePath emissionPath
		{
			get
			{
				return this._emissionPath;
			}
			set
			{
				this._emissionPath = value;
				this.triggerChanged();
			}
		}

		public event SkinInfo.SkinInfoChangedHandler changed;

		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.readInfo(reader);
		}

		protected virtual void readInfo(IFormattedFileReader reader)
		{
			InspectableFilePath inspectableFilePath = this.albedoPath;
			inspectableFilePath.absolutePath = reader.readValue("Albedo");
			this.albedoPath = inspectableFilePath;
			inspectableFilePath = this.metallicPath;
			inspectableFilePath.absolutePath = reader.readValue("Metallic");
			this.metallicPath = inspectableFilePath;
			inspectableFilePath = this.emissionPath;
			inspectableFilePath.absolutePath = reader.readValue("Emission");
			this.emissionPath = inspectableFilePath;
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			this.writeInfo(writer);
			writer.endObject();
		}

		protected virtual void writeTexture(IFormattedFileWriter writer, string source, string name)
		{
			if (File.Exists(source))
			{
				string text = this.absolutePath + this.relativePath;
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string str = "/" + name + ".png";
				string text2 = text + str;
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Copy(source, text2);
				writer.writeValue(name, this.relativePath + str);
			}
		}

		protected virtual void writeInfo(IFormattedFileWriter writer)
		{
			this.writeTexture(writer, this.albedoPath.absolutePath, "Albedo");
			this.writeTexture(writer, this.metallicPath.absolutePath, "Metallic");
			this.writeTexture(writer, this.emissionPath.absolutePath, "Emission");
		}

		protected virtual void triggerChanged()
		{
			SkinInfo.SkinInfoChangedHandler skinInfoChangedHandler = this.changed;
			if (skinInfoChangedHandler != null)
			{
				skinInfoChangedHandler(this);
			}
		}

		public string absolutePath;

		public string relativePath;

		protected InspectableFilePath _albedoPath;

		protected InspectableFilePath _metallicPath;

		protected InspectableFilePath _emissionPath;

		public delegate void SkinInfoChangedHandler(SkinInfo info);
	}
}
