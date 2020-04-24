using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableObjectQuest : InteractableObject
	{
		public override void use()
		{
			if (base.objectAsset.interactabilityEffect != 0 && Time.realtimeSinceStartup - this.lastEffect > 1f)
			{
				this.lastEffect = Time.realtimeSinceStartup;
				Transform transform = base.transform.FindChild("Effect");
				if (transform != null)
				{
					EffectManager.effect(base.objectAsset.interactabilityEffect, transform.position, transform.forward);
				}
				else
				{
					EffectManager.effect(base.objectAsset.interactabilityEffect, base.transform.position, base.transform.forward);
				}
			}
			ObjectManager.useObjectQuest(base.transform);
			if (!Provider.isServer)
			{
				base.objectAsset.applyInteractabilityConditions(Player.player, false);
				base.objectAsset.grantInteractabilityRewards(Player.player, false);
			}
		}

		public override bool checkUseable()
		{
			return base.objectAsset.areInteractabilityConditionsMet(Player.player);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			int i = 0;
			while (i < base.objectAsset.interactabilityConditions.Length)
			{
				INPCCondition inpccondition = base.objectAsset.interactabilityConditions[i];
				if (!inpccondition.isConditionMet(Player.player))
				{
					text = inpccondition.formatCondition(Player.player);
					color = Color.white;
					if (string.IsNullOrEmpty(text))
					{
						message = EPlayerMessage.NONE;
						return false;
					}
					message = EPlayerMessage.CONDITION;
					return true;
				}
				else
				{
					i++;
				}
			}
			message = EPlayerMessage.INTERACT;
			text = base.objectAsset.interactabilityText;
			color = Color.white;
			return true;
		}

		private float lastEffect;
	}
}
