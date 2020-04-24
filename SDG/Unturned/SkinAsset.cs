using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class SkinAsset : Asset
	{
		public SkinAsset(bool isPattern, Material primarySkin, Dictionary<ushort, Material> secondarySkins, Material attachmentSkin, Material tertiarySkin)
		{
			this._isPattern = isPattern;
			this._hasSight = true;
			this._hasTactical = true;
			this._hasGrip = true;
			this._hasBarrel = true;
			this._hasMagazine = true;
			this._primarySkin = primarySkin;
			this._secondarySkins = secondarySkins;
			this._attachmentSkin = attachmentSkin;
			this._tertiarySkin = tertiarySkin;
		}

		public SkinAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 2000 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this._isPattern = data.has("Pattern");
			this._hasSight = data.has("Sight");
			this._hasTactical = data.has("Tactical");
			this._hasGrip = data.has("Grip");
			this._hasBarrel = data.has("Barrel");
			this._hasMagazine = data.has("Magazine");
			if (!Dedicator.isDedicated)
			{
				this._primarySkin = (Material)bundle.load("Skin_Primary");
				this._secondarySkins = new Dictionary<ushort, Material>();
				ushort num = data.readUInt16("Secondary_Skins");
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					ushort num3 = data.readUInt16("Secondary_" + num2);
					if (!this.secondarySkins.ContainsKey(num3))
					{
						this.secondarySkins.Add(num3, (Material)bundle.load("Skin_Secondary_" + num3));
					}
				}
				this._attachmentSkin = (Material)bundle.load("Skin_Attachment");
				this._tertiarySkin = (Material)bundle.load("Skin_Tertiary");
			}
			bundle.unload();
		}

		public bool isPattern
		{
			get
			{
				return this._isPattern;
			}
		}

		public bool hasSight
		{
			get
			{
				return this._hasSight;
			}
		}

		public bool hasTactical
		{
			get
			{
				return this._hasTactical;
			}
		}

		public bool hasGrip
		{
			get
			{
				return this._hasGrip;
			}
		}

		public bool hasBarrel
		{
			get
			{
				return this._hasBarrel;
			}
		}

		public bool hasMagazine
		{
			get
			{
				return this._hasMagazine;
			}
		}

		public Material primarySkin
		{
			get
			{
				return this._primarySkin;
			}
		}

		public Dictionary<ushort, Material> secondarySkins
		{
			get
			{
				return this._secondarySkins;
			}
		}

		public Material attachmentSkin
		{
			get
			{
				return this._attachmentSkin;
			}
		}

		public Material tertiarySkin
		{
			get
			{
				return this._tertiarySkin;
			}
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.SKIN;
			}
		}

		protected bool _isPattern;

		protected bool _hasSight;

		protected bool _hasTactical;

		protected bool _hasGrip;

		protected bool _hasBarrel;

		protected bool _hasMagazine;

		protected Material _primarySkin;

		protected Dictionary<ushort, Material> _secondarySkins;

		protected Material _attachmentSkin;

		protected Material _tertiarySkin;
	}
}
