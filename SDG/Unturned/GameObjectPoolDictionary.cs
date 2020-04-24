using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class GameObjectPoolDictionary
	{
		public GameObjectPoolDictionary()
		{
			this.pools = new Dictionary<GameObject, GameObjectPool>();
		}

		public GameObject Instantiate(GameObject prefab)
		{
			return this.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		}

		public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			GameObjectPool gameObjectPool;
			if (!this.pools.TryGetValue(prefab, out gameObjectPool))
			{
				gameObjectPool = new GameObjectPool(prefab);
				this.pools.Add(prefab, gameObjectPool);
			}
			return gameObjectPool.Instantiate(position, rotation);
		}

		public void Instantiate(GameObject prefab, Transform parent, string name, int count)
		{
			GameObjectPool gameObjectPool;
			if (!this.pools.TryGetValue(prefab, out gameObjectPool))
			{
				gameObjectPool = new GameObjectPool(prefab, count);
				this.pools.Add(prefab, gameObjectPool);
			}
			GameObject[] array = new GameObject[count];
			for (int i = 0; i < count; i++)
			{
				GameObject gameObject = gameObjectPool.Instantiate();
				gameObject.name = name;
				gameObject.transform.parent = parent;
				array[i] = gameObject;
			}
			for (int j = 0; j < count; j++)
			{
				gameObjectPool.Destroy(array[j].GetComponent<PoolReference>());
			}
		}

		public void Destroy(GameObject element)
		{
			if (element == null)
			{
				return;
			}
			PoolReference component = element.GetComponent<PoolReference>();
			if (component == null || component.pool == null)
			{
				Object.Destroy(element);
				return;
			}
			component.pool.Destroy(component);
		}

		public void Destroy(GameObject element, float t)
		{
			if (element == null)
			{
				return;
			}
			PoolReference component = element.GetComponent<PoolReference>();
			if (component == null || component.pool == null)
			{
				Object.Destroy(element);
				return;
			}
			component.Destroy(t);
		}

		private Dictionary<GameObject, GameObjectPool> pools;
	}
}
