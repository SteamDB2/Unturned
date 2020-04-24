using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Interactable2SalvageBarricade : Interactable2
	{
		public override bool checkHint(out EPlayerMessage message, out float data)
		{
			message = EPlayerMessage.SALVAGE;
			if (this.hp != null)
			{
				data = (float)this.hp.hp / 100f;
			}
			else
			{
				data = 0f;
			}
			return base.hasOwnership;
		}

		public override void use()
		{
			BarricadeManager.salvageBarricade(this.root);
		}

		public Transform root;

		public Interactable2HP hp;
	}
}
