using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableFire : Interactable
	{
		public bool isLit
		{
			get
			{
				return this._isLit;
			}
		}

		private void updateFire()
		{
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", (!this.isLit) ? Color.black : Color.white);
			}
			if (this.fire != null)
			{
				this.fire.gameObject.SetActive(this.isLit);
			}
		}

		public void updateLit(bool newLit)
		{
			this._isLit = newLit;
			this.updateFire();
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._isLit = (state[0] == 1);
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			this.fire = base.transform.FindChild("Fire");
			LightLODTool.applyLightLOD(this.fire);
			this.updateFire();
		}

		public override void use()
		{
			BarricadeManager.toggleFire(base.transform);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.isLit)
			{
				message = EPlayerMessage.FIRE_OFF;
			}
			else
			{
				message = EPlayerMessage.FIRE_ON;
			}
			text = string.Empty;
			color = Color.white;
			return true;
		}

		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
		}

		private bool _isLit;

		private Material material;

		private Transform fire;
	}
}
