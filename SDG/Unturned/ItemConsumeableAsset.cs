using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemConsumeableAsset : ItemAsset
	{
		public ItemConsumeableAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._use = (AudioClip)bundle.load("Use");
			this._health = data.readByte("Health");
			this._food = data.readByte("Food");
			this._water = data.readByte("Water");
			this._virus = data.readByte("Virus");
			this._disinfectant = data.readByte("Disinfectant");
			this._energy = data.readByte("Energy");
			this._vision = data.readByte("Vision");
			this._warmth = data.readUInt32("Warmth");
			this._hasBleeding = data.has("Bleeding");
			this._hasBroken = data.has("Broken");
			this._hasAid = data.has("Aid");
			this.foodConstrainsWater = (this.food >= this.water);
		}

		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		public byte health
		{
			get
			{
				return this._health;
			}
		}

		public byte food
		{
			get
			{
				return this._food;
			}
		}

		public byte water
		{
			get
			{
				return this._water;
			}
		}

		public byte virus
		{
			get
			{
				return this._virus;
			}
		}

		public byte disinfectant
		{
			get
			{
				return this._disinfectant;
			}
		}

		public byte energy
		{
			get
			{
				return this._energy;
			}
		}

		public byte vision
		{
			get
			{
				return this._vision;
			}
		}

		public uint warmth
		{
			get
			{
				return this._warmth;
			}
		}

		public bool hasBleeding
		{
			get
			{
				return this._hasBleeding;
			}
		}

		public bool hasBroken
		{
			get
			{
				return this._hasBroken;
			}
		}

		public bool hasAid
		{
			get
			{
				return this._hasAid;
			}
		}

		public bool foodConstrainsWater { get; protected set; }

		public override bool showQuality
		{
			get
			{
				return this.type == EItemType.FOOD || this.type == EItemType.WATER;
			}
		}

		protected AudioClip _use;

		private byte _health;

		private byte _food;

		private byte _water;

		private byte _virus;

		private byte _disinfectant;

		private byte _energy;

		private byte _vision;

		private uint _warmth;

		private bool _hasBleeding;

		private bool _hasBroken;

		private bool _hasAid;
	}
}
