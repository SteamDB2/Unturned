using System;
using UnityEngine;

namespace SDG.Unturned
{
	public struct PlayerStateUpdate
	{
		public PlayerStateUpdate(Vector3 pos, byte angle, byte rot)
		{
			this.pos = pos;
			this.angle = angle;
			this.rot = rot;
		}

		public Vector3 pos;

		public byte angle;

		public byte rot;
	}
}
