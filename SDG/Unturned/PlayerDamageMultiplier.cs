using System;
using SDG.Framework.Debug;

namespace SDG.Unturned
{
	public class PlayerDamageMultiplier : IDamageMultiplier
	{
		public PlayerDamageMultiplier(float newDamage, float newLeg, float newArm, float newSpine, float newSkull)
		{
			this.damage = newDamage;
			this.leg = newLeg;
			this.arm = newArm;
			this.spine = newSpine;
			this.skull = newSkull;
		}

		public float multiply(ELimb limb)
		{
			switch (limb)
			{
			case ELimb.LEFT_FOOT:
				return this.damage * this.leg;
			case ELimb.LEFT_LEG:
				return this.damage * this.leg;
			case ELimb.RIGHT_FOOT:
				return this.damage * this.leg;
			case ELimb.RIGHT_LEG:
				return this.damage * this.leg;
			case ELimb.LEFT_HAND:
				return this.damage * this.arm;
			case ELimb.LEFT_ARM:
				return this.damage * this.arm;
			case ELimb.RIGHT_HAND:
				return this.damage * this.arm;
			case ELimb.RIGHT_ARM:
				return this.damage * this.arm;
			case ELimb.SPINE:
				return this.damage * this.spine;
			case ELimb.SKULL:
				return this.damage * this.skull;
			}
			return this.damage;
		}

		public float armor(ELimb limb, Player player)
		{
			if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
			{
				if (player.clothing.pants != 0)
				{
					ItemClothingAsset itemClothingAsset = (ItemClothingAsset)Assets.find(EAssetType.ITEM, player.clothing.pants);
					if (itemClothingAsset != null)
					{
						if (Provider.modeConfigData.Items.Has_Durability && player.clothing.pantsQuality > 0)
						{
							PlayerClothing clothing = player.clothing;
							clothing.pantsQuality -= 1;
							player.clothing.sendUpdatePantsQuality();
						}
						return itemClothingAsset.armor + (1f - itemClothingAsset.armor) * (1f - (float)player.clothing.pantsQuality / 100f);
					}
				}
			}
			else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM)
			{
				if (player.clothing.shirt != 0)
				{
					ItemClothingAsset itemClothingAsset2 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, player.clothing.shirt);
					if (itemClothingAsset2 != null)
					{
						if (Provider.modeConfigData.Items.Has_Durability && player.clothing.shirtQuality > 0)
						{
							PlayerClothing clothing2 = player.clothing;
							clothing2.shirtQuality -= 1;
							player.clothing.sendUpdateShirtQuality();
						}
						return itemClothingAsset2.armor + (1f - itemClothingAsset2.armor) * (1f - (float)player.clothing.shirtQuality / 100f);
					}
				}
			}
			else
			{
				if (limb == ELimb.SPINE)
				{
					float num = 1f;
					if (player.clothing.vest != 0)
					{
						ItemClothingAsset itemClothingAsset3 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, player.clothing.vest);
						if (itemClothingAsset3 != null)
						{
							if (Provider.modeConfigData.Items.Has_Durability && player.clothing.vestQuality > 0)
							{
								PlayerClothing clothing3 = player.clothing;
								clothing3.vestQuality -= 1;
								player.clothing.sendUpdateVestQuality();
							}
							num *= itemClothingAsset3.armor + (1f - itemClothingAsset3.armor) * (1f - (float)player.clothing.vestQuality / 100f);
						}
					}
					if (player.clothing.shirt != 0)
					{
						ItemClothingAsset itemClothingAsset4 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, player.clothing.shirt);
						if (itemClothingAsset4 != null)
						{
							if (Provider.modeConfigData.Items.Has_Durability && player.clothing.shirtQuality > 0)
							{
								PlayerClothing clothing4 = player.clothing;
								clothing4.shirtQuality -= 1;
								player.clothing.sendUpdateShirtQuality();
							}
							num *= itemClothingAsset4.armor + (1f - itemClothingAsset4.armor) * (1f - (float)player.clothing.shirtQuality / 100f);
						}
					}
					return num;
				}
				if (limb == ELimb.SKULL && player.clothing.hat != 0)
				{
					ItemClothingAsset itemClothingAsset5 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, player.clothing.hat);
					if (itemClothingAsset5 != null)
					{
						if (Provider.modeConfigData.Items.Has_Durability && player.clothing.hatQuality > 0)
						{
							PlayerClothing clothing5 = player.clothing;
							clothing5.hatQuality -= 1;
							player.clothing.sendUpdateHatQuality();
						}
						return itemClothingAsset5.armor + (1f - itemClothingAsset5.armor) * (1f - (float)player.clothing.hatQuality / 100f);
					}
				}
			}
			return 1f;
		}

		[Inspectable("#SDG::Asset.Player_Damage_Multiplier.Damage.Name", null)]
		public float damage;

		[Inspectable("#SDG::Asset.Player_Damage_Multiplier.Leg.Name", null)]
		public float leg;

		[Inspectable("#SDG::Asset.Player_Damage_Multiplier.Arm.Name", null)]
		public float arm;

		[Inspectable("#SDG::Asset.Player_Damage_Multiplier.Spine.Name", null)]
		public float spine;

		[Inspectable("#SDG::Asset.Player_Damage_Multiplier.Skull.Name", null)]
		public float skull;
	}
}
