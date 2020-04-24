using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Acid : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (this.isExploded)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform.CompareTag("Agent"))
			{
				return;
			}
			this.isExploded = true;
			if (Provider.isServer)
			{
				if (Dedicator.isDedicated)
				{
					EffectManager.effect(121, this.lastPos, Vector3.up);
				}
				EffectManager.sendEffectReliable(121, EffectManager.LARGE, this.lastPos);
			}
			Object.Destroy(base.transform.parent.gameObject);
		}

		private void FixedUpdate()
		{
			this.lastPos = base.transform.position;
		}

		private void Awake()
		{
			this.lastPos = base.transform.position;
		}

		private bool isExploded;

		private Vector3 lastPos;
	}
}
