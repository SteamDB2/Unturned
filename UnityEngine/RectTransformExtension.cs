using System;

namespace UnityEngine
{
	public static class RectTransformExtension
	{
		public static void reset(this RectTransform transform)
		{
			transform.anchorMin = Vector2.zero;
			transform.anchorMax = Vector2.one;
			transform.offsetMin = Vector2.zero;
			transform.offsetMax = Vector2.zero;
			transform.localScale = Vector3.one;
		}
	}
}
