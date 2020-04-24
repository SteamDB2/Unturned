using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class SkinCreatorOutput : IFormattedFileReadable, IFormattedFileWritable
	{
		public SkinCreatorOutput()
		{
			this.primarySkin = new SkinInfo();
			this.primarySkin.changed += this.handleSkinInfoChanged;
			this.secondarySkins = new InspectableList<SecondarySkinInfo>();
			this.secondarySkins.inspectorAdded += this.handleSecondarySkinsInspectorAdded;
			this.secondarySkins.inspectorRemoved += this.handleSecondarySkinsInspectorRemoved;
			this.secondarySkins.inspectorChanged += this.handleSecondarySkinsInspectorChanged;
			this.tertiarySkin = new SkinInfo();
			this.tertiarySkin.changed += this.handleSkinInfoChanged;
			this.attachmentSkin = new SkinInfo();
			this.attachmentSkin.changed += this.handleSkinInfoChanged;
			this.outputPath = default(InspectableDirectoryPath);
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Item_ID.Name", null)]
		public ushort itemID
		{
			get
			{
				return this._itemID;
			}
			set
			{
				this._itemID = value;
				this.triggerChanged();
			}
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Primary_Skin.Name", null)]
		public SkinInfo primarySkin { get; protected set; }

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Secondary_Skins.Name", null)]
		public InspectableList<SecondarySkinInfo> secondarySkins { get; protected set; }

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Tertiary_Skin.Name", null)]
		public SkinInfo tertiarySkin { get; protected set; }

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Attachment_Skin.Name", null)]
		public SkinInfo attachmentSkin { get; protected set; }

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Output_Path.Name", null)]
		public InspectableDirectoryPath outputPath
		{
			get
			{
				return this._outputPath;
			}
			set
			{
				this._outputPath = value;
				this.triggerChanged();
			}
		}

		public event SkinCreatorOutput.SkinCreatorOutputChangedHandler changed;

		public virtual void read(IFormattedFileReader reader)
		{
			if (reader == null)
			{
				return;
			}
			reader = reader.readObject();
			this.itemID = reader.readValue<ushort>("Item_ID");
			this.primarySkin = reader.readValue<SkinInfo>("Primary_Skin");
			int num = reader.readArrayLength("Secondary_Skins");
			this.secondarySkins = new InspectableList<SecondarySkinInfo>(num);
			for (int i = 0; i < num; i++)
			{
				this.secondarySkins.Add(reader.readValue<SecondarySkinInfo>(i));
			}
			this.tertiarySkin = reader.readValue<SkinInfo>("Tertiary_Skin");
			this.attachmentSkin = reader.readValue<SkinInfo>("Attachment_Skin");
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<ushort>("Item_ID", this.itemID);
			this.primarySkin.absolutePath = this.outputPath.absolutePath;
			this.primarySkin.relativePath = "/Primary_Skin";
			writer.writeValue<SkinInfo>("Primary_Skin", this.primarySkin);
			writer.writeKey("Secondary_Skins");
			writer.beginArray();
			for (int i = 0; i < this.secondarySkins.Count; i++)
			{
				this.secondarySkins[i].absolutePath = this.outputPath.absolutePath;
				this.secondarySkins[i].relativePath = "/Secondary_Skin_" + this.secondarySkins[i].itemID;
				writer.writeValue<SecondarySkinInfo>(this.secondarySkins[i]);
			}
			writer.endArray();
			this.tertiarySkin.absolutePath = this.outputPath.absolutePath;
			this.tertiarySkin.relativePath = "/Tertiary_Skin";
			writer.writeValue<SkinInfo>("Tertiary_Skin", this.tertiarySkin);
			this.attachmentSkin.absolutePath = this.outputPath.absolutePath;
			this.attachmentSkin.relativePath = "/Attachment_Skin";
			writer.writeValue<SkinInfo>("Attachment_Skin", this.attachmentSkin);
			writer.endObject();
		}

		protected virtual void handleSkinInfoChanged(SkinInfo info)
		{
			this.triggerChanged();
		}

		protected virtual void handleSecondarySkinsInspectorAdded(IInspectableList list, object instance)
		{
			SecondarySkinInfo secondarySkinInfo = instance as SecondarySkinInfo;
			secondarySkinInfo.changed += this.handleSkinInfoChanged;
		}

		protected virtual void handleSecondarySkinsInspectorRemoved(IInspectableList list, object instance)
		{
			SecondarySkinInfo secondarySkinInfo = instance as SecondarySkinInfo;
			secondarySkinInfo.changed -= this.handleSkinInfoChanged;
		}

		protected virtual void handleSecondarySkinsInspectorChanged(IInspectableList list)
		{
			this.triggerChanged();
		}

		protected virtual void triggerChanged()
		{
			SkinCreatorOutput.SkinCreatorOutputChangedHandler skinCreatorOutputChangedHandler = this.changed;
			if (skinCreatorOutputChangedHandler != null)
			{
				skinCreatorOutputChangedHandler(this);
			}
		}

		protected ushort _itemID;

		protected InspectableDirectoryPath _outputPath;

		public delegate void SkinCreatorOutputChangedHandler(SkinCreatorOutput output);
	}
}
