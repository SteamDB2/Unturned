using System;
using System.Runtime.CompilerServices;

namespace SDG.Framework.Utilities
{
	public static class PoolablePool<T> where T : IPoolable
	{
		public static void empty()
		{
			PoolablePool<T>.pool.empty();
		}

		public static void warmup(uint count)
		{
			Pool<T> pool = PoolablePool<T>.pool;
			if (PoolablePool<T>.<>f__mg$cache0 == null)
			{
				PoolablePool<T>.<>f__mg$cache0 = new Pool<T>.PoolClaimHandler(PoolablePool<T>.handlePoolClaim);
			}
			pool.warmup(count, PoolablePool<T>.<>f__mg$cache0);
		}

		public static T claim()
		{
			Pool<T> pool = PoolablePool<T>.pool;
			if (PoolablePool<T>.<>f__mg$cache1 == null)
			{
				PoolablePool<T>.<>f__mg$cache1 = new Pool<T>.PoolClaimHandler(PoolablePool<T>.handlePoolClaim);
			}
			T result = pool.claim(PoolablePool<T>.<>f__mg$cache1);
			result.poolClaim();
			return result;
		}

		public static void release(T poolable)
		{
			Pool<T> pool = PoolablePool<T>.pool;
			if (PoolablePool<T>.<>f__mg$cache2 == null)
			{
				PoolablePool<T>.<>f__mg$cache2 = new Pool<T>.PoolReleasedHandler(PoolablePool<T>.handlePoolRelease);
			}
			pool.release(poolable, PoolablePool<T>.<>f__mg$cache2);
		}

		private static T handlePoolClaim(Pool<T> pool)
		{
			return Activator.CreateInstance<T>();
		}

		private static void handlePoolRelease(Pool<T> pool, T poolable)
		{
			poolable.poolRelease();
		}

		private static Pool<T> pool = new Pool<T>();

		[CompilerGenerated]
		private static Pool<T>.PoolClaimHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static Pool<T>.PoolClaimHandler <>f__mg$cache1;

		[CompilerGenerated]
		private static Pool<T>.PoolReleasedHandler <>f__mg$cache2;
	}
}
