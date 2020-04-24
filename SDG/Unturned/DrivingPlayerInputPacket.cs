using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class DrivingPlayerInputPacket : PlayerInputPacket
	{
		public override void read(SteamChannel channel)
		{
			base.read(channel);
			this.position = (Vector3)channel.read(Types.VECTOR3_TYPE);
			this.angle_x = (byte)channel.read(Types.BYTE_TYPE);
			this.angle_y = (byte)channel.read(Types.BYTE_TYPE);
			this.angle_z = (byte)channel.read(Types.BYTE_TYPE);
			this.speed = (byte)channel.read(Types.BYTE_TYPE);
			this.physicsSpeed = (byte)channel.read(Types.BYTE_TYPE);
			this.turn = (byte)channel.read(Types.BYTE_TYPE);
		}

		public override void write(SteamChannel channel)
		{
			base.write(channel);
			channel.write(this.position);
			channel.write(this.angle_x);
			channel.write(this.angle_y);
			channel.write(this.angle_z);
			channel.write(this.speed);
			channel.write(this.physicsSpeed);
			channel.write(this.turn);
		}

		public Vector3 position;

		public byte angle_x;

		public byte angle_y;

		public byte angle_z;

		public byte speed;

		public byte physicsSpeed;

		public byte turn;
	}
}
