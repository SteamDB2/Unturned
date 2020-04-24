using System;

namespace SDG.Unturned
{
	public class SleekVendor : SleekButton
	{
		public SleekVendor(VendorElement newElement)
		{
			this.element = newElement;
			base.init();
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, this.element.id);
			if (itemAsset == null)
			{
				return;
			}
			SleekImageTexture sleekImageTexture = new SleekImageTexture();
			sleekImageTexture.positionOffset_X = 5;
			sleekImageTexture.positionOffset_Y = 5;
			if (itemAsset.size_y == 1)
			{
				sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 100);
				sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 100);
			}
			else
			{
				sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 50);
				sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 50);
			}
			base.add(sleekImageTexture);
			ItemTool.getIcon(this.element.id, 100, itemAsset.getState(false), itemAsset, sleekImageTexture.sizeOffset_X, sleekImageTexture.sizeOffset_Y, new ItemIconReady(sleekImageTexture.updateTexture));
			base.sizeOffset_Y = sleekImageTexture.sizeOffset_Y + 10;
			base.add(new SleekLabel
			{
				positionOffset_X = sleekImageTexture.sizeOffset_X + 10,
				positionOffset_Y = 5,
				sizeOffset_X = -sleekImageTexture.sizeOffset_X - 15,
				sizeOffset_Y = 30,
				sizeScale_X = 1f,
				text = itemAsset.itemName,
				fontSize = 14,
				fontAlignment = 0,
				foregroundTint = ESleekTint.NONE,
				foregroundColor = ItemTool.getRarityColorUI(itemAsset.rarity)
			});
			base.add(new SleekLabel
			{
				positionOffset_X = sleekImageTexture.sizeOffset_X + 10,
				positionOffset_Y = 25,
				sizeOffset_X = -sleekImageTexture.sizeOffset_X - 15,
				sizeOffset_Y = -30,
				sizeScale_X = 1f,
				sizeScale_Y = 1f,
				fontAlignment = 0,
				foregroundTint = ESleekTint.NONE,
				isRich = true,
				text = itemAsset.itemDescription
			});
			SleekLabel sleekLabel = new SleekLabel();
			sleekLabel.positionOffset_X = sleekImageTexture.sizeOffset_X + 10;
			sleekLabel.positionOffset_Y = -35;
			sleekLabel.positionScale_Y = 1f;
			sleekLabel.sizeOffset_X = -sleekImageTexture.sizeOffset_X - 15;
			sleekLabel.sizeOffset_Y = 30;
			sleekLabel.sizeScale_X = 1f;
			sleekLabel.fontAlignment = 8;
			sleekLabel.foregroundTint = ESleekTint.NONE;
			sleekLabel.foregroundColor = Palette.COLOR_Y;
			base.add(sleekLabel);
			if (this.element is VendorBuying)
			{
				sleekLabel.text = PlayerNPCVendorUI.localization.format("Price", new object[]
				{
					this.element.cost
				});
			}
			else
			{
				sleekLabel.text = PlayerNPCVendorUI.localization.format("Cost", new object[]
				{
					this.element.cost
				});
			}
			this.amountLabel = new SleekLabel();
			this.amountLabel.positionOffset_X = sleekImageTexture.sizeOffset_X + 10;
			this.amountLabel.positionOffset_Y = -35;
			this.amountLabel.positionScale_Y = 1f;
			this.amountLabel.sizeOffset_X = -sleekImageTexture.sizeOffset_X - 15;
			this.amountLabel.sizeOffset_Y = 30;
			this.amountLabel.sizeScale_X = 1f;
			this.amountLabel.fontAlignment = 6;
			this.amountLabel.foregroundTint = ESleekTint.NONE;
			base.add(this.amountLabel);
			this.updateAmount();
		}

		public void updateAmount()
		{
			if (this.element == null || this.amountLabel == null)
			{
				return;
			}
			if (this.element is VendorBuying)
			{
				ushort num;
				byte b;
				(this.element as VendorBuying).format(Player.player, out num, out b);
				this.amountLabel.foregroundColor = ((num < (ushort)b) ? Palette.COLOR_R : Palette.COLOR_G);
				this.amountLabel.text = PlayerNPCVendorUI.localization.format("Amount_Buy", new object[]
				{
					num,
					b
				});
			}
			else
			{
				ushort num2;
				(this.element as VendorSelling).format(Player.player, out num2);
				this.amountLabel.foregroundColor = ((num2 <= 0) ? Palette.COLOR_R : Palette.COLOR_G);
				this.amountLabel.text = PlayerNPCVendorUI.localization.format("Amount_Sell", new object[]
				{
					num2
				});
			}
		}

		private VendorElement element;

		private SleekLabel amountLabel;
	}
}
