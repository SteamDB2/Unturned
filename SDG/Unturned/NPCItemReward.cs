using System;

namespace SDG.Unturned
{
	public class NPCItemReward : INPCReward
	{
		public NPCItemReward(ushort newID, byte newAmount, ushort newSight, ushort newTactical, ushort newGrip, ushort newBarrel, ushort newMagazine, byte newAmmo, string newText) : base(newText)
		{
			this.id = newID;
			this.amount = newAmount;
			this.sight = newSight;
			this.tactical = newTactical;
			this.grip = newGrip;
			this.barrel = newBarrel;
			this.magazine = newMagazine;
			this.ammo = newAmmo;
		}

		public ushort id { get; protected set; }

		public byte amount { get; protected set; }

		public ushort sight { get; protected set; }

		public ushort tactical { get; protected set; }

		public ushort grip { get; protected set; }

		public ushort barrel { get; protected set; }

		public ushort magazine { get; protected set; }

		public byte ammo { get; protected set; }

		public override void grantReward(Player player, bool shouldSend)
		{
			if (!Provider.isServer)
			{
				return;
			}
			Item item;
			if (this.sight > 0 || this.tactical > 0 || this.grip > 0 || this.barrel > 0 || this.magazine > 0)
			{
				ItemGunAsset itemGunAsset = Assets.find(EAssetType.ITEM, this.id) as ItemGunAsset;
				byte[] state = itemGunAsset.getState((this.sight <= 0) ? itemGunAsset.sightID : this.sight, (this.tactical <= 0) ? itemGunAsset.tacticalID : this.tactical, (this.grip <= 0) ? itemGunAsset.gripID : this.grip, (this.barrel <= 0) ? itemGunAsset.barrelID : this.barrel, (this.magazine <= 0) ? itemGunAsset.getMagazineID() : this.magazine, (this.ammo <= 0) ? itemGunAsset.ammoMax : this.ammo);
				item = new Item(this.id, 1, 100, state);
			}
			else
			{
				item = new Item(this.id, EItemOrigin.CRAFT);
			}
			for (byte b = 0; b < this.amount; b += 1)
			{
				player.inventory.forceAddItem(item, false, false);
			}
		}

		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Item");
			}
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.id) as ItemAsset;
			string arg;
			if (itemAsset != null)
			{
				arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(itemAsset.rarity)),
					">",
					itemAsset.itemName,
					"</color>"
				});
			}
			else
			{
				arg = "?";
			}
			return string.Format(this.text, this.amount, arg);
		}

		public override Sleek createUI(Player player)
		{
			string text = this.formatReward(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
			if (itemAsset == null)
			{
				return null;
			}
			SleekBox sleekBox = new SleekBox();
			if (itemAsset.size_y == 1)
			{
				sleekBox.sizeOffset_Y = (int)(itemAsset.size_y * 50 + 10);
			}
			else
			{
				sleekBox.sizeOffset_Y = (int)(itemAsset.size_y * 25 + 10);
			}
			sleekBox.sizeScale_X = 1f;
			SleekImageTexture sleekImageTexture = new SleekImageTexture();
			sleekImageTexture.positionOffset_X = 5;
			sleekImageTexture.positionOffset_Y = 5;
			if (itemAsset.size_y == 1)
			{
				sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 50);
				sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 50);
			}
			else
			{
				sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 25);
				sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 25);
			}
			sleekBox.add(sleekImageTexture);
			ItemTool.getIcon(this.id, 100, itemAsset.getState(false), itemAsset, sleekImageTexture.sizeOffset_X, sleekImageTexture.sizeOffset_Y, new ItemIconReady(sleekImageTexture.updateTexture));
			sleekBox.add(new SleekLabel
			{
				positionOffset_X = 10 + sleekImageTexture.sizeOffset_X,
				sizeOffset_X = -15 - sleekImageTexture.sizeOffset_X,
				sizeScale_X = 1f,
				sizeScale_Y = 1f,
				fontAlignment = 3,
				foregroundTint = ESleekTint.NONE,
				isRich = true,
				text = text
			});
			return sleekBox;
		}
	}
}
