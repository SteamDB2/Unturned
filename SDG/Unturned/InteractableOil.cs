using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableOil : InteractablePower
	{
		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		public ushort capacity
		{
			get
			{
				return 500;
			}
		}

		public bool isRefillable
		{
			get
			{
				return this.fuel < this.capacity;
			}
		}

		public bool isSiphonable
		{
			get
			{
				return this.fuel > 0;
			}
		}

		public void tellFuel(ushort newFuel)
		{
			this._fuel = newFuel;
		}

		public void askBurn(ushort amount)
		{
			if (amount == 0)
			{
				return;
			}
			if (amount >= this.fuel)
			{
				this._fuel = 0;
			}
			else
			{
				this._fuel -= amount;
			}
			if (Provider.isServer)
			{
				this.updateState();
			}
		}

		public void askFill(ushort amount)
		{
			if (amount == 0)
			{
				return;
			}
			if (amount >= this.capacity - this.fuel)
			{
				this._fuel = this.capacity;
			}
			else
			{
				this._fuel += amount;
			}
			if (Provider.isServer)
			{
				this.updateState();
			}
		}

		protected override void updateWired()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(base.isWired);
			}
			if (this.root != null)
			{
				if (base.isWired)
				{
					this.root.Play();
					this.root["Drill"].time = Random.Range(0f, this.root["Drill"].length);
				}
				else
				{
					this.root.Stop();
				}
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._fuel = BitConverter.ToUInt16(state, 0);
			if (!Dedicator.isDedicated)
			{
				this.engine = base.transform.FindChild("Engine");
				this.root = base.transform.FindChild("Root").GetComponent<Animation>();
			}
			if (Provider.isServer)
			{
				this.metadata = state;
			}
		}

		public override bool checkUseable()
		{
			return this.fuel > 0;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.VOLUME_FUEL;
			text = string.Empty;
			color = Color.white;
			return true;
		}

		private void updateState()
		{
			if (this.metadata == null)
			{
				return;
			}
			BitConverter.GetBytes(this.fuel).CopyTo(this.metadata, 0);
		}

		private void Update()
		{
			if (!base.isWired)
			{
				this.lastDrilled = Time.realtimeSinceStartup;
				return;
			}
			if (Time.realtimeSinceStartup - this.lastDrilled > 2f)
			{
				this.lastDrilled = Time.realtimeSinceStartup;
				if (this.fuel < this.capacity)
				{
					this.askFill(1);
				}
			}
		}

		private ushort _fuel;

		private byte[] metadata;

		private Transform engine;

		private Animation root;

		private float lastDrilled;
	}
}
