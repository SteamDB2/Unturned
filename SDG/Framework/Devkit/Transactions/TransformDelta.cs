using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	public class TransformDelta
	{
		public TransformDelta(Transform newParent)
		{
			this.parent = newParent;
		}

		public void get(Transform transform)
		{
			this.localPosition = transform.localPosition;
			this.localRotation = transform.localRotation;
			this.localScale = transform.localScale;
		}

		public void set(Transform transform)
		{
			transform.parent = this.parent;
			transform.localPosition = this.localPosition;
			transform.localRotation = this.localRotation;
			transform.localScale = this.localScale;
		}

		public Transform parent;

		public Vector3 localPosition;

		public Quaternion localRotation;

		public Vector3 localScale;
	}
}
