using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Sticky : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (this.isStuck)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform == this.ignoreTransform)
			{
				return;
			}
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.useGravity = false;
			component.isKinematic = true;
			this.isStuck = true;
		}

		private void Awake()
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			component.isTrigger = true;
			component.size *= 2f;
		}

		public Transform ignoreTransform;

		private bool isStuck;
	}
}
