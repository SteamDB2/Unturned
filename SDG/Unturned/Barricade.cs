using System;

namespace SDG.Unturned
{
	public class Barricade
	{
		public Barricade(ushort newID)
		{
			this._id = newID;
			this.asset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, this.id);
			if (this.asset == null)
			{
				this.health = 0;
				this.state = new byte[0];
				return;
			}
			this.health = this.asset.health;
			this.state = this.asset.getState();
		}

		public Barricade(ushort newID, ushort newHealth, byte[] newState, ItemBarricadeAsset newAsset)
		{
			this._id = newID;
			this.health = newHealth;
			this.state = newState;
			this.asset = newAsset;
		}

		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		public bool isRepaired
		{
			get
			{
				return this.health == this.asset.health;
			}
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		public ItemBarricadeAsset asset { get; private set; }

		public void askDamage(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
			}
			else
			{
				this.health -= amount;
			}
		}

		public void askRepair(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.asset.health - this.health)
			{
				this.health = this.asset.health;
			}
			else
			{
				this.health += amount;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.id,
				" ",
				this.health,
				" ",
				this.state.Length
			});
		}

		private ushort _id;

		public ushort health;

		public byte[] state;
	}
}
