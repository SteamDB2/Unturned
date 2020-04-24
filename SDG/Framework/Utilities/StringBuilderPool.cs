using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SDG.Framework.Utilities
{
	public static class StringBuilderPool
	{
		public static StringBuilder claim()
		{
			Pool<StringBuilder> pool = StringBuilderPool.pool;
			if (StringBuilderPool.<>f__mg$cache0 == null)
			{
				StringBuilderPool.<>f__mg$cache0 = new Pool<StringBuilder>.PoolClaimHandler(StringBuilderPool.handlePoolClaim);
			}
			return pool.claim(StringBuilderPool.<>f__mg$cache0);
		}

		public static void release(StringBuilder sb)
		{
			Pool<StringBuilder> pool = StringBuilderPool.pool;
			if (StringBuilderPool.<>f__mg$cache1 == null)
			{
				StringBuilderPool.<>f__mg$cache1 = new Pool<StringBuilder>.PoolReleasedHandler(StringBuilderPool.handlePoolRelease);
			}
			pool.release(sb, StringBuilderPool.<>f__mg$cache1);
		}

		private static StringBuilder handlePoolClaim(Pool<StringBuilder> pool)
		{
			return new StringBuilder();
		}

		private static void handlePoolRelease(Pool<StringBuilder> pool, StringBuilder sb)
		{
			sb.Length = 0;
		}

		private static Pool<StringBuilder> pool = new Pool<StringBuilder>();

		[CompilerGenerated]
		private static Pool<StringBuilder>.PoolClaimHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static Pool<StringBuilder>.PoolReleasedHandler <>f__mg$cache1;
	}
}
