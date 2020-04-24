using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class WorkzoneSelection
	{
		public WorkzoneSelection(Transform newTransform, Transform newParent)
		{
			this._transform = newTransform;
			this._parent = newParent;
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

		public Vector3 point;
	}
}
