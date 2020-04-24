using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public struct DevkitSelection : IEquatable<DevkitSelection>
	{
		public DevkitSelection(GameObject newGameObject, Collider newCollider)
		{
			this.gameObject = newGameObject;
			this.collider = newCollider;
		}

		public Transform transform
		{
			get
			{
				return (!(this.gameObject != null)) ? null : this.gameObject.transform;
			}
			set
			{
				this.gameObject = ((!(value != null)) ? null : value.gameObject);
			}
		}

		public bool isValid
		{
			get
			{
				return this.gameObject != null && this.collider != null;
			}
		}

		public static bool operator ==(DevkitSelection a, DevkitSelection b)
		{
			return a.gameObject == b.gameObject;
		}

		public static bool operator !=(DevkitSelection a, DevkitSelection b)
		{
			return !(a == b);
		}

		public bool Equals(DevkitSelection other)
		{
			return this.gameObject == other.gameObject;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			DevkitSelection devkitSelection = (DevkitSelection)obj;
			return this.gameObject == devkitSelection.gameObject;
		}

		public override int GetHashCode()
		{
			if (this.gameObject == null)
			{
				return -1;
			}
			return this.gameObject.GetHashCode();
		}

		public static DevkitSelection invalid = new DevkitSelection(null, null);

		public GameObject gameObject;

		public Collider collider;
	}
}
