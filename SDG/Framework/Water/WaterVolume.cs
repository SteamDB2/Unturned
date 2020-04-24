using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Water
{
	public class WaterVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		public WaterVolume()
		{
			this._isSurfaceVisible = true;
			this.waterType = ERefillWaterType.SALTY;
		}

		public GameObject waterPlane { get; protected set; }

		public Material sea { get; protected set; }

		public PlanarReflection planarReflection { get; protected set; }

		[Inspectable("#SDG::Devkit.Water.Volume.Is_Surface_Visible", null)]
		public bool isSurfaceVisible
		{
			get
			{
				return this._isSurfaceVisible;
			}
			set
			{
				this._isSurfaceVisible = value;
				if (this.waterPlane != null)
				{
					this.waterPlane.SetActive(this.isSurfaceVisible);
				}
			}
		}

		[Inspectable("#SDG::Devkit.Water.Volume.Is_Reflection_Visible", null)]
		public bool isReflectionVisible
		{
			get
			{
				return this._isReflectionVisible;
			}
			set
			{
				this._isReflectionVisible = value;
				this.updatePlanarReflection();
			}
		}

		[Inspectable("#SDG::Devkit.Water.Volume.Is_Sea_Level", null)]
		public bool isSeaLevel
		{
			get
			{
				return this._isSeaLevel;
			}
			set
			{
				this._isSeaLevel = value;
				if (this.isSeaLevel)
				{
					WaterSystem.seaLevelVolume = this;
				}
			}
		}

		protected virtual void updatePlanarReflection()
		{
			if (this.planarReflection != null)
			{
				this.planarReflection.enabled = (this.isReflectionVisible && GraphicsSettings.waterQuality == EGraphicQuality.ULTRA);
			}
		}

		protected virtual void createWaterPlanes()
		{
			if (!Dedicator.isDedicated && this.waterPlane == null)
			{
				this.waterPlane = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Level/Water_Plane"));
				this.waterPlane.name = "Plane";
				this.waterPlane.transform.parent = base.transform;
				this.waterPlane.transform.localPosition = new Vector3(0f, 0.5f, 0f);
				this.waterPlane.transform.localRotation = Quaternion.identity;
				this.waterPlane.transform.localScale = new Vector3(1f, 1f, 1f);
				this.waterPlane.SetActive(this.isSurfaceVisible);
				this.planarReflection = this.waterPlane.GetComponent<PlanarReflection>();
				int num = Mathf.Max(1, Mathf.FloorToInt(base.transform.localScale.x / (float)WaterVolume.WATER_SURFACE_TILE_SIZE));
				int num2 = Mathf.Max(1, Mathf.FloorToInt(base.transform.localScale.z / (float)WaterVolume.WATER_SURFACE_TILE_SIZE));
				float num3 = 1f / (float)num;
				float num4 = 1f / (float)num2;
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Level/Water_Tile"));
						gameObject.name = string.Concat(new object[]
						{
							"Tile_",
							i,
							"_",
							j
						});
						gameObject.transform.parent = this.waterPlane.transform;
						gameObject.transform.localPosition = new Vector3(-0.5f + num3 / 2f + (float)i * num3, 0f, -0.5f + num4 / 2f + (float)j * num4);
						gameObject.transform.localRotation = Quaternion.identity;
						gameObject.transform.localScale = new Vector3(0.01f * num3, 0.01f, 0.01f * num4);
						if (this.sea == null)
						{
							this.sea = gameObject.GetComponent<Renderer>().material;
						}
						else
						{
							gameObject.GetComponent<Renderer>().material = this.sea;
						}
						gameObject.GetComponent<WaterTile>().reflection = this.planarReflection;
					}
				}
				this.planarReflection.sharedMaterial = this.sea;
				this.applyGraphicsSettings();
				LevelLighting.updateLighting();
			}
		}

		public void beginCollision(Collider collider)
		{
			if (collider == null)
			{
				return;
			}
			IWaterVolumeInteractionHandler component = collider.gameObject.GetComponent<IWaterVolumeInteractionHandler>();
			if (component != null)
			{
				component.waterBeginCollision(this);
			}
		}

		public void endCollision(Collider collider)
		{
			if (collider == null)
			{
				return;
			}
			IWaterVolumeInteractionHandler component = collider.gameObject.GetComponent<IWaterVolumeInteractionHandler>();
			if (component != null)
			{
				component.waterEndCollision(this);
			}
		}

		public void devkitHierarchySpawn()
		{
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.isSurfaceVisible = reader.readValue<bool>("Is_Surface_Visible");
			this.isReflectionVisible = reader.readValue<bool>("Is_Reflection_Visible");
			this.isSeaLevel = reader.readValue<bool>("Is_Sea_Level");
			this.waterType = reader.readValue<ERefillWaterType>("Water_Type");
			this.createWaterPlanes();
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Is_Surface_Visible", this.isSurfaceVisible);
			writer.writeValue<bool>("Is_Reflection_Visible", this.isReflectionVisible);
			writer.writeValue<bool>("Is_Sea_Level", this.isSeaLevel);
			writer.writeValue<ERefillWaterType>("Water_Type", this.waterType);
		}

		public void OnTriggerEnter(Collider other)
		{
			this.beginCollision(other);
		}

		public void OnTriggerExit(Collider other)
		{
			this.endCollision(other);
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (!Level.isEditor || WaterSystem.waterVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected virtual void applyGraphicsSettings()
		{
			this.updatePlanarReflection();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			WaterSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			WaterSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Water_Volume";
			base.gameObject.layer = LayerMasks.TRAP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			base.box.isTrigger = true;
			this.updateBoxEnabled();
			WaterSystem.waterVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
			if (!Dedicator.isDedicated)
			{
				GraphicsSettings.graphicsSettingsApplied += this.applyGraphicsSettings;
			}
		}

		protected void Start()
		{
			this.createWaterPlanes();
		}

		protected void OnDestroy()
		{
			if (!Dedicator.isDedicated)
			{
				GraphicsSettings.graphicsSettingsApplied -= this.applyGraphicsSettings;
			}
			WaterSystem.waterVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		public static readonly int WATER_SURFACE_TILE_SIZE = 1024;

		[SerializeField]
		protected bool _isSurfaceVisible;

		[SerializeField]
		protected bool _isReflectionVisible;

		[SerializeField]
		protected bool _isSeaLevel;

		[Inspectable("#SDG::Devkit.Water.Volume.Water_Type", null)]
		public ERefillWaterType waterType;
	}
}
