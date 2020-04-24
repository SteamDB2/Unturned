using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemGunAsset : ItemWeaponAsset, IDevkitAssetSpawnable
	{
		public ItemGunAsset()
		{
		}

		public ItemGunAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this._shoot = (AudioClip)bundle.load("Shoot");
			this._reload = (AudioClip)bundle.load("Reload");
			this._hammer = (AudioClip)bundle.load("Hammer");
			this._aim = (AudioClip)bundle.load("Aim");
			this._minigun = (AudioClip)bundle.load("Minigun");
			this._projectile = (GameObject)bundle.load("Projectile");
			bundle.unload();
		}

		public ItemGunAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this._shoot = (AudioClip)bundle.load("Shoot");
			this._reload = (AudioClip)bundle.load("Reload");
			this._hammer = (AudioClip)bundle.load("Hammer");
			this._aim = (AudioClip)bundle.load("Aim");
			this._minigun = (AudioClip)bundle.load("Minigun");
			this._projectile = (GameObject)bundle.load("Projectile");
			this.ammoMin = data.readByte("Ammo_Min");
			this.ammoMax = data.readByte("Ammo_Max");
			this.sightID = data.readUInt16("Sight");
			this.tacticalID = data.readUInt16("Tactical");
			this.gripID = data.readUInt16("Grip");
			this.barrelID = data.readUInt16("Barrel");
			this.magazineID = data.readUInt16("Magazine");
			int num = data.readInt32("Magazine_Replacements");
			this.magazineReplacements = new MagazineReplacement[num];
			for (int i = 0; i < num; i++)
			{
				ushort id2 = data.readUInt16("Magazine_Replacement_" + i + "_ID");
				string map = data.readString("Magazine_Replacement_" + i + "_Map");
				MagazineReplacement magazineReplacement = default(MagazineReplacement);
				magazineReplacement.id = id2;
				magazineReplacement.map = map;
				this.magazineReplacements[i] = magazineReplacement;
			}
			this.unplace = data.readSingle("Unplace");
			this.replace = data.readSingle("Replace");
			if ((double)this.replace < 0.01)
			{
				this.replace = 1f;
			}
			this.hasSight = data.has("Hook_Sight");
			this.hasTactical = data.has("Hook_Tactical");
			this.hasGrip = data.has("Hook_Grip");
			this.hasBarrel = data.has("Hook_Barrel");
			int num2 = data.readInt32("Magazine_Calibers");
			if (num2 > 0)
			{
				this.magazineCalibers = new ushort[num2];
				for (int j = 0; j < num2; j++)
				{
					this.magazineCalibers[j] = data.readUInt16("Magazine_Caliber_" + j);
				}
				int num3 = data.readInt32("Attachment_Calibers");
				if (num3 > 0)
				{
					this.attachmentCalibers = new ushort[num3];
					for (int k = 0; k < num3; k++)
					{
						this.attachmentCalibers[k] = data.readUInt16("Attachment_Caliber_" + k);
					}
				}
				else
				{
					this.attachmentCalibers = this.magazineCalibers;
				}
			}
			else
			{
				this.magazineCalibers = new ushort[1];
				this.magazineCalibers[0] = data.readUInt16("Caliber");
				this.attachmentCalibers = this.magazineCalibers;
			}
			this.firerate = data.readByte("Firerate");
			this.action = (EAction)Enum.Parse(typeof(EAction), data.readString("Action"), true);
			this.deleteEmptyMagazines = data.has("Delete_Empty_Magazines");
			this.bursts = data.readInt32("Bursts");
			this.hasSafety = data.has("Safety");
			this.hasSemi = data.has("Semi");
			this.hasAuto = data.has("Auto");
			this.hasBurst = (this.bursts > 0);
			this.isTurret = data.has("Turret");
			if (this.hasAuto)
			{
				this.firemode = EFiremode.AUTO;
			}
			else if (this.hasSemi)
			{
				this.firemode = EFiremode.SEMI;
			}
			else if (this.hasBurst)
			{
				this.firemode = EFiremode.BURST;
			}
			else if (this.hasSafety)
			{
				this.firemode = EFiremode.SAFETY;
			}
			this.spreadAim = data.readSingle("Spread_Aim");
			this.spreadHip = data.readSingle("Spread_Hip");
			if (data.has("Recoil_Aim"))
			{
				this.recoilAim = data.readSingle("Recoil_Aim");
				this.useRecoilAim = true;
			}
			else
			{
				this.recoilAim = 1f;
				this.useRecoilAim = false;
			}
			this.recoilMin_x = data.readSingle("Recoil_Min_X");
			this.recoilMin_y = data.readSingle("Recoil_Min_Y");
			this.recoilMax_x = data.readSingle("Recoil_Max_X");
			this.recoilMax_y = data.readSingle("Recoil_Max_Y");
			this.recover_x = data.readSingle("Recover_X");
			this.recover_y = data.readSingle("Recover_Y");
			this.shakeMin_x = data.readSingle("Shake_Min_X");
			this.shakeMin_y = data.readSingle("Shake_Min_Y");
			this.shakeMin_z = data.readSingle("Shake_Min_Z");
			this.shakeMax_x = data.readSingle("Shake_Max_X");
			this.shakeMax_y = data.readSingle("Shake_Max_Y");
			this.shakeMax_z = data.readSingle("Shake_Max_Z");
			this.ballisticSteps = data.readByte("Ballistic_Steps");
			this.ballisticTravel = data.readSingle("Ballistic_Travel");
			if (data.has("Ballistic_Steps"))
			{
				this.ballisticSteps = data.readByte("Ballistic_Steps");
				this.ballisticTravel = data.readSingle("Ballistic_Travel");
			}
			else
			{
				this.ballisticTravel = 10f;
				this.ballisticSteps = (byte)Mathf.CeilToInt(this.range / this.ballisticTravel);
			}
			if (data.has("Ballistic_Drop"))
			{
				this.ballisticDrop = data.readSingle("Ballistic_Drop");
			}
			else
			{
				this.ballisticDrop = 0.002f;
			}
			if (data.has("Ballistic_Force"))
			{
				this.ballisticForce = data.readSingle("Ballistic_Force");
			}
			else
			{
				this.ballisticForce = 0.002f;
			}
			this.reloadTime = data.readSingle("Reload_Time");
			this.hammerTime = data.readSingle("Hammer_Time");
			this.muzzle = data.readUInt16("Muzzle");
			this.explosion = data.readUInt16("Explosion");
			if (data.has("Shell"))
			{
				this.shell = data.readUInt16("Shell");
			}
			else if (this.action == EAction.Pump || this.action == EAction.Break)
			{
				this.shell = 33;
			}
			else if (this.action != EAction.Rail)
			{
				this.shell = 1;
			}
			else
			{
				this.shell = 0;
			}
			if (data.has("Alert_Radius"))
			{
				this.alertRadius = data.readSingle("Alert_Radius");
			}
			else
			{
				this.alertRadius = 48f;
			}
			bundle.unload();
		}

		public AudioClip shoot
		{
			get
			{
				return this._shoot;
			}
		}

		public AudioClip reload
		{
			get
			{
				return this._reload;
			}
		}

		public AudioClip hammer
		{
			get
			{
				return this._hammer;
			}
		}

		public AudioClip aim
		{
			get
			{
				return this._aim;
			}
		}

		public AudioClip minigun
		{
			get
			{
				return this._minigun;
			}
		}

		public GameObject projectile
		{
			get
			{
				return this._projectile;
			}
		}

		public void devkitAssetSpawn()
		{
		}

		public override bool isDangerous
		{
			get
			{
				return true;
			}
		}

		public override string getContext(string desc, byte[] state)
		{
			ushort id = BitConverter.ToUInt16(state, 8);
			ItemMagazineAsset itemMagazineAsset = (ItemMagazineAsset)Assets.find(EAssetType.ITEM, id);
			if (itemMagazineAsset != null)
			{
				desc += PlayerDashboardInventoryUI.localization.format("Ammo", new object[]
				{
					string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemMagazineAsset.rarity)),
						">",
						itemMagazineAsset.itemName,
						"</color>"
					}),
					state[10],
					itemMagazineAsset.amount
				});
			}
			else
			{
				desc += PlayerDashboardInventoryUI.localization.format("Ammo", new object[]
				{
					PlayerDashboardInventoryUI.localization.format("None"),
					0,
					0
				});
			}
			desc += "\n\n";
			return desc;
		}

		public override byte[] getState(EItemOrigin origin)
		{
			byte[] magazineState = this.getMagazineState(this.getMagazineID());
			byte[] array = new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				100,
				100,
				100,
				100,
				100
			};
			array[0] = this.sightState[0];
			array[1] = this.sightState[1];
			array[2] = this.tacticalState[0];
			array[3] = this.tacticalState[1];
			array[4] = this.gripState[0];
			array[5] = this.gripState[1];
			array[6] = this.barrelState[0];
			array[7] = this.barrelState[1];
			array[8] = magazineState[0];
			array[9] = magazineState[1];
			array[10] = ((origin == EItemOrigin.WORLD && Random.value >= ((Provider.modeConfigData == null) ? 0.9f : Provider.modeConfigData.Items.Gun_Bullets_Full_Chance)) ? ((byte)Mathf.CeilToInt((float)Random.Range((int)this.ammoMin, (int)(this.ammoMax + 1)) * ((Provider.modeConfigData == null) ? 1f : Provider.modeConfigData.Items.Gun_Bullets_Multiplier))) : this.ammoMax);
			array[11] = (byte)this.firemode;
			return array;
		}

		public byte[] getState(ushort sight, ushort tactical, ushort grip, ushort barrel, ushort magazine, byte ammo)
		{
			byte[] bytes = BitConverter.GetBytes(sight);
			byte[] bytes2 = BitConverter.GetBytes(tactical);
			byte[] bytes3 = BitConverter.GetBytes(grip);
			byte[] bytes4 = BitConverter.GetBytes(barrel);
			byte[] bytes5 = BitConverter.GetBytes(magazine);
			byte[] array = new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				100,
				100,
				100,
				100,
				100
			};
			array[0] = bytes[0];
			array[1] = bytes[1];
			array[2] = bytes2[0];
			array[3] = bytes2[1];
			array[4] = bytes3[0];
			array[5] = bytes3[1];
			array[6] = bytes4[0];
			array[7] = bytes4[1];
			array[8] = bytes5[0];
			array[9] = bytes5[1];
			array[10] = ammo;
			array[11] = (byte)this.firemode;
			return array;
		}

		[Inspectable("#SDG::Asset.Item.Gun.Sight_ID.Name", null)]
		public ushort sightID
		{
			get
			{
				return this._sightID;
			}
			set
			{
				this._sightID = value;
				this.sightState = BitConverter.GetBytes(this.sightID);
			}
		}

		[Inspectable("#SDG::Asset.Item.Gun.Tactical_ID.Name", null)]
		public ushort tacticalID
		{
			get
			{
				return this._tacticalID;
			}
			set
			{
				this._tacticalID = value;
				this.tacticalState = BitConverter.GetBytes(this.tacticalID);
			}
		}

		[Inspectable("#SDG::Asset.Item.Gun.Grip_ID.Name", null)]
		public ushort gripID
		{
			get
			{
				return this._gripID;
			}
			set
			{
				this._gripID = value;
				this.gripState = BitConverter.GetBytes(this.gripID);
			}
		}

		[Inspectable("#SDG::Asset.Item.Gun.Barrel_ID.Name", null)]
		public ushort barrelID
		{
			get
			{
				return this._barrelID;
			}
			set
			{
				this._barrelID = value;
				this.barrelState = BitConverter.GetBytes(this.barrelID);
			}
		}

		public ushort getMagazineID()
		{
			if (Level.info != null && this.magazineReplacements != null)
			{
				foreach (MagazineReplacement magazineReplacement in this.magazineReplacements)
				{
					if (magazineReplacement.map == Level.info.name)
					{
						return magazineReplacement.id;
					}
				}
			}
			return this.magazineID;
		}

		private byte[] getMagazineState(ushort id)
		{
			return BitConverter.GetBytes(id);
		}

		[Inspectable("#SDG::Asset.Item.Gun.Attachment_Calibers.Name", null)]
		public ushort[] attachmentCalibers { get; private set; }

		[Inspectable("#SDG::Asset.Item.Gun.Magazine_Calibers.Name", null)]
		public ushort[] magazineCalibers { get; private set; }

		public override bool showQuality
		{
			get
			{
				return true;
			}
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.ammoMin = reader.readValue<byte>("Ammo_Min");
			this.ammoMax = reader.readValue<byte>("Ammo_Max");
			this.sightID = reader.readValue<ushort>("Sight_ID");
			this.tacticalID = reader.readValue<ushort>("Tactical_ID");
			this.gripID = reader.readValue<ushort>("Grip_ID");
			this.barrelID = reader.readValue<ushort>("Barrel_ID");
			this.magazineID = reader.readValue<ushort>("Magazine_ID");
			this.unplace = reader.readValue<float>("Unplace");
			this.replace = reader.readValue<float>("Replace");
			if ((double)this.replace < 0.01)
			{
				this.replace = 1f;
			}
			this.hasSight = reader.readValue<bool>("Hook_Sight");
			this.hasTactical = reader.readValue<bool>("Hook_Tactical");
			this.hasGrip = reader.readValue<bool>("Hook_Grip");
			this.hasBarrel = reader.readValue<bool>("Hook_Barrel");
			int num = reader.readArrayLength("Magazine_Calibers");
			if (num > 0)
			{
				this.magazineCalibers = new ushort[num];
				for (int i = 0; i < num; i++)
				{
					this.magazineCalibers[i] = reader.readValue<ushort>(i);
				}
				int num2 = reader.readArrayLength("Attachment_Calibers");
				if (num2 > 0)
				{
					this.attachmentCalibers = new ushort[num2];
					for (int j = 0; j < num2; j++)
					{
						this.attachmentCalibers[j] = reader.readValue<ushort>(j);
					}
				}
				else
				{
					this.attachmentCalibers = this.magazineCalibers;
				}
			}
			else
			{
				this.magazineCalibers = new ushort[1];
				this.magazineCalibers[0] = reader.readValue<ushort>("Caliber");
				this.attachmentCalibers = this.magazineCalibers;
			}
			this.firerate = reader.readValue<byte>("Firerate");
			this.action = reader.readValue<EAction>("Action");
			this.deleteEmptyMagazines = reader.readValue<bool>("Delete_Empty_Magazines");
			this.bursts = reader.readValue<int>("Bursts");
			this.hasSafety = reader.readValue<bool>("Safety");
			this.hasSemi = reader.readValue<bool>("Semi");
			this.hasAuto = reader.readValue<bool>("Auto");
			this.hasBurst = (this.bursts > 0);
			this.isTurret = reader.readValue<bool>("Turret");
			if (this.hasAuto)
			{
				this.firemode = EFiremode.AUTO;
			}
			else if (this.hasSemi)
			{
				this.firemode = EFiremode.SEMI;
			}
			else if (this.hasBurst)
			{
				this.firemode = EFiremode.BURST;
			}
			else if (this.hasSafety)
			{
				this.firemode = EFiremode.SAFETY;
			}
			this.spreadAim = reader.readValue<float>("Spread_Aim");
			this.spreadHip = reader.readValue<float>("Spread_Hip");
			this.recoilAim = reader.readValue<float>("Recoil_Aim");
			this.useRecoilAim = reader.readValue<bool>("Use_Recoil_Aim");
			this.recoilMin_x = reader.readValue<float>("Recoil_Min_X");
			this.recoilMin_y = reader.readValue<float>("Recoil_Min_Y");
			this.recoilMax_x = reader.readValue<float>("Recoil_Max_X");
			this.recoilMax_y = reader.readValue<float>("Recoil_Max_Y");
			this.recover_x = reader.readValue<float>("Recover_X");
			this.recover_y = reader.readValue<float>("Recover_Y");
			this.shakeMin_x = reader.readValue<float>("Shake_Min_X");
			this.shakeMin_y = reader.readValue<float>("Shake_Min_Y");
			this.shakeMin_z = reader.readValue<float>("Shake_Min_Z");
			this.shakeMax_x = reader.readValue<float>("Shake_Max_X");
			this.shakeMax_y = reader.readValue<float>("Shake_Max_Y");
			this.shakeMax_z = reader.readValue<float>("Shake_Max_Z");
			this.ballisticSteps = reader.readValue<byte>("Ballistic_Steps");
			this.ballisticTravel = (float)reader.readValue<byte>("Ballistic_Travel");
			this.ballisticDrop = reader.readValue<float>("Ballistic_Drop");
			this.ballisticForce = reader.readValue<float>("Ballistic_Force");
			this.reloadTime = reader.readValue<float>("Reload_Time");
			this.hammerTime = reader.readValue<float>("Hammer_Time");
			this.muzzle = reader.readValue<ushort>("Muzzle");
			this.explosion = reader.readValue<ushort>("Explosion");
			this.shell = reader.readValue<ushort>("Shell");
			if (reader.containsKey("Alert_Radius"))
			{
				this.alertRadius = reader.readValue<float>("Alert_Radius");
			}
			else
			{
				this.alertRadius = 48f;
			}
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<byte>("Ammo_Min", this.ammoMin);
			writer.writeValue<byte>("Ammo_Max", this.ammoMax);
			writer.writeValue<ushort>("Sight_ID", this.sightID);
			writer.writeValue<ushort>("Tactical_ID", this.tacticalID);
			writer.writeValue<ushort>("Grip_ID", this.gripID);
			writer.writeValue<ushort>("Barrel_ID", this.barrelID);
			writer.writeValue<ushort>("Magazine_ID", this.magazineID);
			writer.writeValue<float>("Unplace", this.unplace);
			writer.writeValue<float>("Replace", this.replace);
			writer.writeValue<bool>("Hook_Sight", this.hasSight);
			writer.writeValue<bool>("Hook_Tactical", this.hasTactical);
			writer.writeValue<bool>("Hook_Grip", this.hasGrip);
			writer.writeValue<bool>("Hook_Barrel", this.hasBarrel);
			writer.beginArray("Magazine_Calibers");
			foreach (ushort value in this.magazineCalibers)
			{
				writer.writeValue<ushort>(value);
			}
			writer.endArray();
			writer.beginArray("Attachment_Calibers");
			foreach (ushort value2 in this.attachmentCalibers)
			{
				writer.writeValue<ushort>(value2);
			}
			writer.endArray();
			writer.writeValue<byte>("Firerate", this.firerate);
			writer.writeValue<EAction>("Action", this.action);
			writer.writeValue<bool>("Delete_Empty_Magazines", this.deleteEmptyMagazines);
			writer.writeValue<int>("Bursts", this.bursts);
			writer.writeValue<bool>("Safety", this.hasSafety);
			writer.writeValue<bool>("Semi", this.hasSemi);
			writer.writeValue<bool>("Auto", this.hasAuto);
			writer.writeValue<bool>("Turret", this.isTurret);
			writer.writeValue<float>("Spread_Aim", this.spreadAim);
			writer.writeValue<float>("Spread_Hip", this.spreadHip);
			writer.writeValue<float>("Recoil_Aim", this.recoilAim);
			writer.writeValue<bool>("Use_Recoil_Aim", this.useRecoilAim);
			writer.writeValue<float>("Recoil_Min_X", this.recoilMin_x);
			writer.writeValue<float>("Recoil_Min_Y", this.recoilMin_y);
			writer.writeValue<float>("Recoil_Max_X", this.recoilMax_x);
			writer.writeValue<float>("Recoil_Max_Y", this.recoilMax_y);
			writer.writeValue<float>("Recover_X", this.recover_x);
			writer.writeValue<float>("Recover_Y", this.recover_y);
			writer.writeValue<float>("Shake_Min_X", this.shakeMin_x);
			writer.writeValue<float>("Shake_Min_Y", this.shakeMin_y);
			writer.writeValue<float>("Shake_Min_Z", this.shakeMin_z);
			writer.writeValue<float>("Shake_Max_X", this.shakeMax_x);
			writer.writeValue<float>("Shake_Max_Y", this.shakeMax_y);
			writer.writeValue<float>("Shake_Max_Z", this.shakeMax_z);
			writer.writeValue<byte>("Ballistic_Steps", this.ballisticSteps);
			writer.writeValue<float>("Ballistic_Travel", this.ballisticTravel);
			writer.writeValue<float>("Ballistic_Drop", this.ballisticDrop);
			writer.writeValue<float>("Ballistic_Force", this.ballisticForce);
			writer.writeValue<float>("Reload_Time", this.reloadTime);
			writer.writeValue<float>("Hammer_Time", this.hammerTime);
			writer.writeValue<ushort>("Muzzle", this.muzzle);
			writer.writeValue<ushort>("Explosion", this.explosion);
			writer.writeValue<ushort>("Shell", this.shell);
			writer.writeValue<float>("Alert_Radius", this.alertRadius);
		}

		protected AudioClip _shoot;

		protected AudioClip _reload;

		protected AudioClip _hammer;

		protected AudioClip _aim;

		protected AudioClip _minigun;

		protected GameObject _projectile;

		[Inspectable("#SDG::Asset.Item.Gun.Alert_Radius.Name", null)]
		public float alertRadius;

		[Inspectable("#SDG::Asset.Item.Gun.Ammo_Min.Name", null)]
		public byte ammoMin;

		[Inspectable("#SDG::Asset.Item.Gun.Ammo_Max.Name", null)]
		public byte ammoMax;

		private ushort _sightID;

		private byte[] sightState;

		private ushort _tacticalID;

		private byte[] tacticalState;

		private ushort _gripID;

		private byte[] gripState;

		private ushort _barrelID;

		private byte[] barrelState;

		private ushort magazineID;

		private MagazineReplacement[] magazineReplacements;

		[Inspectable("#SDG::Asset.Item.Gun.Unplace.Name", null)]
		public float unplace;

		[Inspectable("#SDG::Asset.Item.Gun.Replace.Name", null)]
		public float replace;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Sight.Name", null)]
		public bool hasSight;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Tactical.Name", null)]
		public bool hasTactical;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Grip.Name", null)]
		public bool hasGrip;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Barrel.Name", null)]
		public bool hasBarrel;

		[Inspectable("#SDG::Asset.Item.Gun.Firerate.Name", null)]
		public byte firerate;

		[Inspectable("#SDG::Asset.Item.Gun.Action.Name", null)]
		public EAction action;

		[Inspectable("#SDG::Asset.Item.Gun.Delete_Empty_Magazines.Name", null)]
		public bool deleteEmptyMagazines;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Safety.Name", null)]
		public bool hasSafety;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Semi.Name", null)]
		public bool hasSemi;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Auto.Name", null)]
		public bool hasAuto;

		[Inspectable("#SDG::Asset.Item.Gun.Has_Burst.Name", null)]
		public bool hasBurst;

		[Inspectable("#SDG::Asset.Item.Gun.Is_Turret.Name", null)]
		public bool isTurret;

		[Inspectable("#SDG::Asset.Item.Gun.Bursts.Name", null)]
		public int bursts;

		private EFiremode firemode;

		[Inspectable("#SDG::Asset.Item.Gun.Spread_Aim.Name", null)]
		public float spreadAim;

		[Inspectable("#SDG::Asset.Item.Gun.Spread_Hip.Name", null)]
		public float spreadHip;

		[Inspectable("#SDG::Asset.Item.Gun.Recoil_Aim.Name", null)]
		public float recoilAim;

		[Inspectable("#SDG::Asset.Item.Gun.Use_Recoil_Aim.Name", null)]
		public bool useRecoilAim;

		[Inspectable("#SDG::Asset.Item.Gun.Recoil_Min_X.Name", null)]
		public float recoilMin_x;

		[Inspectable("#SDG::Asset.Item.Gun.Recoil_Min_Y.Name", null)]
		public float recoilMin_y;

		[Inspectable("#SDG::Asset.Item.Gun.Recoil_Max_X.Name", null)]
		public float recoilMax_x;

		[Inspectable("#SDG::Asset.Item.Gun.Recoil_Max_Y.Name", null)]
		public float recoilMax_y;

		[Inspectable("#SDG::Asset.Item.Gun.Recover_X.Name", null)]
		public float recover_x;

		[Inspectable("#SDG::Asset.Item.Gun.Recover_Y.Name", null)]
		public float recover_y;

		[Inspectable("#SDG::Asset.Item.Gun.Shake_Min_X.Name", null)]
		public float shakeMin_x;

		[Inspectable("#SDG::Asset.Item.Gun.Shake_Min_Y.Name", null)]
		public float shakeMin_y;

		[Inspectable("#SDG::Asset.Item.Gun.Shake_Min_Z.Name", null)]
		public float shakeMin_z;

		[Inspectable("#SDG::Asset.Item.Gun.Shake_Max_X.Name", null)]
		public float shakeMax_x;

		[Inspectable("#SDG::Asset.Item.Gun.Shake_Max_Y.Name", null)]
		public float shakeMax_y;

		[Inspectable("#SDG::Asset.Item.Gun.Shake_Max_Z.Name", null)]
		public float shakeMax_z;

		[Inspectable("#SDG::Asset.Item.Gun.Ballistic_Steps.Name", null)]
		public byte ballisticSteps;

		[Inspectable("#SDG::Asset.Item.Gun.Ballistic_Travel.Name", null)]
		public float ballisticTravel;

		[Inspectable("#SDG::Asset.Item.Gun.Ballistic_Drop.Name", null)]
		public float ballisticDrop;

		[Inspectable("#SDG::Asset.Item.Gun.Ballistic_Force.Name", null)]
		public float ballisticForce;

		[Inspectable("#SDG::Asset.Item.Gun.Reload_Time.Name", null)]
		public float reloadTime;

		[Inspectable("#SDG::Asset.Item.Gun.Hammer_Time.Name", null)]
		public float hammerTime;

		[Inspectable("#SDG::Asset.Item.Gun.Muzzle.Name", null)]
		public ushort muzzle;

		[Inspectable("#SDG::Asset.Item.Gun.Shell.Name", null)]
		public ushort shell;

		[Inspectable("#SDG::Asset.Item.Gun.Explosion.Name", null)]
		public ushort explosion;
	}
}
