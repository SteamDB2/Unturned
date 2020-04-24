using System;

namespace UnityEngine
{
	public static class GameObjectExtension
	{
		public static T getOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}

		public static RectTransform getRectTransform(this GameObject gameObject)
		{
			return gameObject.transform as RectTransform;
		}
	}
}
