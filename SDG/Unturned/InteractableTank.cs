using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableTank : Interactable
	{
		public ETankSource source
		{
			get
			{
				return this._source;
			}
		}

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

		public void updateAmount(ushort newAmount)
		{
			this._amount = newAmount;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._amount = BitConverter.ToUInt16(state, 0);
			this._capacity = ((ItemTankAsset)asset).resource;
			this._source = ((ItemTankAsset)asset).source;
		}

		public override bool checkUseable()
		{
			return this.amount > 0;
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.source == ETankSource.WATER)
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

		private ETankSource _source;

		private ushort _amount;

		private ushort _capacity;
	}
}
