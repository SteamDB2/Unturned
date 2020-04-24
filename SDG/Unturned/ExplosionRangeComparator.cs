using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ExplosionRangeComparator : IComparer<Transform>
	{
		public int Compare(Transform a, Transform b)
		{
			return Mathf.RoundToInt(((a.position - this.point).sqrMagnitude - (b.position - this.point).sqrMagnitude) * 100f);
		}

		public Vector3 point;
	}
}
