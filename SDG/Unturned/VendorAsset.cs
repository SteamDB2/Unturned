using System;

namespace SDG.Unturned
{
	public class VendorAsset : Asset
	{
		public VendorAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 2000 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this.vendorName = localization.format("Name");
			this.vendorName = ItemTool.filterRarityRichText(this.vendorName);
			this.vendorDescription = localization.format("Description");
			this.vendorDescription = ItemTool.filterRarityRichText(this.vendorDescription);
			this.buying = new VendorBuying[(int)data.readByte("Buying")];
			byte b = 0;
			while ((int)b < this.buying.Length)
			{
				ushort newID = data.readUInt16("Buying_" + b + "_ID");
				uint newCost = data.readUInt32("Buying_" + b + "_Cost");
				INPCCondition[] array = new INPCCondition[(int)data.readByte("Buying_" + b + "_Conditions")];
				NPCTool.readConditions(data, localization, "Buying_" + b + "_Condition_", array);
				this.buying[(int)b] = new VendorBuying(b, newID, newCost, array);
				b += 1;
			}
			this.selling = new VendorSelling[(int)data.readByte("Selling")];
			byte b2 = 0;
			while ((int)b2 < this.selling.Length)
			{
				ushort newID2 = data.readUInt16("Selling_" + b2 + "_ID");
				uint newCost2 = data.readUInt32("Selling_" + b2 + "_Cost");
				INPCCondition[] array2 = new INPCCondition[(int)data.readByte("Selling_" + b2 + "_Conditions")];
				NPCTool.readConditions(data, localization, "Selling_" + b2 + "_Condition_", array2);
				this.selling[(int)b2] = new VendorSelling(b2, newID2, newCost2, array2);
				b2 += 1;
			}
			bundle.unload();
		}

		public string vendorName { get; protected set; }

		public string vendorDescription { get; protected set; }

		public VendorBuying[] buying { get; protected set; }

		public VendorSelling[] selling { get; protected set; }

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NPC;
			}
		}
	}
}
