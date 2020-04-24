using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class HumanClothes : MonoBehaviour
	{
		public Transform hatModel { get; private set; }

		public Transform backpackModel { get; private set; }

		public Transform vestModel { get; private set; }

		public Transform maskModel { get; private set; }

		public Transform glassesModel { get; private set; }

		public Transform hairModel { get; private set; }

		public Transform beardModel { get; private set; }

		public bool isVisual
		{
			get
			{
				return this._isVisual;
			}
			set
			{
				if (this.isVisual != value)
				{
					this._isVisual = value;
					this.updateAll(true);
				}
			}
		}

		public bool isMythic
		{
			get
			{
				return this._isMythic;
			}
			set
			{
				if (this.isMythic != value)
				{
					this._isMythic = value;
					this.updateAll(true);
				}
			}
		}

		public bool hasBackpack
		{
			get
			{
				return this._hasBackpack;
			}
			set
			{
				if (value != this._hasBackpack)
				{
					this._hasBackpack = value;
					if (this.backpackModel != null)
					{
						this.backpackModel.gameObject.SetActive(this.hasBackpack);
					}
				}
			}
		}

		public int visualShirt
		{
			get
			{
				return this._visualShirt;
			}
			set
			{
				if (this.visualShirt != value)
				{
					this._visualShirt = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualShirt != 0)
						{
							try
							{
								this.visualShirtAsset = (ItemShirtAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualShirt));
							}
							catch
							{
								this.visualShirtAsset = null;
							}
							if (this.visualShirtAsset != null && !this.visualShirtAsset.isPro)
							{
								this._visualShirt = 0;
								this.visualShirtAsset = null;
							}
						}
						else
						{
							this.visualShirtAsset = null;
						}
						this.needsShirtUpdate = true;
					}
				}
			}
		}

		public int visualPants
		{
			get
			{
				return this._visualPants;
			}
			set
			{
				if (this.visualPants != value)
				{
					this._visualPants = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualPants != 0)
						{
							try
							{
								this.visualPantsAsset = (ItemPantsAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualPants));
							}
							catch
							{
								this.visualPantsAsset = null;
							}
							if (this.visualPantsAsset != null && !this.visualPantsAsset.isPro)
							{
								this._visualPants = 0;
								this.visualPantsAsset = null;
							}
						}
						else
						{
							this.visualPantsAsset = null;
						}
						this.needsPantsUpdate = true;
					}
				}
			}
		}

		public int visualHat
		{
			get
			{
				return this._visualHat;
			}
			set
			{
				if (this.visualHat != value)
				{
					this._visualHat = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualHat != 0)
						{
							try
							{
								this.visualHatAsset = (ItemHatAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualHat));
							}
							catch
							{
								this.visualHatAsset = null;
							}
							if (this.visualHatAsset != null && !this.visualHatAsset.isPro)
							{
								this._visualHat = 0;
								this.visualHatAsset = null;
							}
						}
						else
						{
							this.visualHatAsset = null;
						}
						this.needsHatUpdate = true;
					}
				}
			}
		}

		public int visualBackpack
		{
			get
			{
				return this._visualBackpack;
			}
			set
			{
				if (this.visualBackpack != value)
				{
					this._visualBackpack = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualBackpack != 0)
						{
							try
							{
								this.visualBackpackAsset = (ItemBackpackAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualBackpack));
							}
							catch
							{
								this.visualBackpackAsset = null;
							}
							if (this.visualBackpackAsset != null && !this.visualBackpackAsset.isPro)
							{
								this._visualBackpack = 0;
								this.visualBackpackAsset = null;
							}
						}
						else
						{
							this.visualBackpackAsset = null;
						}
						this.needsBackpackUpdate = true;
					}
				}
			}
		}

		public int visualVest
		{
			get
			{
				return this._visualVest;
			}
			set
			{
				if (this.visualVest != value)
				{
					this._visualVest = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualVest != 0)
						{
							try
							{
								this.visualVestAsset = (ItemVestAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualVest));
							}
							catch
							{
								this.visualVestAsset = null;
							}
							if (this.visualVestAsset != null && !this.visualVestAsset.isPro)
							{
								this._visualVest = 0;
								this.visualVestAsset = null;
							}
						}
						else
						{
							this.visualVestAsset = null;
						}
						this.needsVestUpdate = true;
					}
				}
			}
		}

		public int visualMask
		{
			get
			{
				return this._visualMask;
			}
			set
			{
				if (this.visualMask != value)
				{
					this._visualMask = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualMask != 0)
						{
							try
							{
								this.visualMaskAsset = (ItemMaskAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualMask));
							}
							catch
							{
								this.visualMaskAsset = null;
							}
							if (this.visualMaskAsset != null && !this.visualMaskAsset.isPro)
							{
								this._visualMask = 0;
								this.visualMaskAsset = null;
							}
						}
						else
						{
							this.visualMaskAsset = null;
						}
						this.needsMaskUpdate = true;
					}
				}
			}
		}

		public int visualGlasses
		{
			get
			{
				return this._visualGlasses;
			}
			set
			{
				if (this.visualGlasses != value)
				{
					this._visualGlasses = value;
					if (!Dedicator.isDedicated)
					{
						if (this.visualGlasses != 0)
						{
							try
							{
								this.visualGlassesAsset = (ItemGlassesAsset)Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualGlasses));
							}
							catch
							{
								this.visualGlassesAsset = null;
							}
							if (this.visualGlassesAsset != null && !this.visualGlassesAsset.isPro)
							{
								this._visualGlasses = 0;
								this.visualGlassesAsset = null;
							}
						}
						else
						{
							this.visualGlassesAsset = null;
						}
						this.needsGlassesUpdate = true;
					}
				}
			}
		}

		public ItemShirtAsset shirtAsset
		{
			get
			{
				return this._shirtAsset;
			}
		}

		public ItemPantsAsset pantsAsset
		{
			get
			{
				return this._pantsAsset;
			}
		}

		public ItemHatAsset hatAsset
		{
			get
			{
				return this._hatAsset;
			}
		}

		public ItemBackpackAsset backpackAsset
		{
			get
			{
				return this._backpackAsset;
			}
		}

		public ItemVestAsset vestAsset
		{
			get
			{
				return this._vestAsset;
			}
		}

		public ItemMaskAsset maskAsset
		{
			get
			{
				return this._maskAsset;
			}
		}

		public ItemGlassesAsset glassesAsset
		{
			get
			{
				return this._glassesAsset;
			}
		}

		public ushort shirt
		{
			get
			{
				return this._shirt;
			}
			set
			{
				if (this.shirt != value)
				{
					this._shirt = value;
					if (this.shirt != 0)
					{
						try
						{
							this._shirtAsset = (ItemShirtAsset)Assets.find(EAssetType.ITEM, this.shirt);
						}
						catch
						{
							this._shirtAsset = null;
						}
						if (this.shirtAsset != null && this.shirtAsset.isPro && !this.canWearPro)
						{
							this._shirt = 0;
							this._shirtAsset = null;
						}
					}
					else
					{
						this._shirtAsset = null;
					}
					this.needsShirtUpdate = true;
				}
			}
		}

		public ushort pants
		{
			get
			{
				return this._pants;
			}
			set
			{
				if (this.pants != value)
				{
					this._pants = value;
					if (this.pants != 0)
					{
						try
						{
							this._pantsAsset = (ItemPantsAsset)Assets.find(EAssetType.ITEM, this.pants);
						}
						catch
						{
							this._pantsAsset = null;
						}
						if (this.pantsAsset != null && this.pantsAsset.isPro && !this.canWearPro)
						{
							this._pants = 0;
							this._pantsAsset = null;
						}
					}
					else
					{
						this._pantsAsset = null;
					}
					this.needsPantsUpdate = true;
				}
			}
		}

		public ushort hat
		{
			get
			{
				return this._hat;
			}
			set
			{
				if (this.hat != value)
				{
					this._hat = value;
					if (this.hat != 0)
					{
						try
						{
							this._hatAsset = (ItemHatAsset)Assets.find(EAssetType.ITEM, this.hat);
						}
						catch
						{
							this._hatAsset = null;
						}
						if (this.hatAsset != null && this.hatAsset.isPro && !this.canWearPro)
						{
							this._hat = 0;
							this._hatAsset = null;
						}
					}
					else
					{
						this._hatAsset = null;
					}
					this.needsHatUpdate = true;
				}
			}
		}

		public ushort backpack
		{
			get
			{
				return this._backpack;
			}
			set
			{
				if (this.backpack != value)
				{
					this._backpack = value;
					if (this.backpack != 0)
					{
						try
						{
							this._backpackAsset = (ItemBackpackAsset)Assets.find(EAssetType.ITEM, this.backpack);
						}
						catch
						{
							this._backpackAsset = null;
						}
						if (this.backpackAsset != null && this.backpackAsset.isPro && !this.canWearPro)
						{
							this._backpack = 0;
							this._backpackAsset = null;
						}
					}
					else
					{
						this._backpackAsset = null;
					}
					this.needsBackpackUpdate = true;
				}
			}
		}

		public ushort vest
		{
			get
			{
				return this._vest;
			}
			set
			{
				if (this.vest != value)
				{
					this._vest = value;
					if (this.vest != 0)
					{
						try
						{
							this._vestAsset = (ItemVestAsset)Assets.find(EAssetType.ITEM, this.vest);
						}
						catch
						{
							this._vestAsset = null;
						}
						if (this.vestAsset != null && this.vestAsset.isPro && !this.canWearPro)
						{
							this._vest = 0;
							this._vestAsset = null;
						}
					}
					else
					{
						this._vestAsset = null;
					}
					this.needsVestUpdate = true;
				}
			}
		}

		public ushort mask
		{
			get
			{
				return this._mask;
			}
			set
			{
				if (this.mask != value)
				{
					this._mask = value;
					if (this.mask != 0)
					{
						try
						{
							this._maskAsset = (ItemMaskAsset)Assets.find(EAssetType.ITEM, this.mask);
						}
						catch
						{
							this._maskAsset = null;
						}
						if (this.maskAsset != null && this.maskAsset.isPro && !this.canWearPro)
						{
							this._mask = 0;
							this._maskAsset = null;
						}
					}
					else
					{
						this._maskAsset = null;
					}
					this.needsMaskUpdate = true;
				}
			}
		}

		public ushort glasses
		{
			get
			{
				return this._glasses;
			}
			set
			{
				if (this.glasses != value)
				{
					this._glasses = value;
					if (this.glasses != 0)
					{
						try
						{
							this._glassesAsset = (ItemGlassesAsset)Assets.find(EAssetType.ITEM, this.glasses);
						}
						catch
						{
							this._glassesAsset = null;
						}
						if (this.glassesAsset != null && this.glassesAsset.isPro && !this.canWearPro)
						{
							this._glasses = 0;
							this._glassesAsset = null;
						}
					}
					else
					{
						this._glassesAsset = null;
					}
					this.needsGlassesUpdate = true;
				}
			}
		}

		public byte face
		{
			get
			{
				return this._face;
			}
			set
			{
				if (this.face != value)
				{
					this._face = value;
					if (!Dedicator.isDedicated)
					{
						this.clothing.face = (Texture2D)Resources.Load("Faces/" + this.face + "/Texture");
						this.clothing.faceEmission = (Texture2D)Resources.Load("Faces/" + this.face + "/Emission");
						this.clothing.faceMetallic = (Texture2D)Resources.Load("Faces/" + this.face + "/Metallic");
						this.needsClothesUpdate = true;
					}
				}
			}
		}

		public byte hair
		{
			get
			{
				return this._hair;
			}
			set
			{
				if (this.hair != value)
				{
					this._hair = value;
					this.needsHairUpdate = true;
				}
			}
		}

		public byte beard
		{
			get
			{
				return this._beard;
			}
			set
			{
				if (this.beard != value)
				{
					this._beard = value;
					this.needsBeardUpdate = true;
				}
			}
		}

		public Color skin
		{
			get
			{
				return this.clothing.skin;
			}
			set
			{
				this.clothing.skin = value;
				this.needsClothesUpdate = true;
			}
		}

		public Color color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		private void updateAll(bool isNeeded)
		{
			this.needsHairUpdate = isNeeded;
			this.needsBeardUpdate = isNeeded;
			this.needsClothesUpdate = isNeeded;
			this.needsShirtUpdate = isNeeded;
			this.needsPantsUpdate = isNeeded;
			this.needsHatUpdate = isNeeded;
			this.needsBackpackUpdate = isNeeded;
			this.needsVestUpdate = isNeeded;
			this.needsMaskUpdate = isNeeded;
			this.needsGlassesUpdate = isNeeded;
		}

		public void apply()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			ItemShirtAsset itemShirtAsset = (this.visualShirtAsset == null || !this.isVisual) ? this.shirtAsset : this.visualShirtAsset;
			ItemPantsAsset itemPantsAsset = (this.visualPantsAsset == null || !this.isVisual) ? this.pantsAsset : this.visualPantsAsset;
			if (this.needsClothesUpdate || this.needsShirtUpdate || this.needsPantsUpdate)
			{
				this.clothing.shirt = null;
				this.clothing.shirtEmission = null;
				this.clothing.shirtMetallic = null;
				this.clothing.flipShirt = false;
				if (itemShirtAsset != null)
				{
					this.clothing.shirt = itemShirtAsset.shirt;
					this.clothing.shirtEmission = itemShirtAsset.emission;
					this.clothing.shirtMetallic = itemShirtAsset.metallic;
					this.clothing.flipShirt = (this.hand && itemShirtAsset.ignoreHand);
				}
				this.clothing.pants = null;
				this.clothing.pantsEmission = null;
				this.clothing.pantsMetallic = null;
				if (itemPantsAsset != null)
				{
					this.clothing.pants = itemPantsAsset.pants;
					this.clothing.pantsEmission = itemPantsAsset.emission;
					this.clothing.pantsMetallic = itemPantsAsset.metallic;
				}
				this.clothing.apply();
				if (this.materialClothing != null)
				{
					this.materialClothing.mainTexture = this.clothing.texture;
					this.materialClothing.SetTexture("_EmissionMap", this.clothing.emission);
					this.materialClothing.SetTexture("_MetallicGlossMap", this.clothing.metallic);
				}
			}
			if (!this.isMine)
			{
				if (this.needsShirtUpdate)
				{
					if (this.isUpper && this.upperSystems != null)
					{
						for (int i = 0; i < this.upperSystems.Length; i++)
						{
							Transform transform = this.upperSystems[i];
							if (transform != null)
							{
								Object.Destroy(transform.gameObject);
							}
						}
						this.isUpper = false;
					}
					if (this.isVisual && this.isMythic && this.visualShirt != 0)
					{
						ushort inventoryMythicID = Provider.provider.economyService.getInventoryMythicID(this.visualShirt);
						if (inventoryMythicID != 0)
						{
							ItemTool.applyEffect(this.upperBones, this.upperSystems, inventoryMythicID, EEffectType.AREA);
							this.isUpper = true;
						}
					}
				}
				if (this.needsPantsUpdate)
				{
					if (this.isLower && this.lowerSystems != null)
					{
						for (int j = 0; j < this.lowerSystems.Length; j++)
						{
							Transform transform2 = this.lowerSystems[j];
							if (transform2 != null)
							{
								Object.Destroy(transform2.gameObject);
							}
						}
						this.isLower = false;
					}
					if (this.isVisual && this.isMythic && this.visualPants != 0)
					{
						ushort inventoryMythicID2 = Provider.provider.economyService.getInventoryMythicID(this.visualPants);
						if (inventoryMythicID2 != 0)
						{
							ItemTool.applyEffect(this.lowerBones, this.lowerSystems, inventoryMythicID2, EEffectType.AREA);
							this.isLower = true;
						}
					}
				}
				ItemHatAsset itemHatAsset = (this.visualHatAsset == null || !this.isVisual) ? this.hatAsset : this.visualHatAsset;
				ItemBackpackAsset itemBackpackAsset = (this.visualBackpackAsset == null || !this.isVisual) ? this.backpackAsset : this.visualBackpackAsset;
				ItemVestAsset itemVestAsset = (this.visualVestAsset == null || !this.isVisual) ? this.vestAsset : this.visualVestAsset;
				ItemMaskAsset itemMaskAsset = (this.visualMaskAsset == null || !this.isVisual) ? this.maskAsset : this.visualMaskAsset;
				ItemGlassesAsset itemGlassesAsset = (this.visualGlassesAsset == null || !this.isVisual || (this.glassesAsset != null && (this.glassesAsset.vision != ELightingVision.NONE || this.glassesAsset.isBlindfold))) ? this.glassesAsset : this.visualGlassesAsset;
				bool flag = true;
				bool flag2 = true;
				if (this.needsHatUpdate)
				{
					if (this.hatModel != null)
					{
						Object.Destroy(this.hatModel.gameObject);
					}
					if (itemHatAsset != null && itemHatAsset.hat != null)
					{
						this.hatModel = Object.Instantiate<GameObject>(itemHatAsset.hat).transform;
						this.hatModel.name = "Hat";
						this.hatModel.transform.parent = this.skull;
						this.hatModel.transform.localPosition = Vector3.zero;
						this.hatModel.transform.localRotation = Quaternion.identity;
						this.hatModel.transform.localScale = Vector3.one;
						if (!this.isView)
						{
							Object.Destroy(this.hatModel.GetComponent<Collider>());
						}
						if (this.isVisual && this.isMythic && this.visualHat != 0)
						{
							ushort inventoryMythicID3 = Provider.provider.economyService.getInventoryMythicID(this.visualHat);
							if (inventoryMythicID3 != 0)
							{
								ItemTool.applyEffect(this.hatModel, inventoryMythicID3, EEffectType.HOOK);
							}
						}
					}
				}
				if (itemHatAsset != null && itemHatAsset.hat != null)
				{
					if (!itemHatAsset.hasHair)
					{
						flag = false;
					}
					if (!itemHatAsset.hasBeard)
					{
						flag2 = false;
					}
				}
				if (this.needsBackpackUpdate)
				{
					if (this.backpackModel != null)
					{
						Object.Destroy(this.backpackModel.gameObject);
					}
					if (itemBackpackAsset != null && itemBackpackAsset.backpack != null)
					{
						this.backpackModel = Object.Instantiate<GameObject>(itemBackpackAsset.backpack).transform;
						this.backpackModel.name = "Backpack";
						this.backpackModel.transform.parent = this.spine;
						this.backpackModel.transform.localPosition = Vector3.zero;
						this.backpackModel.transform.localRotation = Quaternion.identity;
						this.backpackModel.transform.localScale = Vector3.one;
						if (!this.isView)
						{
							Object.Destroy(this.backpackModel.GetComponent<Collider>());
						}
						if (this.isVisual && this.isMythic && this.visualBackpack != 0)
						{
							ushort inventoryMythicID4 = Provider.provider.economyService.getInventoryMythicID(this.visualBackpack);
							if (inventoryMythicID4 != 0)
							{
								ItemTool.applyEffect(this.backpackModel, inventoryMythicID4, EEffectType.HOOK);
							}
						}
						this.backpackModel.gameObject.SetActive(this.hasBackpack);
					}
				}
				if (this.needsVestUpdate)
				{
					if (this.vestModel != null)
					{
						Object.Destroy(this.vestModel.gameObject);
					}
					if (itemVestAsset != null && itemVestAsset.vest != null)
					{
						this.vestModel = Object.Instantiate<GameObject>(itemVestAsset.vest).transform;
						this.vestModel.name = "Vest";
						this.vestModel.transform.parent = this.spine;
						this.vestModel.transform.localPosition = Vector3.zero;
						this.vestModel.transform.localRotation = Quaternion.identity;
						this.vestModel.transform.localScale = Vector3.one;
						if (!this.isView)
						{
							Object.Destroy(this.vestModel.GetComponent<Collider>());
						}
						if (this.isVisual && this.isMythic && this.visualVest != 0)
						{
							ushort inventoryMythicID5 = Provider.provider.economyService.getInventoryMythicID(this.visualVest);
							if (inventoryMythicID5 != 0)
							{
								ItemTool.applyEffect(this.vestModel, inventoryMythicID5, EEffectType.HOOK);
							}
						}
					}
				}
				if (this.needsMaskUpdate)
				{
					if (this.maskModel != null)
					{
						Object.Destroy(this.maskModel.gameObject);
					}
					if (itemMaskAsset != null && itemMaskAsset.mask != null)
					{
						this.maskModel = Object.Instantiate<GameObject>(itemMaskAsset.mask).transform;
						this.maskModel.name = "Mask";
						this.maskModel.transform.parent = this.skull;
						this.maskModel.transform.localPosition = Vector3.zero;
						this.maskModel.transform.localRotation = Quaternion.identity;
						this.maskModel.transform.localScale = Vector3.one;
						if (!this.isView)
						{
							Object.Destroy(this.maskModel.GetComponent<Collider>());
						}
						if (this.isVisual && this.isMythic && this.visualMask != 0)
						{
							ushort inventoryMythicID6 = Provider.provider.economyService.getInventoryMythicID(this.visualMask);
							if (inventoryMythicID6 != 0)
							{
								ItemTool.applyEffect(this.maskModel, inventoryMythicID6, EEffectType.HOOK);
							}
						}
					}
				}
				if (itemMaskAsset != null && itemMaskAsset.mask != null)
				{
					if (!itemMaskAsset.hasHair)
					{
						flag = false;
					}
					if (!itemMaskAsset.hasBeard)
					{
						flag2 = false;
					}
				}
				if (this.needsGlassesUpdate)
				{
					if (this.glassesModel != null)
					{
						Object.Destroy(this.glassesModel.gameObject);
					}
					if (itemGlassesAsset != null && itemGlassesAsset.glasses != null)
					{
						this.glassesModel = Object.Instantiate<GameObject>(itemGlassesAsset.glasses).transform;
						this.glassesModel.name = "Glasses";
						this.glassesModel.transform.parent = this.skull;
						this.glassesModel.transform.localPosition = Vector3.zero;
						this.glassesModel.transform.localRotation = Quaternion.identity;
						this.glassesModel.localScale = Vector3.one;
						if (!this.isView)
						{
							Object.Destroy(this.glassesModel.GetComponent<Collider>());
						}
						if (this.isVisual && this.isMythic && this.visualGlasses != 0)
						{
							ushort inventoryMythicID7 = Provider.provider.economyService.getInventoryMythicID(this.visualGlasses);
							if (inventoryMythicID7 != 0)
							{
								ItemTool.applyEffect(this.glassesModel, inventoryMythicID7, EEffectType.HOOK);
							}
						}
					}
				}
				if (itemGlassesAsset != null && itemGlassesAsset.glasses != null)
				{
					if (!itemGlassesAsset.hasHair)
					{
						flag = false;
					}
					if (!itemGlassesAsset.hasBeard)
					{
						flag2 = false;
					}
				}
				if (this.materialHair != null)
				{
					this.materialHair.color = this.color;
				}
				if (this.hasHair != flag)
				{
					this.hasHair = flag;
					this.needsHairUpdate = true;
				}
				if (this.needsHairUpdate)
				{
					if (this.hairModel != null)
					{
						Object.Destroy(this.hairModel.gameObject);
					}
					if (this.hasHair)
					{
						Object @object = Resources.Load("Hairs/" + this.hair + "/Hair");
						if (@object != null)
						{
							this.hairModel = ((GameObject)Object.Instantiate(@object)).transform;
							this.hairModel.name = "Hair";
							this.hairModel.transform.parent = this.skull;
							this.hairModel.transform.localPosition = Vector3.zero;
							this.hairModel.transform.localRotation = Quaternion.identity;
							this.hairModel.transform.localScale = Vector3.one;
							if (this.hairModel.FindChild("Model_0") != null)
							{
								this.hairModel.FindChild("Model_0").GetComponent<Renderer>().sharedMaterial = this.materialHair;
							}
						}
					}
				}
				if (this.hasBeard != flag2)
				{
					this.hasBeard = flag2;
					this.needsBeardUpdate = true;
				}
				if (this.needsBeardUpdate)
				{
					if (this.beardModel != null)
					{
						Object.Destroy(this.beardModel.gameObject);
					}
					if (this.hasBeard)
					{
						Object object2 = Resources.Load("Beards/" + this.beard + "/Beard");
						if (object2 != null)
						{
							this.beardModel = ((GameObject)Object.Instantiate(object2)).transform;
							this.beardModel.name = "Beard";
							this.beardModel.transform.parent = this.skull;
							this.beardModel.transform.localPosition = Vector3.zero;
							this.beardModel.transform.localRotation = Quaternion.identity;
							this.beardModel.localScale = Vector3.one;
							if (this.beardModel.FindChild("Model_0") != null)
							{
								this.beardModel.FindChild("Model_0").GetComponent<Renderer>().sharedMaterial = this.materialHair;
							}
						}
					}
				}
			}
			this.updateAll(false);
		}

		private void Awake()
		{
			this.spine = base.transform.FindChild("Skeleton").FindChild("Spine");
			this.skull = this.spine.FindChild("Skull");
			this.upperBones = new Transform[]
			{
				this.spine,
				this.spine.FindChild("Left_Shoulder/Left_Arm"),
				this.spine.FindChild("Left_Shoulder/Left_Arm/Left_Hand"),
				this.spine.FindChild("Right_Shoulder/Right_Arm"),
				this.spine.FindChild("Right_Shoulder/Right_Arm/Right_Hand")
			};
			this.upperSystems = new Transform[this.upperBones.Length];
			this.lowerBones = new Transform[]
			{
				this.spine.parent.FindChild("Left_Hip/Left_Leg"),
				this.spine.parent.FindChild("Left_Hip/Left_Leg/Left_Foot"),
				this.spine.parent.FindChild("Right_Hip/Right_Leg"),
				this.spine.parent.FindChild("Right_Hip/Right_Leg/Right_Foot")
			};
			this.lowerSystems = new Transform[this.lowerBones.Length];
			if (!Dedicator.isDedicated)
			{
				if (HumanClothes.humanTexture == null)
				{
					HumanClothes.humanTexture = (Texture2D)Resources.Load("Characters/Human");
				}
				if (HumanClothes.shader == null)
				{
					HumanClothes.shader = Shader.Find("Standard");
				}
				if (base.transform.FindChild("Model_0") != null)
				{
					this.materialClothing = base.transform.FindChild("Model_0").GetComponent<Renderer>().material;
				}
				else if (base.transform.FindChild("Model_1") != null)
				{
					this.materialClothing = base.transform.FindChild("Model_1").GetComponent<Renderer>().material;
				}
				this.materialClothing.name = "Human";
				this.materialClothing.hideFlags = 61;
				this.materialHair = new Material(HumanClothes.shader);
				this.materialHair.name = "Hair";
				this.materialHair.hideFlags = 61;
				this.materialHair.SetFloat("_Glossiness", 0f);
			}
			if (base.transform.FindChild("Model_0") != null)
			{
				base.transform.FindChild("Model_0").GetComponent<Renderer>().sharedMaterial = this.materialClothing;
			}
			if (base.transform.FindChild("Model_1") != null)
			{
				base.transform.FindChild("Model_1").GetComponent<Renderer>().sharedMaterial = this.materialClothing;
			}
			this.clothing = new HumanClothing();
			this.updateAll(true);
		}

		private void OnDestroy()
		{
			if (this.materialClothing != null)
			{
				Object.DestroyImmediate(this.materialClothing);
				this.materialClothing = null;
			}
			if (this.materialHair != null)
			{
				Object.DestroyImmediate(this.materialHair);
				this.materialHair = null;
			}
			if (this.clothing != null)
			{
				this.clothing.destroy();
				this.clothing = null;
			}
		}

		private static Texture2D humanTexture;

		private static Shader shader;

		private Material materialClothing;

		private Material materialHair;

		private Transform spine;

		private Transform skull;

		private Transform[] upperBones;

		private Transform[] upperSystems;

		private Transform[] lowerBones;

		private Transform[] lowerSystems;

		public HumanClothing clothing;

		public bool isMine;

		public bool isView;

		public bool canWearPro;

		private bool _isVisual = true;

		private bool _isMythic = true;

		public bool hand;

		private bool _hasBackpack = true;

		private bool isUpper;

		private bool isLower;

		private ItemShirtAsset visualShirtAsset;

		private ItemPantsAsset visualPantsAsset;

		private ItemHatAsset visualHatAsset;

		private ItemBackpackAsset visualBackpackAsset;

		private ItemVestAsset visualVestAsset;

		private ItemMaskAsset visualMaskAsset;

		private ItemGlassesAsset visualGlassesAsset;

		private int _visualShirt;

		private int _visualPants;

		private int _visualHat;

		public int _visualBackpack;

		public int _visualVest;

		public int _visualMask;

		public int _visualGlasses;

		private ItemShirtAsset _shirtAsset;

		private ItemPantsAsset _pantsAsset;

		private ItemHatAsset _hatAsset;

		private ItemBackpackAsset _backpackAsset;

		private ItemVestAsset _vestAsset;

		private ItemMaskAsset _maskAsset;

		private ItemGlassesAsset _glassesAsset;

		private ushort _shirt;

		private ushort _pants;

		private ushort _hat;

		public ushort _backpack;

		public ushort _vest;

		public ushort _mask;

		public ushort _glasses;

		private byte _face = byte.MaxValue;

		private byte _hair;

		private byte _beard;

		private Color _color;

		private bool hasHair;

		private bool hasBeard;

		private bool needsHairUpdate;

		private bool needsBeardUpdate;

		private bool needsClothesUpdate;

		private bool needsShirtUpdate;

		private bool needsPantsUpdate;

		private bool needsHatUpdate;

		private bool needsBackpackUpdate;

		private bool needsVestUpdate;

		private bool needsMaskUpdate;

		private bool needsGlassesUpdate;
	}
}
