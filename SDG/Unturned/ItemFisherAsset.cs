using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemFisherAsset : ItemAsset
	{
		public ItemFisherAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._cast = (AudioClip)bundle.load("Cast");
			this._reel = (AudioClip)bundle.load("Reel");
			this._tug = (AudioClip)bundle.load("Tug");
			this._rewardID = data.readUInt16("Reward_ID");
			bundle.unload();
		}

		public AudioClip cast
		{
			get
			{
				return this._cast;
			}
		}

		public AudioClip reel
		{
			get
			{
				return this._reel;
			}
		}

		public AudioClip tug
		{
			get
			{
				return this._tug;
			}
		}

		public ushort rewardID
		{
			get
			{
				return this._rewardID;
			}
		}

		private AudioClip _cast;

		private AudioClip _reel;

		private AudioClip _tug;

		private ushort _rewardID;
	}
}
