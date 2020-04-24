using System;
using System.Collections.Generic;

namespace SDG.Framework.Utilities
{
	public class Pool<T>
	{
		public Pool()
		{
			this.pool = new Queue<T>();
		}

		public event Pool<T>.PoolClaimedHandler claimed;

		public event Pool<T>.PoolReleasedHandler released;

		public void empty()
		{
			this.pool.Clear();
		}

		public void warmup(uint count)
		{
			this.warmup(count, null);
		}

		public void warmup(uint count, Pool<T>.PoolClaimHandler callback)
		{
			if (callback == null)
			{
				callback = new Pool<T>.PoolClaimHandler(this.handleClaim);
			}
			for (uint num = 0u; num < count; num += 1u)
			{
				T item = callback(this);
				this.release(item);
			}
		}

		public T claim()
		{
			return this.claim(null);
		}

		public T claim(Pool<T>.PoolClaimHandler callback)
		{
			T t;
			if (this.pool.Count > 0)
			{
				t = this.pool.Dequeue();
			}
			else if (callback != null)
			{
				t = callback(this);
			}
			else
			{
				t = this.handleClaim(this);
			}
			this.triggerClaimed(t);
			return t;
		}

		public void release(T item)
		{
			this.release(item, null);
		}

		public void release(T item, Pool<T>.PoolReleasedHandler callback)
		{
			if (item == null)
			{
				return;
			}
			if (callback != null)
			{
				callback(this, item);
			}
			this.triggerReleased(item);
			this.pool.Enqueue(item);
		}

		protected T handleClaim(Pool<T> pool)
		{
			return Activator.CreateInstance<T>();
		}

		protected void triggerClaimed(T item)
		{
			if (this.claimed != null)
			{
				this.claimed(this, item);
			}
		}

		protected void triggerReleased(T item)
		{
			if (this.released != null)
			{
				this.released(this, item);
			}
		}

		protected Queue<T> pool;

		public delegate T PoolClaimHandler(Pool<T> pool);

		public delegate void PoolReleaseHandler(Pool<T> pool, T item);

		public delegate void PoolClaimedHandler(Pool<T> pool, T item);

		public delegate void PoolReleasedHandler(Pool<T> pool, T item);
	}
}
