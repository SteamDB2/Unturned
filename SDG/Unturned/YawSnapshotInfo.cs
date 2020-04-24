using System;
using UnityEngine;

namespace SDG.Unturned
{
	public struct YawSnapshotInfo : ISnapshotInfo
	{
		public YawSnapshotInfo(Vector3 pos, float yaw)
		{
			this.pos = pos;
			this.yaw = yaw;
		}

		public ISnapshotInfo lerp(ISnapshotInfo targetTemp, float delta)
		{
			YawSnapshotInfo yawSnapshotInfo = (YawSnapshotInfo)targetTemp;
			return new YawSnapshotInfo
			{
				pos = Vector3.Lerp(this.pos, yawSnapshotInfo.pos, delta),
				yaw = Mathf.LerpAngle(this.yaw, yawSnapshotInfo.yaw, delta)
			};
		}

		public Vector3 pos;

		public float yaw;
	}
}
