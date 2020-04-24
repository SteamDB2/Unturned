using System;
using System.Runtime.CompilerServices;
using SDG.Framework.Utilities;

namespace SDG.Framework.Landscapes
{
	public static class LandscapeHeightmapCopyPool
	{
		public static void empty()
		{
			LandscapeHeightmapCopyPool.pool.empty();
		}

		public static void warmup(uint count)
		{
			Pool<float[,]> pool = LandscapeHeightmapCopyPool.pool;
			if (LandscapeHeightmapCopyPool.<>f__mg$cache0 == null)
			{
				LandscapeHeightmapCopyPool.<>f__mg$cache0 = new Pool<float[,]>.PoolClaimHandler(LandscapeHeightmapCopyPool.handlePoolClaim);
			}
			pool.warmup(count, LandscapeHeightmapCopyPool.<>f__mg$cache0);
		}

		public static float[,] claim()
		{
			Pool<float[,]> pool = LandscapeHeightmapCopyPool.pool;
			if (LandscapeHeightmapCopyPool.<>f__mg$cache1 == null)
			{
				LandscapeHeightmapCopyPool.<>f__mg$cache1 = new Pool<float[,]>.PoolClaimHandler(LandscapeHeightmapCopyPool.handlePoolClaim);
			}
			return pool.claim(LandscapeHeightmapCopyPool.<>f__mg$cache1);
		}

		public static void release(float[,] copy)
		{
			Pool<float[,]> pool = LandscapeHeightmapCopyPool.pool;
			if (LandscapeHeightmapCopyPool.<>f__mg$cache2 == null)
			{
				LandscapeHeightmapCopyPool.<>f__mg$cache2 = new Pool<float[,]>.PoolReleasedHandler(LandscapeHeightmapCopyPool.handlePoolRelease);
			}
			pool.release(copy, LandscapeHeightmapCopyPool.<>f__mg$cache2);
		}

		private static float[,] handlePoolClaim(Pool<float[,]> pool)
		{
			return new float[Landscape.HEIGHTMAP_RESOLUTION, Landscape.HEIGHTMAP_RESOLUTION];
		}

		private static void handlePoolRelease(Pool<float[,]> pool, float[,] copy)
		{
		}

		private static Pool<float[,]> pool = new Pool<float[,]>();

		[CompilerGenerated]
		private static Pool<float[,]>.PoolClaimHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static Pool<float[,]>.PoolClaimHandler <>f__mg$cache1;

		[CompilerGenerated]
		private static Pool<float[,]>.PoolReleasedHandler <>f__mg$cache2;
	}
}
