using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelBuildableObject
	{
		public LevelBuildableObject(Vector3 newPoint, Quaternion newRotation, ushort newID)
		{
			this.point = newPoint;
			this.rotation = newRotation;
			this._id = newID;
			this._asset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
			if (this.asset == null || this.asset.id != this.id)
			{
				this._asset = (ItemAsset)Assets.find(EAssetType.ITEM, this.id);
				if (this.asset == null)
				{
					return;
				}
			}
			if (Level.isEditor)
			{
				ItemBarricadeAsset itemBarricadeAsset = this.asset as ItemBarricadeAsset;
				ItemStructureAsset itemStructureAsset = this.asset as ItemStructureAsset;
				if (itemBarricadeAsset != null)
				{
					this._transform = Object.Instantiate<GameObject>(itemBarricadeAsset.barricade).transform;
				}
				else if (itemStructureAsset != null)
				{
					this._transform = Object.Instantiate<GameObject>(itemStructureAsset.structure).transform;
				}
				if (this.transform != null)
				{
					this.transform.name = this.id.ToString();
					this.transform.parent = LevelObjects.models;
					this.transform.position = newPoint;
					this.transform.rotation = newRotation;
					Rigidbody rigidbody = this.transform.GetComponent<Rigidbody>();
					if (rigidbody == null)
					{
						rigidbody = this.transform.gameObject.AddComponent<Rigidbody>();
						rigidbody.useGravity = false;
						rigidbody.isKinematic = true;
					}
					this.transform.gameObject.SetActive(false);
					LevelBuildableObject.colliders.Clear();
					this.transform.GetComponentsInChildren<Collider>(true, LevelBuildableObject.colliders);
					for (int i = 0; i < LevelBuildableObject.colliders.Count; i++)
					{
						if (LevelBuildableObject.colliders[i].gameObject.layer != LayerMasks.BARRICADE && LevelBuildableObject.colliders[i].gameObject.layer != LayerMasks.STRUCTURE)
						{
							Object.Destroy(LevelBuildableObject.colliders[i].gameObject);
						}
					}
				}
			}
		}

		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		public ItemAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		public bool isEnabled { get; private set; }

		public void enable()
		{
			this.isEnabled = true;
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(true);
			}
		}

		public void disable()
		{
			this.isEnabled = false;
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(false);
			}
		}

		public void destroy()
		{
			if (this.transform != null)
			{
				Object.Destroy(this.transform.gameObject);
			}
		}

		private static List<Collider> colliders = new List<Collider>();

		public Vector3 point;

		public Quaternion rotation;

		private Transform _transform;

		private ushort _id;

		private ItemAsset _asset;
	}
}
