using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableForage : Interactable
	{
		public override void use()
		{
			ResourceManager.forage(base.transform.parent);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.FORAGE;
			text = string.Empty;
			color = Color.white;
			return true;
		}
	}
}
