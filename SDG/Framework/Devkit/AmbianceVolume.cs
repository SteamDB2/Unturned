using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class AmbianceVolume : DevkitHierarchyVolume, IAmbianceNode, IDevkitHierarchySpawnable
	{
		public AmbianceVolume()
		{
			this.id = 0;
			this.noWater = false;
			this.noLighting = false;
			this.canRain = true;
			this.canSnow = true;
			this.overrideFog = false;
			this.fogColor = Color.white;
			this.fogHeight = -1024f;
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.ID", null)]
		public ushort id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.No_Water", null)]
		public bool noWater
		{
			get
			{
				return this._noWater;
			}
			set
			{
				this._noWater = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.No_Lighting", null)]
		public bool noLighting
		{
			get
			{
				return this._noLighting;
			}
			set
			{
				this._noLighting = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.Can_Rain", null)]
		public bool canRain
		{
			get
			{
				return this._canRain;
			}
			set
			{
				this._canRain = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.Can_Snow", null)]
		public bool canSnow
		{
			get
			{
				return this._canSnow;
			}
			set
			{
				this._canSnow = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.Override_Fog", null)]
		public bool overrideFog
		{
			get
			{
				return this._overrideFog;
			}
			set
			{
				this._overrideFog = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.Fog_Color", null)]
		public Color fogColor
		{
			get
			{
				return this._fogColor;
			}
			set
			{
				this._fogColor = value;
			}
		}

		[Inspectable("#SDG::Devkit.Volumes.Ambiance.Fog_Height", null)]
		public float fogHeight
		{
			get
			{
				return this._fogHeight;
			}
			set
			{
				this._fogHeight = value;
			}
		}

		public void devkitHierarchySpawn()
		{
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.id = reader.readValue<ushort>("Ambiance_ID");
			this.noWater = reader.readValue<bool>("No_Water");
			this.noLighting = reader.readValue<bool>("No_Lighting");
			if (reader.containsKey("Can_Rain"))
			{
				this.canRain = reader.readValue<bool>("Can_Rain");
			}
			if (reader.containsKey("Can_Snow"))
			{
				this.canSnow = reader.readValue<bool>("Can_Snow");
			}
			this.overrideFog = reader.readValue<bool>("Override_Fog");
			this.fogColor = reader.readValue<Color>("Fog_Color");
			this.fogHeight = reader.readValue<float>("Fog_Height");
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<ushort>("Ambiance_ID", this.id);
			writer.writeValue<bool>("No_Water", this.noWater);
			writer.writeValue<bool>("No_Lighting", this.noLighting);
			writer.writeValue<bool>("Can_Rain", this.canRain);
			writer.writeValue<bool>("Can_Snow", this.canSnow);
			writer.writeValue<bool>("Override_Fog", this.overrideFog);
			writer.writeValue<Color>("Fog_Color", this.fogColor);
			writer.writeValue<float>("Fog_Height", this.fogHeight);
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (Level.isEditor && AmbianceSystem.ambianceVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			AmbianceSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			AmbianceSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Ambiance_Volume";
			base.gameObject.layer = LayerMasks.TRAP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			base.box.isTrigger = true;
			this.updateBoxEnabled();
			AmbianceSystem.ambianceVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			AmbianceSystem.ambianceVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		[SerializeField]
		protected ushort _id;

		[SerializeField]
		protected bool _noWater;

		[SerializeField]
		protected bool _noLighting;

		[SerializeField]
		protected bool _canRain;

		[SerializeField]
		protected bool _canSnow;

		[SerializeField]
		protected bool _overrideFog;

		[SerializeField]
		protected Color _fogColor;

		[SerializeField]
		protected float _fogHeight;
	}
}
