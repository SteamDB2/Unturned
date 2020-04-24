using System;

namespace Pathfinding.Voxels
{
	public struct CompactVoxelCell
	{
		public CompactVoxelCell(uint i, uint c)
		{
			this.index = i;
			this.count = c;
		}

		public uint index;

		public uint count;
	}
}
