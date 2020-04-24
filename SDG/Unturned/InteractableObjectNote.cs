using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableObjectNote : InteractableObject
	{
		public override void use()
		{
			PlayerBarricadeSignUI.open(base.objectAsset.interactabilityText);
			PlayerLifeUI.close();
			ObjectManager.useObjectQuest(base.transform);
			if (!Provider.isServer)
			{
				base.objectAsset.applyInteractabilityConditions(Player.player, false);
				base.objectAsset.grantInteractabilityRewards(Player.player, false);
			}
		}

		public override bool checkUseable()
		{
			return !PlayerUI.window.showCursor;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			EObjectInteractabilityHint interactabilityHint = base.objectAsset.interactabilityHint;
			if (interactabilityHint != EObjectInteractabilityHint.USE)
			{
				message = EPlayerMessage.NONE;
			}
			else
			{
				message = EPlayerMessage.USE;
			}
			text = string.Empty;
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}
	}
}
