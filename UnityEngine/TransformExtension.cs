using System;

namespace UnityEngine
{
	public static class TransformExtension
	{
		public static T getOrAddComponent<T>(this Transform transform) where T : Component
		{
			return transform.gameObject.getOrAddComponent<T>();
		}
	}
}
