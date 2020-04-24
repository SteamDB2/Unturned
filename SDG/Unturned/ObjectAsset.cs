using System;
using System.Text;
using SDG.Framework.Debug;
using SDG.Framework.Foliage;
using UnityEngine;

namespace SDG.Unturned
{
	public class ObjectAsset : Asset
	{
		public ObjectAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 2000 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this._objectName = localization.format("Name");
			this.type = (EObjectType)Enum.Parse(typeof(EObjectType), data.readString("Type"), true);
			if (this.type == EObjectType.NPC)
			{
				if (Dedicator.isDedicated)
				{
					this._modelGameObject = (GameObject)Resources.Load("Characters/NPC_Server");
				}
				else
				{
					this._modelGameObject = (GameObject)Resources.Load("Characters/NPC_Client");
				}
				this.useScale = true;
				this.interactability = EObjectInteractability.NPC;
			}
			else if (this.type == EObjectType.DECAL)
			{
				float num = data.readSingle("Decal_X");
				float num2 = data.readSingle("Decal_Y");
				float num3 = 1f;
				if (data.has("Decal_LOD_Bias"))
				{
					num3 = data.readSingle("Decal_LOD_Bias");
				}
				Texture2D texture2D = (Texture2D)bundle.load("Decal");
				this._modelGameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Materials/Decal_Template"));
				this._modelGameObject.hideFlags = 61;
				Object.DontDestroyOnLoad(this._modelGameObject);
				BoxCollider component = this.modelGameObject.GetComponent<BoxCollider>();
				component.size = new Vector3(num2, num, 1f);
				Decal component2 = this.modelGameObject.transform.FindChild("Decal").GetComponent<Decal>();
				Material material = Object.Instantiate<Material>(component2.material);
				material.name = "Decal_Deferred";
				material.hideFlags = 52;
				material.SetTexture("_MainTex", texture2D);
				component2.material = material;
				component2.lodBias = num3;
				component2.transform.localScale = new Vector3(num, num2, 1f);
				MeshRenderer component3 = this.modelGameObject.transform.FindChild("Mesh").GetComponent<MeshRenderer>();
				Material material2 = Object.Instantiate<Material>(component3.sharedMaterial);
				material2.name = "Decal_Forward";
				material2.hideFlags = 52;
				material2.SetTexture("_MainTex", texture2D);
				component3.sharedMaterial = material2;
				component3.transform.localScale = new Vector3(num2, num, 1f);
				this.useScale = true;
			}
			else
			{
				if (Dedicator.isDedicated)
				{
					this._modelGameObject = (GameObject)bundle.load("Clip");
					if (this.modelGameObject == null && this.type != EObjectType.SMALL)
					{
						Debug.LogError(this.objectName + " is missing collision data. Highly recommended to fix.");
					}
				}
				else
				{
					this._modelGameObject = (GameObject)bundle.load("Object");
					if (this.modelGameObject == null)
					{
						throw new NotSupportedException("Missing object gameobject");
					}
					this._skyboxGameObject = (GameObject)bundle.load("Skybox");
				}
				if (this.modelGameObject != null)
				{
					if (Mathf.Abs(this.modelGameObject.transform.localScale.x - 1f) > 0.01f || Mathf.Abs(this.modelGameObject.transform.localScale.y - 1f) > 0.01f || Mathf.Abs(this.modelGameObject.transform.localScale.z - 1f) > 0.01f)
					{
						this.useScale = false;
						Assets.errors.Add(this.objectName + " should have a scale of one.");
					}
					else
					{
						this.useScale = true;
					}
					Transform transform = this.modelGameObject.transform.FindChild("Block");
					if (transform != null && transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().sharedMaterial == null)
					{
						Assets.errors.Add(this.objectName + " has a clip object but no physics material.");
					}
					Transform transform2 = this.modelGameObject.transform.FindChild("Model_0");
					if (this.type == EObjectType.SMALL)
					{
						if (!this.modelGameObject.CompareTag("Small"))
						{
							Assets.errors.Add(this.objectName + " is set up as small, but is not tagged as small.");
						}
						if (this.modelGameObject.layer != LayerMasks.SMALL)
						{
							Assets.errors.Add(this.objectName + " is set up as small, but is not layered as small.");
						}
						if (transform2 != null)
						{
							if (!transform2.CompareTag("Small"))
							{
								Assets.errors.Add(this.objectName + " is set up as small, but is not tagged as small.");
							}
							if (transform2.gameObject.layer != LayerMasks.SMALL)
							{
								Assets.errors.Add(this.objectName + " is set up as small, but is not layered as small.");
							}
						}
					}
					else if (this.type == EObjectType.MEDIUM)
					{
						if (!this.modelGameObject.CompareTag("Medium"))
						{
							Assets.errors.Add(this.objectName + " is set up as medium, but is not tagged as medium.");
						}
						if (this.modelGameObject.layer != LayerMasks.MEDIUM)
						{
							Assets.errors.Add(this.objectName + " is set up as medium, but is not layered as medium.");
						}
						if (transform2 != null)
						{
							if (!transform2.CompareTag("Medium"))
							{
								Assets.errors.Add(this.objectName + " is set up as medium, but is not tagged as medium.");
							}
							if (transform2.gameObject.layer != LayerMasks.MEDIUM)
							{
								Assets.errors.Add(this.objectName + " is set up as medium, but is not layered as medium.");
							}
						}
					}
					else if (this.type == EObjectType.LARGE)
					{
						if (!this.modelGameObject.CompareTag("Large"))
						{
							Assets.errors.Add(this.objectName + " is set up as large, but is not tagged as large.");
						}
						if (this.modelGameObject.layer != LayerMasks.LARGE)
						{
							Assets.errors.Add(this.objectName + " is set up as large, but is not layered as large.");
						}
						if (transform2 != null)
						{
							if (!transform2.CompareTag("Large"))
							{
								Assets.errors.Add(this.objectName + " is set up as large, but is not tagged as large.");
							}
							if (transform2.gameObject.layer != LayerMasks.LARGE)
							{
								Assets.errors.Add(this.objectName + " is set up as large, but is not layered as large.");
							}
						}
					}
				}
				this._navGameObject = (GameObject)bundle.load("Nav");
				if (this.navGameObject == null && this.type == EObjectType.LARGE)
				{
					Assets.errors.Add(this.objectName + " is missing navigation data. Highly recommended to fix.");
				}
				if (this.navGameObject != null)
				{
					if (!this.navGameObject.CompareTag("Navmesh"))
					{
						Assets.errors.Add(this.objectName + " is set up as navmesh, but is not tagged as navmesh.");
					}
					if (this.navGameObject.layer != LayerMasks.NAVMESH)
					{
						Assets.errors.Add(this.objectName + " is set up as navmesh, but is not layered as navmesh.");
					}
				}
				this._slotsGameObject = (GameObject)bundle.load("Slots");
				this._triggersGameObject = (GameObject)bundle.load("Triggers");
				this.isSnowshoe = data.has("Snowshoe");
				if (data.has("Chart"))
				{
					this.chart = (EObjectChart)Enum.Parse(typeof(EObjectChart), data.readString("Chart"), true);
				}
				else
				{
					this.chart = EObjectChart.NONE;
				}
				this.isFuel = data.has("Fuel");
				this.isRefill = data.has("Refill");
				this.isSoft = data.has("Soft");
				this.isCollisionImportant = data.has("Collision_Important");
				if (this.isFuel || this.isRefill)
				{
					Assets.errors.Add(this.objectName + " is using the legacy fuel/water system.");
				}
				if (data.has("LOD"))
				{
					this.lod = (EObjectLOD)Enum.Parse(typeof(EObjectLOD), data.readString("LOD"), true);
					this.lodBias = data.readSingle("LOD_Bias");
					if (this.lodBias < 0.01f)
					{
						this.lodBias = 1f;
					}
					this.lodCenter = data.readVector3("LOD_Center");
					this.lodSize = data.readVector3("LOD_Size");
				}
				if (data.has("Interactability"))
				{
					this.interactability = (EObjectInteractability)Enum.Parse(typeof(EObjectInteractability), data.readString("Interactability"), true);
					this.interactabilityRemote = data.has("Interactability_Remote");
					this.interactabilityDelay = data.readSingle("Interactability_Delay");
					this.interactabilityReset = data.readSingle("Interactability_Reset");
					if (data.has("Interactability_Hint"))
					{
						this.interactabilityHint = (EObjectInteractabilityHint)Enum.Parse(typeof(EObjectInteractabilityHint), data.readString("Interactability_Hint"), true);
					}
					this.interactabilityEmission = data.has("Interactability_Emission");
					if (this.interactability == EObjectInteractability.NOTE)
					{
						ushort num4 = data.readUInt16("Interactability_Text_Lines");
						StringBuilder stringBuilder = new StringBuilder();
						for (ushort num5 = 0; num5 < num4; num5 += 1)
						{
							string value = localization.format("Interactability_Text_Line_" + num5);
							stringBuilder.AppendLine(value);
						}
						this.interactabilityText = stringBuilder.ToString();
					}
					else
					{
						this.interactabilityText = localization.read("Interact");
					}
					if (data.has("Interactability_Power"))
					{
						this.interactabilityPower = (EObjectInteractabilityPower)Enum.Parse(typeof(EObjectInteractabilityPower), data.readString("Interactability_Power"), true);
					}
					else
					{
						this.interactabilityPower = EObjectInteractabilityPower.NONE;
					}
					if (data.has("Interactability_Editor"))
					{
						this.interactabilityEditor = (EObjectInteractabilityEditor)Enum.Parse(typeof(EObjectInteractabilityEditor), data.readString("Interactability_Editor"), true);
					}
					else
					{
						this.interactabilityEditor = EObjectInteractabilityEditor.NONE;
					}
					if (data.has("Interactability_Nav"))
					{
						this.interactabilityNav = (EObjectInteractabilityNav)Enum.Parse(typeof(EObjectInteractabilityNav), data.readString("Interactability_Nav"), true);
					}
					else
					{
						this.interactabilityNav = EObjectInteractabilityNav.NONE;
					}
					this.interactabilityDrops = new ushort[(int)data.readByte("Interactability_Drops")];
					byte b = 0;
					while ((int)b < this.interactabilityDrops.Length)
					{
						this.interactabilityDrops[(int)b] = data.readUInt16("Interactability_Drop_" + b);
						b += 1;
					}
					this.interactabilityRewardID = data.readUInt16("Interactability_Reward_ID");
					this.interactabilityEffect = data.readUInt16("Interactability_Effect");
					this.interactabilityConditions = new INPCCondition[(int)data.readByte("Interactability_Conditions")];
					NPCTool.readConditions(data, localization, "Interactability_Condition_", this.interactabilityConditions);
					this.interactabilityRewards = new INPCReward[(int)data.readByte("Interactability_Rewards")];
					NPCTool.readRewards(data, localization, "Interactability_Reward_", this.interactabilityRewards);
					this.interactabilityResource = data.readUInt16("Interactability_Resource");
					this.interactabilityResourceState = BitConverter.GetBytes(this.interactabilityResource);
				}
				else
				{
					this.interactability = EObjectInteractability.NONE;
					this.interactabilityPower = EObjectInteractabilityPower.NONE;
					this.interactabilityEditor = EObjectInteractabilityEditor.NONE;
				}
				if (this.interactability == EObjectInteractability.RUBBLE)
				{
					this.rubble = EObjectRubble.DESTROY;
					this.rubbleReset = data.readSingle("Interactability_Reset");
					this.rubbleHealth = data.readUInt16("Interactability_Health");
					this.rubbleEffect = data.readUInt16("Interactability_Effect");
					this.rubbleFinale = data.readUInt16("Interactability_Finale");
					this.rubbleRewardID = data.readUInt16("Interactability_Reward_ID");
					this.rubbleRewardXP = data.readUInt32("Interactability_Reward_XP");
					this.rubbleIsVulnerable = !data.has("Interactability_Invulnerable");
					this.rubbleProofExplosion = data.has("Interactability_Proof_Explosion");
				}
				else if (data.has("Rubble"))
				{
					this.rubble = (EObjectRubble)Enum.Parse(typeof(EObjectRubble), data.readString("Rubble"), true);
					this.rubbleReset = data.readSingle("Rubble_Reset");
					this.rubbleHealth = data.readUInt16("Rubble_Health");
					this.rubbleEffect = data.readUInt16("Rubble_Effect");
					this.rubbleFinale = data.readUInt16("Rubble_Finale");
					this.rubbleRewardID = data.readUInt16("Rubble_Reward_ID");
					this.rubbleRewardXP = data.readUInt32("Rubble_Reward_XP");
					this.rubbleIsVulnerable = !data.has("Rubble_Invulnerable");
					this.rubbleProofExplosion = data.has("Rubble_Proof_Explosion");
					if (data.has("Rubble_Editor"))
					{
						this.rubbleEditor = (EObjectRubbleEditor)Enum.Parse(typeof(EObjectRubbleEditor), data.readString("Rubble_Editor"), true);
					}
					else
					{
						this.rubbleEditor = EObjectRubbleEditor.ALIVE;
					}
				}
				if (data.has("Foliage"))
				{
					this.foliage = new AssetReference<FoliageInfoCollectionAsset>(new Guid(data.readString("Foliage")));
				}
				this.useWaterHeightTransparentSort = data.has("Use_Water_Height_Transparent_Sort");
				if (data.has("Material_Palette"))
				{
					this.materialPalette = new AssetReference<MaterialPaletteAsset>(data.readGUID("Material_Palette"));
				}
				if (data.has("Landmark_Quality"))
				{
					this.landmarkQuality = (EGraphicQuality)Enum.Parse(typeof(EGraphicQuality), data.readString("Landmark_Quality"), true);
				}
				else
				{
					this.landmarkQuality = EGraphicQuality.LOW;
				}
			}
			this.conditions = new INPCCondition[(int)data.readByte("Conditions")];
			NPCTool.readConditions(data, localization, "Condition_", this.conditions);
			bundle.unload();
		}

		public string objectName
		{
			get
			{
				return this._objectName;
			}
		}

		public GameObject modelGameObject
		{
			get
			{
				return this._modelGameObject;
			}
		}

		public GameObject skyboxGameObject
		{
			get
			{
				return this._skyboxGameObject;
			}
		}

		public GameObject navGameObject
		{
			get
			{
				return this._navGameObject;
			}
		}

		public GameObject slotsGameObject
		{
			get
			{
				return this._slotsGameObject;
			}
		}

		public GameObject triggersGameObject
		{
			get
			{
				return this._triggersGameObject;
			}
		}

		public virtual byte[] getState()
		{
			byte[] array;
			if (this.interactability == EObjectInteractability.BINARY_STATE)
			{
				array = new byte[]
				{
					(!Level.isEditor || this.interactabilityEditor == EObjectInteractabilityEditor.NONE) ? 0 : 1
				};
			}
			else if (this.interactability == EObjectInteractability.WATER || this.interactability == EObjectInteractability.FUEL)
			{
				array = new byte[]
				{
					this.interactabilityResourceState[0],
					this.interactabilityResourceState[1]
				};
			}
			else
			{
				array = null;
			}
			if (this.rubble == EObjectRubble.DESTROY)
			{
				if (array != null)
				{
					byte[] array2 = new byte[array.Length + 1];
					Array.Copy(array, array2, array.Length);
					array = array2;
				}
				else
				{
					array = new byte[1];
				}
				array[array.Length - 1] = ((!Level.isEditor || this.rubbleEditor != EObjectRubbleEditor.DEAD) ? byte.MaxValue : 0);
			}
			return array;
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.OBJECT;
			}
		}

		public bool areConditionsMet(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					if (!this.conditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool areInteractabilityConditionsMet(Player player)
		{
			if (this.interactabilityConditions != null)
			{
				for (int i = 0; i < this.interactabilityConditions.Length; i++)
				{
					if (!this.interactabilityConditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		public void applyInteractabilityConditions(Player player, bool shouldSend)
		{
			if (this.interactabilityConditions != null)
			{
				for (int i = 0; i < this.interactabilityConditions.Length; i++)
				{
					this.interactabilityConditions[i].applyCondition(player, shouldSend);
				}
			}
		}

		public void grantInteractabilityRewards(Player player, bool shouldSend)
		{
			if (this.interactabilityRewards != null)
			{
				for (int i = 0; i < this.interactabilityRewards.Length; i++)
				{
					this.interactabilityRewards[i].grantReward(player, shouldSend);
				}
			}
		}

		protected string _objectName;

		[Inspectable("#SDG::Asset.Object.Type.Name", null)]
		public EObjectType type;

		protected GameObject _modelGameObject;

		protected GameObject _skyboxGameObject;

		private GameObject _navGameObject;

		private GameObject _slotsGameObject;

		private GameObject _triggersGameObject;

		[Inspectable("#SDG::Asset.Object.Is_Snowshoe.Name", null)]
		public bool isSnowshoe;

		[Inspectable("#SDG::Asset.Object.Chart.Name", null)]
		public EObjectChart chart;

		[Inspectable("#SDG::Asset.Object.Is_Fuel.Name", null)]
		public bool isFuel;

		[Inspectable("#SDG::Asset.Object.Is_Refill.Name", null)]
		public bool isRefill;

		[Inspectable("#SDG::Asset.Object.Is_Soft.Name", null)]
		public bool isSoft;

		[Inspectable("#SDG::Asset.Object.Is_Collision_Important.Name", null)]
		public bool isCollisionImportant;

		[Inspectable("#SDG::Asset.Object.Use_Scale.Name", null)]
		public bool useScale;

		[Inspectable("#SDG::Asset.Object.LOD.Name", null)]
		public EObjectLOD lod;

		[Inspectable("#SDG::Asset.Object.LOD_Bias.Name", null)]
		public float lodBias;

		[Inspectable("#SDG::Asset.Object.LOD_Center.Name", null)]
		public Vector3 lodCenter;

		[Inspectable("#SDG::Asset.Object.LOD_Size.Name", null)]
		public Vector3 lodSize;

		[Inspectable("#SDG::Asset.Object.Conditions.Name", null)]
		public INPCCondition[] conditions;

		[Inspectable("#SDG::Asset.Object.Interactability.Name", null)]
		public EObjectInteractability interactability;

		[Inspectable("#SDG::Asset.Object.Interactability_Remote.Name", null)]
		public bool interactabilityRemote;

		[Inspectable("#SDG::Asset.Object.Interactability_Delay.Name", null)]
		public float interactabilityDelay;

		[Inspectable("#SDG::Asset.Object.Interactability_Emission.Name", null)]
		public bool interactabilityEmission;

		[Inspectable("#SDG::Asset.Object.Interactability_Hint.Name", null)]
		public EObjectInteractabilityHint interactabilityHint;

		[Inspectable("#SDG::Asset.Object.Interactability_Text.Name", null)]
		public string interactabilityText;

		[Inspectable("#SDG::Asset.Object.Interactability_Power.Name", null)]
		public EObjectInteractabilityPower interactabilityPower;

		[Inspectable("#SDG::Asset.Object.Interactability_Editor.Name", null)]
		public EObjectInteractabilityEditor interactabilityEditor;

		[Inspectable("#SDG::Asset.Object.Interactability_Nav.Name", null)]
		public EObjectInteractabilityNav interactabilityNav;

		[Inspectable("#SDG::Asset.Object.Interactability_Reset.Name", null)]
		public float interactabilityReset;

		[Inspectable("#SDG::Asset.Object.Interactability_Resource.Name", null)]
		public ushort interactabilityResource;

		private byte[] interactabilityResourceState;

		[Inspectable("#SDG::Asset.Object.Interactability_Drops.Name", null)]
		public ushort[] interactabilityDrops;

		[Inspectable("#SDG::Asset.Object.Interactability_Reward_ID.Name", null)]
		public ushort interactabilityRewardID;

		[Inspectable("#SDG::Asset.Object.Interactability_Effect.Name", null)]
		public ushort interactabilityEffect;

		[Inspectable("#SDG::Asset.Object.Interactability_Conditions.Name", null)]
		public INPCCondition[] interactabilityConditions;

		[Inspectable("#SDG::Asset.Object.Interactability_Rewards.Name", null)]
		public INPCReward[] interactabilityRewards;

		[Inspectable("#SDG::Asset.Object.Rubble.Name", null)]
		public EObjectRubble rubble;

		[Inspectable("#SDG::Asset.Object.Rubble_Reset.Name", null)]
		public float rubbleReset;

		[Inspectable("#SDG::Asset.Object.Rubble_Health.Name", null)]
		public ushort rubbleHealth;

		[Inspectable("#SDG::Asset.Object.Rubble_Effect.Name", null)]
		public ushort rubbleEffect;

		[Inspectable("#SDG::Asset.Object.Rubble_Finale.Name", null)]
		public ushort rubbleFinale;

		[Inspectable("#SDG::Asset.Object.Rubble_Editor.Name", null)]
		public EObjectRubbleEditor rubbleEditor;

		[Inspectable("#SDG::Asset.Object.Rubble_Reward_ID.Name", null)]
		public ushort rubbleRewardID;

		[Inspectable("#SDG::Asset.Object.Rubble_Reward_XP.Name", null)]
		public uint rubbleRewardXP;

		[Inspectable("#SDG::Asset.Object.Rubble_Vulnerable.Name", null)]
		public bool rubbleIsVulnerable;

		[Inspectable("#SDG::Asset.Object.Rubble_Proof_Explosion.Name", null)]
		public bool rubbleProofExplosion;

		[Inspectable("#SDG::Asset.Object.Foliage.Name", null)]
		public AssetReference<FoliageInfoCollectionAsset> foliage;

		[Inspectable("#SDG::Asset.Object.Use_Water_Height_Transparent_Sort.Name", null)]
		public bool useWaterHeightTransparentSort;

		[Inspectable("#SDG::Asset.Object.Material_Palettte.Name", null)]
		public AssetReference<MaterialPaletteAsset> materialPalette;

		[Inspectable("#SDG::Asset.Object.Landmark_Quality.Name", null)]
		public EGraphicQuality landmarkQuality;
	}
}
