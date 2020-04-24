using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	public class DevkitTransformChangeParentTransaction : IDevkitTransaction
	{
		public DevkitTransformChangeParentTransaction(Transform newTransform, Transform newParent)
		{
			this.transform = newTransform;
			this.parentAfter = new TransformDelta(newParent);
		}

		public bool delta
		{
			get
			{
				return this.parentBefore.parent != this.parentAfter.parent;
			}
		}

		public void undo()
		{
			this.parentBefore.set(this.transform);
		}

		public void redo()
		{
			this.parentAfter.set(this.transform);
		}

		public void begin()
		{
			this.parentBefore = new TransformDelta(this.transform.parent);
			this.parentBefore.get(this.transform);
			this.transform.parent = this.parentAfter.parent;
			this.parentAfter.get(this.transform);
		}

		public void end()
		{
		}

		public void forget()
		{
		}

		protected Transform transform;

		protected TransformDelta parentBefore;

		protected TransformDelta parentAfter;
	}
}
