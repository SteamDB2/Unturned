using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ReunObjectRemove : IReun
	{
		public ReunObjectRemove(int newStep, Transform newModel, ObjectAsset newObjectAsset, ItemAsset newItemAsset, Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
		{
			this.step = newStep;
			this.model = newModel;
			this.objectAsset = newObjectAsset;
			this.itemAsset = newItemAsset;
			this.position = newPosition;
			this.rotation = newRotation;
			this.scale = newScale;
		}

		public int step { get; private set; }

		public Transform redo()
		{
			if (this.model != null)
			{
				if (this.objectAsset != null)
				{
					LevelObjects.removeObject(this.model);
					this.model = null;
				}
				else if (this.itemAsset != null)
				{
					LevelObjects.removeBuildable(this.model);
					this.model = null;
				}
			}
			return null;
		}

		public void undo()
		{
			if (this.model == null)
			{
				if (this.objectAsset != null)
				{
					this.model = LevelObjects.addObject(this.position, this.rotation, this.scale, this.objectAsset.id, this.objectAsset.name, this.objectAsset.GUID, ELevelObjectPlacementOrigin.MANUAL);
				}
				else if (this.itemAsset != null)
				{
					this.model = LevelObjects.addBuildable(this.position, this.rotation, this.itemAsset.id);
				}
			}
		}

		private Transform model;

		private ObjectAsset objectAsset;

		private ItemAsset itemAsset;

		private Vector3 position;

		private Quaternion rotation;

		private Vector3 scale;
	}
}
