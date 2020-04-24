using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	public class PoolReference : MonoBehaviour
	{
		private IEnumerator PoolDestroy()
		{
			yield return new WaitForSeconds(this.delay);
			if (this.pool == null)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				this.pool.Destroy(this);
			}
			yield break;
		}

		public void Destroy(float t)
		{
			if (this.pool == null)
			{
				Object.Destroy(base.gameObject, t);
				return;
			}
			this.delay = t;
			base.StartCoroutine("PoolDestroy");
		}

		public GameObjectPool pool;

		public bool inPool;

		private float delay;
	}
}
