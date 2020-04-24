using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekJars : Sleek
	{
		public SleekJars(float radius, List<InventorySearch> search)
		{
			base.init();
			float num = 6.28318548f / (float)search.Count;
			for (int i = 0; i < search.Count; i++)
			{
				ItemJar jar = search[i].jar;
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, jar.item.id);
				if (itemAsset != null)
				{
					SleekItem sleekItem = new SleekItem(jar);
					sleekItem.positionOffset_X = (int)(Mathf.Cos(num * (float)i) * radius) - sleekItem.sizeOffset_X / 2;
					sleekItem.positionOffset_Y = (int)(Mathf.Sin(num * (float)i) * radius) - sleekItem.sizeOffset_Y / 2;
					sleekItem.positionScale_X = 0.5f;
					sleekItem.positionScale_Y = 0.5f;
					sleekItem.onClickedItem = new ClickedItem(this.onClickedButton);
					sleekItem.onDraggedItem = new DraggedItem(this.onClickedButton);
					base.add(sleekItem);
				}
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private void onClickedButton(SleekItem item)
		{
			int num = base.search(item);
			if (num != -1 && this.onClickedJar != null)
			{
				this.onClickedJar(this, num);
			}
		}

		public ClickedJar onClickedJar;
	}
}
