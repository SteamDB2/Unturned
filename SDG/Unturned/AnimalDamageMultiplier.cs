using System;
using SDG.Framework.Debug;

namespace SDG.Unturned
{
	public class AnimalDamageMultiplier : IDamageMultiplier
	{
		public AnimalDamageMultiplier(float newDamage, float newLeg, float newSpine, float newSkull)
		{
			this.damage = newDamage;
			this.leg = newLeg;
			this.spine = newSpine;
			this.skull = newSkull;
		}

		public float multiply(ELimb limb)
		{
			switch (limb)
			{
			case ELimb.LEFT_BACK:
				return this.damage * this.leg * Provider.modeConfigData.Animals.Armor_Multiplier;
			case ELimb.RIGHT_BACK:
				return this.damage * this.leg * Provider.modeConfigData.Animals.Armor_Multiplier;
			case ELimb.LEFT_FRONT:
				return this.damage * this.leg * Provider.modeConfigData.Animals.Armor_Multiplier;
			case ELimb.RIGHT_FRONT:
				return this.damage * this.leg * Provider.modeConfigData.Animals.Armor_Multiplier;
			case ELimb.SPINE:
				return this.damage * this.spine * Provider.modeConfigData.Animals.Armor_Multiplier;
			case ELimb.SKULL:
				return this.damage * this.skull * Provider.modeConfigData.Animals.Armor_Multiplier;
			default:
				return this.damage;
			}
		}

		public static readonly float MULTIPLIER_EASY = 1.25f;

		public static readonly float MULTIPLIER_HARD = 0.75f;

		[Inspectable("#SDG::Asset.Animal_Damage_Multiplier.Damage.Name", null)]
		public float damage;

		[Inspectable("#SDG::Asset.Animal_Damage_Multiplier.Leg.Name", null)]
		public float leg;

		[Inspectable("#SDG::Asset.Animal_Damage_Multiplier.Spine.Name", null)]
		public float spine;

		[Inspectable("#SDG::Asset.Animal_Damage_Multiplier.Skull.Name", null)]
		public float skull;
	}
}
