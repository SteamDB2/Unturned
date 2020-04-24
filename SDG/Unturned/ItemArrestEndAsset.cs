using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemArrestEndAsset : ItemAsset
	{
		public ItemArrestEndAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._recover = data.readUInt16("Recover");
			bundle.unload();
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public ushort recover
		{
			get
			{
				return this._recover;
			}
		}

		protected AudioClip _use;

		protected ushort _recover;
	}
}
