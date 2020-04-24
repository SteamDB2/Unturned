using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemRegion
	{
		public ItemRegion()
		{
			this._drops = new List<ItemDrop>();
			this.items = new List<ItemData>();
			this.isNetworked = false;
			this.lastRespawn = Time.realtimeSinceStartup;
			this.despawnItemIndex = 0;
			this.respawnItemIndex = 0;
		}

		public List<ItemDrop> drops
		{
			get
			{
				return this._drops;
			}
		}

		public void destroy()
		{
			ushort num = 0;
			while ((int)num < this.drops.Count)
			{
				Object.Destroy(this.drops[(int)num].model.gameObject);
				num += 1;
			}
			this.drops.Clear();
		}

		private List<ItemDrop> _drops;

		public List<ItemData> items;

		public bool isNetworked;

		public ushort despawnItemIndex;

		public ushort respawnItemIndex;

		public float lastRespawn;
	}
}
