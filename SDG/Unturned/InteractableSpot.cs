using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableSpot : InteractablePower
	{
		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		private void updateLights()
		{
			bool flag = base.isWired && this.isPowered;
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", (!flag) ? Color.black : Color.white);
			}
			if (this.spot != null)
			{
				this.spot.gameObject.SetActive(flag);
			}
		}

		protected override void updateWired()
		{
			this.updateLights();
		}

		public void updatePowered(bool newPowered)
		{
			this._isPowered = newPowered;
			this.updateLights();
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isPowered = (state[0] == 1);
			if (!Dedicator.isDedicated)
			{
				this.material = HighlighterTool.getMaterialInstance(base.transform);
				this.spot = base.transform.FindChild("Spots");
				LightLODTool.applyLightLOD(this.spot);
			}
		}

		public override void use()
		{
			BarricadeManager.toggleSpot(base.transform);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.isPowered)
			{
				message = EPlayerMessage.SPOT_OFF;
			}
			else
			{
				message = EPlayerMessage.SPOT_ON;
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

		private bool _isPowered;

		private Material material;

		private Transform spot;
	}
}
