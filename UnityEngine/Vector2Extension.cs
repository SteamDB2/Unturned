using System;

namespace UnityEngine
{
	public static class Vector2Extension
	{
		public static Vector2 Cross(this Vector2 vector)
		{
			return new Vector2(vector.y, -vector.x);
		}
	}
}
