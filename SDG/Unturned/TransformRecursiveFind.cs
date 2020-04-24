using System;
using UnityEngine;

namespace SDG.Unturned
{
	public static class TransformRecursiveFind
	{
		public static Transform FindChildRecursive(this Transform parent, string name)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = parent.GetChild(i);
				if (transform.name == name)
				{
					return transform;
				}
				if (transform.childCount != 0)
				{
					transform = transform.FindChildRecursive(name);
					if (transform != null)
					{
						return transform;
					}
				}
			}
			return null;
		}
	}
}
