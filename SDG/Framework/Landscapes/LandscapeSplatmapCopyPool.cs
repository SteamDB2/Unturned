using System;
using System.Runtime.CompilerServices;
using SDG.Framework.Utilities;

namespace SDG.Framework.Landscapes
{
	public static class LandscapeSplatmapCopyPool
	{
		public static void empty()
		{
			LandscapeSplatmapCopyPool.pool.empty();
		}

		public static void warmup(uint count)
		{
			Pool<float[,,]> pool = LandscapeSplatmapCopyPool.pool;
			if (LandscapeSplatmapCopyPool.<>f__mg$cache0 == null)
			{
				LandscapeSplatmapCopyPool.<>f__mg$cache0 = new Pool<float[,,]>.PoolClaimHandler(LandscapeSplatmapCopyPool.handlePoolClaim);
			}
			pool.warmup(count, LandscapeSplatmapCopyPool.<>f__mg$cache0);
		}

		public static float[,,] claim()
		{
			Pool<float[,,]> pool = LandscapeSplatmapCopyPool.pool;
			if (LandscapeSplatmapCopyPool.<>f__mg$cache1 == null)
			{
				LandscapeSplatmapCopyPool.<>f__mg$cache1 = new Pool<float[,,]>.PoolClaimHandler(LandscapeSplatmapCopyPool.handlePoolClaim);
			}
			return pool.claim(LandscapeSplatmapCopyPool.<>f__mg$cache1);
		}

		public static void release(float[,,] copy)
		{
			Pool<float[,,]> pool = LandscapeSplatmapCopyPool.pool;
			if (LandscapeSplatmapCopyPool.<>f__mg$cache2 == null)
			{
				LandscapeSplatmapCopyPool.<>f__mg$cache2 = new Pool<float[,,]>.PoolReleasedHandler(LandscapeSplatmapCopyPool.handlePoolRelease);
			}
			pool.release(copy, LandscapeSplatmapCopyPool.<>f__mg$cache2);
		}

		private static float[,,] handlePoolClaim(Pool<float[,,]> pool)
		{
			return new float[Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_LAYERS];
		}

		private static void handlePoolRelease(Pool<float[,,]> pool, float[,,] copy)
		{
		}

		private static Pool<float[,,]> pool = new Pool<float[,,]>();

		[CompilerGenerated]
		private static Pool<float[,,]>.PoolClaimHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static Pool<float[,,]>.PoolClaimHandler <>f__mg$cache1;

		[CompilerGenerated]
		private static Pool<float[,,]>.PoolReleasedHandler <>f__mg$cache2;
	}
}
