using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableFarm : Interactable
	{
		public uint planted
		{
			get
			{
				return this._planted;
			}
		}

		public uint growth
		{
			get
			{
				return this._growth;
			}
		}

		public ushort grow
		{
			get
			{
				return this._grow;
			}
		}

		public void updatePlanted(uint newPlanted)
		{
			this._planted = newPlanted;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._growth = ((ItemFarmAsset)asset).growth;
			this._grow = ((ItemFarmAsset)asset).grow;
			this._planted = BitConverter.ToUInt32(state, 0);
		}

		public bool checkFarm()
		{
			return this.planted > 0u && Provider.time > this.planted && Provider.time - this.planted > this.growth;
		}

		public override bool checkUseable()
		{
			return this.checkFarm();
		}

		public override void use()
		{
			BarricadeManager.farm(base.transform);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.FARM;
			}
			else
			{
				message = EPlayerMessage.GROW;
			}
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
			this.updatePlanted(1u);
			if (Provider.isServer)
			{
				BarricadeManager.updateFarm(base.transform, this.planted, false);
			}
		}

		private void Update()
		{
			if (!Dedicator.isDedicated && !this.isGrown && this.checkFarm())
			{
				this.isGrown = true;
				Transform transform = base.transform.FindChild("Foliage_0");
				if (transform != null)
				{
					transform.gameObject.SetActive(false);
				}
				Transform transform2 = base.transform.FindChild("Foliage_1");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(true);
				}
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

		private uint _planted;

		private uint _growth;

		private ushort _grow;

		private bool isGrown;
	}
}
