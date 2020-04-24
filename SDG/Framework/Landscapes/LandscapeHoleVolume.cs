using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Visibility;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public class LandscapeHoleVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		public void beginCollision(Collider collider)
		{
			if (collider == null || this.terrainColliders == null)
			{
				return;
			}
			ILandscaleHoleVolumeInteractionHandler component = collider.gameObject.GetComponent<ILandscaleHoleVolumeInteractionHandler>();
			if (component != null)
			{
				component.landscapeHoleBeginCollision(this, this.terrainColliders);
			}
			if (component == null || component.landscapeHoleAutoIgnoreTerrainCollision)
			{
				foreach (TerrainCollider terrainCollider in this.terrainColliders)
				{
					Physics.IgnoreCollision(collider, terrainCollider, true);
				}
			}
		}

		public void endCollision(Collider collider)
		{
			if (collider == null || this.terrainColliders == null)
			{
				return;
			}
			ILandscaleHoleVolumeInteractionHandler component = collider.gameObject.GetComponent<ILandscaleHoleVolumeInteractionHandler>();
			if (component != null)
			{
				component.landscapeHoleEndCollision(this, this.terrainColliders);
			}
			if (component == null || component.landscapeHoleAutoIgnoreTerrainCollision)
			{
				foreach (TerrainCollider terrainCollider in this.terrainColliders)
				{
					Physics.IgnoreCollision(collider, terrainCollider, false);
				}
			}
		}

		public void devkitHierarchySpawn()
		{
		}

		public void OnTriggerEnter(Collider other)
		{
			this.beginCollision(other);
		}

		public void OnTriggerExit(Collider other)
		{
			this.endCollision(other);
		}

		protected void findTerrainColliders()
		{
			this.terrainColliders.Clear();
			Bounds bounds = base.box.bounds;
			LandscapeBounds landscapeBounds = new LandscapeBounds(bounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord coord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(coord);
					if (tile != null)
					{
						this.terrainColliders.Add(tile.collider);
					}
				}
			}
			if (LevelGround.terrain != null)
			{
				this.terrainColliders.Add(LevelGround.terrain.transform.GetComponent<TerrainCollider>());
			}
		}

		protected void handleLandscapeLoaded()
		{
			this.findTerrainColliders();
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (!Level.isEditor || LandscapeHoleSystem.holeVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			LandscapeHoleSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			LandscapeHoleSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Landscape_Hole_Volume";
			base.gameObject.layer = LayerMasks.TRAP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			base.box.isTrigger = true;
			this.updateBoxEnabled();
			this.terrainColliders = new List<TerrainCollider>();
			this.findTerrainColliders();
			Landscape.loaded += this.handleLandscapeLoaded;
			LandscapeHoleSystem.holeVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			Landscape.loaded -= this.handleLandscapeLoaded;
			LandscapeHoleSystem.holeVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		protected List<TerrainCollider> terrainColliders;
	}
}
