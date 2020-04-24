using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class StructureData
	{
		public StructureData(Structure newStructure, Vector3 newPoint, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, ulong newOwner, ulong newGroup, uint newObjActiveDate)
		{
			this._structure = newStructure;
			this.point = newPoint;
			this.angle_x = newAngle_X;
			this.angle_y = newAngle_Y;
			this.angle_z = newAngle_Z;
			this._owner = newOwner;
			this._group = newGroup;
			this.objActiveDate = newObjActiveDate;
		}

		public Structure structure
		{
			get
			{
				return this._structure;
			}
		}

		public ulong owner
		{
			get
			{
				return this._owner;
			}
		}

		public ulong group
		{
			get
			{
				return this._group;
			}
		}

		private Structure _structure;

		public Vector3 point;

		public byte angle_x;

		public byte angle_y;

		public byte angle_z;

		private ulong _owner;

		private ulong _group;

		public uint objActiveDate;
	}
}
