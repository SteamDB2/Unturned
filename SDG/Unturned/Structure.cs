using System;

namespace SDG.Unturned
{
	public class Structure
	{
		public Structure(ushort newID)
		{
			this._id = newID;
			this.asset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, this.id);
			this.health = this.asset.health;
		}

		public Structure(ushort newID, ushort newHealth, ItemStructureAsset newAsset)
		{
			this._id = newID;
			this.health = newHealth;
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

		public ItemStructureAsset asset { get; private set; }

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
			return this.id + " " + this.health;
		}

		private ushort _id;

		public ushort health;
	}
}
