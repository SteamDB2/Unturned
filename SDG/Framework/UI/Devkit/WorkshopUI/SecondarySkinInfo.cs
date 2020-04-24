using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class SecondarySkinInfo : SkinInfo
	{
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

		protected override void readInfo(IFormattedFileReader reader)
		{
			base.readInfo(reader);
			this.itemID = reader.readValue<ushort>("Item_ID");
		}

		protected override void writeInfo(IFormattedFileWriter writer)
		{
			base.writeInfo(writer);
			writer.writeValue<ushort>("Item_ID", this.itemID);
		}

		protected ushort _itemID;
	}
}
