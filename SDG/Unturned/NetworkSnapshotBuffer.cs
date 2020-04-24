using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class NetworkSnapshotBuffer
	{
		public NetworkSnapshotBuffer(float newDuration, float newDelay)
		{
			this.snapshots = new NetworkSnapshot[8];
			this.readIndex = 0;
			this.readCount = 0;
			this.writeIndex = 0;
			this.writeCount = 0;
			this.readDuration = newDuration;
			this.readDelay = newDelay;
		}

		public NetworkSnapshot[] snapshots { get; private set; }

		public ISnapshotInfo getCurrentSnapshot()
		{
			int num = this.writeCount - this.readCount;
			if (num <= 0)
			{
				this.readLast = Time.realtimeSinceStartup;
				return this.lastInfo;
			}
			if (num > 4)
			{
				if (this.writeIndex == 0)
				{
					this.readIndex = this.snapshots.Length - 1;
				}
				else
				{
					this.readIndex = this.writeIndex - 1;
				}
				this.readCount = this.writeCount - 1;
				this.lastInfo = this.snapshots[this.readIndex].info;
				this.readLast = Time.realtimeSinceStartup;
				return this.lastInfo;
			}
			if (Time.realtimeSinceStartup - this.readLast > this.readDuration && num > 1)
			{
				this.lastInfo = this.snapshots[this.readIndex].info;
				this.readLast += this.readDuration;
				this.incrementReadIndex();
			}
			if (Time.realtimeSinceStartup - this.snapshots[this.readIndex].timestamp < this.readDelay)
			{
				this.readLast = Time.realtimeSinceStartup;
				return this.lastInfo;
			}
			float delta = Mathf.Clamp01((Time.realtimeSinceStartup - this.readLast) / this.readDuration);
			return this.lastInfo.lerp(this.snapshots[this.readIndex].info, delta);
		}

		public void updateLastSnapshot(ISnapshotInfo info)
		{
			this.readIndex = 0;
			this.readCount = 0;
			this.writeIndex = 0;
			this.writeCount = 0;
			this.lastInfo = info;
			this.readLast = Time.realtimeSinceStartup;
		}

		public void addNewSnapshot(ISnapshotInfo info)
		{
			this.snapshots[this.writeIndex].info = info;
			this.snapshots[this.writeIndex].timestamp = Time.realtimeSinceStartup;
			this.incrementWriteIndex();
		}

		private void incrementReadIndex()
		{
			this.readIndex++;
			if (this.readIndex == this.snapshots.Length)
			{
				this.readIndex = 0;
			}
			this.readCount++;
		}

		private void incrementWriteIndex()
		{
			this.writeIndex++;
			if (this.writeIndex == this.snapshots.Length)
			{
				this.writeIndex = 0;
			}
			this.writeCount++;
		}

		private int readIndex;

		private int readCount;

		private int writeIndex;

		private int writeCount;

		private ISnapshotInfo lastInfo;

		private float readLast;

		private float readDuration;

		private float readDelay;
	}
}
