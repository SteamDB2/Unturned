using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SDG.Framework.Utilities
{
	public static class ListPool<T>
	{
		public static void empty()
		{
			ListPool<T>.pool.empty();
		}

		public static void warmup(uint count)
		{
			Pool<List<T>> pool = ListPool<T>.pool;
			if (ListPool<T>.<>f__mg$cache0 == null)
			{
				ListPool<T>.<>f__mg$cache0 = new Pool<List<T>>.PoolClaimHandler(ListPool<T>.handlePoolClaim);
			}
			pool.warmup(count, ListPool<T>.<>f__mg$cache0);
		}

		public static List<T> claim()
		{
			Pool<List<T>> pool = ListPool<T>.pool;
			if (ListPool<T>.<>f__mg$cache1 == null)
			{
				ListPool<T>.<>f__mg$cache1 = new Pool<List<T>>.PoolClaimHandler(ListPool<T>.handlePoolClaim);
			}
			return pool.claim(ListPool<T>.<>f__mg$cache1);
		}

		public static void release(List<T> list)
		{
			Pool<List<T>> pool = ListPool<T>.pool;
			if (ListPool<T>.<>f__mg$cache2 == null)
			{
				ListPool<T>.<>f__mg$cache2 = new Pool<List<T>>.PoolReleasedHandler(ListPool<T>.handlePoolRelease);
			}
			pool.release(list, ListPool<T>.<>f__mg$cache2);
		}

		private static List<T> handlePoolClaim(Pool<List<T>> pool)
		{
			return new List<T>();
		}

		private static void handlePoolRelease(Pool<List<T>> pool, List<T> list)
		{
			list.Clear();
		}

		private static Pool<List<T>> pool = new Pool<List<T>>();

		[CompilerGenerated]
		private static Pool<List<T>>.PoolClaimHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static Pool<List<T>>.PoolClaimHandler <>f__mg$cache1;

		[CompilerGenerated]
		private static Pool<List<T>>.PoolReleasedHandler <>f__mg$cache2;
	}
}
