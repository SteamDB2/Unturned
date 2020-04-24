using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class Spawnpoint : DevkitHierarchyWorldItem, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler, IDevkitHierarchySpawnable
	{
		public SphereCollider sphere { get; protected set; }

		public virtual void beginSelection(InteractionData data)
		{
			this.isSelected = true;
		}

		public virtual void endSelection(InteractionData data)
		{
			this.isSelected = false;
		}

		public void devkitHierarchySpawn()
		{
		}

		protected virtual void updateSphereEnabled()
		{
			this.sphere.enabled = (Level.isEditor && SpawnpointSystem.spawnpointVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateSphereEnabled();
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.id = reader.readValue<string>("ID");
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue("ID", this.id);
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			SpawnpointSystem.addSpawnpoint(this);
		}

		protected void OnDisable()
		{
			SpawnpointSystem.removeSpawnpoint(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Spawnpoint";
			base.gameObject.layer = LayerMasks.TRAP;
			this.sphere = base.gameObject.getOrAddComponent<SphereCollider>();
			this.sphere.radius = 0.5f;
			this.updateSphereEnabled();
			SpawnpointSystem.spawnpointVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			SpawnpointSystem.spawnpointVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		[Inspectable("#SDG::Devkit.Spawns.Spawnpoint.ID.Name", null)]
		public string id;

		public bool isSelected;
	}
}
