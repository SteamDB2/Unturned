using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemBarrelAsset : ItemCaliberAsset
	{
		public ItemBarrelAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._shoot = (AudioClip)bundle.load("Shoot");
			this._barrel = (GameObject)bundle.load("Barrel");
			this._isBraked = data.has("Braked");
			this._isSilenced = data.has("Silenced");
			this._volume = data.readSingle("Volume");
			this._durability = data.readByte("Durability");
			if (data.has("Ballistic_Drop"))
			{
				this._ballisticDrop = data.readSingle("Ballistic_Drop");
			}
			else
			{
				this._ballisticDrop = 1f;
			}
			bundle.unload();
		}

		public AudioClip shoot
		{
			get
			{
				return this._shoot;
			}
		}

		public GameObject barrel
		{
			get
			{
				return this._barrel;
			}
		}

		public bool isBraked
		{
			get
			{
				return this._isBraked;
			}
		}

		public bool isSilenced
		{
			get
			{
				return this._isSilenced;
			}
		}

		public float volume
		{
			get
			{
				return this._volume;
			}
		}

		public byte durability
		{
			get
			{
				return this._durability;
			}
		}

		public override bool showQuality
		{
			get
			{
				return this.durability > 0;
			}
		}

		public float ballisticDrop
		{
			get
			{
				return this._ballisticDrop;
			}
		}

		protected AudioClip _shoot;

		protected GameObject _barrel;

		private bool _isBraked;

		private bool _isSilenced;

		private float _volume;

		private byte _durability;

		private float _ballisticDrop;
	}
}
