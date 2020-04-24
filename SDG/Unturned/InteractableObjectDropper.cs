using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableObjectDropper : InteractableObject
	{
		public bool isUsable
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityDelay && (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired);
			}
		}

		private void initAudioSourceComponent()
		{
			this.audioSourceComponent = base.transform.GetComponent<AudioSource>();
		}

		private void updateAudioSourceComponent()
		{
			if (this.audioSourceComponent != null && !Dedicator.isDedicated)
			{
				this.audioSourceComponent.Play();
			}
		}

		private void initDropTransform()
		{
			this.dropTransform = base.transform.FindChild("Drop");
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this.interactabilityDrops = ((ObjectAsset)asset).interactabilityDrops;
			this.interactabilityRewardID = ((ObjectAsset)asset).interactabilityRewardID;
			this.initAudioSourceComponent();
			this.initDropTransform();
		}

		public void drop()
		{
			this.lastUsed = Time.realtimeSinceStartup;
			if (this.dropTransform == null)
			{
				return;
			}
			if (this.interactabilityRewardID != 0)
			{
				ushort num = SpawnTableTool.resolve(this.interactabilityRewardID);
				if (num != 0)
				{
					ItemManager.dropItem(new Item(num, EItemOrigin.NATURE), this.dropTransform.position, false, true, false);
				}
			}
			else
			{
				ushort num2 = this.interactabilityDrops[Random.Range(0, this.interactabilityDrops.Length)];
				if (num2 != 0)
				{
					ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), this.dropTransform.position, false, true, false);
				}
			}
		}

		public override void use()
		{
			this.updateAudioSourceComponent();
			ObjectManager.useObjectDropper(base.transform);
		}

		public override bool checkUseable()
		{
			return (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired) && base.objectAsset.areInteractabilityConditionsMet(Player.player);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			for (int i = 0; i < base.objectAsset.interactabilityConditions.Length; i++)
			{
				INPCCondition inpccondition = base.objectAsset.interactabilityConditions[i];
				if (!inpccondition.isConditionMet(Player.player))
				{
					message = EPlayerMessage.CONDITION;
					text = inpccondition.formatCondition(Player.player);
					color = Color.white;
					return true;
				}
			}
			if (base.objectAsset.interactabilityPower != EObjectInteractabilityPower.NONE && !base.isWired)
			{
				message = EPlayerMessage.POWER;
			}
			else
			{
				switch (base.objectAsset.interactabilityHint)
				{
				case EObjectInteractabilityHint.DOOR:
					message = EPlayerMessage.DOOR_OPEN;
					break;
				case EObjectInteractabilityHint.SWITCH:
					message = EPlayerMessage.SPOT_ON;
					break;
				case EObjectInteractabilityHint.FIRE:
					message = EPlayerMessage.FIRE_ON;
					break;
				case EObjectInteractabilityHint.GENERATOR:
					message = EPlayerMessage.GENERATOR_ON;
					break;
				case EObjectInteractabilityHint.USE:
					message = EPlayerMessage.USE;
					break;
				default:
					message = EPlayerMessage.NONE;
					break;
				}
			}
			text = string.Empty;
			color = Color.white;
			return true;
		}

		private float lastUsed = -9999f;

		private ushort[] interactabilityDrops;

		private ushort interactabilityRewardID;

		private AudioSource audioSourceComponent;

		private Transform dropTransform;
	}
}
