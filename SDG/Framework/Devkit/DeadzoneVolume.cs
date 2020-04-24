using System;
using SDG.Framework.Devkit.Visibility;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DeadzoneVolume : DevkitHierarchyVolume, IDeadzoneNode, IDevkitHierarchySpawnable
	{
		public void devkitHierarchySpawn()
		{
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (Level.isEditor && DeadzoneSystem.deadzoneVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			DeadzoneSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			DeadzoneSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Deadzone_Volume";
			base.gameObject.layer = LayerMasks.TRAP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			base.box.isTrigger = true;
			this.updateBoxEnabled();
			DeadzoneSystem.deadzoneVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			DeadzoneSystem.deadzoneVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}
	}
}
