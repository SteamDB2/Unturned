﻿using System;

namespace SDG.Unturned
{
	public class ItemKeyAsset : ItemAsset
	{
		public ItemKeyAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			bundle.unload();
		}
	}
}
