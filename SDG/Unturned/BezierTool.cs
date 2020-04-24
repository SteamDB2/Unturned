using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class BezierTool
	{
		public static Vector3 getPosition(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return num * num * num * a + 3f * num * num * t * b + 3f * num * t * t * c + t * t * t * d;
		}

		public static Vector3 getVelocity(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			t = Mathf.Clamp01(t);
			float num = 1f - t;
			return 3f * num * num * (b - a) + 6f * num * t * (c - b) + 3f * t * t * (d - c);
		}

		public static float getLength(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			return ((d - a).magnitude + (d - c).magnitude + (b - c).magnitude + (b - a).magnitude) / 2f;
		}
	}
}
