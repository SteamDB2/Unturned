using System;

namespace SDG.Unturned
{
	public class ItemTireAsset : ItemVehicleRepairToolAsset
	{
		public ItemTireAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._mode = (EUseableTireMode)Enum.Parse(typeof(EUseableTireMode), data.readString("Mode"), true);
			bundle.unload();
		}

		public EUseableTireMode mode
		{
			get
			{
				return this._mode;
			}
		}

		private EUseableTireMode _mode;
	}
}
