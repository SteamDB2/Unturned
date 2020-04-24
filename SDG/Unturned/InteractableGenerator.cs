using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableGenerator : Interactable, IManualOnDestroy
	{
		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		public float wirerange
		{
			get
			{
				return this._wirerange;
			}
		}

		public float sqrWirerange
		{
			get
			{
				return this._sqrWirerange;
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

		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
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

		public void tellFuel(ushort newFuel)
		{
			this._fuel = newFuel;
			this.updateWire();
		}

		public void updatePowered(bool newPowered)
		{
			this._isPowered = newPowered;
			this.updateWire();
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._capacity = ((ItemGeneratorAsset)asset).capacity;
			this._wirerange = ((ItemGeneratorAsset)asset).wirerange;
			this._sqrWirerange = this.wirerange * this.wirerange;
			this.burn = ((ItemGeneratorAsset)asset).burn;
			this._isPowered = (state[0] == 1);
			this._fuel = BitConverter.ToUInt16(state, 1);
			if (!Dedicator.isDedicated)
			{
				this.engine = base.transform.FindChild("Engine");
			}
			if (Provider.isServer)
			{
				this.metadata = state;
			}
		}

		public override void use()
		{
			BarricadeManager.toggleGenerator(base.transform);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.isPowered)
			{
				message = EPlayerMessage.GENERATOR_OFF;
			}
			else
			{
				message = EPlayerMessage.GENERATOR_ON;
			}
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
			BitConverter.GetBytes(this.fuel).CopyTo(this.metadata, 1);
		}

		private void updateWire()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(this.isPowered && this.fuel > 0);
			}
			ushort maxValue = ushort.MaxValue;
			if (base.isPlant)
			{
				byte b;
				byte b2;
				BarricadeRegion barricadeRegion;
				BarricadeManager.tryGetPlant(base.transform.parent, out b, out b2, out maxValue, out barricadeRegion);
			}
			List<InteractablePower> list = PowerTool.checkPower(base.transform.position, this.wirerange, maxValue);
			for (int i = 0; i < list.Count; i++)
			{
				InteractablePower interactablePower = list[i];
				if (interactablePower.isWired)
				{
					if (!this.isPowered || this.fuel == 0)
					{
						bool flag = false;
						List<InteractableGenerator> list2 = PowerTool.checkGenerators(interactablePower.transform.position, 64f, maxValue);
						for (int j = 0; j < list2.Count; j++)
						{
							if (list2[j] != this && list2[j].isPowered && list2[j].fuel > 0 && (list2[j].transform.position - interactablePower.transform.position).sqrMagnitude < list2[j].sqrWirerange)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							interactablePower.updateWired(false);
						}
					}
				}
				else if (this.isPowered && this.fuel > 0)
				{
					interactablePower.updateWired(true);
				}
			}
		}

		public void ManualOnDestroy()
		{
			this.updatePowered(false);
		}

		private void Start()
		{
			this.updateWire();
			this.lastBurn = Time.realtimeSinceStartup;
		}

		private void Update()
		{
			if (Time.realtimeSinceStartup - this.lastBurn > this.burn)
			{
				this.lastBurn = Time.realtimeSinceStartup;
				if (this.isPowered)
				{
					if (this.fuel > 0)
					{
						this.isWiring = true;
						this.askBurn(1);
					}
					else if (this.isWiring)
					{
						this.isWiring = false;
						this.updateWire();
					}
				}
			}
		}

		private ushort _capacity;

		private float _wirerange;

		private float _sqrWirerange;

		private float burn;

		private bool _isPowered;

		private ushort _fuel;

		private Transform engine;

		private float lastBurn;

		private bool isWiring;

		private byte[] metadata;
	}
}
