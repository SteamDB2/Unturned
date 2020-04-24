using System;
using HighlightingSystem;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableCharge : Interactable
	{
		public bool hasOwnership
		{
			get
			{
				return OwnershipTool.checkToggle(this.owner, this.group);
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.range2 = ((ItemChargeAsset)asset).range2;
			this.playerDamage = ((ItemChargeAsset)asset).playerDamage;
			this.zombieDamage = ((ItemChargeAsset)asset).zombieDamage;
			this.animalDamage = ((ItemChargeAsset)asset).animalDamage;
			this.barricadeDamage = ((ItemChargeAsset)asset).barricadeDamage;
			this.structureDamage = ((ItemChargeAsset)asset).structureDamage;
			this.vehicleDamage = ((ItemChargeAsset)asset).vehicleDamage;
			this.resourceDamage = ((ItemChargeAsset)asset).resourceDamage;
			this.objectDamage = ((ItemChargeAsset)asset).objectDamage;
			this.explosion2 = ((ItemChargeAsset)asset).explosion2;
		}

		public override bool checkInteractable()
		{
			return false;
		}

		public void detonate(CSteamID killer)
		{
			EffectManager.sendEffect(this.explosion2, EffectManager.LARGE, base.transform.position);
			DamageTool.explode(base.transform.position, this.range2, EDeathCause.CHARGE, killer, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage, this.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true);
			BarricadeManager.damage(base.transform, 5f, 1f, false);
		}

		public bool isSelected { get; private set; }

		public bool isTargeted { get; private set; }

		public void select()
		{
			if (this.isSelected)
			{
				return;
			}
			this.isSelected = true;
			this.updateHighlight();
		}

		public void deselect()
		{
			if (!this.isSelected)
			{
				return;
			}
			this.isSelected = false;
			this.updateHighlight();
		}

		public void target()
		{
			if (this.isTargeted)
			{
				return;
			}
			this.isTargeted = true;
			this.updateHighlight();
		}

		public void untarget()
		{
			if (!this.isTargeted)
			{
				return;
			}
			this.isTargeted = false;
			this.updateHighlight();
		}

		public void highlight()
		{
			if (this.highlighter != null)
			{
				return;
			}
			this.highlighter = base.gameObject.AddComponent<Highlighter>();
			this.updateHighlight();
		}

		public void unhighlight()
		{
			if (this.highlighter == null)
			{
				return;
			}
			Object.DestroyImmediate(this.highlighter);
			this.highlighter = null;
			this.isSelected = false;
			this.isTargeted = false;
		}

		private void updateHighlight()
		{
			if (this.highlighter == null)
			{
				return;
			}
			if (this.isSelected)
			{
				if (this.isTargeted)
				{
					this.highlighter.ConstantOn(Color.cyan);
				}
				else
				{
					this.highlighter.ConstantOn(Color.green);
				}
			}
			else if (this.isTargeted)
			{
				this.highlighter.ConstantOn(Color.yellow);
			}
			else
			{
				this.highlighter.ConstantOn(Color.red);
			}
		}

		public ulong owner;

		public ulong group;

		private float range2;

		private float playerDamage;

		private float zombieDamage;

		private float animalDamage;

		private float barricadeDamage;

		private float structureDamage;

		private float vehicleDamage;

		private float resourceDamage;

		private float objectDamage;

		private ushort explosion2;

		private Highlighter highlighter;
	}
}
