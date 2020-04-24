using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerEquipment : PlayerCaller
	{
		public ushort itemID
		{
			get
			{
				return this._itemID;
			}
		}

		public byte[] state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		public byte quality
		{
			get
			{
				return this._quality;
			}
			set
			{
				if (this.isTurret)
				{
					return;
				}
				this._quality = value;
			}
		}

		public Transform firstModel
		{
			get
			{
				return this._firstModel;
			}
		}

		public Transform thirdModel
		{
			get
			{
				return this._thirdModel;
			}
		}

		public Transform characterModel
		{
			get
			{
				return this._characterModel;
			}
		}

		public ItemAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		public Useable useable
		{
			get
			{
				return this._useable;
			}
		}

		public Transform thirdPrimaryMeleeSlot
		{
			get
			{
				return this._thirdPrimaryMeleeSlot;
			}
		}

		public Transform thirdPrimaryLargeGunSlot
		{
			get
			{
				return this._thirdPrimaryLargeGunSlot;
			}
		}

		public Transform thirdPrimarySmallGunSlot
		{
			get
			{
				return this._thirdPrimarySmallGunSlot;
			}
		}

		public Transform thirdSecondaryMeleeSlot
		{
			get
			{
				return this._thirdSecondaryMeleeSlot;
			}
		}

		public Transform thirdSecondaryGunSlot
		{
			get
			{
				return this._thirdSecondaryGunSlot;
			}
		}

		public Transform characterPrimaryMeleeSlot
		{
			get
			{
				return this._characterPrimaryMeleeSlot;
			}
		}

		public Transform characterPrimaryLargeGunSlot
		{
			get
			{
				return this._characterPrimaryLargeGunSlot;
			}
		}

		public Transform characterPrimarySmallGunSlot
		{
			get
			{
				return this._characterPrimarySmallGunSlot;
			}
		}

		public Transform characterSecondaryMeleeSlot
		{
			get
			{
				return this._characterSecondaryMeleeSlot;
			}
		}

		public Transform characterSecondaryGunSlot
		{
			get
			{
				return this._characterSecondaryGunSlot;
			}
		}

		public Transform firstLeftHook
		{
			get
			{
				return this._firstLeftHook;
			}
		}

		public Transform firstRightHook
		{
			get
			{
				return this._firstRightHook;
			}
		}

		public Transform thirdLeftHook
		{
			get
			{
				return this._thirdLeftHook;
			}
		}

		public Transform thirdRightHook
		{
			get
			{
				return this._thirdRightHook;
			}
		}

		public Transform characterLeftHook
		{
			get
			{
				return this._characterLeftHook;
			}
		}

		public Transform characterRightHook
		{
			get
			{
				return this._characterRightHook;
			}
		}

		public HotkeyInfo[] hotkeys
		{
			get
			{
				return this._hotkeys;
			}
		}

		public bool isSelected
		{
			get
			{
				return this.thirdModel != null && this.useable != null;
			}
		}

		public bool isEquipped
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastEquipped > this.equippedTime;
			}
		}

		public bool isTurret { get; private set; }

		public byte equippedPage
		{
			get
			{
				return this._equippedPage;
			}
		}

		public byte equipped_x
		{
			get
			{
				return this._equipped_x;
			}
		}

		public byte equipped_y
		{
			get
			{
				return this._equipped_y;
			}
		}

		public bool primary
		{
			get
			{
				return this._primary;
			}
		}

		public bool secondary
		{
			get
			{
				return this._secondary;
			}
		}

		public float lastPunching { get; private set; }

		public bool isInspecting
		{
			get
			{
				return Time.realtimeSinceStartup - PlayerEquipment.lastInspect < PlayerEquipment.inspectTime;
			}
		}

		public bool canInspect
		{
			get
			{
				return this.isSelected && this.isEquipped && !this.isBusy && base.player.animator.checkExists("Inspect") && !this.isInspecting && this.useable.canInspect;
			}
		}

		public void inspect()
		{
			base.player.animator.setAnimationSpeed("Inspect", 1f);
			PlayerEquipment.lastInspect = Time.realtimeSinceStartup;
			PlayerEquipment.inspectTime = base.player.animator.getAnimationLength("Inspect");
			base.player.animator.play("Inspect", false);
		}

		public void uninspect()
		{
			base.player.animator.setAnimationSpeed("Inspect", float.MaxValue);
		}

		public bool checkSelection(byte page)
		{
			return page == this.equippedPage;
		}

		public bool checkSelection(byte page, byte x, byte y)
		{
			return page == this.equippedPage && x == this.equipped_x && y == this.equipped_y;
		}

		public void applySkinVisual()
		{
			if (this.firstModel != null && this.firstSkinned != base.player.clothing.isSkinned)
			{
				this.firstSkinned = base.player.clothing.isSkinned;
				if (this.tempFirstMaterial != null)
				{
					Attachments component = this.firstModel.GetComponent<Attachments>();
					if (component != null)
					{
						component.isSkinned = this.firstSkinned;
						component.applyVisual();
					}
					HighlighterTool.rematerialize(this.firstModel, this.tempFirstMaterial, out this.tempFirstMaterial);
				}
			}
			if (this.thirdModel != null && this.thirdSkinned != base.player.clothing.isSkinned)
			{
				this.thirdSkinned = base.player.clothing.isSkinned;
				if (this.tempThirdMaterial != null)
				{
					Attachments component2 = this.thirdModel.GetComponent<Attachments>();
					if (component2 != null)
					{
						component2.isSkinned = this.thirdSkinned;
						component2.applyVisual();
					}
					HighlighterTool.rematerialize(this.thirdModel, this.tempThirdMaterial, out this.tempThirdMaterial);
				}
			}
			if (this.characterModel != null && this.characterSkinned != base.player.clothing.isSkinned)
			{
				this.characterSkinned = base.player.clothing.isSkinned;
				if (this.tempCharacterMaterial != null)
				{
					Attachments component3 = this.characterModel.GetComponent<Attachments>();
					if (component3 != null)
					{
						component3.isSkinned = this.characterSkinned;
						component3.applyVisual();
					}
					HighlighterTool.rematerialize(this.characterModel, this.tempCharacterMaterial, out this.tempCharacterMaterial);
				}
			}
			if (this.thirdSlots != null)
			{
				byte b = 0;
				while ((int)b < this.thirdSlots.Length)
				{
					if (this.thirdSlots[(int)b] != null && this.thirdSkinneds[(int)b] != base.player.clothing.isSkinned)
					{
						this.thirdSkinneds[(int)b] = base.player.clothing.isSkinned;
						if (this.tempThirdMaterials[(int)b] != null)
						{
							Attachments component4 = this.thirdSlots[(int)b].GetComponent<Attachments>();
							if (component4 != null)
							{
								component4.isSkinned = this.thirdSkinneds[(int)b];
								component4.applyVisual();
							}
							HighlighterTool.rematerialize(this.thirdSlots[(int)b], this.tempThirdMaterials[(int)b], out this.tempThirdMaterials[(int)b]);
						}
					}
					if (this.characterSlots != null && this.characterSlots[(int)b] != null && this.characterSkinneds[(int)b] != base.player.clothing.isSkinned)
					{
						this.characterSkinneds[(int)b] = base.player.clothing.isSkinned;
						if (this.tempCharacterMaterials[(int)b] != null)
						{
							Attachments component5 = this.characterSlots[(int)b].GetComponent<Attachments>();
							if (component5 != null)
							{
								component5.isSkinned = this.characterSkinneds[(int)b];
								component5.applyVisual();
							}
							HighlighterTool.rematerialize(this.characterSlots[(int)b], this.tempCharacterMaterials[(int)b], out this.tempCharacterMaterials[(int)b]);
						}
					}
					b += 1;
				}
			}
		}

		public void applyMythicVisual()
		{
			if (this.firstMythic != null)
			{
				this.firstMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
			}
			if (this.thirdMythic != null)
			{
				this.thirdMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
			}
			if (this.characterMythic != null)
			{
				this.characterMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
			}
			if (this.thirdSlots != null)
			{
				byte b = 0;
				while ((int)b < this.thirdSlots.Length)
				{
					if (this.thirdMythics[(int)b] != null)
					{
						this.thirdMythics[(int)b].isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					if (this.characterSlots != null && this.characterMythics[(int)b] != null)
					{
						this.characterMythics[(int)b].isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					b += 1;
				}
			}
		}

		private void updateSlot(byte slot, ushort id, byte[] state)
		{
			if (!Dedicator.isDedicated)
			{
				if (this.thirdSlots == null)
				{
					return;
				}
				if (this.thirdSlots[(int)slot] != null)
				{
					Object.Destroy(this.thirdSlots[(int)slot].gameObject);
					this.thirdSkinneds[(int)slot] = false;
					this.tempThirdMaterials[(int)slot] = null;
					this.thirdMythics[(int)slot] = null;
				}
				if (this.characterSlots != null && this.characterSlots[(int)slot] != null)
				{
					Object.Destroy(this.characterSlots[(int)slot].gameObject);
					this.characterSkinneds[(int)slot] = false;
					this.tempCharacterMaterials[(int)slot] = null;
					this.characterMythics[(int)slot] = null;
				}
				if (base.channel.isOwner)
				{
					if (slot == 0)
					{
						Characters.active.primaryItem = id;
						Characters.active.primaryState = state;
					}
					else if (slot == 1)
					{
						Characters.active.secondaryItem = id;
						Characters.active.secondaryState = state;
					}
				}
				if (id == 0)
				{
					return;
				}
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
				if (itemAsset != null)
				{
					int item = 0;
					ushort skin = 0;
					ushort num = 0;
					if (base.channel.owner.skinItems != null && base.channel.owner.skins != null && base.channel.owner.skins.TryGetValue(id, out item))
					{
						skin = Provider.provider.economyService.getInventorySkinID(item);
						num = Provider.provider.economyService.getInventoryMythicID(item);
					}
					Transform item2 = ItemTool.getItem(id, skin, 100, state, false, itemAsset, out this.tempThirdMaterials[(int)slot]);
					if (slot == 0)
					{
						if (itemAsset.type == EItemType.MELEE)
						{
							item2.transform.parent = this.thirdPrimaryMeleeSlot;
						}
						else if (itemAsset.slot == ESlotType.PRIMARY)
						{
							item2.transform.parent = this.thirdPrimaryLargeGunSlot;
						}
						else
						{
							item2.transform.parent = this.thirdPrimarySmallGunSlot;
						}
					}
					else if (slot == 1)
					{
						if (itemAsset.type == EItemType.MELEE)
						{
							item2.transform.parent = this.thirdSecondaryMeleeSlot;
						}
						else
						{
							item2.transform.parent = this.thirdSecondaryGunSlot;
						}
					}
					item2.localPosition = Vector3.zero;
					item2.localRotation = Quaternion.Euler(0f, 0f, 90f);
					item2.localScale = Vector3.one;
					item2.gameObject.SetActive(false);
					item2.gameObject.SetActive(true);
					Object.Destroy(item2.GetComponent<Collider>());
					Layerer.enemy(item2);
					if (num != 0)
					{
						Transform transform = ItemTool.applyEffect(item2, num, EEffectType.THIRD);
						if (transform != null)
						{
							this.thirdMythics[(int)slot] = transform.GetComponent<MythicLockee>();
						}
					}
					this.thirdSlots[(int)slot] = item2;
					this.thirdSkinneds[(int)slot] = true;
					this.applySkinVisual();
					if (this.thirdMythics[(int)slot] != null)
					{
						this.thirdMythics[(int)slot].isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					if (this.characterSlots != null)
					{
						item2 = ItemTool.getItem(id, skin, 100, state, false, itemAsset, out this.tempCharacterMaterials[(int)slot]);
						if (slot == 0)
						{
							if (itemAsset.type == EItemType.MELEE)
							{
								item2.transform.parent = this.characterPrimaryMeleeSlot;
							}
							else if (itemAsset.slot == ESlotType.PRIMARY)
							{
								item2.transform.parent = this.characterPrimaryLargeGunSlot;
							}
							else
							{
								item2.transform.parent = this.characterPrimarySmallGunSlot;
							}
						}
						else if (slot == 1)
						{
							if (itemAsset.type == EItemType.MELEE)
							{
								item2.transform.parent = this.characterSecondaryMeleeSlot;
							}
							else
							{
								item2.transform.parent = this.characterSecondaryGunSlot;
							}
						}
						item2.localPosition = Vector3.zero;
						item2.localRotation = Quaternion.Euler(0f, 0f, 90f);
						item2.localScale = Vector3.one;
						item2.gameObject.SetActive(false);
						item2.gameObject.SetActive(true);
						Object.Destroy(item2.GetComponent<Collider>());
						Layerer.enemy(item2);
						if (num != 0)
						{
							Transform transform2 = ItemTool.applyEffect(item2, num, EEffectType.THIRD);
							if (transform2 != null)
							{
								this.characterMythics[(int)slot] = transform2.GetComponent<MythicLockee>();
							}
						}
						this.characterSlots[(int)slot] = item2;
						this.characterSkinneds[(int)slot] = true;
						this.applySkinVisual();
						if (this.characterMythics[(int)slot] != null)
						{
							this.characterMythics[(int)slot].isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
						}
					}
				}
			}
		}

		[SteamCall]
		public void askToggleVision(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (!this.hasVision)
				{
					return;
				}
				if (base.player.clothing.glassesState.Length != 1)
				{
					return;
				}
				base.channel.send("tellToggleVision", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
				if (base.player.clothing.glassesAsset != null)
				{
					if (base.player.clothing.glassesAsset.vision == ELightingVision.HEADLAMP)
					{
						EffectManager.sendEffect(8, EffectManager.SMALL, base.transform.position);
					}
					else if (base.player.clothing.glassesAsset.vision == ELightingVision.CIVILIAN || base.player.clothing.glassesAsset.vision == ELightingVision.MILITARY)
					{
						EffectManager.sendEffect(56, EffectManager.SMALL, base.transform.position);
					}
				}
			}
		}

		[SteamCall]
		public void tellToggleVision(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				if (!this.hasVision)
				{
					return;
				}
				if (base.player.clothing.glassesState.Length != 1)
				{
					return;
				}
				base.player.clothing.glassesState[0] = ((base.player.clothing.glassesState[0] != 0) ? 0 : 1);
				this.updateVision();
			}
		}

		[SteamCall]
		public void tellSlot(CSteamID steamID, byte slot, ushort id, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				this.updateSlot(slot, id, state);
			}
		}

		[SteamCall]
		public void tellUpdateStateTemp(CSteamID steamID, byte[] newState)
		{
			if (base.channel.checkServer(steamID))
			{
				this._state = newState;
				if (this.useable != null)
				{
					this.useable.updateState(this.state);
				}
			}
		}

		[SteamCall]
		public void tellUpdateState(CSteamID steamID, byte page, byte index, byte[] newState)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdSlots == null)
				{
					return;
				}
				this._state = newState;
				if (this.slot != 255 && (int)this.slot < this.thirdSlots.Length && this.thirdSlots[(int)this.slot] != null)
				{
					this.updateSlot(this.slot, this.itemID, newState);
					this.thirdSlots[(int)this.slot].gameObject.SetActive(false);
					if (this.characterSlots != null)
					{
						this.characterSlots[(int)this.slot].gameObject.SetActive(false);
					}
				}
				if (base.channel.isOwner || Provider.isServer)
				{
					base.player.inventory.updateState(page, index, this.state);
				}
				if (this.useable != null)
				{
					this.useable.updateState(this.state);
				}
				if (this.characterModel != null)
				{
					Object.Destroy(this.characterModel.gameObject);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.itemID);
					if (itemAsset != null)
					{
						int item = 0;
						ushort skin = 0;
						ushort num = 0;
						if (base.channel.owner.skinItems != null && base.channel.owner.skins != null && base.channel.owner.skins.TryGetValue(this.itemID, out item))
						{
							skin = Provider.provider.economyService.getInventorySkinID(item);
							num = Provider.provider.economyService.getInventoryMythicID(item);
						}
						this._characterModel = ItemTool.getItem(this.itemID, skin, 100, this.state, false, itemAsset, out this.tempCharacterMaterial);
						if (itemAsset.isBackward)
						{
							this.characterModel.transform.parent = this._characterLeftHook;
						}
						else
						{
							this.characterModel.transform.parent = this._characterRightHook;
						}
						this.characterModel.localPosition = Vector3.zero;
						this.characterModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
						this.characterModel.localScale = Vector3.one;
						this.characterModel.gameObject.AddComponent<Rigidbody>();
						this.characterModel.GetComponent<Rigidbody>().useGravity = false;
						this.characterModel.GetComponent<Rigidbody>().isKinematic = true;
						if (num != 0)
						{
							Transform transform = ItemTool.applyEffect(this.characterModel, num, EEffectType.THIRD);
							if (transform != null)
							{
								this.characterMythic = transform.GetComponent<MythicLockee>();
							}
						}
						this.characterSkinned = true;
						this.applySkinVisual();
						if (this.characterMythic != null)
						{
							this.characterMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
						}
					}
				}
			}
		}

		[SteamCall]
		public void tellAskEquip(CSteamID steamID, byte page, byte x, byte y)
		{
			if (!base.channel.checkServer(steamID))
			{
				return;
			}
			this.wasTryingToSelect = true;
			byte index = base.player.inventory.getIndex(page, x, y);
			if (index == 255)
			{
				return;
			}
			ItemJar item = base.player.inventory.getItem(page, index);
			if (item == null)
			{
				return;
			}
			this.equip(page, x, y, item.item.id);
		}

		[SteamCall]
		public void tellEquip(CSteamID steamID, byte page, byte x, byte y, ushort id, byte newQuality, byte[] newState)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdSlots == null)
				{
					return;
				}
				if (this.slot != 255 && (int)this.slot < this.thirdSlots.Length && this.thirdSlots[(int)this.slot] != null)
				{
					this.thirdSlots[(int)this.slot].gameObject.SetActive(true);
					if (this.characterSlots != null)
					{
						this.characterSlots[(int)this.slot].gameObject.SetActive(true);
					}
				}
				this.slot = page;
				if (this.slot != 255 && (int)this.slot < this.thirdSlots.Length && this.thirdSlots[(int)this.slot] != null)
				{
					this.thirdSlots[(int)this.slot].gameObject.SetActive(false);
					if (this.characterSlots != null)
					{
						this.characterSlots[(int)this.slot].gameObject.SetActive(false);
					}
				}
				if (this.isSelected)
				{
					this.useable.dequip();
					if (base.player.life.isDead)
					{
						Object.Destroy(this.useable);
					}
					else
					{
						Object.DestroyImmediate(this.useable);
					}
					this._useable = null;
					this._itemID = 0;
					if (this.firstModel != null)
					{
						Object.Destroy(this.firstModel.gameObject);
					}
					this.firstSkinned = false;
					this.tempFirstMaterial = null;
					this.firstMythic = null;
					if (this.thirdModel != null)
					{
						Object.Destroy(this.thirdModel.gameObject);
					}
					this.thirdSkinned = false;
					this.tempThirdMaterial = null;
					this.thirdMythic = null;
					if (this.characterModel != null)
					{
						Object.Destroy(this.characterModel.gameObject);
					}
					this.characterSkinned = false;
					this.tempCharacterMaterial = null;
					this.characterMythic = null;
					for (int i = 0; i < this.asset.animations.Length; i++)
					{
						base.player.animator.removeAnimation(this.asset.animations[i]);
					}
					base.channel.build();
				}
				this.isBusy = false;
				if (id == 0)
				{
					this._equippedPage = byte.MaxValue;
					this._equipped_x = byte.MaxValue;
					this._equipped_y = byte.MaxValue;
					this._itemID = 0;
					this._asset = null;
					return;
				}
				this._equippedPage = page;
				this._equipped_x = x;
				this._equipped_y = y;
				this._asset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
				Type type = Assets.useableTypes.getType(this.asset.useable);
				if (this.asset != null && type != null && typeof(Useable).IsAssignableFrom(type))
				{
					this._itemID = id;
					this.quality = newQuality;
					this._state = newState;
					int item = 0;
					ushort skin = 0;
					ushort num = 0;
					if (base.channel.owner.skinItems != null && base.channel.owner.skins != null && base.channel.owner.skins.TryGetValue(id, out item))
					{
						skin = Provider.provider.economyService.getInventorySkinID(item);
						num = Provider.provider.economyService.getInventoryMythicID(item);
					}
					if (base.channel.isOwner)
					{
						this._firstModel = ItemTool.getItem(id, skin, this.quality, this.state, true, this.asset, out this.tempFirstMaterial);
						if (this.asset.isBackward)
						{
							this.firstModel.transform.parent = this.firstLeftHook;
						}
						else
						{
							this.firstModel.transform.parent = this.firstRightHook;
						}
						this.firstModel.localPosition = Vector3.zero;
						this.firstModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
						this.firstModel.localScale = Vector3.one;
						this.firstModel.gameObject.SetActive(false);
						this.firstModel.gameObject.SetActive(true);
						this.firstModel.gameObject.AddComponent<Rigidbody>();
						this.firstModel.GetComponent<Rigidbody>().useGravity = false;
						this.firstModel.GetComponent<Rigidbody>().isKinematic = true;
						Object.Destroy(this.firstModel.GetComponent<Collider>());
						Layerer.viewmodel(this.firstModel);
						if (num != 0)
						{
							Transform transform = ItemTool.applyEffect(this.firstModel, num, EEffectType.FIRST);
							if (transform != null)
							{
								this.firstMythic = transform.GetComponent<MythicLockee>();
							}
						}
						this.firstSkinned = true;
						this.applySkinVisual();
						if (this.firstMythic != null)
						{
							this.firstMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
						}
						this._characterModel = ItemTool.getItem(id, skin, this.quality, this.state, false, this.asset, out this.tempCharacterMaterial);
						if (this.asset.isBackward)
						{
							this.characterModel.transform.parent = this.characterLeftHook;
						}
						else
						{
							this.characterModel.transform.parent = this.characterRightHook;
						}
						this.characterModel.localPosition = Vector3.zero;
						this.characterModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
						this.characterModel.localScale = Vector3.one;
						this.characterModel.gameObject.AddComponent<Rigidbody>();
						this.characterModel.GetComponent<Rigidbody>().useGravity = false;
						this.characterModel.GetComponent<Rigidbody>().isKinematic = true;
						if (num != 0)
						{
							Transform transform2 = ItemTool.applyEffect(this.characterModel, num, EEffectType.THIRD);
							if (transform2 != null)
							{
								this.characterMythic = transform2.GetComponent<MythicLockee>();
							}
						}
						this.characterSkinned = true;
						this.applySkinVisual();
						if (this.characterMythic != null)
						{
							this.characterMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
						}
					}
					this._thirdModel = ItemTool.getItem(id, skin, this.quality, this.state, false, this.asset, out this.tempThirdMaterial);
					if (this.asset.isBackward)
					{
						this.thirdModel.transform.parent = this.thirdLeftHook;
					}
					else
					{
						this.thirdModel.transform.parent = this.thirdRightHook;
					}
					this.thirdModel.localPosition = Vector3.zero;
					this.thirdModel.localRotation = Quaternion.Euler(0f, 0f, 90f);
					this.thirdModel.localScale = Vector3.one;
					this.thirdModel.gameObject.SetActive(false);
					this.thirdModel.gameObject.SetActive(true);
					this.thirdModel.gameObject.AddComponent<Rigidbody>();
					this.thirdModel.GetComponent<Rigidbody>().useGravity = false;
					this.thirdModel.GetComponent<Rigidbody>().isKinematic = true;
					Object.Destroy(this.thirdModel.GetComponent<Collider>());
					Layerer.enemy(this.thirdModel);
					if (num != 0)
					{
						Transform transform3 = ItemTool.applyEffect(this.thirdModel, num, EEffectType.THIRD);
						if (transform3 != null)
						{
							this.thirdMythic = transform3.GetComponent<MythicLockee>();
						}
					}
					this.thirdSkinned = true;
					this.applySkinVisual();
					if (this.thirdMythic != null)
					{
						this.thirdMythic.isMythic = (base.player.clothing.isSkinned && base.player.clothing.isMythic);
					}
					for (int j = 0; j < this.asset.animations.Length; j++)
					{
						base.player.animator.addAnimation(this.asset.animations[j]);
					}
					this._useable = (base.gameObject.AddComponent(type) as Useable);
					base.channel.build();
					this.useable.equip();
					this.lastEquipped = Time.realtimeSinceStartup;
					this.equippedTime = base.player.animator.getAnimationLength("Equip");
					if (!Dedicator.isDedicated && this.asset.equip != null)
					{
						base.player.playSound(this.asset.equip, 1f, 0.05f);
					}
				}
			}
		}

		public void tryEquip(byte page, byte x, byte y)
		{
			base.channel.send("tellAskEquip", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		public void tryEquip(byte page, byte x, byte y, byte[] hash)
		{
			if (this.isBusy || !this.canEquip || base.player.life.isDead || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.DRIVING)
			{
				return;
			}
			if (this.isSelected && !this.isEquipped)
			{
				return;
			}
			if (this.isTurret)
			{
				return;
			}
			if ((page == this.equippedPage && x == this.equipped_x && y == this.equipped_y) || page == 255)
			{
				this.dequip();
			}
			else
			{
				if (page < 0 || page >= PlayerInventory.PAGES - 2)
				{
					return;
				}
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index == 255)
				{
					return;
				}
				ItemJar item = base.player.inventory.getItem(page, index);
				if (item == null)
				{
					return;
				}
				if (ItemTool.checkUseable(page, item.item.id))
				{
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset == null)
					{
						return;
					}
					if (base.player.stance.stance == EPlayerStance.SWIM && itemAsset.slot == ESlotType.PRIMARY)
					{
						return;
					}
					if (base.player.animator.gesture == EPlayerGesture.ARREST_START)
					{
						return;
					}
					if (itemAsset.shouldVerifyHash && !Hash.verifyHash(hash, itemAsset.hash))
					{
						return;
					}
					if (item.item.state != null)
					{
						base.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							page,
							x,
							y,
							item.item.id,
							item.item.quality,
							item.item.state
						});
					}
					else
					{
						base.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							page,
							x,
							y,
							item.item.id,
							item.item.quality,
							new byte[0]
						});
					}
				}
			}
		}

		public void turretEquipClient()
		{
			this.isTurret = true;
		}

		public void turretEquipServer(ushort id, byte[] state)
		{
			base.channel.send("tellEquip", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				254,
				254,
				254,
				id,
				100,
				state
			});
			this.tellEquip(Provider.server, 254, 254, 254, id, 100, state);
		}

		public void turretDequipClient()
		{
			this.isTurret = false;
		}

		public void turretDequipServer()
		{
			base.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				byte.MaxValue,
				byte.MaxValue,
				byte.MaxValue,
				0,
				0,
				new byte[0]
			});
		}

		[SteamCall]
		public void askEquip(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				this.tryEquip(page, x, y, hash);
			}
		}

		[SteamCall]
		public void askEquipment(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < PlayerInventory.SLOTS; b += 1)
				{
					ItemJar item = base.player.inventory.getItem(b, 0);
					if (item != null)
					{
						if (item.item.state != null)
						{
							base.channel.send("tellSlot", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								b,
								item.item.id,
								item.item.state
							});
						}
						else
						{
							base.channel.send("tellSlot", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								b,
								item.item.id,
								new byte[0]
							});
						}
					}
					else
					{
						base.channel.send("tellSlot", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							b,
							0,
							new byte[0]
						});
					}
				}
				if (this.isSelected)
				{
					if (this.state != null)
					{
						base.channel.send("tellEquip", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.equippedPage,
							this.equipped_x,
							this.equipped_y,
							this.itemID,
							this.quality,
							this.state
						});
					}
					else
					{
						base.channel.send("tellEquip", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.equippedPage,
							this.equipped_x,
							this.equipped_y,
							this.itemID,
							this.quality,
							new byte[0]
						});
					}
				}
			}
		}

		public void updateState()
		{
			if (this.isTurret)
			{
				return;
			}
			base.player.inventory.updateState(this.equippedPage, base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y), this.state);
		}

		public void updateQuality()
		{
			if (this.isTurret)
			{
				return;
			}
			base.player.inventory.updateQuality(this.equippedPage, base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y), this.quality);
		}

		public void sendUpdateState()
		{
			if (this.isTurret)
			{
				base.channel.send("tellUpdateStateTemp", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.state
				});
				this.tellUpdateStateTemp(Provider.server, this.state);
			}
			else
			{
				byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
				base.channel.send("tellUpdateState", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.equippedPage,
					index,
					this.state
				});
			}
		}

		public void sendUpdateQuality()
		{
			if (this.isTurret)
			{
				return;
			}
			base.player.inventory.sendUpdateQuality(this.equippedPage, this.equipped_x, this.equipped_y, this.quality);
		}

		public void sendSlot(byte slot)
		{
			ItemJar item = base.player.inventory.getItem(slot, 0);
			if (item != null)
			{
				if (item.item.state != null)
				{
					base.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						slot,
						item.item.id,
						item.item.state
					});
				}
				else
				{
					base.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						slot,
						item.item.id,
						new byte[0]
					});
				}
			}
			else
			{
				base.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					slot,
					0,
					new byte[0]
				});
			}
		}

		public void equip(byte page, byte x, byte y, ushort id)
		{
			if (page < 0 || page >= PlayerInventory.PAGES - 2)
			{
				return;
			}
			if (this.isBusy || !this.canEquip || base.player.life.isDead || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.DRIVING)
			{
				return;
			}
			if (this.isSelected && !this.isEquipped)
			{
				return;
			}
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
			if (itemAsset == null)
			{
				return;
			}
			if (base.player.stance.stance == EPlayerStance.SWIM && itemAsset.slot == ESlotType.PRIMARY)
			{
				return;
			}
			if (base.player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			this.lastEquip = Time.realtimeSinceStartup;
			base.channel.send("askEquip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y,
				itemAsset.hash
			});
		}

		public void dequip()
		{
			if (this.isTurret)
			{
				return;
			}
			if (this.ignoreDequip_A)
			{
				return;
			}
			if (Provider.isServer)
			{
				base.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					byte.MaxValue,
					byte.MaxValue,
					byte.MaxValue,
					0,
					0,
					new byte[0]
				});
			}
			else
			{
				if (this.isBusy)
				{
					return;
				}
				base.channel.send("askEquip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					byte.MaxValue,
					byte.MaxValue,
					byte.MaxValue
				});
			}
		}

		public void use()
		{
			if (this.isSelected)
			{
				ushort itemID = this.itemID;
				byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
				ItemJar item = base.player.inventory.getItem(this.equippedPage, index);
				byte equippedPage = this.equippedPage;
				byte equipped_x = this.equipped_x;
				byte equipped_y = this.equipped_y;
				byte rot = item.rot;
				base.player.inventory.removeItem(this.equippedPage, index);
				this.dequip();
				InventorySearch inventorySearch = base.player.inventory.has(itemID);
				if (inventorySearch != null)
				{
					base.player.inventory.askDragItem(base.channel.owner.playerID.steamID, inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, equippedPage, equipped_x, equipped_y, rot);
					this.tryEquip(equippedPage, equipped_x, equipped_y);
				}
			}
		}

		public void useStepA()
		{
			if (this.isSelected)
			{
				byte index = base.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y);
				ItemJar item = base.player.inventory.getItem(this.equippedPage, index);
				this.page_A = this.equippedPage;
				this.x_A = this.equipped_x;
				this.y_A = this.equipped_y;
				this.rot_A = item.rot;
				this.ignoreDequip_A = true;
				base.player.inventory.removeItem(this.equippedPage, index);
				this.ignoreDequip_A = false;
			}
		}

		public void useStepB()
		{
			if (this.isSelected)
			{
				ushort itemID = this.itemID;
				this.dequip();
				InventorySearch inventorySearch = base.player.inventory.has(itemID);
				if (inventorySearch != null)
				{
					base.player.inventory.askDragItem(base.channel.owner.playerID.steamID, inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, this.page_A, this.x_A, this.y_A, this.rot_A);
					this.tryEquip(this.page_A, this.x_A, this.y_A);
				}
			}
		}

		private void punch(EPlayerPunch mode)
		{
			if (base.channel.isOwner)
			{
				base.player.playSound((AudioClip)Resources.Load("Sounds/General/Punch"));
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 1.75f, RayMasks.DAMAGE_CLIENT);
				if (raycastInfo.player != null && PlayerEquipment.DAMAGE_PLAYER_MULTIPLIER.damage > 1f && !base.player.quests.isMemberOfSameGroupAs(raycastInfo.player) && Provider.isPvP)
				{
					PlayerUI.hitmark(0, raycastInfo.point, false, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
				}
				else if ((raycastInfo.zombie != null && PlayerEquipment.DAMAGE_ZOMBIE_MULTIPLIER.damage > 1f) || (raycastInfo.animal != null && PlayerEquipment.DAMAGE_ANIMAL_MULTIPLIER.damage > 1f))
				{
					PlayerUI.hitmark(0, raycastInfo.point, false, (raycastInfo.limb != ELimb.SKULL) ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && PlayerEquipment.DAMAGE_BARRICADE > 1f)
				{
					InteractableDoorHinge component = raycastInfo.transform.GetComponent<InteractableDoorHinge>();
					if (component != null)
					{
						raycastInfo.transform = component.transform.parent.parent;
					}
					ushort id;
					if (ushort.TryParse(raycastInfo.transform.name, out id))
					{
						ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id);
						if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
						{
							PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && PlayerEquipment.DAMAGE_STRUCTURE > 1f)
				{
					ushort id2;
					if (ushort.TryParse(raycastInfo.transform.name, out id2))
					{
						ItemStructureAsset itemStructureAsset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id2);
						if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
						{
							PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.vehicle != null && !raycastInfo.vehicle.isDead && PlayerEquipment.DAMAGE_VEHICLE > 1f)
				{
					if (raycastInfo.vehicle.asset != null && raycastInfo.vehicle.asset.isVulnerable)
					{
						PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && PlayerEquipment.DAMAGE_RESOURCE > 1f)
				{
					byte x;
					byte y;
					ushort index;
					if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
					{
						ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
						if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead)
						{
							PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.transform != null && PlayerEquipment.DAMAGE_OBJECT > 1f)
				{
					InteractableObjectRubble component2 = raycastInfo.transform.GetComponent<InteractableObjectRubble>();
					if (component2 != null)
					{
						raycastInfo.section = component2.getSection(raycastInfo.collider.transform);
						if (!component2.isSectionDead(raycastInfo.section) && component2.asset.rubbleIsVulnerable)
						{
							PlayerUI.hitmark(0, raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				base.player.input.sendRaycast(raycastInfo);
			}
			if (mode == EPlayerPunch.LEFT)
			{
				base.player.animator.play("Punch_Left", false);
				if (Provider.isServer)
				{
					base.player.animator.sendGesture(EPlayerGesture.PUNCH_LEFT, false);
				}
			}
			else if (mode == EPlayerPunch.RIGHT)
			{
				base.player.animator.play("Punch_Right", false);
				if (Provider.isServer)
				{
					base.player.animator.sendGesture(EPlayerGesture.PUNCH_RIGHT, false);
				}
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return;
				}
				InputInfo input = base.player.input.getInput(true);
				if (input == null)
				{
					return;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 36f)
				{
					return;
				}
				DamageTool.impact(input.point, input.normal, input.material, input.type != ERaycastInfoType.NONE && input.type != ERaycastInfoType.OBJECT);
				EPlayerKill eplayerKill = EPlayerKill.NONE;
				uint num = 0u;
				float num2 = 1f;
				num2 *= 1f + base.channel.owner.player.skills.mastery(0, 0) * 0.5f;
				if (input.type == ERaycastInfoType.PLAYER)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.player != null && !base.player.quests.isMemberOfSameGroupAs(input.player) && Provider.isPvP)
					{
						DamageTool.damage(input.player, EDeathCause.PUNCH, input.limb, base.channel.owner.playerID.steamID, input.direction, PlayerEquipment.DAMAGE_PLAYER_MULTIPLIER, num2, true, out eplayerKill);
					}
				}
				else if (input.type == ERaycastInfoType.ZOMBIE)
				{
					if (input.zombie != null)
					{
						DamageTool.damage(input.zombie, input.limb, input.direction, PlayerEquipment.DAMAGE_ZOMBIE_MULTIPLIER, num2, true, out eplayerKill, out num);
						if (base.player.movement.nav != 255)
						{
							input.zombie.alert(base.transform.position, true);
						}
					}
				}
				else if (input.type == ERaycastInfoType.ANIMAL)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.animal != null)
					{
						DamageTool.damage(input.animal, input.limb, input.direction, PlayerEquipment.DAMAGE_ANIMAL_MULTIPLIER, num2, out eplayerKill, out num);
						input.animal.alertPoint(base.transform.position, true);
					}
				}
				else if (input.type == ERaycastInfoType.VEHICLE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					if (input.vehicle != null && input.vehicle.asset != null && input.vehicle.asset.isVulnerable)
					{
						DamageTool.damage(input.vehicle, false, Vector3.zero, false, PlayerEquipment.DAMAGE_VEHICLE, num2, true, out eplayerKill);
					}
				}
				else if (input.type == ERaycastInfoType.BARRICADE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					ushort id3;
					if (input.transform != null && input.transform.CompareTag("Barricade") && ushort.TryParse(input.transform.name, out id3))
					{
						ItemBarricadeAsset itemBarricadeAsset2 = (ItemBarricadeAsset)Assets.find(EAssetType.ITEM, id3);
						if (itemBarricadeAsset2 != null && itemBarricadeAsset2.isVulnerable)
						{
							DamageTool.damage(input.transform, false, PlayerEquipment.DAMAGE_BARRICADE, num2, out eplayerKill);
						}
					}
				}
				else if (input.type == ERaycastInfoType.STRUCTURE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					ushort id4;
					if (input.transform != null && input.transform.CompareTag("Structure") && ushort.TryParse(input.transform.name, out id4))
					{
						ItemStructureAsset itemStructureAsset2 = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id4);
						if (itemStructureAsset2 != null && itemStructureAsset2.isVulnerable)
						{
							DamageTool.damage(input.transform, false, input.direction, PlayerEquipment.DAMAGE_STRUCTURE, num2, out eplayerKill);
						}
					}
				}
				else if (input.type == ERaycastInfoType.RESOURCE)
				{
					this.lastPunching = Time.realtimeSinceStartup;
					byte x2;
					byte y2;
					ushort index2;
					if (input.transform != null && input.transform.CompareTag("Resource") && ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
					{
						ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
						if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead)
						{
							DamageTool.damage(input.transform, input.direction, PlayerEquipment.DAMAGE_RESOURCE, num2, 1f, out eplayerKill, out num);
						}
					}
				}
				else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
				{
					InteractableObjectRubble component3 = input.transform.GetComponent<InteractableObjectRubble>();
					if (component3 != null && !component3.isSectionDead(input.section) && component3.asset.rubbleIsVulnerable)
					{
						DamageTool.damage(input.transform, input.direction, input.section, PlayerEquipment.DAMAGE_OBJECT, num2, out eplayerKill, out num);
					}
				}
				if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
				{
					float num3 = 2f + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num3 *= num3;
					float num4 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num4 *= num4;
					Vector3 forward = base.player.look.aim.forward;
					for (int i = 0; i < Provider.clients.Count; i++)
					{
						if (Provider.clients[i] != base.channel.owner)
						{
							Player player = Provider.clients[i].player;
							if (!(player == null))
							{
								Vector3 vector = player.look.aim.position - base.player.look.aim.position;
								Vector3 vector2 = Vector3.Project(vector, forward);
								if (vector2.sqrMagnitude < num3 && (vector2 - vector).sqrMagnitude < num4)
								{
									base.player.life.markAggressive(false, true);
								}
							}
						}
					}
				}
				if (Level.info.type == ELevelType.HORDE)
				{
					if (input.zombie != null)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(10u);
						}
						else
						{
							base.player.skills.askPay(5u);
						}
					}
					if (eplayerKill == EPlayerKill.ZOMBIE)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(50u);
						}
						else
						{
							base.player.skills.askPay(25u);
						}
					}
				}
				else
				{
					if (eplayerKill == EPlayerKill.PLAYER)
					{
						base.player.sendStat(EPlayerStat.KILLS_PLAYERS);
						if (Level.info.type == ELevelType.ARENA)
						{
							base.player.skills.askPay(100u);
						}
					}
					else if (eplayerKill == EPlayerKill.ZOMBIE)
					{
						base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
					}
					else if (eplayerKill == EPlayerKill.MEGA)
					{
						base.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
					}
					else if (eplayerKill == EPlayerKill.ANIMAL)
					{
						base.player.sendStat(EPlayerStat.KILLS_ANIMALS);
					}
					else if (eplayerKill == EPlayerKill.RESOURCE)
					{
						base.player.sendStat(EPlayerStat.FOUND_RESOURCES);
					}
					if (num > 0u)
					{
						base.player.skills.askPay(num);
					}
				}
			}
		}

		public void simulate(uint simulation, bool inputPrimary, bool inputSecondary, bool inputSteady)
		{
			if (base.player.stance.stance == EPlayerStance.CLIMB || (base.player.stance.stance == EPlayerStance.DRIVING && !this.isTurret) || (base.player.stance.stance == EPlayerStance.SWIM && this.asset != null && this.asset.slot == ESlotType.PRIMARY))
			{
				if (this.isSelected && Provider.isServer)
				{
					this.dequip();
				}
				return;
			}
			if (Time.realtimeSinceStartup - this.lastEquip < 0.1f || base.player.life.isDead)
			{
				return;
			}
			if (base.player.movement.isSafe)
			{
				if (this.asset == null)
				{
					if (base.player.movement.isSafeInfo.noWeapons)
					{
						return;
					}
				}
				else if (this.asset.isDangerous)
				{
					if (this.asset is ItemBarricadeAsset || this.asset is ItemStructureAsset)
					{
						if (base.player.movement.isSafeInfo.noBuildables)
						{
							inputPrimary = false;
							inputSecondary = false;
						}
					}
					else if (base.player.movement.isSafeInfo.noWeapons)
					{
						inputPrimary = false;
						inputSecondary = false;
					}
				}
			}
			if (Level.info != null && Level.info.type != ELevelType.SURVIVAL && this.asset == null)
			{
				return;
			}
			if (base.player.stance.stance == EPlayerStance.SWIM && this.asset == null)
			{
				return;
			}
			if (base.player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			if (this.isSelected)
			{
				if (inputPrimary != this.lastPrimary)
				{
					this.lastPrimary = inputPrimary;
					if (this.isEquipped)
					{
						if (inputPrimary)
						{
							this.useable.startPrimary();
						}
						else
						{
							this.useable.stopPrimary();
						}
					}
				}
				if (inputSecondary != this.lastSecondary)
				{
					this.lastSecondary = inputSecondary;
					if (this.isEquipped)
					{
						if (inputSecondary)
						{
							this.useable.startSecondary();
						}
						else
						{
							this.useable.stopSecondary();
						}
					}
				}
				if (this.isSelected && this.isEquipped)
				{
					this.useable.simulate(simulation, inputSteady);
				}
			}
			else
			{
				if (inputPrimary != this.lastPrimary)
				{
					this.lastPrimary = inputPrimary;
					if (!this.isBusy && base.player.stance.stance != EPlayerStance.PRONE && inputPrimary && simulation - this.lastPunch > 5u)
					{
						this.lastPunch = simulation;
						this.punch(EPlayerPunch.LEFT);
					}
				}
				if (inputSecondary != this.lastSecondary)
				{
					this.lastSecondary = inputSecondary;
					if (!this.isBusy && base.player.stance.stance != EPlayerStance.PRONE && inputSecondary && simulation - this.lastPunch > 5u)
					{
						this.lastPunch = simulation;
						this.punch(EPlayerPunch.RIGHT);
					}
				}
			}
		}

		public void tock(uint clock)
		{
			if (this.isSelected && this.isEquipped)
			{
				this.useable.tock(clock);
			}
		}

		private void updateVision()
		{
			if (this.hasVision && base.player.clothing.glassesState[0] != 0)
			{
				if (base.player.clothing.glassesAsset.vision == ELightingVision.HEADLAMP)
				{
					base.player.updateHeadlamp(true);
					if (base.channel.isOwner)
					{
						LevelLighting.vision = ELightingVision.NONE;
						LevelLighting.updateLighting();
						PlayerLifeUI.updateGrayscale();
					}
				}
				else
				{
					base.player.updateHeadlamp(false);
					if (base.channel.isOwner)
					{
						LevelLighting.vision = ((base.player.look.perspective != EPlayerPerspective.FIRST) ? ELightingVision.NONE : base.player.clothing.glassesAsset.vision);
						LevelLighting.updateLighting();
						PlayerLifeUI.updateGrayscale();
					}
				}
				base.player.updateGlassesLights(true);
			}
			else
			{
				base.player.updateHeadlamp(false);
				if (base.channel.isOwner)
				{
					LevelLighting.vision = ELightingVision.NONE;
					LevelLighting.updateLighting();
					PlayerLifeUI.updateGrayscale();
				}
				base.player.updateGlassesLights(false);
			}
		}

		private void onVisionUpdated(bool isViewing)
		{
			if (isViewing)
			{
				this.warp = (Random.value < 0.25f);
			}
			else
			{
				this.warp = false;
			}
		}

		private void onPerspectiveUpdated(EPlayerPerspective perspective)
		{
			if (this.hasVision)
			{
				this.updateVision();
			}
		}

		private void onGlassesUpdated(ushort id, byte quality, byte[] state)
		{
			this.hasVision = (id != 0 && base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.vision != ELightingVision.NONE);
			this.updateVision();
		}

		private void OnVisualToggleChanged(PlayerClothing sender)
		{
			if (this.hasVision)
			{
				this.updateVision();
			}
		}

		private void onLifeUpdated(bool isDead)
		{
			if (isDead)
			{
				for (byte b = 0; b < PlayerInventory.SLOTS; b += 1)
				{
					this.updateSlot(b, 0, new byte[0]);
				}
				if (Provider.isServer)
				{
					this.dequip();
				}
				this.isBusy = false;
				this.canEquip = true;
				this._equippedPage = byte.MaxValue;
				this._equipped_x = byte.MaxValue;
				this._equipped_y = byte.MaxValue;
			}
		}

		private void hotkey(byte button)
		{
			byte b = button - 2;
			if (!PlayerUI.window.showCursor)
			{
				if (!this.isBusy)
				{
					if (button < PlayerInventory.SLOTS)
					{
						ItemJar item = base.player.inventory.getItem(button, 0);
						if (item != null)
						{
							this.equip(button, item.x, item.y, item.item.id);
						}
						else if (this.isSelected && this.isEquipped)
						{
							this.dequip();
						}
					}
					else
					{
						HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
						if (hotkeyInfo.id != 0)
						{
							this.equip(hotkeyInfo.page, hotkeyInfo.x, hotkeyInfo.y, hotkeyInfo.id);
						}
						else if (this.isSelected && this.isEquipped)
						{
							this.dequip();
						}
					}
				}
			}
			else if (button >= PlayerInventory.SLOTS && PlayerDashboardInventoryUI.active)
			{
				if (PlayerDashboardInventoryUI.selectedPage >= PlayerInventory.SLOTS && PlayerDashboardInventoryUI.selectedPage < PlayerInventory.STORAGE)
				{
					if (ItemTool.checkUseable(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selectedJar.item.id))
					{
						HotkeyInfo hotkeyInfo2 = this.hotkeys[(int)b];
						hotkeyInfo2.id = PlayerDashboardInventoryUI.selectedJar.item.id;
						hotkeyInfo2.page = PlayerDashboardInventoryUI.selectedPage;
						hotkeyInfo2.x = PlayerDashboardInventoryUI.selected_x;
						hotkeyInfo2.y = PlayerDashboardInventoryUI.selected_y;
						PlayerDashboardInventoryUI.closeSelection();
						for (int i = 0; i < this.hotkeys.Length; i++)
						{
							if (i != (int)b)
							{
								HotkeyInfo hotkeyInfo3 = this.hotkeys[i];
								if (hotkeyInfo3.page == hotkeyInfo2.page && hotkeyInfo3.x == hotkeyInfo2.x && hotkeyInfo3.y == hotkeyInfo2.y)
								{
									hotkeyInfo3.id = 0;
									hotkeyInfo3.page = byte.MaxValue;
									hotkeyInfo3.x = byte.MaxValue;
									hotkeyInfo3.y = byte.MaxValue;
								}
							}
						}
						if (this.onHotkeysUpdated != null)
						{
							this.onHotkeysUpdated();
						}
					}
				}
				else if (PlayerDashboardInventoryUI.selectedPage == 255)
				{
					HotkeyInfo hotkeyInfo4 = this.hotkeys[(int)b];
					hotkeyInfo4.id = 0;
					hotkeyInfo4.page = byte.MaxValue;
					hotkeyInfo4.x = byte.MaxValue;
					hotkeyInfo4.y = byte.MaxValue;
					if (this.onHotkeysUpdated != null)
					{
						this.onHotkeysUpdated();
					}
				}
			}
		}

		public void init()
		{
			base.channel.send("askEquipment", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		private void Update()
		{
			if (base.channel.isOwner)
			{
				if (!PlayerUI.window.showCursor && !base.player.workzone.isBuilding && (base.player.movement.getVehicle() == null || base.player.look.perspective == EPlayerPerspective.FIRST))
				{
					this._primary = (this.prim || Input.GetKey(ControlsSettings.primary));
					if (ControlsSettings.aiming == EControlMode.TOGGLE && this.asset != null && (this.asset.type == EItemType.GUN || this.asset.type == EItemType.OPTIC))
					{
						if ((this.sec || Input.GetKey(ControlsSettings.secondary)) != this.flipSecondary)
						{
							this.flipSecondary = (this.sec || Input.GetKey(ControlsSettings.secondary));
							if (this.flipSecondary)
							{
								this._secondary = !this.secondary;
							}
						}
					}
					else
					{
						this._secondary = (this.sec || Input.GetKey(ControlsSettings.secondary));
						this.flipSecondary = this.secondary;
					}
					this.prim = false;
					this.sec = false;
					if (this.warp)
					{
						bool primary = this.primary;
						this._primary = this.secondary;
						this._secondary = primary;
					}
				}
				else
				{
					this._primary = false;
					this._secondary = false;
				}
			}
			this.wasTryingToSelect = false;
			if (base.channel.isOwner)
			{
				if (!PlayerUI.window.showCursor && !base.player.workzone.isBuilding)
				{
					if (Input.GetKeyDown(ControlsSettings.vision) && this.hasVision && !PlayerLifeUI.scopeOverlay.isVisible)
					{
						base.channel.send("askToggleVision", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
					}
					if (Input.GetKey(ControlsSettings.primary))
					{
						this.prim = true;
					}
					if (Input.GetKey(ControlsSettings.secondary))
					{
						this.sec = true;
					}
					if (Input.GetKeyDown(ControlsSettings.dequip) && this.isSelected && !this.isBusy && this.isEquipped)
					{
						this.dequip();
					}
				}
				if (Input.GetKeyDown(49))
				{
					this.hotkey(0);
				}
				if (Input.GetKeyDown(50))
				{
					this.hotkey(1);
				}
				if (Input.GetKeyDown(51))
				{
					this.hotkey(2);
				}
				if (Input.GetKeyDown(52))
				{
					this.hotkey(3);
				}
				if (Input.GetKeyDown(53))
				{
					this.hotkey(4);
				}
				if (Input.GetKeyDown(54))
				{
					this.hotkey(5);
				}
				if (Input.GetKeyDown(55))
				{
					this.hotkey(6);
				}
				if (Input.GetKeyDown(56))
				{
					this.hotkey(7);
				}
				if (Input.GetKeyDown(57))
				{
					this.hotkey(8);
				}
				if (Input.GetKeyDown(48))
				{
					this.hotkey(9);
				}
			}
			if (this.isSelected)
			{
				this.useable.tick();
			}
		}

		private void Start()
		{
			this.hasVision = (base.player.clothing.glasses != 0 && base.player.clothing.glassesAsset != null && base.player.clothing.glassesAsset.vision != ELightingVision.NONE);
			this.updateVision();
			this.thirdSlots = new Transform[(int)PlayerInventory.SLOTS];
			this.thirdSkinneds = new bool[(int)PlayerInventory.SLOTS];
			this.tempThirdMaterials = new Material[(int)PlayerInventory.SLOTS];
			this.thirdMythics = new MythicLockee[(int)PlayerInventory.SLOTS];
			if (base.channel.isOwner && base.player.character != null)
			{
				this.characterSlots = new Transform[(int)PlayerInventory.SLOTS];
				this.characterSkinneds = new bool[(int)PlayerInventory.SLOTS];
				this.tempCharacterMaterials = new Material[(int)PlayerInventory.SLOTS];
				this.characterMythics = new MythicLockee[(int)PlayerInventory.SLOTS];
			}
			this.warp = false;
			this._equippedPage = byte.MaxValue;
			this._equipped_x = byte.MaxValue;
			this._equipped_y = byte.MaxValue;
			this.isBusy = false;
			this.canEquip = true;
			if (base.player.third != null)
			{
				this._thirdPrimaryMeleeSlot = base.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Primary_Melee");
				this._thirdPrimaryLargeGunSlot = base.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Primary_Large_Gun");
				this._thirdPrimarySmallGunSlot = base.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Primary_Small_Gun");
				this._thirdSecondaryMeleeSlot = base.player.animator.thirdSkeleton.FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Melee");
				this._thirdSecondaryGunSlot = base.player.animator.thirdSkeleton.FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Gun");
			}
			if (base.channel.isOwner)
			{
				this._characterPrimaryMeleeSlot = base.player.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Melee");
				this._characterPrimaryLargeGunSlot = base.player.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Large_Gun");
				this._characterPrimarySmallGunSlot = base.player.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Small_Gun");
				this._characterSecondaryMeleeSlot = base.player.character.FindChild("Skeleton").FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Melee");
				this._characterSecondaryGunSlot = base.player.character.FindChild("Skeleton").FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Gun");
			}
			if (base.player.first != null)
			{
				this._firstLeftHook = base.player.animator.firstSkeleton.FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").FindChild("Left_Hand").FindChild("Left_Hook");
				this._firstRightHook = base.player.animator.firstSkeleton.FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
			}
			if (base.player.third != null)
			{
				this._thirdLeftHook = base.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").FindChild("Left_Hand").FindChild("Left_Hook");
				this._thirdRightHook = base.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
			}
			if (base.channel.isOwner && base.player.character != null)
			{
				this._characterLeftHook = base.player.character.transform.FindChild("Skeleton").FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").FindChild("Left_Hand").FindChild("Left_Hook");
				this._characterRightHook = base.player.character.transform.FindChild("Skeleton").FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
			}
			if (base.channel.isOwner || Provider.isServer)
			{
				PlayerLife life = base.player.life;
				life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
			}
			PlayerClothing clothing = base.player.clothing;
			clothing.onGlassesUpdated = (GlassesUpdated)Delegate.Combine(clothing.onGlassesUpdated, new GlassesUpdated(this.onGlassesUpdated));
			base.player.clothing.VisualToggleChanged += this.OnVisualToggleChanged;
			if (base.channel.isOwner)
			{
				this._hotkeys = new HotkeyInfo[8];
				byte b = 0;
				while ((int)b < this.hotkeys.Length)
				{
					this.hotkeys[(int)b] = new HotkeyInfo();
					b += 1;
				}
				this.load();
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			PlayerLife life2 = base.player.life;
			life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			base.Invoke("init", 0.1f);
		}

		private void OnDestroy()
		{
			if (this.useable != null)
			{
				this.useable.dequip();
			}
			if (base.channel.isOwner)
			{
				this.save();
			}
		}

		private void load()
		{
			if (ReadWrite.fileExists(string.Concat(new object[]
			{
				"/Worlds/Hotkeys/Equip_",
				Provider.currentServerInfo.ip,
				"_",
				Provider.currentServerInfo.port,
				"_",
				Characters.selected,
				".dat"
			}), false))
			{
				Block block = ReadWrite.readBlock(string.Concat(new object[]
				{
					"/Worlds/Hotkeys/Equip_",
					Provider.currentServerInfo.ip,
					"_",
					Provider.currentServerInfo.port,
					"_",
					Characters.selected,
					".dat"
				}), false, 0);
				block.readByte();
				byte b = 0;
				while ((int)b < this.hotkeys.Length)
				{
					HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
					hotkeyInfo.id = block.readUInt16();
					hotkeyInfo.page = block.readByte();
					hotkeyInfo.x = block.readByte();
					hotkeyInfo.y = block.readByte();
					b += 1;
				}
			}
		}

		private void save()
		{
			bool flag = false;
			byte b = 0;
			while ((int)b < this.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo = this.hotkeys[(int)b];
				if (hotkeyInfo.id != 0 || (hotkeyInfo.page != 255 && hotkeyInfo.x != 255 && hotkeyInfo.y != 255))
				{
					flag = true;
					break;
				}
				b += 1;
			}
			if (!flag)
			{
				if (ReadWrite.fileExists(string.Concat(new object[]
				{
					"/Worlds/Hotkeys/Equip_",
					Provider.currentServerInfo.ip,
					"_",
					Provider.currentServerInfo.port,
					"_",
					Characters.selected,
					".dat"
				}), false))
				{
					ReadWrite.deleteFile(string.Concat(new object[]
					{
						"/Worlds/Hotkeys/Equip_",
						Provider.currentServerInfo.ip,
						"_",
						Provider.currentServerInfo.port,
						"_",
						Characters.selected,
						".dat"
					}), false);
				}
				return;
			}
			Block block = new Block();
			block.writeByte(PlayerEquipment.SAVEDATA_VERSION);
			byte b2 = 0;
			while ((int)b2 < this.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo2 = this.hotkeys[(int)b2];
				block.writeUInt16(hotkeyInfo2.id);
				block.writeByte(hotkeyInfo2.page);
				block.writeByte(hotkeyInfo2.x);
				block.writeByte(hotkeyInfo2.y);
				b2 += 1;
			}
			ReadWrite.writeBlock(string.Concat(new object[]
			{
				"/Worlds/Hotkeys/Equip_",
				Provider.currentServerInfo.ip,
				"_",
				Provider.currentServerInfo.port,
				"_",
				Characters.selected,
				".dat"
			}), false, block);
		}

		public static readonly byte SAVEDATA_VERSION = 1;

		private static readonly float DAMAGE_BARRICADE = 2f;

		private static readonly float DAMAGE_STRUCTURE = 2f;

		private static readonly float DAMAGE_VEHICLE = 0f;

		private static readonly float DAMAGE_RESOURCE = 0f;

		private static readonly float DAMAGE_OBJECT = 5f;

		private static readonly PlayerDamageMultiplier DAMAGE_PLAYER_MULTIPLIER = new PlayerDamageMultiplier(15f, 0.6f, 0.6f, 0.8f, 1.1f);

		private static readonly ZombieDamageMultiplier DAMAGE_ZOMBIE_MULTIPLIER = new ZombieDamageMultiplier(15f, 0.3f, 0.3f, 0.6f, 1.1f);

		private static readonly AnimalDamageMultiplier DAMAGE_ANIMAL_MULTIPLIER = new AnimalDamageMultiplier(15f, 0.3f, 0.6f, 1.1f);

		private ushort _itemID;

		private byte[] _state;

		private byte _quality;

		private Transform[] thirdSlots;

		private bool[] thirdSkinneds;

		private Material[] tempThirdMaterials;

		private MythicLockee[] thirdMythics;

		private Transform[] characterSlots;

		private bool[] characterSkinneds;

		private Material[] tempCharacterMaterials;

		private MythicLockee[] characterMythics;

		private Transform _firstModel;

		private bool firstSkinned;

		private Material tempFirstMaterial;

		private MythicLockee firstMythic;

		private Transform _thirdModel;

		private bool thirdSkinned;

		private Material tempThirdMaterial;

		private MythicLockee thirdMythic;

		private Transform _characterModel;

		private bool characterSkinned;

		private Material tempCharacterMaterial;

		private MythicLockee characterMythic;

		private ItemAsset _asset;

		private Useable _useable;

		private Transform _thirdPrimaryMeleeSlot;

		private Transform _thirdPrimaryLargeGunSlot;

		private Transform _thirdPrimarySmallGunSlot;

		private Transform _thirdSecondaryMeleeSlot;

		private Transform _thirdSecondaryGunSlot;

		private Transform _characterPrimaryMeleeSlot;

		private Transform _characterPrimaryLargeGunSlot;

		private Transform _characterPrimarySmallGunSlot;

		private Transform _characterSecondaryMeleeSlot;

		private Transform _characterSecondaryGunSlot;

		private Transform _firstLeftHook;

		private Transform _firstRightHook;

		private Transform _thirdLeftHook;

		private Transform _thirdRightHook;

		private Transform _characterLeftHook;

		private Transform _characterRightHook;

		private HotkeyInfo[] _hotkeys;

		public HotkeysUpdated onHotkeysUpdated;

		public bool wasTryingToSelect;

		public bool isBusy;

		public bool canEquip;

		private byte slot = byte.MaxValue;

		private bool warp;

		private byte _equippedPage;

		private byte _equipped_x;

		private byte _equipped_y;

		private bool prim;

		private bool lastPrimary;

		private bool _primary;

		private bool sec;

		private bool flipSecondary;

		private bool lastSecondary;

		private bool _secondary;

		private bool hasVision;

		private float lastEquip;

		private float lastEquipped;

		private float equippedTime;

		private uint lastPunch;

		private static float lastInspect;

		private static float inspectTime;

		protected byte page_A;

		protected byte x_A;

		protected byte y_A;

		protected byte rot_A;

		protected bool ignoreDequip_A;
	}
}
