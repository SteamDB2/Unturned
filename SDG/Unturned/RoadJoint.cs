using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RoadJoint
	{
		public RoadJoint(Vector3 vertex)
		{
			this.vertex = vertex;
			this.tangents = new Vector3[2];
			this.mode = ERoadMode.MIRROR;
			this.offset = 0f;
			this.ignoreTerrain = false;
		}

		public RoadJoint(Vector3 vertex, Vector3[] tangents, ERoadMode mode, float offset, bool ignoreTerrain)
		{
			this.vertex = vertex;
			this.tangents = tangents;
			this.mode = mode;
			this.offset = offset;
			this.ignoreTerrain = ignoreTerrain;
		}

		public Vector3 getTangent(int index)
		{
			return this.tangents[index];
		}

		public void setTangent(int index, Vector3 tangent)
		{
			this.tangents[index] = tangent;
			if (this.mode == ERoadMode.MIRROR)
			{
				this.tangents[1 - index] = -tangent;
			}
			else if (this.mode == ERoadMode.ALIGNED)
			{
				this.tangents[1 - index] = -tangent.normalized * this.tangents[1 - index].magnitude;
			}
		}

		public Vector3 vertex;

		private Vector3[] tangents;

		public ERoadMode mode;

		public float offset;

		public bool ignoreTerrain;
	}
}
