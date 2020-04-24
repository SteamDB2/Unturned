using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableOven : InteractablePower
	{
		public bool isLit
		{
			get
			{
				return this._isLit;
			}
		}

		protected override void updateWired()
		{
			if (this.fire != null)
			{
				this.fire.gameObject.SetActive(base.isWired && this.isLit);
			}
		}

		public void updateLit(bool newLit)
		{
			this._isLit = newLit;
			if (this.fire != null)
			{
				this.fire.gameObject.SetActive(base.isWired && this.isLit);
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._isLit = (state[0] == 1);
			this.fire = base.transform.FindChild("Fire");
			LightLODTool.applyLightLOD(this.fire);
		}

		public override void use()
		{
			BarricadeManager.toggleOven(base.transform);
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

		private bool _isLit;

		private Transform fire;
	}
}
