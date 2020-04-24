using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableRainBarrel : Interactable
	{
		public bool isFull
		{
			get
			{
				return this._isFull;
			}
		}

		public void updateFull(bool newFull)
		{
			this._isFull = newFull;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._isFull = (state[0] == 1);
		}

		public override bool checkUseable()
		{
			return this.isFull;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.VOLUME_WATER;
			text = string.Empty;
			color = Color.white;
			return true;
		}

		private void onRainUpdated(ELightingRain rain)
		{
			if (rain != ELightingRain.POST_DRIZZLE)
			{
				return;
			}
			if (Physics.Raycast(base.transform.position + Vector3.up, Vector3.up, 32f, RayMasks.BLOCK_WIND))
			{
				return;
			}
			this._isFull = true;
			if (Provider.isServer)
			{
				BarricadeManager.updateRainBarrel(base.transform, this.isFull, false);
			}
		}

		private void OnEnable()
		{
			LightingManager.onRainUpdated = (RainUpdated)Delegate.Combine(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
		}

		private void OnDisable()
		{
			LightingManager.onRainUpdated = (RainUpdated)Delegate.Remove(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
		}

		private bool _isFull;
	}
}
