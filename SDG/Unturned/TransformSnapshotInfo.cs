using System;
using UnityEngine;

namespace SDG.Unturned
{
	public struct TransformSnapshotInfo : ISnapshotInfo
	{
		public TransformSnapshotInfo(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this.rot = rot;
		}

		public ISnapshotInfo lerp(ISnapshotInfo targetTemp, float delta)
		{
			TransformSnapshotInfo transformSnapshotInfo = (TransformSnapshotInfo)targetTemp;
			return new TransformSnapshotInfo
			{
				pos = Vector3.Lerp(this.pos, transformSnapshotInfo.pos, delta),
				rot = Quaternion.Slerp(this.rot, transformSnapshotInfo.rot, delta)
			};
		}

		public Vector3 pos;

		public Quaternion rot;
	}
}
