using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableObjectResource : InteractableObject
	{
		public ushort amount
		{
			get
			{
				return this._amount;
			}
		}

		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		public bool isRefillable
		{
			get
			{
				return this.amount < this.capacity;
			}
		}

		public bool isSiphonable
		{
			get
			{
				return this.amount > 0;
			}
		}

		public bool checkCanReset(float multiplier)
		{
			if (this.amount == this.capacity)
			{
				return false;
			}
			if (base.objectAsset.interactabilityReset < 1f)
			{
				return false;
			}
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				return Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityReset * multiplier;
			}
			return base.objectAsset.interactability == EObjectInteractability.FUEL && Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityReset * multiplier;
		}

		public void updateAmount(ushort newAmount)
		{
			this._amount = newAmount;
			this.lastUsed = Time.realtimeSinceStartup;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._amount = BitConverter.ToUInt16(state, 0);
			this._capacity = ((ObjectAsset)asset).interactabilityResource;
			this.lastUsed = Time.realtimeSinceStartup;
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				if (this.isListeningForRain)
				{
					return;
				}
				this.isListeningForRain = true;
				LightingManager.onRainUpdated = (RainUpdated)Delegate.Combine(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
			}
		}

		public override bool checkUseable()
		{
			return this.amount > 0;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				message = EPlayerMessage.VOLUME_WATER;
				text = this.amount + "/" + this.capacity;
			}
			else
			{
				message = EPlayerMessage.VOLUME_FUEL;
				text = string.Empty;
			}
			color = Color.white;
			return true;
		}

		private void onRainUpdated(ELightingRain rain)
		{
			if (rain != ELightingRain.POST_DRIZZLE)
			{
				return;
			}
			this._amount = this.capacity;
			if (Provider.isServer)
			{
				ObjectManager.updateObjectResource(base.transform, this.amount, false);
			}
		}

		private void OnDestroy()
		{
			if (base.objectAsset.interactability == EObjectInteractability.WATER)
			{
				if (!this.isListeningForRain)
				{
					return;
				}
				this.isListeningForRain = false;
				LightingManager.onRainUpdated = (RainUpdated)Delegate.Remove(LightingManager.onRainUpdated, new RainUpdated(this.onRainUpdated));
			}
		}

		private ushort _amount;

		private ushort _capacity;

		private bool isListeningForRain;

		private float lastUsed = -9999f;
	}
}
