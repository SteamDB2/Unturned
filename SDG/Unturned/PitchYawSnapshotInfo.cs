using System;
using UnityEngine;

namespace SDG.Unturned
{
	public struct PitchYawSnapshotInfo : ISnapshotInfo
	{
		public PitchYawSnapshotInfo(Vector3 pos, float pitch, float yaw)
		{
			this.pos = pos;
			this.pitch = pitch;
			this.yaw = yaw;
		}

		public ISnapshotInfo lerp(ISnapshotInfo targetTemp, float delta)
		{
			PitchYawSnapshotInfo pitchYawSnapshotInfo = (PitchYawSnapshotInfo)targetTemp;
			return new PitchYawSnapshotInfo
			{
				pos = Vector3.Lerp(this.pos, pitchYawSnapshotInfo.pos, delta),
				pitch = Mathf.LerpAngle(this.pitch, pitchYawSnapshotInfo.pitch, delta),
				yaw = Mathf.LerpAngle(this.yaw, pitchYawSnapshotInfo.yaw, delta)
			};
		}

		public Vector3 pos;

		public float pitch;

		public float yaw;
	}
}
