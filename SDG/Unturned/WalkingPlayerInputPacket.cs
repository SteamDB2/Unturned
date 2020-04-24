using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class WalkingPlayerInputPacket : PlayerInputPacket
	{
		public override void read(SteamChannel channel)
		{
			base.read(channel);
			this.analog = (byte)channel.read(Types.BYTE_TYPE);
			this.position = (Vector3)channel.read(Types.VECTOR3_TYPE);
			this.yaw = (float)channel.read(Types.SINGLE_TYPE);
			this.pitch = (float)channel.read(Types.SINGLE_TYPE);
		}

		public override void write(SteamChannel channel)
		{
			base.write(channel);
			channel.write(this.analog);
			channel.write(this.position);
			channel.write(this.yaw);
			channel.write(this.pitch);
		}

		public byte analog;

		public Vector3 position;

		public float yaw;

		public float pitch;
	}
}
