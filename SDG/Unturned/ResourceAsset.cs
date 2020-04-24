using System;
using System.Collections.Generic;
using SDG.Framework.Debug;
using UnityEngine;

namespace SDG.Unturned
{
	public class ResourceAsset : Asset
	{
		public ResourceAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (id < 50 && !bundle.hasResource && !data.has("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 50");
			}
			if (Dedicator.isDedicated || GraphicsSettings.treeMode == ETreeGraphicMode.LEGACY)
			{
				this.isSpeedTree = false;
			}
			else
			{
				this.isSpeedTree = data.has("SpeedTree");
			}
			this.defaultLODWeights = data.has("SpeedTree_Default_LOD_Weights");
			this._resourceName = localization.format("Name");
			if (Dedicator.isDedicated)
			{
				this._modelGameObject = (GameObject)bundle.load("Resource_Clip");
				if (this.modelGameObject == null)
				{
					Assets.errors.Add(this.resourceName + " is missing collision data. Highly recommended to fix.");
				}
				this._stumpGameObject = (GameObject)bundle.load("Stump_Clip");
				if (this.stumpGameObject == null)
				{
					Assets.errors.Add(this.resourceName + " is missing collision data. Highly recommended to fix.");
				}
			}
			else
			{
				this._modelGameObject = null;
				this._stumpGameObject = null;
				this._skyboxGameObject = null;
				this._debrisGameObject = null;
				if (GraphicsSettings.treeMode == ETreeGraphicMode.LEGACY)
				{
					this._modelGameObject = (GameObject)bundle.load("Resource_Old");
				}
				if (this._modelGameObject == null)
				{
					this._modelGameObject = (GameObject)bundle.load("Resource");
				}
				if (this.defaultLODWeights)
				{
					Transform transform = this.modelGameObject.transform.FindChild("Billboard");
					if (transform != null)
					{
						BillboardRenderer component = transform.GetComponent<BillboardRenderer>();
						if (component != null)
						{
							component.shadowCastingMode = 0;
						}
					}
				}
				if (GraphicsSettings.treeMode == ETreeGraphicMode.LEGACY)
				{
					this._stumpGameObject = (GameObject)bundle.load("Stump_Old");
				}
				if (this._stumpGameObject == null)
				{
					this._stumpGameObject = (GameObject)bundle.load("Stump");
				}
				if (GraphicsSettings.treeMode == ETreeGraphicMode.LEGACY)
				{
					this._skyboxGameObject = (GameObject)bundle.load("Skybox_Old");
				}
				if (this._skyboxGameObject == null)
				{
					this._skyboxGameObject = (GameObject)bundle.load("Skybox");
				}
				if (this.defaultLODWeights)
				{
					Transform transform2 = this.skyboxGameObject.transform.FindChild("Model_0");
					if (transform2 != null)
					{
						BillboardRenderer component2 = transform2.GetComponent<BillboardRenderer>();
						if (component2 != null)
						{
							component2.shadowCastingMode = 0;
						}
					}
				}
				if (this.isSpeedTree)
				{
					this._debrisGameObject = (GameObject)bundle.load("Debris");
					if (this.modelGameObject != null)
					{
						LODGroup component3 = this.modelGameObject.GetComponent<LODGroup>();
						if (component3 != null)
						{
							if (GraphicsSettings.treeMode == ETreeGraphicMode.SPEEDTREE_FADE_SPEEDTREE)
							{
								component3.fadeMode = 2;
								if (this.defaultLODWeights && GraphicsSettings.treeMode != ETreeGraphicMode.LEGACY)
								{
									this.applyDefaultLODs(component3, true);
								}
							}
							else
							{
								component3.fadeMode = 0;
								if (this.defaultLODWeights && GraphicsSettings.treeMode != ETreeGraphicMode.LEGACY)
								{
									this.applyDefaultLODs(component3, false);
								}
							}
						}
					}
					if (this.debrisGameObject != null)
					{
						LODGroup component4 = this.debrisGameObject.GetComponent<LODGroup>();
						if (component4 != null)
						{
							if (GraphicsSettings.treeMode == ETreeGraphicMode.SPEEDTREE_FADE_SPEEDTREE)
							{
								component4.fadeMode = 2;
								if (this.defaultLODWeights && GraphicsSettings.treeMode != ETreeGraphicMode.LEGACY)
								{
									this.applyDefaultLODs(component4, true);
								}
							}
							else
							{
								component4.fadeMode = 0;
								if (this.defaultLODWeights && GraphicsSettings.treeMode != ETreeGraphicMode.LEGACY)
								{
									this.applyDefaultLODs(component4, false);
								}
							}
						}
					}
				}
				if (data.has("Auto_Skybox") && !this.isSpeedTree && this.skyboxGameObject)
				{
					Transform transform3 = this.modelGameObject.transform.FindChild("Model_0");
					if (transform3)
					{
						ResourceAsset.meshes.Clear();
						transform3.GetComponentsInChildren<MeshFilter>(true, ResourceAsset.meshes);
						if (ResourceAsset.meshes.Count > 0)
						{
							Bounds bounds = default(Bounds);
							for (int i = 0; i < ResourceAsset.meshes.Count; i++)
							{
								Mesh sharedMesh = ResourceAsset.meshes[i].sharedMesh;
								if (!(sharedMesh == null))
								{
									Bounds bounds2 = sharedMesh.bounds;
									bounds.Encapsulate(bounds2.min);
									bounds.Encapsulate(bounds2.max);
								}
							}
							if (bounds.min.y < 0f)
							{
								float num = Mathf.Abs(bounds.min.z);
								bounds.center += new Vector3(0f, 0f, num / 2f);
								bounds.size -= new Vector3(0f, 0f, num);
							}
							float num2 = Mathf.Max(bounds.size.x, bounds.size.y);
							float z = bounds.size.z;
							this.skyboxGameObject.transform.localScale = new Vector3(z, z, z);
							Transform transform4 = Object.Instantiate<GameObject>(this.modelGameObject).transform;
							Transform transform5 = new GameObject().transform;
							transform5.parent = transform4;
							transform5.localPosition = new Vector3(0f, z / 2f, -num2 / 2f);
							transform5.localRotation = Quaternion.identity;
							Transform transform6 = new GameObject().transform;
							transform6.parent = transform4;
							transform6.localPosition = new Vector3(-num2 / 2f, z / 2f, 0f);
							transform6.localRotation = Quaternion.Euler(0f, 90f, 0f);
							if (!ResourceAsset.shader)
							{
								ResourceAsset.shader = Shader.Find("Custom/Card");
							}
							Texture2D card = ItemTool.getCard(transform4, transform5, transform6, 64, 64, z / 2f, num2);
							this.skyboxMaterial = new Material(ResourceAsset.shader);
							this.skyboxMaterial.mainTexture = card;
						}
					}
				}
			}
			this.health = data.readUInt16("Health");
			this.radius = data.readSingle("Radius");
			this.scale = data.readSingle("Scale");
			this.explosion = data.readUInt16("Explosion");
			this.log = data.readUInt16("Log");
			this.stick = data.readUInt16("Stick");
			this.rewardID = data.readUInt16("Reward_ID");
			this.rewardXP = data.readUInt32("Reward_XP");
			if (data.has("Reward_Min"))
			{
				this.rewardMin = data.readByte("Reward_Min");
			}
			else
			{
				this.rewardMin = 6;
			}
			if (data.has("Reward_Max"))
			{
				this.rewardMax = data.readByte("Reward_Max");
			}
			else
			{
				this.rewardMax = 9;
			}
			this.bladeID = data.readByte("BladeID");
			this.reset = data.readSingle("Reset");
			this.isForage = data.has("Forage");
			this.hasDebris = !data.has("No_Debris");
			bundle.unload();
		}

		public string resourceName
		{
			get
			{
				return this._resourceName;
			}
		}

		public GameObject modelGameObject
		{
			get
			{
				return this._modelGameObject;
			}
		}

		public GameObject stumpGameObject
		{
			get
			{
				return this._stumpGameObject;
			}
		}

		public GameObject skyboxGameObject
		{
			get
			{
				return this._skyboxGameObject;
			}
		}

		public GameObject debrisGameObject
		{
			get
			{
				return this._debrisGameObject;
			}
		}

		public Material skyboxMaterial { get; private set; }

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.RESOURCE;
			}
		}

		protected void applyDefaultLODs(LODGroup lod, bool fade)
		{
			LOD[] lods = lod.GetLODs();
			lods[0].screenRelativeTransitionHeight = ((!fade) ? 0.6f : 0.7f);
			lods[1].screenRelativeTransitionHeight = ((!fade) ? 0.4f : 0.5f);
			lods[2].screenRelativeTransitionHeight = 0.15f;
			lods[3].screenRelativeTransitionHeight = 0.03f;
			lod.SetLODs(lods);
		}

		private static List<MeshFilter> meshes = new List<MeshFilter>();

		private static Shader shader;

		protected string _resourceName;

		protected GameObject _modelGameObject;

		protected GameObject _stumpGameObject;

		protected GameObject _skyboxGameObject;

		protected GameObject _debrisGameObject;

		[Inspectable("#SDG::Asset.Resource.Health.Name", null)]
		public ushort health;

		[Inspectable("#SDG::Asset.Resource.Reward_XP.Name", null)]
		public uint rewardXP;

		[Inspectable("#SDG::Asset.Resource.Radius.Name", null)]
		public float radius;

		[Inspectable("#SDG::Asset.Resource.Scale.Name", null)]
		public float scale;

		[Inspectable("#SDG::Asset.Resource.Explosion.Name", null)]
		public ushort explosion;

		[Inspectable("#SDG::Asset.Resource.Log.Name", null)]
		public ushort log;

		[Inspectable("#SDG::Asset.Resource.Stick.Name", null)]
		public ushort stick;

		[Inspectable("#SDG::Asset.Resource.Reward_Min.Name", null)]
		public byte rewardMin;

		[Inspectable("#SDG::Asset.Resource.Reward_Max.Name", null)]
		public byte rewardMax;

		[Inspectable("#SDG::Asset.Resource.Reward_ID.Name", null)]
		public ushort rewardID;

		[Inspectable("#SDG::Asset.Resource.Is_Forage.Name", null)]
		public bool isForage;

		[Inspectable("#SDG::Asset.Resource.Has_Debris.Name", null)]
		public bool hasDebris;

		[Inspectable("#SDG::Asset.Resource.Blade_ID.Name", null)]
		public byte bladeID;

		[Inspectable("#SDG::Asset.Resource.Reset.Name", null)]
		public float reset;

		[Inspectable("#SDG::Asset.Resource.Is_SpeedTree.Name", null)]
		public bool isSpeedTree;

		[Inspectable("#SDG::Asset.Resource.Default_LOD_Weights.Name", null)]
		public bool defaultLODWeights;
	}
}
