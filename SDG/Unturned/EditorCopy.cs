using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorCopy
	{
		public EditorCopy(Vector3 newPosition, Quaternion newRotation, Vector3 newScale, ObjectAsset newObjectAsset, ItemAsset newItemAsset)
		{
			this._position = newPosition;
			this._rotation = newRotation;
			this._scale = newScale;
			this._objectAsset = newObjectAsset;
			this._itemAsset = newItemAsset;
		}

		public Vector3 position
		{
			get
			{
				return this._position;
			}
		}

		public Quaternion rotation
		{
			get
			{
				return this._rotation;
			}
		}

		public Vector3 scale
		{
			get
			{
				return this._scale;
			}
		}

		public ObjectAsset objectAsset
		{
			get
			{
				return this._objectAsset;
			}
		}

		public ItemAsset itemAsset
		{
			get
			{
				return this._itemAsset;
			}
		}

		private Vector3 _position;

		private Quaternion _rotation;

		private Vector3 _scale;

		private ObjectAsset _objectAsset;

		private ItemAsset _itemAsset;
	}
}
