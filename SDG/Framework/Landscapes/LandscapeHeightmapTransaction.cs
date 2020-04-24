using System;
using SDG.Framework.Devkit.Transactions;

namespace SDG.Framework.Landscapes
{
	public class LandscapeHeightmapTransaction : IDevkitTransaction
	{
		public LandscapeHeightmapTransaction(LandscapeTile newTile)
		{
			this.tile = newTile;
		}

		public bool delta
		{
			get
			{
				return true;
			}
		}

		public void undo()
		{
			if (this.tile == null)
			{
				return;
			}
			float[,] sourceHeightmap = this.tile.sourceHeightmap;
			this.tile.sourceHeightmap = this.heightmapCopy;
			this.heightmapCopy = sourceHeightmap;
			this.tile.data.SetHeightsDelayLOD(0, 0, this.tile.sourceHeightmap);
			this.tile.terrain.ApplyDelayedHeightmapModification();
		}

		public void redo()
		{
			this.undo();
		}

		public void begin()
		{
			this.heightmapCopy = LandscapeHeightmapCopyPool.claim();
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					this.heightmapCopy[i, j] = this.tile.sourceHeightmap[i, j];
				}
			}
		}

		public void end()
		{
		}

		public void forget()
		{
			LandscapeHeightmapCopyPool.release(this.heightmapCopy);
		}

		protected LandscapeTile tile;

		protected float[,] heightmapCopy;
	}
}
