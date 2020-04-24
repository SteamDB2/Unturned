using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemRefillAsset : ItemAsset
	{
		public ItemRefillAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._water = data.readByte("Water");
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public byte water
		{
			get
			{
				return this._water;
			}
		}

		public override byte[] getState(EItemOrigin origin)
		{
			byte[] array = new byte[1];
			if (origin == EItemOrigin.ADMIN)
			{
				array[0] = 1;
			}
			else
			{
				array[0] = 0;
			}
			return array;
		}

		public override string getContext(string desc, byte[] state)
		{
			string key;
			switch (state[0])
			{
			case 0:
				key = "Empty";
				break;
			case 1:
				key = "Clean";
				break;
			case 2:
				key = "Salty";
				break;
			case 3:
				key = "Dirty";
				break;
			default:
				key = "Full";
				break;
			}
			desc += PlayerDashboardInventoryUI.localization.format("Refill", new object[]
			{
				PlayerDashboardInventoryUI.localization.format(key)
			});
			desc += "\n\n";
			return desc;
		}

		protected AudioClip _use;

		protected byte _water;
	}
}
