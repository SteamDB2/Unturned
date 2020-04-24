using System;

namespace SDG.Unturned
{
	public class ItemVehicleRepairToolAsset : ItemToolAsset
	{
		public ItemVehicleRepairToolAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			bundle.unload();
		}

		public override bool isDangerous
		{
			get
			{
				return false;
			}
		}
	}
}
