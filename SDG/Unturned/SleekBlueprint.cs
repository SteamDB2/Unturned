using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekBlueprint : SleekButton
	{
		public SleekBlueprint(Blueprint newBlueprint)
		{
			this._blueprint = newBlueprint;
			base.init();
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
			SleekLabel sleekLabel = new SleekLabel();
			sleekLabel.positionOffset_X = 5;
			sleekLabel.positionOffset_Y = 5;
			sleekLabel.sizeOffset_X = -10;
			sleekLabel.sizeOffset_Y = 30;
			sleekLabel.sizeScale_X = 1f;
			sleekLabel.foregroundColor = ((!this.blueprint.hasSupplies || !this.blueprint.hasTool || !this.blueprint.hasItem || !this.blueprint.hasSkills) ? Palette.COLOR_R : Palette.COLOR_G);
			sleekLabel.foregroundTint = ESleekTint.NONE;
			sleekLabel.fontSize = 14;
			base.add(sleekLabel);
			if (this.blueprint.skill != EBlueprintSkill.NONE)
			{
				base.add(new SleekLabel
				{
					positionOffset_X = 5,
					positionOffset_Y = -35,
					positionScale_Y = 1f,
					sizeOffset_X = -10,
					sizeOffset_Y = 30,
					sizeScale_X = 1f,
					text = PlayerDashboardCraftingUI.localization.format("Skill_" + (int)this.blueprint.skill, new object[]
					{
						PlayerDashboardSkillsUI.localization.format("Level_" + this.blueprint.level)
					}),
					foregroundColor = ((!this.blueprint.hasSkills) ? Palette.COLOR_R : Palette.COLOR_G),
					foregroundTint = ESleekTint.NONE,
					fontSize = 14
				});
			}
			Sleek sleek = new Sleek();
			sleek.positionOffset_Y = 40;
			sleek.positionScale_X = 0.5f;
			sleek.sizeOffset_Y = -45;
			sleek.sizeScale_Y = 1f;
			base.add(sleek);
			int num = 0;
			for (int i = 0; i < this.blueprint.supplies.Length; i++)
			{
				BlueprintSupply blueprintSupply = this.blueprint.supplies[i];
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, blueprintSupply.id);
				if (itemAsset != null)
				{
					SleekLabel sleekLabel2 = sleekLabel;
					sleekLabel2.text += itemAsset.itemName;
					SleekImageTexture sleekImageTexture = new SleekImageTexture();
					sleekImageTexture.positionOffset_X = num;
					sleekImageTexture.positionOffset_Y = (int)(-itemAsset.size_y * 25);
					sleekImageTexture.positionScale_Y = 0.5f;
					sleekImageTexture.sizeOffset_X = (int)(itemAsset.size_x * 50);
					sleekImageTexture.sizeOffset_Y = (int)(itemAsset.size_y * 50);
					sleek.add(sleekImageTexture);
					ItemTool.getIcon(blueprintSupply.id, 100, itemAsset.getState(false), itemAsset, new ItemIconReady(sleekImageTexture.updateTexture));
					SleekLabel sleekLabel3 = new SleekLabel();
					sleekLabel3.positionOffset_X = -100;
					sleekLabel3.positionOffset_Y = -30;
					sleekLabel3.positionScale_Y = 1f;
					sleekLabel3.sizeOffset_X = 100;
					sleekLabel3.sizeOffset_Y = 30;
					sleekLabel3.sizeScale_X = 1f;
					sleekLabel3.foregroundTint = ESleekTint.NONE;
					sleekLabel3.fontAlignment = 5;
					sleekLabel3.text = blueprintSupply.hasAmount + "/" + blueprintSupply.amount;
					sleekImageTexture.add(sleekLabel3);
					SleekLabel sleekLabel4 = sleekLabel;
					string text = sleekLabel4.text;
					sleekLabel4.text = string.Concat(new object[]
					{
						text,
						" ",
						blueprintSupply.hasAmount,
						"/",
						blueprintSupply.amount
					});
					if (this.blueprint.type == EBlueprintType.AMMO)
					{
						if (blueprintSupply.hasAmount == 0 || blueprintSupply.amount == 0)
						{
							sleekLabel3.backgroundColor = Palette.COLOR_R;
							sleekLabel3.foregroundColor = Palette.COLOR_R;
						}
					}
					else if (blueprintSupply.hasAmount < blueprintSupply.amount)
					{
						sleekLabel3.backgroundColor = Palette.COLOR_R;
						sleekLabel3.foregroundColor = Palette.COLOR_R;
					}
					num += (int)(itemAsset.size_x * 50 + 25);
					if (i < this.blueprint.supplies.Length - 1 || this.blueprint.tool != 0 || this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
					{
						SleekLabel sleekLabel5 = sleekLabel;
						sleekLabel5.text += " + ";
						sleek.add(new SleekImageTexture((Texture2D)PlayerDashboardCraftingUI.icons.load("Plus"))
						{
							positionOffset_X = num,
							positionOffset_Y = -20,
							positionScale_Y = 0.5f,
							sizeOffset_X = 40,
							sizeOffset_Y = 40,
							backgroundTint = ESleekTint.FOREGROUND
						});
						num += 65;
					}
				}
			}
			if (this.blueprint.tool != 0)
			{
				ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, this.blueprint.tool);
				if (itemAsset2 != null)
				{
					SleekLabel sleekLabel6 = sleekLabel;
					sleekLabel6.text += itemAsset2.itemName;
					SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
					sleekImageTexture2.positionOffset_X = num;
					sleekImageTexture2.positionOffset_Y = (int)(-itemAsset2.size_y * 25);
					sleekImageTexture2.positionScale_Y = 0.5f;
					sleekImageTexture2.sizeOffset_X = (int)(itemAsset2.size_x * 50);
					sleekImageTexture2.sizeOffset_Y = (int)(itemAsset2.size_y * 50);
					sleek.add(sleekImageTexture2);
					ItemTool.getIcon(this.blueprint.tool, 100, itemAsset2.getState(), itemAsset2, new ItemIconReady(sleekImageTexture2.updateTexture));
					SleekLabel sleekLabel7 = new SleekLabel();
					sleekLabel7.positionOffset_X = -100;
					sleekLabel7.positionOffset_Y = -30;
					sleekLabel7.positionScale_Y = 1f;
					sleekLabel7.sizeOffset_X = 100;
					sleekLabel7.sizeOffset_Y = 30;
					sleekLabel7.sizeScale_X = 1f;
					sleekLabel7.foregroundTint = ESleekTint.NONE;
					sleekLabel7.fontAlignment = 5;
					sleekLabel7.text = this.blueprint.tools + "/1";
					sleekImageTexture2.add(sleekLabel7);
					SleekLabel sleekLabel8 = sleekLabel;
					string text = sleekLabel8.text;
					sleekLabel8.text = string.Concat(new object[]
					{
						text,
						" ",
						this.blueprint.tools,
						"/1"
					});
					if (!this.blueprint.hasTool)
					{
						sleekLabel7.backgroundColor = Palette.COLOR_R;
						sleekLabel7.foregroundColor = Palette.COLOR_R;
					}
					num += (int)(itemAsset2.size_x * 50 + 25);
					if (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
					{
						SleekLabel sleekLabel9 = sleekLabel;
						sleekLabel9.text += " + ";
						sleek.add(new SleekImageTexture((Texture2D)PlayerDashboardCraftingUI.icons.load("Plus"))
						{
							positionOffset_X = num,
							positionOffset_Y = -20,
							positionScale_Y = 0.5f,
							sizeOffset_X = 40,
							sizeOffset_Y = 40,
							backgroundTint = ESleekTint.FOREGROUND
						});
						num += 65;
					}
				}
			}
			if (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
			{
				ItemAsset itemAsset3 = (ItemAsset)Assets.find(EAssetType.ITEM, this.blueprint.outputs[0].id);
				if (itemAsset3 != null)
				{
					SleekLabel sleekLabel10 = sleekLabel;
					sleekLabel10.text += itemAsset3.itemName;
					SleekImageTexture sleekImageTexture3 = new SleekImageTexture();
					sleekImageTexture3.positionOffset_X = num;
					sleekImageTexture3.positionOffset_Y = (int)(-itemAsset3.size_y * 25);
					sleekImageTexture3.positionScale_Y = 0.5f;
					sleekImageTexture3.sizeOffset_X = (int)(itemAsset3.size_x * 50);
					sleekImageTexture3.sizeOffset_Y = (int)(itemAsset3.size_y * 50);
					sleek.add(sleekImageTexture3);
					ItemTool.getIcon(this.blueprint.outputs[0].id, 100, itemAsset3.getState(), itemAsset3, new ItemIconReady(sleekImageTexture3.updateTexture));
					SleekLabel sleekLabel11 = new SleekLabel();
					sleekLabel11.positionOffset_X = -100;
					sleekLabel11.positionOffset_Y = -30;
					sleekLabel11.positionScale_Y = 1f;
					sleekLabel11.sizeOffset_X = 100;
					sleekLabel11.sizeOffset_Y = 30;
					sleekLabel11.sizeScale_X = 1f;
					sleekLabel11.foregroundTint = ESleekTint.NONE;
					sleekLabel11.fontAlignment = 5;
					if (this.blueprint.type == EBlueprintType.REPAIR)
					{
						SleekLabel sleekLabel12 = sleekLabel;
						string text = sleekLabel12.text;
						sleekLabel12.text = string.Concat(new object[]
						{
							text,
							" ",
							this.blueprint.items,
							"%"
						});
						sleekLabel11.text = this.blueprint.items + "%";
						sleekLabel11.backgroundColor = ItemTool.getQualityColor((float)this.blueprint.items / 100f);
						sleekLabel11.foregroundColor = sleekLabel11.backgroundColor;
					}
					else if (this.blueprint.type == EBlueprintType.AMMO)
					{
						SleekLabel sleekLabel13 = sleekLabel;
						string text = sleekLabel13.text;
						sleekLabel13.text = string.Concat(new object[]
						{
							text,
							" ",
							this.blueprint.items,
							"/",
							this.blueprint.products
						});
						sleekLabel11.text = this.blueprint.items + "/" + itemAsset3.amount;
					}
					if (!this.blueprint.hasItem)
					{
						sleekLabel11.backgroundColor = Palette.COLOR_R;
						sleekLabel11.foregroundColor = Palette.COLOR_R;
					}
					sleekImageTexture3.add(sleekLabel11);
					num += (int)(itemAsset3.size_x * 50 + 25);
				}
			}
			SleekLabel sleekLabel14 = sleekLabel;
			sleekLabel14.text += " = ";
			sleek.add(new SleekImageTexture((Texture2D)PlayerDashboardCraftingUI.icons.load("Equals"))
			{
				positionOffset_X = num,
				positionOffset_Y = -20,
				positionScale_Y = 0.5f,
				sizeOffset_X = 40,
				sizeOffset_Y = 40,
				backgroundTint = ESleekTint.FOREGROUND
			});
			num += 65;
			for (int j = 0; j < this.blueprint.outputs.Length; j++)
			{
				BlueprintOutput blueprintOutput = this.blueprint.outputs[j];
				ItemAsset itemAsset4 = (ItemAsset)Assets.find(EAssetType.ITEM, blueprintOutput.id);
				if (itemAsset4 != null)
				{
					SleekLabel sleekLabel15 = sleekLabel;
					sleekLabel15.text += itemAsset4.itemName;
					SleekImageTexture sleekImageTexture4 = new SleekImageTexture();
					sleekImageTexture4.positionOffset_X = num;
					sleekImageTexture4.positionOffset_Y = (int)(-itemAsset4.size_y * 25);
					sleekImageTexture4.positionScale_Y = 0.5f;
					sleekImageTexture4.sizeOffset_X = (int)(itemAsset4.size_x * 50);
					sleekImageTexture4.sizeOffset_Y = (int)(itemAsset4.size_y * 50);
					sleek.add(sleekImageTexture4);
					ItemTool.getIcon(blueprintOutput.id, 100, itemAsset4.getState(), itemAsset4, new ItemIconReady(sleekImageTexture4.updateTexture));
					SleekLabel sleekLabel16 = new SleekLabel();
					sleekLabel16.positionOffset_X = -100;
					sleekLabel16.positionOffset_Y = -30;
					sleekLabel16.positionScale_Y = 1f;
					sleekLabel16.sizeOffset_X = 100;
					sleekLabel16.sizeOffset_Y = 30;
					sleekLabel16.sizeScale_X = 1f;
					sleekLabel16.foregroundTint = ESleekTint.NONE;
					sleekLabel16.fontAlignment = 5;
					if (this.blueprint.type == EBlueprintType.REPAIR)
					{
						SleekLabel sleekLabel17 = sleekLabel;
						sleekLabel17.text += " 100%";
						sleekLabel16.text = "100%";
						sleekLabel16.backgroundColor = Palette.COLOR_G;
						sleekLabel16.foregroundColor = Palette.COLOR_G;
					}
					else if (this.blueprint.type == EBlueprintType.AMMO)
					{
						ItemAsset itemAsset5 = (ItemAsset)Assets.find(EAssetType.ITEM, blueprintOutput.id);
						if (itemAsset5 != null)
						{
							SleekLabel sleekLabel18 = sleekLabel;
							string text = sleekLabel18.text;
							sleekLabel18.text = string.Concat(new object[]
							{
								text,
								" ",
								this.blueprint.products,
								"/",
								itemAsset5.amount
							});
							sleekLabel16.text = this.blueprint.products + "/" + itemAsset5.amount;
						}
					}
					else
					{
						SleekLabel sleekLabel19 = sleekLabel;
						sleekLabel19.text = sleekLabel19.text + " x" + blueprintOutput.amount;
						sleekLabel16.text = "x" + blueprintOutput.amount.ToString();
					}
					sleekImageTexture4.add(sleekLabel16);
					num += (int)(itemAsset4.size_x * 50);
					if (j < this.blueprint.outputs.Length - 1)
					{
						num += 25;
						SleekLabel sleekLabel20 = sleekLabel;
						sleekLabel20.text += " + ";
						sleek.add(new SleekImageTexture((Texture2D)PlayerDashboardCraftingUI.icons.load("Plus"))
						{
							positionOffset_X = num,
							positionOffset_Y = -20,
							positionScale_Y = 0.5f,
							sizeOffset_X = 40,
							sizeOffset_Y = 40,
							backgroundTint = ESleekTint.FOREGROUND
						});
						num += 65;
					}
				}
			}
			sleek.positionOffset_X = -num / 2;
			sleek.sizeOffset_X = num;
			base.tooltip = sleekLabel.text;
			this.foregroundTint = ESleekTint.NONE;
			base.foregroundColor = sleekLabel.foregroundColor;
		}

		public Blueprint blueprint
		{
			get
			{
				return this._blueprint;
			}
		}

		private Blueprint _blueprint;
	}
}
