using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableOxygenator : InteractablePower
	{
		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		protected override void updateWired()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(base.isWired && this.isPowered);
			}
			this.updateBubble();
		}

		public void updatePowered(bool newPowered)
		{
			this._isPowered = newPowered;
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(base.isWired && this.isPowered);
			}
			this.updateBubble();
		}

		private void updateBubble()
		{
			if (base.isWired && this.isPowered)
			{
				this.registerBubble();
			}
			else
			{
				this.deregisterBubble();
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isPowered = (state[0] == 1);
			if (!Dedicator.isDedicated)
			{
				this.engine = base.transform.FindChild("Engine");
			}
		}

		public override void use()
		{
			BarricadeManager.toggleOxygenator(base.transform);
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

		private void registerBubble()
		{
			if (this.bubble != null)
			{
				return;
			}
			this.bubble = OxygenManager.registerBubble(base.transform, 24f);
		}

		private void deregisterBubble()
		{
			OxygenManager.deregisterBubble(this.bubble);
			this.bubble = null;
		}

		private void OnDestroy()
		{
			this.deregisterBubble();
		}

		private bool _isPowered;

		private Transform engine;

		private OxygenBubble bubble;
	}
}
