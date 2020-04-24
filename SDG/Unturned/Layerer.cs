using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Layerer
	{
		public static void relayer(Transform target, int layer)
		{
			if (target == null)
			{
				return;
			}
			target.gameObject.layer = layer;
			for (int i = 0; i < target.childCount; i++)
			{
				Layerer.relayer(target.GetChild(i), layer);
			}
		}

		public static void viewmodel(Transform target)
		{
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().shadowCastingMode = 0;
				target.GetComponent<Renderer>().receiveShadows = false;
				target.tag = "Viewmodel";
				target.gameObject.layer = LayerMasks.VIEWMODEL;
			}
			else if (target.GetComponent<LODGroup>() != null)
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.FindChild("Model_" + i);
					if (transform == null)
					{
						break;
					}
					if (transform.GetComponent<Renderer>() != null)
					{
						transform.GetComponent<Renderer>().shadowCastingMode = 0;
						transform.GetComponent<Renderer>().receiveShadows = false;
						transform.tag = "Viewmodel";
						transform.gameObject.layer = LayerMasks.VIEWMODEL;
					}
				}
			}
		}

		public static void enemy(Transform target)
		{
			if (target.GetComponent<Renderer>() != null)
			{
				target.tag = "Enemy";
				target.gameObject.layer = LayerMasks.ENEMY;
			}
			else if (target.GetComponent<LODGroup>() != null)
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.FindChild("Model_" + i);
					if (!(transform == null))
					{
						if (transform.GetComponent<Renderer>() != null)
						{
							transform.tag = "Enemy";
							transform.gameObject.layer = LayerMasks.ENEMY;
						}
					}
				}
			}
		}
	}
}
