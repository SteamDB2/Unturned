﻿using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ReunObjectAdd : IReun
	{
		public ReunObjectAdd(int newStep, ObjectAsset newObjectAsset, ItemAsset newItemAsset, Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
		{
			this.step = newStep;
			this.model = null;
			this.objectAsset = newObjectAsset;
			this.itemAsset = newItemAsset;
			this.position = newPosition;
			this.rotation = newRotation;
			this.scale = newScale;
		}

		public int step { get; private set; }

		public Transform redo()
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
			return this.model;
		}

		public void undo()
		{
			if (this.model != null)
			{
				LevelObjects.removeObject(this.model);
				this.model = null;
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
