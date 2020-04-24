using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorSelection
	{
		public EditorSelection(Transform newTransform, Transform newParent, Vector3 newFromPosition, Quaternion newFromRotation, Vector3 newFromScale)
		{
			this._transform = newTransform;
			this._parent = newParent;
			this.fromPosition = newFromPosition;
			this.fromRotation = newFromRotation;
			this.fromScale = newFromScale;
		}

		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		public Transform parent
		{
			get
			{
				return this._parent;
			}
		}

		private Transform _transform;

		private Transform _parent;

		public Vector3 fromPosition;

		public Quaternion fromRotation;

		public Vector3 fromScale;
	}
}
