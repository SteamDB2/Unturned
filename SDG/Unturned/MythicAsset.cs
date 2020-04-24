using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MythicAsset : Asset
	{
		public MythicAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 500 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 500");
			}
			if (!Dedicator.isDedicated)
			{
				this._systemArea = (GameObject)bundle.load("System_Area");
				this._systemHook = (GameObject)bundle.load("System_Hook");
				this._systemFirst = (GameObject)bundle.load("System_First");
				this._systemThird = (GameObject)bundle.load("System_Third");
			}
			bundle.unload();
		}

		public GameObject systemArea
		{
			get
			{
				return this._systemArea;
			}
		}

		public GameObject systemHook
		{
			get
			{
				return this._systemHook;
			}
		}

		public GameObject systemFirst
		{
			get
			{
				return this._systemFirst;
			}
		}

		public GameObject systemThird
		{
			get
			{
				return this._systemThird;
			}
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.MYTHIC;
			}
		}

		protected GameObject _systemArea;

		protected GameObject _systemHook;

		protected GameObject _systemFirst;

		protected GameObject _systemThird;
	}
}
