using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemArrestStartAsset : ItemAsset
	{
		public ItemArrestStartAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._strength = data.readUInt16("Strength");
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public ushort strength
		{
			get
			{
				return this._strength;
			}
		}

		public override bool isDangerous
		{
			get
			{
				return true;
			}
		}

		protected AudioClip _use;

		protected ushort _strength;
	}
}
