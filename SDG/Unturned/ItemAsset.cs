using System;
using System.Collections;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemAsset : Asset
	{
		public ItemAsset()
		{
			this._animations = new AnimationClip[0];
			this._blueprints = new List<Blueprint>();
			this._actions = new List<Action>();
		}

		public ItemAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this._itemName = localization.format("Name");
			this._itemDescription = localization.format("Description");
			this._itemDescription = ItemTool.filterRarityRichText(this.itemDescription);
			this._equip = (AudioClip)bundle.load("Equip");
			GameObject gameObject = (GameObject)bundle.load("Animations");
			if (gameObject != null)
			{
				Animation component = gameObject.GetComponent<Animation>();
				this._animations = new AnimationClip[component.GetClipCount()];
				int num = 0;
				IEnumerator enumerator = component.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						AnimationState animationState = (AnimationState)obj;
						this.animations[num] = animationState.clip;
						num++;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			else
			{
				this._animations = new AnimationClip[0];
			}
			this._item = (GameObject)bundle.load("Item");
			if (this.item == null)
			{
				throw new NotSupportedException("Missing item gameobject");
			}
			if (this.item.transform.FindChild("Icon") != null && this.item.transform.FindChild("Icon").GetComponent<Camera>() != null)
			{
				Assets.errors.Add(this.itemName + " icon has a camera attached!");
			}
			if (!Dedicator.isDedicated)
			{
				this._albedoBase = (Texture2D)bundle.load("Albedo_Base");
				this._metallicBase = (Texture2D)bundle.load("Metallic_Base");
				this._emissionBase = (Texture2D)bundle.load("Emission_Base");
			}
		}

		public ItemAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this.isPro = data.has("Pro");
			if (id < 2000 && !bundle.hasResource && !this.isPro && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			if (!this.isPro)
			{
				this._itemName = localization.format("Name");
				this._itemDescription = localization.format("Description");
				this._itemDescription = ItemTool.filterRarityRichText(this.itemDescription);
			}
			this.type = (EItemType)Enum.Parse(typeof(EItemType), data.readString("Type"), true);
			if (data.has("Rarity"))
			{
				this.rarity = (EItemRarity)Enum.Parse(typeof(EItemRarity), data.readString("Rarity"), true);
			}
			else
			{
				this.rarity = EItemRarity.COMMON;
			}
			if (this.isPro)
			{
				if (this.type == EItemType.SHIRT)
				{
					this._proPath = "/Shirts";
				}
				else if (this.type == EItemType.PANTS)
				{
					this._proPath = "/Pants";
				}
				else if (this.type == EItemType.HAT)
				{
					this._proPath = "/Hats";
				}
				else if (this.type == EItemType.BACKPACK)
				{
					this._proPath = "/Backpacks";
				}
				else if (this.type == EItemType.VEST)
				{
					this._proPath = "/Vests";
				}
				else if (this.type == EItemType.MASK)
				{
					this._proPath = "/Masks";
				}
				else if (this.type == EItemType.GLASSES)
				{
					this._proPath = "/Glasses";
				}
				else if (this.type == EItemType.KEY)
				{
					this._proPath = "/Keys";
				}
				else if (this.type == EItemType.BOX)
				{
					this._proPath = "/Boxes";
				}
				this._proPath = this._proPath + "/" + this.name;
			}
			this.size_x = data.readByte("Size_X");
			if (this.size_x < 1)
			{
				this.size_x = 1;
			}
			this.size_y = data.readByte("Size_Y");
			if (this.size_y < 1)
			{
				this.size_y = 1;
			}
			this.size_z = data.readSingle("Size_Z");
			this.size2_z = data.readSingle("Size2_Z");
			this.amount = data.readByte("Amount");
			if (this.amount < 1)
			{
				this.amount = 1;
			}
			this.countMin = data.readByte("Count_Min");
			if (this.countMin < 1)
			{
				this.countMin = 1;
			}
			this.countMax = data.readByte("Count_Max");
			if (this.countMax < 1)
			{
				this.countMax = 1;
			}
			if (data.has("Quality_Min"))
			{
				this.qualityMin = data.readByte("Quality_Min");
			}
			else
			{
				this.qualityMin = 10;
			}
			if (data.has("Quality_Max"))
			{
				this.qualityMax = data.readByte("Quality_Max");
			}
			else
			{
				this.qualityMax = 90;
			}
			this.isBackward = data.has("Backward");
			this.useable = data.readString("Useable");
			this.isUseable = !string.IsNullOrEmpty(this.useable);
			if (this.isUseable)
			{
				this._equip = (AudioClip)bundle.load("Equip");
				if (!this.isPro)
				{
					GameObject gameObject = (GameObject)bundle.load("Animations");
					if (gameObject != null)
					{
						Animation component = gameObject.GetComponent<Animation>();
						this._animations = new AnimationClip[component.GetClipCount()];
						int num = 0;
						IEnumerator enumerator = component.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								AnimationState animationState = (AnimationState)obj;
								this.animations[num] = animationState.clip;
								num++;
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
					}
					else
					{
						this._animations = new AnimationClip[0];
					}
				}
			}
			string text = data.readString("Slot");
			if (text == null)
			{
				this.slot = ESlotType.NONE;
			}
			else
			{
				this.slot = (ESlotType)Enum.Parse(typeof(ESlotType), text, true);
			}
			if (!Dedicator.isDedicated || this.type == EItemType.GUN || this.type == EItemType.MELEE)
			{
				this._item = (GameObject)bundle.load("Item");
				if (this.item == null)
				{
					throw new NotSupportedException("Missing item gameobject");
				}
				if (this.item.transform.FindChild("Icon") != null && this.item.transform.FindChild("Icon").GetComponent<Camera>() != null)
				{
					Assets.errors.Add(this.itemName + " icon has a camera attached!");
				}
			}
			byte b = data.readByte("Blueprints");
			byte b2 = data.readByte("Actions");
			this._blueprints = new List<Blueprint>((int)b);
			this._actions = new List<Action>((int)b2);
			for (byte b3 = 0; b3 < b; b3 += 1)
			{
				if (!data.has("Blueprint_" + b3 + "_Type"))
				{
					throw new NotSupportedException("Missing blueprint type");
				}
				EBlueprintType newType = (EBlueprintType)Enum.Parse(typeof(EBlueprintType), data.readString("Blueprint_" + b3 + "_Type"), true);
				byte b4 = data.readByte("Blueprint_" + b3 + "_Supplies");
				if (b4 < 1)
				{
					b4 = 1;
				}
				BlueprintSupply[] array = new BlueprintSupply[(int)b4];
				byte b5 = 0;
				while ((int)b5 < array.Length)
				{
					ushort newID = data.readUInt16(string.Concat(new object[]
					{
						"Blueprint_",
						b3,
						"_Supply_",
						b5,
						"_ID"
					}));
					bool newCritical = data.has(string.Concat(new object[]
					{
						"Blueprint_",
						b3,
						"_Supply_",
						b5,
						"_Critical"
					}));
					byte b6 = data.readByte(string.Concat(new object[]
					{
						"Blueprint_",
						b3,
						"_Supply_",
						b5,
						"_Amount"
					}));
					if (b6 < 1)
					{
						b6 = 1;
					}
					array[(int)b5] = new BlueprintSupply(newID, newCritical, b6);
					b5 += 1;
				}
				byte b7 = data.readByte("Blueprint_" + b3 + "_Outputs");
				BlueprintOutput[] array2;
				if (b7 > 0)
				{
					array2 = new BlueprintOutput[(int)b7];
					byte b8 = 0;
					while ((int)b8 < array2.Length)
					{
						ushort newID2 = data.readUInt16(string.Concat(new object[]
						{
							"Blueprint_",
							b3,
							"_Output_",
							b8,
							"_ID"
						}));
						byte b9 = data.readByte(string.Concat(new object[]
						{
							"Blueprint_",
							b3,
							"_Output_",
							b8,
							"_Amount"
						}));
						if (b9 < 1)
						{
							b9 = 1;
						}
						array2[(int)b8] = new BlueprintOutput(newID2, b9);
						b8 += 1;
					}
				}
				else
				{
					array2 = new BlueprintOutput[1];
					ushort num2 = data.readUInt16("Blueprint_" + b3 + "_Product");
					if (num2 == 0)
					{
						num2 = id;
					}
					byte b10 = data.readByte("Blueprint_" + b3 + "_Products");
					if (b10 < 1)
					{
						b10 = 1;
					}
					array2[0] = new BlueprintOutput(num2, b10);
				}
				ushort newTool = data.readUInt16("Blueprint_" + b3 + "_Tool");
				bool newToolCritical = data.has("Blueprint_" + b3 + "_Tool_Critical");
				ushort newBuild = data.readUInt16("Blueprint_" + b3 + "_Build");
				byte b11 = data.readByte("Blueprint_" + b3 + "_Level");
				EBlueprintSkill newSkill = EBlueprintSkill.NONE;
				if (b11 > 0)
				{
					newSkill = (EBlueprintSkill)Enum.Parse(typeof(EBlueprintSkill), data.readString("Blueprint_" + b3 + "_Skill"), true);
				}
				bool newTransferState = data.has("Blueprint_" + b3 + "_State_Transfer");
				string newMap = data.readString("Blueprint_" + b3 + "_Map");
				this.blueprints.Add(new Blueprint(id, b3, newType, array, array2, newTool, newToolCritical, newBuild, b11, newSkill, newTransferState, newMap));
			}
			for (byte b12 = 0; b12 < b2; b12 += 1)
			{
				if (!data.has("Action_" + b12 + "_Type"))
				{
					throw new NotSupportedException("Missing action type");
				}
				EActionType newType2 = (EActionType)Enum.Parse(typeof(EActionType), data.readString("Action_" + b12 + "_Type"), true);
				byte b13 = data.readByte("Action_" + b12 + "_Blueprints");
				if (b13 < 1)
				{
					b13 = 1;
				}
				ActionBlueprint[] array3 = new ActionBlueprint[(int)b13];
				byte b14 = 0;
				while ((int)b14 < array3.Length)
				{
					byte newID3 = data.readByte(string.Concat(new object[]
					{
						"Action_",
						b12,
						"_Blueprint_",
						b14,
						"_Index"
					}));
					bool newLink = data.has(string.Concat(new object[]
					{
						"Action_",
						b12,
						"_Blueprint_",
						b14,
						"_Link"
					}));
					array3[(int)b14] = new ActionBlueprint(newID3, newLink);
					b14 += 1;
				}
				string newText = data.readString("Action_" + b12 + "_Text");
				string newTooltip = data.readString("Action_" + b12 + "_Tooltip");
				string newKey = data.readString("Action_" + b12 + "_Key");
				ushort num3 = data.readUInt16("Action_" + b12 + "_Source");
				if (num3 == 0)
				{
					num3 = id;
				}
				this.actions.Add(new Action(num3, newType2, array3, newText, newTooltip, newKey));
			}
			if (b2 == 0)
			{
				bool flag = false;
				byte b15 = 0;
				while ((int)b15 < this.blueprints.Count)
				{
					Blueprint blueprint = this.blueprints[(int)b15];
					if (blueprint.type == EBlueprintType.REPAIR)
					{
						Action item = new Action(id, EActionType.BLUEPRINT, new ActionBlueprint[]
						{
							new ActionBlueprint(b15, true)
						}, null, null, "Repair");
						this.actions.Insert(0, item);
					}
					else if (blueprint.type == EBlueprintType.AMMO)
					{
						flag = true;
					}
					else if (blueprint.supplies.Length == 1 && blueprint.supplies[0].id == id)
					{
						Action item2 = new Action(id, EActionType.BLUEPRINT, new ActionBlueprint[]
						{
							new ActionBlueprint(b15, this.type == EItemType.GUN || this.type == EItemType.MELEE)
						}, null, null, "Salvage");
						this.actions.Add(item2);
					}
					b15 += 1;
				}
				if (flag)
				{
					List<ActionBlueprint> list = new List<ActionBlueprint>();
					byte b16 = 0;
					while ((int)b16 < this.blueprints.Count)
					{
						Blueprint blueprint2 = this.blueprints[(int)b16];
						if (blueprint2.type == EBlueprintType.AMMO)
						{
							ActionBlueprint item3 = new ActionBlueprint(b16, true);
							list.Add(item3);
						}
						b16 += 1;
					}
					Action item4 = new Action(id, EActionType.BLUEPRINT, list.ToArray(), null, null, "Refill");
					this.actions.Add(item4);
				}
			}
			this._shouldVerifyHash = !data.has("Bypass_Hash_Verification");
			this.overrideShowQuality = data.has("Override_Show_Quality");
			if (!Dedicator.isDedicated)
			{
				this._albedoBase = (Texture2D)bundle.load("Albedo_Base");
				this._metallicBase = (Texture2D)bundle.load("Metallic_Base");
				this._emissionBase = (Texture2D)bundle.load("Emission_Base");
			}
		}

		public bool shouldVerifyHash
		{
			get
			{
				return this._shouldVerifyHash;
			}
		}

		public string itemName
		{
			get
			{
				return this._itemName;
			}
		}

		public string itemDescription
		{
			get
			{
				return this._itemDescription;
			}
		}

		public string proPath
		{
			get
			{
				return this._proPath;
			}
		}

		public byte[] getState()
		{
			return this.getState(false);
		}

		public byte[] getState(bool isFull)
		{
			return this.getState((!isFull) ? EItemOrigin.WORLD : EItemOrigin.ADMIN);
		}

		public virtual byte[] getState(EItemOrigin origin)
		{
			return new byte[0];
		}

		public virtual string getContext(string desc, byte[] state)
		{
			return desc;
		}

		public byte count
		{
			get
			{
				float num;
				float num2;
				if (Provider.modeConfigData != null)
				{
					if (this.type == EItemType.MAGAZINE)
					{
						num = Provider.modeConfigData.Items.Magazine_Bullets_Full_Chance;
						num2 = Provider.modeConfigData.Items.Magazine_Bullets_Multiplier;
					}
					else
					{
						num = Provider.modeConfigData.Items.Crate_Bullets_Full_Chance;
						num2 = Provider.modeConfigData.Items.Crate_Bullets_Multiplier;
					}
				}
				else
				{
					num = 0.9f;
					num2 = 1f;
				}
				if (Random.value < num)
				{
					return this.amount;
				}
				return (byte)Mathf.CeilToInt((float)Random.Range((int)this.countMin, (int)(this.countMax + 1)) * num2);
			}
		}

		public byte quality
		{
			get
			{
				if (Random.value < ((Provider.modeConfigData == null) ? 0.9f : Provider.modeConfigData.Items.Quality_Full_Chance))
				{
					return 100;
				}
				return (byte)Mathf.CeilToInt((float)Random.Range((int)this.qualityMin, (int)(this.qualityMax + 1)) * ((Provider.modeConfigData == null) ? 1f : Provider.modeConfigData.Items.Quality_Multiplier));
			}
		}

		public GameObject item
		{
			get
			{
				return this._item;
			}
		}

		public AudioClip equip
		{
			get
			{
				return this._equip;
			}
		}

		public AnimationClip[] animations
		{
			get
			{
				return this._animations;
			}
		}

		public List<Blueprint> blueprints
		{
			get
			{
				return this._blueprints;
			}
		}

		public List<Action> actions
		{
			get
			{
				return this._actions;
			}
		}

		public bool overrideShowQuality { get; protected set; }

		public virtual bool showQuality
		{
			get
			{
				return this.overrideShowQuality;
			}
		}

		public Texture albedoBase
		{
			get
			{
				return this._albedoBase;
			}
		}

		public Texture metallicBase
		{
			get
			{
				return this._metallicBase;
			}
		}

		public Texture emissionBase
		{
			get
			{
				return this._emissionBase;
			}
		}

		public virtual bool isDangerous
		{
			get
			{
				return false;
			}
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.ITEM;
			}
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.isPro = reader.readValue<bool>("Is_Pro");
			this.type = reader.readValue<EItemType>("Type");
			this.rarity = reader.readValue<EItemRarity>("Rarity");
			if (this.isPro)
			{
				if (this.type == EItemType.SHIRT)
				{
					this._proPath = "/Shirts";
				}
				else if (this.type == EItemType.PANTS)
				{
					this._proPath = "/Pants";
				}
				else if (this.type == EItemType.HAT)
				{
					this._proPath = "/Hats";
				}
				else if (this.type == EItemType.BACKPACK)
				{
					this._proPath = "/Backpacks";
				}
				else if (this.type == EItemType.VEST)
				{
					this._proPath = "/Vests";
				}
				else if (this.type == EItemType.MASK)
				{
					this._proPath = "/Masks";
				}
				else if (this.type == EItemType.GLASSES)
				{
					this._proPath = "/Glasses";
				}
				else if (this.type == EItemType.KEY)
				{
					this._proPath = "/Keys";
				}
				else if (this.type == EItemType.BOX)
				{
					this._proPath = "/Boxes";
				}
				this._proPath = this._proPath + "/" + this.name;
			}
			this.size_x = reader.readValue<byte>("Size_X");
			if (this.size_x < 1)
			{
				this.size_x = 1;
			}
			this.size_y = reader.readValue<byte>("Size_Y");
			if (this.size_y < 1)
			{
				this.size_y = 1;
			}
			this.size_z = reader.readValue<float>("Size_Z");
			this.size2_z = reader.readValue<float>("Size2_Z");
			this.amount = reader.readValue<byte>("Amount");
			if (this.amount < 1)
			{
				this.amount = 1;
			}
			this.countMin = reader.readValue<byte>("Count_Min");
			if (this.countMin < 1)
			{
				this.countMin = 1;
			}
			this.countMax = reader.readValue<byte>("Count_Max");
			if (this.countMax < 1)
			{
				this.countMax = 1;
			}
			this.qualityMin = reader.readValue<byte>("Quality_Min");
			this.qualityMax = reader.readValue<byte>("Quality_Max");
			this.isBackward = reader.readValue<bool>("Backward");
			this.useable = reader.readValue<string>("Useable");
			this.isUseable = !string.IsNullOrEmpty(this.useable);
			this.slot = reader.readValue<ESlotType>("Slot");
			int num = reader.readArrayLength("Blueprints");
			this._blueprints = new List<Blueprint>(num);
			for (int i = 0; i < num; i++)
			{
				IFormattedFileReader formattedFileReader = reader.readObject(i);
				EBlueprintType newType = formattedFileReader.readValue<EBlueprintType>("Type");
				int num2 = formattedFileReader.readArrayLength("Supplies");
				BlueprintSupply[] array = new BlueprintSupply[num2];
				for (int j = 0; j < num2; j++)
				{
					IFormattedFileReader formattedFileReader2 = formattedFileReader.readObject(j);
					ushort newID = formattedFileReader2.readValue<ushort>("ID");
					bool newCritical = formattedFileReader2.readValue<bool>("Critical");
					byte newAmount = formattedFileReader2.readValue<byte>("Amount");
					array[j] = new BlueprintSupply(newID, newCritical, newAmount);
				}
				int num3 = formattedFileReader.readArrayLength("Output");
				BlueprintOutput[] array2 = new BlueprintOutput[num3];
				for (int k = 0; k < num3; k++)
				{
					IFormattedFileReader formattedFileReader3 = formattedFileReader.readObject(k);
					ushort newID2 = formattedFileReader3.readValue<ushort>("ID");
					byte newAmount2 = formattedFileReader3.readValue<byte>("Amount");
					array2[k] = new BlueprintOutput(newID2, newAmount2);
				}
				ushort newTool = formattedFileReader.readValue<ushort>("Tool");
				bool newToolCritical = formattedFileReader.readValue<bool>("Tool_Critical");
				ushort newBuild = formattedFileReader.readValue<ushort>("Build");
				byte newLevel = formattedFileReader.readValue<byte>("Level");
				EBlueprintSkill newSkill = formattedFileReader.readValue<EBlueprintSkill>("Skill");
				bool newTransferState = formattedFileReader.readValue<bool>("Transfer_State");
				string newMap = formattedFileReader.readValue("Map");
				this.blueprints.Add(new Blueprint(this.id, (byte)i, newType, array, array2, newTool, newToolCritical, newBuild, newLevel, newSkill, newTransferState, newMap));
			}
			int num4 = reader.readArrayLength("Actions");
			this._actions = new List<Action>(num4);
			byte b = 0;
			while ((int)b < num4)
			{
				IFormattedFileReader formattedFileReader4 = reader.readObject((int)b);
				EActionType newType2 = formattedFileReader4.readValue<EActionType>("Type");
				int num5 = formattedFileReader4.readArrayLength("Blueprints");
				ActionBlueprint[] array3 = new ActionBlueprint[num5];
				byte b2 = 0;
				while ((int)b2 < array3.Length)
				{
					IFormattedFileReader formattedFileReader5 = formattedFileReader4.readObject((int)b2);
					byte newID3 = formattedFileReader5.readValue<byte>("Index");
					bool newLink = formattedFileReader5.readValue<bool>("Is_Link");
					array3[(int)b2] = new ActionBlueprint(newID3, newLink);
					b2 += 1;
				}
				string newText = formattedFileReader4.readValue<string>("Text");
				string newTooltip = formattedFileReader4.readValue<string>("Tooltip");
				string newKey = formattedFileReader4.readValue<string>("Key");
				ushort num6 = formattedFileReader4.readValue<ushort>("Source");
				if (num6 == 0)
				{
					num6 = this.id;
				}
				this.actions.Add(new Action(num6, newType2, array3, newText, newTooltip, newKey));
				b += 1;
			}
			if (num4 == 0)
			{
				bool flag = false;
				byte b3 = 0;
				while ((int)b3 < this.blueprints.Count)
				{
					Blueprint blueprint = this.blueprints[(int)b3];
					if (blueprint.type == EBlueprintType.REPAIR)
					{
						Action item = new Action(this.id, EActionType.BLUEPRINT, new ActionBlueprint[]
						{
							new ActionBlueprint(b3, true)
						}, null, null, "Repair");
						this.actions.Insert(0, item);
					}
					else if (blueprint.type == EBlueprintType.AMMO)
					{
						flag = true;
					}
					else if (blueprint.supplies.Length == 1 && blueprint.supplies[0].id == this.id)
					{
						Action item2 = new Action(this.id, EActionType.BLUEPRINT, new ActionBlueprint[]
						{
							new ActionBlueprint(b3, this.type == EItemType.GUN || this.type == EItemType.MELEE)
						}, null, null, "Salvage");
						this.actions.Add(item2);
					}
					b3 += 1;
				}
				if (flag)
				{
					List<ActionBlueprint> list = new List<ActionBlueprint>();
					byte b4 = 0;
					while ((int)b4 < this.blueprints.Count)
					{
						Blueprint blueprint2 = this.blueprints[(int)b4];
						if (blueprint2.type == EBlueprintType.AMMO)
						{
							ActionBlueprint item3 = new ActionBlueprint(b4, true);
							list.Add(item3);
						}
						b4 += 1;
					}
					Action item4 = new Action(this.id, EActionType.BLUEPRINT, list.ToArray(), null, null, "Refill");
					this.actions.Add(item4);
				}
			}
			this._shouldVerifyHash = reader.readValue<bool>("Should_Verify_Hash");
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<bool>("Is_Pro", this.isPro);
			writer.writeValue<EItemType>("Type", this.type);
			writer.writeValue<EItemRarity>("Rarity", this.rarity);
			writer.writeValue<byte>("Size_X", this.size_x);
			writer.writeValue<byte>("Size_Y", this.size_y);
			writer.writeValue<float>("Size_Z", this.size_z);
			writer.writeValue<float>("Size2_Z", this.size2_z);
			writer.writeValue<byte>("Amount", this.amount);
			writer.writeValue<byte>("Count_Min", this.countMin);
			writer.writeValue<byte>("Count_Max", this.countMax);
			writer.writeValue<byte>("Quality_Min", this.qualityMin);
			writer.writeValue<byte>("Quality_Max", this.qualityMax);
			writer.writeValue<bool>("Backward", this.isBackward);
			writer.writeValue("Useable", this.useable);
			writer.writeValue<ESlotType>("Slot", this.slot);
			writer.beginArray("Blueprints");
			for (int i = 0; i < this.blueprints.Count; i++)
			{
				writer.beginObject();
				Blueprint blueprint = this.blueprints[i];
				writer.writeValue<EBlueprintType>("Type", blueprint.type);
				writer.beginArray("Supplies");
				for (int j = 0; j < blueprint.supplies.Length; j++)
				{
					writer.beginObject();
					BlueprintSupply blueprintSupply = blueprint.supplies[j];
					writer.writeValue<ushort>("ID", blueprintSupply.id);
					writer.writeValue<bool>("Critical", blueprintSupply.isCritical);
					writer.writeValue<ushort>("Amount", blueprintSupply.amount);
					writer.endObject();
				}
				writer.endArray();
				writer.beginArray("Output");
				for (int k = 0; k < blueprint.outputs.Length; k++)
				{
					writer.beginObject();
					BlueprintOutput blueprintOutput = blueprint.outputs[k];
					writer.writeValue<ushort>("ID", blueprintOutput.id);
					writer.writeValue<ushort>("Amount", blueprintOutput.amount);
					writer.endObject();
				}
				writer.endArray();
				writer.writeValue<ushort>("Tool", blueprint.tool);
				writer.writeValue<bool>("Tool_Critical", blueprint.toolCritical);
				writer.writeValue<ushort>("Build", blueprint.build);
				writer.writeValue<byte>("Level", blueprint.level);
				writer.writeValue<EBlueprintSkill>("Skill", blueprint.skill);
				writer.writeValue<bool>("Transfer_State", blueprint.transferState);
				writer.endObject();
			}
			writer.endArray();
			writer.beginArray("Actions");
			byte b = 0;
			while ((int)b < this.actions.Count)
			{
				writer.beginObject();
				Action action = this.actions[(int)b];
				writer.writeValue<EActionType>("Type", action.type);
				writer.beginArray("Blueprints");
				byte b2 = 0;
				while ((int)b2 < action.blueprints.Length)
				{
					writer.beginObject();
					ActionBlueprint actionBlueprint = action.blueprints[(int)b2];
					writer.writeValue<byte>("Index", actionBlueprint.id);
					writer.writeValue<bool>("Is_Link", actionBlueprint.isLink);
					writer.endObject();
					b2 += 1;
				}
				writer.endArray();
				writer.writeValue("Text", action.text);
				writer.writeValue("Tooltip", action.tooltip);
				writer.writeValue("Key", action.key);
				writer.writeValue<ushort>("Source", action.source);
				writer.endObject();
				b += 1;
			}
			writer.endArray();
			writer.writeValue<bool>("Should_Verify_Hash", this._shouldVerifyHash);
		}

		protected bool _shouldVerifyHash;

		protected string _itemName;

		protected string _itemDescription;

		[Inspectable("#SDG::Asset.Item.Type.Name", null)]
		public EItemType type;

		[Inspectable("#SDG::Asset.Item.Rarity.Name", null)]
		public EItemRarity rarity;

		protected string _proPath;

		[Inspectable("#SDG::Asset.Item.Is_Pro.Name", null)]
		public bool isPro;

		[Inspectable("#SDG::Asset.Item.Useable.Name", null)]
		public string useable;

		[Inspectable("#SDG::Asset.Item.Is_Useable.Name", null)]
		public bool isUseable;

		[Inspectable("#SDG::Asset.Item.Slot.Name", null)]
		public ESlotType slot;

		[Inspectable("#SDG::Asset.Item.Size_X.Name", null)]
		public byte size_x;

		[Inspectable("#SDG::Asset.Item.Size_Y.Name", null)]
		public byte size_y;

		[Inspectable("#SDG::Asset.Item.Size_Z.Name", null)]
		public float size_z;

		[Inspectable("#SDG::Asset.Item.Size2_Z.Name", null)]
		public float size2_z;

		[Inspectable("#SDG::Asset.Item.Amount.Name", null)]
		public byte amount;

		[Inspectable("#SDG::Asset.Item.Count_Min.Name", null)]
		public byte countMin;

		[Inspectable("#SDG::Asset.Item.Count_Max.Name", null)]
		public byte countMax;

		[Inspectable("#SDG::Asset.Item.Quality_Min.Name", null)]
		public byte qualityMin;

		[Inspectable("#SDG::Asset.Item.Quality_Max.Name", null)]
		public byte qualityMax;

		[Inspectable("#SDG::Asset.Item.Is_Backward.Name", null)]
		public bool isBackward;

		protected GameObject _item;

		protected AudioClip _equip;

		protected AnimationClip[] _animations;

		protected List<Blueprint> _blueprints;

		protected List<Action> _actions;

		protected Texture2D _albedoBase;

		protected Texture2D _metallicBase;

		protected Texture2D _emissionBase;
	}
}
