using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableStorage : Interactable, IManualOnDestroy
	{
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		public Items items
		{
			get
			{
				return this._items;
			}
		}

		public bool isDisplay
		{
			get
			{
				return this._isDisplay;
			}
		}

		private void onStateUpdated()
		{
			if (this.isDisplay)
			{
				this.updateDisplay();
				if (Dedicator.isDedicated)
				{
					BarricadeManager.sendStorageDisplay(base.transform, this.displayItem, this.displaySkin, this.displayMythic);
				}
				this.refreshDisplay();
			}
			this.rebuildState();
		}

		public void rebuildState()
		{
			if (this.items == null)
			{
				return;
			}
			SteamPacker.openWrite(0);
			SteamPacker.write(this.owner, this.group, this.items.getItemCount());
			for (byte b = 0; b < this.items.getItemCount(); b += 1)
			{
				ItemJar item = this.items.getItem(b);
				SteamPacker.write(item.x, item.y, item.rot, item.item.id, item.item.amount, item.item.quality, item.item.state);
			}
			if (this.isDisplay)
			{
				SteamPacker.write(this.displaySkin);
				SteamPacker.write(this.displayMythic);
				SteamPacker.write(this.rot_comp);
			}
			int size;
			byte[] state = SteamPacker.closeWrite(out size);
			BarricadeManager.updateState(base.transform, state, size);
		}

		private void updateDisplay()
		{
			if (this.items != null && this.items.getItemCount() > 0)
			{
				if (this.displayItem == null || this.items.getItem(0).item != this.displayItem)
				{
					if (this.displayItem != null)
					{
						this.displaySkin = 0;
						this.displayMythic = 0;
					}
					this.displayItem = this.items.getItem(0).item;
					int item;
					if (this.opener != null && this.opener.channel.owner.skinItems != null && this.opener.channel.owner.skins != null && this.opener.channel.owner.skins.TryGetValue(this.displayItem.id, out item))
					{
						this.displaySkin = Provider.provider.economyService.getInventorySkinID(item);
						this.displayMythic = Provider.provider.economyService.getInventoryMythicID(item);
					}
				}
			}
			else
			{
				this.displayItem = null;
				this.displaySkin = 0;
				this.displayMythic = 0;
			}
		}

		public void setDisplay(ushort id, byte quality, byte[] state, ushort skin, ushort mythic)
		{
			if (id == 0)
			{
				this.displayItem = null;
			}
			else
			{
				this.displayItem = new Item(id, 0, quality, state);
			}
			this.displaySkin = skin;
			this.displayMythic = mythic;
			this.refreshDisplay();
		}

		public byte getRotation(byte rot_x, byte rot_y, byte rot_z)
		{
			return (byte)((int)rot_x << 4 | (int)rot_y << 2 | (int)rot_z);
		}

		public void applyRotation(byte rotComp)
		{
			this.rot_comp = rotComp;
			this.rot_x = (byte)(rotComp >> 4 & 3);
			this.rot_y = (byte)(rotComp >> 2 & 3);
			this.rot_z = (rotComp & 3);
			this.displayRotation = Quaternion.Euler((float)(this.rot_x * 90), (float)(this.rot_y * 90), (float)(this.rot_z * 90));
		}

		public void setRotation(byte rotComp)
		{
			this.applyRotation(rotComp);
			this.refreshDisplay();
		}

		public virtual void refreshDisplay()
		{
			if (this.displayModel != null)
			{
				Object.Destroy(this.displayModel.gameObject);
				this.displayModel = null;
				this.displayAsset = null;
			}
			if (this.displayItem == null)
			{
				return;
			}
			if (this.gunLargeTransform == null || this.gunSmallTransform == null || this.meleeTransform == null || this.itemTransform == null)
			{
				return;
			}
			this.displayAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.displayItem.id);
			if (this.displayAsset == null)
			{
				return;
			}
			if (this.displaySkin != 0)
			{
				if ((SkinAsset)Assets.find(EAssetType.SKIN, this.displaySkin) == null)
				{
					return;
				}
				this.displayModel = ItemTool.getItem(this.displayItem.id, this.displaySkin, this.displayItem.quality, this.displayItem.state, false);
				if (this.displayMythic != 0)
				{
					ItemTool.applyEffect(this.displayModel, this.displayMythic, EEffectType.THIRD);
				}
			}
			else
			{
				this.displayModel = ItemTool.getItem(this.displayItem.id, 0, this.displayItem.quality, this.displayItem.state, false);
				if (this.displayMythic != 0)
				{
					ItemTool.applyEffect(this.displayModel, this.displayMythic, EEffectType.HOOK);
				}
			}
			if (this.displayModel == null)
			{
				return;
			}
			if (this.displayAsset.type == EItemType.GUN)
			{
				if (this.displayAsset.slot == ESlotType.PRIMARY)
				{
					this.displayModel.parent = this.gunLargeTransform;
				}
				else
				{
					this.displayModel.parent = this.gunSmallTransform;
				}
			}
			else if (this.displayAsset.type == EItemType.MELEE)
			{
				this.displayModel.parent = this.meleeTransform;
			}
			else
			{
				this.displayModel.parent = this.itemTransform;
			}
			this.displayModel.localPosition = Vector3.zero;
			this.displayModel.localRotation = this.displayRotation;
			this.displayModel.localScale = Vector3.one;
			Object.Destroy(this.displayModel.GetComponent<Collider>());
		}

		public bool checkRot(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		public bool checkStore(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || ((!this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group)) && !this.isOpen);
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.gunLargeTransform = base.transform.FindChildRecursive("Gun_Large");
			this.gunSmallTransform = base.transform.FindChildRecursive("Gun_Small");
			this.meleeTransform = base.transform.FindChildRecursive("Melee");
			this.itemTransform = base.transform.FindChildRecursive("Item");
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._isDisplay = ((ItemStorageAsset)asset).isDisplay;
			if (Provider.isServer)
			{
				SteamPacker.openRead(0, state);
				this._owner = (CSteamID)SteamPacker.read(Types.STEAM_ID_TYPE);
				this._group = (CSteamID)SteamPacker.read(Types.STEAM_ID_TYPE);
				this._items = new Items(PlayerInventory.STORAGE);
				this.items.resize(((ItemStorageAsset)asset).storage_x, ((ItemStorageAsset)asset).storage_y);
				byte b = (byte)SteamPacker.read(Types.BYTE_TYPE);
				for (byte b2 = 0; b2 < b; b2 += 1)
				{
					if (BarricadeManager.version > 7)
					{
						object[] array = SteamPacker.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
						ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, (ushort)array[3]);
						if (itemAsset != null)
						{
							this.items.loadItem((byte)array[0], (byte)array[1], (byte)array[2], new Item((ushort)array[3], (byte)array[4], (byte)array[5], (byte[])array[6]));
						}
					}
					else
					{
						object[] array2 = SteamPacker.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
						ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, (ushort)array2[2]);
						if (itemAsset2 != null)
						{
							this.items.loadItem((byte)array2[0], (byte)array2[1], 0, new Item((ushort)array2[2], (byte)array2[3], (byte)array2[4], (byte[])array2[5]));
						}
					}
				}
				if (this.isDisplay)
				{
					this.displaySkin = (ushort)SteamPacker.read(Types.UINT16_TYPE);
					this.displayMythic = (ushort)SteamPacker.read(Types.UINT16_TYPE);
					if (BarricadeManager.version > 8)
					{
						this.applyRotation((byte)SteamPacker.read(Types.BYTE_TYPE));
					}
					else
					{
						this.applyRotation(0);
					}
				}
				this.items.onStateUpdated = new StateUpdated(this.onStateUpdated);
				SteamPacker.closeRead();
				if (this.isDisplay)
				{
					this.updateDisplay();
					this.refreshDisplay();
				}
			}
			else
			{
				Block block = new Block(state);
				this._owner = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
				this._group = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
				if (state.Length > 16)
				{
					object[] array3 = block.read(Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE, Types.UINT16_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE);
					this.applyRotation((byte)array3[5]);
					this.setDisplay((ushort)array3[0], (byte)array3[1], (byte[])array3[2], (ushort)array3[3], (ushort)array3[4]);
				}
			}
		}

		public override bool checkUseable()
		{
			return this.checkStore(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		public override void use()
		{
			BarricadeManager.storeStorage(base.transform, Input.GetKey(ControlsSettings.other));
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			text = string.Empty;
			color = Color.white;
			if (this.checkUseable())
			{
				message = EPlayerMessage.STORAGE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			return true;
		}

		private void Start()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (BarricadeManager.version < 9)
			{
				this.onStateUpdated();
			}
		}

		public void ManualOnDestroy()
		{
			if (this.isDisplay)
			{
				this.setDisplay(0, 0, null, 0, 0);
			}
			if (!Provider.isServer)
			{
				return;
			}
			this.items.onStateUpdated = null;
			if (!this.despawnWhenDestroyed)
			{
				for (byte b = 0; b < this.items.getItemCount(); b += 1)
				{
					ItemJar item = this.items.getItem(b);
					ItemManager.dropItem(item.item, base.transform.position, false, true, true);
				}
			}
			this.items.clear();
			this._items = null;
			if (this.isOpen)
			{
				if (this.opener != null)
				{
					if (this.opener.inventory.isStoring)
					{
						this.opener.inventory.isStoring = false;
						this.opener.inventory.storage = null;
						this.opener.inventory.updateItems(PlayerInventory.STORAGE, null);
						this.opener.inventory.sendStorage();
					}
					this.opener = null;
				}
				this.isOpen = false;
			}
		}

		private CSteamID _owner;

		private CSteamID _group;

		private Items _items;

		private Transform gunLargeTransform;

		private Transform gunSmallTransform;

		private Transform meleeTransform;

		private Transform itemTransform;

		protected Transform displayModel;

		protected ItemAsset displayAsset;

		public Item displayItem;

		public ushort displaySkin;

		public ushort displayMythic;

		private Quaternion displayRotation;

		public byte rot_comp;

		public byte rot_x;

		public byte rot_y;

		public byte rot_z;

		public bool isOpen;

		public Player opener;

		private bool isLocked;

		private bool _isDisplay;

		public bool despawnWhenDestroyed;
	}
}
