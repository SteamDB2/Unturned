using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Attachments : MonoBehaviour
	{
		public ItemGunAsset gunAsset
		{
			get
			{
				return this._gunAsset;
			}
		}

		public SkinAsset skinAsset
		{
			get
			{
				return this._skinAsset;
			}
		}

		public ushort sightID
		{
			get
			{
				return this._sightID;
			}
		}

		public ushort tacticalID
		{
			get
			{
				return this._tacticalID;
			}
		}

		public ushort gripID
		{
			get
			{
				return this._gripID;
			}
		}

		public ushort barrelID
		{
			get
			{
				return this._barrelID;
			}
		}

		public ushort magazineID
		{
			get
			{
				return this._magazineID;
			}
		}

		public ItemSightAsset sightAsset
		{
			get
			{
				return this._sightAsset;
			}
		}

		public ItemTacticalAsset tacticalAsset
		{
			get
			{
				return this._tacticalAsset;
			}
		}

		public ItemGripAsset gripAsset
		{
			get
			{
				return this._gripAsset;
			}
		}

		public ItemBarrelAsset barrelAsset
		{
			get
			{
				return this._barrelAsset;
			}
		}

		public ItemMagazineAsset magazineAsset
		{
			get
			{
				return this._magazineAsset;
			}
		}

		public Transform sightModel
		{
			get
			{
				return this._sightModel;
			}
		}

		public Transform tacticalModel
		{
			get
			{
				return this._tacticalModel;
			}
		}

		public Transform gripModel
		{
			get
			{
				return this._gripModel;
			}
		}

		public Transform barrelModel
		{
			get
			{
				return this._barrelModel;
			}
		}

		public Transform magazineModel
		{
			get
			{
				return this._magazineModel;
			}
		}

		public Transform sightHook
		{
			get
			{
				return this._sightHook;
			}
		}

		public Transform viewHook
		{
			get
			{
				return this._viewHook;
			}
		}

		public Transform tacticalHook
		{
			get
			{
				return this._tacticalHook;
			}
		}

		public Transform gripHook
		{
			get
			{
				return this._gripHook;
			}
		}

		public Transform barrelHook
		{
			get
			{
				return this._barrelHook;
			}
		}

		public Transform magazineHook
		{
			get
			{
				return this._magazineHook;
			}
		}

		public Transform ejectHook
		{
			get
			{
				return this._ejectHook;
			}
		}

		public Transform lightHook
		{
			get
			{
				return this._lightHook;
			}
		}

		public Transform light2Hook
		{
			get
			{
				return this._light2Hook;
			}
		}

		public Transform aimHook
		{
			get
			{
				return this._aimHook;
			}
		}

		public Transform scopeHook
		{
			get
			{
				return this._scopeHook;
			}
		}

		public Transform reticuleHook
		{
			get
			{
				return this._reticuleHook;
			}
		}

		public Transform leftHook
		{
			get
			{
				return this._leftHook;
			}
		}

		public Transform rightHook
		{
			get
			{
				return this._rightHook;
			}
		}

		public Transform nockHook
		{
			get
			{
				return this._nockHook;
			}
		}

		public Transform restHook
		{
			get
			{
				return this._restHook;
			}
		}

		public LineRenderer rope
		{
			get
			{
				return this._rope;
			}
		}

		public void applyVisual()
		{
			if (this.isSkinned != this.wasSkinned)
			{
				this.wasSkinned = this.isSkinned;
				if (this.tempSightMaterial != null)
				{
					HighlighterTool.rematerialize(this.sightModel, this.tempSightMaterial, out this.tempSightMaterial);
				}
				if (this.tempTacticalMaterial != null)
				{
					HighlighterTool.rematerialize(this.tacticalModel, this.tempTacticalMaterial, out this.tempTacticalMaterial);
				}
				if (this.tempGripMaterial != null)
				{
					HighlighterTool.rematerialize(this.gripModel, this.tempGripMaterial, out this.tempGripMaterial);
				}
				if (this.tempBarrelMaterial != null)
				{
					HighlighterTool.rematerialize(this.barrelModel, this.tempBarrelMaterial, out this.tempBarrelMaterial);
				}
				if (this.tempMagazineMaterial != null)
				{
					HighlighterTool.rematerialize(this.magazineModel, this.tempMagazineMaterial, out this.tempMagazineMaterial);
				}
			}
		}

		public void updateGun(ItemGunAsset newGunAsset, SkinAsset newSkinAsset)
		{
			this._gunAsset = newGunAsset;
			this._skinAsset = newSkinAsset;
		}

		public void updateAttachments(byte[] state, bool viewmodel)
		{
			if (state == null || state.Length != 18)
			{
				return;
			}
			base.transform.localScale = Vector3.one;
			this._sightID = BitConverter.ToUInt16(state, 0);
			this._tacticalID = BitConverter.ToUInt16(state, 2);
			this._gripID = BitConverter.ToUInt16(state, 4);
			this._barrelID = BitConverter.ToUInt16(state, 6);
			this._magazineID = BitConverter.ToUInt16(state, 8);
			if (this.sightModel != null)
			{
				Object.Destroy(this.sightModel.gameObject);
				this._sightModel = null;
			}
			try
			{
				this._sightAsset = (ItemSightAsset)Assets.find(EAssetType.ITEM, this.sightID);
			}
			catch
			{
				this._sightAsset = null;
			}
			this.tempSightMaterial = null;
			if (this.sightAsset != null && this.sightHook != null)
			{
				this._sightModel = Object.Instantiate<GameObject>(this.sightAsset.sight).transform;
				this.sightModel.name = "Sight";
				this.sightModel.transform.parent = this.sightHook;
				this.sightModel.transform.localPosition = Vector3.zero;
				this.sightModel.transform.localRotation = Quaternion.identity;
				this.sightModel.localScale = Vector3.one;
				if (viewmodel)
				{
					Layerer.viewmodel(this.sightModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.sightID, out material) && this.skinAsset.hasSight && this.sightAsset.isPaintable)
					{
						if (this.sightAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							material = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							material.SetTexture("_AlbedoBase", this.sightAsset.albedoBase);
							material.SetTexture("_MetallicBase", this.sightAsset.metallicBase);
							material.SetTexture("_EmissionBase", this.sightAsset.emissionBase);
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							material = this.skinAsset.tertiarySkin;
						}
					}
					if (material != null)
					{
						HighlighterTool.rematerialize(this.sightModel, material, out this.tempSightMaterial);
					}
				}
			}
			if (this.tacticalModel != null)
			{
				Object.Destroy(this.tacticalModel.gameObject);
				this._tacticalModel = null;
			}
			try
			{
				this._tacticalAsset = (ItemTacticalAsset)Assets.find(EAssetType.ITEM, this.tacticalID);
			}
			catch
			{
				this._tacticalAsset = null;
			}
			this.tempTacticalMaterial = null;
			if (this.tacticalAsset != null && this.tacticalHook != null)
			{
				this._tacticalModel = Object.Instantiate<GameObject>(this.tacticalAsset.tactical).transform;
				this.tacticalModel.name = "Tactical";
				this.tacticalModel.transform.parent = this.tacticalHook;
				this.tacticalModel.transform.localPosition = Vector3.zero;
				this.tacticalModel.transform.localRotation = Quaternion.identity;
				this.tacticalModel.localScale = Vector3.one;
				if (viewmodel)
				{
					Layerer.viewmodel(this.tacticalModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material2 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.tacticalID, out material2) && this.skinAsset.hasTactical && this.tacticalAsset.isPaintable)
					{
						if (this.tacticalAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							material2 = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							material2.SetTexture("_AlbedoBase", this.tacticalAsset.albedoBase);
							material2.SetTexture("_MetallicBase", this.tacticalAsset.metallicBase);
							material2.SetTexture("_EmissionBase", this.tacticalAsset.emissionBase);
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							material2 = this.skinAsset.tertiarySkin;
						}
					}
					if (material2 != null)
					{
						HighlighterTool.rematerialize(this.tacticalModel, material2, out this.tempTacticalMaterial);
					}
				}
			}
			if (this.gripModel != null)
			{
				Object.Destroy(this.gripModel.gameObject);
				this._gripModel = null;
			}
			try
			{
				this._gripAsset = (ItemGripAsset)Assets.find(EAssetType.ITEM, this.gripID);
			}
			catch
			{
				this._gripAsset = null;
			}
			this.tempGripMaterial = null;
			if (this.gripAsset != null && this.gripHook != null)
			{
				this._gripModel = Object.Instantiate<GameObject>(this.gripAsset.grip).transform;
				this.gripModel.name = "Grip";
				this.gripModel.transform.parent = this.gripHook;
				this.gripModel.transform.localPosition = Vector3.zero;
				this.gripModel.transform.localRotation = Quaternion.identity;
				this.gripModel.localScale = Vector3.one;
				if (viewmodel)
				{
					Layerer.viewmodel(this.gripModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material3 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.gripID, out material3) && this.skinAsset.hasGrip && this.gripAsset.isPaintable)
					{
						if (this.gripAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							material3 = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							material3.SetTexture("_AlbedoBase", this.gripAsset.albedoBase);
							material3.SetTexture("_MetallicBase", this.gripAsset.metallicBase);
							material3.SetTexture("_EmissionBase", this.gripAsset.emissionBase);
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							material3 = this.skinAsset.tertiarySkin;
						}
					}
					if (material3 != null)
					{
						HighlighterTool.rematerialize(this.gripModel, material3, out this.tempGripMaterial);
					}
				}
			}
			if (this.barrelModel != null)
			{
				Object.Destroy(this.barrelModel.gameObject);
				this._barrelModel = null;
			}
			try
			{
				this._barrelAsset = (ItemBarrelAsset)Assets.find(EAssetType.ITEM, this.barrelID);
			}
			catch
			{
				this._barrelAsset = null;
			}
			this.tempBarrelMaterial = null;
			if (this.barrelAsset != null && this.barrelHook != null)
			{
				this._barrelModel = Object.Instantiate<GameObject>(this.barrelAsset.barrel).transform;
				this.barrelModel.name = "Barrel";
				this.barrelModel.transform.parent = this.barrelHook;
				this.barrelModel.transform.localPosition = Vector3.zero;
				this.barrelModel.transform.localRotation = Quaternion.identity;
				this.barrelModel.localScale = Vector3.one;
				if (viewmodel)
				{
					Layerer.viewmodel(this.barrelModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material4 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.barrelID, out material4) && this.skinAsset.hasBarrel && this.barrelAsset.isPaintable)
					{
						if (this.barrelAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							material4 = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							material4.SetTexture("_AlbedoBase", this.barrelAsset.albedoBase);
							material4.SetTexture("_MetallicBase", this.barrelAsset.metallicBase);
							material4.SetTexture("_EmissionBase", this.barrelAsset.emissionBase);
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							material4 = this.skinAsset.tertiarySkin;
						}
					}
					if (material4 != null)
					{
						HighlighterTool.rematerialize(this.barrelModel, material4, out this.tempBarrelMaterial);
					}
				}
			}
			if (this.magazineModel != null)
			{
				Object.Destroy(this.magazineModel.gameObject);
				this._magazineModel = null;
			}
			try
			{
				this._magazineAsset = (ItemMagazineAsset)Assets.find(EAssetType.ITEM, this.magazineID);
			}
			catch
			{
				this._magazineAsset = null;
			}
			this.tempMagazineMaterial = null;
			if (this.magazineAsset != null && this.magazineHook != null)
			{
				Transform transform = null;
				if (this.magazineAsset.calibers.Length > 0)
				{
					transform = this.magazineHook.FindChild("Caliber_" + this.magazineAsset.calibers[0]);
				}
				if (transform == null)
				{
					transform = this.magazineHook;
				}
				this._magazineModel = Object.Instantiate<GameObject>(this.magazineAsset.magazine).transform;
				this.magazineModel.name = "Magazine";
				this.magazineModel.transform.parent = transform;
				this.magazineModel.transform.localPosition = Vector3.zero;
				this.magazineModel.transform.localRotation = Quaternion.identity;
				this.magazineModel.localScale = Vector3.one;
				if (viewmodel)
				{
					Layerer.viewmodel(this.magazineModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material5 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.magazineID, out material5) && this.skinAsset.hasMagazine && this.magazineAsset.isPaintable)
					{
						if (this.magazineAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							material5 = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							material5.SetTexture("_AlbedoBase", this.magazineAsset.albedoBase);
							material5.SetTexture("_MetallicBase", this.magazineAsset.metallicBase);
							material5.SetTexture("_EmissionBase", this.magazineAsset.emissionBase);
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							material5 = this.skinAsset.tertiarySkin;
						}
					}
					if (material5 != null)
					{
						HighlighterTool.rematerialize(this.magazineModel, material5, out this.tempMagazineMaterial);
					}
				}
			}
			if (this.tacticalModel != null && this.tacticalModel.childCount > 0)
			{
				this._lightHook = this.tacticalModel.transform.FindChild("Model_0").FindChild("Light");
				this._light2Hook = this.tacticalModel.transform.FindChild("Model_0").FindChild("Light2");
				if (viewmodel)
				{
					if (this.lightHook != null)
					{
						this.lightHook.tag = "Viewmodel";
						this.lightHook.gameObject.layer = LayerMasks.VIEWMODEL;
						Transform transform2 = this.lightHook.FindChild("Light");
						if (transform2 != null)
						{
							transform2.tag = "Viewmodel";
							transform2.gameObject.layer = LayerMasks.VIEWMODEL;
						}
					}
					if (this.light2Hook != null)
					{
						this.light2Hook.tag = "Viewmodel";
						this.light2Hook.gameObject.layer = LayerMasks.VIEWMODEL;
						Transform transform3 = this.light2Hook.FindChild("Light");
						if (transform3 != null)
						{
							transform3.tag = "Viewmodel";
							transform3.gameObject.layer = LayerMasks.VIEWMODEL;
						}
					}
				}
				else
				{
					LightLODTool.applyLightLOD(this.lightHook);
					LightLODTool.applyLightLOD(this.light2Hook);
				}
			}
			else
			{
				this._lightHook = null;
				this._light2Hook = null;
			}
			if (this.sightModel != null)
			{
				this._aimHook = this.sightModel.transform.FindChild("Model_0").FindChild("Aim");
				if (this.aimHook != null)
				{
					Transform transform4 = this.aimHook.parent.FindChild("Reticule");
					if (transform4 != null)
					{
						Renderer component = transform4.GetComponent<Renderer>();
						Material material6 = component.material;
						material6.SetColor("_Color", OptionsSettings.criticalHitmarkerColor);
						material6.SetColor("_EmissionColor", OptionsSettings.criticalHitmarkerColor);
					}
				}
				this._reticuleHook = this.sightModel.transform.FindChild("Model_0").FindChild("Reticule");
			}
			else
			{
				this._aimHook = null;
				this._reticuleHook = null;
			}
			if (this.aimHook != null)
			{
				this._scopeHook = this.aimHook.FindChild("Scope");
			}
			else
			{
				this._scopeHook = null;
			}
			if (this.rope != null && viewmodel)
			{
				this.rope.tag = "Viewmodel";
				this.rope.gameObject.layer = LayerMasks.VIEWMODEL;
			}
			this.wasSkinned = true;
			this.applyVisual();
		}

		private void Awake()
		{
			this._sightHook = base.transform.FindChild("Sight");
			this._viewHook = base.transform.FindChild("View");
			this._tacticalHook = base.transform.FindChild("Tactical");
			this._gripHook = base.transform.FindChild("Grip");
			this._barrelHook = base.transform.FindChild("Barrel");
			this._magazineHook = base.transform.FindChild("Magazine");
			this._ejectHook = base.transform.FindChild("Eject");
			this._leftHook = base.transform.FindChild("Left");
			this._rightHook = base.transform.FindChild("Right");
			this._nockHook = base.transform.FindChild("Nock");
			this._restHook = base.transform.FindChild("Rest");
			Transform transform = base.transform.FindChild("Rope");
			if (transform != null)
			{
				this._rope = (LineRenderer)transform.GetComponent<Renderer>();
			}
		}

		private ItemGunAsset _gunAsset;

		private SkinAsset _skinAsset;

		private ushort _sightID;

		private ushort _tacticalID;

		private ushort _gripID;

		private ushort _barrelID;

		private ushort _magazineID;

		private ItemSightAsset _sightAsset;

		private ItemTacticalAsset _tacticalAsset;

		private ItemGripAsset _gripAsset;

		private ItemBarrelAsset _barrelAsset;

		private ItemMagazineAsset _magazineAsset;

		private Transform _sightModel;

		private Transform _tacticalModel;

		private Transform _gripModel;

		private Transform _barrelModel;

		private Transform _magazineModel;

		private Transform _sightHook;

		private Transform _viewHook;

		private Transform _tacticalHook;

		private Transform _gripHook;

		private Transform _barrelHook;

		private Transform _magazineHook;

		private Transform _ejectHook;

		private Transform _lightHook;

		private Transform _light2Hook;

		private Transform _aimHook;

		private Transform _scopeHook;

		private Transform _reticuleHook;

		private Transform _leftHook;

		private Transform _rightHook;

		private Transform _nockHook;

		private Transform _restHook;

		private LineRenderer _rope;

		public bool isSkinned;

		private bool wasSkinned;

		private Material tempSightMaterial;

		private Material tempTacticalMaterial;

		private Material tempGripMaterial;

		private Material tempBarrelMaterial;

		private Material tempMagazineMaterial;
	}
}
