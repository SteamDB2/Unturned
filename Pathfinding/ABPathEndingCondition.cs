using System;

namespace Pathfinding
{
	public class ABPathEndingCondition : PathEndingCondition
	{
		public ABPathEndingCondition(ABPath p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("Please supply a non-null path");
			}
			this.abPath = p;
		}

		public override bool TargetFound(PathNode node)
		{
			return node.node == this.abPath.endNode;
		}

		protected ABPath abPath;
	}
}
