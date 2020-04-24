using System;
using SDG.Framework.Debug;

namespace SDG.Unturned
{
	public class ZombieDamageMultiplier : IDamageMultiplier
	{
		public ZombieDamageMultiplier(float newDamage, float newLeg, float newArm, float newSpine, float newSkull)
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
				return this.damage * this.leg * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.LEFT_LEG:
				return this.damage * this.leg * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.RIGHT_FOOT:
				return this.damage * this.leg * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.RIGHT_LEG:
				return this.damage * this.leg * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.LEFT_HAND:
				return this.damage * this.arm * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.LEFT_ARM:
				return this.damage * this.arm * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.RIGHT_HAND:
				return this.damage * this.arm * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.RIGHT_ARM:
				return this.damage * this.arm * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.SPINE:
				return this.damage * this.spine * Provider.modeConfigData.Zombies.Armor_Multiplier;
			case ELimb.SKULL:
				return this.damage * this.skull * Provider.modeConfigData.Zombies.Armor_Multiplier;
			}
			return this.damage;
		}

		public float armor(ELimb limb, Zombie zombie)
		{
			if ((int)zombie.type < LevelZombies.tables.Count)
			{
				if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
				{
					if (zombie.pants != 255 && (int)zombie.pants < LevelZombies.tables[(int)zombie.type].slots[1].table.Count)
					{
						ItemClothingAsset itemClothingAsset = (ItemClothingAsset)Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[1].table[(int)zombie.pants].item);
						if (itemClothingAsset != null)
						{
							return itemClothingAsset.armor;
						}
					}
				}
				else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM)
				{
					if (zombie.shirt != 255 && (int)zombie.shirt < LevelZombies.tables[(int)zombie.type].slots[0].table.Count)
					{
						ItemClothingAsset itemClothingAsset2 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[0].table[(int)zombie.shirt].item);
						if (itemClothingAsset2 != null)
						{
							return itemClothingAsset2.armor;
						}
					}
				}
				else
				{
					if (limb == ELimb.SPINE)
					{
						float num = 1f;
						if (zombie.gear != 255 && (int)zombie.gear < LevelZombies.tables[(int)zombie.type].slots[3].table.Count)
						{
							ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[3].table[(int)zombie.gear].item);
							if (itemAsset != null && itemAsset.type == EItemType.VEST)
							{
								num *= ((ItemClothingAsset)itemAsset).armor;
							}
						}
						if (zombie.shirt != 255 && (int)zombie.shirt < LevelZombies.tables[(int)zombie.type].slots[0].table.Count)
						{
							ItemClothingAsset itemClothingAsset3 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[0].table[(int)zombie.shirt].item);
							if (itemClothingAsset3 != null)
							{
								num *= itemClothingAsset3.armor;
							}
						}
						return num;
					}
					if (limb == ELimb.SKULL && zombie.hat != 255 && (int)zombie.hat < LevelZombies.tables[(int)zombie.type].slots[2].table.Count)
					{
						ItemClothingAsset itemClothingAsset4 = (ItemClothingAsset)Assets.find(EAssetType.ITEM, LevelZombies.tables[(int)zombie.type].slots[2].table[(int)zombie.hat].item);
						if (itemClothingAsset4 != null)
						{
							return itemClothingAsset4.armor;
						}
					}
				}
			}
			return 1f;
		}

		[Inspectable("#SDG::Asset.Zombie_Damage_Multiplier.Damage.Name", null)]
		public float damage;

		[Inspectable("#SDG::Asset.Zombie_Damage_Multiplier.Leg.Name", null)]
		public float leg;

		[Inspectable("#SDG::Asset.Zombie_Damage_Multiplier.Arm.Name", null)]
		public float arm;

		[Inspectable("#SDG::Asset.Zombie_Damage_Multiplier.Spine.Name", null)]
		public float spine;

		[Inspectable("#SDG::Asset.Zombie_Damage_Multiplier.Skull.Name", null)]
		public float skull;
	}
}
