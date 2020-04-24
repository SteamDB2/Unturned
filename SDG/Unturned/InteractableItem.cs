using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableItem : Interactable
	{
		public override void use()
		{
			ItemManager.takeItem(base.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		}

		public override bool checkHighlight(out Color color)
		{
			color = ItemTool.getRarityColorHighlight(this.asset.rarity);
			return true;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.ITEM;
			text = this.asset.itemName;
			color = ItemTool.getRarityColorUI(this.asset.rarity);
			return true;
		}

		public void clampRange()
		{
			if (this.wasReset)
			{
				return;
			}
			if ((base.transform.position - base.transform.parent.position).sqrMagnitude > 400f)
			{
				base.transform.position = base.transform.parent.position;
				this.wasReset = true;
				ItemManager.clampedItems.RemoveFast(this);
				Object.Destroy(base.GetComponent<Rigidbody>());
			}
		}

		private void OnEnable()
		{
			ItemManager.clampedItems.Add(this);
		}

		private void OnDisable()
		{
			if (this.wasReset)
			{
				return;
			}
			ItemManager.clampedItems.RemoveFast(this);
		}

		public Item item;

		public ItemJar jar;

		public ItemAsset asset;

		private bool wasReset;
	}
}
