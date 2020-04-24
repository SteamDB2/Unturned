using System;

namespace SDG.Unturned
{
	public class ItemBeaconAsset : ItemBarricadeAsset
	{
		public ItemBeaconAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._wave = data.readUInt16("Wave");
			this._rewards = data.readByte("Rewards");
			this._rewardID = data.readUInt16("Reward_ID");
		}

		public ushort wave
		{
			get
			{
				return this._wave;
			}
		}

		public byte rewards
		{
			get
			{
				return this._rewards;
			}
		}

		public ushort rewardID
		{
			get
			{
				return this._rewardID;
			}
		}

		private ushort _wave;

		private byte _rewards;

		private ushort _rewardID;
	}
}
