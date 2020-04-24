using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		[Inspectable("#SDG::Devkit.Foliage.Volume.Mode", null)]
		public FoliageVolume.EFoliageVolumeMode mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (!base.enabled)
				{
					this._mode = value;
					return;
				}
				FoliageVolumeSystem.removeVolume(this);
				this._mode = value;
				FoliageVolumeSystem.addVolume(this);
			}
		}

		public void devkitHierarchySpawn()
		{
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.mode = reader.readValue<FoliageVolume.EFoliageVolumeMode>("Mode");
			if (reader.containsKey("Instanced_Meshes"))
			{
				this.instancedMeshes = reader.readValue<bool>("Instanced_Meshes");
			}
			if (reader.containsKey("Resources"))
			{
				this.resources = reader.readValue<bool>("Resources");
			}
			if (reader.containsKey("Objects"))
			{
				this.objects = reader.readValue<bool>("Objects");
			}
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<FoliageVolume.EFoliageVolumeMode>("Mode", this.mode);
			writer.writeValue<bool>("Instanced_Meshes", this.instancedMeshes);
			writer.writeValue<bool>("Resources", this.resources);
			writer.writeValue<bool>("Objects", this.objects);
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (Dedicator.isDedicated || FoliageVolumeSystem.foliageVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			FoliageVolumeSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			FoliageVolumeSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Foliage_Volume";
			base.gameObject.layer = LayerMasks.TRAP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			base.box.isTrigger = true;
			this.updateBoxEnabled();
			this._mode = FoliageVolume.EFoliageVolumeMode.SUBTRACTIVE;
			this.instancedMeshes = true;
			this.resources = true;
			this.objects = true;
			FoliageVolumeSystem.foliageVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			FoliageVolumeSystem.foliageVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		protected FoliageVolume.EFoliageVolumeMode _mode;

		[Inspectable("#SDG::Devkit.Foliage.Volume.Instanced_Meshes", null)]
		public bool instancedMeshes;

		[Inspectable("#SDG::Devkit.Foliage.Volume.Resources", null)]
		public bool resources;

		[Inspectable("#SDG::Devkit.Foliage.Volume.Objects", null)]
		public bool objects;

		public enum EFoliageVolumeMode
		{
			ADDITIVE,
			SUBTRACTIVE
		}
	}
}
