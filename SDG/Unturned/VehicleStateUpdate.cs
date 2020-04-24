using System;
using UnityEngine;

namespace SDG.Unturned
{
	public struct VehicleStateUpdate
	{
		public VehicleStateUpdate(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this.rot = rot;
		}

		public Vector3 pos;

		public Quaternion rot;
	}
}
