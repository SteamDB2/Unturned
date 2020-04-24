using System;
using SDG.Framework.Devkit.Visibility;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class NavClipVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		public override VolumeVisibilityGroup visibilityGroupOverride
		{
			get
			{
				return PlayerClipVolumeSystem.navClipVisibilityGroup;
			}
		}

		public void devkitHierarchySpawn()
		{
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (Provider.isServer || (Level.isEditor && PlayerClipVolumeSystem.navClipVisibilityGroup.isVisible));
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			PlayerClipVolumeSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			PlayerClipVolumeSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Nav_Clip_Volume";
			base.gameObject.layer = LayerMasks.NAVMESH;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			this.updateBoxEnabled();
			PlayerClipVolumeSystem.navClipVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			PlayerClipVolumeSystem.navClipVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}
	}
}
