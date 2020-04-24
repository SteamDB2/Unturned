using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	public class ItemWeaponAsset : ItemAsset
	{
		public ItemWeaponAsset()
		{
			this.playerDamageMultiplier = new PlayerDamageMultiplier(0f, 0f, 0f, 0f, 0f);
			this.zombieDamageMultiplier = new ZombieDamageMultiplier(0f, 0f, 0f, 0f, 0f);
			this.animalDamageMultiplier = new AnimalDamageMultiplier(0f, 0f, 0f, 0f);
		}

		public ItemWeaponAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
		}

		public ItemWeaponAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this.bladeID = data.readByte("BladeID");
			this.range = data.readSingle("Range");
			this.playerDamageMultiplier = new PlayerDamageMultiplier(data.readSingle("Player_Damage"), data.readSingle("Player_Leg_Multiplier"), data.readSingle("Player_Arm_Multiplier"), data.readSingle("Player_Spine_Multiplier"), data.readSingle("Player_Skull_Multiplier"));
			this.zombieDamageMultiplier = new ZombieDamageMultiplier(data.readSingle("Zombie_Damage"), data.readSingle("Zombie_Leg_Multiplier"), data.readSingle("Zombie_Arm_Multiplier"), data.readSingle("Zombie_Spine_Multiplier"), data.readSingle("Zombie_Skull_Multiplier"));
			this.animalDamageMultiplier = new AnimalDamageMultiplier(data.readSingle("Animal_Damage"), data.readSingle("Animal_Leg_Multiplier"), data.readSingle("Animal_Spine_Multiplier"), data.readSingle("Animal_Skull_Multiplier"));
			this.barricadeDamage = data.readSingle("Barricade_Damage");
			this.structureDamage = data.readSingle("Structure_Damage");
			this.vehicleDamage = data.readSingle("Vehicle_Damage");
			this.resourceDamage = data.readSingle("Resource_Damage");
			if (data.has("Object_Damage"))
			{
				this.objectDamage = data.readSingle("Object_Damage");
			}
			else
			{
				this.objectDamage = this.resourceDamage;
			}
			this.durability = data.readSingle("Durability");
			this.wear = data.readByte("Wear");
			if (this.wear < 1)
			{
				this.wear = 1;
			}
			this.isInvulnerable = data.has("Invulnerable");
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.bladeID = reader.readValue<byte>("Blade_ID");
			this.range = reader.readValue<float>("Range");
			this.playerDamageMultiplier = new PlayerDamageMultiplier(reader.readValue<float>("Player_Damage"), reader.readValue<float>("Player_Leg_Multiplier"), reader.readValue<float>("Player_Arm_Multiplier"), reader.readValue<float>("Player_Spine_Multiplier"), reader.readValue<float>("Player_Skull_Multiplier"));
			this.zombieDamageMultiplier = new ZombieDamageMultiplier(reader.readValue<float>("Zombie_Damage"), reader.readValue<float>("Zombie_Leg_Multiplier"), reader.readValue<float>("Zombie_Arm_Multiplier"), reader.readValue<float>("Zombie_Spine_Multiplier"), reader.readValue<float>("Zombie_Skull_Multiplier"));
			this.animalDamageMultiplier = new AnimalDamageMultiplier(reader.readValue<float>("Animal_Damage"), reader.readValue<float>("Animal_Leg_Multiplier"), reader.readValue<float>("Animal_Spine_Multiplier"), reader.readValue<float>("Animal_Skull_Multiplier"));
			this.barricadeDamage = reader.readValue<float>("Barricade_Damage");
			this.structureDamage = reader.readValue<float>("Structure_Damage");
			this.vehicleDamage = reader.readValue<float>("Vehicle_Damage");
			this.resourceDamage = reader.readValue<float>("Resource_Damage");
			this.objectDamage = reader.readValue<float>("Object_Damage");
			this.durability = reader.readValue<float>("Durability");
			this.wear = reader.readValue<byte>("Wear");
			this.isInvulnerable = reader.readValue<bool>("Invulnerable");
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<byte>("Blade_ID", this.bladeID);
			writer.writeValue<float>("Range", this.range);
			writer.writeValue<float>("Player_Damage", this.playerDamageMultiplier.damage);
			writer.writeValue<float>("Player_Leg_Multiplier", this.playerDamageMultiplier.leg);
			writer.writeValue<float>("Player_Arm_Multiplier", this.playerDamageMultiplier.arm);
			writer.writeValue<float>("Player_Spine_Multiplier", this.playerDamageMultiplier.spine);
			writer.writeValue<float>("Player_Skull_Multiplier", this.playerDamageMultiplier.skull);
			writer.writeValue<float>("Zombie_Damage", this.zombieDamageMultiplier.damage);
			writer.writeValue<float>("Zombie_Leg_Multiplier", this.zombieDamageMultiplier.leg);
			writer.writeValue<float>("Zombie_Arm_Multiplier", this.zombieDamageMultiplier.arm);
			writer.writeValue<float>("Zombie_Spine_Multiplier", this.zombieDamageMultiplier.spine);
			writer.writeValue<float>("Zombie_Skull_Multiplier", this.zombieDamageMultiplier.skull);
			writer.writeValue<float>("Animal_Damage", this.animalDamageMultiplier.damage);
			writer.writeValue<float>("Animal_Leg_Multiplier", this.animalDamageMultiplier.leg);
			writer.writeValue<float>("Animal_Spine_Multiplier", this.animalDamageMultiplier.spine);
			writer.writeValue<float>("Animal_Skull_Multiplier", this.animalDamageMultiplier.skull);
			writer.writeValue<float>("Barricade_Damage", this.barricadeDamage);
			writer.writeValue<float>("Structure_Damage", this.structureDamage);
			writer.writeValue<float>("Vehicle_Damage", this.vehicleDamage);
			writer.writeValue<float>("Resource_Damage", this.resourceDamage);
			writer.writeValue<float>("Object_Damage", this.objectDamage);
			writer.writeValue<float>("Durability", this.durability);
			writer.writeValue<byte>("Wear", this.wear);
			writer.writeValue<bool>("Invulnerable", this.isInvulnerable);
		}

		[Inspectable("#SDG::Asset.Item.Weapon.Range.Name", null)]
		public float range;

		[Inspectable("#SDG::Asset.Item.Weapon.Blade_ID.Name", null)]
		public byte bladeID;

		[Inspectable("#SDG::Asset.Item.Weapon.Player_Damage_Multiplier.Name", null)]
		public PlayerDamageMultiplier playerDamageMultiplier;

		[Inspectable("#SDG::Asset.Item.Weapon.Zombie_Damage_Multiplier.Name", null)]
		public ZombieDamageMultiplier zombieDamageMultiplier;

		[Inspectable("#SDG::Asset.Item.Weapon.Animal_Damage_Multiplier.Name", null)]
		public AnimalDamageMultiplier animalDamageMultiplier;

		[Inspectable("#SDG::Asset.Item.Weapon.Barricade_Damage.Name", null)]
		public float barricadeDamage;

		[Inspectable("#SDG::Asset.Item.Weapon.Structure_Damage.Name", null)]
		public float structureDamage;

		[Inspectable("#SDG::Asset.Item.Weapon.Vehicle_Damage.Name", null)]
		public float vehicleDamage;

		[Inspectable("#SDG::Asset.Item.Weapon.Resource_Damage.Name", null)]
		public float resourceDamage;

		[Inspectable("#SDG::Asset.Item.Weapon.Object_Damage.Name", null)]
		public float objectDamage;

		[Inspectable("#SDG::Asset.Item.Weapon.Durability.Name", null)]
		public float durability;

		[Inspectable("#SDG::Asset.Item.Weapon.Wear.Name", null)]
		public byte wear;

		[Inspectable("#SDG::Asset.Item.Weapon.Is_Invulnerable.Name", null)]
		public bool isInvulnerable;
	}
}
