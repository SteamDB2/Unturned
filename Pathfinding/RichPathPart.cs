using System;
using Pathfinding.Util;

namespace Pathfinding
{
	public abstract class RichPathPart : IAstarPooledObject
	{
		public abstract void OnEnterPool();
	}
}
