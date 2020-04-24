using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekInventory : Sleek
	{
		public SleekInventory()
		{
			base.init();
			this.button = new SleekButton();
			this.button.sizeScale_X = 1f;
			this.button.sizeScale_Y = 1f;
			this.button.backgroundTint = ESleekTint.NONE;
			this.button.foregroundTint = ESleekTint.NONE;
			this.button.onClickedButton = new ClickedButton(this.onClickedButton);
			base.add(this.button);
			this.button.isClickable = false;
			this.icon = new SleekImageTexture();
			this.icon.positionOffset_X = 5;
			this.icon.positionOffset_Y = 5;
			this.icon.sizeScale_X = 1f;
			this.icon.sizeScale_Y = 1f;
			this.icon.sizeOffset_X = -10;
			this.icon.constraint = ESleekConstraint.XY;
			base.add(this.icon);
			this.icon.isVisible = false;
			this.equippedIcon = new SleekImageTexture();
			this.equippedIcon.positionOffset_X = 5;
			this.equippedIcon.positionOffset_Y = 5;
			this.equippedIcon.backgroundTint = ESleekTint.FOREGROUND;
			base.add(this.equippedIcon);
			this.equippedIcon.isVisible = false;
			this.nameLabel = new SleekLabel();
			this.nameLabel.positionScale_Y = 1f;
			this.nameLabel.sizeScale_X = 1f;
			this.nameLabel.foregroundTint = ESleekTint.NONE;
			base.add(this.nameLabel);
			this.nameLabel.isVisible = false;
		}

		public ItemAsset itemAsset
		{
			get
			{
				return this._itemAsset;
			}
		}

		public void updateInventory(ulong instance, int item, ushort quantity, bool isClickable, bool isLarge)
		{
			this.button.isClickable = isClickable;
			if (isLarge)
			{
				this.icon.sizeOffset_Y = -70;
				this.nameLabel.fontSize = 18;
				this.nameLabel.positionOffset_Y = -70;
				this.nameLabel.sizeOffset_Y = 70;
				this.equippedIcon.sizeOffset_X = 20;
				this.equippedIcon.sizeOffset_Y = 20;
			}
			else
			{
				this.icon.sizeOffset_Y = -50;
				this.nameLabel.fontSize = 12;
				this.nameLabel.positionOffset_Y = -50;
				this.nameLabel.sizeOffset_Y = 50;
				this.equippedIcon.sizeOffset_X = 10;
				this.equippedIcon.sizeOffset_Y = 10;
			}
			if (item != 0)
			{
				if (item < 0)
				{
					this._itemAsset = null;
					this.icon.texture = (Texture2D)Resources.Load("Economy/Mystery" + ((!isLarge) ? "/Icon_Small" : "/Icon_Large"));
					this.icon.isVisible = true;
					this.nameLabel.text = MenuSurvivorsClothingUI.localization.format("Mystery_" + item + "_Text");
					this.button.tooltip = MenuSurvivorsClothingUI.localization.format("Mystery_Tooltip");
					this.button.backgroundColor = Palette.MYTHICAL;
					this.equippedIcon.isVisible = false;
				}
				else
				{
					ushort inventoryItemID = Provider.provider.economyService.getInventoryItemID(item);
					if (inventoryItemID != 0)
					{
						this._itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, inventoryItemID);
						if (this.itemAsset != null)
						{
							if (this.itemAsset.proPath == null || this.itemAsset.proPath.Length == 0)
							{
								ushort inventorySkinID = Provider.provider.economyService.getInventorySkinID(item);
								SkinAsset skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, inventorySkinID);
								if (skinAsset != null)
								{
									this.icon.texture = (Texture2D)Resources.Load(string.Concat(new string[]
									{
										"Economy/Skins/",
										this.itemAsset.name,
										"/",
										skinAsset.name,
										(!isLarge) ? "/Icon_Small" : "/Icon_Large"
									}));
									this.icon.isVisible = true;
								}
								else
								{
									this.icon.isVisible = false;
								}
							}
							else
							{
								this.icon.texture = (Texture2D)Resources.Load("Economy" + this.itemAsset.proPath + ((!isLarge) ? "/Icon_Small" : "/Icon_Large"));
								this.icon.isVisible = true;
							}
						}
						else
						{
							this.icon.texture = null;
							this.icon.isVisible = true;
						}
						this.nameLabel.text = Provider.provider.economyService.getInventoryName(item);
						if (quantity > 1)
						{
							SleekLabel sleekLabel = this.nameLabel;
							sleekLabel.text = sleekLabel.text + " x" + quantity;
						}
						this.button.tooltip = Provider.provider.economyService.getInventoryType(item);
						this.button.backgroundColor = Provider.provider.economyService.getInventoryColor(item);
						bool flag;
						if (this.itemAsset.proPath == null || this.itemAsset.proPath.Length == 0)
						{
							flag = Characters.isSkinEquipped(instance);
						}
						else
						{
							flag = Characters.isCosmeticEquipped(instance);
						}
						this.equippedIcon.isVisible = flag;
						if (flag && this.equippedIcon.texture == null)
						{
							this.equippedIcon.texture = (Texture2D)MenuSurvivorsClothingUI.icons.load("Equip");
						}
					}
					else
					{
						this._itemAsset = null;
						this.icon.texture = null;
						this.icon.isVisible = true;
						this.nameLabel.text = "itemdefid: " + item;
						this.button.tooltip = "itemdefid: " + item;
						this.button.backgroundColor = Color.white;
						this.equippedIcon.isVisible = false;
					}
				}
				this.nameLabel.isVisible = true;
			}
			else
			{
				this._itemAsset = null;
				this.button.tooltip = string.Empty;
				this.button.backgroundColor = Color.white;
				this.icon.isVisible = false;
				this.nameLabel.isVisible = false;
				this.equippedIcon.isVisible = false;
			}
			this.button.foregroundColor = this.button.backgroundColor;
			this.nameLabel.foregroundColor = this.button.backgroundColor;
			this.nameLabel.backgroundColor = this.button.backgroundColor;
		}

		private void onClickedButton(SleekButton button)
		{
			if (this.onClickedInventory != null)
			{
				this.onClickedInventory(this);
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private ItemAsset _itemAsset;

		private SleekButton button;

		private SleekImageTexture icon;

		private SleekLabel nameLabel;

		private SleekImageTexture equippedIcon;

		public ClickedInventory onClickedInventory;
	}
}
