using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerClothing : PlayerCaller
	{
		public event VisualToggleChanged VisualToggleChanged;

		public HumanClothes firstClothes { get; private set; }

		public HumanClothes thirdClothes { get; private set; }

		public HumanClothes characterClothes { get; private set; }

		public bool isVisual
		{
			get
			{
				return this.thirdClothes.isVisual;
			}
		}

		public bool isSkinned { get; private set; }

		public bool isMythic
		{
			get
			{
				return this.thirdClothes.isMythic;
			}
		}

		public ItemShirtAsset shirtAsset
		{
			get
			{
				return this.thirdClothes.shirtAsset;
			}
		}

		public ItemPantsAsset pantsAsset
		{
			get
			{
				return this.thirdClothes.pantsAsset;
			}
		}

		public ItemHatAsset hatAsset
		{
			get
			{
				return this.thirdClothes.hatAsset;
			}
		}

		public ItemBackpackAsset backpackAsset
		{
			get
			{
				return this.thirdClothes.backpackAsset;
			}
		}

		public ItemVestAsset vestAsset
		{
			get
			{
				return this.thirdClothes.vestAsset;
			}
		}

		public ItemMaskAsset maskAsset
		{
			get
			{
				return this.thirdClothes.maskAsset;
			}
		}

		public ItemGlassesAsset glassesAsset
		{
			get
			{
				return this.thirdClothes.glassesAsset;
			}
		}

		public int visualShirt
		{
			get
			{
				return this.thirdClothes.visualShirt;
			}
		}

		public int visualPants
		{
			get
			{
				return this.thirdClothes.visualPants;
			}
		}

		public int visualHat
		{
			get
			{
				return this.thirdClothes.visualHat;
			}
		}

		public int visualBackpack
		{
			get
			{
				return this.thirdClothes.visualBackpack;
			}
		}

		public int visualVest
		{
			get
			{
				return this.thirdClothes.visualVest;
			}
		}

		public int visualMask
		{
			get
			{
				return this.thirdClothes.visualMask;
			}
		}

		public int visualGlasses
		{
			get
			{
				return this.thirdClothes.visualGlasses;
			}
		}

		public ushort shirt
		{
			get
			{
				return this.thirdClothes.shirt;
			}
		}

		public ushort pants
		{
			get
			{
				return this.thirdClothes.pants;
			}
		}

		public ushort hat
		{
			get
			{
				return this.thirdClothes.hat;
			}
		}

		public ushort backpack
		{
			get
			{
				return this.thirdClothes.backpack;
			}
		}

		public ushort vest
		{
			get
			{
				return this.thirdClothes.vest;
			}
		}

		public ushort mask
		{
			get
			{
				return this.thirdClothes.mask;
			}
		}

		public ushort glasses
		{
			get
			{
				return this.thirdClothes.glasses;
			}
		}

		public byte face
		{
			get
			{
				return this.thirdClothes.face;
			}
		}

		public byte hair
		{
			get
			{
				return this.thirdClothes.hair;
			}
		}

		public byte beard
		{
			get
			{
				return this.thirdClothes.beard;
			}
		}

		public Color skin
		{
			get
			{
				return this.thirdClothes.skin;
			}
		}

		public Color color
		{
			get
			{
				return this.thirdClothes.color;
			}
		}

		[SteamCall]
		public void tellUpdateShirtQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.shirtQuality = quality;
				if (this.onShirtUpdated != null)
				{
					this.onShirtUpdated(this.shirt, this.shirtQuality, this.shirtState);
				}
			}
		}

		public void sendUpdateShirtQuality()
		{
			base.channel.send("tellUpdateShirtQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.shirtQuality
			});
		}

		[SteamCall]
		public void tellUpdatePantsQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.pantsQuality = quality;
				if (this.onPantsUpdated != null)
				{
					this.onPantsUpdated(this.pants, this.pantsQuality, this.pantsState);
				}
			}
		}

		public void sendUpdatePantsQuality()
		{
			base.channel.send("tellUpdatePantsQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.pantsQuality
			});
		}

		[SteamCall]
		public void tellUpdateHatQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.hatQuality = quality;
				if (this.onHatUpdated != null)
				{
					this.onHatUpdated(this.hat, this.hatQuality, this.hatState);
				}
			}
		}

		public void sendUpdateHatQuality()
		{
			base.channel.send("tellUpdateHatQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.hatQuality
			});
		}

		[SteamCall]
		public void tellUpdateBackpackQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.backpackQuality = quality;
				if (this.onBackpackUpdated != null)
				{
					this.onBackpackUpdated(this.backpack, this.backpackQuality, this.backpackState);
				}
			}
		}

		public void sendUpdateBackpackQuality()
		{
			base.channel.send("tellUpdateBackpackQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.backpackQuality
			});
		}

		[SteamCall]
		public void tellUpdateVestQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.vestQuality = quality;
				if (this.onVestUpdated != null)
				{
					this.onVestUpdated(this.vest, this.vestQuality, this.vestState);
				}
			}
		}

		public void sendUpdateVestQuality()
		{
			base.channel.send("tellUpdateVestQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.vestQuality
			});
		}

		[SteamCall]
		public void tellUpdateMaskQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.maskQuality = quality;
				if (this.onMaskUpdated != null)
				{
					this.onMaskUpdated(this.mask, this.maskQuality, this.maskState);
				}
			}
		}

		public void sendUpdateMaskQuality()
		{
			base.channel.send("tellUpdateMaskQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.maskQuality
			});
		}

		public void updateMaskQuality()
		{
			if (this.onMaskUpdated != null)
			{
				this.onMaskUpdated(this.mask, this.maskQuality, this.maskState);
			}
		}

		[SteamCall]
		public void tellUpdateGlassesQuality(CSteamID steamID, byte quality)
		{
			if (base.channel.checkServer(steamID))
			{
				this.glassesQuality = quality;
				if (this.onGlassesUpdated != null)
				{
					this.onGlassesUpdated(this.glasses, this.glassesQuality, this.glassesState);
				}
			}
		}

		public void sendUpdateGlassesQuality()
		{
			base.channel.send("tellUpdateGlassesQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.glassesQuality
			});
		}

		[SteamCall]
		public void tellWearShirt(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.shirt = id;
				this.shirtQuality = quality;
				this.shirtState = state;
				this.thirdClothes.apply();
				if (this.firstClothes != null)
				{
					this.firstClothes.shirt = id;
					this.firstClothes.apply();
				}
				if (this.characterClothes != null)
				{
					this.characterClothes.shirt = id;
					this.characterClothes.apply();
					Characters.active.shirt = id;
				}
				if (this.onShirtUpdated != null)
				{
					this.onShirtUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapShirt(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.checkSelection(PlayerInventory.SHIRT))
				{
					if (base.player.equipment.isBusy)
					{
						return;
					}
					base.player.equipment.dequip();
				}
				if (page == 255)
				{
					if (this.shirt == 0)
					{
						return;
					}
					this.askWearShirt(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.SHIRT)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearShirt(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearShirt(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort shirt = this.shirt;
			byte newQuality = this.shirtQuality;
			byte[] newState = this.shirtState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(9, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearShirt", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (shirt != 0)
			{
				base.player.inventory.forceAddItem(new Item(shirt, 1, newQuality, newState), false);
			}
		}

		public void sendSwapShirt(byte page, byte x, byte y)
		{
			if (page == 255 && this.shirt == 0)
			{
				return;
			}
			base.channel.send("askSwapShirt", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellWearPants(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.pants = id;
				this.pantsQuality = quality;
				this.pantsState = state;
				this.thirdClothes.apply();
				if (this.characterClothes != null)
				{
					this.characterClothes.pants = id;
					this.characterClothes.apply();
					Characters.active.pants = id;
				}
				if (this.onPantsUpdated != null)
				{
					this.onPantsUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapPants(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.checkSelection(PlayerInventory.PANTS))
				{
					if (base.player.equipment.isBusy)
					{
						return;
					}
					base.player.equipment.dequip();
				}
				if (page == 255)
				{
					if (this.pants == 0)
					{
						return;
					}
					this.askWearPants(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.PANTS)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearPants(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearPants(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort pants = this.pants;
			byte newQuality = this.pantsQuality;
			byte[] newState = this.pantsState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(9, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearPants", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (pants != 0)
			{
				base.player.inventory.forceAddItem(new Item(pants, 1, newQuality, newState), false);
			}
		}

		public void sendSwapPants(byte page, byte x, byte y)
		{
			if (page == 255 && this.pants == 0)
			{
				return;
			}
			base.channel.send("askSwapPants", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellWearHat(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.hat = id;
				this.hatQuality = quality;
				this.hatState = state;
				this.thirdClothes.apply();
				if (this.characterClothes != null)
				{
					this.characterClothes.hat = id;
					this.characterClothes.apply();
					Characters.active.hat = id;
				}
				if (this.onHatUpdated != null)
				{
					this.onHatUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapHat(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (page == 255)
				{
					if (this.hat == 0)
					{
						return;
					}
					this.askWearHat(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.HAT)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearHat(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearHat(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort hat = this.hat;
			byte newQuality = this.hatQuality;
			byte[] newState = this.hatState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(9, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearHat", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (hat != 0)
			{
				base.player.inventory.forceAddItem(new Item(hat, 1, newQuality, newState), false);
			}
		}

		public void sendSwapHat(byte page, byte x, byte y)
		{
			if (page == 255 && this.hat == 0)
			{
				return;
			}
			base.channel.send("askSwapHat", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellWearBackpack(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.backpack = id;
				this.backpackQuality = quality;
				this.backpackState = state;
				this.thirdClothes.apply();
				if (this.characterClothes != null)
				{
					this.characterClothes.backpack = id;
					this.characterClothes.apply();
					Characters.active.backpack = id;
				}
				if (this.onBackpackUpdated != null)
				{
					this.onBackpackUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapBackpack(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.checkSelection(PlayerInventory.BACKPACK))
				{
					if (base.player.equipment.isBusy)
					{
						return;
					}
					base.player.equipment.dequip();
				}
				if (page == 255)
				{
					if (this.backpack == 0)
					{
						return;
					}
					this.askWearBackpack(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.BACKPACK)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearBackpack(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearBackpack(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort backpack = this.backpack;
			byte newQuality = this.backpackQuality;
			byte[] newState = this.backpackState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(10, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearBackpack", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (backpack != 0)
			{
				base.player.inventory.forceAddItem(new Item(backpack, 1, newQuality, newState), false);
			}
		}

		public void sendSwapBackpack(byte page, byte x, byte y)
		{
			if (page == 255 && this.backpack == 0)
			{
				return;
			}
			base.channel.send("askSwapBackpack", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellVisualToggle(CSteamID steamID, byte index, bool toggle)
		{
			if (base.channel.checkServer(steamID))
			{
				if (index != 0)
				{
					if (index != 1)
					{
						if (index == 2)
						{
							this.thirdClothes.isMythic = toggle;
							this.thirdClothes.apply();
							if (this.firstClothes != null)
							{
								this.firstClothes.isMythic = toggle;
								this.firstClothes.apply();
							}
							if (this.characterClothes != null)
							{
								this.characterClothes.isMythic = toggle;
								this.characterClothes.apply();
							}
							base.player.equipment.applyMythicVisual();
						}
					}
					else
					{
						this.isSkinned = toggle;
						base.player.equipment.applySkinVisual();
						base.player.equipment.applyMythicVisual();
					}
				}
				else
				{
					this.thirdClothes.isVisual = toggle;
					this.thirdClothes.apply();
					if (this.firstClothes != null)
					{
						this.firstClothes.isVisual = toggle;
						this.firstClothes.apply();
					}
					if (this.characterClothes != null)
					{
						this.characterClothes.isVisual = toggle;
						this.characterClothes.apply();
					}
				}
				if (this.VisualToggleChanged != null)
				{
					this.VisualToggleChanged(this);
				}
			}
		}

		[SteamCall]
		public void askVisualToggle(CSteamID steamID, byte index)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (Time.realtimeSinceStartup - this.lastVisualToggle < 0.5f)
				{
					return;
				}
				this.lastVisualToggle = Time.realtimeSinceStartup;
				if (index != 0)
				{
					if (index != 1)
					{
						if (index == 2)
						{
							base.channel.send("tellVisualToggle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								index,
								!this.isMythic
							});
						}
					}
					else
					{
						base.channel.send("tellVisualToggle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							index,
							!this.isSkinned
						});
					}
				}
				else
				{
					base.channel.send("tellVisualToggle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						index,
						!this.isVisual
					});
				}
			}
		}

		public void sendVisualToggle(EVisualToggleType type)
		{
			base.channel.send("askVisualToggle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				(byte)type
			});
		}

		[SteamCall]
		public void tellWearVest(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.vest = id;
				this.vestQuality = quality;
				this.vestState = state;
				this.thirdClothes.apply();
				if (this.characterClothes != null)
				{
					this.characterClothes.vest = id;
					this.characterClothes.apply();
					Characters.active.vest = id;
				}
				if (this.onVestUpdated != null)
				{
					this.onVestUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapVest(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (base.player.equipment.checkSelection(PlayerInventory.VEST))
				{
					if (base.player.equipment.isBusy)
					{
						return;
					}
					base.player.equipment.dequip();
				}
				if (page == 255)
				{
					if (this.vest == 0)
					{
						return;
					}
					this.askWearVest(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.VEST)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearVest(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearVest(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort vest = this.vest;
			byte newQuality = this.vestQuality;
			byte[] newState = this.vestState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(10, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearVest", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (vest != 0)
			{
				base.player.inventory.forceAddItem(new Item(vest, 1, newQuality, newState), false);
			}
		}

		public void sendSwapVest(byte page, byte x, byte y)
		{
			if (page == 255 && this.vest == 0)
			{
				return;
			}
			base.channel.send("askSwapVest", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellWearMask(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.mask = id;
				this.maskQuality = quality;
				this.maskState = state;
				this.thirdClothes.apply();
				if (this.characterClothes != null)
				{
					this.characterClothes.mask = id;
					this.characterClothes.apply();
					Characters.active.mask = id;
				}
				if (this.onMaskUpdated != null)
				{
					this.onMaskUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapMask(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (page == 255)
				{
					if (this.mask == 0)
					{
						return;
					}
					this.askWearMask(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.MASK)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearMask(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearMask(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort mask = this.mask;
			byte newQuality = this.maskQuality;
			byte[] newState = this.maskState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(9, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearMask", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (mask != 0)
			{
				base.player.inventory.forceAddItem(new Item(mask, 1, newQuality, newState), false);
			}
		}

		public void sendSwapMask(byte page, byte x, byte y)
		{
			if (page == 255 && this.mask == 0)
			{
				return;
			}
			base.channel.send("askSwapMask", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellWearGlasses(CSteamID steamID, ushort id, byte quality, byte[] state)
		{
			if (base.channel.checkServer(steamID))
			{
				if (this.thirdClothes == null)
				{
					return;
				}
				this.thirdClothes.glasses = id;
				this.glassesQuality = quality;
				this.glassesState = state;
				this.thirdClothes.apply();
				if (this.characterClothes != null)
				{
					this.characterClothes.glasses = id;
					this.characterClothes.apply();
					Characters.active.glasses = id;
				}
				if (this.onGlassesUpdated != null)
				{
					this.onGlassesUpdated(id, quality, state);
				}
			}
		}

		[SteamCall]
		public void askSwapGlasses(CSteamID steamID, byte page, byte x, byte y)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (page == 255)
				{
					if (this.glasses == 0)
					{
						return;
					}
					this.askWearGlasses(0, 0, new byte[0], true);
				}
				else
				{
					byte index = base.player.inventory.getIndex(page, x, y);
					if (index == 255)
					{
						return;
					}
					ItemJar item = base.player.inventory.getItem(page, index);
					ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item.item.id);
					if (itemAsset != null && itemAsset.type == EItemType.GLASSES)
					{
						base.player.inventory.removeItem(page, index);
						this.askWearGlasses(item.item.id, item.item.quality, item.item.state, true);
					}
				}
			}
		}

		public void askWearGlasses(ushort id, byte quality, byte[] state, bool playEffect)
		{
			ushort glasses = this.glasses;
			byte newQuality = this.glassesQuality;
			byte[] newState = this.glassesState;
			if (id != 0 && playEffect)
			{
				EffectManager.sendEffect(9, EffectManager.SMALL, base.transform.position);
			}
			base.channel.send("tellWearGlasses", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				quality,
				state
			});
			if (glasses != 0)
			{
				base.player.inventory.forceAddItem(new Item(glasses, 1, newQuality, newState), false);
			}
		}

		public void sendSwapGlasses(byte page, byte x, byte y)
		{
			if (page == 255 && this.glasses == 0)
			{
				return;
			}
			base.channel.send("askSwapGlasses", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				page,
				x,
				y
			});
		}

		[SteamCall]
		public void tellClothing(CSteamID steamID, ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState, bool newVisual, bool newSkinned, bool newMythic)
		{
			if (base.channel.checkServer(steamID))
			{
				base.player.animator.unlock();
				if (base.channel.isOwner)
				{
					Player.isLoadingClothing = false;
				}
				if (this.thirdClothes != null)
				{
					this.thirdClothes.face = base.channel.owner.face;
					this.thirdClothes.hair = base.channel.owner.hair;
					this.thirdClothes.beard = base.channel.owner.beard;
					this.thirdClothes.skin = base.channel.owner.skin;
					this.thirdClothes.color = base.channel.owner.color;
					this.thirdClothes.shirt = newShirt;
					this.shirtQuality = newShirtQuality;
					this.shirtState = newShirtState;
					this.thirdClothes.pants = newPants;
					this.pantsQuality = newPantsQuality;
					this.pantsState = newPantsState;
					this.thirdClothes.hat = newHat;
					this.hatQuality = newHatQuality;
					this.hatState = newHatState;
					this.thirdClothes.backpack = newBackpack;
					this.backpackQuality = newBackpackQuality;
					this.backpackState = newBackpackState;
					this.thirdClothes.vest = newVest;
					this.vestQuality = newVestQuality;
					this.vestState = newVestState;
					this.thirdClothes.mask = newMask;
					this.maskQuality = newMaskQuality;
					this.maskState = newMaskState;
					this.thirdClothes.glasses = newGlasses;
					this.glassesQuality = newGlassesQuality;
					this.glassesState = newGlassesState;
					this.thirdClothes.isVisual = newVisual;
					this.thirdClothes.isMythic = newMythic;
					this.thirdClothes.apply();
				}
				if (this.firstClothes != null)
				{
					this.firstClothes.skin = base.channel.owner.skin;
					this.firstClothes.shirt = newShirt;
					this.firstClothes.isVisual = newVisual;
					this.firstClothes.isMythic = newMythic;
					this.firstClothes.apply();
				}
				if (this.characterClothes != null)
				{
					this.characterClothes.face = base.channel.owner.face;
					this.characterClothes.hair = base.channel.owner.hair;
					this.characterClothes.beard = base.channel.owner.beard;
					this.characterClothes.skin = base.channel.owner.skin;
					this.characterClothes.color = base.channel.owner.color;
					this.characterClothes.shirt = newShirt;
					this.characterClothes.pants = newPants;
					this.characterClothes.hat = newHat;
					this.characterClothes.backpack = newBackpack;
					this.characterClothes.vest = newVest;
					this.characterClothes.mask = newMask;
					this.characterClothes.glasses = newGlasses;
					this.characterClothes.isVisual = newVisual;
					this.characterClothes.isMythic = newMythic;
					this.characterClothes.apply();
					Characters.active.shirt = newShirt;
					Characters.active.pants = newPants;
					Characters.active.hat = newHat;
					Characters.active.backpack = newBackpack;
					Characters.active.vest = newVest;
					Characters.active.mask = newMask;
					Characters.active.glasses = newGlasses;
					Characters.hasPlayed = true;
				}
				this.isSkinned = newSkinned;
				base.player.equipment.applySkinVisual();
				base.player.equipment.applyMythicVisual();
				if (this.onShirtUpdated != null)
				{
					this.onShirtUpdated(newShirt, newShirtQuality, newShirtState);
				}
				if (this.onPantsUpdated != null)
				{
					this.onPantsUpdated(newPants, newPantsQuality, newPantsState);
				}
				if (this.onHatUpdated != null)
				{
					this.onHatUpdated(newHat, newHatQuality, newHatState);
				}
				if (this.onBackpackUpdated != null)
				{
					this.onBackpackUpdated(newBackpack, newBackpackQuality, newBackpackState);
				}
				if (this.onVestUpdated != null)
				{
					this.onVestUpdated(newVest, newVestQuality, newVestState);
				}
				if (this.onMaskUpdated != null)
				{
					this.onMaskUpdated(newMask, newMaskQuality, newMaskState);
				}
				if (this.onGlassesUpdated != null)
				{
					this.onGlassesUpdated(newGlasses, newGlassesQuality, newGlassesState);
				}
			}
		}

		public void updateClothes(ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			base.channel.send("tellClothing", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				newShirt,
				newShirtQuality,
				newShirtState,
				newPants,
				newPantsQuality,
				newPantsState,
				newHat,
				newHatQuality,
				newHatState,
				newBackpack,
				newBackpackQuality,
				newBackpackState,
				newVest,
				newVestQuality,
				newVestState,
				newMask,
				newMaskQuality,
				newMaskState,
				newGlasses,
				newGlassesQuality,
				newGlassesState,
				this.isVisual,
				this.isSkinned,
				this.isMythic
			});
		}

		[SteamCall]
		public void askClothing(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				base.channel.send("tellClothing", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					this.shirt,
					this.shirtQuality,
					this.shirtState,
					this.pants,
					this.pantsQuality,
					this.pantsState,
					this.hat,
					this.hatQuality,
					this.hatState,
					this.backpack,
					this.backpackQuality,
					this.backpackState,
					this.vest,
					this.vestQuality,
					this.vestState,
					this.mask,
					this.maskQuality,
					this.maskState,
					this.glasses,
					this.glassesQuality,
					this.glassesState,
					this.isVisual,
					this.isSkinned,
					this.isMythic
				});
			}
		}

		[SteamCall]
		public void tellSwapFace(CSteamID steamID, byte index)
		{
			if (base.channel.checkServer(steamID))
			{
				base.channel.owner.face = index;
				if (this.thirdClothes != null)
				{
					this.thirdClothes.face = base.channel.owner.face;
					this.thirdClothes.apply();
				}
				if (this.characterClothes != null)
				{
					this.characterClothes.face = base.channel.owner.face;
					this.characterClothes.apply();
				}
			}
		}

		[SteamCall]
		public void askSwapFace(CSteamID steamID, byte index)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (Time.realtimeSinceStartup - this.lastFaceSwap < 0.5f)
				{
					return;
				}
				this.lastFaceSwap = Time.realtimeSinceStartup;
				if (index >= Customization.FACES_FREE + Customization.FACES_PRO)
				{
					return;
				}
				if (!base.channel.owner.isPro && index >= Customization.FACES_FREE)
				{
					return;
				}
				base.channel.send("tellSwapFace", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					index
				});
			}
		}

		public void sendSwapFace(byte index)
		{
			base.channel.send("askSwapFace", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				index
			});
		}

		private void onStanceUpdated()
		{
			if (this.thirdClothes == null)
			{
				return;
			}
			if (base.player.movement.getVehicle() != null)
			{
				this.thirdClothes.hasBackpack = (base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].obj == null);
			}
			else
			{
				this.thirdClothes.hasBackpack = true;
			}
		}

		private void onLifeUpdated(bool isDead)
		{
			if (isDead && Provider.isServer)
			{
				bool flag = (!base.player.life.wasPvPDeath) ? Provider.modeConfigData.Players.Lose_Clothes_PvE : Provider.modeConfigData.Players.Lose_Clothes_PvP;
				if (flag)
				{
					if (this.shirt != 0)
					{
						ItemManager.dropItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), base.transform.position, false, true, true);
					}
					if (this.pants != 0)
					{
						ItemManager.dropItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), base.transform.position, false, true, true);
					}
					if (this.hat != 0)
					{
						ItemManager.dropItem(new Item(this.hat, 1, this.hatQuality, this.hatState), base.transform.position, false, true, true);
					}
					if (this.backpack != 0)
					{
						ItemManager.dropItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), base.transform.position, false, true, true);
					}
					if (this.vest != 0)
					{
						ItemManager.dropItem(new Item(this.vest, 1, this.vestQuality, this.vestState), base.transform.position, false, true, true);
					}
					if (this.mask != 0)
					{
						ItemManager.dropItem(new Item(this.mask, 1, this.maskQuality, this.maskState), base.transform.position, false, true, true);
					}
					if (this.glasses != 0)
					{
						ItemManager.dropItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), base.transform.position, false, true, true);
					}
					this.thirdClothes.shirt = 0;
					this.shirtQuality = 0;
					this.thirdClothes.pants = 0;
					this.pantsQuality = 0;
					this.thirdClothes.hat = 0;
					this.hatQuality = 0;
					this.thirdClothes.backpack = 0;
					this.backpackQuality = 0;
					this.thirdClothes.vest = 0;
					this.vestQuality = 0;
					this.thirdClothes.mask = 0;
					this.maskQuality = 0;
					this.thirdClothes.glasses = 0;
					this.glassesQuality = 0;
					this.thirdClothes.isVisual = true;
					this.shirtState = new byte[0];
					this.pantsState = new byte[0];
					this.hatState = new byte[0];
					this.backpackState = new byte[0];
					this.vestState = new byte[0];
					this.maskState = new byte[0];
					this.glassesState = new byte[0];
					this.isSkinned = true;
					this.thirdClothes.isMythic = true;
					base.channel.send("tellClothing", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						this.shirt,
						this.shirtQuality,
						this.shirtState,
						this.pants,
						this.pantsQuality,
						this.pantsState,
						this.hat,
						this.hatQuality,
						this.hatState,
						this.backpack,
						this.backpackQuality,
						this.backpackState,
						this.vest,
						this.vestQuality,
						this.vestState,
						this.mask,
						this.maskQuality,
						this.maskState,
						this.glasses,
						this.glassesQuality,
						this.glassesState,
						this.isVisual,
						this.isSkinned,
						this.isMythic
					});
				}
			}
		}

		public void init()
		{
			base.channel.send("askClothing", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		private void Start()
		{
			if (!Dedicator.isDedicated)
			{
				PlayerStance stance = base.player.stance;
				stance.onStanceUpdated = (StanceUpdated)Delegate.Combine(stance.onStanceUpdated, new StanceUpdated(this.onStanceUpdated));
			}
			if (base.channel.isOwner)
			{
				if (base.player.first != null)
				{
					this.firstClothes = base.player.first.FindChild("Camera").FindChild("Viewmodel").GetComponent<HumanClothes>();
					this.firstClothes.isMine = true;
				}
				if (base.player.third != null)
				{
					this.thirdClothes = base.player.third.GetComponent<HumanClothes>();
				}
				if (base.player.character != null)
				{
					this.characterClothes = base.player.character.GetComponent<HumanClothes>();
				}
			}
			else if (base.player.third != null)
			{
				this.thirdClothes = base.player.third.GetComponent<HumanClothes>();
			}
			if (this.firstClothes != null)
			{
				this.firstClothes.visualShirt = base.channel.owner.shirtItem;
				this.firstClothes.hand = base.channel.owner.hand;
			}
			if (this.thirdClothes != null)
			{
				this.thirdClothes.visualShirt = base.channel.owner.shirtItem;
				this.thirdClothes.visualPants = base.channel.owner.pantsItem;
				this.thirdClothes.visualHat = base.channel.owner.hatItem;
				this.thirdClothes.visualBackpack = base.channel.owner.backpackItem;
				this.thirdClothes.visualVest = base.channel.owner.vestItem;
				this.thirdClothes.visualMask = base.channel.owner.maskItem;
				this.thirdClothes.visualGlasses = base.channel.owner.glassesItem;
				this.thirdClothes.hand = base.channel.owner.hand;
			}
			if (this.characterClothes != null)
			{
				this.characterClothes.visualShirt = base.channel.owner.shirtItem;
				this.characterClothes.visualPants = base.channel.owner.pantsItem;
				this.characterClothes.visualHat = base.channel.owner.hatItem;
				this.characterClothes.visualBackpack = base.channel.owner.backpackItem;
				this.characterClothes.visualVest = base.channel.owner.vestItem;
				this.characterClothes.visualMask = base.channel.owner.maskItem;
				this.characterClothes.visualGlasses = base.channel.owner.glassesItem;
				this.characterClothes.hand = base.channel.owner.hand;
			}
			this.isSkinned = true;
			if (Provider.isServer)
			{
				this.load();
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
			if (Provider.isClient)
			{
				base.Invoke("init", 0.1f);
			}
		}

		public void load()
		{
			this.thirdClothes.visualShirt = base.channel.owner.shirtItem;
			this.thirdClothes.visualPants = base.channel.owner.pantsItem;
			this.thirdClothes.visualHat = base.channel.owner.hatItem;
			this.thirdClothes.visualBackpack = base.channel.owner.backpackItem;
			this.thirdClothes.visualVest = base.channel.owner.vestItem;
			this.thirdClothes.visualMask = base.channel.owner.maskItem;
			this.thirdClothes.visualGlasses = base.channel.owner.glassesItem;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Clothing.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Clothing.dat", 0);
				byte b = block.readByte();
				if (b > 1)
				{
					this.thirdClothes.shirt = block.readUInt16();
					this.shirtQuality = block.readByte();
					this.thirdClothes.pants = block.readUInt16();
					this.pantsQuality = block.readByte();
					this.thirdClothes.hat = block.readUInt16();
					this.hatQuality = block.readByte();
					this.thirdClothes.backpack = block.readUInt16();
					this.backpackQuality = block.readByte();
					this.thirdClothes.vest = block.readUInt16();
					this.vestQuality = block.readByte();
					this.thirdClothes.mask = block.readUInt16();
					this.maskQuality = block.readByte();
					this.thirdClothes.glasses = block.readUInt16();
					this.glassesQuality = block.readByte();
					if (b > 2)
					{
						this.thirdClothes.isVisual = block.readBoolean();
					}
					if (b > 5)
					{
						this.isSkinned = block.readBoolean();
						this.thirdClothes.isMythic = block.readBoolean();
					}
					else
					{
						this.isSkinned = true;
						this.thirdClothes.isMythic = true;
					}
					if (b > 4)
					{
						this.shirtState = block.readByteArray();
						this.pantsState = block.readByteArray();
						this.hatState = block.readByteArray();
						this.backpackState = block.readByteArray();
						this.vestState = block.readByteArray();
						this.maskState = block.readByteArray();
						this.glassesState = block.readByteArray();
					}
					else
					{
						this.shirtState = new byte[0];
						this.pantsState = new byte[0];
						this.hatState = new byte[0];
						this.backpackState = new byte[0];
						this.vestState = new byte[0];
						this.maskState = new byte[0];
						this.glassesState = new byte[0];
						if (this.glasses == 334)
						{
							this.glassesState = new byte[1];
						}
					}
					this.thirdClothes.apply();
					return;
				}
			}
			this.thirdClothes.shirt = 0;
			this.shirtQuality = 0;
			this.thirdClothes.pants = 0;
			this.pantsQuality = 0;
			this.thirdClothes.hat = 0;
			this.hatQuality = 0;
			this.thirdClothes.backpack = 0;
			this.backpackQuality = 0;
			this.thirdClothes.vest = 0;
			this.vestQuality = 0;
			this.thirdClothes.mask = 0;
			this.maskQuality = 0;
			this.thirdClothes.glasses = 0;
			this.glassesQuality = 0;
			this.shirtState = new byte[0];
			this.pantsState = new byte[0];
			this.hatState = new byte[0];
			this.backpackState = new byte[0];
			this.vestState = new byte[0];
			this.maskState = new byte[0];
			this.maskState = new byte[0];
			this.thirdClothes.apply();
		}

		public void save()
		{
			bool flag = (!base.player.life.wasPvPDeath) ? Provider.modeConfigData.Players.Lose_Clothes_PvE : Provider.modeConfigData.Players.Lose_Clothes_PvP;
			if ((base.player.life.isDead && flag) || this.thirdClothes == null)
			{
				if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Clothing.dat"))
				{
					PlayerSavedata.deleteFile(base.channel.owner.playerID, "/Player/Clothing.dat");
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(PlayerClothing.SAVEDATA_VERSION);
				block.writeUInt16(this.thirdClothes.shirt);
				block.writeByte(this.shirtQuality);
				block.writeUInt16(this.thirdClothes.pants);
				block.writeByte(this.pantsQuality);
				block.writeUInt16(this.thirdClothes.hat);
				block.writeByte(this.hatQuality);
				block.writeUInt16(this.thirdClothes.backpack);
				block.writeByte(this.backpackQuality);
				block.writeUInt16(this.thirdClothes.vest);
				block.writeByte(this.vestQuality);
				block.writeUInt16(this.thirdClothes.mask);
				block.writeByte(this.maskQuality);
				block.writeUInt16(this.thirdClothes.glasses);
				block.writeByte(this.glassesQuality);
				block.writeBoolean(this.isVisual);
				block.writeBoolean(this.isSkinned);
				block.writeBoolean(this.isMythic);
				block.writeByteArray(this.shirtState);
				block.writeByteArray(this.pantsState);
				block.writeByteArray(this.hatState);
				block.writeByteArray(this.backpackState);
				block.writeByteArray(this.vestState);
				block.writeByteArray(this.maskState);
				block.writeByteArray(this.glassesState);
				PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Clothing.dat", block);
			}
		}

		public static readonly byte SAVEDATA_VERSION = 6;

		public ShirtUpdated onShirtUpdated;

		public PantsUpdated onPantsUpdated;

		public HatUpdated onHatUpdated;

		public BackpackUpdated onBackpackUpdated;

		public VestUpdated onVestUpdated;

		public MaskUpdated onMaskUpdated;

		public GlassesUpdated onGlassesUpdated;

		private float lastFaceSwap;

		private float lastVisualToggle;

		public byte shirtQuality;

		public byte pantsQuality;

		public byte hatQuality;

		public byte backpackQuality;

		public byte vestQuality;

		public byte maskQuality;

		public byte glassesQuality;

		public byte[] shirtState;

		public byte[] pantsState;

		public byte[] hatState;

		public byte[] backpackState;

		public byte[] vestState;

		public byte[] maskState;

		public byte[] glassesState;
	}
}
