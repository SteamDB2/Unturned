using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class GameObjectPool
	{
		public GameObjectPool(GameObject prefab) : this(prefab, 1)
		{
		}

		public GameObjectPool(GameObject prefab, int count)
		{
			this.prefab = prefab;
			this.pool = new Stack<GameObject>(count);
		}

		public GameObject Instantiate()
		{
			return this.Instantiate(Vector3.zero, Quaternion.identity);
		}

		public GameObject Instantiate(Vector3 position, Quaternion rotation)
		{
			GameObject gameObject;
			if (this.pool.Count > 0)
			{
				gameObject = this.pool.Pop();
				if (gameObject == null)
				{
					gameObject = this.Instantiate(position, rotation);
				}
				else
				{
					gameObject.transform.parent = null;
					gameObject.transform.position = position;
					gameObject.transform.rotation = rotation;
					gameObject.transform.localScale = Vector3.one;
					gameObject.SetActive(true);
				}
				PoolReference component = gameObject.GetComponent<PoolReference>();
				component.inPool = false;
			}
			else
			{
				gameObject = Object.Instantiate<GameObject>(this.prefab, position, rotation);
				PoolReference poolReference = gameObject.AddComponent<PoolReference>();
				poolReference.pool = this;
				poolReference.inPool = false;
			}
			return gameObject;
		}

		public void Destroy(PoolReference reference)
		{
			if (reference == null || reference.inPool || reference.pool != this)
			{
				return;
			}
			GameObject gameObject = reference.gameObject;
			gameObject.SetActive(false);
			this.pool.Push(gameObject);
			reference.inPool = true;
		}

		private GameObject prefab;

		private Stack<GameObject> pool;
	}
}
