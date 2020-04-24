using System;
using SDG.Framework.Devkit.Transactions;

namespace SDG.Framework.Landscapes
{
	public class LandscapeSplatmapTransaction : IDevkitTransaction
	{
		public LandscapeSplatmapTransaction(LandscapeTile newTile)
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
			float[,,] sourceSplatmap = this.tile.sourceSplatmap;
			this.tile.sourceSplatmap = this.splatmapCopy;
			this.splatmapCopy = sourceSplatmap;
			this.tile.data.SetAlphamaps(0, 0, this.tile.sourceSplatmap);
		}

		public void redo()
		{
			this.undo();
		}

		public void begin()
		{
			this.splatmapCopy = LandscapeSplatmapCopyPool.claim();
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						this.splatmapCopy[i, j, k] = this.tile.sourceSplatmap[i, j, k];
					}
				}
			}
		}

		public void end()
		{
		}

		public void forget()
		{
			LandscapeSplatmapCopyPool.release(this.splatmapCopy);
		}

		protected LandscapeTile tile;

		protected float[,,] splatmapCopy;
	}
}
